/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Enums;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Core
{

    public sealed partial class Logger : IDisposable
    {



        public ValueTask LogInfo(string LogText,
                                 string Details = "",
                                 int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.INFO, LogText, Details, EventId);
        }


        public ValueTask LogNote(string LogText,
                             string Details = "",
                             int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.NOTE, LogText, Details, EventId);
        }


        public ValueTask LogTodo(string LogText,
                             string Details = "",
                             int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.TODO, LogText, Details, EventId);
        }



        public ValueTask LogWarning(string LogText,
                                    string Details = "",
                                    int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.WARNING, LogText, Details, EventId);
        }


        public ValueTask LogAlert(string LogText,
                                string Details = "",
                                int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.ALERT, LogText, Details, EventId);
        }


        public ValueTask LogError(string LogText,
                                  string Details = "",
                                  int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.ERROR, LogText, Details, EventId);
        }



        public ValueTask LogDebug(string LogText,
                                  string Details = "",
                                  int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.DEBUG, LogText, Details, EventId);
        }


        public ValueTask LogException(Exception exception, int EventId = 0)
        {

            return LogEventHelper(exception, EventId);



        }



        public ValueTask LogSystem(string LogText,
                                   string Details = "",
                                   int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.SYSTEM, LogText, Details, EventId);
        }


        public ValueTask LogSecurity(string LogText,
                                     string Details = "",
                                     int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.SECURITY, LogText, Details, EventId);
        }


        public ValueTask LogConsole(string LogText,
                                    string Details = "",
                                    int EventId = 0)
        {
            return LogEventHelper(LogEventTypes.CONSOLE, LogText, Details, EventId);
        }




        #region Helpers
        private ValueTask LogEventHelper(LogEventTypes LogType,
                                  string LogText,
                                  string Details = "",
                                  int EventId = 0)
        {

            if (!IsLoggerRunning)
            {
                throw new InvalidOperationException("The logger is not running , please call \"StartLogger\" to start the logger.");

            }

            if (string.IsNullOrWhiteSpace(LogText))
            {

#if NET5_0_OR_GREATER
                return ValueTask.CompletedTask;
#else
                return default;
#endif

            }

            try
            {
                ILogEventModel LogEvent = new LogEventModel(LogType,
                                             LogText,
                                             Details,
                                             EventId);

                // Just for sure !! in fact never gonna happen ! long Max value is "9,223,372,036,854,775,807"

                if (queueTotalEventCount >= long.MaxValue) { queueTotalEventCount = 0; }

                Interlocked.Increment(ref queueTotalEventCount);

                // Raise the event
                try
                {
                    OnEventOccured?.Invoke(this, LogEvent);
                }
                catch (Exception ex)
                {
                    InternalLogger?.LogInternalException(ex);
                }


                // Put the event to the Channel.
                return LoggerChannelWriter.WriteAsync(LogEvent, _cts.Token);
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

#if NET5_0_OR_GREATER
            return ValueTask.CompletedTask;
#else
            return default;
#endif
        }


        private ValueTask LogEventHelper(Exception exception,
                                         int EventId = 0)
        {
            if (!IsLoggerRunning)
            {
                throw new InvalidOperationException("The logger is not running , please call \"StartLogger\" to start the logger.");
            }

            if (exception is null)
            {
#if NET5_0_OR_GREATER
                return ValueTask.CompletedTask;
#else
                return default;
#endif
            }

            try
            {
                ILogEventModel LogEvent = new LogEventModel(exception,
                                                           EventId);

                // Just for sure !! in fact never gonna happen ! long Max value is "9,223,372,036,854,775,807"

                if (queueTotalEventCount >= long.MaxValue) { queueTotalEventCount = 0; }

                Interlocked.Increment(ref queueTotalEventCount);

                // Raise the event
                try
                {
                    OnEventOccured?.Invoke(this, LogEvent);
                }
                catch (Exception ex)
                {
                    InternalLogger?.LogInternalException(ex);
                }


                // Put the event to the Channel.
                return LoggerChannelWriter.WriteAsync(LogEvent, _cts.Token);
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

#if NET5_0_OR_GREATER
            return ValueTask.CompletedTask;
#else
            return default;
#endif
        }
        #endregion


    }
}
