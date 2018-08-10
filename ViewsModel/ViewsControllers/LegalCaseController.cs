using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class LegalCaseController : EditableControllerBase
    {

        #region Constcustors

        public LegalCaseController()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        #endregion
        #region Fields

        private int _caseNo;
        private string _registeredAt;
        private string _defendant;
        private string _description;
        private int _statusId;
        private ControllerStates _state;
        private ObservableCollection<CaseStatus> _caseStatuses;
        private CaseStatus _caseStatus;
        private int _searchField;
        private bool _hasChanges;

        //
        private bool _canSave;
        private bool _canDelete;
        private bool _canSearch;

        #endregion
        #region Properties
        public int CaseNo
        {
            get { return _caseNo; }
            set
            {
                _caseNo = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public string RegisteredAt
        {
            get { return _registeredAt; }
            set
            {
                _registeredAt = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public DateTime GregDate
        {
            get { return Helper.ConvertToGregDate(RegisteredAt); }
        }

        public string Defendant
        {
            get { return _defendant; }
            set
            {
                _defendant = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);

            }

        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public int StatusId
        {
            get { return _statusId; }
            set
            {
                _statusId = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);

            }
        }
        public ControllerStates ControllerState
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CaseStatus> CaseStatuses
        {
            get { return _caseStatuses; }
            set
            {
                _caseStatuses = value;
                RaisePropertyChanged();
            }
        }

        public CaseStatus CaseStatus
        {
            get { return _caseStatus; }
            set
            {
                _caseStatus = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public int SearchField
        {
            get { return _searchField; }
            set
            {
                _searchField = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #region Methods
        public void LoadExisted(int caseNo)
        {
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {

                    var legalCase = unit.LegalCases.GetById(caseNo);
                    FillFromExisted(legalCase);
                    ControlState(ControllerStates.Saved);
                }

            }
            catch (Exception ex) //Here means the object not found or there is a problem get the object. in either case, log and inform the user
            {

                Helper.LogShowError(ex);
                //ToDo: Revise this when you complete a full implentation of how error messages ineract with the view.
                RaiseContorllerChanged(ControllerAction.Invalid);
                ControlState(ControllerStates.Blank);
            }

        }

        private void FillFromExisted(LegalCase legalCase)
        {
            CaseNo = legalCase.CaseNo;
            RegisteredAt = legalCase.RegisteredAt;
            Defendant = legalCase.Defendant;
            Description = legalCase.Description;
            StatusId = legalCase.StatusId;
            CaseStatus = legalCase.CaseStatus;
        }

        public void CreateNew()
        {

            CaseStatuses = new ObservableCollection<CaseStatus>(LoadCaseStatuses());
            ControlState(ControllerStates.Blank);
        }

        private IEnumerable<CaseStatus> LoadCaseStatuses()
        {
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    return unit.CaseStatuses.GetAll();
                }
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
            return null;
        }

        private bool IsValid()
        {
            bool isValid = true;
            if (CaseNo <= 0)
            {
                AddError("CaseNo", CASENOERROR);
                isValid = false;
            }
            else
            {
                RemoveError("CaseNo", CASENOERROR);

            }
            if (string.IsNullOrEmpty(RegisteredAt) || !(Helper.ValidDate(RegisteredAt)))
            {
                AddError("RegisteredAt", REGISTEREDERROR);
                isValid = false;
            }
            else
            {
                RemoveError("RegisteredAt", REGISTEREDERROR);

            }
            if (string.IsNullOrEmpty(Defendant))
            {
                AddError("Defendant", DEFENDANTERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Defendant", DEFENDANTERROR);

            }
            if (string.IsNullOrEmpty(Description))
            {
                AddError("Description", DESCRIPTIONERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Description", DESCRIPTIONERROR);

            }
            if ( CaseStatus == null)
            {
                AddError("CaseStatus", STATUSERROR);
                isValid = false;
            }
            else
            {
                RemoveError("CaseStatus", STATUSERROR);

            }
            return isValid;
        }

        private void AddNew(LegalCase legalCase, IUnitOfWork db)
        {
            if (legalCase == null) throw new ArgumentNullException("legalCase");
            if (db == null) throw new ArgumentNullException("db");
            legalCase.CaseNo = CaseNo;
            legalCase.RegisteredAt = RegisteredAt;
            legalCase.GregDate = GregDate;
            legalCase.Defendant = Defendant;
            legalCase.Description = Description;
            legalCase.StatusId = CaseStatus.Id;
            db.LegalCases.Add(legalCase);
        }

        private void Update(LegalCase legalCase)
        {
            legalCase.RegisteredAt = RegisteredAt;
            legalCase.GregDate = GregDate;
            legalCase.Defendant = Defendant;
            legalCase.Description = Description;
            legalCase.StatusId = CaseStatus.Id;
        }

        public void CheckExitence()
        {
            try
            {
                using (IUnitOfWork db = new UnitOfWork())
                {
                    int caseNo = CaseNo;
                    LegalCase lc;
                    if (((LegalCaseRepository)db.LegalCases).TryCheckExist(caseNo, out lc))
                    {
                        FillFromExisted(lc);
                        ControlState(ControllerStates.Saved);
                    }
                }

            }
            catch (Exception ex)
            {
                
                Helper.LogShowError(ex);
            }
        }

        #endregion
        #region Base



        public override void ControlState(ControllerStates state)
        {
            ControllerState = state;
            switch (state)
            {
                case ControllerStates.Blank:
                    _canSave = true;
                    _canSearch = true;
                    _hasChanges = false;
                    break;
                case ControllerStates.Edited:
                    _hasChanges = true;
                    _canSearch = false;
                    break;
                case ControllerStates.Saved:
                    _canSave = true;
                    _canSearch = false;
                    _hasChanges = false;
                    break;
                case ControllerStates.Loaded:
                    _hasChanges = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }

        protected override void ClearView()
        {
            if (_hasChanges && !Helper.UserConfirmed(Properties.Resources.SavePrompetMsg))
            {
                return;
            }

            ControlState(ControllerStates.Blank);
            RaiseContorllerChanged(ControllerAction.Cleared);
        }

        protected override bool CanClear()
        {
            return true;
        }

        protected override void Save()
        {
            try
            {
                if (!IsValid())
                {
                    return;
                }
                using (IUnitOfWork db = new UnitOfWork())
                {
                    LegalCase legalCase;
                    if (!((LegalCaseRepository)db.LegalCases).TryCheckExist(CaseNo, out legalCase))
                    {
                        AddNew(legalCase, db);
                    }
                    else
                    {
                        Update(legalCase);
                    }
                    db.Save();
                    ControlState(ControllerStates.Saved);
                }
                
            }
            catch (Exception ex)
            {
                
                Helper.LogShowError(ex);
            }
            

        }

        protected override bool CanSave()
        {
            return _canSave;
        }

        protected override void Print()
        {
            throw new System.NotImplementedException();
        }

        protected override bool CanPrint()
        {
            throw new System.NotImplementedException();
        }

        protected override void Search()
        {
            if(SearchField <= 0)return;
            LoadExisted(SearchField);
        }

        protected override bool CanSearch()
        {
            return _canSearch;
        }

        protected override void Delete()
        {
            throw new System.NotImplementedException();
        }

        protected override bool CanDelete()
        {
            return _canDelete;
        }
        #endregion

        #region ErrorsMessages
         private const string CASENOERROR = "رقم القضية مطلوب";
         private const string REGISTEREDERROR = "تاريخ القيد مطلوب";
         private const string DEFENDANTERROR = "المدعى عليه مطلوب";
         private const string DESCRIPTIONERROR = "وصف القضية مطلوب";
         private const string STATUSERROR = "حالة القضية مطلوبة";
        #endregion
    }
}