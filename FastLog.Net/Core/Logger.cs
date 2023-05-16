using System;
using System.Diagnostics;
using System.Linq;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {


        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private Logger(InternalLogger internalLogger = null)
        {
            // Initialize Channels Reader/Writer
            LoggerChannelReader = LoggerChannel.Reader;
            LoggerChannelWriter = LoggerChannel.Writer;

            InternalLogger = internalLogger;
        }

        public static Logger Create(InternalLogger internalLogger = null) => new Logger(internalLogger);


        public Logger WithBeep(BeepAgent beepAgent)
        {
            AddLoggingAgent(beepAgent);
            return this;
        }


        public Logger WithPrintOnConsole(ConsoleLogger consoleLogger)
        {
            AddLoggingAgent(consoleLogger);
            return this;
        }




        public Logger WithPrintOnDebugWindow(DebugWindowLogger debugWindowLogger)
        {
            AddLoggingAgent(debugWindowLogger);
            return this;
        }


        public Logger AddWindowsEventLogger(WindowsEventLogger windowsEventLogger)
        {
            AddLoggingAgent(windowsEventLogger);
            return this;
        }

        public Logger AddHeavyOperationSimulator(HeavyOperationSimulator heavyOperationSimulator)
        {
            AddLoggingAgent(heavyOperationSimulator);
            return this;
        }


        public Logger AddPlaintTextFileLogger(PlainTextFileLogger plainTextFileLogger)
        {
            AddLoggingAgent(plainTextFileLogger);
            return this;
        }


        public Logger LogMachineName()
        {
            saveMachineName = true;
            return this;
        }


        public Logger LogApplicationName(string applicationName)
        {
            this.applicationName = applicationName;
            return this;
        }

        /// <summary>
        /// WARNING : Run "Logger Agents" in parallel may impact the performance.
        /// </summary>
        public Logger RunAgentsInParallel()
        {
            runAgentsInParallel = true;
            return this;
        }

#endregion



        #region "Logger Agents" management functions

        private void AddLoggingAgent(ILoggerAgent agent)
        {
            if (agent is ConsoleLogger && _loggerAgents.Any(a => a is ConsoleLogger))
            {
                throw new Exception("A \"ConsoleLogger\" agent already exists on the agent list.");
            }


#if DEBUG
            if (agent is DebugWindowLogger && _loggerAgents.Any(a => a is DebugWindowLogger))
            {
                throw new Exception("A \"DebugWindowLogger\" agent already exists on the agent list.");
            }
#endif

            if (agent is BeepAgent && _loggerAgents.Any(a => a is BeepAgent))
            {
                throw new Exception("A \"BeepAgent\" agent already exists on the agent list.");
            }


            _loggerAgents.Add(agent);
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
                    _loggerAgents.Clear();
                    _loggerAgents = null;
                }

                catch (Exception ex)
                {
                    this.InternalLogger?.LogInternalException(ex);
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
