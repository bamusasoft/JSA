using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jsa.DomainModel
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class Contract
    {
        public Contract()
        {
            ScheduleDetails = new HashSet<ScheduleDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContractNo { get; set; }

        [Required]
        [StringLength(8)]
        public string StartDate { get; set; }

        [Required]
        [StringLength(8)]
        public string EndDate { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(20)]
        public string PropertyNo { get; set; }

        public int AgreedRent { get; set; }

        public int RentDue { get; set; }

        public int AgreedDeposit { get; set; }

        public int AgreedMaintenance { get; set; }

        public int RentBalance { get; set; }

        public int MaintenanceBalance { get; set; }

        public int DepositBalance { get; set; }

        public int Total { get; set; }

        public bool Closed { get; set; }

        public int? ActivityId { get; set; }

        [StringLength(10)]
        public string SignDay { get; set; }

        [StringLength(8)]
        public string SignHijriDate { get; set; }

        [StringLength(8)]
        public string SignGregDate { get; set; }

        public bool Scheduled { get; set; }

        public string PhotoPath { get; set; }

        public virtual ContractsActivity ContractsActivity { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Property Property { get; set; }

        public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; }




        public int Paid
        {
            get 
            {
                return ((RentDue - RentBalance)+ (AgreedMaintenance- MaintenanceBalance) + (AgreedDeposit - DepositBalance));
            }
        }

        public int Balance
        {
            get { return (RentBalance + MaintenanceBalance + DepositBalance); }
        }

        public string ContractYear
        {
            get
            {
                if (string.IsNullOrEmpty(StartDate)) return null;
                return StartDate.Substring(0, 4);
            }
        }

        
    }
}
