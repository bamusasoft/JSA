using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Jsa.DomainModel;
namespace Jsa.ViewsModel.ViewsControllers
{
    public class RepreController : EditableControllerBase
    {
        #region Fields
        int _autoKey;
        string _name;
        string _id;
        string _idDate;
        string _issueAt;
        readonly int _customerId;
        //
        Dictionary<string, string> _errorsDic;
        #endregion
        #region Constr
        public RepreController(int customerId)
        {
            _customerId = customerId;
            Initialize();
        }
        #endregion
        #region Properties
        public int AutoKey
        {
            get { return _autoKey; }
            private set
            {
                _autoKey = value;
                RaisePropertyChanged();
            }
        }
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();

            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();

            }
        }
        public string IdDate
        {
            get { return _idDate; }
            set
            {
                _idDate = value;
                RaisePropertyChanged();
            }
        }
        public string IssueAt
        {
            get { return _issueAt; }
            set
            {
                _issueAt = value;
                RaisePropertyChanged();
            }
        }

        #endregion
        #region Helpers
        private void Initialize()
        {
            AutoKey = -1;
            _errorsDic = new Dictionary<string, string>();
            Errors = new Dictionary<string, List<string>>();
        }
        private bool Valid()
        {
            _errorsDic.Clear();
            bool isValid = true;
            string propertyHasError = null;
            string errorMessage = null;
            if (string.IsNullOrEmpty(Id))
            {
                propertyHasError = "رقم الهوية1";
                errorMessage = "أدخل رقم الهوية";
                _errorsDic.Add(propertyHasError, errorMessage);
                isValid = false;
            }
            if (!string.IsNullOrEmpty(Id) && Id.Length != 10)
            {
                propertyHasError = "2رقم الهوية";
                errorMessage = "رقم الهوية يجب ان يكون 10 أرقام";
                _errorsDic.Add(propertyHasError, errorMessage);
                isValid = false;
            }
            if (!string.IsNullOrEmpty(Id) && !Id.IsDigit())
            {
                propertyHasError = "3رقم الهوية";
                errorMessage = "رقم الهوية يجب ان يكون فقط أرقام";
                _errorsDic.Add(propertyHasError, errorMessage);
                isValid = false;
            }
            if (string.IsNullOrEmpty(Name))
            {
                propertyHasError = "الإسم";
                errorMessage = "أدخل الإسم";
                _errorsDic.Add(propertyHasError, errorMessage);
                isValid = false;
            }
            if (string.IsNullOrEmpty(IdDate))
            {
                propertyHasError = "تاريخ الهوية";
                errorMessage = "أدخل تاريخ الهوية";
                _errorsDic.Add(propertyHasError, errorMessage);
                isValid = false;
            }
            if (string.IsNullOrEmpty(IssueAt))
            {
                propertyHasError = "مكان الإصدار";
                errorMessage = "أدخل مكان الإصدار";
                _errorsDic.Add(propertyHasError, errorMessage);
                isValid = false;
            }


            return isValid;
        }
        private Representative CreateNewRepresentative()
        {
            return new Representative()
            {
                Id = Id,
                Name = Name,
                IdDate = IdDate,
                IssueAt = IssueAt,
                CustomerId = _customerId
            };
        }
        private void UpdateRepresentative(Representative rep)
        {
            rep.Name = Name;
            rep.IssueAt = IssueAt;
            rep.IdDate = IdDate;
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
            if (!Valid())
            {
                string msg = null;
                foreach (var error in _errorsDic)
                {
                    msg += string.Format("{0}{1}{2}{3}",
                        error.Key,
                        "\n",
                        error.Value,
                        "\n"
                        );

                }
                Helper.ShowMessage(msg);
                return;
            }
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    bool isExit = unit.Representatives.GetAll().Any(x => x.AutoKey == AutoKey);
                    if (!isExit)
                    {
                        Representative rep = CreateNewRepresentative();
                        unit.Representatives.Add(rep);
                    }
                    else
                    {
                        Representative rep = unit.Representatives.GetById(Id);
                        UpdateRepresentative(rep);
                    }
                    unit.Save();
                }
                RaiseContorllerChanged(ControllerAction.Saved);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }

        protected override bool CanSave()
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
