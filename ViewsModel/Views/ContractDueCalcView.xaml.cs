using System.Windows;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ContractDueCalcView.xaml
    /// </summary>
    public partial class ContractDueCalcView : Window
    {
        IController _controller;
        public ContractDueCalcView()
        {
            InitializeComponent();
        }
        public ContractDueCalcView(int contractNo):this()
        {
            _controller = new AmountDueCalcController(contractNo);
            DataContext = _controller;

        }
    }
}
