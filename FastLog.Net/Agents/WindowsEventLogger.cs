using FastLog.Net.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    public class WindowsEventLogger : ILoggerAgent
    {


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            // Note : will be available on the next release.
            return Task.FromException(new NotImplementedException());
        }


    }

}

