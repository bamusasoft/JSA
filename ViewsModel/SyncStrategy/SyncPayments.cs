using System;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jsa.DomainModel.Repositories;
using Jsa.WinIrsService;

namespace Jsa.ViewsModel.SyncStrategy
{
    public class SyncPayments : ISyncIrs
    {
        IUnitOfWork _unitOfWork;
        string _paymentYear;
        public SyncPayments(IUnitOfWork unitOfWork, string paymentYear)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            if (!Helper.ValidYear(paymentYear)) throw new InvalidOperationException("Invalid year");
            _unitOfWork = unitOfWork;
            _paymentYear = paymentYear;

        }
        public Task SyncAsync()
        {
            Task tsk = Task.Run(
                    () =>
                    {
                        string paysYear = _paymentYear.Substring(2, 2);
                        RepositoryBase<Payment> paymentRepository = _unitOfWork.Payments;
                        var sgePayments = paymentRepository.GetAll();
                        var irsPayments = new PaymentTableAdapter(Properties.Settings.Default.IrsDbPath,
                                                                   paysYear).GetData();
                        double count = irsPayments.Count;
                        double current = 0.0;

                        foreach (Payment irsPayment in irsPayments)
                        {
                            bool exist = sgePayments.Any(p => p.PaymentNo == irsPayment.PaymentNo);
                            if (!exist) //If new Add it
                            {
                                Payment newSgePayment = new Payment()
                                {
                                    PaymentNo = irsPayment.PaymentNo,
                                    AccountNo = irsPayment.AccountNo,
                                    ContractNo = irsPayment.ContractNo,
                                    Renewal = irsPayment.Renewal,
                                    Name = irsPayment.Name,
                                    PayDate = irsPayment.PayDate,
                                    PaymentType = irsPayment.PaymentType,
                                    PaymentFor1 = irsPayment.PaymentFor1,
                                    PaymentFor2 = irsPayment.PaymentFor2,
                                    TotalPayment = irsPayment.TotalPayment,
                                    Rent = irsPayment.Rent,
                                    Deposit = irsPayment.Deposit,
                                    Maintenance = irsPayment.Maintenance,
                                    Others = irsPayment.Others,
                                    Posted = irsPayment.Posted,
                                    DebitAccount = irsPayment.DebitAccount,
                                    PayCode = irsPayment.PayCode,
                                    Tax = irsPayment.Tax,

                                };
                                paymentRepository.Add(newSgePayment);
                            }
                            else //Otherwise udpate it.
                            {
                                var existingPayment = sgePayments.Single(p => p.PaymentNo == irsPayment.PaymentNo);

                                existingPayment.AccountNo = irsPayment.AccountNo;
                                existingPayment.ContractNo = irsPayment.ContractNo;
                                existingPayment.Renewal = irsPayment.Renewal;
                                existingPayment.Name = irsPayment.Name;
                                existingPayment.PayDate = irsPayment.PayDate;
                                existingPayment.PaymentType = irsPayment.PaymentType;
                                existingPayment.PaymentFor1 = irsPayment.PaymentFor1;
                                existingPayment.PaymentFor2 = irsPayment.PaymentFor2;
                                existingPayment.TotalPayment = irsPayment.TotalPayment;
                                existingPayment.Rent = irsPayment.Rent;
                                existingPayment.Deposit = irsPayment.Deposit;
                                existingPayment.Maintenance = irsPayment.Maintenance;
                                existingPayment.Others = irsPayment.Others;
                                existingPayment.Posted = irsPayment.Posted;
                                existingPayment.DebitAccount = irsPayment.DebitAccount;
                                existingPayment.PayCode = irsPayment.PayCode;
                                existingPayment.Tax = irsPayment.Tax;

                            }
                            current++;
                            double progress = (current / count) * 100;
                            RaiseProgress(progress);
                        }
                    }
                );
            return tsk;
        }

        public event EventHandler<ProgressEventArgs> ReportProgress;

        public void RaiseProgress(double progress)
        {
            if (ReportProgress != null)
            {
                ReportProgress(this, new ProgressEventArgs(progress));
            }
        }
    }
    public class SyncContracts : ISyncIrs
    {
        IUnitOfWork _unitOfWork;
        string _contractYear;
        public SyncContracts(IUnitOfWork unitOfWork, string contractYear)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            if (!Helper.ValidYear(contractYear)) throw new InvalidOperationException("Invalid year");
            _unitOfWork = unitOfWork;
            _contractYear = contractYear;

        }
        public Task SyncAsync()
        {
            Task tsk = Task.Run(() =>
                {
                    string contsYear = _contractYear.Substring(2, 2);
                    RepositoryBase<Contract> contractRepository = _unitOfWork.Contracts;
                    var sgeContracts = contractRepository.GetAll();
                    var irsContracts = new ContractTableAdapter(Properties.Settings.Default.IrsDbPath,
                                                                 contsYear).GetData();
                    double count = irsContracts.Count;
                    double current = 0.0;

                    foreach (Contract irsContract in irsContracts)
                    {

                        bool exist = sgeContracts.Any(contr => contr.ContractNo == irsContract.ContractNo);
                        if (!exist) //If new Add it
                        {
                            Contract newSgeContract = new Contract()
                            {
                                ContractNo = irsContract.ContractNo,
                                StartDate = irsContract.StartDate,
                                EndDate = irsContract.EndDate,
                                CustomerId = irsContract.CustomerId,
                                PropertyNo = irsContract.PropertyNo,
                                Closed = irsContract.Closed,
                                AgreedRent = irsContract.AgreedRent,
                                RentDue = irsContract.RentDue,
                                AgreedDeposit = irsContract.AgreedDeposit,
                                AgreedMaintenance = irsContract.AgreedMaintenance,
                                RentBalance = irsContract.RentBalance,
                                MaintenanceBalance = irsContract.MaintenanceBalance,
                                DepositBalance = irsContract.DepositBalance,
                                Total =
                                    irsContract.RentBalance + irsContract.MaintenanceBalance +
                                    irsContract.DepositBalance,
                                //Set scheduled to false initially
                                Scheduled = false,
                                Tax = irsContract.Tax,
                            };
                            contractRepository.Add(newSgeContract);
                        }
                        else //Otherwise udpate it.
                        {
                            var existingContract = sgeContracts.Single(x => x.ContractNo == irsContract.ContractNo);

                            existingContract.StartDate = irsContract.StartDate;
                            existingContract.EndDate = irsContract.EndDate;
                            existingContract.CustomerId = irsContract.CustomerId;
                            existingContract.PropertyNo = irsContract.PropertyNo;
                            existingContract.Closed = irsContract.Closed;
                            existingContract.AgreedRent = irsContract.AgreedRent;
                            existingContract.RentDue = irsContract.RentDue;
                            existingContract.AgreedDeposit = irsContract.AgreedDeposit;
                            existingContract.AgreedMaintenance = irsContract.AgreedMaintenance;
                            existingContract.RentBalance = irsContract.RentBalance;
                            existingContract.MaintenanceBalance = irsContract.MaintenanceBalance;
                            existingContract.DepositBalance = irsContract.DepositBalance;
                            existingContract.Total = irsContract.RentBalance + irsContract.MaintenanceBalance +
                                                     irsContract.DepositBalance;
                            existingContract.Closed = irsContract.Closed;

                            bool contractHadScheduled = existingContract.ScheduleDetails.Count > 0;
                            existingContract.Scheduled = contractHadScheduled;
                            existingContract.Tax = irsContract.Tax;

                        }
                        current++;
                        double progress = (current / count) * 100;
                        RaiseProgress(progress);
                    }
                    //Now determine the closed contracts in IRS db and update their counterparts in SGE db.
                    //foreach (var sgeContract in sgeContracts)
                    //{
                    //    //From SGE to IRS, same process as above but the other way round.
                    //    bool exist = irsContracts.Any(contr => contr.ContractNo == sgeContract.ContractNo);
                    //    if (!exist) //Only if the contract is missing from data coming from IRS db, that is an evidence for closed contract.
                    //    {
                    //        sgeContract.Closed = true;
                    //    }


                    //}

                });
            return tsk;

        }

        public event EventHandler<ProgressEventArgs> ReportProgress;

        public void RaiseProgress(double progress)
        {
            if (ReportProgress != null)
            {
                ReportProgress(this, new ProgressEventArgs(progress));
            }
        }
    }

    public class SyncCustomers : ISyncIrs
    {
        IUnitOfWork _unitOfWork;

        public SyncCustomers(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _unitOfWork = unitOfWork;
        }
        public Task SyncAsync()
        {
            Task ta = Task.Run(() =>
                {
                    RepositoryBase<Customer> customerRepository = _unitOfWork.Customers;
                    IList<Customer> sgeCustomers = customerRepository.GetAll();
                    IList<Customer> irsCustomers = new CustomerTableAdapter(Properties.Settings.Default.IrsDbPath).GetData();
                    double count = irsCustomers.Count;
                    double current = 0.0;

                    foreach (Customer irsCustomer in irsCustomers)
                    {
                        bool exist = sgeCustomers.Any(cus => cus.CustomerId == irsCustomer.CustomerId);
                        if (!exist) //If new Add it
                        {
                            //sgeCustomers.AddObject(irsCustomer);
                            var newSgeCustomer = new Customer
                            {
                                CustomerId = irsCustomer.CustomerId,
                                Name = irsCustomer.Name
                            };
                            customerRepository.Add(newSgeCustomer);
                        }
                        else //Otherwise udpate it.
                        {
                            Customer existingCustomer =
                                sgeCustomers.Single(x => x.CustomerId == irsCustomer.CustomerId);
                            existingCustomer.Name = irsCustomer.Name;
                        }
                        current++;
                        double progress = (current / count) * 100;
                        RaiseProgress(progress);
                    }
                }
                );
            return ta;

        }

        public event EventHandler<ProgressEventArgs> ReportProgress;

        public void RaiseProgress(double progress)
        {
            if (ReportProgress != null)
            {
                ReportProgress(this, new ProgressEventArgs(progress));
            }
        }
    }

    public class SyncProperties : ISyncIrs
    {
        IUnitOfWork _unitOfWork;

        public SyncProperties(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _unitOfWork = unitOfWork;
        }

        public Task SyncAsync()
        {
            Task tsk = Task.Run(() =>
                {
                    RepositoryBase<Property> propertyRepository = _unitOfWork.Properties;
                    var sgeProperties = propertyRepository.GetAll();
                    var irsProperties = new PropertiesTableAdpater(Properties.Settings.Default.IrsDbPath).GetData();
                    double count = irsProperties.Count;
                    double current = 0.0;

                    foreach (Property irsProperty in irsProperties)
                    {
                        bool exist = sgeProperties.Any(p => p.PropertyNo == irsProperty.PropertyNo);
                        if (!exist) //If new Add it
                        {
                            Property newSgePropety = new Property()
                            {
                                PropertyNo = irsProperty.PropertyNo,
                                Type = irsProperty.Type,
                                Location = irsProperty.Location,
                                Description = irsProperty.Description
                            };
                            propertyRepository.Add(newSgePropety);
                        }
                        else //Otherwise udpate it.
                        {
                            var existingProperty = sgeProperties.Single(p => p.PropertyNo == irsProperty.PropertyNo);
                            existingProperty.Type = irsProperty.Type;
                            existingProperty.Location = irsProperty.Location;
                            existingProperty.Description = irsProperty.Description;
                        }
                        current++;
                        double progress = (current / count) * 100;
                        RaiseProgress(progress);
                    }
                });
            return tsk;

        }

        public event EventHandler<ProgressEventArgs> ReportProgress;

        public void RaiseProgress(double progress)
        {
            if (ReportProgress != null)
            {
                ReportProgress(this, new ProgressEventArgs(progress));
            }
        }
    }

    public class SyncSchedulesPayments : ISyncIrs
    {
        IUnitOfWork _unitOfWork;
        string _paymentYear;
        public SyncSchedulesPayments(IUnitOfWork unitOfWork, string paymentYear)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            if (!Helper.ValidYear(paymentYear)) throw new InvalidOperationException("Invalid year");
            _unitOfWork = unitOfWork;
            _paymentYear = paymentYear;

        }

        public Task SyncAsync()
        {
            Task tsk = Task.Run(() =>
            {
                string paymentYear = _paymentYear.Substring(2, 2);
                RepositoryBase<SchedulePayment> schedulePaymentsRepository = _unitOfWork.SchedulePayments;
                RepositoryBase<ScheduleDetail> scheduleDetailRepository = _unitOfWork.ScheduleDetails;
                var jsaSchedulePayments = schedulePaymentsRepository.GetAll();
                var allSchedulesDetail = scheduleDetailRepository.GetAll();

                var irsPayments = new SchedulePaymentsTableAdapter(Properties.Settings.Default.IrsDbPath,
                                                                   paymentYear).GetData();
                double count = irsPayments.Count;
                double current = 0.0;

                foreach (SchedulePayment irsPayment in irsPayments)
                {
                    bool exist = jsaSchedulePayments.Any(pay => pay.ContractNo == irsPayment.ContractNo);
                    double progress = 0.0;
                    if (exist)
                    {
                        var existingJsaSchedulePayment = jsaSchedulePayments.Single(pay => pay.ContractNo == irsPayment.ContractNo);

                        int totalPaymentInIrs = irsPayment.TotalPayment; // Get The total payments From IRS

                        if (totalPaymentInIrs == existingJsaSchedulePayment.UnscheduledPayment) //Means no payments paid yet for the schedule.
                        {
                            current++;
                            progress = (current / count) * 100;
                            RaiseProgress(progress);

                            continue;

                        }
                        if (totalPaymentInIrs > existingJsaSchedulePayment.UnscheduledPayment)
                        {

                            int unscheduledPaymentInJsa = existingJsaSchedulePayment.UnscheduledPayment;
                            // Get payments that is already registred in the RPS, which represents the amount befor making the schedule
                            int amountPaidForSchedule = totalPaymentInIrs - unscheduledPaymentInJsa; //This is the net amount that is paid for our schedule

                            var contractScheduleDetails =
                                allSchedulesDetail.Where(x => x.ContractNo == existingJsaSchedulePayment.ContractNo).ToList();
                            PayScheduledDetails(contractScheduleDetails, amountPaidForSchedule);
                        }

                    }
                    current++;
                    progress = (current / count) * 100;
                    RaiseProgress(progress);

                }
            });
            return tsk;
        }

        public event EventHandler<ProgressEventArgs> ReportProgress;

        public void RaiseProgress(double progress)
        {
            if (ReportProgress != null)
            {
                ReportProgress(this, new ProgressEventArgs(progress));
            }
        }

        #region "Helpers"
        private bool HadScheduled(int contractNo)
        {
            var conts = _unitOfWork.Contracts.Query(x => x.ContractNo == contractNo);
            if (conts.Count() > 0)
            {
                var cont = conts.Single();
                return cont.Scheduled;
            }
            return false;
            //var result = _unitOfWork.Schedules.Query(x => x.ContractNo == contractNo).ToArray();
            //return result.Any();
        }

        private void PayScheduledDetails(List<ScheduleDetail> details, int paymentsTotal)
        {
            int detailsSum = details.Sum(x => x.AmountDue);
            //If details sum = payemnts total then all the schedule details had already paid. Just pay Pay them.
            if (detailsSum == paymentsTotal)
            {
                foreach (var scheduleDetail in details)
                {
                    scheduleDetail.AmountPaid = scheduleDetail.AmountDue;
                    scheduleDetail.Balance = 0;

                }
                return;
            }
            foreach (var scheduleDetail in details)
            {
                if (scheduleDetail.AmountDue == paymentsTotal)
                {
                    scheduleDetail.AmountPaid = paymentsTotal;
                    scheduleDetail.Balance = 0;
                    break;
                }
                if (scheduleDetail.AmountDue > paymentsTotal)
                {
                    scheduleDetail.AmountPaid = paymentsTotal;
                    scheduleDetail.Balance = (scheduleDetail.AmountDue - scheduleDetail.AmountPaid);
                    break;
                }
                if (scheduleDetail.AmountDue < paymentsTotal)
                {
                    scheduleDetail.AmountPaid = scheduleDetail.AmountDue;
                    scheduleDetail.Balance = 0;
                    paymentsTotal = (paymentsTotal - scheduleDetail.AmountDue);
                }
            }
        }

        #endregion

    }

}
