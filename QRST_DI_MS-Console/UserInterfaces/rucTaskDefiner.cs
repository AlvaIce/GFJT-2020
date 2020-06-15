using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_TS_Process.Tasks;
using QRST_DI_DS_Metadata.Paths;
using System.IO;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class rucTaskDefiner : RibbonPageBaseUC
    {
        public mucTaskDefiner muctaskDefiner;

        public rucTaskDefiner()
            : base()
        {
            InitializeComponent();
        }
        public rucTaskDefiner(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            muctaskDefiner = objMUC as mucTaskDefiner;
            muctaskDefiner.rowChangedDel += FocusedRowChange;
        }
        /// <summary>
        /// 添加新的任务组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmTaskDef frmAddTask = new FrmTaskDef();
            frmAddTask.taskFinishedEvent += muctaskDefiner.RefreshDataLst;
            frmAddTask.Show();
        }

        /// <summary>
        /// 删除任务组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            taskdef _taskclass = muctaskDefiner.GetFocusedTask();
            if (_taskclass != null)
                TaskClass.DeleteTask(_taskclass.QRST_CODE);

            //删除已有的组件目录
            string pluginDir = StoragePath.GetPluginDir(_taskclass.ProcessExec);
            Directory.Delete(pluginDir, true);
            muctaskDefiner.RefreshDataLst();
        }

        void FocusedRowChange(TaskClass _taskclass)
        {
            if (_taskclass != null)
            {
                if (_taskclass.TaskType == EnumTaskType.Installed)
                {
                    barButtonItemDelete.Enabled = false;
                }
                else
                {
                    barButtonItemDelete.Enabled = true;
                }
            }
        }

        //组件查询
        private void barButtonItemQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string keyword = "";
            string tasktype = "";
            if (barEditItemKeyWord.EditValue != null)
            {
                keyword = barEditItemKeyWord.EditValue.ToString();
            }
            if (barEditItemType.EditValue != null)
            {
                tasktype = barEditItemType.EditValue.ToString();
            }
            string queryCondition = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                queryCondition = string.Format("(NAME like '%{0}%' or DESCRIPTION like '%{0}%' or ProcessExec like '%{0}%')", keyword);
            }
            if (!string.IsNullOrEmpty(tasktype))
            {
                if (!string.IsNullOrEmpty(queryCondition))
                {
                    queryCondition = queryCondition + " and ";
                }
                queryCondition = queryCondition + string.Format(" Type = '{0}'", tasktype);
            }
            muctaskDefiner.Query(queryCondition);
        }
    }
}
