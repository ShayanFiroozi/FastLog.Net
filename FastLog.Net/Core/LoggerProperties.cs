using FastLog.Net.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {



        #region Private Properties
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly InternalLogger internalLogger = null;
        private ConfigManager configManager;
        private bool isLoggerRunning = false;

        private List<LogEventModel> inMemoryEvents { get; set; } = new List<LogEventModel>();


        #region Channel Properties
        private readonly Channel<LogEventModel> LoggerChannel = Channel.CreateUnbounded<LogEventModel>(new UnboundedChannelOptions());
        private readonly ChannelReader<LogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<LogEventModel> LoggerChannelWriter;
        #endregion

        #endregion




        public AgentsManager Agents { get; private set; }
        public IEnumerable<LogEventModel> InMemoryEvents => inMemoryEvents;



    }
}
