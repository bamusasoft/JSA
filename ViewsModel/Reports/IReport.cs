using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jsa.ViewsModel.Reports
{
    public interface IReport<T>:IDisposable
    {
        void Print();
        

    }
}
