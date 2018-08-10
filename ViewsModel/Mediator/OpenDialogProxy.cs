using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Mediator
{
    public class OpenDialogProxy
    {
        public event EventHandler<object> OpenDialog;
        public void RaiseOpenDialog<T>(T arg)
        {
            OpenDialog?.Invoke(this, arg);
        }
    }
}
