namespace QRST_DI_MS_TOOLS_DataImportorUI.TAR.GZ
{
    partial class UCImportTarGZ
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btDeleteData = new System.Windows.Forms.Button();
            this.btStopMonitor = new System.Windows.Forms.Button();
            this.btStartMonitor = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.buttonCertify = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TreeView_type = new System.Windows.Forms.TreeView();
            this.panel = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(711, 475);
            this.tabControl1.TabIndex = 10;
            this.tabControl1.Tag = "";
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btDeleteData);
            this.tabPage1.Controls.Add(this.btStopMonitor);
            this.tabPage1.Controls.Add(this.btStartMonitor);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.buttonOpenFolder);
            this.tabPage1.Controls.Add(this.buttonCertify);
            this.tabPage1.Controls.Add(this.buttonImport);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(703, 449);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "原始数据入库";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btDeleteData
            // 
            this.btDeleteData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeleteData.Location = new System.Drawing.Point(389, 75);
            this.btDeleteData.Name = "btDeleteData";
            this.btDeleteData.Size = new System.Drawing.Size(75, 23);
            this.btDeleteData.TabIndex = 21;
            this.btDeleteData.Text = "删除数据";
            this.btDeleteData.UseVisualStyleBackColor = true;
            this.btDeleteData.Click += new System.EventHandler(this.btDeleteData_Click);
            // 
            // btStopMonitor
            // 
            this.btStopMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStopMonitor.Enabled = false;
            this.btStopMonitor.Location = new System.Drawing.Point(583, 75);
            this.btStopMonitor.Name = "btStopMonitor";
            this.btStopMonitor.Size = new System.Drawing.Size(75, 23);
            this.btStopMonitor.TabIndex = 20;
            this.btStopMonitor.Text = "停止监控";
            this.btStopMonitor.UseVisualStyleBackColor = true;
            this.btStopMonitor.Click += new System.EventHandler(this.btStopMonitor_Click_1);
            // 
            // btStartMonitor
            // 
            this.btStartMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStartMonitor.Location = new System.Drawing.Point(583, 30);
            this.btStartMonitor.Name = "btStartMonitor";
            this.btStartMonitor.Size = new System.Drawing.Size(75, 23);
            this.btStartMonitor.TabIndex = 19;
            this.btStartMonitor.Text = "开始监控";
            this.btStartMonitor.UseVisualStyleBackColor = true;
            this.btStartMonitor.Click += new System.EventHandler(this.btStartMonitor_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(193, 75);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(82, 21);
            this.textBox2.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(503, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "请选择原始数据所在文件夹路径(目前仅支持GF1、GF2、GF4、HJ、SJ9A、ZY3、ZY02C、HJ1C)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "新数据数目：";
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFolder.Location = new System.Drawing.Point(489, 30);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFolder.TabIndex = 11;
            this.buttonOpenFolder.Text = "...";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // buttonCertify
            // 
            this.buttonCertify.Location = new System.Drawing.Point(10, 75);
            this.buttonCertify.Name = "buttonCertify";
            this.buttonCertify.Size = new System.Drawing.Size(75, 23);
            this.buttonCertify.TabIndex = 10;
            this.buttonCertify.Text = "新数据检验";
            this.buttonCertify.UseVisualStyleBackColor = true;
            this.buttonCertify.Click += new System.EventHandler(this.buttonCertify_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(489, 75);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 8;
            this.buttonImport.Text = "开始导入";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(10, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(440, 21);
            this.textBox1.TabIndex = 7;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 126);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(700, 317);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // TreeView_type
            // 
            this.TreeView_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView_type.HideSelection = false;
            this.TreeView_type.LineColor = System.Drawing.Color.Empty;
            this.TreeView_type.Location = new System.Drawing.Point(0, 0);
            this.TreeView_type.Name = "TreeView_type";
            this.TreeView_type.Size = new System.Drawing.Size(256, 435);
            this.TreeView_type.TabIndex = 0;
            this.TreeView_type.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_type_AfterSelect);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.tabControl1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(711, 475);
            this.panel.TabIndex = 0;
            // 
            // UCImportTarGZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Name = "UCImportTarGZ";
            this.Size = new System.Drawing.Size(711, 475);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.Button buttonCertify;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.TextBox textBox1;
        public  System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TreeView TreeView_type;
        private System.Windows.Forms.Button btStartMonitor;
        private System.Windows.Forms.Button btStopMonitor;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btDeleteData;
    }
}

