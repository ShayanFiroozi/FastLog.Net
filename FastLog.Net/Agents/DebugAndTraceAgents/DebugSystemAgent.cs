using FastLog.Core;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.DebugAndTraceAgents
{


    public sealed class DebugSystemAgent : AgentBase<DebugSystemAgent>, IAgent
    {


        private bool useJsonFormat { get; set; } = false;


        //Keep it private to make it non accessible from the outside of the class !!
        private DebugSystemAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }

        public static DebugSystemAgent Create(AgentsManager manager) => new DebugSystemAgent(manager);


        public DebugSystemAgent UseJsonFormat()
        {
            useJsonFormat = true;
            return this;
        }


        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            if (!CanExecuteOnThidMode()) return Task.CompletedTask;


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            try
            {

                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;

                Debug.WriteLine(useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }



}

