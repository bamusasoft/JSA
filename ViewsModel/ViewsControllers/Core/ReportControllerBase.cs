using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public abstract class ReportControllerBase : IReportController
    {

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of IController

        public ControllerStates State { get; set; }
        public abstract void ControlState(ControllerStates state);
        public event ControllerChangedEventHandler ControllerChanged;


        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RaiseContorllerChanged(ControllerAction action)
        {
            if (ControllerChanged != null)
            {
                ControllerChanged(this, new ControllerChangedEventArgs(action));
            }
        }

        #endregion

        #region IReportController

        private RelayCommand _searchCommand;
        private RelayCommand _printCommand;
        private RelayCommand _editCommand;
        private RelayCommand _refreshCommand;
        private Object _selectedItem;
        private IList<object> _result;
        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(Search, CanSearch)); }
        }

        public ICommand PrintCommand
        {
            get { return _printCommand ?? ((_printCommand = new RelayCommand(Print, CanPrint))); }
        }

        public ICommand EditCommand
        {
            get { return _editCommand ?? ((_editCommand = new RelayCommand(Edit, CanEdit))); }
        }

        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? ((_refreshCommand = new RelayCommand(Refresh, CanRefresh))); }
        }

        public IList<object> SearchResult
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged();
            }

        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }

        protected abstract void Search();
        protected abstract bool CanSearch();
        protected abstract void Print();
        protected abstract bool CanPrint();
        protected abstract void Edit();
        protected abstract bool CanEdit();
        protected abstract void Refresh();
        protected abstract bool CanRefresh();


        #endregion

    }
}
