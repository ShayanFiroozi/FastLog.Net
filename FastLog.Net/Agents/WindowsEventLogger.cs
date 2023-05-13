using FastLog.Net.Enums;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    public class WindowsEventLogger : ILoggerAgent
    {

        public string ApplicationName { get; set; }

        public WindowsEventLogger(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentException($"'{nameof(applicationName)}' cannot be null or whitespace.", nameof(applicationName));
            }

            ApplicationName = applicationName;

        }


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
                        case LogEventTypes.DEBUG:
                        case LogEventTypes.SYSTEM:
                        default:
                            return WriteEventLogEntry(LogModel, EventLogEntryType.Information);

                        case LogEventTypes.WARNING:
                        case LogEventTypes.ALERT:
                            return WriteEventLogEntry(LogModel, EventLogEntryType.Warning);

                        case LogEventTypes.ERROR:
                        case LogEventTypes.EXCEPTION:
                            return WriteEventLogEntry(LogModel, EventLogEntryType.Error);


                    }

                }

            }

            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
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
                InternalExceptionLogger.LogInternalException(ex);
            }


            return Task.CompletedTask;
        }

    }

}

