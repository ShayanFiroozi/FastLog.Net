/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Interfaces
{

    /// <summary>
    /// Interface which all agents should implement.
    /// </summary>
    public interface IAgent
    {

#if NETFRAMEWORK || NETSTANDARD2_0
        Task ExecuteAgent(ILogEventModel logEvent, CancellationToken cancellationToken = default);
#else
        internal Task ExecuteAgent(ILogEventModel logEvent, CancellationToken cancellationToken = default);
#endif


    }
}
