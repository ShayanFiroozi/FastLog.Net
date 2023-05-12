using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : DebugWindowLogger class uses fluent "Builder" pattern.

    public class DebugWindowLogger : ILoggerAgent
    {

        private readonly List<LogEventTypes> _logEventTypesToReflect = new List<LogEventTypes>();

        public IEnumerable<LogEventTypes> LogEventTypesToReflect => _logEventTypesToReflect;




        private DebugWindowLogger()
        {
            //Keep it private to make it non accessible from the outside of the class !!


            ReflectAllEventTypeToDebugWindow();
        }

        public static DebugWindowLogger Create()

        {

            return new DebugWindowLogger();
        }



        public DebugWindowLogger ReflectEventTypeToDebugWindow(LogEventTypes logEventType)
        {
            if (!_logEventTypesToReflect.Any(type => type == logEventType))
            {
                _logEventTypesToReflect.Add(logEventType);
            }

            return this;
        }

        public DebugWindowLogger DoNotReflectEventTypeToDebugWindow(LogEventTypes logEventType)
        {
            if (_logEventTypesToReflect.Any(type => type == logEventType))
            {
                _logEventTypesToReflect.Remove(logEventType);
            }

            return this;
        }

        public DebugWindowLogger ReflectAllEventTypeToDebugWindow()
        {
            _logEventTypesToReflect.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _logEventTypesToReflect.Add(eventType);
            }

            return this;
        }

        public DebugWindowLogger DoNotReflectAnyEventTypeToDebugWindow()
        {
            _logEventTypesToReflect.Clear();

            return this;
        }

       

        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            // Check if any "Event Type" exists to show on Debug Window ?
            if (!_logEventTypesToReflect.Any()) return Task.CompletedTask;


            // Check if current log "Event Type" should be reflected onthe Debug Window or not.
            if (!_logEventTypesToReflect.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


            Debug.WriteLine(LogModel.GetLogMessage(false));


            return Task.CompletedTask;


        }


    }

}

