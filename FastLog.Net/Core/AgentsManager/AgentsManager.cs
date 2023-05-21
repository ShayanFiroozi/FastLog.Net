using FastLog.Agents;
using FastLog.Agents.AdvancedAgents;
using FastLog.Agents.ConsoleAgents;
using FastLog.Agents.DebugAndTraceAgents;
using FastLog.Agents.FileBaseAgents;
using System;
using System.Collections.Generic;
using System.Linq;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;

namespace FastLog.Core
{
    public class AgentsManager
    {
        private readonly InternalLogger InternalLogger = null;
        private readonly string ApplicationName = "N/A";

        private readonly List<IAgent> loggerAgents = new List<IAgent>();
        public IEnumerable<IAgent> AgentList => loggerAgents;



        private AgentsManager(InternalLogger internalLogger, string applicationName)
        {
            InternalLogger = internalLogger;
            ApplicationName = applicationName;
        }

        public static AgentsManager Create(InternalLogger internalLogger, string applicationName)
        {
            if (internalLogger is null)
            {
                throw new ArgumentNullException(nameof(internalLogger));
            }

            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException($"'{nameof(applicationName)}' cannot be null or empty.", nameof(applicationName));
            }

            return new AgentsManager(internalLogger, applicationName);
        }

        public AgentsManager AddBeepAgent(BeepAgent beepAgent)
        {
            AddAgent(beepAgent.WithInternalLogger(InternalLogger).WithApplicationName(ApplicationName));
            return this;
        }

        public AgentsManager AddConsoleAgent(ConsoleAgent consoleLogger)
        {
            AddAgent(consoleLogger.WithInternalLogger(InternalLogger).WithApplicationName(ApplicationName));
            return this;
        }

        public AgentsManager AddDebugSystemAgent(DebugSystemAgent debugSystemAgent)
        {
            AddAgent(debugSystemAgent.WithInternalLogger(InternalLogger)
                                     .WithApplicationName(ApplicationName));
            return this;
        }


        public AgentsManager AddTraceSystemAgent(TraceSystemAgent traceSystemAgent)
        {
            AddAgent(traceSystemAgent.WithInternalLogger(InternalLogger).WithApplicationName(ApplicationName));
            return this;
        }


        public AgentsManager AddHeavyOperationSimulatorAgent(HeavyOperationSimulatorAgent heavyOperationSimulator)
        {

            AddAgent(heavyOperationSimulator.WithInternalLogger(InternalLogger).WithApplicationName(ApplicationName));
            return this;
        }


        public AgentsManager AddTextFileAgent(TextFileAgent textFileAgent)
        {
            AddAgent(textFileAgent.WithInternalLogger(InternalLogger).WithApplicationName(ApplicationName));
            return this;
        }


        public AgentsManager AddRunProcessAgent(RunProcessAgent runProcessAgent)
        {
            AddAgent(runProcessAgent.WithInternalLogger(InternalLogger).WithApplicationName(ApplicationName));
            return this;
        }



        public AgentsManager AddMethodExecutionAgent(MethodExecutionAgent methodExecutionAgent)
        {
            AddAgent(methodExecutionAgent.WithInternalLogger(InternalLogger).WithApplicationName(ApplicationName));
            return this;
        }



        public void AddAgent(IAgent agent)
        {

            if (agent is TextFileAgent)
            {
                if (loggerAgents.Any(a => ((TextFileAgent)a).LogFile == ((TextFileAgent)agent).LogFile))
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


            loggerAgents.Add(agent);
        }

    }
}
