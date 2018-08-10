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
    /// Interaction logic for FollowingReportView.xaml
    /// </summary>
    public partial class CasesReportView : Window
    {
        private IController _controller;
        public CasesReportView()
        {
            InitializeComponent();
            _controller = new LegalCaseReportController();
            DataContext = _controller;
        }
    }
}
