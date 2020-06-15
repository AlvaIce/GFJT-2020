namespace QRST_DI_TS_Server
{
    partial class Form1
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
            this.btnExecuteTask = new System.Windows.Forms.Button();
            this.btnStartRemoteServer = new System.Windows.Forms.Button();
            this.labelExecuteState = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelRemoteStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSiteIP = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnConnection2SQL = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelDbIP = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelDataStorePath = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTcpSSPort = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelDbState = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textDbIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textUserName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textTcpSSPort = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textDataStorePath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textSiteIP = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecuteTask
            // 
            this.btnExecuteTask.Enabled = false;
            this.btnExecuteTask.Location = new System.Drawing.Point(241, 335);
            this.btnExecuteTask.Name = "btnExecuteTask";
            this.btnExecuteTask.Size = new System.Drawing.Size(106, 29);
            this.btnExecuteTask.TabIndex = 16;
            this.btnExecuteTask.Text = "执行任务";
            this.btnExecuteTask.UseVisualStyleBackColor = true;
            this.btnExecuteTask.Click += new System.EventHandler(this.btnExecuteTask_Click);
            // 
            // btnStartRemoteServer
            // 
            this.btnStartRemoteServer.Enabled = false;
            this.btnStartRemoteServer.Location = new System.Drawing.Point(130, 335);
            this.btnStartRemoteServer.Name = "btnStartRemoteServer";
            this.btnStartRemoteServer.Size = new System.Drawing.Size(106, 29);
            this.btnStartRemoteServer.TabIndex = 14;
            this.btnStartRemoteServer.Text = "开启远程对象";
            this.btnStartRemoteServer.UseVisualStyleBackColor = true;
            this.btnStartRemoteServer.Click += new System.EventHandler(this.btnStartRemoteServer_Click);
            // 
            // labelExecuteState
            // 
            this.labelExecuteState.AutoSize = true;
            this.labelExecuteState.Location = new System.Drawing.Point(154, 53);
            this.labelExecuteState.Name = "labelExecuteState";
            this.labelExecuteState.Size = new System.Drawing.Size(41, 12);
            this.labelExecuteState.TabIndex = 12;
            this.labelExecuteState.Text = "未执行";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "任务执行状态:";
            // 
            // labelRemoteStatus
            // 
            this.labelRemoteStatus.AutoSize = true;
            this.labelRemoteStatus.Location = new System.Drawing.Point(154, 27);
            this.labelRemoteStatus.Name = "labelRemoteStatus";
            this.labelRemoteStatus.Size = new System.Drawing.Size(41, 12);
            this.labelRemoteStatus.TabIndex = 10;
            this.labelRemoteStatus.Text = "未启动";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "远程对象服务运行状态:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "站点IP:";
            // 
            // labelSiteIP
            // 
            this.labelSiteIP.AutoSize = true;
            this.labelSiteIP.Location = new System.Drawing.Point(154, 26);
            this.labelSiteIP.Name = "labelSiteIP";
            this.labelSiteIP.Size = new System.Drawing.Size(17, 12);
            this.labelSiteIP.TabIndex = 18;
            this.labelSiteIP.Text = "40";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(363, 398);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnConnection2SQL);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.btnExecuteTask);
            this.tabPage1.Controls.Add(this.btnStartRemoteServer);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(355, 372);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "站点状态";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnConnection2SQL
            // 
            this.btnConnection2SQL.Location = new System.Drawing.Point(18, 335);
            this.btnConnection2SQL.Name = "btnConnection2SQL";
            this.btnConnection2SQL.Size = new System.Drawing.Size(106, 29);
            this.btnConnection2SQL.TabIndex = 21;
            this.btnConnection2SQL.Text = "连接数据库";
            this.btnConnection2SQL.UseVisualStyleBackColor = true;
            this.btnConnection2SQL.Click += new System.EventHandler(this.btnConnection2SQL_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelDbIP);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.labelDataStorePath);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.labelTcpSSPort);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.labelSiteIP);
            this.groupBox2.Location = new System.Drawing.Point(12, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 157);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "基本信息";
            // 
            // labelDbIP
            // 
            this.labelDbIP.AutoSize = true;
            this.labelDbIP.Location = new System.Drawing.Point(156, 112);
            this.labelDbIP.Name = "labelDbIP";
            this.labelDbIP.Size = new System.Drawing.Size(77, 12);
            this.labelDbIP.TabIndex = 24;
            this.labelDbIP.Text = "172.16.0.185";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 112);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 12);
            this.label14.TabIndex = 23;
            this.label14.Text = "数据库服务器地址:";
            // 
            // labelDataStorePath
            // 
            this.labelDataStorePath.AutoSize = true;
            this.labelDataStorePath.Location = new System.Drawing.Point(156, 79);
            this.labelDataStorePath.Name = "labelDataStorePath";
            this.labelDataStorePath.Size = new System.Drawing.Size(107, 12);
            this.labelDataStorePath.TabIndex = 22;
            this.labelDataStorePath.Text = "数据阵列映射地址:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "数据阵列映射地址:";
            // 
            // labelTcpSSPort
            // 
            this.labelTcpSSPort.AutoSize = true;
            this.labelTcpSSPort.Location = new System.Drawing.Point(154, 50);
            this.labelTcpSSPort.Name = "labelTcpSSPort";
            this.labelTcpSSPort.Size = new System.Drawing.Size(77, 12);
            this.labelTcpSSPort.TabIndex = 20;
            this.labelTcpSSPort.Text = "TCP通信端口:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "TCP通信端口:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelDbState);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelExecuteState);
            this.groupBox1.Controls.Add(this.labelRemoteStatus);
            this.groupBox1.Location = new System.Drawing.Point(12, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 160);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务状态";
            // 
            // labelDbState
            // 
            this.labelDbState.AutoSize = true;
            this.labelDbState.Location = new System.Drawing.Point(154, 81);
            this.labelDbState.Name = "labelDbState";
            this.labelDbState.Size = new System.Drawing.Size(41, 12);
            this.labelDbState.TabIndex = 15;
            this.labelDbState.Text = "未连接";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 14;
            this.label13.Text = "数据库连接状态：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(86, 81);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 12);
            this.label12.TabIndex = 13;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(355, 372);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(187, 333);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 33);
            this.button1.TabIndex = 27;
            this.button1.Text = "测试连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textDbIP);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.textPassword);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.textUserName);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(11, 153);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(338, 159);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "数据库连接参数信息";
            // 
            // textDbIP
            // 
            this.textDbIP.Location = new System.Drawing.Point(127, 29);
            this.textDbIP.Name = "textDbIP";
            this.textDbIP.Size = new System.Drawing.Size(203, 21);
            this.textDbIP.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "数据库服务器:";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(129, 103);
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.Size = new System.Drawing.Size(203, 21);
            this.textPassword.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(76, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "密码:";
            // 
            // textUserName
            // 
            this.textUserName.Location = new System.Drawing.Point(129, 70);
            this.textUserName.Name = "textUserName";
            this.textUserName.Size = new System.Drawing.Size(203, 21);
            this.textUserName.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(64, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "用户名:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(271, 333);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 33);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存修改";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textTcpSSPort);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textDataStorePath);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textSiteIP);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(9, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(338, 140);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "基本参数设置";
            // 
            // textTcpSSPort
            // 
            this.textTcpSSPort.Location = new System.Drawing.Point(129, 97);
            this.textTcpSSPort.Name = "textTcpSSPort";
            this.textTcpSSPort.Size = new System.Drawing.Size(203, 21);
            this.textTcpSSPort.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 97);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "TCP通信端口:";
            // 
            // textDataStorePath
            // 
            this.textDataStorePath.Location = new System.Drawing.Point(129, 63);
            this.textDataStorePath.Name = "textDataStorePath";
            this.textDataStorePath.Size = new System.Drawing.Size(203, 21);
            this.textDataStorePath.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "数据阵列映射地址:";
            // 
            // textSiteIP
            // 
            this.textSiteIP.Location = new System.Drawing.Point(129, 30);
            this.textSiteIP.Name = "textSiteIP";
            this.textSiteIP.Size = new System.Drawing.Size(203, 21);
            this.textSiteIP.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(66, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "站点IP:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 398);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "分布式服务站点";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExecuteTask;
        private System.Windows.Forms.Button btnStartRemoteServer;
        private System.Windows.Forms.Label labelExecuteState;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelRemoteStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSiteIP;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelTcpSSPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelDataStorePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textTcpSSPort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textDataStorePath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textSiteIP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textUserName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textDbIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnConnection2SQL;
        private System.Windows.Forms.Label labelDbState;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelDbIP;
        private System.Windows.Forms.Label label14;

    }
}