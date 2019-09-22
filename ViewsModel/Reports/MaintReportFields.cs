using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public sealed class MaintReportFields
    {
         public MaintReportFields(int contractNo, string propertyNo, int customerNo, string propertyType, string customerName, string propertyDescription, string location, int agreedMaint, double paid, int receiptNo, string payDate)
        {
            DateDue = payDate;
            ReceiptNo = receiptNo;
            Paid = paid;
            AgreedMaint = agreedMaint;
            Location = location;
            PropertyDescription = propertyDescription;
            CustomerName = customerName;
            PropertyType = propertyType;
            CustomerNo = customerNo;
            PropertyNo = propertyNo;
            ContractNo = contractNo;
        }


        public int ContractNo { get; private set; }
        public string PropertyNo { get; private set; }
        public int CustomerNo { get; private set; }
        public string PropertyType { get; private set; }
        public string CustomerName { get; private set; }
        public string PropertyDescription { get; private set; }
        public string Location { get; private set; }
        public double AgreedMaint { get; private set; }
        public double Paid { get; private set; }

        public double Balance
        {
            get { return AgreedMaint - Paid; }
        }
        public int ReceiptNo { get; private set; }
        public string DateDue { get; private set; }
    }
}
