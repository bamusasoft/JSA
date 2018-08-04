using System;

namespace Jsa.ViewsModel.Helpers
{
    [AttributeUsage(AttributeTargets.All)]
    public class UserSettingAttribute:System.Attribute
    {
        private string _name;
        private Type _settingType;
        public UserSettingAttribute(string name, Type type)
        {
            _name = name;
            _settingType = type;
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

            }
        }
        public Type SettingType
        {
            get { return _settingType; }
            set { _settingType = value; }
        }
    }
}
