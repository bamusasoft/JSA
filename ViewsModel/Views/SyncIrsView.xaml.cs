using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.SyncStrategy;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for SyncIrsView.xaml
    /// </summary>
    public partial class SyncIrsView : INotifyPropertyChanged
    {
        public SyncIrsView()
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWork();
            DataContext = this;
            _settings = Settings.Default;
            prog.Visibility = Visibility.Hidden;
            Loaded += OnWindowLoaded;
            

        }

        
        private readonly Settings _settings;
        private IUnitOfWork _unitOfWork;
        private double _syncProgress;
        private string _currentOperation;
        private RelayCommand _saveCommand;
        private RelayCommand _syncCommand;

        private bool _saveEnabled;
        private bool _syncEnabled;


        string _syncYear;

        #region "Properties"
        public bool SaveEnabled
        {
            get { return _saveEnabled; }
            set
            {
                _saveEnabled = value;
                RaisePropertyChanged();
            }
        }
        
        public bool SyncEnabled
        {
            get
            {
                return _syncEnabled;
            }
            set
            {
                _syncEnabled = value;
                RaisePropertyChanged();
            }
        }
        
        
        public string CurrentOperation
        {
            get
            {
                return _currentOperation;
            }
            set
            {
                _currentOperation = value;
                RaisePropertyChanged();
            }
        }
        public string SyncYear
        {
            get 
            {
                return _syncYear;
            }
            set 
            {
                _syncYear = value;
            }
        }
        
       
        #endregion

        #region "Commands"
        public ICommand SyncCommand
        { 
            get
            {
                if (_syncCommand == null)
                {
                    _syncCommand = new RelayCommand(Sync, CanSync);
                    
                }
                return _syncCommand;
            }
        }
        void Sync()
        {
            SyncAsync();    
        }
        async void SyncAsync()
        {
            try
            {
                ToogleControls(SyncStatus.Started);
                Queue<ISyncIrs> q = new Queue<ISyncIrs>();
                //The order we add ISyncIrs objects as:
                //1- Properties, 2- Customer, 3-Conracts, 4-Payments, 5-SchedulesPayment
                //Is mandetory, due to the db design, thus, sync tables that relate to
                //other tables in correct order.
                if (ckbSyncProperties.IsChecked == true)
                {
                    ISyncIrs isi = new SyncProperties(_unitOfWork);
                    isi.ReportProgress += ShowSyncPropertiesProgress;
                    q.Enqueue(isi);
                }
                if (ckbSyncCusomers.IsChecked == true)
                {
                    ISyncIrs isi = new SyncCustomers(_unitOfWork);
                    isi.ReportProgress += ShowSyncCustomersProgress;
                    q.Enqueue(isi);
                }
                if (ckbSyncContracts.IsChecked == true)
                {
                    ISyncIrs isi = new SyncContracts(_unitOfWork, SyncYear);
                    isi.ReportProgress += ShowSyncContractsProgress;
                    q.Enqueue(isi);
                }
                if (ckbSyncPayments.IsChecked == true)
                {
                    ISyncIrs isi = new SyncPayments(_unitOfWork, SyncYear);
                    isi.ReportProgress += ShowSyncPaymentsProgress;
                    q.Enqueue(isi);
                }
                if (ckbSyncShedPays.IsChecked == true)
                {
                    ISyncIrs isi = new SyncSchedulesPayments(_unitOfWork, SyncYear);
                    isi.ReportProgress += ShowSyncScheduelesPaysProgress;
                    q.Enqueue(isi);
                }
                Queue<Task> allTasks = new Queue<Task>();
                foreach (var syncIrsObject in q)
                {
                    SyncIrsContext context = new SyncIrsContext(syncIrsObject);
                    await context.SyncAsync();
                }
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);
            }
            finally
            {
                ToogleControls(SyncStatus.Completed);
            }
            
        }

        
        bool CanSync()
        {
            return SyncEnabled;
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

        void Save()
        {
            try
            {
                _unitOfWork.Save();
                _unitOfWork.Dispose();
                _unitOfWork = new UnitOfWork();
            }
            catch (Exception ex)
            {
                string s = Helper.ProcessExceptionMessages(ex);
                MessageBox.Show(s);

            }
        }
        bool CanSave()
        {
            return SaveEnabled;
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region "Methods"

        #endregion

        private void ToogleControls(SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Started:
                    SaveEnabled = false;
                    SyncEnabled = false;
                    prog.Visibility = Visibility.Visible;
                    lblCurrentOperation.Visibility = Visibility.Visible;
                    ckbSyncContracts.IsEnabled = false;
                    ckbSyncCusomers.IsEnabled = false;
                    ckbSyncPayments.IsEnabled = false;
                    ckbSyncProperties.IsEnabled = false;
                    ckbSyncShedPays.IsEnabled = false;
                    break;
                case SyncStatus.Completed:
                    SaveEnabled = true;
                    SyncEnabled = true;
                    prog.Visibility = Visibility.Hidden;
                    lblCurrentOperation.Visibility = Visibility.Hidden;
                    ckbSyncContracts.IsEnabled = true;
                    ckbSyncCusomers.IsEnabled = true;
                    ckbSyncPayments.IsEnabled = true;
                    ckbSyncProperties.IsEnabled = true;
                    ckbSyncShedPays.IsEnabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
            ribbon.UpdateLayout();
            UpdateLayout();
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

     

        #region "Helper Methods"
        public double SyncProgress
        {
            get { return _syncProgress; }
            set
            {
                if ((int)value != (int)_syncProgress)
                {
                    _syncProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void SyncCompleted(Task tsks)
        {
            if (tsks != null)
            {
                if (tsks.IsFaulted)
                {
                    string s = null;
                    if (tsks.Exception != null)
                    {
                        foreach (Exception exception in tsks.Exception.InnerExceptions)
                        {
                            s += exception.Message + Environment.NewLine;
                            if (exception.InnerException != null)
                            {
                                s += exception.InnerException.Message + Environment.NewLine;
                            }
                        }

                        Action<string> action = (m) => MessageBox.Show(m, "SGE", MessageBoxButton.OK, MessageBoxImage.Error);
                        Dispatcher.Invoke(action, new[] { s });
                    }
                }
            }
            Action<SyncStatus> a = (s) => ToogleControls(SyncStatus.Completed);
            Dispatcher.Invoke(a, new object[] { SyncStatus.Completed });
        }

        private void UpdateProgress(double count, double current, string currentOperation)
        {
            //Thread.Sleep(100);
            double p = (current / count) * 100;
            string of = " من ";
            Action action = () =>
                {
                    SyncProgress = p;
                    CurrentOperation = currentOperation + current.ToString() + of + count.ToString();
                };
            Dispatcher.Invoke(action, null);
        }
        void ShowSyncPaymentsProgress(object sender, ProgressEventArgs e)
        {
            CurrentOperation = "جاري تحديث سندات القبض";
            SyncProgress = e.Progress;
        }
        void ShowSyncCustomersProgress(object sender, ProgressEventArgs e)
        {
            CurrentOperation = "جاري تحديث بيانات المستأجرين";
            SyncProgress = e.Progress;
        }
        void ShowSyncContractsProgress(object sender, ProgressEventArgs e)
        {
            CurrentOperation = "جاري تحديث بيانات العقود";
            SyncProgress = e.Progress;
        }
        void ShowSyncPropertiesProgress(object sender, ProgressEventArgs e)
        {
            CurrentOperation = "جاري تحديث بيانات العقارات";
            SyncProgress = e.Progress;
        }
        void ShowSyncScheduelesPaysProgress(object sender, ProgressEventArgs e)
        {
            CurrentOperation = "جاري تحديث تسديدات التعهدات";
            SyncProgress = e.Progress;
        }
        #endregion

        #region Nested type: SyncStatus

        private enum SyncStatus
        {
            Started,
            Completed,
        }

        #endregion

        #region "Events"
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ToogleControls(SyncStatus.Completed);
        }
     

        private void OnSyncCusomtersChecked(object sender, RoutedEventArgs e)
        {

        }

        private void OnSyncContractsChecked(object sender, RoutedEventArgs e)
        {
            //It is mandetory by db design to sync customers when sync contracts
            //foreign key conflict.
            ckbSyncCusomers.IsChecked = true;
            ckbSyncCusomers.IsEnabled = false;

        }

        private void OnSyncContractsUnchecked(object sender, RoutedEventArgs e)
        {
            ckbSyncCusomers.IsChecked = false;
            ckbSyncCusomers.IsEnabled = true;
        }

        private void OnSyncPaymentsChecked(object sender, RoutedEventArgs e)
        {
            //It is mandetory by db design to sync customers and contract when sync payment to avoid
            //foreign key conflict.
            ckbSyncCusomers.IsChecked = true;
            ckbSyncCusomers.IsEnabled = false;
            //
            ckbSyncContracts.IsChecked = true;
            ckbSyncContracts.IsEnabled = false;
            
        }

        private void OnSyncPaymentsUnchecked(object sender, RoutedEventArgs e)
        {
            ckbSyncCusomers.IsChecked = false;
            ckbSyncCusomers.IsEnabled = true;
            //
            ckbSyncContracts.IsChecked = false;
            ckbSyncContracts.IsEnabled = true;
        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            txtYear.Text = _settings.CurrentYear;
            txtYear.Focus();
        }
        #endregion
    }
}
