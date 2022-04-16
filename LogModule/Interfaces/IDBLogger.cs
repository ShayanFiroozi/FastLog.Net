using System.Threading.Tasks;

namespace LogModule
{
    public interface IDBLogger : ILogger
    {
        public int MAX_LOGS_COUNT { get; set; }

        public int GetLogCount { get; }

        public void DeleteOldLogs(short OlderThanDays);


        public Task DeleteOldLogsTaskAsync(short OlderThanDays);


    }
}
