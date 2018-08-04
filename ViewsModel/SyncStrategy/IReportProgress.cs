using Jsa.ViewsModel.Helpers;
using System;

namespace Jsa.ViewsModel.SyncStrategy
{
    public interface IReportProgress
    {
        event EventHandler<ProgressEventArgs> ReportProgress;
        void RaiseProgress(double progress);
    }
}
