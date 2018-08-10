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
    /// Interaction logic for DocRecord.xaml
    /// </summary>
    public partial class DocRecordView : Window
    {
        DocRecordController _controller;
        public DocRecordView()
        {
            InitializeComponent();
            _controller = new DocRecordController();
            DataContext = _controller;
            txtDocId.Focus();
            Closing += OnWindowClosing;
        }

        private void OnGridContentKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var uie = e.OriginalSource as UIElement;
            var textbox = uie as TextBox;
            if (textbox == null || textbox.AcceptsReturn)
            {
                return;
            }
            e.Handled = true;
            uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((!_controller.CanExit()) && !Helper.UserConfirmed(Properties.Resources.SavePrompetMsg))
            {
                e.Cancel = true;
                return;
            }

        }

        private void OnDocRecordNoKeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text)) return;
            if (e.Key != Key.Enter || e.Key != Key.Return) return;
            ((DocRecordController)_controller).GenerateDocRecordNo();
        }
    }
}
