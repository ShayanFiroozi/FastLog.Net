/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.DebugAndTraceAgents
{

    /// <summary>
    /// An agent to write the logging info on the Debug system.
    /// Notes : This class uses "Builder" pattern.
    /// </summary>
    public sealed class DebugSystemAgent : BaseAgent<DebugSystemAgent>, IAgent
    {


        private bool useJsonFormat { get; set; } = false;


        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>
        private DebugSystemAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }


        /// <summary>
        /// Create a new DebugSystemAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern : Returns DebugSystemAgent class</returns>
        public static DebugSystemAgent Create(AgentsManager manager) => new DebugSystemAgent(manager);


        /// <summary>
        /// Ask the agent to use json format for the logging data.
        /// </summary>
        /// <returns>Builder pattern : Returns DebugSystemAgent class</returns>
        public DebugSystemAgent UseJsonFormat()
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
        public Task ExecuteAgent(ILogEventModel LogModel, CancellationToken cancellationToken = default)
        {

            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            if (!CanExecuteOnThidMode()) return Task.CompletedTask;




            try
            {

                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;

                Debug.WriteLine(useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }



}

