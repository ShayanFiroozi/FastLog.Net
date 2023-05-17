
using FastLog.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : RunCommandAgent class uses fluent "Builder" pattern.

    public class RunProcessAgent : ILoggerAgent
    {
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private readonly InternalLogger InternalLogger = null;
        private bool executeOnlyOnDebugMode { get; set; } = false;
        private bool executeOnlyOnReleaseMode { get; set; } = false;

        private string WorkingDirectory { get; set; } = string.Empty;
        private string ProcessToExecute { get; set; } = string.Empty;
        private string ExecutionArgument { get; set; }
        private bool runAsAdministrator { get; set; }
        private bool useShellExecute { get; set; }

        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private RunProcessAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }

        public static RunProcessAgent Create(InternalLogger internalLogger = null) => new RunProcessAgent(internalLogger);
        public RunProcessAgent ExecuteOnlyOnDebugMode()
        {
            executeOnlyOnDebugMode = true;
            return this;
        }

        public RunProcessAgent ExecuteOnlyOnReleaseMode()
        {
            executeOnlyOnReleaseMode = true;
            return this;
        }

        public RunProcessAgent ExecuteProcess(string commandToExecute)
        {
            if (string.IsNullOrWhiteSpace(commandToExecute))
            {
                throw new ArgumentException($"'{nameof(commandToExecute)}' cannot be null or whitespace.", nameof(commandToExecute));
            }

            ProcessToExecute = commandToExecute;

            return this;
        }

        public RunProcessAgent WithArgument(string commandArgument)
        {
            if (string.IsNullOrWhiteSpace(ProcessToExecute))
            {
                throw new ArgumentException($"'{nameof(ProcessToExecute)}' cannot be null or whitespace.", nameof(ProcessToExecute));
            }


            ExecutionArgument = commandArgument;

            return this;
        }


        public RunProcessAgent RunAsAdministrator()
        {
            runAsAdministrator = true;

            return this;
        }


        public RunProcessAgent WithWorkingDirectory(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;

            return this;
        }


        public RunProcessAgent WithShellExecute()
        {
            useShellExecute = true;

            return this;
        }


        public RunProcessAgent IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }



        public RunProcessAgent ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public RunProcessAgent IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public RunProcessAgent ExcludeAllEventTypes()
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

            if (string.IsNullOrWhiteSpace(ProcessToExecute)) return Task.CompletedTask;





            if (LogModel is null)
            {
                return Task.CompletedTask;
            }




            try
            {

                // Check if current log "Event Type" should be execute or not.
                if (!_registeredEvents.Any()) return Task.CompletedTask;
                if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


                RunProcess(WorkingDirectory, ProcessToExecute, ExecutionArgument, useShellExecute, runAsAdministrator);

            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;

        }


        private Process RunProcess(string workingDirectory,
                                                string fileName,
                                                string arguments = "",
                                                bool UseShellExecute = false,
                                                bool RunAsAdministrator = false)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = workingDirectory,
                    FileName = fileName,
                    Verb = RunAsAdministrator ? "runas" : "",
                    ErrorDialog = false,
                    UseShellExecute = UseShellExecute,
                    Arguments = arguments,
                };

                return Process.Start(startInfo);

            }

            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return new Process();

        }




    }

}



