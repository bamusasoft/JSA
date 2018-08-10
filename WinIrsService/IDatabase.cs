using System.Data;

namespace Jsa.WinIrsService
{
    internal interface IDatabase
    {
        DataTable GetTable(ITable table);
        
    }
}