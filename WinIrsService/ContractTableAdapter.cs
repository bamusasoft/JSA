using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Jsa.DomainModel;
using Jsa.WinIrsService.Properties;

namespace Jsa.WinIrsService
{
    public class ContractTableAdapter : IDomainList<Contract>
    {
        private readonly IDatabase _database;


        /// <summary>
        /// Transform Contract Table into domain list.
        /// </summary>
        /// <param name="dbPath">Where the irs db path.</param>
        /// <param name="contractsYear">Should be the first two numbers from left at the contract number.</param>
        public ContractTableAdapter(string dbPath, string contractsYear)
        {
            if (string.IsNullOrEmpty(dbPath)) throw new ArgumentNullException("dbPath");
            if (string.IsNullOrEmpty(contractsYear)) throw new ArgumentNullException("currentYear");

            _database = new IrsDatabase(dbPath);
            Settings.Default.CurrentYear = contractsYear;
        }

        #region IDomainList<Contract> Members

        public IList<Contract> GetData()
        {
            ITable contractTable = new ContractsTable();
            DataTable dataTable = _database.GetTable(contractTable);
            return CreateContracts(dataTable);
        }
        
        #endregion

        #region "Helpers"

        private static IList<Contract> CreateContracts(DataTable table)
        {
            if ((table == null) || (table.Rows.Count == 0)) throw new ArgumentNullException(Resources.NoContractsMsg);
            return (from DataRow row in table.Rows select TransformToContract(row)).ToList();
        }

        private static Contract TransformToContract(DataRow row)
        {
            return new Contract
                       {
                           ContractNo = row.Field<int>("ContractNo"),
                           StartDate = row.Field<string>("StartDate"),
                           EndDate = row.Field<string>("EndDate"),
                           CustomerId = row.Field<int>("Customer"),
                           PropertyNo = row.Field<string>("Property"),
                           Closed = row.Field<bool>("Closed"),
                           AgreedRent = row.Field<int>("AgreedRent"),
                           RentDue = row.Field<int>("Rent"),
                           AgreedDeposit = row.Field<int>("Deposit"),
                           AgreedMaintenance = row.Field<int>("Maintenance"),
                           RentBalance = row.Field<int>("RentBal"),
                           MaintenanceBalance = row.Field<int>("MaintBal"),
                           DepositBalance = row.Field<int>("DepositBal"),
                           Total = row.Field<int>("RentBal") +
                                   row.Field<int>("MaintBal") +
                                   row.Field<int>("DepositBal"),
                           Tax = row.Field<double?>("Tax")

                       };
        }

        #endregion
    }
}