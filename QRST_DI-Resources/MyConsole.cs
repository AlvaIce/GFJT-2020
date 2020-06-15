using System;
using System.IO;

namespace QRST_DI_Resources
{
    public class MyConsole
    {
        private static object lockobj = new object();
        public static void WriteLine(string str)
        {
            lock (lockobj)
            {
                string logdirpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logdirpath))
                {
                    Directory.CreateDirectory(logdirpath);
                }
                string logpath = Path.Combine(new string[] { logdirpath, DateTime.Now.ToString("yyyy-MM-dd") + ".log" });
                File.AppendAllText(logpath, string.Format("{0}\r\n", str));
                Console.WriteLine(str);
            }
        }
    }
}
