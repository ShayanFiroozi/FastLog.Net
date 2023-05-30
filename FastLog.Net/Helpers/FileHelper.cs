/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System.IO;



namespace FastLog.Helpers
{
    internal static class FileHelper
    {

        public static short GetFileSizeMB(string fileName)
        {

            try
            {


                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return (short)0;
                }
                else
                {
                    return !File.Exists(fileName) ? (short)0 :
                (short)(new FileInfo(fileName).Length / 1024 / 1024);
                }

            }
            catch
            {
                return 0;
            }


        }

    }
}
