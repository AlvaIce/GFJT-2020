using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Basis.DataDownLoad;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Metadata.MetaDataCls;
using System.Threading;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class FrmDownLoadLst : DevExpress.XtraEditors.XtraForm
    {
        public bool stopDownload = false;        //判断是否停止下载线程         

        private static FrmDownLoadLst fdL = null;

        private List<DownLoadDataObj> downloadObjLst = new List<DownLoadDataObj>();     //存放所有下载对象

        private List<CtrlDownloadInfo> downloadMonitorLst = new List<CtrlDownloadInfo>();      //下载监控队列
        private ConcurrentQueue<DownLoadDataObj> downLoadDataQueue = new ConcurrentQueue<DownLoadDataObj>(); //要下载的队列

        private List<string> downLoadFileNameList = new List<string>(); //

        static List<Task> MSTaskList = new List<Task>();     //管控下载任务

        private FrmDownLoadLst()
        {
            InitializeComponent();
            timer1.Start();
        }

        public static FrmDownLoadLst GetInstance()
        {
            if (fdL == null || fdL.IsDisposed)
            {
                fdL = new FrmDownLoadLst();
            }

            return fdL;
        }

        /// <summary>
        /// 添加下载任务
        /// </summary>
        public void AddDownLoadTask(List<DownLoadDataObj> _downloadLst)
        {
            //将下载任务添加到监控列表
            downloadObjLst.AddRange(_downloadLst);

            //开启五个下载线程进行下载
            for (int i = MSTaskList.Count; i < 5; i++)
            {
                MSTaskList.Add(System.Threading.Tasks.Task.Factory.StartNew(DownloadData));
            }

        }
        /// <summary>
        /// 添加下载任务 修改 = @ZhangFeiLong，时间：20150126
        /// </summary>
        public void AddDownLoadTaskThreads(List<string> downLoadLst, string destPath)
        {
            downLoadFileNameList = downLoadLst;
            //将需要下载的数据放入的下载队列中
            Thread thread = new Thread(addFilePathToDownLoadQueue);
            thread.Start(destPath);

            //开启五个下载线程进行下载
            for (int i = MSTaskList.Count; i < 5; i++)
            {
                MSTaskList.Add(System.Threading.Tasks.Task.Factory.StartNew(DownloadDataFromQueue));
            }

        }

        /// <summary>
        /// 将需要下载的数据添加到下载队列中 @张飞龙，@20150126
        /// </summary> 
        /// <param name="obj"></param>
        public void addFilePathToDownLoadQueue(object obj)
        {
            string destDir = (string)obj;
            foreach (string fileName in downLoadFileNameList)
            {
                try
                {
                    downLoadDataQueue.Enqueue(new DownLoadDataObj(MetaData.GetDataAddress(fileName), destDir));
                }
                catch (Exception ex)   //若在寻址过程中出现异常，则将空路径作为下载源，在下载列表中则可显示无法下载的数据，zxw 20130905
                {
                    downLoadDataQueue.Enqueue(new DownLoadDataObj("", destDir));
                }
            }

        }

        /// <summary>
        /// 检测下载队列进行下载数据，@张飞龙，@20150126
        /// </summary>
        void DownloadDataFromQueue()
        {
            while (!stopDownload)
            {
                //抓取等待下载的下载对象
                DownLoadDataObj waitingDownloadObj = null;
                CtrlDownloadInfo ctrlDownloadInfo = null;
                lock (this)
                {
                    while (true)
                    {
                        if (!downLoadDataQueue.IsEmpty)
                        {
                            downLoadDataQueue.TryDequeue(out waitingDownloadObj);
                            downloadObjLst.Add(waitingDownloadObj);
                            waitingDownloadObj.status = "开始下载";
                            try
                            {
                                ctrlDownloadInfo = new CtrlDownloadInfo();
                                ctrlDownloadInfo.BindingDownLoadObj(waitingDownloadObj);
                                downloadMonitorLst.Add(ctrlDownloadInfo);                    //将数据添加到监控队列
                                //将下载对象添加到监控列表
                                if (flowLayoutPanelDownloading.IsHandleCreated)
                                {
                                    flowLayoutPanelDownloading.Invoke(new AddMonitorCtrlDel(AddMonitorCtrl), ctrlDownloadInfo);
                                }
                            }
                            catch (Exception ex)  //若错处，则重新将获取的下载对象修改为未下载
                            {
                                waitingDownloadObj.status = "未下载";
                                ctrlDownloadInfo = null;
                            }
                            break;
                        }
                    }
                }
                if (ctrlDownloadInfo != null)
                {
                    ctrlDownloadInfo.StartDownload();
                }
            }
        }

        /// <summary>
        /// <summary>
        /// 下载数据，循环寻找要下载的数据
        /// </summary>
        void DownloadData()
        {
            while (!stopDownload)
            {
                //抓取等待下载的下载对象
                DownLoadDataObj waitingDownloadObj = null;
                CtrlDownloadInfo ctrlDownloadInfo = null;
                lock (this)
                {
                    for (int i = 0; i < downloadObjLst.Count; i++)
                    {
                        if (downloadObjLst[i].status == "未下载")
                        {
                            waitingDownloadObj = downloadObjLst[i];
                            waitingDownloadObj.status = "开始下载";
                            try
                            {
                                ctrlDownloadInfo = new CtrlDownloadInfo();
                                ctrlDownloadInfo.BindingDownLoadObj(waitingDownloadObj);
                                downloadMonitorLst.Add(ctrlDownloadInfo);                    //将数据添加到监控队列
                                //将下载对象添加到监控列表
                                if (flowLayoutPanelDownloading.IsHandleCreated)
                                {
                                    flowLayoutPanelDownloading.Invoke(new AddMonitorCtrlDel(AddMonitorCtrl), ctrlDownloadInfo);
                                }
                            }
                            catch(Exception ex)  //若错处，则重新将获取的下载对象修改为未下载
                            {
                                downloadObjLst[i].status = "未下载";
                                ctrlDownloadInfo = null;
                            }
                            break;
                        }
                    }
                }
                if (ctrlDownloadInfo != null)
                {
                    ctrlDownloadInfo.StartDownload();
                }
            }
        }
        //在显示列表中添加下载对象
        public delegate void AddMonitorCtrlDel(CtrlDownloadInfo _ctrl);

        void AddMonitorCtrl(CtrlDownloadInfo _ctrl)
        {
            if (_ctrl != null)
                flowLayoutPanelDownloading.Controls.Add(_ctrl);
        }

        /// <summary>
        /// 刷新下载任务状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //统计下载情况
            int downloadingCount = 0;
            int faileddownloadCount = 0;
            int finisheddownloadCount = 0;
            int waitingCount = 0;
            for (int j = 0; j < downloadObjLst.Count; j++)
            {
                if (downloadObjLst[j].status == "正在下载")
                {
                    downloadingCount++;
                }
                else if (downloadObjLst[j].status == "下载完成")
                {
                    finisheddownloadCount++;
                }
                else if (downloadObjLst[j].status == "下载失败")
                {
                    faileddownloadCount++;
                }
                else if (downloadObjLst[j].status == "未下载")
                {
                    waitingCount++;
                }
            }

            //lblDownloadInfo.Text = string.Format("下载总任务数：{0}；已下载：{1}；正在下载：{2}；等待下载：{4}；下载失败：{3}", downloadObjLst.Count, finisheddownloadCount, downloadingCount, faileddownloadCount, waitingCount);
            // 修改监控下载信息，@张飞龙，@20150126
            lblDownloadInfo.Text = string.Format("下载总任务数：{0}；已下载：{1}；正在下载：{2}；等待下载：{4}；下载失败：{3}", downLoadFileNameList.Count, finisheddownloadCount, downloadingCount, faileddownloadCount, downLoadFileNameList.Count - finisheddownloadCount - downloadingCount);
            
            for (int i = downloadMonitorLst.Count - 1; i >= 0; i--)
            {
                downloadMonitorLst[i].RefreshDisplayInfo();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}