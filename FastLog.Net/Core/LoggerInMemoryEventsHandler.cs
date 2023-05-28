/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Helpers;
using FastLog.Interfaces;
using System;

namespace FastLog.Core
{

    public sealed partial class Logger : IDisposable
    {


        /// <summary>
        /// Check if the number of In Memory Event(s) are greater than "MaxEventsToKeep" property value.
        /// Note : Use "ReaderWriterLockSlim" to lock the object when modifying and making this method Thread-Safe.
        /// Warning : If the count of in-memory-events reached the "MaxEventsToKeep" , this method will drop the oldet event in the list and then add new one.
        /// </summary>
        /// <param name="logEvent">Log Event to store inthe In-Memory-Event list.</param>
        private void HandleInMemoryEvents(ILogEventModel logEvent)
        {

            if (Configuration.MaxEventsToKeep == 0) return;

            try
            {

                // Grab the lock
                SlimReadWriteLock.Lock.EnterWriteLock();

                if (Configuration.MaxEventsToKeep == 0 && inMemoryEvents.Count != 0)
                {

                    inMemoryEvents.Clear();

                    return;
                }


                if (inMemoryEvents.Count >= Configuration.MaxEventsToKeep)
                {
                    inMemoryEvents.RemoveAt(0); // Remove the oldest event.

                }

                inMemoryEvents.Add(logEvent);

            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }
            finally
            {
                // Release the lock
                SlimReadWriteLock.Lock.ExitWriteLock();
            }

        }


    }
}
