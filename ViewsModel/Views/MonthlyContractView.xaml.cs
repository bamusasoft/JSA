using System.Windows;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for MonthlyContractView.xaml
    /// </summary>
    public partial class MonthlyContractView : Window
    {
        public MonthlyContractView()
        {
            InitializeComponent();
        }
        public MonthlyContractView(int contractNo):this()
        {
            IController controller = new MonthlyContractController(contractNo);
            DataContext = controller;
        }
    }
}
