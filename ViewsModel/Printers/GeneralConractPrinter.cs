using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Jsa.ViewsModel.Annotations;
using Jsa.ViewsModel.DomainEntities;
using Microsoft.Office.Interop.Word;

namespace Jsa.ViewsModel.Printers
{
    public class GeneralConractPrinter
    {
        private readonly ContractDto _contract;
        private readonly Dictionary<string, string> _fieldsValues;
        private readonly string _templatePath;
        private _Application _word;
        private readonly Func<string, string> _extractPropertyNo;
        private readonly Func<string, string, string> _extractLocation;

        public GeneralConractPrinter(string templatePath, [NotNull] ContractDto contract, [NotNull] Func<string, string> extractPropertyNo, [NotNull] Func<string,string, string> extractLocation )
        {
            _extractPropertyNo = extractPropertyNo;
            _extractLocation = extractLocation;
            _contract = contract;
            _templatePath = templatePath;
            _fieldsValues = PopulateFieldsInfo();
        }

        public void Print(string printerName)
        {
            _Document doc = null;
            try
            {
                doc = OpenDoc(_templatePath, printerName);
                PrintHeaderField(doc);
                var docFields = doc.Fields;
                FillFields(docFields);
                doc.PrintOut(false); //False to wait for word to finish printing 
            }
            catch
            {
                throw;
            }
            finally
            {
                _word.Quit(false);
                ReleaseResources(_word, doc);
            }
        }

        /// <summary>
        ///     Print the field that is on the header of the document.
        /// </summary>
        /// <param name="doc"></param>
        /// <remarks>
        ///     Note that fields in the header does not behave as the rest of fields on the document, for unkonwn reason (for me)
        ///     they wouldn't accept writing to them by calling Field.Result.Text, which how other fields can be written to.
        ///     Instead, we use the method Selection.TypeText, which write directly on the field, but this approach will not
        ///     preserve
        ///     the format of the field, which you have to format it manullay.
        /// </remarks>
        private void PrintHeaderField(_Document doc)
        {
            foreach (Section sec in doc.Sections)
            {
                doc.TrackRevisions = false;
                var headers = sec.Headers;
                foreach (HeaderFooter header in headers)
                {
                    var fields = header.Range.Fields;
                    foreach (Field field in fields)
                    {
                        if (field.Code.Text.Contains("ContractNo"))
                        {
                            field.Select();
                            var app = doc.Application;
                            app.Selection.Font.Bold = -1;
                            app.Selection.Font.Size = 18;
                            app.Selection.TypeText(_contract.ContractNo.ToString());
                            break;
                        }
                    }
                }
            }
        }

        private _Document OpenDoc(string path, string printerName)
        {
            _word = CreateWordApp();
            var s = _word.Documents.Add(path);
            //_word.ActivePrinter = printerName; //Note: This will change the Windows deafault printer
            //Therefore, we use this function to print in word without affecting the Windows default printer.
            _word.WordBasic.FilePrintSetup(Printer: printerName, DoNotSetAsSysDefault: 1);
            return s;
        }

        private static _Application CreateWordApp()
        {
            return new Application();
        }

        private void FillFields(Fields fields)
        {
            foreach (Field field in fields)
            {
                field.Result.Text = GetFieldValue(field.Result.Text);
            }
        }

        private Dictionary<string, string> PopulateFieldsInfo()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("«ContractNo»", _contract.ContractNo.ToString());
            dic.Add("«Day»", _contract.SignDay);
            dic.Add("«HijDate»", Helper.DateMiror(_contract.SignHijriDate) + " هـ");
            dic.Add("«GregDate»", Helper.DateMiror(_contract.SignGregDate) + " م");
            dic.Add("«CustName»", _contract.Customer.Name);
            dic.Add("«Nationality»", _contract.Customer.Nationality);
            dic.Add("«IdType»", _contract.Customer.IdType);
            dic.Add("«IdNo»", _contract.Customer.IdNumber);
            dic.Add("«IdDate»", Helper.DateMiror(_contract.Customer.IdDate));
            dic.Add("«IdIssue»", _contract.Customer.IdIssue);
            dic.Add("«AddressLineOne»", _contract.Customer.AddressLine1);
            dic.Add("«AddressLineTwo»", _contract.Customer.AddressLine2);
            dic.Add("«PropertyType»", _contract.Property.Type);
            dic.Add("«PropertyNo»", _extractPropertyNo(_contract.Property.PropertyNo));
            dic.Add("«Location»", _extractLocation(_contract.Property.PropertyNo,  _contract.Property.Location));
            dic.Add("«City»", _contract.Property.City);
            dic.Add("«District»", _contract.Property.District);
            dic.Add("«Activity»", _contract.Activity.Description);
            dic.Add("«Period»", _contract.Period);
            dic.Add("«StartDate»", Helper.DateMiror(_contract.StartDate) + " هـ");
            dic.Add("«EndDate»", Helper.DateMiror(_contract.EndDate) + " هـ");
            dic.Add("«Rent»", _contract.AgreedRent.ToString("#,0") + " ريـال");
            dic.Add("«RentInWords»", _contract.RentInWords);
            if (_contract.AgreedDeposit == 0)
            {
                dic.Add("«Deposit»", "-----");
                dic.Add("«DepositInWords»", "-------");
            }
            else
            {
                dic.Add("«Deposit»", _contract.AgreedDeposit.ToString("#,0") + " ريـال");
                dic.Add("«DepositInWords»", _contract.DepositInWords);
            }
            dic.Add("«Court»", _contract.Court);
            return dic;
        }

        private string GetFieldValue(string fieldCode)
        {
            Console.WriteLine(fieldCode);
            return _fieldsValues[fieldCode];
        }

        private void ReleaseResources(params object[] objs)
        {
            for (var i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null)
                {
                    Marshal.FinalReleaseComObject(objs[i]);
                }
            }
        }
    }
}