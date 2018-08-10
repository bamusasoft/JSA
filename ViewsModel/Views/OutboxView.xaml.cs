using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for OutboxView.xaml
    /// </summary>
    public partial class OutboxView : Window
    {
        readonly IController _controller;
        public OutboxView()
        {
            InitializeComponent();
            _controller = new OutboxController();
            DataContext = _controller;
            txtOutboxNo.Focus();
            Closing += OnWindowClosing;
            _controller.ControllerChanged +=OnControllerChanged;

        }

        private void OnControllerChanged(object sender, ControllerChangedEventArgs e)
        {
            switch (e.Action)
            {
                case ControllerAction.Cleared:
                    txtOutboxNo.Focus();
                    break;
                case ControllerAction.Saved:
                    break;
                case ControllerAction.Edited:
                    break;
                case ControllerAction.Deleted:
                    break;
                case ControllerAction.Invalid:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!((OutboxController)_controller).CanExit && !Helper.UserConfirmed(Properties.Resources.SavePrompetMsg))
            {
                e.Cancel = true;
                return;
            }

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
        protected void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            var selected = listOutboxes.SelectedItem as Outbox;
            if (selected != null) ((OutboxController)_controller).Show(selected);
        }

        private void OnOutboxNoKeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text)) return;
            if (e.Key != Key.Enter || e.Key != Key.Return) return;
            ((OutboxController)_controller).GenerateOutboxNo();
        }

    }
}
