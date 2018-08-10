using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.DomainEntities
{
    public sealed class CustomerSchedule:DomainEntityBase<CustomerSchedule>
    {
        public string ScheduleId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PropertyNo { get; set; }
        public string PropertyDescription { get; set; }

        public CustomerSchedule(string scheduleId, int customerId, string customerName, string propertyNo,
            string propertyDescription)
        {
            ScheduleId = scheduleId;
            CustomerId = customerId;
            CustomerName = customerName;
            PropertyNo = propertyNo;
            PropertyDescription = propertyDescription;
        }
    }
}
