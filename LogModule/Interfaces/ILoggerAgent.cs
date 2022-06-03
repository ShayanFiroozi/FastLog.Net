namespace LogModule
{
    public interface ILoggerAgent
    {
        public string LogFile { get; }

        public int LogFileSizeMB { get; }

        public void DeleteLogFile();

    }
}
