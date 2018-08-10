using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace Jsa.ViewsModel.Helpers
{
    
    public static class Logger
    {

        private static string LogFilePath
        {
            get
            {
                return Properties.Settings.Default.LogFilePath;
            }
        }
        const long MaxFileSize = 100000;

        public static void Log(LogMessageTypes type, string msg, string targetSite, string trace)
        {
            if (string.IsNullOrEmpty(LogFilePath)) return;

            var dateAndTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            XDocument xdoc;
            //Replace this portion with a command in options view for example to clear the log file if is exceeds a certin size.
            //if (File.Exists(LogFilePath))
            //{
            //    var info = new FileInfo(LogFilePath);
            //    var length = info.Length;
            //    if (length >= MaxFileSize)
            //    {
            //        info.Delete();
            //        Log(type, msg, targetSite, trace);
            //    }
               
            //}
            if (File.Exists(LogFilePath))
            {
                xdoc = XDocument.Load(LogFilePath);
                var xElement = xdoc.Element("Logs");
                if (xElement != null)
                    xElement.Add(
                        new XElement("Log",
                                     new XElement("DateAndTime", dateAndTime),
                                     new XElement("Type", type.ToString()),
                                     new XElement("Message",msg),
                                     new XElement("TargetSite", targetSite),
                                     new XElement("Tarce", trace)

                            ));
            }
            else
            {
                xdoc = new XDocument(
                new XElement("Logs",
                    new XElement("Log",
                    new XElement("DateAndTime", dateAndTime),
                    new XElement("Type", type.ToString()),
                    new XElement("Message", msg),
                    new XElement("TargetSite", targetSite),
                    new XElement("Tarce", trace)

                    )));
            }

            xdoc.Save(LogFilePath);
        }
        public static  void Log(LogMessageTypes type,string msg)
        {
            Log(type,msg, "NoNeed", "NoNeed");
        }
    }
}
