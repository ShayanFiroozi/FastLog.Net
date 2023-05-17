using System.IO;
using System.Threading;

namespace FastLog.Helpers
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


        public static short GetFileSize(string fileName)
        {

            try
            {
                // Set Status to Locked
                _readWriteLock.EnterReadLock();

                return string.IsNullOrWhiteSpace(fileName) ? (short)0 : !File.Exists(fileName) ? (short)0 :
                (short)(new FileInfo(fileName).Length / 1024 / 1024);

            }
            catch
            {
                return 0;
            }

            finally
            {
                // Release lock
                _readWriteLock.ExitReadLock();
            }
        }

    }
}
