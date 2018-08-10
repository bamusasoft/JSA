using System.Collections.Generic;

namespace Jsa.ViewsModel.Helpers
{
    public class SearchedClaim
    {
        public string ClaimId { get; set; }
        public string CustomerName { get; set; }
        public string SequenceNo
        {
            get
            {
                if (_sequenceTextLookup == null) return "";
                return _sequenceTextLookup[_sequenceNo];
            }
        }
        short _sequenceNo { get; set; }
        public string Year { get; set; }
        Dictionary<short, string> _sequenceTextLookup;
        public SearchedClaim(string claimId, string customerName, short SeqeunceNo, string year)
        {
            ClaimId = claimId;
            CustomerName = customerName;
            _sequenceNo = SeqeunceNo;
            Year = year;
            CreateSequenceLookup();
            
        }


        private void CreateSequenceLookup()
        {
            _sequenceTextLookup = new Dictionary<short, string>();
            _sequenceTextLookup.Add(1, "رقم 1");
            _sequenceTextLookup.Add(2, "رقم 2");
            _sequenceTextLookup.Add(3, "رقم 3");
            _sequenceTextLookup.Add(-1, "نهائية");
        }
    }
}
