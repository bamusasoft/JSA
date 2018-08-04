using System.ComponentModel.DataAnnotations;

namespace Jsa.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    [Serializable]
    public partial class Schedule
    {
        public Schedule()
        {
            ScheduleDetails = new HashSet<ScheduleDetail>();
        }

        [StringLength(7)]
        public string ScheduleId { get; set; }

        [Required]
        [StringLength(8)]
        public string ScheduleDate { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(10)]
        public string SignerId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; }

        public virtual Signer Signer { get; set; }





        public static Schedule Create()
        {
            return new Schedule();
        }
        public static string MaxNo
        {
            get
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var max = unit.Schedules.GetAll().Max(sch => sch.ScheduleId);
                    return max;
                }
            }

        }
    }
}
