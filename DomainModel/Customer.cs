using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class Customer
    {
        public Customer()
        {
            Claims = new HashSet<Claim>();
            Contracts = new HashSet<Contract>();
            Representatives = new HashSet<Representative>();
            Schedules = new HashSet<Schedule>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string IdNumber { get; set; }

        [StringLength(8)]
        public string IdDate { get; set; }

        [StringLength(50)]
        public string IdIssue { get; set; }

        [StringLength(200)]
        public string AddressLine1 { get; set; }

        [StringLength(200)]
        public string AddressLine2 { get; set; }

        [StringLength(10)]
        public string MainMobile { get; set; }

        [StringLength(10)]
        public string SecondMobile { get; set; }

        [StringLength(9)]
        public string HomePhone { get; set; }

        [StringLength(9)]
        public string WorkPhone { get; set; }

        [StringLength(9)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Nationality { get; set; }

        [StringLength(20)]
        public string IdType { get; set; }

        public virtual ICollection<Claim> Claims { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual ICollection<Representative> Representatives { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
