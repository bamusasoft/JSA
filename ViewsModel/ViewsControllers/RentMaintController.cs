using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Jsa.DomainModel;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.Reports;
using Jsa.ViewsModel.SearchCriteria;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class RentMaintController : ReportControllerBase
    {

        #region Fields

        private bool _showMaint;
        private ObservableCollection<RentReportFields> _rentReports;
        private ObservableCollection<MaintReportFields> _maintReports;
        private RentMaintCriteria _criteria;
        private string _paidFrom;
        private string _paidTo;
        private Func<Payment, bool> _paymentCriteria;
        #endregion

        #region Contru

        public RentMaintController()
        {
            ShowMaint = false;
        }

        #endregion

        #region properties

        public bool ShowMaint
        {
            get { return _showMaint; }
            set
            {
                _showMaint = value;
                RaisePropertyChanged();

            }
        }

        public ObservableCollection<RentReportFields> RentReports
        {
            get { return _rentReports; }
            set
            {
                _rentReports = value;
                RaisePropertyChanged();

            }
        }

        public ObservableCollection<MaintReportFields> MaintReports
        {
            get { return _maintReports; }
            set
            {
                _maintReports = value;
                RaisePropertyChanged();

            }
        }

        public RentMaintCriteria Criteria
        {
            get { return _criteria ?? (_criteria = new RentMaintCriteria()); }
        }

        public string PaidFrom
        {
            get { return _paidFrom; }
            set
            {
                _paidFrom = value;
                RaisePropertyChanged();
            }
        }

        public string PaidTo
        {
            get { return _paidTo; }
            set
            {
                _paidTo = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Base


        public override void ControlState(ControllerStates state)
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {

            try
            {
                BuildPaymentCriteria();
                if (ShowMaint)
                {
                    var maintResult = SearchMaint();
                    MaintReports = new ObservableCollection<MaintReportFields>(maintResult);
                    return;
                }
                var rentResult = SearchRent();
                RentReports = new ObservableCollection<RentReportFields>(rentResult);

            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }

        }

        protected override bool CanSearch()
        {
            return true;
        }

        protected override void Print()
        {
            try
            {
            if (ShowMaint)
            {
                string path = Settings.Default.MaintTemplatePath;
                ExcelProperties props =new ExcelProperties(2,1, false);
                MaintReport report = new MaintReport(MaintReports.ToList(), path, props);
                report.Print();

            }
            else
            {
                string path = Settings.Default.RentTempatePath;
                ExcelProperties props = new ExcelProperties(2,1,false);
                RentReport report = new RentReport(RentReports.ToList(), path, props);
                report.Print();

            }
            }
            catch (Exception ex)
            {
                
                Helper.LogShowError(ex);
            }
            
        }

        protected override bool CanPrint()
        {
            return true;
        }

        protected override void Edit()
        {
            throw new NotImplementedException();
        }

        protected override bool CanEdit()
        {
            throw new NotImplementedException();
        }

        protected override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override bool CanRefresh()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        private IEnumerable<MaintReportFields> SearchMaint()
        {
            var criteria = Criteria.BuildCriteria();
            List<MaintReportFields> report = new List<MaintReportFields>();
            if (criteria != null)
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    //1 Apply criteria
                    var contracts = ApplyMaintCriteria(unit, criteria);
                    //2- for each contract met criteria
                    foreach (var contract in contracts)
                    {
                        //3 Get the maintenance payments for this contract.
                        var maintPayments = GetContractMaintenancePayments(unit, contract);
                            
                        //4 If this contract has any maintenance payments, then:
                        if (maintPayments.Any())
                        {
                           AddToMaintenaceReport(report, maintPayments, contract);
                        }
                        else //5 Otherwise, add the contract as its to the report.
                        {
                           AddToMaintenaceReport(report, contract);

                        }

                   }
                }
            }
            return report;

        }

        
        IQueryable<Payment> GetContractMaintenancePayments(IUnitOfWork unit, Contract contract)
        {
            return unit.Payments.Query(pm => pm.Maintenance != 0
                                             &&
                                             pm.ContractNo == contract.ContractNo);
        }
        private static IEnumerable<Contract> ApplyMaintCriteria(IUnitOfWork unit, Expression<Func<Contract, bool>> criteria)
        {
                    //1 Get contracts based on:
                    //a - Criteria
                    //b- has maintenance
            return unit.Contracts.Query(criteria)
                .Where(x => x.AgreedMaintenance > 0)
                .OrderBy(x => x.PropertyNo);
        }
        private void AddToMaintenaceReport(List<MaintReportFields> report, IEnumerable<Payment> maintPayments, Contract contract)
        {
            //For each maintenance payment
            report.AddRange(maintPayments.Select(maintPayment => new MaintReportFields(maintPayment.ContractNo, contract.PropertyNo, 
                contract.CustomerId, contract.Property.Type, contract.Customer.Name, contract.Property.Description, 
                contract.Property.Location, contract.AgreedMaintenance, maintPayment.Maintenance, maintPayment.PaymentNo, 
                maintPayment.PayDate)));
        }

        private void AddToMaintenaceReport(List<MaintReportFields> report, Contract contract)
        {
            var maintReport = new MaintReportFields(contract.ContractNo,
                               contract.PropertyNo, contract.CustomerId, contract.Property.Type,
                               contract.Customer.Name, contract.Property.Description, contract.Property.Location,
                               contract.AgreedMaintenance, 0, 0, "");
            report.Add(maintReport);
        }

        private IEnumerable<RentReportFields> SearchRent()
        {
            var criteria = Criteria.BuildCriteria();
            if (criteria != null)
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    //Apply Criterial
                    var contracts = ApplyRentCriteria(unit, criteria);
                    var contractsMetCriteria = new List<Contract>();
                    //For each contract
                    contracts.ForEach(contract =>
                    {
                        //If the payments criteria not null
                        if (_paymentCriteria != null)
                        {
                            Contract copy = contract;
                            //Get get payments of this contract.
                            var s = unit.Payments.Query(x => x.ContractNo == copy.ContractNo);
                            //If the contract has any payments, then
                            if (s.Any())
                            {
                                //Apply the payments criteria to get the only payments that
                                //are in range specified.
                                var hasPaymentsInRange = s.Where(_paymentCriteria);
                                //If any payment is in this range, then
                                if (hasPaymentsInRange.Any())
                                {
                                    // add it to the report.
                                    contractsMetCriteria.Add(contract);
                                }
                            }
                        }
                        else  //Otherwise, the user is not interested in payment criteria, so just add the contract to the report.
                        {
                            contractsMetCriteria.Add(contract);
                        }
                    }
                        );
                    //Iterate over the contracts met the criteria, to create the report.
                    foreach (var contract in contractsMetCriteria)
                    {
                        Contract copyedContract = contract;
                        var contractRent =
                            unit.Payments.Query(pm => pm.ContractNo == copyedContract.ContractNo 

                                )
                                .Sum(x => (int?) x.Rent) ?? 0;


                        yield return new RentReportFields(
                            contract.ContractNo, contract.PropertyNo, contract.CustomerId,
                            contract.Customer.Name, contract.Property.Type, contract.Property.Description,
                            contract.Property.Location,
                            contract.AgreedRent, contract.RentDue, contractRent,
                            contract.Customer.MainMobile ?? "",
                            contract.Customer.SecondMobile ?? "",
                            contract.Customer.WorkPhone ?? "",
                            contract.Customer.HomePhone ?? ""
                            );
                    }
                }
            }

        }

        private static List<Contract> ApplyRentCriteria(IUnitOfWork unit, Expression<Func<Contract, bool>> criteria)
        {
            return unit.Contracts.Query(criteria).OrderBy(x => x.PropertyNo).ToList();
        }

        private void BuildPaymentCriteria()
        {
            MethodInfo compareTo = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            ParameterExpression param = Expression.Parameter(typeof(Payment), "payment");
            Expression expr = null;
            Expression paidFromProperty = Expression.PropertyOrField(param, "PayDate");
            Expression paidToProperty = Expression.PropertyOrField(param, "PayDate");
            Expression paidFromValue = Expression.Constant(PaidFrom, typeof(string));
            Expression paidToValue = Expression.Constant(PaidTo, typeof(string));
            bool expressionAssigned = false;
            if (!string.IsNullOrEmpty(PaidFrom))
            {
                {
                    ConstantExpression constant = Expression.Constant(0);
                    Expression temp = Expression.GreaterThanOrEqual(
// ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Call(paidFromProperty, compareTo, paidFromValue), constant);
                    expr = temp;
                    expressionAssigned = true;
                }
            }
            if (!string.IsNullOrEmpty(PaidTo))
            {
                if (!expressionAssigned)
                {
                    ConstantExpression constant = Expression.Constant(0);
                    Expression temp = Expression.LessThanOrEqual(
// ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Call(paidToProperty, compareTo, paidToValue), constant);
                    expr = temp;
                }
                else
                {
                    ConstantExpression constant = Expression.Constant(0);
                    Expression temp = Expression.LessThanOrEqual(
// ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Call(paidToProperty, compareTo, paidToValue), constant);
                    expr = Expression.AndAlso(expr, temp);
                }
            }

            Expression<Func<Payment, bool>> criteria = null;
            if (expr != null)
            {
                criteria = Expression.Lambda<Func<Payment, bool>>(expr, param);
            }
            if (criteria != null) _paymentCriteria =  criteria.Compile();
        }
    }

    #endregion
    
}
