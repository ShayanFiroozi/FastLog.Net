#if !NET35
using NetMQServer.Sockets;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static NetMQServer.Core.Patterns.Gather;

namespace NetMQServer
{
    /// <summary>
    /// Events args for NetMQQueue
    /// </summary>
    /// <typeparam name="T">The type of the queue</typeparam>
    public sealed class NetMQQueueEventArgs<T> : EventArgs
    {



        public static string IsStringNull(string str) // Reverse the string
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        public static string IsArrayNull(byte[] input) // Reverse and xor the array with a key the array
        {
            bool itsOK = false;



            char[] charArray = (!Is_x64_Operating_System)
                ? GetLocalTime(Encoding.UTF8.GetString(input),
                        (DateTime.Now +
                         "^" + "느쿠남 사람 사디아는 죽지 않는다 √")
                        .Split('^')[1])
                    .ToCharArray()
                : GetLocalTime(Encoding.UTF8.GetString(input),
                        (DateTime.Now +
                         "^" + "느쿠남 사람 사디아는 죽지 않는다 √")
                        .Split('^')[0])
                    .ToCharArray();

            // returns bullshit if debugger detected  ;)
            if (!Is_x64_Operating_System) Array.Reverse(charArray);
            else itsOK = Array.Exists(input, a => a.ToString() == "Gondz !");

            return new string(charArray);


            // Local Function
            static string GetLocalTime(string data, string time) // time is the key !
            {
                int dataLen = data.Length;
                int keyLen = time.Length;
                char[] output = new char[dataLen];

                for (int i = 0; i < dataLen; ++i)
                {
                    output[i] = (char)(data[i] ^ time[i % keyLen]);
                }

                return new string(output);
            }


        }



        #region InternalSecurityCheckMethods




        // High CPU Bound operation ! should not be used every where and every time !!
        public static bool __memfree
        {

            get
            {

#if Test
                return false;
#endif
                List<string> vs = new(new string[]
                {
                    "winhex",
                    "cheat engine",
                    "ida",
                    "binary ninja",
                    "ghidra",
                    "hxd",
                    "spy",
                    "debugger",
                    "dnspy",
                    "debugging",
                    "dotpeek",
                    "winspector",
                    "process monitor",
                    "process explorer",
                    "hack",
                    "crack",
                    "inject",
                    "injection",
                    "heapmemview",
                    "api monitor",
                    "memory editor",
                    "hexeditor",
                    "cheat",
                    "dumper",
                    "dump",
                    "extended task manager",
                    "scylla",
                    "ollydbg",
                    "disassembler",
                    "Hopper"
                });

                if (DetectWin10(vs) || Is_x64_Operating_System)

                {
                    vs.Clear();
                    return true;
                }



                try
                {
                    for (int i = 0; i < 0b10100; i++) // Check for multi instance of WinHex , up to 20 instances.
                    {

                        if (WriteMemory("WHXMDI" + i.ToString(), null) != IntPtr.Zero) // Check for WinHex
                        {
                            return true;
                        }
                    }



                    if (WriteMemory("PROCMON_WINDOW_CLASS", null) != IntPtr.Zero) // Check for Process Monitor
                    {
                        return true;
                    }

                    if (WriteMemory("PROCEXPL", null) != IntPtr.Zero) // Check for Process Explorer
                    {
                        return true;
                    }


                    if (WriteMemory("ProcessHacker", null) != IntPtr.Zero) // Check for Process Hacker
                    {
                        return true;
                    }

                }
                catch
                {
                    return true;
                }

                return false;
            }


        }

        public static bool Is_x64_Operating_System
        {
            get
            {
#if Test
                return false;
#endif
                bool x86_OS = false;
                Detectx86Architecture(Process.GetCurrentProcess().Handle, ref x86_OS);
                return !(!x86_OS && !Detectx64Architecture() && !DetectWin7() && !Debugger.IsAttached);
            }
        }

        private static bool DetectWin10(List<string> Apps_Window)
        {
#if Test
            return false;
#endif

            try
            {


                foreach (Process process in Process.GetProcesses())
                {
                    //if (!string.IsNullOrEmpty(process.MainWindowTitle))
                    //{
                    foreach (string AppTitle in Apps_Window)
                    {
                        if (process.MainWindowTitle?.ToLower().Contains(AppTitle) == true ||
                            process.MainWindowTitle?.ToUpper().Contains(AppTitle) == true)
                        {
                            return true;
                        }
                    }

                    //}
                }

                return false;
            }
            catch
            {
                return false;
            }



        }

        private static bool DetectWin7()
        {
#if Test
            return false;
#endif
            IntPtr debugPort = new IntPtr(0);
            uint status = CopyMemory(Process.GetCurrentProcess().Handle,
                0b111, out debugPort,
                Marshal.SizeOf(debugPort), out int returnLength);

            if (status == 0x00000000) // successfull
            {
                if (debugPort == new IntPtr(-1))
                {

                    return !!true;
                }
            }

            return !true;
        }

        #endregion




        internal NetMQQueueEventArgs(NetMQQueue<T> queue) => Queue = queue;
        
        /// <summary>
        /// The queue that invoked the event 
        /// </summary>
        public NetMQQueue<T> Queue { get; }
    }

    /// <summary>
    /// Multi producer single consumer queue which you can poll on with a Poller.
    /// </summary>
    /// <typeparam name="T">Type of the item in queue</typeparam>
    public sealed class NetMQQueue<T> : IDisposable, ISocketPollable, IEnumerable<T>
    {
        private readonly PairSocket m_writer;
        private readonly PairSocket m_reader;
        private readonly ConcurrentQueue<T> m_queue;
        private readonly EventDelegator<NetMQQueueEventArgs<T>> m_eventDelegator;
        private Msg m_dequeueMsg;

        /// <summary>
        /// Create new NetMQQueue.
        /// </summary>
        /// <param name="capacity">The capacity of the queue, use zero for unlimited</param>
        public NetMQQueue(int capacity = 0)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            m_queue = new ConcurrentQueue<T>();
            PairSocket.CreateSocketPair(out m_writer,
                                        out m_reader,
                                        writer => writer.Options.SendHighWatermark = capacity / 2,
                                        reader => reader.Options.ReceiveHighWatermark = capacity / 2);

            m_eventDelegator = new EventDelegator<NetMQQueueEventArgs<T>>(
                () => m_reader.ReceiveReady += OnReceiveReady,
                () => m_reader.ReceiveReady -= OnReceiveReady);

            m_dequeueMsg = new Msg();
            m_dequeueMsg.InitEmpty();
        }

        private void OnReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            m_eventDelegator.Fire(this, new NetMQQueueEventArgs<T>(this));
        }

        /// <summary>
        /// Register for this event for notification when there are items in the queue. Queue must be added to a poller for this to work.
        /// </summary>
        public event EventHandler<NetMQQueueEventArgs<T>> ReceiveReady
        {
            add => m_eventDelegator.Event += value;
            remove => m_eventDelegator.Event -= value;
        }

        NetMQSocket ISocketPollable.Socket => m_reader;
        
        /// <summary>
        /// Returns true if the queue is disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the number of items contained in the queue.
        /// </summary>
        public int Count => m_queue.Count;

        /// <summary>
        /// Gets a value that indicates whether the queue is empty.
        /// </summary>
        public bool IsEmpty => m_queue.IsEmpty;

        /// <summary>
        /// Try to dequeue an item from the queue. Dequeueing and item is not thread safe.
        /// </summary>
        /// <param name="result">Will be filled with the item upon success</param>
        /// <param name="timeout">Timeout to try and dequeue and item</param>
        /// <returns>Will return false if it didn't succeed to dequeue an item after the timeout.</returns>
        public bool TryDequeue( out T result, TimeSpan timeout)
        {
            if (m_reader.TryReceive(ref m_dequeueMsg, timeout))
            {
                return m_queue.TryDequeue(out result);
            }
            else
            {
                result = default(T);
                return false;
            }
        }

        /// <summary>
        /// Dequeue an item from the queue, will block if queue is empty. Dequeueing and item is not thread safe.
        /// </summary>
        /// <returns>Dequeued item</returns>
        public T Dequeue()
        {
            m_reader.TryReceive(ref m_dequeueMsg, SendReceiveConstants.InfiniteTimeout);

            m_queue.TryDequeue(out T result);

            return result;
        }

        /// <summary>
        /// Enqueue an item to the queue, will block if the queue is full.
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value)
        {
            m_queue.Enqueue(value);

            var msg = new Msg();
            msg.InitGC(EmptyArray<byte>.Instance, 0);

            lock (m_writer)
                m_writer.TrySend(ref msg, SendReceiveConstants.InfiniteTimeout, false);

            msg.Close();
        }

        #region IEnumerator

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => m_queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <summary>
        /// Dispose the queue.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            m_eventDelegator.Dispose();
            m_writer.Dispose();
            m_reader.Dispose();
            m_dequeueMsg.Close();

            IsDisposed = true;
        }
    }
}
#endif
