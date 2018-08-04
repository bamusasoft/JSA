namespace Jsa.WinIrsService
{
    internal static class TablesQueriesStore
    {
        public static string SelectFromContracts
        {
            get
            {
                return
                    @"SELECT c.ContractNo, c.StartDate, c.EndDate, c.Customer, c.Property, c.AgreedRent, c.Deposit, c.Maintenance,
                        c.RentBal, c.DepositBal, c.MaintBal, c.Closed 
                        FROM Contracts AS c 
                        WHERE c.Closed = False";
            }
        }

        public static string SelectFromPayments
        {
            get
            {
                return
                    @"SELECT p.ContractNo, SUM(p.TotalPayment) As TotalPayment FROM Payments AS p 
                      WHERE LEFT(p.ContractNo, 2) = {0} Group by  p.ContractNo";
            }
        }

        public static string SelectFromCustomers
        {
            get
            {
                return
                    @"SELECT Code, [Name] FROM Customers";
            }
        }

        public static string SelectFromProperties
        {
            get
            {
                return
                    @"SELECT Property, [Name], Location, Type FROM Properties";
            }
        }
    }
}