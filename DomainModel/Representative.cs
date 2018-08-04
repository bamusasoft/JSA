using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    public partial class Representative
    {
        [Key]
        public int AutoKey { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Id { get; set; }

        [Required]
        [StringLength(8)]
        public string IdDate { get; set; }

        [Required]
        [StringLength(50)]
        public string IssueAt { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
