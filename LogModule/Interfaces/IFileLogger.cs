using System.Threading.Tasks;

namespace LogModule
{


    public interface IFileLogger : ILogger
    {

        public void SaveLog(LogMessage logMessage , bool threadSafeWrite);



    }
}
