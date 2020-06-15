using System;
using System.Data;
using System.Windows.Forms;
using QRST_DI_TS_Process.Site;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using System.Threading;
using System.Threading.Tasks;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucTSSiterManager : DevExpress.XtraEditors.XtraUserControl
    {
        bool isFirstLoad = true;
        private System.Threading.Timer ctrlTimer;
        public static bool[] visibleArr;

        private int serverSiteIndex;  //用于存储正在刷新的站点索引
        Task[] tasks;                         //用于执行站点监控的任务

        public mucTSSiterManager()
        {
            InitializeComponent();
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
        }

        private void mucTSSiterManager_Load(object sender, EventArgs e)
        {

        }

        private void gridControlNodeLst_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 第一次加载时，初始化组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mucTSSiterManager_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (isFirstLoad)
                {
                    using (WaitDialogForm dlg = new WaitDialogForm("请稍后", "正在初始化组件"))
                    {
                        InitializeControl();
                    }
                    isFirstLoad = false;
                }
                MonitorInfo.stopped = false;
                Task.Factory.StartNew(() => { MonitorInfo.GetAllSiteInfo(); });
                timer1.Start();
            }
            else
            {
                MonitorInfo.stopped = true;
                timer1.Stop();
            }

        }

        public void InitializeControl()
        {
            checkedListBoxSiteLst.Items.Clear();
            //    gridControlNodeLst.DataSource = TServerSiteMoniter.GetBasicSiteInfo();

            //初始化所有节点可见
            visibleArr = new bool[TServerSiteManager.StorageSites.Count];

            for (int i = 0 ; i < TServerSiteManager.StorageSites.Count ; i++)
            {
                checkedListBoxSiteLst.Items.Add(TServerSiteManager.StorageSites[i].SiteName);
                checkedListBoxSiteLst.Items[i].CheckState = CheckState.Checked;

                visibleArr[i] = true;

                //初始化chart，如果没有运行则初始化为0
                //if (((DataTable)gridControlNodeLst.DataSource).Rows[i]["RunningState"] == "正在运行")
                //{
                //      chartMemory.Series["SeriesUsedMemory"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, TServerSiteManager.StorageSites[i].Status.GetDivLoadSize()));
                //      chartMemory.Series["SeriesUnusedMemory"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, TServerSiteManager.StorageSites[i].Status.GetDivAvalableFreeSpace()));

                //      double memPercent = TServerSiteManager.StorageSites[i].Status.TotalPhysicalMemory;
                //      memPercent = ((memPercent - TServerSiteManager.StorageSites[i].Status.AvailPhys) / memPercent) * 100;
                //      double cpuPercent = TServerSiteManager.StorageSites[i].Status.CPU;

                //      chartCPURate.Series["SeriesMemRate"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, memPercent));
                //      chartCPURate.Series["SeriesCPURate"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, cpuPercent));
                //}
                //else
                //{
                //    chartMemory.Series["SeriesUsedMemory"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, 0));
                //    chartMemory.Series["SeriesUnusedMemory"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, 0));

                //    chartCPURate.Series["SeriesMemRate"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, 0));
                //    chartCPURate.Series["SeriesCPURate"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, 0));
                //}

            }
            //默认选中第一个节点，将信息显示在信息框中
            if (checkedListBoxSiteLst.Items.Count != 0)
            {
                checkedListBoxSiteLst.SelectedIndex = 0;
                //RefreshText(TServerSiteManager.StorageSites[0]);
                // RefreshText(namenodelst[0]);
            }

        }

        //使用委托，设置主线程中控件textBox的值
        private delegate void SetTextDelegate(TServerSite site);


        //void RefreshText(TServerSite site)
        //{
        //    if (((DataTable)gridControlNodeLst.DataSource).Rows[checkedListBoxSiteLst.SelectedIndex]["RunningState"] == "正在运行")
        //    {
        //        try
        //        {
        //            if (this.InvokeRequired)
        //            {
        //                SetTextDelegate d = new SetTextDelegate(RefreshText);
        //                this.BeginInvoke(d,site);
        //            }
        //            else
        //            {
        //                  textDiskSize.Text = site.Status.GetDivToatalsize().ToString();
        //                  textMemSize.Text = site.Status.AvailPhys.ToString();
        //                  textTaskCount.Text = "0";
        //                  textCPUVersion.Text = site.Status.CPUVersion;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ((DataTable)gridControlNodeLst.DataSource).Rows[checkedListBoxSiteLst.SelectedIndex]["RunningState"] = "运行停止";
        //        }

        //    }
        //    else
        //    {
        //        textTaskCount.Text = "0";
        //    }
        //    textethAddress.Text = site.IPAdress;
        //    textHost.Text = site.SiteName;
        //}

        /// <summary>
        /// 当左侧列表变化触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxSiteLst_SelectedIndexChanged(object sender, EventArgs e)
        {
             DisplayMonitor();
           // textHost.Text = 

        }

        public void HideSeries(string series, bool visible1)
        {

            switch (series)
            {
                case "SeriesCPURate":
                    lock (chartCPURate)
                    {
                        chartCPURate.Series["SeriesCPURate"].Visible = visible1;
                    }
                    break;
                case "SeriesMemRate":
                    lock (chartCPURate)
                    {
                        chartCPURate.Series["SeriesMemRate"].Visible = visible1;
                    }
                    break;
                case "SeriesUnusedMemory":
                    lock (chartMemory)
                    {
                        chartMemory.Series["SeriesUnusedMemory"].Visible = visible1;
                    }
                    break;
                case "SeriesUsedMemory":
                    lock (chartMemory)
                    {
                        chartMemory.Series["SeriesUsedMemory"].Visible = visible1;
                    }
                    break;
            }

        }

        public void OpenTabPage(int pageIndex)
        {
            xtraTabControl1.SelectedTabPageIndex = pageIndex;
        }

        /// <summary>
        ///设置刷新时间
        /// </summary>
        /// <param name="ms"></param>
        public void SetTimeSpan(int ms)
        {
            timer1.Interval = ms;
        }

        private void checkedListBoxSiteLst_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            visibleArr[e.Index] = (e.State == CheckState.Checked);
        }

        /// <summary>
        /// 时钟控制，动态监控站点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            DisplayMonitor();
        }

        /// <summary>
        ///在UI线程中将站点监控信息显示
        /// </summary>
        public void DisplayMonitor()
        {
            gridControlNodeLst.DataSource = MonitorInfo.siteDataTable;
            if (MonitorInfo.monitorInforArr == null)
            {
                return;
            }
            textethAddress.Text = TServerSiteManager.StorageSites[checkedListBoxSiteLst.SelectedIndex].IPAdress;
            textHost.Text = TServerSiteManager.StorageSites[checkedListBoxSiteLst.SelectedIndex].SiteName;

            for (int i = 0 ; i < MonitorInfo.monitorInforArr.Length ; i++)
            {
                if (MonitorInfo.monitorInforArr[i] == null)
                {
                    continue;
                }
                if (i < chartMemory.Series["SeriesUsedMemory"].Points.Count)
                {
                    //更新ChartMemory
                    //如果前后两次结果不一，则更新
                    //判断新获取的站点是否与原来相等
                    if (chartMemory.Series["SeriesUsedMemory"].Points[i].Values[0] != MonitorInfo.monitorInforArr[i].usedMemory)
                    {
                        chartMemory.Series["SeriesUsedMemory"].Points.RemoveAt(i);
                        chartMemory.Series["SeriesUsedMemory"].Points.Insert(i, new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].usedMemory));

                        chartMemory.Series["SeriesUnusedMemory"].Points.RemoveAt(i);
                        chartMemory.Series["SeriesUnusedMemory"].Points.Insert(i, new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].remainedMemory));
                    }

                    //刷新chartCPURate
                    if (MonitorInfo.monitorInforArr[i].cpuPercent != chartCPURate.Series["SeriesCPURate"].Points[i].Values[0])
                    {
                        chartCPURate.Series["SeriesCPURate"].Points.RemoveAt(i);
                        chartCPURate.Series["SeriesCPURate"].Points.Insert(i, new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].cpuPercent));

                        if (MonitorInfo.monitorInforArr[i].memPercent != chartCPURate.Series["SeriesMemRate"].Points[i].Values[0])
                        {
                            chartCPURate.Series["SeriesMemRate"].Points.RemoveAt(i);
                            chartCPURate.Series["SeriesMemRate"].Points.Insert(i, new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].memPercent));
                        }
                    }
                }
                else
                {
                    chartMemory.Series["SeriesUsedMemory"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].usedMemory));
                    chartMemory.Series["SeriesUnusedMemory"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].remainedMemory));
                    chartCPURate.Series["SeriesCPURate"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].cpuPercent));
                    chartCPURate.Series["SeriesMemRate"].Points.Add(new SeriesPoint(TServerSiteManager.StorageSites[i].SiteName, MonitorInfo.monitorInforArr[i].memPercent));
                }


                //移去已经删除的节点
                int currentNum = MonitorInfo.monitorInforArr.Length;
                int oldNum = chartMemory.Series["SeriesUsedMemory"].Points.Count;
                if (currentNum < oldNum)
                {
                    for (int j = currentNum ; j < oldNum ; j++)
                    {
                        chartMemory.Series["SeriesUsedMemory"].Points.RemoveAt(j);
                        chartMemory.Series["SeriesUnusedMemory"].Points.RemoveAt(j);
                        chartCPURate.Series["SeriesCPURate"].Points.RemoveAt(j);
                        chartCPURate.Series["SeriesMemRate"].Points.RemoveAt(j);
                    }
                }
                if (MonitorInfo.monitorInforArr[checkedListBoxSiteLst.SelectedIndex] != null)
                {
                    if (((DataTable)gridControlNodeLst.DataSource).Rows[checkedListBoxSiteLst.SelectedIndex]["RunningState"] == "正在运行")
                    {
                        textDiskSize.Text = MonitorInfo.monitorInforArr[checkedListBoxSiteLst.SelectedIndex].divToatalsize.ToString();
                        textMemSize.Text = MonitorInfo.monitorInforArr[checkedListBoxSiteLst.SelectedIndex].availPhys.ToString();
                        textTaskCount.Text = "0";
                        textCPUVersion.Text = MonitorInfo.monitorInforArr[checkedListBoxSiteLst.SelectedIndex].cPUVersion;
                    }
                    else
                    {
                        textTaskCount.Text = "0";
                        textDiskSize.Text = "0";
                        textCPUVersion.Text = "";
                        textMemSize.Text = "0";
                    }
          
                }
            }
        }


        

        /// <summary>
        /// 监控信息
        /// </summary>
        public class MonitorInfo
        {
            public static DataTable siteDataTable;
            public static MonitorInfo[] monitorInforArr;

            public double cpuPercent = 0;     //cpu使用率
            public double memPercent = 0;   //内存使用率
            public double remainedMemory = 0;  //可用空间
            public double usedMemory = 0;     //已用空间
            public double divToatalsize = 0;     //磁盘总量
            public string cPUVersion = "";        //cpu版本
            public double availPhys = 0;

            public string SiteName = "";
            public string siteIP = "";


            public static bool stopped = false;                    //控制是否监控

            /// <summary>
            /// 获取监控信息
            /// </summary>
            /// <returns></returns>
            public static void RefreshMonitorInfo(int siteIndex)
            {
                //  siteDataTable = TServerSiteMoniter.GetBasicSiteInfo();
               // Console.WriteLine("$" + siteIndex.ToString() + "$");
                if (siteIndex > TServerSiteManager.StorageSites.Count - 1)
                {
                    return;
                }
                MonitorInfo monitorInfo = new MonitorInfo();
                if (TServerSiteManager.StorageSites[siteIndex] != null)
                {
                    if (TServerSiteManager.optimalStorageSites.Contains(TServerSiteManager.StorageSites[siteIndex]))
                    {
                        try
                        {
                            if (!mucTSSiterManager.visibleArr[siteIndex]) //将该节点信息隐藏
                            {
                                monitorInfo.usedMemory = 0;
                                monitorInfo.remainedMemory = 0;
                                monitorInfo.cpuPercent = 0;
                                monitorInfo.memPercent = 0;
                            }
                            else
                            {   //刷新chartMemory
                            monitorInfo.remainedMemory = TServerSiteManager.StorageSites[siteIndex].Status.GetDivAvalableFreeSpace();
                            monitorInfo.usedMemory = TServerSiteManager.StorageSites[siteIndex].Status.GetDivLoadSize();
                            monitorInfo.divToatalsize = TServerSiteManager.StorageSites[siteIndex].Status.GetDivToatalsize();
                            monitorInfo.cPUVersion = TServerSiteManager.StorageSites[siteIndex].Status.CPUVersion;
                            monitorInfo.availPhys = TServerSiteManager.StorageSites[siteIndex].Status.AvailPhys;

                            monitorInfo.SiteName = TServerSiteManager.StorageSites[siteIndex].SiteName;
                            monitorInfo.siteIP = TServerSiteManager.StorageSites[siteIndex].IPAdress;
                        

                            //刷新chartCPURate
                            monitorInfo.cpuPercent = Convert.ToDouble(TServerSiteManager.StorageSites[siteIndex].Status.CPU.ToString("0.00"));//TServerSiteManager.StorageSites[siteIndex].Status.CPU;
                            monitorInfo.memPercent = TServerSiteManager.StorageSites[siteIndex].Status.TotalPhysicalMemory;
                            monitorInfo.memPercent = Convert.ToDouble((((monitorInfo.memPercent - TServerSiteManager.StorageSites[siteIndex].Status.AvailPhys) / monitorInfo.memPercent) * 100).ToString("0.00")); //((monitorInfo.memPercent - TServerSiteManager.StorageSites[siteIndex].Status.AvailPhys) / monitorInfo.memPercent) * 100;
                            }
                         
                        }
                        catch (Exception ex)
                        {
                            siteDataTable.Rows[siteIndex]["RunningState"] = "运行出错";
                            //将站点状态改为stop
                        }
                    }
                }
                monitorInforArr[siteIndex] = monitorInfo;
            }


            /// <summary>
            /// 并发获取所有站点监控信息
            /// </summary>
            public static void GetAllSiteInfo()
            {
                while (!stopped)
                {
                    siteDataTable = TServerSiteMoniter.GetBasicSiteInfo();
                    monitorInforArr = new MonitorInfo[TServerSiteManager.StorageSites.Count];
                    TServerSiteManager.UpdateOptimalStorageSiteList();
                    for (int i = 0 ; i < TServerSiteManager.StorageSites.Count ; i++)
                    {
                        Task.Factory.StartNew(() => { RefreshMonitorInfo(i); });
                        Thread.Sleep(10);
                    }
                    Thread.Sleep(1000);
                }
            }

        }

        //当控件容器变化时
        private void splitContainerControl1_SizeChanged(object sender, EventArgs e)
        {
            splitContainerControl1.SplitterPosition = splitContainerControl1.Height / 2;
        }


    }
}
