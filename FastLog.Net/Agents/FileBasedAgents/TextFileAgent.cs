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


        #region Private Properties

        private bool useJsonFormat { get; set; } = false;
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


                // This is necessary to prevent repeatedly internal exception if the destination path ("Directory" or "Drive") ...
                // are not exist or ready.
                if (!Directory.Exists(Path.GetDirectoryName(LogFile)))
                {
                    return Task.CompletedTask;
                }



                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;


                // If the log file exceeded the maximum size , we delete it !!
                CheckAndDeleteLogFileSize();


                // Create the new log file and add file header.
                if (!File.Exists(LogFile))
                {
                    ThreadSafeFileHelper.AppendAllText(LogFile, FileHeader.GenerateFileHeader(LogFile,ApplicationName));
                }



                // Note : This approach below will throw exception if "RunAgentsInParallel=true" , because it's is likely two or more threads access the file simultaneously.

                //                    #region Not-Thread-Safe File Write Approach

                //#if NETFRAMEWORK || NETSTANDARD2_0

                //                    return Task.Run(() =>
                //                    {
                //                        try
                //                        {
                //                            File.AppendAllText(LogFile, useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());
                //                        }
                //                        catch (Exception ex)
                //                        {
                //                            InternalLogger?.LogInternalException(ex);
                //                        }
                //                    }, cancellationToken);

                //#else
                //                    return File.AppendAllTextAsync(LogFile, useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText(), cancellationToken);
                //#endif



                //                    #endregion


                #region ThreadSafe File Write Approach

                return Task.Run(() =>
                      {
                          try
                          {
                              ThreadSafeFileHelper.AppendAllText(LogFile, useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());
                          }
                          catch (Exception ex)
                          {
                              InternalLogger?.LogInternalException(ex);
                          }
                      }, cancellationToken);
                #endregion



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
                                   $"The log file \"{LogFile}\" exceeded the maximum permitted size of \"{MaxLogFileSizeMB:N0} MB\".", EventId: 1));

                    InternalLogger?.LogInternalSystemEvent(new LogEventModel(LogEventTypes.SYSTEM,
                                   $"The log file \"{LogFile}\" has been deleted.", EventId: 2));



                }
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

        }



    }

}

