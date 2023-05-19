using FastLog.Enums;
using FastLog.Net.Helpers.ExtendedMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace FastLog.Net.Agents.DebugAndTraceAgents
{

    // Note : DebugWindowLogger class uses fluent "Builder" pattern.
    // Note : DebugWindowLogger is only available in "Debug" mode.



    public class DebugSystemAgent : ILoggerAgent
    {

        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private readonly InternalLogger InternalLogger = null;
        private bool useJsonFormat { get; set; } = false;


        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private DebugSystemAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }


        public static DebugSystemAgent Create(InternalLogger internalLogger = null) => new DebugSystemAgent(internalLogger);


        public DebugSystemAgent UseJsonFormat()
        {
            useJsonFormat = true;
            return this;
        }

        public DebugSystemAgent IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public DebugSystemAgent ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public DebugSystemAgent IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public DebugSystemAgent ExcludeAllEventTypes()
        {
            _registeredEvents.Clear();

            return this;
        }

        #endregion


        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
#if !DEBUG
#warning "DebugWindowLogger.LogEvent" only works on the "Debug" mode and has no effect in the "Relase" mode !
            return Task.CompletedTask;
#else

            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            try
            {
                // Check if current log "Event Type" should be execute or not.
                if (!_registeredEvents.Any()) return Task.CompletedTask;
                if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


                Debug.WriteLine(useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;
#endif

        }


    }



}

