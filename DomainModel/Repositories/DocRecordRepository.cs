using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    class DocRecordRepository : RepositoryBase<DocRecord>
    {
        internal DocRecordRepository(JsaEntities context)
            : base(context)
        {
        }
        public override void Add(DocRecord entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Context.DocRecords.Add(entity);
        }

        public override void Delete(DocRecord entity)
        {
            throw new NotImplementedException();
        }

        public override DocRecord GetById(object id)
        {
            string i = (string)id;
            return Query(x => x.Id == i).Single();
        }

        public override IQueryable<DocRecord> Query(System.Linq.Expressions.Expression<Func<DocRecord, bool>> filter)
        {
            return Context.DocRecords.Where(filter);
        }

        public override IList<DocRecord> GetAll()
        {
            return Context.DocRecords.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
