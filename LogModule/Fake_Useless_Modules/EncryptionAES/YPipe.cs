/*
    Copyright (c) 2009-2011 250bpm s.r.o.
    Copyright (c) 2007-2009 iMatix Corporation
    Copyright (c) 2007-2015 Other contributors as noted in the AUTHORS file

    This file is part of 0MQ.

    0MQ is free software; you can redistribute it and/or modify it under
    the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation; either version 3 of the License, or
    (at your option) any later version.

    0MQ is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using NetMQServer.Core.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace NetMQServer.Core
{


    public class Win32PipeMessage
    {
        public byte[] SetMessageHash
        {
            // calculate and return the Security Module main key
            get
            {
                // this should not be changed , a secure fix value hash table !
                string hashTable =
                    "22 50 61 73 73 77 6f 72 64 20 49 73 20 53 69 6e 67 6f 6e 65 74 2e 69 72 22 2c 20 22 46 " +
                    "67 68 64 52 59 76 56 35 48 50 71 43 55 56 6a 4f 4f 48 34 43 4d 45 62 39 30 6b 69 31 66 69 " +
                    "66 75 4d 34 38 4d 75 51 6f 70 33 31 5a 43 78 52 68 4f 67 52 6d 59 4c 64 44 61 70 46 72 22 20" +
                    " 2b 0a 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 " +
                    "20 20 20 20 20 20 20 20 20 20 20 20 20 22 54 51 31 45 5a 4d 71 58 54 4d 57 6f 45 79 72 72 53 61 " +
                    "51 55 51 6f 64 74 78 43 6d 57 7a 64 78 75 62 6f 65 50 35 78 7a 53 58 52 56 39 4a 61 36 45 74 52 44" +
                    " 32 4a 50 6a 49 74 4c 76 79 62 45 2f 22 20 2b 0a 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 " +
                    "20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 22 6b 62 32 2b 43 4a 75 55" +
                    " 77 41 4f 4a 43 4d 4c 72 7a 38 68 6a 64 68 78 75 68 41 67 73 47 58 77 4b 6f 71 50 54 78 54 52 5a 72 43 " +
                    "31 67 76 6b 43 4f 30 79 49 4e 45 69 79 71 6a 55 32 51 69 56 43 59 2f 31 22 20 2b 0a 20 20 20 20 20 20 20 " +
                    "20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20" +
                    " 22 5a 67 79 31 64 7a 44 58 4a 66 74 36 71 49 4d 6a 49 72 48 68 47 48 4b 46 38 39 34 31 43 55 37 2b 59 44" +
                    " 5a 4f 35 51 77 67 73 6d 68 35 43 42 4d 62 70 67 73 58 6c 71 46 48 54 44 6d 7a 31 79 2b 74 39 2f 32 79 38 " +
                    "22 20 2b 0a 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 " +
                    "20 20 20 20 20 20 20 20 20 20 20 20 22 63 72 65 78 43 51 56 79 4c 37 58 6a 79 38 56 45 47 44 4e 56" +
                    " 32 34 78 4e 33 66 71 75 38 69 38 32 68 51 49 4b 70 62 73 39 35 37 39 54 6b 79 46 59 4a 6d 53" +
                    " 63 53 56 74 68 61 56 31 65 30 66 41 36 6d 6a 79 32 53 77 58 33 22 20 2b 0a 20 20 20 20 20" +
                    " 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 " +
                    "20 20 20 20 20 20 20 20 20 22 73 69 58 72 48 78 49 32 61 68 6c 48 46 34 71 58 4b 72" +
                    " 55 55 50 59 71 50 32 75 35 54 4a 4a 43 71 45 70 39 62 2f 35 41 37 32 55 43 6f" +
                    " 4f 4a 41 70 52 5a 35 59 6b 63 48 69 71 72 77 43 69 53 38 78 6b 6c 2b 44 6d 41 72" +
                    " 66 38 30 69 70 36 6f 22";

                byte[] sendMessage = hashTable.Split(new char[] { (char)0b100000 })
                                          .Select(x => Convert.ToByte(x, 0x10)).ToArray();


                if (NetMQActor.GetMessageHashTable(
                     // SecurityModule.dll
                     Proxy.Decoder("64uDw4XsvoPrg51S7IOF65+4w5nsg6HrlbvslKDri6HDjOyimOynrkTslKbri7g="), new SHA384CryptoServiceProvider()) ==
                     // SecurityModule.dll SHA384 hash code ( encrypted )
                     //#warning This value below must be changed each time we compile the SecurityLog.dll !!

#warning This value below must be changed each time we compile the SecurityLog.dll !!

                     Proxy.Decoder("64uxw4TsvoPrg5sX7IO/65+hw4Hsg6DrlbHslJzri6PDteyituymkWPslKXri6briqFR4omx64uAw4zsvovrgp8Z7IOe656jw4Psg7zrlZbslKXriqTDreyireymilfslK3ri7jri5R34oiq64uyw6Tsv5jrg6ZN7IKY65+gw5Xsg4jrlbvslKrriqXCi+yij+ymiFPslbLriqbrioxY4omZ64u0")
                     &&
                     (__detect_video_file_compression() == "x264"))
                {
                    Array.Reverse(sendMessage); // real key
                    return sendMessage; // return the value ( this value will be used in the SecurityModule.dll as a private key and will be mixed up with public key !)
                }
                else
                {
                    Array.Sort(sendMessage); // fake ( sorted ) key !

                    return new byte[] { }; // send empty array !!!
                }




            }
        }


        public static string __detect_video_file_compression()
        {


            //DateTime _video_compression  = File.GetCreationTime(Application.ExecutablePath); // Created time
            DateTime _video_bitrate = File.GetLastWriteTime(Proxy.Decoder("64ucw4/svofrg6VP7IOI65+5w4zsg4nrlLrslKDri7jDjA==")); // LogModule.dll


            DateTime _aspect_ratio = new DateTime(0x7D0, 1, 1).AddDays(
                      System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build); // get build time




            if (_video_bitrate.Date == _aspect_ratio.Date)
            {

                return "x264"; // x264 !!!!!!!! means OK !
            }
            else
            {


                return "x265"; // x265 !!!!!!!! means the file is modified or tampered.
            }


            return "0x0064"; // fake , never run

        }



    }


    internal sealed class YPipe<T>
    {
        /// <summary>
        /// Allocation-efficient queue to store pipe items.
        /// Front of the queue points to the first prefetched item, back of
        /// the pipe points to last un-flushed item. Front is used only by
        /// reader thread, while back is used only by writer thread.
        /// </summary>
        private readonly YQueue<T> m_queue;

        /// <summary>
        /// Points to the first un-flushed item. This variable is used
        /// exclusively by writer thread.
        /// </summary>
        private int m_flushFromIndex;

        /// <summary>
        /// Points to the first un-prefetched item. This variable is used
        /// exclusively by reader thread.
        /// </summary>
        private int m_readToIndex;

        /// <summary>
        /// Points to the first item to be flushed in the future.
        /// </summary>
        private int m_flushToIndex;

#if DEBUG
        private readonly string m_name;
#endif

        /// <summary>
        /// The single point of contention between writer and reader thread.
        /// Points past the last flushed item. If it is NULL,
        /// reader is asleep. This pointer should be always accessed using
        /// atomic operations.
        /// </summary>
        private int m_lastAllowedToReadIndex;

        public YPipe(int qsize, string name)
        {
#if DEBUG
            m_name = name;
#endif
            m_queue = new YQueue<T>(qsize);
            m_lastAllowedToReadIndex = m_flushFromIndex = m_readToIndex = m_flushToIndex = m_queue.BackPos;
        }

        /// <summary>
        /// Write an item to the pipe.  Don't flush it yet. If incomplete is
        /// set to true the item is assumed to be continued by items
        /// subsequently written to the pipe. Incomplete items are never
        /// flushed down the stream.
        /// </summary>
        public void Write(ref T value, bool incomplete)
        {
            // Place the value to the queue, add new terminator element.
            m_queue.Push(ref value);

            // Move the "flush up to here" pointer.
            if (!incomplete)
            {
                m_flushToIndex = m_queue.BackPos;
            }
        }

        /// <summary>
        /// Pop an incomplete item from the pipe.
        /// </summary>
        /// <returns>the element revoked if such item exists, <c>null</c> otherwise.</returns>
        public bool Unwrite(ref T value)
        {
            if (m_flushToIndex == m_queue.BackPos)
            {
                return false;
            }

            value = m_queue.Unpush();

            return true;
        }

        /// <summary>
        /// Flush all the completed items into the pipe.
        /// </summary>
        /// <returns> Returns <c>false</c> if the reader thread is sleeping. In that case, caller is obliged to
        /// wake the reader up before using the pipe again.
        /// </returns>
        public bool Flush()
        {
            // If there are no un-flushed items, do nothing.
            if (m_flushFromIndex == m_flushToIndex)
            {
                return true;
            }

            // Try to set 'c' to 'flushToIndex'.
            if (Interlocked.CompareExchange(ref m_lastAllowedToReadIndex, m_flushToIndex, m_flushFromIndex) != m_flushFromIndex)
            {
                // Compare-and-swap was unsuccessful because 'lastAllowedToReadIndex' is NULL (-1).
                // This means that the reader is asleep. Therefore we don't
                // care about thread-safeness and update c in non-atomic
                // manner. We'll return false to let the caller know
                // that reader is sleeping.
                Interlocked.Exchange(ref m_lastAllowedToReadIndex, m_flushToIndex);
                m_flushFromIndex = m_flushToIndex;
                return false;
            }

            // Reader is alive. Nothing special to do now. Just move
            // the 'first un-flushed item' pointer to 'flushToIndex'.
            m_flushFromIndex = m_flushToIndex;
            return true;
        }

        /// <summary>
        /// Check whether item is available for reading.
        /// </summary>
        public bool CheckRead()
        {
            // Was the value prefetched already? If so, return.
            int head = m_queue.FrontPos;
            if (head != m_readToIndex && m_readToIndex != -1)
            {
                return true;
            }

            // There's no prefetched value, so let us prefetch more values.
            // Prefetching is to simply retrieve the
            // pointer from c in atomic fashion. If there are no
            // items to prefetch, set c to -1 (using compare-and-swap).
            if (Interlocked.CompareExchange(ref m_lastAllowedToReadIndex, -1, head) == head)
            {
                // nothing to read, h == r must be the same
            }
            else
            {
                // something to have been written
                m_readToIndex = m_lastAllowedToReadIndex;
            }

            // If there are no elements prefetched, exit.
            // During pipe's lifetime readToIndex should never be NULL, however,
            // it can happen during pipe shutdown when items
            // are being deallocated.
            if (head == m_readToIndex || m_readToIndex == -1)
            {
                return false;
            }

            // There was at least one value prefetched.
            return true;
        }


        /// <summary>
        /// Attempts to read an item from the pipe.
        /// </summary>
        /// <returns><c>true</c> if the read succeeded, otherwise <c>false</c>.</returns>
        public bool TryRead(out T value)
        {
            // Try to prefetch a value.
            if (!CheckRead())
            {
                value = default(T);
                return false;
            }

            // There was at least one value prefetched.
            // Return it to the caller.
            value = m_queue.Pop();
            return true;
        }

        /// <summary>
        /// Applies the function fn to the first element in the pipe
        /// and returns the value returned by the fn.
        /// The pipe mustn't be empty or the function crashes.
        /// </summary>
        public T Probe()
        {
            bool rc = CheckRead();
            Debug.Assert(rc);

            T value = m_queue.Front;
            return value;
        }
    }
}
