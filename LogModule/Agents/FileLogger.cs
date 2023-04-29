using System;
using System.IO;
using System.Threading;

namespace LogModule.Agents
{
    public class FileLogger : IFileLogger
    {


        private readonly int LOG_FILE_MAX_SIZE_IN_MB = 0;

        private readonly string _LogFile;


        #region Properties


        public string LogFile => _LogFile;


        public int LogFileSizeMB
        {
            get
            {
                try
                {
                    return Convert.ToInt32((new FileInfo(LogFile).Length / 1024) / 1024);
                }
                catch (Exception ex)
                {
                    InnerException.InnerException.LogInnerException(ex);
                    return 0;
                }
            }
        }



        #endregion


        #region Constructors


        public FileLogger(string LogFile,
                          int LOG_FILE_MAX_SIZE_IN_MB = 100)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(LogFile))
                {
                    throw new ArgumentNullException("Invalid logging path or file name");
                }


                _LogFile = LogFile;
                this.LOG_FILE_MAX_SIZE_IN_MB = LOG_FILE_MAX_SIZE_IN_MB;


                // Create the log file directory

                Directory.CreateDirectory(new FileInfo(this.LogFile).Directory.FullName);



                // Delete the log file if it's bigger than LOG_FILE_MAX_SIZE_IN_MB

                DeleteLogFile();


            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        #endregion



        #region Methods


        public void DeleteLogFile()
        {
            try
            {

                if (!File.Exists(LogFile))
                {
                    return;
                }


                if (LogFileSizeMB
                     <= LOG_FILE_MAX_SIZE_IN_MB)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }




            try
            {
                File.Delete(LogFile);

                SaveLog(new LogMessage(LogMessage.LogTypeEnum.INFO,
                                       "The Log file has been deleted.",
                                       $"Reached the maximum file size ({LOG_FILE_MAX_SIZE_IN_MB:N0} MB)"), true);

            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }



        public void SaveLog(LogMessage logMessage, bool threadSafeWrite = true)
        {

            try
            {

                if (logMessage == null)
                {
                    throw new ArgumentNullException("logMessage parameter can not be null.");
                }

                if (threadSafeWrite)
                {
                    WriteToFileThreadSafe(LogFile, logMessage.GetLogMessage().ToString());
                }
                else
                {
                    using (StreamWriter streamWriter = new(LogFile, append: true))
                    {
                        streamWriter.WriteLine(logMessage.GetLogMessage());
                    }
                }

            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        #endregion


        private static readonly ReaderWriterLockSlim _readWriteLock = new();

        private void WriteToFileThreadSafe(string path, string text)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }



    }
}
