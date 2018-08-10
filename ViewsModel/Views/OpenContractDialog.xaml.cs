using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for OpenContractView.xaml
    /// </summary>
    public partial class OpenContractDialog : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Contract> _contracts;
        private readonly IUnitOfWork _unitOfWork;
        public OpenContractDialog(IUnitOfWork ownerUnitOfWork)
        {
            if (ownerUnitOfWork == null) ownerUnitOfWork = new UnitOfWork(); //throw new ArgumentNullException("ownerUnitOfWork");
            InitializeComponent();
            _unitOfWork = ownerUnitOfWork;
            DataContext = this;
            txtPropertyNo.Focus();
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string propertyNo = txtPropertyNo.Text;
            if (!string.IsNullOrEmpty(propertyNo))
            {
                try
                {
                    var contractsRepo = (ContractsRepository)_unitOfWork.Contracts;
                    var result = contractsRepo.ActiveContracts(propertyNo);
                    Contracts = new ObservableCollection<Contract>(result);
                    lstContracts.SelectedIndex = 0;

                }
                catch (Exception ex)
                {
                    string msg = Helper.ProcessExceptionMessages(ex);
                    Helper.ShowMessage(msg);
                }

            }
        }

        public ObservableCollection<Contract> Contracts
        {
            get
            {
                if (_contracts != null) return _contracts;
                return new ObservableCollection<Contract>();
            }
            private set
            {
                _contracts = value;
                RaisePropertyChanged();
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ListViewDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            e.Handled = true;
        }
        public Contract SelectedContract
        {
            get;
            private set;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SelectedContractChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedContract = lstContracts.SelectedItem as Contract;
        }

    }
}
