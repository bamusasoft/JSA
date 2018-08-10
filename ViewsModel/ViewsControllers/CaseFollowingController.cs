using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Reports;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class CaseFollowingController : EditableControllerBase
    {

        #region Constcustors

        public CaseFollowingController()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        #endregion

        #region Fields

        private int _id;
        private int _caseNo;
        private string _followingDate;
        private string _followingDescription;
        private string _nextFollowingDate;
        private int _searchField;

        //
        private bool _canSave;
        private bool _canPrint;
        private bool _canDelete;
        private bool _canSearch;
        private bool _canAddNew;
        private bool _caseNoEnabled;
        private string _nextFollowingDestination;
        private string _followingDestination;
        private ObservableCollection<CaseFollowing> _followings;
        private LegalCase _legalCase;
        private ControllerStates _controllerState;
        private bool _listEnabled;
        private bool _hasChanges;
        private RelayCommand _addNewCommand;
        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        public int CaseNo
        {
            get { return _caseNo; }
            set
            {
                _caseNo = value;
                RaisePropertyChanged();
            }
        }

        public string FollowingDate
        {
            get { return _followingDate; }
            set
            {
                _followingDate = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public DateTime GregFollowingDate
        {
            get { return Helper.ConvertToGregDate(FollowingDate); }
        }

        public string FollowingDestination
        {
            get { return _followingDestination; }
            set
            {
                _followingDestination = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public string FollowingDescription
        {
            get { return _followingDescription; }
            set
            {
                _followingDescription = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }

        }

        public string NextFollowingDate
        {
            get { return _nextFollowingDate; }
            set
            {
                _nextFollowingDate = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public DateTime GregNextFollowingDate
        {
            get { return Helper.ConvertToGregDate(NextFollowingDate); }
        }

        public string NextFollowingDestination
        {
            get { return _nextFollowingDestination; }
            set
            {
                _nextFollowingDestination = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }

        }

        public ObservableCollection<CaseFollowing> Followings
        {
            get { return _followings; }
            set
            {
                _followings = value;
                RaisePropertyChanged();
            }
        }

        public LegalCase LegalCase
        {
            get { return _legalCase; }
            set
            {
                _legalCase = value;
                RaisePropertyChanged();
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

        public ControllerStates ControllerState
        {
            get { return _controllerState; }
            set
            {
                _controllerState = value;
                RaisePropertyChanged();
            }
        }

        public bool ListEnabled
        {
            get { return _listEnabled; }
            set
            {
                _listEnabled = value;
                RaisePropertyChanged();
            }
            
        }

        public bool CaseNoEnabled
        {
            get { return _caseNoEnabled; }
            set
            {
                _caseNoEnabled = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Methods

        public void LoadExisted(int followingId)
        {
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {

                    var following = unit.CaseFollowings.GetById(followingId);
                    FillFromExisted(following);
                    ControlState(ControllerStates.Saved);
                }

            }
            catch (Exception ex)
            //Here means the object not found or there is a problem get the object. in either case, log and inform the user
            {

                Helper.LogShowError(ex);
                //ToDo: Revise this when you complete a full implentation of how error messages ineract with the view.
                RaiseContorllerChanged(ControllerAction.Invalid);
                ControlState(ControllerStates.Blank);
            }

        }

        private void FillFromExisted(CaseFollowing following)
        {
            Id = following.Id;
            CaseNo = following.CaseNo;
            FollowingDate = following.FollowingDate;
            FollowingDestination = following.FollowingDestination;
            FollowingDescription = following.FollowingDescription;
            NextFollowingDate = following.NextFollowingDate;
            NextFollowingDestination = following.NextFollowingDestination;
            LegalCase = following.LegalCase;

        }

        public void CreateNew()
        {
            ControlState(ControllerStates.Blank);
        }
        public void CreateNew(int caseNo)
        {
            var cf = LoadCaseFollowings(caseNo);
            if (cf != null) //Only if there is existence followings
            {
                Followings = new ObservableCollection<CaseFollowing>(cf);
                CaseNo = caseNo;
                ControlState(ControllerStates.Loaded);
                
            }
        }

        private IEnumerable<CaseFollowing> LoadCaseFollowings(int caseNo)
        {
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var legalCase = unit.LegalCases.GetById(caseNo);
                    LegalCase = legalCase;
                    return ((CaseFollowingRepository) unit.CaseFollowings).GetCaseFollowing(caseNo);
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
            if (LegalCase == null || LegalCase.CaseNo != CaseNo)
            {
                AddError("CaseNo", CASENOERROR);
                isValid = false;
            }
            else
            {
                RemoveError("CaseNo", CASENOERROR);

            }
            if (string.IsNullOrEmpty(FollowingDate) || !(Helper.ValidDate(FollowingDate)))
            {
                AddError("FollowingDate", FOLLOWINGDATEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("FollowingDate", FOLLOWINGDATEERROR);

            }
            if (string.IsNullOrEmpty(NextFollowingDate) || !(Helper.ValidDate(NextFollowingDate)))
            {
                AddError("NextFollowingDate", FOLLOWINGDATEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("NextFollowingDate", FOLLOWINGDATEERROR);

            }
            if (string.IsNullOrEmpty(FollowingDestination))
            {
                AddError("FollowingDestination", DESTERROR);
                isValid = false;
            }
            else
            {
                RemoveError("FollowingDestination", DESTERROR);

            }
            if (string.IsNullOrEmpty(NextFollowingDestination))
            {
                AddError("NextFollowingDestination", DESTERROR);
                isValid = false;
            }
            else
            {
                RemoveError("NextFollowingDestination", DESTERROR);

            }
            if (string.IsNullOrEmpty(FollowingDescription))
            {
                AddError("FollowingDescription", RESULTERROR);
                isValid = false;
            }
            else
            {
                RemoveError("FollowingDescription", RESULTERROR);

            }

            return isValid;
        }

        private void AddNew(CaseFollowing following, IUnitOfWork db)
        {
            if (following == null) throw new ArgumentNullException("following");
            if (db == null) throw new ArgumentNullException("db");
            following.Id = Id;
            following.CaseNo = CaseNo;
            following.FollowingDate = FollowingDate;
            following.GregFollowingDate = GregFollowingDate;
            following.FollowingDestination = FollowingDestination;
            following.FollowingDescription = FollowingDescription;
            following.NextFollowingDate = NextFollowingDate;
            following.GregNextFollowingDate = GregNextFollowingDate;
            following.NextFollowingDestination = NextFollowingDestination;

            db.CaseFollowings.Add(following);
        }

        private void Update(CaseFollowing following)
        {
            following.CaseNo = CaseNo;
            following.FollowingDate = FollowingDate;
            following.GregFollowingDate = GregFollowingDate;
            following.FollowingDestination = FollowingDestination;
            following.FollowingDescription = FollowingDescription;
            following.NextFollowingDate = NextFollowingDate;
            following.GregNextFollowingDate = GregNextFollowingDate;
            following.NextFollowingDestination = NextFollowingDestination;
        }

        private void RefreshCaseFollowings()
        {
            Followings = new ObservableCollection<CaseFollowing>(LoadCaseFollowings(CaseNo));
        }

        private void ClearUiValues()
        {
            Id = 0;
            FollowingDate = "";
            FollowingDestination = "";
            FollowingDescription = "";
            NextFollowingDate = "";
            NextFollowingDestination = "";
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
                    ListEnabled = true;
                    _hasChanges = false;
                    _canPrint = false;
                    _canAddNew = false;
                    CaseNoEnabled = true;
                     break;
                case ControllerStates.Edited:
                    _canSearch = false;
                    ListEnabled = false;
                    _hasChanges = true;
                    _canPrint = false;
                    _canAddNew = true;
                    CaseNoEnabled = false;
                    break;
                case ControllerStates.Saved:
                    _canSave = true;
                    _canSearch = false;
                    RefreshCaseFollowings();
                    ListEnabled = false;
                    _hasChanges = false;
                    _canPrint = true;
                    _canAddNew = true;
                    CaseNoEnabled = false;
                    break;
                case ControllerStates.Loaded:
                    ListEnabled = true;
                    _hasChanges = false;
                    _canPrint = true;
                    _canAddNew = true;
                    CaseNoEnabled = false;
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
                    Helper.ShowMessage("Error");
                    return;
                }
                using (IUnitOfWork db = new UnitOfWork())
                {
                    CaseFollowing following;
                    if (!((CaseFollowingRepository)db.CaseFollowings).TryCheckExist(Id, out following))
                    {
                        AddNew(following, db);
                    }
                    else
                    {
                        Update(following);
                    }
                    db.Save();
                    Id = following.Id; //Sync with the generated database Id.
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
            var source = FillData().ToList();
            int caseNo = CaseNo;
            var appoint = LegalCase.CaseAppointments.LastOrDefault();
            string nextAppoint = "";
            if (appoint != null)
            {
                nextAppoint = appoint.AppointmentDate;
            }
            string defendant = LegalCase.Defendant;
            ExcelProperties excelProp= new ExcelProperties(4,1, false);
            string path = @"C:\BaMusaSoft\JeddahStationProject\Templates\CaseFollowings.xltx";
            FollowingsReport report = new FollowingsReport(source, caseNo, defendant, nextAppoint, path, excelProp);
            report.Print();
        }

        IEnumerable<FollowingReportFields> FillData()
        {
            return Followings.Select(following => new FollowingReportFields(following.FollowingDate, following.FollowingDestination,
                following.FollowingDescription));
        }

        protected override bool CanPrint()
        {
            return _canPrint;
        }

        protected override void Search()
        {
            if (SearchField <= 0) return;
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

        #region Commands

        public ICommand AddNewCommand
        {
            get { return _addNewCommand ?? new RelayCommand(AddNew, CanAddNew); }
        }

        private void AddNew()
        {
            if (_hasChanges && !Helper.UserConfirmed(Properties.Resources.SavePrompetMsg))
            {
                return;
            }
            ClearUiValues();
            ControlState(ControllerStates.Loaded);
        }

        private bool CanAddNew()
        {
            return _canAddNew;
        }
        #endregion
        #region ErrorsMessages

        private const string CASENOERROR = "حدد القضية";
        private const string FOLLOWINGDATEERROR = "التاريخ غير صحيح";
        private const string DESTERROR = "الجهة مطلوبة";
        private const string RESULTERROR = "النتيجة مطلوبة";

        #endregion
    }
}

