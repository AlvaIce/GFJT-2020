namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class mucDataMaintainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mucDataMaintainer));
            this.splitContainerControlMaintain = new DevExpress.XtraEditors.SplitContainerControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.barManagerMaintain = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControlTable = new DevExpress.XtraEditors.PanelControl();
            this.gridControlMaintainTable = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.imageListMaintain = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMaintain)).BeginInit();
            this.splitContainerControlMaintain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerMaintain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTable)).BeginInit();
            this.panelControlTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMaintainTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControlMaintain
            // 
            this.splitContainerControlMaintain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlMaintain.Horizontal = false;
            this.splitContainerControlMaintain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControlMaintain.Name = "splitContainerControlMaintain";
            this.splitContainerControlMaintain.Panel1.Controls.Add(this.pictureEdit1);
            this.splitContainerControlMaintain.Panel1.Text = "Panel1";
            this.splitContainerControlMaintain.Panel2.Controls.Add(this.panelControlTable);
            this.splitContainerControlMaintain.Panel2.Text = "Panel2";
            this.splitContainerControlMaintain.Size = new System.Drawing.Size(600, 600);
            this.splitContainerControlMaintain.SplitterPosition = 1;
            this.splitContainerControlMaintain.TabIndex = 0;
            this.splitContainerControlMaintain.Text = "splitContainerControl1";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit1.MenuManager = this.barManagerMaintain;
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(600, 1);
            this.pictureEdit1.TabIndex = 0;
            // 
            // barManagerMaintain
            // 
            this.barManagerMaintain.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManagerMaintain.DockControls.Add(this.barDockControlTop);
            this.barManagerMaintain.DockControls.Add(this.barDockControlBottom);
            this.barManagerMaintain.DockControls.Add(this.barDockControlLeft);
            this.barManagerMaintain.DockControls.Add(this.barDockControlRight);
            this.barManagerMaintain.Form = this.panelControlTable;
            this.barManagerMaintain.Images = this.imageListMaintain;
            this.barManagerMaintain.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticItem1,
            this.barEditItem1,
            this.barButtonItem1,
            this.barButtonItem2});
            this.barManagerMaintain.MainMenu = this.bar2;
            this.barManagerMaintain.MaxItemId = 4;
            this.barManagerMaintain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barEditItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2, true)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "筛选";
            this.barStaticItem1.Id = 0;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "筛选关键字";
            this.barEditItem1.Edit = this.repositoryItemTextEdit1;
            this.barEditItem1.Id = 1;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.Width = 99;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.ImageIndex = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "切换显示模式";
            this.barButtonItem2.Id = 3;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(2, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(596, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(2, 591);
            this.barDockControlBottom.Size = new System.Drawing.Size(596, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(2, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 565);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(598, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 565);
            // 
            // panelControlTable
            // 
            this.panelControlTable.Controls.Add(this.gridControlMaintainTable);
            this.panelControlTable.Controls.Add(this.barDockControlLeft);
            this.panelControlTable.Controls.Add(this.barDockControlRight);
            this.panelControlTable.Controls.Add(this.barDockControlBottom);
            this.panelControlTable.Controls.Add(this.barDockControlTop);
            this.panelControlTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlTable.Location = new System.Drawing.Point(0, 0);
            this.panelControlTable.Name = "panelControlTable";
            this.panelControlTable.Size = new System.Drawing.Size(600, 593);
            this.panelControlTable.TabIndex = 0;
            // 
            // gridControlMaintainTable
            // 
            this.gridControlMaintainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlMaintainTable.Location = new System.Drawing.Point(2, 26);
            this.gridControlMaintainTable.MainView = this.gridView1;
            this.gridControlMaintainTable.MenuManager = this.barManagerMaintain;
            this.gridControlMaintainTable.Name = "gridControlMaintainTable";
            this.gridControlMaintainTable.Size = new System.Drawing.Size(596, 565);
            this.gridControlMaintainTable.TabIndex = 5;
            this.gridControlMaintainTable.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControlMaintainTable;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // imageListMaintain
            // 
            this.imageListMaintain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMaintain.ImageStream")));
            this.imageListMaintain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMaintain.Images.SetKeyName(0, "tiny_refresh.png");
            // 
            // mucDataMaintainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControlMaintain);
            this.Name = "mucDataMaintainer";
            this.Size = new System.Drawing.Size(600, 600);
            this.VisibleChanged += new System.EventHandler(this.mucDataMaintainer_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMaintain)).EndInit();
            this.splitContainerControlMaintain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerMaintain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTable)).EndInit();
            this.panelControlTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMaintainTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlMaintain;
        private DevExpress.XtraBars.BarManager barManagerMaintain;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.ImageList imageListMaintain;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraEditors.PanelControl panelControlTable;
        private DevExpress.XtraGrid.GridControl gridControlMaintainTable;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
    }
}
