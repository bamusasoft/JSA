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
        // Summary:
        //     Finds an entity with the given primary key values. If an entity with the given
        //     primary key values exists in the context, then it is returned immediately without
        //     making a request to the store. Otherwise, a request is made to the store for
        //     an entity with the given primary key values and this entity, if found, is attached
        //     to the context and returned. If no entity is found in the context or the store,
        //     then null is returned.
        //
        // Parameters:
        //   keyValues:
        //     The values of the primary key for the entity to be found.
        //
        // Returns:
        //     The entity found, or null.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     Thrown if multiple entities exist in the context with the primary key values
        //     given.
        //
        //   T:System.InvalidOperationException:
        //     Thrown if the type of entity is not part of the data model for this context.
        //
        //   T:System.InvalidOperationException:
        //     Thrown if the types of the key values do not match the types of the key values
        //     for the entity type to be found.
        //
        //   T:System.InvalidOperationException:
        //     Thrown if the context has been disposed.
        //
        // Remarks:
        //     The ordering of composite key values is as defined in the EDM, which is in turn
        //     as defined in the designer, by the Code First fluent API, or by the DataMember
        //     attribute.
        /// </summary>
        public override DocRecord GetById(object id)
        {
            return Context.DocRecords.Find(id);
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
