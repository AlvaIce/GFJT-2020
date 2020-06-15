namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class FrmDownLoadLst
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
            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            //base.Dispose(disposing);
            this.Hide();
          
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.flowLayoutPanelDownloading = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDownloadInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(676, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // flowLayoutPanelDownloading
            // 
            this.flowLayoutPanelDownloading.AutoScroll = true;
            this.flowLayoutPanelDownloading.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanelDownloading.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelDownloading.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelDownloading.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelDownloading.Name = "flowLayoutPanelDownloading";
            this.flowLayoutPanelDownloading.Size = new System.Drawing.Size(772, 386);
            this.flowLayoutPanelDownloading.TabIndex = 4;
            // 
            // lblDownloadInfo
            // 
            this.lblDownloadInfo.AutoSize = true;
            this.lblDownloadInfo.Location = new System.Drawing.Point(12, 401);
            this.lblDownloadInfo.Name = "lblDownloadInfo";
            this.lblDownloadInfo.Size = new System.Drawing.Size(55, 14);
            this.lblDownloadInfo.TabIndex = 5;
            this.lblDownloadInfo.Text = "下载信息";
            // 
            // FrmDownLoadLst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 424);
            this.Controls.Add(this.lblDownloadInfo);
            this.Controls.Add(this.flowLayoutPanelDownloading);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDownLoadLst";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据下载列表";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDownloading;
        private System.Windows.Forms.Label lblDownloadInfo;
    }
}