using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    public class OutboxRepository : RepositoryBase<Outbox>
    {
        internal OutboxRepository(JsaEntities context)
            : base(context)
        {
        }

        public override void Add(Outbox entity)
        {
            Context.Outboxes.Add(entity);
        }

        public override void Delete(Outbox entity)
        {
            Context.Outboxes.Remove(entity);
        }

        public override Outbox GetById(object id)
        {
            string i = (string)id;
            return Query(cd => cd.OutboxNo == i).Single();
        }

        public override IQueryable<Outbox> Query(System.Linq.Expressions.Expression<Func<Outbox, bool>> filter)
        {
            return Context.Outboxes.Where(filter);
        }

        public override IList<Outbox> GetAll()
        {
            return Context.Outboxes.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
