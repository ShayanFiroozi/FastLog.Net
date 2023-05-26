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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.AdvancedAgents
{

    /// <summary>
    /// An agent to run a process in "Normal" and "As administrator" mode.
    /// Note : This class uses "Builder" pattern.
    /// </summary>

    public sealed class RunProcessAgent : BaseAgent<MethodExecutionAgent>, IAgent
    {


        #region Private Properties
        private string WorkingDirectory { get; set; } = string.Empty;
        private string processToExecute { get; set; } = string.Empty;
        private string ExecutionArgument { get; set; }
        private bool runAsAdministrator { get; set; }
        private bool useShellExecute { get; set; }
        #endregion



        /// <summary>
        /// Builder Pattern : Keep it private to make it non accessible from the outside of the class !!
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the AgentBase class to achieve Builder pattern.</param>
        private RunProcessAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }



        /// <summary>
        /// Create a new RunProcessAgent object.
        /// </summary>
        /// <param name="manager">AgentManager reference to pass to the class private constructor</param>
        /// <returns>Builder pattern : Returns RunProcessAgent class</returns>
        public static RunProcessAgent Create(AgentsManager manager) => new RunProcessAgent(manager);




        /// <summary>
        /// (Required) Define an excecutable file to excecute.
        /// </summary>
        /// <param name="processToExecute">executable file</param>
        /// <returns>Builder pattern : Returns RunProcessAgent class</returns>
        /// <exception cref="ArgumentException"></exception>
        /// 
        public RunProcessAgent ProcessToExecute(string processToExecute)
        {
            if (string.IsNullOrWhiteSpace(processToExecute))
            {
                throw new ArgumentException($"'{nameof(processToExecute)}' cannot be null or whitespace.", nameof(processToExecute));
            }

            this.processToExecute = processToExecute;

            return this;
        }



        /// <summary>
        /// (Optional) Define process argument(s).
        /// </summary>
        /// <param name="commandArgument"></param>
        /// <returns>Builder pattern : Returns RunProcessAgent class</returns>
        /// <exception cref="ArgumentException"></exception>
        public RunProcessAgent WithArgument(string commandArgument)
        {
            if (string.IsNullOrWhiteSpace(commandArgument))
            {
                throw new ArgumentException($"'{nameof(commandArgument)}' cannot be null or whitespace.", nameof(commandArgument));
            }


            ExecutionArgument = commandArgument;

            return this;
        }



        /// <summary>
        /// (Optional) Run process as administartor.
        /// </summary>
        /// <returns>Builder pattern  : Returns RunProcessAgent class</returns>
        public RunProcessAgent RunAsAdministrator()
        {
            runAsAdministrator = true;

            return this;
        }




        /// <summary>
        /// (Optional) Define the working directory of process.
        /// </summary>
        /// <param name="workingDirectory">Working directory of the process</param>
        /// <returns>Builder pattern : Returns RunProcessAgent class</returns>
        public RunProcessAgent SetWorkingDirectory(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;

            return this;
        }



        /// <summary>
        /// Run process in shell ?
        /// for more info visit : <seealso cref="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.useshellexecute?view=net-7.0"/>
        /// </summary>
        /// <returns>Builder pattern : Returns RunProcessAgent class</returns>
        public RunProcessAgent UseShellExecute()
        {
            useShellExecute = true;

            return this;
        }


        /// <summary>
        /// Execute the Agent.
        /// </summary>
        /// <param name="LogModel">This parameter will be ignored in this agent.</param>
        /// <param name="cancellationToken">CancellationToken for canceling the running task</param>
        /// <returns>Task</returns>
        public Task ExecuteAgent(ILogEventModel LogModel, CancellationToken cancellationToken = default)
        {

            if (LogModel is null)
            {
                return Task.CompletedTask;
            }


            if (!CanExecuteOnThidMode()) return Task.CompletedTask;


         


            try
            {

                if (!CanThisEventTypeExecute(LogModel.LogEventType)) return Task.CompletedTask;


                RunProcess(WorkingDirectory, processToExecute, ExecutionArgument, useShellExecute, runAsAdministrator);

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



