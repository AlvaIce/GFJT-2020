using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_MS_Basis.Log;
using QRST_DI_DS_MetadataQuery.PagingQuery;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucSysMonitor : DevExpress.XtraEditors.XtraUserControl
    {
        private static bool isFirstLoad = true;
        private static DateTime orderMonitorTime;        // 开始监控订单日志的时间
        private static DateTime sysMonitorTime;            //开始监控系统日志的时间

           
        public mucSysMonitor()
        { 
            InitializeComponent();
           // translog.sqlUtilities = TheUniversal.MIDB.sqlUtilities;
            translog.sqlUtilities = new QRST_DI_DS_Basis.DBEngine.MySqlBaseUtilities();
            ctrlPage1.queryFinishedEventHandle += QueryFinishedEvent;
           
        }

        /// <summary>
        /// 当第一次加载的时候，将订单日志信息显示在面板上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mucSysMonitor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
               timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        /// <summary>
        /// 将日志信息显示在面板上
        /// </summary>
        void DisplayLogMsg(List<translog> logs,MemoEdit edit)
        {
            edit.Text = "";
            foreach(translog value in logs)
            {
                edit.Text = value.ToString() + "\r\n" + edit.Text;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(Refresh);
            
 
        }

        void Refresh()
        {
            if(memoEditLog.IsHandleCreated)
            {
                List<translog> orderLogs;
               
                lock (translog.sqlUtilities)
                {
                   orderLogs = translog.GetList(string.Format("Type = '{0}' ", EnumLogType.OrderLog.ToString()));
                }
                //DisplayLogMsg(orderLogs, memoEditLog);
                memoEditLog.Invoke(new DisplayLogMsgDel(DisplayLogMsg), new object[] { orderLogs, memoEditLog });
            }
            if (memoEditSysLog.IsHandleCreated)
            {
                List<translog> sysLogs;
                lock (translog.sqlUtilities)
                {
                  sysLogs = translog.GetList(string.Format("Type = '{0}' ", EnumLogType.SystemLog.ToString()));
                }
              //  DisplayLogMsg(sysLogs, memoEditSysLog);
                memoEditSysLog.Invoke(new DisplayLogMsgDel(DisplayLogMsg), new object[] { sysLogs, memoEditSysLog });
            }
        
        }
        delegate void DisplayLogMsgDel(List<translog> logs, MemoEdit edit);



        /// <summary>
        /// 清空面板
        /// </summary>
        public void ClearLogMsg()
        {
            memoEditLog.Text = "";
            memoEditSysLog.Text = "";
        }

        public void DisplayOrderLog()
        {
            xtraTabControl1.SelectedTabPageIndex = 0;
        }

        public void DisplaySysLog()
        {
            xtraTabControl1.SelectedTabPageIndex = 1;
        }

        public void RefreshQueryLog(DataTable dt)
        {
            gridControlQueryLog.DataSource = dt;
            xtraTabControl1.SelectedTabPageIndex = 2;
        }

        public void Query(NormalPagingQuery _normalPagingQueryObj)
        {
            if (_normalPagingQueryObj!=null)
            {
                ctrlPage1.Binding(_normalPagingQueryObj);
                ctrlPage1.FirstQuery();
                ctrlPage1.UpdatePageUC();
            }
        }

        public object GetQueryDataSource()
        {
            return gridControlQueryLog.DataSource;
        }

        public void QueryFinishedEvent()
        {
            gridControlQueryLog.DataSource = ctrlPage1.dt;
            xtraTabControl1.SelectedTabPageIndex = 2;
        }

    }
}
