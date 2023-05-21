using FastLog.Core;
using FastLog.Enums;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Agents.ConsoleAgents
{

    // Note : ConsoleLogger class uses fluent "Builder" pattern.

    public sealed class ConsoleAgent : AgentBase<ConsoleAgent>, IAgent
    {

        private bool useJsonFormat { get; set; } = false;


        //Keep it private to make it non accessible from the outside of the class !!
        private ConsoleAgent(AgentsManager manager)
        {
            _manager = manager;
            IncludeAllEventTypes();
        }

        public static ConsoleAgent Create(AgentsManager manager) => new ConsoleAgent(manager);

        public ConsoleAgent UseJsonFormat()
        {
            useJsonFormat = true;
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

                // Set the proper console forecolor

                switch (LogModel.LogEventType)
                {
                    case LogEventTypes.INFO:
                    case LogEventTypes.NOTE:
                    case LogEventTypes.TODO:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;

                    case LogEventTypes.WARNING:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogEventTypes.ALERT:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;

                    case LogEventTypes.DEBUG:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;

                    case LogEventTypes.ERROR:
                    case LogEventTypes.EXCEPTION:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;


                    case LogEventTypes.SYSTEM:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;

                    case LogEventTypes.SECURITY:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    default:
                        break;
                }


                Console.WriteLine(useJsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }

}

