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

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for DocFileExplorer.xaml
    /// </summary>
    public partial class DocFileExplorerView : Window
    {
        readonly DocFileExplorerController _controller;
        public DocFileExplorerView(string docRecordId)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(docRecordId))
            {
                return;
            }
            _controller = new DocFileExplorerController(docRecordId);
            DataContext = _controller;
            _controller.DocFilePathChanged += OnDocFilePathChanged;
        }

        private void OnDocFilePathChanged(object sender, string e)
        {
            if (!string.IsNullOrEmpty(e))
            {
                try
                {
                    browser.Navigate("file:///" + e);
                }
                catch (Exception ex)
                {
                   Helper.LogShowError(ex);
                }
               
            }
            else
            {
                browser.NavigateToString("No File Selected");
            }
        }
    }
}
