namespace QRST_DI_MS_Component.Common
{
    partial class CtrlDownloadInfo
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
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDownloadInfo = new System.Windows.Forms.Label();
            this.lblDownloadDate = new System.Windows.Forms.Label();
            this.contextMenuStripOpenFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开下载目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureEdit1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStripOpenFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Location = new System.Drawing.Point(57, 21);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(580, 15);
            this.progressBarDownload.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(57, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(449, 12);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "下载文件名";
            // 
            // lblDownloadInfo
            // 
            this.lblDownloadInfo.Location = new System.Drawing.Point(57, 41);
            this.lblDownloadInfo.Name = "lblDownloadInfo";
            this.lblDownloadInfo.Size = new System.Drawing.Size(580, 12);
            this.lblDownloadInfo.TabIndex = 3;
            this.lblDownloadInfo.Text = "下载信息";
            // 
            // lblDownloadDate
            // 
            this.lblDownloadDate.Location = new System.Drawing.Point(512, 3);
            this.lblDownloadDate.Name = "lblDownloadDate";
            this.lblDownloadDate.Size = new System.Drawing.Size(125, 12);
            this.lblDownloadDate.TabIndex = 4;
            this.lblDownloadDate.Text = "下载时间";
            // 
            // contextMenuStripOpenFolder
            // 
            this.contextMenuStripOpenFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开下载目录ToolStripMenuItem});
            this.contextMenuStripOpenFolder.Name = "contextMenuStripOpenFolder";
            this.contextMenuStripOpenFolder.Size = new System.Drawing.Size(149, 26);
            this.contextMenuStripOpenFolder.Click += new System.EventHandler(this.contextMenuStripOpenFolder_Click);
            // 
            // 打开下载目录ToolStripMenuItem
            // 
            this.打开下载目录ToolStripMenuItem.Name = "打开下载目录ToolStripMenuItem";
            this.打开下载目录ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.打开下载目录ToolStripMenuItem.Text = "打开下载目录";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Image = global::QRST_DI_MS_Component.Properties.Resources.文件夹;
            this.pictureEdit1.Location = new System.Drawing.Point(11, 10);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(40, 34);
            this.pictureEdit1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureEdit1.TabIndex = 0;
            this.pictureEdit1.TabStop = false;
            // 
            // CtrlDownloadInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ContextMenuStrip = this.contextMenuStripOpenFolder;
            this.Controls.Add(this.lblDownloadDate);
            this.Controls.Add(this.lblDownloadInfo);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.progressBarDownload);
            this.Controls.Add(this.pictureEdit1);
            this.Name = "CtrlDownloadInfo";
            this.Size = new System.Drawing.Size(644, 56);
            this.Click += new System.EventHandler(this.CtrlDownloadInfo_Click);
            this.MouseEnter += new System.EventHandler(this.CtrlDownloadInfo_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.CtrlDownloadInfo_MouseLeave);
            this.contextMenuStripOpenFolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureEdit1;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDownloadInfo;
        private System.Windows.Forms.Label lblDownloadDate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem 打开下载目录ToolStripMenuItem;
    }
}
