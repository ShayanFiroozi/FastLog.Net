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
        DEBUG = 1,
        WARNING = 2,
        ALERT = 3,
        ERROR = 4,
        EXCEPTION = 5,
    }



}
