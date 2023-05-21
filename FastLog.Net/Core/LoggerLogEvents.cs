using FastLog.Enums;
using System;
using System.Threading.Tasks;
using FastLog.Models;

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




        #region Helpers
        private ValueTask LogEventHelper(LogEventTypes LogType,
                                  string LogText,
                                  string Details = "",
                                  int EventId = 0)
        {

            if (!IsLoggerRunning)
            {
                throw new Exception("The logger is not running , please call \"StartLogger\" to start the logger.");

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
                LogEventModel LogEvent = new LogEventModel(LogType,
                                             LogText,
                                             Details,
                                             EventId);

                return LoggerChannelWriter.WriteAsync(LogEvent);
            }
            catch (Exception ex)
            {
                this.InternalLogger?.LogInternalException(ex);
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
                throw new Exception("The logger is not running , please call \"StartLogger\" to start the logger.");
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
                LogEventModel LogEvent = new LogEventModel(exception,
                                                           EventId);

                return LoggerChannelWriter.WriteAsync(LogEvent);
            }
            catch (Exception ex)
            {
                this.InternalLogger?.LogInternalException(ex);
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
