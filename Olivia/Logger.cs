using System;
using System.IO;

namespace Olivia
{
    public static class Logger
    {
        private static readonly string _logoutput = "MyFileOutput.txt";
        
        public static void AppendLine(string s)
        {
            File.AppendAllText(_logoutput, s);
            File.AppendAllText(_logoutput, Environment.NewLine);
        }

        public static Stream GetLogFile()
        {
            return File.OpenRead(_logoutput);
        }
    }
}