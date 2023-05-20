using FastLog.Core;
using System;
using TrendSoft.FastLog.Internal;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {


        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private Logger(InternalLogger internalLogger, ConfigManager config)
        {

            this.internalLogger = internalLogger ?? throw new ArgumentNullException(nameof(internalLogger));

            configManager = config ?? throw new ArgumentNullException(nameof(config));


            // Initialize Channels Reader/Writer
            LoggerChannelReader = LoggerChannel.Reader;
            LoggerChannelWriter = LoggerChannel.Writer;


            Agents = AgentsManager.Create();

        }

        public static Logger Create(InternalLogger internalLogger, ConfigManager config) => new Logger(internalLogger, config);



        public Logger ApplyAgents(AgentsManager agentsManager)
        {
            this.Agents = agentsManager;
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
                    inMemoryEvents.Clear();

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
