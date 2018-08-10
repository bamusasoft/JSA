using System;
using System.Data;
using System.Data.OleDb;
using Jsa.WinIrsService.Properties;


namespace Jsa.WinIrsService
{
    internal class IrsDatabase : IDatabase
    {
        private const string Provider = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        private readonly string _connectionString;

        public IrsDatabase(string dbPath)
        {
            _connectionString = string.Format("{0}{1}", Provider, dbPath);
        }

        #region IDatabase Members

        public DataTable GetTable(ITable table)
        {
            var dataTable = new DataTable();
            try
            {
                using (var conn = new OleDbConnection(_connectionString))
                {
                    using (var command = new OleDbCommand(table.BuildQuery(), conn))
                    {
                        using (var adp = new OleDbDataAdapter(command))
                        {
                            adp.Fill(dataTable);
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new WinIrsServiceException(Resources.AccessDbErrorMsg, ex);
            }
        }
        

        #endregion


        
    }
}