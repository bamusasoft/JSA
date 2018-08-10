using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represent Contract Repostiory. Instantiate via UnitOfWork object
    /// </summary>
    public sealed class ContractsRepository:RepositoryBase<Contract>
    {
        internal ContractsRepository(JsaEntities context)
            : base(context)
        {
        }

        #region Overrides of BaseRepository<Contract>

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(Contract entity)
        {
            Context.Contracts.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(Contract entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public override Contract GetById(object id)
        {
            var i = (int) id;
            return Query(co => co.ContractNo == i).Single();
        }

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override IQueryable<Contract> Query(Expression<Func<Contract, bool>> filter)
        {
            return Context.Contracts
                .Include(a => a.Customer)
                .Include(b => b.Property )
                .Include(c => c.ContractsActivity)
                .Include(d => d.ScheduleDetails)
                .Where(filter);
        }

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public override IList<Contract> GetAll()
        {
           return Context.Contracts.ToList();
        }

        public override void Refresh()
        {
            
        }

       
        
        

        #endregion
        public IList<Contract> ActiveContracts(string propertyNo)
        {
            return Query
                         (
                         x => x.Closed == false &&
                         x.Property.PropertyNo.StartsWith(propertyNo)
                         )
                         .OrderBy(p => p.Property.PropertyNo)
                         .ToList();
        }

        /// <summary>
        /// Get contracts that is valid. Valid means, Not closed and also not scheduled yet.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Contract> CustomerActiveContracts(int customerId)
        {
            return Query(x => x.CustomerId == customerId
                              && x.Closed == false
                              && x.ScheduleDetails.Count == 0
                );
        }
        /// <summary>
        /// Get The scheduled contracts of this year.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Contract> ScheduledContracts()
        {
            return Query(x => !x.Closed && x.ScheduleDetails.Count > 0);
        }
    }
}