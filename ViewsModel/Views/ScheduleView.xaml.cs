using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Jsa.ViewsModel.DomainEntities;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ScheduleView.xaml
    /// </summary>
    public partial class ScheduleView
    {
        #region "Fields" 
        
     
        private ScheduleViewController _controller;
        #endregion

        #region "Constr"
        public ScheduleView()
            :this(null)
        {
           
        }

        public ScheduleView(string scheduleId )
        {
            InitializeComponent();
            NewSchedule(scheduleId);

        }

        private void OnControllerChanged(object sender, ControllerChangedEventArgs e)
        {
            switch (e.Action)
            {
                case ControllerAction.Cleared:
                    NewSchedule(null);
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

        private void NewSchedule(string scheduleId)
        {
            _controller = new ScheduleViewController();
            _controller.ControllerChanged += OnControllerChanged;
            DataContext = _controller;
            if (!string.IsNullOrEmpty(scheduleId))
            {
                _controller.ScheduleId = scheduleId;
                _controller.SearchCommand.Execute(null);
            }
            txtScheduleId.Focus();  
        }

        #endregion

      

       
       
      

        #region "UI Controls Events"
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (!_controller.OkExit())
            {
                var msg = Properties.Resources.SavePrompetMsg;
                if (!Helper.UserConfirmed(msg))
                {
                    e.Cancel = true;
                }
            }
            //ReleaseResources();
        }

        private void OnDetailsGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDetail = dgDetails.SelectedItem as ScheduleDetailsController;
            _controller.SelectedDetail = selectedDetail;


        }

        
        
        #endregion

        private void OnCustomersKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                _controller.FindCustomerContracts();
            }
        }

        private void OnSignersKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                _controller.SetSigner();
                dgDetails.Focus();
            }
        }

        private void OnScheduleIdKeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Key == Key.Enter || e.Key == Key.Return))return;
            _controller.GenerateScheduleId();
            
            Helper.MoveFocus(e);
            e.Handled = true;
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
            //e.Handled = true;
            uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        
        }

        
    }

    
}
