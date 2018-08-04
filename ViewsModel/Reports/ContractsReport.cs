using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace Jsa.ViewsModel.Reports
{
    public class ContractsReport
    {


        public int ContractNo { get; set; }

        public int CustomerNo { get; set; }

        public string PropertyNo { get; set; }

        public string CustomerName { get; set; }

        public string PropertyDescription { get; set; }

        public string Location { get; set; }

        public int AgreedRent { get; set; }

        public int RentDue { get; set; }
        

        public int MaintenanaceDue { get; set; }

        public int DepositDue { get; set; }

        public int DueTotal
        {
            get { return (RentDue + MaintenanaceDue + DepositDue); }
        }

        public int RentPaid { get; set; }

        public int MaintenancePaid { get; set; }

        public int DepositPaid { get; set; }

        public int PaidTotal
        {
            get { return (RentPaid + MaintenancePaid + DepositPaid); }
            
        }

        public int Balance
        {
            get { return (DueTotal - PaidTotal); }
        }

        public int TotalSum { get; set; }

        public bool FooterRow { get; set; }

        public string ContactMobile { get; set; }

        public ContractsReport(int contractNo, int customerNo, string propertyNo, string customerName, 
            string propertyDescription, string location, int agreedRent,
            int rentDue, int maintDue, int depositDue,
            int rentPaid, int maintPaid, int depositPaid, bool footerRow = false)
        {
            ContractNo = contractNo;
            CustomerNo = customerNo;
            PropertyNo = propertyNo;
            CustomerName = customerName;
            PropertyDescription = propertyDescription;
            Location = location;
            AgreedRent = agreedRent;
            RentDue = rentDue;
            MaintenanaceDue = maintDue;
            DepositDue = depositDue;
            RentPaid = rentPaid;
            MaintenancePaid = maintPaid;
            DepositPaid = depositPaid;
            FooterRow = footerRow;

        }

        public ContractsReport(string caption, int agreedRentSum,
            int rentDueSum, int maintDueSum, int depositDueSum,
            int rentPaidSum, int maintPaidSum, int depositPaidSum, int totalSum):this(-1,-1,caption,"","","",
            agreedRentSum, rentDueSum, maintDueSum, depositDueSum, rentPaidSum, maintPaidSum, depositPaidSum, true)
        {
            TotalSum = totalSum;
        }
        public static DataTable CreateReport(IEnumerable<ContractsReport> contracts,  ContractsReportFooter footer)
        {
            if (contracts == null) throw new ArgumentNullException("contracts");
            List<ContractsReport> report = new List<ContractsReport>();
            foreach (var contract in contracts)
            {
                var r = new ContractsReport
                    (
                    contract.ContractNo, contract.CustomerNo, contract.PropertyNo, contract.CustomerName,
                    contract.PropertyDescription, contract.Location, contract.AgreedRent, contract.RentDue,
                    contract.MaintenanaceDue, contract.DepositDue, contract.RentPaid, contract.MaintenancePaid,
                    contract.DepositPaid);
                    
                report.Add(r);
            }
            if (footer != null)
            {
                report.Add(new ContractsReport("الإجمالي", footer.AgreedRentSum, footer.RentDueSum,
                    footer.MaintDueSum,
                    footer.DepositDueSum, footer.RentPaidSum, footer.MainPaidSum, footer.DepositPaidSum, footer.TotalSum));
            }
            return FillData(report);

        }
        private static DataTable CreateTable()
        {
            DataTable table = new DataTable("ContractsReport");
            AddColumns(table);
            return table;


        }
        private static void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("ContractNo");
            table.Columns.Add(c1);
            //
            DataColumn c2 = new DataColumn("CustomerId");
            table.Columns.Add(c2);
            //
            DataColumn c3 = new DataColumn("PropertyNo");
            table.Columns.Add(c3);
            //
            DataColumn c4 = new DataColumn("CustomerName");
            table.Columns.Add(c4);
            //
            DataColumn c5 = new DataColumn("PropertyDescription");
            table.Columns.Add(c5);
            //
            DataColumn c6 = new DataColumn("Location");
            table.Columns.Add(c6);
            //
            DataColumn c7 = new DataColumn("RentDue");
            table.Columns.Add(c7);
            //
            DataColumn c8 = new DataColumn("MaintDue");
            table.Columns.Add(c8);
            //
            DataColumn c9 = new DataColumn("DepositDue");
            table.Columns.Add(c9);
            //
            DataColumn c10 = new DataColumn("DueTotal");
            table.Columns.Add(c10);

            //
            DataColumn c11 = new DataColumn("RentPaid");
            table.Columns.Add(c11);
            //
            DataColumn c12 = new DataColumn("MaintPaid");
            table.Columns.Add(c12);
            //
            DataColumn c13 = new DataColumn("DepositPaid");
            table.Columns.Add(c13);
            //
            DataColumn c14 = new DataColumn("PaidTotal");
            table.Columns.Add(c14);
            //
            DataColumn c15 = new DataColumn("Balance");
            table.Columns.Add(c15);

        }

        private static DataTable FillData(List<ContractsReport> data)
        {
            DataTable table = CreateTable();
            foreach (var report in data)
            {
                DataRow row = table.NewRow();
                row.SetField<int>("ContractNo", report.ContractNo);
                row.SetField<int>("CustomerId", report.CustomerNo);
                row.SetField<string>("PropertyNo", report.PropertyNo);
                row.SetField<string>("CustomerName", report.CustomerName);
                row.SetField<string>("PropertyDescription", report.PropertyDescription);
                row.SetField<string>("Location", report.Location);
                row.SetField<int>("RentDue", report.RentDue);
                row.SetField<int>("MaintDue", report.MaintenanaceDue);
                row.SetField<int>("DepositDue", report.DepositDue);
                row.SetField<int>("DueTotal", report.DueTotal);
                row.SetField<int>("RentPaid", report.RentPaid);
                row.SetField<int>("MaintPaid", report.MaintenancePaid);
                row.SetField<int>("DepositPaid", report.DepositPaid);
                row.SetField<int>("PaidTotal", report.PaidTotal);
                row.SetField<int>("Balance", report.Balance);
                table.Rows.Add(row);
                row.AcceptChanges();
            }
            return table;
        }

     
  
    }

    public class ContractsReportFooter
    {
        public int MaintDueSum { get; set; }
        public int AgreedRentSum { get; set; }
        public int RentDueSum { get; set; }
        public int DepositDueSum { get; set; }
        public int RentPaidSum { get; set; }
        public int MainPaidSum { get; set; }
        public int DepositPaidSum { get; set; }
        public int TotalSum { get; set; }
    }
}
