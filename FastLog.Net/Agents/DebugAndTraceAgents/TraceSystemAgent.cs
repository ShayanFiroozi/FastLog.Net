using FastLog.Helpers.ExtendedMethods;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace FastLog.Agents.DebugAndTraceAgents
{


    public class TraceSystemAgent : AgentBase<TraceSystemAgent>, IAgent
    {

        private bool useJsonFormat { get; set; } = false;



        //Keep it private to make it non accessible from the outside of the class !!
        private TraceSystemAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }


        public static TraceSystemAgent Create(InternalLogger internalLogger = null) => new TraceSystemAgent(internalLogger);


        public TraceSystemAgent UseJsonFormat()
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


                Trace.WriteLine(useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }



}

