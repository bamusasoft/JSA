using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.Reports
{

    public class RglReport
    {
        static DataTable _reportTable;

        private static DataTable ReportTable
        {
            get
            {
                if (_reportTable == null)
                {
                    _reportTable = CreateTable();
                }
                else
                {
                    _reportTable.Clear();
                }
                return _reportTable;
            }
        }
        public static ReportLayout Layout
        {
            get;
            private set;
        }


        public string ContratStart { get; private set; }
        public string ContractEnd { get; private set; }
        public string PropertyNo { get; private set; }
        public string CustomerName { get; private set; }
        public string PropertyDescription { get; private set; }
        public object AgreedRent { get; private set; }
        public object AgreedMaint { get; private set; }
        public object AgreedDeposit { get; private set; }
        public object AgreedTotal { get; private set; }
        public int RentPay { get; private set; }
        public int MaintPay { get; private set; }
        public int DepositPay { get; private set; }
        public int PaymentNo { get; private set; }
        public string PaymentDate { get; private set; }
        public object PaymentTotal { get; private set; }
        public object BalancesTotal { get; private set; }
        public bool HeaderRow { get; private set; }

        private RglReport(string contractStart, string contractEnd, string propNo, string custName, string propDesc,
                         object agreedRent,object agreedDeposit, object agreedMaint, object agreedTotal,
                         int rentPay, int maintPay, int depositPay, int paymentNo, string paymentDate,
                         object paymentTotal, object balancesTotal, bool headerRow)
        {
            ContratStart = contractStart;
            ContractEnd = contractEnd;
            CustomerName = custName;
            PropertyNo = propNo;
            PropertyDescription = propDesc;
            AgreedRent = agreedRent;
            AgreedMaint = agreedMaint;
            AgreedDeposit = agreedDeposit;
            AgreedTotal = agreedTotal;
            RentPay = rentPay;
            MaintPay = maintPay;
            DepositPay = depositPay;
            PaymentNo = paymentNo;
            PaymentDate = paymentDate;
            PaymentTotal = paymentTotal;
            BalancesTotal = balancesTotal;
            HeaderRow = headerRow;
        }
        private RglReport(int rentPay, int maintPay, int depositPay, int paymentNo, string paymentDate)
            : this(null, null, null, null, null, null, null,null, null, rentPay, maintPay, depositPay, paymentNo, paymentDate, null, null,false)
        {

        }


        private static DataTable CreateTable()
        {
            DataTable table = new DataTable("RGLReport");
            AddColumns(table);
            return table;


        }
        private static void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("ContractStart");
            table.Columns.Add(c1);

            DataColumn c2 = new DataColumn("ContractEnd");
            table.Columns.Add(c2);

            DataColumn c3 = new DataColumn("PropertyNo");
            table.Columns.Add(c3);


            DataColumn c4 = new DataColumn("CustomerName");
            table.Columns.Add(c4);

            DataColumn c5 = new DataColumn("PropertyDescription");
            table.Columns.Add(c5);

            DataColumn c6 = new DataColumn("AgreedRent");
            table.Columns.Add(c6);

            DataColumn c7 = new DataColumn("AgreedMaint");
            table.Columns.Add(c7);

            DataColumn c17 = new DataColumn("AgreedDeposit");
            table.Columns.Add(c17);

            DataColumn c8 = new DataColumn("AgreedTotal");
            table.Columns.Add(c8);

            DataColumn c9 = new DataColumn("RentPay");
            table.Columns.Add(c9);

            DataColumn c10 = new DataColumn("MaintPay");
            table.Columns.Add(c10);

            DataColumn c11 = new DataColumn("DepositPay");
            table.Columns.Add(c11);

            DataColumn c12 = new DataColumn("PaymentNo");
            table.Columns.Add(c12);

            DataColumn c13 = new DataColumn("PaymentDate");
            table.Columns.Add(c13);

            DataColumn c14 = new DataColumn("PaymentTotal");
            table.Columns.Add(c14);

            DataColumn c15 = new DataColumn("BalancesTotal");
            table.Columns.Add(c15);

            DataColumn c16 = new DataColumn("HeaderRow");
            table.Columns.Add(c16);

        }

        private static void AddRow(RglReport rowData, DataTable ownerTable)
        {
            DataRow row = ownerTable.NewRow();
            row.SetField<object>("ContractStart", Helper.ApplyDateMask(rowData.ContratStart));
            row.SetField<object>("ContractEnd", Helper.ApplyDateMask(rowData.ContractEnd));
            row.SetField<object>("PropertyNo", rowData.PropertyNo);
            row.SetField<object>("CustomerName", rowData.CustomerName);
            row.SetField<object>("PropertyDescription", rowData.PropertyDescription);
            row.SetField<object>("AgreedRent", rowData.AgreedRent);
            row.SetField<object>("AgreedMaint", rowData.AgreedMaint);
            row.SetField<object>("AgreedDeposit", rowData.AgreedDeposit);
            row.SetField<object>("AgreedTotal", rowData.AgreedTotal);
            row.SetField<object>("RentPay", rowData.RentPay);
            row.SetField<object>("MaintPay", rowData.MaintPay);
            row.SetField<object>("DepositPay", rowData.DepositPay);
            row.SetField<object>("PaymentNo", rowData.PaymentNo);
            row.SetField<object>("PaymentDate", Helper.ApplyDateMask(rowData.PaymentDate));
            row.SetField<object>("PaymentTotal", rowData.PaymentTotal);
            row.SetField<object>("BalancesTotal", rowData.BalancesTotal);
            row.SetField<bool>("HeaderRow", rowData.HeaderRow);
            ownerTable.Rows.Add(row);
            row.AcceptChanges();

        }
        public static DataTable BuildReport(IEnumerable<ContractPayments> contractsPayments)
        {
            if (contractsPayments == null) throw new ArgumentNullException("contractsPayments");
            DataTable reportTable = ReportTable;
            foreach (var cp in contractsPayments)
            {
                bool headerInfo = true;

                int agreedTotal = cp.DueTotals;// (cp.AgreedRent + cp.AgreedMaintenance + cp.AgreedDeposit);
                int paymentTotals = (cp.PaymentsDetails.Sum(x => x.Rent) + cp.PaymentsDetails.Sum(y => y.Maintenance) + cp.PaymentsDetails.Sum(d => d.Deposit));
                int agreedBalance = agreedTotal - paymentTotals;
                if (cp.PaymentsDetails.Count == 0) //This contract has no payments yet, so just display its contract info.
                {

                    RglReport noPaymentsHeaderRow = new RglReport(
                        cp.StartDate, cp.EndDate, cp.PropertyNo, cp.CustomerName, cp.PropertyLocation,
                        cp.RentDue, cp.DepositDue, cp.MentDue, agreedTotal,
                        0, 0, 0, 0, null, paymentTotals, agreedBalance,true);
                    AddRow(noPaymentsHeaderRow, reportTable);
                    continue;

                }
                foreach (var payment in cp.PaymentsDetails)
                {
                    if (headerInfo)
                    {
                        RglReport headerRow = new RglReport
                        (
                             cp.StartDate, cp.EndDate, cp.PropertyNo,
                             cp.CustomerName, cp.PropertyLocation,
                            cp.RentDue, cp.DepositDue, cp.MentDue, agreedTotal,
                             payment.Rent, payment.Maintenance, payment.Deposit, payment.PaymentNo,
                            payment.PaymentDate, paymentTotals, agreedBalance, true
                        );
                        AddRow(headerRow, reportTable);
                        headerInfo = false;
                    }
                    else
                    {
                        RglReport chiledRow = new RglReport(
                             payment.Rent, payment.Maintenance, payment.Deposit, payment.PaymentNo, payment.PaymentDate
                            );
                        AddRow(chiledRow, reportTable);
                    }
                }
            }
            Layout = new ReportLayout(true);
            return reportTable;
        }



    }
}
