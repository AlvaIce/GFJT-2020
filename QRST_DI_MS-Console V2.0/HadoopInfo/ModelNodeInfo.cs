using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_MS_Desktop.HadoopInfo
{
    public class ModelNodeInfo
    {
        public List<Beans> beans { get; set; }
    }

    public class Beans
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public double Total { get; set; }
        public double Used { get; set; }
        public double Free { get; set; }
        public double NonDfsUsedSpace { get; set; }
        public double PercentRemaining { get; set; }
        public string LiveNodes { get; set; }
    }

    public class DataNode
    {
        public string name { get; set; }
        public string xferaddr { get; set; }
        public double usedSpace { get; set; }
        public double nonDfsUsedSpace { get; set; }
        public double capacity { get; set; }
        public double remaining { get; set; }
    }

}
