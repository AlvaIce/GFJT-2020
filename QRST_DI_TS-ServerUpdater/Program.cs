using System;
using System.Collections.Generic;
using System.IO;
using QRST_DI_Resources;
using System.Threading;
using System.Runtime.InteropServices;
using QRST_DI_DS_Basis;
using QRST_DI_TS_Process.Site;
using System.Configuration;
using System.Data;
using QRST_DI_SS_DBClient.TCP;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_ServerUpdater
{
    class Program
    {

        //\\{IP}\QRST_DB_Prototype\QRST_DB_Program\QRST_DI_TS_SiteServer_Console\QDB_TS_Server\
        //\\{IP}\QRST_DB_Prototype\QRST_DB_Program\QRST_DI_TS_SiteServer_Console\QDB_TS_Updater\
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public static string TSServerDir;
        static string curDir;
        static string RecordTextdir;
        static string RecordTextPath;
        static string updateDir;
        static string extractTmpDir;
        private static int _tsscheckInterval;

        private static IDbBaseUtilities mysql = null;
        public static void openAllServe()
        {
            bool isStart = false;
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in ps)
            {
                if (p.ProcessName == "QRST_DI_TS_SiteServer_Console")
                {
                    isStart = true;
                }
            }
            if (!isStart)
            {
                string TSServerExePath = string.Format("{0}{1}", TSServerDir, "\\QRST_DI_TS_SiteServer_Console.exe");
                System.Diagnostics.Process.Start(TSServerExePath);
            }
        }
        //关闭所有服务
        public static void closeAllServe()
        {
            setTSServerState("Stopped");
            DateTime now = DateTime.Now;
            bool isStopped=false;
            while ((DateTime.Now - now).TotalSeconds < 120 && !isStopped)
            {
                bool existTSSProcess = false;
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in ps)
                {
                    if (p.ProcessName == "QRST_DI_TS-SiteServer" || p.ProcessName == "QRST_DI_TS_SiteServer_Console")
                    {
                        existTSSProcess = true;                        
                        break;
                    }
                }

                if (existTSSProcess)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    isStopped = true;
                }
            }

            if (!isStopped)
            {
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in ps)
                {
                    if (p.ProcessName == "QRST_DI_TS-SiteServer" || p.ProcessName == "QRST_DI_TS_SiteServer_Console")
                    {
                        p.Kill();
                    }
                }
            }
        }
        //重启所有服务
        public static void restartAllServe()
        {
            closeAllServe();
            string TSServerExePath = string.Format("{0}{1}", TSServerDir, "\\QRST_DI_TS_SiteServer_Console.exe");
            System.Diagnostics.Process.Start(TSServerExePath);
        }
        public static void setUpdaterState(string str)
        {
            Console.WriteLine("开始更新自身状态");

            //MySqlBaseUtilities mysql = new MySqlBaseUtilities();
            Console.WriteLine("SQLite：" + mysql.GetDbConnection());

            string sql = string.Format("UPDATE tileserversitesinfo set UpdateIsAlive='{0}' where ADDRESSIP='{1}'", str, Constant.UsingIPAddress);
            mysql.ExecuteSql(sql);
        }

        public static string getTSServerState()
        {
            //MySqlBaseUtilities mysql = new MySqlBaseUtilities();
            string sql = string.Format("Select RunningState from tileserversitesinfo where ADDRESSIP='{0}'", Constant.UsingIPAddress);
            DataSet ds = mysql.GetDataSet(sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }


        public static void setTSServerState(string str)
        {
            //MySqlBaseUtilities mysql = new MySqlBaseUtilities();
            string sql = string.Format("UPDATE tileserversitesinfo set RunningState='{0}' where ADDRESSIP='{1}'", str, Constant.UsingIPAddress);
            mysql.ExecuteSql(sql);
        }

        static void Main(string[] args)
        {
            Console.Title = "QRST_DI_TS-ServerUpdater";
            _tsscheckInterval = 0;
            curDir = System.AppDomain.CurrentDomain.BaseDirectory;
            RecordTextdir = string.Format(@"{0}Logs", curDir);
            if(!Constant.Created)
            {
                Constant.InitializeTcpConnection();
                Constant.Create();

            }
            mysql = Constant.IdbServerUtilities;
            //打开所有服务
            if (!Directory.Exists(RecordTextdir))
            {
                Directory.CreateDirectory(RecordTextdir);
            }
            RecordTextPath = string.Format(@"{0}\TSSUpdaterLog_{1}.txt", RecordTextdir, DateTime.Today.ToString("yyyyMMdd"));
            if (!File.Exists(RecordTextPath))
            {
                FileStream fs = File.Create(RecordTextPath);
                fs.Close();
            }
            //初始化更新包文件夹
            updateDir = Path.Combine(curDir, "Updates");
            extractTmpDir = Path.Combine(curDir, "ZipTmp");
            TSServerDir = Path.Combine(curDir.Substring(0, curDir.TrimEnd('\\').LastIndexOf("\\")), "QDB_TS_Server");
            //测试
            string strr = string.Format("{0}{1}", TSServerDir.Substring(0, TSServerDir.LastIndexOf("\\")), "\\QDB_TS_Updater\\msg\\1.txt");
            //获取站点配置信息
            getTSServerSetting();
            setUpdaterState("true");


            //显示程序
            IntPtr mwinhand = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(mwinhand, 1);


            while (true)
            {
                #region 更新包
                try
                {
                    if (!Directory.Exists(updateDir))
                    {
                        Directory.CreateDirectory(updateDir);
                    }
                    //检查是否有新的更新包
                    string[] fn = Directory.GetFiles(updateDir, "*.zip", SearchOption.TopDirectoryOnly);
                    List<string> updateFiles = new List<string>();
                    updateFiles.AddRange(fn);

                    if (updateFiles.Count > 0)
                    {
                        //显示程序
                         mwinhand = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                        ShowWindow(mwinhand, 1);
                        //关闭站点服务进程
                        System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
                        foreach (System.Diagnostics.Process p in ps)
                        {
                            if (p.ProcessName == "QRST_DI_TS-SiteServer" || p.ProcessName == "QRST_DI_TS_SiteServer_Console")
                            {
                                p.Kill();
                            }
                        }
                        //删除站点程序
                        bool deletesucc = false;
                        int times = 0;
                        while (!deletesucc && times < 15)
                        {

                            deletesucc = DirectoryUtil.DeleteDirTraversal(TSServerDir);
                            if (deletesucc)
                            {
                                break;
                            }
                            System.Threading.Thread.Sleep(1000);
                            Console.WriteLine("旧版本删除失败，等待占用被解除后重试（{0}/15）。", times.ToString());
                            times++;
                        }

                        ////按时间排序
                        //DateTime[] fcdts = new DateTime[fn.Length];
                        for (int i = updateFiles.Count - 1; i > 0; i--)
                        {
                            if (File.GetCreationTime(updateFiles[i]) < File.GetCreationTime(updateFiles[i - 1]))
                            {
                                updateFiles.RemoveAt(i);
                            }
                            else
                            {
                                updateFiles.RemoveAt(i - 1);
                            }
                        }
                        //解压更新包
                        try
                        {
                            if (Directory.Exists(extractTmpDir))
                            {
                                Directory.Delete(extractTmpDir, true);
                            }
                            Directory.CreateDirectory(extractTmpDir);
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                        ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
                        //fz.ExtractZip(updateFiles[0], TSServerDir, "");
                        fz.ExtractZip(updateFiles[0], extractTmpDir, "");
                        //copy files
                        DirectoryUtil.CopyDirTraversal(extractTmpDir, TSServerDir);

                        //重启服务
                        string TSServerExePath = Path.Combine(TSServerDir, "QRST_DI_TS_SiteServer_Console.exe");
                        System.Diagnostics.Process.Start(TSServerExePath);


                        StreamWriter sw = new StreamWriter(RecordTextPath, true);
                        sw.WriteLine(string.Format("{0}更新成功，更新包:{1}", DateTime.Now.ToString(), updateFiles[0]));
                        sw.Close();
                        //清空更新包
                        Directory.Delete(updateDir, true);
                    }
                }
                catch (Exception ex)
                {
                    StreamWriter sw = new StreamWriter(RecordTextPath, true);
                    sw.WriteLine(string.Format("{0}更新失败:{1}", DateTime.Now.ToString(), ex.Message));
                    sw.Close();
                    Thread.Sleep(5000);
                }
                    #endregion

                #region 站点更新管理器命令
                string url1 = string.Format("{0}{1}", TSServerDir.Substring(0, TSServerDir.LastIndexOf("\\")), "\\QDB_TS_Updater\\msg\\start.txt");
                string url2 = string.Format("{0}{1}", TSServerDir.Substring(0, TSServerDir.LastIndexOf("\\")), "\\QDB_TS_Updater\\msg\\end.txt");
                string url3 = string.Format("{0}{1}", TSServerDir.Substring(0, TSServerDir.LastIndexOf("\\")), "\\QDB_TS_Updater\\msg\\restart.txt");
                string url4 = string.Format("{0}{1}", TSServerDir.Substring(0, TSServerDir.LastIndexOf("\\")), "\\QDB_TS_Updater\\msg\\isAlive.txt");
                //查询Updater状态
                if (File.Exists(url4))
                {

                    Console.WriteLine("开始查询状态");

                    Thread.Sleep(200); //防止写未释放
                    File.Delete(url4);
                    //更新Updater设置
                    getTSServerSetting();
                    setUpdaterState("true");
                    CheckUpdateTSServerStates();
                }
                //打开所有服务
                if (File.Exists(url1))
                {
                    File.Delete(url1);
                    openAllServe();
                }

                //关闭所有服务
                if (File.Exists(url2))
                {
                    File.Delete(url2);
                    closeAllServe();
                }
                //重启所有服务
                if (File.Exists(url3))
                {
                    if (File.Exists(url1))
                    {
                        File.Delete(url1);
                    }
                    if (File.Exists(url2))
                    {
                        File.Delete(url2);
                    }
                    File.Delete(url3);
                    restartAllServe();
                }
                #endregion

                #region 站点服务程序状态更新
                //自动环境下每1分钟执行一次
                if (_tsscheckInterval == 30)
                {
                    CheckUpdateTSServerStates();
                    _tsscheckInterval = 0;
                }

                _tsscheckInterval++;

                #endregion

                Thread.Sleep(2000); //JOKI 150615 更新 减少负荷 原高速循环
            }
        }

        private static void CheckUpdateTSServerStates()
        {
            string ttsstate = getTSServerState();
            string tssstate=checkTSServerState();
            Console.WriteLine("站点状态检查结果：" + tssstate);

                if (ttsstate != "" && ttsstate != "Dead" && ttsstate != "Stopped" && tssstate.Contains("false"))
            {
                setTSServerState("Dead");
            }
        }

        private static void getTSServerSetting()
        {
            if (checkTSServerFilesExist())
            {
                string tssconfigfile = Path.Combine(TSServerDir, "QRST_DI_TS_SiteServer_Console.exe.config");
                string thisconfigfile = Path.Combine(curDir, "QRST_DI_TS-ServerUpdater.exe.config");
                File.Copy(tssconfigfile, thisconfigfile, true);
                Configuration cfg = ConfigurationManager.OpenExeConfiguration(Path.Combine(curDir, "QRST_DI_TS-ServerUpdater.exe"));
                Constant.Create(cfg);
                Console.WriteLine("获取站点配置mysql");
                Console.WriteLine(Constant.ConnectionStringMySql);

            }
            else
            {
                Constant.Create();
            }
        }

        private static bool checkTSServerFilesExist()
        {
            if (Directory.Exists(TSServerDir))
            {
                if (File.Exists(Path.Combine(TSServerDir, "QRST_DI_TS_SiteServer_Console.exe")) &&
                    File.Exists(Path.Combine(TSServerDir, "QRST_DI_TS_SiteServer_Console.exe.config")))
                {
                    return true;
                }
            }
            return false;
        }

        static TServerSite MyTSSite = null;
        private static string checkTSServerState()
        {
            //检查站点是否已经部署
            if (!checkTSServerFilesExist())
            {
                return @"false,未在../QDB_TS_Server/下找到站点部署文件！";
            }

            //检查站点程序是否运行进程
            bool tssIsInProcesses = false;
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in ps)
            {
                if (p.ProcessName == "QRST_DI_TS_SiteServer_Console")
                {
                    tssIsInProcesses = true;
                    break;
                }
            }
            if (!tssIsInProcesses)
            {
                return "false,未找到QRST_DI_TS_SiteServer_Console进程！";
            }

            //进程中有站点程序但是指定IP下无响应
            if (MyTSSite == null)
            {
                MyTSSite = TServerSiteManager.CreateTSSiteByIP(Constant.UsingIPAddress);
            }
            try
            {
                bool isrunning = MyTSSite.TCPService.IsRunning;
                if (!isrunning)
                {
                    return "false,站点服务未响应！";
                }
            }
            catch
            {
                return "false,站点服务未响应！";
            }

            return "true";
        }

    }
}