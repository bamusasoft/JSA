using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for AddActivityView.xaml
    /// </summary>
    public partial class AddActivityView : Window, INotifyPropertyChanged
    {
        public AddActivityView()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += OnWindowLoaded;
            Closing += OnWindowClosing;
        }

        
        #region Fields

        private IUnitOfWork _unitOfWork;
        private event EventHandler<ViewState> ViewStateChanged;
        private ViewState _currentViewState;
        private bool _canExit;
        private List<RuleViolation> _validationRules;
        //
        private string _description;
        //
        private RelayCommand _saveCommand;
        private RelayCommand _clearCommand;
        private RelayCommand _exitCommand;

        #endregion
        #region Properties
        public  string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
                RaiseViewStateChanged(ViewState.Edited);

            }
        }
        #endregion
        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                if(_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(Save);
                }
                return _saveCommand;
            }

        }
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand(ClearView);
                }
                return _clearCommand;
            }

        }
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(Exit);
                }
                return _exitCommand;
            }

        }
        #endregion
        #region Commands Methods
        private void Save()
        {
            try
            {
                if(!ValidActivity())
                {
                    string msg = _validationRules[0].ErrorMessage;
                    Logger.Log(LogMessageTypes.Info, msg);
                    Helper.ShowMessage(msg);
                    return;
                }
                var activity = new ContractsActivity();
                ReadActivityValue(activity);
                _unitOfWork.ContractsActivities.Add(activity);
                _unitOfWork.Save();
                RaiseViewStateChanged(ViewState.Saved);

            }
            catch (Exception ex)
            {
                string msg = Helper.ProcessExceptionMessages(ex);
                Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(),ex.StackTrace);
                Helper.ShowMessage(msg);

            }
        }
        void ClearView()
        {
            if (CanExit())
            {
                ClearData();
                RaiseViewStateChanged(ViewState.Blank);
            }
        }
        void Exit()
        {
            Close();
        }
 
        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Events
        private  void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            ViewStateChanged += OnViewStateChanged;
            InitializeFields();
        }

        

        void OnViewStateChanged(object sender, ViewState e)
        {
            ControlState(e);
        }

        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if(!CanExit())
            {
                e.Cancel = true;
                return;
            }
            _unitOfWork.Dispose();
            DialogResult = true;

        }

        #endregion

        #region Helpers

        enum ViewState
        {
            Blank,
            Edited,
            Saved
        }
        private void ControlState(ViewState state)
        {
            _currentViewState = state;
            switch (_currentViewState)
            {
                case ViewState.Blank:
                    _canExit = true;
                    break;
                case ViewState.Edited:
                    _canExit = false;
                    break;
                case ViewState.Saved:
                    _canExit = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        void RaiseViewStateChanged(ViewState state)
        {
            if(ViewStateChanged != null)
            {
                ViewStateChanged(this, state);
            }
        }
        private void InitializeFields()
        {
            _unitOfWork = new UnitOfWork();
            _validationRules = new List<RuleViolation>();
            RaiseViewStateChanged(ViewState.Blank);
        }
        private bool CanExit()
        {
            if (!_canExit)
            {
                string msg = Properties.Resources.SavePrompetMsg;
                bool agree = Helper.UserConfirmed(msg);
                return agree;
            }
            return _canExit;
        }
        void ClearData()
        {
            Description = "";
        }
        void ReadActivityValue(ContractsActivity activity)
        {
            if (activity == null) throw new ArgumentNullException("activity");
            activity.Description = Description;
        }
        bool ValidActivity()
        {
            _validationRules.Clear();
            if(string.IsNullOrEmpty(Description))
            {
                string msg = Properties.Resources.ActivityView_DescriptionMissing;
                _validationRules.Add(new RuleViolation(msg));
                return false;
            }
            return true;
        }
        #endregion
    }
}
