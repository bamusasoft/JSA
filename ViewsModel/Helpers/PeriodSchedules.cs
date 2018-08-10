using System.Collections.Generic;
using System.Linq;
using Jsa.DomainModel;

namespace Jsa.ViewsModel.Helpers
{
    public class PeriodSchedule
    {
        public string ScheduleId { get; set; }
        public string ScheduleDate { get; set; }
        public int CustomerId { get; set; }
        
        public string CustomerName { get; set; }

        public string PropertyNo { get; set; }
        public string PropertyDescription { get; set; }

        public int AmountDue { get; set; }
        public string DateDue { get; set; }
        public int AmountPaid { get; set; }
        public int Balance { get; set; }
        public string SignerId { get; set; }
        public string SignerName { get; set; }
        public string SignerMobile { get; set; }
        public bool HadPaid { get; set; }

        public static IList<PeriodSchedule> LoadData()
        {

            using (IUnitOfWork unit = new UnitOfWork())
            {
                var schedules = unit.Schedules.GetAll();
                var details = unit.ScheduleDetails.GetAll();
                return (from schedule in schedules
                        join detail in details
                        on schedule.ScheduleId equals detail.ScheduleId
                        //orderby schedule.Contract.PropertyNo, detail.DateDue
                        select new PeriodSchedule
                        {
                            ScheduleId = schedule.ScheduleId,
                            ScheduleDate = schedule.ScheduleDate,
                            //RegistredAt = schedule.RegistredAt,
                            CustomerId = schedule.Customer.CustomerId,
                            CustomerName = schedule.Customer.Name,
                            // ContractNo = schedule.ContractNo,
                            PropertyNo = detail.Contract.PropertyNo,
                            PropertyDescription = detail.Contract.Property.Description,
                            AmountDue = detail.AmountDue,
                            DateDue = detail.DateDue,
                            AmountPaid = detail.AmountPaid,
                            Balance = detail.Balance,
                            SignerId = schedule.SignerId,
                            SignerName = schedule.Signer.Name,
                            SignerMobile = schedule.Signer.Mobile,
                            //Phone = schedule.Signer.FirstPhone,
                            HadPaid = (detail.Balance == 0)
                        }).ToList();

            }
        }
    }
}
