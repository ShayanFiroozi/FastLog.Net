using FastLog.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : UserDefinedAgent class uses fluent "Builder" pattern.

    public class MethodExecutionAgent : ILoggerAgent
    {
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private readonly InternalLogger InternalLogger = null;
        private bool executeOnlyOnDebugMode { get; set; } = false;
        private bool executeOnlyOnReleaseMode { get; set; } = false;
        private Action methodToExecute { get; set; }

        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private MethodExecutionAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }

        public static MethodExecutionAgent Create(InternalLogger internalLogger = null) => new MethodExecutionAgent(internalLogger);

        public MethodExecutionAgent MethodToExecute(Action method)
        {
            methodToExecute = method;
            return this;
        }

        public MethodExecutionAgent ExecuteOnlyOnDebugMode()
        {
            executeOnlyOnDebugMode = true;
            return this;
        }

        public MethodExecutionAgent ExecuteOnlyOnReleaseMode()
        {
            executeOnlyOnReleaseMode = true;
            return this;
        }


        public MethodExecutionAgent IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public MethodExecutionAgent ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public MethodExecutionAgent IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public MethodExecutionAgent ExcludeAllEventTypes()
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


            if (methodToExecute != null)
            {
                return Task.Run(methodToExecute, cancellationToken);
            }

            try
            {

                // Call user-defined function here !


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;

        }

        public MethodExecutionAgent MethodToExecute(Task task)
        {
            throw new NotImplementedException();
        }
    }

}



