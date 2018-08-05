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
        [StringLength(13)]
        public string Id { get; set; }

        [Required]
        [StringLength(8)]
        public string FollowDate { get; set; }

        [Required]
        [StringLength(10)]
        public string DocRecodId { get; set; }

        [Required]
        public string FollowContent { get; set; }

        public string FollowPath { get; set; }

        public virtual DocRecord DocRecord { get; set; }
    }
}
