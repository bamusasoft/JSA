using Jsa.WinIrsService.Properties;

namespace Jsa.WinIrsService
{
    internal class SchedulePaymentsTable : ITable
    {
        private const string TableName = "Payments";
        private const string TableAbbreviation = "pay";
        private readonly string[] _columns;
        private string _query;

        public SchedulePaymentsTable()
        {
            _columns = new[]
                           {
                               "ContractNo", "TotalPayment"
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
            string s = 
                @"SELECT p.ContractNo, SUM(p.TotalPayment) As TotalPayment FROM Payments AS p 
                      WHERE LEFT(p.ContractNo, 2) = {0} Group by  p.ContractNo";
            _query = string.Format(s, Settings.Default.CurrentYear);

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