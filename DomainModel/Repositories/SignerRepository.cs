using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represent Signer Repostiory. Instantiate via UnitOfWork object
    /// </summary>
    public sealed class SignerRepository : RepositoryBase<Signer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignerRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        internal SignerRepository(JsaEntities context)
            : base(context)
        {
        }

        #region Overrides of BaseRepository<Signer>

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(Signer entity)
        {
            Context.Signers.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(Signer entity)
        {
            Context.Signers.Remove(entity);
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public override Signer GetById(object id)
        {
            if (id == null) throw new ArgumentNullException("id");
            string idNo = (string)id;
            return Context.Signers.Single(x => x.SignerId == idNo);
        }

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override IQueryable<Signer> Query(Expression<Func<Signer, bool>> filter)
        {
            return Context.Signers.Where(filter);
        }

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public override IList<Signer> GetAll()
        {
            return Context.Signers.ToList();
        }

        public override void Refresh()
        {
            
        }

        #endregion

        public bool Exist(Signer signer)
        {
            return Context.Signers.Any
                (
                 si => si.SignerId.Equals(signer.SignerId)
                );
        }
    }
}