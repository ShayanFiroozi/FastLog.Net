/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.AdvancedAgents
{

    /// <summary>
    /// An agent to execute a method.
    /// Note : This class uses "Builder" pattern.
    /// </summary>
    public sealed class MethodExecutionAgent : BaseAgent<MethodExecutionAgent>, IAgent
    {

        private Action methodToExecute { get; set; }



        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>
        private MethodExecutionAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }




        /// <summary>
        /// Create a new MethodExecutionAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern : Returns MethodExecutionAgent class</returns>
        public static MethodExecutionAgent Create(AgentsManager manager) => new MethodExecutionAgent(manager);




        /// <summary>
        /// (Required) Define a method to execute.
        /// </summary>
        /// <param name="method">Method reference to execute.</param>
        /// <returns>Builder pattern : Returns MethodExecutionAgent object</returns>
        public MethodExecutionAgent MethodToExecute(Action method)
        {
            methodToExecute = method;
            return this;
        }



        /// <summary>
        /// Execute the Agent.
        /// </summary>
        /// <param name="LogModel">Logging info provided by the user</param>
        /// <param name="cancellationToken">CancellationToken for canceling the running task</param>
        /// <returns>Task</returns>
        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {

            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            if (!CanExecuteOnThidMode()) return Task.CompletedTask;

            if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;



            try
            {

                if (methodToExecute != null)
                {
                    // Run method
                    return Task.Run(methodToExecute, cancellationToken);
                }


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;

        }




    }

}



