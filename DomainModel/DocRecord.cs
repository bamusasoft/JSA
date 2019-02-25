using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    public class DocRecord
    {
        public DocRecord()
        {
            DocRecordFollows = new HashSet<DocRecordFollow>();
            DocRecordFiles = new HashSet<DocRecordFile>();

        }

        [StringLength(8)]
        public string Id { get; set; }

        [StringLength(10)]
        public string RefId { get; set; }

        [Required]
        [StringLength(8)]
        public string DocDate { get; set; }

        [Required]
        public string Subject { get; set; }

        public int DestId { get; set; }

        public string DocPath { get; set; }

        public DocRecordStatus DocStatus { get; set; }

        public int SecurityLevel { get; set; }

        public virtual Destination Destination { get; set; }

        public virtual ICollection<DocRecordFollow> DocRecordFollows { get; set; }

        public virtual ICollection<DocRecordFile> DocRecordFiles { get; set; }
    }
}
