using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Jsa.DomainModel.Repositories
{
    public class ClaimRepository : RepositoryBase<Claim>
    {
        internal ClaimRepository(JsaEntities context)
            : base(context)
        {
        }
        public override void Add(Claim entity)
        {
            Context.Claims.Add(entity);
        }

        public override void Delete(Claim entity)
        {
            Context.Claims.Remove(entity);
        }
        /// <summary>
        /// Get the claim for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Claim GetById(object id)
        {
            string i = (string)id;
            return Query(cl => cl.ClaimId == i).Single();
        }

        public override IQueryable<Claim> Query(System.Linq.Expressions.Expression<Func<Claim, bool>> filter)
        {
            return Context.Claims
                .Include(c => c.ClaimDetails)
                .Include(c => c.Customer)
                //.Include(c => c.ClaimDetails.Select(cd => cd.Property))
                .Where(filter);
        }

        public override IList<Claim> GetAll()
        {
            return Context.Claims.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
        public short MaxClaimSeqeunce(int customerId, string year)
        {
            var result = Query(c => c.CustomerId == customerId && c.ClaimYear == year);
            if (result == null || result.Count() == 0) return 0; //The customer has no claim issued yet.
            return result.Max(x => x.Sequence);
        }
    }
}
