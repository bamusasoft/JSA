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
    /// Interaction logic for CaseAppointView.xaml
    /// </summary>
    public partial class CaseAppointView : Window
    {
        private IController _controller;
        public CaseAppointView()
        {
            InitializeComponent();
            _controller = new CaseAppointmentController();
            _controller.ControllerChanged += OnControllerChanged;
            DataContext = _controller;
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
        protected void OnHyperAppointClick(object sender, RoutedEventArgs e)
        {
            var appint = dgAppointments.SelectedItem as CaseAppointment;
            ((CaseAppointmentController)_controller).ShowAppointment(appint);
        }

        protected void OnHyperCaseClick(object sender, RoutedEventArgs e)
        {
            var legalCase = dgCases.SelectedItem as LegalCase;
            ((CaseAppointmentController)_controller).LoadCaseAppointments(legalCase);
        }
    }
}
