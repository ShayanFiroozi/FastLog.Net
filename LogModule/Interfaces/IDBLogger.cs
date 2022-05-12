using System;
using System.Threading.Tasks;

namespace LogModule
{
    public interface IDBLogger : ILogger
    {

        public string DBFile { get; }

        public int DBFileSizeMB { get; }

        public int GetLogCount { get; }

        public void DeleteOldLogs(short OlderThanDays);


    }
}
