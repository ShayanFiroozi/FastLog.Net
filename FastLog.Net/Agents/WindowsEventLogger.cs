using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : BeepAgent class uses fluent "Builder" pattern.

    public class WindowsEventLogger : ILoggerAgent
    {
        private InternalLogger InternalLogger = null;
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();

        public string ApplicationName { get; set; }



        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private WindowsEventLogger() => IncludeAllEventTypes();

        public static WindowsEventLogger Create() => new WindowsEventLogger();

        public WindowsEventLogger WithInternalLogger(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger ?? throw new ArgumentNullException(nameof(internalLogger));

            return this;
        }

        public WindowsEventLogger WithApplicationName(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentException($"'{nameof(applicationName)}' cannot be null or whitespace.", nameof(applicationName));
            }

            ApplicationName = applicationName;

            return this;
        }

        public WindowsEventLogger IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public WindowsEventLogger ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public WindowsEventLogger IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public WindowsEventLogger ExcludeAllEventTypes()
        {
            _registeredEvents.Clear();

            return this;
        }

        #endregion




        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            try
            {

                using (EventLog eventLog = new EventLog())
                {
                    eventLog.Source = ApplicationName;


                    switch (LogModel.LogEventType)
                    {
                        case LogEventTypes.INFO:
                        case LogEventTypes.NOTE:
                        case LogEventTypes.TODO:
                        case LogEventTypes.DEBUG:
                        case LogEventTypes.SYSTEM:
                        default:
                            return WriteEventLogEntry(LogModel, EventLogEntryType.Information);

                        case LogEventTypes.WARNING:
                        case LogEventTypes.ALERT:
                        case LogEventTypes.SECURITY:
                            return WriteEventLogEntry(LogModel, EventLogEntryType.Warning);

                        case LogEventTypes.ERROR:
                        case LogEventTypes.EXCEPTION:
                            return WriteEventLogEntry(LogModel, EventLogEntryType.Error);


                    }

                }

            }

            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;
        }


        private Task WriteEventLogEntry(LogEventModel LogModel, EventLogEntryType entryType)
        {
            try
            {

                // Create an instance of EventLog
                using (EventLog eventLog = new EventLog())
                {

                    // Check if the event source exists. If not create it.
                    if (!EventLog.SourceExists(ApplicationName))
                    {
                        EventLog.CreateEventSource(LogModel.LogEventType.ToString(), ApplicationName);
                    }

                    // Set the source name for writing log entries.
                    eventLog.Source = LogModel.LogEventType.ToString();


                    // Write an entry to the event log.
                    eventLog.WriteEntry(LogModel.GetLogMessage(false),
                                        entryType,
                                        0);

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

