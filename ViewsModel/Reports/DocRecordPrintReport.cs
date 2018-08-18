using Jsa.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public class DocRecordPrintReport : ExcelReportBase<DocFollowsReport>
    {
        public DocRecordPrintReport(List<DocFollowsReport> source, string excelTemplatePath, ExcelProperties excelProperties)
           : base(excelTemplatePath, excelProperties)
        {
            Data = CreateReport(source);
        }

        protected override sealed DataTable CreateReport(List<DocFollowsReport> source)
        {
            //source.OrderBy(x => x.Destination).ThenBy(p => p.FollowDate);
            DataTable table = CreateTable();
            AddColumns(table);

            string currentDocId = "";
            foreach (var docFollow in source)
            {
                if(string.IsNullOrEmpty(currentDocId) || currentDocId != docFollow.DocId)
                {
                    AddDocRow(table, docFollow);
                    AddFollowHeader(table);
                    AddFollowRow(table, docFollow);
                    currentDocId = docFollow.DocId;
                }
                else
                {
                    AddFollowRow(table, docFollow);
                }
            }
            return table;
        }

        protected override DataTable CreateTable()
        {
            return new DataTable("FollowingsReport");
        }

        protected override void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("DocId");
            table.Columns.Add(c1);
            DataColumn c2 = new DataColumn("DocRefId");
            table.Columns.Add(c2);
            DataColumn c3 = new DataColumn("DocDate");
            table.Columns.Add(c3);
            DataColumn c4 = new DataColumn("Destination");
            table.Columns.Add(c4);
            DataColumn c5 = new DataColumn("Subject");
            table.Columns.Add(c5);
            DataColumn c6 = new DataColumn("Status");
            table.Columns.Add(c6);
        }

        protected override void AddRow(DataTable table, DocFollowsReport data)
        {
            
        }
        private void AddDocRow(DataTable table, DocFollowsReport data)
        {
            DataRow row = table.NewRow();
            row.SetField("DocId", data.DocId);
            row.SetField("DocRefId", data.RefId);
            row.SetField("DocDate", Helper.PutMask(data.DocDate));
            row.SetField("Destination", data.Destination);
            row.SetField("Subject", data.Subject);
            row.SetField("Status", data.StatusArabic);
            table.Rows.Add(row);
            row.AcceptChanges();
        }
        private void AddFollowHeader(DataTable table)
        {
            DataRow row = table.NewRow();
            row.SetField("DocId", "");
            row.SetField("DocRefId", "");
            row.SetField("DocDate", "");
            row.SetField("Destination", "تاريخ المتابعة");
            row.SetField("Subject", "الإجراء");
            row.SetField("Status", "");
            table.Rows.Add(row);
            row.AcceptChanges();


        }
        private void AddFollowRow(DataTable table, DocFollowsReport data)
        {
            DataRow row = table.NewRow();
            row.SetField("DocId", "");
            row.SetField("DocRefId", "");
            row.SetField("DocDate", "");
            row.SetField("Destination", Helper.PutMask(data.FollowDate));
            row.SetField("Subject", data.FollowContent);
            row.SetField("Status", "");
            table.Rows.Add(row);
            row.AcceptChanges();
        }

       


        public override void Print()
        {
            base.Print();
        }

        
    }
}
