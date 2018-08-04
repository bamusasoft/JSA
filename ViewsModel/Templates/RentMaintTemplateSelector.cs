using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Jsa.ViewsModel.Templates
{
    public class RentMaintTemplateSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is bool)
            {
                Window win =
                    Application.Current.MainWindow.OwnedWindows.OfType<Window>().SingleOrDefault(x => x.Name == "RentMaintWindow");
                if (win == null) return null;
                bool showMaint = (bool) item;
                if (showMaint)
                {
                    return win.FindResource("MaintTemplate") as DataTemplate;

                }
                return win.FindResource("RentTemplate") as DataTemplate;
            }


            return null;
        }
    }
}
