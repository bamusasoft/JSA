using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace Jsa.DomainModel
{
    public class JsaEntities : DbContext
    {
        private static string JsaConnection
        {
            get
            {
                string providerName = "System.Data.SqlClient";
                string serverName = GlobalConst.ServerName; //".";
                string databaseName = "JsaDb";
                // Initialize the connection string builder for the
                // underlying provider.
                
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                // Set the properties for the data source.
                sqlBuilder.DataSource = serverName;
                sqlBuilder.InitialCatalog = databaseName;
                //sqlBuilder.IntegratedSecurity = true;
                sqlBuilder.UserID = "abdullah";
                //sqlBuilder.UserID = "bamusa";
                sqlBuilder.Password = "12345678";
                sqlBuilder.MultipleActiveResultSets = true;
               sqlBuilder.ApplicationName = "EntityFramework";

                
                // Build the SqlConnection connection string.
                string providerString = sqlBuilder.ToString();

                // Initialize the EntityConnectionStringBuilder.
                SqlConnection sqlConnection = new SqlConnection(providerString);
                
                #region "Entity connection when using EF designer model"
                //EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                ////Set the provider name.
                //entityBuilder.Provider = providerName;
                //// Set the provider-specific connection string.
                //entityBuilder.ProviderConnectionString = providerString;

                //// Set the Metadata location.
                //entityBuilder.Metadata = @"res://*/DbModel.JsaDbModel.csdl|res://*/DbModel.JsaDbModel.ssdl|res://*/DbModel.JsaDbModel.msl";

                ////Console.WriteLine(entityBuilder.ToString());
                //var s = entityBuilder.ToString();
                //return s;
                #endregion

                var  connection =  sqlConnection.ConnectionString;
                return connection;

                //string s = @"<add name="JsaDb" connectionString="data source=GHALIB-PC;initial catalog=JsaDb;persist security info=True;user id=abdullah;password=12345678;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />"
            }
        }

        public JsaEntities()
            : base(JsaConnection)
        {
            //: base("name=JsaDbEntities")
        }

        public virtual DbSet<CaseAppointment> CaseAppointments { get; set; }
        public virtual DbSet<CaseFollowing> CaseFollowings { get; set; }
        public virtual DbSet<CaseStatus> CaseStatuses { get; set; }
        public virtual DbSet<ClaimDetail> ClaimDetails { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractsActivity> ContractsActivities { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<LegalCase> LegalCases { get; set; }
        public virtual DbSet<Outbox> Outboxes { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Representative> Representatives { get; set; }
        public virtual DbSet<ScheduleDetail> ScheduleDetails { get; set; }
        public virtual DbSet<SchedulePayment> SchedulePayments { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Signer> Signers { get; set; }
        public virtual DbSet<CustomerClass> CustomersClasses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaseAppointment>()
                .Property(e => e.AppointmentDate)
                .IsFixedLength();

            modelBuilder.Entity<CaseFollowing>()
                .Property(e => e.FollowingDate)
                .IsFixedLength();

            modelBuilder.Entity<CaseFollowing>()
                .Property(e => e.NextFollowingDate)
                .IsFixedLength();

            modelBuilder.Entity<CaseStatus>()
                .HasMany(e => e.LegalCases)
                .WithRequired(e => e.CaseStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.ClaimId)
                .IsFixedLength();

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.Rent)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.Maintenance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.Deposit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.Others)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.Total)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.Paid)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.Balance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.OutstandingRentBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.OutstandingMaintBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ClaimDetail>()
                .Property(e => e.NetBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Claim>()
                .Property(e => e.ClaimId)
                .IsFixedLength();

            modelBuilder.Entity<Claim>()
                .Property(e => e.ClaimYear)
                .IsFixedLength();

            modelBuilder.Entity<Claim>()
                .Property(e => e.GrandTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Claim>()
                .Property(e => e.CreationDate)
                .IsFixedLength();

            modelBuilder.Entity<Claim>()
                .HasMany(e => e.ClaimDetails)
                .WithRequired(e => e.Claim)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.StartDate)
                .IsFixedLength();

            modelBuilder.Entity<Contract>()
                .Property(e => e.EndDate)
                .IsFixedLength();

            modelBuilder.Entity<Contract>()
                .Property(e => e.SignHijriDate)
                .IsFixedLength();

            modelBuilder.Entity<Contract>()
                .Property(e => e.SignGregDate)
                .IsFixedLength();

            modelBuilder.Entity<Contract>()
                .Property(e => e.PhotoPath)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .HasMany(e => e.ScheduleDetails)
                .WithRequired(e => e.Contract)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractsActivity>()
                .HasMany(e => e.Contracts)
                .WithOptional(e => e.ContractsActivity)
                .HasForeignKey(e => e.ActivityId);

            modelBuilder.Entity<Customer>()
                .Property(e => e.IdNumber)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.IdDate)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.MainMobile)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.SecondMobile)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.HomePhone)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.WorkPhone)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.Fax)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Claims)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Representatives)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Schedules)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LegalCase>()
                .Property(e => e.RegisteredAt)
                .IsFixedLength();

            modelBuilder.Entity<LegalCase>()
                .HasMany(e => e.CaseAppointments)
                .WithRequired(e => e.LegalCase)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LegalCase>()
                .HasMany(e => e.CaseFollowings)
                .WithRequired(e => e.LegalCase)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Outbox>()
                .Property(e => e.OutboxNo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Outbox>()
                .Property(e => e.OutboxDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Outbox>()
                .Property(e => e.Subject)
                .IsUnicode(false);

            modelBuilder.Entity<Outbox>()
                .Property(e => e.GoingTo)
                .IsUnicode(false);

            modelBuilder.Entity<Outbox>()
                .Property(e => e.AttachmentNo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Outbox>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.AccountNo)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.PayDate)
                .IsFixedLength();

            modelBuilder.Entity<Property>()
                .HasMany(e => e.ClaimDetails)
                .WithRequired(e => e.Property)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Property>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.Property)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Representative>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Representative>()
                .Property(e => e.IdDate)
                .IsFixedLength();

            modelBuilder.Entity<ScheduleDetail>()
                .Property(e => e.ScheduleId)
                .IsFixedLength();

            modelBuilder.Entity<ScheduleDetail>()
                .Property(e => e.DateDue)
                .IsFixedLength();

            modelBuilder.Entity<Schedule>()
                .Property(e => e.ScheduleId)
                .IsFixedLength();

            modelBuilder.Entity<Schedule>()
                .Property(e => e.ScheduleDate)
                .IsFixedLength();

            modelBuilder.Entity<Schedule>()
                .Property(e => e.SignerId)
                .IsFixedLength();

            modelBuilder.Entity<Schedule>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<Schedule>()
                .HasMany(e => e.ScheduleDetails)
                .WithRequired(e => e.Schedule)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Signer>()
                .Property(e => e.SignerId)
                .IsFixedLength();

            modelBuilder.Entity<Signer>()
                .Property(e => e.IdDate)
                .IsFixedLength();

            modelBuilder.Entity<Signer>()
                .Property(e => e.IdIssue)
                .IsUnicode(false);

            modelBuilder.Entity<Signer>()
                .Property(e => e.Mobile)
                .IsFixedLength();

            modelBuilder.Entity<Signer>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<Signer>()
                .HasMany(e => e.Schedules)
                .WithRequired(e => e.Signer)
                .WillCascadeOnDelete(false);
           
        }

    }
}
