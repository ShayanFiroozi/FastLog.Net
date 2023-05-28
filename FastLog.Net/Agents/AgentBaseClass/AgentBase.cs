/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using FastLog.Enums;
using FastLog.Interfaces;
using FastLog.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FastLog.Agents
{

    /// <summary>
    /// Base class of the agents.
    /// </summary>
    /// <typeparam name="AgentType">Any Agent class which implements IAgent</typeparam>
    public class BaseAgent<AgentType> where AgentType : BaseAgent<AgentType>, IAgent
    {
        #region Private Properties
        private protected string LoggerName { get; private set; } = "N/A";

        private protected AgentsManager _manager { get; set; } = null; // Just for Builder Pattern

        private protected InternalLogger InternalLogger = null;
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private bool executeOnlyOnDesignTime { get; set; }
        private bool executeOnlyOnRuntime { get; set; }
        #endregion



        /// <summary>
        /// All active event type(s) in the execution channel.
        /// </summary>
        public List<LogEventTypes> RegisteredEvents => _registeredEvents;




        /// <summary>
        /// Build the agent.
        /// </summary>
        /// <returns>Builder Pattern : AgentsManager</returns>
        public AgentsManager BuildAgent() => _manager;


        /// <summary>
        /// (Optional) Define Logger Name or Title.
        /// </summary>
        /// <param name="loggerName">Friendly name or title for the logger.</param>
        /// <returns>AgentType</returns>
        /// <exception cref="ArgumentException"></exception>
        internal AgentType WithLoggerName(string loggerName)
        {
            if (string.IsNullOrWhiteSpace(loggerName))
            {
                throw new ArgumentException($"'{nameof(loggerName)}' cannot be null or whitespace.", nameof(loggerName));
            }

            LoggerName = loggerName;

            return (AgentType)this;
        }



        /// <summary>
        /// (Optional) (High Recommended) Define an internal logger for FastLog.Net itself ! to log the internal exceptions or events.
        /// It is highly recommended to provide an internal logger to catch and trace FastLog.Net internal exception or events.
        /// </summary>
        /// <param name="internalLogger"></param>
        /// <returns>AgentType</returns>
        /// <exception cref="ArgumentNullException"></exception>
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


        public AgentType ExecuteOnlyOnDesignTime()
        {
            executeOnlyOnDesignTime = true;
            return (AgentType)this;
        }

        public AgentType ExecuteOnlyOnRuntime()
        {
            executeOnlyOnRuntime = true;
            return (AgentType)this;
        }


        /// <summary>
        /// Check if agent should execute on the current mode or not ( Debug and Release ).
        /// </summary>
        /// <returns>bool</returns>
        private protected bool CanExecuteOnThidMode()
        {

            return (!Debugger.IsAttached || !executeOnlyOnRuntime) && (Debugger.IsAttached || !executeOnlyOnDesignTime);
        }

        #endregion


        #region Event Management



        /// <summary>
        /// Incude an event to the execution list.(the event will be ignored if already exists on the list).
        /// By default all events are included.
        /// </summary>
        /// <param name="logEventType">LogEventTypes to exclude.</param>
        /// <returns>Builder Pattern : AgentType</returns>
        public AgentType IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return (AgentType)this;
        }



        /// <summary>
        /// Exclude an event from the execution list.
        /// </summary>
        /// <param name="logEventType">LogEventTypes to exclude.</param>
        /// <returns>Builder Pattern : AgentType</returns>
        public AgentType ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return (AgentType)this;
        }


        /// <summary>
        /// Include all event types to the execution list.
        /// </summary>
        /// <returns>Builder Pattern : AgentType</returns>
        public AgentType IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return (AgentType)this;
        }


        /// <summary>
        /// Exclude all event type(s) from the execution list.
        /// </summary>
        /// <returns>Builder Pattern : AgentType</returns>
        public AgentType ExcludeAllEventTypes()
        {
            _registeredEvents.Clear();

            return (AgentType)this;
        }



        /// <summary>
        /// Check if current event should be logged(execute) or not.
        /// </summary>
        /// <param name="eventType">LogEventTypes</param>
        /// <returns>bool</returns>
        private protected bool CanThisEventTypeExecute(LogEventTypes eventType) =>
                               RegisteredEvents.Any() && RegisteredEvents.Any(type => eventType == type);

        #endregion


    }
}
