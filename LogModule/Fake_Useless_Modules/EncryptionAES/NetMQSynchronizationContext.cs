using System.Threading;
using System.Threading.Tasks;

#if !NET35
namespace NetMQServer
{
    internal class NetMQSynchronizationContext : SynchronizationContext
    {
        private readonly NetMQPoller m_poller;

        public NetMQSynchronizationContext(NetMQPoller poller)
        {
            m_poller = poller;
        }

        /// <summary>Dispatches an asynchronous message to a synchronization context.</summary>
        public override void Post(SendOrPostCallback d, object state)
        {
            Task task = new Task(() => d(state));
            task.Start(m_poller);
        }

        /// <summary>Dispatches a synchronous message to a synchronization context.</summary>
        public override void Send(SendOrPostCallback d, object state)
        {
            Task task = new Task(() => d(state));
            task.Start(m_poller);
            task.Wait();
        }
    }
}
#endif