using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Service;
using QRST_DI_Resources;
using QRST_DI_TS_Basis.Search;
using DotSpatial;
using DotSpatial.Topology;
/// <summary>
///SqliteWsImportUtils 的摘要说明
/// </summary>
namespace TilesImport
{
    public class SqliteWsImportUtils
    {
        public List<DataSet> allDtCol;
        public int allrecordNum = 0;
        MySqlBaseUtilities DbOperator;
        private Object obj = new object();
        public static Dictionary<string, TServerSiteTCPService> _dicTSSTCPServ;
        public static TServerSiteTCPService GetTServerSiteTCPService(string ip)
        {
            if (_dicTSSTCPServ == null)
            {
                _dicTSSTCPServ = new Dictionary<string, TServerSiteTCPService>();
            }
            if (_dicTSSTCPServ.ContainsKey(ip) && _dicTSSTCPServ[ip] != null && _dicTSSTCPServ[ip].IsRunning)
            {
                return _dicTSSTCPServ[ip];
            }
            else
            {
                return TServerSiteTCPService.InitTCPClient_StorageChl(ip, Constant.TcpSSPort);
            }

        }
        public SqliteWsImportUtils()
        {
            //
            //TODO: 在此处添加构造函数逻辑

            //
            DbOperator = new MySqlBaseUtilities();
            allDtCol = new List<DataSet>();
        }
        //从MySQL中获取IP
        public string[] GetIPFromMySql()
        {
            DataSet IPDateSet = DirectlyAddressingIPMod.GetTileDsMod();
            int tbl = IPDateSet.Tables[0].Rows.Count;
            string[] s = new string[tbl];
            for (int i = 0; i < tbl; i++)
                s[i] = IPDateSet.Tables[0].Rows[i][0].ToString();
            return s;
        }
        private DirectlyAddressing daUtil = null;
        public DirectlyAddressing _DAUtil
        {
            get
            {
                if (daUtil == null)
                {
                    daUtil = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                }
                return daUtil;
            }
        }

        /// <summary>
        /// 查找目标配号的索引文件路径
        /// </summary>
        /// <param name="modId">8</param>
        /// <returns></returns>
        public string getSqlitedbfilepathByRowCol(string row, string col)
        {
            int mod = _DAUtil.GetStorageIPMod(Convert.ToInt16(row), Convert.ToInt16(col));
            return getSqlitedbfilepathByMod(mod.ToString());
        }
        /// <summary>
        /// 查找目标配号的索引文件路径
        /// </summary>
        /// <param name="modId">8</param>
        /// <returns></returns>
        public string getSqlitedbfilepathByMod(string modId)
        {
            string Sqlitedbfilepath = "-1";

            string ip = _DAUtil.GetIPbyMod(modId);
            if (ip == "-1")
            {
                return "-1";
            }

            string commonsharepath = GetCommonSharePathBaseIP(ip);
            Sqlitedbfilepath = string.Format(@"{0}\QRST_DB_Tile\{1}\QDB_IDX_{1}.db", commonsharepath, modId);
            return Sqlitedbfilepath;
        }

        public string GetCommonSharePathBaseIP(string ip)
        {
            string sql = string.Format("select CommonSharePath from midb.tileserversitesinfo where addressip = '{0}'", ip);
            DataSet ds = DbOperator.GetDataSet(sql);
            string tilepath = ds.Tables[0].Rows[0][0].ToString();
            return tilepath;
        }

        /// <summary>
        /// 方法暂时弃用 查询privew记录，删除不存在.png;.pgw;-1.tif;-2.tif;-3.tif;-4.tif文件的privew记录
        /// </summary>
        /// <param name="sqlIpTilepath">长度为3，存放sql语句、IP地址、切片路径</param>
        public void DeletePngRecordWithGFFfileMissing(List<string> ipTilepath)
        {
            int recordNum = 0;
            DataSet DtCol = new DataSet();
            try
            {
                TServerSiteTCPService sm = SqliteWsImportUtils.GetTServerSiteTCPService(ipTilepath[0]);//ipandsql[1]为IP

                //方法暂时弃用 DtCol = sm.DeletePngRecordWithGFFfileMissing(ipTilepath[1]);//ipandsql[1]为sql

            }
            catch
            {
                recordNum = 0;
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.Add(DtCol);
                }
            }

        }

        /// <summary>
        /// 将jpg瓦片快视图变更为png格式，包括jgw变为pgw
        /// </summary>
        public void UpdateTileName2NewStyle(string ip)
        {
            try
            {
                TServerSiteTCPService sm = SqliteWsImportUtils.GetTServerSiteTCPService(ip);//ipandsql[1]为IP
                sm.UpdateTileName2NewStyle();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 将jpg瓦片快视图变更为png格式，包括jgw变为pgw
        /// </summary>
        public void ChangeTileJpg2Png(string ip)
        {
            try
            {
                TServerSiteTCPService sm = SqliteWsImportUtils.GetTServerSiteTCPService(ip);//ipandsql[1]为IP
                sm.ChangeTileJpg2Png();
            }
            catch
            {
            }
        }


        /// <summary>
        /// 查询并删除有记录但文件缺失的瓦片记录，遍历每个配号下的DataSet，不分页返回全部结果。
        /// </summary>
        /// <param name="sqlIpTilepath">长度为3，存放sql语句、IP地址、切片路径</param>
        public void DeleteTileFileMissingRecord(List<string> ipTilepath)
        {
            int recordNum = 0;
            DataSet DtCol = new DataSet();
            try
            {
                TServerSiteTCPService sm = SqliteWsImportUtils.GetTServerSiteTCPService(ipTilepath[0]);//ipandsql[1]为IP

                DtCol = sm.DeleteTileFileMissingRecord(ipTilepath[1]);//ipandsql[1]为sql

            }
            catch
            {
                recordNum = 0;
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.Add(DtCol);
                }
            }

        }

        /// <summary>
        /// 查询缺少满幅度云量的瓦片（需要1.2.3.4波段）,计算更新瓦片的满幅度云量信息，重命名，更新记录。
        /// 建议调用本方法前执行下DeleteTileFileMissingRecord方法，对缺失数据的记录进行清理
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commonSharePath"></param>
        /// <returns></returns>
        public void UpdateAvailabilityCloudInfoOfTiles(List<string> ipTilepath)
        {
            int recordNum = 0;
            DataSet DtCol = new DataSet();
            try
            {
                TServerSiteTCPService sm = SqliteWsImportUtils.GetTServerSiteTCPService(ipTilepath[0]);//ipandsql[0]为IP

                DtCol = sm.UpdateAvailabilityCloudInfoOfTiles(ipTilepath[1]);//ipandsql[1]为commonSharePath

            }
            catch
            {
                recordNum = 0;
            }
            if (DtCol != null)
            {
                lock (obj)
                {
                    allDtCol.Add(DtCol);
                }
            }

        }



        /// <summary>
        /// 将List融合成一个DataSet
        /// </summary>
        /// <param name="allDtCol">所有检索返回的List</param>
        /// <returns></returns>
        public DataSet MergeAllDataSet(List<DataSet> allDtCol)
        {
            DataSet DS = new DataSet();
            if (allDtCol.Count != 0)
            {
                foreach (DataSet ds in allDtCol)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (DS.Tables.Count == 0)
                        {
                            DS.Tables.Add(ds.Tables[0].Copy());
                        }
                        else
                        {
                            //DataRow[] drs=new DataRow[ds.Tables[0].Rows.Count];
                            //ds.Tables[0].Rows.CopyTo(drs, 0);
                            //DS.Tables[0].LoadDataRow(drs,  LoadOption.OverwriteChanges);

                            DS.Merge(ds, false, MissingSchemaAction.Add);
                        }
                    }

                }


            }
            return DS;

        }


    }
}