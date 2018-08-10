using Jsa.DomainModel;
using System.Collections.Generic;

namespace Jsa.ViewsModel.Helpers
{
    public class OutboxNoComparer : IEqualityComparer<Outbox>
    {

        public bool Equals(Outbox x, Outbox y)
        {
            string s = x.OutboxNo.Substring(0, 4);
            string n = y.OutboxNo.Substring(0, 4);
            return s == n;
        }

        public int GetHashCode(Outbox obj)
        {
            return 0;

        }
    }
}
