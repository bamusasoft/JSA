using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    public class DocRecordFollow
    {
        public DocRecordFollow()
        {
            DocRecordFiles = new HashSet<DocRecordFile>();
        }
        [StringLength(11)]
        public string Id { get; set; }

        [Required]
        [StringLength(8)]
        public string FollowDate { get; set; }

        [Required]
        [StringLength(8)]
        public string DocRecodId { get; set; }

        [Required]
        public string FollowContent { get; set; }


        public virtual DocRecord DocRecord { get; set; }

        public virtual ICollection<DocRecordFile> DocRecordFiles { get; set; }
    }
}
