using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.ViewsModel.Properties;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    ///     Interaction logic for OptionsView.xaml
    /// </summary>
    public partial class OptionsView : Window, INotifyPropertyChanged
    {
        private int _appointsDue;
        private ObservableCollection<string> _branches;
        private string _claimLetterPartOne;
        private string _claimLetterPartTwo;
        private string _claimTemplatePath;
        private string _contractsPhotosFolderPath;
        private string _contractsTemplatePath;
        private string _contractTemplatePath;
        private string _currentYear;
        private string _customerCardTemplatePath;
        private string _logFilePath;
        private string _mainTemplatePath;
        private string _monthlyContractTemplatePath;
        private RelayCommand _openAccessDbCommand;
        private RelayCommand _openClaimTemplateCommand;
        private RelayCommand _openContractsPhotosFolderCommand;
        private RelayCommand  _openContractsReportCommand;
        private RelayCommand _openContractTemplatePath;
        private RelayCommand _openCustomerCardTemplateCommand;
        private RelayCommand _openMainTemplateCommand;
        private RelayCommand _openMonthlyContractTemplateCommand;
        private RelayCommand _openPeriodTemplateCommand;
        private RelayCommand _openRentTemplateCommand;
        private RelayCommand _openRglTemplateCommand;
        private RelayCommand _openScanShareCommand;
        private RelayCommand _openScheduleTemplateCommand;
        private RelayCommand _openClassesTemplateCommand;
        private RelayCommand _openDocFollowTemplateCommand;
        private RelayCommand _openDocTemplateCommand;
        private string _periodScheduleTemplatePath;
        private string _propertyDbPath;
        private string _rentTamplatePath;
        private string _rglTemplatePath;
        private string _classesTemplatePath;
        private string _docFollowTemplatePath;
        private string _docTemplatePath;
        private RelayCommand _saveCommand;
        private string _scheduleTemplatePath;
        private string _selectedBranch;
        private readonly Settings _settings;
        private string _sharedScaneFolderPath;


        public OptionsView()
        {
            try
            {
                InitializeComponent();
                DataContext = this;
                _settings = Settings.Default;

                CreateRomaingAppFolder();
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }

        #region "Helper methods"

        private void CreateRomaingAppFolder()
        {
            var logFileName = "\\JsaLog.xml";
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fullPath = appDataFolder + "\\BaMusaSoft\\Jsa" + logFileName;

            if (File.Exists(fullPath))
            {
                if (_settings.LogFilePath == fullPath) return; //It's already updated.
                _settings.LogFilePath = fullPath;
                _settings.Save();
                return;
            }
            var bamusaSoftFolder = appDataFolder + "\\BaMusaSoft";
            if (!Directory.Exists(bamusaSoftFolder))
            {
                Directory.CreateDirectory(bamusaSoftFolder);
            }
            var jsaFolder = bamusaSoftFolder + "\\Jsa";
            if (!Directory.Exists(jsaFolder))
            {
                Directory.CreateDirectory(jsaFolder);
            }

            fullPath = jsaFolder + logFileName;
            _settings.LogFilePath = fullPath;
            _settings.Save();
        }

        #endregion

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ReadOption();
        }

        private string OpenExcelFile()
        {
            var filter = "Excel Templates (.xlt, .xltx)|*.xlt; *.xltx";
            var dlg = new OpenFileDialog();
            dlg.Filter = filter;
            if (dlg.ShowDialog() == true) return dlg.FileName;
            return null;
        }

        private string OpenWordFile()
        {
            var filter = "Word Tempaltes (.dot, .dotx)|*.dot; *.dotx";
            var dlg = new OpenFileDialog();
            dlg.Filter = filter;
            if (dlg.ShowDialog() == true) return dlg.FileName;
            return null;
        }

        private string OpenAccessFile()
        {
            var filter = "Access Database (.mdb)|*.mdb";
            var dlg = new OpenFileDialog();
            dlg.Filter = filter;
            if (dlg.ShowDialog() == true) return dlg.FileName;
            return null;
        }

        public string OpenFolder()
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) return fbd.SelectedPath;
            return null;
        }

        #region "Properties"

        public string CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                RaisePropertyChanged();
            }
        }

        public string PropertyDbPath
        {
            get { return _propertyDbPath; }
            set
            {
                _propertyDbPath = value;
                RaisePropertyChanged();
            }
        }

        public string PeriodScheduelsReportPath
        {
            get { return _periodScheduleTemplatePath; }
            set
            {
                _periodScheduleTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string ContractsReportPath
        {
            get { return _contractsTemplatePath; }
            set
            {
                _contractsTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> Branches
        {
            get { return _branches; }
            private set
            {
                _branches = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedBranch
        {
            get { return _selectedBranch; }
            set
            {
                _selectedBranch = value;
                RaisePropertyChanged();
            }
        }

        public string LogFilePath
        {
            get { return _logFilePath; }
            set
            {
                _logFilePath = value;
                RaisePropertyChanged();
            }
        }

        public string RglTemplatePath
        {
            get { return _rglTemplatePath; }
            set
            {
                _rglTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string ClaimLetterPartOne
        {
            get { return _claimLetterPartOne; }
            set
            {
                _claimLetterPartOne = value;
                RaisePropertyChanged();
            }
        }

        public string ClaimLetterPartTwo
        {
            get { return _claimLetterPartTwo; }
            set
            {
                _claimLetterPartTwo = value;
                RaisePropertyChanged();
            }
        }

        public string ClaimTemplatePath
        {
            get { return _claimTemplatePath; }
            set
            {
                _claimTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string SharedScaneFolderPath
        {
            get { return _sharedScaneFolderPath; }
            set
            {
                _sharedScaneFolderPath = value;
                RaisePropertyChanged();
            }
        }

        public string ContractsPhotosFolderPath
        {
            get { return _contractsPhotosFolderPath; }
            set
            {
                _contractsPhotosFolderPath = value;
                RaisePropertyChanged();
            }
        }

        public string CustomerCardTemplatePath
        {
            get { return _customerCardTemplatePath; }
            set
            {
                _customerCardTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string MonthlyContractTemplatePath
        {
            get { return _monthlyContractTemplatePath; }
            set
            {
                _monthlyContractTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string ScheduleTemplatePath
        {
            get { return _scheduleTemplatePath; }
            set
            {
                _scheduleTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string RentTemplatePath
        {
            get { return _rentTamplatePath; }
            set
            {
                _rentTamplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string MaintTemplatePath
        {
            get { return _mainTemplatePath; }
            set
            {
                _mainTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public int AppointsDue
        {
            get { return _appointsDue; }
            set
            {
                _appointsDue = value;
                RaisePropertyChanged();
            }
        }

        public string ContractTemplatePath
        {
            get { return _contractTemplatePath; }
            set
            {
                _contractTemplatePath = value;
                RaisePropertyChanged();
            }
        }

        public string ClassesTemplatePath
        {
            get { return _classesTemplatePath; }
            set
            {
                _classesTemplatePath = value;
                RaisePropertyChanged();
            }
        }
        public string DocFollowTemplatePath
        {
            get { return _docFollowTemplatePath; }
            set
            {
                _docFollowTemplatePath = value;
                RaisePropertyChanged();
            }
        }
        public string DocTemplatePath
        {
            get { return _docTemplatePath; }
            set
            {
                _docTemplatePath = value;
                RaisePropertyChanged();
            }
        }
        #endregion

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

        #region "Commands"

        public ICommand OpenPeriodTemplateCommand
        {
            get
            {
                if (_openPeriodTemplateCommand == null)
                {
                    _openPeriodTemplateCommand = new RelayCommand(OpenPeriodTemplate);
                }
                return _openPeriodTemplateCommand;
            }
        }

        private void OpenPeriodTemplate()
        {
            PeriodScheduelsReportPath = OpenExcelFile();
        }

        public ICommand OpenContractsReportCommand
        {
            get
            {
                if (_openContractsReportCommand == null)
                {
                    _openContractsReportCommand = new RelayCommand(OpenContractsReport);
                }
                return _openContractsReportCommand;
            }
        }

        private void OpenContractsReport()
        {
            ContractsReportPath = OpenExcelFile();
        }

        public ICommand OpenAccessDbCommand
        {
            get
            {
                if (_openAccessDbCommand == null)
                {
                    _openAccessDbCommand = new RelayCommand(OpenAccessDb);
                }
                return _openAccessDbCommand;
            }
        }

        private void OpenAccessDb()
        {
            PropertyDbPath = OpenAccessFile();
        }

        public ICommand OpenRglTemplateCommand
        {
            get
            {
                if (_openRglTemplateCommand == null)
                {
                    _openRglTemplateCommand = new RelayCommand(OpenRglTemplate);
                }
                return _openRglTemplateCommand;
            }
        }

        private void OpenRglTemplate()
        {
            RglTemplatePath = OpenExcelFile();
        }

        public ICommand OpenClaimTemplateCommand
        {
            get
            {
                if (_openClaimTemplateCommand == null)
                {
                    _openClaimTemplateCommand = new RelayCommand(OpenClaimTemplate);
                }
                return _openClaimTemplateCommand;
            }
        }

        private void OpenClaimTemplate()
        {
            ClaimTemplatePath = OpenWordFile();
        }

        public ICommand OpenScanShareCommand
        {
            get
            {
                if (_openScanShareCommand == null)
                {
                    _openScanShareCommand = new RelayCommand(OpenScanFolder);
                }
                return _openScanShareCommand;
            }
        }

        private void OpenScanFolder()
        {
            SharedScaneFolderPath = OpenFolder();
        }

        public ICommand OpenContractsPhotosFolderCommand
        {
            get
            {
                if (_openContractsPhotosFolderCommand == null)
                {
                    _openContractsPhotosFolderCommand = new RelayCommand(OpenContractsPhotosFolder);
                }
                return _openContractsPhotosFolderCommand;
            }
        }

        private void OpenContractsPhotosFolder()
        {
            ContractsPhotosFolderPath = OpenFolder();
        }

        public ICommand OpenCustomerCardTemplateCommand
        {
            get
            {
                if (_openCustomerCardTemplateCommand == null)
                {
                    _openCustomerCardTemplateCommand = new RelayCommand(OpenCustomerCardTemplate);
                }
                return _openCustomerCardTemplateCommand;
            }
        }

        private void OpenCustomerCardTemplate()
        {
            CustomerCardTemplatePath = OpenWordFile();
        }

        public ICommand OpenMonthlyContractTemplateCommand
        {
            get
            {
                if (_openMonthlyContractTemplateCommand == null)
                {
                    _openMonthlyContractTemplateCommand = new RelayCommand(OpenMonthlyContractTemplate);
                }
                return _openMonthlyContractTemplateCommand;
            }
        }

        private void OpenMonthlyContractTemplate()
        {
            MonthlyContractTemplatePath = OpenWordFile();
        }

        public ICommand OpenScheduleTemplateCommand
        {
            get
            {
                return _openScheduleTemplateCommand ??
                       (_openScheduleTemplateCommand = new RelayCommand(OpenScheduleTemplate));
            }
        }

        private void OpenScheduleTemplate()
        {
            ScheduleTemplatePath = OpenWordFile();
        }

        public ICommand OpenRentTemplateCommand
        {
            get
            {
                return _openRentTemplateCommand ??
                       (_openRentTemplateCommand = new RelayCommand(OpenRentTemplate));
            }
        }

        private void OpenRentTemplate()
        {
            RentTemplatePath = OpenExcelFile();
        }

        public ICommand OpenMaintTemplateCommand
        {
            get
            {
                return _openMainTemplateCommand ??
                       (_openMainTemplateCommand = new RelayCommand(OpenMaintTemplate));
            }
        }

        public ICommand OpenContractTemplateCommand
        {
            get
            {
                return _openContractTemplatePath ?? (_openContractTemplatePath = new RelayCommand(OpenContractTempalte));
            }
        }

        private void OpenContractTempalte()
        {
            ContractTemplatePath = OpenWordFile();
        }

        public ICommand OpenClassesTemplateCommand
        {
            get
            {
                return _openClassesTemplateCommand ?? (_openClassesTemplateCommand = new RelayCommand(OpenClasses));
            }
        }

        private void OpenClasses()
        {
            ClassesTemplatePath = OpenExcelFile();
        }
        public ICommand OpenDocFollowTemplateCommand
        {
            get
            {
                return _openDocFollowTemplateCommand ?? (_openDocFollowTemplateCommand = new RelayCommand(OpenDocFollowTemplate));
            }
        }
        public ICommand OpenDocTemplateCommand
        {
            get
            {
                return _openDocTemplateCommand ?? (_openDocTemplateCommand = new RelayCommand(OpenDocTemplate));
            }
        }
        private void OpenDocTemplate()
        {
            DocTemplatePath = OpenExcelFile();
        }
        private void OpenDocFollowTemplate()
        {
            DocFollowTemplatePath = OpenExcelFile();
        }

        private void OpenMaintTemplate()
        {
            MaintTemplatePath = OpenExcelFile();
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

        private void Save()
        {
            Helper.ExplicitUpdateBinding();
            WriteOptions();
        }

        #endregion

        #region "Helper Methods"

        private void WriteOptions()
        {
            _settings.CurrentYear = CurrentYear;
            _settings.IrsDbPath = PropertyDbPath;
            _settings.PeriodSchedulesExcelTemplate = PeriodScheduelsReportPath;
            _settings.ContractReportExcelTemplate = ContractsReportPath;
            _settings.LogFilePath = LogFilePath;
            _settings.RglTemplatePath = RglTemplatePath;
            _settings.ClaimTextPartOne = ClaimLetterPartOne;
            _settings.ClaimTextPartTwo = ClaimLetterPartTwo;
            _settings.ClaimTemplatePath = ClaimTemplatePath;
            _settings.ScanShareFolder = SharedScaneFolderPath;
            _settings.ContractsPhotosPath = ContractsPhotosFolderPath;
            _settings.CustomerCardTemplatePath = CustomerCardTemplatePath;
            _settings.MonthlyContractTemplate = MonthlyContractTemplatePath;
            _settings.ScheduleTemplatePath = ScheduleTemplatePath;
            _settings.RentTempatePath = RentTemplatePath;
            _settings.MaintTemplatePath = MaintTemplatePath;
            _settings.AppointDueDays = AppointsDue;
            _settings.GeneralContractTemplate = ContractTemplatePath;
            _settings.ClassesTemplate = ClassesTemplatePath;
            _settings.DocFollowTemplate = DocFollowTemplatePath;
            _settings.DocTemplate = DocTemplatePath;
            switch (SelectedBranch)
            {
                case "جدة":
                    _settings.Branch = 2;
                    break;
                case "مكة":
                    _settings.Branch = 1;
                    break;
                default:
                    break;
            }
            _settings.Save();
        }

        private void ReadOption()
        {
            CurrentYear = _settings.CurrentYear;
            PropertyDbPath = _settings.IrsDbPath;
            LogFilePath = _settings.LogFilePath;
            PeriodScheduelsReportPath = _settings.PeriodSchedulesExcelTemplate;
            ContractsReportPath = _settings.ContractReportExcelTemplate;
            RglTemplatePath = _settings.RglTemplatePath;
            ClaimLetterPartOne = _settings.ClaimTextPartOne;
            ClaimLetterPartTwo = _settings.ClaimTextPartTwo;
            ClaimTemplatePath = _settings.ClaimTemplatePath;
            SharedScaneFolderPath = _settings.ScanShareFolder;
            ContractsPhotosFolderPath = _settings.ContractsPhotosPath;
            CustomerCardTemplatePath = _settings.CustomerCardTemplatePath;
            MonthlyContractTemplatePath = _settings.MonthlyContractTemplate;
            ScheduleTemplatePath = _settings.ScheduleTemplatePath;
            RentTemplatePath = _settings.RentTempatePath;
            MaintTemplatePath = _settings.MaintTemplatePath;
            AppointsDue = _settings.AppointDueDays;
            ContractTemplatePath = _settings.GeneralContractTemplate;
            ClassesTemplatePath = _settings.ClassesTemplate;
            DocFollowTemplatePath = _settings.DocFollowTemplate;
            DocTemplatePath = _settings.DocTemplate;
            Branches = new ObservableCollection<string>
                (
                new List<string>
                {
                    "مكة",
                    "جدة"
                }
                );
            switch (_settings.Branch)
            {
                case 1:
                    SelectedBranch = Branches[0];
                    break;
                case 2:
                    SelectedBranch = Branches[1];
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}