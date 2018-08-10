using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represent SchedulePayments Repostiory. Instantiate via UnitOfWork object
    /// </summary>
    public sealed class SchedulePaymentsRepository:RepositoryBase<SchedulePayment>
    {
        internal SchedulePaymentsRepository(JsaEntities context)
            : base(context)
        {
        }

        #region Overrides of BaseRepository<SchedulePayment>

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(SchedulePayment entity)
        {
            Context.SchedulePayments.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(SchedulePayment entity)
        {
            Context.SchedulePayments.Remove(entity);
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public override SchedulePayment GetById(object id)
        {
            int i = (int) id;
            return Query(x => x.ContractNo == i).Single();
        }

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override IQueryable<SchedulePayment> Query(Expression<Func<SchedulePayment, bool>> filter)
        {
            return Context.SchedulePayments.Where(filter);
        }

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public override IList<SchedulePayment> GetAll()
        {
            return Context.SchedulePayments.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}