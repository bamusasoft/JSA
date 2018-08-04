using System.Threading.Tasks;

namespace Jsa.ViewsModel.SyncStrategy
{
    public interface ISyncIrs:IReportProgress
    {
        Task SyncAsync();
        
    }
}
