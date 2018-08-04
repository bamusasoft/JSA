using System;

namespace Jsa.ViewsModel.Helpers
{
    public class SystemFileWrapper
    {
        public string Name { get; private set; }
        public DateTime Date { get; private set; }
        public string Path { get; private set; }
        public SystemFileWrapper(string name, DateTime date, string path)
        {
            Name = name;
            Date = date;
            Path = path;
        }
    }
}
