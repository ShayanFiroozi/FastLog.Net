using System;
using System.Threading.Tasks;

namespace LogModule
{
    public interface IDBLogger : ILogger
    {
     
        public int GetLogCount { get; }

        public void DeleteOldLogs(short OlderThanDays);


    }
}
