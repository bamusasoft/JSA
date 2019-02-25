using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Jsa.DomainModel.Exceptions;
using Jsa.DomainModel.Repositories;

namespace Jsa.DomainModel
{
    public class UnitOfWork : IUnitOfWork
    {

        #region Fields

        private JsaEntities _context;
        private RepositoryBase<Contract> _contracts;
        private RepositoryBase<Customer> _customers;
        private RepositoryBase<Property> _properties;
        private RepositoryBase<ScheduleDetail> _scheduleDetails;
        private RepositoryBase<SchedulePayment> _schedulePayments;
        private RepositoryBase<Schedule> _schedules;
        private RepositoryBase<Signer> _signer;
        private RepositoryBase<ContractsActivity> _contractsActivities;
        private RepositoryBase<Payment> _payments;
        private RepositoryBase<Claim> _claims;
        private RepositoryBase<ClaimDetail> _claimDetails;
        private RepositoryBase<Representative> _representatives;
        private RepositoryBase<Outbox> _outboxes;

        private RepositoryBase<LegalCase> _legalCases;
        private RepositoryBase<CaseAppointment> _caseAppointments;
        private RepositoryBase<CaseStatus> _caseStatuses;
        private RepositoryBase<CaseFollowing> _caseFollowings;
        private RepositoryBase<CustomerClass> _customersClasses;

        private RepositoryBase<Destination> _destinations;
        private RepositoryBase<DocRecord> _docRecords;
        private RepositoryBase<DocRecordFollow> _docRecordFollows;
        private RepositoryBase<DocRecordFile> _docRecordFiles;
        #endregion

        #region Contsturctor

        public UnitOfWork()
        {
            _context = new JsaEntities();
        }

        #endregion

        #region Implementation of IDisposable

        private bool _disposed;
       

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
            }
            _disposed = true;
        }

        #endregion

        #region Implementation of IUnitOfWork


        /// <summary>
        /// Gets the contracts.
        /// </summary>
        public RepositoryBase<Contract> Contracts
        {
            get { return _contracts ?? (_contracts = new ContractsRepository(_context)); }
        }

        /// <summary>
        /// Gets the customers.
        /// </summary>
        public RepositoryBase<Customer> Customers
        {
            get { return _customers ?? (_customers = new CustomersRepository(_context)); }
        }



        /// <summary>
        /// Gets the properties.
        /// </summary>
        public RepositoryBase<Property> Properties
        {
            get { return _properties ?? (_properties = new PropertiesRepository(_context)); }
        }

        /// <summary>
        /// Gets the schedule details.
        /// </summary>
        public RepositoryBase<ScheduleDetail> ScheduleDetails
        {
            get { return _scheduleDetails ?? (_scheduleDetails = new ScheduleDetailsRepository(_context)); }
        }

        /// <summary>
        /// Gets the schedules.
        /// </summary>
        public RepositoryBase<Schedule> Schedules
        {
            get { return _schedules ?? (_schedules = new SchedulesRepository(_context)); }
        }

        /// <summary>
        /// Gets the schedule payments.
        /// </summary>
        public RepositoryBase<SchedulePayment> SchedulePayments
        {
            get { return _schedulePayments ?? (_schedulePayments = new SchedulePaymentsRepository(_context)); }
        }

        /// <summary>
        /// Gets the signers.
        /// </summary>
        public RepositoryBase<Signer> Signers
        {
            get { return _signer ?? (_signer = new SignerRepository(_context)); }
        }

        /// <summary>
        /// Gets the signers.
        /// </summary>
        public RepositoryBase<ContractsActivity> ContractsActivities
        {
            get { return _contractsActivities ?? (_contractsActivities = new ActivitiesRepository(_context)); }
        }
        /// <summary>
        /// Get the payments
        /// </summary>
        public RepositoryBase<Payment> Payments
        {
            get { return _payments ?? (_payments = new PaymentRepository(_context)); }
        }

        /// <summary>
        /// Get the payments
        /// </summary>
        public RepositoryBase<Claim> Claims
        {
            get { return _claims ?? (_claims = new ClaimRepository(_context)); }
        }

        /// <summary>
        /// Get the payments
        /// </summary>
        public RepositoryBase<ClaimDetail> ClaimDetails
        {
            get { return _claimDetails ?? (_claimDetails = new ClaimDetailRepository(_context)); }
        }
        public RepositoryBase<Representative> Representatives
        {
            get { return _representatives ?? (_representatives = new RepresRepository(_context)); }

        }
        public RepositoryBase<Outbox> Outboxes
        {
            get { return _outboxes ?? (_outboxes = new OutboxRepository(_context)); }

        }

        public RepositoryBase<LegalCase> LegalCases
        {

            get { return _legalCases ?? (_legalCases = new LegalCaseRepository(_context)); }
        }

        public RepositoryBase<CaseStatus> CaseStatuses
        {
            get { return _caseStatuses ?? (_caseStatuses = new CaseStatusRepository(_context)); }
        }

        public RepositoryBase<CaseAppointment> CaseAppointments
        {
            get { return _caseAppointments ?? (_caseAppointments = new CaseAppointmentRepository(_context)); }
        }

        public RepositoryBase<CaseFollowing> CaseFollowings
        {
            get { return _caseFollowings ?? (_caseFollowings = new CaseFollowingRepository(_context)); }
        }

        public RepositoryBase<CustomerClass> CustomersClasses
        {
            get { return _customersClasses ?? (_customersClasses = new CustomersClassesRepository(_context)); }
        }

        public RepositoryBase<Destination> Destinations
        {
            get { return _destinations ?? (_destinations = new DestinationRepository(_context)); }
        }

        public RepositoryBase<DocRecord> DocRecords
        {
            get { return _docRecords ?? (_docRecords = new DocRecordRepository(_context)); }
        }

        public RepositoryBase<DocRecordFollow> DocRecordFollows
        {
            get { return _docRecordFollows ?? (_docRecordFollows = new DocRecordFollowRepository(_context)); }
        }

        public RepositoryBase<DocRecordFile> DocRecordFiles
        {
            get { return _docRecordFiles ?? (_docRecordFiles = new DocRecordFileRepository(_context)); }
        }
        /// <summary>
        /// Saves all changes made to current unit of work.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();

                RaiseStateChanges();
            }
            catch (Exception ex)
            {
                string msg = @"An error accured while saving data. See the following details:" + "\n";
                throw new DataModelException(msg, ex);
            }
        }

        public event EventHandler StateChanged;



        #endregion

        #region Helpers

        private void RaiseStateChanges()
        {
            EventHandler handler = StateChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void ResetRepositories()
        {

            _contracts = null;
            _customers = null;
            _properties = null;
            _scheduleDetails = null;
            _schedulePayments = null;
            _schedules = null;
            _signer = null;
        }

        public DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return _context.Database.SqlQuery<TElement>(sql, parameters);
        }

        #endregion


    }
}