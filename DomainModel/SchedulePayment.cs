using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class SchedulePayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContractNo { get; set; }

        public int ScheduledPayment { get; set; }

        public int UnscheduledPayment { get; set; }

        public int TotalPayment { get; set; }
    }
}
