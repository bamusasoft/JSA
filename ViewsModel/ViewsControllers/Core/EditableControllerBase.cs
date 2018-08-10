using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public abstract class EditableControllerBase : IEditableController
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

        #region Implementation of IEditableController

        private RelayCommand _clearComamand;
        private RelayCommand _saveCommand;
        private RelayCommand _printCommand;
        private RelayCommand _searchCommand;
        private RelayCommand _deleteCommand;

        public ICommand ClearCommand
        {
            get { return _clearComamand ?? (_clearComamand = new RelayCommand(ClearView, CanClear)); }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(Save, CanSave)); }
        }

        public ICommand PrintCommand
        {
            get { return _printCommand ?? (_printCommand = new RelayCommand(Print, CanPrint)); }
        }


        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(Search, CanSearch)); }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(Delete, CanDelete)); }
        }

        protected abstract void ClearView();
        protected abstract bool CanClear();
        protected abstract void Save();
        protected abstract bool CanSave();
        protected abstract void Print();
        protected abstract bool CanPrint();
        protected abstract void Search();
        protected abstract bool CanSearch();
        protected abstract void Delete();
        protected abstract bool CanDelete();
        #endregion

        #region DataError
        protected Dictionary<string, List<string>> Errors { get; set; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !Errors.ContainsKey(propertyName)) return null;
            return Errors[propertyName];
        }


        public bool HasErrors
        {
            get { return Errors.Count > 0; }
        }
        protected void AddError(string propertyName, string error)
        {
            if (!Errors.ContainsKey(propertyName))
                Errors[propertyName] = new List<string>();
            if (!Errors[propertyName].Contains(error))
            {
                Errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        protected void RemoveError(string propertyName, string error)
        {
            if (Errors.ContainsKey(propertyName) && Errors[propertyName].Contains(error))
            {
                Errors[propertyName].Remove(error);
                if (Errors[propertyName].Count == 0) Errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
