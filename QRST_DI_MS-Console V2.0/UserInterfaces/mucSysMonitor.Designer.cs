namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class mucSysMonitor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.memoEditLog = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.memoEditSysLog = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControlQueryLog = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ctrlPage1 = new QRST_DI_MS_Desktop.UserInterfaces.CtrlPage();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSysLog.Properties)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlQueryLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // memoEditLog
            // 
            this.memoEditLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEditLog.Location = new System.Drawing.Point(0, 0);
            this.memoEditLog.Name = "memoEditLog";
            this.memoEditLog.Properties.Appearance.BackColor = System.Drawing.Color.DarkGray;
            this.memoEditLog.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoEditLog.Properties.Appearance.Options.UseBackColor = true;
            this.memoEditLog.Properties.Appearance.Options.UseFont = true;
            this.memoEditLog.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.memoEditLog.Properties.ReadOnly = true;
            this.memoEditLog.Size = new System.Drawing.Size(668, 382);
            this.memoEditLog.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(674, 409);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.memoEditLog);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(668, 382);
            this.xtraTabPage1.Text = "订单日志";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.memoEditSysLog);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(668, 382);
            this.xtraTabPage2.Text = "系统日志";
            // 
            // memoEditSysLog
            // 
            this.memoEditSysLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEditSysLog.Location = new System.Drawing.Point(0, 0);
            this.memoEditSysLog.Name = "memoEditSysLog";
            this.memoEditSysLog.Properties.Appearance.BackColor = System.Drawing.Color.DarkGray;
            this.memoEditSysLog.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoEditSysLog.Properties.Appearance.Options.UseBackColor = true;
            this.memoEditSysLog.Properties.Appearance.Options.UseFont = true;
            this.memoEditSysLog.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.memoEditSysLog.Properties.ReadOnly = true;
            this.memoEditSysLog.Size = new System.Drawing.Size(668, 382);
            this.memoEditSysLog.TabIndex = 1;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.ctrlPage1);
            this.xtraTabPage3.Controls.Add(this.gridControlQueryLog);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(668, 382);
            this.xtraTabPage3.Text = "日志查询信息";
            // 
            // gridControlQueryLog
            // 
            this.gridControlQueryLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlQueryLog.Location = new System.Drawing.Point(0, 0);
            this.gridControlQueryLog.MainView = this.gridView1;
            this.gridControlQueryLog.Name = "gridControlQueryLog";
            this.gridControlQueryLog.Size = new System.Drawing.Size(668, 346);
            this.gridControlQueryLog.TabIndex = 0;
            this.gridControlQueryLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControlQueryLog;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ctrlPage1
            // 
            this.ctrlPage1.CurrentPage = 1;
            this.ctrlPage1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ctrlPage1.dt = null;
            this.ctrlPage1.Location = new System.Drawing.Point(0, 352);
            this.ctrlPage1.Name = "ctrlPage1";
            this.ctrlPage1.PageSize = 20;
            this.ctrlPage1.Size = new System.Drawing.Size(668, 30);
            this.ctrlPage1.TabIndex = 1;
            this.ctrlPage1.TotalPage = 1;
            // 
            // mucSysMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "mucSysMonitor";
            this.Size = new System.Drawing.Size(674, 409);
            this.VisibleChanged += new System.EventHandler(this.mucSysMonitor_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEditSysLog.Properties)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlQueryLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit memoEditLog;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.MemoEdit memoEditSysLog;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraGrid.GridControl gridControlQueryLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private CtrlPage ctrlPage1;

    }
}
