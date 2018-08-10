using Jsa.DomainModel;
using Jsa.ViewsModel.Annotations;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.DomainEntities
{
    public class ContractDto
    {
        private const string FULL_YEAR = "سنة";
        private const string PART_OF_YEAR = "تكملة سنة";
        public ContractDto(int contractNo, string startDate, string endDate,
            [NotNull]Customer customer, [NotNull] Property property, int agreedRent, int agreedDeposit,
            string signDay, string signHijDate, string signGregDate, [NotNull] ContractsActivity activity, string selectedCourt)
        {
            ContractNo = contractNo;
            StartDate = startDate;
            EndDate = endDate;
            Customer = customer;
            Property = property;
            AgreedRent = agreedRent;
            AgreedDeposit = agreedDeposit;
            SignDay = signDay;
            SignHijriDate = signHijDate;
            SignGregDate = signGregDate;
            Activity = activity;
            Court = selectedCourt;

        }

        public int ContractNo { get; set; }

        public string Period
        {
            get
            {
                string startPortion = StartDate.Substring(4, 4);
                string endPortion = EndDate.Substring(4, 4);
                if (startPortion == "0101" && endPortion == "1230" ||
                   startPortion == "0101" && endPortion == "1229")
                    return FULL_YEAR;
                return PART_OF_YEAR;
            }
        }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Customer Customer { get; set; }
        public Property Property { get; set; }
        public int AgreedRent { get; set; }
        public int AgreedDeposit { get; set; }
        public string SignDay { get; set; }
        public string SignHijriDate { get; set; }
        public string SignGregDate { get; set; }
        public ContractsActivity Activity { get; set; }

        public string RentInWords
        {
            get { return SayNumber.ToWords(AgreedRent); }
        }

        public string DepositInWords
        {
            get { return SayNumber.ToWords(AgreedDeposit); }
        }

        public string Court { get; set; }
    }
}