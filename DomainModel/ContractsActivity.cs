using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    [Serializable]
    public partial class ContractsActivity
    {
        public ContractsActivity()
        {
            Contracts = new HashSet<Contract>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
