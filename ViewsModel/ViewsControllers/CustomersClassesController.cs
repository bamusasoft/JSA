using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Mediator;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.Reports;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class CustomersClassesController : ReportControllerBase
    {
        #region Fields
        private Dispatcher _currentDispatcher;
        private readonly OpenDialogProxy _dialogProxy;
        private bool _saved;
        private ObservableCollection<CustomerClassFields> _customersClassesList;
        private enum InternalState
        {
            Created,
            Saved,
            Loaded,
            Failed,
            Loading
        }
        #endregion
        #region Properties

        private bool _progressVisibility;
        public bool ProgressVisibility
        {
            get { return _progressVisibility; }
            set
            {
                _progressVisibility = value;
                RaisePropertyChanged();
            }
        }
        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RaisePropertyChanged();
            }
        }
        private bool _searchEnabled;
        public bool SearchEnabled
        {
            get { return _searchEnabled; }
            set
            {
                _searchEnabled = value;
                RaisePropertyChanged();
            }
        }
        private bool _saveEnabled;
        public bool SaveEnabled
        {
            get { return _saveEnabled; }
            set
            {
                _saveEnabled = value;
                RaisePropertyChanged();
            }
        }
        private bool _printEnabled;
        public bool PrintEnabled
        {
            get { return _printEnabled; }
            set
            {
                _printEnabled = value;
                RaisePropertyChanged();
            }
        }
        private bool _filterEnabled;
        public bool FilterEnabled
        {
            get { return _filterEnabled; }
            set
            {
                _filterEnabled = value;
                RaisePropertyChanged();
            }
        }
        private int _customerId;
        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                RaisePropertyChanged();
            }
        }
        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        public CustomersClassesController() : this(null) { }
        public CustomersClassesController(OpenDialogProxy dialogProxy)
        {
            _dialogProxy = dialogProxy;
            Contracts = new ObservableCollection<ClassContractFields>();
            _customersClassesList = new ObservableCollection<CustomerClassFields>();
            ClassesView = (CollectionView) CollectionViewSource.GetDefaultView(_customersClassesList);
            _currentDispatcher = Application.Current.Dispatcher;
            MaintainSate(InternalState.Created);
        }

        #region Commands
        private RelayCommand _loadCommand;

        public ICommand LoadCommand
        {
            get { return _loadCommand ?? (_loadCommand = new RelayCommand(LoadData)); }
        }
        private RelayCommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(Save)); }
        }
        private ICommand _opendDiglogCommand;
        public ICommand OpenDialogCommmand
        {
            get
            {
                return _opendDiglogCommand ?? (_opendDiglogCommand = new RelayCommand(OpenHistory));

            }
        }
        private void OpenHistory()
        {
            if (!_saved)
            {
                string msg = Resources.ClassesView_CannotOpenBeforSave;
                Helper.ShowMessage(msg);
                return;
            }
            CustomerClassFields selected = SelectedItem as CustomerClassFields;
            if (selected != null)
            {
                _dialogProxy.RaiseOpenDialog(selected.CustomerId);
            }
        }
        private void Save()
        {
            try
            {
                using (IUnitOfWork w = new UnitOfWork())
                {
                    CustomersClassesRepository custClassesRepos = (CustomersClassesRepository)w.CustomersClasses;
                    Console.WriteLine("At Save Method Customer Classes List Count = " + _customersClassesList.Count);
                    foreach (var customerClass in _customersClassesList)
                    {
                        CustomerClassFields f = customerClass;
                        CustomerClass existCustomerClass = custClassesRepos.GetById(f.CustomerId);
                        if (existCustomerClass != null)
                        {
                            existCustomerClass.Class = f.CustomerClass;
                        }
                        else
                        {
                            CustomerClass dbCustomerClass = new CustomerClass();
                            dbCustomerClass.CustomerId = f.CustomerId;
                            dbCustomerClass.Class = f.CustomerClass;
                            custClassesRepos.Add(dbCustomerClass);
                        }
                    }
                    w.Save();
                }
                MaintainSate(InternalState.Saved);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
                MaintainSate(InternalState.Failed);

            }
        }
        private RelayCommand _filterCommand;
        public ICommand FilterCommand
        {
            get { return _filterCommand ?? (_filterCommand = new RelayCommand(AddFilter)); }
        }
        private void AddFilter()
        {
            Filter();
        }
        #endregion

        #region Customer Classes


        private async void LoadData()
        {
            try
            {
                MaintainSate(InternalState.Loading);
                await SearchPayment();
                MaintainSate(InternalState.Loaded);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
                MaintainSate(InternalState.Failed);
            }


        }

        private void ToggleUi(InternalState state)
        {
            switch (state)
            {
                case InternalState.Created:
                    SearchEnabled = true;
                    SaveEnabled = false;
                    ProgressVisibility = false;
                    PrintEnabled = false;
                    FilterEnabled = false;
                    break;
                case InternalState.Saved:
                    PrintEnabled = true;
                    FilterEnabled = true;
                    break;
                case InternalState.Loaded:
                    SearchEnabled = false;
                    SaveEnabled = true;
                    ProgressVisibility = false;
                    PrintEnabled = true;
                    FilterEnabled = true;
                    break;
                case InternalState.Failed:
                    SaveEnabled = false;
                    SearchEnabled = true;
                    ProgressVisibility = false;
                    PrintEnabled = false;
                    FilterEnabled = false;
                    break;
                case InternalState.Loading:
                    SaveEnabled = false;
                    SearchEnabled = false;
                    ProgressVisibility = true;
                    PrintEnabled = false;
                    FilterEnabled = false;
                    break;
            }

        }

        private ObservableCollection<ClassContractFields> _contracts;

        public ObservableCollection<ClassContractFields> Contracts
        {
            get { return _contracts; }
            set
            {
                _contracts = value;
                RaisePropertyChanged();
            }
        }
       
        private CollectionView _classesView;
        public CollectionView ClassesView
        {
            get { return _classesView; }
            private set
            {
                _classesView = value;
                RaisePropertyChanged();
            }
        }
        public override void ControlState(ControllerStates state)
        {
            throw new NotImplementedException();
        }

        private Task SearchPayment()
        {
            return Task.Factory.StartNew(() =>
             {
             #region Olod algorithm--based on every contract for the property
             //List<ClassContractFields> classContracts = new List<ClassContractFields>();
             //var contracts = GetActiveContracts();
             //double count = contracts.Count;
             //int counter = 0;
             //ProgressVisibility = true;

             //foreach (var contract in contracts)
             //{

             //    using (IUnitOfWork w = new UnitOfWork())
             //    {
             //        var propertycontracts = w.Contracts.Query(x => x.PropertyNo == contract.PropertyNo
             //        && x.CustomerId == contract.CustomerId
             //                                                                       &&
             //                                                                      // ReSharper disable once StringCompareToIsCultureSpecific
             //                                                                      x.StartDate.CompareTo("14340101") >= 0
             //                                                                      &&
             //                                                                      // ReSharper disable once StringCompareToIsCultureSpecific
             //                                                                      x.EndDate.CompareTo("14361230") <= 0);
             //        if(propertycontracts.Count() < 2)
             //        {
             //            AddToContracts(contract, "New");
             //            UpdateProgress(count, counter);
             //            counter++;
             //            continue;
             //        }
             //        List<Payment> p = new List<Payment>();
             //        foreach (var propertyContract in propertycontracts)
             //        {
             //            //var p = GetRangeContractPayments(customerContract);
             //            var payment = GetContractLastPayment(propertyContract);
             //            if (payment != null)
             //            {
             //                p.Add(payment);
             //            }
             //        }
             //        if (CustomersRules.IsInClassA(p))
             //        {
             //            AddToContracts(contract, "A");

             //        }
             //        else if (CustomersRules.IsInClassB(p))
             //        {
             //            AddToContracts(contract, "B");
             //        }
             //        else if (CustomersRules.IsInClassC(p))
             //        {
             //            AddToContracts(contract, "C");
             //        }
             //        else if (CustomersRules.IsInClassF(p))
             //        {
             //            AddToContracts(contract, "F");
             //        }
             //        else
             //        {
             //            AddToContracts(contract, "Undefined");
             //        }
             //        UpdateProgress(count, counter);
             //        counter++;
             //    }
             #endregion

             List<Customer> currentCustomers = GetActiveCustomers();
             double count = currentCustomers.Count;
             int counter = 0;
             ProgressVisibility = true;
                 foreach (Customer customer in currentCustomers)
                 {
                     using (IUnitOfWork w = new UnitOfWork())
                     {
                         List<Contract> customerContracts = GetCustomerContractsInLastThreeYears(w, customer);
                         if (customerContracts.Count() < 2)
                         {
                             AddToClasses(customer, "New");
                             UpdateProgress(count, counter);
                             counter++;
                             continue;
                         }
                         List<Payment> p = new List<Payment>();
                         foreach (var propertyContract in customerContracts)
                         {
                             //var p = GetRangeContractPayments(customerContract);
                             var payment = GetContractLastPayment(propertyContract);
                             if (payment != null)
                             {
                                 p.Add(payment);
                             }
                         }
                         if (CustomersRules.IsInClassA(p))
                         {
                             AddToClasses(customer, "A");
                         }
                         else if (CustomersRules.IsInClassB(p))
                         {
                             AddToClasses(customer, "B");
                         }
                         else if (CustomersRules.IsInClassC(p))
                         {
                             AddToClasses(customer, "C");
                         }
                         else if (CustomersRules.IsInClassF(p))
                         {
                             AddToClasses(customer, "F");
                         }
                         else if (CustomersRules.IsInBlacklist(p))
                         {
                             AddToClasses(customer, "Blacklist");
                         }
                         
                         else
                         {
                             AddToClasses(customer, "Undefined");
                         }
                         UpdateProgress(count, counter);
                         counter++;
                     }
                 }
                 });
        }

        private void AddToContracts(Contract contract, string customerClass)
        {
            _currentDispatcher.BeginInvoke((Action)(() =>
               {
                   ClassContractFields c = new ClassContractFields
                   {
                       PropertyNo = contract.PropertyNo,
                       CustomerNo = contract.CustomerId,
                       Name = contract.Customer.Name,
                       Location = contract.Property.Location,
                       Property = contract.Property.Description,
                       Rent = contract.AgreedRent,
                       CustomerClass = customerClass
                   };
                   Contracts.Add(c);

               }
                ));

        }
        private void AddToClasses(Customer contract, string customerClass)
        {
            _currentDispatcher.BeginInvoke((Action)(() =>
            {
                CustomerClassFields c = new CustomerClassFields
                {
                    CustomerId = contract.CustomerId,
                    CustomerName = contract.Name,
                    CustomerClass = customerClass
                };
                _customersClassesList.Add(c);
             
            }
                ));

        }
        private void UpdateProgress(double count, int counter)
        {
            _currentDispatcher.BeginInvoke((Action)(() =>
            {
                Progress = (counter / count) * 100;
            }
                ));

        }
        private List<Contract> GetActiveContracts()
        {
            using (IUnitOfWork w = new UnitOfWork())
            {
                return w.Contracts.Query(
                    x => x.StartDate.CompareTo("14360101") >= 0
                    && x.EndDate.CompareTo("14361230") <= 0
                    ).ToList();
            }
        }

        private List<Payment> GetRangeContractPayments(Contract contract)
        {
            using (IUnitOfWork w = new UnitOfWork())
            {
                var contracts1434 = w.Payments.Query(
                    x => x.ContractNo == contract.ContractNo
                         &&
                       x.PayDate.Contains("1434")
                    ).OrderBy(x => x.PayDate).ToList();

                var contracts1435 = w.Payments.Query(
                   x => x.ContractNo == contract.ContractNo
                        &&
                      x.PayDate.Contains("1435")
                   ).OrderBy(x => x.PayDate).ToList();

                var contracts1436 = w.Payments.Query(
                   x => x.ContractNo == contract.ContractNo
                        &&
                      x.PayDate.Contains("1436")
                   ).OrderBy(x => x.PayDate).ToList();

                var last1434 = contracts1434.LastOrDefault();
                var last1435 = contracts1435.LastOrDefault();
                var last1436 = contracts1436.LastOrDefault();
                List<Payment> p = new List<Payment>();
                if (last1434 != null)
                {
                    p.Add(last1434);
                }
                if (last1435 != null)
                {
                    p.Add(last1435);
                }
                if (last1436 != null)
                {
                    p.Add(last1436);
                }
                return p;
            }
        }

        private Payment GetContractLastPayment(Contract contract)
        {
            using (IUnitOfWork w = new UnitOfWork())
            {
                //Get only rent payments
                var payments = w.Payments.Query(x => x.ContractNo == contract.ContractNo
                && x.Rent > 0
                ).OrderBy(x => x.PayDate).ToList();
                return payments.LastOrDefault();
            }
        }
        #endregion

        #region Base

        
        protected override void Search()
        {
            LoadStartRent();
        }

        protected override bool CanSearch()
        {
            return true;
        }

        protected override void Print()
        {
            string path = Settings.Default.ClassesTemplate;
            ExcelProperties props = new ExcelProperties(2, 1, false);
            ClassesReport report = new ClassesReport(Contracts.ToList(), path, props);
            report.Print();
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

        #region Start Rent
        private ObservableCollection<CustomerStartRent> _startRent;
        public ObservableCollection<CustomerStartRent> StartRents
        {
            get { return _startRent; }
            set
            {
                _startRent = value;
                RaisePropertyChanged();
            }
        }
        private void LoadStartRent()
        {
            StartRents = new ObservableCollection<CustomerStartRent>();
            var activeContracts = GetActiveContracts();
            foreach (var activeContract in activeContracts)
            {
                var startRent = GetStartRent(activeContract);
                CustomerStartRent oldest = new CustomerStartRent
                {
                    CustomerNo = activeContract.CustomerId,
                    Name = activeContract.Customer.Name,
                    PropertyNo = activeContract.PropertyNo,
                    PropertyDescription = activeContract.Property.Description,
                    Location = activeContract.Property.Location,
                    StartDate = Helper.ApplyDateMask(startRent),
                    StartYear = Helper.GetYear(startRent)
                };
                StartRents.Add(oldest);
            }
        }
        public string GetStartRent (Contract contract)
        {
            var customerContracts = GetAllPropertyCustomerContracts(contract.CustomerId, contract.PropertyNo);
            var olderContract = customerContracts.OrderBy(x => x.ContractYear).FirstOrDefault();
            return olderContract.StartDate ?? "Unkown";
        }
        private List<Contract> GetAllPropertyCustomerContracts(int customerId, string propertyNo)
        {
            using (IUnitOfWork w = new UnitOfWork())
            {
                return w.Contracts.Query(x => x.CustomerId == customerId
                                    && x.PropertyNo == propertyNo).ToList();
            }
        }
        private List<Customer> GetActiveCustomers()
        {
            using (IUnitOfWork w = new UnitOfWork())
            {
                var activeCustomers = w.Contracts.Query(x => x.Closed != true).ToList();
                var disitinctCustomers = activeCustomers.Select(c => c.Customer).Distinct(new CustomerComparer()).ToList();
                return disitinctCustomers;
            }
        }
        public List<Contract> GetCustomerContractsInLastThreeYears(IUnitOfWork w, Customer customer)
        {
            string currentYear = Helper.GetCurrentYear;
            int endYear = int.Parse(currentYear) - 1;
            int startYear = endYear - 3;
            string startDate = startYear + "0101";
            string endDate = endYear + "1230";
            return w.Contracts.Query(cont => (cont.CustomerId == customer.CustomerId)
                                    &&
                                     (cont.StartDate.CompareTo(startDate) >= 0)
                                    &&
                                     (cont.EndDate.CompareTo(endDate) <= 0))
                                    .ToList();
        }
        #endregion

        #region Helpers

        private void MaintainSate(InternalState state)
        {
            switch (state)
            {
                case InternalState.Created:
                    ToggleUi(InternalState.Created);
                    _saved = false;
                    break;
                case InternalState.Saved:
                    ToggleUi(InternalState.Saved);
                    _saved = true;
                    break;
                case InternalState.Loaded:
                    ToggleUi(InternalState.Loaded);
                    _saved = false;
                    break;
                case InternalState.Failed:
                    _customersClassesList = new ObservableCollection<CustomerClassFields>();
                    ClassesView = new ListCollectionView(_customersClassesList);
                    ToggleUi(InternalState.Failed);
                    _saved = false;
                    break;
                case InternalState.Loading:
                    ToggleUi(InternalState.Loading);
                    _saved = false;
                    break;
            }
        }
        private void Filter()
        {
            var viewCopy = _customersClassesList.AsEnumerable();
            if(CustomerId != 0)
            {
                viewCopy = FilterByCustomerId(viewCopy);
            }
            if (!string.IsNullOrEmpty(CustomerName))
            {
                viewCopy = FilterByCustomerName(viewCopy);
            }
            ClassesView = (CollectionView)CollectionViewSource.GetDefaultView(viewCopy);
            
        }
        private bool FilterByCustomerId(object o)
        {
            CustomerClassFields c = (CustomerClassFields)o;
            return c.CustomerId == CustomerId;
        }
        private IEnumerable<CustomerClassFields> FilterByCustomerId(IEnumerable<CustomerClassFields> view)
        {
            return view.Where(x => x.CustomerId == CustomerId);
        }
        private bool FilterByCustomerName(object o)
        {
            CustomerClassFields c = (CustomerClassFields)o;
            return c.CustomerName.Contains(CustomerName);
        }
        private IEnumerable<CustomerClassFields> FilterByCustomerName(IEnumerable<CustomerClassFields> view)
        {
            return view.Where(x => x.CustomerName.Contains(CustomerName));
        }
        #endregion

    }
}
