﻿using System;

namespace FastLog.Core
{
    public class ConfigManager
    {
        internal int MaxEventsToKeep { get; set; } = 0;
        internal bool RunAgentsInParallel { get; set; } = false;


        private ConfigManager() { }

        public static ConfigManager Create()
        {
            return new ConfigManager();
        }

        public ConfigManager WithMaxEventsToKeepInMemory(int maxEventsToKeep)
        {

            if (maxEventsToKeep < 0)
            {
                throw new ArgumentException($"'{nameof(maxEventsToKeep)}' can not be negative.", nameof(maxEventsToKeep));
            }

            this.MaxEventsToKeep = maxEventsToKeep;
            return this;


        }


    

        /// <summary>
        /// WARNING : Run "Logger Agents" in parallel may impact the performance.
        /// </summary>
        public ConfigManager RunAgentsInParallelMode()
        {
            RunAgentsInParallel = true;
            return this;
        }

    }
}
