using System;
using System.Collections.Generic;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Orders;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_DBClient.TCP;
using QRST_DI_SS_DBInterfaces.IDBService;
 

namespace QRST_DI_TS_Process.Site
{
    [Serializable]
    public class TServerSite
    {
        public int _CurrentPaiallelTasks = 0;
        public string SharePath { get { return string.Format(@"\\{0}\{1}\", this.IPAdress, StorageBasePath.QRST_DB_Common); } }
        public string SourcePath = "";
        public string SiteName = "";
        //站点运行状态，添加日期：2013/1/14  zxw
        public EnumSiteStatus RunningState = EnumSiteStatus.Stopped;

        public TServerSite(string _Code, string _IPAdress,string _sourcePath,string sitename = "")
        {
            string tcpPort = Constant.dbUtilityTcpPort;
            if (tcpPort == "0")
            {
                throw new Exception("未获取TCP端口");
            }

            this.TCPService = InitTCPClient_StorageChl(_IPAdress, tcpPort);
            this.Code = _Code;
            this.IPAdress = _IPAdress;
            this._Status = new TServerSiteStatus(this);
            this.SourcePath = _sourcePath;
            this.SiteName = sitename;
            this.ProcessingOrders = new List<OrderClass>();
        }

        private ITCPService InitTCPClient_StorageChl(string _IPAdress, string tcpPort)
        {
            return TileSiteTCPServiceClient.InitTCPClient_StorageChl(_IPAdress, tcpPort);

        }    
        


        # region 属性
        /// <summary>
        /// 任务（订单）队列
        /// </summary>
        public List<OrderClass> ProcessingOrders
        {
            get;
            set;
        }

        public ITCPService TCPService;


        private TServerSiteStatus _Status;
        /// <summary>
        /// 站点状态
        /// </summary>
        public TServerSiteStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        private string _IPAdress;
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAdress
        {
            get
            {
                return _IPAdress;
            }
            set
            {
                _IPAdress = value;
            }
        }
        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
            }
        }

        public string Version
        { get; set; }

        #endregion 公共方法#endregion 公共方法
        public string ToString()
        {
            return string.Format("Site:{0}", this.IPAdress);
        }
    }
}
