using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    public class ClaimDetailRepository : RepositoryBase<ClaimDetail>
    {
        internal ClaimDetailRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(ClaimDetail entity)
        {
            Context.ClaimDetails.Add(entity);
        }

        public override void Delete(ClaimDetail entity)
        {
            Context.ClaimDetails.Remove(entity);
        }

        public override ClaimDetail GetById(object id)
        {
            int i = (int)id;
            return Query(cd => cd.ClaimDetailId == i).Single();
        }

        public override IQueryable<ClaimDetail> Query(System.Linq.Expressions.Expression<Func<ClaimDetail, bool>> filter)
        {
            return Context.ClaimDetails.Where(filter);
        }

        public override IList<ClaimDetail> GetAll()
        {
            return Context.ClaimDetails.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
