/*---------------------------------------------------------------------------------------------

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
        internal static ReaderWriterLockSlim Lock { get; private set; }

        static SlimReadWriteLock()
        {
            Lock = new ReaderWriterLockSlim();
        }
    }
}
