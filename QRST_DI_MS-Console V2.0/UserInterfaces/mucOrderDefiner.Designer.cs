namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class mucOrderDefiner
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("内置类型");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("用户自定义类型");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("未知类型");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("内置任务");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("自定义任务");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("远程任务");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("未知任务");
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.treeOrderList = new System.Windows.Forms.TreeView();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.hideContainerLeft = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeViewTask = new System.Windows.Forms.TreeView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.hideContainerLeft.SuspendLayout();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.treeOrderList);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(20, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(242, 466);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "订单列表";
            // 
            // treeOrderList
            // 
            this.treeOrderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeOrderList.Location = new System.Drawing.Point(2, 23);
            this.treeOrderList.Name = "treeOrderList";
            treeNode1.Name = "节点0";
            treeNode1.Text = "内置类型";
            treeNode2.Name = "节点1";
            treeNode2.Text = "用户自定义类型";
            treeNode3.Name = "节点2";
            treeNode3.Text = "未知类型";
            this.treeOrderList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.treeOrderList.Size = new System.Drawing.Size(238, 441);
            this.treeOrderList.TabIndex = 0;
            this.treeOrderList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeOrderList_AfterSelect);
            // 
            // dockManager1
            // 
            this.dockManager1.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerLeft});
            this.dockManager1.Form = this;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // hideContainerLeft
            // 
            this.hideContainerLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.hideContainerLeft.Controls.Add(this.dockPanel1);
            this.hideContainerLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.hideContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.hideContainerLeft.Name = "hideContainerLeft";
            this.hideContainerLeft.Size = new System.Drawing.Size(20, 466);
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.ID = new System.Guid("6fe5043d-4bff-429a-b703-f9288569d496");
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.SavedIndex = 0;
            this.dockPanel1.Size = new System.Drawing.Size(200, 466);
            this.dockPanel1.Text = "任务库";
            this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeViewTask);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(192, 439);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeViewTask
            // 
            this.treeViewTask.AllowDrop = true;
            this.treeViewTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTask.Location = new System.Drawing.Point(0, 0);
            this.treeViewTask.Name = "treeViewTask";
            treeNode4.Name = "节点9";
            treeNode4.Text = "内置任务";
            treeNode5.Name = "节点10";
            treeNode5.Text = "自定义任务";
            treeNode6.Name = "节点11";
            treeNode6.Text = "远程任务";
            treeNode7.Name = "节点12";
            treeNode7.Text = "未知任务";
            this.treeViewTask.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7});
            this.treeViewTask.Size = new System.Drawing.Size(192, 439);
            this.treeViewTask.TabIndex = 0;
            this.treeViewTask.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewTask_ItemDrag);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AllowDrop = true;
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 23);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(534, 441);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.flowLayoutPanel1_ControlRemoved);
            this.flowLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanel1_DragDrop);
            this.flowLayoutPanel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanel1_DragEnter);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.flowLayoutPanel1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(262, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(538, 466);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "订单工作流定制";
            // 
            // mucOrderDefiner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.hideContainerLeft);
            this.Name = "mucOrderDefiner";
            this.Size = new System.Drawing.Size(800, 466);
            this.VisibleChanged += new System.EventHandler(this.mucOrderDefiner_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.hideContainerLeft.ResumeLayout(false);
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private System.Windows.Forms.TreeView treeViewTask;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        public System.Windows.Forms.TreeView treeOrderList;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerLeft;


    }
}
