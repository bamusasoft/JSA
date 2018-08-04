using System;
using System.Windows.Data;
using System.Windows;

namespace Jsa.ViewsModel.Converters
{
    public class OutboxNoConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value as string;
            if (string.IsNullOrEmpty(s))
            {
                return DependencyProperty.UnsetValue;
            }
            return ReverseOutbox(s);



        }

        private static string ReverseOutbox(string s)
        {
            string revsersed = string.Empty;
            if (s.Length == 8)
            {
                string yearPortion = s.Substring(0, 4);
                string noPortion = s.Substring(4, 4);
                revsersed = string.Format("{0}{1}", noPortion, yearPortion);
            }
            return revsersed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value as string;
            if (string.IsNullOrEmpty(s))
            {
                return DependencyProperty.UnsetValue;
            }
            return UnReverseOutboxNo(s);
        }

        private static string UnReverseOutboxNo(string s)
        {
            string unRevsersed = string.Empty;
            if (s.Length == 8)
            {
                string noPortion = s.Substring(0, 4);
                string yearPortion = s.Substring(4, 4);
                unRevsersed = string.Format("{0}{1}", yearPortion, noPortion);
            }
            return unRevsersed;
        }
    }
}
