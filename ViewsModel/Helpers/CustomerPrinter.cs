using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using ThreadingTasks = System.Threading.Tasks;
using Jsa.DomainModel;
using Jsa.ViewsModel.Annotations;
using Microsoft.Office.Interop.Word;

namespace Jsa.ViewsModel.Helpers
{
    public class CustomerPrinter
    {
        private readonly Dictionary<string, string> _filedsMapper;

        public CustomerPrinter([NotNull] Customer customer, string proprtyNo, int contractNo, string startDate)
        {
            if (customer == null) throw new ArgumentNullException("customer");
            _filedsMapper = MapToFields(customer, proprtyNo, contractNo, startDate);
        }

        private Dictionary<string, string> MapToFields(Customer customer, string propertyNo, int contractNo,
                                                       string startDate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                {"PropertyNo", propertyNo},
                {"CustomerNo", customer.CustomerId.ToString(CultureInfo.InvariantCulture)},
                {"CustomerName", customer.Name},
                {"IdNo", customer.IdNumber},
                {"Nationality", customer.Nationality},
                {"Mobile1", customer.MainMobile},
                {"Mobile2", customer.SecondMobile},
                {"WorkPhone", customer.WorkPhone},
                {"HomePhone", customer.HomePhone},
                {"Fax", customer.Fax},
                {"Email", customer.Email},
                {"Address", customer.AddressLine1 + Environment.NewLine + customer.AddressLine2},
                {"ContractNo", contractNo.ToString(CultureInfo.InvariantCulture)},
                {"StartDate", Helper.DateMiror(startDate)}
            };
            return dic;
        }

        public ThreadingTasks.Task Print(string path, string selectedPrinter)
        {
            ThreadingTasks.Task tsk = ThreadingTasks.Task.Run(async () =>
                {
                    _Application wordApplication = new Application();
                    Documents docs = wordApplication.Documents;
                    Document document = null;
                    try
                    {
                        document = docs.Add(path);
                        FillFields(document.Fields);
                        //document.Activate();
                        //wordApplication.Visible = true;
                        await PrintOut(wordApplication, document, selectedPrinter);
                       // document.PrintOut(false);
                    }
                    catch (Exception)
                    {
                        wordApplication.Quit(false);
                        ReleaseResources(wordApplication, docs, document);
                        throw;
                    }
                    finally
                    {
                        wordApplication.Quit(false);
                        ReleaseResources(wordApplication, docs, document);
                    }
                }
                );
            return tsk;
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

        private void FillFields(Fields fields)
        {
            foreach (var item in _filedsMapper)
            {
                var p = fields.Cast<Field>().FirstOrDefault(x => x.Code.Text.Contains(item.Key));
                if (p != null) p.Result.Text = item.Value;
            }
        }

        /// <summary>
        /// Use this method to simulate background printing in word
        /// by using a task then await it unil it return.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="word"></param>
        /// <param name="selectedPrinter"></param>
        /// <returns></returns>
        private async System.Threading.Tasks.Task PrintOut(_Application app, _Document word, string selectedPrinter)
        {
            System.Threading.Tasks.Task tsk = System.Threading.Tasks.Task.Run(() =>
            {
                //app.ActivePrinter = selectedPrinter;  //Note: This will change the Windows deafault printer.
                //Therefore, we use this function to print in word without affecting the Windows default printer.
                    app.WordBasic.FilePrintSetup(Printer: selectedPrinter, DoNotSetAsSysDefault: 1);
                    
                    word.PrintOut(false);
                }
                );
            await tsk;
        }
    }
}