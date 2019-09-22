using System.ComponentModel;
using System.Windows.Input;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public interface IEditableController : IController, INotifyDataErrorInfo
    {
        ICommand ClearCommand { get; }
        ICommand SaveCommand { get; }
        ICommand PrintCommand { get; }
        ICommand SearchCommand { get; }
        ICommand DeleteCommand { get; }

    }
}