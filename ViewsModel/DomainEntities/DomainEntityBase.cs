using System.ComponentModel;
using System.Runtime.CompilerServices;
using Jsa.ViewsModel.Annotations;

namespace Jsa.ViewsModel.DomainEntities
{
    public abstract class DomainEntityBase<T>:INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
