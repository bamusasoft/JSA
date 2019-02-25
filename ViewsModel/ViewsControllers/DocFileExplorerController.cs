using GalaSoft.MvvmLight.Command;
using Jsa.DomainModel;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
            _counter = 0;
        }

        #region Fields
        static int _counter;

        ObservableCollection<DocRecordFile> _docRecordFiles;
        public event EventHandler<string> DocFilePathChanged;
        private RelayCommand _nextCommand;
        private RelayCommand _previousCommand;




        #endregion
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
        #endregion
        #region Commands
        public ICommand NextCommand
        {
            get { return _nextCommand ?? (_nextCommand = new RelayCommand(Next)); }
        }
        private void Next()
        {
            if(_counter > DocRecordFiles.Count || _counter < 0)
            {
                _counter = 0;
            }
            var next = DocRecordFiles[_counter];
            _counter++;
            RaiseFilePathChanged(next.Path);

        }

        public ICommand PreviousCommand
        {
            get { return _previousCommand ?? (_previousCommand = new RelayCommand(Previous)); }
        }
        private void Previous()
        {
            if (_counter > DocRecordFiles.Count || _counter < 0)
            {
                _counter = 0;
            }
            var next = DocRecordFiles[_counter];
            _counter--;

            RaiseFilePathChanged(next.Path);

        }
        #endregion
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        protected override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods
        private async Task LoadDocRecordFiles(string docRecordId)
        {
           DocRecordFiles =  new ObservableCollection<DocRecordFile>(await GetDocFilesAsync(docRecordId));
        }

        async Task<IList<DocRecordFile>> GetDocFilesAsync(string docRecordId)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            return await unitOfWork.DocRecordFiles.Query(x => x.DocRecordId == docRecordId).ToListAsync();
        }

        private void RaiseFilePathChanged(string path)
        {
            if (DocFilePathChanged == null)
            {
                return;
            }

            DocFilePathChanged(this, path);
        }
        #endregion
    }
}
