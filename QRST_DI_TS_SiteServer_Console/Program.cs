//添加MySql.Data引用 joki 131129
/*
 *"1.3.0811.1653"添加执行输出 
 *"1.5.0618.1920"加TableLock和并行强化
 *"1.5.1029.0006"将瓦片JPG修改为PNG
 *"1.5.1101.1815"将瓦片库Level字段由int修改为char
 *"1.5.1116.1856"修改磁盘阵列数据满了,路径问题
 *"1.5.1117.1434"将瓦片检索Limit与Order by 顺序问题
 *"1.5.1117.1522"存在重复order，导致站点signorder失败的问题
 *"1.5.1125.1623"向tileserversitesinfo表中添加OrderSubTime、CurOrderCode字段
 *"1.6.1218.1741"海量快视图bug路径整理，GF4不规范数据入库bug
 *"1.6.1227.1014"当前执行订单与时间bug
 *"1.7.0110.2209"修改ITDownLoadDataP2P的bug
 *"1.7.0117.1904"修改StoragePath的StoreHistoryPath为null的bug
 *"1.7.0118.0840"修改task.wait() 添加try catch查看抛出的异常
 *"1.7.0125.1551"gff P2P下载标准内容，暂时移除天顶角和方位角
 *"1.7.0204.1829"增加瓦片Mini、Tiny快视图服务
 *"1.7.0208.0917"增加基于坐标点串的瓦片空间过滤检索、时空发布DEM检索等服务支持
 *"1.7.0209.1005"DataSet.AcceptChange()>>WS_Sqlite 返回DS的XML去掉<diffgr:before>增加基于坐标点串的瓦片空间过滤检索、时空发布DEM检索等服务支持;提供WS_Sqlite瓦片检索needTilePath返回瓦片路径的方法支持
 *"1.7.0211.1006"增加查找删除GFF缺失1、2、3、4、png、pgw其中文件的瓦片preview记录的远程服务
 *"1.7.0211.1744"修改瓦片检索的多个bug，包括瓦片检索时共用变量导致结果交叉冲突，以及通过便利共享文件夹下查找Sqlite文件时没有不符合Mod匹配，有些有sqlite文件，但没有Mod不应该被检索到。
 *"1.7.0215.1705"增加查找删除缺失文件的瓦片记录的远程服务
 *"1.7.0221.1552"增加GFF表,及其更新机制
 *"1.7.0306.0856"增加瓦片云量满幅度
 *"1.7.0306.1708" “高分3号数据入库”
 * "1.7.0405.1531"更新瓦片命名规范，去掉#字符。更新步骤（1）sqlite表更新，使用MultiSqliteManager工具。（2）更新站点与服务程序（包含全部相关的运管、入库程序）。（3）使用CleanTileRecords工具，1）更新库中.jpg .jgw瓦片为.png .pgw瓦片；2）更新库中旧瓦片名到新瓦片名；3）删除缺失文件的瓦片记录；4）更新库中瓦片满幅度和云量信息。
 * 1.7.0407.1601 "更改P2P瓦片下载的过程"
 * 1.7.0408.1201 "由于瓦片的重命名导致检索不到新入库的瓦片数据，原因是时间增加了小时"
 *"1.7.0412.0943" 增加瓦片入库自动计算云量满幅度
 *"1.7.0419.1246" 新命名瓦片Tiny和Mini拇指图显示问题
 *"1.7.0422.1156" 旧命名瓦片入库满幅度云量计算bug
 *"1.7.0427.2152" 设置tcpRemote为单实例
 *"1.7.0503.0950" 解决Col Row Int16太小bug;解决CloudMask.exe缺乏msvcp100.dll msvcp100d.dll msvcr100d.dll等bug
 *"1.7.0504.1854" 优化瓦片检索初始化时长，建立distinct表，提高效率
 *"1.7.0504.2209" 创建GFF表bug，非Groupby字段，需要显示聚合函数
 *"1.7.0515.0939" WS检索Distinct方法，SQLite操作加锁
 *"1.7.0520.1519" 提升批量瓦片入库效率
 */
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels.Tcp;
using QRST_DI_TS_Process.Orders;
using QRST_DI_Resources;
using QRST_DI_TS_Process;
using System.Threading;
using QRST_DI_TS_Process.Site;
using System.Threading.Tasks;
using QRST_DI_TileStore;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBClient.DBEngine;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_SiteServer_Console
{
    public delegate bool ControlCtrlDelegate(int CtrlType);

    class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);//控制台关闭
        static ControlCtrlDelegate newDelegate = new ControlCtrlDelegate(HandlerRoutine);

        public static TServerSite curSite;
        static TcpServerChannel chan;    //远程对象信道
        public static int _CurrentPaiallelTasks = 1; //订单一次并行处理数
        public static bool isSuspended = false;
        public static bool isCancelled = false;
        static List<System.Threading.Tasks.Task> MSTaskList = new List<Task>();
        static List<OrderClass> WorkingOrderList { get; set; }
        private static string _currentSiteIP;
        public static string SelfTSSiteCode { get; set; }
        public static string version = "1.7.0520.1519";
        private static string connectionState = "未连接";
        private static string executionState = "未执行";
        private static string remotingState = "未开启";

        static void Main(string[] args)
        {
            try
            {
                try
                {
                    if (!Constant.Created)
                    {
                        Constant.InitializeTcpConnection();
                        Constant.Create();
                    }

                }
                catch (Exception ex)
                {
                    MyConsole.WriteLine("通信进程启动异常，请联系管理员！\r\n" + ex);
                    Console.ReadLine();
                    return;
                }
                _currentSiteIP = Constant.UsingIPAddress;
                WorkingOrderList = new List<OrderClass>();
                //启动服务进程 @jianghua 20170415
                Process serverProcess = new Process();

                StartService();
                DisplayConfigInfo();

                //启动更新程序
                startUpdater();

                try
                {
                    TServerSiteManager.UpdateStorageSiteVersion(_currentSiteIP, version);
                }
                catch
                { }

                bool bRet = SetConsoleCtrlHandler(newDelegate, true); //控制台关闭事件

                string key = "";
                while (!(key == "exit"))
                {
                    MyConsole.WriteLine("input 'exit' to exit the application！");
                    key = Console.ReadLine();
                }

                try
                {
                    TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Stopped);

                }
                catch (Exception ex)
                {
                    return;
                }
            }
            catch(Exception e)
            {
                MyConsole.WriteLine(e.ToString());
                Console.ReadLine();
            }

        }

        private static void startUpdater()
        {
            string curDir = AppDomain.CurrentDomain.BaseDirectory;
            string updaterExeDir = Path.Combine(curDir.Substring(0, curDir.TrimEnd('\\').LastIndexOf("\\")), "QDB_TS_Updater");
            string updaterExePath = Path.Combine(updaterExeDir, "QRST_DI_TS-ServerUpdater.exe");
            if (File.Exists(updaterExePath))
            {
                //关闭站点服务进程
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in ps)
                {
                    if (p.ProcessName == "QRST_DI_TS-ServerUpdater")
                    {
                        p.Kill();
                    }
                }

                System.Diagnostics.Process newp = System.Diagnostics.Process.Start(updaterExePath);
                Thread.Sleep(500);
                //ShowWindow(newp.Handle, 0);
                ShowWindow(newp.MainWindowHandle, 0);

            }
        }

        public static void StartService()
        {
            //连接数据库、开启远程对象、执行任务
            try
            {
                //TSPCommonReference.Create();
                connectionState = "已连接";
                //验证站点
                SelfTSSiteCode = TServerSiteManager.ConvertTSSiteIP2TSSiteCode(_currentSiteIP);
                if (SelfTSSiteCode == "-1")
                {
                    MyConsole.WriteLine(string.Format("本站点IP{0}未注册，服务启动失败，请联系管理员！", _currentSiteIP));
                    return;
                }
                //同步时间 改成基于TCP的远程调用 @jianghua 20170414
                //SynDBTimeToLocalClass sdbt = new SynDBTimeToLocalClass();
                //sdbt.SynDBTimeToLocal();
                //IDbOperating iLiteOperating = TSPCommonReference.idbOperating;
                IDbOperating iLiteOperating = Constant.IdbOperating;

                iLiteOperating.SynDBTimeToLocal();
                //开启通信信道，包括远程对象通信和TCP通信  迁移到服务进程  @jianghua 20170416
                //InitTCPStorageServChl();
                //InitIPCConsoleServChl();
                TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Running);
                TServerSiteManager.UpdateStorageSiteList();
                curSite = TServerSiteManager.getSiteFromSiteCode(SelfTSSiteCode);
                curSite.ProcessingOrders = WorkingOrderList;
                remotingState = "已开启";
                //启动服务
                executionState = "正在执行";
                Start();

                //恢复站点切片数据
                try
                {
                    Task.Factory.StartNew(RecoveryLostTile);
                }
                catch (Exception ex)
                {

                    MyConsole.WriteLine("[恢复站点切片数据]" + ex.Message);
                    throw ex;
                }
                //20140318 ksk 注释，目的为了解决执行部分切片入库后，站点不再监控订单。将以下内容放在finally块内。功能待测。
                //isSuspended = false;
                //isCancelled = false;
                //MSTaskList = new List<System.Threading.Tasks.Task>();
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine(ex.ToString());
                throw ex;
            }
            //ksk 20140318添加
            finally
            {
                isSuspended = false;
                isCancelled = false;
                //MSTaskList = new List<System.Threading.Tasks.Task>();
            }
        }

        public static void DisplayConfigInfo()
        {
            MyConsole.WriteLine(string.Format("Version:{0}", version));
            MyConsole.WriteLine("#########站点配置信息#########");


            MyConsole.WriteLine(string.Format("站点IP:{0}", _currentSiteIP));
            MyConsole.WriteLine(string.Format("数据库服务通信端口：{0}", Constant.dbUtilityTcpPort));
            MyConsole.WriteLine(string.Format("数据阵列服务器地址：{0}", Constant.DeployedHadoopIP));
            string connectionStr = null;
            string[] connections = null;
            MyConsole.WriteLine(string.Format("数据库服务器地址：{0}",
  Constant.IdbOperating.GetDbServerIp()));
            MyConsole.WriteLine("#########站点状态信息#########");
            MyConsole.WriteLine(string.Format("数据库连接状态：{0}", connectionState));
            MyConsole.WriteLine(string.Format("远程对象服务运行状态：{0}", remotingState));
            MyConsole.WriteLine(string.Format("任务执行状态：{0}", executionState));

        }

        #region 迁移到服务进程 @jianghua 20170414
        /// <summary>
        /// 初始化存储站点TCP远程通信服务端
        /// </summary>
        //public static void InitTCPStorageServChl()
        //{
        //    TCPServiceServer.StartTCPService(Constant.TcpSSPort);
        //}

        ///// <summary>
        ///// 初始化控制台IPC远程通信服务端
        ///// </summary>
        //public static void InitIPCConsoleServChl()
        //{
        //    /// <param name="ht">基本参数</param>
        //    /// <param name="type">通信基类</param>
        //    /// <param name="uri">标识</param>
        //    /// <param name="mode">对象模式（单一新建/共享对象池）</param>

        //    //本地进程间 IPC 通信初始化
        //    IPCServiceServer.StartIPCService("HJDB_Console_IPC");
        //}
        #endregion

        private static void Start()
        {
            Task.Factory.StartNew(() =>
            {
                bool dif = true;
                while (true)
                {
                    _CurrentPaiallelTasks = TServerSiteManager.GetSiteThreadsMaxCount(_currentSiteIP);
                    while (_CurrentPaiallelTasks - MSTaskList.Count > 0)
                    {
                        //增加判断语句，如果i是偶数，则待处理订单通过时间排序然后去订单，如果是奇数则通过优先级加时间的排序进行取订单，@张飞龙
                        if (dif)
                        {
                            MSTaskList.Add(System.Threading.Tasks.Task.Factory.StartNew(AcceptOrder, OrderProcessBy.ONLY_BY_TIME));
                            dif = !dif;
                        }
                        else
                        {
                            MSTaskList.Add(System.Threading.Tasks.Task.Factory.StartNew(AcceptOrder, OrderProcessBy.BY_TIME_AND_PRIORITY));
                        }
                        Thread.Sleep(500);
                    } 
                    try
                    {
                        Task.WaitAny(MSTaskList.ToArray());
                    }
                    catch (System.AggregateException ex)
                    { 
                        throw(ex.InnerException);
                    
                    }
                    for (int i = MSTaskList.Count - 1; i > -1; i--)
                    {
                        Task t = MSTaskList[i];
                        if (t.IsCanceled || t.IsCompleted || t.IsFaulted)
                        {
                            MSTaskList.RemoveAt(i);
                        }
                    }
                    Thread.Sleep(500);

                }
            });
        }

        private static EnumSiteStatus CheckSuspended()
        {
            try
            {
                IDbBaseUtilities iSqLiteBaseUtilities = Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
                string sql = string.Format("select RunningState from tileserversitesinfo where QRST_CODE = '{0}'", SelfTSSiteCode);
                DataSet ds = iSqLiteBaseUtilities.GetDataSet(sql);

                EnumSiteStatus sitestatustype = EnumSiteStatus.Stopped;
                Enum.TryParse(ds.Tables[0].Rows[0][0].ToString(), out sitestatustype);
                if (sitestatustype == EnumSiteStatus.Running)
                {
                    isSuspended = false;
                }
                else
                {
                    isSuspended = true;
                }
                return sitestatustype;
            }
            catch
            {
                return EnumSiteStatus.Error;
            }
        }

        private static void AcceptOrder(Object obj)
        {
            OrderProcessBy processBy = (OrderProcessBy)obj;
            //MyConsole.WriteLine(string.Format("{1}启动执行线程{0}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now.ToString()));

            ////判断线程是否取消
            //while (!isCancelled)
            //{
            //判断任务是不是中断
            EnumSiteStatus sitestatustype = CheckSuspended();
            if (sitestatustype == EnumSiteStatus.Stopped)
            {
                TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Stopped);//按控制台关闭
                Environment.Exit(0);
            }

            while (sitestatustype == EnumSiteStatus.Error)
            {
                if (executionState != "站点存在异常，任务执行被挂起")
                {
                    executionState = "站点存在异常，任务执行被挂起";
                    MyConsole.WriteLine(string.Format("任务执行状态：{0}", executionState));
                }
                Thread.Sleep(1000);
                sitestatustype = CheckSuspended();
            }

            while (sitestatustype == EnumSiteStatus.Suspended)
            {
                if (executionState != "任务执行被挂起")
                {
                    executionState = "任务执行被挂起";
                    MyConsole.WriteLine(string.Format("任务执行状态：{0}", executionState));
                }
                Thread.Sleep(1000);
                sitestatustype = CheckSuspended();
            }

            if (executionState != "正在执行")
            {
                executionState = "正在执行";
                MyConsole.WriteLine(string.Format("任务执行状态：{0}", executionState));
            }

            //查找数据库受理排队订单
            OrderClass neworder = GetNewOrder(processBy);
            if (neworder != null)
            {
                if (OrderManager.SignOrderProcessing(neworder, _currentSiteIP))
                {
                    MyConsole.WriteLine(string.Format("{3} 受理任务订单：{0}-{1}/{2}<<<{4}", neworder.OrderName, neworder.TaskPhase, neworder.Tasks.Count, DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId.ToString()));
                    WorkingOrderList.Add(neworder);             
                    string sql = string.Format("update tileserversitesinfo set CurOrderCode='{0}',OrderSubTime='{1}' where QRST_CODE = '{2}'", neworder.OrderName, DateTime.Now.ToString(), SelfTSSiteCode);
                    IDbBaseUtilities sqLiteBaseUtilities = Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
                    sqLiteBaseUtilities.ExecuteSql(sql); 
                    try
                    {
                        neworder.OrderProcess();
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format("[{0}<<<{2}]{1}", neworder.OrderName, ex.Message, Thread.CurrentThread.ManagedThreadId.ToString());
                        MyConsole.WriteLine(msg);
                        neworder.Status = EnumOrderStatusType.Error;
                        OrderManager.UpdateOrder2DB(neworder);
                        neworder.Logs.Add(msg);
                    }
                    WorkingOrderList.Remove(neworder);
                    MyConsole.WriteLine(string.Format("{3} 结束任务订单：{0}-{1}/{2}<<<{4}", neworder.OrderName, neworder.TaskPhase, neworder.Tasks.Count, DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId.ToString()));
                }
            }
            else
            {
                Thread.Sleep(500 * _CurrentPaiallelTasks);
            }
            GC.Collect();
            //}
        }

        private static OrderClass GetNewOrder(OrderProcessBy processBy)
        {
            //查找数据库受理排队订单
            OrderClass order = null;
            //查找之前意外中断的订单，继续执行
            List<string> workingordercode = new List<string>();
            foreach (OrderClass od in WorkingOrderList)
            {
                workingordercode.Add(od.OrderCode);
            }
            order = OrderManager.GetMissingProcessingOrderFromDB(SelfTSSiteCode, workingordercode);
            //如果没有在执行新订单
            if (order != null)
            {
                //标记订单未签出状态
                OrderManager.ResignOrderProcessing(order);
            }
            else
            {
                order = OrderManager.GetNewOrderFromDB(SelfTSSiteCode, processBy);
            }
            return order;
        }

        /// <summary>
        /// 恢复丢失的切片数据
        /// </summary>
        public static void RecoveryLostTile()
        {
            try
            {
                Thread.Sleep(1000);
                RecoverTileStore.IPAddress = _currentSiteIP;
                RecoverTileStore.RecoverAddedTile();
            }
            catch (Exception)
            {
                return;
            }
        }

        //控制台关闭事件
        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                    TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Stopped); //Ctrl+C关闭  
                    break;
                case 2:
                    TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Stopped);//按控制台关闭按钮关闭  
                    break;
            }
            return false;
        }
    }
}
