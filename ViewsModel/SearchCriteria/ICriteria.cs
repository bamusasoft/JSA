using System;
using System.Linq.Expressions;

namespace Jsa.ViewsModel.SearchCriteria
{
    public interface ICriteria<T>
    {
        Expression<Func<T, bool>> BuildCriteria();

    }
}
