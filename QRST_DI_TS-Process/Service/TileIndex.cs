using System;
using System.Collections.Generic;
using System.Linq;
using QRST_DI_TS_Process.Site;
using System.Threading.Tasks;
using QRST_DI_SS_Basis;
 
namespace QRST_DI_TS_Process.Service
{
    public class TileIndex
    {
        /// <summary>
        /// 创建切片索引  
        /// </summary>
        /// <param name="ips"></param>
        /// <param name="filenames"></param>
        /// <param name="listFTNs"></param>
        public static void BuildTilesIndex(string[] ips, string[] filenames, List<string> listFTNs)
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

        /// <summary>
        /// 更新切片索引
        /// </summary>
        /// <param name="tilefileNamesPerIP"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static void UpdateTilesIndex(Dictionary<string, List<string>> tilefileNamesPerIP, TileIndexUpdateType type)
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
        private static void UpdateTilesIndexTask(List<string> namelist)
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
}
