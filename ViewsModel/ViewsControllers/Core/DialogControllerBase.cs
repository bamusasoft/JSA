using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public abstract class DialogControllerBase:IDialogController
    {
        private ICommand _okCommand;
        private ICommand _cancelCommand;
        private object _selectedItem;
        public event PropertyChangedEventHandler PropertyChanged;

        public ControllerStates State { get; set; }

        public abstract void  ControlState(ControllerStates state);
        public event EventHandler<DialogCloseState> CloseDialog;

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

        protected void RaiseCloseDialog(DialogCloseState closeState)
        {
            if (CloseDialog != null)
            {
                CloseDialog(this, closeState);
            }
        }

        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new RelayCommand(Ok, OkEnabled)); }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
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

       

        public abstract void Ok();
        public abstract bool OkEnabled();
        public abstract void Cancel();
    }
}
