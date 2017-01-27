using System;
using System.IO;

namespace DICOMopener
{
    public class Logger
    {
        private string LogFile { get; set; }

        /// <summary>
        /// Constructor. Creates a new log file
        /// </summary>
        /// <param name="FileName">The name of a file to write the log</param>
        public Logger(string FileName)
        {
            LogFile = FileName;
            using (StreamWriter outputFile = new StreamWriter(LogFile))
            {
                outputFile.Write("");
            }
        }

        /// <summary>
        /// Writes string to the log file
        /// </summary>
        /// <param name="log">String to be written to the log file</param>
        public void WriteLog(string log)
        {
            using (StreamWriter outputFile = File.AppendText(LogFile))
            {
                outputFile.Write(log);
            }
        }
    }
}
