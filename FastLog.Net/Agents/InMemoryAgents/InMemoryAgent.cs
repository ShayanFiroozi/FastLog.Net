using FastLog.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace FastLog.Net.Agents.ConsoleAgents
{


    public class InMemoryAgent : AgentBase<InMemoryAgent>, IAgent
    {

        private int maxEventsToKeep { get; set; }
        public List<LogEventModel> inMemoryEvents { get; set; } = new List<LogEventModel>();

        public IEnumerable<LogEventModel> GetInMemoryEvents() => inMemoryEvents;


        //Keep it private to make it non accessible from the outside of the class !!
        private InMemoryAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }

        public static InMemoryAgent Create(InternalLogger internalLogger = null) => new InMemoryAgent(internalLogger);

        public InMemoryAgent WithMaxEventsToKeep(int maxEventsToKeep)
        {
            this.maxEventsToKeep = maxEventsToKeep;
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

                if(inMemoryEvents.Count > maxEventsToKeep)
                {
                    inMemoryEvents.RemoveAt(0); // Remove the oldest item.
                 
                }

                inMemoryEvents.Add(LogModel);


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;

        }

    }

}



