using Jsa.ViewsModel.ViewsControllers;
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

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for CustomerHistoryView.xaml
    /// </summary>
    public partial class CustomerHistoryView : Window
    {
        public CustomerHistoryView()
        {
            InitializeComponent();
        }
        public CustomerHistoryView(int customerId):this()
        {
            CustomerHistoryController controller = new CustomerHistoryController(customerId);
            DataContext = controller;
        }
    }
}
