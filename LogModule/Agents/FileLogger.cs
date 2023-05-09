using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{
    public class FileLogger : ILoggerAgent
    {

        #region Channel Definitions

        private Channel<LogMessageModel> FileLoggerChannel = Channel.CreateUnbounded<LogMessageModel>(new UnboundedChannelOptions());

        private ChannelReader<LogMessageModel> FileLoggerChannelReader;
        private ChannelWriter<LogMessageModel> FileLoggerChannelWriter;

        #endregion


        #region Properties

        public string LogFile { get; private set; } = string.Empty;
        public short MaxLogFileSizeMB { get; private set; } = 0;

        #endregion


        public FileLogger()
        {
            ChannelReader<LogMessageModel> FileLoggerChannelReader = FileLoggerChannel.Reader;
            ChannelWriter<LogMessageModel> FileLoggerChannelWriter = FileLoggerChannel.Writer;
        }

        public void SetLogFile(string LogFile)
        {
            if (string.IsNullOrWhiteSpace(LogFile))
            {
                throw new ArgumentException($"'{nameof(LogFile)}' cannot be null or whitespace.", nameof(LogFile));
            }


            this.LogFile = LogFile;


            if (!Directory.Exists(Path.GetDirectoryName(LogFile)))
            {
                Directory.CreateDirectory(LogFile);
            }


        }

        public void SetLogFileMaxSizeMB(short MaxLogFileSizeMB)
        {

            if (MaxLogFileSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(MaxLogFileSizeMB)}' must be greater then zero.", nameof(MaxLogFileSizeMB));
            }

            this.MaxLogFileSizeMB = MaxLogFileSizeMB;


        }

        public Task StartLogger()
        {
            if (string.IsNullOrWhiteSpace(LogFile))
            {
                throw new ArgumentException($"'{nameof(LogFile)}' cannot be null or whitespace.", nameof(LogFile));
            }


            if (MaxLogFileSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(MaxLogFileSizeMB)}' must be greater then zero.", nameof(MaxLogFileSizeMB));
            }

            return StartFileLoggerEngine();
        }


        public ValueTask SaveLog(LogMessageModel LogModel)
        {
            if ((LogModel is not null))
            {
                return FileLoggerChannelWriter.WriteAsync(LogModel);
            }
            else
            {
                return ValueTask.CompletedTask;
            }
        }




        #region Private Functions


        private Task AppendLogModelToFile(LogMessageModel LogModel)
        {
            if (string.IsNullOrWhiteSpace(LogFile))
            {
                throw new ArgumentException($"'{nameof(LogFile)}' cannot be null or whitespace.", nameof(LogFile));
            }

            if (MaxLogFileSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(MaxLogFileSizeMB)}' must be greater then zero.", nameof(MaxLogFileSizeMB));
            }


            if (LogModel is null) return Task.CompletedTask;


            try
            {


                return File.AppendAllTextAsync(LogFile, LogModel.GetLogMessage().ToString());

            }
            catch
            {
                return Task.CompletedTask;
            }

        }


        private Task StartFileLoggerEngine()
        {
            return Task.Run(async () =>
            {

                while (!FileLoggerChannelReader.Completion.IsCompleted)
                {
                    LogMessageModel? LogModelFromChannel = await FileLoggerChannelReader.ReadAsync();

                    if (LogModelFromChannel is not null)
                    {
                      
                        // Delete the log file if it's getting big !
                        await DeleteInternalExceptionsLogFile();

                        // Save to the log file.
                        await AppendLogModelToFile(LogModelFromChannel);

                    }

                }

            });
        }



        private Task<short> GetLogFileSizeMB()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(LogFile)) return Task.FromResult<short>(0);
                if (!File.Exists(LogFile)) return Task.FromResult<short>(0);

                return Task.FromResult<short>((short)((new FileInfo(LogFile).Length / 1024) / 1024));
            }
            catch
            {
                return Task.FromResult<short>(0);
            }
        }

        private async Task DeleteInternalExceptionsLogFile()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(LogFile)) return;
                if (!File.Exists(LogFile)) return;


                if (await GetLogFileSizeMB() <= MaxLogFileSizeMB)
                {
                    return;
                }
                else
                {
                    File.Delete(LogFile);
                }
            }
            catch
            {

            }

        }

        #endregion



    }

}

