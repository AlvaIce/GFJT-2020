namespace QRST_DI_TS_Server
{
    partial class ConsoleForm
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
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.labelRemoteStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelExecuteState = new System.Windows.Forms.Label();
            this.btnStopRemoteServer = new System.Windows.Forms.Button();
            this.btnStartRemoteServer = new System.Windows.Forms.Button();
            this.btnStopExecuteTask = new System.Windows.Forms.Button();
            this.btnExecuteTask = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "QRST_DI综合数据库TileServerSite终端服务";
            this.notifyIcon1.Visible = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "远程对象服务运行状态:";
            // 
            // labelRemoteStatus
            // 
            this.labelRemoteStatus.AutoSize = true;
            this.labelRemoteStatus.Location = new System.Drawing.Point(160, 25);
            this.labelRemoteStatus.Name = "labelRemoteStatus";
            this.labelRemoteStatus.Size = new System.Drawing.Size(41, 12);
            this.labelRemoteStatus.TabIndex = 2;
            this.labelRemoteStatus.Text = "未启动";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "任务执行状态:";
            // 
            // labelExecuteState
            // 
            this.labelExecuteState.AutoSize = true;
            this.labelExecuteState.Location = new System.Drawing.Point(160, 51);
            this.labelExecuteState.Name = "labelExecuteState";
            this.labelExecuteState.Size = new System.Drawing.Size(41, 12);
            this.labelExecuteState.TabIndex = 4;
            this.labelExecuteState.Text = "未执行";
            // 
            // btnStopRemoteServer
            // 
            this.btnStopRemoteServer.Location = new System.Drawing.Point(117, 103);
            this.btnStopRemoteServer.Name = "btnStopRemoteServer";
            this.btnStopRemoteServer.Size = new System.Drawing.Size(99, 41);
            this.btnStopRemoteServer.TabIndex = 5;
            this.btnStopRemoteServer.Text = "停止远程对象";
            this.btnStopRemoteServer.UseVisualStyleBackColor = true;
            this.btnStopRemoteServer.Click += new System.EventHandler(this.btnStopRemoteServer_Click);
            // 
            // btnStartRemoteServer
            // 
            this.btnStartRemoteServer.Location = new System.Drawing.Point(12, 103);
            this.btnStartRemoteServer.Name = "btnStartRemoteServer";
            this.btnStartRemoteServer.Size = new System.Drawing.Size(99, 41);
            this.btnStartRemoteServer.TabIndex = 6;
            this.btnStartRemoteServer.Text = "开启远程对象";
            this.btnStartRemoteServer.UseVisualStyleBackColor = true;
            this.btnStartRemoteServer.Click += new System.EventHandler(this.btnStartRemoteServer_Click);
            // 
            // btnStopExecuteTask
            // 
            this.btnStopExecuteTask.Location = new System.Drawing.Point(327, 103);
            this.btnStopExecuteTask.Name = "btnStopExecuteTask";
            this.btnStopExecuteTask.Size = new System.Drawing.Size(99, 41);
            this.btnStopExecuteTask.TabIndex = 7;
            this.btnStopExecuteTask.Text = "暂停任务";
            this.btnStopExecuteTask.UseVisualStyleBackColor = true;
            this.btnStopExecuteTask.Click += new System.EventHandler(this.btnStopExecuteTask_Click);
            // 
            // btnExecuteTask
            // 
            this.btnExecuteTask.Location = new System.Drawing.Point(222, 103);
            this.btnExecuteTask.Name = "btnExecuteTask";
            this.btnExecuteTask.Size = new System.Drawing.Size(99, 41);
            this.btnExecuteTask.TabIndex = 8;
            this.btnExecuteTask.Text = "执行任务";
            this.btnExecuteTask.UseVisualStyleBackColor = true;
            this.btnExecuteTask.Click += new System.EventHandler(this.btnExecuteTask_Click);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 188);
            this.Controls.Add(this.btnExecuteTask);
            this.Controls.Add(this.btnStopExecuteTask);
            this.Controls.Add(this.btnStartRemoteServer);
            this.Controls.Add(this.btnStopRemoteServer);
            this.Controls.Add(this.labelExecuteState);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelRemoteStatus);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsoleForm";
            this.ShowInTaskbar = false;
            this.Text = "QRST_DI综合数据库TileServerSite终端服务";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelRemoteStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelExecuteState;
        private System.Windows.Forms.Button btnStopRemoteServer;
        private System.Windows.Forms.Button btnStartRemoteServer;
        private System.Windows.Forms.Button btnStopExecuteTask;
        private System.Windows.Forms.Button btnExecuteTask;
    }
}

