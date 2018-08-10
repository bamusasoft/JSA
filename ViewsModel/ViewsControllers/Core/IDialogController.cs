using System;
using System.Windows.Input;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public interface IDialogController : IController
    {
        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }

        object SelectedItem { get; set; }
        event EventHandler<DialogCloseState> CloseDialog;
    }
}