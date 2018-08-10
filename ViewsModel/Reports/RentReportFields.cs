using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public class RentReportFields
    {
         public RentReportFields(int contractNo, string propertyNo, int customerNo, string customerName, 
             string propertyType, string propertyDescription, string location,
             int agreedRent, int rentDue, int paid,
             string mobile1, string mobile2, string workPhone, string homePhone)
        {
            Paid = paid;
            RentDue = rentDue;
            AgreedRent = agreedRent;
            Location = location;
            PropertyDescription = propertyDescription;
            PropertyType = propertyType;
            CustomerName = customerName;
            CustomerNo = customerNo;
            PropertyNo = propertyNo;
            ContractNo = contractNo;
            Mobile1 = mobile1;
            Mobile2 = mobile2;
            WorkPhone = workPhone;
            HomePhone = homePhone;
        }


        public int ContractNo { get; private set; }
        public string PropertyNo { get; private set; }
        public int CustomerNo { get; private set; }
        public string CustomerName { get; private set; }
        public string PropertyType { get; private set; }
        public string PropertyDescription { get; private set; }
        public string Location { get; private set; }
        public int AgreedRent { get; private set; }
        public int RentDue { get; private set; }
        public int Paid { get; private set; }

        public int Balance
        {
            get { return RentDue - Paid; }
        }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        
       
    }
}
