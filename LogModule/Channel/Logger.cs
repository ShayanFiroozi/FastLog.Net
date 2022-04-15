using System;
using System.Collections.Generic;

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
            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.INFO, LogText, ExtraInfo, Source));
        }


        public void LogWarning(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.WARNING, LogText, ExtraInfo, Source));
        }


        public void LogError(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.ERROR, LogText, ExtraInfo, Source));
        }



        public void LogException(Exception exception)
        {

            if (exception == null) return;

            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.EXCEPTION,
                                           "Message : " + exception.Message ?? "-",
                                           "InnerMessage : " + (exception.InnerException?.Message ?? "-") + 
                                           " , " + 
                                           "StackTrace : " + (exception.StackTrace ?? "-"),
                                           "Source : " + (exception.Source ?? "-")));

        }

        #endregion



        #region PrivateMethods
        private void _executeLogging(LogMessage LogMessage)
        {
            foreach (ILogger _logger in _loggingChannels)
            {
                _logger.SaveLog(LogMessage);
            }
        } 
        #endregion



    }
}
