﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace LogModule.FileLogger
{
    public class FileLogger : IFileLogger
    {


        #region Properties


        public readonly byte LOG_FILE_MAX_SIZE_IN_MB = 0;

        public string LogFileName { get; }
        public string LogFilePath { get; }

        public string LogFileFullPath => LogFilePath + "\\" + LogFileName;



        public int LogFileSizeMB
        {
            get
            {
                try
                {
                    return Convert.ToInt32((new FileInfo(LogFileFullPath).Length / 1024) / 1024);
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
                if (LogFileSizeMB
                     <= LOG_FILE_MAX_SIZE_IN_MB)
                {
                    return;
                }
            }
            catch
            {
                throw;
            }


            if (!File.Exists(LogFileFullPath))
            {
                return;
            }

            try
            {
                File.Delete(LogFileFullPath);
            }
            catch
            {
                throw;
            }
        }


        public async Task DeleteLogFileTaskAsync()
        {
            await Task.Run(() => DeleteLogFile());
        }


        public void SaveLog(LogMessage logMessage)
        {

            if (logMessage == null)
            {
                throw new ArgumentNullException("logMessage parameter can not be null.");
            }

            try
            {
                using (StreamWriter streamWriter = new(LogFileFullPath, append: true))
                {
                    streamWriter.WriteLine(logMessage.GetLogMessage());
                   
                }
            }
            catch
            {

                throw;
            }
        }


        public async Task SaveLogTaskAsync(LogMessage logMessage)
        {
            await Task.Run(()=>SaveLog(logMessage));
        }

        #endregion



    }
}
