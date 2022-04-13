using System;
using System.IO;

namespace LogModule.FileLogger
{
    public class FileLogger : IFileLogger
    {


        #region Properties


        public readonly byte LOG_FILE_MAX_SIZE_IN_MB = 0;

        public string LogFileName { get; }
        public string LogFilePath { get; }

        public string LogFileFullPath => this.LogFilePath + "\\" + this.LogFileName;



        public int LogFileSizeMB
        {
            get
            {
                try
                {
                    return Convert.ToInt32((new FileInfo(this.LogFileFullPath).Length / 1024) / 1024);
                }
                catch
                {
                    throw;
                }
            }
        }




        #endregion


        #region Constructors


        public FileLogger(string LogFilePath,
                          string LogFileName,
                          byte LOG_FILE_MAX_SIZE_IN_MB = 20)
        {

            if (string.IsNullOrWhiteSpace(LogFilePath) ||
               string.IsNullOrWhiteSpace(LogFileName))
            {
                throw new ArgumentNullException("Invalid logging path or file name");
            }

            this.LogFilePath = LogFilePath;
            this.LogFileName = LogFileName;
            this.LOG_FILE_MAX_SIZE_IN_MB = LOG_FILE_MAX_SIZE_IN_MB;


            try
            {
                Directory.CreateDirectory(this.LogFilePath);
            }
            catch
            {

                throw;
            }


        }


        #endregion





        #region Methods


        public void DeleteLogFile()
        {
            try
            {
                if (this.LogFileSizeMB
                     <= this.LOG_FILE_MAX_SIZE_IN_MB)
                {
                    return;
                }
            }
            catch
            {
                throw;
            }


            if (!File.Exists(this.LogFileFullPath)) return;



            try
            {
                File.Delete(this.LogFileFullPath);
            }
            catch
            {
                throw;
            }
        }


        public void SaveLog(LogMessage logMessage)
        {

            if (logMessage == null)
            {
                throw new ArgumentNullException("logMessage parameter can not be null.");
            }

            try
            {
                using (StreamWriter streamWriter = new(this.LogFileFullPath, append: true))
                {
                    streamWriter.WriteLine(logMessage.GetLogMessage());
                   
                }
            }
            catch
            {

                throw;
            }
        }


        #endregion



    }
}
