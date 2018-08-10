using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jsa.DomainModel.Exceptions
{
    public class DataModelException:Exception
    {
        public DataModelException(string msg, Exception innerException)
            : base(msg, innerException) { }
    }
    
}
