using System.Collections.Generic;
using System.Data;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.SearchCriteria;
using Jsa.ViewsModel.ViewsControllers;

namespace Jsa.ViewsModel.Reports
{
    public class MaintReport:ExcelReportBase<MaintReportFields>
    {

        public MaintReport(List<MaintReportFields> source, string excelTemplatePath, ExcelProperties excelProperties)
            : base(excelTemplatePath, excelProperties)
        {
            Data = CreateReport(source);
        }



        protected override sealed DataTable CreateReport(List<MaintReportFields> source)
        {
            DataTable table = CreateTable();
            AddColumns(table);
            source.ForEach((maint) => AddRow(table, maint));
            return table;
        }

        protected override DataTable CreateTable()
        {
            return new DataTable("MaintReport");
        }

        protected override void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("ContractNo");
            table.Columns.Add(c1);
            DataColumn c2 = new DataColumn("PropertyNo");
            table.Columns.Add(c2);
            DataColumn c3 = new DataColumn("CustomerNo");
            table.Columns.Add(c3);
            DataColumn c4 = new DataColumn("PropertyType");
            table.Columns.Add(c4);
            DataColumn c5 = new DataColumn("CustomerName");
            table.Columns.Add(c5);
            DataColumn c6 = new DataColumn("PropertyDescription");
            table.Columns.Add(c6);
            DataColumn c7 = new DataColumn("Location");
            table.Columns.Add(c7);
            DataColumn c8 = new DataColumn("AgreedMaint");
            table.Columns.Add(c8);
            DataColumn c9 = new DataColumn("Paid");
            table.Columns.Add(c9);
            DataColumn c10 = new DataColumn("Balance");
            table.Columns.Add(c10);
            DataColumn c11 = new DataColumn("ReceiptNo");
            table.Columns.Add(c11);
            DataColumn c12 = new DataColumn("DateDue");
            table.Columns.Add(c12);
        }

        protected override void AddRow(DataTable table, MaintReportFields data)
        {
            DataRow row = table.NewRow();
            row.SetField("ContractNo", data.ContractNo);
            row.SetField("PropertyNo", data.PropertyNo);
            row.SetField("CustomerNo", data.CustomerNo);
            row.SetField("PropertyType", data.PropertyType);
            row.SetField("CustomerName", data.CustomerName);
            row.SetField("PropertyDescription", data.PropertyDescription);
            row.SetField("Location", data.Location);
            row.SetField("AgreedMaint", data.AgreedMaint);
            row.SetField("Paid", data.Paid);
            row.SetField("Balance", data.Balance);
            row.SetField("ReceiptNo", data.ReceiptNo);
            row.SetField("DateDue", Helper.PutMask(data.DateDue));
            table.Rows.Add(row);
            row.AcceptChanges();

        }

        
    }


}
