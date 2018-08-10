using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represent Proeprties Repostiory. Instantiate via UnitOfWork object
    /// </summary>
    public sealed class PropertiesRepository:RepositoryBase<Property>
    {
        internal PropertiesRepository(JsaEntities context)
            : base(context)
        {
        }

        #region Overrides of BaseRepository<Property>

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(Property entity)
        {
            Context.Properties.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(Property entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public override Property GetById(object id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override IQueryable<Property> Query(Expression<Func<Property, bool>> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public override IList<Property> GetAll()
        {
            return Context.Properties.ToList();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}