using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Windows.Input;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class AmountDueCalcController : EditableControllerBase
    {

        #region Fields
        readonly Contract _contract;
        int _rentDue;
        int _maintDue;

        RelayCommand _caluclateCommand;

        #endregion

        #region Constr


        public AmountDueCalcController(int contractNo)
        {
            try
            {
                _contract = LoadContract(contractNo);
                Errors = new Dictionary<string, List<string>>();
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }

        }


        #endregion

        #region Propeties
        public Contract Contract
        {
            get { return _contract; }

        }
        public int RentDue
        {
            get { return _rentDue; }
            set
            {
                _rentDue = value;
                RaisePropertyChanged();
            }
        }
        public int MaintDue
        {
            get { return _maintDue; }
            set
            {
                _maintDue = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand CalculateCommand
        {
            get { return _caluclateCommand ?? (_caluclateCommand = new RelayCommand(Calculate)); }
        }
        private void Calculate()
        {
            RentDue = CalculateAmountDue(_contract.AgreedRent, _contract.StartDate, _contract.EndDate);
            MaintDue = CalculateAmountDue(_contract.AgreedMaintenance, _contract.StartDate, _contract.EndDate);
        }
        #endregion

        #region Helpers
        private int CalculateAmountDue(decimal agreedAmount, string startDate, string endDate)
        {
            int result = 0;
            try
            {
                var diff = Helper.CaculateDate(_contract.StartDate, _contract.EndDate);

                decimal amounDue = (((agreedAmount / 12.00m) / 30.00m) * (decimal)diff.Item1);

                amounDue += ((agreedAmount / 12.00m) * (decimal)diff.Item2);
                result = (int)Math.Round(amounDue, 0, MidpointRounding.AwayFromZero);

            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
            return result;
        }
        private Contract LoadContract(int contractNo)
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                return unit.Contracts.GetById(contractNo);
            }
        }


        #endregion

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
