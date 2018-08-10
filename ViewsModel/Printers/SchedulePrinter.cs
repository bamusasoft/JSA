using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Reports;
using Microsoft.Office.Interop.Word;

namespace Jsa.ViewsModel.Printers
{
    public class SchedulePrinter
    {
        private ScheduleReport _report;
        private readonly string _path;
        private const string SUM = "الإجمالي";
        public SchedulePrinter(Schedule schedule, string path)
        {
            if (schedule == null) throw new ArgumentNullException("schedule");
            _report = BuildReport(schedule);
            _path = path;
        }

        private ScheduleReport BuildReport(Schedule schedule)
        {
            string name = schedule.Customer.Name;
            
            var contracts = schedule.ScheduleDetails.Distinct(new CompareDetailsByContract());
            string locatoion = contracts.Aggregate("", (current, scheduleDetail) => current + (scheduleDetail.Contract.Property.Description + " / "));
            var sdReport =
                schedule.ScheduleDetails.Select(
                    scheduleDetail =>
                        new ScheduleDetailReport(scheduleDetail.AmountDue, scheduleDetail.DateDue,
                            scheduleDetail.AmountPaid, scheduleDetail.Balance, false)).OrderBy(detail => detail.DateDue) .ToList();
            //Add sum row
            var dueSum = schedule.ScheduleDetails.Sum(x => x.AmountDue);
            var paidSum = schedule.ScheduleDetails.Sum(x => x.AmountPaid);
            var balanceSum = schedule.ScheduleDetails.Sum(x => x.Balance);
            sdReport.Add(new ScheduleDetailReport(dueSum, SUM, paidSum, balanceSum, true));

            return _report = new ScheduleReport(name, locatoion, sdReport);
        }

        public void Print()
        {
            _Application wordApplication = null;
            Documents docs = null;
            _Document doc = null;
            try
            {
                wordApplication = new Application();
                docs = wordApplication.Documents;
                doc = docs.Add(_path);

                WriteFields(doc.Fields);
                //Insert table for schedule details
                var wrdRng = doc.Bookmarks["TableLocation"].Range;
                var detailsTable = doc.Tables.Add(wrdRng, _report.Details.Count + 1, _report.PropertiesCount);
                detailsTable.Range.ParagraphFormat.SpaceAfter = 6;

                var headerRow = detailsTable.Rows[1];
                WriteHeaderRow(headerRow);
                StyleHeaderRow(headerRow);
                var tableRows = detailsTable.Rows;
                WriteDetails(tableRows, _report.Details);
                ApplyTableStyle(detailsTable);
                wordApplication.Visible = true;
            }
            catch
            {
                ReleaseResources(wordApplication, docs, doc);
                throw;
            }


        }

        private void WriteDetails(Rows tableRows, IEnumerable<ScheduleDetailReport> details)
        {
            int startRowIndex = 1;
            foreach (var detail in details)
            {
                startRowIndex = startRowIndex + 1;
                var row = tableRows[startRowIndex];
                WriteRowCells(row, detail);
            }
        }
        private void WriteHeaderRow(Row headerRow)
        {
            var headerRowCells = headerRow.Cells;
            headerRowCells[1].Range.Text = "مبلغ الدفعة";
            headerRowCells[2].Range.Text = "التاريخ";
            headerRowCells[3].Range.Text = "المسدد";
            headerRowCells[4].Range.Text = "الرصيد";
            headerRowCells[5].Range.Text = "سددت؟";
        }

        private void WriteRowCells(Row row, ScheduleDetailReport detail)
        {
            var cells = row.Cells;
            cells[1].Range.Text = detail.AmountDue.ToString("#,0");
            cells[2].Range.Text = detail.DateDue;
            cells[3].Range.Text = detail.AmountPaid.ToString("#,0");
            cells[4].Range.Text = detail.Balance.ToString("#,0");
            cells[5].Range.Text = detail.Status;
        }
        private void ApplyTableStyle(Table table)
        {
            // ReSharper disable once UseIndexedProperty
            //table.set_Style("Table Grid");
            //table.set_Style(WdBuiltinStyle.wdStyleTableMediumGrid1);
            //var s = table.get_Style();

            #region Table's Outside Borders
            table.Borders[WdBorderType.wdBorderLeft].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders[WdBorderType.wdBorderLeft].LineWidth = WdLineWidth.wdLineWidth150pt;

            table.Borders[WdBorderType.wdBorderRight].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders[WdBorderType.wdBorderRight].LineWidth = WdLineWidth.wdLineWidth150pt;

            table.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders[WdBorderType.wdBorderTop].LineWidth = WdLineWidth.wdLineWidth150pt;

            table.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders[WdBorderType.wdBorderBottom].LineWidth = WdLineWidth.wdLineWidth150pt;
            #endregion

            #region Table's Inside Borders
            table.Borders[WdBorderType.wdBorderHorizontal].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders[WdBorderType.wdBorderHorizontal].LineWidth = WdLineWidth.wdLineWidth025pt;

            table.Borders[WdBorderType.wdBorderVertical].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders[WdBorderType.wdBorderVertical].LineWidth = WdLineWidth.wdLineWidth025pt;
            #endregion
        }

        private void StyleHeaderRow(Row headerRow)
        {
            foreach (Cell cell in headerRow.Cells)
            {
                cell.Range.BoldBi = 1;
                cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            }
        }

        private void WriteFields(Fields fields)
        {
            foreach (Field field in fields)
            {
                if (field.Code.Text.Contains("CustomerName"))
                {
                    field.Result.Text = _report.CustomerName;
                    continue;
                }
                if (field.Code.Text.Contains("PropertyLocation"))
                {
                    field.Result.Text = _report.PropertyLocation;
                }

            }
        }
        

        private void ReleaseResources(params object[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null)
                {
                    Marshal.FinalReleaseComObject(objs[i]);
                }
            }
        }
    }
}
