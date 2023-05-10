using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{
    public class FileLogger : ILoggerAgent
    {

        #region Properties

        public string LogFile { get; private set; } = string.Empty;
        public short MaxLogFileSizeMB { get; private set; } = 0;

        #endregion



        public FileLogger(string logFile, short maxLogFileSizeMB)
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



        public Task SaveLog(LogMessageModel LogModel)
        {

            if (LogModel is null) return Task.CompletedTask;

            try
            {

                CheckLogFileSize();

                return File.AppendAllTextAsync(LogFile, LogModel.GetLogMessage().ToString());

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
                if (string.IsNullOrWhiteSpace(LogFile)) return (short)0;
                if (!File.Exists(LogFile)) return (short)0;

                return (short)((new FileInfo(LogFile).Length / 1024) / 1024);
            }
            catch
            {
                return (short)0;
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
            catch
            {

            }

        }





    }

}

