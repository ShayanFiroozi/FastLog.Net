using FastLog.Internal;
using FastLog.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;

namespace FastLog.Core
{

    public sealed partial class Logger : IDisposable
    {



        #region Private Properties
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private InternalLogger InternalLogger = null;
        internal ConfigManager Configuration;
        private bool IsLoggerRunning = false;
        private AgentsManager Agents { get; set; }
        private List<LogEventModel> inMemoryEvents { get; set; } = new List<LogEventModel>();


        #region Channel Properties
        private readonly Channel<LogEventModel> LoggerChannel = Channel.CreateUnbounded<LogEventModel>(new UnboundedChannelOptions());
        private readonly ChannelReader<LogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<LogEventModel> LoggerChannelWriter;
        #endregion

        #endregion




        public IEnumerable<LogEventModel> InMemoryEvents => inMemoryEvents;



    }
}
