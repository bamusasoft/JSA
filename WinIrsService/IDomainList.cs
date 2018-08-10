using System.Collections.Generic;

namespace Jsa.WinIrsService
{
    public interface IDomainList<T>
    {
        IList<T> GetData();
    }
}