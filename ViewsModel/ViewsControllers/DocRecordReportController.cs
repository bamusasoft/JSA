using Jsa.DomainModel;
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
    public class DocRecordReportController : ReportControllerBase
    {

        #region Fields
        string _docId;
        string _refId;
        string _docDate;
        string _followDate;
        Destination _destination;
        string _subject;
        DocRecordStatus _status;
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

        public ObservableCollection<Destination> Destinations
        {
            get;set;
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
        }

        protected override void Refresh()
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            string sql = @"Select DocRecords.Id As DocId, DocRecords.Subject, DocRecords.RefId, DocRecords.DocDate, 
	                              DocRecords.DocPath, DocRecords.DocStatus, DocRecords.SecurityLevel,
	                              DocRecordFollows.Id AS DocFollowId, DocRecordFollows.FollowDate, 
	                              DocRecordFollows.FollowContent, DocRecordFollows.FollowPath,
	                              Destinations.Description AS Destination
                           FROM DocRecords
                           INNER JOIN DocRecordFollows
                           ON DocRecords.Id = DocRecordFollows.DocRecodId
                           INNER JOIN Destinations
                           ON DocRecords.DestId = Destinations.Id ";
           var query =  BuildQuery();
            string whereClause = "";
            object[] paramters = query.Values.ToArray(); ;
            int counter = 0;
            foreach (var item in query)
            {
                if(counter == 0)
                {
                    whereClause =  "WHERE " + item.Key;
                }
                else
                {
                    whereClause += "AND " + item.Key;
                }
                counter++;
            }
            sql += whereClause;
            using (IUnitOfWork unit = new UnitOfWork())
            {
                var s = unit.SqlQuery<DocRecordReprot>(sql, paramters).ToList(); ;
            }
        }
        #endregion

        #region Methods
        public Dictionary<string, SqlParameter> BuildQuery()
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
            return query;

        }
        #endregion
    }
}
