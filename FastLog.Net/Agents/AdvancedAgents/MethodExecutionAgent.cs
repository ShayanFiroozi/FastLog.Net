using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace FastLog.Agents.AdvancedAgents
{


    public class MethodExecutionAgent : AgentBase<MethodExecutionAgent>, IAgent
    {

        private Action methodToExecute { get; set; }


        //Keep it private to make it non accessible from the outside of the class !!
        private MethodExecutionAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }

        public static MethodExecutionAgent Create(InternalLogger internalLogger = null) => new MethodExecutionAgent(internalLogger);

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



