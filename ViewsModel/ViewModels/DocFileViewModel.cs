using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.ViewModels
{
    public class DocFileViewModel : ViewModelBase
    {


        string _id;
        string _docRecordId;
        string _docFollowId;
        string _path;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }



        public string DocRecordId
        {
            get
            {
                return _docRecordId;
            }
            set
            {
                _docRecordId = value;
                RaisePropertyChanged();
            }
        }



        public string DocFollowId
        {
            get
            {
                return _docFollowId;
            }
            set
            {
                _docFollowId = value;
                RaisePropertyChanged();
            }
        }



        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RaisePropertyChanged();
            }
        }

    }
}
