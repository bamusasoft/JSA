using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for OpenClaimDialog.xaml
    /// </summary>
    public partial class OpenClaimDialog : Window, INotifyPropertyChanged
    {
        public OpenClaimDialog()
        {
            InitializeComponent();
            DataContext = this;
            txtCustmerNo.Focus();
            
        }
        #region "Fields"
       
        ObservableCollection<SearchedClaim> _claims;
        int _customerNo;
        string _year;
        RelayCommand _searchCommand;
        RelayCommand _okCommand;
        RelayCommand _cancelCommand;
        #endregion

        #region "Properties"
        public ObservableCollection<SearchedClaim> Claims
        {
            get { return _claims; }
            set
            {
                _claims = value;
                RaisePropertyChanged();
            }
        }
        public int CustomerNo
        {
            get { return _customerNo; }
            set
            {
                _customerNo = value;
                RaisePropertyChanged();
            }
        }
        public string Year
        {
            get { return _year; }
            set
            {
                _year = value;
                RaisePropertyChanged();
            }
        }
        public SearchedClaim SelectedClaim { get; set; }
        
        #endregion
        #region "Commands"
        public ICommand SearchCommand
        {
            get 
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(Search);
                }
                return _searchCommand;
            }
        }
        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new RelayCommand(Ok, OkEnabled);
                }
                return _okCommand;
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(Cancel);
                }
                return _cancelCommand;
            }
        }
        #endregion
        #region "HelperMethods"
        List<SearchedClaim> SearchCliams()
        {
            List<SearchedClaim> scs = new List<SearchedClaim>();
            if (CustomerNo > 0 && (!string.IsNullOrEmpty(Year)))
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    var result = uow.Claims.Query(x => x.CustomerId == CustomerNo && x.ClaimYear == Year);
                    scs = TransformClaims(result);
                }
            }
            else if(CustomerNo > 0 && (string.IsNullOrEmpty(Year)))
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    var result = uow.Claims.Query(x => x.CustomerId == CustomerNo);
                   scs = TransformClaims(result);
                }
            }
            else if ((!string.IsNullOrEmpty(Year) && CustomerNo <= 0))
            {
                using (IUnitOfWork uow = new UnitOfWork())
                {
                    var result = uow.Claims.Query(x => x.ClaimYear == Year);
                    scs =  TransformClaims(result);
                }
            }
            return scs;
        }
        static List<SearchedClaim> TransformClaims(IQueryable<Claim> source)
        {
            List<SearchedClaim> lst = new List<SearchedClaim>();
            foreach (var item in source)
            {
              lst.Add(new SearchedClaim(item.ClaimId, item.Customer.Name, item.Sequence, item.ClaimYear));  
            }
            return lst;
        }
        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;



        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void Search()
        {
            try
            {
                var scs = SearchCliams();
                Claims = new ObservableCollection<SearchedClaim>(scs);

            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);
            }
        }
        private void Ok()
        {
            DialogResult = true;
        }
        bool OkEnabled()
        {
            return SelectedClaim != null;
        }
        private void Cancel()
        {
            DialogResult = false;
        }
        private void GridRowDoubleClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            e.Handled = true;
        }

        private void GridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedClaim = dgClaims.SelectedItem as SearchedClaim;
        }

        private void OnGridContentKeyDown(object sender, KeyEventArgs e)
        {
            Helper.MoveFocus(e);
        }

    }
}
