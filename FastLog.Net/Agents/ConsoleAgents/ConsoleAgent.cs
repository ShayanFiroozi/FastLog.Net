/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Enums;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.ConsoleAgents
{

    /// <summary>
    /// An agent to write the logging info on the Console.
    /// Notes : This class uses "Builder" pattern.
    /// Performance Warning : The Console has poor performance in comparison with file operation.
    /// It is not recommended to use it on high performance and multi-threaded scenarios unless in time of debuggin.
    /// </summary>

    public sealed class ConsoleAgent : BaseAgent<ConsoleAgent>, IAgent
    {

        private bool useJsonFormat { get; set; } = false;


        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>
        private ConsoleAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }


        /// <summary>
        /// Create a new ConsoleAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern : Returns ConsoleAgent class</returns>
        public static ConsoleAgent Create(AgentsManager manager) => new ConsoleAgent(manager);


        /// <summary>
        /// Ask the agent to use json format for the logging data.
        /// </summary>
        /// <returns>Builder pattern : Returns ConsoleAgent class</returns>
        public ConsoleAgent UseJsonFormat()
        {
            useJsonFormat = true;
            return this;
        }


        /// <summary>
        /// Execute the Agent.
        /// </summary>
        /// <param name="logModel">Logging info</param>
        /// <param name="cancellationToken">CancellationToken for canceling the running task.</param>
        /// <returns>Task</returns>
        public Task ExecuteAgent(ILogEventModel logModel, CancellationToken cancellationToken = default)
        {



            if (logModel is null)
            {
                return Task.CompletedTask;
            }

            if (!CanExecuteOnThidMode()) return Task.CompletedTask;



            try
            {

                if (!CanThisEventTypeExecute(logModel.LogEventType)) return Task.CompletedTask;

                // Set the proper console forecolor

                switch (logModel.LogEventType)
                {
                    case LogEventTypes.INFO:
                    case LogEventTypes.NOTE:
                    case LogEventTypes.TODO:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;

                    case LogEventTypes.WARNING:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogEventTypes.ALERT:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;

                    case LogEventTypes.DEBUG:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;

                    case LogEventTypes.ERROR:
                    case LogEventTypes.EXCEPTION:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;


                    case LogEventTypes.SYSTEM:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;

                    case LogEventTypes.SECURITY:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    default:
                        break;
                }


                Console.WriteLine(useJsonFormat ? logModel.ToJsonText() : logModel.ToPlainText());


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }

}

