using System;

namespace QRST_DI_MS_Basis.HostService
{
    public class DataSourceHost
    {
        public DataSourceHost(HostTerminal host)
        {
            hostserver = host;
            UpdateResponseTime();
            Initialize();
        }

        public void Initialize()
        {
            Name = hostserver.Name;
            Code = hostserver.Code;
            IP = hostserver.IPAdress;
            Desc = hostserver.Description;
            TcpPort = hostserver.TCPPort;
            RegDate = hostserver.Regdate;
            LastActiveTime = hostserver.LastWorkingTime;
            Response = hostserver.ResponesTime;

        }

        public HostTerminal hostserver = null;

        public string Name { get; set; }
        public string Code { get; set; }
        public string IP { get; set; }
        public string Desc { get; set; }
        public string TcpPort { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime LastActiveTime { get; set; }
        public double Response { get; set; }

        public void UpdateResponseTime()
        {
            hostserver.UpdateResponseTime();
            Initialize();
        }
    }
}
