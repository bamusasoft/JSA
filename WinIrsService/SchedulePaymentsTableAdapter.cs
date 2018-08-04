using System;
using System.Collections.Generic;
using System.Data;
using Jsa.DomainModel;
using Jsa.WinIrsService.Properties;

namespace Jsa.WinIrsService
{
    public class SchedulePaymentsTableAdapter : IDomainList<SchedulePayment>
    {
        private readonly IDatabase _database;

        public SchedulePaymentsTableAdapter(string dbPath, string paymentsYear)
        {
            if (dbPath == null) throw new ArgumentNullException("dbPath");
            if (paymentsYear == null) throw new ArgumentNullException("paymentsYear");
            _database = new IrsDatabase(dbPath);
            Settings.Default.CurrentYear = paymentsYear;
        }

        #region IDomainList<Payment> Members

        public IList<SchedulePayment> GetData()
        {
            ITable paymentTable = new SchedulePaymentsTable();
            DataTable dataTable = _database.GetTable(paymentTable);
            return CreatePayments(dataTable);
        }

        #endregion

        private static IList<SchedulePayment> CreatePayments(DataTable table)
        {
            if (table == null) throw new ArgumentNullException();
            if (table.Rows.Count == 0) throw new InvalidOperationException(Resources.NoPaymentsMsg);
            IList<SchedulePayment> payments = new List<SchedulePayment>();
            foreach (DataRow row in table.Rows)
            {
                payments.Add(TransformToPayment(row));
            }
            return payments;
        }

        private static SchedulePayment TransformToPayment(DataRow row)
        {
            return new SchedulePayment
                       {
                           ContractNo = row.Field<int>("ContractNo"),
                           TotalPayment = (int) row.Field<double>("TotalPayment")
                       };
        }
    }
}