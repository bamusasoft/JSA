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
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for LegalCaseView.xaml
    /// </summary>
    public partial class LegalCaseView : Window
    {
        private IController _controller;
        private int _caseNo;
        public LegalCaseView(int caseNo)
        {
            InitializeComponent();
            _controller = new LegalCaseController();
            _controller.ControllerChanged += OnControllerChanged;
            DataContext = _controller;
            Loaded += OnViewLoaded;

        }

        void OnControllerChanged(object sender, ControllerChangedEventArgs e)
        {
            switch (e.Action)
            {
                case ControllerAction.Cleared:
                    if (_controller != null)
                    {
                        _controller = new LegalCaseController();
                        _controller.ControllerChanged += OnControllerChanged;
                        DataContext = _controller;
                        ((LegalCaseController)_controller).CreateNew();
                        txtCaseNo.Focus();

                    }
                    break;
                case ControllerAction.Saved:
                    break;
                case ControllerAction.Edited:
                    break;
                case ControllerAction.Deleted:
                    break;
                case ControllerAction.Invalid:
                    Helper.ShowMessage("OOPs, Something went wrong");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            if (_caseNo == 0)
            {
                ((LegalCaseController)_controller).CreateNew();
            }
            else
            {
                ((LegalCaseController)_controller).LoadExisted(_caseNo);

            }
            txtCaseNo.Focus();
        }
        public LegalCaseView()
            : this(0)
        {

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

        private void OnCaseNoKeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text)) return;
            if (e.Key != Key.Enter || e.Key != Key.Return) return;
            ((LegalCaseController)_controller).CheckExitence();
            e.Handled = false;
        }
    }
}
