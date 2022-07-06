using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWIN.Self.Hosted
{
    public interface ILogService
    {
        void Debug(string msg, params object[] args);
    }
}
