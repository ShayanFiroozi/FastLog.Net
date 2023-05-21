using FastLog.Agents;
using FastLog.Agents.AdvancedAgents;
using FastLog.Agents.ConsoleAgents;
using FastLog.Agents.DebugAndTraceAgents;
using FastLog.Agents.FileBaseAgents;
using System;
using System.Collections.Generic;
using System.Linq;
using FastLog.Core;
using FastLog.Interfaces;
using FastLog.Internal;
using System.Diagnostics;

namespace FastLog.Core
{
    public sealed class AgentsManager
    {
        private readonly Logger _logger = null; // Will be used by the builder pattern to pass refrences.

        private InternalLogger InternalLogger = null;
        private string LoggerName = "N/A";

        private readonly List<IAgent> loggerAgents = new List<IAgent>();
        public IEnumerable<IAgent> AgentList => loggerAgents;



        private AgentsManager(Logger logger) => _logger = logger;


        internal AgentsManager WithInternalLogger(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            return this;
        }

        internal AgentsManager WithLoggerName(string loggerName)
        {
            LoggerName = loggerName;
            return this;
        }



        internal static AgentsManager Create(Logger logger) => new AgentsManager(logger);


        public Logger BuildLogger()
        {
            ValidateAgents();
            return _logger; // Just Used by "Builder" pattern
        }

        public BeepAgent AddBeepAgent()
        {
            return (BeepAgent)AddUserDefinedAgent(BeepAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }

        public ConsoleAgent AddConsoleAgent()
        {
            return (ConsoleAgent)AddUserDefinedAgent(ConsoleAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }

        public DebugSystemAgent AddDebugSystemAgent()
        {
            return (DebugSystemAgent)AddUserDefinedAgent(DebugSystemAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        public TraceSystemAgent AddTraceSystemAgent()
        {
            return (TraceSystemAgent)AddUserDefinedAgent(TraceSystemAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        public HeavyOperationSimulatorAgent AddHeavyOperationSimulatorAgent()
        {
            return (HeavyOperationSimulatorAgent)AddUserDefinedAgent(HeavyOperationSimulatorAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        public TextFileAgent AddTextFileAgent()
        {
            return (TextFileAgent)AddUserDefinedAgent(TextFileAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }


        public RunProcessAgent AddRunProcessAgent()
        {
            return (RunProcessAgent)AddUserDefinedAgent(RunProcessAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }



        public MethodExecutionAgent AddMethodExecutionAgent()
        {
            return (MethodExecutionAgent)AddUserDefinedAgent(MethodExecutionAgent.Create(this) // pass the current logger to the AgeManager for builder pattern.
                                  .WithInternalLogger(InternalLogger)
                                  .WithLoggerName(_logger.Configuration.LoggerName));
        }



        public IAgent AddUserDefinedAgent(IAgent agent)
        {

            loggerAgents.Add(agent);

            return agent;
        }


        public IAgent RemoveAgent(IAgent agent)
        {
            loggerAgents.Remove(agent);
            return agent;
        }



        private void ValidateAgents()
        {
            foreach (IAgent agent in _logger.WithAgents().loggerAgents)
            {

                if (agent is TextFileAgent)
                {

                    if (loggerAgents.Count(a => ((TextFileAgent)a).LogFile == ((TextFileAgent)agent).LogFile) > 1)
                    {
                        throw new Exception("A \"TextFileAgent\" agent with same log file already exists on the agent list.");
                    }

                }

                if (agent is ConsoleAgent && loggerAgents.Any(a => a is ConsoleAgent))
                {
                    throw new Exception("A \"ConsoleLogger\" agent already exists on the agent list.");
                }


#if DEBUG
                if (agent is DebugSystemAgent && loggerAgents.Any(a => a is DebugSystemAgent))
                {
                    throw new Exception("A \"DebugWindowLogger\" agent already exists on the agent list.");
                }
#endif

                if (agent is TraceSystemAgent && loggerAgents.Any(a => a is TraceSystemAgent))
                {
                    throw new Exception("A \"TraceSystemAgent\" agent already exists on the agent list.");
                }


                if (agent is BeepAgent && loggerAgents.Any(a => a is BeepAgent))
                {
                    throw new Exception("A \"BeepAgent\" agent already exists on the agent list.");
                }

            }
        }






    }
}
