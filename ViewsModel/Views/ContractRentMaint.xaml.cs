using System;
using System.Windows;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ContractRentMaint.xaml
    /// </summary>
    public partial class ContractRentMaint : Window
    {
        private IController _controller;
        public ContractRentMaint()
        {
            InitializeComponent();
            _controller = new RentMaintController();
            DataContext = _controller;
            _controller.ControllerChanged += OnControllerChanged;
        }

        void OnControllerChanged(object sender, ControllerChangedEventArgs e)
        {
            switch (e.Action)
            {
                case ControllerAction.Cleared:
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
    }
}
