using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Helpers
{
    public class Access
    {
        private readonly string _filePath;
        public event EventHandler<ProgressEventArgs<IresContract>> ProgressNotification;
        public Access(string filePath)
        {
            _filePath = filePath;
        }
        public async Task<bool> WriteContractsAsync(ICollection<IresContract> contracts)
        {
            bool b = await Task.Run(() =>
                 {
                     bool succeed = false;
                     double prog = 0.0;
                     double count = contracts.Count;
                     foreach (IresContract contract in contracts)
                     {
                         string connString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                             + "Data Source=" + _filePath;
                         using (OleDbConnection conn = new OleDbConnection(connString))
                         {
                             string insertStatment = "INSERT INTO Contracts "
                                 + "(ContractNo, Renewal, StartDate, EndDate, Type, Customer, AgreedRent, Property, NoPayments, Rent, Deposit, Maintenance, "
                                 + "Others, Desc_Others, Remarks, Closed, RentBal, DepositBal, MaintBal, OthersBal, Posted) "
                                 + "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?) ";
                             using (OleDbCommand insertCommand = new OleDbCommand(insertStatment, conn))
                             {
                                 insertCommand.Parameters.Add("ContractNo", OleDbType.Integer).Value = contract.ContractNo;
                                 insertCommand.Parameters.Add("Renewal", OleDbType.Integer).Value = contract.Renewal;
                                 insertCommand.Parameters.Add("StartDate", OleDbType.Char).Value = contract.StartDate;
                                 insertCommand.Parameters.Add("EndDate", OleDbType.Char).Value = contract.EndDate;
                                 insertCommand.Parameters.Add("Type", OleDbType.Integer).Value = contract.Type;
                                 insertCommand.Parameters.Add("Customer", OleDbType.Integer).Value = contract.Customer;
                                 insertCommand.Parameters.Add("AgreedRent", OleDbType.Integer).Value = contract.AgreedRent;
                                 insertCommand.Parameters.Add("Property", OleDbType.Char).Value = contract.Property;
                                 insertCommand.Parameters.Add("NoPayments", OleDbType.Integer).Value = contract.NoPayments;
                                 insertCommand.Parameters.Add("Rent", OleDbType.Integer).Value = contract.Rent;
                                 insertCommand.Parameters.Add("Deposit", OleDbType.Integer).Value = contract.Deposit;
                                 insertCommand.Parameters.Add("Maintenance", OleDbType.Integer).Value = contract.Maintenance;
                                 insertCommand.Parameters.Add("Others", OleDbType.Integer).Value = contract.Others;
                                 insertCommand.Parameters.Add("Desc_Others", OleDbType.Char).Value = contract.Desc_Others;
                                 insertCommand.Parameters.Add("Remarks", OleDbType.Char).Value = contract.Remarks;
                                 insertCommand.Parameters.Add("Closed", OleDbType.Boolean).Value = contract.Closed;
                                 insertCommand.Parameters.Add("RentBal", OleDbType.Integer).Value = contract.RentBal;
                                 insertCommand.Parameters.Add("DepositBal", OleDbType.Integer).Value = contract.DepositBal;
                                 insertCommand.Parameters.Add("MaintBal", OleDbType.Integer).Value = contract.MaintBal;
                                 insertCommand.Parameters.Add("OthersBal", OleDbType.Integer).Value = contract.OthersBal;
                                 insertCommand.Parameters.Add("Posted", OleDbType.Boolean).Value = contract.Posted;
                                 conn.Open();
                                 insertCommand.ExecuteNonQuery();

                             }

                         }
                         prog++;
                         double progSoFar = ((prog / count) * 100);
                         RaiseProgress(progSoFar, contract);
                     }
                     succeed = true;
                     return succeed;
                 });
            return b;

        }
        private void RaiseProgress(double prog, IresContract savedContract)
        {
            if (ProgressNotification != null)
            {
                ProgressNotification(this, new ProgressEventArgs<IresContract>(prog, savedContract));
            }
        }

    }
}
