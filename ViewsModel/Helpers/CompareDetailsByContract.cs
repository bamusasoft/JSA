using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jsa.DomainModel;

namespace Jsa.ViewsModel.Helpers
{
    public class CompareDetailsByContract : IEqualityComparer<ScheduleDetail>
    {
        public bool Equals(ScheduleDetail x, ScheduleDetail y)
        {
            return x.ContractNo == y.ContractNo;
        }

        public int GetHashCode(ScheduleDetail obj)
        {
            return obj.ContractNo.GetHashCode();
        }
    }
}
