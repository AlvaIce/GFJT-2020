using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRST_DI_MS_Desktop.HadoopInfo
{
    public class ModelNodesMemory
    {
        public Nodes nodes { get; set; }
    }
    public class Nodes
    {
        public List<Node> node { get; set; }
    }
    public class Node
    {
        public string nodeHostName { get; set; }
        public double usedMemoryMB { get; set; }
        public double availMemoryMB { get; set; }
    }
}
