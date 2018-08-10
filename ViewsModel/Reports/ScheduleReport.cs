using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jsa.DomainModel;

namespace Jsa.ViewsModel.Reports
{
    public class ScheduleReport
    {
        public string CustomerName { get; set; }
        public string PropertyLocation { get; set; }

        public List<ScheduleDetailReport> Details { get; set; }

        public int PropertiesCount
        {
            get
            {
                return typeof(ScheduleDetailReport).GetProperties().Count();
            }
        }

        public ScheduleReport(string name, string loc, IEnumerable<ScheduleDetailReport> details)
        {
            CustomerName = name;
            PropertyLocation = loc;
            Details = new List<ScheduleDetailReport>(details);
        }

    }

}
