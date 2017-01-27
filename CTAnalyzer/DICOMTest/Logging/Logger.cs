using System;
using System.IO;

namespace DICOMopener
{
    public class Logger
    {
        private string LogFile { get; set; }

        public Logger(string FileName)
        {
            LogFile = FileName;
            using (StreamWriter outputFile = new StreamWriter(LogFile))
            {
                outputFile.Write("");
            }
        }

        public void WriteLog(string log)
        {
            using (StreamWriter outputFile = File.AppendText(LogFile))
            {
                outputFile.Write(log);
            }
        }
    }
}
