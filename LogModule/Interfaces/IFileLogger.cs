using System.Threading.Tasks;

namespace LogModule
{


    public interface IFileLogger : ILogger
    {

        public string LogFileName { get; }

        public string LogFilePath { get; }

        public string LogFileFullPath { get; }

        public int LogFileSizeMB { get; }

        public void DeleteLogFile();

        public Task DeleteLogFileTaskAsync();


    }
}
