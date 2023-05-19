using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace FastLog.Net.Agents.AdvancedAgents
{


    public class HeavyOperationSimulatorAgent : AgentBase<HeavyOperationSimulatorAgent>, IAgent
    {

        public TimeSpan OperationTimeSpan { get; set; }


        //Keep it private to make it non accessible from the outside of the class !!
        private HeavyOperationSimulatorAgent(TimeSpan operationTimeSpan) { OperationTimeSpan = operationTimeSpan; }

        public static HeavyOperationSimulatorAgent Create(TimeSpan operationTimeSpan) => new HeavyOperationSimulatorAgent(operationTimeSpan);



        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {

            if (!CanExecuteOnThidMode()) return Task.CompletedTask;


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;



            // Simulate some heavy CPU or IO bound operation.
              return Task.Delay(OperationTimeSpan, cancellationToken);



        }



    }



}

