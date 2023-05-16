using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{


    // Note : HeavyOperationSimulator class uses fluent "Builder" pattern.
    // Note : HeavyOperationSimulator is only available in "Debug" mode.



    /// <summary>
    /// This class will be used when debugging (testing) the FastLog.Net to simulate a heavy CPU or IO bound operation.
    /// </summary>
    public class HeavyOperationSimulatorAgent : ILoggerAgent
    {



        public TimeSpan OperationTimeSpan { get; set; }


        //Keep it private to make it non accessible from the outside of the class !!
        private HeavyOperationSimulatorAgent(TimeSpan operationTimeSpan) { OperationTimeSpan = operationTimeSpan; }

        public static HeavyOperationSimulatorAgent Create(TimeSpan operationTimeSpan) => new HeavyOperationSimulatorAgent(operationTimeSpan);



        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
#if !DEBUG
#warning "HeavyOperationSimulator.LogEvent" only works on the "Debug" mode and has no effect in the "Relase" mode !
            return Task.CompletedTask;

#else
            // Simulate some heavy CPU or IO bound operation.
            return Task.Delay(OperationTimeSpan, cancellationToken);

#endif

        }



    }



}

