using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Interfaces
{

    public interface IFileLogger : ILoggerAgent
    {

        public void SaveLog(LogMessageModel logMessage);



    }
}
