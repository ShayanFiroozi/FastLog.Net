﻿/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Enums;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using FluentConsoleNet;
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
        public Task ExecuteAgent(ILogEventModel logEvent, CancellationToken cancellationToken = default)
        {

            if (logEvent is null)
            {
                return Task.CompletedTask;
            }


            // We won't process a console log on a "Console" agent !!
            //if (logEvent.LogEventType is LogEventTypes.CONSOLE)
            //{
            //    return Task.CompletedTask;
            //}

            if (!CanExecuteOnThidMode()) return Task.CompletedTask;



            try
            {

                if (!CanThisEventTypeExecute(logEvent.LogEventType)) return Task.CompletedTask;

                // Set the proper console forecolor

                switch (logEvent.LogEventType)
                {
                    case LogEventTypes.INFO:

                        // Print on console
                        FastConsole.PrintInfo(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.NOTE:

                        // Print on console
                        FastConsole.PrintNote(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.TODO:

                        // Print on console
                        FastConsole.PrintTodo(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.WARNING:

                        // Print on console
                        FastConsole.PrintWarning(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.ALERT:

                        // Print on console
                        FastConsole.PrintAlert(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.DEBUG:

                        // Print on console
                        FastConsole.PrintDebug(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.ERROR:

                        // Print on console
                        FastConsole.PrintError(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.EXCEPTION:

                        // Print on console
                        FastConsole.PrintException(logEvent.Exception, JsonFormat: useJsonFormat);

                        break;


                    case LogEventTypes.SYSTEM:

                        // Print on console
                        FastConsole.PrintSystem(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    case LogEventTypes.SECURITY:

                        // Print on console
                        FastConsole.PrintSecurity(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;


                    case LogEventTypes.CONSOLE:

                        // Print on console
                        FastConsole.PrintText(useJsonFormat ? logEvent.ToJsonText() : logEvent.ToPlainText(false, false));

                        break;

                    default:
                        break;
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

