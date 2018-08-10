namespace Jsa.ViewsModel.Helpers
{
    public struct GridColumnPreference
    {
        public string Header { get; private set; }
        public int DispalyOrder { get; private set; }
        public double Width { get; private set; }
        public GridColumnPreference(string header, int dispalyOrder, double width)
            :this()
        {
            Header = header;
            DispalyOrder = dispalyOrder;
            Width = width;
        }
    }
}
