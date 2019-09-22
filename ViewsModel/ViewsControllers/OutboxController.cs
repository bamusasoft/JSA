using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jsa.DomainModel;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class OutboxController : EditableControllerBase
    {
        #region Fields
        string _outboxNo;
        string _outboxDate;
        string _subject;
        string _goingTo;
        string _attachements;
        string _notes;
        bool _isDeleted;
        bool _hasChanges;
        ObservableCollection<string> _destinations;
        ObservableCollection<Outbox> _outboxes;
        Dictionary<int, string> _years;
        string _latestYear;
        bool _searchOutboxNoChecked;
        bool _searchSubjectChecked;
        //
        bool _searchEnabled;
        bool _editEnabled;
        Outbox _selectedOutbox;
        bool _canSave;
        bool _canDelete;
        bool _outboxNoEnabled;
        KeyValuePair<int, string> _selectedYear;
        string _searchValue;
        List<string> _errors;
        #endregion
        #region Constr
        public OutboxController()
        {
            Initialize();
        }
        #endregion
        #region Properties
        public string OutboxNo
        {
            get { return _outboxNo; }
            set
            {
                _outboxNo = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string OutboxDate
        {
            get { return _outboxDate; }
            set
            {
                _outboxDate = value;
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
        public string GoingTo
        {
            get { return _goingTo; }
            set
            {
                _goingTo = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string Attachements
        {
            get { return _attachements; }
            set
            {
                _attachements = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;
                RaisePropertyChanged();
            }
        }
        public bool SearchEnabled
        {
            get { return _searchEnabled; }
            set
            {
                _searchEnabled = value;
                RaisePropertyChanged();
            }
        }
        public bool EditEnabled
        {
            get { return _editEnabled; }
            set
            {
                _editEnabled = value;
                RaisePropertyChanged();
            }
        }
        public Outbox SelectedOutobx
        {
            get { return _selectedOutbox; }
            set
            {
                _selectedOutbox = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<string> Destinations
        {
            get { return _destinations; }
            set
            {
                _destinations = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Outbox> Outboxes
        {
            get { return _outboxes; }
            set
            {
                _outboxes = value;
                RaisePropertyChanged();
            }
        }
        public Dictionary<int, string> Years
        {
            get { return _years; }
            set
            {
                _years = value;
                RaisePropertyChanged();
            }
        }
        public string LatestYear
        {
            get { return _latestYear; }
            set
            {
                _latestYear = value;
                RaisePropertyChanged();
                RaisePropertyChanged("SelectedYearIndex");
            }
        }
        public int SelectedYearIndex
        {
            get
            {
                return SelectedYear.Key;
            }
        }
        public bool OutboxNoEnabled
        {
            get { return _outboxNoEnabled; }
            set
            {
                _outboxNoEnabled = value;
                RaisePropertyChanged();
            }
        }
        public KeyValuePair<int, string> SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                try
                {
                    Outboxes.Clear();
                    if (value.Key == 0)
                    {
                        LoadOutboxes(null);
                    }
                    else
                    {
                        LoadOutboxes(value.Value);
                    }
                    _selectedYear = value;
                    RaisePropertyChanged();
                }
                catch (Exception ex)
                {

                    Helper.LogShowError(ex);
                }

            }
        }
        public bool SearchOutboxNoChecked
        {
            get { return _searchOutboxNoChecked; }
            set
            {
                _searchOutboxNoChecked = value;
                RaisePropertyChanged();
            }

        }
        public bool SearchSubjectChecked
        {
            get { return _searchSubjectChecked; }
            set
            {
                _searchSubjectChecked = value;
                RaisePropertyChanged();
            }
        }
        public string SearchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;
                RaisePropertyChanged();

            }
        }
        #endregion
        #region Commands
        #endregion
        #region Helpers
        void Initialize()
        {
            try
            {
                _errors = new List<string>();
                ControlState(ControllerStates.Blank);
                Outboxes = new ObservableCollection<Outbox>();
                Destinations = new ObservableCollection<string>();
                Years = new Dictionary<int, string>();
                Errors = new Dictionary<string, List<string>>();
                LoadData();
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }

        }
        void LoadData()
        {
            LoadYears();
            LoadOutboxes(LatestYear);
            LoadDestinations();
        }
        void LoadYears()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                //var years = unit.Outboxes.GetAll().Distinct(new OutboxNoComparer()).Select(x => x.OutboxNo); 
                var years = unit.Outboxes.GetAll().Select(x => x.OutboxNo.Substring(0, 4)).Distinct();//This version is better than the above one.
                LatestYear = years.Max();
                Years.Add(0, "الكل");
                int id = 1;
                foreach (var year in years)
                {
                    Years.Add(id, year);
                    id++;
                }
                SelectedYear = Years.FirstOrDefault(x => x.Value == LatestYear);
            }
        }
        void LoadOutboxes(string year)
        {
            Outboxes.Clear();
            using (IUnitOfWork unit = new UnitOfWork())
            {
                if (string.IsNullOrEmpty(year))
                {
                    var all = unit.Outboxes.GetAll().OrderByDescending(x => x.OutboxNo);
                    foreach (var outbox in all)
                    {
                        Outboxes.Add(outbox);
                    }
                    return;
                }
                var latestOutboxes = unit.Outboxes.Query(x => x.OutboxNo.StartsWith(year)).OrderByDescending(x => x.OutboxNo);
                foreach (var outbox in latestOutboxes)
                {
                    Outboxes.Add(outbox);
                }
            }
        }

        void LoadDestinations()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var dists = unit.Outboxes.GetAll().Select(x => x.GoingTo).Distinct();
                foreach (var dist in dists)
                {
                    Destinations.Add(dist);
                }

            }
        }

        public void Show(Outbox outbox)
        {
            OutboxNo = outbox.OutboxNo;
            OutboxDate = outbox.OutboxDate;
            Subject = outbox.Subject;
            GoingTo = outbox.GoingTo;
            Attachements = outbox.AttachmentNo;
            Notes = outbox.Notes;
            IsDeleted = outbox.Deleted;
            ControlState(ControllerStates.Loaded);
        }
        private void ClearUIValues()
        {
            OutboxNo = "";
            OutboxDate = "";
            Subject = "";
            GoingTo = "";
            Attachements = "";
            Notes = "";
            SelectedOutobx = null;
        }
        private Outbox CreateNew()
        {
            return new Outbox
            {
                OutboxNo = OutboxNo,
                OutboxDate = OutboxDate,
                Subject = Subject,
                GoingTo = GoingTo,
                AttachmentNo = Attachements,
                Notes = Notes,
                Deleted = false
            };
        }
        private void UpdateOutboxValues(Outbox ob)
        {
            ob.OutboxDate = OutboxDate;
            ob.Subject = Subject;
            ob.GoingTo = GoingTo;
            ob.AttachmentNo = Attachements;
            ob.Notes = Notes;

        }
        public void GenerateOutboxNo()
        {
            try
            {
                string max = Outbox.MaxOutboxNo;
                OutboxNo = Helper.GenerateOutboxNo(max);
                OutboxDate = Helper.GetCurrentDate();
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }
        private void FindOutboxNo(string outboxNo)
        {
            Outboxes = new ObservableCollection<Outbox>(Outboxes.Where(x => x.OutboxNo.Contains(outboxNo)));
        }
        private void FindSubject(string subject)
        {
            Outboxes = new ObservableCollection<Outbox>(Outboxes.Where(x => x.Subject.Contains(subject)));
        }
        private bool IsValid()
        {
            _errors.Clear();
            bool isValid = true;
            if (string.IsNullOrEmpty(OutboxNo))
            {
                string msg = "ادخل رقم الصادر";
                _errors.Add(msg);
                isValid = false;
            }
            if (string.IsNullOrEmpty(OutboxDate))
            {
                string msg = "ادخل تاريخ الصادر";
                _errors.Add(msg);
                isValid = false;

            }
            if (string.IsNullOrEmpty(Subject))
            {
                string msg = "ادخل موضوع المعاملة";
                _errors.Add(msg);
                isValid = false;
            }
            if (string.IsNullOrEmpty(GoingTo))
            {
                string msg = "ادخل الجهة الصادر لها";
                _errors.Add(msg);
                isValid = false;
            }
            return isValid;
        }
        public bool CanExit
        {
            get
            {
                return !_hasChanges;
            }
        }
        #endregion

        #region Base
        public override void ControlState(ControllerStates state)
        {
            State = state;
            switch (State)
            {
                case ControllerStates.Blank:
                    SearchEnabled = true;
                    EditEnabled = true;
                    _hasChanges = false;
                    _canSave = false;
                    _canDelete = false;
                    OutboxNoEnabled = true;
                    SearchOutboxNoChecked = false;
                    SearchSubjectChecked = true;
                    break;
                case ControllerStates.Edited:
                    _hasChanges = true;
                    SearchEnabled = false;
                    _canSave = true;
                    _canDelete = false;
                    OutboxNoEnabled = false;
                    break;
                case ControllerStates.Saved:
                    LoadOutboxes(LatestYear);
                    _hasChanges = false;
                    _canDelete = true;
                    OutboxNoEnabled = false;
                    break;
                case ControllerStates.Loaded:
                    _hasChanges = false;
                    SearchEnabled = false;
                    _canDelete = true;
                    _canSave = true;
                    OutboxNoEnabled = false;
                    break;
            }
        }

        protected override void ClearView()
        {
            if (_hasChanges && !Helper.UserConfirmed(Resources.SavePrompetMsg))
            {
                return;
            }
            ClearUIValues();
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
                    string msg = string.Empty;
                    foreach (var error in _errors)
                    {
                        msg += error;
                        msg += "\n";

                    }
                    Helper.ShowMessage(msg);
                    return;
                }
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var exist = unit.Outboxes.GetAll().Any(x => x.OutboxNo == OutboxNo);
                    Outbox ob = null;
                    if (!exist)
                    {
                        ob = CreateNew();
                        unit.Outboxes.Add(ob);
                    }
                    else
                    {
                        ob = unit.Outboxes.GetById(OutboxNo);
                        UpdateOutboxValues(ob);
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
        protected override bool CanSave()
        {
            return _canSave;
        }

        protected override void Print()
        {
            throw new NotImplementedException();
        }

        protected override bool CanPrint()
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                if (SelectedYear.Key == 0)
                {
                    LoadOutboxes(null);
                    return;
                }
                LoadOutboxes(SelectedYear.Value);
                return;
            }
            if (SearchOutboxNoChecked)
            {
                FindOutboxNo(SearchValue);
                return;
            }
            if (SearchSubjectChecked)
            {
                FindSubject(SearchValue);
            }

        }

        protected override bool CanSearch()
        {
            return true;
        }

        protected override void Delete()
        {
            if (!Helper.UserConfirmed(Resources.DeletePrompetMsg)) return;
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var entity = unit.Outboxes.GetById(OutboxNo);
                entity.Deleted = true;
                unit.Save();
                ClearUIValues();
                ControlState(ControllerStates.Blank);
            }
        }

        protected override bool CanDelete()
        {
            return _canDelete;
        }
        #endregion
    }
}
