using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly InternalLogger InternalLogger = null;
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
