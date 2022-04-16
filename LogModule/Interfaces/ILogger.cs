using System.Threading.Tasks;

namespace LogModule
{
    public interface ILogger
    {
        public void SaveLog(LogMessage logMessage);

        public Task SaveLogTaskAsync(LogMessage logMessage);

    }
}
