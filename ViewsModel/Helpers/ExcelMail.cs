using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using SysData = System.Data;
using Jsa.ViewsModel.Reports;
using System.Drawing;
using Jsa.ViewsModel.SyncStrategy;
namespace Jsa.ViewsModel.Helpers
{
    public class ExcelMail : IReportProgress
    {
        private ReportLayout Layout { get; set; }
        public ExcelMail()
        {

        }
        public ExcelMail(ReportLayout layout)
        {
            Layout = layout;
        }

        void ForceExcleToQuit(_Application objExcel)
        {
            if (objExcel != null)
            {
                objExcel.DisplayAlerts = false;
                objExcel.Quit();
            }
        }
        public Task Send(SysData.DataTable table, string excelTemplatePath, bool directlyPrint, int startRowIndex = 2)
        {

            if (table == null) throw new ArgumentNullException("table");
            if (string.IsNullOrEmpty(excelTemplatePath))
            {
                throw new ArgumentNullException("excelTemplatePath", "You must specify path to the excel workbook");
            }
           Task tsk =  Task.Run(() =>
                {
                    _Application excelApp = null;
                    Workbooks excelWorkbooks = null;
                    _Workbook excelWorkbook = null;
                    _Worksheet excelSheet = null;
                    string fileNamePath = excelTemplatePath; ;

                    //Start Excel and create new workbook from the template
                    excelApp = StartExcel();
                    try
                    {
                        excelWorkbooks = excelApp.Workbooks;
                        excelWorkbook = excelWorkbooks.Open(fileNamePath);
                        excelSheet = excelWorkbook.Sheets[1];
                        //Insert the DataGridView into the excel spreadsheet
                        TableToExcelSheet(table, excelSheet, startRowIndex, 1);
                        //if visible , then exit so user can see it, otherwise save and exit

                        if (directlyPrint)
                        {
                            //bool okPrint = objExcel.Dialogs[XlBuiltInDialog.xlDialogPrint].Show();
                            // if (!okPrint)
                            //{
                            //    objWorkbook.Close(false);
                            //    return;
                            //}
                            excelSheet.PrintOut();
                            //excelWorkbook.Close(false);
                            excelApp.DisplayAlerts = false;
                            excelApp.Quit();
                            ReleaseResources(excelApp, excelWorkbooks, excelWorkbook, excelSheet);
                            return;
                        }
                        excelApp.Visible = true;
                        ReleaseResources(excelApp, excelWorkbooks, excelWorkbook, excelSheet);
                    }
                    catch (Exception)
                    {
                        ForceExcleToQuit(excelApp);
                        ReleaseResources(excelApp, excelWorkbooks, excelWorkbook, excelSheet);
                        throw;
                    }
                }

                );
           return tsk;
        }
        void TableToExcelSheet(SysData.DataTable table, _Worksheet excelSheet, int startRow, int startCol)
        {
            int headerColIndex = table.Columns.IndexOf("HeaderRow");
            double count = table.Rows.Count;
            double current = 0.0;
            if (headerColIndex == -1)
            {
                for (int nRow = 0; nRow < table.Rows.Count; nRow++)
                {
                    for (int nCol = 0; nCol < table.Columns.Count; nCol++)
                    {
                        excelSheet.Cells[startRow + nRow, startCol + nCol] = table.Rows[nRow].ItemArray[nCol]; //table.Rows[nRow].Cells[nCol].Value;
                    }
                    current++;
                    double progress = (current / count) * 100;
                    RaiseProgress(progress);


                }
            }
            else
            {
                for (int nRow = 0; nRow < table.Rows.Count; nRow++)
                {
                    var headerRow = Convert.ToBoolean(table.Rows[nRow].ItemArray[headerColIndex]);
                    for (int nCol = 0; nCol < table.Columns.Count; nCol++)
                    {
                        if (table.Columns[nCol].Caption == "HeaderRow") continue;
                        excelSheet.Cells[startRow + nRow, startCol + nCol] = table.Rows[nRow].ItemArray[nCol]; //table.Rows[nRow].Cells[nCol].Value;
                        if (headerRow && nRow != 0)
                        {
                            var cell = excelSheet.Cells[startRow + nRow, startCol + nCol];
                            FormatCell(cell);
                        }

                    }
                    current++;
                    double progress = (current / count) * 100;
                    RaiseProgress(progress);

                }
            }

        }
        private void FormatCell(Range cell)
        {
            cell.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlMedium;
            cell.Borders[XlBordersIndex.xlEdgeTop].Color = Color.Blue.ToArgb();
        }

        _Application StartExcel()
        {
            return new Application();
        }
        void ReleaseResources(params object[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null)
                {
                    Marshal.FinalReleaseComObject(objs[i]);
                }
            }
        }


        public event EventHandler<ProgressEventArgs> ReportProgress;

        public void RaiseProgress(double progress)
        {
            if (ReportProgress != null)
            {
                ReportProgress(this, new ProgressEventArgs(progress));
            }
        }
        
        
    }
}
