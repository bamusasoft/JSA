using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Jsa.ViewsModel.Helpers
{
    [Serializable]
    public class ViewClaimDetail : INotifyPropertyChanged
    {
        int _id;
        string _claimId;
        string _propertyType;
        string _propertyNo;
        string _propertyLocation;
        int _rent;
        int _maintenance;
        int _deposit;
        int _others;
        int _paid;
        int _outstandingRentBalance;
        int _outstandingMaintBalance;
        public ViewClaimDetail(int id, string claimId, string propertyType, string propertyNo, string propertyLocation, int rent, int maint,
            int deposit, int others, int paid, int outstandingRent, int outstandingMaint)
        {
            _id = id;
            _claimId = claimId;
            _propertyType = propertyType;
            _propertyNo = propertyNo;
            _propertyLocation = propertyLocation;
            _rent = rent;
            _maintenance = maint;
            _deposit = deposit;
            _others = others;
            _paid = paid;
            _outstandingRentBalance = outstandingRent;
            _outstandingMaintBalance = outstandingMaint;
        }


        public int Id
        {
            get { return _id; }
        }
        public string ClaimId
        {
            get
            {
                return _claimId;
            }
        }
        public string PropertyType
        {
            get { return _propertyType; }
        }
        public string TypeNo
        {
            get
            {
                return ExtractProeprtyNo(PropertyNo);
            }
        }
        public string PropertyNo
        {
            get { return _propertyNo; }
        }
        public string PropertyLocation
        {
            get { return _propertyLocation; }
        }
        public int Rent
        {
            get { return _rent; }
            set
            {
                _rent = value;
                RaisePropertyChanged("Rent");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("NetBalance");
            }
        }
        public int Maintenance
        {
            get { return _maintenance; }
            set
            {
                _maintenance = value;
                RaisePropertyChanged("Maintenance");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("NetBalance");

            }
        }
        public int Deposit
        {
            get { return _deposit; }
            set
            {
                _deposit = value;
                RaisePropertyChanged("Deposit");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("NetBalance");

            }
        }
        public int Others
        {
            get { return _others; }
            set
            {
                _others = value;
                RaisePropertyChanged("Others");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("NetBalance");

            }
        }
        public int Total
        {
            get
            {
                return (Rent + Maintenance + Deposit + Others);
            }

        }
        public int Paid
        {
            get { return _paid; }
            set
            {
                _paid = value;
                RaisePropertyChanged("Paid");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("NetBalance");

            }
        }
        public int Balance
        {
            get
            {
                return (Total - Paid);
            }
        }
        public int OutstandingRentBalance
        {
            get { return _outstandingRentBalance; }
            set
            {
                _outstandingRentBalance = value;
                RaisePropertyChanged("OutstandingRentBalance");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("NetBalance");

            }
        }
        public int OutstandingMaintBalance
        {
            get { return _outstandingMaintBalance; }
            set
            {
                _outstandingMaintBalance = value;
                RaisePropertyChanged("OutstandingMaintBalance");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("NetBalance");

            }
        }
        public int NetBalance
        {
            get
            {
                return (Balance + OutstandingRentBalance + OutstandingMaintBalance);
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
            }
        }
        #endregion
        private string ExtractProeprtyNo(string propertyNo)
        {
            if (propertyNo.Length < 6) return "";//These are special cases(aka ارض النحال و عزلة حارة اليمن) will be add to the printed contract by hand.
            string numPortion = propertyNo.Substring(4);
            if (numPortion.Length == 2)
            {
                string firstDigit = numPortion.Substring(0, 1);
                if (firstDigit == "0")
                {
                    string realNo = numPortion.Substring(1, 1);
                    return realNo;
                }
                return numPortion;
            }
            if (numPortion.Length == 3)
            {
                string firstTwoDigit = numPortion.Substring(0, 2);
                if (firstTwoDigit == "00")
                {
                    string realNo = numPortion.Substring(2, 1);
                    return realNo;
                }
                string firstDigit = numPortion.Substring(0, 1);
                if (firstDigit == "0")
                {
                    string realNo = numPortion.Substring(1, 2);
                    return realNo;
                }
                return numPortion;
            }
            return "";
        }
    }
}
