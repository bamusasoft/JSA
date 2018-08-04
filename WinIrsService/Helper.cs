using System;
using System.Text;

namespace Jsa.WinIrsService
{
    internal static class Helper
    {
        private const string Select = "SELECT ";
        private const string From = "FROM ";
        private const string Comma = ",";
        private const string Space = " ";
        private const string As = "AS ";
        private const string Dot = ".";

        public static string BuildQuery(ITable table, string[] columns, Func<string> whereClause)
        {
            var sb = new StringBuilder();
            sb.Append(SelectClause(columns, table));
            sb.Append(FromCaluse(table));
            if (whereClause != null)
            {
                sb.Append(WhereClause(table, whereClause));
            }
            return sb.ToString();
        }

        private static string SelectClause(string[] columns, ITable table)
        {
            var sb = new StringBuilder();
            sb.Append(Select);
            int current = 0;
            foreach (string column in columns)
            {
                if (columns.Length - 1 == current)
                {
                    sb.Append(table.Abbreviation + Dot + column + Space);
                    break;
                }
                sb.Append(table.Abbreviation + Dot + column + Comma + Space);

                current++;
            }
            return sb.ToString();
        }

        private static string FromCaluse(ITable table)
        {
            return From + table.Name + Space + As + table.Abbreviation + Space;
        }

        private static string WhereClause(ITable table, Func<string> where)
        {
            return where();
        }
    }
}