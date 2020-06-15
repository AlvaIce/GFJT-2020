using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI;
using QRST_DI_MS_Desktop.HadoopInfo;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using QRST_DI_MS_Basis.JobInfo;
using System.Diagnostics;
using System.Configuration;
using QRST_DI_TS_Process.Site;
using QRST_DI_MS_Basis.Taskinfo;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucComputeService : DevExpress.XtraEditors.XtraUserControl
    {
        //记录当前选择的算法
        private int selectedAlgIndex = 1;
        //记录算法描述
        private string[] des = { "是将大量的矢量数据进行分布式的分配和处理，像栅格切片那样将其按照不同层级的格网进行分割，并将其转绘为对应的栅格图像即瓦片。可在前端根据显示需要按需请求不同的瓦片数据进行绘图。",
                                 "将原始影像按照给定的矢量多边形进行裁剪，获取原始影像中所需的一部分。裁剪结果的形状与输入的多边形相同，范围为该多边形的最小外包矩形。可同时输入多个矢量要素，并以分布式的方法进行裁剪。",
                                 "计算矢量数据中的一些多边形要素或栅格图像中具有某些波普特征的地物的表面积。",
                                 "将多个本身是分开的，但在空间上具有相邻关系的矢量要素或者栅格图像进行空间连接，以把握整体上的关系。"
                               };

        private string shareRoot = ConfigurationManager.AppSettings["SubTaskDataPath"];
        private string masterIP = ConfigurationManager.AppSettings["NameNodeIP"];
        private string hadoopHome = ConfigurationManager.AppSettings["HadoopHome"];
        private string mountPath = ConfigurationManager.AppSettings["MountPath"];

        public mucComputeService()
        {
            InitializeComponent();
            //QRST_DI_MS_Desktop.UserInterfaces.mucTSSiterManager.MonitorInfo
            //ShellHelp sh = new ShellHelp();
            //sh.OpenShell(masterIP, "root", "checc", 22);
            //sh.Shell("mkdir /usr/local/swq");
        }
        //根据所选算法更换界面
        public void changeUI(string algorithm)
        {
            groupControl1.Text = algorithm;
            switch (algorithm)
            {
                default:
                case "矢量切片": memo_Description.Text = des[0]; selectedAlgIndex = 1; showTileUI(); break;
                case "影像裁剪": memo_Description.Text = des[1]; selectedAlgIndex = 2; showClipUI(); break;
                case "表面积计算": memo_Description.Text = des[2]; selectedAlgIndex = 3; this.xtraScrollableControl1.Controls.Clear(); break;
                case "空间连接": memo_Description.Text = des[3]; selectedAlgIndex = 4; this.xtraScrollableControl1.Controls.Clear(); break;
            }
        }
        //显示切片的界面
        private void showTileUI()
        {
            this.xtraScrollableControl1.Controls.Clear();
            int x = 3;
            int y = 3, ystep = 109;

            CtrlSelectArg csa = new CtrlSelectArg();
            csa.groupControl.Text = "输入文件";
            csa.memoEdit.Text = "\\\\" + shareRoot + "\\gfjtdata\\compute\\data\\tile\\";
            csa.Location = new System.Drawing.Point(x, y);
            this.xtraScrollableControl1.Controls.Add(csa);

            CtrlInputArg cia1 = new CtrlInputArg();
            cia1.labelControl.Text = "输出文件夹名";
            cia1.groupControl.Text = "参数1";
            cia1.textEdit.Text = "tile";
            cia1.Location = new System.Drawing.Point(x, y + ystep);
            this.xtraScrollableControl1.Controls.Add(cia1);

            CtrlInputArg cia = new CtrlInputArg();
            cia.labelControl.Text = "切片层级";
            cia.groupControl.Text = "参数2";
            cia.textEdit.Text = "3";
            cia.Location = new System.Drawing.Point(x, y + 2 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia);

            CtrlInputArg cia2 = new CtrlInputArg();
            cia2.labelControl.Text = "最小经度";
            cia2.groupControl.Text = "参数3";
            cia2.textEdit.Text = "75.96";
            cia2.Location = new System.Drawing.Point(x, y + 3 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia2);

            CtrlInputArg cia3 = new CtrlInputArg();
            cia3.labelControl.Text = "最大经度";
            cia3.groupControl.Text = "参数4";
            cia3.textEdit.Text = "134.52";
            cia3.Location = new System.Drawing.Point(x, y + 4 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia3);

            CtrlInputArg cia4 = new CtrlInputArg();
            cia4.labelControl.Text = "最小纬度";
            cia4.groupControl.Text = "参数5";
            cia4.textEdit.Text = "18.29";
            cia4.Location = new System.Drawing.Point(x, y + 5 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia4);

            CtrlInputArg cia5 = new CtrlInputArg();
            cia5.labelControl.Text = "最大纬度";
            cia5.groupControl.Text = "参数6";
            cia5.textEdit.Text = "53.02";
            cia5.Location = new System.Drawing.Point(x, y + 6 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia5);
        }
        //显示裁剪的界面
        private void showClipUI()
        {
            this.xtraScrollableControl1.Controls.Clear();
            int x = 3;
            int y = 3, ystep = 109;

            CtrlSelectArg csa1 = new CtrlSelectArg();
            csa1.groupControl.Text = "输入栅格";
            csa1.memoEdit.Text = "\\\\" + shareRoot + "\\gfjtdata\\compute\\data\\clip\\reprojecttif.tif";
            csa1.Location = new System.Drawing.Point(x, y);
            this.xtraScrollableControl1.Controls.Add(csa1);

            CtrlSelectArg csa2 = new CtrlSelectArg();
            csa2.groupControl.Text = "输入矢量";
            csa2.memoEdit.Text = "\\\\" + shareRoot + "\\gfjtdata\\compute\\data\\clip\\road_buffer.txt";
            csa2.Location = new System.Drawing.Point(x, y + ystep);
            this.xtraScrollableControl1.Controls.Add(csa2);

            CtrlInputArg cia1 = new CtrlInputArg();
            cia1.labelControl.Text = "输出文件夹名";
            cia1.groupControl.Text = "参数1";
            cia1.textEdit.Text = "clip";
            cia1.Location = new System.Drawing.Point(x, y + 2 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia1);

            CtrlInputArg cia2 = new CtrlInputArg();
            cia2.labelControl.Text = "缓冲区宽度";
            cia2.groupControl.Text = "参数2";
            cia2.textEdit.Text = "0";
            cia2.Location = new System.Drawing.Point(x, y + 3 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia2);

            CtrlInputArg cia3 = new CtrlInputArg();
            cia3.labelControl.Text = "Map操作数";
            cia3.groupControl.Text = "参数3";
            cia3.textEdit.Text = "3";
            cia3.Location = new System.Drawing.Point(x, y + 4 * ystep);
            this.xtraScrollableControl1.Controls.Add(cia3);
        }
        private void mucComputeService_Load(object sender, EventArgs e)
        {
            showTileUI();
            memo_Description.Text = des[0];
            groupControl1.Text = "矢量切片";
            getJobInfo();
            Thread t1 = new Thread(new ThreadStart(getNodeInfo));
            Thread t2 = new Thread(new ThreadStart(getSubSystemInfo));
            t1.IsBackground = true;
            t2.IsBackground = true;
            t1.Start();
            t2.Start();

        }
        //清除所填的参数
        public void clearText()
        {
            foreach (Control c in xtraScrollableControl1.Controls)
            {
                if (c.GetType().ToString() == "QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI.CtrlInputArg")
                {
                    CtrlInputArg cia = c as CtrlInputArg;
                    cia.textEdit.Text = string.Empty;
                }
                else if (c.GetType().ToString() == "QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI.CtrlSelectArg")
                {
                    CtrlSelectArg csa = c as CtrlSelectArg;
                    csa.memoEdit.Text = string.Empty;
                }
            }
        }
        //unix时间转为windows时间
        private string getWindowsTime(ulong unixTime)
        {
            DateTime dt = DateTime.MinValue;
            DateTime start = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            dt = start.AddMilliseconds(unixTime);
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        //根据毫秒数获取时间
        private string getTime(ulong ms)
        {
            ulong h = ms / 1000 / 60 / 60;
            ulong m = ms / 1000 / 60 % 60;
            ulong s = ms / 1000 % 60;
            string time = string.Format("{0}小时{1}分钟{2}.{3}秒", h, m, s, ms % 1000);
            return time;
        }
        //获取任务信息
        public void getJobInfo()
        {
            //while (true)
            //{
            DataTable dt = jobinfo.getJobInfo();
            this.Invoke((EventHandler)delegate
            {
                grid_JobInfo.DataSource = dt;
                gridView1.Columns[0].BestFit();
                gridView1.Columns[1].BestFit();
                grid_JobInfo.Refresh();
            });
            //    Thread.Sleep(1000);
            //}
        }
        //获取节点信息
        public void getNodeInfo()
        {
            string url1 = "http://" + masterIP + ":50070/jmx?qry=Hadoop:service=NameNode,name=NameNodeInfo";
            string info = string.Empty;
            string url2 = "http://" + masterIP + ":8088/ws/v1/cluster/nodes";
            string minfo = string.Empty;
            List<DataNode> dns = new List<DataNode>();
            List<Node> ns = new List<Node>();
            while (true)
            {
                info = HttpRequestHelper.DoGet(url1, null);
                minfo = HttpRequestHelper.DoGet(url2, null);
                if (info.Length > 0)
                {
                    ModelNodeInfo mni = JsonConvert.DeserializeObject<ModelNodeInfo>(info);
                    if (mni.beans != null && mni.beans.Count > 0)
                    {
                        //mni.beans[0].Name = "master";
                        //mni.beans[0].IP = "202.121.180.3";
                        //mni.beans[0].Total = Math.Round(mni.beans[0].Total / 1024 / 1024 / 1024, 2);
                        //mni.beans[0].Used = Math.Round(mni.beans[0].Used / 1024 / 1024 / 1024, 2);
                        //mni.beans[0].Free = Math.Round(mni.beans[0].Free / 1024 / 1024 / 1024, 2);
                        //mni.beans[0].NonDfsUsedSpace = Math.Round(mni.beans[0].NonDfsUsedSpace / 1024 / 1024 / 1024, 2);
                        dns = getDataNodes(mni.beans[0].LiveNodes);
                        foreach (DataNode dn in dns)
                        {
                            if (dn == null)
                                continue;
                            dn.usedSpace = Math.Round(dn.usedSpace / 1024 / 1024 / 1024, 2);
                            dn.nonDfsUsedSpace = Math.Round(dn.nonDfsUsedSpace / 1024 / 1024 / 1024, 2);
                            dn.capacity = Math.Round(dn.capacity / 1024 / 1024 / 1024, 2);
                            dn.remaining = Math.Round(dn.remaining / 1024 / 1024 / 1024, 2);
                            dn.xferaddr = dn.xferaddr.Substring(0, dn.xferaddr.IndexOf(":"));
                        }
                    }
                }
                if (minfo.Length > 0)
                {
                    ModelNodesMemory mnm = JsonConvert.DeserializeObject<ModelNodesMemory>(minfo);
                    if (mnm != null && mnm.nodes != null && mnm.nodes.node != null && mnm.nodes.node.Count > 0)
                    {
                        foreach (Node n in mnm.nodes.node)
                        {
                            if (n == null)
                                continue;
                            n.availMemoryMB = Math.Round(n.availMemoryMB / 1024, 2);
                            n.usedMemoryMB = Math.Round(n.usedMemoryMB / 1024, 2);
                        }
                        ns = mnm.nodes.node;
                    }
                }
                this.Invoke((EventHandler)delegate
                {
                    grid_DataNode.DataSource = dns;
                    grid_DataNode.Refresh();
                    updateChart(dns, ns);
                });
                Thread.Sleep(3000);
            }
        }
        //获取子节点信息列表
        private List<DataNode> getDataNodes(string json)
        {
            List<DataNode> dns = new List<DataNode>();
            if (json.Length == 0)
                return new List<DataNode>();

            JsonReader reader = new JsonTextReader(new System.IO.StringReader(json));
            string namenode = ConfigurationManager.AppSettings["NameNodeName"];
            string datanode = ConfigurationManager.AppSettings["DataNodeName"];
            while (reader.Read())
            {
                if (reader.Value != null && (reader.Value.ToString().StartsWith(datanode) || reader.Value.ToString().StartsWith(namenode)))
                {
                    DataNode dn = new DataNode();
                    dn.name = reader.Value.ToString();//.Substring(0, reader.Value.ToString().IndexOf(":"));
                    while (reader.TokenType.ToString() != "EndObject")
                    {
                        if (reader.Value != null)
                        {
                            switch (reader.Value.ToString())
                            {
                                case "xferaddr": { reader.Read(); dn.xferaddr = reader.Value.ToString(); break; }
                                case "usedSpace": { reader.Read(); dn.usedSpace = ulong.Parse(reader.Value.ToString()); break; }
                                case "nonDfsUsedSpace": { reader.Read(); dn.nonDfsUsedSpace = ulong.Parse(reader.Value.ToString()); break; }
                                case "capacity": { reader.Read(); dn.capacity = ulong.Parse(reader.Value.ToString()); break; }
                                case "remaining": { reader.Read(); dn.remaining = ulong.Parse(reader.Value.ToString()); break; }
                                default: break;
                            }
                        }
                        reader.Read();
                    }
                    dns.Add(dn);
                }
            }
            return dns;
        }
        //更新图表
        private void updateChart(List<DataNode> dns, List<Node> ns)
        {
            //主节点
            //if (bean != null)
            //{
            //    if (chartControl1.Series["UsedDFS"].Points.Count > 0 &&( chartControl1.Series["UsedDFS"].Points[0].Values[0] != bean.Used || chartControl1.Series["RemainDFS"].Points[0].Values[0] != bean.Free))
            //    {
            //        chartControl1.Series["UsedDFS"].Points.RemoveAt(0);
            //        chartControl1.Series["UsedDFS"].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("master", bean.Used));
            //        chartControl1.Series["RemainDFS"].Points.RemoveAt(0);
            //        chartControl1.Series["RemainDFS"].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("master", bean.Free));
            //        chartControl1.Series["NonDFS"].Points.RemoveAt(0);
            //        chartControl1.Series["NonDFS"].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("master", bean.NonDfsUsedSpace));
            //    }
            //    else
            //    {
            //        chartControl1.Series["UsedDFS"].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("master", bean.Used));
            //        chartControl1.Series["RemainDFS"].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("master", bean.Free));
            //        chartControl1.Series["NonDFS"].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("master", bean.NonDfsUsedSpace));
            //    }
            //}
            //磁盘
            int i;
            for (i = 0; i < dns.Count; i++)
            {
                if (dns[i] == null)
                    continue;
                //新节点序号小于原总节点数，比较二者是否相同，不同则修改
                if (i < chartControl1.Series["UsedDFS"].Points.Count)
                {
                    if (chartControl1.Series["UsedDFS"].Points[i].Values[0] != dns[i].usedSpace || chartControl1.Series["RemainDFS"].Points[i].Values[0] != dns[i].remaining)
                    {
                        chartControl1.Series["UsedDFS"].Points.RemoveAt(i);
                        chartControl1.Series["UsedDFS"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(dns[i].name, dns[i].usedSpace));
                        chartControl1.Series["RemainDFS"].Points.RemoveAt(i);
                        chartControl1.Series["RemainDFS"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(dns[i].name, dns[i].remaining));
                        chartControl1.Series["NonDFS"].Points.RemoveAt(i);
                        chartControl1.Series["NonDFS"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(dns[i].name, dns[i].nonDfsUsedSpace));
                    }
                }
                //新节点序号大于原总节点数，直接插入
                else
                {
                    chartControl1.Series["UsedDFS"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(dns[i].name, dns[i].usedSpace));
                    chartControl1.Series["RemainDFS"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(dns[i].name, dns[i].remaining));
                    chartControl1.Series["NonDFS"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(dns[i].name, dns[i].nonDfsUsedSpace));
                }
            }
            //新节点总数小于原节点总数，删除多余的节点
            if (i < chartControl1.Series["UsedDFS"].Points.Count)
            {
                for (; i < chartControl1.Series["UsedDFS"].Points.Count; i++)
                {
                    chartControl1.Series["UsedDFS"].Points.RemoveAt(i);
                    chartControl1.Series["RemainDFS"].Points.RemoveAt(i);
                    chartControl1.Series["NonDFS"].Points.RemoveAt(i);
                }
            }
            //内存
            for (i = 0; i < ns.Count; i++)
            {
                if (ns[i] == null)
                    continue;
                if (i < chartControl2.Series["usedMemory"].Points.Count)
                {
                    if (chartControl2.Series["usedMemory"].Points[i].Values[0] != ns[i].usedMemoryMB)
                    {
                        chartControl2.Series["usedMemory"].Points.RemoveAt(i);
                        chartControl2.Series["usedMemory"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(ns[i].nodeHostName, ns[i].usedMemoryMB));
                        chartControl2.Series["availMemory"].Points.RemoveAt(i);
                        chartControl2.Series["availMemory"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(ns[i].nodeHostName, ns[i].availMemoryMB));
                    }
                }
                else
                {
                    chartControl2.Series["usedMemory"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(ns[i].nodeHostName, ns[i].usedMemoryMB));
                    chartControl2.Series["availMemory"].Points.Insert(i, new DevExpress.XtraCharts.SeriesPoint(ns[i].nodeHostName, ns[i].availMemoryMB));
                }
            }
            if (i < chartControl2.Series["usedMemory"].Points.Count)
            {
                for (; i < chartControl2.Series["usedMemory"].Points.Count; i++)
                {
                    chartControl2.Series["usedMemory"].Points.RemoveAt(i);
                    chartControl2.Series["availMemory"].Points.RemoveAt(i);
                }
            }
        }
        //执行算法
        public void runAlgorithm()
        {
            Thread t = new Thread(new ThreadStart(run));
            t.IsBackground = true;
            t.Start();
        }

        public void run()
        {
            ShellHelp sh = null;
            DataTable dt = grid_JobInfo.DataSource as DataTable;

            string command = string.Empty;
            switch (selectedAlgIndex)
            {
                default:
                case 1: command = getTileCommand(); break;
                case 2: command = getClipCommand(); break;
                case 3: MessageBox.Show("算法未添加"); return;
                case 4: MessageBox.Show("算法未添加"); return;
                case 0: command = getMakeCommand(); break;
            }
            if (command.Length == 0)
            {
                MessageBox.Show("参数未输入或参数格式不正确！");
                return;
            }

            string user = ConfigurationManager.AppSettings["NameNodeUsr"];
            string pwd = ConfigurationManager.AppSettings["NameNodePwd"];

            try
            {
                //获取最近一次任务的id
                DataTable ndt = jobinfo.getNewsestJob();
                string newsetJob;
                if (ndt.Rows.Count > 0)
                    newsetJob = jobinfo.getNewsestJob().Rows[0][1].ToString();
                else
                    newsetJob = string.Empty;

                //***********************************重要******************************************
                //搞事情之前先把要连接的主机的/etc/ssh/sshd_config中的PasswordAuthentication改为yes
                //并service sshd restart
                sh = new ShellHelp();
                sh.OpenShell(masterIP, user, pwd, 22);
                sh.Shell(command);

                //获取任务信息
                string url = "http://" + masterIP + ":8088/ws/v1/cluster/apps";
                string info = HttpRequestHelper.DoGet(url, null);
                ModelJobStatus mjs = JsonConvert.DeserializeObject<ModelJobStatus>(info);
                int newestIndex = 0;
                if (mjs != null && mjs.apps != null && mjs.apps.app != null)
                {
                    for (int i = 0; i < mjs.apps.app.Count; i++)
                    {
                        if (mjs.apps.app[i].startedTime > mjs.apps.app[newestIndex].startedTime)
                            newestIndex = i;
                    }
                }
                Stopwatch sw = new Stopwatch();               //计时器
                sw.Start();
                //判断jar包运行后是否有新任务提交，若超过60s仍没有，则判断任务出错
                while (mjs == null || mjs.apps == null || mjs.apps.app == null || mjs.apps.app.Count == 0 || mjs.apps.app[newestIndex].id == newsetJob)
                {
                    if (sw.ElapsedMilliseconds > 60000)
                    {
                        MessageBox.Show("任务超时！");
                        return;
                    }
                    info = HttpRequestHelper.DoGet(url, null);
                    if (info.Length == 0)
                        continue;
                    mjs = JsonConvert.DeserializeObject<ModelJobStatus>(info);
                    if (mjs != null && mjs.apps != null && mjs.apps.app != null)
                    {
                        for (int i = 0; i < mjs.apps.app.Count; i++)
                        {
                            if (mjs.apps.app[i].startedTime > mjs.apps.app[newestIndex].startedTime)
                                newestIndex = i;
                        }
                    }
                }

                //若有新任务提交，则将其插入数据库
                int id;
                if (newsetJob.Length > 0)
                    id = Int32.Parse(jobinfo.getNewsestJob().Rows[0][0].ToString()) + 1;
                else
                    id = 1;
                string value = string.Format("({0},'{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}')",
                    id, mjs.apps.app[newestIndex].id, mjs.apps.app[newestIndex].name, mjs.apps.app[newestIndex].state, mjs.apps.app[newestIndex].finalStatus, mjs.apps.app[newestIndex].progress, getWindowsTime(mjs.apps.app[newestIndex].startedTime), getWindowsTime(mjs.apps.app[newestIndex].finishedTime), getTime(mjs.apps.app[newestIndex].elapsedTime));
                jobinfo.insertNewJob(value);
                getJobInfo();

                string set = string.Empty;
                string where = string.Format("ComputeID='{0}'", mjs.apps.app[newestIndex].id);
                //在任务结束前不断更新其进度和状态
                while (mjs.apps.app[newestIndex].finalStatus == "UNDEFINED")
                {
                    set = string.Format("state='{0}',progress={1},finishedTime='{2}',elapsedTime='{3}'", mjs.apps.app[newestIndex].state, mjs.apps.app[newestIndex].progress, getWindowsTime(mjs.apps.app[newestIndex].finishedTime), getTime(mjs.apps.app[newestIndex].elapsedTime));
                    //dt.Rows[dt.Rows.Count - 1]["progress"] = mjs.apps.app[newestIndex].progress;
                    jobinfo.updateJob(set, where);
                    getJobInfo();
                    info = HttpRequestHelper.DoGet(url, null);
                    mjs = JsonConvert.DeserializeObject<ModelJobStatus>(info);
                    Thread.Sleep(1000);
                }
                set = string.Format("state='{0}',finalStatus='{1}',progress={2},finishedTime='{3}',elapsedTime='{4}'", mjs.apps.app[newestIndex].state, mjs.apps.app[newestIndex].finalStatus, mjs.apps.app[newestIndex].progress, getWindowsTime(mjs.apps.app[newestIndex].finishedTime), getTime(mjs.apps.app[newestIndex].elapsedTime));
                jobinfo.updateJob(set, where);
                getJobInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sh != null)
                    sh.Close();
            }
        }
        //生成切片运算的命令
        private string getTileCommand()
        {
            string command = string.Empty;
            string input = string.Empty;
            string foldname = string.Empty;
            string z = string.Empty;
            string xmin = string.Empty;
            string xmax = string.Empty;
            string ymin = string.Empty;
            string ymax = string.Empty;
            foreach (Control c in xtraScrollableControl1.Controls)
            {
                if (c.GetType().ToString() == "QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI.CtrlInputArg")
                {
                    CtrlInputArg cia = c as CtrlInputArg;
                    if (cia.textEdit.Text.Length == 0)
                        return string.Empty;
                    switch (cia.labelControl.Text)
                    {
                        default:
                        case "输出文件夹名": foldname = cia.textEdit.Text; break;
                        case "切片层级": z = cia.textEdit.Text; break;
                        case "最小经度": xmin = cia.textEdit.Text; break;
                        case "最大经度": xmax = cia.textEdit.Text; break;
                        case "最小纬度": ymin = cia.textEdit.Text; break;
                        case "最大纬度": ymax = cia.textEdit.Text; break;
                    }
                }
                else if (c.GetType().ToString() == "QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI.CtrlSelectArg")
                {
                    CtrlSelectArg csa = c as CtrlSelectArg;
                    if (csa.memoEdit.Text.Length == 0)
                        return string.Empty;
                    input = csa.memoEdit.Text;
                }
            }

            try
            {
                Int32.Parse(z);
                Double.Parse(xmin);
                Double.Parse(ymin);
                Double.Parse(xmax);
                Double.Parse(ymax);
            }
            catch
            {
                return string.Empty;
            }

            string head = "//" + shareRoot + "/gfjtdata";
            input = input.Replace("\\", "/").Replace(head, "file://" + mountPath);
            command = string.Format("hadoop jar /root/share/compute/jarPackage/TileLine.jar {0} " + "file:///root/share/compute/result/tile/{1} {2} {3} {4} {5} {6}" + " tileline", input, foldname, z, xmin, xmax, ymin, ymax);

            string dir = "\\\\" + shareRoot + "\\gfjtdata\\compute\\result\\tile\\" + foldname;
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);

            return command;
        }
        //生成裁剪运算的命令
        private string getClipCommand()
        {
            string command = "";
            string inpolygon = string.Empty;
            string inraster = string.Empty;
            string foldname = string.Empty;
            string bufdist = string.Empty;
            string mapnum = string.Empty;

            foreach (Control c in xtraScrollableControl1.Controls)
            {
                if (c.GetType().ToString() == "QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI.CtrlInputArg")
                {
                    CtrlInputArg cia = c as CtrlInputArg;
                    if (cia.textEdit.Text.Length == 0)
                        return string.Empty;
                    switch (cia.labelControl.Text)
                    {
                        default:
                        case "输出文件夹名": foldname = cia.textEdit.Text; break;
                        case "缓冲区宽度": bufdist = cia.textEdit.Text; break;
                        case "Map操作数": mapnum = cia.textEdit.Text; break;
                    }
                }
                else if (c.GetType().ToString() == "QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI.CtrlSelectArg")
                {
                    CtrlSelectArg csa = c as CtrlSelectArg;
                    if (csa.memoEdit.Text.Length == 0)
                        return string.Empty;
                    switch (csa.groupControl.Text)
                    {
                        default:
                        case "输入栅格": inraster = csa.memoEdit.Text; break;
                        case "输入矢量": inpolygon = csa.memoEdit.Text; break;
                    }
                }
            }

            try
            {
                Double.Parse(bufdist);
                Int32.Parse(mapnum);
            }
            catch
            {
                return string.Empty;
            }

            string head = "//" + shareRoot + "/gfjtdata";
            inraster = inraster.Replace("\\", "/").Replace(head, mountPath);
            inpolygon = inpolygon.Replace("\\", "/").Replace(head, "file://" + mountPath);
            string dir = "\\\\" + shareRoot + "\\gfjtdata\\result\\clipoutput" + foldname;
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            command = string.Format("hadoop jar /root/share/compute/jarPackage/clip.jar {0} {1} {2} file:///root/share/compute/result/clipoutput/{3} {4} {5} {6} {7}",
                foldname, inpolygon, inraster, foldname, "1", "0", bufdist, mapnum);
            return command;
        }
        //生成自定义算法的命令
        private string getMakeCommand()
        {
            string command = string.Empty;
            if (args.Length > 0)
                args = " " + args;
            command = "hadoop jar " + jarPath + args;
            return command;
        }
        //自定义算法
        string jarPath;
        string args;
        public bool makeAlgorithm()
        {
            FrmAlgMake fam = new FrmAlgMake();
            fam.StartPosition = FormStartPosition.CenterParent;
            if (fam.ShowDialog() == DialogResult.OK)
            {
                selectedAlgIndex = 0;
                this.xtraScrollableControl1.Controls.Clear();
                jarPath = fam.jarPath;
                string head = "//" + shareRoot + "/gfjtdata";
                jarPath = jarPath.Replace("\\", "/").Replace(head, mountPath);
                args = string.Empty;
                for (int i = 0; i < fam.args.Count; i++)
                {
                    args += fam.args[i];
                    if (i < fam.args.Count - 1)
                        args += " ";
                }
                Thread t = new Thread(new ThreadStart(run));
                t.IsBackground = true;
                t.Start();
                return true;
            }
            else
                return false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //DataTable siteDataTable = TServerSiteMoniter.GetBasicSiteInfo();
            //double d=TServerSiteManager.StorageSites[0].Status.GetDivAvalableFreeSpace();
           // d = TServerSiteManager.StorageSites[0].Status.GetDivLoadSize();
            //d = TServerSiteManager.StorageSites[0].Status.GetDivToatalsize();
            getSubSystemInfo();
        }

        //获取子系统监控信息
        public void getSubSystemInfo() 
        {
            DataTable subInfoTb;
            DataTable waitT;
            DataTable runT;
            DataTable comT;
            while (true)
            {
                //获取子系统主机信息
                subInfoTb = TServerSiteMoniter.GetBasicSiteInfo();
                //删除运管行
                //subInfoTb.Rows.RemoveAt(0);
                //增添信息列
                subInfoTb.Columns.Add("WaitingTasks");
                subInfoTb.Columns.Add("ProcessingTasks");
                subInfoTb.Columns.Add("CompletedTasks");
                //获取任务信息
                waitT = taskorder.GetTaskTable("waiting");
                runT = taskorder.GetTaskTable("processing");
                comT = taskorder.GetTaskTable("completed");
                //填表
                foreach (DataRow row in subInfoTb.Rows) 
                {
                    string subSystem = row["SiteName"].ToString();
                    if (row["RunningState"].ToString() == "正在运行")
                    {
                        var count = waitT.Compute("Count(SubsystemName)", "SubsystemName = '" + subSystem + "'");
                        row["WaitingTasks"] = (int)count;
                        count = runT.Compute("Count(SubsystemName)", "SubsystemName = '" + subSystem + "'");
                        row["ProcessingTasks"] = count;
                        count = comT.Compute("Count(SubsystemName)", "SubsystemName = '" + subSystem + "'");
                        row["CompletedTasks"] = count;
                    }
                }
                //double rateStore = 0;
                double rateMemo = 0;
                double rateCPU = 0;
                string subSystemName = "";
                double usedStore = 0;
                double remainedStore = 0;
                DataRow selectedRow = gridView4.GetFocusedDataRow();
                int selectedIndex = 0;
                if (selectedRow != null)
                {
                    selectedIndex = gridView4.FocusedRowHandle;
                    subSystemName = selectedRow["SiteName"].ToString();
                }
                else
                {
                    subSystemName = subInfoTb.Rows[0]["SiteName"].ToString();
                }
                for (int i = 0; i < TServerSiteManager.StorageSites.Count; i++)
                {
                    if (TServerSiteManager.StorageSites[i].SiteName == subSystemName)
                    {
                        if (TServerSiteManager.StorageSites[i].RunningState == QRST_DI_SS_Basis.EnumSiteStatus.Running)
                        {
                            //计算所选子系统的磁盘使用情况
                            usedStore = TServerSiteManager.StorageSites[i].Status.GetDivLoadSize();
                            remainedStore = TServerSiteManager.StorageSites[i].Status.GetDivAvalableFreeSpace();
                            //double allStore = TServerSiteManager.StorageSites[i].Status.GetDivToatalsize();
                            //rateStore = allStore == 0 ? 0 : (usedStore / allStore);
                            //计算所选子系统的内存使用率
                            double availMemo = TServerSiteManager.StorageSites[i].Status.AvailPhys;
                            double allMemo = TServerSiteManager.StorageSites[i].Status.TotalPhysicalMemory;
                            rateMemo = allMemo == 0 ? 0 : (availMemo / allMemo);
                            //获取所选子系统的CPU使用率
                            rateCPU = TServerSiteManager.StorageSites[i].Status.CPU / 100;
                        }
                        else
                        {
                            usedStore = remainedStore = rateMemo = rateCPU = 0;
                        }
                        break;
                    }
                }
               
                this.Invoke((EventHandler)delegate
                {
                    
                    grid_subSystemInfo.DataSource = subInfoTb;
                    //gridView4.Columns[6].BestFit();
                    //gridView4.Columns[7].BestFit();
                    //gridView4.Columns[8].BestFit();
                    grid_subSystemInfo.Refresh();
                    gridView4.FocusedRowHandle = selectedIndex;

                    if (chartControl3.Series[0].Points.Count > 0)
                    {
                        //chartControl3.Series[0].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("内存使用率", rateMemo));
                        chartControl3.Series[0].Points.RemoveAt(0);
                    }
                    if (chartControl3.Series[1].Points.Count > 0)
                    {
                        //chartControl3.Series[1].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("CPU使用率", rateCPU));
                        chartControl3.Series[1].Points.RemoveAt(0);
                    }
                    if (chartControl4.Series[0].Points.Count > 0)
                    {
                        //chartControl4.Series[0].Points.Add(new DevExpress.XtraCharts.SeriesPoint("磁盘使用率", usedStore));
                        //chartControl4.Series[0].Points.Add(new DevExpress.XtraCharts.SeriesPoint("磁盘可用率", remainedStore));
                        chartControl4.Series[0].Points.RemoveAt(1);
                        chartControl4.Series[0].Points.RemoveAt(0);
                    }
                    //绘制柱状图和饼状图
                    chartControl3.Series[0].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("内存使用率", rateMemo));
                    chartControl3.Series[1].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("CPU使用率", rateCPU));
                    //chartControl3.Series[0].Points[0].Values[0] = rateMemo;
                    //chartControl3.Series[0].Points[0].Argument = "内存使用率";
                    //chartControl3.Series[1].Points[0].Values[0] = rateCPU;
                    //chartControl3.Series[1].Points[0].Argument = "CPU使用率";
                    chartControl3.Titles[0].Text = subSystemName;

                    chartControl4.Series[0].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("磁盘使用率", usedStore));
                    chartControl4.Series[0].Points.Insert(1, new DevExpress.XtraCharts.SeriesPoint("磁盘可用率", remainedStore));
                    //chartControl4.Series[0].Points[0].Values[0] = usedStore;
                    //chartControl4.Series[0].Points[0].Argument = "已用磁盘空间";
                    //chartControl4.Series[0].Points[1].Values[0] = remainedStore;
                    //chartControl4.Series[0].Points[1].Argument = "剩余磁盘空间";
                    chartControl4.Titles[0].Text = subSystemName;
                });
                Thread.Sleep(1000);
            }
        }
        private void gridView4_Click(object sender, EventArgs e)
        {
            double rateMemo = 0;
            double rateCPU = 0;       
            double usedStore = 0;
            double remainedStore = 0;
            DataRow selectedRow = gridView4.GetFocusedDataRow();
            string subSystemName = selectedRow["SiteName"].ToString();
            for (int i = 0; i < TServerSiteManager.StorageSites.Count; i++)
            {
                if (TServerSiteManager.StorageSites[i].SiteName == subSystemName)
                {
                    if (TServerSiteManager.StorageSites[i].RunningState == QRST_DI_SS_Basis.EnumSiteStatus.Running)
                    {
                        //计算所选子系统的磁盘使用情况
                        usedStore = TServerSiteManager.StorageSites[i].Status.GetDivLoadSize();
                        remainedStore = TServerSiteManager.StorageSites[i].Status.GetDivAvalableFreeSpace();
                        //计算所选子系统的内存使用率
                        double availMemo = TServerSiteManager.StorageSites[i].Status.AvailPhys;
                        double allMemo = TServerSiteManager.StorageSites[i].Status.TotalPhysicalMemory;
                        rateMemo = allMemo == 0 ? 0 : (availMemo / allMemo);
                        //获取所选子系统的CPU使用率
                        rateCPU = TServerSiteManager.StorageSites[i].Status.CPU / 100;
                    }
                    else
                    {
                        usedStore = remainedStore = rateMemo = rateCPU = 0;
                    }
                    break;
                }
            }

            if (chartControl3.Series[0].Points.Count > 0)
            {
                chartControl3.Series[0].Points.RemoveAt(0);
            }
            if (chartControl3.Series[1].Points.Count > 0)
            {
                chartControl3.Series[1].Points.RemoveAt(0);
            }
            if (chartControl4.Series[0].Points.Count > 0)
            {
                chartControl4.Series[0].Points.RemoveAt(1);
                chartControl4.Series[0].Points.RemoveAt(0);
            }
            //绘制柱状图和饼状图
            chartControl3.Series[0].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("内存使用率", rateMemo));
            chartControl3.Series[1].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("CPU使用率", rateCPU));
            chartControl3.Titles[0].Text = subSystemName;

            chartControl4.Series[0].Points.Insert(0, new DevExpress.XtraCharts.SeriesPoint("磁盘使用率", usedStore));
            chartControl4.Series[0].Points.Insert(1, new DevExpress.XtraCharts.SeriesPoint("磁盘可用率", remainedStore));
            chartControl4.Titles[0].Text = subSystemName;
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }
        
    }
}
