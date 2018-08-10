using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.Reports;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class ContractsController : ReportControllerBase
    {
        #region Fields

        private ObservableCollection<ContractsReport> _reports;
        private ContractsCriteria _criteria;
        private static readonly object ReportsLocker = new object();
        
        #endregion

        #region Constrs

        public ContractsController()
        {
            Reports = new ObservableCollection<ContractsReport>();
            BindingOperations.EnableCollectionSynchronization(Reports, ReportsLocker);
        }
        #endregion

        #region Properties

        public ObservableCollection<ContractsReport> Reports
        {
            get { return _reports; }
            set
            {
                _reports = value;
               
                RaisePropertyChanged();
            }
        }
        public ContractsCriteria Criteria
        {
            get
            {
                if (_criteria == null)
                {
                    _criteria = new ContractsCriteria();
                }
                return _criteria;
            }
        }

        public int AgreedRentSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.AgreedRent);
            }
        }
        public int RentDueSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.RentDue);
            }
        }
        public int MaintDueSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.MaintenanaceDue);
            }
        }
        public int DepositDueSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.DepositDue);
            }
        }
        public int DueTotalSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.DueTotal);
            }
        }
        public int RentPaidSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.RentPaid);
            }
        }
        public int MaintPaidSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.MaintenancePaid);
            }
        }
        public int DepositPaidSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.DepositPaid);
            }
        }
        public int PaidTotalSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.PaidTotal);
            }
        }
        public int BalanceSum
        {
            get
            {
                if (Reports == null) return 0;
                return Reports.AsEnumerable().Sum(x => x.Balance);
            }
        }
        #endregion

        #region Helpers

        private Task TransformContracts(IEnumerable<Contract> result)
        {

            return Task.Run(() =>
            {
                foreach (var contract in result)
                {
                      
                   AddToReport(contract);
                }
            }
                );

        }

        private void AddToReport(Contract contract)
        {

            ContractsReport report = new ContractsReport(contract.ContractNo, contract.CustomerId,
                        contract.PropertyNo,
                        contract.Customer.Name, contract.Property.Description, contract.Property.Location,
                        contract.AgreedRent,
                        contract.RentDue, contract.AgreedMaintenance, contract.AgreedDeposit,
                        (contract.RentDue - contract.RentBalance),
                        (contract.AgreedMaintenance - contract.MaintenanceBalance),
                        (contract.AgreedDeposit - contract.DepositBalance)
                        );

                Reports.Add(report);  
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
                Reports.Clear();
                SearchAsync();
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }

        private async void SearchAsync()
        {
            var filter = Criteria.BuildCriteria();
            if (filter != null)
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var result = unit.Contracts.Query(filter)
                        .OrderBy
                        (
                            cont => cont.PropertyNo
                        )
                        .ThenBy
                        (
                            cont => cont.ContractNo
                        );

                    await TransformContracts(result);
                }
                var sums = new ContractsReport(
                    "الإجمالي", AgreedRentSum, RentDueSum, MaintDueSum, DepositDueSum, RentPaidSum, MaintPaidSum,
                    DepositPaidSum, BalanceSum);
                Reports.Add(sums);
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
                var template = Settings.Default.ContractReportExcelTemplate;
                DataTable report = ContractsReport.CreateReport(Reports, null);
                ExcelMail excel = new ExcelMail();
                excel.Send(report, template, false, 3);
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
            return true;
        }

        protected override void Refresh()
        {

        }

        protected override bool CanRefresh()
        {
            return true;
        }
        #endregion
    }
}
