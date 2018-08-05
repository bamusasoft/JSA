using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Jsa.DomainModel.Repositories;

namespace Jsa.DomainModel
{
    /// <summary>
    /// Represents unit of business transactions grouped together
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {


        /// <summary>
        /// Gets the customers.
        /// </summary>
        RepositoryBase<Customer> Customers { get; }

        /// <summary>
        /// Get the contracts.
        /// </summary>
        RepositoryBase<Contract> Contracts { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        RepositoryBase<Property> Properties { get; }

        /// <summary>
        /// Gets the schedule details.
        /// </summary>
        RepositoryBase<ScheduleDetail> ScheduleDetails { get; }

        /// <summary>
        /// Gets the schedules.
        /// </summary>
        RepositoryBase<Schedule> Schedules { get; }

        /// <summary>
        /// Gets the schedule payments.
        /// </summary>
        RepositoryBase<SchedulePayment> SchedulePayments { get; }

        /// <summary>
        /// Gets the contracts activities.
        /// </summary>
        RepositoryBase<ContractsActivity> ContractsActivities { get; }



        /// <summary>
        /// Gets the signers.
        /// </summary>
        RepositoryBase<Signer> Signers { get; }

        /// <summary>
        /// Get the payments
        /// </summary>
        RepositoryBase<Payment> Payments { get; }

        /// <summary>
        /// Get the Claims
        /// </summary>
        RepositoryBase<Claim> Claims { get; }

        /// <summary>
        /// Get the ClaimDetails
        /// </summary>
        RepositoryBase<ClaimDetail> ClaimDetails { get; }
        RepositoryBase<Representative> Representatives { get; }
        RepositoryBase<Outbox> Outboxes { get; }

        RepositoryBase<LegalCase> LegalCases { get; }
        RepositoryBase<CaseStatus> CaseStatuses { get; }
        RepositoryBase<CaseAppointment> CaseAppointments { get; }
        RepositoryBase<CaseFollowing> CaseFollowings { get; }
        RepositoryBase<CustomerClass> CustomersClasses { get;}

        RepositoryBase<Destination> Destinations { get; }
        RepositoryBase<DocRecord> DocRecords { get; }
        RepositoryBase<DocRecordFollow> DocRecordFollows { get; }


        /// <summary>
        /// Saves all changes made to current unit of work.
        /// </summary>
        void Save();

        /// <summary>
        /// Occurs when [state changed].
        /// </summary>
        event EventHandler StateChanged;



    }
}