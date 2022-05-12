using System.Threading.Tasks;

namespace LogModule
{


    public interface IFileLogger : ILogger
    {

        public string LogFile { get; }

        public int LogFileSizeMB { get; }

        public void DeleteLogFile();



    }
}
