namespace Jsa.ViewsModel.Reports
{
    public struct ReportLayout
    {
        public bool AddHeaderLine
        {
            get;
            private set;
        }
        public ReportLayout(bool addHeaderLine)
            :this()
        {
            AddHeaderLine = addHeaderLine;
        }
    }
}
