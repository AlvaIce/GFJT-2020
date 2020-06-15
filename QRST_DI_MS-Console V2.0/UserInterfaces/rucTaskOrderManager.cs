using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_MS_Basis.Taskinfo;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class rucTaskOrderManager : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarButtonItem btWaiting;
        private DevExpress.XtraBars.BarButtonItem btProcessing;
        private DevExpress.XtraBars.BarButtonItem btCompleted;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem btError;
        private DevExpress.XtraBars.BarEditItem barEditItemOrderCode;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemStartTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemEndTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.BarEditItem barEditItemExeState;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraBars.BarEditItem barEditItemKewWord;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSearch;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit1;
        //private DevExpress.XtraBars.BarButtonItem barButtonItemSelectAll;

        public rucTaskOrderManager()
            : base()
        {
            InitializeComponent();
        }
        public rucTaskOrderManager(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.btWaiting = new DevExpress.XtraBars.BarButtonItem();
            this.btProcessing = new DevExpress.XtraBars.BarButtonItem();
            this.btCompleted = new DevExpress.XtraBars.BarButtonItem();
            this.btError = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemOrderCode = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemExeState = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemStartTime = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItemEndTime = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItemKewWord = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItemSearch = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemTimeEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btWaiting,
            this.btProcessing,
            this.btCompleted,
            this.btError});
            this.ribbonControl1.MaxItemId = 5;
            this.ribbonControl1.Size = new System.Drawing.Size(1000, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btWaiting);
            this.ribbonPageGroup1.ItemLinks.Add(this.btProcessing);
            this.ribbonPageGroup1.ItemLinks.Add(this.btCompleted);
            this.ribbonPageGroup1.ItemLinks.Add(this.btError);
            this.ribbonPageGroup1.Text = "任务监控";
            // 
            // btWaiting
            // 
            this.btWaiting.Caption = "等待任务";
            this.btWaiting.Id = 1;
            this.btWaiting.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.等待__1_;
            this.btWaiting.Name = "btWaiting";
            this.btWaiting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btWaiting_ItemClick);
            // 
            // btProcessing
            // 
            this.btProcessing.Caption = "正在执行";
            this.btProcessing.Id = 2;
            this.btProcessing.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.执行;
            this.btProcessing.Name = "btProcessing";
            this.btProcessing.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btProcessing_ItemClick);
            // 
            // btCompleted
            // 
            this.btCompleted.Caption = "已完成";
            this.btCompleted.Id = 3;
            this.btCompleted.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.已完成;
            this.btCompleted.Name = "btCompleted";
            this.btCompleted.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btCompleted_ItemClick);
            // 
            // btError
            // 
            this.btError.Caption = "挂起任务";
            this.btError.Id = 4;
            this.btError.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.错误;
            this.btError.Name = "btError";
            this.btError.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btError_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemOrderCode, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemExeState);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemStartTime, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEndTime);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemKewWord, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemSearch, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "任务查询";
            // 
            // barEditItemOrderCode
            // 
            this.barEditItemOrderCode.Caption = "任务编号：";
            this.barEditItemOrderCode.Edit = this.repositoryItemTextEdit1;
            this.barEditItemOrderCode.Id = 10;
            this.barEditItemOrderCode.Name = "barEditItemOrderCode";
            this.barEditItemOrderCode.Width = 100;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barEditItemExeState
            // 
            this.barEditItemExeState.Caption = "执行状态：";
            this.barEditItemExeState.Edit = this.repositoryItemComboBox2;
            this.barEditItemExeState.Id = 13;
            this.barEditItemExeState.Name = "barEditItemExeState";
            this.barEditItemExeState.Width = 100;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Items.AddRange(new object[] {
            "waiting",
            "processing",
            "completed",
            "error"});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // barEditItemStartTime
            // 
            this.barEditItemStartTime.Caption = "开始时间：";
            this.barEditItemStartTime.Edit = this.repositoryItemDateEdit1;
            this.barEditItemStartTime.Id = 11;
            this.barEditItemStartTime.Name = "barEditItemStartTime";
            this.barEditItemStartTime.Width = 100;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // barEditItemEndTime
            // 
            this.barEditItemEndTime.Caption = "结束时间：";
            this.barEditItemEndTime.Edit = this.repositoryItemDateEdit2;
            this.barEditItemEndTime.Id = 12;
            this.barEditItemEndTime.Name = "barEditItemEndTime";
            this.barEditItemEndTime.Width = 100;
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            this.repositoryItemDateEdit2.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // barEditItemKewWord
            // 
            this.barEditItemKewWord.Caption = "关键字：";
            this.barEditItemKewWord.Edit = this.repositoryItemTextEdit2;
            this.barEditItemKewWord.Id = 14;
            this.barEditItemKewWord.Name = "barEditItemKewWord";
            this.barEditItemKewWord.Width = 100;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // barButtonItemSearch
            // 
            this.barButtonItemSearch.Caption = "检索";
            this.barButtonItemSearch.Id = 15;
            this.barButtonItemSearch.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.检索;
            this.barButtonItemSearch.Name = "barButtonItemSearch";
            this.barButtonItemSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSearch_ItemClick);
            // 
            // repositoryItemTimeEdit1
            // 
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            // 
            // rucTaskOrderManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "rucTaskOrderManager";
            this.Size = new System.Drawing.Size(1000, 150);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        //等待完成的任务列表
        public void btWaiting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = taskorder.GetTaskTable("waiting");
            ((mucTaskOrderMonitor)ObjMainUC).GridSet(dt, 1);
        }

        //正在执行的任务列表
        private void btProcessing_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = taskorder.GetTaskTable("processing");
            ((mucTaskOrderMonitor)ObjMainUC).GridSet(dt, 1);
        }

        //执行完成的任务列表
        private void btCompleted_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = taskorder.GetTaskTable("completed");
            ((mucTaskOrderMonitor)ObjMainUC).GridSet(dt, 1);
        }

        //执行出错的任务列表
        private void btError_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = taskorder.GetTaskTable("error");
            ((mucTaskOrderMonitor)ObjMainUC).GridSet(dt, 1);
        }

        //任务查询的点击事件
        private void barButtonItemSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = taskorder.GetTaskList(ConstructQueryCondition());
            if (barEditItemExeState.EditValue != null && barEditItemExeState.EditValue.ToString() != "completed")
            {
                ((mucTaskOrderMonitor)ObjMainUC).GridSet(dt, 1);
            }
            else
            {
                ((mucTaskOrderMonitor)ObjMainUC).GridSet(dt, 2);
            }
        }

        //任务查询的条件
        string ConstructQueryCondition()
        {
            //订单编号,订单描述,提交时间,执行站点,执行进度,状态
            StringBuilder sb = new StringBuilder();
            DateTime dt;
            if (barEditItemOrderCode.EditValue != null && barEditItemOrderCode.EditValue.ToString() != "")
            {
                sb.AppendFormat(" ID = '{0}' and", barEditItemOrderCode.EditValue.ToString());
            }
            if (barEditItemStartTime.EditValue != null && barEditItemStartTime.EditValue.ToString() != "")
            {
                dt = Convert.ToDateTime(barEditItemStartTime.EditValue);
                string startTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                sb.AppendFormat(" AssignTime >= '{0}' and", startTime);
            }
            if (barEditItemEndTime.EditValue != null && barEditItemEndTime.EditValue.ToString() != "")
            {
                dt = Convert.ToDateTime(barEditItemEndTime.EditValue);
                string endTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                sb.AppendFormat(" AssignTime <= '{0}' and", endTime);
            }
            if (barEditItemExeState.EditValue != null && barEditItemExeState.EditValue.ToString() != "")
            {
                sb.AppendFormat(" Status = '{0}' and", barEditItemExeState.EditValue.ToString());
            }
            if (barEditItemKewWord.EditValue != null && barEditItemKewWord.EditValue.ToString() != "")
            {
                sb.AppendFormat(" (NAME like '%{0}%' or Comment like '%{0}%') ", barEditItemKewWord.EditValue.ToString());
            }
            return sb.ToString().TrimEnd("and".ToArray());
        }
    }
}
