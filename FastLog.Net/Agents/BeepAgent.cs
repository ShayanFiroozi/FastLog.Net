using FastLog.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : BeepAgent class uses fluent "Builder" pattern.

    public class BeepAgent : ILoggerAgent
    {
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private InternalLogger InternalLogger = null;
        private bool executeOnlyOnDebugMode { get; set; } = false;
        private bool executeOnlyOnReleaseMode { get; set; } = false;

        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private BeepAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }

        public static BeepAgent Create(InternalLogger internalLogger = null) => new BeepAgent(internalLogger);

        public BeepAgent ExecuteOnlyOnDebugMode()
        {
            executeOnlyOnDebugMode = true;
            return this;
        }


        public BeepAgent ExecuteOnlyOnReleaseMode()
        {
            executeOnlyOnReleaseMode = true;
            return this;
        }


        public BeepAgent IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public BeepAgent ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public BeepAgent IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public BeepAgent ExcludeAllEventTypes()
        {
            _registeredEvents.Clear();

            return this;
        }

        #endregion


        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {


#if !RELEASE

            if (executeOnlyOnReleaseMode) return Task.CompletedTask;

#endif

#if !DEBUG
            if (executeOnlyOnDebugMode) return Task.CompletedTask;

#endif


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }




            try
            {

                // Check if current log "Event Type" should be execute or not.
                if (!_registeredEvents.Any()) return Task.CompletedTask;
                if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


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



