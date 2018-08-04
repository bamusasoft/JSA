using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    public class RepresRepository : RepositoryBase<Representative>
    {
        internal RepresRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(Representative entity)
        {
            Context.Representatives.Add(entity);
        }

        public override void Delete(Representative entity)
        {
            Context.Representatives.Remove(entity);
        }

        public override Representative GetById(object id)
        {
            string i = (string)id;
            return Query(cd => cd.Id == i).Single();
        }

        public override IQueryable<Representative> Query(System.Linq.Expressions.Expression<Func<Representative, bool>> filter)
        {
            return Context.Representatives.Where(filter);
        }

        public override IList<Representative> GetAll()
        {
            return Context.Representatives.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
