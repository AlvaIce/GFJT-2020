using System;
using QRST_DI_SS_Basis;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_TS_Process.Service;
 
namespace QRST_DI_TS_Process.Site
{
    [Serializable]
    public class TServerSiteStatus
    {

        public double maxband = 10240;      //KB为单位
        readonly ping pin = null;
        public string diskname;
        public string Path;
        public TServerSite ParentSite;
        private ITCPService tcpservice;

        public TServerSiteStatus(TServerSite parentSite)
        {
            this.ParentSite = parentSite;

            pin = new ping();
            tcpservice = parentSite.TCPService;
        }
        /// <summary>
        /// 是否运行
        /// </summary>

        internal bool IsRunning
        {
            get
            {
                //修改日期：2013/1/29 zxw
                try
                {
                    return tcpservice.IsRunning;
                }
                catch(Exception ex)
                {
                    return false;
                }
              //  return IsRunning;
            }
        }

        public string CPUVersion
        {
            get
            {
                return tcpservice.GetCPUVresionInfor();
            }
        }
        /// <summary>
        /// 获取CPU利用率
        /// </summary>
        public double CPU
        {
            get
            {
                return tcpservice.CpuLoad;
            }
        }
        /// <summary>
        /// 获取磁盘存储空间
        /// diskname为磁盘标识
        /// </summary>
        public double GetDivToatalsize()
        {
            return tcpservice.GetDivTotalsize(ParentSite.SourcePath.Substring(0, 1));
            //string IPAdress = ParentSite.IPAdress;
            //string url = String.Format("http://{0}/WS_HJDATABASE/WS_StorageSite/WebService.asmx", IPAdress);
            //Path = path;
            //object obj5 = WebServiceUtil.InvokeWebservice(url, "xiaobie", "WebService", "Getdivtotalsize", new object[] { Path });
            //double DivAvalableFreeSpace = (double)obj5;
            //return DivAvalableFreeSpace;

        }
        /// <summary>
        /// 获取磁盘已用存储空间
        /// diskname为磁盘标识
        /// </summary>
        public double GetDivLoadSize()
        {
            return tcpservice.GetDivLoadSize(ParentSite.SourcePath.Substring(0, 1));
            //string IPAdress = ParentSite.IPAdress;
            //string url = String.Format("http://{0}/WS_HJDATABASE/WS_StorageSite/WebService.asmx", IPAdress);
            //Path = path;
            //object obj5 = WebServiceUtil.InvokeWebservice(url, "xiaobie", "WebService", "GetdivAvalableFreeSpace", new object[] { Path });
            //double DivAvalableFreeSpace = (double)obj5;
            //return DivAvalableFreeSpace;
        }
        /// <summary>
        /// 获取磁盘可用存储空间
        /// diskname为磁盘标识
        /// </summary>
        public double GetDivAvalableFreeSpace()
        {
            return tcpservice.GetDivAvalableFreeSpace(ParentSite.SourcePath.Substring(0,1));
            //string IPAdress = ParentSite.IPAdress;
            //string url = String.Format("http://{0}/WS_HJDATABASE/WS_StorageSite/WebService.asmx", IPAdress);
            //Path = path;
            //object obj5 = WebServiceUtil.InvokeWebservice(url, "xiaobie", "WebService", "GetdivAvalableFreeSpace", new object[] { Path });
            //double DivAvalableFreeSpace = (double)obj5;
            //return DivAvalableFreeSpace;
        }
        /// <summary>
        /// 获取全部物理内存
        /// </summary>
        public long TotalPhysicalMemory
        {
            get
            {
                return tcpservice.GetTotalPhysicalMemory();

           }

        }
        ///// <summary>
        /////获取网络速度
        ///// </summary>
        //public string InterNetSpeed
        //{
        //    get
        //    {
        //        return WSstorage.GetIntSpeed();
        //    }
        //}
        /// <returns></returns>
        /// <summary>
        ///获取可用物理内存
        /// </summary>
        /// <returns></returns>
        public long AvailPhys
        {
            get
            {
                return tcpservice.GetAvailablePhysicalMemory();
            }
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        public string IPAddress
        {
            get
            {

                return tcpservice.GetIPAddress();

            }
        }
        /// <summary>
        /// 获取主机名
        /// </summary>
        /// <returns></returns>
        public string ComputerName
        {
            get
            {
                return tcpservice.GetComputerName();
            }
        }
        /// <summary>
        /// 获取ping值返回的时间,字符串
        /// </summary>
        /// <returns></returns>
        public string PingtimeString
        {
            get
            {
                return pin.PingHostStr(ParentSite.IPAdress);
            }
        }
        /// <summary>
        /// 获取ping值返回的时间
        /// </summary>
        /// <returns></returns>
        public Double PingtimeDouble
        {
            get
            {
                return pin.PingHostInt(ParentSite.IPAdress);
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
                isrunning = tcpservice.IsRunning;
            }
            catch 
            {
                isrunning = false;
            }
            return isrunning;
        }

        /// <summary>
        /// 判断远程服务运行状态
        /// </summary>
        /// <returns>EnumSiteStatus 运行 挂起 异常 停止</returns>
        private EnumSiteStatus GetServerStatus()
        {
            return tcpservice.GetServerStatus();
        }

    }
}
