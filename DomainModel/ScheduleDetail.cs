using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    [Serializable]
    public class ScheduleDetail 
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ScheduleId { get; set; }

        public int ContractNo { get; set; }

        [Required]
        [StringLength(8)]
        public string DateDue { get; set; }

        public int AmountDue { get; set; }

        public int AmountPaid { get; set; }

        public int Balance { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        public bool DiscountAmount { get; set; }

        public virtual Contract Contract { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}
