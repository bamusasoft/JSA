
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    public class DocRecordFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        public string DocRecordId { get; set; }

        [Required]
        public string DocFollowId { get; set; }

        [Required]
        public string Path { get; set; }

        public DocRecord DocRecord { get; set; }
        public DocRecordFollow DocRecordFollow { get; set; }
       
    }
}
