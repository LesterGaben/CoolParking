using CoolParking.BL.Interfaces;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace CoolParking.BL.Services
{
    public class LogService : ILogService
    {
        readonly string logFilePath;
        public string LogPath=> logFilePath;
        public LogService(string logPath= @".\Transactions.log")
        {
            logFilePath = logPath;
        }
        public string Read()
        {
            string log;
            using (var file = new StreamReader(logFilePath))
            {
                 log = file.ReadToEnd();
            }
            return log;
        }
        public void Write(string logInfo)
        {
            using (var file = new StreamWriter(logFilePath, true))
            {
                file.WriteLine(logInfo);
            }
        }
    }
}