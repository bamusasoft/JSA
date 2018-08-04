using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    public class CaseFollowingRepository : RepositoryBase<CaseFollowing>
    {
        public CaseFollowingRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(CaseFollowing entity)
        {
            Context.CaseFollowings.Add(entity);
        }

        public override void Delete(CaseFollowing entity)
        {
            Context.CaseFollowings.Remove(entity);
        }

        public override CaseFollowing GetById(object id)
        {
            int i = (int)id;
            return Query(x => x.Id == i).Single();
        }

        public override IQueryable<CaseFollowing> Query(Expression<Func<CaseFollowing, bool>> filter)
        {
            return Context.CaseFollowings.Where(filter)
                .Include(x => x.LegalCase);
        }

        public override IList<CaseFollowing> GetAll()
        {
            return Context.CaseFollowings.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public IList<CaseFollowing> GetCaseFollowing(int caseNo)
        {
            return Query(x => x.CaseNo == caseNo).ToList();
        }
        public bool TryCheckExist(int id, out CaseFollowing following)
        {
            bool exist = false;
            try
            {
                var s = Context.CaseFollowings.Single(x => x.Id == id);
                following = s;
                exist = true;
            }
            catch
            {

                following = new CaseFollowing();
            }
            return exist;
        }

        public IList<CaseFollowing> DueFollowings(int days)
        {

            //Use DbFunctikons.AddDays to get rid of limitation in linq to entities that it does not support 
            //DataTime.AddDays, instead of loading all the store data then filter using linq to objects.
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            DateTime dueDate = DateTime.Now.AddDays(days);
            var s = Query(x =>
                DbFunctions.CreateDateTime(x.GregNextFollowingDate.Year, x.GregNextFollowingDate.Month, x.GregNextFollowingDate.Day, 0, 0, 0) >= DateTime.Now
                &&
                DbFunctions.CreateDateTime(x.GregNextFollowingDate.Year, x.GregNextFollowingDate.Month, x.GregNextFollowingDate.Day, 0, 0, 0) <= dueDate
                );
            return s.ToList();
        }
        
    }
}