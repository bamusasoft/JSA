using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represents base for all repositories.
    /// </summary>
    /// <typeparam name="T">T must be a class</typeparam>
    public abstract class RepositoryBase<T> where T:class 
    {
        protected JsaEntities Context { get; private set; }

        protected RepositoryBase(JsaEntities context)
        {
            if (context == null) throw new ArgumentNullException("context");
            Context = context;
        }

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public abstract void Add(T entity);

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public abstract void Delete(T entity);


        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public abstract T GetById(object id);

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public abstract IQueryable<T> Query(Expression<Func<T, bool>>  filter);

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public abstract IList<T> GetAll();

        /// <summary>
        /// Refreshes current repository from the data store.
        /// </summary>
        public abstract void Refresh();


    }
}
