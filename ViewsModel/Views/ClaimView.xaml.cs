using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ClaimView.xaml
    /// </summary>
    public partial class ClaimView : Window, INotifyPropertyChanged
    {
        public ClaimView()
        {
            InitializeComponent();
            InitializeFields();
            DataContext = this;
            Closing += OnWindowClosing;
        }

        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (CanClear())
            {
                if (_printer != null && !(_printer.CanExist))
                {
                    string msg = Properties.Resources.BackgroundPrintJobsExist;
                    Helper.ShowMessage(msg);
                    e.Cancel = true;
                    return;
                }
                else if (_printer != null && _printer.CanExist)
                {
                    _printer.Dispose();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void InitializeFields()
        {
            _unitOfWork = new UnitOfWork();
            ControlModelState(ModelState.New);
        }
        #region "Fields"
        RelayCommand _saveCommand;
        RelayCommand _clearCommand;
        RelayCommand _printCommand;
        RelayCommand _deleteCommand;
        RelayCommand _openCustomerCommand;
        RelayCommand _searchCommand;
        RelayCommand _deleteDetailCommand;
        //
        ViewClaim _shownViewClaim;
        IUnitOfWork _unitOfWork;
        ModelState _modelState;
        //
        bool _canAddCustomer;
        bool _canSave;
        bool _canPrint;
        bool _canDelete;
        bool _canSearch;

        ClaimPrinter _printer;
        List<ViewClaimDetail> _deletedClaimDetails;
        #endregion

        #region "Properties"
        public ViewClaim ShownViewClaim
        {
            get { return _shownViewClaim; }
            set
            {
                _shownViewClaim = value;
                RaisePropertyChanged();
            }
        }
        public string CurrentYear
        {
            get
            {
                return Properties.Settings.Default.CurrentYear; 
            }
        }
        public int Branch
        {
            get { return Properties.Settings.Default.Branch; }
        }
        private string ClaimLitterPartOne
        {
            get
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.ClaimTextPartOne))
                {
                    throw new InvalidOperationException(Properties.Resources.ExceMsg_ClaimLetterMissing);
                }
                return Properties.Settings.Default.ClaimTextPartOne;


            }
        }
        private string ClaimLitterPartTwo
        {
            get
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.ClaimTextPartTwo))
                {
                    throw new InvalidOperationException(Properties.Resources.ExceMsg_ClaimLetterMissing);
                }
                return Properties.Settings.Default.ClaimTextPartTwo;


            }
        }

        public ViewClaimDetail SelectedDetail
        {
            get;
            set;
        }


        #endregion

        #region "Commands"
        public ICommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand(Print, CanPrint);
                }
                return _printCommand;
            }
        }

        public ICommand OpenCustomerCommand
        {
            get
            {
                if (_openCustomerCommand == null)
                {
                    _openCustomerCommand = new RelayCommand(AddCustomer, CanAddCustomer);
                }
                return _openCustomerCommand;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(Save, CanSave);
                }
                return _saveCommand;
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(Search, CanSearch);
                }
                return _searchCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand(Clear);
                }
                return _clearCommand;
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(Delete, CanDelete);
                }
                return _deleteCommand;
            }
        }
        public ICommand DeleteDetailCommand
        {
            get
            {

                if (_deleteDetailCommand == null)
                {
                    _deleteDetailCommand = new RelayCommand(DeleteDetail, CanDeleteDetail);
                }
                return _deleteDetailCommand;
            }
        }
        #endregion

        #region "Commands Methods"
        async void Print()
        {
            try
            {
                PrintDialog pdg = new PrintDialog();
                if (pdg.ShowDialog() == true)
                {
                    var printableData = ShownViewClaim.DeepClone();
                    var selectedPrinter = pdg.PrintQueue.FullName;
                    string path = Properties.Settings.Default.ClaimTemplatePath;
                    if (string.IsNullOrEmpty(path))
                    {
                        string msg = Properties.Resources.ClaimView_ClaimTemplMissing;
                        Helper.ShowMessage(msg);
                        return;
                    }
                    if (_printer == null) _printer = new ClaimPrinter(path);
                    await _printer.PrintAsync(printableData, selectedPrinter);
                }

            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
        }
        bool CanPrint()
        {
            return _canPrint;
        }
        void Delete()
        {
            try
            {
                if (DeleteConfirmed())
                {
                    var storeClaim = _unitOfWork.Claims.GetById(ShownViewClaim.ClaimId);
                    _unitOfWork.Claims.Delete(storeClaim);
                    _unitOfWork.Save();
                    ClearView();
                    ControlModelState(ModelState.New);
                }
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);

            }

        }
        bool CanDelete()
        {
            return _canDelete;
        }
        void Save()
        {
            try
            {
                if (ValidClaim())
                {
                    if (_modelState == ModelState.InEdit)
                    {
                        Claim c = new Claim();
                        WriteClaimValues(c, _modelState);
                        _unitOfWork.Claims.Add(c);
                    }
                    else
                    {
                        Claim existingClaim = GetExistingClaim(ShownViewClaim.ClaimId);
                        WriteClaimValues(existingClaim, _modelState);
                    }
                    _unitOfWork.Save();
                    if (_modelState == ModelState.InEdit)
                    {
                        //Sync between shown claim and store claim to get the claim detail id 
                        //generated on database,
                        //so, you can update the existing details.
                        Claim storeClaim = GetExistingClaim(ShownViewClaim.ClaimId);
                        ShownViewClaim = ReadStoreClaim(storeClaim);
                    }
                    ControlModelState(ModelState.Saved);
                }
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }


        }
        bool CanSave()
        {
            return _canSave;
        }

        void Clear()
        {
            if (CanClear())
            {
                ClearView();
                ControlModelState(ModelState.New);
            }
        }
        bool CanClear()
        {
            string msg = Properties.Resources.SavePrompetMsg;
            if (ShownViewClaim == null) return true;
            if (_modelState == ModelState.InEdit)
            {
                return Helper.UserConfirmed(msg);
            }
            if (ShownViewClaim.Changed)
            {

                return Helper.UserConfirmed(msg);
            }
            return true;
        }
        void Search()
        {
            OpenClaimDialog ocd = new OpenClaimDialog();
            ocd.Owner = this;
            ocd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            if (ocd.ShowDialog() == true)
            {
                try
                {
                    string id = ocd.SelectedClaim.ClaimId;
                    var claim = GetExistingClaim(id);
                    ShownViewClaim = ReadStoreClaim(claim);
                    ControlModelState(ModelState.Loaded);

                }
                catch (Exception ex)
                {
                    Helper.LogShowError(ex);
                }
            }

        }
        bool CanSearch()
        {
            return _canSearch;
        }
        void AddCustomer()
        {
            OpenCustomerDialog ocd = new OpenCustomerDialog(OpenCustomerDialog.SearchField.CustomerCode);
            if (ocd.ShowDialog() == true)
            {
                try
                {
                    var customer = ocd.SelectedCustomer;
                    ShownViewClaim = CreateViewClaim(customer);
                    ControlModelState(ModelState.InEdit);

                }
                catch (Exception ex)
                {
                    string msg = Helper.ProcessExceptionMessages(ex);
                    Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                    Helper.ShowMessage(msg);
                }
            }
        }
        bool CanAddCustomer()
        {
            return _canAddCustomer;
        }

        void DeleteDetail()
        {
            if (SelectedDetail != null)
            {
                //Before delete detail store in the deleted details so you can sync it with store details.
                if (_modelState != ModelState.InEdit)
                {
                    _deletedClaimDetails.Add(SelectedDetail);
                }

                ShownViewClaim.Details.Remove(SelectedDetail);
            }
        }
        bool CanDeleteDetail()
        {
            if (ShownViewClaim == null || ShownViewClaim.Details == null || ShownViewClaim.Details.Count == 0) return false;
            return true;

        }
        #endregion

        #region "Helpers"
        void ClearView()
        {
            ShownViewClaim = ViewClaim.Create();
        }
        bool DeleteConfirmed()
        {
            string msg = Properties.Resources.DeletePrompetMsg;
            return Helper.UserConfirmed(msg);
        }
        void WriteClaimValues(Claim c, ModelState modelState)
        {
            if (modelState == ModelState.InEdit)
            {
                c.ClaimId = ShownViewClaim.ClaimId;
                c.ClaimYear = ShownViewClaim.ClaimYear;
                c.CustomerId = ShownViewClaim.CustomerId;
                c.Sequence = ShownViewClaim.SequenceNo;
                c.GrandTotal = ShownViewClaim.GrandTotal;
                c.LetterPartOne = ShownViewClaim.LetterPartOne;
                c.LetterPartTwo = ShownViewClaim.LetterPartTwo;
                c.CreationDate = Helper.GetCurrentDate();
                HashSet<ClaimDetail> details = new HashSet<ClaimDetail>();
                foreach (var detial in ShownViewClaim.Details)
                {
                    details.Add(new ClaimDetail()
                        {
                            ClaimId = c.ClaimId,
                            PropertyNo = detial.PropertyNo,
                            Rent = detial.Rent,
                            Maintenance = detial.Maintenance,
                            Deposit = detial.Deposit,
                            Others = detial.Others,
                            Total = detial.Total,
                            Paid = detial.Paid,
                            Balance = detial.Balance,
                            OutstandingRentBalance = detial.OutstandingRentBalance,
                            OutstandingMaintBalance = detial.OutstandingMaintBalance,
                            NetBalance = detial.NetBalance
                        }
                        );
                }
                c.ClaimDetails = details;
            }
            else
            {
                c.ClaimYear = ShownViewClaim.ClaimYear;
                c.CustomerId = ShownViewClaim.CustomerId;
                c.Sequence = ShownViewClaim.SequenceNo;
                c.GrandTotal = ShownViewClaim.GrandTotal;
                c.LetterPartOne = ShownViewClaim.LetterPartOne;
                c.LetterPartTwo = ShownViewClaim.LetterPartTwo;
                HashSet<ClaimDetail> details = new HashSet<ClaimDetail>();
                foreach (var exsitingStoreDetail in c.ClaimDetails)
                {
                    var exsitingShownDetail = ShownViewClaim.Details.FirstOrDefault(cd => cd.Id == exsitingStoreDetail.ClaimDetailId);
                    if (exsitingShownDetail != null)
                    {
                        exsitingStoreDetail.ClaimId = exsitingShownDetail.ClaimId;
                        exsitingStoreDetail.PropertyNo = exsitingShownDetail.PropertyNo;
                        exsitingStoreDetail.Rent = exsitingShownDetail.Rent;
                        exsitingStoreDetail.Maintenance = exsitingShownDetail.Maintenance;
                        exsitingStoreDetail.Deposit = exsitingShownDetail.Deposit;
                        exsitingStoreDetail.Others = exsitingShownDetail.Others;
                        exsitingStoreDetail.Total = exsitingShownDetail.Total;
                        exsitingStoreDetail.Paid = exsitingShownDetail.Paid;
                        exsitingStoreDetail.Balance = exsitingShownDetail.Balance;
                        exsitingStoreDetail.OutstandingRentBalance = exsitingShownDetail.OutstandingRentBalance;
                        exsitingStoreDetail.OutstandingMaintBalance = exsitingShownDetail.OutstandingMaintBalance;
                        exsitingStoreDetail.NetBalance = exsitingShownDetail.NetBalance;
                    }
                }
                if (_deletedClaimDetails.Count > 0) // Sync deletion of any details;
                {
                    foreach (ViewClaimDetail deleted in _deletedClaimDetails)
                    {
                        var existingStoreDetail = c.ClaimDetails.FirstOrDefault(cd => cd.ClaimDetailId == deleted.Id);
                        if (existingStoreDetail != null)
                        {
                            c.ClaimDetails.Remove(existingStoreDetail);
                        }
                    }
                }
            }
        }
        ViewClaim ReadStoreClaim(Claim claim)
        {

            List<ViewClaimDetail> details = new List<ViewClaimDetail>();
            foreach (var detail in claim.ClaimDetails)
            {
                details.Add(new ViewClaimDetail
                    (
                    detail.ClaimDetailId,
                    detail.ClaimId,
                    detail.Property.Type,
                    detail.Property.PropertyNo,
                    detail.Property.Location,
                    (int)detail.Rent,
                    (int)detail.Maintenance,
                    (int)detail.Deposit,
                    (int)detail.Others,
                    (int)detail.Paid,
                    (int)detail.OutstandingRentBalance,
                    (int)detail.OutstandingMaintBalance
                )
                );
            }
            ViewClaim c = new ViewClaim(claim.ClaimId, claim.Sequence, claim.Customer.CustomerId, claim.Customer.Name, claim.ClaimYear,
                claim.LetterPartOne, claim.LetterPartTwo, details);
            return c;
        }


        bool ValidClaim()
        {
            return true;
        }
        ViewClaim CreateViewClaim(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException("customer");

            int customerId = customer.CustomerId;
            var balancedContracts = CustomerBalancedContracts(customerId);
            string currentYear = CurrentYear;
            var currentYearContracts = FindCurrentYearBalancedContract(currentYear, balancedContracts);

            List<ViewClaimDetail> details = CreateViewClaimDetails(balancedContracts, currentYearContracts);

            return CreateViewClaim(customer, details);

        }
        private ViewClaim CreateViewClaim(Customer customer, List<ViewClaimDetail> details)
        {

            string claimId = GenerateId();
            short claimSequence = GenerateSequence(customer, CurrentYear);
            return new ViewClaim(
                claimId,
                claimSequence,
                customer.CustomerId,
                customer.Name,
                CurrentYear,
                ClaimLitterPartOne,
                ClaimLitterPartTwo,
                details);
        }

        private short GenerateSequence(Customer customer, string currentYear)
        {
            ClaimRepository clRepos = (ClaimRepository)_unitOfWork.Claims;
            short maxCustomerClaim = clRepos.MaxClaimSeqeunce(customer.CustomerId, currentYear);
            if (maxCustomerClaim >= 3) return -1;
            return ++maxCustomerClaim;
        }
        private string GenerateId()
        {
            string currentYear = CurrentYear.Substring(2, 2);
            string branch = Branch.ToString();
            string maxNo = Claim.MaxNo;

            if (!string.IsNullOrEmpty(maxNo))
            {
                string yearPortion = maxNo.Substring(0, 2);
                if (yearPortion.Equals(currentYear))
                {
                    string incrementedPortion = maxNo.Substring(3, 4);
                    int incrementedNo;

                    if (int.TryParse(incrementedPortion, out incrementedNo))
                    {
                        incrementedNo++;
                    }
                    return currentYear + branch + DecorateNo(incrementedNo); ;
                }
            }
            return Helper.StartNewIncrement(currentYear, branch).ToString();
        }
        private string DecorateNo(int i)
        {
            string s = i.ToString();
            switch (s.Length)
            {
                case 1:
                    return "000" + s;
                case 2:
                    return "00" + s;
                case 3:
                    return "0" + s;
                case 4:
                    return s;
                default:
                    throw new IndexOutOfRangeException("Schedule No. can't be greater than 9999");
            }

        }
        private static List<ViewClaimDetail> CreateViewClaimDetails(List<Contract> balancedContracts, List<Contract> currentYearContracts)
        {
            List<ViewClaimDetail> details = new List<ViewClaimDetail>();
            foreach (var currentYearContract in currentYearContracts)
            {
                int outstandingRent = 0;
                int outstandingMaint = 0;
                int sumContractPayments = ((currentYearContract.RentDue - currentYearContract.RentBalance)
                                    + (currentYearContract.AgreedMaintenance - currentYearContract.MaintenanceBalance)
                                    + (currentYearContract.AgreedDeposit - currentYearContract.DepositBalance));

                //Past contracts on this property for this customer is all balanced contracts that 
                //have the same property number except the contract that is already the current year contract.
                var pastYearsContracts = balancedContracts.Where(co => co.PropertyNo == currentYearContract.PropertyNo
                    &&
                    co.ContractNo != currentYearContract.ContractNo
                    );
                if (pastYearsContracts.Count() > 0)
                {
                    outstandingRent = pastYearsContracts.Sum(pyc => pyc.RentBalance);
                    outstandingMaint = pastYearsContracts.Sum(pyc => pyc.MaintenanceBalance);
                }

                ViewClaimDetail vcd = new ViewClaimDetail
                (-1, null,
                   currentYearContract.Property.Type,
                    currentYearContract.PropertyNo,
                    currentYearContract.Property.Location,
                    currentYearContract.RentDue,
                    currentYearContract.AgreedMaintenance,
                    currentYearContract.AgreedDeposit,
                    0,
                    sumContractPayments,
                    outstandingRent,
                    outstandingMaint
                );
                details.Add(vcd);
            }
            return details;
        }
        List<Contract> FindCurrentYearBalancedContract(string currentYear, IList<Contract> balancedContracts)
        {
            currentYear = currentYear.Substring(2, 2);
            return balancedContracts.Where(x => x.ContractNo.ToString().Substring(0, 2) == currentYear).ToList();
        }
        List<Contract> CustomerBalancedContracts(int customerId)
        {
            var contractsRepos = _unitOfWork.Contracts;
            var balancedContracts = contractsRepos.Query
                (
                    cont => cont.CustomerId == customerId
                     &&
                     cont.RentBalance > 0
                     ||
                     cont.CustomerId == customerId
                     &&
                     cont.MaintenanceBalance > 0

                );
            return balancedContracts.ToList();
        }

        void ControlModelState(ModelState modelState)
        {
            _modelState = modelState;
            _deletedClaimDetails = new List<ViewClaimDetail>();
            switch (_modelState)
            {
                case ModelState.New:
                    _canSave = false;
                    _canPrint = false;
                    _canDelete = false;
                    _canAddCustomer = true;
                    _canSearch = true;
                    break;
                case ModelState.Saved:
                    _canPrint = true;
                    _canAddCustomer = false;
                    _canAddCustomer = false;
                    _canSearch = false;
                    _canDelete = true;
                    ShownViewClaim.ResetChanges();
                    break;
                case ModelState.Loaded:
                    _canSave = true;
                    _canDelete = true;
                    _canPrint = true;
                    _canAddCustomer = false;
                    _canSearch = false;

                    break;
                case ModelState.InEdit:
                    _canSearch = false;
                    _canAddCustomer = false;
                    _canDelete = false;
                    _canPrint = false;
                    _canSave = true;
                    break;
            }

        }
        private Claim GetExistingClaim(string claimId)
        {
            return _unitOfWork.Claims.GetById(claimId);
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;



        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void OnDetailsGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDetail = dgDetails.SelectedItem as ViewClaimDetail;
        }




    }
}
