using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_MS_Basis.HostService
{
    public class HostTerminalManager
    {
        private static List<HostTerminal> hosts = new List<HostTerminal>();
        public static List<HostTerminal> optimalHostServers = new List<HostTerminal>();
        //连接数据库进行数据库操作
        static IDbBaseUtilities dbserver = Constant.IdbServerUtilities;
        //生成新的code
        static IDbOperating dbcode = Constant.IdbOperating;
        /// <summary>
        /// 站点队列
        /// </summary>
        public static List<HostTerminal> HostServers
        {
            get
            {
                return hosts;
            }
        }

        static string tablename = "HOSTTERMINALSINFO";
        /// <summary>
        /// 新增站点，插入到数据库siteinfo
        /// </summary>
        public static void AddHostServer(string name,string desc, string ipadress, string tcpPort)
        {

            string newqrstcode = dbcode.GetNewQrstCodeFromTableName(tablename, EnumDBType.MIDB);
            dbserver.ExecuteSql(String.Format("insert into {0} (NAME,DESCRIPTION,IPADDRESS,TCPPORT,QRST_CODE,REGDATE) values ('{1}','{2}','{3}','{4}','{5}','{6}')", tablename, name, desc, ipadress, tcpPort, newqrstcode, DateTime.Now));
            //Create StorageSite Object
            //StorageSite newsite = new StorageSite(tablecode, ipadress, sourcePath, tilePath, productPath, Port, FTPUsername, Password, Maxband);
            HostTerminal newhost = new HostTerminal(newqrstcode, name, desc, ipadress, tcpPort, DateTime.Now, DateTime.Now);
            HostServers.Add(newhost);
        }

        /// <summary>
        /// 更新站点配置
        /// </summary>        
        /// <returns>如果站点不存在，不予更新，并返回False</returns>
        public static bool UpdateHostSite(string code, string name,string desc, string ipadress, string tcpPort)
        {
            //查找列表中的目标站点，如果不存在则返回
            //如果存在更新相关属性
            int isNull = dbserver.GetRecordCount(String.Format("select * from {0} where QRST_CODE ='{1}'", tablename, code));
            if (isNull != 0)
            {
                dbserver.ExecuteSql(String.Format("update {0} set IPADDRESS ='{1}',NAME = '{2}',DESCRIPTION='{3}',TCPPORT='{4}',REGDATE='{5}',LASTTIME='{5}'where QRST_CODE='{6}'", tablename, ipadress, name, desc, tcpPort, DateTime.Now, code));
            }
            else
            {
                return false;
                //AddHostServer(name, desc, ipadress, tcpPort);
            }
            return true;
        }

        /// <summary>
        /// 更新站点配置
        /// </summary>        
        /// <returns>如果站点不存在，不予更新，并返回False</returns>
        public static bool UpdateHostSiteLastTime(HostTerminal host)
        {
            //查找列表中的目标站点，如果不存在则返回
            //如果存在更新相关属性
            //IDataBaseUtilities dbserver = new MySqlBaseUtilities();
            int isNull = dbserver.GetRecordCount(String.Format("select * from {0} where QRST_CODE ='{1}'", tablename, host.Code));
            if (isNull != 0)
            {
                dbserver.ExecuteSql(String.Format("update {0} set LASTTIME='{1}'where QRST_CODE='{2}'", tablename, DateTime.Now, host.Code));
            }
            else
            {
                return false;
                //AddHostServer(name, desc, ipadress, tcpPort);
            }
            return true;
        }
        /// <summary>
        /// 初始化站点信息
        /// </summary>
        public static void UpdateHostServerList()
        { 
            //读取数据库站点信息
            //IDataBaseUtilities newHostInfo = new MySqlBaseUtilities();
            int num = dbserver.GetRecordCount("select * from " + tablename );
            if(num ==0)return;
            DataSet HostInfo = dbserver.GetDataSet("select QRST_CODE,NAME,DESCRIPTION,IPADDRESS,TCPPORT,REGDATE,LASTTIME from " + tablename);
            DataTable hostTable = HostInfo.Tables[0];

            string[] codelist = new string[num];
            //重新构建storagesites列表
            for (int ii = 0; ii < num;ii++ )
            {
                bool isExist = false;

                foreach (HostTerminal host in HostServers)
                {
                    //相同code 的站点
                    if (host.Code.Trim() == hostTable.Rows[ii][0].ToString().Trim())
                    {
                        isExist = true;
                            //更新已有站点
                            host.Code = hostTable.Rows[ii][0].ToString().Trim();
                            host.Name = hostTable.Rows[ii][1].ToString().Trim();
                            host.Description = hostTable.Rows[ii][2].ToString().Trim();
                            host.IPAdress = hostTable.Rows[ii][3].ToString().Trim();
                            host.TCPPort=hostTable.Rows[ii][4].ToString().Trim();
                        break;
                    }
                }
               
                if (!isExist)
                {
                    
                    //新建新增站点
                    HostTerminal tempHost = new HostTerminal(hostTable.Rows[ii][0].ToString(), hostTable.Rows[ii][1].ToString(), hostTable.Rows[ii][2].ToString(), hostTable.Rows[ii][3].ToString(), hostTable.Rows[ii][4].ToString(), DateTime.Parse(hostTable.Rows[ii][5].ToString()), DateTime.Parse(hostTable.Rows[ii][6].ToString()));
                        
                    //StorageSite tempSite = new StorageSite(siteTable.Rows[ii][0].ToString(), siteTable.Rows[ii][1].ToString(), siteTable.Rows[ii][2].ToString(),
                        //siteTable.Rows[ii][3].ToString(), siteTable.Rows[ii][4].ToString(), siteTable.Rows[ii][5].ToString(), siteTable.Rows[ii][6].ToString(),
                        //siteTable.Rows[ii][7].ToString(), Convert.ToDouble(siteTable.Rows[ii][8]));
                   
                    HostServers.Add(tempHost);
                }

                codelist[ii] = hostTable.Rows[ii][0].ToString().Trim();
            }

            //删除不存在节点
            foreach (HostTerminal site in HostServers)
            {
                if (!codelist.Contains(site.Code.Trim()))
                {
                    //节点订单迁移（待增加 JOKI）

                    //移除节点
                    HostServers.Remove(site);
                }
            }
            
        }

        public static void UpdateAllHostServersResponseTime()
        {
            UpdateHostServerList();

            foreach (HostTerminal host in HostServers)
            {
                Thread testThread = new Thread(new ParameterizedThreadStart(UpdateRspTime));
                testThread.Start(host);
            }
        }

        private static void UpdateRspTime(object obj_site)
        {
            HostTerminal host = obj_site as HostTerminal;
            host.ResponesTime = TestHostServerRunning(host);
        }

        public static double TestHostServerRunning(HostTerminal site)
        {
            return TestHostServerRunning(site, true,long.Parse(Constant.SSResponseThreshold));
        }

        /// <summary>
        /// 测试站点响应时间
        /// </summary>
        /// <param name="site">站点</param>
        /// <param name="usingDefaultThreshold">是否使用默认超时阈值</param>
        /// <param name="userThreshold">自定义超时阈值。如果usingDefaultThreshold=true，可为任意值。</param>
        /// <returns>如果为-1则无响应或超时</returns>
        public static double TestHostServerRunning(HostTerminal site, bool usingDefaultThreshold, long userThreshold)
        {
            DateTime start = DateTime.Now;
            long threshold = 0;
            double costtime = -1;
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

            if(costtime!=-1&&!(costtime > threshold))
            {
                UpdateHostSiteLastTime(site);
            }
            
            return (costtime > threshold) ? -1 : costtime;

        }

        /// <summary>
        /// 刷新响应时间在阈值范围之内的目前最优站点列表
        /// </summary>
        public static void UpdateOptimalHostList()
        {
            //初始化站点
            UpdateHostServerList();

            optimalHostServers.Clear();

            //foreach (StorageSite item in StorageSites)
            //{
            //    //删除相应慢（低于阈值）的节点
            //    System.Threading.Tasks.Task.Factory.StartNew(new Action<object>(TestRunning), item);
            //}
            ////等待站点响应，时间为阈值时差
            //System.Threading.Thread.Sleep(int.Parse(Constant.SSResponseThreshold));

            List<Thread> testTasks = new List<Thread>();

            foreach (HostTerminal item in HostServers)
            {                
                Thread testThread = new Thread(new ParameterizedThreadStart(TestRunning));
                testThread.Start(item);
                testTasks.Add(testThread);
            }

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
            HostTerminal site = obj_site as HostTerminal;
            if (site.IsRunning && !optimalHostServers.Contains(site))
            {
                optimalHostServers.Add(site);

                UpdateHostSiteLastTime(site);
            }
        }      
                                  
        /// <summary>
        /// 从站点Code中获取站点对象
        /// </summary>
        /// <param name="siteCode">站点Code</param>
        /// <returns></returns>
        public static HostTerminal getSiteFromSiteCode(string siteCode)
        {
            HostTerminal existSite = null;

            foreach (HostTerminal site in HostServers)
            {
                if (site.Code.Trim() == siteCode)
                {

                    existSite = site;

                }
            }
            return existSite;
        }

        /// <summary>
        /// 从站点IP中获取站点对象
        /// </summary>
        /// <param name="IP">站点IP</param>
        /// <returns></returns>
        public static HostTerminal getSiteFromSiteIP(string IP)
        {
            HostTerminal existSite = null;

            foreach (HostTerminal site in HostServers)
            {
                if (site.IPAdress == IP)
                {

                    existSite = site;

                }
            }
            return existSite;
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

    }
}