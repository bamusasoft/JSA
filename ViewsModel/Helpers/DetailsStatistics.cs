namespace Jsa.ViewsModel.Helpers
{
    public class DetailsStatistics
    {
        public DetailsStatistics(int amountDueSum, int amountPaidSum)
        {
            AmountDueSum = amountDueSum;
            AmountPaidSum = amountPaidSum;
        }
        public int AmountDueSum { get; private set; }
        public int AmountPaidSum { get; private set; }
         
        public double AmountPaidPercent
        {
            get
            {
                if (AmountDueSum == 0) return 0;
                return ((double)AmountPaidSum / (double)AmountDueSum) * 100;
            }
        }
    }
}
