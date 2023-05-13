using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FastLog.Net.Helpers
{
    internal static class ThreadSafeFileHelper
    {
        private static readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public static void AppendAllText(string path, string content)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();

            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }

        }


        public static void DeleteFile(string path)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();

            try
            {
             File.Delete(path);
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }

        }


    }
}
