using Jsa.WinIrsService.Properties;
namespace Jsa.WinIrsService
{
    internal class ContractsTable : ITable
    {

        private readonly string[] _columns;
        private string _query;
        private const string TableName = "Contracts";
        private const string TableAbbreviation = "cont";
        private readonly Settings _settings ;
        public ContractsTable()
        {
            _columns = new[]
                           {
                               "ContractNo", "StartDate", "EndDate", "Customer", "Property",
                               "AgreedRent", "Rent", "Deposit", "Maintenance", "RentBal",
                               "DepositBal", "MaintBal", "Closed"
                           };
            _settings = Settings.Default;
        }

        #region ITable Members

        public string[] Columns
        {
            get { return _columns; }
        }

        public string BuildQuery()
        {
            if (_query != null) return _query;
            _query = Helper.BuildQuery(this, Columns, WhereClause);

            return _query;
        }

        public string Name
        {
            get { return TableName; }
        }

        public string Abbreviation
        {
            get { return TableAbbreviation; }
        }

        #endregion
        private string WhereClause()
        {
            return "WHERE Left(cont.ContractNo, 2) = " + _settings.CurrentYear;
        }
    }

}