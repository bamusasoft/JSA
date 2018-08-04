using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Reports;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ContractPaymentsStatus.xaml
    /// </summary>
    public partial class ContractPaymentsStatus : Window, INotifyPropertyChanged
    {
        public ContractPaymentsStatus()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
            DataContext = this;
        }

        void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            GetReady();
        }

        private void GetReady()
        {
            txtPropertyNo.Focus();
            ToogleControls(ReportViewState.New);
        }
        #region "Fields"

        ContractPaymentsCriteria _criteria;
        ObservableCollection<ContractPayments> _contractsPayments;
        RelayCommand _searchCommand;
        RelayCommand _printCommand;
        double _progress;
        bool _canSearch;
        bool _canPrint;
        #endregion

        #region "Properties"
        public ContractPaymentsCriteria Criteria
        {
            get
            {
                if (_criteria == null)
                {
                    _criteria = new ContractPaymentsCriteria();
                }
                return _criteria;
            }
        }
        public ObservableCollection<ContractPayments> ContractsPayments
        {
            get { return _contractsPayments; }
            set
            {
                _contractsPayments = value;
                RaisePropertyChanged();
            }
        }
        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region "Commands"
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
        bool CanSearch()
        {
            return _canSearch;
        }
        bool CanPrint()
        {
            return _canPrint;
        }
        #endregion

        #region "Commands Methods"
        void Search()
        {
            try
            {
                var filter = Criteria.BuildCriteria();
                if (filter != null)
                {
                    using (IUnitOfWork unitOfWork = new UnitOfWork())
                    {
                        var contracts = unitOfWork.Contracts.Query(filter);
                        var cps = FillContractPayments(unitOfWork, contracts);
                        ContractsPayments = new ObservableCollection<ContractPayments>(cps);
                        ToogleControls(ReportViewState.Searched);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
            }
        }

        private IList<ContractPayments> FillContractPayments(IUnitOfWork unitOfWork, IQueryable<Contract> contracts)
        {
            List<ContractPayments> contractsPayments = new List<ContractPayments>();
            foreach (var contract in contracts)
            {
                List<ContractPaymentDetails> details = new List<ContractPaymentDetails>();
                var contractPayments = RetriveContractPayments(unitOfWork, contract.ContractNo);

                foreach (var payment in contractPayments)
                {
                    ContractPaymentDetails d = new ContractPaymentDetails()
                    {
                        PaymentNo = payment.PaymentNo,
                        PaymentDate = payment.PayDate,
                        Rent = payment.Rent,
                        Maintenance = payment.Maintenance,
                        Deposit = payment.Deposit,
                        Total = (int)payment.TotalPayment
                    };
                    details.Add(d);
                }
                ContractPayments cp = new ContractPayments()
                {
                    ContractNo = contract.ContractNo,
                    CustomerNo = contract.CustomerId,
                    CustomerName = contract.Customer.Name,
                    PropertyNo = contract.PropertyNo,
                    PropertyLocation = contract.Property.Description,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    AgreedRent = contract.AgreedRent,
                    RentDue = contract.RentDue,
                    RentBalance = contract.RentBalance,
                    MentDue = contract.AgreedMaintenance,
                    MentBalance = contract.MaintenanceBalance,
                    DepositDue = contract.AgreedDeposit,
                    DepositBalance =contract.DepositBalance,
                    PaymentsDetails = details
                };
                contractsPayments.Add(cp);
            }
            return contractsPayments;
        }

        private IQueryable<Payment> RetriveContractPayments(IUnitOfWork unitOfWork, int contractNo)
        { 
           return ((PaymentRepository)unitOfWork.Payments).GetContractPayments(contractNo);
        }
        private void Print()
        {
            PrintInExcel();
        }
        private async void PrintInExcel()
        {
            try
            {
                ToogleControls(ReportViewState.Busy);
                if (ContractsPayments == null || ContractsPayments.Count == 0) return;
                var report = RglReport.BuildReport(ContractsPayments);
                string templatPath = Properties.Settings.Default.RglTemplatePath;
                ExcelMail mail = new ExcelMail(RglReport.Layout);
                mail.ReportProgress += OnReportProgress;
                await mail.Send(report, templatPath, false, 3);

            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);
            }
            finally
            {
                ToogleControls(ReportViewState.Searched);
            }
        }
        void OnReportProgress(object sender, ProgressEventArgs e)
        {
            Progress = e.Progress;
        }
        #endregion
        void ToogleControls(ReportViewState state)
        {
            switch (state)
            {
                case ReportViewState.New:
                    _canSearch = true;
                    _canPrint = false;
                    prog.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case ReportViewState.Searched:
                    _canSearch = true;
                    _canPrint = true;
                    prog.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case ReportViewState.Busy:
                    _canSearch = false;
                    _canPrint = false;
                    prog.Visibility = System.Windows.Visibility.Visible;

                    break;
                default:
                    break;
            }
        }
        

        #region "INotifyPropertyChanged Members"
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void OnGridContentKeyDown(object sender, KeyEventArgs e)
        {
            Helper.MoveFocus(e);
        }

    }
}
