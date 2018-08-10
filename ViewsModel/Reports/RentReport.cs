using System.Collections.Generic;
using System.Data;
using System.Linq;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.SearchCriteria;
using Jsa.ViewsModel.ViewsControllers;

namespace Jsa.ViewsModel.Reports
{
    public class RentReport:ExcelReportBase<RentReportFields>
    {
        public RentReport(List<RentReportFields> source,  string excelTemplatePath, ExcelProperties excelProperties) : base(excelTemplatePath, excelProperties)
        {
            Data = CreateReport(source);
        }

        protected override sealed DataTable CreateReport(List<RentReportFields> source)
        {
            DataTable table = CreateTable();
            AddColumns(table);
            source.ForEach((rent) => AddRow(table, rent));
            return table;
        }

        protected override DataTable CreateTable()
        {
            return new DataTable("RentReport");
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
            DataColumn c8 = new DataColumn("AgreedRent");
            table.Columns.Add(c8);
            DataColumn c9 = new DataColumn("RentDue");
            table.Columns.Add(c9);
            DataColumn c10 = new DataColumn("Paid");
            table.Columns.Add(c10);
            DataColumn c11 = new DataColumn("Balance");
            table.Columns.Add(c11);
            DataColumn c12 = new DataColumn("Mobile1");
            table.Columns.Add(c12);
            DataColumn c13 = new DataColumn("Mobile2");
            table.Columns.Add(c13);
            DataColumn c14 = new DataColumn("WorkPhone");
            table.Columns.Add(c14);
            DataColumn c15 = new DataColumn("HomePhone");
            table.Columns.Add(c15);

        }

        protected override void AddRow(DataTable table, RentReportFields data)
        {
            DataRow row = table.NewRow();
            row.SetField("ContractNo", data.ContractNo);
            row.SetField("PropertyNo", data.PropertyNo);
            row.SetField("CustomerNo", data.CustomerNo);
            row.SetField("PropertyType", data.PropertyType);
            row.SetField("CustomerName", data.CustomerName);
            row.SetField("PropertyDescription", data.PropertyDescription);
            row.SetField("Location", data.Location);
            row.SetField("AgreedRent", data.AgreedRent);
            row.SetField("RentDue", data.RentDue);
            row.SetField("Paid", data.Paid);
            row.SetField("Balance", data.Balance);
            row.SetField("Mobile1", data.Mobile1);
            row.SetField("Mobile2", data.Mobile2);
            row.SetField("WorkPhone", data.WorkPhone);
            row.SetField("HomePhone", data.HomePhone);

            table.Rows.Add(row);
            row.AcceptChanges();

        }
    }
}
