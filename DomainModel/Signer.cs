
using System.ComponentModel.DataAnnotations;

namespace Jsa.DomainModel
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class Signer
    {
        public Signer()
        {
            Schedules = new HashSet<Schedule>();
        }

        [StringLength(10)]
        public string SignerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(8)]
        public string IdDate { get; set; }

        [StringLength(20)]
        public string IdIssue { get; set; }

        [StringLength(10)]
        public string Mobile { get; set; }

        [StringLength(7)]
        public string Phone { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }



        public static Signer Create()
        {
            return new Signer();
        }
    }
}
