namespace QRST_DI_DataImportTool
{
    partial class Form1
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDataBaseState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripFileServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripFileServerIP = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDataBase = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.ctrlCommonDataImport1 = new QRST_DI_DataImportTool.ctrlCommonDataImport();
            this.ctrlCommonDataImport3 = new QRST_DI_DataImportTool.ctrlCommonDataImport();
            this.ctrlCommonDataImport2 = new QRST_DI_DataImportTool.ctrlCommonDataImport();
            this.ctrlCommonDataImport4 = new QRST_DI_DataImportTool.ctrlCommonDataImport();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDataBaseState,
            this.toolStripFileServer,
            this.toolStripProgressBar1,
            this.toolStripFileServerIP,
            this.toolStripDataBase,
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 497);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(958, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDataBaseState
            // 
            this.toolStripDataBaseState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDataBaseState.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripDataBaseState.ForeColor = System.Drawing.Color.Red;
            this.toolStripDataBaseState.Name = "toolStripDataBaseState";
            this.toolStripDataBaseState.Size = new System.Drawing.Size(140, 17);
            this.toolStripDataBaseState.Text = "数据库连接状态：未连接";
            // 
            // toolStripFileServer
            // 
            this.toolStripFileServer.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripFileServer.ForeColor = System.Drawing.Color.Red;
            this.toolStripFileServer.Name = "toolStripFileServer";
            this.toolStripFileServer.Size = new System.Drawing.Size(143, 17);
            this.toolStripFileServer.Text = "文件系统连接状态:未连接";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripFileServerIP
            // 
            this.toolStripFileServerIP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripFileServerIP.ForeColor = System.Drawing.Color.Red;
            this.toolStripFileServerIP.Name = "toolStripFileServerIP";
            this.toolStripFileServerIP.Size = new System.Drawing.Size(104, 17);
            this.toolStripFileServerIP.Text = "文件服务器地址：";
            // 
            // toolStripDataBase
            // 
            this.toolStripDataBase.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripDataBase.ForeColor = System.Drawing.Color.Red;
            this.toolStripDataBase.Name = "toolStripDataBase";
            this.toolStripDataBase.Size = new System.Drawing.Size(116, 17);
            this.toolStripDataBase.Text = "数据库服务器地址：";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(958, 497);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ctrlCommonDataImport1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(950, 468);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "矢量数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ctrlCommonDataImport3);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(950, 468);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "用户文档";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ctrlCommonDataImport2);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(950, 468);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "栅格数据";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.ctrlCommonDataImport4);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(950, 468);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "用户工具";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // ctrlCommonDataImport1
            // 
            this.ctrlCommonDataImport1.dataType = QRST_DI_DataImportTool.DataImport.DataType.UserDocument;
            this.ctrlCommonDataImport1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport1.IsSingleImport = false;
            this.ctrlCommonDataImport1.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport1.Name = "ctrlCommonDataImport1";
            this.ctrlCommonDataImport1.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport1.TabIndex = 0;
            // 
            // ctrlCommonDataImport3
            // 
            this.ctrlCommonDataImport3.dataType = QRST_DI_DataImportTool.DataImport.DataType.UserDocument;
            this.ctrlCommonDataImport3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport3.IsSingleImport = false;
            this.ctrlCommonDataImport3.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport3.Name = "ctrlCommonDataImport3";
            this.ctrlCommonDataImport3.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport3.TabIndex = 0;
            // 
            // ctrlCommonDataImport2
            // 
            this.ctrlCommonDataImport2.dataType = QRST_DI_DataImportTool.DataImport.DataType.UserDocument;
            this.ctrlCommonDataImport2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport2.IsSingleImport = false;
            this.ctrlCommonDataImport2.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport2.Name = "ctrlCommonDataImport2";
            this.ctrlCommonDataImport2.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport2.TabIndex = 0;
            // 
            // ctrlCommonDataImport4
            // 
            this.ctrlCommonDataImport4.dataType = QRST_DI_DataImportTool.DataImport.DataType.UserDocument;
            this.ctrlCommonDataImport4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport4.IsSingleImport = false;
            this.ctrlCommonDataImport4.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport4.Name = "ctrlCommonDataImport4";
            this.ctrlCommonDataImport4.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport4.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 519);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据入库工具";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDataBaseState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripFileServer;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private ctrlCommonDataImport ctrlCommonDataImport1;
        private ctrlCommonDataImport ctrlCommonDataImport3;
        private ctrlCommonDataImport ctrlCommonDataImport2;
        private ctrlCommonDataImport ctrlCommonDataImport4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripFileServerIP;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDataBase;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

