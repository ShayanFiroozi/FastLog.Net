using FastLog.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;

namespace FastLog.Agents
{
    public class AgentBase<AgentType> where AgentType : AgentBase<AgentType>, IAgent
    {
        public List<LogEventTypes> RegisteredEvents => _registeredEvents;


        #region Private Properties
        private protected string ApplicationName { get; set; } = "N/A";

        private protected InternalLogger InternalLogger = null;
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private bool executeOnlyOnDebugMode { get; set; }
        private bool executeOnlyOnReleaseMode { get; set; }
        #endregion


        internal AgentType WithApplicationName(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentException($"'{nameof(applicationName)}' cannot be null or whitespace.", nameof(applicationName));
            }

            ApplicationName = applicationName;

            return (AgentType)this;
        }

        internal AgentType WithInternalLogger(InternalLogger internalLogger)
        {
            if (internalLogger is null)
            {
                throw new ArgumentNullException(nameof(internalLogger));
            }

            InternalLogger = internalLogger;

            return (AgentType)this;
        }


        #region Execution Conditions

        public AgentType ExecuteOnlyOnDebugMode()
        {
            executeOnlyOnDebugMode = true;
            return (AgentType)this;
        }

        public AgentType ExecuteOnlyOnReleaseMode()
        {
            executeOnlyOnReleaseMode = true;
            return (AgentType)this;
        }

        private protected bool CanExecuteOnThidMode()
        {

#if !RELEASE

            if (executeOnlyOnReleaseMode) return false;

#endif

#if !DEBUG
            if (executeOnlyOnDebugMode) return false;

#endif

            return true;

        }

        #endregion


        #region Event Management
        public AgentType IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return (AgentType)this;
        }

        public AgentType ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return (AgentType)this;
        }

        public AgentType IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return (AgentType)this;
        }

        public AgentType ExcludeAllEventTypes()
        {
            _registeredEvents.Clear();

            return (AgentType)this;
        }

        private protected bool CanThisEventTypeExecute(LogEventTypes eventType) => RegisteredEvents.Any() && RegisteredEvents.Any(type => eventType == type);

        #endregion


    }
}
