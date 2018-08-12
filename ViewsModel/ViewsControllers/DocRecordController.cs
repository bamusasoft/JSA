using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Jsa.DomainModel.Repositories;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class DocRecordController : EditableControllerBase
    {

        public DocRecordController()
        {
            Initialize();
            LoadDestinations();
            ControlState(ControllerStates.Blank);
        }
        private void Initialize()
        {
            
            Errors = new Dictionary<string, List<string>>();
            DocStatuses = new Dictionary<DocRecordStatus, string>();
            DocStatuses.Add(DocRecordStatus.Open, "تحت الإجراء");
            DocStatuses.Add(DocRecordStatus.Closed, "منتهية");
            DocStatuses.Add(DocRecordStatus.Hold, "متوقفة");
            SelectedDocStatus = DocStatuses.FirstOrDefault();

        }
        private async Task LoadDestinations()
        {
            Task<List<Destination>> task = null;
            try
            {
                task = LoadDesintationsAsync();
                List<Destination> result = await task;
                DocDestinations = new ObservableCollection<Destination>(result);
                AddEmptyDestination(DocDestinations);
                Destination = DocDestinations.Where(x => x.Id == -1).SingleOrDefault();
            }
            catch(Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }
        private Task<List<Destination>> LoadDesintationsAsync()
        {
            Task<List<Destination>> task = Task.Run(() =>
            {
                List<Destination> destinations = null;
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    destinations = unit.Destinations.GetAll().ToList();
                }
                return destinations;
            });

            return task;
        }
        #region Fields
        string _id;
        string _refId;
        string _docDate;
        string _subject;
        Destination _destination;
        string _docPath;
        DocRecordStatus _docStatus;
        ObservableCollection<Destination> _docDestinations;
        Dictionary<DocRecordStatus, string> _docStatuses;
        KeyValuePair<DocRecordStatus, string> _selectedStatus;
        ControllerStates _controllerStates;
        string _idSearchTerm;
        string _refSearchTerm;
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
                Console.WriteLine("In Destination Property");
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

        public ObservableCollection<Destination> DocDestinations
        {
            get { return _docDestinations; }
            set
            {
                _docDestinations = value;
                RaisePropertyChanged("DocDestinations");
            }
        }
        public Dictionary<DocRecordStatus, string> DocStatuses
        {
            get { return _docStatuses; }
            set
            {
                _docStatuses = value;
                RaisePropertyChanged("DocStatuses");
            }
        }
        public KeyValuePair<DocRecordStatus, string> SelectedDocStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                RaisePropertyChanged("SelectedDocStatus");

            }
        }
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
            return true;
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
            if (_controllerStates == ControllerStates.Edited)
            {
                string msg = Properties.Resources.SavePrompetMsg;
                
                if (!Helper.UserConfirmed(msg))
                {
                    return;
                }

            }
            Id = "";
            RefId = "";
            DocDate = "";
            Subject = "";
            Destination = DocDestinations.Where(x => x.Id == -1).FirstOrDefault();
            DocPath = "";
            SelectedDocStatus = DocStatuses.SingleOrDefault(x => x.Key == DocRecordStatus.Open);
            ControlState(ControllerStates.Blank);

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
                foreach (var error in Errors)
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
            DocRecord resultDoc = null;
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var result = unit.DocRecords.Query(criteria);
                resultDoc = result.FirstOrDefault();
            }
            if(resultDoc != null)
            {
                ShowDocRecord(resultDoc);
                ControlState(ControllerStates.Saved);
            }
            
            
        }
        #endregion
        #region Methods

        private Expression<Func<DocRecord, bool>> BuildCriteria()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(DocRecord), "docRecord");

            Expression idSearchValue = Expression.Constant(IdSearchTerm, typeof(string));
            Expression refIdSearchValue = Expression.Constant(RefSearchTerm, typeof(string));

            Expression idProperty = Expression.PropertyOrField(parameter, "Id");
            Expression idRefProperty = Expression.PropertyOrField(parameter, "RefId");

            
            Expression resultedExpression = null;

            if (!string.IsNullOrEmpty(IdSearchTerm))
            {
                Expression idSearchExpression = Expression.Equal(idProperty, idSearchValue);

                resultedExpression = idSearchExpression;
            }
            if (!string.IsNullOrEmpty(RefSearchTerm))
            {
                Expression refIdSearchExpression = Expression.Equal(idRefProperty, refIdSearchValue);
                if (resultedExpression != null)
                {

                    resultedExpression = Expression.AndAlso(resultedExpression, refIdSearchExpression);
                }
                else
                {
                    resultedExpression = refIdSearchExpression;
                }
            }
            Expression<Func<DocRecord, bool>> criteria = null;
            if(resultedExpression != null)
            {
                criteria = Expression.Lambda<Func<DocRecord, bool>>(resultedExpression, parameter);
            }
            return criteria;
        }

        private bool IsValid()
        {
            Errors.Clear();
            bool isValid = true;
            if (string.IsNullOrEmpty(Id))
            {
                string msg = "ادخل رقم المعاملة";
                AddError("Id", msg);
                isValid = false;
            }
            
            if (string.IsNullOrEmpty(DocDate))
            {
                string msg = "ادخل التاريخ";
                AddError("DocDate", msg);
                isValid = false;
            }
            if (string.IsNullOrEmpty(Subject))
            {
                string msg = "ادخل الموضوع";
                AddError("Subject", msg);
                isValid = false;
            }
            if (Destination == null || Destination.Id == -1)
            {
                string msg = "ادخل الجهة";
                AddError("Destination", msg);
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
            SelectedDocStatus = DocStatuses.SingleOrDefault(x => x.Key == docRecord.DocStatus);

        }

        DocRecord CreateNewDocRecord()
        {
            DocRecord docRecord = new DocRecord();
            docRecord.Id = Id;
            docRecord.RefId = RefId;
            docRecord.DocDate = DocDate;
            docRecord.Subject = Subject;
            docRecord.DestId = Destination.Id;
            docRecord.DocPath = DocPath;
            docRecord.DocStatus = SelectedDocStatus.Key;
            docRecord.SecurityLevel = 0;
            return docRecord;
        }
        void UpdateDocRecord(DocRecord docRecord)
        {
            docRecord.Id = Id;
            docRecord.RefId = RefId;
            docRecord.DocDate = DocDate;
            docRecord.Subject = Subject;
            docRecord.DestId = Destination.Id;
            docRecord.DocPath = DocPath;
            docRecord.DocStatus = SelectedDocStatus.Key;
            docRecord.SecurityLevel = 0;
        }
        /// <summary>
        /// Use this method to add a reset item, so user has the choice to not selected any of the item in combox.
        /// The id of added item is -1 which you can use to test for that.
        /// </summary>
        /// <param name="list"></param>
        private void AddEmptyDestination(ObservableCollection<Destination> list)
        {
            Destination d = new Destination();
            d.Id = -1;
            d.Description = "اختر جهة";
            list.Insert(0, d);
        }
        #endregion

        #region Public Methods
        public void GenerateDocRecordNo()
        {
                try
                {
                    
                    string max;
                    using (IUnitOfWork unit = new UnitOfWork())
                    {
                    max = ((DocRecordRepository)unit.DocRecords).GetMaxDocRecordNo();
                    }
                    Id = Helper.GenerateOutboxNo(max);
                    DocDate = Helper.GetCurrentDate();
                }
                catch (Exception ex)
                {
                    Helper.LogShowError(ex);
                }
            
        }
        public bool CanExit()
        {
            return _controllerStates != ControllerStates.Edited;
        }
        #endregion

    }
}
