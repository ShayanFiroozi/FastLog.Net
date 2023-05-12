using FastLog.Net.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{


#if DEBUG

    public class HeavyOperationSimulator : ILoggerAgent
    {

        public TimeSpan OperationTimeSpan { get; set; }

        public HeavyOperationSimulator(TimeSpan operationTimeSpan)
        {
            OperationTimeSpan = operationTimeSpan;
        }


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            // Simulate some heavy CPU or IO operation
            return Task.Delay(OperationTimeSpan, cancellationToken);

        }



    }

#endif

}

