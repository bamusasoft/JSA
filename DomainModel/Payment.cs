using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    [Serializable]
    public partial class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaymentNo { get; set; }

        [StringLength(20)]
        public string AccountNo { get; set; }

        public int ContractNo { get; set; }

        public short Renewal { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(8)]
        public string PayDate { get; set; }

        public byte PaymentType { get; set; }

        [Required]
        [StringLength(100)]
        public string PaymentFor1 { get; set; }

        [StringLength(100)]
        public string PaymentFor2 { get; set; }

        public double TotalPayment { get; set; }

        public int Rent { get; set; }

        public int Deposit { get; set; }

        public int Maintenance { get; set; }

        public int Others { get; set; }

        public bool Posted { get; set; }

        [StringLength(15)]
        public string DebitAccount { get; set; }

        public short PayCode { get; set; }
    }
}
