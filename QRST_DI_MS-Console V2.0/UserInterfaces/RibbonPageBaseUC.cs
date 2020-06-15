using System;
using DevExpress.XtraBars.Ribbon;
using System.Windows.Forms;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public class RibbonPageBaseUC:UserControl
    {
        protected RibbonControl ribbonControl1;
        protected RibbonPage ribbonPage1;
        protected RibbonPageGroup ribbonPageGroup1;

        public RibbonPage RibbonPage { get { return ribbonPage1; } }
        public Object ObjMainUC { get; protected set; }

        public RibbonPageBaseUC()
            : base()
        {
            InitializeComponent();
        }

        public RibbonPageBaseUC(Object objMainUC)
            : base()
        {
            ObjMainUC = objMainUC;
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
			this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
			this.SuspendLayout();
			// 
			// ribbonControl1
			// 
			this.ribbonControl1.ApplicationButtonText = null;
			this.ribbonControl1.BackColor = System.Drawing.Color.DodgerBlue;
			// 
			// 
			// 
			this.ribbonControl1.ExpandCollapseItem.Id = 0;
			this.ribbonControl1.ExpandCollapseItem.Name = "";
			this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem});
			this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
			this.ribbonControl1.MaxItemId = 1;
			this.ribbonControl1.Name = "ribbonControl1";
			this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
			this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
			// 
			// ribbonPage1
			// 
			this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
			this.ribbonPage1.Name = "ribbonPage1";
			this.ribbonPage1.Text = "主页";
			// 
			// ribbonPageGroup1
			// 
			this.ribbonPageGroup1.Name = "ribbonPageGroup1";
			this.ribbonPageGroup1.Text = "ribbonPageGroup1";
			// 
			// RibbonPageBaseUC
			// 
			this.Controls.Add(this.ribbonControl1);
			this.Name = "RibbonPageBaseUC";
			this.Size = new System.Drawing.Size(687, 150);
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
			this.ResumeLayout(false);

        }
    }
}
