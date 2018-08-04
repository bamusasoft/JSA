using System.Collections.Generic;

namespace Jsa.ViewsModel.Helpers
{
    public class ContractPayments
    {
        public int ContractNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CustomerName { get; set; }
        public string PropertyLocation { get; set; }
        public string PropertyNo { get; set; }
        public int CustomerNo { get; set; }
        public int AgreedRent { get; set; }
        public int RentDue { get; set; }
        public int RentPaid
        {
            get { return RentDue - RentBalance; }

        }
        public int RentBalance { get; set; }
        
        public int MentDue { get; set; }
        public int MentPaid
        {
            get { return MentDue - MentBalance; }
        }
            
        public int MentBalance { get; set; }

        public int DepositDue { get; set; }
        public int DepositPaid
        {
            get { return DepositDue - DepositBalance; }
            
        }
        public int DepositBalance { get; set; }

        public int DueTotals
        {
            get { return RentDue + MentDue + DepositDue; }

        }
        public int PaidTotals
        {
            get { return RentPaid + MentPaid + DepositPaid; }
        }
        public int BalanceTotals
        {
            get { return RentBalance + MentBalance + DepositBalance; }
        }
        
        public IList<ContractPaymentDetails> PaymentsDetails
        {
            get;
            set;
        }
        
    }
    
}
