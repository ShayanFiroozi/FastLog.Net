﻿using System;
using System.Collections.Generic;
using System.Linq;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Interfaces;

namespace FastLog.Net.Core
{
    public class AgentsManager
    {
        private readonly List<ILoggerAgent> loggerAgents = new List<ILoggerAgent>();
        public IEnumerable<ILoggerAgent> AgentList => loggerAgents;



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

        public AgentsManager AddDebugWindowAgent(DebugWindowAgent debugWindowLogger)
        {
            AddAgent(debugWindowLogger);
            return this;
        }


        public AgentsManager AddHeavyOperationSimulatorAgent(HeavyOperationSimulatorAgent heavyOperationSimulator)
        {
            AddAgent(heavyOperationSimulator);
            return this;
        }


        public AgentsManager AddPlaintTextFileAgent(PlainTextFileAgent plainTextFileLogger)
        {
            AddAgent(plainTextFileLogger);
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



        public void AddAgent(ILoggerAgent agent)
        {
            if (agent is ConsoleAgent && loggerAgents.Any(a => a is ConsoleAgent))
            {
                throw new Exception("A \"ConsoleLogger\" agent already exists on the agent list.");
            }


#if DEBUG
            if (agent is DebugWindowAgent && loggerAgents.Any(a => a is DebugWindowAgent))
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

    }
}
