using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel.Repositories
{
    public class CustomersClassesRepository : RepositoryBase<CustomerClass>
    {
        public CustomersClassesRepository(JsaEntities context) : base(context)
        {
        }

        public override void Add(CustomerClass entity)
        {
            Context.CustomersClasses.Add(entity);
        }

        public override void Delete(CustomerClass entity)
        {
            Context.CustomersClasses.Remove(entity);
        }

        public override IList<CustomerClass> GetAll()
        {
            return Context.CustomersClasses.ToList();
        }
        /// <summary>
        /// Return the customer class with the supplied id or null if not exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override CustomerClass GetById(object id)
        {
            int i = (int)id;
            return Query(cc => cc.CustomerId == i).SingleOrDefault();
        }

        public override IQueryable<CustomerClass> Query(Expression<Func<CustomerClass, bool>> filter)
        {
            return Context.CustomersClasses.Where(filter);
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
        
    }
}
