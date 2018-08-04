using System;

namespace Jsa.ViewsModel.Helpers
{
    public class ProgressEventArgs : EventArgs
    {
        public double Progress { get; private set; }
        public ProgressEventArgs(double progress)
        {
            Progress = progress;
        }

    }
    public class ProgressEventArgs<T> : ProgressEventArgs
        where T : class
    {
        public T Entity { get; private set; }
        public ProgressEventArgs(double progress)
            : this(progress, null)
        {

        }
        public ProgressEventArgs(double progress, T entity)
            :base(progress)
        {
            this.Entity = entity;
        }
        
    }
}
