
using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly InternalExceptionLogger InternalLogger = null;
        private List<ILoggerAgent> _loggerAgents = new List<ILoggerAgent>();
        private bool _IsLoggerRunning = false;

        // Channel properties
        private readonly Channel<LogEventModel> LoggerChannel = Channel.CreateUnbounded<LogEventModel>(new UnboundedChannelOptions());
        private readonly ChannelReader<LogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<LogEventModel> LoggerChannelWriter;


        public IEnumerable<ILoggerAgent> Agents => _loggerAgents;

        

        public bool LogMachineName { get; private set; } = false;
        public bool RunAgentParallel { get; private set; } = true;


    }
}
