using System;
using System.Collections.ObjectModel;
using System.Linq;
using Jsa.DomainModel;
using Jsa.ViewsModel.DomainEntities;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class LegalCaseReportController:ReportControllerBase
    {

        #region Consts

        public LegalCaseReportController()
        {
            LegalCases = LoadData();
        }
        #endregion
        #region Fields

        private ObservableCollection<DomainLegalCase> _legalCases;
        #endregion
        #region Proeprties

        public ObservableCollection<DomainLegalCase> LegalCases
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

        private ObservableCollection<DomainLegalCase> LoadData()
        {
            ObservableCollection<DomainLegalCase> data = new ObservableCollection<DomainLegalCase>();
            using (IUnitOfWork db = new UnitOfWork())
            {

                var q = db.LegalCases.GetAll();
                foreach (var legalCase in q)
                {
                    DomainLegalCase dlc = new DomainLegalCase(
                        legalCase.CaseNo, legalCase.RegisteredAt, legalCase.GregDate, legalCase.Defendant, legalCase.Description,
                        legalCase.StatusId, legalCase.CaseStatus, legalCase.CaseAppointments.ToList() , legalCase.CaseFollowings.ToList());
                        data.Add(dlc);
                    
                }

            }
            return data;
        }
        #endregion

        #region Base


        public override void ControlState(ControllerStates state)
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            throw new NotImplementedException();
        }

        protected override bool CanSearch()
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

        protected override void Edit()
        {
            throw new NotImplementedException();
        }

        protected override bool CanEdit()
        {
            throw new NotImplementedException();
        }

        protected override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override bool CanRefresh()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
