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
using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for CaseFollowingView.xaml
    /// </summary>
    public partial class CaseFollowingView : Window
    {
        private IController _controller;
        private int _id;
        public CaseFollowingView(int id)
        {
            InitializeComponent();
            _id = id;
            Initialize();
            Loaded += OnViewLoaded;
        }

        private void Initialize()
        {
            _controller = new CaseFollowingController();
            _controller.ControllerChanged += OnControllerChanged;
            DataContext = _controller;
            
        }
        protected void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            var selected = dgFollowings.SelectedItem as CaseFollowing;
            if (selected != null) ((CaseFollowingController)_controller).LoadExisted(selected.Id);
        }
        void OnControllerChanged(object sender, ControllerChangedEventArgs e)
        {
            switch (e.Action)
            {
                case ControllerAction.Cleared:
                    Initialize();
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
            if (_id == 0)
            {
                ((CaseFollowingController)_controller).CreateNew();
            }
            else
            {
                ((CaseFollowingController)_controller).LoadExisted(_id);

            }
        }
        public CaseFollowingView()
            : this(0)
        {

        }

        private void OnCaseNoKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key != Key.Enter || e.Key != Key.Return) return;
            string caseNo = txtCaseNo.Text;
            if(string.IsNullOrEmpty(caseNo))return;
            ((CaseFollowingController)_controller).CreateNew(int.Parse(caseNo));
           

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

    }
}
