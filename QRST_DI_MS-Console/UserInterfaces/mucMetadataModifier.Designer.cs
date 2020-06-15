namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class mucMetadataModifier
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mucMetadataModifier));
			this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
			this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
			this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.ctrlTableManager1 = new QRST_DI_MS_Console.UserInterfaces.CtrlTableManager();
			((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
			this.dockPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dockManager1
			// 
			this.dockManager1.Form = this;
			this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
			this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
			// 
			// dockPanel1
			// 
			this.dockPanel1.Controls.Add(this.dockPanel1_Container);
			this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
			this.dockPanel1.ID = new System.Guid("de0f6983-61e7-47fd-bedf-cdd6862f835e");
			this.dockPanel1.Location = new System.Drawing.Point(0, 0);
			this.dockPanel1.Name = "dockPanel1";
			this.dockPanel1.OriginalSize = new System.Drawing.Size(320, 200);
			this.dockPanel1.Size = new System.Drawing.Size(320, 487);
			this.dockPanel1.Text = "元数据节点信息";
			// 
			// dockPanel1_Container
			// 
			this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
			this.dockPanel1_Container.Name = "dockPanel1_Container";
			this.dockPanel1_Container.Size = new System.Drawing.Size(312, 460);
			this.dockPanel1_Container.TabIndex = 0;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "DB_Save.ico");
			this.imageList1.Images.SetKeyName(1, "Aqua%20scenery%20ii.png");
			this.imageList1.Images.SetKeyName(2, "Document.ico");
			this.imageList1.Images.SetKeyName(3, "矢量数据 .png");
			this.imageList1.Images.SetKeyName(4, "栅格.png");
			// 
			// ctrlTableManager1
			// 
			this.ctrlTableManager1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlTableManager1.Location = new System.Drawing.Point(320, 0);
			this.ctrlTableManager1.Name = "ctrlTableManager1";
			this.ctrlTableManager1.Size = new System.Drawing.Size(584, 487);
			this.ctrlTableManager1.TabIndex = 4;
			// 
			// mucMetadataModifier
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ctrlTableManager1);
			this.Controls.Add(this.dockPanel1);
			this.Name = "mucMetadataModifier";
			this.Size = new System.Drawing.Size(904, 487);
			this.Load += new System.EventHandler(this.mucMetadataModifier_Load);
			this.VisibleChanged += new System.EventHandler(this.mucMetadataModifier_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
			this.dockPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
		private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        public CtrlTableManager ctrlTableManager1;
        private System.Windows.Forms.ImageList imageList1;
   //     private System.Windows.Forms.ImageList imageList1;
    }
}
