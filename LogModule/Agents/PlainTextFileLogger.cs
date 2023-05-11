using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{
    public class PlainTextFileLogger : ILoggerAgent
    {

        #region Properties

        public string LogFile { get; private set; } = string.Empty;
        public short MaxLogFileSizeMB { get; private set; } = 0;

        #endregion



        public PlainTextFileLogger(string logFile, short maxLogFileSizeMB = 100)
        {
            if (string.IsNullOrWhiteSpace(logFile))
            {
                throw new ArgumentException($"'{nameof(logFile)}' cannot be null or whitespace.", nameof(logFile));
            }

            if (maxLogFileSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(maxLogFileSizeMB)}' must be greater then zero.", nameof(maxLogFileSizeMB));
            }

            LogFile = logFile;
            MaxLogFileSizeMB = maxLogFileSizeMB;
        }



        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {

            if (LogModel is null) return Task.CompletedTask;

            try
            {

                CheckLogFileSize();

                return File.AppendAllTextAsync(LogFile, LogModel.ToString(), cancellationToken);

            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return Task.CompletedTask;
        }


        private short GetLogFileSizeMB()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LogFile)) return 0;
                if (!File.Exists(LogFile)) return 0;

                return (short)(new FileInfo(LogFile).Length / 1024 / 1024);
            }
            catch
            {
                return 0;
            }
        }


        private void CheckLogFileSize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LogFile)) return;
                if (!File.Exists(LogFile)) return;


                if (GetLogFileSizeMB() >= MaxLogFileSizeMB)
                {
                    File.Delete(LogFile);
                }
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

        }





    }

}

