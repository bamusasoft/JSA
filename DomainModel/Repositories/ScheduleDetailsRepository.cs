using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represent ScheduleDetails Repostiory. Instantiate via UnitOfWork object
    /// </summary>
    public sealed class ScheduleDetailsRepository:RepositoryBase<ScheduleDetail>
    {
        internal ScheduleDetailsRepository(JsaEntities context)
            : base(context)
        {
        }

        #region Overrides of BaseRepository<ScheduleDetail>

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(ScheduleDetail entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(ScheduleDetail entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public override ScheduleDetail GetById(object id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override IQueryable<ScheduleDetail> Query(Expression<Func<ScheduleDetail, bool>> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public override IList<ScheduleDetail> GetAll()
        {
            return Context.ScheduleDetails.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}