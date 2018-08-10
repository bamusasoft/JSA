using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Jsa.DomainModel.Repositories
{
    /// <summary>
    /// Represent Schedules Repostiory. Instantiate via UnitOfWork object
    /// </summary>
    public sealed class SchedulesRepository : RepositoryBase<Schedule>
    {
        internal SchedulesRepository(JsaEntities context)
            : base(context)
        {
        }

        #region Overrides of BaseRepository<SchedulesInfo>

        /// <summary>
        /// Adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(Schedule entity)
        {
            Context.Schedules.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(Schedule entity)
        {
            Context.Schedules.Remove(entity);
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public override Schedule GetById(object id)
        {
            string i = (string)id;
            return Query(sch => sch.ScheduleId == i).Single();
            
        }

        /// <summary>
        /// Query repository by specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public override IQueryable<Schedule> Query(Expression<Func<Schedule, bool>> filter)
        {
            return
                Context.Schedules
                    .Include(s => s.Customer)
                    .Include(s => s.Signer)
                    .Include(s => s.ScheduleDetails)
                    .Include(s => s.ScheduleDetails.Select(x => x.Contract))
                    .Include(s => s.ScheduleDetails.Select(n => n.Contract.Property))
                    .Where(filter);
        }

        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns></returns>
        public override IList<Schedule> GetAll()
        {
            return Context.Schedules.ToList();
        }

        public override void Refresh()
        {

                
        }

        #endregion

        #region "Repository Specifiec Members"
        public string MaxSchedule
        {
            get
            {
                return
                    (from schedule in Context.Schedules select schedule.ScheduleId).Max();
            }
        }
        public bool IsScheduledContract(Contract contract)
        {
            return Context.Schedules.Any(s => s.Customer.Contracts.Any(x=> x.ContractNo == contract.ContractNo));
        }
        public bool Exist(Schedule schedule)
        {
            return Context.Schedules.Any
                    (
                    sch => sch.ScheduleId.Equals(schedule.ScheduleId)
                    );

        }
        public string MaxScheduleNo()
        {
            return Context.Schedules.Max(x => x.ScheduleId);
        }
        #endregion

    }
}