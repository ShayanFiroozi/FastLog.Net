using FastLog.Agents;
using FastLog.Agents.AdvancedAgents;
using FastLog.Agents.ConsoleAgents;
using FastLog.Agents.DebugAndTraceAgents;
using FastLog.Agents.FileBaseAgents;
using System;
using System.Collections.Generic;
using System.Linq;
using TrendSoft.FastLog.Interfaces;

namespace FastLog.Core
{
    public class AgentsManager
    {
        private readonly List<IAgent> loggerAgents = new List<IAgent>();
        public IEnumerable<IAgent> AgentList => loggerAgents;



        private AgentsManager() { }

        public static AgentsManager Create()
        {
            return new AgentsManager();
        }

        public AgentsManager AddBeepAgent(BeepAgent beepAgent)
        {
            AddAgent(beepAgent);
            return this;
        }

        public AgentsManager AddConsoleAgent(ConsoleAgent consoleLogger)
        {
            AddAgent(consoleLogger);
            return this;
        }

        public AgentsManager AddDebugSystemAgent(DebugSystemAgent debugSystemAgent)
        {
            AddAgent(debugSystemAgent);
            return this;
        }


        public AgentsManager AddTraceSystemAgent(TraceSystemAgent traceSystemAgent)
        {
            AddAgent(traceSystemAgent);
            return this;
        }


        public AgentsManager AddHeavyOperationSimulatorAgent(HeavyOperationSimulatorAgent heavyOperationSimulator)
        {
            
            AddAgent(heavyOperationSimulator);
            return this;
        }


        public AgentsManager AddTextFileAgent(TextFileAgent textFileAgent)
        {
            AddAgent(textFileAgent);
            return this;
        }


        public AgentsManager AddRunProcessAgent(RunProcessAgent runProcessAgent)
        {
            AddAgent(runProcessAgent);
            return this;
        }



        public AgentsManager AddMethodExecutionAgent(MethodExecutionAgent methodExecutionAgent)
        {
            AddAgent(methodExecutionAgent);
            return this;
        }



        public void AddAgent(IAgent agent)
        {

            if (agent is TextFileAgent)
            {
                if(loggerAgents.Any(a=>((TextFileAgent)a).LogFile == ((TextFileAgent)agent).LogFile))
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
