using System;
using System.Collections.Generic;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.IO;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public class ITTileImport
    {
        public void tileImport(string strPath)
        {
            //this.ParentOrder.Logs.Add(string.Format("开始切片入库。"));
            //string orderWorkspace = this.ProcessArgu[0];

            string[] filenames;
            string[] ips;
            string[] failedTiles;

            ProcessStoreTiles(strPath, out filenames, out ips, out failedTiles);
            List<string> listFNs = new List<string>();
            listFNs.AddRange(filenames);
            List<string> listFTNs = new List<string>();
            listFTNs.AddRange(failedTiles);
            //失败消息
            for (int i = 0; i < failedTiles.Length; i++)
            {
                int j = listFNs.IndexOf(failedTiles[i]);
                if (j != -1)
                {
                    //this.ParentOrder.Logs.Add(string.Format("!!!切片{0}入库失败,所属站点{1}。", failedTiles[i], ips[j]));
                }
                else
                {
                    //this.ParentOrder.Logs.Add(string.Format("!!!文件{0}入库失败。", failedTiles[i]));
                }
            }
            //.ParentOrder.Logs.Add(string.Format("正在建立切片索引。"));
            BuildTilesIndex(ips, filenames, listFTNs);
            //this.ParentOrder.Logs.Add(string.Format("完成切片索引更新。"));
            //this.ParentOrder.Logs.Add(string.Format("完成切片入库。"));
        }

        private bool ProcessStoreTiles(string orderWorkspace, out string[] filenames, out string[] ips, out string[] failedTiles)
        {
            //string[] tilePaths = new string[2];
            //tilePaths[0] = StorageBasePath.SharePath_TiledData(orderWorkspace);
            //tilePaths[1] = StorageBasePath.SharePath_Products(orderWorkspace);

            bool rtn = StoreTiles(orderWorkspace, out filenames, out ips, out failedTiles);

            //  切片文件夹删除  

            if (Directory.Exists(StorageBasePath.SharePath_TiledData(orderWorkspace)))
            {
                Directory.Delete(StorageBasePath.SharePath_TiledData(orderWorkspace), true);
            }
            //Directory.Delete(storagePath.SharePath_ClassfiedData, true);
            if (Directory.Exists(StorageBasePath.SharePath_Products(orderWorkspace)))
            {
                Directory.Delete(StorageBasePath.SharePath_Products(orderWorkspace), true);
            }

            return rtn;
        }


        private bool StoreTiles(string tilesPaths, out string[] filenames, out string[] ips, out string[] failedTiles)
        {
            bool rtn = true;
            List<string> failedFilenames = new List<string>();
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

                TServerSiteManager.UpdateOptimalStorageSiteList();
                //  切片入库
                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);

                if (!File.Exists(tilesPaths))
                {
                    return false;
                }

                //DirectoryInfo di_data = new DirectoryInfo(tilesPaths);
                Dictionary<string, string> tilefileNames = new Dictionary<string, string>();
                //foreach (FileInfo fi in di_data.GetFiles("*.*", SearchOption.AllDirectories))
                //{
                FileInfo fi = new FileInfo(tilesPaths);
                if (fi.Name.ToUpper().EndsWith(".TIF") || fi.Name.ToUpper().EndsWith(".JPG") || fi.Name.ToUpper().EndsWith(".JGW"))
                {

                    try
                    {
                        string ip = "-1";
                        string destPath = da.GetPathByFileName(fi.Name, out ip);
                        tilefileNames.Add(fi.Name, ip);
                        if (ip != "-1")
                        {
                            Directory.CreateDirectory(destPath.Substring(0, destPath.LastIndexOf("\\") + 1));
                            if (!FileMoveTo(fi.FullName, destPath))
                            {
                                throw new System.UnauthorizedAccessException();
                            }
                        }
                        else
                        {
                            throw new System.UnauthorizedAccessException();
                        }
                        //File.Copy(fi.FullName, destPath, true);
                    }
                    catch (Exception)
                    {
                        //失败的瓦片暂时存放在服务器上的FailedTileTempPath里
                        string failedpath = string.Format(@"{0}{1}", StorageBasePath.FailedTileTempPath, fi.Name);
                        if (!Directory.Exists(Path.GetDirectoryName(failedpath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(failedpath));
                        }
                        if (!fi.FullName.Equals(failedpath) && File.Exists(failedpath))
                        {
                            File.Delete(failedpath);
                        }
                        fi.MoveTo(failedpath);
                        failedFilenames.Add(fi.Name);
                    }
                }

                #region   lxl20120906 Update wyn20120908 修改切片不能入库问题
                string[] tempfilenames = new string[tilefileNames.Keys.Count];
                tilefileNames.Keys.CopyTo(tempfilenames, 0);
                string[] tempallfilenames = new string[tilefileNames.Keys.Count + filenames.Length];
                tempfilenames.CopyTo(tempallfilenames, 0);
                filenames.CopyTo(tempallfilenames, tempfilenames.Length);
                filenames = tempallfilenames;
                string[] tempips = new string[tilefileNames.Values.Count];
                tilefileNames.Values.CopyTo(tempips, 0);
                string[] tempallipss = new string[tilefileNames.Values.Count + ips.Length];
                tempips.CopyTo(tempallipss, 0);
                ips.CopyTo(tempallipss, tempips.Length);
                ips = tempallipss;
                //ips = new string[tilefileNames.Values.Count];
                //tilefileNames.Keys.CopyTo(filenames, 0);
                //tilefileNames.Values.CopyTo(ips, 0);

                #endregion

                //0906前语句
                //filenames = new string[tilefileNames.Keys.Count];
                //ips = new string[tilefileNames.Values.Count];
                //tilefileNames.Keys.CopyTo(filenames, 0);
                //tilefileNames.Values.CopyTo(ips, 0);

            }
            catch (Exception)
            {
                rtn = false;
            }
            failedTiles = new string[failedFilenames.Count];
            failedFilenames.CopyTo(failedTiles);
            return rtn;
        }

        public bool FileMoveTo(string scrFilePath, string destFilePath)
        {
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
                return false;
            }
            return true;
        }

        private static void BuildTilesIndex(string[] ips, string[] filenames, List<string> listFTNs)
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
            //更新成功的切片
            UpdateTilesIndex(tilefileNamesPerIP, TileIndexUpdateType.InsertUpdate);
            //更新失败的切片
            UpdateFailedTilesIndex(listFTNs, TileIndexUpdateType.InsertUpdate);
        }

        private static void UpdateFailedTilesIndex(List<string> files, TileIndexUpdateType type)
        {
            string serverIP = Constant.ConsoleServerIP;
            TServerSite site = TServerSiteManager.getSiteFromSiteIP(serverIP);
            //   site.TCPService.UpdateFailedTilesIndex(type, files);
        }

        private static bool UpdateTilesIndex(Dictionary<string, List<string>> tilefileNamesPerIP, TileIndexUpdateType type)
        {
            try
            {
                foreach (KeyValuePair<string, List<string>> kvp in tilefileNamesPerIP)
                {
                    TServerSite site = TServerSiteManager.getSiteFromSiteIP(kvp.Key);
                    site.TCPService.UpdateTileIndex(type, kvp.Value);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
