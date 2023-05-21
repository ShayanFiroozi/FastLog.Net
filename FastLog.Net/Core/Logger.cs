using FastLog.Core;
using System;
using FastLog.Internal;

namespace FastLog.Core
{

    public sealed partial class Logger : IDisposable
    {


        #region Fluent Builder Methods


        //Keep it private to make it non accessible from the outside of the class !!
        private Logger()
        {

            // Initialize Channels Reader/Writer
            LoggerChannelReader = LoggerChannel.Reader;
            LoggerChannelWriter = LoggerChannel.Writer;


            //Agents = AgentsManager.Create();

        }

        public static Logger Create() => new Logger();


        public Logger WithInternalLogger(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger ?? throw new ArgumentNullException(nameof(internalLogger));
            return this;
        }


        public Logger WithConfiguration(ConfigManager configuration)
        {
            //#Refactor : The fluent builder should be design to force the user to build the Logger in proper order.
            //https://methodpoet.com/builder-pattern/

            if (InternalLogger == null)
            {
                throw new ArgumentNullException($"{nameof(InternalLogger)} can not be null.", $"The \"ApplyInternalLogger\" should be called first !");
            }

            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // Init AgentManager with "Internal Logger" & "Logger Name" dependencies.
            Agents = AgentsManager.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(Configuration.LoggerName);

            return this;
        }



        public AgentsManager WithAgents() => Agents;
//        {

        //    //#Refactor : The fluent builder should be design to force the user to build the Logger in proper order.
        //    //https://methodpoet.com/builder-pattern/

        //    if (Configuration == null)
        //    {
        //        throw new ArgumentNullException($"{nameof(Configuration)} can not be null.", $"The \"ApplyConfiguration\" should be called first !");
        //    }


        //    return this;
        //}



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
