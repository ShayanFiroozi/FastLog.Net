/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.AdvancedAgents
{

    /// <summary>
    /// An agent to simulate an heavy operation , specially IO bound operations such as Email , SMS send or HTTP request.
    /// Note : This class uses "Builder" pattern.
    /// </summary>
    public sealed class HeavyOperationSimulatorAgent : BaseAgent<HeavyOperationSimulatorAgent>, IAgent
    {

        private TimeSpan OperationTimeSpan { get; set; }




        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
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
        /// <param name="manager">AgentManager reference to pass to the class private constructor.</param>
        /// <returns>Builder pattern : Returns HeavyOperationSimulatorAgent class.</returns>
        public static HeavyOperationSimulatorAgent Create(AgentsManager manager) => new HeavyOperationSimulatorAgent(manager);





        /// <summary>
        /// (Required) Set the simulated delay time a timespan class.
        /// </summary>
        /// <param name="timeSpan">Time to simulate the delay.</param>
        /// <returns>Builder pattern : Returns HeavyOperationSimulatorAgent object.</returns>
        public HeavyOperationSimulatorAgent WithDelay(TimeSpan timeSpan)
        {
            OperationTimeSpan = timeSpan;

            return this;
        }





        /// <summary>
        /// Execute the Agent.
        /// </summary>
        /// <param name="logModel">This parameter will be ignored in this agent.</param>
        /// <param name="cancellationToken">CancellationToken for canceling the running task.</param>
        /// <returns>Task</returns>
        public Task ExecuteAgent(ILogEventModel logModel, CancellationToken cancellationToken = default)
        {

            if (logModel is null)
            {
                return Task.CompletedTask;
            }


            if (!CanExecuteOnThidMode()) return Task.CompletedTask;

            if (!CanThisEventTypeExecute(logModel.LogEventType)) return Task.CompletedTask;



            // Simulate some heavy CPU or IO bound operation wth Task.Delay()
            return Task.Delay(OperationTimeSpan, cancellationToken);



        }



    }




}

