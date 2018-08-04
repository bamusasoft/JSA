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
using Jsa.ViewsModel.Helpers;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for SelectContractsDialog.xaml
    /// </summary>
    public partial class SelectContractsDialog : Window
    {
        public SelectContractsDialog(int customerId)
        {
            InitializeComponent();
            IDialogController controller = new SelectContractsController(customerId);
            controller.CloseDialog += OnCloseDialog;
            DataContext = controller;
        }

        void OnCloseDialog(object sender, DialogCloseState e)
        {
            switch (e)
            {
                case DialogCloseState.Ok:
                    DialogResult = true;
                    Close();
                    break;
                case DialogCloseState.Cancel:
                    DialogResult = false;
                    Close();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("e");
            }
        }
    }
}
