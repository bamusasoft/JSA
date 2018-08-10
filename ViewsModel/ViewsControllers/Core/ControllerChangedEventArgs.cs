using System;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public class ControllerChangedEventArgs:EventArgs
    {

        public ControllerChangedEventArgs(ControllerAction action)
        {
            Action = action;
        }
        public ControllerAction Action { get; private set; }
    }
}