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
    /// Interaction logic for OpenSignerDialog.xaml
    /// </summary>
    public partial class OpenSignerDialog : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Signer> _signers;
        private readonly IUnitOfWork _unitOfWork;
        public OpenSignerDialog(IUnitOfWork ownerUnitOfWork)
        {
            if (ownerUnitOfWork == null) ownerUnitOfWork = new UnitOfWork(); //throw new ArgumentNullException("ownerUnitOfWork");
            InitializeComponent();
            _unitOfWork = ownerUnitOfWork;
            DataContext = this;
        }
        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string signerName = txtSignerName.Text;
            if (!string.IsNullOrEmpty(signerName))
            {
                try
                {
                    var contractsRepo = (SignerRepository)_unitOfWork.Signers;
                    var result = contractsRepo.Query(x => x.Name.Contains(signerName));
                    Signers = new ObservableCollection<Signer>(result);
                    lstSigners.SelectedIndex = 0;

                }
                catch (Exception ex)
                {
                    string msg = Helper.ProcessExceptionMessages(ex);
                    Helper.ShowMessage(msg);

                }
            }
        }

        public ObservableCollection<Signer> Signers
        {
            get
            {
                if (_signers != null) return _signers;
                return new ObservableCollection<Signer>();
            }
            private set
            {
                _signers = value;
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
        public Signer SelectedSigner
        {
            get;
            private set;
        }

        private void DialogLoaded(object sender, RoutedEventArgs e)
        {
            txtSignerName.Focus();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SelectedSignerChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedSigner = lstSigners.SelectedItem as Signer;

        }
    }
}
