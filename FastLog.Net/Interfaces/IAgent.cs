using FastLog.Core;
using FastLog.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Interfaces
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
