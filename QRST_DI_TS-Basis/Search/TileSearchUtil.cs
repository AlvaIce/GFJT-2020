using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_Resources; 
using DotSpatial.Topology;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Basis.Search
{
    public class TileSearchUtil
    {
        public TileSearchUtil()
        {
            _dbBaseUtilities = Constant.ITileDbUtilities;
        }

        #region 切片远程检索 tile Searching
        /// <summary>
        /// 分布式切片检索
        /// </summary>

        List<DataSet> DSCol;        //分布式并发检索结果子集
        List<ModIDSearchInfo> ModsList;   //DLF 20130428添加
        public static DateTime Transdt1;
        private IDbBaseUtilities _dbBaseUtilities;
        DirectlyAddressing _DAUtil = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        /// <summary>
        /// 查找目标路径下的全部配号索引文件
        /// </summary>
        /// <param name="commonSharePath">\\172.16.0.78\QRST_DB_Prototype\</param>
        /// <returns></returns>
        private List<string> getSqlitedbfilepath(string commonSharePath)
        {
            List<string> Sqlitedbfilepath = new List<string>();

            if (commonSharePath.EndsWith(".db") && File.Exists(commonSharePath))
            {
                //如果传入的是\\172.16.0.78\QRST_DB_Prototype\QRST_DB_Tile\1\QDB_IDX_1.db
                Sqlitedbfilepath.Add(commonSharePath);
            }
            else
            {
                if (!commonSharePath.EndsWith(@"\"))
                {
                    commonSharePath = commonSharePath + @"\";
                }
                string ip = commonSharePath.Substring(2, commonSharePath.IndexOf(@"\QRST_DB_Prototype") - 2);
                List<string> mods = _DAUtil.GetModArrByIP(ip);
                foreach (string modstr in mods)
                {
                    string modpath = (modstr == "Failed") ? "FailedTile" : modstr;
                    string curtilepath = null;
                    //增加单机地址  jianghua
                    switch (Constant.DbStorage)
                    {
                        case EnumDbStorage.MULTIPLE:
                            curtilepath = string.Format(@"{0}QRST_DB_Tile\{1}\QDB_IDX_{2}.db", commonSharePath, modpath, modstr);
                            break;
                        case EnumDbStorage.SINGLE:
                            curtilepath = string.Format(@"{0}\QRST_DB_Tile\{1}\QDB_IDX_{2}.db", Constant.PcDBRootPath, modpath, modstr);
                            break;
                     
                    }
                    if (File.Exists(curtilepath))
                    {
                        Sqlitedbfilepath.Add(curtilepath);
                    }
                }
            }
            return Sqlitedbfilepath;
        }

        /// <summary>
        /// 查询得到每个配号下的DataSet，不分页返回全部结果。适用于结果不多的情况，如查询所有数据库中的  一共所包含的切片等级。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<System.Data.DataSet> GetDataSetCol_CoordsFilter(string sql, string coordsStr, string tilepath)
        {
            List<System.Data.DataSet> dss = GetDataSetCol(sql, tilepath);

            #region 用多边形过滤结果列表

            if (dss!=null)
            {
                IList<Coordinate> coords = GridGeneration.GetCoordsFormStr(coordsStr);
                DotSpatial.Data.IFeature feature = GridGeneration.GetFeatureFromCoords(coords);

                foreach (System.Data.DataSet ds in dss)
                {
                    GridGeneration.TilesFilter(feature, ds);
                }

            }
            #endregion

            return dss;

        }

        /// <summary>
        /// 查询得到每个配号下的DataSet，不分页返回全部结果。适用于结果不多的情况，如查询所有数据库中的  一共所包含的切片等级。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetCol(string sql, string tilepath)
        {
            int recordNum;
            ModsList = null;    //不分页
            List<string> Sqlitedbfilepath = getSqlitedbfilepath(tilepath);

            List<DataSet> ds1 = MultiTasksUtil(Sqlitedbfilepath, sql, out recordNum);

            return ds1;

        }
        /// <summary>
        /// 由于分布式检索无法采用Limit方式检索，因此本方法检索合并全部结果，在结果列表里截取当前分页内容，效率低不建议使用
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetCol(string sql, int pageIndex, out int recordNum, string tilepath, int pageSize)
        {
            recordNum = 0;
            
            //检索并合并全部结果
            List<DataSet> ds1 = GetDataSetCol(sql,tilepath);

            //在结果列表里截取当前分页内容
            List<DataSet> PageDataSetCol = new List<DataSet>();
            foreach (DataSet item in ds1)
            {
                DataSet pageds = new DataSet();
                if (item.Tables[0].Rows.Count > pageSize)
                {
                    pageds = SplitDataSet(item, pageSize, pageIndex);//分页读取
                }
                else
                    //pageds = SplitDataSet(item, item.Tables[0].Rows.Count, 0);
                    pageds = item;
                PageDataSetCol.Add(pageds);
            }

            return PageDataSetCol;

        }

        public void UpdateGFFTable(string commsharepath)
        {
            List<string> Sqlitedbfilepath = getSqlitedbfilepath(commsharepath);
            //SqliteBaseUtilities sqliteutil = new SqliteBaseUtilities();
            foreach (string dbfile in Sqlitedbfilepath)
            {
                _dbBaseUtilities.UpdateTableGFF(dbfile);
            }
        }

        public void UpdateDistinctTables(string commsharepath)
        {
            List<string> Sqlitedbfilepath = getSqlitedbfilepath(commsharepath);
            //SqliteBaseUtilities sqliteutil = new SqliteBaseUtilities();
            foreach (string dbfile in Sqlitedbfilepath)
            {
                _dbBaseUtilities.UpdateDistinctTables(dbfile);
            }
        }

		public void ExecuteNonQuery(string sql, string tilepath)
		{
            List<string> Sqlitedbfilepath = getSqlitedbfilepath(tilepath);

			MultiTasksExecute(Sqlitedbfilepath, sql);

		}
        /// <summary>
        ///分步骤，第一步检索各配号下的数据记录数；第二步根据每个配号下的记录条数和页大小、页索引，计算每个配号下应查询的记录索引及数目；分页查询得到每个配号下的DataSet，返回列表。DLF 20130428
        ///tilepath仅支持单一站点下多配号并行检索
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetColPaged(string sql, int startIndex, out int recordNum, string tilepath, int offset)
        {
            recordNum = 0;
            //DLF 20130428
            ModsList = new List<ModIDSearchInfo>();

            DateTime dt1 = DateTime.Now;
            List<string> Sqlitedbfilepath = getSqlitedbfilepath(tilepath);

            foreach (string sqlitedbf in Sqlitedbfilepath)//sqlitedbf为\\172.16.0.78\QRST_DB_Prototype\QRST_DB_Tile\1\QDB_IDX_1.db
            {
                ModIDSearchInfo mod = new ModIDSearchInfo(Path.GetFileNameWithoutExtension(sqlitedbf).Substring("QDB_IDX_".Length), sqlitedbf);
                ModsList.Add(mod);
            }
            DateTime dt2 = DateTime.Now;
            TimeSpan dt = dt2 - dt1;

            //List<DataSet> ds1 = 
            //在每个配号下执行查询结果数目的sql语句，得到每个配号下的结果数目，填充ModsList中每个ModIDInfo的ModRecordsCount属性。
            MultiTasksUtil_GetRecordCount(Sqlitedbfilepath, sql);

            //根据每个配号下的记录条数和页大小、页索引，计算每个配号下应查询的记录索引及数目
            PagedSearchTool.GetPageInfo(startIndex, ModsList, offset);
            recordNum = PagedSearchTool.SumModsRecordsCount(ModsList);      //ModsList里记录了每个Mod的分配查询记录数

            int recordNumThisPage = 0;
            List<DataSet> dsModIDs = MultiTasksUtil(Sqlitedbfilepath, sql, out recordNumThisPage);
           
            Transdt1 = DateTime.Now;
            return dsModIDs;

        }

        /// <summary>
        /// 根据已分配好的的查询方案，按统一SQL分布式查询分页信息，返回查询结果
        /// siteModsList具备多站点多配号检索能力
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="siteModsList"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetColPaged2(string sql, out int recordNum, List<ModIDSearchInfo> siteModsList)
        {
            recordNum = 0;
            List<string> Sqlitedbfilepath = new List<string>();
            this.ModsList = siteModsList;

            recordNum = PagedSearchTool.SumModsRecordsCount(ModsList);

            foreach (ModIDSearchInfo modinfo in siteModsList)
            {
                if (modinfo.modPageInfo.recordNumber != 0)
                {
                    Sqlitedbfilepath.Add(modinfo.ModDbFilePath);
                }
            }

            int recordNumThisPage = 0;
            List<DataSet> dsModIDs = MultiTasksUtil(Sqlitedbfilepath, sql, out recordNumThisPage);
            Transdt1 = DateTime.Now;
            return dsModIDs;

        }
        /// <summary>
        /// 根据已分配好的的查询方案，按各自的SQL分布式查询分页信息，返回查询结果
        /// siteModsList具备多站点多配号检索能力
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="siteModsList"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetColPaged2(out int recordNum, List<ModIDSearchInfo> siteModsList)
        {
            recordNum = 0;
            List<string> Sqlitedbfilepath = new List<string>();
            List<string> FilterSql = new List<string>();
            this.ModsList = siteModsList;

            recordNum = PagedSearchTool.SumModsRecordsCount(ModsList);

            foreach (ModIDSearchInfo modinfo in siteModsList)
            {
                if (modinfo.modPageInfo.recordNumber != 0)
                {
                    Sqlitedbfilepath.Add(modinfo.ModDbFilePath);
                    FilterSql.Add(modinfo.ModSql);
                }
            }

            int recordNumThisPage = 0;
            List<DataSet> dsModIDs = MultiTasksUtil(Sqlitedbfilepath, FilterSql, out recordNumThisPage);
          
            Transdt1 = DateTime.Now;
            return dsModIDs;

        }
        /// <summary>
        /// 查询每个配号下得结果信息，以汇总到网站上
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        public List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string sql, out int recordNum, string tilepath)
        {
            recordNum = 0;
            //DLF 20130428
            ModsList = new List<ModIDSearchInfo>();

            List<string> Sqlitedbfilepath = getSqlitedbfilepath(tilepath);

            foreach (string sqlitedbf in Sqlitedbfilepath)//sqlitedbf为\\172.16.0.78\QRST_DB_Prototype\QRST_DB_Tile\1\QDB_IDX_1.db
            {
                ModIDSearchInfo mod = new ModIDSearchInfo(Path.GetFileNameWithoutExtension(sqlitedbf).Substring("QDB_IDX_".Length), sqlitedbf);
                ModsList.Add(mod);
            }


            //List<DataSet> ds1 = 
            //在每个配号下执行查询结果数目的sql语句，得到每个配号下的结果数目，填充ModsList中每个ModIDInfo的ModRecordsCount属性。
            MultiTasksUtil_GetRecordCount(Sqlitedbfilepath, sql);

            ////根据每个配号下的记录条数和页大小、页索引，计算每个配号下应查询的记录索引及数目
            //PagedSearchTool.GetPageInfo(startIndex, ModsList, offset);
            recordNum = PagedSearchTool.SumModsRecordsCount(ModsList);

            return ModsList;

        }

        /// <summary>
        /// 过滤查询每个配号下得结果信息，以汇总到网站上
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        public List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string type, string sql, out int recordNum, string tilepath, DotSpatial.Data.IFeature iFeature)
        {
            recordNum = 0;
            ModsList = new List<ModIDSearchInfo>();

            DateTime dt1 = DateTime.Now;

            List<string> Sqlitedbfilepath = getSqlitedbfilepath(tilepath);

            foreach (string sqlitedbf in Sqlitedbfilepath)//sqlitedbf为\\172.16.0.78\QRST_DB_Prototype\QRST_DB_Tile\1\QDB_IDX_1.db
            {
                ModIDSearchInfo mod = new ModIDSearchInfo(Path.GetFileNameWithoutExtension(sqlitedbf).Substring("QDB_IDX_".Length), sqlitedbf);
                ModsList.Add(mod);
            }
            DateTime dt2 = DateTime.Now;
            TimeSpan dt = dt2 - dt1;

            //List<DataSet> ds1 = 
            //在每个配号下执行查询结果数目的sql语句，得到每个配号下的结果数目，填充ModsList中每个ModIDInfo的ModRecordsCount属性。
            MultiTasksUtil_GetRecordCount(Sqlitedbfilepath, sql, iFeature, type);

            ////根据每个配号下的记录条数和页大小、页索引，计算每个配号下应查询的记录索引及数目
            //PagedSearchTool.GetPageInfo(startIndex, ModsList, offset);
            recordNum = PagedSearchTool.SumModsRecordsCount(ModsList);

            //int recordNumThisPage = 0;
            //List<DataSet> dsModIDs = TaskUtil(Sqlitedbfilepath, sql, out recordNumThisPage);
            ////List<DataSet> ds1 = TaskUtilWithoneTask(Sqlitedbfilepath, sql);
            //List<DataSet> PageDataSetCol = new List<DataSet>();
            //foreach (DataSet item in ds1)
            //{
            //    DataSet pageds = new DataSet();
            //    if (item.Tables[0].Rows.Count > pageSize)
            //    {
            //        pageds = SplitDataSet(item, pageSize, pageIndex);//分页读取
            //    }
            //    else
            //        //pageds = SplitDataSet(item, item.Tables[0].Rows.Count, 0);
            //        pageds = item;
            //    PageDataSetCol.Add(pageds);
            //}
            //Transdt1 = DateTime.Now;
            return ModsList;

        }
        /// <summary>
        /// DataSet 分页
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="pageSize">一页记录条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public static DataSet SplitDataSet(DataSet ds, int pageSize, int pageIndex)
        {
            DataSet vds = new DataSet();
            vds = ds.Clone();
            int fromIndex = pageSize * (pageIndex - 1);
            int toIndex = pageSize * pageIndex - 1;

            //DLF 20130425 添加为空处理
            if (ds == null || ds.Tables.Count == 0)
            {
                return vds;
            }

            for (int i = fromIndex; i <= toIndex; i++)
            {
                if (i >= ds.Tables[0].Rows.Count)
                    break;
                vds.Tables[0].ImportRow(ds.Tables[0].Rows[i]);
            }
            ds.Dispose();
            return vds;
        }

        /*
        /// <summary>
        /// 单线程 每台站点机只开一个线程
        /// </summary>
        /// <param name="dbpathCol"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<DataSet> TaskUtilWithoneTask(List<string> dbpathCol, string sql)
        {
            DSCol = new List<DataSet>();
            List<Task> TaskCol = new List<Task>();
            DateTime dtTask1 = new DateTime();
            dtTask1 = DateTime.Now;


            List<string> pm = new List<string>();
            pm.Add(dbpathCol[0]);
            pm.Add(sql);
            DataSet ds = TaskUtilWithoneTask(pm);
            DSCol.Add(ds);
            DateTime dtTask2 = DateTime.Now;
            TimeSpan dttotal = dtTask2 - dtTask1;
            int recordNum = 0;

            recordNum = ds.Tables[0].Rows.Count;


            Console.WriteLine("总记录条数:{0}", recordNum);
            return DSCol;

        }
        /// <summary>
        /// 单线程
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet TaskUtilWithoneTask(List<string> param)
        {
            DataSet ds = new DataSet();
            string dbpath = param[0];
            string sql = param[1];
            try
            {
                //Console.WriteLine("任务开始：" + dbpath);
                SqliteBaseUtilities Operator2 = new SqliteBaseUtilities();



                ds = Operator2.GetDataSet(sql + "order by ID", dbpath);
                //Console.WriteLine("任务内容：" + ds.Tables[0].Rows.Count.ToString());
            }

            catch (Exception ex)
            {
                Console.WriteLine("任务出错：" + ex.Message);
            }
            return ds;
        }*/

        /// <summary>
        /// 多任务执行SQLite查询,查询得到每个配号下符合要求的记录数目
        /// </summary>
        /// <param name="dbpathCol"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public void MultiTasksUtil_GetRecordCount(List<string> dbpathCol, string sql)
        {
            string sqlGetCount = ConvertGetCountSQL(sql);
            DSCol = new List<DataSet>();
            //recordNum = 0;
            List<Task> TaskCol = new List<Task>();
            DateTime dtTask1 = new DateTime();
            dtTask1 = DateTime.Now;
            //int count = System.IO.Directory.GetDirectories(path).Length;
            int count = dbpathCol.Count;
            if (count != 0)
            {
                //int Taskcount = 255 - Convert.ToInt32(this.Taskcount_txb.Text.ToString());
                for (int i = 0; i < count; i++)//暂时只测试4个线程
                {

                    List<string> pm = new List<string>();
                    pm.Add(dbpathCol[i]);
                    pm.Add(sqlGetCount);
                    Task t = new Task(o => TaskUtil_GetCount((List<string>)o), pm);//(((List<string>)o), pm));
                    TaskCol.Add(t);

                    t.Start();
                }
                foreach (Task task in TaskCol)
                {
                    task.Wait();
                }
            }

        }
        /// <summary>
        /// 多任务执行SQLite过滤查询,查询得到每个配号下符合要求的记录数目
        /// </summary>
        /// <param name="dbpathCol"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public void MultiTasksUtil_GetRecordCount(List<string> dbpathCol, string sql, DotSpatial.Data.IFeature iFeature, string type)
        {
            string sqlGetCount = ConvertGetCountSQL(sql);
            DSCol = new List<DataSet>();
            //recordNum = 0;
            List<Task> TaskCol = new List<Task>();
            DateTime dtTask1 = new DateTime();
            dtTask1 = DateTime.Now;
            int count = dbpathCol.Count;
          
            if (count != 0)
            {
                for (int i = 0; i < count; i++)//暂时只测试4个线程
                {
                    List<string> pm = new List<string>();
                    pm.Add(type);
                    pm.Add(dbpathCol[i]);
                    pm.Add(sqlGetCount);
                    Dictionary<List<string>, DotSpatial.Data.IFeature> dic = new Dictionary<List<string>, DotSpatial.Data.IFeature>();
                    dic.Add(pm,iFeature);
                    Task t = new Task((o) => TaskUtil_GetTileCount((Dictionary<List<string>, DotSpatial.Data.IFeature>)o), dic);//(((List<string>)o), pm));
                    TaskCol.Add(t);

                    t.Start();
                }
                foreach (Task task in TaskCol)
                {
                    task.Wait();

                }
            }
        }

        /// <summary>
        /// 把查询结果的sql语句转化为查询结果数目的sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ConvertGetCountSQL(string sql)
        {
            return string.Format("select count(*) FROM {0}", sql.Substring(sql.ToUpper().IndexOf("FROM") + 4));
        }

        /// <summary>
        /// 多任务执行SQLite查询
        /// </summary>
        /// <param name="dbpathCol"></param>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <returns></returns>
        public List<DataSet> MultiTasksUtil(List<string> dbpathCol, string sql, out int recordNum)
        {
            return MultiTasksUtil(dbpathCol, new List<string>() { sql }, out recordNum, true);
        }
        /// <summary>
        /// 多任务执行SQLite查询
        /// </summary>
        /// <param name="dbpathCol"></param>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <returns></returns>
        public List<DataSet> MultiTasksUtil(List<string> dbpathCol, List<string> FilterSql, out int recordNum, bool onlySql = false)
        {
            DSCol = new List<DataSet>();
            recordNum = 0;
            List<Task> TaskCol = new List<Task>();
            DateTime dtTask1 = new DateTime();
            dtTask1 = DateTime.Now;
            int count = dbpathCol.Count;
            if (count != 0)
            {
                for (int i = 0; i < count; i++)//暂时只测试4个线程
                {
                    List<string> pm = new List<string>();
                    pm.Add(dbpathCol[i]);
                    if (onlySql)
                    {
                        pm.Add(FilterSql[0]);
                    }
                    else
                    {
                        pm.Add(FilterSql[i]);
                    }
                    string limitstr = GetLimitstrByModDBPath(dbpathCol[i]);
                    pm.Add(limitstr);
                    //第三个参数是限制查询的记录起始索引和数目
                    Task t = new Task(o => TaskUtil((List<string>)o), pm);
                    TaskCol.Add(t);

                    t.Start();
                }
                foreach (Task task in TaskCol)
                {
                    task.Wait();

                    TaskStatus st = task.Status;

                }
            }
            else
            {
                return null;
            }
            DateTime dtTask2 = DateTime.Now;
            TimeSpan dttotal = dtTask2 - dtTask1;

            foreach (DataSet item in DSCol)
            {
                if (item.Tables.Count>0)
                {
                    recordNum = recordNum + item.Tables[0].Rows.Count;
                }
            }

            MyConsole.WriteLine(string.Format("总记录条数:{0},耗时:{1:0.###}秒", recordNum.ToString(), dttotal.TotalSeconds.ToString()));
            return DSCol;

        }

		public void MultiTasksExecute(List<string> dbpathCol, string sql)
		{
			DSCol = new List<DataSet>();
			List<Task> TaskCol = new List<Task>();
			DateTime dtTask1 = new DateTime();
			dtTask1 = DateTime.Now;
			//int count = System.IO.Directory.GetDirectories(path).Length;
			int count = dbpathCol.Count;
			if (count != 0)
			{
				//int Taskcount = 255 - Convert.ToInt32(this.Taskcount_txb.Text.ToString());
				for (int i = 0; i < count; i++)//暂时只测试4个线程
				{

					List<string> pm = new List<string>();
					pm.Add(dbpathCol[i]);
					pm.Add(sql);

					Task t = new Task(o => TaskUtil_Execute((List<string>)o), pm);//(((List<string>)o), pm));
					TaskCol.Add(t);

					t.Start();
				}
				foreach (Task task in TaskCol)
				{
					task.Wait();

					TaskStatus st = task.Status;

				}
			}
			else
			{
			}
			//Task.WaitAll(TaskCol.ToArray());
		}
        /// <summary>
        /// 根据ModDBPath找到对应的ModIDInfo对象，找到他的PageInfo信息，并组装成SQL语句中的Limit部分
        /// </summary>
        /// <param name="strModDBFilePath">配号下数据库文件路径</param>
        /// <returns></returns>
        private string GetLimitstrByModDBPath(string strModDBFilePath)
        {
            string limitStr = string.Empty;
            //在List<ModIDInfo> ModsList中根据ModIDInfo中的配号数据库文件路径ModDbFilePath得到 该配号下数据库应返回记录的索引信息modPageInfo
            PagedInfo pageInfo = GetPageInfoByModDBPath(strModDBFilePath);
            if (pageInfo != null)
            {
                limitStr = string.Format("limit {0},{1} ", pageInfo.recordStartIndex, pageInfo.recordNumber);
            }
            return limitStr;
        }
        private PagedInfo GetPageInfoByModDBPath(string strModDBFilePath)
        {
            PagedInfo page = null;
            if (ModsList == null)
            {
                return null;
            }
            foreach (ModIDSearchInfo mod in ModsList)
            {
                if (mod.ModDbFilePath == strModDBFilePath)
                {
                    page = mod.modPageInfo;
                    //注意获取到page对象即跳出，否则page对象会再次赋值，会出错。
                    break;
                }
                else { page = null; }
            }
            return page;
        }
        /// <summary>
        /// 多线程  DLF 20130428添加
        /// </summary>
        /// <param name="param"></param>
        public void TaskUtil_GetCount(List<string> param)
        {
            string dbpath = param[0];
            string sql = param[1];

            try
            {
                //Console.WriteLine("任务开始：" + dbpath);
                //SqliteBaseUtilities Operator2 = new SqliteBaseUtilities();
                DataSet ds = new DataSet();
                //DLF 20130428 ，去掉“+ "order by ID"”，因为只是查询数目
                ds = _dbBaseUtilities.GetDataSetByPath(sql, dbpath);
                //Console.WriteLine("任务内容：" + ds.Tables[0].Rows.Count.ToString());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //DSCol.Add(ds);
                    int recordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    for (int i = 0; i < ModsList.Count; i++)
                    {
                        if (ModsList[i].ModDbFilePath == dbpath)
                        {
                            ModsList[i].ModRecordsCount = recordCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("任务出错：" + ex.Message);
            }
        }
        private IGeometry getGeomFromRow(DataRow dr)
        {
            string row = dr["Row"].ToString();
            string col = dr["Col"].ToString();
            string lv = dr["Level"].ToString();
            string[] rowAndColum = { row, col };
            List<Coordinate> coords = new List<Coordinate>();

            double lulat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[2].ToString());
            double lulon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[1].ToString());
            double rulat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[2].ToString());
            double rulon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[3].ToString());
            double rdlat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[0].ToString());
            double rdlon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[3].ToString());
            double ldlat = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[0].ToString());
            double ldlon = Convert.ToDouble(DirectlyAddressing.GetLatAndLong(rowAndColum, lv)[1].ToString());

            coords.Add(new Coordinate(lulon, lulat));
            coords.Add(new Coordinate(rulon, rulat));
            coords.Add(new Coordinate(rdlon, rdlat));
            coords.Add(new Coordinate(ldlon, ldlat));
            coords.Add(new Coordinate(lulon, lulat));
            IGeometry poly = new Polygon(coords);
            return poly;

        }
        /// <summary>
        /// 多线程  过滤瓦片数据
        /// </summary>
        /// <param name="param"></param>
        public void TaskUtil_GetTileCount(Dictionary<List<string>,DotSpatial.Data.IFeature> dic)
        {
            List<string> param = new List<string>();
            DotSpatial.Data.IFeature iFeature = null;
            foreach (var item in dic)
            {
                param = item.Key;
                iFeature = item.Value;
            }
            string dbpath = param[1];
            string sql = param[2];
            string newsql = "select Row,Col,Level,count(*)" + sql.Substring(15) + " group by Row,Col,Level";

            try
            {
                //SqliteBaseUtilities Operator2 = new SqliteBaseUtilities();
                DataSet ds = new DataSet();
                ds = _dbBaseUtilities.GetDataSetByPath(newsql, dbpath);
                
                 if (ds != null && ds.Tables.Count > 0)
                 {
                     System.Data.DataTable tab = ds.Tables[0];
                     if (param[0]=="Intersect")
                     {
                         for (int i = tab.Rows.Count - 1; i > -1; i--)
                         {
                             IGeometry poly = getGeomFromRow(tab.Rows[i]);
                             if (!DotSpatial.Data.FeatureExt.Intersects(iFeature, poly))
                             {
                                 tab.Rows.RemoveAt(i);
                             }
                         }
                     }
                     else 
                     {
                         for (int i = tab.Rows.Count - 1; i > -1; i--)
                         {
                             IGeometry poly = getGeomFromRow(tab.Rows[i]);
                             if (!DotSpatial.Data.FeatureExt.Contains(iFeature, poly))
                             {
                                 tab.Rows.RemoveAt(i);
                             }
                         }
                     }
                     //for (int i = tab.Rows.Count - 1; i > -1; i--)
                     //{
                     //    IGeometry poly = getGeomFromRow(tab.Rows[i]);
                     //    if (!DotSpatial.Data.FeatureExt.Intersects(iFeature, poly))
                     //    {
                     //        tab.Rows.RemoveAt(i);
                     //    }
                     //}
                     ds.Tables.Clear();
                     ds.Tables.Add(tab);
                 }
                 if (ds!=null&&ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    string filterSql = "select * FROM  correctedTiles where ((";
                    string otherSql = sql.Substring(sql.LastIndexOf("))")+2);
                    string Levelsql = null;
                    string sql1 = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            if (i < ds.Tables[0].Rows.Count - 1)
                            {
                                Levelsql += string.Format("Level='{0}' and Row={1} and Col={2}", ds.Tables[0].Rows[i][2], ds.Tables[0].Rows[i][0], ds.Tables[0].Rows[i][1]) + ") or (";
                            }
                            else
                            {
                                Levelsql += string.Format("Level='{0}' and Row={1} and Col={2}", ds.Tables[0].Rows[i][2], ds.Tables[0].Rows[i][0], ds.Tables[0].Rows[i][1]);
                            }
                        }
                        else
                        {
                            Levelsql = string.Format("Level='{0}' and Row={1} and Col={2}", ds.Tables[0].Rows[i][2], ds.Tables[0].Rows[i][0], ds.Tables[0].Rows[i][1]) ;
                        }
                        recordCount += Convert.ToInt32(ds.Tables[0].Rows[i][3].ToString());
                    }
                     string finalSql=filterSql+Levelsql+"))"+otherSql;
                     for (int i = 0; i < ModsList.Count; i++)
                     {
                         if (ModsList[i].ModDbFilePath == dbpath)
                         {
                             ModsList[i].ModRecordsCount = recordCount;
                             ModsList[i].ModSql = finalSql;
                         }
                     }
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("任务出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 多线程  DLF 20130428修改
        /// </summary>
        /// <param name="param"></param>
        public void TaskUtil(List<string> param)
        {
            string dbpath = param[0];
            string sql = param[1];
            //DLF 20130428,添加读取第三个参数
            string limitstr = param[2];
            try
            {
                //Console.WriteLine("任务开始：" + dbpath);
                //SqliteBaseUtilities Operator2 = new SqliteBaseUtilities();
                DataSet ds = new DataSet();

                //DLF 20130428 ，添加组装SQL语句的限制条数部分,注意sql语句中的空格
                string newsql = (limitstr == "" || sql.ToLower().Contains("limit") || sql.ToLower().Contains("order")) ? sql : sql + " order by ID " + limitstr;
                MyConsole.WriteLine(String.Format("{0}:{1},{2},{3}", DateTime.Now, "DbServer开始查询：","数据库地址：" +dbpath,"SQL："+ newsql));
                ds = _dbBaseUtilities.GetDataSetByPath(newsql, dbpath);
                MyConsole.WriteLine(String.Format("{0}:{1},{2}", DateTime.Now, "DbServer结束查询：", "数目：" + ds.Tables[0].Rows.Count));
                //Console.WriteLine("任务内容：" + ds.Tables[0].Rows.Count.ToString());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DSCol.Add(ds);
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("任务出错：" + ex.Message);
            }
        }

		public void TaskUtil_Execute(List<string> param)
		{
			string dbpath = param[0];
			string sql = param[1];
			try
			{
                //Console.WriteLine("任务开始：" + dbpath);
                //SqliteBaseUtilities Operator2 = new SqliteBaseUtilities();

                //DLF 20130428 ，添加组装SQL语句的限制条数部分,注意sql语句中的空格
                _dbBaseUtilities.DeleteData(sql, dbpath);
				//Console.WriteLine("任务内容：" + ds.Tables[0].Rows.Count.ToString());				
			}
			catch (Exception ex)
			{
                MyConsole.WriteLine("任务出错：" + ex.Message);
			}
		}
        #endregion
    }
}
