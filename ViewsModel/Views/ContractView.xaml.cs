using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.DomainEntities;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Printers;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ContractView.xaml
    /// </summary>
    public partial class ContractView : Window, INotifyPropertyChanged
    {
        #region "Constr"

        public ContractView()
        {
            InitializeComponent();
            GetReadyForStartup();
            DataContext = this;

        }

        #endregion

        #region Fields
        private Dictionary<string, string> _propertiesLookup;
        private RelayCommand _searchCommand;
        private RelayCommand _printCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _clearCommand;
        private RelayCommand _addActivityCommand;
        private RelayCommand _printPhotoCommand;
        private RelayCommand _openScanDialogCommand;
        private RelayCommand _openSharedFolderCommand;
        private RelayCommand _printMonthlyContractCommand;
        private RelayCommand _calculatorCommand;
        private RelayCommand _openContractDialogCommand;

        //
        private int _contractNo;
        private string _signDay;
        private string _hijDate;
        private string _gregDate;
        private string _customerName;
        private string _idNo;
        private string _idDate;
        private string _idIssue;
        private string _addressLine1;
        private string _addressLine2;
        private string _propertyType;
        private string _propertyNo;
        private string _propertyLocation;
        private string _locationDistrict;
        private string _contractPeriod;
        private string _startDate;
        private string _endDate;
        private int _agreedRent;
        private int _agreedDeposit;
        private string _agreedMaintenance;
        private string _city;
        private string _nationality;
        private string _idType;
        private string _photoPath;
        private System.Windows.Media.Imaging.BitmapImage _contractImage;
        private const string RYAL = "ريـال";


        //
        private IUnitOfWork _unitOfWork;
        private event EventHandler<ViewState> ViewStateChanged;
        private List<RuleViolation> _ruleViolations;
        private ViewState _currentViewState;
        private bool _canExit;
        private bool _canPrint;
        private bool _canSave;
        private bool _canSearch;
        private bool _canAddActivity;
        private bool _canScan;
        private bool _canOpenSharedFolder;
        private const string FULL_YEAR = "سنة";
        private const string PART_OF_YEAR = "تكملة سنة";
        private ObservableCollection<ContractsActivity> _activities;
        private ContractsActivity _selectedActivity;
        private string _prompetMessage;
        private bool _savedImageChanged;
        private Dictionary<string, string> _weekDaysArabicNames;
        #endregion


        #region "UI Properties"
        public int ContractNo
        {
            get { return _contractNo; }
            set
            {
                _contractNo = value;
                RaisePropertyChanged();
            }
        }
        public string SignDay
        {
            get { return _signDay; }
            set
            {
                _signDay = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string HijriDate
        {
            get { return _hijDate; }
            set
            {
                _hijDate = value;

                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string GregDate
        {
            get { return _gregDate; }
            set
            {
                _gregDate = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                RaisePropertyChanged();
            }
        }
        public string IdNo
        {
            get { return _idNo; }
            set { _idNo = value; RaisePropertyChanged(); }

        }
        public string IdDate
        {
            get { return _idDate; }
            set { _idDate = value; RaisePropertyChanged(); }
        }
        public string IdIssue
        {
            get { return _idIssue; }
            set { _idIssue = value; RaisePropertyChanged(); }

        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; RaisePropertyChanged(); }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; RaisePropertyChanged(); }
        }

        public string PropertyType
        {
            get { return _propertyType; }
            set { _propertyType = value; RaisePropertyChanged(); }
        }
        public string PropertyNo
        {
            get { return _propertyNo; }
            set { _propertyNo = value; RaisePropertyChanged(); }
        }

        public string PropertyLocation
        {
            get { return _propertyLocation; }
            set { _propertyLocation = value; RaisePropertyChanged(); }
        }
        public string LocationDistrict
        {
            get { return _locationDistrict; }
            set
            {
                _locationDistrict = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }
        public string ContractPeriod
        {
            get { return _contractPeriod; }
            set { _contractPeriod = value; RaisePropertyChanged(); }
        }
        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; RaisePropertyChanged(); }
        }
        public string EndDate
        {
            get { return _endDate; }
            set { _endDate = value; RaisePropertyChanged(); }
        }

        public int AgreedRent
        {
            get { return _agreedRent; }
            set { _agreedRent = value; RaisePropertyChanged(); }
        }

        public int AgreedDeposit
        {
            get { return _agreedDeposit; }
            set
            {
                _agreedDeposit = value;
                RaisePropertyChanged();
            }
        }

        public string AgreedMaintenance
        {
            get { return _agreedMaintenance; }
            set
            {
                _agreedMaintenance = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ContractsActivity> Activities
        {
            get { return _activities; }
            set
            {
                _activities = value;
                RaisePropertyChanged();
            }
        }
        public ContractsActivity SelectedActivity
        {
            get { return _selectedActivity; }
            set
            {
                _selectedActivity = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }
        public string Nationality
        {
            get { return _nationality; }
            set
            {
                _nationality = value;
                RaisePropertyChanged();
            }
        }
        public string IdType
        {
            get { return _idType; }
            set
            {
                _idType = value;
                RaisePropertyChanged();
            }
        }
        public string PhotoPath
        {
            get { return _photoPath; }
            set
            {
                _photoPath = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }

            }
        }
        public System.Windows.Media.Imaging.BitmapImage ContractImage
        {
            get { return _contractImage; }
            set
            {
                _contractImage = value;
                RaisePropertyChanged();

            }

        }
        #endregion

        #region Other Properties
        private string AgreedRentWords
        {
            get { return SayNumber.ToWords(AgreedRent); }
        }
        private string AgreedDepositWords
        {
            get { return SayNumber.ToWords(AgreedDeposit); }
        }
        private string FullPropertyNo
        {
            get;
            set;
        }
        #endregion

        #region Commands
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
                    _clearCommand = new RelayCommand(ClearView);

                }
                return _clearCommand;

            }
        }
        public ICommand AddActivityCommand
        {
            get
            {
                if (_addActivityCommand == null)
                {
                    _addActivityCommand = new RelayCommand(OpenAddActivity, CanAddActivity);

                }
                return _addActivityCommand;

            }
        }
        public ICommand PrintPhotoCommand
        {
            get
            {
                if (_printPhotoCommand == null)
                {
                    _printPhotoCommand = new RelayCommand(PrintPhoto, CanPrintPhoto);
                }
                return _printPhotoCommand;


            }
        }
        public ICommand OpenScanDialogCommand
        {
            get
            {
                if (_openScanDialogCommand == null)
                {
                    _openScanDialogCommand = new RelayCommand(OpenScanDialog, CanScan);
                }
                return _openScanDialogCommand;
            }
        }
        public ICommand OpenSharedFolderCommand
        {
            get
            {
                if (_openSharedFolderCommand == null)
                {
                    _openSharedFolderCommand = new RelayCommand(OpenSharedFolder, CanOpenSharedFolder);

                }
                return _openSharedFolderCommand;
            }
        }
        public ICommand PrintMonthlyContractCommand
        {
            get { return _printMonthlyContractCommand ?? (_printMonthlyContractCommand = new RelayCommand(PrintMonthlyContract, CanPrintMonthlyContract)); }
        }
        public ICommand CalculatorCommand
        {
            get { return _calculatorCommand ?? (_calculatorCommand = new RelayCommand(OpenCalculator, CanOpenCalc)); }

        }

        public ICommand OpenContractDialogCommand
        {
            get
            {
                return _openContractDialogCommand ??
                       (_openContractDialogCommand = new RelayCommand(OpenContract, CanSearch));
            }
        }
        #endregion

        #region Commands Methods
        private void Save()
        {
            Helper.ExplicitUpdateBinding();
            if (ValidContract())
            {
                try
                {
                    //First Save The image to the contracts photos folder
                    //Note the behavior of saving the photo is by save the image
                    //to Windows sepcial folder named ProgramData which is created
                    //for every user on the machine inside that folder we create a folder 
                    //named BaMusaSoft which in turn contains a folder called
                    //ContractsPhotos
                    if (!string.IsNullOrEmpty(PhotoPath) && _savedImageChanged)
                    {
                        var img = ConvertImageToBitmap(ContractImage);
                        img.Save(PhotoPath);
                    }
                    var currentContract = _unitOfWork.Contracts.GetById(ContractNo);
                    WriteValues(currentContract);
                    _unitOfWork.Save();
                    RaiseViewStateChanged(ViewState.Saved);
                }
                catch (Exception ex)
                {
                    string msg = Helper.ProcessExceptionMessages(ex);
                    Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                    Helper.ShowMessage(msg);
                }
            }
        }

        private void Print()
        {

                try
                {
                    string path = Properties.Settings.Default.GeneralContractTemplate;
                    Print(path);

                }
                catch (Exception ex)
                {
                    Helper.LogShowError(ex);

                }

        }

        private void Print(string path)
        {
            if (!ValidContractForPrint())
            {
                string msg = _prompetMessage;
                Helper.ShowMessage(msg);
                return;
            }
            if (_currentViewState == ViewState.Edited)
            {
                string msg = Properties.Resources.ContractView_NotSaved;
                Helper.ShowMessage(msg);
                return;
            }
            if (Properties.Settings.Default.Branch == 2)//Only Jeddah
            {
                if (AgreedDeposit <= 0)
                {
                    string msg = Properties.Resources.ContractView_InvalidForPrint2;
                    Helper.ShowMessage(msg);
                    return;
                }
            }
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //var pd = new PrintDocument();
                //pd.PrinterSettings.PrinterName = dialog.PrinterSettings.PrinterName;
                //pd.PrintPage += OnPringPage;
                //pd.Print();
                string printerName = dialog.PrinterSettings.PrinterName;
                ContractDto contract = MapToContractDto();
                GeneralConractPrinter printer = new GeneralConractPrinter(path, contract, ExtractProeprtyNo, ExtractPropertyLocation);
                printer.Print(printerName);
            }
        }
        private ContractDto MapToContractDto()
        {
            Contract c = _unitOfWork.Contracts.GetById(ContractNo);
            string selectedCourt = ((System.Windows.Controls.ComboBoxItem)cmbCourt.SelectedItem).Content as string;
            return new ContractDto(
                c.ContractNo, c.StartDate, c.EndDate, c.Customer, c.Property,
                c.AgreedRent, c.AgreedDeposit, c.SignDay, c.SignHijriDate, c.SignGregDate, c.ContractsActivity,selectedCourt);
        }

        void Search()
        {
            string s = txtSearch.Text;
            int cn;
            if (int.TryParse(s, out cn))
            {
                try
                {

                    Search(cn);
                }
                catch (InvalidOperationException ex)
                {
                    string msg = Properties.Resources.ContractView_ContractNotExist;
                    Logger.Log(LogMessageTypes.Info, msg);
                    Helper.ShowMessage(msg);
                }
                catch (Exception ex)
                {
                    string msg = Helper.ProcessExceptionMessages(ex);
                    Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                    Helper.ShowMessage(msg);
                }

            }
        }
        void ClearView()
        {
            if (CanExit())
            {
                ClearContractData();
                RaiseViewStateChanged(ViewState.Blank);
            }
        }
        void OpenAddActivity()
        {
            var v = new AddActivityView();
            v.Owner = this;
            v.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (v.ShowDialog() == true)
            {
                Activities = LoadActivities();
            }
        }
        void PrintPhoto()
        {
            try
            {
                if (!File.Exists(PhotoPath))
                {
                    //If the Shown photo is just a photo in memory
                    //we will save it to the temp folder of the current user
                    //so we can point to an physical path when we ask
                    //Windows Print photos process to print our photo.
                    //every time we save the photo it will overwrite the 
                    //Exsiting one, so no worry of leaving too much files in
                    //the user temp folder.
                    var tempPath = Environment.GetEnvironmentVariable("TEMP");
                    PhotoPath = tempPath + "\\PrintedPhoto.jpg";
                    Bitmap bm = ConvertImageToBitmap(ContractImage);
                    bm.Save(PhotoPath);

                }
                Process p = new Process();
                p.StartInfo.FileName = PhotoPath;
                p.StartInfo.Verb = "Print";
                p.Start();
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }

        void OpenScanDialog()
        {
            ScanDialog sdl = new ScanDialog();
            sdl.Owner = this;
            sdl.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            try
            {
                if (sdl.ShowDialog() == true)
                {
                    ContractImage = ConvertBitmapToImage(sdl.ScannedImage);
                    PhotoPath = Helper.CreateContractPhotoPath(ContractNo);
                    _savedImageChanged = true;
                }
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
                sdl.Close();
            }

        }
        void OpenSharedFolder()
        {
            SharedFolderDialog sfdl = new SharedFolderDialog();
            sfdl.Owner = this;
            sfdl.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            try
            {
                if (sfdl.ShowDialog() == true)
                {
                    ContractImage = sfdl.SelectedImage;
                    PhotoPath = Helper.CreateContractPhotoPath(ContractNo);
                    _savedImageChanged = true;
                }

            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
                sfdl.Close();

            }
        }
        void PrintMonthlyContract()
        {
            try
            {
                //if (AgreedDeposit == 0)
                //{
                //    string msg =  Properties.Resources.ContractView_DepositMissing;
                //    if (!Helper.UserConfirmed(msg)) return;
                //}
                //MonthlyContractView mv = new MonthlyContractView(ContractNo);
                //mv.Owner = this;
                //mv.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                //mv.ShowDialog();
                string path = Properties.Settings.Default.MonthlyContractTemplate;
                Print(path);


            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }

        }
        private void OpenCalculator()
        {
            ContractDueCalcView cdcv = new ContractDueCalcView(ContractNo);
            cdcv.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cdcv.ShowDialog();
        }

        void OpenContract()
        {
            try
            {
                OpenContractDialog ocd = new OpenContractDialog(null);
                ocd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ocd.ShowDialog();
                int cn = ocd.SelectedContract.ContractNo;
                Search(cn);
            }
            catch (InvalidOperationException)
            {
                string msg = Properties.Resources.ContractView_ContractNotExist;
                Logger.Log(LogMessageTypes.Info, msg);
                Helper.ShowMessage(msg);
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);
            }
        }
        bool CanOpenCalc()
        {
            return _canSave;
        }
        bool CanPrintMonthlyContract()
        {
            return _canPrint;
        }

        bool CanAddActivity()
        {
            return _canAddActivity;
        }
        bool CanSave()
        {
            return _canSave;
        }
        bool CanSearch()
        {
            return _canSearch;
        }
        bool CanPrint()
        {
            return _canPrint;
        }
        bool CanPrintPhoto()
        {
            return ContractImage != null;
        }
        bool CanScan()
        {
            return _canScan;
        }
        bool CanOpenSharedFolder()
        {
            return _canOpenSharedFolder;
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

        #region HelperMethods

        private void TryAutocomplete()
        {
            if (string.IsNullOrEmpty(SignDay) && string.IsNullOrEmpty(GregDate) && string.IsNullOrEmpty(HijriDate))
            {
                string dayOfWeek;
                if (_weekDaysArabicNames.TryGetValue(Helper.GetDayOfWeek(), out dayOfWeek))
                {
                    SignDay = dayOfWeek;
                }
                HijriDate = Helper.GetCurrentDate();
                GregDate = Helper.GetCurrentGregDate();
            }
            if (string.IsNullOrEmpty(City) && string.IsNullOrEmpty(LocationDistrict))
            {
                int branch = Properties.Settings.Default.Branch;
                if (branch == 2)
                {
                    City = "جدة";
                    LocationDistrict = "البلد";
                }
            }
        }

        private bool TryGetPropteryLocation(string locationNo, out string propertyLocation)
        {
            string loc;
            if (_propertiesLookup.TryGetValue(locationNo, out loc))
            {
                propertyLocation = loc;
                return true;
            }
            propertyLocation = null;
            return false;

        }
        /// <summary>
        /// Find the contract for the given no.
        /// </summary>
        /// <param name="contractNo"></param>
        /// <returns>Contract if exist, otherwise null</returns>
        Contract FindContract(int contractNo)
        {
            var contractRepos = _unitOfWork.Contracts;
            return contractRepos.GetById(contractNo);
        }
        void ShowContract(Contract contract)
        {
            if (contract == null) throw new ArgumentNullException("contract");
            ContractNo = contract.ContractNo;
            SignDay = contract.SignDay;
            HijriDate = contract.SignHijriDate;
            GregDate = contract.SignGregDate;
            SelectedActivity = contract.ContractsActivity;
            CustomerName = contract.Customer.Name;
            IdNo = contract.Customer.IdNumber;
            IdDate = contract.Customer.IdDate;
            IdIssue = contract.Customer.IdIssue;
            IdType = contract.Customer.IdType;
            Nationality = contract.Customer.Nationality;
            AddressLine1 = contract.Customer.AddressLine1;
            AddressLine2 = contract.Customer.AddressLine2;
            PropertyType = contract.Property.Type;
            PropertyNo = ExtractProeprtyNo(contract.Property.PropertyNo);
            PropertyLocation = ExtractPropertyLocation(contract.PropertyNo, contract.Property.Location);
            LocationDistrict = contract.Property.District;
            City = contract.Property.City;
            ContractPeriod = GetContractPeriod(contract.StartDate, contract.EndDate);
            StartDate = contract.StartDate;
            EndDate = contract.EndDate;
            AgreedRent = contract.AgreedRent;
            AgreedDeposit = contract.AgreedDeposit;
            AgreedMaintenance = "حسب الفواتير";
            FullPropertyNo = contract.PropertyNo;
            ShowPhoto(contract.PhotoPath);
        }

        private string ExtractPropertyLocation(string propertyNo, string propertyName)
        {
            string locNo = null;
            if (!string.IsNullOrEmpty(propertyNo))
            {
                locNo = propertyNo.Substring(0, 3);
            }
            else
            {
                locNo = "0";
            }
            string printedLoc;
            if (!TryGetPropteryLocation(locNo, out printedLoc))
            {
                printedLoc = propertyName;
            }
            return printedLoc;

        }
        public static string ExtractProeprtyNo(string propertyNo)
        {
            if (propertyNo.Length < 6) return "";//These are special cases(aka ارض النحال و عزلة حارة اليمن) will be add to the printed contract by hand.
            string numPortion = propertyNo.Substring(4);
            if (numPortion.Length == 2)
            {
                string firstDigit = numPortion.Substring(0, 1);
                if (firstDigit == "0")
                {
                    string realNo = numPortion.Substring(1, 1);
                    return realNo;
                }
                return numPortion;
            }
            if (numPortion.Length == 3)
            {
                string firstTwoDigit = numPortion.Substring(0, 2);
                if (firstTwoDigit == "00")
                {
                    string realNo = numPortion.Substring(2, 1);
                    return realNo;
                }
                string firstDigit = numPortion.Substring(0, 1);
                if (firstDigit == "0")
                {
                    string realNo = numPortion.Substring(1, 2);
                    return realNo;
                }
                return numPortion;
            }
            return "";
        }
        enum ViewState
        {
            Blank,
            Saved,
            Edited
        }


        private void ControlState(ViewState state)
        {
            _currentViewState = state;
            switch (_currentViewState)
            {
                case ViewState.Blank:
                    _canExit = true;
                    _canSearch = true;
                    _canSave = false;
                    _canAddActivity = false;
                    _canPrint = false;
                    _canScan = false;
                    _canOpenSharedFolder = false;
                    txtSearch.Text = "";
                    txtSearch.Focus();
                    _savedImageChanged = false;
                    break;
                case ViewState.Saved:
                    _canExit = true;
                    _canSearch = false;
                    _canSave = true;
                    _canAddActivity = true;
                    _canPrint = true;
                    _canScan = true;
                    _canOpenSharedFolder = true;
                    txtSignDay.Focus();
                    _savedImageChanged = false;
                    break;
                case ViewState.Edited:
                    _canExit = false;
                    _canSearch = false;
                    _canSave = true;
                    _canAddActivity = true;
                    _canPrint = true;
                    _canScan = true;
                    _canOpenSharedFolder = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void RaiseViewStateChanged(ViewState state)
        {
            if (ViewStateChanged != null)
            {
                ViewStateChanged(this, state);
            }

        }

        bool ValidContractForPrint()
        {
            //if (AgreedDeposit <= 0)
            //{
            //    _prompetMessage = Properties.Resources.ContractView_InvalidForPrint2;
            //    return false;
            //}

            if (string.IsNullOrEmpty(SignDay))
            {
                _prompetMessage = Properties.Resources.ContractView_InvalidForPrint1;
                return false;
            }

            if (string.IsNullOrEmpty(HijriDate))
            {
                _prompetMessage = Properties.Resources.ContractView_InvalidForPrint4;
                return false;
            }
            if (string.IsNullOrEmpty(GregDate))
            {
                _prompetMessage = Properties.Resources.ContractView_InvalidForPrint5;
                return false;
            }

            if (string.IsNullOrEmpty(LocationDistrict))
            {
                _prompetMessage = Properties.Resources.ContractView_InvalidForPrint3;
                return false;
            }
            if (string.IsNullOrEmpty(City))
            {
                _prompetMessage = Properties.Resources.ContractView_InvalidForPrint6;
                return false;
            }
            return true;

        }
        void InitilizeFields()
        {
            try
            {
                _unitOfWork = new UnitOfWork();
                _ruleViolations = new List<RuleViolation>();
                Activities = LoadActivities();
                RaiseViewStateChanged(ViewState.Blank);
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);

            }

        }



        bool ValidContract()
        {
            return true;
        }
        void WriteValues(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }
            contract.SignDay = SignDay;
            contract.SignHijriDate = HijriDate;
            contract.SignGregDate = GregDate;
            contract.ContractsActivity = SelectedActivity;
            contract.AgreedDeposit = AgreedDeposit;
            contract.Property.District = LocationDistrict;
            contract.Property.City = City;
            contract.PhotoPath = PhotoPath;
        }

        private bool CanExit()
        {
            if (!_canExit)
            {
                string msg = Properties.Resources.SavePrompetMsg;
                bool agree = Helper.UserConfirmed(msg);
                return agree;
            }
            return _canExit;
        }

        private ObservableCollection<ContractsActivity> LoadActivities()
        {
            var acts = _unitOfWork.ContractsActivities.GetAll();
            return new ObservableCollection<ContractsActivity>(acts);
        }

        private void GetReadyForStartup()
        {
            Loaded += ContractViewLoaded;
            ViewStateChanged += OnViewStateChanged;
            _propertiesLookup = BuildPropertiesLookup();
            _weekDaysArabicNames = BuildWeekDays();
            int branch = Properties.Settings.Default.Branch;
            if (branch == 1)
            {
                cmbCourt.SelectedIndex = 0;
            }
            else if (branch == 2)
            {
                cmbCourt.SelectedIndex = 2;
            }
            else
            {
                cmbCourt.SelectedIndex = -1;
            }
        }

        private static Dictionary<string, string> BuildPropertiesLookup()
        {
            Dictionary<string, string> pro = new Dictionary<string, string>()
            {
               {"201", "عمائر هاشم"},
               {"202", "عمارة الصالحية"},
               {"204", "عمارة الشرائبي"},
               {"205", "عمارة العمار"},
               {"206", "عمارة المظلوم"},
               {"207", "عمارة العسالة"},
               {"208", "الدور القديمة"},
               {"209", "الدور القديمة"},
               {"210", "الدور القديمة"},
               {"211", "شارع الأشراف"},
               {"212", "عمارة الخاسكية"},
               {"213", "ارض النحال"},
            };
            return pro;
        }

        private static Dictionary<string, string> BuildWeekDays()
        {
            return new Dictionary<string, string>()
            {
                {"Sunday", "الأحد"},
                {"Monday", "الإثنين"},
                {"Tuesday", "الثلاثاء"},
                {"Wednesday", "الأربعاء"},
                {"Thursday", "الخميس"},
                {"Friday", "الجمعة"},
                {"Saturday", "السبت"}

            };
        }

        private void ShowPhoto(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    PhotoPath = null;
                    var unsignedContract = Properties.Resources.UnsignedContract;
                    Graphics graphicImage = Graphics.FromImage(unsignedContract);
                    graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
                    PrintOnImage(graphicImage);
                    ContractImage = ConvertBitmapToImage(unsignedContract);

                }
                else
                {
                    PhotoPath = path;
                    ContractImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(PhotoPath));
                }
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }

        }
        /// <summary>
        /// Create System.Windows.Media.Imaging.BitmapImage from 
        /// System.Drawing.Bitmap
        /// </summary>
        /// <param name="bm">The System.Drawing.Bitmap to be converted</param>
        /// <returns>The created System.Windows.Media.Imaging.BitmapImage</returns>
        private System.Windows.Media.Imaging.BitmapImage ConvertBitmapToImage(Bitmap bm)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bi.EndInit();
                return bi;
            }
        }
        /// <summary>
        /// Create System.Drawing.Bitmap from 
        /// System.Windows.Media.Imaging.BitmapImage
        /// </summary>
        /// <param name="bitmapImage">The System.Windows.Media.Imaging.BitmapImage To converted.</param>
        /// <returns>The created System.Drawing.Bitmap</returns>
        private Bitmap ConvertImageToBitmap(System.Windows.Media.Imaging.BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                System.Windows.Media.Imaging.BitmapEncoder enc = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                // return bitmap; <-- leads to problems, stream is closed/closing ...
                return new Bitmap(bitmap);
            }
        }
        void Search(int cn)
        {
                   var result = FindContract(cn);
                    ShowContract(result);
                    TryAutocomplete();
                    RaiseViewStateChanged(ViewState.Saved);


        }
        #endregion

        #region Events
        void ContractViewLoaded(object sender, RoutedEventArgs e)
        {
            InitilizeFields();

        }
        private void OnViewStateChanged(object sender, ViewState e)
        {
            ControlState(e);
        }
        private void OnActivitySelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void OnGridContentKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Helper.MoveFocus(e);
        }
        private void ClearContractData()
        {
            ContractNo = 0;
            SignDay = "";
            HijriDate = "";
            GregDate = "";
            SelectedActivity = null;
            cmbActivities.SelectedIndex = -1;
            CustomerName = "";
            IdNo = "";
            IdDate = "";
            IdIssue = "";
            IdType = "";
            Nationality = "";
            City = "";
            AddressLine1 = "";
            AddressLine2 = "";
            PropertyType = "";
            PropertyNo = "";
            PropertyLocation = "";
            LocationDistrict = "";
            ContractPeriod = "";
            StartDate = "";
            EndDate = "";
            AgreedRent = 0;
            AgreedDeposit = 0;
            AgreedMaintenance = "";
            PhotoPath = null;
            ContractImage = null;
        }
        private string GetContractPeriod(string startDate, string endDate)
        {

            string startPortion = startDate.Substring(4, 4);
            string endPortion = endDate.Substring(4, 4);
            if (startPortion == "0101" && endPortion == "1230" ||
               startPortion == "0101" && endPortion == "1229") return FULL_YEAR;
            return PART_OF_YEAR;


        }
        #endregion



        //************************Take Care From Here On******************************************************


        #region Printing Methods ****** TAKE CARE WHEN EDIT, A MISTAKE WILL COST YOU A LOT OF TESTING AND DEBUG ******

        private void PrintOnImage(Graphics g)
        {
            using (g)
            {
                g.PageUnit = GraphicsUnit.Millimeter;
                g.PageScale = 1;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var sf = StringFormat.GenericTypographic)
                {
                    sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    sf.Trimming = StringTrimming.Word;


                    //Note: Don't mix StringFormatFlags.DirectionRightToLeft with
                    //StringAlignment as using any of StringAlignment enums will
                    //cancel the effect of DirectionRightToLeft.
                    //sf.Alignment = StringAlignment.Far;

                    using (var font = new Font("Simplified Arabic", 12f, System.Drawing.FontStyle.Bold))
                    {
                        Brush brush = new SolidBrush(Color.Black);
                        //Contract No
                        float x = g.VisibleClipBounds.Right - XCmToMm(12f);
                        float y = g.VisibleClipBounds.Top + YCmToMm(2.8f);
                        g.DrawString(ContractNo.ToString(), font, brush, x, y, sf);
                        //Sign Day
                        x = g.VisibleClipBounds.Right - XCmToMm(3f);
                        y = g.VisibleClipBounds.Top + YCmToMm(4.1f);
                        g.DrawString(SignDay, font, brush, x, y, sf);
                        //Sign Hijri Date
                        x = g.VisibleClipBounds.Right - XCmToMm(6.2f);
                        y = g.VisibleClipBounds.Top + YCmToMm(4.1f);
                        g.DrawString(Helper.PutMask(HijriDate), font, brush, x, y, sf);
                        //Sign Greg Date
                        x = g.VisibleClipBounds.Right - XCmToMm(10.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(4.1f);
                        g.DrawString(Helper.PutMask(GregDate), font, brush, x, y, sf);
                        //Customer Name
                        x = g.VisibleClipBounds.Right - XCmToMm(2.8f);
                        y = g.VisibleClipBounds.Top + YCmToMm(6.9f);
                        g.DrawString(CustomerName, font, brush, x, y, sf);
                        //Nationality
                        x = g.VisibleClipBounds.Right - XCmToMm(17.9f);
                        y = g.VisibleClipBounds.Top + YCmToMm(6.9f);
                        g.DrawString(Nationality, font, brush, x, y, sf);
                        //Id Type
                        x = g.VisibleClipBounds.Right - XCmToMm(3.4f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7.69f);
                        g.DrawString(IdType, font, brush, x, y, sf);
                        //ID Number
                        x = g.VisibleClipBounds.Right - XCmToMm(7f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7.69f);
                        g.DrawString(IdNo, font, brush, x, y, sf);
                        //ID Date
                        x = g.VisibleClipBounds.Right - XCmToMm(12.3f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7.69f);
                        g.DrawString(Helper.PutMask(IdDate), font, brush, x, y, sf);
                        //ID Issue
                        x = g.VisibleClipBounds.Right - XCmToMm(18f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7.69f);
                        g.DrawString(IdIssue, font, brush, x, y, sf);
                        //Address Line 1
                        x = g.VisibleClipBounds.Right - XCmToMm(3.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(8.39f);
                        g.DrawString(AddressLine1, font, brush, x, y, sf);
                        //Address Line 2
                        x = g.VisibleClipBounds.Right - XCmToMm(3.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(9.19f);
                        g.DrawString(AddressLine2, font, brush, x, y, sf);
                        //Property Type
                        x = g.VisibleClipBounds.Right - XCmToMm(6.8f);
                        y = g.VisibleClipBounds.Top + YCmToMm(10.55f);
                        g.DrawString(PropertyType, font, brush, x, y, sf);
                        //Property No
                        x = g.VisibleClipBounds.Right - XCmToMm(9.2f);
                        y = g.VisibleClipBounds.Top + YCmToMm(10.55f);
                        g.DrawString(PropertyNo, font, brush, x, y, sf);
                        //Property Location
                        x = g.VisibleClipBounds.Right - XCmToMm(14f);
                        y = g.VisibleClipBounds.Top + YCmToMm(10.55f);
                        g.DrawString(PropertyLocation, font, brush, x, y, sf);
                        //Location City
                        x = g.VisibleClipBounds.Right - XCmToMm(19.4f);
                        y = g.VisibleClipBounds.Top + YCmToMm(10.55f);
                        g.DrawString(City, font, brush, x, y, sf);
                        //Location District
                        x = g.VisibleClipBounds.Right - XCmToMm(3f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11.19f);
                        g.DrawString(LocationDistrict, font, brush, x, y, sf);
                        //Activity Description
                        if (SelectedActivity != null)
                        {
                            x = g.VisibleClipBounds.Right - XCmToMm(7.1f);
                            y = g.VisibleClipBounds.Top + YCmToMm(11.19f);
                            g.DrawString(SelectedActivity.Description, font, brush, x, y, sf);
                        }
                        //Period
                        x = g.VisibleClipBounds.Right - XCmToMm(4f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11.8f);
                        g.DrawString(ContractPeriod, font, brush, x, y, sf);
                        //Start Date
                        x = g.VisibleClipBounds.Right - XCmToMm(8.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11.8f);
                        g.DrawString(Helper.PutMask(StartDate), font, brush, x, y, sf);
                        //End Date
                        x = g.VisibleClipBounds.Right - XCmToMm(13.7f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11.8f);
                        g.DrawString(Helper.PutMask(EndDate), font, brush, x, y, sf);
                        //Agreed Rent
                        string formatedRent = AgreedRent.ToString("#,#") + " " + RYAL;
                        x = g.VisibleClipBounds.Right - XCmToMm(7.8f);
                        y = g.VisibleClipBounds.Top + YCmToMm(12.99f);
                        g.DrawString(formatedRent, font, brush, x, y, sf);
                        //Agreed Rent Words
                        x = g.VisibleClipBounds.Right - XCmToMm(10.8f);
                        y = g.VisibleClipBounds.Top + YCmToMm(12.99f);
                        g.DrawString(AgreedRentWords, font, brush, x, y, sf);
                        //Agreed Deposit
                        string formatedDeposit = AgreedDeposit.ToString("#,#") + " " + RYAL;
                        x = g.VisibleClipBounds.Right - XCmToMm(7.2f);
                        y = g.VisibleClipBounds.Top + YCmToMm(16.39f);
                        g.DrawString(formatedDeposit, font, brush, x, y, sf);
                        //Agreed Deposit Words
                        x = g.VisibleClipBounds.Right - XCmToMm(10f);
                        y = g.VisibleClipBounds.Top + YCmToMm(16.39f);
                        g.DrawString(AgreedDepositWords, font, brush, x, y, sf);
                        //Court
                        string selectedCourt = ((System.Windows.Controls.ComboBoxItem)cmbCourt.SelectedItem).Content as string;
                        x = g.VisibleClipBounds.Right - XCmToMm(10.99f);
                        y = g.VisibleClipBounds.Top + YCmToMm(19.1f);
                        g.DrawString(selectedCourt, font, brush, x, y, sf);
                    }
                }
            }

        }

        //Print the contract Using Gdi+ Graphics.DrawString
        //Which results in some letters being not connected properly when
        //printing using the Traditional Arabic or Simplified Arabic fonts.
        //although it's simpler in use(no scale is needed to print on the printer) and more accurate thant TextRender.DrawText.
        //private void OnPringPage(object sender, PrintPageEventArgs e)
        //{
        //    using (Graphics g = e.Graphics)
        //    {
        //        g.PageUnit = GraphicsUnit.Millimeter;
        //        g.PageScale = 1;
        //        g.SmoothingMode = SmoothingMode.AntiAlias;
        //        g.TextRenderingHint = TextRenderingHint.AntiAlias;
        //        g.PixelOffsetMode = PixelOffsetMode.HighQuality;


        //        using (var sf = StringFormat.GenericTypographic)
        //        {
        //            sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
        //            sf.Trimming = StringTrimming.Word;


        //            //Note: Don't mix StringFormatFlags.DirectionRightToLeft with
        //            //StringAlignment as using any of StringAlignment enums will
        //            //cancel the effect of DirectionRightToLeft.
        //            //sf.Alignment = StringAlignment.Far;

        //            using (var font = new Font("Simplified Arabic", 12.5f, System.Drawing.FontStyle.Regular))
        //            {
        //                Brush brush = new SolidBrush(Color.Black);
        //                //Contract No
        //                float x = g.VisibleClipBounds.Right - XCmToMm(10f);
        //                float y = g.VisibleClipBounds.Top + YCmToMm(2.6f);
        //                g.DrawString(ContractNo.ToString(), font, brush, x, y, sf);
        //                //Sign Day
        //                 x = g.VisibleClipBounds.Right - XCmToMm(1f);
        //                 y = g.VisibleClipBounds.Top + YCmToMm(4f);
        //                g.DrawString(SignDay, font, brush, x, y, sf);
        //                //Sign Hijri Date
        //                x = g.VisibleClipBounds.Right - XCmToMm(4.5f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(4f);
        //                g.DrawString(Helper.PutMask(HijriDate), font, brush, x, y, sf);
        //                //Sign Greg Date
        //                x = g.VisibleClipBounds.Right - XCmToMm(9f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(4f);
        //                g.DrawString(Helper.PutMask( FollowingDate), font, brush, x, y, sf);
        //                //Customer Name
        //                x = g.VisibleClipBounds.Right - XCmToMm(1f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(6.2f);
        //                g.DrawString(CustomerName, font, brush, x, y, sf);
        //                //ID Number
        //                x = g.VisibleClipBounds.Right - XCmToMm(3.5f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(7f);
        //                g.DrawString(IdNo, font, brush, x, y, sf);
        //                //ID Date
        //                x = g.VisibleClipBounds.Right - XCmToMm(9f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(7f);
        //                g.DrawString(Helper.PutMask(IdDate), font, brush, x, y, sf);
        //                //ID Issue
        //                x = g.VisibleClipBounds.Right - XCmToMm(14.5f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(7f);
        //                g.DrawString(IdIssue, font, brush, x, y, sf);
        //                //Address
        //                x = g.VisibleClipBounds.Right - XCmToMm(2.5f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(7.5f);
        //                g.DrawString(AddressLine1, font, brush, x, y, sf);
        //                //Property Type
        //                x = g.VisibleClipBounds.Right - XCmToMm(6.5f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
        //                g.DrawString(PropertyType, font, brush, x, y, sf);
        //                //Property No
        //                x = g.VisibleClipBounds.Right - XCmToMm(11f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
        //                g.DrawString(PropertyNo, font, brush, x, y, sf);
        //                //Property Location
        //                x = g.VisibleClipBounds.Right - XCmToMm(13.8f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
        //                g.DrawString(PropertyLocation, font, brush, x, y, sf);
        //                //Location District
        //                x = g.VisibleClipBounds.Right - XCmToMm(18.5f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
        //                g.DrawString(LocationDistrict, font, brush, x, y, sf);
        //                //Activity Description
        //                x = g.VisibleClipBounds.Right - XCmToMm(4f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(10.3f);
        //                g.DrawString("بيع الذهب والمحوهرات", font, brush, x, y, sf);
        //                //Start Date
        //                x = g.VisibleClipBounds.Right - XCmToMm(8.2f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(11f);
        //                g.DrawString(Helper.PutMask(StartDate), font, brush, x, y, sf);
        //                //End Date
        //                x = g.VisibleClipBounds.Right - XCmToMm(13f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(11f);
        //                g.DrawString(Helper.PutMask(EndDate), font, brush, x, y, sf);
        //                //Agreed Rent
        //                x = g.VisibleClipBounds.Right - XCmToMm(5f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(11.5f);
        //                g.DrawString(AgreedRent.ToString("#,#"), font, brush, x, y, sf);
        //                //Agreed Rent Words
        //                x = g.VisibleClipBounds.Right - XCmToMm(10.3f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(11.5f);
        //                g.DrawString(AgreedRentWords, font, brush, x, y, sf);
        //                //Agreed Deposit
        //                x = g.VisibleClipBounds.Right - XCmToMm(6.8f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(14f);
        //                g.DrawString(AgreedDeposit.ToString("#,#"), font, brush, x, y, sf);
        //                //Agreed Deposit Words
        //                x = g.VisibleClipBounds.Right - XCmToMm(10f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(14f);
        //                g.DrawString(AgreedDepositWords, font, brush, x, y, sf);
        //                //Agreed Maintenance
        //                x = g.VisibleClipBounds.Right - XCmToMm(10f);
        //                y = g.VisibleClipBounds.Top + YCmToMm(18.5f);
        //                g.DrawString(AgreedMaintenance, font, brush, x, y, sf);
        //            }
        //        }
        //    }
        //}

        private void OnPringPage(object sender, PrintPageEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                Font font = new Font("Simplified Arabic", 12, System.Drawing.FontStyle.Bold);
                font = PrintingHelper.ScaleFont(g, font);
                TextFormatFlags formatFlags = TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                Color fontColor = Color.Black;
                var size = new System.Drawing.Size((int)e.Graphics.VisibleClipBounds.Size.Width, (int)e.Graphics.VisibleClipBounds.Size.Height);
                size = PrintingHelper.ScaleSize(g, size);

                //Contract No
                Font contractNoFont = new Font("Simplified Arabic", 16, System.Drawing.FontStyle.Bold);
                contractNoFont = PrintingHelper.ScaleFont(g, contractNoFont);
                int x = PrintingHelper.ScaleCm(-10.7f);
                int y = PrintingHelper.ScaleCm(2.290f);
                var pt = PrintingHelper.ScalePoint(g, x, y);
                var contractNoSize = new System.Drawing.Size((int)e.Graphics.VisibleClipBounds.Size.Width, (int)e.Graphics.VisibleClipBounds.Size.Height);
                contractNoSize = PrintingHelper.ScaleSize(g, contractNoSize);
                var rectangle = new Rectangle(pt, contractNoSize);
                TextRenderer.DrawText(g, ContractNo.ToString(), contractNoFont, rectangle, fontColor, formatFlags);
                //Sign Day
                x = PrintingHelper.ScaleCm(-2.5f);
                y = PrintingHelper.ScaleCm(3.6f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, SignDay, font, rectangle, fontColor, formatFlags);
                //Hijri Date
                x = PrintingHelper.ScaleCm(-6f);
                y = PrintingHelper.ScaleCm(3.6f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, Helper.PutMask(HijriDate) + " هـ", font, rectangle, fontColor, formatFlags);
                //Greg Date
                x = PrintingHelper.ScaleCm(-10.2f);
                y = PrintingHelper.ScaleCm(3.6f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, Helper.PutMask(GregDate) + " م", font, rectangle, fontColor, formatFlags);
                //Customer Name
                x = PrintingHelper.ScaleCm(-2.2f);
                y = PrintingHelper.ScaleCm(6.3f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, CustomerName, font, rectangle, fontColor, formatFlags);
                //Nationality
                x = PrintingHelper.ScaleCm(-17.2f);
                y = PrintingHelper.ScaleCm(6.2f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, Nationality, font, rectangle, fontColor, formatFlags);
                //IdType
                x = PrintingHelper.ScaleCm(-3f);
                y = PrintingHelper.ScaleCm(7f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, IdType, font, rectangle, fontColor, formatFlags);
                //IdNo
                x = PrintingHelper.ScaleCm(-6.3f);
                y = PrintingHelper.ScaleCm(7f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, IdNo, font, rectangle, fontColor, formatFlags);
                //Id Date
                x = PrintingHelper.ScaleCm(-11.8f);
                y = PrintingHelper.ScaleCm(7f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, Helper.PutMask(IdDate), font, rectangle, fontColor, formatFlags);
                //Id Issue
                x = PrintingHelper.ScaleCm(-17.5f);
                y = PrintingHelper.ScaleCm(7f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, IdIssue, font, rectangle, fontColor, formatFlags);
                //Address Line 1
                x = PrintingHelper.ScaleCm(-3.2f);
                y = PrintingHelper.ScaleCm(7.8f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, AddressLine1, font, rectangle, fontColor, formatFlags);
                //Address Line 2
                x = PrintingHelper.ScaleCm(-3f);
                y = PrintingHelper.ScaleCm(8.5f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, AddressLine2, font, rectangle, fontColor, formatFlags);
                //Property Type
                x = PrintingHelper.ScaleCm(-6.3f);
                y = PrintingHelper.ScaleCm(9.9f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, PropertyType, font, rectangle, fontColor, formatFlags);
                //Property No
                x = PrintingHelper.ScaleCm(-8.7f);
                y = PrintingHelper.ScaleCm(9.9f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, PropertyNo, font, rectangle, fontColor, formatFlags);
                //Proprty Location
                x = PrintingHelper.ScaleCm(-13.5f);
                y = PrintingHelper.ScaleCm(9.9f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, PropertyLocation, font, rectangle, fontColor, formatFlags);
                //Location City
                x = PrintingHelper.ScaleCm(-18.9f);
                y = PrintingHelper.ScaleCm(9.9f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, City, font, rectangle, fontColor, formatFlags);

                //Location District
                x = PrintingHelper.ScaleCm(-3f);
                y = PrintingHelper.ScaleCm(10.5f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, LocationDistrict, font, rectangle, fontColor, formatFlags);
                //Customer Activity
                if (SelectedActivity != null)
                {
                    x = PrintingHelper.ScaleCm(-7.3f);
                    y = PrintingHelper.ScaleCm(10.5f);
                    pt = PrintingHelper.ScalePoint(g, x, y);
                    rectangle = new Rectangle(pt, size);
                    TextRenderer.DrawText(g, SelectedActivity.Description, font, rectangle, fontColor, formatFlags);
                }

                //Contract Period
                x = PrintingHelper.ScaleCm(-3.9f);
                y = PrintingHelper.ScaleCm(11.2f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, ContractPeriod, font, rectangle, fontColor, formatFlags);
                //Start Date
                x = PrintingHelper.ScaleCm(-8f);
                y = PrintingHelper.ScaleCm(11.1f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, Helper.PutMask(StartDate), font, rectangle, fontColor, formatFlags);
                //End Date
                x = PrintingHelper.ScaleCm(-13.3f);
                y = PrintingHelper.ScaleCm(11.1f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, Helper.PutMask(EndDate), font, rectangle, fontColor, formatFlags);

                //Rent
                string printedAgreedRent = AgreedRent.ToString("#,#") + " " + RYAL;
                x = PrintingHelper.ScaleCm(-7.7f);
                y = PrintingHelper.ScaleCm(12.2f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, printedAgreedRent, font, rectangle, fontColor, formatFlags);
                //Rent Words
                x = PrintingHelper.ScaleCm(-10.40f);
                y = PrintingHelper.ScaleCm(12.2f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, AgreedRentWords, font, rectangle, fontColor, formatFlags);

                //Deposit
                string printedAgreedDeposit = "----";
                if (AgreedDeposit > 0)
                {
                    printedAgreedDeposit = AgreedDeposit.ToString("#,#") + " " + RYAL;
                }
                x = PrintingHelper.ScaleCm(-6.7f);
                y = PrintingHelper.ScaleCm(15.6f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, printedAgreedDeposit, font, rectangle, fontColor, formatFlags);
                //Deposit Words
                string printedDepositWords = "----";
                if (AgreedDeposit > 0)
                {
                    printedDepositWords = AgreedDepositWords;
                }
                x = PrintingHelper.ScaleCm(-9.3f);
                y = PrintingHelper.ScaleCm(15.6f);
                pt = PrintingHelper.ScalePoint(g, x, y);
                rectangle = new Rectangle(pt, size);
                TextRenderer.DrawText(g, printedDepositWords, font, rectangle, fontColor, formatFlags);

                //Court
                string selectedCourt = ((System.Windows.Controls.ComboBoxItem)cmbCourt.SelectedItem).Content as string;
                if (selectedCourt != null)
                {
                    x = PrintingHelper.ScaleCm(-11f);
                    y = PrintingHelper.ScaleCm(18.3f);
                    pt = PrintingHelper.ScalePoint(g, x, y);
                    rectangle = new Rectangle(pt, size);
                    TextRenderer.DrawText(g, selectedCourt, font, rectangle, fontColor, formatFlags);
                }

                //Maintainance
                //x = PrintingHelper.ScaleCm(-7f);
                //y = PrintingHelper.ScaleCm(19.7f);
                //pt = PrintingHelper.ScalePoint(g, x, y);
                //rectangle = new Rectangle(pt, size);
                //TextRenderer.DrawText(g, AgreedMaintenance, font, rectangle, fontColor, formatFlags);
                font.Dispose();
                contractNoFont.Dispose();

            }
        }

        private float XCmToMm(float cm)
        {
            //return (int)Math.Round((mm / 25.4) * dpi);
            return (cm * 10.00f);
        }

        private float YCmToMm(float cm)
        {
            return (cm * 10.00f);
        }
        #endregion



    }
}