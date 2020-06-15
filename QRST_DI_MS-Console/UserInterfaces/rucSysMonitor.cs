 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_MS_Basis.Log;
using QRST_DI_MS_Basis.ExcelOperation;
using QRST_DI_DS_MetadataQuery.PagingQuery;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class rucSysMonitor :RibbonPageBaseUC
    {
        private mucSysMonitor mucsysmonitor;

        public rucSysMonitor(object objmuc)
            : base(objmuc)
        {
            InitializeComponent();

            mucsysmonitor = (mucSysMonitor)base.ObjMainUC;
        }


        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSysLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucsysmonitor.DisplaySysLog();
        }

        /// <summary>
        /// 订单日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemOrderLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucsysmonitor.DisplayOrderLog();
        }

        /// <summary>
        /// 清空日志面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemClearLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucsysmonitor.ClearLogMsg();
        }

        /// <summary>
        /// 导出日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            object obj = mucsysmonitor.GetQueryDataSource();
            if (obj == null || ((DataTable)obj).Rows.Count == 0)
            {
                XtraMessageBox.Show("没有需要导出的日志！");
                return;
            }
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = string.Format("{0}文件(*.{0})|*.{0}", "xlsx");
            sf.Title = "保存";
            sf.CreatePrompt = false;
            sf.ShowDialog();
            if (sf.FileName == null || sf.FileName == "")
            {
                return;
            }
            try
            {
                ExcelOperation.ExcelExport(((DataTable)obj), sf.FileName);
                XtraMessageBox.Show("日志导出成功！");
            }
            catch (System.Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
            }
            
        }

        /// <summary>
        /// 日志查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //object obj1 = barEditEndTime.EditValue;
            //object obj2 = barEditItemKeyWord.EditValue;
            //object obj3 = barEditStartTime.EditValue;
            //object obj4 = barEditItemLogType.EditValue;
            StringBuilder sb = new StringBuilder();

            if (barEditItemKeyWord.EditValue != null)
            {
                string keyWord = barEditItemKeyWord.EditValue.ToString();
                sb.AppendFormat(" IP地址 like '%{0}%' or 日志消息 like '%{0}%'",keyWord);
            }
            if (barEditEndTime.EditValue != null)
            {
                string endTime =((DateTime)barEditEndTime.EditValue).ToShortDateString() ;
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat("日志时间 <= '{0}'",endTime);
            }
            if (barEditStartTime.EditValue != null)
            {
                string startTime =((DateTime)barEditStartTime.EditValue).ToShortDateString() ;
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat("日志时间 >= '{0}'", startTime);
            }
            if (barEditItemLogType.EditValue != null && barEditItemLogType.EditValue.ToString() != "所有日志")
            {
                string typename ;
                if (barEditItemLogType.EditValue.ToString() == "系统日志")
                {
                    typename = EnumLogType.SystemLog.ToString();
                }
                else
                {
                    typename = EnumLogType.OrderLog.ToString();
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat("日志类型 = '{0}'", typename);
            }

          //  DataSet ds = TheUniversal.MIDB.sqlUtilities.GetDataSet(querysql);
            NormalPagingQuery normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "translog_view", sb.ToString());
            mucsysmonitor.Query(normalPagingQuery);
        }
    }
}
