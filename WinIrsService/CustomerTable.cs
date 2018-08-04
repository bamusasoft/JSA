namespace Jsa.WinIrsService
{
    internal class CustomerTable : ITable
    {
        private const string TableName = "Customers";
        private const string TableAbbreviation = "cust";
        private readonly string[] _columns;
        private string _query;

        public CustomerTable()
        {
            _columns = new[]
                           {
                               "Code", "[Name]"
                           };
        }

        #region "ITable"

        public string[] Columns
        {
            get { return _columns; }
        }

        public string BuildQuery()
        {
            if (_query != null) return _query;
            _query = Helper.BuildQuery(this, Columns, null);

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
    }
}