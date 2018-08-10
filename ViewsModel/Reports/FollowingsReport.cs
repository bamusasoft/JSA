using System.Collections.Generic;
using System.Data;
using DataTable = System.Data.DataTable;

namespace Jsa.ViewsModel.Reports
{
    public class FollowingsReport:ExcelReportBase<FollowingReportFields>
    {
        private Header _header;
         public FollowingsReport(List<FollowingReportFields> source, int caseNo, string defendant, string courtAppointment, string excelTemplatePath, ExcelProperties excelProperties)
            : base(excelTemplatePath, excelProperties)
        {
            Data = CreateReport(source);
            CreateHeader(caseNo, defendant, courtAppointment);
        }
         
        protected override sealed DataTable CreateReport(List<FollowingReportFields> source)
        {
            DataTable table = CreateTable();
            AddColumns(table);
            source.ForEach((folowing) => AddRow(table, folowing));
            return table;
        }

        protected override DataTable CreateTable()
        {
            return new DataTable("FollowingsReport");
        }

        protected override void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("FollowingDate");
            table.Columns.Add(c1);
            DataColumn c2 = new DataColumn("Destination");
            table.Columns.Add(c2);
            DataColumn c3 = new DataColumn("Subject");
            table.Columns.Add(c3);
        }

        protected override void AddRow(DataTable table, FollowingReportFields data)
        {
            DataRow row = table.NewRow();
            row.SetField("FollowingDate", Helper.PutMask(data.FollowingDate));
            row.SetField("Destination", data.Destination);
            row.SetField("Subject", data.Subject);
            table.Rows.Add(row);
            row.AcceptChanges();
        }

        private void CreateHeader(int caseNo, string defendant, string nextAppointment)
        {
            _header =new Header(caseNo, defendant, nextAppointment);
        }

        public Header ReportHeader
        {
            get { return _header; }
            
        }
        public struct Header
        {
            public int CaseNo { get; private set; }
            public string Defenant { get; private set; }
            public string NextAppointment { get; private set; }

            public Header(int caseNo, string defendant, string nextAppoint)
                :this()
            {
                CaseNo = caseNo;
                Defenant = defendant;
                NextAppointment = nextAppoint;

            }
             
        }

        public override void Print()
        {
            PrintHeader();
            base.Print();
        }

        private void PrintHeader()
        {
            ExcelSheet.Cells[2, 1] = ReportHeader.CaseNo.ToString();
            ExcelSheet.Cells[2, 2] = Helper.PutMask(ReportHeader.NextAppointment);
            ExcelSheet.Cells[2, 3] = ReportHeader.Defenant;
        }
    }
}
