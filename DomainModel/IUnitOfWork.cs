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
        RepositoryBase<CustomerClass> CustomersClasses { get; }

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

        //
        // Summary:
        //     Creates a raw SQL query that will return elements of the given generic type.
        //     The type can be any type that has properties that match the names of the columns
        //     returned from the query, or can be a simple primitive type. The type does not
        //     have to be an entity type. The results of this query are never tracked by the
        //     context even if the type of object returned is an entity type. Use the System.Data.Entity.DbSet`1.SqlQuery(System.String,System.Object[])
        //     method to return entities that are tracked by the context. As with any API that
        //     accepts SQL it is important to parameterize any user input to protect against
        //     a SQL injection attack. You can include parameter place holders in the SQL query
        //     string and then supply parameter values as additional arguments. Any parameter
        //     values you supply will automatically be converted to a DbParameter. context.Database.SqlQuery<Post>("SELECT
        //     * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); Alternatively, you
        //     can also construct a DbParameter and supply it to SqlQuery. This allows you to
        //     use named parameters in the SQL query string. context.Database.SqlQuery<Post>("SELECT
        //     * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        //
        // Parameters:
        //   sql:
        //     The SQL query string.
        //
        //   parameters:
        //     The parameters to apply to the SQL query string. If output parameters are used,
        //     their values will not be available until the results have been read completely.
        //     This is due to the underlying behavior of DbDataReader, see http://go.microsoft.com/fwlink/?LinkID=398589
        //     for more details.
        //
        // Type parameters:
        //   TElement:
        //     The type of object returned by the query.
        //
        // Returns:
        //     A System.Data.Entity.Infrastructure.DbRawSqlQuery`1 object that will execute
        //     the query when it is enumerated.
        DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters);




    }
}