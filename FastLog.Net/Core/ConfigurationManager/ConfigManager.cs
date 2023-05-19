using System;

namespace FastLog.Core
{
    public class ConfigManager
    {
        internal int MaxEventsToKeep { get; set; } = 0;
        internal string ApplicationName { get; set; } = string.Empty;
        internal bool SaveMachineName { get; set; } = false;
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


        public ConfigManager IncludeMachineName()
        {
            SaveMachineName = true;
            return this;
        }


        public ConfigManager IncludeApplicationName(string applicationName)
        {
            this.ApplicationName = applicationName;
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
