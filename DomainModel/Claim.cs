using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    [Serializable]
    public partial class Claim
    {
        public Claim()
        {
            ClaimDetails = new HashSet<ClaimDetail>();
        }

        [StringLength(7)]
        public string ClaimId { get; set; }

        public int CustomerId { get; set; }

        public short Sequence { get; set; }

        [Required]
        [StringLength(4)]
        public string ClaimYear { get; set; }

        [Column(TypeName = "money")]
        public decimal GrandTotal { get; set; }

        [Required]
        [StringLength(500)]
        public string LetterPartOne { get; set; }

        [Required]
        [StringLength(500)]
        public string LetterPartTwo { get; set; }

        [Required]
        [StringLength(8)]
        public string CreationDate { get; set; }

        public virtual ICollection<ClaimDetail> ClaimDetails { get; set; }

        public virtual Customer Customer { get; set; }




        public static string MaxNo
        {
            get
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    return unitOfWork.Claims.GetAll().Max(x => x.ClaimId);
                }
            
            }
        
        }
    }
}
