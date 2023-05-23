/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System;

namespace FastLog.Core
{
    public sealed class ConfigManager
    {
        internal string LoggerName { get; set; } = "N/A";
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



        public ConfigManager WithLoggerName(string loggerName)
        {
            if (string.IsNullOrWhiteSpace(loggerName))
            {
                throw new ArgumentException($"'{nameof(loggerName)}' cannot be null or whitespace.", nameof(loggerName));
            }

            LoggerName = loggerName;
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
