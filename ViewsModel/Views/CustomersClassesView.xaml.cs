using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.Mediator;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for CustomersClassesView.xaml
    /// </summary>
    public partial class CustomersClassesView : Window
    {
        OpenDialogProxy _dialog;
        CustomersClassesController _controller;
        public CustomersClassesView()
        {
            InitializeComponent();
            _dialog = new OpenDialogProxy();
            _dialog.OpenDialog += Dialog_OpenDialog;
            _controller =new CustomersClassesController(_dialog);
            DataContext = _controller;
        }

        private void Dialog_OpenDialog(object sender, object e)
        {
            int customerId = (int)e;
            CustomerHistoryView historyView = new CustomerHistoryView(customerId);
            historyView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            historyView.ShowDialog();
        }
        private void OnGridDoubleClick(object sender, RoutedEventArgs e)
        {
            _controller.OpenDialogCommmand.Execute(null);
        }
    }
}
