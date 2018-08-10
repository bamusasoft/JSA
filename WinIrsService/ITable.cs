namespace Jsa.WinIrsService
{
    internal interface ITable
    {
        string[] Columns { get; }
        string Name { get; }
        string Abbreviation { get; }
        string BuildQuery();
    }
}