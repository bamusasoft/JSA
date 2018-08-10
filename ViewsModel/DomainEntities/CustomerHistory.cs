using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Jsa.ViewsModel.DomainEntities
{
    public class CustomerHistory
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerClass { get; set; }
        public string TenantSince { get; set; }
        public List<CustomerPropertyDto> CustomerProperties { get; set; }
        public List<CustomerPaymentDto> CustomerPayments { get; set; }

        public ListCollectionView BindingCustomerPayments
        {
            get
            {
                if (CustomerPayments == null)
                {
                    return null;
                }
                CustomerPayments.OrderBy(x => x.PayDate);
                ListCollectionView coll = new ListCollectionView(CustomerPayments);
                coll.GroupDescriptions.Add(new PropertyGroupDescription("PropertyDescription"));
                return coll;
            }
        }
    }
}
