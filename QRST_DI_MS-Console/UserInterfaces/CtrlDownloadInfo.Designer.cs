namespace QRST_DI_MS_Console.UserInterfaces
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
			this.progressBarDownload = new DevExpress.XtraEditors.ProgressBarControl();
			this.lblName = new DevExpress.XtraEditors.LabelControl();
			this.lblDownloadInfo = new DevExpress.XtraEditors.LabelControl();
			this.lblDownloadDate = new DevExpress.XtraEditors.LabelControl();
			this.contextMenuStripOpenFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.打开下载目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
			((System.ComponentModel.ISupportInitialize)(this.progressBarDownload.Properties)).BeginInit();
			this.contextMenuStripOpenFolder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// progressBarDownload
			// 
			this.progressBarDownload.Location = new System.Drawing.Point(66, 24);
			this.progressBarDownload.Name = "progressBarDownload";
			this.progressBarDownload.Size = new System.Drawing.Size(677, 18);
			this.progressBarDownload.TabIndex = 1;
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(66, 4);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(60, 14);
			this.lblName.TabIndex = 2;
			this.lblName.Text = "下载文件名";
			// 
			// lblDownloadInfo
			// 
			this.lblDownloadInfo.Location = new System.Drawing.Point(66, 48);
			this.lblDownloadInfo.Name = "lblDownloadInfo";
			this.lblDownloadInfo.Size = new System.Drawing.Size(48, 14);
			this.lblDownloadInfo.TabIndex = 3;
			this.lblDownloadInfo.Text = "下载信息";
			// 
			// lblDownloadDate
			// 
			this.lblDownloadDate.Location = new System.Drawing.Point(597, 3);
			this.lblDownloadDate.Name = "lblDownloadDate";
			this.lblDownloadDate.Size = new System.Drawing.Size(48, 14);
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
			this.pictureEdit1.EditValue = global::QRST_DI_MS_Console.Properties.Resources.文件夹;
			this.pictureEdit1.Location = new System.Drawing.Point(13, 12);
			this.pictureEdit1.Name = "pictureEdit1";
			this.pictureEdit1.Properties.ReadOnly = true;
			this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
			this.pictureEdit1.Size = new System.Drawing.Size(47, 40);
			this.pictureEdit1.TabIndex = 0;
			// 
			// CtrlDownloadInfo
			// 
			this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.Appearance.Options.UseBackColor = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ContextMenuStrip = this.contextMenuStripOpenFolder;
			this.Controls.Add(this.lblDownloadDate);
			this.Controls.Add(this.lblDownloadInfo);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.progressBarDownload);
			this.Controls.Add(this.pictureEdit1);
			this.Name = "CtrlDownloadInfo";
			this.Size = new System.Drawing.Size(762, 65);
			this.Click += new System.EventHandler(this.CtrlDownloadInfo_Click);
			this.MouseEnter += new System.EventHandler(this.CtrlDownloadInfo_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.CtrlDownloadInfo_MouseLeave);
			((System.ComponentModel.ISupportInitialize)(this.progressBarDownload.Properties)).EndInit();
			this.contextMenuStripOpenFolder.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.ProgressBarControl progressBarDownload;
        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraEditors.LabelControl lblDownloadInfo;
        private DevExpress.XtraEditors.LabelControl lblDownloadDate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem 打开下载目录ToolStripMenuItem;
    }
}
