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
    /// Interaction logic for AddScheduleDetailsDialog.xaml
    /// </summary>
    public partial class AddScheduleDetailsDialog : Window
    {
        private readonly IDialogController _controller;
        public AddScheduleDetailsDialog(int contractNo)
        {
            InitializeComponent();
            _controller = new AddSchedulDetailsController(contractNo);
            _controller.CloseDialog += OnCloseDialog;
            DataContext = _controller;
            dgDetails.InitializingNewItem += dgDetails_InitializingNewItem;
            
        }

        private void dgDetails_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            var detail = e.NewItem as ScheduleDetailsController;
            if (detail != null)
            {
                if (detail.ContractNo == 0)
                {
                    ((AddSchedulDetailsController) _controller).SetContractNo(detail);
                }
            }
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

        

        private void OnDetailsGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgDetails.SelectedItem as ScheduleDetailsController;
            ((AddSchedulDetailsController) _controller).Selected = selected;
        }

    }
}
