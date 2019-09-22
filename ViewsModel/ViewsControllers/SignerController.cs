using System;
using System.Collections.Generic;
using System.Linq;
using Jsa.DomainModel;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class SignerController:EditableControllerBase
    {

        #region Consts

        public SignerController()
        {
            Initialize();
        }

        private void Initialize()
        {
            Errors = new Dictionary<string, List<string>>();
            ControlState(ControllerStates.Blank);
        }

        #endregion

        #region Fields


        private string _id;
        private string _name;
        private string _idDate;
        private string _issue;
        private string _mobile;
        private string _phone;

        private bool  _canSave ;
        private bool _canSearch ;
        private bool _canDelete ;
        #endregion

        #region Proeprties
        
        
        public string Id    
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public string IdDate
        {
            get { return _idDate; }
            set
            {
                _idDate = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public string Issue
        {
            get { return _issue; }
            set
            {
                _issue = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public string Mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                RaisePropertyChanged();
                ControlState(ControllerStates.Edited);
            }
        }
        #endregion

        #region Base
        
        
        public override void ControlState(ControllerStates state)
        {
            State = state;
            switch (State)
            {
                case ControllerStates.Blank:
                    _canSave = true;
                    _canSearch = true;
                    _canDelete = false;

                    RaiseContorllerChanged(ControllerAction.Cleared);
                    break;
                case ControllerStates.Edited:
                    _canSave = true;
                    _canSearch = false;
                    _canDelete = true;

                    RaiseContorllerChanged(ControllerAction.Edited);

                    break;
                case ControllerStates.Saved:
                    _canSave = true;
                    _canSearch = false;
                    _canDelete = true;

                    RaiseContorllerChanged(ControllerAction.Saved);
                    break;
                case ControllerStates.Loaded:
                    _canSave = true;
                    _canSearch = false;
                    _canDelete = true;

                    RaiseContorllerChanged(ControllerAction.Saved);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        protected override void ClearView()
        {
            if (State == ControllerStates.Edited)
            {
                if (!Helper.UserConfirmed(Resources.SavePrompetMsg))
                {
                    return;
                }
            }

            ControlState(ControllerStates.Blank);
        }

        protected override bool CanClear()
        {
            return true;
        }

        protected override void Save()
        {
            try
            {
                if(!IsValid())return;
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var existed = unit.Signers.GetAll().Any(x => x.SignerId == Id);
                    if (!existed)
                    {
                        var signer = CreateNewSigner();
                        unit.Signers.Add(signer);
                    }
                    else
                    {
                        var exSigner = unit.Signers.GetById(Id);
                        UpdateSigner(exSigner);
                    }
                    unit.Save();
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
            return _canSave;
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
            return _canSearch;
        }

        protected override void Delete()
        {
            
        }

        protected override bool CanDelete()
        {
            return _canDelete;
        }
        #endregion

        #region Helpers

        private Signer CreateNewSigner()
        {
            return new Signer
            {
                SignerId = Id,
                Name = Name,
                IdDate = IdDate,
                IdIssue = Issue,
                Mobile = Mobile,
                Phone = Phone
            };
        }

        private void UpdateSigner(Signer exSigner)
        {
            exSigner.Name = Name;
            exSigner.IdDate = IdDate;
            exSigner.IdIssue = Issue;
            exSigner.Mobile = Mobile;
            exSigner.Phone = Phone;
        }

        private bool IsValid()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(Id))
            {
                AddError("Id", IDERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Id", IDERROR);

            }
            if (string.IsNullOrEmpty(Name))
            {
                AddError("Name", NAMEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Name", NAMEERROR);

            }
            if (string.IsNullOrEmpty(IdDate))
            {
                AddError("IdDate", DATEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("IdDate", DATEERROR);
            }
            if (string.IsNullOrEmpty(Issue))
            {
                AddError("Issue", ISSUEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Issue", ISSUEERROR);

            }
            if (string.IsNullOrEmpty(Mobile))
            {
                AddError("Mobile", MOBILEERROR);
                isValid = false;
            }
            else
            {
                RemoveError("Mobile", MOBILEERROR);

            }
            
            return isValid;
        }

        private const string IDERROR = "خطأ رقم الهوية";
        private const string NAMEERROR = "خطأ الإسم";
        private const string DATEERROR = "خطأ التاريخ";
        private const string ISSUEERROR = "خطأ المصدر";
        private const string MOBILEERROR = "خطأ الجوال";


        #endregion
    }
}
