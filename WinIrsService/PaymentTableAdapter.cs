using Jsa.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jsa.WinIrsService.Properties;
namespace Jsa.WinIrsService
{
    public class PaymentTableAdapter:IDomainList<Payment>
    {
        readonly IDatabase _database;
        public PaymentTableAdapter(string dbPath, string paysYear)
        {
            if (string.IsNullOrEmpty(dbPath)) throw new ArgumentNullException("dbPath");
            _database = new IrsDatabase(dbPath);
            Settings.Default.CurrentYear = paysYear;
        }
        public IList<Payment> GetData()
        {
            ITable payTable = new PaymentsTable();
            DataTable dataTable = _database.GetTable(payTable);
            return CreatePayments(dataTable);
        }

        private static IList<Payment> CreatePayments(DataTable table)
        {
            if ((table == null) || (table.Rows.Count == 0)) throw new ArgumentNullException(Resources.NoPaymentsMsg);
            IList<Payment> payments = new List<Payment>();
            foreach (DataRow row in table.Rows)
            {
                payments.Add(TransformToCustomer(row));
            }
            return payments;
        }

        private static Payment TransformToCustomer(DataRow row)
        {
            Payment p = new Payment();

               p. PaymentNo = row.Field<int>("PaymentNo");
               p. AccountNo = row.Field<string>("AccountNo");
               p. ContractNo = row.Field<int>("ContractNo");
               p.Renewal = row.Field<short>("Renewal");
               p. Name = row.Field<string>("Name");
               p. PayDate = row.Field<string>("PayDate");
               p. PaymentType = row.Field<byte>("PaymentType");
               p. PaymentFor1 = row.Field<string>("PaymentFor1");
               p. PaymentFor2 = row.Field<string>("PaymentFor2");
               p. TotalPayment = row.Field<double>("TotalPayment");
               p. Rent = row.Field<int>("Rent");
               p. Deposit = row.Field<int>("Deposit");
               p. Maintenance = row.Field<int>("Maintenance");
               p. Others = row.Field<int>("Others");
               p. Posted = row.Field<bool>("Posted");
               p. DebitAccount = row.Field<string>("DebitAccount");
               p.PayCode = row.Field<short>("PayCode");

               return p;
            
        }
    }
}
