using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_MS_Desktop.HadoopInfo
{
    public class ModelJobStatus
    {
        public Apps apps { set; get; }
    }
    public class Apps
    {
        public List<App> app { set; get; }
    }
    public class App
    {
        public string id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string finalStatus { get; set; }
        public double progress { get; set; }
        public ulong startedTime { get; set; }
        public ulong finishedTime { get; set; }
        public ulong elapsedTime { get; set; }
    }
}
