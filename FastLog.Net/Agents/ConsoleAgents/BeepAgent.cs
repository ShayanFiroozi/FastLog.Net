using FastLog.Core;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.ConsoleAgents
{


    public sealed class BeepAgent : AgentBase<BeepAgent>, IAgent
    {

        //Keep it private to make it non accessible from the outside of the class !!
        private BeepAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }

        public static BeepAgent Create(AgentsManager manager) => new BeepAgent(manager);


        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {


            if (!CanExecuteOnThidMode()) return Task.CompletedTask;


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }




            try
            {

                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;


                // Note : "Beep" only works on Windows® OS.
                // ATTENTION : There's a chance of "HostProtectionException" or "PlatformNotSupportedException" exception.
                // For more info please visit : https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Console.Beep();


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;

        }

    }

}



