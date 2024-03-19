/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Interfaces;
using FastLog.Internal;
using System;

namespace FastLog.Core
{

    /// <summary>
    /// Logger core class.
    /// </summary>
    public sealed partial class Logger : IFastLogger, IDisposable
    {

        #region Fluent Builder Methods

        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>

        private Logger()
        {
            // Initialize Channels Reader/Writer
            LoggerChannelReader = LoggerChannel.Reader;
            LoggerChannelWriter = LoggerChannel.Writer;

        }


        /// <summary>
        /// Creae AgentManager object.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns>Builder Pattern : Logger</returns>
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
                throw new InvalidOperationException($"The \"ApplyInternalLogger\" should be called first !");
            }

            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // Init AgentManager with "Internal Logger" & "Logger Name" dependencies.
            Agents = AgentsManager.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger);

            return this;
        }



        public AgentsManager WithAgents() => Agents;


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
                    _cts.Dispose();

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
