using Jsa.DomainModel;
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
    /// Interaction logic for DocDestinationView.xaml
    /// </summary>
    public partial class DocDestinationView : Window
    {
        DocDestinationController _controller;
        public DocDestinationView()
        {
            InitializeComponent();
            _controller = new DocDestinationController();
            DataContext = _controller;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow selectedRow = sender as DataGridRow;
            if (selectedRow != null)
            {
                Destination destination = selectedRow.Item as Destination;
                if (destination != null)
                {
                    _controller.OnSelectedDestinationChanged(destination);
                }
            }
        }
    }
}
