using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.Reports
{
    public class ClassesReport : ExcelReportBase<ClassContractFields>
    {
        public ClassesReport(List<ClassContractFields> source, string excelTemplatePath, ExcelProperties properties)
            :base(excelTemplatePath, properties)
        {
            Data = CreateReport(source);
        }
        protected override sealed DataTable CreateReport(List<ClassContractFields> source)
        {
            DataTable table = CreateTable();
            AddColumns(table);
            source.ForEach((cla) => AddRow(table, cla));
            return table;
        }

        protected override DataTable CreateTable()
        {
            return new DataTable("ClassesReport");
        }

        protected override void AddColumns(DataTable table)
        {
            DataColumn c1 = new DataColumn("PropertyNo");
            table.Columns.Add(c1);
            DataColumn c2 = new DataColumn("CustomerNo");
            table.Columns.Add(c2);
            DataColumn c3 = new DataColumn("Name");
            table.Columns.Add(c3);
            DataColumn c4 = new DataColumn("Location");
            table.Columns.Add(c4);
            DataColumn c5 = new DataColumn("Property");
            table.Columns.Add(c5);
            DataColumn c6 = new DataColumn("Rent");
            table.Columns.Add(c6);

            DataColumn c7 = new DataColumn("CustomerClass");
            table.Columns.Add(c7);

        }

        protected override void AddRow(DataTable table, ClassContractFields data)
        {
            DataRow row = table.NewRow();
            row.SetField("PropertyNo", data.PropertyNo);
            row.SetField("CustomerNo", data.CustomerNo);
            row.SetField("Name", data.Name);
            row.SetField("Location", data.Location);
            row.SetField("Property", data.Property);
            row.SetField("Rent", data.Rent);
            row.SetField("CustomerClass", data.CustomerClass);

            table.Rows.Add(row);
            row.AcceptChanges();

        }
    }
}
