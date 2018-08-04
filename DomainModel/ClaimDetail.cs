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
    public partial class ClaimDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClaimDetailId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ClaimId { get; set; }

        [Required]
        [StringLength(20)]
        public string PropertyNo { get; set; }

        [Column(TypeName = "money")]
        public decimal Rent { get; set; }

        [Column(TypeName = "money")]
        public decimal Maintenance { get; set; }

        [Column(TypeName = "money")]
        public decimal Deposit { get; set; }

        [Column(TypeName = "money")]
        public decimal Others { get; set; }

        [Column(TypeName = "money")]
        public decimal Total { get; set; }

        [Column(TypeName = "money")]
        public decimal Paid { get; set; }

        [Column(TypeName = "money")]
        public decimal Balance { get; set; }

        [Column(TypeName = "money")]
        public decimal OutstandingRentBalance { get; set; }

        [Column(TypeName = "money")]
        public decimal OutstandingMaintBalance { get; set; }

        [Column(TypeName = "money")]
        public decimal NetBalance { get; set; }

        public virtual Claim Claim { get; set; }

        public virtual Property Property { get; set; }
    }
}
