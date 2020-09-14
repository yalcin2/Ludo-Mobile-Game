using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Logger
    {
        public static void LogMessage(string user, string method, string message)
        {
            Trace.WriteLine("Log " + DateTime.Now + ", User: " + user +
                ", Method: " + method + ", Message: " + message);

            Trace.Flush();
            Trace.Close();
        }
    }
}
