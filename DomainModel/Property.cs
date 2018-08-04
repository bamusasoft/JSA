using System.ComponentModel.DataAnnotations;

namespace Jsa.DomainModel
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class Property
    {
        public Property()
        {
            ClaimDetails = new HashSet<ClaimDetail>();
            Contracts = new HashSet<Contract>();
        }

        [Key]
        [StringLength(20)]
        public string PropertyNo { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(40)]
        public string Location { get; set; }

        [StringLength(50)]
        public string District { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        public virtual ICollection<ClaimDetail> ClaimDetails { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
