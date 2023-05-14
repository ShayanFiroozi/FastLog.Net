using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    public class BeepAgent : ILoggerAgent
    {

        private readonly InternalExceptionLogger InternalLogger = null;


        public BeepAgent(InternalExceptionLogger internalLogger = null) => InternalLogger = internalLogger;



        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            // ATTENTION : there's a chance of "HostProtectionException" exception.

            // For more info please visit : https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0

            try
            {

                // Note : "Beep" works only on Windows® OS.
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



