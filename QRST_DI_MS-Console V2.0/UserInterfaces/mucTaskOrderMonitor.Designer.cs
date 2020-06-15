namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class mucTaskOrderMonitor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.taskID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskSystem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskMethod = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskPriority = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskDistributionTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskSubmitTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.taskOutput = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(922, 338);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.taskID,
            this.taskName,
            this.taskSystem,
            this.taskMethod,
            this.taskStatus,
            this.taskPriority,
            this.taskDistributionTime,
            this.taskDescription,
            this.taskSubmitTime,
            this.taskOutput});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // taskID
            // 
            this.taskID.Caption = "任务序号";
            this.taskID.FieldName = "ID";
            this.taskID.Name = "taskID";
            this.taskID.Visible = true;
            this.taskID.VisibleIndex = 0;
            // 
            // taskName
            // 
            this.taskName.Caption = "任务名称";
            this.taskName.FieldName = "NAME";
            this.taskName.Name = "taskName";
            this.taskName.Visible = true;
            this.taskName.VisibleIndex = 1;
            // 
            // taskSystem
            // 
            this.taskSystem.Caption = "执行子系统";
            this.taskSystem.FieldName = "SubsystemName";
            this.taskSystem.Name = "taskSystem";
            this.taskSystem.Visible = true;
            this.taskSystem.VisibleIndex = 2;
            // 
            // taskMethod
            // 
            this.taskMethod.Caption = "服务方法";
            this.taskMethod.FieldName = "MethodName";
            this.taskMethod.Name = "taskMethod";
            this.taskMethod.Visible = true;
            this.taskMethod.VisibleIndex = 3;
            // 
            // taskStatus
            // 
            this.taskStatus.Caption = "任务状态";
            this.taskStatus.FieldName = "Status";
            this.taskStatus.Name = "taskStatus";
            this.taskStatus.Visible = true;
            this.taskStatus.VisibleIndex = 4;
            // 
            // taskPriority
            // 
            this.taskPriority.Caption = "优先级";
            this.taskPriority.FieldName = "Priority";
            this.taskPriority.Name = "taskPriority";
            this.taskPriority.Visible = true;
            this.taskPriority.VisibleIndex = 5;
            // 
            // taskDistributionTime
            // 
            this.taskDistributionTime.Caption = "分配时间";
            this.taskDistributionTime.FieldName = "AssignTime";
            this.taskDistributionTime.Name = "taskDistributionTime";
            this.taskDistributionTime.Visible = true;
            this.taskDistributionTime.VisibleIndex = 6;
            // 
            // taskDescription
            // 
            this.taskDescription.Caption = "任务说明";
            this.taskDescription.FieldName = "Comment";
            this.taskDescription.Name = "taskDescription";
            this.taskDescription.Visible = true;
            this.taskDescription.VisibleIndex = 9;
            // 
            // taskSubmitTime
            // 
            this.taskSubmitTime.Caption = "提交时间";
            this.taskSubmitTime.FieldName = "SubmitTime";
            this.taskSubmitTime.Name = "taskSubmitTime";
            this.taskSubmitTime.Visible = true;
            this.taskSubmitTime.VisibleIndex = 7;
            // 
            // taskOutput
            // 
            this.taskOutput.Caption = "提交数据";
            this.taskOutput.FieldName = "Output";
            this.taskOutput.Name = "taskOutput";
            this.taskOutput.Visible = true;
            this.taskOutput.VisibleIndex = 8;
            // 
            // mucTaskOrderMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Name = "mucTaskOrderMonitor";
            this.Size = new System.Drawing.Size(922, 338);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn taskID;
        private DevExpress.XtraGrid.Columns.GridColumn taskName;
        private DevExpress.XtraGrid.Columns.GridColumn taskSystem;
        private DevExpress.XtraGrid.Columns.GridColumn taskMethod;
        private DevExpress.XtraGrid.Columns.GridColumn taskStatus;
        private DevExpress.XtraGrid.Columns.GridColumn taskPriority;
        private DevExpress.XtraGrid.Columns.GridColumn taskDistributionTime;
        private DevExpress.XtraGrid.Columns.GridColumn taskDescription;
        private DevExpress.XtraGrid.Columns.GridColumn taskSubmitTime;
        private DevExpress.XtraGrid.Columns.GridColumn taskOutput;
        public DevExpress.XtraGrid.GridControl gridControl1;
    }
}
