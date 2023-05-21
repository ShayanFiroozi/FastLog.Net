using FastLog.Core;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.AdvancedAgents
{


    public sealed class MethodExecutionAgent : AgentBase<MethodExecutionAgent>, IAgent
    {

        private Action methodToExecute { get; set; }


        //Keep it private to make it non accessible from the outside of the class !!
        private MethodExecutionAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }

        public static MethodExecutionAgent Create(AgentsManager manager) => new MethodExecutionAgent(manager);

        public MethodExecutionAgent MethodToExecute(Action method)
        {
            methodToExecute = method;
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



            try
            {

                if (methodToExecute != null)
                {
                    return Task.Run(methodToExecute, cancellationToken);
                }


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;

        }


    }

}



