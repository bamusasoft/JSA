using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Annotations;
using Jsa.ViewsModel.DomainEntities;
using Jsa.ViewsModel.Views;
using Jsa.ViewsModel.ViewsControllers.Core;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.ViewsControllers
{
    public sealed class ScheduleViewController : EditableControllerBase
    {
        #region Fields

        private bool _canSave;
        private bool _canDelete;
        private bool _canSearch;
        private bool _canPrint;
        private bool _canOpenSigner;
        private bool _canOpenContract;
        private bool _isScheduleIdEnabled;
        //
        private string _scheduleId;
        private string _scheduleDate;
        private Signer _signer;

        //
        private RelayCommand _openContractCommand;
        private RelayCommand _openSignerCommand;
        private RelayCommand _deleteDetailCommand;
        private RelayCommand _addScheduleDetailsCommand;
        private ObservableCollection<Customer> _allCustomers;
        private ObservableCollection<Contract> _customerContracts;
        private ObservableCollection<Contract> _scheduledContracts;
        private ObservableCollection<ScheduleDetailsController> _details;
        private ObservableCollection<Signer> _allSigners;
        private Customer _customer;
        private Contract _selectedContract;
        private IList _deletedDetails;
        private DetailsStatistics _statistics;

        #endregion

        #region Constractor

        public ScheduleViewController()
        {
            GetReady();
        }

        #endregion

        #region Out Properties

        public string ScheduleId
        {
            get { return _scheduleId; }
            set
            {
                _scheduleId = value;
                RaisePropertyChanged();
            }
        }

        public string ScheduleDate
        {
            get { return _scheduleDate; }
            set
            {
                _scheduleDate = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Customer> AllCustomers
        {
            get { return _allCustomers; }
            set
            {
                _allCustomers = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Contract> CustomerContracts
        {
            get { return _customerContracts; }
            set
            {
                _customerContracts = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Contract> ScheduledContracts
        {
            get { return _scheduledContracts; }
            set
            {
                _scheduledContracts = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Signer> AllSigners
        {
            get { return _allSigners; }
            set
            {
                _allSigners = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ScheduleDetailsController> Details
        {
            get { return _details; }
            set
            {
                _details = value;
                RaisePropertyChanged();
            }
        }

        public ScheduleDetailsController SelectedDetail { get; set; }

        public Signer Signer
        {
            get { return _signer; }
            set
            {
                _signer = value;
                RaisePropertyChanged();
            }
        }

        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                RaisePropertyChanged();
            }
        }

        public Contract SelectedContract
        {
            get { return _selectedContract; }
            set
            {
                _selectedContract = value;
                RaisePropertyChanged();
            }
        }

        public bool IsScheduleIdEnabled
        {
            get { return _isScheduleIdEnabled; }
            set
            {
                _isScheduleIdEnabled = value;
                RaisePropertyChanged();
            }
        }

        public DetailsStatistics Statistics
        {
            get { return _statistics; }
            set
            {
                _statistics = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Internal Properties



        #endregion

        #region Helpers
        /// <summary>
        /// Responsible of initiliazing the controller.
        /// </summary>
        private void GetReady()
        {

            try
            {
                AllCustomers = LoadCustomers();
                AllSigners = LoadSigners();
                CustomerContracts = new ObservableCollection<Contract>();
                ScheduledContracts = new ObservableCollection<Contract>();
                Details = new ObservableCollection<ScheduleDetailsController>();
                DeletedDetails = new List<ScheduleDetailsController>();
                Errors = new Dictionary<string, List<string>>();
                ControlState(ControllerStates.Blank);
                Details.CollectionChanged += OnDetailsChanged;

            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
                throw; //no benefit from continue as the controller is in a bad state

            }

        }



        ObservableCollection<Customer> LoadCustomers()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var list = ((CustomersRepository)unit.Customers).GetActiveCustomers();
                return new ObservableCollection<Customer>(list);
            }
        }

        ObservableCollection<Signer> LoadSigners()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var list = unit.Signers.GetAll();
                return new ObservableCollection<Signer>(list);
            }
        }

        #endregion

        #region Overrides of EditableControllerBase

        public override void ControlState(ControllerStates state)
        {
            State = state;
            switch (State)
            {
                case ControllerStates.Blank:
                    _canSave = true;
                    _canPrint = false;
                    _canSearch = true;
                    _canDelete = false;
                    _canOpenContract = true;
                    _canOpenSigner = true;
                    IsScheduleIdEnabled = true;
                    RaiseContorllerChanged(ControllerAction.Cleared);
                    UpdateStatistics();
                    break;
                case ControllerStates.Edited:
                    _canSave = true;
                    _canPrint = true;
                    _canSearch = false;
                    _canDelete = true;
                    _canOpenContract = true;
                    _canOpenSigner = true;
                    IsScheduleIdEnabled = false;
                    RaiseContorllerChanged(ControllerAction.Edited);
                    UpdateStatistics();
                    break;
                case ControllerStates.Saved:
                    _canSave = true;
                    _canPrint = true;
                    _canSearch = false;
                    _canDelete = true;
                    _canOpenContract = true;
                    _canOpenSigner = true;
                    IsScheduleIdEnabled = false;
                    RaiseContorllerChanged(ControllerAction.Saved);
                    DeletedDetails = new List<ScheduleDetailsController>();
                    UpdateStatistics();
                    break;
                case ControllerStates.Loaded:
                    _canSave = true;
                    _canPrint = true;
                    _canSearch = false;
                    _canDelete = true;
                    _canOpenContract = true;
                    _canOpenSigner = true;
                    IsScheduleIdEnabled = false;
                    RaisePropertyChanged("");
                    //Here to subscribe to the loaded events;
                    RaiseContorllerChanged(ControllerAction.Saved);
                    UpdateStatistics();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void ClearView()
        {
            if (State == ControllerStates.Edited)
            {
                if (!Helper.UserConfirmed(Properties.Resources.SavePrompetMsg))
                {
                    return;
                }
            }

            ControlState(ControllerStates.Blank);
        }

        protected override bool CanClear()
        {
            return true;
        }

        protected override void Save()
        {
            try
            {
                if (!IsValid()) return;
                SaveSchedule();
                ControlState(ControllerStates.Saved);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }



        protected override bool CanSave()
        {
            return _canSave;
        }

        protected override void Print()
        {
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var schedule = unit.Schedules.GetById(ScheduleId);
                    Helper.PrintSchedule(schedule);
                }
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }


        }

        protected override bool CanPrint()
        {
            return _canPrint;
        }

        protected override void Search()
        {
            try
            {
                OpenCustomerDialog dg = new OpenCustomerDialog(OpenCustomerDialog.SearchField.CustomerName);
                if (dg.ShowDialog() == true)
                {
                    var schedule = SearchSchedules(dg.SelectedCustomer);
                    ShowSchedule(schedule);
                    ControlState(ControllerStates.Loaded);
                }
            }
            catch (ObjectNotFoundException ex)
            {
                Helper.LogShowError(ex);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }

        protected override bool CanSearch()
        {
            return _canSearch;
        }

        protected override void Delete()
        {
            var msg = Properties.Resources.DeletePrompetMsg;
            if (!Helper.UserConfirmed(msg)) return;
            try
            {
                Schedule schedule = SearchSchedules(Customer);
                Delete(schedule);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }

        protected override bool CanDelete()
        {
            return _canDelete;
        }

        #endregion

        #region ScheduleController specific Commands

        public ICommand OpenContractCommand
        {
            get { return _openContractCommand ?? (_openContractCommand = new RelayCommand(OpenContract, CanOpenContract)); }
        }

        private void OpenContract()
        {
            OpenContractDialog ocd = new OpenContractDialog(null);
            if (ocd.ShowDialog() == true)
            {
                var selected = ocd.SelectedContract;
                RetriveContract(selected.ContractNo);
            }
        }

        private bool CanOpenContract()
        {
            return _canOpenContract;
        }

        public ICommand OpenSignerCommand
        {
            get { return _openSignerCommand ?? (_openSignerCommand = new RelayCommand(OpenSigner, CanOpenSigner)); }
        }

        private void OpenSigner()
        {
            SignerView signerView = new SignerView();
            signerView.SignerSaved += OnSignerSaved;
            signerView.ShowDialog();
            signerView.SignerSaved -= OnSignerSaved;
        }

        private void OnSignerSaved(object sender, EventArgs e)
        {
            AllSigners = LoadSigners();
        }

        private bool CanOpenSigner()
        {
            return _canOpenSigner;
        }

        public ICommand DeleteDetailCommand
        {
            get { return _deleteDetailCommand ?? (_deleteDetailCommand = new RelayCommand(DeleteDetail, CanDeleteDetail)); }
        }

        private void DeleteDetail()
        {
            if (SelectedDetail == null) return;
            DeletedDetails.Add(SelectedDetail);
            Details.Remove(SelectedDetail);


            RaisePropertyChanged("");
        }

        private bool CanDeleteDetail()
        {
            return SelectedDetail != null;
        }

        #endregion



        #region Functions

        private void SaveSchedule()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                //First if new add it.
                var result = unit.Schedules.Query(x => x.ScheduleId == ScheduleId).FirstOrDefault();
                if (result == null) //New
                {
                    result = CreateNewSchedule();

                    unit.Schedules.Add(result);
                    foreach (var scheduledContract in ScheduledContracts)
                    {
                        var sp = CreateSchedulePayment(scheduledContract);
                        unit.SchedulePayments.Add(sp);
                    }
                }

                else //otherwise update
                {
                    UpdateSchedule(result);
                }
                unit.Save();
                SyncStore(unit);
            }
        }

        private SchedulePayment CreateSchedulePayment(Contract contract)
        {
            SchedulePayment sp = new SchedulePayment
            {
                ContractNo = contract.ContractNo,
                ScheduledPayment = contract.Balance,
                UnscheduledPayment = contract.Paid,
                TotalPayment = contract.Balance + contract.Paid
            };
            return sp;

        }

        private void UpdateSchedule(Schedule schedule)
        {
            schedule.ScheduleDate = ScheduleDate;
            schedule.CustomerId = Customer.CustomerId;
            schedule.SignerId = Signer.SignerId;
            UpdateDetails(schedule.ScheduleDetails);
        }

        private void UpdateDetails(ICollection<ScheduleDetail> details)
        {
            foreach (var scheduleDetailsController in Details)
            {

                var exist = details.Any(x => x.Id == scheduleDetailsController.Id
                                             &&
                                             x.Id != 0  //Check for not id = 0 as the new add when update its ID will be always = 0
                    );
                if (exist)
                {
                    var scheduleDetail = details.Single(x => x.Id == scheduleDetailsController.Id);
                    scheduleDetail.AmountDue = scheduleDetailsController.AmountDue;
                    scheduleDetail.DateDue = scheduleDetailsController.DateDue;
                    scheduleDetail.AmountPaid = scheduleDetailsController.AmountPaid;
                    scheduleDetail.Balance = scheduleDetailsController.Balance;
                    scheduleDetail.DiscountAmount = scheduleDetailsController.DiscountAmount;
                    scheduleDetail.Remarks = scheduleDetailsController.Remarks;
                    scheduleDetail.ContractNo = scheduleDetailsController.ContractNo;
                }
                else //There are new entries, Add them
                {
                    details.Add(CreateScheduleDetail(scheduleDetailsController));
                }

            }
            //Check if there are deleted items.
            foreach (ScheduleDetailsController deletedDetail in DeletedDetails)
            {
                var scheduleDetail = details.Single(x => x.Id == deletedDetail.Id);
                details.Remove(scheduleDetail);

            }

        }

        public IList DeletedDetails
        {
            get { return _deletedDetails; }
            set
            {
                _deletedDetails = value;
                RaisePropertyChanged();
            }
        }
        private bool IsValid()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(ScheduleId))
            {
                AddError("ScheduleId", SCHEDUELIDERROR);
                isValid = false;
            }
            else
            {
                RemoveError("ScheduleId", SCHEDUELIDERROR);

            }
            if (string.IsNullOrEmpty("ScheduleDate"))
            {
                AddError("ScheduleDate", SCHEDULEDATEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("ScheduleDate", SCHEDULEDATEERROR);
            }
            if (!Helper.ValidDate(ScheduleDate))
            {
                AddError("ScheduleDate", SCHEDULEDATEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("ScheduleDate", SCHEDULEDATEERROR);
            }
            if (Customer == null)
            {
                AddError("Customer", CUSTOMERERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Customer", CUSTOMERERROR);
            }
            if (Signer == null)
            {
                AddError("Signer", SIGNERERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Signer", SIGNERERROR);
            }
            if (Details.Count == 0)
            {
                AddError("Details", DETAILSERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Details", DETAILSERROR);
            }

            return isValid;
        }
        private Schedule CreateNewSchedule()
        {
            Schedule schedule = new Schedule
            {
                ScheduleId = ScheduleId,
                ScheduleDate = ScheduleDate,
                CustomerId = Customer.CustomerId,
                SignerId = Signer.SignerId,
                ScheduleDetails = CreateScheduleDetails()
            };
            return schedule;
        }

        private ICollection<ScheduleDetail> CreateScheduleDetails()
        {
            HashSet<ScheduleDetail> details = new HashSet<ScheduleDetail>();
            foreach (var scheduleDetailsController in Details)
            {
                details.Add(CreateScheduleDetail(scheduleDetailsController));
            }
            return details;

        }

        private ScheduleDetail CreateScheduleDetail(ScheduleDetailsController controlller)
        {
            return new ScheduleDetail()
            {
                ScheduleId = ScheduleId,
                AmountDue = controlller.AmountDue,
                DateDue = controlller.DateDue,
                AmountPaid = controlller.AmountPaid,
                Balance = controlller.Balance,
                DiscountAmount = controlller.DiscountAmount,
                Remarks = controlller.Remarks,
                ContractNo = controlller.ContractNo
            };
        }
        private void Delete(Schedule obj)
        {
            using (UnitOfWork unit = new UnitOfWork())
            {
                foreach (var detail in obj.ScheduleDetails)
                {
                    int contractNo = detail.ContractNo;
                    var schedulePaymenet = unit.SchedulePayments.GetById(contractNo);
                    unit.SchedulePayments.Delete(schedulePaymenet);
                }
                unit.Schedules.Delete(obj);
                unit.Save();
            }
        }

        /// <summary>
        /// Search for a schedule in store and create domain schedule if found based on it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DomainSchedule created from store schedule.</returns>
        private Schedule Search(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id");
            try
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    var result = unit.Schedules.GetById(id);
                    //return ReadStoreEntity(result);
                    return null;
                }
            }
            catch (InvalidOperationException ex)
            {
                string msg = Properties.Resources.ObjectNotFoundExcMsg;
                throw new ObjectNotFoundException(msg, ex);

            }


        }

        private void RetriveSigner(string signerId)
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var result = unit.Signers.GetById(signerId);
                //var domainSigner = result.ToDomainSigner();
                //Current.Signer = domainSigner;
            }
        }

        private void RetriveContract(int no)
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var result = unit.Contracts.GetById(no);
                //var domainContract = result.ToDomainContract();
                //Current.Contract = domainContract;
            }
        }

        public void GenerateScheduleId()
        {
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var max = unit.Schedules.GetAll().Max(x => x.ScheduleId);
                    ScheduleId = Helper.GenerateId(max);
                    ControlState(ControllerStates.Edited);

                }
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }


        }

        private void OnDetailsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {

                foreach (ScheduleDetailsController item in e.NewItems)
                {
                    item.PropertyChanged += OnDetailChanged;
                    ControlState(ControllerStates.Edited);
                }
            }
            if (e.OldItems != null)
            {
                foreach (ScheduleDetailsController oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= OnDetailChanged;
                    ControlState(ControllerStates.Edited);
                }
            }

        }

        private void OnDetailChanged(object sender, PropertyChangedEventArgs e)
        {
            ControlState(ControllerStates.Edited);
        }


        public bool OkExit()
        {
            return State != ControllerStates.Edited;
        }

        public void FindCustomerContracts()
        {
            if (Customer == null) return;

            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var contracts =
                        ((ContractsRepository)unit.Contracts).CustomerActiveContracts(Customer.CustomerId);


                    if (contracts.Count() == 1)
                    {
                        ScheduledContracts.Clear();
                        ScheduledContracts.Add(contracts.First());
                    }
                    else if (contracts.Count() > 1)
                    {
                        SelectContractsDialog dialog = new SelectContractsDialog(Customer.CustomerId);
                        dialog.ShowDialog();
                        if (dialog.DialogResult == true)
                        {
                            SelectContractsController controller = dialog.DataContext as SelectContractsController;
                            if (controller != null)
                            {
                                ScheduledContracts.Clear();
                                foreach (var contract in controller.SelectedContracts)
                                {
                                    ScheduledContracts.Add(contract);
                                }
                            }
                        }

                    }
                    else
                    {

                        Helper.ShowMessage(Properties.Resources.ScheduleView_CustomerAlreadyScheduled);
                    }

                }
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
        }

        public void SetSigner()
        {

        }

        public ICommand AddScheduleDetailsCommand
        {
            get { return _addScheduleDetailsCommand ?? (_addScheduleDetailsCommand = new RelayCommand(AddDetails)); }
        }

        void AddDetails()
        {
            if (SelectedContract != null)
            {
                AddScheduleDetailsDialog dialog = new AddScheduleDetailsDialog(SelectedContract.ContractNo);
                dialog.ShowDialog();
                if (dialog.DialogResult == true)
                {
                    var controller = dialog.DataContext as AddSchedulDetailsController;
                    if (controller != null)
                    {
                        if (controller.Details != null)
                        {
                            foreach (var scheduleDetailsController in controller.Details)
                            {
                                Details.Add(scheduleDetailsController);
                            }
                        }
                    }
                }
            }
        }
        private Schedule SearchSchedules(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException("customer");
            using (IUnitOfWork unit = new UnitOfWork())
            {
               var schedules = unit.Schedules.Query(x => x.CustomerId == customer.CustomerId);
                if (schedules.Count() == 1)
                {
                    return schedules.Single();
                }
                if (schedules.Count() > 1)
                {
                    SelectScheduleDialog dialog = new SelectScheduleDialog(schedules);
                    if (dialog.ShowDialog() == true)
                    {
                        CustomerSchedule selected =
                            ((SelectSheduleController) dialog.DataContext).SelectedItem as CustomerSchedule;
                        if (selected != null)
                        {
                            return unit.Schedules.GetById(selected.ScheduleId);

                        }
                    }
                }

            }
            throw new ObjectNotFoundException(Properties.Resources.ObjectNotFoundExcMsg);
        }

        private void ShowSchedule(Schedule schedule)
        {
            if (schedule == null) throw new ArgumentNullException("schedule");
            ScheduleId = schedule.ScheduleId;
            ScheduleDate = schedule.ScheduleDate;
            Customer = AllCustomers.Single(x => x.CustomerId == schedule.Customer.CustomerId);
            Signer = AllSigners.Single(x => x.SignerId == schedule.Signer.SignerId);
            ShowDetails(schedule.ScheduleDetails);
            ShowScheduledContracts(schedule);
        }

        private void SyncStore(IUnitOfWork unit)
        {
            var schedule = unit.Schedules.GetById(ScheduleId);
            ShowSchedule(schedule);
        }
        private void ShowScheduledContracts(Schedule schedule)
        {
            foreach (var scheduleDetail in schedule.ScheduleDetails)
            {
                if (ScheduledContracts.All(x => x.ContractNo != scheduleDetail.ContractNo))
                {
                    ScheduledContracts.Add(scheduleDetail.Contract);
                }

            }
        }

        private void ShowDetails(IEnumerable<ScheduleDetail> details)
        {
            if (Details != null)
            {
                Details.CollectionChanged -= OnDetailsChanged;
                Details = new ObservableCollection<ScheduleDetailsController>();
                Details.CollectionChanged += OnDetailsChanged;
            }
            foreach (var scheduleDetail in details)
            {
                ScheduleDetailsController sdController = new ScheduleDetailsController()
                {
                    Id = scheduleDetail.Id,
                    ScheduleId = scheduleDetail.ScheduleId,
                    AmountDue = scheduleDetail.AmountDue,
                    DateDue = scheduleDetail.DateDue,
                    AmountPaid = scheduleDetail.AmountPaid,
                    ContractNo = scheduleDetail.ContractNo,
                    DiscountAmount = scheduleDetail.DiscountAmount,
                    State = ControllerStates.Loaded,
                    Remarks = scheduleDetail.Remarks
                };
                Debug.Assert(Details != null, "Details != null");
                Details.Add(sdController);

            }
        }

        private void UpdateStatistics()
        {
            if (Details == null)
            {
                Statistics = new DetailsStatistics(0, 0);
                return;
            }
            var dueSum = Details.Sum(x => x.AmountDue);
            var paidSum = Details.Sum(x => x.AmountPaid);
            Statistics = new DetailsStatistics(dueSum, paidSum);
        }
        #endregion
        #region ErrorMessages

        private const string SCHEDUELIDERROR = "رقم التعهد مطلوب";
        private const string SCHEDULEDATEERROR = "تاريخ مطلوب";
        private const string CUSTOMERERROR = "حدد المستأجر";
        private const string SIGNERERROR = "حدد المتعهد";
        private const string DETAILSERROR = "راجع الدفعات";



        #endregion
    }
}