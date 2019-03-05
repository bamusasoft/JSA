using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class DocFileExplorerController : ReportControllerBase
    {
        public DocFileExplorerController(string docRecordId)
        {
            if (string.IsNullOrEmpty(docRecordId))
            {
                throw new ArgumentException($"{nameof(docRecordId)} is null or empty.", nameof(docRecordId));
            }

            _ = LoadDocRecordFiles(docRecordId);
            counter = 0;
            _docRecordFolder = Properties.Settings.Default.DocFileFolder;
        }

        #region Fields

        private static int counter;

        private readonly string _docRecordFolder;
        private ObservableCollection<DocRecordFile> _docRecordFiles;
        private string _docRecordDescription;

        private RelayCommand _nextCommand;

        private RelayCommand _previousCommand;

        public event EventHandler<string> DocFilePathChanged;
        #endregion Fields
        #region Properties


        public ObservableCollection<DocRecordFile> DocRecordFiles
        {
            get { return _docRecordFiles; }
            set
            {
                _docRecordFiles = value;
                RaisePropertyChanged();
            }
        }
        public string DocRecordDescription
        {
            get { return _docRecordDescription; }
            set
            {
                _docRecordDescription = value;
                RaisePropertyChanged();
            }
        }
        #endregion Properties

        #region Commands

        public ICommand NextCommand
        {
            get { return _nextCommand ?? (_nextCommand = new RelayCommand(Next)); }
        }

        public ICommand PreviousCommand
        {
            get { return _previousCommand ?? (_previousCommand = new RelayCommand(Previous)); }
        }

        

        private void Next()
        {
            if (counter >= DocRecordFiles.Count || counter < 0)
            {
                counter = 0;
            }
            var next = DocRecordFiles[counter];
            counter++;
            string path = Path.Combine(_docRecordFolder, next.Path);
            RaiseFilePathChanged(path);
        }
        private void Previous()
        {
            if (counter >= DocRecordFiles.Count || counter < 0)
            {
                counter = 0;
            }
            var prev = DocRecordFiles[counter];
            counter--;
            string path = Path.Combine(_docRecordFolder, prev.Path);
            RaiseFilePathChanged(path);
        }

        #endregion Commands

        #region Base

        public override void ControlState(ControllerStates state)
        {
            throw new NotImplementedException();
        }

        protected override bool CanEdit()
        {
            throw new NotImplementedException();
        }

        protected override bool CanPrint()
        {
            if (DocRecordFiles == null)
            {
                return false;
            }
            return DocRecordFiles.Count > 0;
        }

        protected override bool CanRefresh()
        {
            throw new NotImplementedException();
        }

        protected override bool CanSearch()
        {
            throw new NotImplementedException();
        }

        protected override void Edit()
        {
            throw new NotImplementedException();
        }

        protected override void Print()
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.Verb = "Print";
                info.CreateNoWindow = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                Queue<Process> processes = new Queue<Process>();
                foreach (var file in DocRecordFiles)
                {
                   info.FileName = Path.Combine(_docRecordFolder, file.Path);
                   Process process = new Process { StartInfo = info };
                   process.Start();

                   processes.Enqueue(process);
                }

                while (processes.Count > 0)
                {
                    var process = processes.Dequeue();
                    process.WaitForExit();
                    System.Threading.Thread.Sleep(3000);
                    if (process.CloseMainWindow() == false)
                    {
                        process.Kill();
                    }

                }
                
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }

        protected override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            throw new NotImplementedException();
        }

        #endregion Base

        #region Methods

        private async Task<IList<DocRecordFile>> GetDocFilesAsync(string docRecordId)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            return await unitOfWork.DocRecordFiles.Query(x => x.DocRecordId == docRecordId).ToListAsync();
        }

        private async Task LoadDocRecordFiles(string docRecordId)
        {
            DocRecordDescription = await GetDocRecordDescription(docRecordId);
            DocRecordFiles = new ObservableCollection<DocRecordFile>(await GetDocFilesAsync(docRecordId));
        }

        private async Task<string> GetDocRecordDescription(string docRecordId)
        {
            using (IUnitOfWork unit  = new UnitOfWork())
            {
                var docRecord = await unit.DocRecords.Query(x => x.Id == docRecordId).SingleOrDefaultAsync();
                if(docRecord == null)
                {
                    return string.Empty;
                }

                return docRecord.Subject;
            }
        }

        private void RaiseFilePathChanged(string path)
        {
            DocFilePathChanged?.Invoke(this, path);
        }

        #endregion Methods
    }
}