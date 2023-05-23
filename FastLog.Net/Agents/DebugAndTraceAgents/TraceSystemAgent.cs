/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.DebugAndTraceAgents
{

    /// <summary>
    /// An agent to write the logging info on the Trace system.
    /// Notes : This class uses "Builder" pattern.
    /// </summary>
    public sealed class TraceSystemAgent : BaseAgent<TraceSystemAgent>, IAgent
    {

        private bool useJsonFormat { get; set; } = false;


        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>
        private TraceSystemAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }

        /// <summary>
        /// Create a new TraceSystemAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern : Returns TraceSystemAgent class</returns>
        public static TraceSystemAgent Create(AgentsManager manager) => new TraceSystemAgent(manager);


        /// <summary>
        /// Ask the agent to use json format for the logging data.
        /// </summary>
        /// <returns>Builder pattern : Returns TraceSystemAgent class</returns>
        public TraceSystemAgent UseJsonFormat()
        {
            useJsonFormat = true;
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
            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            if (!CanExecuteOnThidMode()) return Task.CompletedTask;


    

            try
            {

                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;


                Trace.WriteLine(useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }



}

