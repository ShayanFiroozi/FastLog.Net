using System.Threading;
using System.Threading.Tasks;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Interfaces
{
    public interface ILoggerAgent
    {
        public Task LogEvent(LogEventModel logMessage, CancellationToken cancellationToken = default);

    }
}
