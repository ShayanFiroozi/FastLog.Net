namespace FastLog.Net.Core
{
    public class ConfigManager
    {
        public string ApplicationName { get; set; } = string.Empty;
        public bool SaveMachineName { get; set; } = false;
        public bool runAgentsInParallel { get; set; } = false;



        private ConfigManager() { }

        public static ConfigManager Create()
        {
            return new ConfigManager();
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
        public ConfigManager RunAgentsInParallel()
        {
            runAgentsInParallel = true;
            return this;
        }

    }
}
