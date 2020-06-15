// GIS Key Laboratory Zhejiang University
//文件名称：RecoverTileStore
//摘　　要：站点切片数据的自动恢复
//相关文档：
//当前版本：1.0
//作　　者：赵贤威
//创建日期：2013-03
//完成日期：
//说    明：将该站点宕机时丢失的数据恢复
//历    史： 
using System;
using System.Collections.Generic;
using System.Data;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Basis.DBEngine;
using System.IO;
using QRST_DI_SS_Basis;

namespace QRST_DI_TileStore
{
    /// <summary>
    /// 站点切片恢复,该类的方法在需要进行切片数据恢复的站点执行
    /// </summary>
    public class RecoverTileStore
    {
        public static string IPAddress;
        private static DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        /// <summary>
        /// 根据tilelog中的切片存储记录将该站点中丢失的切片恢复过来
        /// 返回恢复失败的数据
        /// </summary>
        public static int RecoverAddedTile()
        {
            DataSet ds = tilelog.Query(string.Format("ReceiverIP = '{0}'  order by ID ", IPAddress));
            TServerSiteManager.UpdateOptimalStorageSiteList();
             TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
             int failedNum = ds.Tables[0].Rows.Count;
            for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
            {
                string fileName = ds.Tables[0].Rows[i]["FileName"].ToString();
                string ReceiverIP = ds.Tables[0].Rows[i]["ReceiverIP"].ToString();
                string [] SenderIPs = ds.Tables[0].Rows[i]["SenderIP"].ToString().Split(";".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
                string operationType = ds.Tables[0].Rows[i]["OperationType"].ToString();
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());

                if(operationType == EnumTileOperationType.Add.ToString())
                {
                    //找到一个活动的，并且有存在切片的站点
                    string sendIP = "";
                    for (int j = 0; j < SenderIPs.Length; j++)
                    {
                        if (TServerSiteManager.optimalStorageSites.Contains(TServerSiteManager.getSiteFromSiteIP(SenderIPs[j])))
                        {
                            sendIP = SenderIPs[j];
                            break;
                        }
                    }
                    if (sendIP == "")  //所有存储了改切片的站点都挂掉了
                    {
                        continue;
                    }
                    else  //将没挂掉的，且存储了该切片的站点作为Sender，把切片传给需要恢复的站点
                    {
                        string ipMod = "-1";
                        string relateDestPath = da.GetRelatePathByFileName(fileName, out ipMod);
                        string srcPath = string.Format(@"\\{0}\{1}",sendIP, relateDestPath);
                        string destPath = string.Format(@"\\{0}\{1}", ReceiverIP, relateDestPath);

                        if (!RedundancyTileStore.FileCopyTo(srcPath, destPath))
                        {
                            //throw new UnauthorizedAccessException();
                            continue;
                        }
                        else
                        {
                            //删除对应的日志文件
                            tilelog.Delete(string.Format(" ID = {0}",id));
                            //建索引
                            tiuu.TileIndexUpdate(TileIndexUpdateType.InsertUpdate,new List<string>(){fileName});
                            failedNum--;
                        }
                    }
                }
                else if (operationType == EnumTileOperationType.Delete.ToString()) //删除
                {
                    string ipMod = "-1";
                    string relateDestPath = da.GetRelatePathByFileName(fileName, out ipMod);
                    string destPath = string.Format(@"\\{0}\{1}", ReceiverIP, relateDestPath);
                    File.Delete(destPath);
                    tilelog.Delete(string.Format(" ID = {id}", id));
                    tiuu.TileIndexUpdate(TileIndexUpdateType.Delete, new List<string>() { fileName });
                    failedNum--;
                }
            }
            return failedNum;
        }
    }
}
