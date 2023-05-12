using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{


#if DEBUG

    /// <summary>
    /// This class will be used when debugging (testing) the FastLog.Net to simulate a heavy CPU or IO bound operation.
    /// </summary>
    public class HeavyOperationSimulator : ILoggerAgent
    {

        public TimeSpan OperationTimeSpan { get; set; }

        public HeavyOperationSimulator(TimeSpan operationTimeSpan)
        {
            OperationTimeSpan = operationTimeSpan;
        }


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            // Simulate some heavy CPU or IO bound operation.
            return Task.Delay(OperationTimeSpan, cancellationToken);

        }



    }

#endif

}

