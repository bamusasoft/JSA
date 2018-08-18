using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Jsa.ViewsModel.Views;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IController _notificationCenter;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
            DataContext = this;
            
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Helper.SetCurrentYear();
            NotificationCenter = new NotificationCenterController();
            
            
        }

        private void AppExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenSyncIrs(object sender, RoutedEventArgs e)
        {
            var v = new SyncIrsView();
            ShowChild(v, true);
        }

        private void OpenSchedule(object sender, RoutedEventArgs e)
        {
            var v = new ScheduleView();
            ShowChild(v);
        }

        private void OpenSigner(object sender, RoutedEventArgs e)
        {
            var v = new SignerView();
            ShowChild(v);
        }

        private void OpenPeriodSchedules(object sender, RoutedEventArgs e)
        {
            var v = new PeriodSchedulesView();
            ShowChild(v);
        }

        private void OpenOptions(object sender, RoutedEventArgs e)
        {
            var v = new OptionsView();
            ShowChild(v);
        }

        private void OpenLegalCase(object sender, RoutedEventArgs e)
        {
            LegalCaseView v = new LegalCaseView();
            ShowChild(v);
        }
        private void OpenFollowingCase(object sender, RoutedEventArgs e)
        {
            CaseFollowingView v = new CaseFollowingView();
            ShowChild(v);
        }
        private void OpenCaseAppointment(object sender, RoutedEventArgs e)
        {
            CaseAppointView v = new CaseAppointView();
            ShowChild(v);
        }

        private void ShowChild(Window view, bool modal = false)
        {
            //Note: setting the Maint window as owner of the child will affect how to find the child window in code
            //so, when looking in code for a child windows, instead of looking at application's windows collection,
            //look at main window's owned windows collection.
            view.Owner = this;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (modal)
            {
                view.ShowDialog();
                return;
            }
            view.Show();
        }

        private void OpenContracts(object sender, RoutedEventArgs e)
        {
            var v = new ContractsView();
            ShowChild(v);
        }

        private void OpenContract(object sender, RoutedEventArgs e)
        {
            var v = new ContractView();
            ShowChild(v);
        }

        private void OpenCustomer(object sender, RoutedEventArgs e)
        {
            var v = new CustomerView();
            ShowChild(v);
        }

        private void OpenBuklIresContracts(object sender, RoutedEventArgs e)
        {
            var v = new BulkIresContractsView();
            ShowChild(v);
        }

        private void OpenContractPayment(object sender, RoutedEventArgs e)
        {
            var v = new ContractPaymentsStatus();
            ShowChild(v);
        }

        private void OpenClaim(object sender, RoutedEventArgs e)
        {
            var v = new ClaimView();
            ShowChild(v);
        }

        private void OpenOutbox(object sender, RoutedEventArgs e)
        {
            var v = new OutboxView();
            ShowChild(v);
        }

        private void OpenRentMaint(object sender, RoutedEventArgs e)
        {
            var v = new ContractRentMaint();
            ShowChild(v);

        }

        public IController NotificationCenter
        {
            get { return _notificationCenter; }
            set
            {
                _notificationCenter = value;
                RaisePropertyChanged();
            }
        }
        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        #endregion

        private void OpenLegalCaseReport(object sender, RoutedEventArgs e)
        {
            var v = new CasesReportView();
            ShowChild(v);
        }

        private void OpenCustomersClassesView(object sender, RoutedEventArgs e)
        {
            var v = new CustomersClassesView();
            ShowChild(v);
        }

        private void OpenDocRecord(object sender, RoutedEventArgs e)
        {
            var v = new DocRecordView();
            ShowChild(v);

        }

        private void OpenDocRecordFollow(object sender, RoutedEventArgs e)
        {
            var v = new DocRecordFollowView();
            ShowChild(v);
        }

        private void OpenDocRecordReport(object sender, RoutedEventArgs e)
        {
            var v = new DocRecordsReportView();
            ShowChild(v);
        }

        private void OpenDocFollowsReport(object sender, RoutedEventArgs e)
        {
            var v = new DocFollowsReportView();
            ShowChild(v);
        }
    }
}