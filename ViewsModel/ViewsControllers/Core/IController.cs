using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public interface IController : INotifyPropertyChanged
    {
        ControllerStates State { get; set; }
        void  ControlState(ControllerStates state);
        event ControllerChangedEventHandler ControllerChanged;

        void RaisePropertyChanged([CallerMemberName] string propertyName = null);
        void RaiseContorllerChanged(ControllerAction action);
    }
}