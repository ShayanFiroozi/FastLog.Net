
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

namespace FastLog.Net.Agents.AdvancedAgents
{

    // Note : RunCommandAgent class uses fluent "Builder" pattern.

    public class RunProcessAgent : AgentBase<MethodExecutionAgent>, IAgent
    {


        #region Private Properties
        private string WorkingDirectory { get; set; } = string.Empty;
        private string ProcessToExecute { get; set; } = string.Empty;
        private string ExecutionArgument { get; set; }
        private bool runAsAdministrator { get; set; }
        private bool useShellExecute { get; set; } 
        #endregion



        //Keep it private to make it non accessible from the outside of the class !!
        private RunProcessAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }

        public static RunProcessAgent Create(InternalLogger internalLogger = null) => new RunProcessAgent(internalLogger);

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


        public RunProcessAgent SetWorkingDirectory(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;

            return this;
        }


        public RunProcessAgent UseShellExecute()
        {
            useShellExecute = true;

            return this;
        }



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



