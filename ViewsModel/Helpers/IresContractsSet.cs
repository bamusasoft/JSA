using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace Jsa.ViewsModel.Helpers
{
    public class IresContractsSet:HashSet<IresContract>
    {
        private IresContractsSet _contracts;
        public event EventHandler<ProgressEventArgs<IresContract>> WritingProgress;
        public IresContractsSet()
        {
            
        }
        public IresContractsSet ReadContracts(string excelFilePath)
        {
            if (string.IsNullOrEmpty(excelFilePath))
            {
                throw new ArgumentNullException("excelFilePath");
            }

            Excel excel = new Excel(excelFilePath);
            DataTable dataTable = excel.ReadContracts();
            TrasnformContracts(dataTable);
            return this;

        }

        private void TrasnformContracts(DataTable dataTable)
        {
             foreach (DataRow contract in dataTable.Rows)
            {
                this.Add(ContractFromRow(contract));
            }
        }

        private IresContract ContractFromRow(DataRow contract)
        {
            TempContract c = new TempContract()
            {
                ContractNo = contract["ContractNo"] ,
                Renewal = contract["Renewal"]  ,
                StartDate = contract["StartDate"],
                EndDate = contract["EndDate"],
                Type = contract["Type"],
                Customer = contract["Customer"],
                AgreedRent = contract["AgreedRent"],
                Property = contract["Property"],
                NoPayments = contract["NoPayments"],
                Rent = contract["Rent"],
                Deposit = contract["Deposit"],
                Maintenance = contract["Maintenance"],
                Others = contract["Others"],
                Closed = contract["Closed"],
                RentBal = contract["RentBal"],
                DepositBal = contract["DepositBal"],
                MaintBal = contract["MaintBal"],
                Desc_Others = contract["Desc_Others"],
                Remarks = contract["Remarks"],
                Posted = contract["Posted"]
            };
            return new IresContract()
            {
                ContractNo = Convert.ToInt32(c.ContractNo),
                Renewal = Convert.ToInt32(c.Renewal),
                StartDate = Convert.ToString(c.StartDate),
                EndDate = Convert.ToString(c.EndDate),
                Type = Convert.ToInt32(c.Type),
                Customer = Convert.ToInt32(c.Customer),
                AgreedRent = Convert.ToInt32(c.AgreedRent),
                Property = Convert.ToString(c.Property),
                NoPayments = Convert.ToInt32(c.NoPayments),
                Rent = Convert.ToInt32(c.Rent),
                Deposit = Convert.ToInt32(c.Deposit),
                Maintenance = Convert.ToInt32(c.Maintenance),
                Others = Convert.ToInt32(c.Others),
                Closed = Convert.ToBoolean(c.Closed),
                RentBal = Convert.ToInt32(c.Rent),
                DepositBal = Convert.ToInt32(c.DepositBal),
                MaintBal = Convert.ToInt32(c.MaintBal),
                Desc_Others = Convert.ToString(c.Desc_Others),
                Remarks = Convert.ToString(c.Remarks),
                Posted = Convert.ToBoolean(c.Posted)
            };

        }
        public async Task<bool> WriteContractsAsync(string accessFilePath)
        {
            if (string.IsNullOrEmpty(accessFilePath)) throw new ArgumentNullException("accessFilePath");
            bool succeed = false;
            Access acc = new Access(accessFilePath);
            acc.ProgressNotification += OnWritingProgressed;
            succeed = await  acc.WriteContractsAsync(this);
           return succeed;
        }

        void OnWritingProgressed(object sender, ProgressEventArgs<IresContract> e)
        {
            if (WritingProgress != null)
            {
                WritingProgress(this, new ProgressEventArgs<IresContract>(e.Progress, e.Entity));
            } 
        }
    }
}
