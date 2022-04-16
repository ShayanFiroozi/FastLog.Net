using System;
using System.Collections.Generic;
using System.IO;

namespace LogModule
{
    public class Logger
    {

        private List<ILogger> _loggingChannels = new List<ILogger>();


        #region RegistrationMethods
        public void RegisterLoggingChannel(ILogger logger)
        {
            _loggingChannels.Add(logger);
        }

        public void ClearLoggingChannel()
        {
            _loggingChannels.Clear();
        }
        #endregion


        #region LoggingMethod
        public void LogInfo(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.INFO, LogText, ExtraInfo, Source));
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        public void LogWarning(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.WARNING, LogText, ExtraInfo, Source));
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        public void LogError(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.ERROR, LogText, ExtraInfo, Source));
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }



        public void LogException(Exception exception)
        {

            if (exception == null) return;

            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.EXCEPTION,
                                               "Message : " + exception.Message ?? "-",
                                               "InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               "StackTrace : " + (exception.StackTrace ?? "-"),
                                               "Source : " + (exception.Source ?? "-")));
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }

        #endregion



        #region PrivateMethods
        private void _executeLogging(LogMessage LogMessage)
        {
            foreach (ILogger _logger in _loggingChannels)
            {
                try
                {
                    _logger.SaveLog(LogMessage);
                }
                catch(Exception ex)
                {
                    InnerException.InnerException.LogInnerException(ex);
                }
            }
        } 
        #endregion



    }
}
