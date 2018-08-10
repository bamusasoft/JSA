using Jsa.DomainModel;
using System.Collections.Generic;
using Jsa.ViewsModel.Views;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
namespace Jsa.ViewsModel.Helpers
{
    public class MonthlyContractPrinter
    {
        readonly Contract _contract;
        readonly Representative _representative;
        readonly string _startDate;
        readonly string _endDate;
        readonly string _templatePath;
        readonly Dictionary<string, string> _fieldsValues;
        _Application _word;
        public MonthlyContractPrinter(string templatePath, Contract contract, Representative representative, string startDate, string endDate)
        {
            _contract = contract;
            _representative = representative;
            _startDate = startDate;
            _endDate = endDate;
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
                Fields docFields = doc.Fields;
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
        /// Print the field that is on the header of the document.
        /// </summary>
        /// <param name="doc"></param>
        /// <remarks>
        /// Note that fields in the header does not behave as the rest of fields on the document, for unkonwn reason (for me)
        /// they wouldn't accept writing to them by calling Field.Result.Text, which how other fields can be written to.
        /// Instead, we use the method Selection.TypeText, which write directly on the field, but this approach will not preserve
        /// the format of the field, which you have to format it manullay.
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
        Dictionary<string, string> PopulateFieldsInfo()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("«ContractNo»", _contract.ContractNo.ToString());
            dic.Add("«Day»", _contract.SignDay);
            dic.Add("«Hij»", Helper.PutMask(_contract.SignHijriDate) + " هـ");
            dic.Add("«Greg»", Helper.PutMask( _contract.SignGregDate) + " م");
            dic.Add("«CusName»", _contract.Customer.Name);
            dic.Add("«CustId»", _contract.Customer.IdNumber);
            dic.Add("«IdIssueAt»", _contract.Customer.IdIssue);
            dic.Add("«IdIssueDate»", Helper.PutMask(_contract.Customer.IdDate));
            dic.Add("«RepName»", _representative.CustomerId == -1 ? "------------" : _representative.Name);
            dic.Add("«RepId»", _representative.Id);
            dic.Add("«RepIssue»", _representative.IssueAt);
            dic.Add("«RepDate»", Helper.PutMask(_representative.IdDate));
            dic.Add("«Address1»", _contract.Customer.AddressLine1);
            dic.Add("«Address2»", _contract.Customer.AddressLine2);
            dic.Add("«PropertyName»", _contract.Property.Location);
            dic.Add("«Activity»", _contract.ContractsActivity != null ? _contract.ContractsActivity.Description : "------------");
            dic.Add("«PropNo»", ContractView.ExtractProeprtyNo(_contract.Property.PropertyNo));
            dic.Add("«Location»", _contract.Property.Location);
            dic.Add("«StartDate»", Helper.PutMask(_startDate) + " هـ");
            dic.Add("«EndDate»", Helper.PutMask(_endDate) + " هـ");
            dic.Add("«Rent»", _contract.AgreedRent.ToString("#,0") + " ريـال");
            dic.Add("«RentWords»", SayNumber.ToWords(_contract.AgreedRent));
            if (_contract.AgreedDeposit == 0)
            {
                dic.Add("«Deposit»", "-----");
                dic.Add("«DepositWords»", "-------");
            }
            else
            {
                dic.Add("«Deposit»", _contract.AgreedDeposit.ToString("#,0") + " ريـال");
                dic.Add("«DepositWords»", SayNumber.ToWords(_contract.AgreedDeposit));
            }
            
            return dic;
        }
        private string GetFieldValue(string fieldCode)
        {
            return _fieldsValues[fieldCode];
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
    }
}
