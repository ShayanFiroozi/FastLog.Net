using System.Threading.Tasks;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Interfaces
{
    public interface ILoggerAgent
    {
        public Task SaveLog(LogMessageModel logMessage);

    }
}
