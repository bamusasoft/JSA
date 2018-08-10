using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    public partial class CaseAppointment
    {
        public int Id { get; set; }

        public int CaseNo { get; set; }

        [Required]
        [StringLength(8)]
        public string AppointmentDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime AppointmentGregDate { get; set; }

        public virtual LegalCase LegalCase { get; set; }
    }   
}