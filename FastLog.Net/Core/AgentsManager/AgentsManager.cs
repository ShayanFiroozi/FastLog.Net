using System;
using System.Collections.Generic;
using System.Linq;
using FastLog.Net.Agents;
using FastLog.Net.Agents.AdvancedAgents;
using FastLog.Net.Agents.ConsoleAgents;
using FastLog.Net.Agents.DebugAndTraceAgents;
using FastLog.Net.Agents.FileBaseAgents;
using TrendSoft.FastLog.Interfaces;

namespace FastLog.Net.Core
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

        public AgentsManager AddDebugWindowAgent(DebugSystemAgent debugWindowLogger)
        {
            AddAgent(debugWindowLogger);
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



        public AgentsManager AddInMemoryAgent(InMemoryAgent methodExecutionAgent)
        {
            AddAgent(methodExecutionAgent);
            return this;
        }



        public void AddAgent(IAgent agent)
        {
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

            if (agent is BeepAgent && loggerAgents.Any(a => a is BeepAgent))
            {
                throw new Exception("A \"BeepAgent\" agent already exists on the agent list.");
            }


            loggerAgents.Add(agent);
        }

        public object AddBeepAgent(AgentBase<BeepAgent> agentBase)
        {
            throw new NotImplementedException();
        }
    }
}
