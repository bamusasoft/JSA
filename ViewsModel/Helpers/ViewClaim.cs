using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Jsa.ViewsModel.Helpers
{
    [Serializable]
    public class ViewClaim : INotifyPropertyChanged
    {
        string _claimId;
        short _sequenceNo;
        int _customerId;
        string _customerName;
        string _claimYear;
        //int _groundTotal;
        string _letterPartOne;
        string _letterPartTwo;
        ObservableCollection<ViewClaimDetail> _details;
        Dictionary<short, string> _sequenceTextLookup;

        public ViewClaim(string id, short sequenceNo, int customerId, string customerName, string claimYear,
                         string letterPartOne, string letterPartTwo, IList<ViewClaimDetail> details)
        {
            _claimId = id;
            _sequenceNo = sequenceNo;
            _customerId = customerId;
            _customerName = customerName;
            _claimYear = claimYear;
            _letterPartOne = letterPartOne;
            _letterPartTwo = letterPartTwo;
            _details = new ObservableCollection<ViewClaimDetail>(details);
            SubscribeDetailsChanges(_details);
            _details.CollectionChanged +=OnDetailsChanged;
            CreateSequenceLookup();


        }

        private void OnDetailsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Changed = true;

            }
           
        }

        private void SubscribeDetailsChanges(ObservableCollection<ViewClaimDetail> details)
        {
            foreach (ViewClaimDetail det in details)
            {
                det.PropertyChanged += OnDetailChanged;
            }
        }

        private void OnDetailChanged(object sender, PropertyChangedEventArgs e)
        {
            Changed = true;
            RaisePropertyChanged("GrandTotal");
            RaisePropertyChanged("RentGrandTotal");
            RaisePropertyChanged("DepositGrandTotal");
            RaisePropertyChanged("PaidGrandTotal");
        }

        private void CreateSequenceLookup()
        {
            _sequenceTextLookup = new Dictionary<short, string>();
            _sequenceTextLookup.Add(1, "رقم 1");
            _sequenceTextLookup.Add(2, "رقم 2");
            _sequenceTextLookup.Add(3, "رقم 3");
            _sequenceTextLookup.Add(-1, "نهائية");
        }

        /// <summary>
        /// Hided contructor for sake of creating new blank claim
        /// </summary>
        private ViewClaim()
        {

        }
        public static ViewClaim Create()
        {
            return new ViewClaim();
        }

        public string ClaimId
        {
            get { return _claimId; }

        }
        public short SequenceNo
        {
            get { return _sequenceNo; }
            set
            {
                _sequenceNo = value;
                RaisePropertyChanged();
            }
        }
        public string SequenceText
        {
            get
            {
                if (_sequenceTextLookup == null) return "";
                return _sequenceTextLookup[SequenceNo];

            }

        }
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                RaisePropertyChanged();
            }
        }
        public int CustomerId
        {
            get { return _customerId; }
        }
        public string ClaimYear
        {
            get { return _claimYear; }
            set
            {
                _claimYear = value;
                RaisePropertyChanged();
            }
        }
        public string LetterPartOne
        {
            get { return _letterPartOne; }
        }
        public string LetterPartTwo
        {
            get { return _letterPartTwo; }
        }
        public string ClaimLetter
        {
            get
            {
                string grandTotalInWords = SayNumber.ToWords(GrandTotal);
                string letter = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                      LetterPartOne,
                      "( ",
                      GrandTotal.ToString("#,0"),
                      " ريـال",
                      " )",
                      grandTotalInWords,
                      LetterPartTwo
                      );
                return letter;
            }
        }
        public int GrandTotal
        {
            get
            {
                if (Details == null) return 0;
                var groundTotal = Details.Sum(x => x.NetBalance);
                return groundTotal;

            }
        }
        public int RentGrandTotal
        {
            get
            {
                if (Details == null) return 0;
                return Details.Sum(x => x.Rent);
            }
        }
        public int DepositGrandTotal
        {
            get
            {
                if (Details == null) return 0;
                return Details.Sum(x => x.Deposit);
            }
        }
        public int PaidGrandTotal
        {
            get
            {
                if (Details == null) return 0;
                return Details.Sum(x => x.Paid);
            }
        }

        public IList<ViewClaimDetail> Details
        {
            get
            {
                return _details;
            }
        }

        #region INotifyPropertyChanged Members
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Changed = true;
            }
        }
        public bool Changed
        {
            get;
            private set;
        }
        public void ResetChanges()
        { 
            Changed = false;
        }
        #endregion

    }

}
