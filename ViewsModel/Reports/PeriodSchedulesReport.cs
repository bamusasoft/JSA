using System;
using System.Collections.Generic;
using System.Data;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.Reports
{
    public class PeriodSchedulesReport
    {
        public string ScheduleId { get; private set; }
        public string CustomerName { get; private  set; }

        public string PropertyNo { get; private set; }
        public string PropertyDescription { get; private set; }

        public int AmountDue { get; private set; }
        public string DateDue { get; private set; }
        public int AmountPaid { get; private set; }
        public int Balance { get; private set; }
        public string SignerId { get; private set; }
        public string SignerName { get; private set; }
        public string SignerMobile { get; private set; }
        public bool HadPaid { get; private set; }
        private PeriodSchedulesReport(string scheduleId, string customerName, string propertyNo, string propertyDesc, int amountDue, string dateDue, int amountPaid, int balance, string signerId, string signerName, string signerMobile, bool hadPaid)
        {
            this.ScheduleId = scheduleId;
            this.CustomerName = customerName;
            this.PropertyNo = propertyNo;
            this.PropertyDescription = propertyDesc;
            this.AmountDue = amountDue;
            this.DateDue = dateDue;
            this.AmountPaid = amountPaid;
            this.Balance = balance;
            this.SignerId = signerId;
            this.SignerName = signerName;
            this.SignerMobile = signerMobile;
            this.HadPaid = hadPaid;


        }
        public static DataTable CreateReport(IList<PeriodSchedule> periodSchedules, Tuple<int, int, int> reportFooter )
        {
            if (periodSchedules == null) throw new ArgumentNullException("periodSchedules");

            List<PeriodSchedulesReport> report = new List<PeriodSchedulesReport>();
            foreach (var periodSchedule in periodSchedules)
            {
                var r = new PeriodSchedulesReport
                    (
                     periodSchedule.ScheduleId,
                     periodSchedule.CustomerName,
                     periodSchedule.PropertyNo,
                     periodSchedule.PropertyDescription,
                     periodSchedule.AmountDue,
                     Helper.ApplyDateMask(periodSchedule.DateDue),
                     periodSchedule.AmountPaid,
                     periodSchedule.Balance,
                     periodSchedule.SignerId,
                     periodSchedule.SignerName,
                     periodSchedule.SignerMobile,
                     periodSchedule.HadPaid

                    );
                report.Add(r);
            }
            report.Add
                (
                  new PeriodSchedulesReport
                ("", "الإجمالي", "","", reportFooter.Item1, "", reportFooter.Item2, reportFooter.Item3, "", "","", false));
            return FillData(report);
        }

        

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable("PeriodScheduleReport");
            AddColumns(table);
            return table;


        }
        private static void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("ScheduleId");
            table.Columns.Add(c1);
            DataColumn c2 = new DataColumn("CustomerName");
            table.Columns.Add(c2);

            DataColumn c3 = new DataColumn("PropertyNo");
            table.Columns.Add(c3);

            DataColumn c4 = new DataColumn("PropertyDescription");
            table.Columns.Add(c4);

            DataColumn c5 = new DataColumn("AmountDue");
            table.Columns.Add(c5);

            DataColumn c6 = new DataColumn("DateDue");
            table.Columns.Add(c6);

            DataColumn c7 = new DataColumn("AmountPaid");
            table.Columns.Add(c7);

            DataColumn c8 = new DataColumn("Balance");
            table.Columns.Add(c8);

            //DataColumn c9 = new DataColumn("SignerId");
            //table.Columns.Add(c9);

            DataColumn c10 = new DataColumn("SignerName");
            table.Columns.Add(c10);

            DataColumn c11 = new DataColumn("SignerMobile");
            table.Columns.Add(c11);

            DataColumn c12 = new DataColumn("HadPaid");
            table.Columns.Add(c12);

        }

        private static DataTable FillData(IEnumerable<PeriodSchedulesReport> data)
        {
            DataTable table = CreateTable();
            foreach (var report in data)
            {
                DataRow row = table.NewRow();
                row.SetField<string>("ScheduleId", report.ScheduleId);
                row.SetField<string>("CustomerName", report.CustomerName);
                row.SetField<string>("PropertyNo", report.PropertyNo);
                row.SetField<string>("PropertyDescription", report.PropertyDescription);
                row.SetField<int>("AmountDue", report.AmountDue);
                row.SetField<string>("DateDue", report.DateDue);
                row.SetField<int>("AmountPaid", report.AmountPaid);
                row.SetField<int>("Balance", report.Balance);
                //row.SetField<string>("SignerId", report.SignerId);
                row.SetField<string>("SignerName", report.SignerName);
                row.SetField<string>("SignerMobile", report.SignerMobile);
                row.SetField<string>("HadPaid", Hadpaidictionary[ report.HadPaid]);
                table.Rows.Add(row);
                row.AcceptChanges();


            }
            return table;

        }
        private static readonly Dictionary<bool, string> Hadpaidictionary = 
            new Dictionary<bool, string>(){
                {true, "نعم"},
                {false, "لا"}
            };
        
    }
}
