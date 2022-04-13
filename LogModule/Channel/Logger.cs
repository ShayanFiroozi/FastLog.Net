using System;
using System.Collections.Generic;

namespace LogModule
{
    public class Logger : IDisposable
    {

        private List<ILogger> _loggingChannels = new List<ILogger>();


        public void RegisterLoggingChannel(ILogger logger)
        {
            _loggingChannels.Add(logger);
        }



        public void ClearLoggingChannel()
        {
            _loggingChannels.Clear();
        }


        public void ExecuteLogging(LogMessage LogMessage)
        {
            foreach (ILogger _logger in _loggingChannels)
            {
                _logger.SaveLog(LogMessage);
            }
        }

        public void Dispose()
        {
            _loggingChannels.Clear();
            _loggingChannels = null;
        }

        ~Logger()
        {
            Dispose();
        }
    }
}
