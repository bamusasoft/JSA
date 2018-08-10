using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jsa.DomainModel;
using System.Globalization;

namespace Jsa.ViewsModel.Helpers
{
    public class CustomersRules
    {
        /// <summary>
        /// Class A has all payments in Muharram.
        /// </summary>
        /// <param name="payments"></param>
        /// <returns></returns>
        public static bool IsInClassA(List<Payment> payments)
        {
            if (payments.Count == 0) return false;
            foreach (var payment in payments)
            {
                if (!PaymentHappenInHisContractYear(payment))
                {
                    return false;
                }
                if(ContractHasBalance(payment.ContractNo))
                {
                    return false;
                }
            }
            bool result = true;
            var average = AveragePaymentDate(payments);

            string monthDayProtion = average.Substring(4, 4);
            //var compare = String.CompareOrdinal(monthDayProtion, "0130");
            //if (compare > 0)
            //{
            //    result = false;
            //}
            //return result;
            return PayFallInRange(monthDayProtion, "0101", "0230");
            
        }
        
        /// <summary>
        /// Class B has an average of last payments between 01-01 to 30-03
        /// </summary>
        /// <param name="payments"></param>
        /// <returns></returns>
        public static bool IsInClassB(List<Payment> payments)
        {
            if (payments.Count == 0) return false;
            foreach (var payment in payments)
            {
                if (!PaymentHappenInHisContractYear(payment))
                {
                    return false;
                }
                if (ContractHasBalance(payment.ContractNo))
                {
                    return false;
                }
            }
            bool result = true;
            var average = AveragePaymentDate(payments);

            string monthDayProtion = average.Substring(4, 4);
            // var compare = String.CompareOrdinal(monthDayProtion, "0330");
            // if (compare > 0)
            // {
            //     result = false;
            // }
            //return result;
            return PayFallInRange(monthDayProtion, "0301", "0630");
        }

        /// <summary>
        /// Class C has an average of last payments if between 01-01 to 30-06
        /// </summary>
        /// <param name="payments"></param>
        /// <returns></returns>
        public static bool IsInClassC(List<Payment> payments)
        {

            if (payments.Count == 0) return false;
            foreach (var payment in payments)
            {
                if (!PaymentHappenInHisContractYear(payment))
                {
                    return false;
                }
                if (ContractHasBalance(payment.ContractNo))
                {
                    return false;
                }
            }
            bool result = true;
            var average = AveragePaymentDate(payments);

            string monthDayProtion = average.Substring(4, 4);
            //var compare = String.CompareOrdinal(monthDayProtion, "0630");
            //if (compare > 0)
            //{
            //    result = false;
            //}
            //return result;
            return PayFallInRange(monthDayProtion, "0701", "1020");

        }

        
        /// <summary>
        /// Class F has an average of last payments is greater than 16-10
        /// </summary>
        /// <param name="payments"></param>
        /// <returns></returns>
        public static bool IsInClassF(List<Payment> payments)
        {

            if (payments.Count == 0) return false;
            foreach (var payment in payments)
            {
                if (!PaymentHappenInHisContractYear(payment))
                {
                    return true; //This customer has payments happen for the contract in the next year. add him to F.
                }
                if (ContractHasBalance(payment.ContractNo))
                {
                    return false;
                }
            }
            bool result = true;
            var average = AveragePaymentDate(payments);

            string monthDayProtion = average.Substring(4, 4);

            //var compare = String.CompareOrdinal(monthDayProtion, "1015");
            //if (compare < 0)
            //{
            //    result = false;
            //}
            //return result;
            return PayFallInRange(monthDayProtion, "1021", "1230");
        }
        /// <summary>
        /// Balcklist is any customer has balance in rent.
        /// </summary>
        /// <param name="payments"></param>
        /// <returns></returns>
        public static bool IsInBlacklist(List<Payment> payments)
        {
            if (payments.Count == 0) return false;
            foreach (var payment in payments)
            {
                if(ContractHasBalance(payment.ContractNo))
                {
                    return true;
                }
               
            }
            return false; //If he doesn't have balance in rent then he is not in Blacklist.
        }
        static bool PaymentHappenInHisContractYear(Payment p)
        {
            using (IUnitOfWork w = new UnitOfWork())
            {
                var contract = w.Contracts.GetById(p.ContractNo);
                return p.PayDate.Contains(contract.ContractYear);
            }
        }
        static string AveragePaymentDate(List<Payment> payments)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (var payment in payments)
            {
                UmAlQuraCalendar calendar = new UmAlQuraCalendar();
                string s = payment.PayDate;
                int d = int.Parse(s.Substring(6, 2));
                int m = int.Parse( s.Substring(4, 2));
                //DateTime d = DateTime.Parse(Helper.ApplyDateMask(payment.PayDate));
                //We have to change the year of all payments to the current year, so all payments will be 
                //treated eqaully when getting average and the average will be calculated based on the difference
                // only in month and day.
                DateTime dateAfterAddingFakeYear;
                try
                {
                    dateAfterAddingFakeYear = new DateTime(1437, m, d, calendar);
                }
                catch (ArgumentOutOfRangeException) //This is in case of the day of month is 30 where calendar sees it only up to 29
                {
                    d = d - 1;
                    dateAfterAddingFakeYear = new DateTime(1437, m, d, calendar);
                }
               
                dates.Add(dateAfterAddingFakeYear);
            }
            var count = payments.Count;
            double temp = 0D;
            for (int i = 0; i < count; i++)
            {
                temp += dates[i].Ticks / (double)count;
            }
            return new DateTime((long)temp).Date.ToString("yyyyMMdd");

        }
        static bool PayFallInRange(string payDate, string start, string end)
        {
            //int startResult = string.CompareOrdinal(payDate, start);
            //int endResult = string.CompareOrdinal(payDate, end);
            //return false;
            return (string.CompareOrdinal(payDate, start) >= 0
                &&
                string.CompareOrdinal(payDate, end) <= 0

                );
        }
        static bool ContractHasBalance(int contractNo)
        {
            using (IUnitOfWork w = new UnitOfWork())
            {
                return w.Contracts.GetById(contractNo).RentBalance > 0;
            }
        }


    }
}
