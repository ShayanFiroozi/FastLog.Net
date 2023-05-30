/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Agents.AdvancedAgents;
using FastLog.Agents.ConsoleAgents;
using FastLog.Agents.DebugAndTraceAgents;
using FastLog.Agents.FileBaseAgents;
using FastLog.Interfaces;
using FastLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastLog.Core
{
    public sealed class AgentsManager
    {

        #region Private Properties

        /// <summary>
        /// Builder Pattern.
        /// </summary>
        private readonly Logger _logger = null;

        /// <summary>
        /// Internal Logger instance.
        /// </summary>
        private InternalLogger InternalLogger = null;

        private readonly List<IAgent> loggerAgents = new List<IAgent>();
        #endregion


        /// <summary>
        /// The list of the active agent(s).
        /// </summary>
        public IEnumerable<IAgent> AgentList => loggerAgents;


        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="logger">Builder Pattern : Logger reference to pass to the logger class to achieve Builder pattern.</param>
        private AgentsManager(Logger logger) => _logger = logger;



        /// <summary>
        /// Creae AgentsManager object.
        /// </summary>
        /// <param name="logger">Builder Pattern : Logger reference to pass to the logger class to achieve Builder pattern.</param>
        /// <returns>Builder Pattern : AgentsManager</returns>
        internal static AgentsManager Create(Logger logger) => new AgentsManager(logger);



        /// <summary>
        /// (Optional) (High Recommended) Define an internal logger for FastLog.Net itself ! to log the internal exceptions or events.
        /// It is highly recommended to provide an internal logger to catch and trace FastLog.Net internal exception or events.
        /// </summary>
        /// <param name="internalLogger">reference to an internal logger object.</param>
        /// <returns>Builder Pattern : AgentsManager</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal AgentsManager WithInternalLogger(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger ?? throw new ArgumentNullException(nameof(internalLogger));
            return this;
        }


        /// <summary>
        /// Build the logger object.
        /// </summary>
        /// <returns>Builder Pattern : Logger</returns>
        public Logger BuildLogger()
        {
            ValidateAgents();
            return _logger; // Just Used by "Builder" pattern
        }


        /// <summary>
        /// Add Beep agent.
        /// </summary>
        /// <returns>Builder Pattern : BeepAgent</returns>
        public BeepAgent AddBeepAgent()
        {
            return (BeepAgent)AddUserDefinedAgent(BeepAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        /// <summary>
        /// Add console agent.
        /// </summary>
        /// <returns>Builder Pattern : ConsoleAgent</returns>
        public ConsoleAgent AddConsoleAgent()
        {
            return (ConsoleAgent)AddUserDefinedAgent(ConsoleAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }



        /// <summary>
        /// Add Debug System agent.
        /// </summary>
        /// <returns>Builder Pattern : DebugSystemAgent</returns>
        public DebugSystemAgent AddDebugSystemAgent()
        {
            return (DebugSystemAgent)AddUserDefinedAgent(DebugSystemAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        /// <summary>
        /// Add Trace System agent.
        /// </summary>
        /// <returns>Builder Pattern : TraceSystemAgent</returns>
        public TraceSystemAgent AddTraceSystemAgent()
        {
            return (TraceSystemAgent)AddUserDefinedAgent(TraceSystemAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        /// <summary>
        /// Add Heavy Operation Simulator agent.
        /// </summary>
        /// <returns>Builder Pattern : HeavyOperationSimulatorAgent</returns>
        public HeavyOperationSimulatorAgent AddHeavyOperationSimulatorAgent()
        {
            return (HeavyOperationSimulatorAgent)AddUserDefinedAgent(HeavyOperationSimulatorAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }



        /// <summary>
        /// Add TextFileAgent agent.
        /// </summary>
        /// <returns>Builder Pattern : TextFileAgent</returns>
        public TextFileAgent AddTextFileAgent()
        {
            return (TextFileAgent)AddUserDefinedAgent(TextFileAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        /// <summary>
        /// Add RunProcessAgent agent.
        /// </summary>
        /// <returns>Builder Pattern : RunProcessAgent</returns>
        public RunProcessAgent AddRunProcessAgent()
        {
            return (RunProcessAgent)AddUserDefinedAgent(RunProcessAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        /// <summary>
        /// Add MethodExecutionAgent agent.
        /// </summary>
        /// <returns>Builder Pattern : MethodExecutionAgent</returns>
        public MethodExecutionAgent AddMethodExecutionAgent()
        {
            return (MethodExecutionAgent)AddUserDefinedAgent(MethodExecutionAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }



        /// <summary>
        /// Add agent (IAgent) class to the active agents list.
        /// </summary>
        /// <param name="agent">Any agent class which implements IAgent</param>
        /// <returns>Builder Pattern : IAgent</returns>
        public IAgent AddUserDefinedAgent(IAgent agent)
        {

            loggerAgents.Add(agent);

            return agent;
        }


        /// <summary>
        /// Remove agent (IAgent) class to the active agents list.
        /// </summary>
        /// <param name="agent">Any agent class which implements IAgent</param>
        /// <returns>Builder Pattern : IAgent</returns>
        public IAgent RemoveAgent(IAgent agent)
        {
            loggerAgents.Remove(agent);
            return agent;
        }



        private void ValidateAgents()
        {
            foreach (IAgent agent in _logger.WithAgents().loggerAgents)
            {
                //Check for duplicate log files in agents.
                if (agent is TextFileAgent && loggerAgents.Where(a => a is TextFileAgent).Count(a => ((TextFileAgent)a).LogFile == ((TextFileAgent)agent).LogFile) > 1)
                {
                    throw new NotSupportedException("A \"TextFileAgent\" agent with the same log file already exists on the agent list.");
                }

                if (loggerAgents.Count(a => a is ConsoleAgent) > 1)
                {
                    throw new NotSupportedException("A \"ConsoleLogger\" agent already exists on the agent list.");
                }



                if (loggerAgents.Count(a => a is DebugSystemAgent) > 1)
                {
                    throw new NotSupportedException("A \"DebugWindowLogger\" agent already exists on the agent list.");
                }


                if (loggerAgents.Count(a => a is TraceSystemAgent) > 1)
                {
                    throw new NotSupportedException("A \"TraceSystemAgent\" agent already exists on the agent list.");
                }


                if (loggerAgents.Count(a => a is BeepAgent) > 1)
                {
                    throw new NotSupportedException("A \"BeepAgent\" agent already exists on the agent list.");
                }

            }
        }






    }
}
