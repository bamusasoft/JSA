using System;
using System.Collections.Generic;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class ScheduleDetailsController : EditableControllerBase
    {

        public ScheduleDetailsController()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        private int _id;
        private int _contractNo;
        private string _scheduleId;
        private int _amountDue;
        private string _dateDue;
        private int _amountPaid;
        private string _remarks;
        private bool _discountAmount;
        //
        
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
                
            }
        }

        
        public int ContractNo
        {
            get { return _contractNo; }
            set
            {
                _contractNo = value;
                RaisePropertyChanged();

            }

        }

        public string ScheduleId
        {
            get { return _scheduleId; }
            set
            {
                _scheduleId = value;
                RaisePropertyChanged();
            }
        }

        public int AmountDue
        {
            get { return _amountDue; }
            set
            {
                _amountDue = value;
                RaisePropertyChanged();
            }
        }

        public string DateDue
        {
            get { return _dateDue; }
            set
            {
                _dateDue = value;
                RaisePropertyChanged();
            }
        }
        public int AmountPaid
        {
            get { return _amountPaid; }
            set
            {
                _amountPaid = value;
                RaisePropertyChanged();
            }
        }

        public int Balance
        {
            get { return (AmountDue - AmountPaid); }
        }

        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                RaisePropertyChanged();
            }
        }

        public bool DiscountAmount
        {
            get { return _discountAmount; }
            set
            {
                _discountAmount = value;
                RaisePropertyChanged();
            }
        }

        #region Base
        
       
        public override void ControlState(ControllerStates state)
        {
            throw new NotImplementedException();
        }

        protected override void ClearView()
        {
            throw new NotImplementedException();
        }

        protected override bool CanClear()
        {
            throw new NotImplementedException();
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        protected override bool CanSave()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        #endregion
    }
}
