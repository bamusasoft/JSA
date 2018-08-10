using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class DocRecordFollowController : EditableControllerBase
    {
        public DocRecordFollowController()
        {
            Errors = new Dictionary<string, List<string>>();
            
            ControlState(ControllerStates.Blank);
        }

        #region Fields
        string _followId;
        string _docId;
        string _refId;
        string _docDate;
        string _subject;
        string _followDate;
        string _followContent;
        string _followPath;
        ObservableCollection<DocRecordFollow> _docFollows;

        //
        string _idSearchTerm;
        string _refSearchTerm;
        //
        bool _canSave;
        bool _canPrint;
        bool _canSearch;
        //
        ControllerStates _controllerStates;
        //
        private RelayCommand _clearFollowCommand;

        
        #endregion

        #region Properties
        public string FollowId
        {
            get { return _followId; }
            set
            {
                _followId = value;
            }
        }
        public string DocId
        {
            get { return _docId; }
            set
            {
                _docId = value;
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

        public string FollowDate
        {
            get { return _followDate; }
            set
            {
                _followDate = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string FollowContent
        {
            get { return _followContent; }
            set
            {
                _followContent = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string FollowPath
        {
            get { return _followPath; }
            set
            {
                _followPath = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public ObservableCollection<DocRecordFollow> DocFollows
        {
            get { return _docFollows; }
            set
            {
                _docFollows = value;
                RaisePropertyChanged();
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

        #region Commands
        public ICommand ClearFollowCommand
        {
            get { return _clearFollowCommand ?? (_clearFollowCommand = new RelayCommand(ClearFollow)); }
        }
        private void ClearFollow()
        {
            FollowId = "";
            FollowContent = "";
            FollowDate = "";
            FollowPath = null;
            ControlState(ControllerStates.Saved);

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
                    RefreshFollows();
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
            FollowId = "";
            DocId = "";
            RefId = "";
            DocDate = "";
            Subject = "";
            FollowContent = "";
            FollowDate = "";
            FollowPath = null;
            DocFollows.Clear();
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
                foreach (var error in Errors.Values)
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
                    DocRecordFollow follow = null;
                    if (string.IsNullOrEmpty(FollowId))
                    {
                        follow = CreateNewDocFollow();
                        unit.DocRecordFollows.Add(follow);
                    }
                     else
                    {
                        follow = unit.DocRecordFollows.GetById(FollowId);
                        UpdateDocFollow(follow);
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
                if (resultDoc != null)
                {
                    ShowDocRecord(resultDoc);
                    var sortedFollows = resultDoc.DocRecordFollows.OrderBy(x => x.FollowDate);
                    DocFollows = new ObservableCollection<DocRecordFollow>(sortedFollows);
                    ControlState(ControllerStates.Saved);
                }
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
            if (resultedExpression != null)
            {
                criteria = Expression.Lambda<Func<DocRecord, bool>>(resultedExpression, parameter);
            }
            return criteria;
        }

        private bool IsValid()
        {
            Errors.Clear();
            bool isValid = true;
            if (string.IsNullOrEmpty(DocId))
            {
                string msg = "يجب فتح معاملة أولاً";
                AddError("FollowContent", msg);
                isValid = false;
            }

            if (string.IsNullOrEmpty(FollowContent))
            {
                string msg = "ادخل متابعة للمعاملة";
                AddError("FollowContent", msg);
                isValid = false;
            }

            if (string.IsNullOrEmpty(FollowDate))
            {
                string msg = "ادخل التاريخ";
                AddError("FollowDate", msg);
                isValid = false;
            }
            
            return isValid;
        }
        private DocRecordFollow CreateNewDocFollow()
        {
            DocRecordFollow follow = new DocRecordFollow();
            follow.Id = GenerateFollowId(DocId);
            follow.DocRecodId = DocId;
            follow.FollowContent = FollowContent;
            follow.FollowDate = FollowDate;
            follow.FollowPath = FollowPath;
            return follow;
        }
        private void UpdateDocFollow(DocRecordFollow follow)
        {
            follow.FollowContent = FollowContent;
            follow.FollowDate = FollowDate;
            follow.FollowPath = FollowPath;
        }
        private void ShowDocRecord(DocRecord docRecord)
        {
            DocId = docRecord.Id;
            RefId = docRecord.RefId;
            DocDate = docRecord.DocDate;
            Subject = docRecord.Subject;
            
        }
        private void RefreshFollows()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var follows = unit.DocRecordFollows.Query(x => x.DocRecodId == DocId)
                    .OrderBy(p => p.FollowDate)
                    .ToList();
                DocFollows = new ObservableCollection<DocRecordFollow>(follows);
            }
        }
        private string GenerateFollowId(string docId)
        {
            string generateFollowId;
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var max = ((DocRecordFollowRepository)unit.DocRecordFollows).GetMaxDocFollowNo(docId);
                if (string.IsNullOrEmpty(max))
                {
                    generateFollowId = docId + "001";
                }
                else
                {
                    generateFollowId = Helper.AppendGeneratedNo(max, 8);
                }
            }
            return generateFollowId;
        }

        #endregion

        #region Public Methods
        public void OnSelectedFollowChanged(DocRecordFollow recordFollow)
        {
            FollowId = recordFollow.Id;
            FollowDate = recordFollow.FollowDate;
            FollowContent = recordFollow.FollowContent;
            FollowPath = recordFollow.FollowPath;
            ControlState(ControllerStates.Saved);
        }
        #endregion
    }

}
