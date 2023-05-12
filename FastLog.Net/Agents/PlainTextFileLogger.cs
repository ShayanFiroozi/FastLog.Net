using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
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

            if (!Directory.Exists(Path.GetDirectoryName(LogFile)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(LogFile));
            }
        }



        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {

            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            try
            {

                CheckLogFileSize();


#if NETFRAMEWORK || NETSTANDARD2_0
                return Task.Run(() => File.AppendAllText(LogFile, LogModel.GetLogMessage(true)), cancellationToken);
#else
                return File.AppendAllTextAsync(LogFile, LogModel.GetLogMessage(true), cancellationToken);
#endif



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
                return string.IsNullOrWhiteSpace(LogFile)
                    ? (short)0
                    : !File.Exists(LogFile) ? (short)0 : (short)(new FileInfo(LogFile).Length / 1024 / 1024);
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
                if (string.IsNullOrWhiteSpace(LogFile))
                {
                    return;
                }

                if (!File.Exists(LogFile))
                {
                    return;
                }

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

