using Jsa.WinIrsService.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.WinIrsService
{
    internal class PaymentsTable: ITable
    {
        private readonly string[] _columns;
        private string _query;
        private const string TableName = "Payments";
        private const string TableAbbreviation = "pays";
        private readonly Settings _settings;
        public PaymentsTable()
        {
            _columns = new[] 
            {
               "PaymentNo" , "AccountNo", "ContractNo",
               "Renewal" , "[Name]", "PayDate",
               "PaymentType" , "PaymentFor1", "PaymentFor2",
               "TotalPayment" , "Rent", "Deposit",
               "Maintenance" , "Others", "Posted",
               "DebitAccount" , "PayCode"
            };
            _settings = Settings.Default;
        }
        public string[] Columns
        {
            get { return _columns; }
        }

        public string Name
        {
            get { return TableName; }
        }

        public string Abbreviation
        {
            get { return TableAbbreviation; }
        }

        public string BuildQuery()
        {
            if (_query != null) return _query;
            _query = Helper.BuildQuery(this, Columns, WhereClause);
            return _query;
        }
        private string WhereClause()
        {
            return "WHERE Left(pays.PaymentNo, 2) = " + _settings.CurrentYear;
        }
    }
}
