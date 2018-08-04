using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.ViewsModel.Helpers;
using Microsoft.Win32;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for BulkIresContractsView.xaml
    /// </summary>
    public partial class BulkIresContractsView : Window, INotifyPropertyChanged
    {
        public BulkIresContractsView()
        {
            InitializeComponent();
            DataContext = this;
            _contracts.WritingProgress += new EventHandler<ProgressEventArgs<IresContract>>(WritingPrgress);
            progBar.Visibility = System.Windows.Visibility.Hidden;
            IresContracts = new ObservableCollection<IresContract>();
            BindingOperations.EnableCollectionSynchronization(IresContracts, PaymentDetailsLock);
            progBar.Maximum = 100;

        }
        #region "Fields"
        private static readonly object PaymentDetailsLock = new object();
        IresContractsSet _contracts = new IresContractsSet();
        string _excelFilePath;
        string _iresPropertyDbPath;
        RelayCommand _openExcelFileCommand;
        RelayCommand _saveCommand;
        ObservableCollection<IresContract> _iresContracts;
        #endregion
        #region "Properties"
        public string PropertyDbFilePath
        {
            get
            {
                return _iresPropertyDbPath;
            }
            set
            {
                _iresPropertyDbPath = value;
                RaisePropertyChanged();
            }

        }
        public string ExcelFilePath
        {
            get { return _excelFilePath; }
            set
            {
                _excelFilePath = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<IresContract> IresContracts
        {
            get
            {
                return _iresContracts;
            }
            set
            {
                _iresContracts = value;
                RaisePropertyChanged();
            }

        }
        #endregion
        #region "Commands"
        public ICommand OpenExcelFileCommand
        {
            get
            {
                if (_openExcelFileCommand == null)
                {
                    _openExcelFileCommand = new RelayCommand(OpenExcelFile);
                }
                return _openExcelFileCommand;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(Save);
                }
                return _saveCommand;
            }
        }

        #endregion
        #region "Commands Methods"
        void OpenExcelFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string filter = "Excel Files (.xls, .xlxs)|*.xls; *.xlxs";
            ofd.Filter = filter;
            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.FileName;
                ExcelFilePath = fileName;
                ReadExcelContracts(fileName);

            }
        }
        async void Save()
        {
            string msg = Properties.Resources.BulkIresContractsView_SaveToIresWarning;
            if (Helper.UserConfirmed(msg))
            {
                await SaveToIres();
            }
        }
        #endregion
        void WritingPrgress(object sender, ProgressEventArgs<IresContract> e)
        {
            Action action = () =>
            {
                if (progBar.Visibility != System.Windows.Visibility.Visible)
                {
                    progBar.Visibility = System.Windows.Visibility.Visible;
                }
                //double progressSoFar = (e.Progress / _contracts.Count) * 100;
                IresContracts.Remove(e.Entity);
                progBar.Value = e.Progress;
            };
            Dispatcher.Invoke(action, null);
        }

        private void ReadExcelContracts(string fileName)
        {
            try
            {
                progBar.Value = 0.0;
                var c = _contracts.ReadContracts(fileName);
                IresContracts = new ObservableCollection<IresContract>(c);
                textNoRecords.Text = _contracts.Count.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        async Task SaveToIres()
        {
            btnOpenExcelFile.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnNew.IsEnabled = false;
            try
            {
                progBar.Value = 0;
                bool b = await _contracts.WriteContractsAsync(PropertyDbFilePath);
                if (b)
                {
                    string msg = Properties.Resources.BulkIresContractsView_BulkSaveFinished;
                    Helper.ShowMessage(msg);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            btnOpenExcelFile.IsEnabled = true;
            btnSave.IsEnabled = true;
            progBar.Value = 0;
            progBar.Visibility = System.Windows.Visibility.Hidden;

        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            //SettingsWindow settingsWindow = new SettingsWindow();

            //settingsWindow.ShowDialog();
        }
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
    }
}
