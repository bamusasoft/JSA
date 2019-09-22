using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.DomainEntities
{
    public class CustomerPaymentDto
    {
        public string PropertyDescription { get; set; }
        public string PayDate { get; set; }
        public double Amount { get; set; }
    }
}
