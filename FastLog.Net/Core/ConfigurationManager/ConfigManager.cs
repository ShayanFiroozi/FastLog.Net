/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System;

namespace FastLog.Core
{

    /// <summary>
    /// Class to create FastLog.Net configuration
    /// </summary>
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


        /// <summary>
        /// Max log event(s) to keep in memory which can be accessed by "InMemoryEvents" property. ( default is 0 )
        /// </summary>
        /// <param name="maxEventsToKeep"></param>
        /// <returns>Builder Pattern : ConfigManager</returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// (Optional) Define Logger name or title.
        /// </summary>
        /// <param name="loggerName">logger name or title.</param>
        /// <returns>Builder Pattern : ConfigManager</returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// (Optional) Run logger's agent(s) in seuential or parallel mode. ( default is false).
        /// WARNING : Run agent in parallel may impact the overall performance.
        /// </summary>
        public ConfigManager RunAgentsInParallelMode()
        {
            RunAgentsInParallel = true;
            return this;
        }

    }
}
