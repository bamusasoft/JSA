using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    public class CaseStatusRepository : RepositoryBase<CaseStatus>
    {
        public CaseStatusRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(CaseStatus entity)
        {
            Context.CaseStatuses.Add(entity);
        }

        public override void Delete(CaseStatus entity)
        {
            Context.CaseStatuses.Remove(entity);
        }

        public override CaseStatus GetById(object id)
        {
            int i = (int)id;
            return Query(x => x.Id == i).Single();
        }

        public override IQueryable<CaseStatus> Query(Expression<Func<CaseStatus, bool>> filter)
        {
            return Context.CaseStatuses.Where(filter);
        }

        public override IList<CaseStatus> GetAll()
        {
            return Context.CaseStatuses.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}