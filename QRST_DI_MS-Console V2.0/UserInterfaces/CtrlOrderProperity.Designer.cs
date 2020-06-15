namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class CtrlOrderProperity
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.propertyGridControl1 = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.repositoryItemComboBoxPriority = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.categoryRow1 = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorOrderName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorPriority = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorSiteServer = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRowOwner = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRowPara = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRowOrderPar = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRowTaskPar = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.propertyGridControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(460, 381);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "订单属性";
            // 
            // propertyGridControl1
            // 
            this.propertyGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControl1.Location = new System.Drawing.Point(2, 23);
            this.propertyGridControl1.Name = "propertyGridControl1";
            this.propertyGridControl1.RecordWidth = 136;
            this.propertyGridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxPriority,
            this.repositoryItemTextEdit1});
            this.propertyGridControl1.RowHeaderWidth = 64;
            this.propertyGridControl1.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.categoryRow1,
            this.categoryRowPara});
            this.propertyGridControl1.Size = new System.Drawing.Size(456, 356);
            this.propertyGridControl1.TabIndex = 0;
            // 
            // repositoryItemComboBoxPriority
            // 
            this.repositoryItemComboBoxPriority.AutoHeight = false;
            this.repositoryItemComboBoxPriority.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxPriority.Items.AddRange(new object[] {
            "Emergency",
            "High",
            "Normal",
            "Low"});
            this.repositoryItemComboBoxPriority.Name = "repositoryItemComboBoxPriority";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // categoryRow1
            // 
            this.categoryRow1.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.editorOrderName,
            this.editorPriority,
            this.editorSiteServer,
            this.editorRowOwner});
            this.categoryRow1.Height = 22;
            this.categoryRow1.Name = "categoryRow1";
            this.categoryRow1.Properties.Caption = "基本信息";
            // 
            // editorOrderName
            // 
            this.editorOrderName.Name = "editorOrderName";
            this.editorOrderName.Properties.Caption = "订单名称";
            this.editorOrderName.Properties.FieldName = "Name";
            this.editorOrderName.Properties.ReadOnly = true;
            this.editorOrderName.Properties.RowEdit = this.repositoryItemTextEdit1;
            // 
            // editorPriority
            // 
            this.editorPriority.Name = "editorPriority";
            this.editorPriority.Properties.Caption = "订单优先级";
            this.editorPriority.Properties.FieldName = "Priority";
            this.editorPriority.Properties.ReadOnly = true;
            this.editorPriority.Properties.RowEdit = this.repositoryItemComboBoxPriority;
            // 
            // editorSiteServer
            // 
            this.editorSiteServer.Name = "editorSiteServer";
            this.editorSiteServer.Properties.Caption = "服务站点";
            this.editorSiteServer.Properties.FieldName = "TServerSite";
            this.editorSiteServer.Properties.ReadOnly = true;
            // 
            // editorRowOwner
            // 
            this.editorRowOwner.Name = "editorRowOwner";
            this.editorRowOwner.Properties.Caption = "所有者";
            this.editorRowOwner.Properties.FieldName = "Owner";
            this.editorRowOwner.Properties.ReadOnly = true;
            // 
            // categoryRowPara
            // 
            this.categoryRowPara.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.editorRowOrderPar,
            this.editorRowTaskPar});
            this.categoryRowPara.Name = "categoryRowPara";
            this.categoryRowPara.Properties.Caption = "参数信息";
            // 
            // editorRowOrderPar
            // 
            this.editorRowOrderPar.Name = "editorRowOrderPar";
            this.editorRowOrderPar.Properties.Caption = "订单参数";
            this.editorRowOrderPar.Properties.FieldName = "OrderParams";
            this.editorRowOrderPar.Properties.ReadOnly = true;
            // 
            // editorRowTaskPar
            // 
            this.editorRowTaskPar.Name = "editorRowTaskPar";
            this.editorRowTaskPar.Properties.Caption = "任务参数";
            this.editorRowTaskPar.Properties.FieldName = "TaskParams";
            this.editorRowTaskPar.Properties.ReadOnly = true;
            // 
            // CtrlOrderProperity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Name = "CtrlOrderProperity";
            this.Size = new System.Drawing.Size(460, 381);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraVerticalGrid.PropertyGridControl propertyGridControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxPriority;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorPriority;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorSiteServer;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRowPara;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRowOrderPar;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow1;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRowOwner;
        public DevExpress.XtraVerticalGrid.Rows.EditorRow editorOrderName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRowTaskPar;
    }
}
