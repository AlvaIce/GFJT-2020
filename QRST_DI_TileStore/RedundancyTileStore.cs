// GIS Key Laboratory Zhejiang University
//文件名称：RedundancyTileStore
//摘　　要：实现切片数据的多节点冗余存储
//相关文档：
//当前版本：1.0
//作　　者：赵贤威
//创建日期：2013-03
//完成日期：
//说    明：该文档主要包括切片数据的导入、切片数据同步、索引的建立
//历    史：
 
using System;
using System.Collections.Generic;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.Data;
using QRST_DI_TS_Process.Site;
using System.IO;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;

namespace QRST_DI_TileStore
{
    /// <summary>
    /// 实现切片的多站点冗余存储
    /// </summary>
    public class RedundancyTileStore
    {
        public DirectlyAddressing da;  //直接寻址对象

        public RedundancyTileStore(DataSet dsIPMod)
        {
            da = new DirectlyAddressing(dsIPMod);
            TServerSiteManager.UpdateStorageSiteList();
            TServerSiteManager.UpdateOptimalStorageSiteList();
        }

        /// <summary>
        /// 根据Mod获取活动站点，即在多个具有相同mod的站点中，选择一个没有挂掉的站点进行入库   zxw 2013/2/28
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public TServerSite GetOptimalStroageSite(string mod)
        {
            List<string> ipLst = da.GetIPArrByMod(mod);
            for (int i = 0; i < ipLst.Count;i++ )
            {
                TServerSite ts = TServerSiteManager.getSiteFromSiteIP(ipLst[i]);
                if(TServerSiteManager.optimalStorageSites.Contains(ts))
                {
                    return ts;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据mod查找到包含该mod的活动站点，如果不存在，返回“-1”
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public string GetOptimalStorageSiteIP(string mod)
        {
            List<string> ipLst = da.GetIPArrByMod(mod);
            //刷新没有挂掉的站点
            TServerSiteManager.UpdateOptimalStorageSiteList();
            for (int i = 0; i < ipLst.Count; i++)
            {
                TServerSite ts = TServerSiteManager.getSiteFromSiteIP(ipLst[i]);
                if (TServerSiteManager.optimalStorageSites.Contains(ts))
                {
                    return ipLst[i];
                }
            }

            return "-1";
        }

        /// <summary>
        /// 根据文件名，获取文件存储路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="storeip"></param>
        /// <param name="ipmod"></param>
        /// <returns></returns>
        public string GetStoragePathByFileName(string fileName, out string storeip, out string ipmod)
        {
            string filePath = "-1";
            storeip = "-1";
            ipmod = "-1";
            TileNameArgs tnargs = da.GetTileNameArgs(fileName);
            if (!tnargs.Created)
            {
                return "-1";
            }
            try
            {
                ipmod =da. GetStorageIPMod(int.Parse(tnargs.Row), int.Parse(tnargs.Col)).ToString();
                storeip = GetOptimalStorageSiteIP(ipmod);
                if (storeip == "-1")
                {
                    throw new Exception("没有找到活动的站点!");
                }
                filePath = tnargs.GetFilePath(storeip, ipmod);
            }
            catch
            {

                return "-1";
            }

            return filePath;
        }

        /// <summary>
        /// 将站点上的切片同步到其他站点上
        /// </summary>
        /// <param name="fileNames">需要同步的文件数组</param>
        /// <param name="ip">源站点IP(即将该站点上的数据同步到其它站点)</param>
        public void SynchronousData(string[] fileNames,string ip)
        {
            List<string> modLst = TServerSiteManager.GetAllModByIP(ip);
            Dictionary<string,List<string>> modToIP = new Dictionary<string,List<string>>();  //记录VDS与IP的对应关系
            Dictionary<string, List<string>> ipToFileName = new Dictionary<string, List<string>>();//记录每一个站点所需要的同步文件 
            if (modLst != null)
            {
                for (int i = 0; i < modLst.Count; i++)
                {
                    List<string> temIPLst = da.GetIPArrByMod(modLst[i]);
                    modToIP.Add(modLst[i], temIPLst);
                    for (int j = 0; j < temIPLst.Count; j++)
                    {
                        if (!ipToFileName.ContainsKey(temIPLst[j]) && temIPLst[j] != ip)
                        {
                            ipToFileName.Add(temIPLst[j], new List<string>());
                        }
                    }
                }
            }
            else
            {
                throw new Exception(string.Format("{0}中没有分配虚拟磁盘空间！",ip));
            }

            for (int k = 0; k < fileNames.Length;k++ )
            {
                string ipMod = "-1";
                string relateDestPath = da.GetRelatePathByFileName(fileNames[k],out ipMod);
                string srcPath = string.Format(@"\\{0}\{1}", ip,relateDestPath);

                List<string> successIps = new List<string>();  //用以记录将当前文件同步成功的站点
                List<string> failedIps = new List<string>();      //记录将当前文件同步失败的站点
                successIps.Add(ip);
                 if(modToIP.ContainsKey(ipMod))
                 {
                     for (int l = 0; l < modToIP[ipMod].Count;l++ )
                     {
                         //检测要同步的站点是否活动,如果在活动，则将文件拷贝过去
                         if (TServerSiteManager.optimalStorageSites.Contains(TServerSiteManager.getSiteFromSiteIP(modToIP[ipMod][l])) && modToIP[ipMod][l] != ip)
                         {
                             string destPath = string.Format(@"\\{0}\{1}", modToIP[ipMod][l], relateDestPath);
                             if (!FileCopyTo(srcPath, destPath))
                             {
                                 throw new UnauthorizedAccessException();
                             }
                             ipToFileName[modToIP[ipMod][l]].Add(fileNames[k]);
                             successIps.Add(modToIP[ipMod][l]);
                         }
                         else if (modToIP[ipMod][l] != ip)  //将没有进行同步的站点记录下来，以便于其他站点进行恢复
                         {
                             failedIps.Add(modToIP[ipMod][l]);
                         }
                     }
                 }

                //将同步消息写入数据库，便于进行数据恢复
                 if (failedIps.Count > 0)
                 {
                     string senderIp = "";
                     foreach (string temp in successIps)
                     {
                         senderIp = String.Format("{0}{1};", senderIp, temp);
                     }
                     foreach(string val in failedIps)
                     {
                         tilelog log = new tilelog();
                         log.FileName = fileNames[k];
                         log.ReceiverIP = val;
                         log.SenderIP = senderIp;
                         log.OperationType = EnumTileOperationType.Add.ToString();
                         tilelog.Add(log);
                     }
                 }
            }

            //建立索引
            UpdateTilesIndex(ipToFileName, TileIndexUpdateType.InsertUpdate);
        }

        /// <summary>
        /// 将数据从一个地方复制到另一个地方
        /// </summary>
        /// <param name="scrFilePath"></param>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        public static bool FileCopyTo(string scrFilePath, string destFilePath)
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
                    //File.Delete(destFilePath);
                }
                fi.CopyTo(destFilePath,true);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将文件从源路径移动到目的路径，删掉原有路径
        /// </summary>
        /// <param name="scrFilePath">源路径</param>
        /// <param name="destFilePath">目的路径</param>
        /// <returns></returns>
        public static bool FileMoveTo(string scrFilePath, string destFilePath)
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
                fi.MoveTo(destFilePath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 更新索引，通过调用远程对象进行站点db索引的更新
        /// </summary>
        /// <param name="tilefileNamesPerIP"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 将失败的切片导入到控制台ConsoleServerIP后，更新入库失败的切片索引
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type"></param>
        private static void UpdateFailedTilesIndex(List<string> files, TileIndexUpdateType type)
        {
            string serverIP = Constant.ConsoleServerIP;
            TServerSite site = TServerSiteManager.getSiteFromSiteIP(serverIP);
            //   site.TCPService.UpdateFailedTilesIndex(type, files);
        }
        /// <summary>
        /// 切片导入
        /// </summary>
        /// <param name="tilesPaths">切片导入的路径数组，可以设置多个切片导入路径</param>
        /// <param name="filenames">返回路径下获取的要入库的切片</param>
        /// <param name="ips">对应切片的入库IP</param>
        /// <param name="failedTiles">入库失败的切片列表</param>
        /// <returns></returns>
        public bool StoreTiles(string[] tilesPaths, out string[] filenames, out string[] ips, out string[] failedTiles)
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

                //  切片入库
                foreach (string tilesPath in tilesPaths)
                {
                    if (!Directory.Exists(tilesPath))
                    {
                        continue;
                    }

                    DirectoryInfo di_data = new DirectoryInfo(tilesPath);
                    Dictionary<string, string> tilefileNames = new Dictionary<string, string>();
                    foreach (FileInfo fi in di_data.GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        if (fi.Name.ToUpper().EndsWith(".TIF") || fi.Name.ToUpper().EndsWith(".JPG"))
                        {

                            try
                            {
                                string[] ipArr = null;
                                string destPath = da.GetPathByFileName(fi.Name, out ipArr);
                                // 
                                //将IP改为IP数组 zxw 2013/2/28
                                //找到一个可以存储该文件的活动站点
                                string ip = "-1";
                                if (ipArr != null)
                                {
                                    for (int i = 0; i < ipArr.Length; i++)
                                    {
                                        if (TServerSiteManager.optimalStorageSites.Contains(TServerSiteManager.getSiteFromSiteIP(ipArr[i])))
                                        {
                                            ip = ipArr[i];
                                            break;
                                        }
                                    }
                                }


                                if (ip != "-1")
                                {
                                    tilefileNames.Add(fi.Name, ip);
                                    destPath = string.Format(@"\\{0}\{1}", ip, destPath);
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
                            catch (Exception ex)
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
            }
            catch (Exception)
            {
                rtn = false;
            }
            failedTiles = new string[failedFilenames.Count];
            failedFilenames.CopyTo(failedTiles);
            return rtn;
        }

        /// <summary>
        /// 处理切片入库
        /// 主要包括以下流程：
        /// 1.切片数据的导入
        /// 2.建立切片索引
        /// 3.同步切片数据到其它站点
        /// </summary>
        /// <param name="tilesPaths">切片导入的路径数组，可以设置多个切片导入路径</param>
        /// <param name="filenames">返回路径下获取的要入库的切片</param>
        /// <param name="ips">对应切片的入库IP</param>
        /// <param name="failedTiles">入库失败的切片列表</param>
        /// <returns></returns>
        public  bool ProcessTileStore(string[] tilesPaths, out string[] filenames, out string[] ips, out string[] failedTiles)
        {
            //切片数据的导入，删除原文件夹
            bool rtn = StoreTiles(tilesPaths, out filenames, out ips, out failedTiles);
            for (int i = 0; i < tilesPaths.Length;i++ )
            {
                if (Directory.Exists(tilesPaths[i]))
                {
                    Directory.Delete(tilesPaths[i], true);
                }
            }
            //建立切片索引
            List<string> listFNs = new List<string>();
            listFNs.AddRange(filenames);
            List<string> listFTNs = new List<string>();
            listFTNs.AddRange(failedTiles);

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

            //将数据同步到其他站点
            foreach (KeyValuePair<string, List<string>> kvp in tilefileNamesPerIP)
            {
                SynchronousData(kvp.Value.ToArray(), kvp.Key);
            }
            return rtn;
        }



    }
}
