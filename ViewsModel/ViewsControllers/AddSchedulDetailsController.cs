using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class AddSchedulDetailsController:DialogControllerBase
    {
        #region Fields
        private readonly int _contractNo;
        private string _customerName;
        private string _propertyDescription;
        private int _total;
        private int _currentBalance;
        private ObservableCollection<ScheduleDetailsController> _details;
        private ScheduleDetailsController _selected;
        private RelayCommand _deleteCommand;
        #endregion
        public AddSchedulDetailsController(int contractsNo)
        {
            _contractNo = contractsNo;
            Initilize();
        }

        #region Properties

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value; 
                RaisePropertyChanged();
            }
        }

        public string PropertyDescription
        {
            get { return _propertyDescription; }
            set
            {
                _propertyDescription = value; 
                RaisePropertyChanged();
            }
        }

        public int Total
        {
            get { return _total; }
            set
            {
                _total = value; 
                RaisePropertyChanged();
            }
        }

        public int CurrentBalance
        {
            get { return _currentBalance; }
            set
            {
                _currentBalance = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ScheduleDetailsController> Details
        {
            get { return _details; }
            set
            {
                _details = value; 
                RaisePropertyChanged();
            }
        }

        public ScheduleDetailsController Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged();
            }
        }
        
        #endregion

        #region Helpers

        void Initilize()
        {
            try
            {
                 if (Details != null) Details.CollectionChanged -= OnDetailsChanged;
                var contract = GetContract(_contractNo);
                CustomerName = contract.Customer.Name;
                PropertyDescription = contract.Property.Description;
                Total = contract.Total;
                CurrentBalance = Total;
                
                Details = new ObservableCollection<ScheduleDetailsController>();
                Details.CollectionChanged += OnDetailsChanged;
            }
            catch (Exception ex)
            {
                
               Helper.LogShowError(ex);
            }
            
        }

        private void OnDetailsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ScheduleDetailsController detail in e.NewItems)
                {
                    detail.PropertyChanged += OnDetailsChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (ScheduleDetailsController detail in e.OldItems)
                {
                    detail.PropertyChanged -= OnDetailsChanged;
                }
            }
        }

        private void OnDetailsChanged(object sender, PropertyChangedEventArgs e)
        {
            
            CalculateDetailsSum();
        }

        private void CalculateDetailsSum()
        {
            
            int sum = Details.Sum(x => x.AmountDue);
            CurrentBalance = (Total - sum);
            
        }

       

        Contract GetContract(int contractNo)
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

        public override void Ok()
        {
            if (CurrentBalance < 0 || CurrentBalance >0)
            {
                if (Helper.UserConfirmed(PAYMENTSSUMERROR))
                {
                    RaiseCloseDialog(DialogCloseState.Ok);
                    return;
                }
                return;
            }
            RaiseCloseDialog(DialogCloseState.Ok);
            
        }

        public override bool OkEnabled()
        {
            return true;
        }

        public override void Cancel()
        {
            RaiseCloseDialog(DialogCloseState.Cancel);
        }

        public void SetContractNo(ScheduleDetailsController detail)
        {
            detail.ContractNo =_contractNo;
        }

        #endregion
        #region Commands

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(Delete, CanDelete)); }
        }

        void Delete()
        {
            Details.Remove(Selected);
        }

        bool CanDelete()
        {
            return Selected != null;
        }
        #endregion
        #region Messages

        private const string PAYMENTSSUMERROR = "اجمالي الدفعات لا يساوي اجمالي رصيد العقد";

        #endregion
    }
}
