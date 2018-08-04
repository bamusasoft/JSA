using System;
using System.Threading.Tasks;

namespace Jsa.ViewsModel.SyncStrategy
{
    public class SyncIrsContext
    {
        ISyncIrs _syncIrs;
        public SyncIrsContext(ISyncIrs syncIrs)
        {
            if (syncIrs == null) throw new ArgumentNullException("syncIrs");
            _syncIrs = syncIrs;
        }
        public Task SyncAsync()
        {
            return _syncIrs.SyncAsync();
            
        }

    }
}
