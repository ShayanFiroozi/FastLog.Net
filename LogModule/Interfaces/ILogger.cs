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
        Task LogDebugTaskAsync(string LogText, string ExtraInfo = "", string Source = "");
        void LogError(string LogText, string ExtraInfo = "", string Source = "");
        Task LogErrorTaskAsync(string LogText, string ExtraInfo = "", string Source = "");
        void LogException(Exception exception);
        Task LogExceptionTaskAsync(Exception exception);
        void LogFatalError(Exception exception);
        Task LogFatalErrorTaskAsync(Exception exception);
        void LogInfo(string LogText, string ExtraInfo = "", string Source = "");
        Task LogInfoTaskAsync(string LogText, string ExtraInfo = "", string Source = "");
        void LogWarning(string LogText, string ExtraInfo = "", string Source = "");
        Task LogWarningTaskAsync(string LogText, string ExtraInfo = "", string Source = "");
    }
}