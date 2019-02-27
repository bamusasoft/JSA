using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.ViewModels
{
    public abstract class ViewModelBase : INotifyDataErrorInfo
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region DataError
        protected Dictionary<string, List<string>> Errors { get; set; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !Errors.ContainsKey(propertyName)) return null;
            return Errors[propertyName];
        }


        public bool HasErrors
        {
            get { return Errors.Count > 0; }
        }
        protected void AddError(string propertyName, string error)
        {
            if (!Errors.ContainsKey(propertyName))
                Errors[propertyName] = new List<string>();
            if (!Errors[propertyName].Contains(error))
            {
                Errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        protected void RemoveError(string propertyName, string error)
        {
            if (Errors.ContainsKey(propertyName) && Errors[propertyName].Contains(error))
            {
                Errors[propertyName].Remove(error);
                if (Errors[propertyName].Count == 0) Errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged == null)
            {
                return;
            }

            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion
    }
}
