using FastLog.Net.Core;
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
     
        private bool _IsLoggerRunning = false;
        private AgentsManager agentsManager;

      
        private string applicationName { get; set; } = string.Empty;
        private bool saveMachineName { get; set; } = false;
        private bool runAgentsInParallel { get; set; } = false;



        // Channel properties
        private readonly Channel<LogEventModel> LoggerChannel = Channel.CreateUnbounded<LogEventModel>(new UnboundedChannelOptions());
        private readonly ChannelReader<LogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<LogEventModel> LoggerChannelWriter;


    





    }
}
