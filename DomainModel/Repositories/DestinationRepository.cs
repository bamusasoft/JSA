using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    public class DestinationRepository : RepositoryBase<Destination>
    {
        internal DestinationRepository(JsaEntities context)
            : base(context)
        {
        }
        public override void Add(Destination entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Context.Destinations.Add(entity);
        }

        public override void Delete(Destination entity)
        {
            throw new NotImplementedException();
        }

        public override Destination GetById(object id)
        {
            int i = (int)id;
            return Query(x => x.Id == i).Single();
        }

        public override IQueryable<Destination> Query(System.Linq.Expressions.Expression<Func<Destination, bool>> filter)
        {
            return Context.Destinations.Where(filter);
        }

        public override IList<Destination> GetAll()
        {
            return Context.Destinations.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
