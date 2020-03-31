using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BRComos.IO
{
    internal class Log
    {
        internal static readonly string logfile = "../Log/"+DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")+".comos.io.log";
        internal static void WriteLog(string message, EventLogEntryType eventType)
        {
            System.IO.File.AppendAllText(logfile,eventType.ToString() + ": " + message + "\r\n");
        }
        internal static void WriteLog(string message)
        {
            System.IO.File.AppendAllText(logfile, EventLogEntryType.Information.ToString() + ": " + message + "\r\n");
        }
        internal static void WriteLog(string message, params object[] args)
        {
            System.IO.File.AppendAllText(logfile, 
                EventLogEntryType.Information.ToString() + ": " + string.Format(message,args)+"\r\n");
        }
    }
}
