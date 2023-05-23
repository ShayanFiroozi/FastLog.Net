/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Enums;
using FastLog.Helpers;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.FileBaseAgents
{


    /// <summary>
    /// An agent to write log into plain text or json file.
    /// </summary>
    public sealed class TextFileAgent : BaseAgent<TextFileAgent>, IAgent
    {


        #region Private Properties

        private bool useJsonFormat { get; set; } = false;
        internal string LogFile { get; set; } = string.Empty;
        private short MaxLogFileSizeMB { get; set; } = 900; // Limited to 900 MB

        #endregion



        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>
        private TextFileAgent(AgentsManager manager)
        {

            _manager = manager; // Just For Builder Pattern.

            IncludeAllEventTypes();
        }


        /// <summary>
        /// Create a new TextFileAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern : Returns TextFileAgent class.</returns>
        public static TextFileAgent Create(AgentsManager manager) => new TextFileAgent(manager);


        /// <summary>
        /// (Optional) Ask the agent to use json format for the logging data.
        /// </summary>
        /// <returns>Builder pattern : Returns TextFileAgent class.</returns>
        public TextFileAgent UseJsonFormat()
        {
            useJsonFormat = true;
            return this;
        }


        /// <summary>
        /// (Required) Define a file to save the logs.
        /// </summary>
        /// <param name="filename">File to save the logs.</param>
        /// <returns>Builder pattern : Returns TextFileAgent class.</returns>
        /// <exception cref="ArgumentException"></exception>
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



        /// <summary>
        /// (Optional) Define the max log file size in megabytes , default file size is  : 900 MB
        /// Warning : The log file will be "DELETED" when reached to this size.
        /// </summary>
        /// <param name="logFileMaxSizeMB">The max log file size in megabytes</param>
        /// <returns>Builder pattern : Returns TextFileAgent class.</returns>
        /// <exception cref="ArgumentException"></exception>
        public TextFileAgent DeleteTheLogFileWhenExceededTheMaximumSizeOf(short logFileMaxSizeMB)
        {

            if (logFileMaxSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(logFileMaxSizeMB)}' must be greater then zero.", nameof(logFileMaxSizeMB));
            }

            MaxLogFileSizeMB = logFileMaxSizeMB;

            return this;

        }

        /// <summary>
        /// Execute the Agent.
        /// </summary>
        /// <param name="LogModel">Logging info</param>
        /// <param name="cancellationToken">CancellationToken for canceling the running task.</param>
        /// <returns>Task</returns>
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
                    ThreadSafeFileHelper.AppendAllText(LogFile, FileHeader.GenerateFileHeader(LogFile, LoggerName));
                }



                #region Non Thread-Safe attempt !  (Disabled)
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
                #endregion



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

