using System;
using System.Runtime.Remoting.Channels;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Channels.Tcp;

namespace QRST_DI_MS_Basis.HostService
{
    public class HostTerminal
    {
        
        public event EventHandler ResponseTimeUpdated;
        public HostTerminal(string _Code, string name, string description, string _IPAdress, string tcpPort, DateTime regdate, DateTime lasttime)
        {
            this.TCPService = InitTCPServiceClient(_IPAdress, tcpPort);
            this.Code = _Code;
            this.IPAdress = _IPAdress;
            this.TCPPort = tcpPort;
            this.Name = name;
            this.Description = description;
            this.Regdate = regdate;
            this.LastWorkingTime = lasttime;
            this.ResponesTime = -1;
        }

        /// <summary>
        /// 初始化中心站点TCP远程通信客户端
        /// </summary>
        private HostTerminalTCPService InitTCPServiceClient(string _IPAdress, string tcpPort)
        {
            if (ChannelServices.GetChannel("CenterTCP") == null)
            {
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                IDictionary props = new Hashtable();
                props["port"] = tcpPort;
                props["name"] = "CenterTCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                TcpClientChannel chan = new TcpClientChannel(
                props, clientProvider);

                ChannelServices.RegisterChannel(chan);
            }

            object tcpObject = System.Activator.GetObject(
            typeof(HostTerminalTCPService),
            string.Format("tcp://{0}:{1}/QDB_Console_TCP", _IPAdress, tcpPort));

            return tcpObject as HostTerminalTCPService;
       }



        # region 属性

        public HostTerminalTCPService TCPService { get; set; }
        public DateTime Regdate { get; set; }
        public DateTime LastWorkingTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #region 站点状态

        /// <summary>
        /// 是否运行
        /// </summary>

        public bool IsRunning
        {
            get
            {
                return GetRunStatus();
            }
        }

        double rsptime = -1;
        public double ResponesTime
        {
            get {
                return rsptime;
            }
            set
            {
                rsptime = value;
                if (ResponseTimeUpdated != null)
                {
                    ResponseTimeUpdated(this, EventArgs.Empty);
                }
            }
                
        }

        /// <summary>
        /// 判断远程机是否运行
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool GetRunStatus()
        {
            bool isrunning = false;
            try
            {
                isrunning = TCPService.IsRunning;
            }
            catch
            {
                isrunning = false;
            }
            return isrunning;
        }
        #endregion

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAdress { get; set; }
        public string TCPPort { get; set; }
        public string Code { get; set; }
        #endregion

        public void UpdateResponseTime()
        {
            ResponesTime = HostTerminalManager.TestHostServerRunning(this);
        }
    }
}
