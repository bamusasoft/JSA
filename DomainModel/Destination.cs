using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    public class Destination
    {
        public Destination()
        {
            DocRecords = new HashSet<DocRecord>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<DocRecord> DocRecords { get; set; }
        public override int GetHashCode()
        {
            int hash = 13;
            hash = hash * Id;
            hash = hash + 17;
            return hash;
        }
        public override bool Equals(object obj)
        {
           
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((Destination)obj).Id == Id;
        }
    }
}
