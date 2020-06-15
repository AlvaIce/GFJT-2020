namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class rucSysMonitor
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
            this.barButtonItemSysLog = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemOrderLog = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemClearLog = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemKeyWord = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemLogType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditStartTime = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditEndTime = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barButtonItemQuery = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemExport = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
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
            this.barButtonItemSysLog,
            this.barButtonItemOrderLog,
            this.barButtonItemClearLog,
            this.barEditEndTime,
            this.barEditStartTime,
            this.barButtonItemExport,
            this.barButtonItemQuery,
            this.barEditItemLogType,
            this.barEditItemKeyWord});
            this.ribbonControl1.MaxItemId = 12;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemComboBox1,
            this.repositoryItemTextEdit1});
            this.ribbonControl1.Size = new System.Drawing.Size(765, 147);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemOrderLog, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemSysLog, true);
            this.ribbonPageGroup1.Text = "日志信息";
            // 
            // barButtonItemSysLog
            // 
            this.barButtonItemSysLog.Caption = "系统日志";
            this.barButtonItemSysLog.Id = 3;
            this.barButtonItemSysLog.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.系统日志;
            this.barButtonItemSysLog.Name = "barButtonItemSysLog";
            this.barButtonItemSysLog.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemSysLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSysLog_ItemClick);
            // 
            // barButtonItemOrderLog
            // 
            this.barButtonItemOrderLog.Caption = "订单日志";
            this.barButtonItemOrderLog.Id = 4;
            this.barButtonItemOrderLog.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.订单日志;
            this.barButtonItemOrderLog.Name = "barButtonItemOrderLog";
            this.barButtonItemOrderLog.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemOrderLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemOrderLog_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemClearLog, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "日志操作";
            // 
            // barButtonItemClearLog
            // 
            this.barButtonItemClearLog.Caption = "清空日志面板";
            this.barButtonItemClearLog.Id = 5;
            this.barButtonItemClearLog.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.清空日志面板;
            this.barButtonItemClearLog.Name = "barButtonItemClearLog";
            this.barButtonItemClearLog.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemClearLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemClearLog_ItemClick);
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemKeyWord);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemLogType);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditStartTime, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditEndTime);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemQuery, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemExport, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "日志查询";
            // 
            // barEditItemKeyWord
            // 
            this.barEditItemKeyWord.Caption = "关 键 字 ";
            this.barEditItemKeyWord.Edit = this.repositoryItemTextEdit1;
            this.barEditItemKeyWord.Id = 11;
            this.barEditItemKeyWord.Name = "barEditItemKeyWord";
            this.barEditItemKeyWord.Width = 100;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barEditItemLogType
            // 
            this.barEditItemLogType.Caption = "日志类型";
            this.barEditItemLogType.Edit = this.repositoryItemComboBox1;
            this.barEditItemLogType.Id = 10;
            this.barEditItemLogType.Name = "barEditItemLogType";
            this.barEditItemLogType.Width = 100;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "所有日志",
            "系统日志",
            "订单日志"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // barEditStartTime
            // 
            this.barEditStartTime.Caption = "开始时间：";
            this.barEditStartTime.Edit = this.repositoryItemDateEdit2;
            this.barEditStartTime.Id = 7;
            this.barEditStartTime.Name = "barEditStartTime";
            this.barEditStartTime.Width = 100;
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
            // barEditEndTime
            // 
            this.barEditEndTime.Caption = "结束时间：";
            this.barEditEndTime.Edit = this.repositoryItemDateEdit1;
            this.barEditEndTime.Id = 6;
            this.barEditEndTime.Name = "barEditEndTime";
            this.barEditEndTime.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditEndTime.Width = 100;
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
            // barButtonItemQuery
            // 
            this.barButtonItemQuery.Caption = "查询";
            this.barButtonItemQuery.Id = 9;
            this.barButtonItemQuery.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.查询浏览子系统;
            this.barButtonItemQuery.Name = "barButtonItemQuery";
            this.barButtonItemQuery.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemQuery_ItemClick);
            // 
            // barButtonItemExport
            // 
            this.barButtonItemExport.Caption = "导出";
            this.barButtonItemExport.Id = 8;
            this.barButtonItemExport.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.导出;
            this.barButtonItemExport.Name = "barButtonItemExport";
            this.barButtonItemExport.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemExport_ItemClick);
            // 
            // rucSysMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "rucSysMonitor";
            this.Size = new System.Drawing.Size(765, 152);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItemSysLog;
        private DevExpress.XtraBars.BarButtonItem barButtonItemOrderLog;
        private DevExpress.XtraBars.BarButtonItem barButtonItemClearLog;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarEditItem barEditEndTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarEditItem barEditStartTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExport;
        private DevExpress.XtraBars.BarButtonItem barButtonItemQuery;
        private DevExpress.XtraBars.BarEditItem barEditItemLogType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem barEditItemKeyWord;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;


    }
}
