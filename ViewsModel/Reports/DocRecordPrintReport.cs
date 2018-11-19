using Jsa.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public class DocRecordPrintReport : ExcelReportBase<DocRecordsReport>
    {
        public DocRecordPrintReport(List<DocRecordsReport> source, string excelTemplatePath, ExcelProperties excelProperties)
           : base(excelTemplatePath, excelProperties)
        {
            Data = CreateReport(source);
        }

        protected sealed override DataTable CreateReport(List<DocRecordsReport> source)
        {
            DataTable table = CreateTable();
            AddColumns(table);

            foreach (var docRecord in source)
            {
               AddRow(table, docRecord);
            }
            return table;
        }

        protected override DataTable CreateTable()
        {
            return new DataTable("DocRecordsReport");
        }

        protected override void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("Id");
            table.Columns.Add(c1);
            DataColumn c2 = new DataColumn("RefId");
            table.Columns.Add(c2);
            DataColumn c3 = new DataColumn("DocDate");
            table.Columns.Add(c3);
            DataColumn c4 = new DataColumn("Destination");
            table.Columns.Add(c4);
            DataColumn c5 = new DataColumn("Subject");
            table.Columns.Add(c5);
            DataColumn c6 = new DataColumn("FollowContent");
            table.Columns.Add(c6);
            DataColumn c7 = new DataColumn("FollowDate");
            table.Columns.Add(c7);
            DataColumn c8 = new DataColumn("Status");
            table.Columns.Add(c8);
        }

        protected override void AddRow(DataTable table, DocRecordsReport data)
        {
            DataRow row = table.NewRow();
            row.SetField("Id", data.Id);
            row.SetField("RefId", data.RefId);
            row.SetField("DocDate", Helper.PutMask(data.DocDate));
            row.SetField("Destination", data.Destination);
            row.SetField("Subject", data.Subject);
            row.SetField("FollowContent", data.FollowContent);
            row.SetField("FollowDate", Helper.PutMask(data.FollowDate));
            row.SetField("Status", data.StatusArabic);
            table.Rows.Add(row);
            row.AcceptChanges();
        }
       
        public override void Print()
        {
            base.Print();
        }

        
    }
}
