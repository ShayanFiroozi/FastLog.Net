using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Interfaces
{
    public interface IAgent
    {

#if NETFRAMEWORK || NETSTANDARD2_0
        Task ExecuteAgent(LogEventModel logMessage, CancellationToken cancellationToken = default);
#else
        internal Task ExecuteAgent(LogEventModel logMessage, CancellationToken cancellationToken = default);
#endif


    }
}
