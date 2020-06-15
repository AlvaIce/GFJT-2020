using System;
using QRST_DI_Resources;

namespace QRST_DI_MS_Basis.Log
{
    public class LogUtils
    {
        public event EventHandler OutputLogUpdated;
        public string orderCode = "";
        string outputLog = "";
        public string AllLogs
        { get; set; }
        public string OutputLog
        {
            get { return outputLog; }
            internal set
            {                
                outputLog = value;
                AllLogs = string.Format("{0}\r\n{1}", AllLogs, outputLog);
                if (OutputLogUpdated != null)
                {
                    OutputLogUpdated(null, new EventArgs());
                }
            }
        }
        
        public void ConsoleWriteOutLine(string info)
        {ConsoleWriteOutLine(info,false);}
        public void ConsoleWriteOutLine(string info, string orderCode)
        { ConsoleWriteOutLine(info, orderCode, false); }
        public void ConsoleWriteOutLine(string info,bool isException)
        {
            orderCode = "";
            if (isException)
            {
                MyConsole.WriteLine(string.Format("!!!异常{0}:{1}", DateTime.Now.ToString(), info));
                OutputLog = string.Format("!!!异常{0}:{1}\n\t", DateTime.Now.ToString(), info);
            }
            else
            {
                MyConsole.WriteLine(string.Format("***{0}:{1}", DateTime.Now.ToString(), info));
                OutputLog = string.Format("***{0}:{1}\n\t", DateTime.Now.ToString(), info);
            }
        }

        public void ConsoleWriteOutLine(string info, string code, bool isException)
        {
            orderCode = code;
            if (isException)
            {
                MyConsole.WriteLine(string.Format("!!!异常{0} 任务{1}:{2}", DateTime.Now.ToString(), code, info));
                OutputLog = string.Format("!!!异常{0} 任务{1}:{2}\n\t", DateTime.Now.ToString(), code, info);
            }
            else
            {
                MyConsole.WriteLine(string.Format("***{0} 任务{1}:{2}", DateTime.Now.ToString(), code, info));
                OutputLog = string.Format("***{0} 任务{1}:{2}\n\t", DateTime.Now.ToString(), code, info);
            }
        }
    }
}
