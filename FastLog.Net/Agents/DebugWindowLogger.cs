using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : DebugWindowLogger class uses fluent "Builder" pattern.
    // Note : DebugWindowLogger only available in "Debug" mode.

#if DEBUG

    public class DebugWindowLogger : ILoggerAgent
    {

        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private readonly InternalExceptionLogger InternalLogger = null;

        public IEnumerable<LogEventTypes> RegisteredEvents => _registeredEvents;




        private DebugWindowLogger(InternalExceptionLogger internalLogger = null)
        {
            //Keep it private to make it non accessible from the outside of the class !!

            InternalLogger = internalLogger;
            RegisterAllEventsToDebugWindow();
        }


        public static DebugWindowLogger Create(InternalExceptionLogger internalLogger = null) => new DebugWindowLogger(internalLogger);



        public DebugWindowLogger RegisterEventToDebugWindow(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public DebugWindowLogger UnRegisterEventFromDebugWindow(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public DebugWindowLogger RegisterAllEventsToDebugWindow()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public DebugWindowLogger UnRegisterAllEventsFromDebugWindow()
        {
            _registeredEvents.Clear();

            return this;
        }



        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            try
            {
                // Check if any "Event Type" exists to show on Debug Window ?
                if (!_registeredEvents.Any()) return Task.CompletedTask;


                // Check if current log "Event Type" should be reflected onthe Debug Window or not.
                if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


                Debug.WriteLine(LogModel.GetLogMessage(false));
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }

#endif

}

