using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.DomainModel.Repositories;
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.Properties;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for SingerView.xaml
    /// </summary>
    public partial class SignerView 
    {
      
        public event EventHandler SignerSaved;
        private IController _controller;

        public SignerView() : this(null)
        {
            
        }
        public SignerView(string id)
        {
            InitializeComponent();
            CreateNewSigner(id);
        }

        void CreateNewSigner(string id)
        {
            _controller = new SignerController();
            _controller.ControllerChanged += OnControllerChanged;
            DataContext = _controller;
            if (!string.IsNullOrEmpty(id))
            {
                ((SignerController) _controller).Id = id;
                ((SignerController) _controller).SearchCommand.Execute(null);
            }
            txtSignerId.Focus();  
        }

        private void OnControllerChanged(object sender, ControllerChangedEventArgs e)
        {
            switch (e.Action)
            {
                case ControllerAction.Cleared:
                    CreateNewSigner(null);
                    break;
                case ControllerAction.Saved:
                    RaiseSignerSaved();
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

        private void RaiseSignerSaved()
        {
            if (SignerSaved != null)
            {
                SignerSaved(this, new EventArgs());
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
    }
}
