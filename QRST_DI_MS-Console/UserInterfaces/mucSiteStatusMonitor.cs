using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using QRST_DI_MS_Console.hdfsReference;
using QRST_DI_MS_Console.JSON;
using System.Threading.Tasks;
using DevExpress.Utils;
using Timer = System.Threading.Timer;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucSiteStatusMonitor : DevExpress.XtraEditors.XtraUserControl
    {
        ChartTitle chartTitle4 = new DevExpress.XtraCharts.ChartTitle();
        ChartTitle chartTitle5 = new DevExpress.XtraCharts.ChartTitle();
        private static bool isFirstLoad = true;

        private static bool serviceOpened = false;

        private static HadoopServicePortTypeClient servicesClient;
        private static namenodeinfo[] namenodelst;

        private static string[] masterInfo = new string[3] { "0.0", "0.0", "0.0" };    //主节点信息，masterInfo[0]=cup使用率，masterInfo[1]=内存使用率，masterInfo[2]=磁盘空间
        

        private static NMInfoService.NMInfoDelegateClient nmserviceClient;

        //已用磁盘空间列表
        //List<DevExpress.XtraCharts.SeriesPoint> usedMenSeriesPointLst = new List<SeriesPoint>();
        //未用磁盘空间信息列表
        //List<DevExpress.XtraCharts.SeriesPoint> unusedMenSeriesPointLst = new List<SeriesPoint>();
        private bool[] visibleArr;

        public mucSiteStatusMonitor()
        {
                InitializeComponent();
        }

        private void mucSiteStatusMonitor_Load(object sender, EventArgs e)
        {
           // Task.Factory.StartNew(InitializeControl());

        }

        /// <summary>
        /// 初始化监控信息列表
        /// </summary>
        void  InitializeControl()
        {
                //获取活动节点
            servicesClient = new HadoopServicePortTypeClient();
            try
            {
                namenodelst = JSON.JSON.parse<namenodeinfo[]>(servicesClient.getLiveNodes());
                RefreshMasterInfo();

            }
            catch (Exception ex)
            {
                serviceOpened = false;
                XtraMessageBox.Show("无法获取数据阵列站点监控服务！,请联系管理员！");
                return;
            }
            serviceOpened = true; 
            //初始化所有节点可见
            visibleArr = new bool[namenodelst.Length];
            checkedListBoxSiteLst.Items.Clear();
          
            for (int i = 0 ; i < namenodelst.Length ; i++)
            {
                //将活动节点添加到节点列表，默认为全部选中监控
                checkedListBoxSiteLst.Items.Add(namenodelst[i].host);
                checkedListBoxSiteLst.Items[i].CheckState = CheckState.Checked;
                visibleArr[i] = true;
                //初始化chartMemory
                double remainedMemory = double.Parse(namenodelst[i].remaining);
                double usedMemory = double.Parse(namenodelst[i].capacity) - remainedMemory;
             //   usedMenSeriesPointLst.Add(new SeriesPoint(namenodelst[i].host,usedMemory));
              //  unusedMenSeriesPointLst.Add(new SeriesPoint(namenodelst[i].host, remainedMemory));
                chartMemory.Series["SeriesUsedMemory"].Points.Add(new SeriesPoint(namenodelst[i].host, usedMemory));
                chartMemory.Series["SeriesUnusedMemory"].Points.Add(new SeriesPoint(namenodelst[i].host, remainedMemory));
              
                //初始化chartCPURate
                double cpuPercent = double.Parse(namenodelst[i].perc.Remove(namenodelst[i].perc.IndexOf("%")));
                double memPercent =double.Parse(namenodelst[i].memPerc) ;
                chartCPURate.Series["SeriesMemRate"].Points.Add(new SeriesPoint(namenodelst[i].host,memPercent));
                chartCPURate.Series["SeriesCPURate"].Points.Add(new SeriesPoint(namenodelst[i].host, cpuPercent));
            }
            //初始化图表
            //初始化chartnmCpu
            double nmcpuPercent=0.0;
            double nmmemPercent=0.0;
            try
            {
                nmcpuPercent = double.Parse(masterInfo[0]);
            }
            catch (Exception e)
            {
                nmcpuPercent = 0.0;
            }
            try
            {
                nmmemPercent = double.Parse(masterInfo[1]);
            }
            catch (Exception e)
            {
                nmcpuPercent = 0.0;
            }
            chartnmCpu.Series["SeriesMemRate"].Points.Add(new SeriesPoint("NameNode", nmmemPercent));
            chartnmCpu.Series["SeriesCPURate"].Points.Add(new SeriesPoint("NameNode", nmcpuPercent));

            //初始化chartnmDisk
            String diskstring = masterInfo[2];
            String avail = diskstring.Split(',')[0];
            String used = diskstring.Split(',')[1];
            chartTitle4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartTitle4.Indent = 8;
            chartTitle4.Text = "硬盘使用率";
            chartTitle5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartTitle5.Text = "已使用" + used + "GB" + ";" + "还可用" + avail + "GB";
            chartTitle5.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
            this.chartnmDisk.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle4,
            chartTitle5});
            double diskusedpercent = 0.0;
            try
            {
                diskusedpercent = double.Parse(diskstring.Split(',')[2]) * 100;
            }
            catch (Exception e)
            {
                diskusedpercent = 0.0;
            }
            SeriesPoint seriesPoint1 = new SeriesPoint("已使用", new object[] { ((object)(diskusedpercent)) });
            SeriesPoint seriesPoint2 = new SeriesPoint("未使用", new object[] { ((object)(100.0 - diskusedpercent)) });
            chartnmDisk.Series["SeriesnmDisk"].Points.Add(seriesPoint1);
            chartnmDisk.Series["SeriesnmDisk"].Points.Add(seriesPoint2);

            




            //默认选中第一个节点，将信息显示在信息框中
            if(checkedListBoxSiteLst.Items.Count!=0)
            {
                checkedListBoxSiteLst.SelectedIndex = 0;
                RefreshText(namenodelst[0]);
            }
 
            //初始化节点概况列表
            DataTable dt =  InitialColumns();
            FillMonitorLst(dt,namenodelst);
            gridControlNodeLst.DataSource = dt;

            timer1.Start();

            isFirstLoad = false;
        }

        //初始化列结构
       DataTable InitialColumns()
        {
           DataTable columnDT = new DataTable();
            columnDT = new DataTable();

            DataColumn host = new DataColumn { ColumnName = "host", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(host);

            DataColumn osArch = new DataColumn { ColumnName = "osArch", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(osArch);

            DataColumn osVendorName = new DataColumn { ColumnName = "osVendorName", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(osVendorName);

            DataColumn osVersion = new DataColumn { ColumnName = "osVersion", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(osVersion);

            DataColumn ethAddress = new DataColumn { ColumnName = "ethAddress", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(ethAddress);

            DataColumn RxPackets = new DataColumn { ColumnName = "RxPackets", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(RxPackets);

            DataColumn TxPackets = new DataColumn { ColumnName = "TxPackets", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(TxPackets);

            DataColumn adminState = new DataColumn { ColumnName = "adminState", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(adminState);

           return columnDT;
        }

        void FillMonitorLst(DataTable dt,namenodeinfo[] nodeArr)
        {
            dt.Rows.Clear();
            for (int i = 0; i < nodeArr.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["host"] = nodeArr[i].host;
                dr["osArch"] = nodeArr[i].osArch;
                dr["osVendorName"] = nodeArr[i].osVendorName;
                dr["osVersion"] = nodeArr[i].osVersion;
                dr["ethAddress"] = nodeArr[i].ethAddress;
                dr["RxPackets"] = nodeArr[i].RxPackets;
                dr["TxPackets"] = nodeArr[i].TxPackets;
                dr["adminState"] = nodeArr[i].adminState;

                dt.Rows.Add(dr);
            }
        }
       
        /// <summary>
        /// 通过调用hdf服务，更新列表信息，更新的内容如下：
        /// 1. chartMemory
        /// 2. chartCPURate
        /// 3. text列表
        /// 4. gridControlNodeLst中的收发包裹数以及运行状态
        /// 5. chartnmCpu
        /// 6. chartnmDisk
        /// </summary>
        void RefreshMonitorInfo()
        {
            if (!serviceOpened)
            {
                return;
            }
            //刷新chartnmCpu
            double nmcpuPercent=0.0;
            double nmmemPercent=0.0;
            try
            {
                nmcpuPercent = double.Parse(masterInfo[0]);
            }
            catch(Exception e)
            {
                nmcpuPercent = 0.0;
            }
            try
            {
                nmmemPercent = double.Parse(masterInfo[1]);
            }
            catch (Exception e)
            {
                nmcpuPercent = 0.0;
            }

            if (nmcpuPercent != chartnmCpu.Series["SeriesCPURate"].Points[0].Values[0])
            {
                chartnmCpu.Series["SeriesCPURate"].Points.RemoveAt(0);
                chartnmCpu.Series["SeriesCPURate"].Points.Insert(0, new SeriesPoint("NameNode", nmcpuPercent));
            }
            if (nmmemPercent != chartCPURate.Series["SeriesMemRate"].Points[0].Values[0])
            {
                chartnmCpu.Series["SeriesMemRate"].Points.RemoveAt(0);
                chartnmCpu.Series["SeriesMemRate"].Points.Insert(0, new SeriesPoint("NameNode", nmmemPercent));
            }
            //刷新chartnmDisk
            String diskstring = nmserviceClient.getDiskUsage();
            String avail = diskstring.Split(',')[0];
            String used = diskstring.Split(',')[1];
            double diskusedpercent = 0.0;
            try
            {
                diskusedpercent = double.Parse(diskstring.Split(',')[2]) * 100;
            }
            catch (Exception e)
            {
                diskusedpercent = 0.0;
            }
            if (diskusedpercent != chartnmDisk.Series["SeriesnmDisk"].Points[0].Values[0])
            {
                SeriesPoint seriesPoint1 = new SeriesPoint("已使用", new object[] { ((object)(diskusedpercent)) });
                SeriesPoint seriesPoint2 = new SeriesPoint("未使用", new object[] { ((object)(100.0 - diskusedpercent)) });
                chartnmDisk.Series["SeriesnmDisk"].Points.Clear();
                chartnmDisk.Series["SeriesnmDisk"].Points.Add(seriesPoint1);
                chartnmDisk.Series["SeriesnmDisk"].Points.Add(seriesPoint2);
                chartTitle4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                chartTitle4.Indent = 8;
                chartTitle4.Text = "硬盘使用率";
                chartTitle5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                chartTitle5.Text = "已使用" + used + "GB" + ";" + "还可用" + avail + "GB";
                chartTitle5.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
                this.chartnmDisk.Titles.Clear();
                this.chartnmDisk.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle4,
            chartTitle5});
            }



            for (int i = 0; i < namenodelst.Length; i++)
            {
                //刷新chartMemory
                double remainedMemory = double.Parse(namenodelst[i].remaining);
                double usedMemory = double.Parse(namenodelst[i].capacity) - remainedMemory;
                if (!visibleArr[i]) //将该节点信息隐藏
                {
                    usedMemory = 0;
                    remainedMemory = 0;
                }
                //如果前后两次结果不一，则更新
               if(chartMemory.Series["SeriesUsedMemory"].Points[i].Values[0] != usedMemory )
               {
                   chartMemory.Series["SeriesUsedMemory"].Points.RemoveAt(i);
                   chartMemory.Series["SeriesUsedMemory"].Points.Insert(i, new SeriesPoint(namenodelst[i].host, usedMemory));

                   chartMemory.Series["SeriesUnusedMemory"].Points.RemoveAt(i);
                   chartMemory.Series["SeriesUnusedMemory"].Points.Insert(i, new SeriesPoint(namenodelst[i].host, remainedMemory));
               }

                //刷新chartCPURate
                double cpuPercent = double.Parse(namenodelst[i].perc.Remove(namenodelst[i].perc.IndexOf("%")));
                double memPercent = double.Parse(namenodelst[i].memPerc);
                if (!visibleArr[i]) //将该节点信息隐藏
                {
                    cpuPercent = 0;
                    memPercent = 0;
                }
                if (cpuPercent != chartCPURate.Series["SeriesCPURate"].Points[i].Values[0]) 
                {
                    chartCPURate.Series["SeriesCPURate"].Points.RemoveAt(i);
                    chartCPURate.Series["SeriesCPURate"].Points.Insert(i, new SeriesPoint(namenodelst[i].host, cpuPercent));
                }
                if (memPercent != chartCPURate.Series["SeriesMemRate"].Points[i].Values[0])
                {
                    chartCPURate.Series["SeriesMemRate"].Points.RemoveAt(i);
                    chartCPURate.Series["SeriesMemRate"].Points.Insert(i, new SeriesPoint(namenodelst[i].host, memPercent));
                }
                //刷新text列表
                namenodeinfo nodeinfo = namenodelst[checkedListBoxSiteLst.SelectedIndex];
                textTxPackets.Text = nodeinfo.TxPackets;
                textTxBytes.Text = nodeinfo.TxBytes;
                textRxPackets.Text = nodeinfo.RxPackets;
                textRxBytes.Text = nodeinfo.RxBytes;

                //刷新gridControlNodeLst
                ((DataTable) gridControlNodeLst.DataSource).Rows[i]["RxPackets"] = namenodelst[i].RxPackets;
                ((DataTable) gridControlNodeLst.DataSource).Rows[i]["TxPackets"] = namenodelst[i].TxPackets;
                ((DataTable) gridControlNodeLst.DataSource).Rows[i]["adminState"] = namenodelst[i].adminState;
            }
        }

        /// <summary>
        /// 当左侧节点列表选中项发生改变时，刷新展示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxSiteLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshText(namenodelst[checkedListBoxSiteLst.SelectedIndex]);
        }

        /// <summary>
        /// 刷新第二个tab页中的text中的信息
        /// </summary>
        /// <param name="nodeinfo"></param>
        void RefreshText(namenodeinfo nodeinfo)
        {
            textTxPackets.Text = nodeinfo.TxPackets;
            textTxBytes.Text = nodeinfo.TxBytes;
            textRxPackets.Text = nodeinfo.RxPackets;
            textRxBytes.Text = nodeinfo.RxBytes;
            textOsVendorName.Text = nodeinfo.osVendorName;
            textOsArch.Text = nodeinfo.osArch;
            textHost.Text = nodeinfo.host;
            textethAddress.Text = nodeinfo.ethAddress;
        }

        /// <summary>
        /// 更新监控信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCallBack(Object state)
        {
            MethodInvoker invoker = new MethodInvoker(RefreshMonitorInfo);
            BeginInvoke(invoker);
        }

        /// <summary>
        /// item不选中，则隐藏该节点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxSiteLst_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
                visibleArr[e.Index] =(e.State ==CheckState.Checked); 
        }

        /// <summary>
        /// 隐藏节点监控信息
        /// </summary>
        /// <param name="nodeindex"></param>
        //void HideNode(int nodeindex)
        //{
        //    namenodelst[nodeindex].SetIsVisible(false);
        //}

        public void HideSeries(string series,bool visible1)
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
                    case  "SeriesUsedMemory":
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

        private void mucSiteStatusMonitor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (isFirstLoad)
                {
                    using (WaitDialogForm dlg = new WaitDialogForm("请稍后", "正在获取监控服务！"))
                    {
                        InitializeControl();
                    }
                }
               // timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!serviceOpened)
            {
                timer1.Stop();
            }
            RefreshMonitorInfo();
          //  Task.Factory.StartNew(() => { GetNodeInfo(); });
           // this.BeginInvoke(del);
        }

        //异步执行节点信息获取服务
        public delegate void GetNodeInfoDel();
        GetNodeInfoDel del = new GetNodeInfoDel(GetNodeInfo);

        private static void GetNodeInfo()
        {
            //调用hdf服务
            try
            {
                
                namenodelst = JSON.JSON.parse<namenodeinfo[]>(servicesClient.getLiveNodes());
                RefreshMasterInfo();
            }
            catch (System.Exception ex)
            {
                XtraMessageBox.Show("无法获取数据阵列站点监控服务！,请联系管理员！");
                serviceOpened = false;
            }
        }

        private void splitContainerControl1_SizeChanged(object sender, EventArgs e)
        {
            splitContainerControl1.SplitterPosition = splitContainerControl1.Height / 2;
        }

        //刷新主节点信息   zxw 20131222
        public static void RefreshMasterInfo()
        {
            try
            {
                if (nmserviceClient == null)
                {
                    nmserviceClient = new NMInfoService.NMInfoDelegateClient();
                }
                masterInfo[0] = nmserviceClient.getcpuPerc();
                masterInfo[1] = nmserviceClient.getPhysicalMemory();
                masterInfo[2] = nmserviceClient.getDiskUsage();
            }
            catch(Exception ex)
            {
                for (int i = 0; i < masterInfo.Length;i++ )
                {
                    masterInfo[i] = "0.0";
                }
            }
           
        }
            
       
    }
}
