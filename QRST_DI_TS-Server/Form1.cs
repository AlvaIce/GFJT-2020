using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_TS_Process.Orders;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using QRST_DI_Resources;
using System.Runtime.Remoting;
using System.Threading;
using QRST_DI_TS_Process.Site;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Configuration;
using QRST_DI_TS_Process;
using QRST_DI_TileStore;
using System.Threading.Tasks;

namespace QRST_DI_TS_Server
{
    public partial class Form1 : Form
    {
        TcpServerChannel chan;    //远程对象信道

        public int _CurrentPaiallelTasks = 1; //订单一次并行处理数
        public bool isSuspended = false;
        public bool isCancelled = false;
        List<System.Threading.Tasks.Task> MSTaskList;
        List<OrderClass> WorkingOrderList { get; set; }
        private string _currentSiteIP;
        public string SelfTSSiteCode { get; set; }

        public Form1()
        {
            InitializeComponent();
            _currentSiteIP = Constant.UsingIPAddress;
        
            DisplayConfigInfo();
            this.Icon = QRST_DI_Resources.Properties.Resources.TSiteServerNoticefyIcon;
         //   this.notifyIcon1.Icon = QRST_DI_Resources.Properties.Resources.TSiteServerNoticefyIcon;
          
            WorkingOrderList = new List<OrderClass>();
        }

        /// <summary>
        /// 初始化存储站点TCP远程通信服务端
        /// </summary>
        public void InitTCPStorageServChl()
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
            ht["portName"] = "QDB_Console_IPC";
            ht["name"] = "Console-WebService IPC";
            ht["authorizedGroup"] = "Users";

            IpcServerChannel serverChannel = new IpcServerChannel(ht, null, null);
            ChannelServices.RegisterChannel(serverChannel, false);

            // Expose an object
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ConsoleServer_IPCService), "QDB_Console_IPC_Service", WellKnownObjectMode.Singleton);
        }

        private void Start()
        {
            for (int i = 0 ; i < _CurrentPaiallelTasks ; i++)
            {
                MSTaskList.Add(System.Threading.Tasks.Task.Factory.StartNew(AcceptOrder));
                Thread.Sleep(500);
            }
        }

        private void AcceptOrder()
        {
            //判断线程是否取消
            while (!isCancelled)
            {
                //判断任务是不是中断
                while (isSuspended)
                {
                    Thread.Sleep(1000);
                }

                //查找数据库受理排队订单
                OrderClass neworder = GetNewOrder();
                if (neworder != null)
                {
                    neworder.TSSiteIP = _currentSiteIP;

                    WorkingOrderList.Add(neworder);
                    neworder.OrderProcess();
                    WorkingOrderList.Remove(neworder);
                }
                else
                {
                    Thread.Sleep(_CurrentPaiallelTasks * 500);
                }
            }
        }

        private OrderClass GetNewOrder()
        {
            //查找数据库受理排队订单
            return OrderManager.GetNewOrderFromDB(SelfTSSiteCode,OrderProcessBy.BY_TIME_AND_PRIORITY);
        }

        private void btnStartRemoteServer_Click(object sender, EventArgs e)
        {
          
            SelfTSSiteCode = TServerSiteManager.ConvertTSSiteIP2TSSiteCode(_currentSiteIP);
            if (SelfTSSiteCode == "-1")
            {
                MessageBox.Show(string.Format("本站点IP{0}未注册，服务启动失败，请联系管理员！", _currentSiteIP));
                return;
            }
            try
            {
                InitTCPStorageServChl();
                InitIPCConsoleServChl();
                TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Running);
                labelRemoteStatus.Text = "已开启";
                btnStartRemoteServer.Enabled = false;
                btnExecuteTask.Enabled = true;
            }
            catch (Exception ex)
            {
                btnExecuteTask.Enabled = false;
                MessageBox.Show(string.Format("远程对象开启失败！"));
                labelRemoteStatus.Text = "未开启";
                return;
            }
            //恢复站点切片数据
            Task.Factory.StartNew(RecoveryLostTile);


            isSuspended = false;
            isCancelled = false;
            MSTaskList = new List<System.Threading.Tasks.Task>();
        }

        private void btnStopRemoteServer_Click(object sender, EventArgs e)
        {
          //  chan.StopListening(null);
        }

        private void btnExecuteTask_Click(object sender, EventArgs e)
        {
            Start();
            labelExecuteState.Text = "正在执行";
            btnExecuteTask.Enabled = false;
        }

        private void btnStopExecuteTask_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((labelRemoteStatus.Text == "已开启") || (labelExecuteState.Text == "正在执行"))
            {
                MessageBox.Show("有正在运行的服务，无法进行对配置参数的修改！");
            }
            if (Check())  //将配置写到配置文件，刷新配置参数
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings["TcpSSPort"].Value = textTcpSSPort.Text;
                cfa.AppSettings.Settings["DeployedHadoopIP"].Value = textDataStorePath.Text;
                cfa.AppSettings.Settings["UsingIPAddress"].Value = textSiteIP.Text;

                cfa.AppSettings.Settings["ConnectionStringMySql"].Value = string.Format("server = {0}; user id = {1}; password = {2}; database = midb", textDbIP.Text.Trim(), textUserName.Text.Trim(), textPassword.Text.Trim());
                cfa.AppSettings.Settings["ConnectionStringEVDB"].Value = string.Format("server = {0}; user id = {1}; password = {2}; database = evdb", textDbIP.Text.Trim(), textUserName.Text.Trim(), textPassword.Text.Trim());
                cfa.AppSettings.Settings["ConnectionStringBSDB"].Value = string.Format("server = {0}; user id = {1}; password = {2}; database = bsdb", textDbIP.Text.Trim(), textUserName.Text.Trim(), textPassword.Text.Trim());
                cfa.AppSettings.Settings["ConnectionStringIPDB"].Value = string.Format("server = {0}; user id = {1}; password = {2}; database = ipdb", textDbIP.Text.Trim(), textUserName.Text.Trim(), textPassword.Text.Trim());
                cfa.AppSettings.Settings["ConnectionStringISDB"].Value = string.Format("server = {0}; user id = {1}; password = {2}; database = isdb", textDbIP.Text.Trim(), textUserName.Text.Trim(), textPassword.Text.Trim());
                cfa.AppSettings.Settings["ConnectionStringMADB"].Value = string.Format("server = {0}; user id = {1}; password = {2}; database = madb", textDbIP.Text.Trim(), textUserName.Text.Trim(), textPassword.Text.Trim());
                cfa.AppSettings.Settings["ConnectionStringRCDB"].Value = string.Format("server = {0}; user id = {1}; password = {2}; database = rcdb", textDbIP.Text.Trim(), textUserName.Text.Trim(), textPassword.Text.Trim());


                cfa.Save();
                ConfigurationManager.RefreshSection("appSettings");
                DisplayConfigInfo();
            }
        }

        public void DisplayConfigInfo()
        {
            labelSiteIP.Text = Constant.UsingIPAddress;
            labelTcpSSPort.Text = Constant.TcpSSPort;
            labelDataStorePath.Text = Constant.DeployedHadoopIP;

            textDataStorePath.Text = Constant.DeployedHadoopIP;
            textTcpSSPort.Text = Constant.TcpSSPort;
            textSiteIP.Text = Constant.UsingIPAddress;

            _currentSiteIP = Constant.UsingIPAddress;

            string connectionStr = Constant.ConnectionStringMySql;
            string[] connections = connectionStr.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            labelDbIP.Text = connections[0].Substring(connections[0].IndexOf("=") + 1).Trim();

            textDbIP.Text = connections[0].Substring(connections[0].IndexOf("=")+1).Trim();
            textUserName.Text = connections[1].Substring(connections[1].IndexOf("=") + 1).Trim();
            textPassword.Text = connections[2].Substring(connections[2].IndexOf("=") + 1).Trim();

        }

        bool Check()
        {
            if (string.IsNullOrEmpty(textSiteIP.Text))
            {
                MessageBox.Show("站点IP不能为空！");
                textSiteIP.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textDataStorePath.Text))
            {
                MessageBox.Show("数据阵列映射地址不能为空！");
                textDataStorePath.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textTcpSSPort.Text))
            {
                  MessageBox.Show("TCP通信端口不能为空！");
                textTcpSSPort.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textDbIP.Text))
            {
                MessageBox.Show("数据库服务器地址不能为空！");
                textDbIP.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textUserName.Text))
            {
                MessageBox.Show("用户名不能为空！");
                textUserName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textPassword.Text))
            {
                MessageBox.Show("密码不能为空！");
                textPassword.Focus();
                return false;
            }


            return true;
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string connectionStr = string.Format("server = {0}; user id = {1}; password = {2}; database = midb",textDbIP.Text.Trim(),textUserName.Text.Trim(),textPassword.Text.Trim());
            try
            {
                MySqlBaseUtilities utilities = new MySqlBaseUtilities(connectionStr);
                if (utilities.ConnectTest())
                {
                    MessageBox.Show("连接成功！");
                    utilities.Close();
                }
                else
                {
                    MessageBox.Show("连接失败！");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接失败！");
            }
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnection2SQL_Click(object sender, EventArgs e)
        {
            try
            {
                TSPCommonReference.Create();
                btnStartRemoteServer.Enabled = true;
                labelDbState.Text = "已连接";
                btnConnection2SQL.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库连接失败！"+ex.ToString());
                btnExecuteTask.Enabled = false;
                btnStartRemoteServer.Enabled = false;
                labelDbState.Text = "未连接";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread th= new Thread(UpdateSiteStatus);
            th.Start();
        }

        void UpdateSiteStatus()
        {
            try
            {
                TServerSiteManager.UpdateSiteRunningState(_currentSiteIP, EnumSiteStatus.Stopped);
            }
            catch (Exception ex)
            {
                return;
            }
            
        }

        /// <summary>
        /// 恢复丢失的切片数据
        /// </summary>
        public void RecoveryLostTile()
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
