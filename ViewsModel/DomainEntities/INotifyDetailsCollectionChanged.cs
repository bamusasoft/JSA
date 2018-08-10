using System;

namespace Jsa.ViewsModel.DomainEntities
{
    public interface INotifyDetailsCollectionChanged
    {
        event EventHandler DetailsChanged;
    }
}
