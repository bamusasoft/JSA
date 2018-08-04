using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for OpenCustomerDialog.xaml
    /// </summary>
    public partial class OpenCustomerDialog : Window, INotifyPropertyChanged
    {
        public OpenCustomerDialog(SearchField field)
        {
            InitializeComponent();
            DataContext = this;
            _searchField = field;
            GetReady();
        }

        private void GetReady()
        {
            txtCustomerNo.Focus();
            switch (_searchField)
            {
                case SearchField.CustomerName:
                    Watermark = "ادخل اسم المستأجر أو جزء منه";
                    break;
                case SearchField.CustomerCode:
                    Watermark = "ادخل رقم المستأجر";
                    break;
            }
        }

        #region "Fields"
        Customer _selectedCustomer;
        private ObservableCollection<Customer> _customers;
        private RelayCommand _searchCommand;
        private string _watermark;
        private readonly SearchField _searchField;
        #endregion

        #region "Properties"
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                RaisePropertyChanged();
            }
        }
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
            }
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

        private void SelectedCustomerChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCustomer = lstCustomers.SelectedItem as Customer;
        }
        private void ListViewDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            e.Handled = true;
        }
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? ((_searchCommand = new RelayCommand(Search))); }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(txtCustomerNo.Text)) return;
            try
            {
                switch (_searchField)
                {
                    case SearchField.CustomerName:
                        string name = txtCustomerNo.Text.Trim();
                        using (IUnitOfWork uow = new UnitOfWork())
                        {
                            var customers = uow.Customers.Query(cu => cu.Name.Contains(name));
                            Customers = new ObservableCollection<Customer>(customers);
                            lstCustomers.SelectedIndex = 0;
                            btnOk.Focus();
                        }
                        break;
                    case SearchField.CustomerCode:
                        int customerNo;
                        if (!int.TryParse(txtCustomerNo.Text, out customerNo)) return;
                        using (IUnitOfWork uow = new UnitOfWork())
                        {
                            var customers = uow.Customers.Query(cu => cu.CustomerId == customerNo);
                            Customers = new ObservableCollection<Customer>(customers);
                            lstCustomers.SelectedIndex = 0;
                            btnOk.Focus();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
                Helper.ShowMessage(msg);

            }
        }

        public string Watermark
        {
            get { return _watermark; }
            set
            {
                _watermark = value;
                RaisePropertyChanged();

            }
        }
        public enum SearchField
        {
            CustomerName,
            CustomerCode,
        }
    }
}
