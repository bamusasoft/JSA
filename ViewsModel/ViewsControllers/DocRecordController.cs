using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class DocRecordController : EditableControllerBase
    {

        public DocRecordController()
        {
            ControlState(ControllerStates.Blank);
        }
        #region Fields
        string _id;
        string _refId;
        string _docDate;
        string _subject;
        Destination _destination;
        string _docPath;
        DocRecordStatus _docStatus;

        ControllerStates _controllerStates;
        string _idSearchTerm;
        string _refSearchTerm;
        List<string> _errors;
        //
        bool _canSave;
        bool _canPrint;
        bool _canSearch;
        #endregion
        #region Properties
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string RefId
        {
            get { return _refId; }
            set
            {
                _refId = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string DocDate
        {
            get { return _docDate; }
            set
            {
                _docDate = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public Destination Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public DocRecordStatus DocStatus
        {
            get { return _docStatus; }
            set
            {
                _docStatus = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string DocPath
        {
            get { return _docPath; }
            set
            {
                _docPath = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        private int SecurityLevel {get; set ;}
        //
        public string IdSearchTerm
        {
            get { return _idSearchTerm; }
            set
            {
                _idSearchTerm = value;
                RaisePropertyChanged();
            }
        }
        public string RefSearchTerm
        {
            get { return _refSearchTerm; }
            set
            {
                _refSearchTerm = value;
                RaisePropertyChanged();
            }
        }

        #endregion
        #region Base


        public override void ControlState(ControllerStates state)
        {
            _controllerStates = state;
            switch (state)
            {
                case ControllerStates.Blank:
                    _canSave = true;
                    _canPrint = false;
                    _canSearch = true;
                    break;
                case ControllerStates.Edited:
                    _canSave = true;
                    _canPrint = false;
                    _canSearch = false;
                    break;
                case ControllerStates.Saved:
                    _canSave = true;
                    _canPrint = true;
                    _canSearch = false;
                    break;
                case ControllerStates.Loaded:
                    break;
            }
        }

        protected override bool CanClear()
        {
            return !(_controllerStates == ControllerStates.Edited);
        }

        protected override bool CanDelete()
        {
            throw new NotImplementedException();
        }

        protected override bool CanPrint()
        {
            return _canPrint;
        }

        protected override bool CanSave()
        {
            return _canSave;
        }

        protected override bool CanSearch()
        {
           return _canSearch;
        }

        protected override void ClearView()
        {
            if (!CanClear())
            {
                string msg = "Exit Without Save?";
                
                if (!Helper.UserConfirmed(msg))
                {
                    return;
                }

            }
            Id = "";
            RefId = "";
            DocDate = "";
            Subject = "";
            Destination = null;
            DocPath = "";
            DocStatus = DocRecordStatus.Open;

        }

        protected override void Delete()
        {
            throw new NotImplementedException();
        }

        protected override void Print()
        {
            
        }

        protected override void Save()
        {
            if (!IsValid())
            {
                string msg = string.Empty;
                foreach (var error in _errors)
                {
                    msg += error;
                    msg += "\n";

                }
                Helper.ShowMessage(msg);
                return;
            }
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    DocRecord record = unit.DocRecords.GetById(Id);
                     if (record == null)
                    {
                        record = CreateNewDocRecord();
                        unit.DocRecords.Add(record);
                    }
                    else
                    {
                        UpdateDocRecord(record);
                    }
                    unit.Save();
                    ControlState(ControllerStates.Saved);

                }
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
        }

        protected override void Search()
        {
            if (string.IsNullOrEmpty(IdSearchTerm) && string.IsNullOrEmpty(RefSearchTerm)) return;
            var criteria = BuildCriteria();
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var result = unit.DocRecords.Query(criteria);
            }
            
            
        }
        #endregion
        #region Methods
        private Expression<Func<DocRecord, bool>> BuildCriteria()
        {
            Expression<Func<DocRecord, bool>> p = x => x.Id == IdSearchTerm;
            Expression<Func<DocRecord, bool>> p2 = c => c.RefId == RefSearchTerm;
            Expression<Func<DocRecord, bool>> resultedExpression = null;
            Expression<Func<DocRecord, bool>> idSearchExpression = null;
            Expression<Func<DocRecord, bool>> refSearchExpression = null;
            if (!string.IsNullOrEmpty(IdSearchTerm))
            {
                idSearchExpression = Expression.Lambda<Func<DocRecord, bool>>(p.Body, p.Parameters[0]);
                resultedExpression = idSearchExpression;
            }
            if (!string.IsNullOrEmpty(RefId))
            {
                if (idSearchExpression != null)
                {

                    refSearchExpression = Expression.Lambda<Func<DocRecord, bool>>(p2.Body, p2.Parameters[0]);
                    resultedExpression = Expression.Lambda<Func<DocRecord, bool>>(Expression.AndAlso(idSearchExpression, refSearchExpression));
                }
                else
                {
                    refSearchExpression = Expression.Lambda<Func<DocRecord, bool>>(p2.Body, p2.Parameters[0]);
                    resultedExpression = refSearchExpression;
                }
            }
            return resultedExpression;
        }

        private bool IsValid()
        {
            _errors.Clear();
            bool isValid = true;
            if (string.IsNullOrEmpty(Id))
            {
                string msg = "ادخل رقم المعاملة";
                _errors.Add(msg);
                isValid = false;
            }
            
            if (string.IsNullOrEmpty(DocDate))
            {
                string msg = "ادخل التاريخ";
                _errors.Add(msg);
                isValid = false;
            }
            if (string.IsNullOrEmpty(Subject))
            {
                string msg = "ادخل الموضوع";
                _errors.Add(msg);
                isValid = false;
            }
            if (Destination == null)
            {
                string msg = "ادخل الجهة";
                _errors.Add(msg);
                isValid = false;
            }
            return isValid;
        }
        void ShowDocRecord(DocRecord docRecord)
        {
            Id = docRecord.Id;
            RefId = docRecord.RefId;
            DocDate = docRecord.DocDate;
            Subject = docRecord.Subject;
            Destination = docRecord.Destination;
            DocPath = docRecord.DocPath;
            DocStatus = docRecord.DocStatus;

        }

        DocRecord CreateNewDocRecord()
        {
            DocRecord docRecord = new DocRecord();
            docRecord.Id = Id;
            docRecord.RefId = RefId;
            docRecord.DocDate = DocDate;
            docRecord.Subject = Subject;
            docRecord.Destination = Destination;
            docRecord.DocPath = DocPath;
            docRecord.DocStatus = DocStatus;
            docRecord.SecurityLevel = 0;
            return docRecord;
        }
        void UpdateDocRecord(DocRecord docRecord)
        {
            docRecord.Id = Id;
            docRecord.RefId = RefId;
            docRecord.DocDate = DocDate;
            docRecord.Subject = Subject;
            docRecord.Destination = Destination;
            docRecord.DocPath = DocPath;
            docRecord.DocStatus = DocStatus;
            docRecord.SecurityLevel = 0;
        }
        
        #endregion

    }
}
