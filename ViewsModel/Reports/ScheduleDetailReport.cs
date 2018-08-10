using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Jsa.ViewsModel.Reports
{
    public class ScheduleDetailReport
    {
        private const string YES = "نعم";
        private const string NO = "لا";
        private string _dateDue;
        private readonly bool _sumRow;
        public int AmountDue { get; private set; }
        public string DateDue
        {
            get
            {
                if (_sumRow) return _dateDue;
                var m = Helper.DateMiror(_dateDue);
                return m;
            }
            private set { _dateDue = value; }
        }
        public int AmountPaid { get; private set; }

        public int Balance { get; private set; }

        public string Status
        {
            get
            {
                if (_sumRow)
                {
                    return "";
                }
                if (Balance == 0)
                {
                    return YES;
                }
                return NO;
            }
        }
        public ScheduleDetailReport(int amountDue, string dateDue, int amountPaid, int balance, bool sumRow)
        {
            AmountDue = amountDue;
            DateDue = dateDue;
            AmountPaid = amountPaid;
            Balance = balance;
            _sumRow = sumRow;
        }
    }
}
