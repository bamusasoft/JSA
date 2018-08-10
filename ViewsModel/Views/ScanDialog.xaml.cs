using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using TwainDotNet;
using TwainDotNet.Wpf;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for ScanDialog.xaml
    /// </summary>
    public partial class ScanDialog : INotifyPropertyChanged
    {
        public ScanDialog()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
            DataContext = this;
        }
        
        
        #region "Fields"
        Twain _twain;
        ScanSettings _settings;
        //
        ImageSource _scannedImageSource;
        System.Drawing.Bitmap _scannedImage;
        //
        RelayCommand _startScanCommand;
        RelayCommand _cancelCommand;
        RelayCommand _okCommand;
        #endregion
        #region "Properties"
        public ImageSource ScannedImageSource
        {
            get { return _scannedImageSource; }
            private set
            {
                _scannedImageSource = value;
                RaisePropertyChanged();
            }
        }
        public System.Drawing.Bitmap ScannedImage
        {
            get { return _scannedImage; }
            private set
            {
                _scannedImage = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region "Commands"
        public ICommand StartScanCommand
        {
            get
            {
                if (_startScanCommand == null)
                {
                    _startScanCommand = new RelayCommand(StartScan);
                }
                return _startScanCommand;
            }
        }
        public ICommand OkCommand
        {
            get 
            {
                if (_okCommand == null)
                {
                    _okCommand = new RelayCommand(Ok, CanOk);
                }
                return _okCommand;
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(Cancel);
                }
                return _cancelCommand;
            }
        }
        #endregion

        #region "Commands Methods"
        void StartScan()
        {
            IsEnabled = false;
            _settings = new ScanSettings()
            {
                UseDocumentFeeder = chkUseFeeder.IsChecked == true,
                ShowTwainUI = chkUseTwainUI.IsChecked == true
            };
            try
            {
                var source = cmbSources.SelectedItem.ToString();
                _twain.SelectSource(source);
                _twain.StartScanning(_settings);
            }
            catch (TwainException ex)
            {
                Helper.LogShowError(ex);
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
            IsEnabled = true;
            
        }
       
        void Ok()
        {
            DialogResult = true;
        }
        bool CanOk()
        {
            return ScannedImage != null;
        }
        void Cancel()
        {
            DialogResult = false;
        }
        #endregion

        #region "Helpers"
        BitmapSource CreateBitmapSource(System.Drawing.Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                                  new System.Drawing.Bitmap(bitmap).GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions());
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

        #region "Events"
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //Initilaize twain lib.
            try
            {
                _twain = new Twain(new WpfWindowMessageHook(this));
                _twain.TransferImage += OnTrasnferImage;
                _twain.ScanningComplete += OnScanningComplete;
                //
                //Populate the available twain sources in the combobox
                var sourceList = _twain.SourceNames;
                cmbSources.ItemsSource = sourceList;
                if (sourceList != null && sourceList.Count > 0)
                {
                    cmbSources.SelectedItem = sourceList[0];
                }
            }
            catch (Exception ex)
            {
                
                Helper.LogShowError(ex);
            }
            

        }

        void OnTrasnferImage(object sender, TransferImageEventArgs e)
        {
            //Note: Is you intend to support scan more than on page
            //Define a List if type Bitmap and come here to add the 
            //e.Image result to the your list.
            //Otherwise just set the e.Image to the ImageSource property
            //Take note in one page scan senario that if there are more than one page on the scanner feeder
            //only the last document will be shown on the imagesource
            if (e.Image != null)
            {
                ScannedImageSource = CreateBitmapSource(e.Image);
                ScannedImage = e.Image;

            }

        }
        void OnScanningComplete(object sender, ScanningCompleteEventArgs e)
        {
            IsEnabled = true;
        }
        #endregion
    }
}
