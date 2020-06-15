using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Site;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRST_DI_MS_Basis.Log;
using System.Data;
using System.Configuration;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_DBInterfaces.IDBEngine;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TilesImport
{
    public class TileStore
    {
        //判断任务是否已全部启动
        Boolean isComplement = false;
        //一次最大执行任务数
        private static int MAX_TASK_NUM = 8;
        private List<int> TaskID = new List<int>();
        //因未知原因，部分切片未找到目的IP地址
        private List<string> noIPTiles = new List<string>();
        //任务列表
        private List<Task> tasks = new List<Task>();
        //总任务数
        private static int taskNum;
        //当总任务数超过最大执行任务总数时，保存正在执行的任务
        private Task[] taskRun = new Task[MAX_TASK_NUM];
        //保存每个任务所包含的切片数
        private int[] eachTaskNum;
        public static int TilesCount = 0;
        public static int TilesImportFailed = 0;
        bool isQuit = false;
        DateTime startTime, endTime;
        TimeSpan totalTime;
        List<string> failedFilenames = new List<string>();
        List<string> ipAll = new List<string>();
        DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        private UCTileImport _mainui;
        public TileStore(UCTileImport mainui)
        {
            TaskID.Clear();
            tasks.Clear();
            failedFilenames.Clear();
            ipAll.Clear();
            isQuit = false;
            startTime = DateTime.Now;
            TServerSiteManager.UpdateOptimalStorageSiteList();
            int count = TServerSiteManager.StorageSites.Count;
            List<string> allSiteName = new List<string>();
            //List<string> StopSiteName = new List<string>();
            for (int i = 0; i < count; i++)
            {
                allSiteName.Add(TServerSiteManager.StorageSites[i].SiteName);
            }
            foreach (var item in TServerSiteManager.optimalStorageSites)
            {
                ipAll.Add(item.IPAdress);
            }
            taskNum = ipAll.Count;
            string strStopSite = "";
            if (taskNum < count)
            {
                //System.Windows.Forms.MessageBox.Show("站点未全部开启，",)
                foreach (var item in allSiteName)
                {
                    if (!ipAll.Contains(item))
                        strStopSite += item + " ";
                }
                DialogResult result = MessageBox.Show("站点" + strStopSite + "未开启，是否继续入库？", "提示", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                    isQuit = true;

            }
            eachTaskNum = new int[taskNum];
            this._mainui = mainui;
        }
        /// <summary>
        /// 切片数据入库
        /// </summary>
        /// <param name="tilesPath"></param>

        public void TilesImport(object obj)
        {
            if (isQuit)
            {
                _mainui.ButtonHandle();
                return;
            }
            DateTime timeStart = DateTime.Now;
            Console.WriteLine("开始时间：" + timeStart);
            string tilesPath = (string)obj;
            //log.Add(string.Format("开始切片入库。"));            
            string[] fileNames;
            string[] ips;
            string[] FailedTileName;
            string[] TilesPath = new string[] { tilesPath };
            _mainui.ShowmessageLabel2("开始入库");
            bool success = StoreTiles(TilesPath, out fileNames, out ips, out FailedTileName);
            List<string> listFNs = new List<string>();
            listFNs.AddRange(fileNames);
            List<string> listFTNs = new List<string>();
            listFTNs.AddRange(FailedTileName);
            //失败消息
            //创建日志记录组件实例
            for (int i = 0; i < FailedTileName.Length; i++)
            {
                int j = listFNs.IndexOf(FailedTileName[i]);

                if (j != -1)
                {
                    InforLog<string>.inforLog.Info(string.Format("Error:切片{0}入库失败,所属站点{1}。", FailedTileName[i], ips[j]));
                    //log.Add(string.Format("!!!切片{0}入库失败,所属站点{1}。", FailedTileName[i], ips[j]));

                }
                else
                {
                    InforLog<string>.inforLog.Info(string.Format(string.Format("Error:文件{0}入库失败。", FailedTileName[i])));
                    //log.Add(string.Format("!!!文件{0}入库失败。", FailedTileName[i]));
                }
            }
            //log.Add(string.Format("正在建立切片索引。"));
            _mainui.ShowmessageLabel2("正在建立切片索引……");
            _mainui.ShowmessageLabel3("");
            _mainui.ShowmessageLabel4("");
            _mainui.ShowmessageLabel5("");
            _mainui.ShowmessageLabel6("");
            _mainui.ShowmessageLabel7("");
            _mainui.ShowmessageLabel8("");
            _mainui.ShowmessageLabel9("");
            _mainui.ShowmessageLabel10("");
            if (isClassify)
            {
                ImportClassifySampleMetadata(TilesPath, out fileNames);
                isClassify = false;
            }
            BuildTilesIndex(ips, fileNames, listFTNs);
            //log.Add(string.Format("完成切片索引更新。"));
            TilesCount = fileNames.Length + FailedTileName.Length;
            TilesImportFailed = FailedTileName.Length;
            //  切片文件夹删除  

            //log.Add(string.Format("正在清理工作空间。"));
            try
            {
                //  切片文件夹删除  
                if (Directory.Exists(StorageBasePath.SharePath_TiledData(tilesPath)))
                {
                    Directory.Delete(StorageBasePath.SharePath_TiledData(tilesPath), true);
                }
                ////Directory.Delete(storagePath.SharePath_ClassfiedData, true);
                //if (Directory.Exists(StorageBasePath.SharePath_Products(tilesPath)))
                //{
                //    Directory.Delete(StorageBasePath.SharePath_Products(tilesPath), true);
                //}
            }
            catch (Exception)
            {
                //log.Add(string.Format("!!!工作空间清理失败。"));
            }

            if (_mainui.ctrlVirtualDirSetting1.UsingVirtualDir)
            {
                Add2VirtualDir(fileNames);
                Add2VirtualDir(FailedTileName); 
            }


            _mainui.ShowmessageLabel2("");
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            string totalTimeString = totalTime.ToString().Remove(totalTime.ToString().IndexOf('.'));
            _mainui.ShowmessageLabel7(string.Format("本次执行共提交 {0} 条切片，入库失败 {1} 条。\n总用时：{2} ", TilesCount, TilesImportFailed, totalTimeString));
            _mainui.ButtonHandle();
            _mainui.ShowtextBox1("");
            Console.WriteLine("结束时间：" + endTime + "\n总用时：" + totalTime);
            //log.Add(string.Format("完成切片数据入库。"));
        }

        private void Add2VirtualDir(string[] filenames)
        {
            List<string> fns = new List<string>();
            fns.AddRange(filenames);
            _mainui.ctrlVirtualDirSetting1.AddFileLink(fns);
        }


        List<string> listName = new List<string>();
        bool isClassify = false;
        public void JudgeIsClaaify()
        {
            string importUrl = _mainui.textBox1.Text;
            string[] directoryInfo = Directory.GetDirectories(importUrl);
            if (directoryInfo.Length != 0)
            {
                foreach (var DInfo in directoryInfo)
                {
                    string[] files = Directory.GetFiles(DInfo);
                    if (files.Length != 0)
                    {
                        foreach (var item in files)
                        {
                            string str1 = item.Substring(item.LastIndexOf("\\") + 1).Substring(0, 2);
                            if (item.Substring(item.LastIndexOf("\\") + 1).Substring(0, 2) == "CS")
                            {
                                isClassify = true;
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            else
            {
                string[] files = Directory.GetFiles(importUrl);
                foreach (var item in files)
                {
                    string str1 = item.Substring(item.LastIndexOf("\\") + 1).Substring(0, 2);
                    if (item.Substring(item.LastIndexOf("\\") + 1).Substring(0, 2) == "CS")
                    {
                        isClassify = true;
                        break;
                    }
                }
            }
        }
        private void ImportClassifySampleMetadata(string[] TilesPath,out string[] fileNames)
         {
             string importUrl = _mainui.textBox1.Text;
            string [] directoryInfo = Directory.GetDirectories(importUrl);
                if (directoryInfo.Length != 0)
                {
                    foreach (var DInfo in directoryInfo)
                    {
                        //判断excel CSXXXX.excl文件
                        reFileName(DInfo);
                    }
                }
                else
                {
                    reFileName(importUrl);
                }
                fileNames = listName.ToArray();
        }
        public void reFileName(string directoryInfo)
        {
            string[] files = Directory.GetFiles(directoryInfo);
            string ID = null;
            if (files.Length != 0)
            {
                foreach (var item in files)
                {
                    string str1 = item.Substring(item.LastIndexOf("\\") + 1).Substring(0, 2);
                    if (item.Substring(item.LastIndexOf("\\") + 1).Substring(0, 2) == "CS")
                    {
                        if (item.Substring(item.LastIndexOf(".") + 1) == "txt")
                        {
                            StreamReader sr = new StreamReader(item, Encoding.Default);
                            string content = sr.ReadLine();
                            string contentTxt = null;
                            while (content != null)
                            {
                                contentTxt = content;
                                if (content.Substring(0, 1) == "*")
                                {
                                    content = "";
                                    content = sr.ReadLine();
                                }
                                else
                                {
                                    content = sr.ReadLine();
                                }
                            }
                            string[] array = contentTxt.Split('#');
                            string str = ConfigurationManager.AppSettings["ConnectionStringSampleMySql"];
                            ISQLiteBaseUtilities mySqlBase = Constant.IMidbUtilities.GetSubDbUtilByCon(str);
                            string sql = string.Format("insert into collectsampleInfo (CategoryName,CategoryCode,CollectPeople,CollectionTime,SampleDescription) values ('{0}','{1}','{2}','{3}','{4}')", array[0], array[1], array[2], array[3], array[4]);
                            mySqlBase.ExecuteSql(sql);
                            string selectSql = string.Format("select ID from collectsampleInfo where CategoryCode='{0}'", array[1]);
                            DataSet ds = mySqlBase.GetDataSet(selectSql);
                            foreach (DataRow idMark in ds.Tables[0].Rows)
                            {
                                ID = idMark[0].ToString();
                            }
                            int index = item.Substring(item.LastIndexOf("\\") + 1).IndexOf("_", 3);
                            string sampleFileNameFirst = item.Substring(item.LastIndexOf("\\") + 1).Substring(0, index + 1);
                            string sampleFileNameLast = item.Substring(item.LastIndexOf("\\") + 1).Substring(index + 1, item.Substring(item.LastIndexOf("\\") + 1).LastIndexOf(".") - index - 1);
                            listName.Add(sampleFileNameFirst + ID + "_" + sampleFileNameLast + ".tif");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 存储切片
        /// </summary>
        /// <param name="tilesPaths"></param>
        /// <param name="ssService"></param>
        /// <param name="filenames"></param>
        /// <param name="ips"></param>
        /// <param name="failedTiles"></param>
        /// <returns></returns>
        private bool StoreTiles(string[] tilesPaths, out string[] filenames, out string[] ips, out string[] failedTiles)
        {
            bool rtn = true;
            //Dictionary<string, List<string>> dic_tilefileNamesPerIP = new Dictionary<string, List<string>>();
            filenames = new string[0];
            ips = new string[0];
            failedTiles = new string[0];
            try
            {
                if (tilesPaths == null)
                {
                    return false;
                }


                foreach (string tilesPath in tilesPaths)
                {
                    if (!Directory.Exists(tilesPath))
                    {
                        continue;
                    }

                    DirectoryInfo di_data = new DirectoryInfo(tilesPath);
                    Dictionary<string, tileDesPath> tilefileNames = new Dictionary<string, tileDesPath>();
                    Dictionary<string, string> tilefileNames1 = new Dictionary<string, string>();
					FileInfo[] fileinfos=di_data.GetFiles("*.*", SearchOption.AllDirectories);
                    foreach (FileInfo fi in fileinfos)
                    {
                        if (fi.Name.ToUpper().EndsWith(".TIF") || fi.Name.ToUpper().EndsWith(".JPG") || fi.Name.ToUpper().EndsWith(".PNG"))
                        {
                            string ip = "-1";   //-1表示错误，0表示不是瓦片
                            string desPath = da.GetPathByFileName(fi.Name, out ip);
                            if (ip != "-1" && ip !="0")
                            {
                                tileDesPath tile = new tileDesPath();
                                tile.Ip = ip;
                                tile.Despath = desPath;
								try {
									tilefileNames.Add(fi.FullName, tile);
									tilefileNames1.Add(fi.Name, ip);
								}
								catch {
									continue;
								}
                            }
                            else if (ip == "-1")
                            {
                                noIPTiles.Add(fi.FullName);
                            }
                        }
                    }
                    //Dictionary<string, tileDesPath>列表，根据目标IP数目创建Dictionary<string, tileDesPath>
                    //每个Dictionary<string, tileDesPath>拥有同一目标站点
                    List<Dictionary<string, tileDesPath>> taskListDic = new List<Dictionary<string, tileDesPath>>();
                    foreach (var item in ipAll)
                    {
                        Dictionary<string, tileDesPath> strDic = new Dictionary<string, tileDesPath>();
                        taskListDic.Add(strDic);
                    }

                    foreach (var item in tilefileNames)
                    {
                        for (int i = 0; i < ipAll.Count; i++)
                        {
                            if (item.Value.Ip == ipAll.ElementAt(i))
                                taskListDic.ElementAt(i).Add(item.Key, item.Value);
                        }
                    }
                    //根据目标IP，为每个站点分配一个任务
                    TaskID.Clear();
                    for (int k = 0; k < taskNum; k++)
                    {
                        Task task = new Task(o => movethread((Dictionary<string, tileDesPath>)o), taskListDic.ElementAt(k));
                        tasks.Add(task);
                        eachTaskNum[k] = taskListDic.ElementAt(k).Count;
                    }
                    //为因未知原因未找到目标IP的瓦片分配一个任务
                    List<Task> taskFail = new List<Task>();
                    if (noIPTiles.Count > 0)
                    {
                        // taskNum++;
                        Task taskfailed = new Task(o => moveFailTiles((List<string>)o), noIPTiles);
                        taskFail.Add(taskfailed);
                        taskfailed.Start();
                        //eachTaskNum[taskNum-1] = noIPTiles.Count;
                    }
                    _mainui.ShowmessageLabel2("分配任务已完成，共分" + taskNum + "个任务");
                    //任务总数不足最大可执行任务总数时，任务全部启动；任务总数大于可执行任务总数时，一次启动可执行任务总数的任务，循环监视，每当一个任务结束时，再启动下一个任务。直到所有任务都已被启动。
                    if (taskNum <= MAX_TASK_NUM)
                    {
                        foreach (Task item in tasks)
                        {
                            item.Start();
                            //Console.WriteLine(DateTime.Now.ToString());
                        }
                        _mainui.ShowmessageLabel7("任务已全部启动，共" + taskNum + "个任务正在执行！请耐心等待……");
                    }
                    else
                    {
                        taskRun = new Task[MAX_TASK_NUM];
                        for (int i = 0; i < MAX_TASK_NUM; i++)
                        {
                            taskRun[i] = tasks.ElementAt(i);
                            taskRun[i].Start();
                            // Console.WriteLine(DateTime.Now.ToString());
                        }
                        _mainui.ShowmessageLabel7(MAX_TASK_NUM + "个任务正在执行,还有" + (taskNum - MAX_TASK_NUM) + "个任务未启动，请耐心等待……");
                        //下一个未启动的任务的索引
                        int runIndex = MAX_TASK_NUM;
                        //判断是否有任务已结束
                        Boolean hasComplement;
                        while (!isComplement)
                        {
                            hasComplement = false;
                            int i = 0;
                            foreach (Task item in taskRun)
                            {
                                if (item.IsCompleted)
                                {
                                    runIndex++;
                                    hasComplement = true;
                                    break;
                                }
                                i++;
                            }
                            if (hasComplement)
                            {
                                taskRun[i] = tasks.ElementAt(runIndex - 1);
                                taskRun[i].Start();
                                _mainui.ShowmessageLabel7(String.Format("已完成{0}个任务，{1}个任务正在执行,还有{2}个任务未启动，请耐心等待……", (runIndex - MAX_TASK_NUM), MAX_TASK_NUM, (taskNum - runIndex)));
                                //若所有任务已启动，则取消监视
                                if (runIndex == taskNum)
                                    isComplement = true;
                            }
                            //Thread.Sleep(60000);
                        }
                        Task.WaitAll(taskRun);
                    }
                    //等待最后任务结束
                    Task.WaitAll(tasks.ToArray());
                    if(taskFail.Count>0)
                        Task.WaitAll(taskFail.ToArray());
                    #region   lxl20120906 Update wyn20120908 修改切片不能入库问题
                    string[] tempfilenames = new string[tilefileNames1.Keys.Count];
                    tilefileNames1.Keys.CopyTo(tempfilenames, 0);
                    string[] tempallfilenames = new string[tilefileNames1.Keys.Count + filenames.Length];
                    tempfilenames.CopyTo(tempallfilenames, 0);
                    filenames.CopyTo(tempallfilenames, tempfilenames.Length);
                    filenames = tempallfilenames;
                    string[] tempips = new string[tilefileNames1.Values.Count];
                    tilefileNames1.Values.CopyTo(tempips, 0);
                    string[] tempallipss = new string[tilefileNames1.Values.Count + ips.Length];
                    tempips.CopyTo(tempallipss, 0);
                    ips.CopyTo(tempallipss, tempips.Length);
                    ips = tempallipss;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                rtn = false;
            }
            failedTiles = new string[failedFilenames.Count];
            failedFilenames.CopyTo(failedTiles);
            return rtn;
        }

        /// <summary>
        /// 存储切片子任务
        /// </summary>
        /// <param name="tilefileNames"></param>
        private void movethread(Dictionary<string, tileDesPath> tilefileNames)
        {
            int taskid = TaskID.Count;
            TaskID.Add(TaskID.Count);
            //Console.WriteLine(string.Format("{0}-{1}", taskid, DateTime.Now.ToString()));
            //Dictionary<string, string> tilefileNames = obj as Dictionary<string, string>;
            int count = tilefileNames.Count;
            for (int i = 0; i < count; i++)
            {
                string Name = tilefileNames.ElementAt(i).Key;
                FileInfo fi = new FileInfo(Name);
                string ip = tilefileNames.ElementAt(i).Value.Ip;
                string strpath = "";
                //  切片入库
                //DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                //string destPath = da.GetPathByFileName(fi.Name, out ip);
                string destPath = tilefileNames.ElementAt(i).Value.Despath;
                try
                {
                    strpath = destPath.Substring(0, destPath.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(strpath))
                    {
                        Directory.CreateDirectory(strpath);
                    }
                    string FullName = fi.FullName;
                    //File.Copy(fi.FullName, destPath, true);
                    if (FileMoveTo(FullName, destPath))
                    {
                        //throw new System.UnauthorizedAccessException();

                        if (fi.Name.ToUpper().EndsWith(".JPG"))
                        {
                            string jgwSourcePath = Path.Combine(Path.GetDirectoryName(FullName), Path.GetFileNameWithoutExtension(FullName) + ".jgw");
                            string jgwDestpath = Path.Combine(Path.GetDirectoryName(destPath), Path.GetFileNameWithoutExtension(destPath) + ".jgw");
                            if (File.Exists(jgwSourcePath))
                            {
                                //string jgwDestpath = Path.GetFileNameWithoutExtension(destPath) + ".jgw";
                                FileMoveTo(jgwSourcePath, jgwDestpath);
                            }
                        }
                        if (fi.Name.ToUpper().EndsWith(".PNG"))
                        {
                            string jgwSourcePath = Path.Combine(Path.GetDirectoryName(FullName), Path.GetFileNameWithoutExtension(FullName) + ".pgw");
                            string jgwDestpath = Path.Combine(Path.GetDirectoryName(destPath), Path.GetFileNameWithoutExtension(destPath) + ".pgw");
                            if (File.Exists(jgwSourcePath))
                            {
                                //string jgwDestpath = Path.GetFileNameWithoutExtension(destPath) + ".jgw";
                                FileMoveTo(jgwSourcePath, jgwDestpath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //失败的瓦片暂时存放在服务器上的FailedTileTempPath里
                    Console.WriteLine(ex.Message);
                    try
                    {
                        string failedpath = string.Format(@"{0}{1}", GetFailedTilePath(), fi.Name);
                        if (!Directory.Exists(Path.GetDirectoryName(failedpath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(failedpath));
                        }
                        if (!fi.FullName.Equals(failedpath) && File.Exists(failedpath))
                        {
                            File.Delete(failedpath);
                        }
                        fi.CopyTo(failedpath, true);
                        failedFilenames.Add(fi.Name);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                finally
                {
                    if (i % 20 == 0 && i != 0 && (eachTaskNum[taskid] - i + 1) > 0)
                    {
                        switch (taskid % MAX_TASK_NUM)
                        {
                            case 0:
                                _mainui.ShowmessageLabel2("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                            case 1:
                                _mainui.ShowmessageLabel3("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                            case 2:
                                _mainui.ShowmessageLabel4("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                            case 3:
                                _mainui.ShowmessageLabel5("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                            case 4:
                                _mainui.ShowmessageLabel6("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                            case 5:
                                _mainui.ShowmessageLabel8("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                            case 6:
                                _mainui.ShowmessageLabel9("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                            case 7:
                                _mainui.ShowmessageLabel10("任务" + (taskid + 1) + "已完成" + i + "条任务，还有" + (eachTaskNum[taskid] - i + 1) + "条未完成！"); break;
                        }
                    }
                }
            }
        }

        private void moveFailTiles(List<string> listTiles)
        {
            //int taskid = TaskID.Count;
            //TaskID.Add(TaskID.Count);
            //int i = 0;
            foreach (string Name in listTiles)
            {
                try
                {
                    //i++;
                    FileInfo fi = new FileInfo(Name);
                    string failedpath = string.Format(@"{0}{1}", GetFailedTilePath(), fi.Name);
                    if (!Directory.Exists(Path.GetDirectoryName(failedpath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(failedpath));
                    }
                    if (!fi.FullName.Equals(failedpath) && File.Exists(failedpath))
                    {
                        File.Delete(failedpath);
                    }
                    fi.CopyTo(failedpath, true);
                    failedFilenames.Add(Name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /// <summary>
        /// 创建切片索引  
        /// </summary>
        /// <param name="ips"></param>
        /// <param name="filenames"></param>
        /// <param name="listFTNs"></param>
        private void BuildTilesIndex(string[] ips, string[] filenames, List<string> listFTNs)
        {
            //建立切片索引参数
            Dictionary<string, List<string>> tilefileNamesPerIP = new Dictionary<string, List<string>>();
            for (int i = 0; i < ips.Length; i++)
            {
                //过滤失败瓦片
                if (listFTNs.IndexOf(filenames[i]) == -1)
                {
                    if (!tilefileNamesPerIP.ContainsKey(ips[i]))
                    {
                        tilefileNamesPerIP.Add(ips[i], new List<string>());
                        tilefileNamesPerIP[ips[i]].Add(filenames[i]);
                    }
                    else
                    {
                        tilefileNamesPerIP[ips[i]].Add(filenames[i]);
                    }
                }
            }

            UpdateTilesIndex(tilefileNamesPerIP, TileIndexUpdateType.InsertUpdate);
            //更新失败的切片
            UpdateFailedTilesIndex(listFTNs, TileIndexUpdateType.InsertUpdate);
            //return tilefileNamesPerIP;
        }
        //private void updatatile(Dictionary<string, List<string>> tiles)
        //{
        //    UpdateTilesIndex(tiles, TileIndexUpdateType.Up);
        //}
        /// <summary>
        /// 文件转移
        /// </summary>
        /// <param name="scrFilePath"></param>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        private bool FileMoveTo(string scrFilePath, string destFilePath)
        {

            //if (File.Exists(destFilePath))
            //    return false;
            try
            {
                FileInfo fi = new FileInfo(scrFilePath);
                if (!Directory.Exists(Path.GetDirectoryName(destFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
                }
                if (!fi.FullName.Equals(destFilePath) && File.Exists(destFilePath))
                {
                    File.Delete(destFilePath);
                }
                fi.CopyTo(destFilePath, true);
            }
            catch (Exception)
            {
                throw new System.UnauthorizedAccessException();
            }
            return true;
        }

        /// <summary>
        /// 获取入库失败切片的存放路径，IP为数据库中标为ISCENTER的站点
        /// </summary>
        /// <returns></returns>
        private string GetFailedTilePath()
        {
            string CenterIP = TServerSiteManager.GetCenterSiteIP();
            string pattern = @"^(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

            if (System.Text.RegularExpressions.Regex.IsMatch(CenterIP, pattern))
            {
                return string.Format(@"\\{0}\{1}\{2}\", CenterIP, StorageBasePath.QRST_DB_Tile, StorageBasePath.FailedTile);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 更新切片索引
        /// </summary>
        /// <param name="tilefileNamesPerIP"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private void UpdateTilesIndex(Dictionary<string, List<string>> tilefileNamesPerIP, TileIndexUpdateType type)
        {
            List<Task> updateIndexTasks = new List<Task>();
            try
            {

                foreach (KeyValuePair<string, List<string>> kvp in tilefileNamesPerIP)
                {
                    //TServerSite site = TServerSiteManager.getSiteFromSiteIP(kvp.Key);
                    //site.TCPService.UpdateTileIndex(type, kvp.Value);
                    List<string> listName = new List<string>();
                    listName.Add(kvp.Key);
                    listName.AddRange(kvp.Value);
                    Task updateIndexTask = new Task(o => UpdateTilesIndexTask((List<string>)o), listName);
                    updateIndexTask.Start();
                    updateIndexTasks.Add(updateIndexTask);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //return false;
            }
            finally
            {
                if (updateIndexTasks.Count > 0)
                    Task.WaitAll(updateIndexTasks.ToArray());
            }


            //return true;
        }
        private void UpdateTilesIndexTask(List<string> namelist)
        {
            try
            {
                TServerSite site = TServerSiteManager.getSiteFromSiteIP(namelist.ElementAt(0));
                namelist.RemoveAt(0);
                site.TCPService.UpdateTileIndex(TileIndexUpdateType.InsertUpdate, namelist);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 更新失败切片索引
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type"></param>
        private static void UpdateFailedTilesIndex(List<string> files, TileIndexUpdateType type)
        {
            string serverIP = TServerSiteManager.GetCenterSiteIP();
            TServerSite site = TServerSiteManager.getSiteFromSiteIP(serverIP);
            site.TCPService.UpdateFailedTilesIndex(type, files);
        }

    }
    //存储每个切片的目标IP地址和目标目录
    public class tileDesPath
    {

        public string Ip
        {
            get;
            set;
        }
        public string Despath
        {
            get;
            set;
        }

    }
}
