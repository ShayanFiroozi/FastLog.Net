
using FastLog.Net.Enums;
using System;
using System.Threading.Tasks;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {


        #region Logging Functions


        private ValueTask LogEventHelper(LogEventTypes LogType,
                                         string LogText,
                                         string ExtraInfo = "",
                                         string Source = "")
        {

            if (!_IsLoggerRunning)
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
                                             ExtraInfo,
                                             Source,
                                             saveMachineName,
                                             applicationName);

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


        private ValueTask LogEventHelper(Exception exception)
        {
            if (!_IsLoggerRunning)
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
                                                           saveMachineName,
                                                           this.applicationName);

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




        public ValueTask LogInfo(string LogText,
                                 string ExtraInfo = "",
                                 string Source = "")
        {
            return LogEventHelper(LogEventTypes.INFO, LogText, ExtraInfo, Source);
        }


        public ValueTask LogNote(string LogText,
                             string ExtraInfo = "",
                             string Source = "")
        {
            return LogEventHelper(LogEventTypes.NOTE, LogText, ExtraInfo, Source);
        }


        public ValueTask LogTodo(string LogText,
                             string ExtraInfo = "",
                             string Source = "")
        {
            return LogEventHelper(LogEventTypes.TODO, LogText, ExtraInfo, Source);
        }



        public ValueTask LogWarning(string LogText,
                                    string ExtraInfo = "",
                                    string Source = "")
        {
            return LogEventHelper(LogEventTypes.WARNING, LogText, ExtraInfo, Source);
        }


        public ValueTask LogAlert(string LogText,
                                string ExtraInfo = "",
                                string Source = "")
        {
            return LogEventHelper(LogEventTypes.ALERT, LogText, ExtraInfo, Source);
        }


        public ValueTask LogError(string LogText,
                                  string ExtraInfo = "",
                                  string Source = "")
        {
            return LogEventHelper(LogEventTypes.ERROR, LogText, ExtraInfo, Source);
        }



        public ValueTask LogDebug(string LogText,
                                  string ExtraInfo = "",
                                  string Source = "")
        {
            return LogEventHelper(LogEventTypes.DEBUG, LogText, ExtraInfo, Source);
        }


        public ValueTask LogException(Exception exception)
        {

            return LogEventHelper(exception);

        }



        public ValueTask LogSystem(string LogText,
                                   string ExtraInfo = "",
                                   string Source = "")
        {
            return LogEventHelper(LogEventTypes.SYSTEM, LogText, ExtraInfo, Source);
        }


        public ValueTask LogSecurity(string LogText,
                                     string ExtraInfo = "",
                                     string Source = "")
        {
            return LogEventHelper(LogEventTypes.SECURITY, LogText, ExtraInfo, Source);
        }



        #endregion



    }
}
