using System;

namespace Jsa.WinIrsService
{
    public class WinIrsServiceException:Exception
    {
        public WinIrsServiceException(string msg, Exception innerException)
            : base(msg, innerException) { }
    }
}
