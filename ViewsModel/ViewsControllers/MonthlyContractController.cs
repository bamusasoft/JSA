using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.Views;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class MonthlyContractController : EditableControllerBase
    {

        #region Fields
        private ObservableCollection<Representative> _representatives;
        private Representative _selectedRepresentative;
        private RelayCommand _addRepreCommand;
        private readonly int _contractNo;
        private Contract _contract;
        private string _endDate;
        #endregion

        #region Consturctors
        public MonthlyContractController(int contractNo)
        {
            _contractNo = contractNo;

            Initilize();
        }
        #endregion

        #region Properties
        public ObservableCollection<Representative> Representatives
        {
            get
            {
                return _representatives;
            }
            private set
            {
                _representatives = value;
                RaisePropertyChanged();
            }

        }

        private Customer Customer { get; set; }
        public Representative SelectedRepresentative
        {
            get { return _selectedRepresentative; }
            set
            {
                _selectedRepresentative = value;
                RaisePropertyChanged();
            }
        }
        public string StartDate
        {
            get
            { return _contract.StartDate; }

        }
        public string EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand AddRepreCommand
        {
            get { return _addRepreCommand ?? (_addRepreCommand = new RelayCommand(AddRepres, CanAddRepres)); }

        }
        void AddRepres()
        {
            try
            {
                RepView rv = new RepView(_contract.CustomerId);
                rv.AddCompleted += OnAddRepresCompleted;
                rv.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                rv.ShowDialog();

            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }


        bool CanAddRepres()
        {
            return true;
        }
        #endregion

        #region Helpers
        private void Initilize()
        {
            _contract = GetContract();
            Representatives = FillRepresentatives();
            Errors = new Dictionary<string, List<string>>();


        }
        private ObservableCollection<Representative> FillRepresentatives()
        {
            ObservableCollection<Representative> ocr = new ObservableCollection<Representative>();
            Representative r = new Representative
            {
                    Id = "------------",
                    CustomerId = -1,
                    Name = "بدون",
                    IdDate = "------------",
                    IssueAt = "------------"

                };
            ocr.Add(r);
            var storeReps = LoadRepresentatives();
            foreach (Representative item in storeReps)
            {
                ocr.Add(item);
            }
            return ocr;

        }
        private Contract GetContract()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                return unit.Contracts.GetById(_contractNo);
            }
        }
        private IList<Representative> LoadRepresentatives()
        {
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var reps = unit.Representatives.Query(x => x.CustomerId == _contract.CustomerId);
                return reps.ToList();
            }
        }
        void OnAddRepresCompleted(object sender, EventArgs e)
        {
            Representatives = FillRepresentatives();
        }
        #endregion

        #region Overrides of EditableControllerBase

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
            try
            {
                if (string.IsNullOrEmpty(EndDate))
                {
                    string msg = Resources.MonthlyContractView_EndDateMissing;
                    Helper.ShowMessage(msg);
                    return;
                }
                PrintDialog pdg = new PrintDialog();
                if (pdg.ShowDialog() == DialogResult.OK)
                {
                    var selectedPrinter = pdg.PrinterSettings.PrinterName;
                    MonthlyContractPrinter printer = new MonthlyContractPrinter(Settings.Default.MonthlyContractTemplate,
                   _contract, SelectedRepresentative, StartDate, EndDate);
                    printer.Print(selectedPrinter);
                }
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
        }

        protected override bool CanPrint()
        {
            return SelectedRepresentative != null;
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
