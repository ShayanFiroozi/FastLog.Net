using System;
using System.Threading;


namespace NetMQServer.Core.Utils
{
    internal class Proactor : PollerBase
    {
        private const int CompletionStatusArraySize = 100;

  
        private readonly string m_name;

        private Thread? m_worker;
        private bool m_stopping;
        private bool m_stopped;

        private class Item
        {
            public Item(IProactorEvents proactorEvents) => ProactorEvents = proactorEvents;

            public IProactorEvents ProactorEvents { get; }
            public bool Cancelled { get; set; }
        }

        public Proactor(string name)
        {
            m_name = name;
            m_stopping = false;
            m_stopped = false;
           
        }

        public void Start()
        {
            m_worker = new Thread(Loop) { IsBackground = true, Name = m_name };
            m_worker.Start();
        }

        public void Stop()
        {
            m_stopping = true;
        }

        public void Destroy()
        {
            if (!m_stopped)
            {
                try
                {
                 
                    m_worker.Join();
                }
                catch (Exception)
                {}

                m_stopped = true;

               
            }
        }

        public void SignalMailbox(IOThreadMailbox mailbox)
        {
           
        }

     

      
        /// <exception cref="ArgumentOutOfRangeException">The completionStatuses item must have a valid OperationType.</exception>
        private void Loop()
        {
            

            while (!m_stopping)
            {
                // Execute any due timers.
                int timeout = ExecuteTimers();

              
                for (int i = 0; i < 1000; i++)
                {
      
                }
            }
        }
    }
}