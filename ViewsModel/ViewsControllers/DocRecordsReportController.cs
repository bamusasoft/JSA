using Jsa.DomainModel;
using Jsa.ViewsModel.Reports;
using Jsa.ViewsModel.ViewsControllers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.ViewsControllers
{
    public class DocRecordsReportController : ReportControllerBase
    {
        public DocRecordsReportController()
        {
            LoadDocStatuses();
            LoadDestinations();
            ShowProgress = false;

        }
        private void LoadDocStatuses()
        {
            DocStatuses = new Dictionary<DocRecordStatus, string>();
            DocStatuses.Add(DocRecordStatus.All, "الكل");
            DocStatuses.Add(DocRecordStatus.Open, "تحت الإجراء");
            DocStatuses.Add(DocRecordStatus.Closed, "منتهية");
            DocStatuses.Add(DocRecordStatus.Hold, "متوقفة");
            SelectedDocStatus = DocStatuses.FirstOrDefault(x => x.Key == DocRecordStatus.All);
        }
        private async Task LoadDestinations()
        {
            Task<List<Destination>> task = null;
            try
            {
                task = LoadDesintationsAsync();
                List<Destination> result = await task;
                Destinations = new ObservableCollection<Destination>(result);
                AddEmptyDestination(Destinations);
                SelectedDestination = Destinations.Where(x => x.Id == -1).FirstOrDefault(); ;
            }
            catch (Exception ex)
            {
                Helper.LogShowError(ex);
            }
        }
        private Task<List<Destination>> LoadDesintationsAsync()
        {
            Task<List<Destination>> task = Task.Run(() =>
            {
                List<Destination> destinations = null;
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    destinations = unit.Destinations.GetAll().ToList();
                }
                return destinations;
            });

            return task;
        }
        #region Fields
        string _docId;
        string _refId;
        string _docDate;
        string _notFollowedSince;
        string _hadFollowedSince;
        Destination _selectedDestination;
        string _subject;
        double _printProgress;
        bool _showProgress;
        ObservableCollection<Destination> _destinations;
        Dictionary<DocRecordStatus, string> _docStatuses;
        KeyValuePair<DocRecordStatus, string> _selectedStatus;
        //
        ObservableCollection<DocRecordsReport> _docRecordReport;
        #endregion

        #region Properties
        public string DocId
        {
            get { return _docId; }
            set
            {
                _docId = value;
                RaisePropertyChanged();
            }
        }
        public string RefId
        {
            get { return _refId; }
            set
            {
                _refId = value;
                RaisePropertyChanged();
            }
        }
        public string DocDate
        {
            get { return _docDate; }
            set
            {
                _docDate = value;
                RaisePropertyChanged();
            }
        }
        public string NotFollowedSince
        {
            get { return _notFollowedSince; }
            set
            {
                _notFollowedSince = value;
                RaisePropertyChanged();
            }
        }
        public string HadFollowedSince
        {
            get { return _hadFollowedSince; }
            set
            {
                _hadFollowedSince = value;
                RaisePropertyChanged();
            }
        }
        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Destination> Destinations
        {
            get { return _destinations; }
            set
            {
                _destinations = value;
                RaisePropertyChanged();

            }
        }
        public Destination SelectedDestination
        {
            get { return _selectedDestination; }
            set
            {
                _selectedDestination = value;
                RaisePropertyChanged();
            }
        }
        public Dictionary<DocRecordStatus, string> DocStatuses
        {
            get { return _docStatuses; }
            set
            {
                _docStatuses = value;
                RaisePropertyChanged("DocStatuses");
            }
        }
        public KeyValuePair<DocRecordStatus, string> SelectedDocStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                RaisePropertyChanged("SelectedDocStatus");

            }
        }
        public ObservableCollection<DocRecordsReport> DocRecordReport
        {
            get { return _docRecordReport; }
            set
            {
                _docRecordReport = value;
                RaisePropertyChanged();

            }
        }
        public double PrintProgress
        {
            get { return _printProgress; }
            set
            {
                _printProgress = value;
                RaisePropertyChanged();
            }
        }
        public bool ShowProgress
        {
            get { return _showProgress; }
            set
            {
                _showProgress = value;
                RaisePropertyChanged();
            }
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
            return true;

        }

        protected override bool CanRefresh()
        {
            throw new NotImplementedException();
        }

        protected override bool CanSearch()
        {
            return true;
        }

        protected override void Edit()
        {
            throw new NotImplementedException();
        }

        protected override void Print()
        {
            PrintReport();

        }
        private async Task PrintReport()
        {
            string path = Properties.Settings.Default.DocTemplate;
            if (string.IsNullOrEmpty(path))
            {
                string msg = "يجب تحديد مسار تقرير المتابعة";
                Helper.ShowMessage(msg);
                return;
            }
            try
            {
                ShowProgress = true;
                await PrintAsync(path);
                ShowProgress = false;
            }
            catch (Exception ex)
            {

                Helper.LogShowError(ex);
            }
        }
        private Task PrintAsync(string path)
        {
            Task task = Task.Run(() =>
            {
                var source = DocRecordReport.ToList();
                ExcelProperties excelProp = new ExcelProperties(2, 1, false);
                DocRecordPrintReport report = new DocRecordPrintReport(source, path, excelProp);
                report.ReportProgress += Report_ReportProgress;
                report.Print();

            });
            return task;
        }

        private void Report_ReportProgress(object sender, Helpers.ProgressEventArgs e)
        {
            PrintProgress = e.Progress;
        }

        protected override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            string sql = @"WITH p AS( 
                        SELECT a.DocRecodId, a.FollowDate, a.FollowContent
                        FROM DocRecordFollows AS a
                        INNER JOIN (
                        SELECT DocRecodId, MAX(FollowDate) AS d
                        FROM DocRecordFollows
                        GROUP BY DocRecodId
                        ) AS b ON a.DocRecodId = b.DocRecodId AND a.FollowDate = b.d)
                        SELECT 
                        DocRecords.Id, DocRecords.RefId, DocRecords.DocDate, DocRecords.DestId, 
                        DocRecords.Subject, DocRecords.DocPath, DocRecords.DocStatus, DocRecords.SecurityLevel,
                        p.FollowDate, p.FollowContent,
                        Destinations.Description as Destination
                        FROM DocRecords
                        LEFT JOIN
                        p ON DocRecords.Id = p.DocRecodId 
                        INNER JOIN
                        Destinations ON DocRecords.DestId = Destinations.Id ";
                         
            var query = BuildQuery();
            string whereClause = "";
            object[] paramters = query.Values.ToArray();
            int counter = 0;
            foreach (var item in query)
            {
                if (counter == 0)
                {
                    whereClause = "WHERE " + item.Key;
                }
                else
                {
                    whereClause += " AND " + item.Key;
                }
                counter++;
            }
            sql += whereClause;
            sql += BuildOrderBy();
            try
            {
                using (IUnitOfWork unit = new UnitOfWork())
                {
                    var s = unit.SqlQuery<DocRecordsReport>(sql, paramters).ToList();
                    DocRecordReport = new ObservableCollection<DocRecordsReport>(s);
                }
            }
            catch(Exception e)
            {
                Helper.LogShowError(e);
            }
        }
        #endregion

        #region Methods
        private Dictionary<string, SqlParameter> BuildQuery()
        {
            Dictionary<String, SqlParameter> query = new Dictionary<string, SqlParameter>();
            if (!string.IsNullOrEmpty(DocId))
            {
                query.Add("DocRecords.Id = @DocId ", new SqlParameter("@DocId", DocId));
            }
            if (!string.IsNullOrEmpty(RefId))
            {
                query.Add("DocRecords.RefId = @RefId", new SqlParameter("@RefId", RefId));

            }
            if (SelectedDocStatus.Key != DocRecordStatus.All)
            {
                query.Add("DocRecords.DocStatus = @DocStatus", new SqlParameter("@DocStatus", SelectedDocStatus.Key));
            }
            if (SelectedDestination != null && SelectedDestination.Id != -1)
            {
                query.Add("DocRecords.DestId = @DestId", new SqlParameter("@DestId", SelectedDestination.Id));

            }
            if (!string.IsNullOrEmpty(Subject))
            {
                query.Add("DocRecords.Subject LIKE @Subject", new SqlParameter("@Subject", Subject));

            }
            if (!string.IsNullOrEmpty(NotFollowedSince))
            {
                query.Add("FollowDate <= @NotFollowed", new SqlParameter("@NotFollowed", NotFollowedSince));

            }
            if (!string.IsNullOrEmpty(HadFollowedSince))
            {
                query.Add("FollowDate >= @HadFollowed", new SqlParameter("@HadFollowed", HadFollowedSince));

            }


            return query;

        }
        private string BuildOrderBy()
        {
            string orderBy = " ORDER BY DestId, DocDate";
            return orderBy;
        }

        /// <summary>
        /// Use this method to add a reset item, so user has the choice to not selected any of the item in combox.
        /// The id of added item is -1 which you can use to test for that.
        /// </summary>
        /// <param name="list"></param>
        private void AddEmptyDestination(ObservableCollection<Destination> list)
        {
            Destination d = new Destination();
            d.Id = -1;
            d.Description = "اختر جهة";
            list.Insert(0, d);
        }
        #endregion

    }
}
