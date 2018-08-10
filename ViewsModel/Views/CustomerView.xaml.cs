using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window, INotifyPropertyChanged
    {
        #region "Constracutor"

        public CustomerView()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += OnWindowLoaded;
            Closing += OnWindowClosing;
            ViewStateChanged += OnViewStateChanged;
            CustomerNo = 000;
        }



        #endregion

        #region Fields

        private const string MOBILENO1 = "جوال رقم: ";
        private const string MOBILENO2 = "جوال رقم: ";
        private const string WORKPHONE = "هاتف العمل : ";
        private const string HOMEPHONE = "هاتف المنزل: ";
        private const string FAXNO = "فاكس رقم: ";
        private const string EMAIL = "إيميل: ";
        private string _addressLine1;
        private string _addressLine2;
        private bool _canExit;
        private bool _canSave;
        private bool _canSearch;
        private bool _canPrint;
        private RelayCommand _clearCommand;
        private ViewState _currentViewState;
        //
        private string _customerName;
        private int _customerNo;
        private string _email;
        private string _fax;
        private string _homePhone;
        private string _idDate;
        private string _idIssue;
        private string _idNo;
        private string _mobile1;
        private string _mobile2;
        private string _nationality;
        private string _idType;
        //
        private RelayCommand _saveCommand;
        private RelayCommand _searchCommand;
        private RelayCommand _printCommand;
        private RelayCommand _openCustomerDialogCommand;
        private IUnitOfWork _unitOfWork;
        private Customer _viewCustomer;
        private string _workPhone;
        private event EventHandler<ViewState> ViewStateChanged;
        private List<RuleViolation> _validationErrors;
        #endregion

        #region UI Properties

        public int CustomerNo
        {
            get { return _customerNo; }
            set
            {
                _customerNo = value;
                RaisePropertyChanged();
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
            set
            {
                _idNo = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string IdDate
        {
            get { return _idDate; }
            set
            {
                _idDate = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string IdIssue
        {
            get { return _idIssue; }
            set
            {
                _idIssue = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }
        public string Nationality
        {
            get
            {
                return _nationality;
            }
            set
            {
                _nationality = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }
        public string IdType
        {
            get { return _idType; }
            set
            {
                _idType = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }
        public string Mobile1
        {
            get { return _mobile1; }
            set
            {
                _mobile1 = value;
                RaisePropertyChanged();
                AddressLine2 = BuildSecondLineAddress();
                RaisePropertyChanged("AddressLine2");
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string Mobile2
        {
            get { return _mobile2; }
            set
            {
                _mobile2 = value;
                RaisePropertyChanged();
                //AddressLine2 = BuildSecondLineAddress();

                //RaisePropertyChanged("AddressLine2");
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string WorkPhone
        {
            get { return _workPhone; }
            set
            {
                _workPhone = value;
                RaisePropertyChanged();
                AddressLine2 = BuildSecondLineAddress();

                RaisePropertyChanged("AddressLine2");
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string HomePhone
        {
            get { return _homePhone; }
            set
            {
                _homePhone = value;
                RaisePropertyChanged();
                AddressLine2 = BuildSecondLineAddress();

                RaisePropertyChanged("AddressLine2");
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = value;
                RaisePropertyChanged();
                AddressLine2 = BuildSecondLineAddress();

                RaisePropertyChanged("AddressLine2");
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged();
                //AddressLine2 = BuildSecondLineAddress();
                //RaisePropertyChanged("AddressLine2");
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            set
            {
                _addressLine1 = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set
            {
                _addressLine2 = value;
                RaisePropertyChanged();
                if (_currentViewState != ViewState.Blank)
                {
                    RaiseViewStateChanged(ViewState.Edited);
                }
            }
        }

        #endregion

        #region Other Properties

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

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(Search, CanSearch)); }
        }

        public ICommand ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new RelayCommand(ClearView)); }
        }
        public ICommand PrintCommand
        {
            get { return _printCommand ?? (_printCommand = new RelayCommand(Print, CanPrint)); }
        }

        public ICommand OpenCustomerDialogCommand
        {
            get
            {
                return _openCustomerDialogCommand ??
                       (_openCustomerDialogCommand = new RelayCommand(OpenCustomer, CanSearch));
            }
        }
        #endregion

        #region Commands Methods

        private void Save()
        {
            Helper.ExplicitUpdateBinding();
            try
            {
                if (_currentViewState == ViewState.Edited)
                {
                    if (!ValidCustomer())
                    {
                        var error = _validationErrors.First();
                        Helper.ShowMessage(error.ErrorMessage);
                        return;
                    }

                    Customer cust = _unitOfWork.Customers.GetById(CustomerNo);
                    ReadCustomerValues(cust);
                    _unitOfWork.Save();
                    RaiseViewStateChanged(ViewState.Saved);

                }
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);
            }
        }

        private void Search()
        {
            string s = txtSearch.Text;
            int custNo;
            if (int.TryParse(s, out custNo))
            {
                try
                {
                    Customer cust = SearchCustomer(custNo);
                    ShowCustomer(cust);
                    RaiseViewStateChanged(ViewState.Saved);
                }
                catch (InvalidOperationException)
                {
                    string msg = Properties.Resources.CustomerView_CustomerNotExist;
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

        private void Print()
        {
            try
            {
                var customerContracts = _unitOfWork.Contracts.Query(c => c.CustomerId == CustomerNo && c.Closed != true);
                string path = Properties.Settings.Default.CustomerCardTemplatePath;
                if (customerContracts.Any())
                {
                    var pdg = new System.Windows.Forms.PrintDialog();
                    if (pdg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string selectedPrinter = pdg.PrinterSettings.PrinterName;
                        foreach (var contract in customerContracts)
                        {
                            CustomerPrinter printer = new CustomerPrinter(contract.Customer,
                                contract.Property.Description, contract.ContractNo, contract.StartDate);
                            printer.Print(path, selectedPrinter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);

            }
        }

        private void OpenCustomer()
        {
            try
            {
                OpenCustomerDialog ocd = new OpenCustomerDialog(OpenCustomerDialog.SearchField.CustomerName);
                if (ocd.ShowDialog() == true)
                {

                    var customer = SearchCustomer(ocd.SelectedCustomer.CustomerId);
                    ShowCustomer(customer);
                    RaiseViewStateChanged(ViewState.Saved);
                }
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);
            }
        }

        private bool CanPrint()
        {
            return _canPrint;
        }


        private void ClearView()
        {
            if (CanExit())
            {
                ClearCustomerData();
                RaiseViewStateChanged(ViewState.Blank);
            }
        }
        bool CanSave()
        {
            return _canSave;
        }
        bool CanSearch()
        {
            return _canSearch;
        }
        #endregion

        #region Events

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            InitializeFields();

        }

        private void OnViewStateChanged(object sender, ViewState e)
        {
            ControlState(e);
        }

        private void OnGridContentKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var uie = e.OriginalSource as UIElement;
            var textbox = uie as TextBox;
            if (textbox == null || textbox.AcceptsReturn)
            {
                return;
            }
            e.Handled = true;
            uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (!CanExit())
            {
                e.Cancel = true;
                return;
            }
        }
        #endregion

        #region Helper Functions

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

        private void RaiseViewStateChanged(ViewState state)
        {
            if (ViewStateChanged != null)
            {
                ViewStateChanged(this, state);
            }
        }

        private void InitializeFields()
        {
            _unitOfWork = new UnitOfWork();
            _validationErrors = new List<RuleViolation>();
            ControlState(ViewState.Blank);
        }

        private void ControlState(ViewState state)
        {
            _currentViewState = state;
            switch (_currentViewState)
            {
                case ViewState.Blank:

                    _canSave = false;
                    _canSearch = true;
                    _canExit = true;
                    _canPrint = false;
                    txtSearch.Text = "";
                    UpdateLayout(); // Needed to insure that the search textbox will get focus otherwise no.
                    txtSearch.Focus();
                    break;
                case ViewState.Saved:
                    _canSave = true;
                    _canSearch = false;
                    _canExit = true;
                    _canPrint = true;
                    txtIdNo.Focus();
                    break;
                case ViewState.Edited:
                    _canSave = true;
                    _canSearch = false;
                    _canExit = false;
                    _canPrint = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerNo"></param>
        /// <returns>Searched Customer, null if not exist</returns>
        private Customer SearchCustomer(int customerNo)
        {
            return _unitOfWork.Customers.GetById(customerNo);
        }

        private void ShowCustomer(Customer cust)
        {
            if (cust == null) throw new ArgumentNullException("cust");
            CustomerNo = cust.CustomerId;
            CustomerName = cust.Name;
            IdNo = cust.IdNumber;
            IdDate = cust.IdDate;
            IdIssue = cust.IdIssue;
            IdType = cust.IdType;
            Nationality = cust.Nationality;
            Mobile1 = cust.MainMobile;
            Mobile2 = cust.SecondMobile;
            WorkPhone = cust.WorkPhone;
            HomePhone = cust.HomePhone;
            Fax = cust.Fax;
            Email = cust.Email;
            AddressLine1 = cust.AddressLine1;
            AddressLine2 = cust.AddressLine2;
        }

        private void ClearCustomerData()
        {
            CustomerNo = 0;
            CustomerName = "";
            IdNo = "";
            IdDate = "";
            IdIssue = "";
            IdType = "";
            Nationality = "";
            Mobile1 = "";
            Mobile2 = "";
            WorkPhone = "";
            HomePhone = "";
            Fax = "";
            Email = "";
            AddressLine1 = "";
            AddressLine2 = "";
        }

        private void ReadCustomerValues(Customer cust)
        {
            if (cust == null) throw new ArgumentNullException("cust");
            cust.IdNumber = IdNo;
            cust.IdDate = IdDate;
            cust.IdIssue = IdIssue;
            cust.Nationality = Nationality;
            cust.IdType = IdType;
            cust.MainMobile = Mobile1;
            cust.SecondMobile = Mobile2;
            cust.WorkPhone = WorkPhone;
            cust.HomePhone = HomePhone;
            cust.Fax = Fax;
            cust.Email = Email;
            cust.AddressLine1 = AddressLine1;
            cust.AddressLine2 = AddressLine2;
        }



        private string BuildSecondLineAddress()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Mobile1))
            {
                sb.Append(MOBILENO1)
                    .Append(Mobile1)
                    .Append("-");
            }
            //if (!string.IsNullOrEmpty(Mobile2))
            //{
            //    sb.Append(MOBILENO2)
            //        .Append(Mobile2)
            //        .Append("-");  
            //}
            if (!string.IsNullOrEmpty(WorkPhone))
            {
                sb.Append(WORKPHONE)
                    .Append(WorkPhone)
                    .Append("-");
            }
            if (!string.IsNullOrEmpty(HomePhone))
            {
                sb.Append(HOMEPHONE)
                    .Append(HomePhone)
                    .Append("-");
            }
            if (!string.IsNullOrEmpty(Fax))
            {
                sb.Append(FAXNO)
                    .Append(Fax);
            }
            //if (!string.IsNullOrEmpty(Email))
            //{
            //    sb.Append(EMAIL)
            //        .Append(Email);
            //}

            return sb.ToString();
        }

        private enum ViewState
        {
            Blank,
            Saved,
            Edited
        }
        bool ValidCustomer()
        {
            _validationErrors.Clear();
            if (!string.IsNullOrEmpty(IdNo))
            {
                if (IdNo.Length != 10)
                {
                    var msg = Properties.Resources.CustomerView_InvalidId;
                    _validationErrors.Add(new RuleViolation(msg));
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(Mobile1))
            {
                if (Mobile1.Length != 10)
                {
                    var msg = Properties.Resources.CustomerView_InvalidMobile;
                    _validationErrors.Add(new RuleViolation(msg));
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(Mobile2))
            {
                if (Mobile2.Length != 10)
                {
                    var msg = Properties.Resources.CustomerView_InvalidMobile;
                    _validationErrors.Add(new RuleViolation(msg));
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(WorkPhone))
            {
                if (WorkPhone.Length != 9)
                {
                    var msg = Properties.Resources.CustomerView_InvalidPhone;
                    _validationErrors.Add(new RuleViolation(msg));
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(HomePhone))
            {
                if (HomePhone.Length != 9)
                {
                    var msg = Properties.Resources.CustomerView_InvalidPhone;
                    _validationErrors.Add(new RuleViolation(msg));
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(Fax))
            {
                if (Fax.Length != 9)
                {
                    var msg = Properties.Resources.CustomerView_InvalidFax;
                    _validationErrors.Add(new RuleViolation(msg));
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}