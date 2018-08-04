using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;

using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represent Customers Repostiory. Instantiate via UnitOfWork object
    /// </summary>
    public sealed class CustomersRepository:RepositoryBase<Customer>
    {
        internal CustomersRepository(JsaEntities context)
            : base(context)
        {
        }

        #region Overrides of BaseRepository<Customer>

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(Customer entity)
        {
            Context.Customers.Add(entity);
        }
        
        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public override Customer GetById(object id)
        {
            var i = (int) id;
            return Query(x => x.CustomerId == i).Single();
        }

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override IQueryable<Customer> Query(Expression<Func<Customer, bool>> filter)
        {
            return Context.Customers.Where(filter);
        }

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public override IList<Customer> GetAll()
        {
            return Context.Customers.ToList();
        }

        public override void Refresh()
        {
           throw new NotImplementedException();
        }

        public IList<Customer> GetActiveCustomers()
        {
            var q =
                Context.Contracts.Where(x => x.Closed == false)
                    .Select(c => c.Customer)
                    .Distinct()
                    .OrderBy(x => x.Name);
            return q.ToList();
        }
        #endregion
    }
}