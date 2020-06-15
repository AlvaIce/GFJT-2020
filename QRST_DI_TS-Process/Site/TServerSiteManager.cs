using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBClient.TCP;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using System.IO;

namespace QRST_DI_TS_Process.Site
{
    public class TServerSiteManager
    {
        static DateTime lastupdateOpt = DateTime.MinValue;
        static bool optimalSiteListAutoUpdated = false;
        static Timer autoupdatetimer = null;

        public static void StartAutoUpdateOptimalStorageSiteList()
        {
            if (!optimalSiteListAutoUpdated)
            {
                //每隔1分钟更新一次站点列表
                autoupdatetimer = new Timer(new TimerCallback(autoUpdate), null, 0, 60000);
                optimalSiteListAutoUpdated = true;
            }
        }

        static void autoUpdate(object sender)
        {
            UpdateOptimalStorageSiteList();
        }
        private static List<TServerSite> sites = new List<TServerSite>();
        public static List<TServerSite> _optimalStorageSites = new List<TServerSite>();
        private static IDbOperating _sqLiteOperating = Constant.IdbOperating;

        private static IDbBaseUtilities _SqLiteBaseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
        public static List<TServerSite> optimalStorageSites
        {
            get
            {
                if ((DateTime.Now - lastupdateOpt).Minutes > 5)
                {
                    UpdateOptimalStorageSiteList();
                }
                return _optimalStorageSites;
            }
        }
        public static List<string> optimalStorageSiteIPs
        {
            get
            {
                List<string> ips = new List<string>();
                foreach (TServerSite site in optimalStorageSites)
                {
                    ips.Add(site.IPAdress);
                }
                return ips;
            }
        }
        /// <summary>
        /// 站点队列
        /// </summary>
        public static List<TServerSite> StorageSites
        {
            get
            {
                return sites;
            }
        }

        static string tablename = "tileserversitesinfo";
        /// <summary>
        /// 新增站点，插入到数据库siteinfo
        /// </summary>
        /// <param name="ipadress">IP地址</param>
        /// <param name="sourcePath">原始数据路径</param>
        /// <param name="tilePath">切片数据路径</param>
        /// <param name="productPath">产品数据路径</param>
        /// <param name="Port">用户站点</param>
        /// <param name="FTPUsername">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="Maxband">最大带宽</param>
        public static void AddStorageSite(string ipadress, string sourcePath, string tilePath, string productPath, string Port, string FTPUsername, string Password, double Maxband, string version)
        {
            //连接数据库进行数据库操作
            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            ////生成新的code
            //IDBOperating dbcode = new DBMySqlOperating();
            string newreccode = _sqLiteOperating.GetNewQrstCodeFromTableName(tablename, EnumDBType.MIDB);
            _SqLiteBaseUtilities.ExecuteSql(
                String.Format(
                    "insert into {0} (ADDRESSIP,VERSION,SOURCEPATH,TILEPATH,PRODUCTPATH,PORT,FTPUSERNAME,PASSWORD,QRST_CODE,MAXBAND) values ('{1}','{10}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                    tablename, ipadress, sourcePath, tilePath, productPath, Port, FTPUsername, Password, newreccode,
                    Maxband, version));
            //Create StorageSite Object
            //StorageSite newsite = new StorageSite(tablecode, ipadress, sourcePath, tilePath, productPath, Port, FTPUsername, Password, Maxband);
            TServerSite newsite = new TServerSite(newreccode, ipadress, sourcePath);
            StorageSites.Add(newsite);
        }

        /// <summary>
        /// 更新站点配置
        /// </summary>
        /// <param name="code">站点编码</param>
        /// <param name="ipadress">IP地址</param>
        /// <param name="sourcePath">原始数据路径</param>
        /// <param name="tilePath">切片数据路径</param>
        /// <param name="productPath">产品数据路径</param>
        /// <param name="Port">用户站点</param>
        /// <param name="FTPUsername">用户名</param>
        /// <param name="Password">密码</param>
        /// <returns>如果站点不存在，不予更新，并返回False</returns>
        public static void UpdateStorageSite(string code, string ipadress,string version, string sourcePath, string tilePath, string productPath, string Port, string FTPUsername, string Password, double Maxband)
        {
            //查找列表中的目标站点，如果不存在则返回
            //如果存在更新相关属性
            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            int isNull = _SqLiteBaseUtilities.GetRecordCount(String.Format("select * from {0} where QRST_CODE ='{1}'", tablename, code));
            if (isNull != 0)
            {
                _SqLiteBaseUtilities.ExecuteSql(String.Format("update {0} set ADDRESSIP ='{1}',VERSION ='{10}',SOURCEPATH = '{2}',TILEPATH='{3}',PRODUCTPATH='{4}',PORT='{5}',FTPUSERNAME='{6}',PASSWORD='{7}',MAXBAND='{8}'where QRST_CODE='{9}'", tablename, ipadress, sourcePath, tilePath, productPath, Port, FTPUsername, Password, Maxband, code, version));
            }
            else
            {
                AddStorageSite(ipadress, sourcePath, tilePath, productPath, Port, FTPUsername, Password, Maxband, version);
            }
        }
        /// <summary>
        /// 根据IP创建站点
        /// </summary>
        public static TServerSite CreateTSSiteByIP(string ip)
        {
            TServerSite newSite = null;
            //QRST_DI_DS_Basis.DBEngine.DBMySqlOperating mySQLOpr = new DBMySqlOperating();
            string sql = string.Format("select QRST_CODE,SOURCEPATH,RunningState from {0} where ADDRESSIP = '{1}'", tablename, ip);
            DataSet siteinfo = _SqLiteBaseUtilities.GetDataSet(sql);
            if (siteinfo.Tables.Count > 0 && siteinfo.Tables[0].Rows.Count > 0)
            {
                DataTable siteTable = siteinfo.Tables[0];
                string sitecode = siteTable.Rows[0][0].ToString();
                string sourcePath = siteTable.Rows[0][1].ToString();
                newSite = new TServerSite(sitecode, ip, sourcePath);
                Enum.TryParse(siteTable.Rows[0]["RunningState"].ToString(),out newSite.RunningState);
          
            }
            return newSite;
        }
        
        /// <summary>
        /// 站点IP转站点Code
        /// </summary>
        public static string ConvertTSSiteIP2TSSiteCode(string ip)
        {
            string siteCode = "-1";
            //QRST_DI_DS_Basis.DBEngine.DBMySqlOperating mySQLOpr = new DBMySqlOperating();
            string sql = string.Format("select QRST_CODE from {0} where ADDRESSIP in ('{1}','127.0.0.1')", tablename, ip);
            DataSet siteinfo = _SqLiteBaseUtilities.GetDataSet(sql);
            if (siteinfo.Tables.Count > 0 && siteinfo.Tables[0].Rows.Count > 0)
            {
                DataTable siteTable = siteinfo.Tables[0];
                siteCode = siteTable.Rows[0][0].ToString();
            }
            return siteCode;
        }

        /// <summary>
        /// 站点Code转站点IP
        /// </summary>
        public static string ConvertTSSiteCode2TSSiteIP(string code)
        {
            string ip = null;
            //QRST_DI_DS_Basis.DBEngine.DBMySqlOperating mySQLOpr = new DBMySqlOperating();
            string sql = string.Format("select ADDRESSIP from {0} where QRST_CODE = '{1}'", tablename, code);
            DataSet siteinfo = _SqLiteBaseUtilities.GetDataSet(sql);
            if (siteinfo.Tables.Count > 0 && siteinfo.Tables[0].Rows.Count > 0)
            {
                DataTable siteTable = siteinfo.Tables[0];
                ip = siteTable.Rows[0][0].ToString();
            }
            if (ip != null)
            {
                DeleteDir("\\\\" + ip + "\\QRST_DB_Share");
            }
            return ip;
        }

        /// <summary>
        /// 删除文件夹内子目录
        /// </summary>
        /// <param name="dirRoot"></param>
        private static void DeleteDir(string dirRoot)
        {
            DirectoryInfo dir = new DirectoryInfo(dirRoot);
            if (dir.Exists)
            {
                DirectoryInfo[] childs = dir.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    DateTime now = DateTime.Now;
                    DateTime createTime = child.CreationTime;
                    System.TimeSpan span = now - createTime;
                    if (span.TotalHours > 1)
                    {
                        child.Delete(true);
                    }
                }
            }
        }
        /// <summary>
        /// 初始化站点信息
        /// </summary>
        public static void UpdateStorageSiteList()
        { 
            //读取数据库站点信息
            //IDataBaseUtilities newSiteInfo = new MySqlBaseUtilities();
            int num = _SqLiteBaseUtilities.GetRecordCount("select * from " + tablename );
            if(num ==0)return;
            DataSet SiteInfo =
                _SqLiteBaseUtilities.GetDataSet(
                    "select QRST_CODE,ADDRESSIP,SOURCEPATH,TILEPATH,PRODUCTPATH,PORT,FTPUSERNAME,PASSWORD,MAXBAND,NAME,RunningState,VERSION from " +
                    tablename);

            DataTable siteTable = SiteInfo.Tables[0];

            string[] codelist = new string[num];
            //重新构建storagesites列表
            for (int ii = 0; ii < num;ii++ )
            {
                bool isExist = false;

                foreach (TServerSite site in StorageSites)
                {
                    //相同code 的站点
                    if (site.Code.Trim() == siteTable.Rows[ii][0].ToString().Trim())
                    {
                        isExist = true;
                        if (siteTable.Rows[ii][2].ToString().Trim().Length > 0)
                        {
                            //更新已有站点
                            site.Code = siteTable.Rows[ii][0].ToString().Trim();
                            site.IPAdress = siteTable.Rows[ii][1].ToString().Trim();
                            site.SourcePath = siteTable.Rows[ii][2].ToString().Trim();
                            site.Version = siteTable.Rows[ii]["VERSION"].ToString().Trim();
                            site.SiteName = siteTable.Rows[ii]["NAME"].ToString().Trim();
                            Enum.TryParse(siteTable.Rows[ii]["RunningState"].ToString().Trim(), out site.RunningState);
                        }
                        else
                        {
                            //如果节点未设置共享目录 移除节点
                            StorageSites.Remove(site);
                        }
                        break;
                    }
                }
               
                if (!isExist && siteTable.Rows[ii][2].ToString().Trim().Length > 0)
                {
                    
                    //新建新增站点
                    TServerSite tempSite = new TServerSite(siteTable.Rows[ii][0].ToString(), siteTable.Rows[ii][1].ToString(), siteTable.Rows[ii][2].ToString(), siteTable.Rows[ii]["NAME"].ToString());
                        
                    //StorageSite tempSite = new StorageSite(siteTable.Rows[ii][0].ToString(), siteTable.Rows[ii][1].ToString(), siteTable.Rows[ii][2].ToString(),
                        //siteTable.Rows[ii][3].ToString(), siteTable.Rows[ii][4].ToString(), siteTable.Rows[ii][5].ToString(), siteTable.Rows[ii][6].ToString(),
                        //siteTable.Rows[ii][7].ToString(), Convert.ToDouble(siteTable.Rows[ii][8]));
                    Enum.TryParse(siteTable.Rows[ii]["RunningState"].ToString().Trim(), out tempSite.RunningState);
                    StorageSites.Add(tempSite);
                }

                codelist[ii] = siteTable.Rows[ii][0].ToString().Trim();
            }

            //删除不存在节点,zxw 2013/1/15
            //for (int i = 0 ; i < StorageSites.Count ; i++)
            //{
            //    if (!codelist.Contains(StorageSites[i].Code.Trim()))
            //    {
            //        StorageSites.Remove(StorageSites[i]);
            //    }
            //}
            foreach (TServerSite site in StorageSites)
            {
                if (!codelist.Contains(site.Code.Trim()))
                {
                    //节点订单迁移（待增加 JOKI）

                    //移除节点
                    StorageSites.Remove(site);
                }
            }
            
        }

        public static double TestStorageSiteRunning(TServerSite site)
        {
            return TestStorageSiteRunning(site, true, long.Parse(Constant.SSResponseThreshold));
        }

        /// <summary>
        /// 测试站点响应时间
        /// </summary>
        /// <param name="site">站点</param>
        /// <param name="usingDefaultThreshold">是否使用默认超时阈值</param>
        /// <param name="userThreshold">自定义超时阈值。如果usingDefaultThreshold=true，可为任意值。</param>
        /// <returns>如果为-1则无响应或超时</returns>
        public static double TestStorageSiteRunning(TServerSite site, bool usingDefaultThreshold, long userThreshold)
        {
            DateTime start = DateTime.Now;
            long threshold = 0;
            double costtime = -1;
            if (site==null)
            {
                return costtime;
            }
            if (usingDefaultThreshold)
            {
                threshold = int.Parse(Constant.SSResponseThreshold);
                Thread testThread = new Thread(new ParameterizedThreadStart(TestRunning));
                testThread.Start(site);
                Thread moniterThread = new Thread(new ParameterizedThreadStart(MonitorTestRunning));
                moniterThread.Start(testThread);
                while (testThread.ThreadState== ThreadState.Running)
                {
                    Thread.Sleep(5);
                }
            }
            else
            {
                threshold = userThreshold;
                Thread testThread = new Thread(new ParameterizedThreadStart(TestRunning));
                testThread.Start(site);
                Thread moniterThread = new Thread(new ParameterizedThreadStart(MonitorTestRunning));
                moniterThread.Start(testThread);
                while (testThread.ThreadState== ThreadState.Running)
                {
                    Thread.Sleep(5);
                    costtime = (double)(DateTime.Now - start).Seconds * 1000 + (double)(DateTime.Now - start).Milliseconds;
                    if (costtime > threshold)
                    {
                        break;
                    }
                }
            }

            costtime = (double)(DateTime.Now - start).Seconds * 1000 + (double)(DateTime.Now - start).Milliseconds;
            return (costtime > threshold) ? -1 : costtime;

        }

        static bool isUpdating = false;
        /// <summary>
        /// 刷新响应时间在阈值范围之内的目前最优站点列表
        /// </summary>
        public static void UpdateOptimalStorageSiteList()
        {
            if (!isUpdating)
            {
                isUpdating = true;

            //初始化站点
            UpdateStorageSiteList();

            _optimalStorageSites.Clear();

            //foreach (StorageSite item in StorageSites)
            //{
            //    //删除相应慢（低于阈值）的节点
            //    System.Threading.Tasks.Task.Factory.StartNew(new Action<object>(TestRunning), item);
            //}
            ////等待站点响应，时间为阈值时差
            //System.Threading.Thread.Sleep(int.Parse(Constant.SSResponseThreshold));

            List<Thread> testTasks = new List<Thread>();

            foreach (TServerSite item in StorageSites)
            {
                
                Thread testThread = new Thread(new ParameterizedThreadStart(TestRunning));
                testThread.Start(item);
                testTasks.Add(testThread);
            }
            Thread.Sleep(2800);     //避免节点运行测试未完成就执行下一步，未添加到_optimalStorageSites
            //删除相应慢（低于阈值）的节点
            Thread moniterThread = new Thread(new ParameterizedThreadStart(MonitorTestRunning));
            moniterThread.Start(testTasks);
            foreach (Thread testThread in testTasks)
            {
                while (testThread.ThreadState== ThreadState.Running)
                {
                    Thread.Sleep(5);
                }
            }
            lastupdateOpt = DateTime.Now;
                isUpdating = false;
            }
            else
            {
                //多线程调用，当已在执行更新时，其他线程的请求变为等待而不是重复更新。
                while (isUpdating)
                {
                    Thread.Sleep(50);
                }
            }
        }

        private static void MonitorTestRunning(object obj_testtask)
        {
            List<Thread> testTasks = new List<Thread>(); ;
            if (obj_testtask is List<Thread>)
            {
                testTasks.AddRange(obj_testtask as List<Thread>);
            }
            else if (obj_testtask is Thread)
            {
                testTasks.Add(obj_testtask as Thread);
            }

            System.Threading.Thread.Sleep(int.Parse(Constant.SSResponseThreshold));
            foreach (Thread testtask in testTasks)
            {
                if (testtask.ThreadState== ThreadState.Running)
                {
                    testtask.Abort();
                }
            }
        }      

        private static void TestRunning(object obj_site)
        {
            TServerSite site = obj_site as TServerSite;
            if (site.Status.IsRunning && !_optimalStorageSites.Contains(site))
            {
                _optimalStorageSites.Add(site);
            }
        }      
                                  
        /// <summary>
        /// 获取最优站点
        /// </summary>
        public static TServerSite GetOptimalStorageSite()
        {
            TServerSite theOptimalSite = null;
            int lestTasks = int.MaxValue;
            
            //如果已存在数据 返回已存在数据站点

            foreach (TServerSite persite in optimalStorageSites)
            {
                //检查可用空间 （略）
                if (persite.ProcessingOrders.Count<lestTasks)
                {
                    lestTasks = persite.ProcessingOrders.Count;
                    theOptimalSite = persite;
                }

            }
            return theOptimalSite;
        }

        /// <summary>
        /// 从站点Code中获取站点对象
        /// </summary>
        /// <param name="siteCode">站点Code</param>
        /// <returns></returns>
        public static TServerSite getSiteFromSiteCode(string siteCode)
        {
            TServerSite existSite = null;

            foreach (TServerSite site in StorageSites)
            {
                if (site.Code.Trim() == siteCode)
                {

                    existSite = site;

                }
            }
            return existSite;
        }

        /// <summary>
        /// 从站点Code中获取站点对象
        /// </summary>
        /// <param name="siteCode">站点Code</param>
        /// <returns></returns>
        public static TServerSite getSiteFromSiteIP(string IP)
        {
            TServerSite existSite = null;

            foreach (TServerSite site in StorageSites)
            {
                if (site.IPAdress == IP)
                {

                    existSite = site;
                    break;
                }
            }
            return existSite;
        }

        /// <summary>
        /// 获取所有站点IP
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllSiteIP()
        {
            List<string> ipStr = new List<string>();
            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            string sql = string.Format("select ADDRESSIP from {0}",tablename);
            DataSet ds = _SqLiteBaseUtilities.GetDataSet(sql);

            for (int i = 0 ; i < ds.Tables[0].Rows.Count ;i++ )
            {
                ipStr.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return ipStr;
        }



        static Dictionary<string, string> modToIP = new Dictionary<string, string>();  //记录VDS与IP的对应关系
        public static Dictionary<string, string> DicModToIP  //记录VDS与IP的对应关系
        {
            get
            {
                if (modToIP == null )
                {
                    modToIP = new Dictionary<string, string>();
                    List<string> ips = GetAllSiteIP();

                    foreach (string ip in ips)
                    {
                        List<string> mods = GetAllModByIP(ip);
                        foreach (string mod in mods)
                        {
                            if (!modToIP.ContainsKey(mod))
                            {
                                modToIP.Add(mod, ip);
                            }
                        }
                    }
                }
                return modToIP;
            }
        }


        /// <summary>
        /// 获取一个IP的所有MODID
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static List<string> GetAllModByIP(string ipAddress)
        {
            List<string> modLst = new List<string>();
            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            string sql = string.Format("select MODID from {0} where ADDRESSIP = '{1}'", tablename,ipAddress);
            DataSet ds = _SqLiteBaseUtilities.GetDataSet(sql);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                string [] modArr = ds.Tables[0].Rows[0][0].ToString().Split(',');
                for (int i = 0; i < modArr.Length; i++)
                {
                    if(!string.IsNullOrEmpty(modArr[i].Trim()))
                    {
                        modLst.Add(modArr[i]);
                    }
                }
                return modLst;
            }
        }
         
        /// <summary>
        /// 获取站点最大线程数
        /// hxz 150617
        /// </summary>
        /// <param name="siteIP"></param>
        /// <returns></returns>
        public static int GetSiteThreadsMaxCount(string siteIP)
        {
            try
            {
            //MySqlBaseUtilities dbserver = new MySqlBaseUtilities();
                int threadscount=0;
                //支持单节点下的IPV4的ip与“127.0.0.1”的兼容 @jh
                threadscount = Convert.ToInt32(_SqLiteBaseUtilities.GetDataSet(String.Format("select threads from tileserversitesinfo where  ADDRESSIP in ('{0}','127.0.0.1')", siteIP)).Tables[0].Rows[0][0]);
                //switch (Constant.DbStorage)   
                //{
                //    case EnumDbStorage.SINGLE:
                //        threadscount = Convert.ToInt32(_SqLiteBaseUtilities.GetDataSet(String.Format("select threads from tileserversitesinfo where  ADDRESSIP in ('{0}','127.0.0.1')", siteIP)).Tables[0].Rows[0][0]);

                //        break;
                //    case EnumDbStorage.MULTIPLE:
                //        threadscount = Convert.ToInt32(_SqLiteBaseUtilities.GetDataSet(String.Format("select threads from tileserversitesinfo where  ADDRESSIP  '{0}'", siteIP)).Tables[0].Rows[0][0]);
                //        break;
                //}
               
            return threadscount;

            }
            catch(Exception ex)
            {
                throw new Exception("获取站点最大线程数异常:",ex);
            }
        }
        /// <summary>
        /// 更改站点状态 
        /// 2013/1/14  zxw
        /// </summary>
        /// <param name="siteIP"></param>
        /// <param name="status"></param>
        public static void UpdateSiteRunningState(string siteIP,EnumSiteStatus status)
        {
            //查找列表中的目标站点，如果不存在则返回
            //如果存在更新相关属性
            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            IDbBaseUtilities dbserver = Constant.IdbServerUtilities;
            int isNull = dbserver.GetRecordCount(String.Format("select * from {0} where ADDRESSIP ='{1}'", tablename, siteIP));
            if (isNull != 0)
            {
                dbserver.ExecuteSql(String.Format("update {0} set RunningState ='{1}' where ADDRESSIP='{2}'", tablename,status.ToString(),siteIP));
            }
        }

        public static void UpdateStorageSiteVersion(string siteIP, string version)
        {
            //查找列表中的目标站点，如果不存在则返回
            //如果存在更新相关属性
            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            int isNull = _SqLiteBaseUtilities.GetRecordCount(String.Format("select * from {0} where ADDRESSIP ='{1}'", tablename, siteIP));
            if (isNull != 0)
            {
                _SqLiteBaseUtilities.ExecuteSql(String.Format("update {0} set VERSION ='{1}' where ADDRESSIP ='{2}'", tablename, version, siteIP));
            }
        }

        ////WYN
        ///// <summary>
        ///// 站点机没开时，文件存放的临时站点
        ///// </summary>
        ///// <returns>站点</returns>
        //public static StorageSite TempStorageSite()
        //{
        //    string ip = DBEngine.Constant.GetIP;
        //    string port = DBEngine.Constant.GetPort;
        //    string ftpUser = DBEngine.Constant.GetUserName;
        //    string ftpPassword = DBEngine.Constant.GetFtpPassword;
        //    string ftpTempPath = "TempSaveTilePath//";
        //    StorageSite newsite = new StorageSite("", ip, "", ftpTempPath, "", port, ftpUser, ftpPassword, 20480);
        //    return newsite;
        //}
        ///// <summary>
        ///// 站点机没开时，文件存放的临时站点
        ///// </summary>
        ///// <returns>站点</returns>
        //public static StorageSite CorrectStorageSite()
        //{
        //    string ip = DBEngine.Constant.GetIP;
        //    string port = DBEngine.Constant.GetPort;
        //    string ftpUser = DBEngine.Constant.GetUserName;
        //    string ftpPassword = DBEngine.Constant.GetFtpPassword;
        //    string ftpTempPath = "TransPath//";
        //    StorageSite newsite = new StorageSite("", ip, "", ftpTempPath, "", port, ftpUser, ftpPassword, 20480);
        //    return newsite;
        //}
        /////// <summary>
        /////// 订单迁移，目标订单从一台电脑移动到另外一台电脑(暂不做)
        /////// </summary>
        /////// <param name="itorder"></param>
        ////public void MoveOrderFromoneSitetoAnotherSite( InternalTransOrder itorder)
        ////{ 
        ////}
        /// <summary>
        /// 获取标记为中心的站点IP
        /// </summary>
        /// <returns></returns>
        public static string GetCenterSiteIP()
        {
            string CenterSiteIP = "";

            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            string sql = string.Format("select ADDRESSIP from {0} where ISCENTER = '1'", tablename);
            DataSet ds = _SqLiteBaseUtilities.GetDataSet(sql);

            if (ds != null && ds.Tables.Count != 0)
            {
                CenterSiteIP = ds.Tables[0].Rows[0][0].ToString();
            }
            return CenterSiteIP;
        }
    }
}
