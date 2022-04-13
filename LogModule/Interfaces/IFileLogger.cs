namespace LogModule
{


    public interface IFileLogger : ILogger
    {

        public string LogFileName { get;}

        public string LogFilePath { get;}

        public string LogFileFullPath { get; }

        public byte LogFileSizeMB { get; }

        public void DeleteLogFile();


    }
}
