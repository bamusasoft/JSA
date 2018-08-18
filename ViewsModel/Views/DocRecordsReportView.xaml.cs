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
    /// Interaction logic for DocRecordsReportView.xaml
    /// </summary>
    public partial class DocRecordsReportView : Window
    {
        DocRecordsReportController _controller;
        public DocRecordsReportView()
        {
            InitializeComponent();
            _controller = new DocRecordsReportController();
            DataContext = _controller;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow selectedRow = sender as DataGridRow;
            if (selectedRow != null)
            {
                DocRecordsReport doc = selectedRow.Item as DocRecordsReport;
                if (doc != null)
                {
                    DocRecordFollowView view = new DocRecordFollowView(doc.Id);
                    view.Owner = this;
                    view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    view.ShowDialog();
                }
            }
        }
    }
}
