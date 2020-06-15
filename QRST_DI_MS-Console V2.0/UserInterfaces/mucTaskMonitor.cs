using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using QRST_DI_TS_Process.Orders;
using System.Threading;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using System.IO;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucTaskMonitor : DevExpress.XtraEditors.XtraUserControl
    {
        //定义当列表行变化时触发
        public delegate void FocusedRowChangedEventHandler(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e);
        public event FocusedRowChangedEventHandler FocusedRowChanged;

        int whichList = 0;   //判断显示哪个列表，0表示全显示，1表示显示正在处理的，2完成表示显示的
        int focusedRow = 0;   //当前选中行

        private System.Threading.Timer ctrlTimer;

        public mucTaskMonitor()
        {
            InitializeComponent();
            ctrlPage1.queryFinishedEventHandle += SetQueryDataSource;
        }
        
        /// <summary>
        /// 当加载到该页面时，进行订单状态的监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mucTaskMonitor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                DisplayProcessOrders();
            }

            TimerCallback timerDelegate = new TimerCallback(timerCallBack);
            ctrlTimer = new System.Threading.Timer(timerDelegate, null,60*1000,60*1000);
            
        }

        /// <summary>
        /// 更新监控信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCallBack(Object state)
        {
            if (this.IsDisposed)
            {
                return;
            }

            MethodInvoker invoker = new MethodInvoker(RefreshMonitorInfo);
            try
            {
                BeginInvoke(invoker);
            }
            catch (Exception ex)
            {


            }
        }

        //获取订单列表,添加进度列
        private DataTable GetDataSource(string whereCondition)
        {
            DataTable dt = OrderManager.GetOrderList(whereCondition);
            DataColumn dc = new DataColumn() { ColumnName = "Progress", DataType = Type.GetType("System.Double") };
            dt.Columns.Add(dc);
            //Choosed
            DataColumn dc1 = new DataColumn() { ColumnName = "Choosed", DataType = Type.GetType("System.Boolean") };
            dt.Columns.Add(dc1);

            for (int i = 0 ; i < dt.Rows.Count ;i++ )
            {
                int tasks = dt.Rows[i]["Tasks"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
                dt.Rows[i]["Progress"] = Convert.ToDouble(dt.Rows[i]["Phase"].ToString()) / Convert.ToDouble(tasks)*100;
                dt.Rows[i]["Choosed"] = false;
            }
            return dt;
        }

        //获取正在处理的订单
        private DataTable GetProcessingDataSource()
        {
            DataTable dt = OrderManager.GetProcessingTable();
            DataColumn dc = new DataColumn() { ColumnName = "Progress", DataType = Type.GetType("System.Double") };
            dt.Columns.Add(dc);
            //Choosed
            DataColumn dc1 = new DataColumn() { ColumnName = "Choosed", DataType = Type.GetType("System.Boolean") };
            dt.Columns.Add(dc1);
            for (int i = 0 ; i < dt.Rows.Count ; i++)
            {
                int tasks = dt.Rows[i]["Tasks"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
                dt.Rows[i]["Progress"] = Convert.ToDouble(dt.Rows[i]["Phase"].ToString()) / Convert.ToDouble(tasks) * 100;
                dt.Rows[i]["Choosed"] = false;
            }
            return dt;
        }

        /// <summary>
        /// 更新列表信息,首先更新现有列的状态，然后添加新的列
        /// </summary>
        /// <param name="dt"></param>
        public void UpdateOrderList(DataTable dt)
        {
            DataTable gridCtrlDataSource = (DataTable)gridControlOrderList.DataSource;
            if (gridCtrlDataSource == null)
            {
                return;
            }
            if (gridCtrlDataSource.Rows.Count >= dt.Rows.Count)
            {
                for (int i = 0 ; i < gridCtrlDataSource.Rows.Count ; i++)
                {
                    if (i < dt.Rows.Count)
                    {
                        UpdataRow(gridCtrlDataSource.Rows[i], dt.Rows[i]);
                    }
                    else
                        gridCtrlDataSource.Rows.RemoveAt(i);
                }
            }
            else
            {
                for (int i = 0 ; i < dt.Rows.Count ; i++)
                {
                    if (i < gridCtrlDataSource.Rows.Count)
                    {
                        UpdataRow(gridCtrlDataSource.Rows[i], dt.Rows[i]);
                    }
                    else
                        gridCtrlDataSource.Rows.Add(dt.Rows[i].ItemArray);
                }
            }
        }

        void UpdataRow(DataRow dr,DataRow newDr)
        {
            for (int i = 0 ; i < dr.ItemArray.Length ;i++ )
            {
                if ( i != 17)
                {
                   dr[i] = newDr[i];
                }
            }
        }

        public void DisplayWaitingOrders()
        {
            gridControlOrderList.DataSource = GetDataSource(string.Format(" s.Status = '{0}'", EnumOrderStatusType.Waiting.ToString()));
            whichList = 0;
            SwitchTab(0);
        }

        public void DisplayProcessOrders()
        {
            gridControlOrderList.DataSource = GetProcessingDataSource();
            whichList = 1;
            SwitchTab(0);
        }

        public void DisplayFinishedOrders()
        {
            gridControlOrderList.DataSource = GetDataSource(string.Format(" s.Status = '{0}'", EnumOrderStatusType.Completed.ToString()));
            whichList = 2;
            SwitchTab(0);
        }

        public void DisplaySuspendOrders()
        {
            gridControlOrderList.DataSource = GetDataSource(string.Format(" s.Status = '{0}'", EnumOrderStatusType.Suspended.ToString()));
            whichList = 3;
            SwitchTab(0);
        }


        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (FocusedRowChanged != null)
            {
                FocusedRowChanged(sender,e);
            }
            focusedRow = e.FocusedRowHandle;
        }

        /// <summary>
        /// 动态监控正在处理的订单
        /// </summary>
       public void RefreshMonitorInfo()
        {
            
            DataTable newDataSource = null;
            if (whichList == 0)
            {
               // newDataSource = GetDataSource("");
                return;
            }
            else if(whichList == 1)
            {
                newDataSource = GetProcessingDataSource();
            }
            else if(whichList == 2)
            {
               // newDataSource = GetDataSource(string.Format(" s.Status = '{0}'", EnumOrderStatusType.Completed.ToString()));
                return;
            }
            else if (whichList == 3)
            {
                return;
            }
             UpdateOrderList(newDataSource);
        }

        /// <summary>
        /// 刷新当前的订单列表
        /// </summary>
       public void RefreshOrderLstInfo()
       {
           DataTable newDataSource = null;
           if (whichList == 0)
           {
               newDataSource = GetDataSource(string.Format(" s.Status = '{0}'", EnumOrderStatusType.Waiting.ToString()));
           }
           else if (whichList == 1)
           {
               newDataSource = GetProcessingDataSource();
           }
           else if (whichList == 2)
           {
                newDataSource = GetDataSource(string.Format(" s.Status = '{0}'", EnumOrderStatusType.Completed.ToString()));
           }
           else if (whichList == 3)
           {
               newDataSource = GetDataSource(string.Format(" s.Status = '{0}'", EnumOrderStatusType.Suspended.ToString()));
           }
           gridControlOrderList.DataSource = newDataSource;
       }

        /// <summary>
        /// 获取选中的订单
        /// </summary>
        public List<string> GetChoosedOrders()
        {
            List<string> orderCodeLst = new List<string>();
            DataTable gridCtrlDataSource = (DataTable)gridControlOrderList.DataSource;

            for (int i = 0 ; i < gridCtrlDataSource.Rows.Count ; i++)
            {
                if ((bool)gridCtrlDataSource.Rows[i]["Choosed"])
                {
                    orderCodeLst.Add(gridCtrlDataSource.Rows[i]["OrderCode"].ToString());
                }
            }
            return orderCodeLst;
        }
        
        /// <summary>
        /// 删除选中的订单
        /// </summary>
        public void DeleteChoosedOrders()
        {
            List<string> orderCodeLst = GetChoosedOrders();
            foreach (string code in orderCodeLst)
            {
                OrderManager.DeleteOrderByCode(code);
            }
        }

        /// <summary>
        /// 挂起所选择的订单
        /// </summary>
        public void SuspendChoosedOrders()
        {
            List<string> orderCodeLst = GetChoosedOrders();
            foreach (string code in orderCodeLst)
            {
                OrderManager.ChangeOrderStatus(code,EnumOrderStatusType.Suspended);
            }
        }

        /// <summary>
        /// 将选择的订单加入等待队列
        /// </summary>
        public void StartChoosedOrders()
        {
            List<string> orderCodeLst = GetChoosedOrders();
            foreach (string code in orderCodeLst)
            {
                OrderManager.ChangeOrderStatus(code, EnumOrderStatusType.Waiting);
            }
        }

        /// <summary>
        ///设置刷新时间
        /// </summary>
        /// <param name="ms"></param>
        public void SetTimeSpan(int ms)
        {
            ctrlTimer.Change(0, ms);
        }

        public void RefreshGridView()
        {
            gridView1.CloseEditor();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        public void QueryOrder(string whereCondition)
        {
            NormalPagingQuery normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "orders_view", whereCondition);
            this.ctrlPage1.Binding(normalPagingQuery);
            this.ctrlPage1.FirstQuery();
            this.ctrlPage1.UpdatePageUC();
        }

        string ChooseDataColumnCaption = "选择";
        string WorkSpace = "工作空间";
        string orderLog = "订单日志";
        void SetQueryDataSource()
        {
            DataTable dt = ctrlPage1.dt;
            if(dt == null)
            {
                gridControlDataList.DataSource = null;
            }
            //数据选择列
            DataColumn checkDownColumn = new DataColumn() { ColumnName = ChooseDataColumnCaption, DataType = typeof(bool) };
            dt.Columns.Add(checkDownColumn);
            DataColumn workspaceColumn = new DataColumn() { ColumnName = WorkSpace, DataType = Type.GetType("System.String") };
            dt.Columns.Add(workspaceColumn);
            DataColumn orderlog = new DataColumn() { ColumnName = orderLog, DataType = Type.GetType("System.String") };
            dt.Columns.Add(orderLog);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
                dt.Rows[i][WorkSpace] = "订单工作空间";
                dt.Rows[i][orderLog] = "订单日志";
            }
            gridControlDataList.DataSource = dt;

            SwitchTab(1);

        }

        /// <summary>
        /// 查看日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemHyperLinkEdit2_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt == null || gridViewMain.FocusedRowHandle < 0 || gridViewMain.FocusedRowHandle > dt.Rows.Count)
            {
                MessageBox.Show("无法获取日志信息！");
            }
            else
            {
                FrmOrderLogInfo frm = new FrmOrderLogInfo(dt.Rows[gridViewMain.FocusedRowHandle]["订单编号"].ToString());
                frm.Show();
            }
        }

        /// <summary>
        /// 打开工作空间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt == null || gridViewMain.FocusedRowHandle < 0 || gridViewMain.FocusedRowHandle > dt.Rows.Count)
            {
                MessageBox.Show("无法获取订单工作空间！");
            }
            string _workspace = string.Format(@"\\{0}\QRST_DB_Share\{1}", dt.Rows[gridViewMain.FocusedRowHandle]["执行站点"].ToString(), dt.Rows[gridViewMain.FocusedRowHandle]["订单编号"].ToString()); 
            Thread openWorkspaceThread = new Thread(new ParameterizedThreadStart(OpenWorkSpace));
            openWorkspaceThread.Start(_workspace);
            Thread.Sleep(4000);    //四秒还未打开，则杀掉线程
            if (openWorkspaceThread.ThreadState == ThreadState.Running)
            {
                openWorkspaceThread.Abort();
                MessageBox.Show(string.Format("打开工作空间超时，该工作空间不存在或已被删除！"));
            }

        }
        
        void  OpenWorkSpace(object _workSpace1)
        {
            string _workSpace = _workSpace1.ToString();
            if (Directory.Exists(_workSpace))
            {
                System.Diagnostics.Process.Start("explorer.exe", _workSpace);
            }
            else
            {
                MessageBox.Show("工作空间不存在！");
            }
        }

        private void gridViewMain_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        public void SwitchTab(int i)
        {
            if(i==0)
            {
                xtraTabControl1.SelectedTabPageIndex = 0;
            }
            else if(i==1)
            {
                xtraTabControl1.SelectedTabPageIndex = 1;
            }
        }

   
        public void SelectAll(bool _select)
        {
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if(dt!=null)
            {
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    dt.Rows[i][ChooseDataColumnCaption] = _select;
                }
            }

        }

        /// <summary>
        /// 将选中的订单改为制定的状态
        /// </summary>
        /// <param name="statusType"></param>
        /// <param name="phase"></param>
        public void ChangeSelectOrderStatus(EnumOrderStatusType statusType,int phase=-1)
        {
            //List<OrderClass> orderLst = OrderManager.GetOrderObjLst(" OrderCode = '{0}'",);
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if(dt!=null)
            {
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        string orderCode = dt.Rows[i]["订单编号"].ToString();
                        List<OrderClass> orderLst = OrderManager.GetOrderObjLst(string.Format(" OrderCode = '{0}'",orderCode));
                        if(orderLst.Count>0)
                        {
                            orderLst[0].Status = statusType;
                            if(phase>=0)
                            {
                                orderLst[0].TaskPhase = phase;
                            }
                            OrderManager.UpdateOrder2DB(orderLst[0]);
                        }
                    }
                }
            }
            ctrlPage1.RefreshCurrentPage();
          
        }

        public void DeleteSelectOrderStatus(EnumOrderStatusType statusType, int phase = -1)
        {
            //List<OrderClass> orderLst = OrderManager.GetOrderObjLst(" OrderCode = '{0}'",);
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        string orderCode = dt.Rows[i]["订单编号"].ToString();
                        List<OrderClass> orderLst = OrderManager.GetOrderObjLst(string.Format(" OrderCode = '{0}'",orderCode));
                        if(orderLst.Count>0&&orderLst[0].Status != EnumOrderStatusType.Processing)  //如果该订单状态为Processing，则禁止删除
                        {
                            OrderManager.DeleteOrderByCode(orderCode);
                        }
                    }
                }
            }
            ctrlPage1.FirstQuery();

        }


    }
}
