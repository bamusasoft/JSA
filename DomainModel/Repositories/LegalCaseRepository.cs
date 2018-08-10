using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    public class LegalCaseRepository : RepositoryBase<LegalCase>
    {
        public LegalCaseRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(LegalCase entity)
        {
            Context.LegalCases.Add(entity);
        }

        public override void Delete(LegalCase entity)
        {
            Context.LegalCases.Remove(entity);
        }

        public override LegalCase GetById(object id)
        {
            int no = (int)id;
            return Query(x => x.CaseNo == no).Single();
        }

        public override IQueryable<LegalCase> Query(Expression<Func<LegalCase, bool>> filter)
        {
            return Context.LegalCases
                .Include(x => x.CaseAppointments)
                .Include(x => x.CaseFollowings)
                .Include(x => x.CaseStatus)
                .Where(filter);
        }

        public override IList<LegalCase> GetAll()
        {
            return Context.LegalCases
                .Include(x => x.CaseAppointments)
                .Include(x => x.CaseFollowings)
                .Include(x => x.CaseStatus)
                .ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public bool TryCheckExist(int caseNo, out LegalCase legalCase)
        {
            bool exist = false;
            try
            {
                var s = Context.LegalCases.Single(x => x.CaseNo == caseNo);
                legalCase = s;
                exist = true;
            }
            catch
            {

                legalCase = new LegalCase();
            }
            return exist;
        }

    }
}