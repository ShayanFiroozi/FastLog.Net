using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Interfaces;

namespace FastLog.Net.Core
{
    public class AgentsManager
    {
        private List<ILoggerAgent> _loggerAgents = new List<ILoggerAgent>();
        public IEnumerable<ILoggerAgent> AgentsList => _loggerAgents;



        internal AgentsManager()
        {
            
        }

        public AgentsManager AddBeepAgent(BeepAgent beepAgent)
        {
            AddLoggingAgent(beepAgent);
            return this;
        }

        public AgentsManager AddConsoleAgent(ConsoleAgent consoleLogger)
        {
            AddLoggingAgent(consoleLogger);
            return this;
        }

        public AgentsManager AddDebugWindowAgent(DebugWindowAgent debugWindowLogger)
        {
            AddLoggingAgent(debugWindowLogger);
            return this;
        }


        public AgentsManager AddHeavyOperationSimulatorAgent(HeavyOperationSimulatorAgent heavyOperationSimulator)
        {
            AddLoggingAgent(heavyOperationSimulator);
            return this;
        }


        public AgentsManager AddPlaintTextFileAgent(PlainTextFileAgent plainTextFileLogger)
        {
            AddLoggingAgent(plainTextFileLogger);
            return this;
        }


        public AgentsManager AddRunProcessAgent(RunProcessAgent runProcessAgent)
        {
            AddLoggingAgent(runProcessAgent);
            return this;
        }





        private void AddLoggingAgent(ILoggerAgent agent)
        {
            if (agent is ConsoleAgent && _loggerAgents.Any(a => a is ConsoleAgent))
            {
                throw new Exception("A \"ConsoleLogger\" agent already exists on the agent list.");
            }


#if DEBUG
            if (agent is DebugWindowAgent && _loggerAgents.Any(a => a is DebugWindowAgent))
            {
                throw new Exception("A \"DebugWindowLogger\" agent already exists on the agent list.");
            }
#endif

            if (agent is BeepAgent && _loggerAgents.Any(a => a is BeepAgent))
            {
                throw new Exception("A \"BeepAgent\" agent already exists on the agent list.");
            }


            _loggerAgents.Add(agent);
        }

    }
}
