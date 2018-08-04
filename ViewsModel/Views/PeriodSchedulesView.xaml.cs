using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Reports;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for PeriodSchedulesView.xaml
    /// </summary>
    public partial class PeriodSchedulesView : Window, INotifyPropertyChanged
    {
        RelayCommand _searchCommand;
        RelayCommand _refreshCommand;
        RelayCommand _editCommand;
        RelayCommand _printCommand;
        private RelayCommand _printScheduleCommand;
        ObservableCollection<PeriodSchedule> _searchResult;
        PeriodSchedulesCriteria _criteria;
        Properties.Settings _settings;
        PeriodSchedule _selectedSchedule;
        int _amountDueTotal;
        int _paidTotal;
        int _balanceTotal;
        public PeriodSchedulesView()
        {
            InitializeComponent();
            DataContext = this;
            _settings = Properties.Settings.Default;
            SearchResult = new ObservableCollection<PeriodSchedule>();
        }
        #region "Search Criteria"
        public PeriodSchedulesCriteria Criteria
        {
            get
            {
                if (_criteria == null)
                {
                    _criteria = new PeriodSchedulesCriteria();
                }
                return _criteria;
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

        #region "Properties"

        public ObservableCollection<PeriodSchedule> SearchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value;
                RaisePropertyChanged();
                UpdateStatistics();

            }
        }
        public int AmountDueTotal
        {
            get {return _amountDueTotal; }
            set
            {
                _amountDueTotal = value;
                RaisePropertyChanged();
            }
        }
        public int PaidTotal
        {
            get { return _paidTotal; }
            set
            {
                _paidTotal = value;
                RaisePropertyChanged();
            }
        }
        public int BalanceTotal
        {
            get { return _balanceTotal; }
            set
            {
                _balanceTotal = value;
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
                    _searchCommand = new RelayCommand(Search);
                }
                return _searchCommand;
            }

        }
        void Search()
        {
            var searchCriteria = Criteria.BuildCriteria();
            if (searchCriteria != null)
            {
                try
                {
                    if (CachedData == null) RefreshData();
                    if (CachedData != null)
                    {
                        var result = CachedData.Where(searchCriteria.Compile()).OrderBy(x => x.PropertyNo).ThenBy(x => x.DateDue);
                        SearchResult = new ObservableCollection<PeriodSchedule>(result);
                    }
                }
                catch (Exception ex)
                {
                    string msg = Helper.ProcessExceptionMessages(ex);
                    Helper.ShowMessage(msg);
                }
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(RefreshData);
                }
                return _refreshCommand;
            }
        }
        void RefreshData()
        {
            try
            {
                CachedData = PeriodSchedule.LoadData();
                //Search();

            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Helper.ShowMessage(msg);
                

            }
        }
        public ICommand EditCommand
        {
            get 
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(EditSchedule, CanEdit);
                }
                return _editCommand;
            }
        }
        void EditSchedule()
        {
            try
            {
                string id = SelectedSchedule.ScheduleId;
                
                ScheduleView view = new ScheduleView(id);
                view.ShowDialog();
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Helper.ShowMessage(msg);

            }
        }
        bool CanEdit()
        {
            return SelectedSchedule != null;
        }

        public ICommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand(Print);
                }
                return _printCommand;
            }
        }
        void Print()
        {
            try
            {
                string template = _settings.PeriodSchedulesExcelTemplate; ;
                var reportTable = PeriodSchedulesReport.CreateReport(SearchResult, 
                    new Tuple<int,int,int>(AmountDueTotal, PaidTotal, BalanceTotal));
                ExcelMail mail = new ExcelMail();
                mail.Send(reportTable, template, false);
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Helper.ShowMessage(msg);
            }
        }

        public ICommand PrintScheduleCommand
        {
            get { return _printScheduleCommand ?? (_printScheduleCommand = new RelayCommand(PrintSchedule)); }
        }

        void PrintSchedule()
        {
            if(SelectedSchedule == null)return;
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var schedule = unit.Schedules.GetById(SelectedSchedule.ScheduleId);
                    Helper.PrintSchedule(schedule);
                }
            }
            catch (Exception ex)
            {
                
                Helper.LogShowError(ex);
            }
        }
        #endregion

        private IList<PeriodSchedule> CachedData
        {
            get;
            set;
        }
        public PeriodSchedule SelectedSchedule
        {
            get { return _selectedSchedule; }
            set
            {
                _selectedSchedule = value;
                RaisePropertyChanged();
            }
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            WriteSettings();
        }

        private void WriteSettings()
        {

            _settings.PeriodSchedulesGridPreference.Clear();
            foreach (DataGridColumn col in dgResult.Columns)
            {
                //Docs:
                /*This will save datagrid columns's header, DispalyIndex, and Width.
                 * In same order they appear.
                 */
                _settings.PeriodSchedulesGridPreference.Add(col.Header.ToString());
                _settings.PeriodSchedulesGridPreference.Add(col.DisplayIndex.ToString());
                _settings.PeriodSchedulesGridPreference.Add(col.ActualWidth.ToString());
            }

            _settings.Save();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ReadSettings();
            txtStartDate.Focus();

        }

        private void ReadSettings()
        {
            if (_settings.PeriodSchedulesGridPreference.Count == 0) return;
            var columnsPreference = new List<GridColumnPreference>();
            int counter = 0;
            string header = null;
            int dispalyIndex = 0;
            double width = 0.0;
            foreach (var pref in _settings.PeriodSchedulesGridPreference)
            {
                /*Reading the datagrid columns preference by making this assumption:
                 * First row contains Header.
                 * Second row contains DispalyIndex.
                 * Third row contains Width.
                */
                if (counter == 0)
                {
                    header = pref;
                    counter++;
                    continue;
                }
                if (counter == 1)
                {
                    dispalyIndex = int.Parse(pref);
                    counter++;
                    continue;
                }
                if (counter == 2)
                {
                    width = double.Parse(pref);
                    columnsPreference.Add(new GridColumnPreference(header, dispalyIndex, width));
                    counter = 0;
                }
                
            }
            foreach (var pref in columnsPreference)
            {
                foreach (var col in dgResult.Columns)
                {
                    if (col.Header.ToString() == pref.Header)
                    {
                        col.Width = pref.Width;
                        col.DisplayIndex = pref.DispalyOrder;
                        break;
                    }
                }
            }



        }


        #region "Helpers methods"
        private void UpdateStatistics()
        {
            AmountDueTotal = SearchResult.Sum(x => x.AmountDue);
            PaidTotal = SearchResult.Sum(x => x.AmountPaid);
            BalanceTotal = SearchResult.Sum(x => x.Balance);
        }
        #endregion

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


    }
}
