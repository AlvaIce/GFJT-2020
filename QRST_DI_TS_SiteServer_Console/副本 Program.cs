//添加MySql.Data引用 joki 131129
/*
 *"1.3.0811.1653"添加执行输出 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Channels.Tcp;
using QRST_DI_TS_Process.Orders;
using QRST_DI_Resources;
using QRST_DI_TS_Process;
using System.Threading;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting;
using QRST_DI_TS_Process.Service;
using System.Runtime.Remoting.Channels.Ipc;
using QRST_DI_TS_Process.Site;
using System.Threading.Tasks;
using QRST_DI_TileStore;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace QRST_DI_TS_SiteServer_Console
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static TServerSite curSite;
        static TcpServerChannel chan;    //远程对象信道
        public static int _CurrentPaiallelTasks = 1; //订单一次并行处理数
        public static bool isSuspended = false;
        public static bool isCancelled = false;
        static List<System.Threading.Tasks.Task> MSTaskList = new List<Task>();
        static List<OrderClass> WorkingOrderList { get; set; }
        private static string _currentSiteIP;
        public static string SelfTSSiteCode { get; set; }
        //public static string version = "1.3.0114.1220";
        public static string version = "1.4.1203.1721";

        private static string connectionState = "未连接";
        private static string executionState = "未执行";
        private static string remotingState = "未开启";

        static void Main(string[] args)
        {
            if (!Constant.Created)
                Constant.Create();
            _currentSiteIP = Constant.UsingIPAddress;
            WorkingOrderList = new List<OrderClass>();
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

            string key = "";
            while (!(key == "exit"))
            {
                Console.WriteLine("input 'exit' to exit the application！");
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
                    if (p.ProcessName == "QRST_DI_TS-ServerUpdater" )
                    {
                        p.Kill();
                    }
                }

                System.Diagnostics.Process newp=System.Diagnostics.Process.Start(updaterExePath);
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
				TSPCommonReference.Create();
				connectionState = "已连接";
				try
				{
					//开启通信信道，包括远程对象通信和TCP通信
					SelfTSSiteCode = TServerSiteManager.ConvertTSSiteIP2TSSiteCode(_currentSiteIP);
					if (SelfTSSiteCode == "-1")
					{
						Console.WriteLine(string.Format("本站点IP{0}未注册，服务启动失败，请联系管理员！", _currentSiteIP));
						return;
					}
					try
					{
						InitTCPStorageServChl();
						InitIPCConsoleServChl();
						TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Running);
						remotingState = "已开启";

						executionState = "正在执行";
						Start();
					}
					catch (Exception ex)
					{
						return;
					}
					//恢复站点切片数据
					Task.Factory.StartNew(RecoveryLostTile);
                    curSite = TServerSiteManager.getSiteFromSiteCode(SelfTSSiteCode);
                    curSite.ProcessingOrders = WorkingOrderList;
					//20140318 ksk 注释，目的为了解决执行部分切片入库后，站点不再监控订单。将以下内容放在finally块内。功能待测。
					//isSuspended = false;
					//isCancelled = false;
					//MSTaskList = new List<System.Threading.Tasks.Task>();
				}
				catch (Exception ex)
				{

				}
			}
			catch (Exception ex)
			{
			}
			//ksk 20140318添加
			finally
			{
				isSuspended = false;
				isCancelled = false;
				MSTaskList = new List<System.Threading.Tasks.Task>();
			}
        }

        public static void DisplayConfigInfo()
        {
            Console.WriteLine(string.Format("Version:{0}",version));
            Console.WriteLine("#########站点配置信息#########");
            string connectionStr = Constant.ConnectionStringMySql;
            string[] connections = connectionStr.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(string.Format("站点IP:{0}",_currentSiteIP));
            Console.WriteLine(string.Format("TCP通信端口：{0}", Constant.TcpSSPort));
            Console.WriteLine(string.Format("数据阵列服务器地址：{0}", Constant.DeployedHadoopIP));
            Console.WriteLine(string.Format("数据库服务器地址：{0}", connections[0].Substring(connections[0].IndexOf("=") + 1).Trim()));

            Console.WriteLine("#########站点状态信息#########");
            Console.WriteLine(string.Format("数据库连接状态：{0}",connectionState));
            Console.WriteLine(string.Format("远程对象服务运行状态：{0}",remotingState));
            Console.WriteLine(string.Format("任务执行状态：{0}",executionState));
           
        }

        /// <summary>
        /// 初始化存储站点TCP远程通信服务端
        /// </summary>
        public static void InitTCPStorageServChl()
        {

            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            int tcpPort = Convert.ToInt16(Constant.TcpSSPort);

            IDictionary props = new Hashtable();
            props["port"] = tcpPort;
            props["name"] = "SSTCP";
            props["typeFilterLevel"] = TypeFilterLevel.Full;
            chan = new TcpServerChannel(
            props, serverProvider);
            ChannelServices.RegisterChannel(chan);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(TServerSiteTCPService),
                            "QDB_Storage_TCP",
                            WellKnownObjectMode.Singleton);
        }

        /// <summary>
        /// 初始化控制台IPC远程通信服务端
        /// </summary>
        public static void InitIPCConsoleServChl()
        {
            /// <param name="ht">基本参数</param>
            /// <param name="type">通信基类</param>
            /// <param name="uri">标识</param>
            /// <param name="mode">对象模式（单一新建/共享对象池）</param>

            //本地进程间 IPC 通信初始化
            IDictionary ht = new Dictionary<string, string>();
            ht["portName"] = "HJDB_Console_IPC";
            ht["name"] = "Console-WebService IPC";
            ht["authorizedGroup"] = "Users";

            IpcServerChannel serverChannel = new IpcServerChannel(ht, null, null);
            ChannelServices.RegisterChannel(serverChannel, false);

            // Expose an object
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ConsoleServer_IPCService), "HJDB_Console_IPC_Service", WellKnownObjectMode.Singleton);
        }


        private static void Start()
        {
            for (int i = 0; i < _CurrentPaiallelTasks; i++)
            {
                MSTaskList.Add(System.Threading.Tasks.Task.Factory.StartNew(AcceptOrder));
                Thread.Sleep(500);
            }
        }

        private static void CheckSuspended()
        {
            try
            {
                string sql = string.Format("select RunningState from tileserversitesinfo where QRST_CODE = '{0}'", SelfTSSiteCode);
                DataSet ds = TSPCommonReference.dbOperating.MIDB.GetDataSet(sql);

                EnumSiteStatus sitestatustype = EnumSiteStatus.Stopped;
                Enum.TryParse(ds.Tables[0].Rows[0][0].ToString(), out sitestatustype);
                if (sitestatustype == EnumSiteStatus.Suspended)
                {
                    isSuspended = true;
                }
                else
                {
                    isSuspended = false;
                }
            }
            catch
            {
            }
        }

        private static void AcceptOrder()
        {
            //判断线程是否取消
            while (!isCancelled)
            {
                //判断任务是不是中断
                CheckSuspended();
                while (isSuspended)
                {
                    if (executionState != "任务执行被挂起")
                    {
                        executionState = "任务执行被挂起";
                        Console.WriteLine(string.Format("任务执行状态：{0}", executionState));
                    }
                    Thread.Sleep(1000);
                    CheckSuspended();
                }

                if (executionState != "正在执行")
                {
                    executionState = "正在执行";
                    Console.WriteLine(string.Format("任务执行状态：{0}", executionState));
                }

                //查找数据库受理排队订单
                OrderClass neworder = GetNewOrder();
                if (neworder != null)
                {
                    neworder.TSSiteIP = _currentSiteIP;
                    Console.WriteLine(string.Format("{3} 受理任务订单：{0}-{1}/{2}", neworder.OrderName, neworder.TaskPhase, neworder.Tasks.Count, DateTime.Now.ToString()));
                    WorkingOrderList.Add(neworder);
                    neworder.OrderProcess();
                    WorkingOrderList.Remove(neworder);
                    Console.WriteLine(string.Format("{3} 结束任务订单：{0}-{1}/{2}", neworder.OrderName, neworder.TaskPhase, neworder.Tasks.Count, DateTime.Now.ToString()));
                }
                else
                {
                    Thread.Sleep(_CurrentPaiallelTasks * 500);
                }
            }
        }
        private static OrderClass GetNewOrder()
        {
            //查找数据库受理排队订单
            OrderClass order = null;
            ////查找之前意外中断的订单，继续执行
            //List<string> workingordercode = new List<string>();
            //foreach (OrderClass od in WorkingOrderList)
            //{
            //    workingordercode.Add(od.OrderCode);
            //}
            //order = OrderManager.GetMissingProcessingOrderFromDB(SelfTSSiteCode,workingordercode);
            ////如果没有在执行新订单
            //if (order==null)
            //{
                order = OrderManager.GetNewOrderFromDB(SelfTSSiteCode);
            //}
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
    }
}
