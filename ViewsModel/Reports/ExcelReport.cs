using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.SyncStrategy;
using Jsa.ViewsModel.ViewsControllers;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace Jsa.ViewsModel.Reports
{
    public abstract class ExcelReportBase<T> : IReport<T>
    {

        protected _Application ExcelApp { get; set; }
        protected Workbooks ExcelWorkbooks { get; set; }
        protected _Workbook ExcelWorkbook { get; set; }
        protected _Worksheet ExcelSheet { get; set; }
        protected string ExcelTemplatePath { get; set; }
        private List<object> ExcelResources { get; set; }
        protected DataTable Data { get; set; }
        protected ExcelProperties ExcelProperties { get; set; }

        protected abstract DataTable CreateReport(List<T> source);
        protected abstract DataTable CreateTable();
        protected abstract void AddColumns(DataTable table);
        protected abstract void AddRow(DataTable table, T data);


        protected ExcelReportBase(string excelTemplatePath, ExcelProperties excelProperties)
        {
            ExcelTemplatePath = excelTemplatePath;
            ExcelProperties = excelProperties;
            StartExcel();
        }

        private void ForceExcleToQuit(_Application objExcel)
        {
            if (objExcel != null)
            {
                objExcel.DisplayAlerts = false;
                objExcel.Quit();
            }
        }

        public virtual void Print()
        {

            try
            {

                //Insert the DataGridView into the excel spreadsheet
                TableToExcelSheet(Data, ExcelSheet, ExcelProperties.StartRow, ExcelProperties.StartColumn);
                //if visible , then exit so user can see it, otherwise save and exit

                if (ExcelProperties.PrintDirectely)
                {
                    //bool okPrint = objExcel.Dialogs[XlBuiltInDialog.xlDialogPrint].Show();
                    // if (!okPrint)
                    //{
                    //    objWorkbook.Close(false);
                    //    return;
                    //}
                    ExcelSheet.PrintOut();
                    //excelWorkbook.Close(false);
                    ExcelApp.DisplayAlerts = false;
                    ExcelApp.Quit();
                    Dispose();
                    return;
                }
                ExcelApp.Visible = true;
                Dispose();
            }
            catch (Exception)
            {
                ForceExcleToQuit(ExcelApp);
                Dispose();
                throw;
            }
        }



        private void TableToExcelSheet(DataTable table, _Worksheet excelSheet, int startRow, int startCol)
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
                        excelSheet.Cells[startRow + nRow, startCol + nCol] = table.Rows[nRow].ItemArray[nCol];
                            //table.Rows[nRow].Cells[nCol].Value;
                    }
                    current++;
                    double progress = (current/count)*100;
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
                        excelSheet.Cells[startRow + nRow, startCol + nCol] = table.Rows[nRow].ItemArray[nCol];
                            //table.Rows[nRow].Cells[nCol].Value;
                        if (headerRow && nRow != 0)
                        {
                            var cell = excelSheet.Cells[startRow + nRow, startCol + nCol];
                            FormatCell(cell);
                        }

                    }
                    current++;
                    double progress = (current/count)*100;
                    RaiseProgress(progress);

                }
            }

        }

        private void FormatCell(Range cell)
        {
            cell.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlMedium;
            cell.Borders[XlBordersIndex.xlEdgeTop].Color = Color.Blue.ToArgb();
        }

        private void StartExcel()
        {
            ExcelApp = new Application();
            ExcelWorkbooks = ExcelApp.Workbooks;
            ExcelWorkbook = ExcelWorkbooks.Open(ExcelTemplatePath);
            ExcelSheet = ExcelWorkbook.Worksheets[1];

            ExcelResources = new List<object>();
            ExcelResources.AddRange(new object[] {ExcelApp, ExcelWorkbooks, ExcelWorkbook, ExcelSheet});
        }

        private void ReleaseResources()
        {
            ExcelResources.ForEach((excelResource) =>
            {
                if (excelResource != null)
                {
                    Marshal.FinalReleaseComObject(excelResource);
                }
            });
        }


        public event EventHandler<ProgressEventArgs> ReportProgress;

        public void RaiseProgress(double progress)
        {
            if (ReportProgress != null)
            {
                ReportProgress(this, new ProgressEventArgs(progress));
            }
        }

        


        public void Dispose()
        {
            ReleaseResources();
        }
    }

}
