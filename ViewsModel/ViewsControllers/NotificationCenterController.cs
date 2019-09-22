using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class NotificationCenterController : IController
    {

        private readonly int _dueDays;
        public NotificationCenterController()
        {
            _dueDays = Settings.Default.AppointDueDays;
             LoadDueAppointmentsAsync();

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

        public ObservableCollection<CaseFollowing> CaseFollowings
        {
            get { return _caseFollowings; }
            set
            {
                _caseFollowings = value;
                RaisePropertyChanged();
            }
        }

        public string FollowingsDueDescription
        {
            get
            {
                int dueDays = Settings.Default.AppointDueDays;
                return string.Format(FOLLOWINGSAPPOINTMENTDESCRIPTION, dueDays);

            }
        }

        public string CasesDueDescription
        {
            get
            {
                int dueDays = Settings.Default.AppointDueDays;
                return string.Format( CASEAPPOINTMENTSDESCRIPTION, dueDays);
            }
        }
        private void LoadDueAppointments()
        {
            try
            {   
                using (IUnitOfWork db = new UnitOfWork())
                {
                    var ca = ((CaseAppointmentRepository)db.CaseAppointments).DueCases(_dueDays);
                    var cf = ((CaseFollowingRepository)db.CaseFollowings).DueFollowings(_dueDays);
                    CaseAppointments = new ObservableCollection<CaseAppointment>(ca);
                    CaseFollowings = new ObservableCollection<CaseFollowing>(cf);
                }
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
            
        }
        private async void LoadDueAppointmentsAsync()
        {
            await Task.Run(() => { LoadDueAppointments(); });

        }


        #region Base


        private ControllerStates _state;
        private ObservableCollection<CaseAppointment> _caseAppointments;
        private ObservableCollection<CaseFollowing> _caseFollowings;
        public event PropertyChangedEventHandler PropertyChanged;

        public ControllerStates State
        {
            get { return _state; }
            set { _state = value; }
        }

        public void ControlState(ControllerStates state)
        {
            throw new NotImplementedException();
        }

        public event ControllerChangedEventHandler ControllerChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RaiseContorllerChanged(ControllerAction action)
        {
            if (ControllerChanged != null)
            {
                ControllerChanged(this, new ControllerChangedEventArgs(action));
            }
        }

        #endregion
        #region Messages

        private const string CASEAPPOINTMENTSDESCRIPTION = "مواعيد الجلسات خلال {0} يوم القادمة";
        private const string FOLLOWINGSAPPOINTMENTDESCRIPTION = "مواعيد المتابعات خلال {0} يوم القادمة";


        #endregion
    }
}
