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
    /// Interaction logic for DocRecordReportView.xaml
    /// </summary>
    public partial class DocRecordReportView : Window
    {
        DocRecordReportController _controller;
        public DocRecordReportView()
        {
            InitializeComponent();
            _controller = new DocRecordReportController();
            DataContext = _controller;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow selectedRow = sender as DataGridRow;
            if (selectedRow != null)
            {
                DocRecordReprot follow = selectedRow.Item as DocRecordReprot;
                if (follow != null)
                {
                    DocRecordFollowView view = new DocRecordFollowView(follow.DocId);
                    view.Owner = this;
                    view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    view.ShowDialog();
                }
            }
        }
    }
}
