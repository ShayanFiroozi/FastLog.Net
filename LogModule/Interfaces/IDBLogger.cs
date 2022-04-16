using System;
using System.Threading.Tasks;

namespace LogModule
{
    public interface IDBLogger : ILogger , IDisposable
    {
        public int MAX_LOGS_COUNT { get; set; }

        public int GetLogCount { get; }


        public int GetLogCountTaskAsync { get; }

        public void DeleteOldLogs(short OlderThanDays);

        public Task DeleteOldLogsTaskAsync(short OlderThanDays);


    }
}
