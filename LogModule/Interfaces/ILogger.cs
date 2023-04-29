using System;
using System.Threading.Tasks;

namespace LogModule
{
    public interface ILogger
    {
        void AddLoggingAgent(ILoggerAgent logger);
        void ClearLoggingAgents();
        void Dispose();
        void LogDebug(string LogText, string ExtraInfo = "", string Source = "");
        Task LogDebugTask(string LogText, string ExtraInfo = "", string Source = "");
        void LogError(string LogText, string ExtraInfo = "", string Source = "");
        Task LogErrorTask(string LogText, string ExtraInfo = "", string Source = "");
        void LogException(Exception exception);
        Task LogExceptionTask(Exception exception);
        void LogLoggerInternalException(Exception exception);
        Task LogLoggerInternalExceptionTask(Exception exception);
        void LogInfo(string LogText, string ExtraInfo = "", string Source = "");
        Task LogInfoTask(string LogText, string ExtraInfo = "", string Source = "");
        void LogWarning(string LogText, string ExtraInfo = "", string Source = "");
        Task LogWarningTask(string LogText, string ExtraInfo = "", string Source = "");
    }
}