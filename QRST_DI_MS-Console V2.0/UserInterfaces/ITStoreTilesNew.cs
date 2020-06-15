/*
 * 作者：zxw
 * 创建时间：2013-08-18
 * 描述：用于处理切片入库
*/
using System;
using System.Collections.Generic;
using System.Linq;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using QRST_DI_SS_Basis;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public class ITStoreTilesNew
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

        public void TileImport(string strPath)
        {
            string tilesPath = strPath;  //切片路径
            tasks.Clear();
            failedFilenames.Clear();
            ipAll.Clear();
            isQuit = false;
            startTime = DateTime.Now;
            TServerSiteManager.UpdateOptimalStorageSiteList();
            //eachTaskNum = new int[MAX_TASK_NUM];
            int count = TServerSiteManager.StorageSites.Count;

            foreach (var item in TServerSiteManager.optimalStorageSites)
            {
                ipAll.Add(item.IPAdress);
            }
            taskNum = ipAll.Count;
            eachTaskNum = new int[taskNum];
            Thread importThread = new Thread(TilesImport);
            importThread.Start(strPath);
        }

        private void TilesImport(object tilePath)
        {
            string tilesPath = (string)tilePath;
            string[] fileNames;
            string[] ips;
            string[] FailedTileName;
            string[] TilesPath = new string[] { tilesPath };
            bool success = StoreTiles(TilesPath, out fileNames, out ips, out FailedTileName);
            List<string> listFNs = new List<string>();
            listFNs.AddRange(fileNames);
            List<string> listFTNs = new List<string>();
            listFTNs.AddRange(FailedTileName);
            //失败消息
            for (int i = 0; i < FailedTileName.Length; i++)
            {
                int j = listFNs.IndexOf(FailedTileName[i]);

                if (j != -1)
                {
                    //log.Add(string.Format("!!!切片{0}入库失败,所属站点{1}。", FailedTileName[i], ips[j]));
                }
                else
                {
                    //log.Add(string.Format("!!!文件{0}入库失败。", FailedTileName[i]));
                }
            }
            BuildTilesIndex(ips, fileNames, listFTNs);
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
                    foreach (FileInfo fi in di_data.GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (fi.Name.ToUpper().EndsWith(".TIF") || fi.Name.ToUpper().EndsWith(".JPG"))
                        {
                            string ip = "-1";
                            string desPath = da.GetPathByFileName(fi.Name, out ip);
                            if (ip != "-1")
                            {
                                tileDesPath tile = new tileDesPath();
                                tile.Ip = ip;
                                tile.Despath = desPath;
                                tilefileNames.Add(fi.FullName, tile);
                                tilefileNames1.Add(fi.Name, ip);
                            }
                            else
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
                    //任务总数不足最大可执行任务总数时，任务全部启动；任务总数大于可执行任务总数时，一次启动可执行任务总数的任务，循环监视，每当一个任务结束时，再启动下一个任务。直到所有任务都已被启动。
                    if (taskNum <= MAX_TASK_NUM)
                    {
                        foreach (Task item in tasks)
                        {
                            item.Start();
                            //Console.WriteLine(DateTime.Now.ToString());
                        }
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
                    if (taskFail.Count > 0)
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
