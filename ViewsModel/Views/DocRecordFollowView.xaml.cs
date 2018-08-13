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
    /// Interaction logic for DocRecordFollow.xaml
    /// </summary>
    public partial class DocRecordFollowView : Window
    {
        DocRecordFollowController _controller;
        public DocRecordFollowView()
        {
            InitializeComponent();
            _controller = new DocRecordFollowController();
            DataContext = _controller;
        }
        public DocRecordFollowView(string docId):this()
        {
            _controller.ShowDocFollow(docId);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow selectedRow = sender as DataGridRow;
            if(selectedRow != null)
            {
                DocRecordFollow recordFollow = selectedRow.Item as DocRecordFollow;
                if(recordFollow != null)
                {
                    _controller.OnSelectedFollowChanged(recordFollow);
                }
            }
            
        }
    }
}
