using System;
using System.Collections.ObjectModel;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class SelectContractsController:DialogControllerBase
    {
        #region Fields

        private ObservableCollection<Contract> _customerContracts;
        private ObservableCollection<Contract> _selectedContracts;
        #endregion

        public SelectContractsController(int customerId)
        {
            CustomerContracts = LoadCustomerContracts(customerId);
            SelectedContracts = new ObservableCollection<Contract>();
        }
        #region Proeprties
         

        public ObservableCollection<Contract> CustomerContracts
        {
            get { return _customerContracts; }
            set
            {
                _customerContracts = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Contract> SelectedContracts
        {
            get { return _selectedContracts; }
            set
            {
                _selectedContracts = value;
                RaisePropertyChanged();
            }
        }

        

        #endregion
        #region Helpers

        ObservableCollection<Contract> LoadCustomerContracts(int customerId)
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var list =
                    ((ContractsRepository) unit.Contracts).CustomerActiveContracts(customerId);
                return new ObservableCollection<Contract>(list);
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
            return SelectedContracts.Count > 0;
        }

        public override void Cancel()
        {
            RaiseCloseDialog(DialogCloseState.Cancel);
        }
        #endregion
    }
}
