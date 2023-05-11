using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{

    /// <summary>
    /// WARNING : "Console.WriteLine" has serious performance and memory issues , Be careful when use it !
    /// </summary>
    public class DebugWindowLogger : ILoggerAgent
    {
        public DebugWindowLogger()
        {
#if RELEASE
#warning "DebugWindowLogger" only works in Debug mode !
#endif
        }


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {

#if !DEBUG
            return Task.CompletedTask;
#endif

            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            Debug.WriteLine($"{LogModel}");


            return Task.CompletedTask;


        }




    }

}

