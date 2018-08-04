using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.Views
{
    /// <summary>
    /// Interaction logic for SharedFolderDialog.xaml
    /// </summary>
    public partial class SharedFolderDialog : Window, INotifyPropertyChanged
    {
        public SharedFolderDialog()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
            DataContext = this;
        }
        #region "Fields"
        RelayCommand _okCommand;
        RelayCommand _cancelCommand;

        ObservableCollection<SystemFileWrapper> _scannedFiles;
        BitmapImage _selectedImage;
        #endregion

        #region "Properties"
        
        public ObservableCollection<SystemFileWrapper> ScannedFiles
        {
            get { return _scannedFiles; }
            private set
            {
                _scannedFiles = value;
                RaisePropertyChanged();
            }
        }
        public BitmapImage SelectedImage
        {
            get { return _selectedImage; }
            private set
            {
                _selectedImage = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region "Commands"
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
        void Ok()
        {
            DialogResult = true;
        }
        bool CanOk()
        {
            return SelectedImage != null;
        }
        void Cancel()
        {
            DialogResult = false;
        }
        #endregion

        #region "Events"
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            string s = Properties.Settings.Default.ScanShareFolder;
            var result = ReadSharedFolderContent(s);
            ScannedFiles = new ObservableCollection<SystemFileWrapper>(result);
        }
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFile = dgFiles.SelectedItem as SystemFileWrapper;
            UpdateShownPicture(selectedFile);
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

        #region "Helpers"
        void UpdateShownPicture(SystemFileWrapper picFile)
        {
            if (picFile == null) return;
            SelectedImage = new BitmapImage(new Uri(picFile.Path));
        }
        private IList<SystemFileWrapper> ReadSharedFolderContent(string sharedFolderPath)
        {
            List<SystemFileWrapper> pics = new List<SystemFileWrapper>();
            string[] jpgPics = Directory.GetFiles(sharedFolderPath, "*.jpg");
            string[] pngPics = Directory.GetFiles(sharedFolderPath, "*.png");
            string[] tifPics = Directory.GetFiles(sharedFolderPath, "*.tif");
            foreach (string path in jpgPics)
            {
                SystemFileWrapper wrapper = CreateFile(path);
                pics.Add(wrapper);
            }
            foreach (string path in pngPics)
            {
                SystemFileWrapper wrapper = CreateFile(path);
                pics.Add(wrapper);
            }
            foreach (string path in tifPics)
            {
                SystemFileWrapper wrapper = CreateFile(path);
                pics.Add(wrapper);
            }
            //Sort dates in descending order form leatest to oldest.
            pics.Sort((x, y) => y.Date.CompareTo(x.Date));
            return pics;

        }
        SystemFileWrapper CreateFile(string path)
        {
            FileInfo info = new FileInfo(path);
            return new SystemFileWrapper(info.Name, info.CreationTime, path);

        }
        private void DataGridDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            e.Handled = true;
        }
        #endregion
       
    }
}
