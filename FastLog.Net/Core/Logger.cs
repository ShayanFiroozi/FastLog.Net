using FastLog.Core;
using System;
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

            this.internalLogger = internalLogger;

            // Init properties below to prevent null exception if user did not set them later !
            Agents = AgentsManager.Create();
            configManager = ConfigManager.Create();
        }

        public static Logger Create(InternalLogger internalLogger = null) => new Logger(internalLogger);



        public Logger ApplyAgents(AgentsManager agentsManager)
        {
            this.Agents = agentsManager;
            return this;
        }


        public Logger ApplyConfig(ConfigManager configManager)
        {
            this.configManager = configManager;
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
                    this.internalLogger?.LogInternalException(ex);
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
