using System;
using System.Linq;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {


        #region Constructors

        public Logger(InternalLogger InternalLogger,
                      bool LogMachineName = false,
                      bool RunAgentParallel = true)
        {

            this.InternalLogger = InternalLogger;



            this.LogMachineName = LogMachineName;
            this.RunAgentParallel = RunAgentParallel;


            // Initialize Channels Reader/Writer
            LoggerChannelReader = LoggerChannel.Reader;
            LoggerChannelWriter = LoggerChannel.Writer;
        }

        #endregion



        #region "Logger Agents" management functions

        public void AddLoggingAgent(ILoggerAgent logger)
        {
            if (logger is ConsoleLogger && _loggerAgents.Any(agent => agent is ConsoleLogger))
            {
                throw new Exception("A \"ConsoleLogger\" agent already exists on the agent list.");
            }


#if DEBUG
            if (logger is DebugWindowLogger && _loggerAgents.Any(agent => agent is DebugWindowLogger))
            {
                throw new Exception("A \"DebugWindowLogger\" agent already exists on the agent list.");
            }
#endif


            // Prevent multi File Logger with same log file !
            if (logger is PlainTextFileLogger && _loggerAgents.Any(agent => ((PlainTextFileLogger)agent).LogFile == ((PlainTextFileLogger)logger).LogFile))
            {
                throw new Exception($"A \"PlainTextFileLogger\" agent with the same log file already exists on the agent list.({((PlainTextFileLogger)logger).LogFile})");
            }


            _loggerAgents.Add(logger);
        }

        public void ClearLoggingAgents()
        {
            _loggerAgents.Clear();
        }

        #endregion



        #region DisposeMethods

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    StopLogger();

                    //logDB?.Dispose();
                    ClearLoggingAgents();
                    _loggerAgents = null;
                }

                catch (Exception ex)
                {
                    InternalLogger?.LogInternalException(ex);
                }
            }

            disposed = true;

        }

        public void Dispose()
        {
            // Do not change this code , Put your clean up code in the Dispose(bool disposing) function instead.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion



    }
}
