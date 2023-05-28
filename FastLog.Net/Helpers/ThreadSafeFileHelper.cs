/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System.IO;



namespace FastLog.Helpers
{

    //#Refactor : In codes below we used "ReaderWriterLockSlim" for thread-safety file access , BUT !
    // Of course there is better solution to achive that.

    //Known Issue : "ReaderWriterLockSlim" is thread-safe but could not be used in async method and also is not process safe.
    // See accepted answer of this thread for more info :
    // https://stackoverflow.com/questions/19659387/readerwriterlockslim-and-async-await


    /// <summary>
    /// Thread-Safe File Access ( read and write ) with "ReaderWriterLockSlim" class.
    /// Note : "ReaderWriterLockSlim" is thread-safe but is NOT process safe.
    /// </summary>
    internal static class ThreadSafeFileHelper
    {



        public static void AppendAllText(string path, string content)
        {
            // Set Status to Locked
            SlimReadWriteLock.Lock.EnterWriteLock();

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
                SlimReadWriteLock.Lock.ExitWriteLock();
            }

        }



        public static void DeleteFile(string path)
        {
            // Set Status to Locked
            SlimReadWriteLock.Lock.EnterWriteLock();

            try
            {
                File.Delete(path);
            }
            finally
            {
                // Release lock
                SlimReadWriteLock.Lock.ExitWriteLock();
            }

        }


        public static short GetFileSize(string fileName)
        {

            try
            {
                // Set Status to Locked
                SlimReadWriteLock.Lock.EnterReadLock();

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
                SlimReadWriteLock.Lock.ExitReadLock();
            }
        }

    }
}
