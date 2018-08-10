namespace Jsa.WinIrsService
{
    internal class PropertiesTable : ITable
    {
        private const string TableName = "Properties";
        private const string TableAbbreviation = "prop";
        private readonly string[] _columns;
        private string _query;

        public PropertiesTable()
        {
            _columns = new[]
                           {
                               "Property", "[Name]", "Location", "Type"
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