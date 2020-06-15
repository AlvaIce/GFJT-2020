using QRST.WorldGlobeTool;
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class muc3DSearcher
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
            this.qrstAxGlobeControl1 = new QRST.WorldGlobeTool.QRSTWorldGlobeControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.uc2DSearcher1 = new QRST_DI_MS_Desktop.UserInterfaces.uc2DSearcher();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // qrstAxGlobeControl1
            // 
            this.qrstAxGlobeControl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.qrstAxGlobeControl1.Cache = null;
            this.qrstAxGlobeControl1.CacheDirectory = "";
            this.qrstAxGlobeControl1.Caption = "";
            this.qrstAxGlobeControl1.CurrentWorld = null;
            this.qrstAxGlobeControl1.DataDirectory = "";
            this.qrstAxGlobeControl1.Is3DMapMode = false;
            this.qrstAxGlobeControl1.IsAnnotationState = false;
            this.qrstAxGlobeControl1.IsGCPDragState = false;
            this.qrstAxGlobeControl1.IsRenderDisabled = false;
            this.qrstAxGlobeControl1.Location = new System.Drawing.Point(-21, 3);
            this.qrstAxGlobeControl1.Name = "qrstAxGlobeControl1";
            this.qrstAxGlobeControl1.PCompiler = null;
            this.qrstAxGlobeControl1.QrstGlobe = null;
            this.qrstAxGlobeControl1.Size = new System.Drawing.Size(394, 372);
            this.qrstAxGlobeControl1.TabIndex = 0;
            this.qrstAxGlobeControl1.Click += new System.EventHandler(this.qrstAxGlobeControl1_Click);
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
            this.dockPanel1.ID = new System.Guid("47773955-46d6-40cc-9fe1-24fb12281163");
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(314, 200);
            this.dockPanel1.Size = new System.Drawing.Size(314, 522);
            this.dockPanel1.Text = "检索数据类型";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(306, 495);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // uc2DSearcher1
            // 
            this.uc2DSearcher1.Location = new System.Drawing.Point(173, 82);
            this.uc2DSearcher1.Name = "uc2DSearcher1";
            this.uc2DSearcher1.Size = new System.Drawing.Size(257, 407);
            this.uc2DSearcher1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(314, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(431, 522);
            this.panelControl1.TabIndex = 3;
            // 
            // muc3DSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.dockPanel1);
            this.Name = "muc3DSearcher";
            this.Size = new System.Drawing.Size(745, 522);
            this.Load += new System.EventHandler(this.muc3DSearcher_Load);
            this.VisibleChanged += new System.EventHandler(this.muc3DSearcher_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public QRSTWorldGlobeControl qrstAxGlobeControl1;
        public uc2DSearcher uc2DSearcher1;
		private DevExpress.XtraBars.Docking.DockManager dockManager1;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
		private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;

    }
}
