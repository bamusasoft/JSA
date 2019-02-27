using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Mediator;
using Jsa.ViewsModel.Reports;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class DocRecordFollowController : EditableControllerBase
    {
        public DocRecordFollowController(OpenDialogProxy dialog)
        {
            _dialog = dialog;
            _docRecordFolder = ViewsModel.Properties.Settings.Default.DocFileFolder;
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
        string _docRecordFolder;
        ObservableCollection<DocRecordFollow> _docFollows;
        public event EventHandler<string> DocFilePathChanged;
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
        private RelayCommand _openDocFileCommand;
        //
        readonly OpenDialogProxy _dialog;
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
                RaiseFilePathChanged(value);
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

        public ICommand OpenDocFileCommand
        {
            get { return _openDocFileCommand ?? (_openDocFileCommand = new RelayCommand(OpenDocFile)); }
        }

        private void OpenDocFile()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var docFiles = new List<DocRecordFile>();// unit.DocRecordFiles.Query(x => x.DocRecordId == DocId).ToList();
                _dialog.RaiseOpenDialog<string>(DocId);

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
                    DocFollows = new ObservableCollection<DocRecordFollow>();
                    RaiseFilePathChanged(string.Empty);
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
            string path = Properties.Settings.Default.DocFollowTemplate;
            if (string.IsNullOrEmpty(path))
            {
                string msg = "يجب تحديد مسار تقرير المتابعة";
                Helper.ShowMessage(msg);
                return;
            }
            try
            {
                DocRecord currentDoc = null;
                using (IUnitOfWork unit = new UnitOfWork())
                {

                    currentDoc = unit.DocRecords.GetById(DocId);

                }
                if (currentDoc != null)
                {
                    List<DocFollowsReport> source = new List<DocFollowsReport>();
                    if (currentDoc.DocRecordFollows.Count > 0)
                    {
                        var follows = currentDoc.DocRecordFollows.OrderBy(x => x.FollowDate);
                        foreach (var follow in follows)
                        {
                            DocFollowsReport row = new DocFollowsReport();
                            row.DocId = currentDoc.Id;
                            row.DocDate = currentDoc.DocDate;
                            row.Destination = currentDoc.Destination.Description;
                            row.Subject = currentDoc.Subject;
                            row.DocStatus = currentDoc.DocStatus;
                            row.FollowDate = follow.FollowDate;
                            row.FollowContent = follow.FollowContent;
                            source.Add(row);
                        }

                    }
                    else
                    {
                        DocFollowsReport header = new DocFollowsReport();
                        header.DocId = currentDoc.Id;
                        header.DocDate = currentDoc.DocDate;
                        header.Destination = currentDoc.Destination.Description;
                        header.Subject = currentDoc.Subject;
                        header.DocStatus = currentDoc.DocStatus;
                        source.Add(header);
                    }

                    ExcelProperties excelProp = new ExcelProperties(2, 1, false);
                    DocRecordFollowPrintReport report = new DocRecordFollowPrintReport(source, path, excelProp);
                    report.Print();
                }


            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
        }


        protected override void Save()
        {
            if (!IsValid())
            {
                string msg = string.Empty;
                foreach (var error in Errors)
                {
                    msg += error.Value?.FirstOrDefault() ?? "";
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
                        DocRecordFile file = CreateDocFile(DocId, follow.Id, FollowPath);
                        unit.DocRecordFiles.Add(file);
                    }
                    else
                    {
                        follow = unit.DocRecordFollows.GetById(FollowId);
                        DocRecordFile file = unit.DocRecordFiles.Query(x => x.DocFollowId == FollowId).SingleOrDefault();
                        if(file != null)
                        {
                            UpdateDocFile(file, FollowPath);
                        }
                        else
                        {
                          var docFile =  CreateDocFile(follow.DocRecodId, follow.Id, FollowPath);
                          unit.DocRecordFiles.Add(docFile);
                        }
                        UpdateDocFollow(follow);
                    }
                    unit.Save();
                    FollowId = follow.Id;
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
            DocRecord resultDoc = Find(criteria);

            if (resultDoc != null)
            {
                ShowDocRecord(resultDoc);
                ControlState(ControllerStates.Saved);
            }
        }
        #endregion

        #region Methods
        private DocRecord Find(Expression<Func<DocRecord, bool>> criteria)
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var result = unit.DocRecords.Query(criteria);
                return result.FirstOrDefault();
            }
        }
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
            bool isValid = true;
            if (string.IsNullOrEmpty(DocId))
            {
                AddError("DocId", DOCNOERROR);
                isValid = false;
            }
            else
            {
                RemoveError("DocId", DOCNOERROR);
            }

            if (string.IsNullOrEmpty(FollowContent))
            {
                AddError("FollowContent", FOLLOWCONTERROR);
                isValid = false;
            }
            else
            {
                RemoveError("FollowContent", FOLLOWCONTERROR);
            }

            if (string.IsNullOrEmpty(FollowDate))
            {
                AddError("FollowDate", FOLLOWDATERROR);
                isValid = false;
            }
            else
            {
                RemoveError("FollowDate", FOLLOWDATERROR);
            }
            if (string.IsNullOrEmpty(FollowPath))
            {
                AddError("FollowPath", FOLLOWPATHERROR);
                isValid = false;

            }
            else
            {
                RemoveError("FollowPath", FOLLOWPATHERROR);
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
            return follow;
        }
        private void UpdateDocFollow(DocRecordFollow follow)
        {
            follow.FollowContent = FollowContent;
            follow.FollowDate = FollowDate;
        }
        private DocRecordFile CreateDocFile(string docId, string followId, string path)
        {
            string newPath =  CopyDocFile(DocId, followId, FollowPath);
            return new DocRecordFile()
            {
                Id = Guid.NewGuid().ToString(),
                DocRecordId = docId,
                DocFollowId = followId,
                Path = newPath

            };
        }
        private void UpdateDocFile(DocRecordFile file, string path)
        {
            if(file.Path != path)
            {
               string newPath =  CopyDocFile(file.DocRecordId, file.DocFollowId, path);
               file.Path = newPath;

            }
        }
        private string CopyDocFile(string docId, string followId, string path)
        {
            string srcFile = path;
            string fileName = $"{docId}-{followId}-{DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss")}.pdf";
            string destFile = Path.Combine(_docRecordFolder, fileName);
            File.Copy(srcFile, destFile);
            return fileName;
        }
        private void ShowDocRecord(DocRecord docRecord)
        {
            DocId = docRecord.Id;
            RefId = docRecord.RefId;
            DocDate = docRecord.DocDate;
            Subject = docRecord.Subject;
            var sortedFollows = docRecord.DocRecordFollows.OrderBy(x => x.FollowDate);
            DocFollows = new ObservableCollection<DocRecordFollow>(sortedFollows);

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

        private void RaiseFilePathChanged(string path)
        {
            if (DocFilePathChanged == null)
            {
                return;
            }

            DocFilePathChanged(this, path);
        }
        #endregion

        #region Public Methods
        public void OnSelectedFollowChanged(DocRecordFollow recordFollow)
        {
            try
            {
                FollowId = recordFollow.Id;
                FollowDate = recordFollow.FollowDate;
                FollowContent = recordFollow.FollowContent;
                string followFilePath = GetFollowFile(FollowId);
                if (!string.IsNullOrEmpty(followFilePath))
                {
                    FollowPath = Path.Combine(_docRecordFolder, GetFollowFile(FollowId));
                }
                else
                {
                    FollowPath = string.Empty;
                }
                ControlState(ControllerStates.Saved);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
          
        }
        private string GetFollowFile(string followId)
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var file= unit.DocRecordFiles.Query(x => x.DocFollowId == followId).SingleOrDefault();
                if(file == null)
                {
                    return string.Empty;
                }
                return file.Path;

            }
        }
        public void ShowDocFollow(string docId)
        {
            var docRecord = Find(x => x.Id == docId);
            ShowDocRecord(docRecord);
        }
        #endregion

        #region Error Messaegs
        private const string DOCNOERROR = "يجب فتح معاملة أولاً";
        private const string FOLLOWCONTERROR = "ادخل محتوى المتابعة";
        private const string FOLLOWDATERROR = "ادخل تاريخ المتابعة";
        private const string FOLLOWPATHERROR = "ادخل مستند المتابعة";

        #endregion
    }

}
