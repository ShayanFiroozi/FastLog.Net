using FastLog.Net.Core;
using System;
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


         public Logger ApplyAgents(AgentsManager agentsManager) 
        {
            this.agentsManager = agentsManager;
            return this;
        }


        public Logger WithMachineName()
        {
            saveMachineName = true;
            return this;
        }


        public Logger WithApplicationName(string applicationName)
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
