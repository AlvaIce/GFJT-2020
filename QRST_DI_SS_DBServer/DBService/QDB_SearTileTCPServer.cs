using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Topology;
using QRST_DI_DS_DBEngine;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Basis.Search;
using QRST_DI_WebServiceUtil;
using QRST_DI_SS_Basis.TileSearch;
using QRST_DI_MS_Basis.Log;
using System.Windows.Forms;

namespace QRST_DI_SS_DBServer.DBService
{
    class QDB_SearTileTCPServer : MarshalByRefObject,IQDB_Searcher_Tile
    {
        private static TcpServerChannel _chan = null;
        private IDbBaseUtilities _baseUtilities=null;
        private IDbOperating _dbsqLiteOperating = null;
        private WebServiceUtil BaseUtil;
        private DataSet returnDs;
        private SQLBaseTool SQLBase;
        private SearchCondition searchcondition;
        private static List<QuerySearchInfo> listQueryResult;
        private int currentIndex = 0;
        private int pageIndex = 1;
        private int pageSize = 100;
        private int allrecordNum;
        private int MinQueryTimeSpan = 5;       //查询缓存间隔时间，单位分钟
        private string logStr = "";
        DateTime outtimeUpdateListTime;


        public QDB_SearTileTCPServer()
        {
            InitializeLifetimeService();
            switch (Constant.QrstDbEngine)
            {
                case EnumDbEngine.MYSQL:
                    _baseUtilities = new MySqlBaseUtilities();
                    _dbsqLiteOperating = new DBMySqlOperating();
                    break;
                case EnumDbEngine.SQLITE:
                    _baseUtilities = new SQLiteBaseUtilities();
                    _dbsqLiteOperating = new DBSQLiteOperating();
                    break;
                case EnumDbEngine.ClOUDDB:
                    break;
            }
            if (!Constant.ServiceIsConnected)
            {
                Constant.InitializeTcpConnection();
            }
            BaseUtil =new WebServiceUtil();
            SQLBase=new SQLBaseTool();
            searchcondition=new SearchCondition();
            QRST_DI_TS_Process.Site.TServerSiteManager.StartAutoUpdateOptimalStorageSiteList();
            outtimeUpdateListTime = DateTime.Now;

        }
        /// <summary>
        /// 开启TCP服务
        /// </summary>
        public static void StartTCPService(string tcpPort)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                //props["port"] = tcpPort;
                props["name"] = "SearTile_TCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                    props, serverProvider);
                ChannelServices.RegisterChannel(_chan);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(QDB_SearTileTCPServer),
                    "QDB_SearTile_TCP",
                    WellKnownObjectMode.SingleCall);
            }
            catch (Exception e)
            {
                throw new Exception("注册QDB_SearTileTCPServer异常", e);
            }
        }

        /// <summary>
        /// 重写远程对象生存周期。默认远程对象一段时间后删除，重写后永久保存。
        /// </summary>
        /// <returns></returns>
        //public override object InitializeLifetimeService()
        //{
        //    return null;
        //}
        #region 接口实现
        public List<string> GetCTileDistinctAttrs()
        {
            List<string> distincList = new List<string>();
            distincList.Add("【distinctSatellite】");
            distincList.AddRange(GetDataDistinct("select * from distinctSatellite").ToArray());
            distincList.Add("【distinctSensor】");
            distincList.AddRange(GetDataDistinct("select * from distinctSensor").ToArray());
            distincList.Add("【distinctCTtype】");
            distincList.AddRange(GetDataDistinct("select * from distinctCTtype").ToArray());
            distincList.Add("【distinctCTLevel】");
            distincList.AddRange(GetDataDistinct("select * from distinctCTLevel").ToArray());
            return distincList;
        }

        public List<string> GetPTileDistinctAttrs()
        {
            List<string> distincList = new List<string>();
            distincList.Add("【distinctPTLevel】");
            distincList.AddRange(GetDataDistinct("select * from distinctPTLevel").ToArray());
            distincList.Add("【distinctPTProdType】");
            distincList.AddRange(GetDataDistinct("select * from distinctPTProdType").ToArray());
            return distincList;
        }

        public List<string> UpdateDistinctTables()
        {
            List<Task> tasks = new List<Task>();
            WebServiceUtil BaseUtil = new WebServiceUtil();

            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                //针对站点奔溃影响检索效率问题的解决方案：          joki170520
                if (!QRST_DI_TS_Process.Site.TServerSiteManager.optimalStorageSiteIPs.Contains(IP))
                {
                    Console.WriteLine("站点" + IP + "无法访问");
                    continue;
                }

                List<string> ipandpath = new List<string>();
                string commonpath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandpath.Add(IP);

                ipandpath.Add(commonpath);

                Task t = new Task(o => BaseUtil.UpdateDistinctTables((List<string>)o), ipandpath);
                t.Start();
                tasks.Add(t);
            }


            WaitingForTasks(tasks, 60000);



            List<string> distincList = new List<string>();
            distincList.Add("【distinctSatellite】");
            distincList.AddRange(GetDataDistinct("select * from distinctSatellite").ToArray());
            distincList.Add("【distinctSensor】");
            distincList.AddRange(GetDataDistinct("select * from distinctSensor").ToArray());
            distincList.Add("【distinctCTtype】");
            distincList.AddRange(GetDataDistinct("select * from distinctCTtype").ToArray());
            distincList.Add("【distinctCTLevel】");
            distincList.AddRange(GetDataDistinct("select * from distinctCTLevel").ToArray());
            distincList.Add("【distinctPTLevel】");
            distincList.AddRange(GetDataDistinct("select * from distinctPTLevel").ToArray());
            distincList.Add("【distinctPTProdType】");
            distincList.AddRange(GetDataDistinct("select * from distinctPTProdType").ToArray());
            return distincList;
        }

        public DataSet SearProdTilePathBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, bool needTilePath = true)
        {
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            return SearTileBatchAllType("productTiles", rows, cols, lv, "", "", string.Format("and ProdType like '{0}'{1}", prodType, timecondition), needTilePath);
        }
        public DataSet SearProdTileBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate)
        {
            return SearProdTilePathBatch(prodType, rows, cols, lv, startdate, enddate, false);
        }

        public DataSet SearProdTileBatch_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate, string priority)
        {
            return SearProdTilePathBatch_coordsStr(prodType, coordsStr, tileLevel, startdate, enddate, priority, false);
        }

        public DataSet SearImgTileBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate)
        {
            return SearImgTilePathBatch(sat, sensor, row, col, tileLevel, startdate, enddate, false);
        }

        public DataSet SearImgTileBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string priority)
        {
            return SearImgTilePathBatch_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate, priority, false);
        }
        public DataSet SearTileBatch(List<string> row, List<string> col, string tileLevel)
        {
            return SearTileBatchAllType("correctedTiles", row, col, tileLevel, "", "", "and type='Preview'");
        }
        public DataSet SearTileYaanBatch(List<string> row, List<string> col, string tileLevel)
        {
            return SearImgTileBatch("", "", row, col, tileLevel, 0, 0);
        }
        public DataSet searTileByColAndRowBatch(List<List<string>> tileInfos)
        {
            //allRecordCount = 0;       
            List<string> tilePath = new List<string>();
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。

            #endregion
            String subSql = null;
            foreach (List<string> tileInfo in tileInfos)
            {
                subSql += String.Format("(Row={0} and Col={1} and Level={2} and type='Preview') or ", tileInfo[1], tileInfo[2], tileInfo[0]);
            }
            subSql = subSql.TrimEnd(("or ").ToCharArray());
            string sql = String.Format("select distinct * from correctedTiles where {0}", subSql);
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                DataTable table = returnDs.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string destPath = da.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                    if (destPath == "-1")
                        continue;
                    tilePath.Add(destPath);
                }
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:DataSet为空");
            //return tilePath;
            returnDs.AcceptChanges();
            return returnDs;
        }

        public DataSet TestSearTileSingleAllType(string tablename)
        {
            //allRecordCount = 0;       
            List<string> tilePath = new List<string>();
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            string sql = String.Format("select * from {0} ", tablename);
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);

            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                DataTable table = returnDs.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string destPath = da.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                    if (destPath == "-1")
                        continue;
                    tilePath.Add(destPath);
                }
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:DataSet为空");
            //return tilePath;
            returnDs.AcceptChanges();
            return returnDs;
        }

        public DataSet SearProdTileSingle(string prodType, string row, string col, string lv, int startdate, int enddate)
        {
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            return SearTileSingleAllType("productTiles", row, col, lv, string.Format("and ProdType like '{0}'{1}", prodType, timecondition));
        }

        public DataSet SearProdTilePathSingle(string prodType, string row, string col, string lv, int startdate, int enddate)
        {
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            return SearTileSingleAllType("productTiles", row, col, lv, string.Format("and ProdType like '{0}'{1}", prodType, timecondition, true));
        }

        public DataSet SearImgTileSingle(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate)
        {
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";

            string sqlSatSensor = "";
            if (sat != "")
            {
                sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
            }
            if (sensor != "")
            {
                sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));
            }
            return SearTileSingleAllType("correctedTiles", row, col, tileLevel, "and type='Preview'" + sqlSatSensor + timecondition);
        }

        public DataSet SearImgTilePathSingle(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate)
        {
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";

            string sqlSatSensor = "";
            if (sat != "")
            {
                sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
            }
            if (sensor != "")
            {
                sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));
            }
            return SearTileSingleAllType("correctedTiles", row, col, tileLevel, "and type='Preview'" + sqlSatSensor + timecondition, true);
        }

        public DataSet SearTileYaan(string row, string col, string tileLevel)
        {
            return SearTileSingleAllType("correctedTiles", row, col, tileLevel, "and type='Preview'");
        }
        public List<string> TilePathsList(List<string> tileNames)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：TilePathsList 输入:tileNames=" + InforLog<string>.returnListStrElem(tileNames));
            List<string> tilePathList = new List<string>();
            DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            foreach (string item in tileNames)
            {
                string destPath = da.GetPathByFileName(item);
                if (destPath == "-1")
                    continue;
                tilePathList.Add(destPath);
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：TilePathsList 返回:tilePathList=" + InforLog<string>.returnListStrElem(tilePathList));
            return tilePathList;
        }

        public DataSet SearFliterTilePaged1(List<double[]> coords, string type, List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
        {
            #region 构建多边形
            List<Coordinate> coordinates = new List<Coordinate>();
            foreach (double[] coord in coords)
            {
                coordinates.Add(new Coordinate(coord));
            }
            if (coordinates.Count > 0 && (coordinates[0].X != coordinates[coordinates.Count - 1].X || coordinates[0].Y != coordinates[coordinates.Count - 1].Y))
            {
                //判断多边形是否闭合，即首尾相等
                coordinates.Add(coordinates[0]);
            }

            //如果有多边形，没有四角坐标，自动从多边形中获取
            if (coordinates.Count > 4 && (position == null || position.Count < 4))
            {
                double minLat = 90;
                double minLon = 180;
                double maxLat = -90;
                double maxLon = -180;
                foreach (Coordinate coord in coordinates)
                {
                    minLon = (coord.X < minLon) ? coord.X : minLon;
                    maxLon = (coord.X > maxLon) ? coord.X : maxLon;
                    minLat = (coord.Y < minLat) ? coord.Y : minLat;
                    maxLat = (coord.Y > maxLat) ? coord.Y : maxLat;
                }
                position = new List<string>();
                position.Add(minLat.ToString());
                position.Add(minLon.ToString());
                position.Add(maxLat.ToString());
                position.Add(maxLon.ToString());
            }
            #endregion

            allRecordCount = 0;
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }

            #region 构建Sql语句
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();

            #region 经纬度范围转成层级行列号范围
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i].Trim(), position);

                listTileLevel.Add(levelposition);
            }
            #endregion

            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }
            if (OtherQuery != null || OtherQuery != "")
            {
                sql += OtherQuery;
            }
            #endregion

            #region 在检索分页信息池中查找短时（5分钟）内相同检索条件的检索分页信息,含各配号符合条件的记录数，用以进行计算分页记录分配
            QuerySearchInfo queryInfo = getQuerySearchInfo(sql, coordinates, null, type);
            #endregion

            #region 根据查询信息(含各配号结果记录数)，构建本次查询各配号的页面分配个数
            PagedSearchTool.GetPageInfo(startIndex, queryInfo.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            #endregion

            #region 根据配号所在的IP，执行分布式并行查询
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryInfo.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryInfo);
                siteQueryInfo.ObjectTime = queryInfo.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;
                Task t = new Task(o => BaseUtil.GetDataSetColPaged3((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);
            #endregion

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryInfo.QRST_ModsList);

            returnDs.AcceptChanges();

            return returnDs;
        }

        public DataSet SearFliterTilePaged2(string coordsStr, string type, List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
        {
            List<double[]> coords = new List<double[]>();

            string[] coordstr = coordsStr.Split(';');
            foreach (string cdstr in coordstr)
            {
                try
                {
                    string[] lonlatstr = cdstr.Split(',');
                    if (lonlatstr.Length == 2)
                    {
                        double lon = double.Parse(lonlatstr[0]);
                        double lat = double.Parse(lonlatstr[1]);
                        coords.Add(new double[] { lon, lat });
                    }
                }
                catch
                {
                }
            }

            return SearFliterTilePaged1(coords, type, position, datetime, satellite, sensor, datatype, tileLevel, OtherQuery, out allRecordCount, startIndex, offset);

        }

        public DataSet SearTilePaged1(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' OtherQuery='{6}' startIndex='{7}' offset='{8}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), OtherQuery, startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged1 输入:" + logStr);
            allRecordCount = 0;
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i].Trim(), position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }
            if (OtherQuery != null || OtherQuery != "")
            {
                sql += OtherQuery;
            }
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);

            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            //if (returnDs.Tables.Count != 0)
            //{
            //allRecordCount = returnDs.Tables[0].Rows.Count;
            //    DataRow dr = returnDs.Tables[0].NewRow();

            //    dr["TileFilePath"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    returnDs.Tables[0].Rows.InsertAt(dr, 0);
            //}
            //int recont = returnDs.Tables[0].Rows.Count;
            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged1 输出:returnDs 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged1 输出:returnDs 为空");

            returnDs.AcceptChanges();
            return returnDs;
        }

        public DataSet SearTilePaged(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' startIndex='{6}' offset='{7}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged 输入:" + logStr);
            allRecordCount = 0;
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i].Trim(), position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }

            QuerySearchInfo queryResult = getQuerySearchInfo(sql);

            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);


            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            //if (returnDs.Tables.Count != 0)
            //{
            //    DataRow dr = returnDs.Tables[0].NewRow();

            //    dr["TileFilePath"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    returnDs.Tables[0].Rows.InsertAt(dr, 0);
            //}
            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged 输出:returnDs 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged 输出:returnDs 为空");

            returnDs.AcceptChanges();
            return returnDs;
        }

        public DataSet SearTilePagedBaseColAndRow(List<string> ColAndRow, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
        {
            logStr = string.Format("ColAndRow='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' OtherQuery='{6}' startIndex='{7}' offset='{8}'", InforLog<string>.returnListStrElem(ColAndRow), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), OtherQuery, startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePagedBaseColAndRow 输入:" + logStr);
            allRecordCount = 0;
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }

            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();
            string sqlColAndRow = "";
            if (ColAndRow[0] != "" && ColAndRow[1] != "" && ColAndRow[2] != "" && ColAndRow[3] != "")
            {
                sqlColAndRow = string.Format("Row >={0} and Row <={1} and Col >={2} And Col <={3} and", ColAndRow[0], ColAndRow[2], ColAndRow[1], ColAndRow[3]);
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            if (sqlColAndRow != "")
                sql += sqlColAndRow;
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("and "));

            }
            else
            {
                sql = sql + sql2;

            }
            if (OtherQuery != null || OtherQuery != "")
            {
                sql += OtherQuery;
            }
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);

            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            //if (returnDs.Tables.Count != 0)
            //{
            //    DataRow dr = returnDs.Tables[0].NewRow();

            //    dr["TileFilePath"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    returnDs.Tables[0].Rows.InsertAt(dr, 0);
            //}

            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePagedBaseColAndRow 输出:returnDs 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePagedBaseColAndRow 输出:returnDs 为空");

            returnDs.AcceptChanges();
            return returnDs;
        }

        public int SearTilePagedBaseColAndRowTest()
        {
            List<string> position = new List<string>();
            position.Add("-180");
            position.Add("-90");
            position.Add("180");
            position.Add("90");
            List<int> datetime = new List<int>();
            List<string> satellite = new List<string>();
            List<string> sensor = new List<string>();
            List<string> datatype = new List<string>();
            List<string> tileLevel = new List<string>();
            int startIndex = 0;
            int allRecordCount = 0;
            int offset = 100;
            string OtherQuery = null;
            SearTilePagedBaseColAndRow(position, datetime, satellite, sensor, datatype, tileLevel, OtherQuery, out allRecordCount, startIndex, offset);
            return allRecordCount;
        }

        public DataSet SearTile2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' pageIndex='{6}' pageSize='{7}'", 
                InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), 
                InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), pageIndex, pageSize);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTile2 输入:" + logStr);
            int allRecordCount = -1;
            DataSet returnDS = SearTile(position, datetime, satellite, sensor, datatype, tileLevel, pageIndex, pageSize, out allRecordCount);
            if (returnDS != null && returnDS.Tables.Count != 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                try
                {
                    DataRow dr = returnDS.Tables[0].NewRow();

                    dr["TileFileName"] = allRecordCount;
                    returnDS.Tables[0].Rows.InsertAt(dr, 0);
                }
                catch
                {

                }
            }
            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTile2 输出:returnDs 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTile2 输出:returnDs 为空");

            returnDs.AcceptChanges();
            return returnDS;
        }

        public DataSet SearTile(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize, out int AllRecordCount)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' pageIndex='{6}' pageSize='{7}'",
                InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), pageIndex, pageSize);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTile 输入:" + logStr);
            WebServiceUtil.allDtCol.Clear();//清理List
            #region//经纬度转行列号
            int[] rowAndColum = new int[4];
            if (position.Count != 0)
            {
                rowAndColum = DirectlyAddressing.GetRowAndColum(position.ToArray(), tileLevel[0]);

                position.Clear();
                foreach (int item in rowAndColum)
                {
                    position.Add(item.ToString());
                }
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite(position, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sql);
                //string IPTEST = "192.168.0.15";
                //ipandsql.Add(IPTEST);
                ipandsql.Add(IP);
                ipandsql.Add(pageIndex.ToString());
                ipandsql.Add(tilepath);
                //DLF 20130416 添加pageSize参数，使分页更灵活。以下方法中都增加 BaseUtil.InputAndOutput，sm.GetDataSetCol，tileSearchUtil.GetDataSetCol。
                ipandsql.Add(pageSize.ToString());
                //InputAndOutput(ipandsql);
                Task t = new Task(o => BaseUtil.InputAndOutput((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();
            allrecordNum = BaseUtil.allrecordNum;

            //if (isDataSyn)
            //{
            //    SQLBase.AddDataFTPAddress(ds, QDB_Base.Enum.DataSynchType.TileData);
            //}

            //if (ds.Tables.Count != 0)
            //{
            //    DataRow dr = ds.Tables[0].NewRow();
            //    dr["ID"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    ds.Tables[0].Rows.InsertAt(dr, 0);
            //}
            //return returnDs;

            //程序更改：不把结果数目在表中返回。而是作为一个输出参数返回。DLF
            AllRecordCount = allrecordNum;
            DataSet pageds = new DataSet();
            pageds = TileSearchUtil.SplitDataSet(returnDs, pageSize, 1);
            //清除ID列（已经无意义），添加切片数据名称列。
            pageds = SQLBaseTool.AddTileDataNameInfo(pageds);
            if (pageds.Tables.Count != 0 && pageds.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTile 输出:pageds 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTile 输出:pageds 为空");

            pageds.AcceptChanges();
            return pageds;
        }

        public List<string> SearProTileLevels()
        {

            //返回值
            List<string> hasTiles = new List<string>();

            WebServiceUtil.allDtCol.Clear();//清理List

            //查询使用的sql语句
            string tilequerysql = string.Format("select distinct Level from productTiles");

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(tilequerysql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }


            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            if (returnDs != null && returnDs.Tables.Count > 0)
            {
                for (int i = 0; i < returnDs.Tables[0].Rows.Count; i++)
                {
                    string tileLevel = returnDs.Tables[0].Rows[i]["Level"].ToString();
                    if (!hasTiles.Contains(tileLevel))
                    {
                        hasTiles.Add(tileLevel);
                    }
                }
            }
            //WebServiceUtil.allDtCol.Clear();
            logStr = string.Format("hasTiles='{0}'", InforLog<string>.returnListStrElem(hasTiles));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearProTileLevels 输出:" + logStr);
            return hasTiles;
            //去除ds中重复的切片等级，并提取成List<string>
        }

        public DataSet SearProTileAllAttr()
        {
            //返回值
            List<string> hasTiles = new List<string>();

            WebServiceUtil.allDtCol.Clear();//清理List

            //查询使用的sql语句
            string tilequerysql = string.Format("select * from productTiles limit 0,1");

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(tilequerysql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();
            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearProTileAllAttr 输出:returnDs 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearProTileAllAttr 输出:returnDs 为空");

            //WebServiceUtil.allDtCol.Clear();
            returnDs.AcceptChanges();
            return returnDs;
            //去除ds中重复的切片等级，并提取成List<string>
        }

        public void ExecuteNonQuery(string sql)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：ExecuteNonQuery 输入:" + sql);
            WebServiceUtil.allDtCol.Clear();//清理List

            //查询使用的sql语句
            string tilequerysql = sql;

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(tilequerysql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.ExecuteNonQuery((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            //去除ds中重复的切片等级，并提取成List<string>
        }

        public DataSet SearTileAllAttr()
        {
            //返回值
            List<string> hasTiles = new List<string>();

            WebServiceUtil.allDtCol.Clear();//清理List

            //查询使用的sql语句
            string tilequerysql = string.Format("select * from correctedTiles limit 0,1");

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(tilequerysql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();
            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileAllAttr 输出:returnDs 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileAllAttr 输出:returnDs 为空");

            //WebServiceUtil.allDtCol.Clear();
            returnDs.AcceptChanges();
            return returnDs;
            //去除ds中重复的切片等级，并提取成List<string>
        }

        public List<string> SearTileLevels()
        {
            //返回值
            List<string> hasTiles = new List<string>();

            WebServiceUtil.allDtCol.Clear();//清理List

            //查询使用的sql语句
            string tilequerysql = string.Format("select distinct Level from correctedTiles");

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(tilequerysql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            if (returnDs != null && returnDs.Tables.Count > 0)
            {
                for (int i = 0; i < returnDs.Tables[0].Rows.Count; i++)
                {
                    string tileLevel = returnDs.Tables[0].Rows[i]["Level"].ToString();
                    if (!hasTiles.Contains(tileLevel))
                    {
                        hasTiles.Add(tileLevel);
                    }
                }
            }
            logStr = string.Format("hasTiles='{0}'", InforLog<string>.returnListStrElem(hasTiles));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileLevels 输出:" + logStr);
            //WebServiceUtil.allDtCol.Clear();
            return hasTiles;
            //去除ds中重复的切片等级，并提取成List<string>
        }

        public List<string> SearProdType()
        {
            return GetDataDistinct("select distinct ProdType from productTiles");
        }
        public List<string> SearTileSensors()
        {
            return GetDataDistinct("select distinct Sensor from correctedTiles");
        }
        public List<string> SearTileSatellites()
        {
            return GetDataDistinct("select distinct Satellite from correctedTiles");
        }
        public List<string> GetDataDistinct(string distinctSql)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：GetDataDistinct 输入:" + distinctSql);
            //返回值
            List<string> distincList = new List<string>();
            
            WebServiceUtil.allDtCol.Clear();//清理List

            //查询使用的sql语句
            string tilequerysql = distinctSql;

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(tilequerysql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            string columnName = "";
            if (returnDs != null && returnDs.Tables.Count > 0)
            {
                int s = returnDs.Tables[0].Columns.Count;
                if (s > 0)
                {
                    columnName = returnDs.Tables[0].Columns[0].ColumnName;
                }

                for (int i = 0; i < returnDs.Tables[0].Rows.Count; i++)
                {
                    string content = returnDs.Tables[0].Rows[i][columnName].ToString();
                    if (!distincList.Contains(content))
                    {
                        distincList.Add(content);
                    }
                }
            }
            logStr = string.Format("distincList='{0}'", InforLog<string>.returnListStrElem(distincList));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：GetDataDistinct 输出:" + logStr);
            //WebServiceUtil.allDtCol.Clear();
            return distincList;
            //去除ds中重复的切片等级，并提取成List<string>
        }

        /// <summary>
        /// 根据坐标点串获取行列号列表
        /// </summary>
        /// <param name="coordsStr"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        public void getLvRowColFromCoordsStr(string coordsStr, string tileLevel, out List<int> rows, out List<int> cols)
        {
            //初始化输出
            rows = new List<int>();
            cols = new List<int>();

        }


        public System.Data.DataSet SearTileBatchAllType_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition, string priority, bool needTilePath = false)
        {

            //--构建多边形

            //获取多边形最大最小经纬度
            double[] mmll = GridGeneration.GetMinMaxLatLonFormCoordsStr(coordsStr);


            //获取最大最小行列号
            int[] mmrc = DirectlyAddressing.GetRowAndColum(new string[] { mmll[0].ToString(), mmll[1].ToString(), mmll[2].ToString(), mmll[3].ToString() }, tileLevel);

            //构建SQL语句
            /*
            select *, count(distinct Date) from (select * from correctedTiles t1 where type='Preview' and (Row>=511 and Row=<598 and Col>=1184 and Col=<1822 and Level=8)) and (select count(*) from correctedTiles t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.type=t1.type and t2.Date>t1.Date) <1)  group by Date,type,level,row,col 
            */

            string sqlrowscols = string.Format("Row>={0} and Row<={1} and Col>={2} and Col<={3} and Level={4}", mmrc[0], mmrc[2], mmrc[1], mmrc[3], tileLevel);

            string sqlSatSensor = "";
            string typestr = "type";
            if (tablename == "correctedTiles")
            {
                if (sat != "")
                {
                    sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
                }
                if (sensor != "")
                {
                    sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));      //'_'是SQL通配符，代表任意单字符，比如WFV_={WFV1,WFV2,WFV3,WFV4}
                }
                typestr = "type";

                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }
            else if (tablename == "productTiles")
            {
                typestr = "ProdType";
            }


            //原按照最新日期的查询条件存在问题，存在相同日期不同数据源的多个数据，需要移除
            //string sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
            string sql;
            //所有的排序都是按照满幅率降序（取最大），时间降序（取最新），云量升序（取最小）的顺序进行筛选的
            //其中按照满幅率优先筛选的时候，要优先考虑非255的，即如果行列号为（582，1183）的所有瓦片数据，既有云量，满幅度（255，255）的也有非（255，255）的，则筛选的时候，会取非（255.255）的。
            switch (priority)
            {
                case "默认":
                    sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "时间-云量-满幅率":
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY  Date ASC , Cloud DESC, Availability ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "时间-满幅率-云量"://时间-满幅率-云量
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY  Date ASC ,Availability ASC , Cloud DESC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "云量-时间-满幅率"://云量-时间-满幅率
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY Cloud DESC, Date ASC , Availability ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "云量-满幅率-时间":
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY  Cloud DESC ,Availability ASC , Date ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "满幅率-时间-云量":
                    sql = string.Format("SELECT * FROM (SELECT  * FROM (SELECT *  FROM (SELECT *  FROM ( SELECT * FROM {0} t1 WHERE {1}{2} {3} AND cloud <> 255 AND availability <> 255) ORDER BY Availability ASC , date ASC , cloud DESC ) GROUP BY row ,col ,level ,{4}  UNION SELECT * FROM (SELECT  *  FROM ( SELECT  * FROM {0} t1 WHERE  {1}{2} {3} AND availability = 255 AND cloud = 255 ) ORDER BY availability ASC , date ASC , cloud DESC ) GROUP BY  row , col , level , {4} ) ORDER BY availability DESC , date DESC , cloud ASC ) GROUP BY row , col ,level ,{4} ", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "满幅率-云量-时间":
                    sql = string.Format("SELECT * FROM (SELECT  * FROM (SELECT  * FROM (SELECT * FROM (SELECT  * FROM {0} t1 WHERE {1}{2} {3} AND cloud <> 255 AND availability <> 255) ORDER BY Availability ASC ,cloud DESC , date ASC) GROUP BY row , col , level ,{4} UNION SELECT * FROM (SELECT  * FROM ( SELECT  * FROM {0} t1 WHERE {1}{2} {3} AND availability = 255 AND cloud = 255 ) ORDER BY Availability ASC ,cloud DESC , date ASC ) GROUP BY row , col , level , {4}) ORDER BY availability DESC ,cloud ASC , date DESC ) GROUP BY row , col, level,{4}", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;

                default:
                    sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3}  and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
            }

            //分布式查询，传入SQL和多边形


            //不分页检索
            WebServiceUtil BaseUtil = new WebServiceUtil();


            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {

                //针对站点奔溃影响检索效率问题的解决方案：          joki170520
                if (!QRST_DI_TS_Process.Site.TServerSiteManager.optimalStorageSiteIPs.Contains(IP))
                {
                    Console.WriteLine("站点" + IP + "无法访问");
                    continue;
                }

                List<string> sqlCoordsIpTilepath = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                sqlCoordsIpTilepath.Add(sql);

                sqlCoordsIpTilepath.Add(coordsStr);

                sqlCoordsIpTilepath.Add(IP);

                sqlCoordsIpTilepath.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll_CoordsFilter((List<string>)o), sqlCoordsIpTilepath);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks, 15000);

            System.Data.DataSet returnDs;
            returnDs = BaseUtil.MergeAllDataSet();

            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Columns.Contains("count(distinct Date)"))
            {
                returnDs.Tables[0].Columns.Remove("count(distinct Date)");
            }

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = 0;
            if (returnDs.Tables.Count != 0)
            {
                allrecordNum = returnDs.Tables[0].Rows.Count;
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);


                if (needTilePath)
                {
                    List<string> tilePath = new List<string>();
                    DataTable table = returnDs.Tables[0];
                    table.Columns.Add("TileFilePath", typeof(string));
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string destPath = BaseUtil._DAUtil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                        table.Rows[i]["TileFilePath"] = destPath;
                    }
                }
            }
            returnDs.AcceptChanges();
            return returnDs;
        }


        public DataSet SearImgTileCountBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate)
        {
            #region 原方法，大范围效率低，暂弃用。先计算出目标多边形范围内全部的行列号，通过枚举比较行列号进行检索
            /*List<string> rows;
        List<string> cols;
        GridGeneration gg = new GridGeneration(tileLevel);
        gg.GetAOITilesGrid(coordsStr, out rows, out cols);

        return SearImgTileCountBatch(sat, sensor, rows, cols, tileLevel, startdate, enddate);
         */
            #endregion

            #region 现方法，通过多边形的最大最小经纬度范围，计算出最大最小行列号，通过比较行列号大小进行检索
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";

            return SearTileCountBatchAllType_coordsStr("correctedTiles", coordsStr, tileLevel, sat, sensor, "and type='Preview'" + timecondition);
            #endregion

        }




        public DataSet SearImgTileCountBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate)
        {
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";

            return SearTileCountBatchAllType("correctedTiles", row, col, tileLevel, sat, sensor, "and type='Preview'" + timecondition);
        }


        public DataSet SearProdTileCountBatch_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate)
        {
            #region 原方法，大范围效率低，暂弃用。先计算出目标多边形范围内全部的行列号，通过枚举比较行列号进行检索
            /*
        List<string> rows;
        List<string> cols;
        GridGeneration gg = new GridGeneration(tileLevel);
        gg.GetAOITilesGrid(coordsStr, out rows, out cols);

        return SearProdTileCountBatch(prodType, rows, cols, tileLevel, startdate, enddate);
         */
            #endregion

            #region 现方法，通过多边形的最大最小经纬度范围，计算出最大最小行列号，通过比较行列号大小进行检索
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            return SearTileCountBatchAllType_coordsStr("productTiles", coordsStr, tileLevel, "", "", string.Format("and ProdType like '{0}'{1}", prodType, timecondition));
            #endregion
        }




        public DataSet SearProdTileCountBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate)
        {
            string timecondition = "";
            timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            return SearTileCountBatchAllType("productTiles", rows, cols, lv, "", "", string.Format("and ProdType like '{0}'{1}", prodType, timecondition));
        }



        public DataSet SearTileCountBatchAllType(string tablename, List<string> row, List<string> col, string tileLevel, string sat, string sensor, string othercondition)
        {

            //logStr = string.Format("row='{0}' col='{1}' tileLevel='{2}'", InforLog<string>.returnListStrElem(row), InforLog<string>.returnListStrElem(col), tileLevel);
            //InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileBatchAllType 输入:" + logStr);


            /*
            select *, count(distinct Date) from (select * from correctedTiles t1 where type='Preview' and ((Row=519 and Col=1184 and Level=8) or (Row=519 and Col=1185 and Level=8) or (Row=519 and Col=1186 and Level=8) or (Row=520 and Col=1184 and Level=8) or (Row=520 and Col=1185 and Level=8) or (Row=520 and Col=1186 and Level=8) or (Row=520 and Col=1187 and Level=8)) and (select count(*) from correctedTiles t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.type=t1.type and t2.Date>t1.Date) <1)  group by Date,type,level,row,col 
            */
            if (row.Count != col.Count)
                return returnDs;

            string sqlrowscols = "(";
            for (int i = 0; i < row.Count; i++)
            {
                sqlrowscols += String.Format("(Row={0} and Col={1} and Level={2}) or ", row.ElementAt(i), col.ElementAt(i), tileLevel);
            }
            sqlrowscols = sqlrowscols.TrimEnd(("or ").ToCharArray());
            sqlrowscols += ")";

            string sqlSatSensor = "";
            if (tablename == "correctedTiles")
            {
                if (sat != "")
                {
                    sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
                }
                if (sensor != "")
                {
                    sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));      //'_'是SQL通配符，代表任意单字符，比如WFV_={WFV1,WFV2,WFV3,WFV4}
                }
                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }

            //SQL语句构建
            string sql = string.Format("Select count(*) as Count,[Level],[Row],[Col] From {0} where {1}{2} {3} group by [Level],[Row], [Col]", tablename, sqlrowscols, sqlSatSensor, othercondition);


            //统一摘出来
            WebServiceUtil.allDtCol.Clear();//清理List

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (returnDs.Tables.Count != 0)
            //{
            //    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaanBatch 返回:DataSet不为空");
            //}
            //else
            //    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaanBatch 返回:DataSet为空");
            //return tilePath;
            returnDs.AcceptChanges();
            return returnDs;
        }
       
        public List<string> SearTileGlobeTest()
        {
            List<string> position = new List<string>();
            List<string> satellite = new List<string>();
            List<string> sensor = new List<string>();
            List<string> datatype = new List<string>();
            List<string> tileLevel = new List<string>();
            return SearTileGlobe(position, satellite, sensor, datatype, tileLevel);
        }

        public List<string> SearTileGlobe(List<string> position, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
        {
            logStr = string.Format("position='{0}' satellite='{1}' sensor='{2}' datatype='{3}' tileLevel='{4}'", InforLog<string>.returnListStrElem(position), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileGlobe 输入:" + logStr);
            //allRecordCount = 0;       
            List<string> tilePath = new List<string>();
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i], position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = String.Format("select * from correctedTiles t1 where (select count(*) from correctedTiles t2 where t2.DataSourceID=t1.DataSourceID and t2.Satellite=t1.Satellite and t2.Sensor=t1.Sensor and t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.type=t1.type and t2.Date>t1.Date)<1 and  ");
            string sql2 = QueryCollectionForSqlite3(listTileLevel, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("t1 "));

            }
            else
            {
                sql = sql + sql2;

            }
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);

            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }


            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                DataTable table = returnDs.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string destPath = da.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                    if (destPath == "-1")
                        continue;
                    tilePath.Add(destPath);
                }
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileGlobe 返回:tilePath=" + InforLog<string>.returnListStrElem(tilePath));
            return tilePath;
            //return returnDs;
        }



        public string SearTileForSimulation(List<string> RowCols, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
        {
            logStr = string.Format("RowCols='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}'", InforLog<string>.returnListStrElem(RowCols), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForSimulation 输入:" + logStr);
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            string sql = "select * from correctedTiles where (";
            #region//经纬度及层级列表处理，方便生成sql语句。
            for (int i = 0; i < RowCols.Count; i++)
            {
                try
                {
                    string[] rowcols = RowCols[i].Split(",".ToCharArray());
                    if (rowcols.Length == 3)
                        sql += String.Format("(Row={0} and Col>={1} and Col<={2}) or ", int.Parse(rowcols[0]), int.Parse(rowcols[1]), int.Parse(rowcols[2]));
                }
                catch
                {
                    continue;
                }
            }
            sql = sql.TrimEnd();
            if (RowCols.Count > 0)
            {
                sql = sql.TrimEnd("or".ToCharArray());
                sql += ")";
            }
            else
                sql = sql.TrimEnd("(".ToCharArray());
            if (tileLevel.Count != 0 || tileLevel != null)
            {
                sql += " and (";
                string str = "";
                for (int i = 0; i < tileLevel.Count; i++)
                    str += " Level=" + tileLevel[i] + " or";
                str = str.TrimEnd("or".ToCharArray());
                sql += str;
                sql += ")";
            }
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 != "")
                sql += " and " + sql2;
            #endregion
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            SQLBase.AddData_TileNameAddress(returnDs);
            List<string> tilePath = new List<string>();
            if (returnDs.Tables.Count != 0)
            {
                for (int i = 0; i < returnDs.Tables[0].Rows.Count; i++)
                {
                    string destPath = returnDs.Tables[0].Rows[i]["TileFileName"].ToString();
                    if (destPath == "-1")
                        continue;
                    tilePath.Add(destPath);
                }
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForSimulation 输出:returnDs.GetXml()=" + returnDs.GetXml());
            returnDs.AcceptChanges();
            return returnDs.GetXml();
            //return returnDs;
        }
        public string SearTileForSimulation1(string[][] RowCol, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
        {
            logStr = string.Format("RowCol='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}'", InforLog<string>.returnTArrStrElem(RowCol), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForSimulation1 输入:" + logStr);
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            string sql = "select * from correctedTiles where (";
            #region//经纬度及层级列表处理，方便生成sql语句。
            for (int i = 0; i < RowCol.Length; i++)
            {
                if (RowCol[i].Length == 3)
                    sql += String.Format("(Row={0} and Col>={1} and Col<={2}) or ", RowCol[i][0], RowCol[i][1], RowCol[i][2]);
            }
            sql = sql.TrimEnd();
            if (RowCol.Length > 0)
            {
                sql = sql.TrimEnd("or".ToCharArray());
                sql += ")";
            }
            else
                sql = sql.TrimEnd("(".ToCharArray());
            if (tileLevel.Count != 0 || tileLevel != null)
            {
                sql += " and (";
                string str = "";
                for (int i = 0; i < tileLevel.Count; i++)
                    str += " Level=" + tileLevel[i] + " or";
                str = str.TrimEnd("or".ToCharArray());
                sql += str;
                sql += ")";
            }
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 != "")
                sql += " and " + sql2;
            #endregion
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            SQLBase.AddData_TileNameAddress(returnDs);
            List<string> tilePath = new List<string>();
            if (returnDs.Tables.Count != 0)
            {
                for (int i = 0; i < returnDs.Tables[0].Rows.Count; i++)
                {
                    string destPath = returnDs.Tables[0].Rows[i]["TileFileName"].ToString();
                    if (destPath == "-1")
                        continue;
                    tilePath.Add(destPath);
                }
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForSimulation1 输出:returnDs.GetXml()=" + returnDs.GetXml());
            returnDs.AcceptChanges();
            return returnDs.GetXml();
            //return returnDs;
        }

        public DataSet SearTilePaged2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int startIndex, int offset)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' startIndex='{6}' offset='{7}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged2 输入:" + logStr);
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                //tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i], position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }

            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged2 输出:returnDs不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged2 输出:returnDs为空");
            returnDs.AcceptChanges();
            return returnDs;
        }

        public string SearTilePaged3(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int startIndex, int offset)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' startIndex='{6}' offset='{7}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged3 输入:" + logStr);
            //allRecordCount = 0;
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i], position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }

            QuerySearchInfo queryResult = getQuerySearchInfo(sql);

            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged3 输出:returnDs.GetXml()=" + returnDs.GetXml());
            returnDs.AcceptChanges();
            return returnDs.GetXml();
        }
        public DataSet SearPRODTileTest()
        {
            List<string> position = new List<string>();
            List<int> datetime = new List<int>();
            List<string> datatype = new List<string>();
            List<string> tileLevel = new List<string>();
            int pageIndex = 0;
            int pageSize = 0;
            int AllRecordCount;
            return SearPRODTile(position, datetime, datatype, tileLevel, pageIndex, pageSize, out AllRecordCount);
        }

        public DataSet SearPRODTile(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize, out int AllRecordCount)
        {
            logStr = string.Format("position='{0}' datetime='{1}' datatype='{2}' tileLevel='{3}' pageIndex='{4}' pageSize='{5}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), pageIndex, pageSize);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTile 输入:" + logStr);
            WebServiceUtil.allDtCol.Clear();//清理List
            #region//经纬度转行列号
            int[] rowAndColum = new int[4];
            if (position.Count != 0)
            {
                rowAndColum = DirectlyAddressing.GetRowAndColum(position.ToArray(), tileLevel[0]);
                position.Clear();
                foreach (int item in rowAndColum)
                {
                    position.Add(item.ToString());
                }
            }
            #endregion
            string sql = string.Format("select * from productTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite_Prod(position, datetime, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }
            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sql);
                //string IPTEST = "192.168.0.15";
                //ipandsql.Add(IPTEST);
                ipandsql.Add(IP);
                ipandsql.Add(pageIndex.ToString());
                ipandsql.Add(tilepath);
                //DLF 20130416 添加pageSize参数，使分页更灵活。以下方法中都增加 BaseUtil.InputAndOutput，sm.GetDataSetCol，tileSearchUtil.GetDataSetCol。
                ipandsql.Add(pageSize.ToString());
                //InputAndOutput(ipandsql);
                Task t = new Task(o => BaseUtil.InputAndOutput((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();
            allrecordNum = BaseUtil.allrecordNum;
            //if (ds.Tables.Count != 0)
            //{
            //    DataRow dr = ds.Tables[0].NewRow();

            //    dr["ID"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    ds.Tables[0].Rows.InsertAt(dr, 0);
            //}
            //return returnDs;

            //程序更改：不把结果数目在表中返回。而是作为一个输出参数返回。DLF。得到最终返回的Dataset后 ，截取用户需要的长度的Dataset。
            AllRecordCount = allrecordNum;
            DataSet pageds = new DataSet();
            pageds = TileSearchUtil.SplitDataSet(returnDs, pageSize, pageIndex);

            //清除ID列（已经无意义），添加切片数据名称列。
            pageds = SQLBaseTool.AddTileDataNameInfo(pageds);
            if (pageds.Tables.Count > 0 && pageds.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTile 输出:pageds不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTile 输出:pageds为空");

            pageds.AcceptChanges();
            return pageds;
        }

        public DataSet SearPRODTile2(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize)
        {
            logStr = string.Format("position='{0}' datetime='{1}' datatype='{2}' tileLevel='{3}' pageIndex='{4}' pageSize='{5}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), pageIndex, pageSize);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTile2 输入:" + logStr);
            int allRecordCount = -1;
            DataSet returnDS = SearPRODTile(position, datetime, datatype, tileLevel, pageIndex, pageSize, out allRecordCount);

            if (returnDS != null && returnDS.Tables.Count != 0 && returnDS.Tables[0].Rows.Count > 0)
            {
                try
                {
                    DataRow dr = returnDS.Tables[0].NewRow();

                    dr["TileFileName"] = allRecordCount;
                    returnDS.Tables[0].Rows.InsertAt(dr, 0);
                    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTile2 输出:returnDS不为空");
                }
                catch
                {

                }
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTile2 输出:returnDS为空");
            returnDs.AcceptChanges();
            return returnDS;
        }

        public DataSet SearPRODTilePaged(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
        {
            logStr = string.Format("position='{0}' datetime='{1}' datatype='{2}' tileLevel='{3}' startIndex='{4}' offset='{5}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTilePaged 输入:" + logStr);
            allRecordCount = 0;
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i], position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from productTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2_Prod(listTileLevel, datetime, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }

            QuerySearchInfo queryResult = getQuerySearchInfo(sql);

            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);

            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            //if (returnDs.Tables.Count != 0)
            //{
            //    DataRow dr = returnDs.Tables[0].NewRow();

            //    dr["TileFilePath"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    returnDs.Tables[0].Rows.InsertAt(dr, 0);
            //}
            if (returnDs.Tables.Count > 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTilePaged 输出:returnDs不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearPRODTilePaged 输出:returnDs为空");
            returnDs.AcceptChanges();
            return returnDs;
        }


        public DataSet SearSpaceDistinctTiles(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearSpaceDistinctTiles 输入:" + logStr);
            WebServiceUtil.allDtCol.Clear();//清理List
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i], position);
                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("Select Level,Row,Col,count(*) as Spacecount From correctedTiles  where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));
            }
            else
            {
                sql = sql + sql2;
            }
            sql += string.Format(" group by Level,Row,Col");

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();
            allrecordNum = BaseUtil.allrecordNum;
            if (returnDs.Tables.Count > 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearSpaceDistinctTiles 输出:returnDs不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearSpaceDistinctTiles 输出:returnDs为空");
            returnDs.AcceptChanges();
            return returnDs;
        }


        public DataSet SearTileForKSHByRowAndCol(string tileLevel, string minRow, string maxRow, string minCol, string maxCol)
        {
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。

            #endregion
            string sql = String.Format("select * from correctedTiles where Row >= '{0}' and Row <= '{1}'and Col >= '{2}' and Col <= '{3}' and Level='{4}'", minRow, maxRow, minCol, maxCol, tileLevel);
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                DataTable table = returnDs.Tables[0];

                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:DataSet不为空");
                returnDs.AcceptChanges();
                return returnDs;
            }
            else
                return null;
        }

        public String SearTileForKSH(string satellite, string sensor, string datetime, string row, string col, string tileLevel)
        {
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 输入:row=" + row + " col=" + col + " tileLevel=" + tileLevel);
            //allRecordCount = 0;       
            string tilePath = null;
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。

            #endregion
            string sql = String.Format("select * from correctedTiles where Satellite='{0}'and Sensor='{1}'and Date='{2}'and Row='{3}' and Col='{4}' and Level='{5}' and type='Preview'", satellite, sensor, datetime, row, col, tileLevel);
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                DataTable table = returnDs.Tables[0];
                string destPath = null;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    destPath = da.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                    if (destPath == "-1")
                        continue;
                    else
                    {
                        break;
                    }
                }

                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:DataSet不为空");
                return destPath;
            }
            else
                return null;
        }


        public DataSet SearTileByRegion(string regionName, string category, string type, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
        {
            List<string> position = new List<string>();
            DotSpatial.Data.IFeatureSet ProVftset, Cityftset, Countyftset;
            string shpPath = string.Format("{0}\\{1}", Application.StartupPath.ToString(), "map\\provincialBoundary.shp");
            ProVftset = DotSpatial.Data.Shapefile.Open(shpPath);
            string cityShpPath = string.Format("{0}\\{1}", Application.StartupPath.ToString(), "map\\cityBoundary.shp");
            Cityftset = DotSpatial.Data.Shapefile.Open(cityShpPath);
            string countryShpPath = string.Format("{0}\\{1}", Application.StartupPath.ToString(), "map\\countyBoundary.shp");
            Countyftset = DotSpatial.Data.Shapefile.Open(countryShpPath);
            string maxLon = null, minLon = null, maxLat = null, minLat = null;
            DotSpatial.Data.IFeature iFeature = null;
            if (category == "省")
            {
                foreach (DotSpatial.Data.Feature f in ProVftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == regionName)
                    {
                        iFeature = f;
                        List<double> latList = new List<double>();
                        List<double> lonList = new List<double>();
                        foreach (var coordinate in f.Coordinates.ToList())
                        {
                            lonList.Add(coordinate[0]);
                            latList.Add(coordinate[1]);
                        }
                        maxLat = latList.Max().ToString();
                        minLat = latList.Min().ToString();
                        maxLon = lonList.Max().ToString();
                        minLon = lonList.Min().ToString();
                        break;
                    }
                }
            }
            else if (category == "市")
            {
                foreach (DotSpatial.Data.Feature f in Cityftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == regionName)
                    {
                        iFeature = f;
                        List<double> latList = new List<double>();
                        List<double> lonList = new List<double>();
                        foreach (var coordinate in f.Coordinates.ToList())
                        {
                            lonList.Add(coordinate[0]);
                            latList.Add(coordinate[1]);
                        }
                        maxLat = latList.Max().ToString();
                        minLat = latList.Min().ToString();
                        maxLon = lonList.Max().ToString();
                        minLon = lonList.Min().ToString();
                        break;
                    }
                }
            }
            else
            {
                foreach (DotSpatial.Data.Feature f in Countyftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == regionName)
                    {
                        iFeature = f;
                        List<double> latList = new List<double>();
                        List<double> lonList = new List<double>();
                        foreach (var coordinate in f.Coordinates.ToList())
                        {
                            lonList.Add(coordinate[0]);
                            latList.Add(coordinate[1]);
                        }
                        maxLat = latList.Max().ToString();
                        minLat = latList.Min().ToString();
                        maxLon = lonList.Max().ToString();
                        minLon = lonList.Min().ToString();
                        break;
                    }
                }
            }
            position.Add(minLat);
            position.Add(minLon);
            position.Add(maxLat);
            position.Add(maxLon);
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' OtherQuery='{6}' startIndex='{7}' offset='{8}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), OtherQuery, startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged1 输入:" + logStr);
            allRecordCount = 0;
            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                tileLevel = new List<string> { "8" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i].Trim(), position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }
            if (OtherQuery != null || OtherQuery != "")
            {
                sql += OtherQuery;
            }


            #region 获取各站点配号检索的起始终止序号  limit startIndex-endIndex
            QuerySearchInfo queryResult = getQuerySearchInfo(sql, null, new string[] { regionName, category }, type);

            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);

            #endregion

            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged3((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);


            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);

            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged1 输出:returnDs 不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTilePaged1 输出:returnDs 为空");
            returnDs.AcceptChanges();
            return returnDs;
        }
       

        public string QueryCollectionForSqlite3(List<TileLevelPosition> levelPositions, List<string> satellite, List<string> sensor, List<string> datatype)
        {
            logStr = string.Format("levelPositions=['{0}'] satellite='{1}' sensor='{2}' datatype='{3}'", InforLog<string>.returnTileLevPosElem(levelPositions), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype));
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：QueryCollectionForSqlite3 输入:" + logStr);
            //string tablename = "correctedTiles";
            string QueryStr = "";

            if (levelPositions.Count != 0)
            {
                QueryStr += "(";
                foreach (TileLevelPosition levelposition in levelPositions)
                {

                    if (levelposition.tileRowandColumn.Length >= 4)
                    {

                        int minRow = levelposition.tileRowandColumn[0];
                        int minColum = levelposition.tileRowandColumn[1];
                        int maxRow = levelposition.tileRowandColumn[2];
                        int maxColum = levelposition.tileRowandColumn[3];

                        QueryStr += string.Format("( t1.Level='{0}' and t1.Row>={1} and t1.Row<={3} and t1.Col>={2} and t1.Col<={4} ) or ", levelposition.TileLevel, minRow, minColum, maxRow, maxColum);
                    }
                }

                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }

            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" t1.Satellite = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" t1.Sensor = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }

            if (datatype.Count != 0)
            {
                QueryStr += " (";
                foreach (string type in datatype)
                {
                    QueryStr += string.Format(" t1.type = '{0}' or ", type);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            //if (tileLevel.Count!=0)
            //{
            //    QueryStr += " (";
            //    foreach (int level in tileLevel)
            //    {
            //        QueryStr += string.Format(" Level = '{0}' or ", level);
            //    }
            //    QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
            //}
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：QueryCollectionForSqlite3 输出:QueryStr=" + QueryStr);
            return QueryStr;
        }
        public string SearchTileJPG(string dateTime, string tileLevel, string row, string col)
        {
            logStr = string.Format("dateTime='{0}' tileLevel='{1}' row='{2}' col='{3}'", dateTime, tileLevel, row, col);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearchTileJPG 输入:" + logStr);
            string jpgPath = "-1";
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();

            string sql = String.Format("select * from correctedTiles where Row='{0}' and Col='{1}' and Level='{2}' and Date='{3}' and type='Preview'", row, col, tileLevel, dateTime);
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                DataTable table = returnDs.Tables[0];
                if (table.Rows.Count > 0)
                    jpgPath = da.GetPathByFileName(table.Rows[1]["TileFileName"].ToString());
                //for (int i = 0; i < table.Rows.Count; i++)
                //{
                //    string destPath = da.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                //    if (destPath == "-1")
                //        continue;				
                //}
            }
            //return tilePath;
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearchTileJPG 输出:jpgPath=" + jpgPath);
            return jpgPath;
        }
        public List<string> SearchTileJPGForGloble(string dateTime, string tileLevel, string row, string col)
        {
            logStr = string.Format("dateTime='{0}' tileLevel='{1}' row='{2}' col='{3}'", dateTime, tileLevel, row, col);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearchTileJPGForGloble 输入:" + logStr);
            List<string> TilePath = new List<string>();
            //string jpgPath = "-1";
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();

            string sql = String.Format("select * from correctedTiles where Row='{0}' and Col='{1}' and Level='{2}' and type='Preview' and  Date='{3}' UNION ALL select * from correctedTiles where Row='{0}' and Col='{1}' and Level='{2}' and type='Preview' and  Date='{3}' ", row, col, tileLevel, dateTime);
            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            int allRecordsCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            PagedSearchTool.GetPageInfo(0, queryResult.QRST_ModsList, allRecordsCount);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }
            //foreach (Task t in tasks)
            //{
            //    t.Wait();
            //}

            Task[] taskArr = tasks.ToArray();
            Task.WaitAll(taskArr, 2000);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            if (returnDs.Tables.Count != 0)
            {
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                DataTable table = returnDs.Tables[0];
                if (table.Rows.Count > 0)
                {
                    //jpgPath = da.GetPathByFileName(table.Rows[1]["TileFileName"].ToString());
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string destPath = da.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                        if (destPath == "-1")
                            continue;
                        else
                            TilePath.Add(destPath);
                    }
                }
            }
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearchTileJPGForGloble 输出:TilePath=" + InforLog<string>.returnListStrElem(TilePath));
            //return tilePath;
            return TilePath;
        }
        public DataSet SearPRODTileForJCGX(List<string> position, List<int> datetime, List<string> ProdType, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
        {
            allRecordCount = 0;

            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                //tileLevel = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9,", "10", "11", "12", "13", "14" };
            }

            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i].Trim(), position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from productTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2_Prod(listTileLevel, datetime, ProdType);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }

            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            //if (returnDs.Tables.Count != 0)
            //{
            //    DataRow dr = returnDs.Tables[0].NewRow();

            //    dr["TileFilePath"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    returnDs.Tables[0].Rows.InsertAt(dr, 0);
            //}
            if (returnDs.Tables.Count > 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForJCGX 输出:returnDs不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForJCGX 输出:returnDs为空");
            returnDs.AcceptChanges();
            return returnDs;
        }

        public DataSet SearTileForJCGX(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
        {
            logStr = string.Format("position='{0}' datetime='{1}' satellite='{2}' sensor='{3}' datatype='{4}' tileLevel='{5}' startIndex='{6}' offset='{7}'", InforLog<string>.returnListStrElem(position), InforLog<int>.returnListStrElem(datetime), InforLog<string>.returnListStrElem(satellite), InforLog<string>.returnListStrElem(sensor), InforLog<string>.returnListStrElem(datatype), InforLog<string>.returnListStrElem(tileLevel), startIndex, offset);
            InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForJCGX 输入:" + logStr);
            allRecordCount = 0;

            if (offset <= 0 || startIndex < 0)
            {
                return null;
            }
            if ((position != null && position.Count > 0) && (tileLevel == null || tileLevel.Count == 0))
            {
                //tileLevel = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9,", "10", "11", "12", "13", "14" };
            }
            if (datatype.Count == null || datatype.Count == 0)
            {
                datatype = new List<string> { "Preview" };
            }
            WebServiceUtil.allDtCol.Clear();//清理List
            WebServiceUtil.QRSTModInfo.Clear();
            #region//经纬度及层级列表处理，方便生成sql语句。
            List<TileLevelPosition> listTileLevel = new List<TileLevelPosition>();

            for (int i = 0; i < tileLevel.Count; i++)
            {
                TileLevelPosition levelposition = new TileLevelPosition(tileLevel[i].Trim(), position);

                listTileLevel.Add(levelposition);
            }
            #endregion
            string sql = string.Format("select * from correctedTiles where ");
            string sql2 = searchcondition.QueryCollectionForSqlite2(listTileLevel, datetime, satellite, sensor, datatype);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf("where "));

            }
            else
            {
                sql = sql + sql2;

            }

            QuerySearchInfo queryResult = getQuerySearchInfo(sql);
            PagedSearchTool.GetPageInfo(startIndex, queryResult.QRST_ModsList, offset);
            //updateModsList(queryResult.QRST_ModsList);
            List<Task> tasks = new List<Task>();
            string[] IPcol = GetIPFromQRST_ModsList(queryResult.QRST_ModsList);
            foreach (string IP in IPcol)
            {
                QuerySearchInfo siteQueryInfo = new QuerySearchInfo(sql, DateTime.Now);

                List<ModIDSearchInfo> siteModsinfo = getSiteQueryInfo(IP, queryResult);
                siteQueryInfo.ObjectTime = queryResult.ObjectTime;
                siteQueryInfo.QRST_ModsList = siteModsinfo;

                Task t = new Task(o => BaseUtil.GetDataSetColPaged2((QuerySearchInfo)o), siteQueryInfo);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);


            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            allRecordCount = PagedSearchTool.SumModsRecordsCount(queryResult.QRST_ModsList);
            //if (returnDs.Tables.Count != 0)
            //{
            //    DataRow dr = returnDs.Tables[0].NewRow();

            //    dr["TileFilePath"] = allrecordNum;
            //    //ds.Tables[0].Rows.Add(dr);
            //    returnDs.Tables[0].Rows.InsertAt(dr, 0);
            //}
            if (returnDs.Tables.Count > 0 && returnDs.Tables[0].Rows.Count > 0)
            {
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForJCGX 输出:returnDs不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileForJCGX 输出:returnDs为空");
            returnDs.AcceptChanges();
            return returnDs;
        }
        public int SearTileForJCGXtest()
        {
            List<string> position = new List<string>();
            position.Add("-180");
            position.Add("-90");
            position.Add("180");
            position.Add("90");
            List<int> datetime = new List<int>();
            List<string> satellite = new List<string>();
            List<string> sensor = new List<string>();
            List<string> datatype = new List<string>();
            List<string> tileLevel = new List<string>();
            tileLevel.Add("7");
            int startIndex = 0;
            int allRecordCount = 0;
            int offset = 100;
            SearTileForJCGX(position, datetime, satellite, sensor, datatype, tileLevel, out allRecordCount, startIndex, offset);
            return allRecordCount;
        }


        #region 接口实现
        public DataSet GetDataSetFromSQLite(string sql, string DataSourcePath)
        {
            return _baseUtilities.GetDataSetByPath(sql, DataSourcePath);
        }

        //public List<string> GetDataDistinct(string distinctSql)
        //{

        //}
        #endregion


        #region 内部方法
        public DataSet SearImgTilePathBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, bool needTilePath = true)
        {
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }

            return SearTileBatchAllType("correctedTiles", row, col, tileLevel, sat, sensor, "and type='Preview'" + timecondition, needTilePath);
        }

        public DataSet SearTileBatchAllType(string tablename, List<string> row, List<string> col, string tileLevel, string sat, string sensor, string othercondition, bool needTilePath = false)
        {
            /*
            select *, count(distinct Date) from (select * from correctedTiles t1 where type='Preview' and ((Row=519 and Col=1184 and Level=8) or (Row=519 and Col=1185 and Level=8) or (Row=519 and Col=1186 and Level=8) or (Row=520 and Col=1184 and Level=8) or (Row=520 and Col=1185 and Level=8) or (Row=520 and Col=1186 and Level=8) or (Row=520 and Col=1187 and Level=8)) and (select count(*) from correctedTiles t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.type=t1.type and t2.Date>t1.Date) <1)  group by Date,type,level,row,col 
            */
            if (row.Count != col.Count)
                return returnDs;
            if (row.Count > 5000)
            {
                throw new Exception("格网数超过限额！");
            }

            string sqlrowscols = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < row.Count; i++)
            {
                sb.AppendFormat("(Row={0} and Col={1} and Level={2}) or ", row.ElementAt(i), col.ElementAt(i), tileLevel);
                //sqlrowscols += String.Format("(Row={0} and Col={1} and Level={2}) or ", row.ElementAt(i), col.ElementAt(i), tileLevel);
            }
            sqlrowscols = sb.ToString();
            sqlrowscols = sqlrowscols.TrimEnd(("or ").ToCharArray());
            sqlrowscols += ")";

            string sqlSatSensor = "";
            string typestr = "type";
            if (tablename == "correctedTiles")
            {
                if (sat != "")
                {
                    sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
                }
                if (sensor != "")
                {
                    sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));      //'_'是SQL通配符，代表任意单字符，比如WFV_={WFV1,WFV2,WFV3,WFV4}
                }
                typestr = "type";

                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }
            else if (tablename == "productTiles")
            {
                typestr = "ProdType";
            }



            //原按照最新日期的查询条件存在问题，存在相同日期不同数据源的多个数据，需要移除
            string sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);


            //不分页检索
            WebServiceUtil.allDtCol.Clear();//清理List

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks, 15000);

            returnDs = BaseUtil.MergeAllDataSet();

            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Columns.Contains("count(distinct Date)"))
            {
                returnDs.Tables[0].Columns.Remove("count(distinct Date)");
            }

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = 0;
            if (returnDs.Tables.Count != 0)
            {
                allrecordNum = returnDs.Tables[0].Rows.Count;
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);


                if (needTilePath)
                {
                    List<string> tilePath = new List<string>();
                    DataTable table = returnDs.Tables[0];
                    table.Columns.Add("TileFilePath", typeof(string));
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string destPath = BaseUtil._DAUtil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                        table.Rows[i]["TileFilePath"] = destPath;
                    }
                }

            }
            returnDs.AcceptChanges();
            return returnDs;
        }


        public DataSet SearTileSingleAllType(string tablename, string row, string col, string tileLevel, string otherCondition, bool needTilePath = false)
        {
            if (tablename == "correctedTiles")
            {
                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }
            string sql = String.Format("select * from {3} where Row='{0}' and Col='{1}' and Level='{2}' {4}", row, col, tileLevel, tablename, otherCondition);

            //不分页检索
            WebServiceUtil.allDtCol.Clear();//清理List
                                            //List<string> tilePath = new List<string>();

            List<Task> tasks = new List<Task>();

            List<string> ipandsql = new List<string>();

            string IP = BaseUtil._DAUtil.GetIpByRowCol(row, col);
            string tilepath = BaseUtil.getSqlitedbfilepathByRowCol(row, col);

            ipandsql.Add(sql);

            ipandsql.Add(IP);

            ipandsql.Add(tilepath);

            Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
            t.Start();
            tasks.Add(t);

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            if (returnDs.Tables.Count != 0)
            {
               int  allrecordNum = returnDs.Tables[0].Rows.Count;
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                if (needTilePath)
                {
                    List<string> tilePath = new List<string>();
                    DataTable table = returnDs.Tables[0];
                    table.Columns.Add("TileFilePath", typeof(string));
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string destPath = BaseUtil._DAUtil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                        table.Rows[i]["TileFilePath"] = destPath;
                    }
                }
            }
            else
            { }
            //return tilePath;
            returnDs.AcceptChanges();
            return returnDs;
        }

        public System.Data.DataSet SearProdTilePathBatch_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate, string priority, bool needTilePath = true)
        {
            #region 原方法，大范围效率低，暂弃用。先计算出目标多边形范围内全部的行列号，通过枚举比较行列号进行检索
            /*List<string> rows;
            List<string> cols;
            GridGeneration gg = new GridGeneration(tileLevel);
            gg.GetAOITilesGrid(coordsStr, out rows, out cols);

            return SearProdTileBatch(prodType, rows, cols, tileLevel, startdate, enddate);
             */
            #endregion

            #region 现方法，通过多边形的最大最小经纬度范围，计算出最大最小行列号，通过比较行列号大小进行检索
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }
            return SearTileBatchAllType_coordsStr("productTiles", coordsStr, tileLevel, "", "", string.Format("and ProdType like '{0}'{1}", prodType, timecondition), priority, needTilePath);
            #endregion
        }

        public DataSet SearTileCountBatchAllType_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition)
        {

            //获取多边形最大最小经纬度
            double[] mmll = GridGeneration.GetMinMaxLatLonFormCoordsStr(coordsStr);


            //获取最大最小行列号
            int[] mmrc = DirectlyAddressing.GetRowAndColum(new string[] { mmll[0].ToString(), mmll[1].ToString(), mmll[2].ToString(), mmll[3].ToString() }, tileLevel);

            //构建SQL语句
            string sqlrowscols = string.Format("Row>={0} and Row<={1} and Col>={2} and Col<={3} and Level={4}", mmrc[0], mmrc[2], mmrc[1], mmrc[3], tileLevel);

            string sqlSatSensor = "";
            if (tablename == "correctedTiles")
            {
                if (sat != "")
                {
                    sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
                }
                if (sensor != "")
                {
                    sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));      //'_'是SQL通配符，代表任意单字符，比如WFV_={WFV1,WFV2,WFV3,WFV4}
                }

                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }

            //SQL语句构建
            string sql = string.Format("Select count(*) as Count,[Level],[Row],[Col] From {0} where {1}{2} {3} group by [Level],[Row], [Col]", tablename, sqlrowscols, sqlSatSensor, othercondition);


            //统一摘出来
            WebServiceUtil.allDtCol.Clear();//清理List

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                List<string> sqlCoordsIpTilepath = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                sqlCoordsIpTilepath.Add(sql);

                sqlCoordsIpTilepath.Add(coordsStr);

                sqlCoordsIpTilepath.Add(IP);

                sqlCoordsIpTilepath.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll_CoordsFilter((List<string>)o), sqlCoordsIpTilepath);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            returnDs = BaseUtil.MergeAllDataSet();

            //if (returnDs.Tables.Count != 0)
            //{
            //    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaanBatch 返回:DataSet不为空");
            //}
            //else
            //    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaanBatch 返回:DataSet为空");
            //return tilePath;
            returnDs.AcceptChanges();
            return returnDs;
        }

        public System.Data.DataSet SearImgTilePathBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string priority, bool needTilePath = true)
        {
            #region 原方法，大范围效率低，暂弃用。先计算出目标多边形范围内全部的行列号，通过枚举比较行列号进行检索
            /*
            List<string> rows;
            List<string> cols;
            GridGeneration gg = new GridGeneration(tileLevel);
            gg.GetAOITilesGrid(coordsStr, out rows, out cols);

            return SearImgTileBatch(sat, sensor, rows, cols, tileLevel, startdate, enddate);
            */
            #endregion

            #region 现方法，通过多边形的最大最小经纬度范围，计算出最大最小行列号，通过比较行列号大小进行检索
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }

            return SearTileBatchAllType_coordsStr("correctedTiles", coordsStr, tileLevel, sat, sensor, "and type='Preview'" + timecondition, priority, needTilePath);
            #endregion
        }

        /// <summary>
        /// 得到所有的ip列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string[] GetIPFromQRST_ModsList(List<ModIDSearchInfo> list)
        {
            List<string> ipList = new List<string>();
            foreach (ModIDSearchInfo modinfo in list)
            {
                if (!ipList.Contains(modinfo.IPaddress))
                {
                    ipList.Add(modinfo.IPaddress);
                }
            }
            return ipList.ToArray();
        }

        /// <summary>
        /// 从总的List<ModIDInfo> QRST_ModsList中读取当前IP，对应的List<ModIDInfo>
        /// </summary>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        private List<ModIDSearchInfo> getSiteQueryInfo(string ipAddress, QuerySearchInfo queryResult)
        {
            List<ModIDSearchInfo> returnList = new List<ModIDSearchInfo>();
            foreach (ModIDSearchInfo modinfo in queryResult.QRST_ModsList)
            {
                if (modinfo.IPaddress == ipAddress)
                {
                    returnList.Add(modinfo);
                }
            }
            return returnList;
        }


        /// <summary>
        /// 并行检索，任务等待
        /// </summary>
        /// <param name="tasks">并行检索任务list</param>
        /// <param name="outtime">超时时间 毫秒单位</param>
        private void WaitingForTasks(List<Task> tasks, int outtime = 20000)
        {
            DateTime startdt = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - startdt).TotalMilliseconds > outtime)
                {
                    //超时，可能站点更新间歇有站点异常退出，触发更新在线站点列表
                    if ((DateTime.Now - outtimeUpdateListTime).TotalMilliseconds > 5000)
                    {
                        //避免多线程重复调用，限制5秒内只调用一次
                        QRST_DI_TS_Process.Site.TServerSiteManager.UpdateOptimalStorageSiteList();
                        outtimeUpdateListTime = DateTime.Now;
                    }
                    break;
                }
                bool cmpl = true;
                foreach (Task t in tasks)
                {
                    if (!t.IsCompleted)
                    {
                        cmpl = false;
                    }
                }
                if (cmpl)
                {
                    break;
                }
                System.Threading.Thread.Sleep(200);
            }

            //Task[] taskArr = tasks.ToArray();
            //Task.WaitAll(taskArr, 2000);

        }


        /// <summary>
        /// 在检索分页信息池中查找短时（5分钟）内相同检索条件的检索分页信息,含各配号符合条件的记录数，用以进行计算分页记录分配
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="coordinates">空间过滤多边形，如果需要进一步空间过滤</param>
        /// <param name="type">过滤类型，默认相交</param>
        /// <returns></returns>
        private QuerySearchInfo getQuerySearchInfo(string sql, List<Coordinate> coordinates = null, string[] region = null, string type = "Intersect")
        {
            #region 在检索分页信息池中查找短时（5分钟）内相同检索条件的检索分页信息
            QuerySearchInfo queryInfo = null;
            bool updateModsInfo = true;

            if (listQueryResult == null)
            {
                listQueryResult = new List<QuerySearchInfo>();
            }

            for (int i = 0; i < listQueryResult.Count; i++)
            {
                string sqlstring = listQueryResult[i].SQLString;
                DateTime dtModsUpdate = listQueryResult[i].ObjectTime;
                TimeSpan ts = DateTime.Now - dtModsUpdate;
                if (sqlstring == sql)
                {
                    if (ts.TotalMinutes <= MinQueryTimeSpan)
                    {
                        updateModsInfo = false;
                        queryInfo = listQueryResult[i];
                        break;
                    }
                    else
                    {
                        listQueryResult.RemoveAt(i);
                        updateModsInfo = true;
                        break;
                    }
                }
                else
                {
                    updateModsInfo = true;
                }
            }
            #endregion

            #region 未找到分页信息，则重新构建配号查询信息(含各配号结果记录数)
            if (updateModsInfo)
            {
                if (coordinates == null || coordinates.Count == 0)
                {
                    queryInfo = GetQueryResultInfo_SiteModRecordCount(sql);
                }
                else if (region != null)
                {
                    queryInfo = GetQueryResultInfo_SiteModRecordCount(sql, region[0], region[1], type);
                }
                else
                {
                    queryInfo = GetQueryResultInfo_SiteModRecordCount(sql, coordinates, type);
                }
                listQueryResult.Add(queryInfo);
            }
            #endregion

            return queryInfo;
        }


        /// <summary>
        /// 并行查询各个配号VDS上符合条件的记录个数，用以分页查询前的分配
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private QuerySearchInfo GetQueryResultInfo_SiteModRecordCount(string sqlString)
        {
            QuerySearchInfo queryResult;
            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sqlString);
                //string IPTEST = "192.168.0.15";
                //ipandsql.Add(IPTEST);
                ipandsql.Add(IP);
                ipandsql.Add(tilepath);
                //InputAndOutput(ipandsql);
                Task t = new Task(o => BaseUtil.GetResultInfo_SiteModRecordCount((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            queryResult = new QuerySearchInfo(sqlString, DateTime.Now);
            queryResult.QRST_ModsList = new List<ModIDSearchInfo>(WebServiceUtil.QRSTModInfo);
            return queryResult;

        }

        /// <summary>
        /// 获取过滤瓦片查询结果信息
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private QuerySearchInfo GetQueryResultInfo_SiteModRecordCount(string sqlString, string regionName, string category, string type)
        {
            QuerySearchInfo queryResult;
            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                List<string> regionLst = new List<string>();
                Dictionary<List<string>, List<string>> dic = new Dictionary<List<string>, List<string>>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sqlString);
                //string IPTEST = "192.168.0.15";
                //ipandsql.Add(IPTEST);
                ipandsql.Add(IP);
                ipandsql.Add(tilepath);
                regionLst.Add(regionName);
                regionLst.Add(category);
                regionLst.Add(type);
                dic.Add(ipandsql, regionLst);
                //InputAndOutput(ipandsql);
                //Task t = new Task(o => BaseUtil.InAndOutResultInfo((List<string>)o), ipandsql);

                Task t = new Task(o => BaseUtil.GetResultInfo_SiteModRecordCount((Dictionary<List<string>, List<string>>)o), dic);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            queryResult = new QuerySearchInfo(sqlString, DateTime.Now);
            queryResult.QRST_ModsList = new List<ModIDSearchInfo>(WebServiceUtil.QRSTModInfo);
            return queryResult;

        }

        /// <summary>
        /// 获取查询结果信息
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private QuerySearchInfo GetQueryResultInfo_SiteModRecordCount(string sqlString, List<Coordinate> coordinate, string type)
        {
            QuerySearchInfo queryResult;
            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();

            foreach (string IP in IPcol)
            {
                List<string> ipandsql = new List<string>();
                Dictionary<List<string>, List<Coordinate>> dic = new Dictionary<List<string>, List<Coordinate>>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);
                ipandsql.Add(type);
                ipandsql.Add(sqlString);
                //string IPTEST = "192.168.0.15";
                //ipandsql.Add(IPTEST);
                ipandsql.Add(IP);
                ipandsql.Add(tilepath);
                dic.Add(ipandsql, coordinate);
                //InputAndOutput(ipandsql);
                Task t = new Task(o => BaseUtil.GetResultInfo_SiteModRecordCount((Dictionary<List<string>, List<Coordinate>>)o), dic);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            Task[] taskArr = tasks.ToArray();
            Task.WaitAll(taskArr, 2000);
            queryResult = new QuerySearchInfo(sqlString, DateTime.Now);
            queryResult.QRST_ModsList = new List<ModIDSearchInfo>(WebServiceUtil.QRSTModInfo);
            return queryResult;

        }
        #endregion

        public System.Data.DataSet SearImgTileSingleAddCloudAvail(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
        {
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            // Mincloud = "0";
            // Maxavailability = "100";
            //Maxcloud = "100";
            //Minavailability = "0";
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }

            string sqlSatSensor = "";
            if (sat != "")
            {
                sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
            }
            if (sensor != "")
            {
                sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));
            }
            string sqlcloudAndAvailability = "";
            sqlcloudAndAvailability += (Mincloud != "") ? "  and Cloud >= " + Mincloud : "  and Cloud >= 0";
            sqlcloudAndAvailability += (Maxcloud != "") ? "  and Cloud <= " + Maxcloud : "  and Cloud <= 255";
            sqlcloudAndAvailability += (Minavailability != "") ? "  and Availability >= " + Minavailability : "  and Availability >= 0";
            sqlcloudAndAvailability += (Maxavailability != "") ? "  and Availability <= " + Maxavailability : "  and Availability <= 255";

            return SearTileSingleAllTypeAddCloudAvai("correctedTiles", row, col, tileLevel, "and type='Preview'" + sqlSatSensor + timecondition, sqlcloudAndAvailability);
        }

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。
        /// priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public System.Data.DataSet SearImgTileBatchAddPriority(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, string priority)
        {
            return SearImgTilePathBatchAddPriority(sat, sensor, row, col, tileLevel, startdate, enddate, priority, false);
        }

        public System.Data.DataSet SearTileSingleAllTypeAddCloudAvai(string tablename, string row, string col, string tileLevel, string otherCondition, string sqlcloudAndAvailability, bool needTilePath = false)
        {

            string sql = String.Format("select * from {3} where Row='{0}' and Col='{1}' and Level='{2}' {4} {5}", row, col, tileLevel, tablename, otherCondition, sqlcloudAndAvailability);

            //不分页检索
            WebServiceUtil BaseUtil = new WebServiceUtil();
            //List<string> tilePath = new List<string>();

            List<Task> tasks = new List<Task>();

            List<string> ipandsql = new List<string>();

            string IP = BaseUtil._DAUtil.GetIpByRowCol(row, col);
            string tilepath = BaseUtil.getSqlitedbfilepathByRowCol(row, col);

            ipandsql.Add(sql);

            ipandsql.Add(IP);

            ipandsql.Add(tilepath);

            Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
            t.Start();
            tasks.Add(t);

            WaitingForTasks(tasks);

            System.Data.DataSet returnDs;
            returnDs = BaseUtil.MergeAllDataSet();

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            if (returnDs.Tables.Count != 0)
            {
                allrecordNum = returnDs.Tables[0].Rows.Count;
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);

                if (needTilePath)
                {
                    List<string> tilePath = new List<string>();
                    DataTable table = returnDs.Tables[0];
                    table.Columns.Add("TileFilePath", typeof(string));
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string destPath = BaseUtil._DAUtil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                        table.Rows[i]["TileFilePath"] = destPath;
                    }
                }
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:System.Data.DataSet不为空");
            }
            else
                InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaan 返回:System.Data.DataSet为空");
            //return tilePath;
            returnDs.AcceptChanges();
            return returnDs;
        }

        /// <summary>
        /// 为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号，云量，满幅率
        /// </summary>
        /// <param name="prodType"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="lv"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="Mincloud"></param>
        /// <param name="Maxcloud"></param>
        /// <param name="Minavailability"></param>
        /// <param name="Maxavailability"></param>
        /// <returns></returns>
        public System.Data.DataSet SearProdTileSingleAddCloudAvail(string prodType, string row, string col, string lv, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
        {
            // Mincloud = "0";
            //Maxavailability = "100";
            //Maxcloud = "100";
            //Minavailability = "0";
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }
            string sqlcloudAndAvailability = "";
            sqlcloudAndAvailability += (Mincloud != "") ? "  and Cloud >= " + Mincloud : "  and Cloud >= 0";
            sqlcloudAndAvailability += (Maxcloud != "") ? "  and Cloud <= " + Maxcloud : "  and Cloud <= 255";
            sqlcloudAndAvailability += (Minavailability != "") ? "  and Availability >= " + Minavailability : "  and Availability >= 0";
            sqlcloudAndAvailability += (Maxavailability != "") ? "  and Availability <= " + Maxavailability : "  and Availability <= 255";

            return SearTileSingleAllTypeAddCloudAvai("productTiles", row, col, lv, string.Format("and ProdType like '{0}'{1}", prodType, timecondition), sqlcloudAndAvailability);
        }

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。cloud,availability 分别为云量和满幅率，priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等"
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="tileLevel"></param>
        /// <param name="coordsStr"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="Mincloud"></param>
        /// <param name="Maxcloud"></param>
        /// <param name="Minavailability"></param>
        /// <param name="Maxavailability"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public System.Data.DataSet SearImgTileBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority)
        {
            //Mincloud = "0";
            //Maxavailability = "100";
            //Maxcloud = "100";
            //Minavailability = "0";
            return SearImgTilePathBatchAddCloudAvail_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate, Mincloud, Maxcloud, Minavailability, Maxavailability, priority, false);
        }

        /// <summary>
        ///"批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        ///sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等"
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="tileLevel"></param>
        /// <param name="coordsStr"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="Mincloud"></param>
        /// <param name="Maxcloud"></param>
        /// <param name="Minavailability"></param>
        /// <param name="Maxavailability"></param>
        /// <param name="priority"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        public System.Data.DataSet SearImgTilePathBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority, bool needTilePath = true)
        {
            #region 现方法，通过多边形的最大最小经纬度范围，计算出最大最小行列号，通过比较行列号大小进行检索
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }

            return SearTileBatchAllTypeAddCloudAvail_coordsStr("correctedTiles", coordsStr, tileLevel, sat, sensor, "and type='Preview'" + timecondition, Mincloud, Maxcloud, Minavailability, Maxavailability, priority, needTilePath);
            #endregion
        }

        /// <summary>
        ///批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        ///参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。
        ///priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="priority"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        public System.Data.DataSet SearImgTilePathBatchAddPriority(string sat, string sensor, List<string> row, List<string> col, string tileLevel, 
            int startdate, int enddate, string priority, bool needTilePath = true)
        {
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }

            return SearTileBatchAllTypeAddPriority("correctedTiles", row, col, tileLevel, sat, sensor, "and type='Preview'" + timecondition, priority, needTilePath);
        }

        /// <summary>
        ///批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        ///sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。
        ///priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等"
        /// </summary>
        /// <param name="prodType"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="lv"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public System.Data.DataSet SearProdTileBatchAddPriority(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, string priority)
        {
            return SearProdTilePathBatchAddPriority(prodType, rows, cols, lv, startdate, enddate, priority, false);
        }

        /// <summary>
        /// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
        /// </summary>
        /// <param name="prodType"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="lv"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="priority"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        public System.Data.DataSet SearProdTilePathBatchAddPriority(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, string priority, bool needTilePath = true)
        {
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }
            return SearTileBatchAllTypeAddPriority("productTiles", rows, cols, lv, "", "", string.Format("and ProdType like '{0}'{1}", prodType, timecondition), priority, needTilePath);
        }

        /// <summary>
        /// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。
        /// priority 为检索优先级,用英文缩词表示，支持为空，例如：ACT:满幅率-云量-时间；CTA:云量-时间-满幅率；TAC:时间-满幅率-云量 等
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="othercondition"></param>
        /// <param name="priority"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        public System.Data.DataSet SearTileBatchAllTypeAddPriority(string tablename, List<string> row, List<string> col, string tileLevel, string sat, string sensor, string othercondition, string priority, bool needTilePath = false)
        {
            /*
            select *, count(distinct Date) from (select * from correctedTiles t1 where type='Preview' and ((Row=519 and Col=1184 and Level=8) or (Row=519 and Col=1185 and Level=8) or (Row=519 and Col=1186 and Level=8) or (Row=520 and Col=1184 and Level=8) or (Row=520 and Col=1185 and Level=8) or (Row=520 and Col=1186 and Level=8) or (Row=520 and Col=1187 and Level=8)) and (select count(*) from correctedTiles t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.type=t1.type and t2.Date>t1.Date) <1)  group by Date,type,level,row,col 
            */

            System.Data.DataSet returnDs = new DataSet();
            if (row.Count != col.Count)
                return returnDs;
            if (row.Count > 5000)
            {
                throw new Exception("格网数超过限额！");
            }

            string sqlrowscols = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < row.Count; i++)
            {
                sb.AppendFormat("(Row={0} and Col={1} and Level={2}) or ", row.ElementAt(i), col.ElementAt(i), tileLevel);
                //sqlrowscols += String.Format("(Row={0} and Col={1} and Level={2}) or ", row.ElementAt(i), col.ElementAt(i), tileLevel);
            }
            sqlrowscols = sb.ToString();
            sqlrowscols = sqlrowscols.TrimEnd(("or ").ToCharArray());
            sqlrowscols += ")";

            string sqlSatSensor = "";
            string typestr = "type";
            if (tablename == "correctedTiles")
            {
                if (sat != "")
                {
                    sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
                }
                if (sensor != "")
                {
                    sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));      //'_'是SQL通配符，代表任意单字符，比如WFV_={WFV1,WFV2,WFV3,WFV4}
                }
                typestr = "type";

                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }
            else if (tablename == "productTiles")
            {
                typestr = "ProdType";
            }
            ////原按照最新日期的查询条件存在问题，存在相同日期不同数据源的多个数据，需要移除
            //string sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
            string sql;
            //所有的排序都是按照满幅率降序（取最大），时间降序（取最新），云量升序（取最小）的顺序进行筛选的
            //其中按照满幅率优先筛选的时候，要优先考虑非255的，即如果行列号为（582，1183）的所有瓦片数据，既有云量，满幅度（255，255）的也有非（255，255）的，则筛选的时候，会取非（255.255）的。
            switch (priority)
            {
                case "默认":
                    sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "时间-云量-满幅率":
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY  Date ASC , Cloud DESC, Availability ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "时间-满幅率-云量"://时间-满幅率-云量
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY  Date ASC ,Availability ASC , Cloud DESC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "云量-时间-满幅率"://云量-时间-满幅率
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY Cloud DESC, Date ASC , Availability ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "云量-满幅率-时间":
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3}) ORDER BY  Cloud DESC ,Availability ASC , Date ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "满幅率-时间-云量":
                    sql = string.Format("SELECT * FROM (SELECT  * FROM (SELECT *  FROM (SELECT *  FROM ( SELECT * FROM {0} t1 WHERE {1}{2} {3} AND cloud <> 255 AND availability <> 255) ORDER BY Availability ASC , date ASC , cloud DESC ) GROUP BY row ,col ,level ,{4}  UNION SELECT * FROM (SELECT  *  FROM ( SELECT  * FROM {0} t1 WHERE  {1}{2} {3} AND availability = 255 AND cloud = 255 ) ORDER BY availability ASC , date ASC , cloud DESC ) GROUP BY  row , col , level , {4} ) ORDER BY availability DESC , date DESC , cloud ASC ) GROUP BY row , col ,level ,{4} ", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
                case "满幅率-云量-时间":
                    sql = string.Format("SELECT * FROM (SELECT  * FROM (SELECT  * FROM (SELECT * FROM (SELECT  * FROM {0} t1 WHERE {1}{2} {3} AND cloud <> 255 AND availability <> 255) ORDER BY Availability ASC ,cloud DESC , date ASC) GROUP BY row , col , level ,{4} UNION SELECT * FROM (SELECT  * FROM ( SELECT  * FROM {0} t1 WHERE {1}{2} {3} AND availability = 255 AND cloud = 255 ) ORDER BY Availability ASC ,cloud DESC , date ASC ) GROUP BY row , col , level , {4}) ORDER BY availability DESC ,cloud ASC , date DESC ) GROUP BY row , col, level,{4}", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;

                default:
                    sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3}  and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
                    break;
            }

            //不分页检索
            WebServiceUtil BaseUtil = new WebServiceUtil();

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {

                //针对站点奔溃影响检索效率问题的解决方案：          joki170520
                if (!QRST_DI_TS_Process.Site.TServerSiteManager.optimalStorageSiteIPs.Contains(IP))
                {
                    Console.WriteLine("站点" + IP + "无法访问");
                    continue;
                }

                List<string> ipandsql = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                ipandsql.Add(sql);

                ipandsql.Add(IP);

                ipandsql.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll((List<string>)o), ipandsql);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks, 15000);

            returnDs = BaseUtil.MergeAllDataSet();

            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Columns.Contains("count(distinct Date)"))
            {
                returnDs.Tables[0].Columns.Remove("count(distinct Date)");
            }

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = 0;
            if (returnDs.Tables.Count != 0)
            {
                allrecordNum = returnDs.Tables[0].Rows.Count;
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);


                if (needTilePath)
                {
                    List<string> tilePath = new List<string>();
                    DataTable table = returnDs.Tables[0];
                    table.Columns.Add("TileFilePath", typeof(string));
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string destPath = BaseUtil._DAUtil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                        table.Rows[i]["TileFilePath"] = destPath;
                    }
                }

            }
            returnDs.AcceptChanges();
            return returnDs;
        }

        /// <summary>
        /// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。
        /// priority 为检索优先级,用英文缩词表示，支持为空，例如：ACT:满幅率-云量-时间；CTA:云量-时间-满幅率；TAC:时间-满幅率-云量 等
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="coordsStr"></param>
        /// <param name="tileLevel"></param>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="othercondition"></param>
        /// <param name="Mincloud"></param>
        /// <param name="Maxcloud"></param>
        /// <param name="Minavailability"></param>
        /// <param name="Maxavailability"></param>
        /// <param name="priority"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        public System.Data.DataSet SearTileBatchAllTypeAddCloudAvail_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority, bool needTilePath = false)
        {
            //--构建多边形

            //获取多边形最大最小经纬度
            double[] mmll = GridGeneration.GetMinMaxLatLonFormCoordsStr(coordsStr);


            //获取最大最小行列号
            int[] mmrc = DirectlyAddressing.GetRowAndColum(new string[] { mmll[0].ToString(), mmll[1].ToString(), mmll[2].ToString(), mmll[3].ToString() }, tileLevel);

            //构建SQL语句
            /*
            select *, count(distinct Date) from (select * from correctedTiles t1 where type='Preview' and (Row>=511 and Row=<598 and Col>=1184 and Col=<1822 and Level=8)) and (select count(*) from correctedTiles t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.type=t1.type and t2.Date>t1.Date) <1)  group by Date,type,level,row,col 
            */

            string sqlrowscols = string.Format("Row>={0} and Row<={1} and Col>={2} and Col<={3} and Level={4}", mmrc[0], mmrc[2], mmrc[1], mmrc[3], tileLevel);
            string sqlcloudAndAvailability = null;
            sqlcloudAndAvailability += (Mincloud != "") ? "  and Cloud >= " + Mincloud : "  and Cloud >= 0";
            sqlcloudAndAvailability += (Maxcloud != "") ? "  and Cloud <= " + Maxcloud : "  and Cloud <= 255";
            sqlcloudAndAvailability += (Minavailability != "") ? "  and Availability >= " + Minavailability : "  and Availability >= 0";
            sqlcloudAndAvailability += (Maxavailability != "") ? "  and Availability <= " + Maxavailability : "  and Availability <= 255";

            //if (cloud == "" && availability == "")
            //{
            //    sqlcloudAndAvailability = string.Format(" and Cloud <= 255 and Availability <= 255 ");
            //}
            //else if (cloud == "" && availability != "")
            //{
            //    sqlcloudAndAvailability = string.Format(" and Cloud <= 255 and Availability <= {0} ", availability);
            //}
            //else if (cloud != ""&& availability == "")
            //{
            //    sqlcloudAndAvailability = string.Format(" and Cloud <= {0} and Availability <= 255 ",cloud);  
            //}
            //else
            //{
            //    sqlcloudAndAvailability = string.Format(" and Cloud <= {0} and Availability <= {1} ", cloud, availability);
            //}
            string sqlSatSensor = " ";
            string typestr = "type";
            if (tablename == "correctedTiles")
            {
                if (sat != "")
                {
                    sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
                }
                if (sensor != "")
                {
                    sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));      //'_'是SQL通配符，代表任意单字符，比如WFV_={WFV1,WFV2,WFV3,WFV4}
                }
                typestr = "type";

                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }
            else if (tablename == "productTiles")
            {
                typestr = "ProdType";
            }


            //原按照最新日期的查询条件存在问题，存在相同日期不同数据源的多个数据，需要移除
            //string sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr);
            string sql;
            //所有的排序都是按照满幅率降序（取最大），时间降序（取最新），云量升序（取最小）的顺序进行筛选的
            //其中按照满幅率优先筛选的时候，要优先考虑非255的，即如果行列号为（582，1183）的所有瓦片数据，既有云量，满幅度（255，255）的也有非（255，255）的，则筛选的时候，会取非（255.255）的。
            switch (priority)
            {
                case "默认":
                    sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} {5}and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;
                case "时间-云量-满幅率":
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3} {5}) ORDER BY  Date ASC , Cloud DESC, Availability ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;
                case "时间-满幅率-云量"://时间-满幅率-云量
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3} {5}) ORDER BY  Date ASC ,Availability ASC , Cloud DESC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;
                case "云量-时间-满幅率"://云量-时间-满幅率
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3} {5}) ORDER BY Cloud DESC, Date ASC , Availability ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;
                case "云量-满幅率-时间":
                    sql = string.Format("SELECT * FROM (SELECT * FROM (SELECT * FROM {0} t1 WHERE {1}{2} {3} {5}) ORDER BY  Cloud DESC ,Availability ASC , Date ASC) GROUP BY {4},level,row,col ORDER BY ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;
                case "满幅率-时间-云量":
                    sql = string.Format("SELECT * FROM (SELECT  * FROM (SELECT *  FROM (SELECT *  FROM ( SELECT * FROM {0} t1 WHERE {1}{2} {3} {5} AND cloud <> 255 AND availability <> 255) ORDER BY Availability ASC , date ASC , cloud DESC ) GROUP BY row ,col ,level ,{4}  UNION SELECT * FROM (SELECT  *  FROM ( SELECT  * FROM {0} t1 WHERE  {1}{2} {3} {5} AND availability = 255 AND cloud = 255 ) ORDER BY availability ASC , date ASC , cloud DESC ) GROUP BY  row , col , level , {4} ) ORDER BY availability DESC , date DESC , cloud ASC ) GROUP BY row , col ,level ,{4} ", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;
                case "满幅率-云量-时间":
                    sql = string.Format("SELECT * FROM (SELECT  * FROM (SELECT  * FROM (SELECT * FROM (SELECT  * FROM {0} t1 WHERE {1}{2} {3} {5} AND cloud <> 255 AND availability <> 255) ORDER BY Availability ASC ,cloud DESC , date ASC) GROUP BY row , col , level ,{4} UNION SELECT * FROM (SELECT  * FROM ( SELECT  * FROM {0} t1 WHERE {1}{2} {3} {5} AND availability = 255 AND cloud = 255 ) ORDER BY Availability ASC ,cloud DESC , date ASC ) GROUP BY row , col , level , {4}) ORDER BY availability DESC ,cloud ASC , date DESC ) GROUP BY row , col, level,{4}", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;

                default:
                    sql = string.Format("select *, count(distinct Date) from (select * from {0} t1 where {1}{2} {3} {5} and (select count(*) from {0} t2 where t2.Level=t1.Level and t2.Row=t1.Row and t2.Col=t1.Col and t2.{4}=t1.{4} and t2.Date>t1.Date)<1) group by Date,{4},level,row,col order by ID", tablename, sqlrowscols, sqlSatSensor, othercondition, typestr, sqlcloudAndAvailability);
                    break;
            }

            //分布式查询，传入SQL和多边形


            //不分页检索
            WebServiceUtil BaseUtil = new WebServiceUtil();


            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {

                //针对站点奔溃影响检索效率问题的解决方案：          joki170520
                if (!QRST_DI_TS_Process.Site.TServerSiteManager.optimalStorageSiteIPs.Contains(IP))
                {
                    Console.WriteLine("站点" + IP + "无法访问");
                    continue;
                }

                List<string> sqlCoordsIpTilepath = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                sqlCoordsIpTilepath.Add(sql);

                sqlCoordsIpTilepath.Add(coordsStr);

                sqlCoordsIpTilepath.Add(IP);

                sqlCoordsIpTilepath.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll_CoordsFilter((List<string>)o), sqlCoordsIpTilepath);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks, 15000);

            System.Data.DataSet returnDs;
            returnDs = BaseUtil.MergeAllDataSet();

            if (returnDs.Tables.Count != 0 && returnDs.Tables[0].Columns.Contains("count(distinct Date)"))
            {
                returnDs.Tables[0].Columns.Remove("count(distinct Date)");
            }

            //if (isDataSyn)
            //{
            SQLBase.AddData_TileNameAddress(returnDs);
            //}

            int allrecordNum = 0;
            if (returnDs.Tables.Count != 0)
            {
                allrecordNum = returnDs.Tables[0].Rows.Count;
                DataRow dr = returnDs.Tables[0].NewRow();

                dr["TileFileName"] = allrecordNum;
                //ds.Tables[0].Rows.Add(dr);
                returnDs.Tables[0].Rows.InsertAt(dr, 0);


                if (needTilePath)
                {
                    List<string> tilePath = new List<string>();
                    DataTable table = returnDs.Tables[0];
                    table.Columns.Add("TileFilePath", typeof(string));
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string destPath = BaseUtil._DAUtil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                        table.Rows[i]["TileFilePath"] = destPath;
                    }
                }
            }
            returnDs.AcceptChanges();
            return returnDs;
        }

        ///批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开,cloud,availability 分别为云量和满幅率，不能为空")]
        public System.Data.DataSet SearImgTileCountBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
        {
            #region 现方法，通过多边形的最大最小经纬度范围，计算出最大最小行列号，通过比较行列号大小进行检索
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            //Mincloud = "0";
            // Maxavailability = "100";
            //Maxcloud = "100";
            //Minavailability = "0";
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }
            string sqlcloudAndAvailability = "";
            sqlcloudAndAvailability += (Mincloud != "") ? "  and Cloud >= " + Mincloud : "  and Cloud >= 0";
            sqlcloudAndAvailability += (Maxcloud != "") ? "  and Cloud <= " + Maxcloud : "  and Cloud <= 255";
            sqlcloudAndAvailability += (Minavailability != "") ? "  and Availability >= " + Minavailability : "  and Availability >= 0";
            sqlcloudAndAvailability += (Maxavailability != "") ? "  and Availability <= " + Maxavailability : "  and Availability <= 255";

            return SearTileCountBatchAllTypeAddCloudAvai_coordsStr("correctedTiles", coordsStr, tileLevel, sat, sensor, "and type='Preview'" + timecondition, sqlcloudAndAvailability);
            #endregion
        }
        
        ///批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
        public System.Data.DataSet SearTileCountBatchAllTypeAddCloudAvai_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition, string sqlcloudAndAvailability)
        {

            //获取多边形最大最小经纬度
            double[] mmll = GridGeneration.GetMinMaxLatLonFormCoordsStr(coordsStr);


            //获取最大最小行列号
            int[] mmrc = DirectlyAddressing.GetRowAndColum(new string[] { mmll[0].ToString(), mmll[1].ToString(), mmll[2].ToString(), mmll[3].ToString() }, tileLevel);

            //构建SQL语句
            string sqlrowscols = string.Format("Row>={0} and Row<={1} and Col>={2} and Col<={3} and Level={4}", mmrc[0], mmrc[2], mmrc[1], mmrc[3], tileLevel);

            string sqlSatSensor = "";
            if (tablename == "correctedTiles")
            {
                if (sat != "")
                {
                    sqlSatSensor += string.Format(" and (Satellite in ('{0}'))", sat.Replace(",", "','"));
                }
                if (sensor != "")
                {
                    sqlSatSensor += string.Format(" and (Sensor in ('{0}'))", sensor.Replace(",", "','"));      //'_'是SQL通配符，代表任意单字符，比如WFV_={WFV1,WFV2,WFV3,WFV4}
                }

                //将1、2、3、4、preview不全的记录过滤，一个gff保留一条记录
                tablename = "gff";
            }

            //SQL语句构建
            string sql = string.Format("Select count(*) as Count,[Level],[Row],[Col] From {0} where {1}{2} {3} {4} group by [Level],[Row], [Col]", tablename, sqlrowscols, sqlSatSensor, othercondition, sqlcloudAndAvailability);


            //不分页检索
            WebServiceUtil BaseUtil = new WebServiceUtil();

            List<Task> tasks = new List<Task>();
            string[] IPcol = BaseUtil.GetIPFromMySql();
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string IP in IPcol)
            {
                //针对站点奔溃影响检索效率问题的解决方案：          joki170520
                if (!QRST_DI_TS_Process.Site.TServerSiteManager.optimalStorageSiteIPs.Contains(IP))
                {
                    Console.WriteLine("站点" + IP + "无法访问");
                    continue;
                }

                List<string> sqlCoordsIpTilepath = new List<string>();
                string tilepath = BaseUtil.GetCommonSharePathBaseIP(IP);

                sqlCoordsIpTilepath.Add(sql);

                sqlCoordsIpTilepath.Add(coordsStr);

                sqlCoordsIpTilepath.Add(IP);

                sqlCoordsIpTilepath.Add(tilepath);

                Task t = new Task(o => BaseUtil.GetDataSetColAll_CoordsFilter((List<string>)o), sqlCoordsIpTilepath);
                t.Start();
                tasks.Add(t);
            }

            WaitingForTasks(tasks);

            System.Data.DataSet returnDs;
            returnDs = BaseUtil.MergeAllDataSet();

            //if (returnDs.Tables.Count != 0)
            //{
            //    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaanBatch 返回:System.Data.DataSet不为空");
            //}
            //else
            //    InforLog<string>.inforLog.Info("文件:WS_QDB_Searcher_Sqlite/App_Code/Service.cs 方法：SearTileYaanBatch 返回:System.Data.DataSet为空");
            //return tilePath;
            returnDs.AcceptChanges();
            return returnDs;
        }

        ///"批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        ///参数说明：行号，列号分别为一维数组，并且行列号一一对应,cloud,availability 分别为云量和满幅率。"
        public System.Data.DataSet SearProdTileCountBatchAddCloudAvail_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
        {
            //Mincloud = "0";
            //Maxavailability = "100";
            //Maxcloud = "100";
            //Minavailability = "0";
            #region 现方法，通过多边形的最大最小经纬度范围，计算出最大最小行列号，通过比较行列号大小进行检索
            //string timecondition = "";
            //timecondition += (startdate > 10000000 && startdate < 29999999) ? " and Date >= " + startdate : "";
            //timecondition += (enddate > 10000000 && enddate < 29999999) ? " and Date <= " + enddate : "";
            // 修改由于瓦片重命名后，时间就增加了2位了，由原来的8位变成了10位
            string timecondition = "";
            string startstr = startdate.ToString();
            string endstr = enddate.ToString();
            if (startstr.Length == 8 && endstr.Length == 8)
            {
                timecondition += (startdate * 100 > 1000000000 && startdate * 100 < 2147483624) ? " and Date >= " + (startdate * 100) : "";
                timecondition += ((enddate * 100 + 24) > 1000000000 && (enddate * 100 + 24) < 2147483624) ? " and Date <= " + (enddate * 100 + 24) : "";
            }
            else
            {
                timecondition += (startdate > 1000000000 && startdate < 2147483624) ? " and Date >= " + startdate : "";
                timecondition += (enddate > 1000000000 && enddate < 2147483624) ? " and Date <= " + enddate : "";

            }
            string sqlcloudAndAvailability = "";

            sqlcloudAndAvailability += (Mincloud != "") ? "  and Cloud >= " + Mincloud : "  and Cloud >= 0";
            sqlcloudAndAvailability += (Maxcloud != "") ? "  and Cloud <= " + Maxcloud : "  and Cloud <= 255";
            sqlcloudAndAvailability += (Minavailability != "") ? "  and Availability >= " + Minavailability : "  and Availability >= 0";
            sqlcloudAndAvailability += (Maxavailability != "") ? "  and Availability <= " + Maxavailability : "  and Availability <= 255";

            return SearTileCountBatchAllTypeAddCloudAvai_coordsStr("productTiles", coordsStr, tileLevel, "", "", string.Format("and ProdType like '{0}'{1}", prodType, timecondition), sqlcloudAndAvailability);
            #endregion
        }

        

        #endregion
    }
}

