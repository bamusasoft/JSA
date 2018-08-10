using System;
using System.Windows;
using System.Windows.Data;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ContractsView.xaml
    /// </summary>
    public partial class ContractsView : Window
    {
        private readonly IController _controller;

        public ContractsView()
        {
            InitializeComponent();
            _controller = new ContractsController();
            DataContext = _controller;
            
            
        }



        private void Datagrid1_OnLayoutUpdated(object sender, EventArgs e)
        {
            custColumnHeader.UpdateLayout();
            dueColumnHeader.UpdateLayout();
            paidColumnHeader.UpdateLayout();
        }
    }

}
