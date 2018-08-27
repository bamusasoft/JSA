using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class DocDestinationController : EditableControllerBase
    {
        public DocDestinationController()
        {
            Errors = new Dictionary<string, List<string>>();
            LoadDestinations();

        }

        #region Fields
        int _destId;
        string _description;
        ObservableCollection<Destination> _destinations;
        //
        ControllerStates _controllerState;
        #endregion
        #region Properties
        public int DestId
        {
            get { return _destId; }
            set
            {
                _destId = value;
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public ObservableCollection<Destination> Destinations
        {
            get { return _destinations; }
            set
            {
                _destinations = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Base

        public override void ControlState(ControllerStates state)
        {
            _controllerState = state;
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
                    break;
            }
        }

        protected override bool CanClear()
        {
            return true;
        }

        protected override bool CanDelete()
        {
            throw new NotImplementedException();
        }

        protected override bool CanPrint()
        {
            throw new NotImplementedException();
        }

        protected override bool CanSave()
        {
            return true;
        }

        protected override bool CanSearch()
        {
            throw new NotImplementedException();
        }

        protected override void ClearView()
        {
            if (_controllerState == ControllerStates.Edited)
            {
                string msg = Properties.Resources.SavePrompetMsg;

                if (!Helper.UserConfirmed(msg))
                {
                    return;
                }

            }
            DestId = 0;
            Description = "";
            ControlState(ControllerStates.Blank);
        }

        protected override void Delete()
        {
            throw new NotImplementedException();
        }

        protected override void Print()
        {
            throw new NotImplementedException();
        }

        protected override void Save()
        {
            if (!IsValid())
            {
                string msg = string.Empty;
                foreach (var error in Errors)
                {
                    msg += error.Value?.FirstOrDefault() ?? "";
                    msg += "\n";

                }
                Helper.ShowMessage(msg);
                return;
            }
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    Destination destination = unit.Destinations.GetById(DestId); ;
                    if (destination == null)
                    {
                        destination = CreateNewDestination();
                        unit.Destinations.Add(destination);
                    }
                    else
                    {
                        UpdateDestination(destination);
                    }
                    unit.Save();
                    LoadDestinations();
                    DestId = destination.Id;
                    ControlState(ControllerStates.Saved);

                }
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
        }

        protected override void Search()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        private bool IsValid()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(Description))
            {
                AddError("Description", DESCRIPTIONERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Description", DESCRIPTIONERROR);
            }
            return isValid;
        }
        private async Task LoadDestinations()
        {
            Task<List<Destination>> task = null;
            try
            {
                task = LoadDesintationsAsync();
                List<Destination> result = await task;
                Destinations = new ObservableCollection<Destination>(result);
                ControlState(ControllerStates.Blank);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }
        private Task<List<Destination>> LoadDesintationsAsync()
        {
            Task<List<Destination>> task = Task.Run(() =>
            {
                List<Destination> destinations = null;
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    destinations = unit.Destinations.GetAll().ToList();
                }
                return destinations;
            });

            return task;
        }
        #endregion
        #region Methods
        private Destination CreateNewDestination()
        {
            Destination destination = new Destination();
            destination.Description = Description;
            return destination;
        }
        private void UpdateDestination(Destination destination)
        {
            destination.Description = Description;
        }
        private void ShowDestination(Destination destination)
        {
            DestId = destination.Id;
            Description = destination.Description;
        }
        #endregion

        #region Public Methods
        public void OnSelectedDestinationChanged(Destination destination)
        {
            ShowDestination(destination);
            ControlState(ControllerStates.Saved);
           
        }

        #endregion
        #region Error Messaegs
        private const string DESCRIPTIONERROR = "ادخل اسم الجهة";
        #endregion
    }
}
