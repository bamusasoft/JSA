using Jsa.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Helpers
{
    public class CustomerComparer : IEqualityComparer<Customer>
    {
        public bool Equals(Customer x, Customer y)
        {
            return x.CustomerId == y.CustomerId;
        }

        public int GetHashCode(Customer obj)
        {
            return obj.CustomerId.GetHashCode();
        }
    }
}
