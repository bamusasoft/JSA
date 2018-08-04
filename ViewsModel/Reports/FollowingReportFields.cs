using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public class FollowingReportFields
    {
        public string FollowingDate { get; private set; }
        public string Destination { get; private set; }
        public string Subject { get; private set; }
        public FollowingReportFields(string followDate, string destination, string subject)
        {
            FollowingDate = followDate;
            Destination = destination;
            Subject = subject;


        }


    }
}
