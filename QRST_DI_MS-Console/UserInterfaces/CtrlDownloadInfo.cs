using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Basis.DataDownLoad;
using System.IO;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class CtrlDownloadInfo : DevExpress.XtraEditors.XtraUserControl
    {
        private DownLoadDataObj downloadObj;

        public CtrlDownloadInfo()
        {
            InitializeComponent();
        }
        DateTime start =DateTime.Now;
        DateTime endTime = DateTime.Now;


        /// <summary>
        /// 绑定下载对象
        /// </summary>
        public void BindingDownLoadObj(DownLoadDataObj _downloadObj)
        {
            downloadObj = _downloadObj;
            if(downloadObj!=null)
            {
                lblName.Text = downloadObj.GetSrcName();
               
            }
        }

        /// <summary>
        /// 刷新显示信息
        /// </summary>
        public void RefreshDisplayInfo()
        {
            if(downloadObj != null)
            {
                lock (progressBarDownload)
                {
                    try
                    {
                        lblName.Text = downloadObj.GetSrcName();
                        lblDownloadDate.Text = downloadObj.startTime.ToString();
                        progressBarDownload.EditValue = downloadObj.GetProgress();
                        if (downloadObj.status.Equals("正在下载"))
                        {
                            int leftTime = (int)((((downloadObj.dataSize - downloadObj.downloadedSize) / 1024) / downloadObj.DownloadSpeed()));
                            lblDownloadInfo.Text = string.Format("剩余{0}秒-{1}/{2}MB,{3}MB/秒", leftTime, downloadObj.downloadedSize / (1024 * 1024), downloadObj.dataSize / (1024 * 1024), (int)downloadObj.DownloadSpeed() / 1024);
                        }
                        else if (downloadObj.status.Equals("下载完成"))
                        {
                            //lblDownloadInfo.Text = string.Format("{0}MB-{1}", downloadObj.dataSize / (1024 * 1024), downloadObj.destPath);
                            lblDownloadInfo.Text = string.Format("数据大小{0}MB，耗时：{1},平均速度：{2}MB/分", downloadObj.dataSize / (1024 * 1024), endTime - start, (downloadObj.dataSize/(1024*1024))/(endTime-start).TotalMinutes);
                        }
                        else if (downloadObj.status.Equals("下载失败"))
                        {
                            lblDownloadInfo.Text = string.Format("{0}-{1}", downloadObj.status, downloadObj.downLoadMsg);
                            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        return;
                    }
                }
            }
        }

        public string GetDownloadStatus()
        {
            return downloadObj.status;
        }

        public void StartDownload()
        {
            start = DateTime.Now;
            downloadObj.Downdata();
            endTime = DateTime.Now;
        }

        private void contextMenuStripOpenFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(downloadObj.destPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", downloadObj.destPath);
            }
            else
            {
                MessageBox.Show("文件夹不存在或已被删除！");
            }
        }

        private void CtrlDownloadInfo_Click(object sender, EventArgs e)
        {
          
        }

        private Color preBackgroundColor;
        private void CtrlDownloadInfo_MouseEnter(object sender, EventArgs e)
        {
           // preBackgroundColor = this.Appearance.BackColor;
           // this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(253)))), ((int)(((byte)(0)))));
        }

        private void CtrlDownloadInfo_MouseLeave(object sender, EventArgs e)
        {
           // this.Appearance.BackColor = preBackgroundColor;
        }

    }
}
