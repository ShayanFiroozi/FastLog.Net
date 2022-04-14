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
        public void LogInfo(string Source,
                         string LogText,
                         string ExtraInfo = "")
        {
            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.INFO, Source, LogText, ExtraInfo));
        }


        public void LogWarning(string Source,
                           string LogText,
                           string ExtraInfo = "")
        {
            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.WARNING, Source, LogText, ExtraInfo));
        }


        public void LogError(string Source,
                           string LogText,
                           string ExtraInfo="")
        {
            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.ERROR, Source, LogText, ExtraInfo));
        }



        public void LogError(Exception exception)
        {

            if (exception == null) return;

            _executeLogging(new LogMessage(LogMessage.LogTypeEnum.ERROR,
                                           "Source : " + (exception.Source ?? "-"),
                                           "Message : " + exception.Message ?? "-",
                                           "InnerMessage : " + (exception.InnerException?.Message ?? "-") + 
                                           " , " + 
                                           "StackTrace : " + (exception.StackTrace ?? "-")));

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
