using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Jsa.ViewsModel.Helpers
{
    public class UserSettings
    {
        private static UserSettings _defaultInstance;

        private UserSettings()
        {
            ReadSettings();
        }

        private void ReadSettings()
        {
            var props = typeof(UserSettings).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(UserSettingAttribute)));

            string xmlFilePath = GetFullSettingFilePath();
            foreach (var pro in props)
            {
                ReadXml(xmlFilePath, pro);
            }
        }
        public static UserSettings Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    _defaultInstance = new UserSettings();
                }
                return _defaultInstance;
            }
        }

        
        [UserSetting("CurrentYear", typeof(string))]
        public string CurrentYear { get; set;}

        [UserSetting("Branch", typeof(int))]
        public int Branch { get; set; }
        public void Save()
        {
            if(!File.Exists(GetFullSettingFilePath()))
            {
            
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string bamusaSoftFolder = appDataFolder + "\\BaMusaSoft";
                if (!Directory.Exists(bamusaSoftFolder))
                {
                    Directory.CreateDirectory(bamusaSoftFolder);
                }
                string jsaFolder = bamusaSoftFolder + "\\Jsa";
                if (!Directory.Exists(jsaFolder))
                {
                    Directory.CreateDirectory(jsaFolder);
                }
            }
            WriteValues("Branch", Branch.ToString());
        }


        XElement CreateXml(XDocument xmlDoc,string elementName, string elementValue)
        {
            var setting = new XElement("Setting");
               setting.Add(new XAttribute("Name", elementName), new XAttribute("Value", elementValue));
               return setting;

        }
        void ReadXml(string path, PropertyInfo property)
        {
            if (!File.Exists(path)) return;
            string elementName = property.Name;
            
            XDocument xDoc = XDocument.Load(path);
            var set = xDoc.Descendants("Setting").FirstOrDefault(
                        sett => sett.Attribute("Name").Value == elementName);
            if (set == null) return;
            var value = set.Attribute("Value").Value;
           
            property.SetValue(this, Convert.ChangeType(value, property.PropertyType));
        }
        string GetFullSettingFilePath()
        {
            string fullPath =null;
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            fullPath = appDataFolder + "\\BaMusaSoft" + "\\Jsa" + "\\UserSettings.xml";
            return fullPath;
        }
        void WriteValues(string settingName, string settingValue)
        {
            string fileName = GetFullSettingFilePath();
            if(File.Exists(fileName))
            {
               XDocument xdoc = XDocument.Load(fileName) ;
                var set = xdoc.Descendants("Setting").FirstOrDefault(
                        sett => sett.Attribute("Name").Value == settingName);
                if(set == null)
                {
                    xdoc.Root.Add(CreateXml(xdoc, settingName, settingValue));
                    xdoc.Save(fileName);
                }
                else
                {
                    set.Attribute("Value").Value = settingValue; 
                    xdoc.Save(fileName);
                }
            }
            else
            {
                XDocument xDoc = new XDocument(new XElement("Settings"));
                xDoc.Root.Add(CreateXml(xDoc, settingName, settingValue));
                xDoc.Save(fileName);
            }
        }
    }
}
