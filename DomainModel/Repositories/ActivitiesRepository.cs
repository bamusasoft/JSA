using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    public sealed  class ActivitiesRepository:RepositoryBase<ContractsActivity>
    {
        internal ActivitiesRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(ContractsActivity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Context.ContractsActivities.Add(entity);
        }

        public override void Delete(ContractsActivity entity)
        {
            throw new NotImplementedException();
        }

        public override ContractsActivity GetById(object id)
        {
            int i = (int) id;
            return Query(x => x.Id == i).Single();
        }

        public override IQueryable<ContractsActivity> Query(System.Linq.Expressions.Expression<Func<ContractsActivity, bool>> filter)
        {
            return Context.ContractsActivities.Where(filter);
        }

        public override IList<ContractsActivity> GetAll()
        {
            return Context.ContractsActivities.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
