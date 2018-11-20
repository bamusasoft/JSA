using Jsa.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public class DocRecordFollowPrintReport : ExcelReportBase<DocFollowsReport>
    {
        public DocRecordFollowPrintReport(List<DocFollowsReport> source, string excelTemplatePath, ExcelProperties excelProperties)
           : base(excelTemplatePath, excelProperties)
        {
            Data = CreateReport(source);
        }

        protected sealed override DataTable CreateReport(List<DocFollowsReport> source)
        {
            //source.OrderBy(x => x.Destination).ThenBy(p => p.FollowDate);
            DataTable table = CreateTable();
            AddColumns(table);

            string currentDocId = "";
            int count = 0;
            foreach (var docFollow in source)
            {
                if(string.IsNullOrEmpty(currentDocId) || currentDocId != docFollow.DocId)
                {
                    if (count > 0)
                    {
                        AddEmptyRow(table);
                    }
                    AddDocRow(table, docFollow);
                    AddFollowHeader(table);
                    AddFollowRow(table, docFollow);
                    currentDocId = docFollow.DocId;
                }
                else
                {
                    AddFollowRow(table, docFollow);
                }

                count++;
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
            //Flag for the row that represents a header of doc follow
            DataColumn c7 = new DataColumn("HeaderRow");
            table.Columns.Add(c7);
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

            row.SetField("HeaderRow", false);

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
            row.SetField("HeaderRow", true);

            table.Rows.Add(row);
            row.AcceptChanges();


        }

        private void AddEmptyRow(DataTable table)
        {
            DataRow row = table.NewRow();

            row.SetField("DocId", "");
            row.SetField("DocRefId", "");
            row.SetField("DocDate", "");
            row.SetField("Destination", "");
            row.SetField("Subject", "");
            row.SetField("Status", "");
            row.SetField("HeaderRow", false);

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

            row.SetField("HeaderRow", false);

            table.Rows.Add(row);
            row.AcceptChanges();
        }

       


        public override void Print()
        {
            base.Print();
        }

        
    }
}
