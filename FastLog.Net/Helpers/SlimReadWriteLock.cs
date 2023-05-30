﻿/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System.Threading;



namespace FastLog.Helpers
{
    internal static class SlimReadWriteLock
    {
        internal readonly static ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();


        internal static void GainReadLock() => Lock.EnterReadLock();
        internal static void GainWriteLock() => Lock.EnterWriteLock();


        internal static void ReleaseReadLock() => Lock.ExitReadLock();
        internal static void RelaseWriteLock() => Lock.ExitWriteLock();

    }
}
