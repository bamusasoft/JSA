using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    [Table("CaseStatuses")]
    public class CaseStatus
    {
        public CaseStatus()
        {
            LegalCases = new HashSet<LegalCase>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public virtual ICollection<LegalCase> LegalCases { get; set; }


            
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            CaseStatus cs = (CaseStatus) obj;
            return Id == cs.Id;
        }
    }
}