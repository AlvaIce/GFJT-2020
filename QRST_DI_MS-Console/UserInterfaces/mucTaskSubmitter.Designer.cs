namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class mucTaskSubmitter
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.OrderID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RunState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Choose = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl1.Size = new System.Drawing.Size(922, 338);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.OrderID,
            this.CreateTime,
            this.RunState,
            this.Choose});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // OrderID
            // 
            this.OrderID.Caption = "订单号";
            this.OrderID.Name = "OrderID";
            this.OrderID.Visible = true;
            this.OrderID.VisibleIndex = 0;
            // 
            // CreateTime
            // 
            this.CreateTime.Caption = "创建日期";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.Visible = true;
            this.CreateTime.VisibleIndex = 1;
            // 
            // RunState
            // 
            this.RunState.Caption = "状态";
            this.RunState.Name = "RunState";
            this.RunState.Visible = true;
            this.RunState.VisibleIndex = 2;
            // 
            // Choose
            // 
            this.Choose.Caption = "选择";
            this.Choose.ColumnEdit = this.repositoryItemCheckEdit1;
            this.Choose.Name = "Choose";
            this.Choose.OptionsFilter.AllowFilter = false;
            this.Choose.Visible = true;
            this.Choose.VisibleIndex = 3;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // mucTaskSubmitter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Name = "mucTaskSubmitter";
            this.Size = new System.Drawing.Size(922, 338);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn OrderID;
        private DevExpress.XtraGrid.Columns.GridColumn CreateTime;
        private DevExpress.XtraGrid.Columns.GridColumn RunState;
        private DevExpress.XtraGrid.Columns.GridColumn Choose;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;


    }
}
