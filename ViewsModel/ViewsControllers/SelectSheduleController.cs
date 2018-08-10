using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jsa.DomainModel;
using Jsa.ViewsModel.Annotations;
using Jsa.ViewsModel.DomainEntities;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class SelectSheduleController:DialogControllerBase
    {
        private ObservableCollection<CustomerSchedule> _customerSchedule;


        public SelectSheduleController([NotNull] IEnumerable<Schedule> schedules)
        {
            if (schedules == null) throw new ArgumentNullException("schedules");
            Initilize();
            FillCustomerSchedules(schedules);
        }

        private void Initilize()
        {
            CustomerSchedules = new ObservableCollection<CustomerSchedule>();
        }

        public void FillCustomerSchedules(IEnumerable<Schedule> schedules )
        {
            foreach (var schedule in schedules)
            {
                string propertyDesc = "";
                string propertyNo = "";
                var contracts = schedule.ScheduleDetails.Distinct(new CompareDetailsByContract());
                foreach( var detail in contracts)
                {
                    propertyDesc += detail.Contract.Property.Description + " - "  ;
                    propertyNo += detail.Contract.PropertyNo + " - " ;
                }
                CustomerSchedule customerSchedule = new CustomerSchedule(
                    schedule.ScheduleId, schedule.CustomerId, schedule.Customer.Name,
                    propertyNo, propertyDesc
                    );
                CustomerSchedules.Add(customerSchedule);
            }
        }

        #region Properties

        public ObservableCollection<CustomerSchedule> CustomerSchedules
        {
            get { return _customerSchedule; }
            set
            {
                _customerSchedule = value; 
                RaisePropertyChanged();
            }
        }
      

        #endregion

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
                    break;
                case ControllerStates.Loaded:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }

        public override void Ok()
        {
            RaiseCloseDialog(DialogCloseState.Ok);
        }

        public override bool OkEnabled()
        {
            return SelectedItem != null;
        }

        public override void Cancel()
        {
            RaiseCloseDialog(DialogCloseState.Cancel);
        }
        #endregion

    }
}
