using System.Data;
using System.Data.OleDb;

namespace Jsa.ViewsModel.Helpers
{
    public class Excel
    {
        readonly string _filePath;
        static string _excelQuery = "Select * from [Sheet1$]";
        public Excel(string filePath)
        {
            _filePath = filePath;
        }
        public DataTable ReadContracts()
        {
            DataTable table = new DataTable();
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + _filePath + "; Extended Properties =Excel 8.0;";
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand command = new OleDbCommand(_excelQuery, conn))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                }

            }
            return table;
        }
    }
}
