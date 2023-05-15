﻿using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : BeepAgent class uses fluent "Builder" pattern.

    public class BeepAgent : ILoggerAgent
    {
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private InternalExceptionLogger InternalLogger = null;


        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private BeepAgent() => IncludeAllEventTypes();

        public static BeepAgent Create() => new BeepAgent();

        public BeepAgent WithInternalLogger(InternalExceptionLogger internalLogger)
        {
            InternalLogger = internalLogger ?? throw new ArgumentNullException(nameof(internalLogger));

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


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {


            if (LogModel is null)
            {
                return Task.CompletedTask;
            }





            try
            {

                // Check if any "Event Type" exists to show on console ?
                if (!_registeredEvents.Any()) return Task.CompletedTask;


                // Check if current log "Event Type" should be reflected onthe Console or not.
                if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


                // Note : "Beep" works only on Windows® OS.
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


