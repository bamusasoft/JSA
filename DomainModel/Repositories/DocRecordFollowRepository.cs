using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    class DocRecordFollowRepository : RepositoryBase<DocRecordFollow>
    {
        internal DocRecordFollowRepository(JsaEntities context)
            : base(context)
        {
        }
        public override void Add(DocRecordFollow entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Context.DocRecordFollows.Add(entity);
        }

        public override void Delete(DocRecordFollow entity)
        {
            throw new NotImplementedException();
        }

        public override DocRecordFollow GetById(object id)
        {
            string i = (string)id;
            return Query(x => x.Id == i).Single();
        }

        public override IQueryable<DocRecordFollow> Query(System.Linq.Expressions.Expression<Func<DocRecordFollow, bool>> filter)
        {
            return Context.DocRecordFollows.Where(filter);
        }

        public override IList<DocRecordFollow> GetAll()
        {
            return Context.DocRecordFollows.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
    
}
