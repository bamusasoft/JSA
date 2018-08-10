using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    public partial class Outbox
    {
        #region "Fields"
        string _subject;
        #endregion

        [Key]
        [StringLength(8)]
        public string OutboxNo { get; set; }

        [Required]
        [StringLength(8)]
        public string OutboxDate { get; set; }

        [Required]
        [StringLength(1000)]
        public string Subject
        {
            get { return _subject.Trim(); }
            set
            {
                _subject = value;
            }
        }

        [Required]
        [StringLength(100)]
        public string GoingTo { get; set; }

        [StringLength(2)]
        public string AttachmentNo { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public bool Deleted { get; set; }

       
        

        public static string MaxOutboxNo
        {
            get
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    return unit.Outboxes.GetAll().Select(x => x.OutboxNo).Max();
                }
            }
        }

    }
}
