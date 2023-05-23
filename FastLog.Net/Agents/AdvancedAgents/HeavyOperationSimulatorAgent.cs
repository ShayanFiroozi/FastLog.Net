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
    /// An agent to simulate an heavy operation , specially IO bound operations such as Email , SMS send or HTTP request.
    /// </summary>
    public sealed class HeavyOperationSimulatorAgent : AgentBase<HeavyOperationSimulatorAgent>, IAgent
    {

        private TimeSpan OperationTimeSpan { get; set; }


        /// <summary>
        /// // Builder Pattern : 
        /// //Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>

        private HeavyOperationSimulatorAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }



        /// <summary>
        /// Create a new HeavyOperationSimulatorAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern  : Return HeavyOperationSimulatorAgent class</returns>
        public static HeavyOperationSimulatorAgent Create(AgentsManager manager) => new HeavyOperationSimulatorAgent(manager);





        /// <summary>
        /// Set the simulated delay time a timespan class.
        /// </summary>
        /// <param name="timeSpan">Time to simulate the delay</param>
        /// <returns>Builder pattern  : Return HeavyOperationSimulatorAgent class</returns>
        public HeavyOperationSimulatorAgent WithDelay(TimeSpan timeSpan)
        {
            OperationTimeSpan = timeSpan;

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

            if (!CanExecuteOnThidMode()) return Task.CompletedTask;


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;



            // Simulate some heavy CPU or IO bound operation.
            return Task.Delay(OperationTimeSpan, cancellationToken);



        }



    }



}

