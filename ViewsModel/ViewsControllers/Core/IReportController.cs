using System.Collections.Generic;
using System.Windows.Input;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public interface IReportController : IController
    {
        ICommand SearchCommand { get; }
        ICommand PrintCommand { get; }
        ICommand EditCommand { get; }
        ICommand RefreshCommand { get; }

        IList<object> SearchResult { get; set; }
        object SelectedItem { get; set; }
    }
}