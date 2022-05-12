using System;
using System.IO;
using System.Threading.Tasks;

namespace LogModule.Agents
{
    public sealed class DbLogger : IDBLogger
    {
        public int MAX_LOGS_COUNT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int GetLogCount => throw new NotImplementedException();

        public int GetLogCountTaskAsync => throw new NotImplementedException();

        public void DeleteOldLogs(short OlderThanDays)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOldLogsTaskAsync(short OlderThanDays)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SaveLog(LogMessage logMessage)
        {
            throw new NotImplementedException();
        }

        public Task SaveLogTaskAsync(LogMessage logMessage)
        {
            throw new NotImplementedException();
        }
    }
}
