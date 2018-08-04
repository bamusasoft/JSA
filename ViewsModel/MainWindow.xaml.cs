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
            ShowChiled(v, true);
        }

        private void OpenSchedule(object sender, RoutedEventArgs e)
        {
            var v = new ScheduleView();
            ShowChiled(v);
        }

        private void OpenSigner(object sender, RoutedEventArgs e)
        {
            var v = new SignerView();
            ShowChiled(v);
        }

        private void OpenPeriodSchedules(object sender, RoutedEventArgs e)
        {
            var v = new PeriodSchedulesView();
            ShowChiled(v);
        }

        private void OpenOptions(object sender, RoutedEventArgs e)
        {
            var v = new OptionsView();
            ShowChiled(v);
        }

        private void OpenLegalCase(object sender, RoutedEventArgs e)
        {
            LegalCaseView v = new LegalCaseView();
            ShowChiled(v);
        }
        private void OpenFollowingCase(object sender, RoutedEventArgs e)
        {
            CaseFollowingView v = new CaseFollowingView();
            ShowChiled(v);
        }
        private void OpenCaseAppointment(object sender, RoutedEventArgs e)
        {
            CaseAppointView v = new CaseAppointView();
            ShowChiled(v);
        }

        private void ShowChiled(Window view, bool modal = false)
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
            ShowChiled(v);
        }

        private void OpenContract(object sender, RoutedEventArgs e)
        {
            var v = new ContractView();
            ShowChiled(v);
        }

        private void OpenCustomer(object sender, RoutedEventArgs e)
        {
            var v = new CustomerView();
            ShowChiled(v);
        }

        private void OpenBuklIresContracts(object sender, RoutedEventArgs e)
        {
            var v = new BulkIresContractsView();
            ShowChiled(v);
        }

        private void OpenContractPayment(object sender, RoutedEventArgs e)
        {
            var v = new ContractPaymentsStatus();
            ShowChiled(v);
        }

        private void OpenClaim(object sender, RoutedEventArgs e)
        {
            var v = new ClaimView();
            ShowChiled(v);
        }

        private void OpenOutbox(object sender, RoutedEventArgs e)
        {
            var v = new OutboxView();
            ShowChiled(v);
        }

        private void OpenRentMaint(object sender, RoutedEventArgs e)
        {
            var v = new ContractRentMaint();
            ShowChiled(v);

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
            ShowChiled(v);
        }

        private void OpenCustomersClassesView(object sender, RoutedEventArgs e)
        {
            var v = new CustomersClassesView();
            ShowChiled(v);
        }
    }
}