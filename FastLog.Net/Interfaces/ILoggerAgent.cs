using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Interfaces
{
    public interface ILoggerAgent
    {

#if NETFRAMEWORK || NETSTANDARD2_0
        Task LogEvent(LogEventModel logMessage, CancellationToken cancellationToken = default);
#else
        public Task LogEvent(LogEventModel logMessage, CancellationToken cancellationToken = default);
#endif


    }
}
