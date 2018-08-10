using Jsa.DomainModel;
using Jsa.ViewsModel.DomainEntities;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class CustomerHistoryController:ReportControllerBase
    {
        public CustomerHistoryController(int customerId)
        {
            LoadCustomerHistory(customerId);
        }
        private CustomerHistory _customerHisotry;
        public CustomerHistory CustomerHistory
        {
            get { return _customerHisotry; }
            set
            {
                _customerHisotry = value;
                RaisePropertyChanged();
            }
        }
        private void LoadCustomerHistory(int customerId)
        {
            using (IUnitOfWork w = new UnitOfWork())
            {


                Customer customer = GetCustomer(w, customerId);
                CustomerClass customClass = GetClass(w, customerId);
                List<CustomerPropertyDto> customerProperties = GetCustomerProperties(w, customerId);
                List<CustomerPaymentDto> customerPayments = GetCustomerPayments(w, customer);

                CustomerHistory = new CustomerHistory();
                CustomerHistory.CustomerId = customer.CustomerId;
                CustomerHistory.CustomerName = customer.Name;
                CustomerHistory.CustomerClass = customClass.Class;
                CustomerHistory.CustomerProperties = customerProperties;
                CustomerHistory.CustomerPayments = customerPayments;
            }
        }
        private Customer GetCustomer(IUnitOfWork w, int id)
        {
            return w.Customers.GetById(id);
        }
        private CustomerClass GetClass(IUnitOfWork w, int id)
        {
            return w.CustomersClasses.GetById(id);
        }
        private List<CustomerPropertyDto> GetCustomerProperties(IUnitOfWork w, int id)
        {
            var contracts = w.Contracts.Query(con => con.CustomerId == id
                                                &&
                                                con.Closed != true);
            List<CustomerPropertyDto> customerProperties = new List<CustomerPropertyDto>();
            foreach (var contract in contracts)
            {
                CustomerPropertyDto cpDto = new CustomerPropertyDto();
                cpDto.PropertyNo = contract.PropertyNo;
                cpDto.Description = contract.Property.Description;
                cpDto.Location = contract.Property.Location;
                customerProperties.Add(cpDto);
            }
            return customerProperties;
        }
        private List<CustomerPaymentDto> GetCustomerPayments(IUnitOfWork w, Customer customer)
        {
            var customerContractsInLastThreeYears = 
                new CustomersClassesController().GetCustomerContractsInLastThreeYears(w, customer);
            List<CustomerPaymentDto> customerPayments = new List<CustomerPaymentDto>();
            foreach (var contract in customerContractsInLastThreeYears)
            {
                var contractPayments = GetContractPayments(w, contract.ContractNo);
                foreach (var payment in contractPayments)
                {
                    CustomerPaymentDto cpDto = new CustomerPaymentDto();
                    cpDto.PropertyDescription = contract.Property.Description;
                    cpDto.PayDate = Helper.ApplyDateMask( payment.PayDate);
                    cpDto.Amount = payment.Rent;
                    customerPayments.Add(cpDto);
                }

            }
            return customerPayments;
        }
        private List<Payment> GetContractPayments(IUnitOfWork w, int contractNo)
        {
            return w.Payments.Query(p => p.ContractNo == contractNo).ToList();
        }
        #region Base
        public override void ControlState(ControllerStates state)
        {
            throw new NotImplementedException();
        }

        protected override bool CanEdit()
        {
            throw new NotImplementedException();
        }

        protected override bool CanPrint()
        {
            return true;
        }

        protected override bool CanRefresh()
        {
            throw new NotImplementedException();
        }

        protected override bool CanSearch()
        {
            throw new NotImplementedException();
        }

        protected override void Edit()
        {
            throw new NotImplementedException();
        }

        protected override void Print()
        {
        }

        protected override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
