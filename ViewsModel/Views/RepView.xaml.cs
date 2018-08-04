using System;
using System.Windows;
using Jsa.ViewsModel.ViewsControllers;
using Jsa.ViewsModel.ViewsControllers.Core;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for RepView.xaml
    /// </summary>
    public partial class RepView : Window
    {
        IController _controller;
        public event EventHandler AddCompleted;
        public RepView()
        {
            InitializeComponent();

        }
        public RepView(int customerId)
            : this()
        {
            _controller = new RepreController(customerId);
            _controller.ControllerChanged += OnControllerChanged;
            DataContext = _controller;
        }

        void OnControllerChanged(object sender, ControllerChangedEventArgs e)
        {
            RaiseAddCompleted();
            this.Close();
        }

        void RaiseAddCompleted()
        {
            if (AddCompleted != null)
            {
                AddCompleted(this, EventArgs.Empty);
            }
        }

    }
}
