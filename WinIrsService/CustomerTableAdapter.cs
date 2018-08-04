using System;
using System.Collections.Generic;
using System.Data;
using Jsa.DomainModel;
using Jsa.WinIrsService.Properties;


namespace Jsa.WinIrsService
{
    public class CustomerTableAdapter : IDomainList<Customer>
    {
        private readonly IDatabase _database;

        public CustomerTableAdapter(string dbPath)
        {
            _database = new IrsDatabase(dbPath);
        }

        #region IDomainList<Customer> Members

        public IList<Customer> GetData()
        {
            ITable customerTable = new CustomerTable();
            DataTable dataTable = _database.GetTable(customerTable);
            return CreateCustomers(dataTable);
        }

        #endregion

        private static IList<Customer> CreateCustomers(DataTable table)
        {
            if ((table == null) || (table.Rows.Count == 0)) throw new ArgumentNullException(Resources.NoCustomersMsg);
            IList<Customer> customers = new List<Customer>();
            foreach (DataRow row in table.Rows)
            {
                customers.Add(TransformToCustomer(row));
            }
            return customers;
        }

        private static Customer TransformToCustomer(DataRow row)
        {
            return new Customer
                       {
                           CustomerId = row.Field<int>("Code"),
                           Name = row.Field<string>("Name")
                       };
        }
    }
}