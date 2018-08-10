namespace Jsa.ViewsModel.Reports
{
    public class ExcelProperties
    {
        public int StartRow { get; private set; }
        public int StartColumn {get; private set; }
        public bool PrintDirectely { get; private set; }

        public ExcelProperties(int startRow, int startColumn, bool printDirectely)
        {
            StartRow = startRow;
            StartColumn = startColumn;
            PrintDirectely = printDirectely;
        }
    }
}
