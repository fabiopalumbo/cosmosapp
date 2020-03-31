using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ComosBRWeb
{
    internal static class LogHandler
    {
        internal static string logfile = "..\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".ComosBRWeb.log";

        internal static void WriteLog(string message, EventLogEntryType eventType = EventLogEntryType.Warning)
        {
            System.IO.File.AppendAllText(logfile,eventType.ToString() + ": " + message + "\r\n");
        }
    }
}
