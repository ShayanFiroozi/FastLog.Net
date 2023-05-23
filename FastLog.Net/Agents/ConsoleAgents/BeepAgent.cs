/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.ConsoleAgents
{

    /// <summary>
    /// An agent to make a Beep sound from the BIOS or system speaker.
    /// Notes : This class uses "Builder" pattern , 
    /// 
    /// "Beep" only works on Windows® OS.
    /// ATTENTION : There's a possibility of "HostProtectionException" or "PlatformNotSupportedException" exception.
    /// For more info please visit : https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0
    /// Performance Warning : The Beep has poor performance and blocks the current thread until the Beep finished.
    /// </summary>

    public sealed class BeepAgent : BaseAgent<BeepAgent>, IAgent
    {

        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>
        private BeepAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }


        /// <summary>
        /// Create a new BeepAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern : Returns BeepAgent class</returns>
        public static BeepAgent Create(AgentsManager manager) => new BeepAgent(manager);



        /// <summary>
        /// Execute the Agent.
        /// </summary>
        /// <param name="LogModel">This parameter will be ignored in this agent.</param>
        /// <param name="cancellationToken">CancellationToken for canceling the running task.</param>
        /// <returns>Task</returns>
        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            if (!CanExecuteOnThidMode()) return Task.CompletedTask;






            try
            {

                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;


                // Note : "Beep" only works on Windows® OS.
                // ATTENTION : There's a possibility of "HostProtectionException" or "PlatformNotSupportedException" exception.
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



