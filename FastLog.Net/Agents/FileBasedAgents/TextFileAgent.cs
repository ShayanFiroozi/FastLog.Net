using FastLog.Enums;
using FastLog.Helpers;
using FastLog.Helpers.ExtendedMethods;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace FastLog.Agents.FileBaseAgents
{


    public class TextFileAgent : AgentBase<TextFileAgent>, IAgent
    {



        private bool useJsonFormat { get; set; } = false;

        #region Private Properties

        private string LogFile { get; set; } = string.Empty;
        private short MaxLogFileSizeMB { get; set; } = 0;

        #endregion



        //Keep it private to make it non accessible from the outside of the class !!
        private TextFileAgent(InternalLogger internalLogger = null)
        {
            IncludeAllEventTypes();
            InternalLogger = internalLogger;
        }

        public static TextFileAgent Create(InternalLogger internalLogger = null) => new TextFileAgent(internalLogger);
        public TextFileAgent UseJsonFormat()
        {
            useJsonFormat = true;
            return this;
        }

        public TextFileAgent SaveLogToFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException($"'{nameof(filename)}' cannot be null or whitespace.", nameof(filename));
            }


            LogFile = filename;

            try
            {

                if (!Directory.Exists(Path.GetDirectoryName(LogFile)))
                {
                    _ = Directory.CreateDirectory(Path.GetDirectoryName(LogFile));
                }

            }

            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return this;


        }


        public TextFileAgent DeleteTheLogFileWhenExceededTheMaximumSizeOf(short logFileMaxSizeMB)
        {

            if (logFileMaxSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(logFileMaxSizeMB)}' must be greater then zero.", nameof(logFileMaxSizeMB));
            }

            MaxLogFileSizeMB = logFileMaxSizeMB;

            return this;

        }





        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            if (!CanExecuteOnThidMode()) return Task.CompletedTask;


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            try
            {



                if (!Directory.Exists(Path.GetDirectoryName(LogFile)))
                {
                    return Task.CompletedTask;
                }



                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;


                CheckAndDeleteLogFileSize();

                //Note :  In code below we will lose exceptions from "ThreadSafeFileHelper.AppendAllText" due to use a "Fire and forget" approach here.
                // Please note that it is not recomended to use "ReaderWriterLockSlim" in async/await methods.
                //For more info please visit -> https://stackoverflow.com/questions/19659387/readerwriterlockslim-and-async-await


                // #Refactor Required. ( Goal : use an approach to be able to catch exceptions properly and not using "Fire and Forget" style )
                return Task.Run(() => ThreadSafeFileHelper.AppendAllText(LogFile, useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText()), cancellationToken);


                // Note : The approach below (when using File.AppendAllTextAsync) is not thread-safe and has some issues ,
                // - specially when two or more loggers are logging to the same file.

                //#if NETFRAMEWORK || NETSTANDARD2_0

                //                await Task.Run(() => ThreadSafeFileHelper.AppendAllText(LogFile, LogModel.GetLogMessage(true)), cancellationToken);
                //#else

                //                await File.AppendAllTextAsync(LogFile, LogModel.GetLogMessage(true), cancellationToken);

                //#endif



            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;
        }


        private void CheckAndDeleteLogFileSize()
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

                if (ThreadSafeFileHelper.GetFileSize(LogFile) >= MaxLogFileSizeMB)
                {
                    // May be not "Thread-Safe"
                    //File.Delete(LogFile);

                    ThreadSafeFileHelper.DeleteFile(LogFile);

                    // Save the detele operation log with Internal Logger agen (if it was not null)
                    InternalLogger?.LogInternalSystemEvent(new LogEventModel(LogEventTypes.SYSTEM,
                                   $"The log file \"{LogFile}\" exceeded the maximum permitted size of \"{MaxLogFileSizeMB:N0} MB\"."));

                    InternalLogger?.LogInternalSystemEvent(new LogEventModel(LogEventTypes.SYSTEM,
                                   $"The log file \"{LogFile}\" has been deleted."));



                }
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

        }



    }

}

