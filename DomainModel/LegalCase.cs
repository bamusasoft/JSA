using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    [Table("LegalCases")]
    public partial class LegalCase
    {
        public LegalCase()
        {
            CaseAppointments = new HashSet<CaseAppointment>();
            CaseFollowings = new HashSet<CaseFollowing>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CaseNo { get; set; }

        [Required]
        [StringLength(8)]
        public string RegisteredAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime GregDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Defendant { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        public int StatusId { get; set; }

        public virtual ICollection<CaseAppointment> CaseAppointments { get; set; }

        public virtual ICollection<CaseFollowing> CaseFollowings { get; set; }

        public virtual CaseStatus CaseStatus { get; set; }

        public string NextAppointmentDate
        {
            get
            {
                if (CaseAppointments == null) return null;
                return CaseAppointments.OrderBy(x => x.AppointmentDate).First().AppointmentDate;
            }
        }
    }
}
