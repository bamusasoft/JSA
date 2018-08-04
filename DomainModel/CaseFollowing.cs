using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    public partial class CaseFollowing
    {
        public int Id { get; set; }

        public int CaseNo { get; set; }

        [Required]
        [StringLength(8)]
        public string FollowingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime GregFollowingDate { get; set; }

        [Required]
        [StringLength(500)]
        public string FollowingDestination { get; set; }

        [Required]
        [StringLength(2000)]
        public string FollowingDescription { get; set; }

        [Required]
        [StringLength(8)]
        public string NextFollowingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime GregNextFollowingDate { get; set; }

        [Required]
        [StringLength(500)]
        public string NextFollowingDestination { get; set; }

        public virtual LegalCase LegalCase { get; set; }
    }
}