namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class rucTaskMonitor
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
            this.OrderState = new DevExpress.XtraBars.BarButtonItem();
            this.ComplishedOrder = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.CompliedOrder = new DevExpress.XtraBars.BarButtonItem();
            this.barListItem1 = new DevExpress.XtraBars.BarListItem();
            this.UnplayedOrder = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.UplayerOrder = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            this.ribbonMiniToolbar1 = new DevExpress.XtraBars.Ribbon.RibbonMiniToolbar(this.components);
            this.HandlingOrder = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.HandleOder = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barMdiChildrenListItem1 = new DevExpress.XtraBars.BarMdiChildrenListItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
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
            this.OrderState,
            this.HandleOder,
            this.CompliedOrder,
            this.UplayerOrder,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barListItem1,
            this.barButtonGroup1,
            this.barMdiChildrenListItem1});
            this.ribbonControl1.MaxItemId = 17;
            this.ribbonControl1.MiniToolbars.Add(this.ribbonMiniToolbar1);
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.HandlingOrder,
            this.ComplishedOrder,
            this.UnplayedOrder});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.OrderState);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem5);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup1.Text = "订单状态";
            // 
            // OrderState
            // 
            this.OrderState.Caption = "订单状态";
            this.OrderState.Id = 1;
            this.OrderState.Name = "OrderState";
            // 
            // ComplishedOrder
            // 
            this.ComplishedOrder.ItemLinks.Add(this.CompliedOrder);
            this.ComplishedOrder.ItemLinks.Add(this.barListItem1);
            this.ComplishedOrder.Name = "ComplishedOrder";
            this.ComplishedOrder.Text = "已完成订单";
            // 
            // CompliedOrder
            // 
            this.CompliedOrder.Caption = "已完成订单";
            this.CompliedOrder.Id = 7;
            this.CompliedOrder.Name = "CompliedOrder";
            // 
            // barListItem1
            // 
            this.barListItem1.Caption = "barListItem1";
            this.barListItem1.Id = 14;
            this.barListItem1.Name = "barListItem1";
            this.barListItem1.Strings.AddRange(new object[] {
            "123",
            "123",
            "12344"});
            this.barListItem1.ListItemClick += new DevExpress.XtraBars.ListItemClickEventHandler(this.barListItem1_ListItemClick);
            // 
            // UnplayedOrder
            // 
            this.UnplayedOrder.ItemLinks.Add(this.UplayerOrder);
            this.UnplayedOrder.ItemLinks.Add(this.barButtonGroup1);
            this.UnplayedOrder.Name = "UnplayedOrder";
            this.UnplayedOrder.Text = "未开始订单";
            // 
            // UplayerOrder
            // 
            this.UplayerOrder.Caption = "未开始订单";
            this.UplayerOrder.Id = 8;
            this.UplayerOrder.Name = "UplayerOrder";
            // 
            // barButtonGroup1
            // 
            this.barButtonGroup1.Caption = "组合";
            this.barButtonGroup1.Id = 15;
            this.barButtonGroup1.Name = "barButtonGroup1";
            // 
            // HandlingOrder
            // 
            this.HandlingOrder.ItemLinks.Add(this.HandleOder);
            this.HandlingOrder.Name = "HandlingOrder";
            this.HandlingOrder.Text = "正在处理订单";
            // 
            // HandleOder
            // 
            this.HandleOder.Caption = "正在处理订单";
            this.HandleOder.Id = 6;
            this.HandleOder.Name = "HandleOder";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "功能";
            this.barButtonItem1.Id = 9;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "功能2";
            this.barButtonItem2.Id = 10;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "功能3";
            this.barButtonItem3.Id = 11;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "功能4";
            this.barButtonItem4.Id = 12;
            this.barButtonItem4.Name = "barButtonItem4";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "功能5";
            this.barButtonItem5.Id = 13;
            this.barButtonItem5.Name = "barButtonItem5";
            // 
            // barMdiChildrenListItem1
            // 
            this.barMdiChildrenListItem1.Caption = "barMdiChildrenListItem1";
            this.barMdiChildrenListItem1.Id = 16;
            this.barMdiChildrenListItem1.Name = "barMdiChildrenListItem1";
            // 
            // rucTaskMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "rucTaskMonitor";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem OrderState;
        private DevExpress.XtraBars.Ribbon.RibbonMiniToolbar ribbonMiniToolbar1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup HandlingOrder;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ComplishedOrder;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup UnplayedOrder;
        private DevExpress.XtraBars.BarButtonItem HandleOder;
        private DevExpress.XtraBars.BarButtonItem CompliedOrder;
        private DevExpress.XtraBars.BarButtonItem UplayerOrder;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarListItem barListItem1;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup1;
        private DevExpress.XtraBars.BarMdiChildrenListItem barMdiChildrenListItem1;
    }
}
