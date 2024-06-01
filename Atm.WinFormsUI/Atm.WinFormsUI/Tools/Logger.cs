using Atm.WinFormsUI.Entities.LogTypes;
using Atm.WinFormsUI.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.Tools
{
    public static class Logger
    {
        public static void Log(Log.LogTypes logType,string logMessage)
        {
            try
            {
                LogManager logManager = new LogManager();
                Log log = new Log();
                log.LogTime = DateTime.Now;
                log.LogType = logType.ToString();
                log.LogMessage = logMessage;

                logManager.AddLog(log);
            }
            catch (Exception exception)
            {
                StreamWriter writer = File.CreateText(@"LoggingFail.txt");
                writer.Write(exception.ToString());
                writer.Dispose();
            }
            
        }
    }
}
