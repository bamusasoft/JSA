using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class CaseAppointmentController:EditableControllerBase
    {
        #region Consts

        public CaseAppointmentController()
        {
            Errors = new Dictionary<string, List<string>>();
            LoadLegalCases();
        }
        #endregion

        #region Fields

        private int _id;
        private int _caseNo;
        private string _appointmentDate;
        private ObservableCollection<CaseAppointment> _caseAppointments;
        private ObservableCollection<LegalCase> _legalCases;
        private LegalCase _selectedCase;

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

        public string AppointmentDate
        {
            get { return _appointmentDate; }
            set
            {
                _appointmentDate = value; 
                RaisePropertyChanged();
            }
        }

        public DateTime AppointmentGregDate
        {
            get { return Helper.ConvertToGregDate(AppointmentDate); }
        }

        public ObservableCollection<CaseAppointment> CaseAppointments
        {
            get { return _caseAppointments; }
            set
            {
                _caseAppointments = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<LegalCase> LegalCases
        {
            get { return _legalCases; }
            set
            {
                _legalCases = value;
                RaisePropertyChanged();
            }
        }

        #endregion
        #region Methods

        private void LoadLegalCases()
        {
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var lc = unit.LegalCases.GetAll();
                    LegalCases = new ObservableCollection<LegalCase>(lc);
                }
                ControlState(ControllerStates.Loaded);
            }
            catch (Exception ex)
            {
                
                Helper.LogShowError(ex);
            }
           
        }

        public void LoadCaseAppointments(LegalCase legalCase)
        {
            if (legalCase == null) throw new ArgumentNullException("legalCase");
            try
            {
                _selectedCase = legalCase;
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var appints = ((CaseAppointmentRepository)unit.CaseAppointments).GetCaseAppointments(legalCase.CaseNo);
                    CaseAppointments = new ObservableCollection<CaseAppointment>(appints);
                    CreateNewAppointment();
                    ControlState(ControllerStates.Blank);
                }
                
            }
            catch (Exception ex)
            {
                
                Helper.LogShowError(ex);
            }
        }

        private void CreateNewAppointment()
        {
            Id = 0;
            CaseNo = _selectedCase.CaseNo;
            AppointmentDate = "";
        }

        public void ShowAppointment(CaseAppointment appointment)
        {
            Id = appointment.Id;
            CaseNo = appointment.CaseNo;
            AppointmentDate = appointment.AppointmentDate;
        }
        #endregion

        private void AddNew(CaseAppointment appointment, IUnitOfWork db)
        {
            if (appointment == null) throw new ArgumentNullException("appointment");
            if (db == null) throw new ArgumentNullException("db");

            appointment.CaseNo = CaseNo;
            appointment.AppointmentDate = AppointmentDate;
            appointment.AppointmentGregDate = AppointmentGregDate;
            db.CaseAppointments.Add(appointment); 
        }

        private void Update(CaseAppointment appointment)
        {
            appointment.CaseNo = CaseNo;
            appointment.AppointmentDate = AppointmentDate;
            appointment.AppointmentGregDate = AppointmentGregDate;
        }

        private bool IsValid()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(AppointmentDate))
            {
                AddError("AppointmentDate", APPOINTMENTERROR);
                isValid = false;
            }
            else
            {
                RemoveError("AppointmentDate", APPOINTMENTERROR);

            }
            return isValid;
        }
        #region Base


        public override void ControlState(ControllerStates state)
        {
            switch (state)
            {
                case ControllerStates.Blank:
                    break;
                case ControllerStates.Edited:
                    break;
                case ControllerStates.Saved:
                    LoadCaseAppointments(_selectedCase);
                    break;
                case ControllerStates.Loaded:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }

        protected override void ClearView()
        {
            throw new NotImplementedException();
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
                    CaseAppointment caseAppointment;
                    if (!((CaseAppointmentRepository)db.CaseAppointments).TryCheckExist(Id, out caseAppointment))
                    {
                        AddNew(caseAppointment, db);
                    }
                    else
                    {
                        Update(caseAppointment);
                    }
                    db.Save();
                    Id = caseAppointment.Id;
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
            return true;
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
            throw new NotImplementedException();
        }

        protected override bool CanSearch()
        {
            throw new NotImplementedException();
        }

        protected override void Delete()
        {
            throw new NotImplementedException();
        }

        protected override bool CanDelete()
        {
            return true;
        }
        #endregion
        #region Messages

        private const string APPOINTMENTERROR = "أدخل تاريخ الموعد";

        #endregion
    }
}
