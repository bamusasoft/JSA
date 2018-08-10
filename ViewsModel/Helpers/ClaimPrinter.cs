using System;
using System.Collections.Generic;
using System.Linq;
using Threading = System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Globalization;
namespace Jsa.ViewsModel.Helpers
{
    public class ClaimPrinter: IDisposable
    {
        #region "Fields"
       
        string _wordTemplatePath;
        Dictionary<int, string> _detailsRowsNames;
        _Application _wordApplication;
        private bool _disposed;
        private Queue<int> _backgroundJobs;
        private int _jobId;
        private static object _locker = new object();
        #endregion
        public ClaimPrinter( string wordTemplatePath)
        {

            if (string.IsNullOrEmpty(wordTemplatePath)) throw new ArgumentNullException("wordTemplatePath");
           
            _wordTemplatePath = wordTemplatePath;
            _detailsRowsNames = CreateRowsNames();
            _backgroundJobs = new Queue<int>();
            _wordApplication = CreateWordApp();
            _jobId = 0;
        }
        private static _Application CreateWordApp()
        {
            return new Application();
        }
        private Dictionary<int, string> CreateRowsNames()
        {
            return new Dictionary<int, string>()
            {
                {1, "a"},{2,"b"}, {3, "c"},
                {4, "d"},{5,"e"}, {6, "f"},
                {7, "g"},{8,"h"}, {9, "i"},
                {10, "j"}

            };
        }
        public Threading.Task PrintAsync(ViewClaim claim, string selectedPrinter)
        {
            if (claim == null) throw new ArgumentNullException("claim");
             Threading.Task tsk = Threading.Task.Run(async () =>
                {

                    _jobId++;
                    lock (_locker) //Write to thread shared variable;
                    {
                        _backgroundJobs.Enqueue(_jobId);
                        
                    }
                    Documents docs = null;
                    _Document doc = null;
                    try
                    {
                        docs = _wordApplication.Documents;
                        doc = docs.Add(_wordTemplatePath);
                        var d = FieldMapping(claim);
                        FillFields(doc.Fields, d);

                        //doc.Activate();
                        //_wordApplication.Dialogs[WdWordDialog.wdDialogFilePrint].Show();
                        //if (result == -1)
                        //{
                        //    doc.PrintOut();
                        //}

                       //Don't call microsoft word PrintOut method directly here because PrintOut method
                       //Runs async and will return immediately, therefor letting you to close the word befor
                       //The print job finished which will lead to uncompleted printing job.
                       //So, the solution is to wrap the Word 'PrintOut' method in an asyn method and then
                       //wait on it so we will not proceed until my async method return which will return only
                       //after the Word PrintOut method runs to complete.
                       await PrintOut(doc, selectedPrinter);

                    }
                    finally
                    {
                        doc.Close(false); 
                        lock (_locker) //Write to thread shared variable;
                        {
                            _backgroundJobs.Dequeue();
                            
                        }
                    }
                }
                );
            return tsk;


        }
        /// <summary>
        /// Use this method to simulate background printing in word
        /// by using a task then await it unil it return.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="selectedPrinter"></param>
        /// <returns></returns>
        private async Threading.Task PrintOut(_Document word, string selectedPrinter)
        { 
            Threading.Task tsk = Threading.Task.Run(() =>
                {
                    _wordApplication.ActivePrinter = selectedPrinter;
                    word.PrintOut();
                }
                );
            await tsk;
            return;

        }
       
        void FillFields(Fields fields, Dictionary<string,string> mapper)
        {
            foreach (var item in mapper)
            {
                
                //foreach (Field field in fields) //Old fashion way of getting fields
                //{
                //    if (field.Code.Text.Contains(item.Key))
                //    {
                //        field.Result.Text = item.Value;
                //        break;
                //    }
                //}
                
                //New way using linq to get access to the fields
                var p = fields.Cast<Field>()
                    .Where(x => x.Code.Text.Contains(item.Key))
                    .FirstOrDefault();
                if(p != null) p.Result.Text = item.Value;
                

            }
        }

        private Dictionary<string,string> FieldMapping( ViewClaim claim)
        {
            Dictionary<string, string> lst = new Dictionary<string,string>();
            lst.Add("ClaimSequence", claim.SequenceText);
            lst.Add("CustomerName", claim.CustomerName);
            lst.Add("ClaimLetter", claim.ClaimLetter);
            lst.Add("GrandTotal", claim.GrandTotal.ToString("#,0", CultureInfo.InvariantCulture));
            //This is a flag to experience the ArgumentOutOfRangeException just once.
            bool argumentOutOfRangeReached = false;
            for (int i = 0; i < 10; i++)
            {
                string row = null;
                switch (i)
                {
                    case 0:
                        row = "a";
                        break;
                    case 1:
                        row = "b";
                        break;
                    case 2:
                        row = "c";
                        break;
                    case 3:
                        row = "d";
                        break;
                    case 4:
                        row = "e";
                        break;
                    case 5:
                        row = "f";
                        break;
                    case 6:
                        row = "g";
                        break;
                    case 7:
                        row = "h";
                        break;
                    case 8:
                        row = "i";
                        break;
                    case 9:
                        row = "j";
                        break;
                }
                try
                {
                    //User "#,0" format for ToString method so that you will get
                    //number formated with thousands separator and if the number is zero 
                    //zero will be returned no empty string.
                    if (!argumentOutOfRangeReached)
                    {
                        var detailRow = claim.Details[i];
                        lst.Add((row + "1"), detailRow.PropertyType);
                        lst.Add((row + "2"), detailRow.TypeNo);
                        lst.Add((row + "3"), detailRow.PropertyLocation);
                        lst.Add((row + "4"), detailRow.Rent.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "5"), detailRow.Maintenance.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "6"), detailRow.Deposit.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "7"), detailRow.Others.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "8"), detailRow.Total.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "9"), detailRow.Paid.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "10"), detailRow.Balance.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "11"), detailRow.OutstandingRentBalance.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "12"), detailRow.OutstandingMaintBalance.ToString("#,0", CultureInfo.InvariantCulture));
                        lst.Add((row + "13"), detailRow.NetBalance.ToString("#,0", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        lst.Add((row + "1"), "");
                        lst.Add((row + "2"), "");
                        lst.Add((row + "3"), "");
                        lst.Add((row + "4"), "");
                        lst.Add((row + "5"), "");
                        lst.Add((row + "6"), "");
                        lst.Add((row + "7"), "");
                        lst.Add((row + "8"), "");
                        lst.Add((row + "9"), "");
                        lst.Add((row + "10"), "");
                        lst.Add((row + "11"), "");
                        lst.Add((row + "12"), "");
                        lst.Add((row + "13"), "");
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    lst.Add((row + "1"), "");
                    lst.Add((row + "2"), "");
                    lst.Add((row + "3"), "");
                    lst.Add((row + "4"), "");
                    lst.Add((row + "5"), "");
                    lst.Add((row + "6"), "");
                    lst.Add((row + "7"), "");
                    lst.Add((row + "8"), "");
                    lst.Add((row + "9"), "");
                    lst.Add((row + "10"), "");
                    lst.Add((row + "11"), "");
                    lst.Add((row + "12"), "");
                    lst.Add((row + "13"), "");
                    argumentOutOfRangeReached = true;
                }
                    
            }
            return lst;
        }
        #region "IDisposable"
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_wordApplication != null)
                    {
                        _wordApplication.Quit(false);
                        ReleaseResources(_wordApplication);
                    }
                }
            }
            _disposed = true;
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
        #endregion
       
        public bool CanExist
        {
            get
            {

                lock (_locker) //Read to thread shared variable;
                {
                    //It takes so long for the app to process the word fields; so for this time between 
                    //the print job reach word application printing queue we put it in our queue,
                    //which will remove the job befor the print queue of word app, therefore a combintion'
                    //of the two queue will insure that we don't exit until the both queues empty, so it's 
                    //safe to release word application reference.
                    //Console.WriteLine(_backgroundJobs.Count);
                    int i = _wordApplication.BackgroundPrintingStatus;
                    //Console.WriteLine(i);
                    return  _backgroundJobs.Count == 0 && i == 0 ;
                    
                }
            }
            
            
        }
    }
}
                        
