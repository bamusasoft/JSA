using Jsa.DomainModel;
using Jsa.ViewsModel.Mediator;
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
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for DocRecordFollow.xaml
    /// </summary>
    public partial class DocRecordFollowView : Window
    {
        OpenDialogProxy _dialog;
        DocRecordFollowController _controller;
        public DocRecordFollowView()
        {
            InitializeComponent();
            _dialog = new OpenDialogProxy();
            _dialog.OpenDialog += Dialog_OpenDialog;
            _controller = new DocRecordFollowController(_dialog);
            DataContext = _controller;
            _controller.DocFilePathChanged += OnDocFilePathChanged;

        }

        private void OnDocFilePathChanged(object sender, string e)
        {
            if (!string.IsNullOrEmpty(e))
            {
                browser.Navigate("file:///" + e);
            }
            else
            {
                browser.NavigateToString("No File Selected");
            }
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

        private void OnAddDocFile(object sender, RoutedEventArgs e)
        {
            string fileName = OpenPdfFile();
            if (!string.IsNullOrEmpty(fileName))
            {
                _controller.FollowPath = fileName;
            }
        }

        private string OpenPdfFile()
        {
            var filter = "PDF Files (.pdf)|*.pdf";
            var dlg = new OpenFileDialog();
            dlg.Filter = filter;
            if (dlg.ShowDialog() == true) return dlg.FileName;
            return null;
        }
        void Dialog_OpenDialog(object sender, object e)
        {
            string docRecordId = e as string;
            DocFileExplorer explorer = new DocFileExplorer(docRecordId);
            explorer.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            explorer.ShowDialog();
        }
    }

}
