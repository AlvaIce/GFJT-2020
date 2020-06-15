namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class mucUserManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mucUserManager));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.treeViewUserList = new System.Windows.Forms.TreeView();
            this.checkedListBoxControlRole = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.checkedListBoxControlFunction = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.listBoxControlSystem = new DevExpress.XtraEditors.ListBoxControl();
            this.textEditRoleName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlRole)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlFunction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlSystem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditRoleName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.treeViewUserList);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(231, 452);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "用户列表";
            // 
            // treeViewUserList
            // 
            this.treeViewUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewUserList.ImageIndex = 0;
            this.treeViewUserList.ImageList = this.imageList1;
            this.treeViewUserList.Location = new System.Drawing.Point(2, 25);
            this.treeViewUserList.Name = "treeViewUserList";
            this.treeViewUserList.SelectedImageIndex = 0;
            this.treeViewUserList.Size = new System.Drawing.Size(227, 425);
            this.treeViewUserList.TabIndex = 0;
            this.treeViewUserList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewUserList_AfterSelect);
            // 
            // checkedListBoxControlRole
            // 
            this.checkedListBoxControlRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxControlRole.Location = new System.Drawing.Point(3, 19);
            this.checkedListBoxControlRole.Name = "checkedListBoxControlRole";
            this.checkedListBoxControlRole.Size = new System.Drawing.Size(337, 430);
            this.checkedListBoxControlRole.TabIndex = 2;
            this.checkedListBoxControlRole.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checkedListBoxControlRole_ItemCheck);
            this.checkedListBoxControlRole.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxControlRole_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBoxControlRole);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 452);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "请为用户选择一个或多个角色";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelControl3);
            this.groupBox2.Controls.Add(this.labelControl2);
            this.groupBox2.Controls.Add(this.checkedListBoxControlFunction);
            this.groupBox2.Controls.Add(this.listBoxControlSystem);
            this.groupBox2.Controls.Add(this.textEditRoleName);
            this.groupBox2.Controls.Add(this.labelControl1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(342, 452);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "角色信息";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(19, 257);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "子模块：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(19, 77);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "子系统：";
            // 
            // checkedListBoxControlFunction
            // 
            this.checkedListBoxControlFunction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxControlFunction.Location = new System.Drawing.Point(19, 277);
            this.checkedListBoxControlFunction.Name = "checkedListBoxControlFunction";
            this.checkedListBoxControlFunction.Size = new System.Drawing.Size(303, 142);
            this.checkedListBoxControlFunction.TabIndex = 3;
            this.checkedListBoxControlFunction.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checkedListBoxControlFunction_ItemCheck);
            // 
            // listBoxControlSystem
            // 
            this.listBoxControlSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxControlSystem.Location = new System.Drawing.Point(19, 97);
            this.listBoxControlSystem.Name = "listBoxControlSystem";
            this.listBoxControlSystem.Size = new System.Drawing.Size(303, 142);
            this.listBoxControlSystem.TabIndex = 2;
            this.listBoxControlSystem.SelectedIndexChanged += new System.EventHandler(this.listBoxControlSystem_SelectedIndexChanged);
            // 
            // textEditRoleName
            // 
            this.textEditRoleName.Location = new System.Drawing.Point(85, 31);
            this.textEditRoleName.Name = "textEditRoleName";
            this.textEditRoleName.Size = new System.Drawing.Size(222, 21);
            this.textEditRoleName.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "角色名称：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(231, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(689, 452);
            this.splitContainer1.SplitterDistance = 343;
            this.splitContainer1.TabIndex = 5;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "用户.png");
            this.imageList1.Images.SetKeyName(1, "角色保存.png");
            // 
            // mucUserManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupControl1);
            this.Name = "mucUserManager";
            this.Size = new System.Drawing.Size(920, 452);
            this.VisibleChanged += new System.EventHandler(this.mucUserManager_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlRole)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlFunction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlSystem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditRoleName.Properties)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        public System.Windows.Forms.TreeView treeViewUserList;
        public DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControlRole;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControlFunction;
        private DevExpress.XtraEditors.ListBoxControl listBoxControlSystem;
        public DevExpress.XtraEditors.TextEdit textEditRoleName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.ImageList imageList1;
    }
}
