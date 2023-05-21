using System;
using System.Threading;
using System.Threading.Tasks;
using FastLog.Core;
using FastLog.Interfaces;
using FastLog.Models;

namespace FastLog.Agents.AdvancedAgents
{


    public sealed class HeavyOperationSimulatorAgent : AgentBase<HeavyOperationSimulatorAgent>, IAgent
    {

        public TimeSpan OperationTimeSpan { get; set; }


        //Keep it private to make it non accessible from the outside of the class !!
        private HeavyOperationSimulatorAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }

        public static HeavyOperationSimulatorAgent Create(AgentsManager manager) => new HeavyOperationSimulatorAgent(manager);


       
        public HeavyOperationSimulatorAgent WithDelay(TimeSpan timeSpan)
        {
            OperationTimeSpan = timeSpan;

            return this;
        }

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

