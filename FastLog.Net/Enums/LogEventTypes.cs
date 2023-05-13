using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastLog.Net.Enums
{

    public enum LogEventTypes : byte
    {
        INFO = 0,
        WARNING = 1,
        ALERT = 2,
        DEBUG = 3,
        ERROR = 4,
        EXCEPTION = 5,
        SYSTEM = 6,
        SECURITY = 7
    }



}
