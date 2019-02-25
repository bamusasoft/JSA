using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
   public sealed class DocRecordFileRepository : RepositoryBase<DocRecordFile>
    {
        internal DocRecordFileRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(DocRecordFile entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Context.DocRecordFiles.Add(entity);
        }

        public override void Delete(DocRecordFile entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Context.DocRecordFiles.Remove(entity);
        }

        public override IList<DocRecordFile> GetAll()
        {
            return Context.DocRecordFiles.ToList();
        }

        public override DocRecordFile GetById(object id)
        {
            return Context.DocRecordFiles.Find(id);
        }

        public override IQueryable<DocRecordFile> Query(Expression<Func<DocRecordFile, bool>> filter)
        {
            return Context.DocRecordFiles.Where(filter);
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
