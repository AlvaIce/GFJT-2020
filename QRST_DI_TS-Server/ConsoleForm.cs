using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process.Site;
using QRST_DI_Resources;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Channels.Tcp;
using QRST_DI_TS_Process;

namespace QRST_DI_TS_Server
{
    public partial class ConsoleForm : Form
    {
        TcpServerChannel chan;    //远程对象信道

        public int _CurrentPaiallelTasks = 1; //订单一次并行处理数
        public bool isSuspended = false;
        public bool isCancelled = false;
        List<System.Threading.Tasks.Task> MSTaskList;
        List<OrderClass> WorkingOrderList { get; set; }
        private string _currentSiteIP;
        public string SelfTSSiteCode { get; set; }

        public ConsoleForm()
        {
            InitializeComponent();
            this.Icon = QRST_DI_Resources.Properties.Resources.TSiteServerNoticefyIcon;
            this.notifyIcon1.Icon = QRST_DI_Resources.Properties.Resources.TSiteServerNoticefyIcon;

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

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            Environment.Exit(-1);
        }


        private void Start()
        {
            for (int i = 0; i < _CurrentPaiallelTasks; i++)
            {
                MSTaskList.Add(System.Threading.Tasks.Task.Factory.StartNew(AcceptOrder));
                Thread.Sleep(500);
            }
        }

        private void CheckSuspended()
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

        private void AcceptOrder()
        {
            //判断线程是否取消
            while (!isCancelled)
            {
                //判断任务是不是中断
                CheckSuspended();
                while (isSuspended)
                {
                    labelExecuteState.Text = "任务执行被挂起";
                    Thread.Sleep(1000);
                    CheckSuspended();
                }
                labelExecuteState.Text = "正在执行任务";

                //查找数据库受理排队订单
                OrderClass neworder = GetNewOrder();
                if (neworder!=null)
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

        private void btnStopExecuteTask_Click(object sender, EventArgs e)
        {
            labelExecuteState.Text = "被动暂停执行任务";

        }

        private void btnExecuteTask_Click(object sender, EventArgs e)
        {
            Start();
            labelExecuteState.Text = "正在执行";
        }

        private void btnStopRemoteServer_Click(object sender, EventArgs e)
        {
          //  chan.StopListening();
          //  InternalRemotingServices.RemotingTrace()
        }

        private void btnStartRemoteServer_Click(object sender, EventArgs e)
        {
            _currentSiteIP = Constant.UsingIPAddress;
            SelfTSSiteCode = TServerSiteManager.ConvertTSSiteIP2TSSiteCode(_currentSiteIP);
            if (SelfTSSiteCode == "-1")
            {
                MessageBox.Show(string.Format("本站点IP{0}未注册，服务启动失败，请联系管理员！", _currentSiteIP));
                return;
            }
            try
            {
                ConsoleServer_IPCService.InitIPCConsoleServChl();
                InitTCPStorageServChl();
                labelRemoteStatus.Text = "已开启";
                btnStartRemoteServer.Enabled = false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("远程对象开启失败！"));
                labelRemoteStatus.Text = "未开启";
                return;
            }
            

            isSuspended = false;
            isCancelled = false;
            MSTaskList = new List<System.Threading.Tasks.Task>();
        }
    }
}
