namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class FrmAddTask
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("内置订单");
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("用户自定义订单");
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("未知类型订单");
			System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("批量订单");
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
			this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textOrderPara = new DevExpress.XtraEditors.TextEdit();
			this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.cmbServerSite = new DevExpress.XtraEditors.ComboBoxEdit();
			this.textOrderName = new DevExpress.XtraEditors.TextEdit();
			this.cmbPriority = new DevExpress.XtraEditors.ComboBoxEdit();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
			this.textOwner = new DevExpress.XtraEditors.TextEdit();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
			this.panelControl1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.textOrderPara.Properties)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbServerSite.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textOrderName.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbPriority.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textOwner.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// groupControl1
			// 
			this.groupControl1.Controls.Add(this.treeView1);
			this.groupControl1.Location = new System.Drawing.Point(4, 12);
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(208, 505);
			this.groupControl1.TabIndex = 0;
			this.groupControl1.Text = "订单列表";
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(2, 23);
			this.treeView1.Name = "treeView1";
			treeNode5.Name = "Installed";
			treeNode5.Text = "内置订单";
			treeNode6.Name = "Customized";
			treeNode6.Text = "用户自定义订单";
			treeNode7.Name = "UnKnown";
			treeNode7.Text = "未知类型订单";
			treeNode8.Name = "Batch";
			treeNode8.Text = "批量订单";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
			this.treeView1.Size = new System.Drawing.Size(204, 480);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(221, 12);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(48, 14);
			this.labelControl1.TabIndex = 1;
			this.labelControl1.Text = "订单类型";
			// 
			// btnConfirm
			// 
			this.btnConfirm.Location = new System.Drawing.Point(457, 492);
			this.btnConfirm.Name = "btnConfirm";
			this.btnConfirm.Size = new System.Drawing.Size(75, 23);
			this.btnConfirm.TabIndex = 2;
			this.btnConfirm.Text = "确定";
			this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
			// 
			// btnCancle
			// 
			this.btnCancle.Location = new System.Drawing.Point(547, 492);
			this.btnCancle.Name = "btnCancle";
			this.btnCancle.Size = new System.Drawing.Size(75, 23);
			this.btnCancle.TabIndex = 3;
			this.btnCancle.Text = "取消";
			this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
			// 
			// panelControl1
			// 
			this.panelControl1.Controls.Add(this.groupBox2);
			this.panelControl1.Controls.Add(this.groupBox1);
			this.panelControl1.Location = new System.Drawing.Point(219, 35);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(403, 451);
			this.panelControl1.TabIndex = 4;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.button1);
			this.groupBox2.Controls.Add(this.textOrderPara);
			this.groupBox2.Controls.Add(this.labelControl8);
			this.groupBox2.Controls.Add(this.labelControl6);
			this.groupBox2.Location = new System.Drawing.Point(5, 238);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(393, 208);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "参数设置";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(327, 59);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(60, 23);
			this.button1.TabIndex = 21;
			this.button1.Text = "设置";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.simpleButton1_Click);
			// 
			// textOrderPara
			// 
			this.textOrderPara.Enabled = false;
			this.textOrderPara.Location = new System.Drawing.Point(74, 62);
			this.textOrderPara.Name = "textOrderPara";
			this.textOrderPara.Size = new System.Drawing.Size(246, 21);
			this.textOrderPara.TabIndex = 20;
			// 
			// labelControl8
			// 
			this.labelControl8.Location = new System.Drawing.Point(28, 27);
			this.labelControl8.Name = "labelControl8";
			this.labelControl8.Size = new System.Drawing.Size(280, 14);
			this.labelControl8.TabIndex = 14;
			this.labelControl8.Text = "订单参数设置与执行订单相关的系统参数,如工作空间";
			// 
			// labelControl6
			// 
			this.labelControl6.Location = new System.Drawing.Point(16, 69);
			this.labelControl6.Name = "labelControl6";
			this.labelControl6.Size = new System.Drawing.Size(48, 14);
			this.labelControl6.TabIndex = 8;
			this.labelControl6.Text = "订单参数";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelControl2);
			this.groupBox1.Controls.Add(this.cmbServerSite);
			this.groupBox1.Controls.Add(this.textOrderName);
			this.groupBox1.Controls.Add(this.cmbPriority);
			this.groupBox1.Controls.Add(this.labelControl3);
			this.groupBox1.Controls.Add(this.labelControl4);
			this.groupBox1.Controls.Add(this.labelControl5);
			this.groupBox1.Controls.Add(this.textOwner);
			this.groupBox1.Location = new System.Drawing.Point(5, 14);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(393, 195);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "订单基本信息";
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(16, 28);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(48, 14);
			this.labelControl2.TabIndex = 0;
			this.labelControl2.Text = "订单名称";
			// 
			// cmbServerSite
			// 
			this.cmbServerSite.Location = new System.Drawing.Point(109, 115);
			this.cmbServerSite.Name = "cmbServerSite";
			this.cmbServerSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cmbServerSite.Size = new System.Drawing.Size(246, 21);
			this.cmbServerSite.TabIndex = 13;
			// 
			// textOrderName
			// 
			this.textOrderName.Location = new System.Drawing.Point(109, 25);
			this.textOrderName.Name = "textOrderName";
			this.textOrderName.Size = new System.Drawing.Size(246, 21);
			this.textOrderName.TabIndex = 1;
			// 
			// cmbPriority
			// 
			this.cmbPriority.Location = new System.Drawing.Point(109, 67);
			this.cmbPriority.Name = "cmbPriority";
			this.cmbPriority.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cmbPriority.Size = new System.Drawing.Size(246, 21);
			this.cmbPriority.TabIndex = 12;
			// 
			// labelControl3
			// 
			this.labelControl3.Location = new System.Drawing.Point(16, 70);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(60, 14);
			this.labelControl3.TabIndex = 2;
			this.labelControl3.Text = "订单优先级";
			// 
			// labelControl4
			// 
			this.labelControl4.Location = new System.Drawing.Point(16, 118);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(48, 14);
			this.labelControl4.TabIndex = 4;
			this.labelControl4.Text = "服务站点";
			// 
			// labelControl5
			// 
			this.labelControl5.Location = new System.Drawing.Point(16, 157);
			this.labelControl5.Name = "labelControl5";
			this.labelControl5.Size = new System.Drawing.Size(36, 14);
			this.labelControl5.TabIndex = 6;
			this.labelControl5.Text = "所有者";
			// 
			// textOwner
			// 
			this.textOwner.Location = new System.Drawing.Point(109, 154);
			this.textOwner.Name = "textOwner";
			this.textOwner.Size = new System.Drawing.Size(246, 21);
			this.textOwner.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 115);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(307, 28);
			this.label1.TabIndex = 22;
			this.label1.Text = "注意：如果参数是源文件，请将其放置于站点能访问到的\r\n共享目录中。如：\\\\192.168.2.101\\Test";
			// 
			// FrmAddTask
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(634, 529);
			this.Controls.Add(this.panelControl1);
			this.Controls.Add(this.btnCancle);
			this.Controls.Add(this.btnConfirm);
			this.Controls.Add(this.labelControl1);
			this.Controls.Add(this.groupControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmAddTask";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "添加新订单";
			this.Load += new System.EventHandler(this.FrmAddTask_Load);
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
			this.panelControl1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.textOrderPara.Properties)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbServerSite.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textOrderName.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbPriority.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textOwner.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.TreeView treeView1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit textOrderName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textOwner;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPriority;
        private DevExpress.XtraEditors.ComboBoxEdit cmbServerSite;
		private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.LabelControl labelControl8;
		private System.Windows.Forms.GroupBox groupBox1;
		private DevExpress.XtraEditors.LabelControl labelControl6;
		private DevExpress.XtraEditors.TextEdit textOrderPara;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
    }
}