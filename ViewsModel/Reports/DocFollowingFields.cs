using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public class DocFollowingFields
    {
        public string FollowDate { get; set; }
        public string FollowContent { get; set; }
        public DocFollowingFields(string followDate, string followContent)
        {
            FollowDate = followDate;
            FollowContent = followContent;
        }
    }
}
