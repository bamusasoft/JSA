namespace Jsa.ViewsModel.Helpers
{
    public class IresContract
    {
        public int  ContractNo { get; set; }
        public int  Renewal { get; set; }
        public string  StartDate { get; set; }
        public string  EndDate { get; set; }
        public int  Type { get; set; }
        public int  Customer { get; set; }
        public int  AgreedRent { get; set; }
        public string  Property { get; set; }
        public int  NoPayments { get; set; }
        public int  Rent { get; set; }
        public int  Deposit { get; set; }
        public int  Maintenance { get; set; }
        public int  Others { get; set; }
        public bool  Closed { get; set; }
        public int  RentBal { get; set; }
        public int  DepositBal { get; set; }
        public int  MaintBal { get; set; }
        public int  OthersBal { get; set; }
        public string  Remarks { get; set; }
        public string  Desc_Others { get; set; }
        public bool  Posted { get; set; }
    }
    public class TempContract
    {
        public object ContractNo { get; set; }
        public object Renewal { get; set; }
        public object StartDate { get; set; }
        public object EndDate { get; set; }
        public object Type { get; set; }
        public object Customer { get; set; }
        public object AgreedRent { get; set; }
        public object Property { get; set; }
        public object NoPayments { get; set; }
        public object Rent { get; set; }
        public object Deposit { get; set; }
        public object Maintenance { get; set; }
        public object Others { get; set; }
        public object Closed { get; set; }
        public object RentBal { get; set; }
        public object DepositBal { get; set; }
        public object MaintBal { get; set; }
        public object OthersBal { get; set; }
        public object Remarks { get; set; }
        public object Desc_Others { get; set; }
        public object Posted { get; set; }
    }
}
