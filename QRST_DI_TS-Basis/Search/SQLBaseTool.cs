using System;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_TS_Basis.DirectlyAddress;
/// <summary> 
///SQLBaseTool 的摘要说明
/// </summary>
namespace QRST_DI_TS_Basis.Search
{
    public class SQLBaseTool
    {
        SearchCondition searchcondition;
        IDbBaseUtilities MySqlOperator;
        public SQLBaseTool()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //

            MySqlOperator = Constant.IdbServerUtilities;
            searchcondition = new SearchCondition();
        }
        /// <summary>
        /// 在查询到的切片数据的表中添加列：切片文件名。DLF 20130416
        /// </summary>
        /// <param name="ds">输入的原始表格</param>
        /// <returns>添加切片文件名的数据表格</returns>
        public static DataSet AddTileDataNameInfo(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return ds;
            }
            //删除ID列（已经无意义）
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].Caption.ToUpper() == "ID")
                {
                    ds.Tables[0].Columns.RemoveAt(i);
                    break;
                }
            }
            ds.Tables[0].Columns.Add("TileFileName");
            ds.Tables[0].Columns["TileFileName"].SetOrdinal(0);
            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            {
                string tilename = GetTilename(ds.Tables[0].Rows[ii]);

                ds.Tables[0].Rows[ii]["TileFileName"] = tilename;
            }

            return ds;
        }
        /// <summary>
        /// 在查询到的切片数据的表中添加列：行业信息（APP用到）。DLF 20130416
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="hostIP"></param>
        /// <returns></returns>
        public DataSet AddDataHostInfo(DataSet ds, string hostIP)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataSet dsHost = MySqlOperator.GetDataSet(string.Format("SELECT NAME FROM db02.db02_hostsinfo where IPADDRESS like '{0}'", hostIP));
                string hostNmae = dsHost.Tables[0].Rows[0][0].ToString();
                ds.Tables[0].Columns.Add("Host");
                ds.Tables[0].Columns.Add("HostIP");
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    ds.Tables[0].Rows[ii]["HostIP"] = hostIP;

                    ds.Tables[0].Rows[ii]["Host"] = hostNmae;
                }
            }

            return ds;
            //return null;
        }

        public static string GetTilename(DataRow dr)
        {
            string tilename = "";
            if (dr != null && dr.Table != null)
            {
                try
                {
                    if (dr.Table.Columns.Contains("ProdType"))
                    {
                        if (dr.Table.Columns.Contains("Availability") && dr.Table.Columns.Contains("Cloud"))
                        {
                            tilename = ProdTileNameArgs.GetTileName(
                                dr["ProdType"].ToString(),
                                dr["Date"].ToString(),
                                dr["DataSourceID"].ToString(),
                                Convert.ToInt16(dr["Availability"]),
                                Convert.ToInt16(dr["Cloud"]),
                                dr["Level"].ToString(),
                                dr["Row"].ToString(),
                                dr["Col"].ToString());
                        }
                        else
                        {
                            tilename = ProdTileNameArgs.GetTileName(
                                dr["ProdType"].ToString(),
                                dr["Date"].ToString(),
                                dr["DataSourceID"].ToString(),
                                dr["Level"].ToString(),
                                dr["Row"].ToString(),
                                dr["Col"].ToString());
                        }
                    }
                    else if (dr.Table.Columns.Contains("CategoryCode"))
                    {
                        tilename = ClassifySampleTileNameArgs.GetTileName(
                              dr["CategoryCode"].ToString(),
                              dr["SampleTypeID"].ToString(),
                              dr["ShootTime"].ToString(),
                              dr["DataSource"].ToString(),
                              dr["Level"].ToString(),
                              dr["Row"].ToString(),
                              dr["Col"].ToString());
                    }
                    else if (dr.Table.Columns.Contains("type"))
                    {
                        if (dr.Table.Columns.Contains("Availability") && dr.Table.Columns.Contains("Cloud"))
                        {
                            tilename = CorrectedTileNameArgs.GetTileName(
                                dr["Satellite"].ToString(),
                                dr["Sensor"].ToString(),
                                dr["Date"].ToString(),
                                dr["DataSourceID"].ToString(),
                                Convert.ToInt16(dr["Availability"]),
                                Convert.ToInt16(dr["Cloud"]),
                                dr["Level"].ToString(),
                                dr["Row"].ToString(),
                                dr["Col"].ToString(),
                                dr["type"].ToString());
                        }
                        else
                        {
                            tilename = CorrectedTileNameArgs.GetTileName(
                                dr["Satellite"].ToString(),
                                dr["Sensor"].ToString(),
                                dr["Date"].ToString(),
                                dr["DataSourceID"].ToString(),
                                dr["Level"].ToString(),
                                dr["Row"].ToString(),
                                dr["Col"].ToString(),
                                dr["type"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return tilename;
        }

        public DataSet AddData_TileNameAddress(DataSet ds)
        {
            //增加了Availability和Cloud字段，需要更新
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //删除ID列（已经无意义）,另外最新数据覆盖（SearTileBatch方法）会额外增加一列“count(distinct Date)”无用，需要移除
                    if (ds.Tables[0].Columns.Contains("ID"))
                    {
                        ds.Tables[0].Columns.Remove("ID");
                    }
                    if (ds.Tables[0].Columns.Contains("count(distinct Date)"))
                    {
                        ds.Tables[0].Columns.Remove("count(distinct Date)");
                    }
                

                ds.Tables[0].Columns.Add("TileFileName");
                ds.Tables[0].Columns["TileFileName"].SetOrdinal(0);
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {

                    string tilename = GetTilename(ds.Tables[0].Rows[ii]);

                    ds.Tables[0].Rows[ii]["TileFileName"] = tilename;

                }

                //else
                //{

                //    ds.Tables[0].Columns.Add("TileFilePath");
                //    ds.Tables[0].Columns["TileFilePath"].SetOrdinal(0);
                //    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                //    {
                //        string qrstcode = ds.Tables[0].Rows[ii]["QRST_CODE"].ToString();
                //        string funFtp = QDB_Utilities.Storage.DirectlyFtpMessage.GetFtpMessage(qrstcode, dataType);

                //        ds.Tables[0].Rows[ii]["TileFilePath"] = funFtp;

                //    }
                //}

            }
            //ds = this.AddDataFTPAddress(ds, DataSynchType.Unkown);


            return ds;
        }

        /// <summary>
        /// 分类样本
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet AddData_SampleTileNameAddress(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //删除ID列（已经无意义）
                //for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                //{
                //    if (ds.Tables[0].Columns[i].Caption.ToUpper() == "ID")
                //    {
                //        ds.Tables[0].Columns.RemoveAt(i);
                //        break;
                //    }
                //}
                ds.Tables[0].Columns.Add("TileFileName");
                ds.Tables[0].Columns["TileFileName"].SetOrdinal(0);
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {

                    string tilename = "";

                    tilename = string.Format("CS_{0}_{1}_{2}_{3}_{4}_{5}_{6}.tif",
                                 ds.Tables[0].Rows[ii]["CategoryCode"],
                                 ds.Tables[0].Rows[ii]["ID"],
                                 ds.Tables[0].Rows[ii]["ShootTime"],
                                 ds.Tables[0].Rows[ii]["DataSource"],
                                 ds.Tables[0].Rows[ii]["Level"],
                                 ds.Tables[0].Rows[ii]["Row"],
                                 ds.Tables[0].Rows[ii]["Col"]);

                    ds.Tables[0].Rows[ii]["TileFileName"] = tilename;

                }
            }

            return ds;
        }
        //public static List<PagedInfo> GetPageInfo(int returnRecordSize,List<int> everyTableSize,int pageIndex)
        //{
        //    List<PagedInfo> listPagesInfo = new List<PagedInfo>();
            

        //}
        /*
        public string QueryCollection(string tablename, List<string> position, List<DateTime> datetime, List<string> satellite, List<string> sensor)
        {
            string QueryStr = "";
            List<string> str = new List<string>();//位置sql
            if (position.Count != 0)
            {

                string minRow = position[0];
                string minColum = position[1];
                string maxRow = position[2];
                string maxColum = position[3];

                str = this.locationSQL_Pol(tablename, minRow, maxRow, maxColum, minColum);
                //QueryStr += string.Format("name NOT IN (SELECT name FROM {0} WHERE DATALOWERLEFTLAT < {1}  or DATAUPPERRIGHTLAT >  {2} or DATAUPPERRIGHTLONG > {3} or DATALOWERLEFTLONG <  {4} ) ", tablename, minRow, maxRow, maxColum, minColum);

                QueryStr += str[0].ToString();
                //QueryStr += string.Format("( DATALOWERLEFTLAT>={1} and DATALOWERLEFTLONG>={2})and (DATAUPPERRIGHTLAT<={3} and DATAUPPERRIGHTLONG<={4})", tablename, minRow, minColum, maxRow, maxColum);
                QueryStr += " and";
            }
            if (datetime.Count != 0)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("SCENEDATE between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" satellite = '{0}' or ", s);

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
                    QueryStr += string.Format(" sensor = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }
        ///// <summary>
        ///// 针对四个角点是矩形四个顶点
        ///// </summary>
        ///// <param name="minRow"></param>
        ///// <param name="maxRow"></param>
        ///// <param name="maxColum"></param>
        ///// <param name="minColum"></param>
        ///// <returns></returns>
        //public List<string> location_Rec(string tablename, string minRow, string maxRow, string maxColum, string minColum)
        //{
        //    List<string> str = new List<string>();
        //    string QueryStr1 = "";//适用于视图db02.view_hj_source_metadata
        //    QueryStr1 += string.Format("name NOT IN (SELECT name FROM {4} WHERE LL_MinLat > {0}  or UR_MaxLat <  {1} or UR_MaxLon < {2} or LL_MinLon >  {3} ) ", maxRow, minRow, minColum, maxColum, tablename);



        //    str.Add(QueryStr1);
        //    return str;
        //}
        /// <summary>
        /// 针对四个角点是矩形四个顶点
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="minRow">框选范围</param>
        /// <param name="maxRow">框选范围</param>
        /// <param name="maxColum">框选范围</param>
        /// <param name="minColum">框选范围</param>
        /// <param name="MINLAT">数据库中的字段</param>
        /// <param name="MAZLAT">数据库中的字段</param>
        /// <param name="MINLONG">数据库中的字段</param>
        /// <param name="MAXLONG">数据库中的字段</param>
        /// <returns></returns>
        public List<string> location_Rec(string tablename,
            string minRow, string maxRow, string maxColum, string minColum,
            string MINLAT, string MAXLAT, string MINLONG, string MAXLONG)
        {
            return location_Rec(tablename, "name",
                 minRow, maxRow, maxColum, minColum,
                 MINLAT, MAXLAT, MINLONG, MAXLONG);

        }
        /// <summary>
        /// 针对四个角点是矩形四个顶点
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="minRow">框选范围</param>
        /// <param name="maxRow">框选范围</param>
        /// <param name="maxColum">框选范围</param>
        /// <param name="minColum">框选范围</param>
        /// <param name="MINLAT">数据库中的字段</param>
        /// <param name="MAZLAT">数据库中的字段</param>
        /// <param name="MINLONG">数据库中的字段</param>
        /// <param name="MAXLONG">数据库中的字段</param>
        /// <returns></returns>
        public List<string> location_Rec(string tablename, string colname,
            string minRow, string maxRow, string maxColum, string minColum,
            string MINLAT, string MAXLAT, string MINLONG, string MAXLONG)
        {
            List<string> str = new List<string>();
            string QueryStr1 = "";//适用于视图db02.view_hj_source_metadata
            QueryStr1 += string.Format("{9} NOT IN (SELECT {9} FROM {4} WHERE {5} > {0}  or {6} <  {1} or {7} < {2} or {8} >  {3} ) ",
                                       maxRow, minRow, minColum, maxColum, tablename, MINLAT, MAXLAT, MAXLONG, MINLONG, colname);



            str.Add(QueryStr1);
            return str;
        }

        /// <summary>
        /// 针对四个角点是真实图像内部四个点
        /// </summary>
        /// <param name="minRow"></param>
        /// <param name="maxRow"></param>
        /// <param name="maxColum"></param>
        /// <param name="minColum"></param>
        /// <returns></returns>
        public List<string> locationSQL_Pol(string tablename, string minRow, string maxRow, string maxColum, string minColum)
        {
            List<string> str = new List<string>();
            string QueryStr1 = "";//适用于视图db02.view_hj_source_metadata
            QueryStr1 += string.Format("name NOT IN (SELECT name FROM {4} WHERE (LL_MinLat > {0} and LR_MinLat > {0})  or (UR_MaxLat <  {1} and UL_MaxLat <  {1}) or (UR_MaxLon < {2} and LR_MaxLon < {2})or( LL_MinLon >  {3} and UL_MinLon >  {3} )) ", maxRow, minRow, minColum, maxColum, tablename);

            str.Add(QueryStr1);
            return str;
        }
        /// <summary>
        /// db02_userdata_vector
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="datetime">可以为null</param>
        /// <param name="usermark"></param>
        /// <returns></returns>
        public string QueryCollection_UserVector(List<string> position, List<DateTime> datetime, string keyword)
        {
            string tablename = "db02.db02_userdata_vector";
            string colname = "DATANAME";
            string QueryStr = "";
            List<string> str = new List<string>();//位置sql
            if (position.Count != 0)
            {

                string minRow = position[0];
                string minColum = position[1];
                string maxRow = position[2];
                string maxColum = position[3];
                //str = this.location_Rec(tablename, minRow, maxRow, maxColum, minColum,"","","","");
                str = this.location_Rec(tablename, colname, minRow, maxRow, maxColum, minColum, "MinY", "MaxY", "MinX", "MaxX");
                //QueryStr += string.Format("name NOT IN (SELECT name FROM {0} WHERE DATALOWERLEFTLAT < {1}  or DATAUPPERRIGHTLAT >  {2} or DATAUPPERRIGHTLONG > {3} or DATALOWERLEFTLONG <  {4} ) ", tablename, minRow, maxRow, maxColum, minColum);

                QueryStr += str[0].ToString();
                //QueryStr += string.Format("( DATALOWERLEFTLAT>={1} and DATALOWERLEFTLONG>={2})and (DATAUPPERRIGHTLAT<={3} and DATAUPPERRIGHTLONG<={4})", tablename, minRow, minColum, maxRow, maxColum);
                QueryStr += " and";
            }
            if (datetime.Count != 0)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("DataTime between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and ";
            }
            if (keyword != null)
            {
                QueryStr += " (";
                QueryStr += string.Format("( DataName like '%{0}%' ) or ( UserMark like '%{0}%' ) ", keyword);
                QueryStr += ") and ";
            }

            if (QueryStr.EndsWith("or ") || QueryStr.EndsWith("and "))
            {
                if (QueryStr.EndsWith("or "))
                {
                    QueryStr = QueryStr.TrimEnd(" or ".ToCharArray());
                }
                else
                {
                    QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());

                }
            }




            return QueryStr;


        }

        /// <summary>
        /// db02_userdata_raster
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="datetime">可以为null</param>
        /// <param name="usermark"></param>
        /// <returns></returns>
        public string QueryCollection_UserRaster(List<string> position, List<DateTime> datetime, string keyword)
        {
            string tablename = "db02.db02_userdata_raster";
            string colname = "DATANAME";
            string QueryStr = "";
            List<string> str = new List<string>();//位置sql
            if (position.Count != 0)
            {

                string minRow = position[0];
                string minColum = position[1];
                string maxRow = position[2];
                string maxColum = position[3];
                //str = this.location_Rec(tablename, minRow, maxRow, maxColum, minColum,"","","","");
                str = this.location_Rec(tablename, colname, minRow, maxRow, maxColum, minColum, "MinY", "MaxY", "MinX", "MaxX");
                
                QueryStr += str[0];
                
                QueryStr += " and";
            }
            if (datetime.Count != 0)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("DataTime between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and ";
            }
            if (keyword != null)
            {
                QueryStr += " (";
                QueryStr += string.Format("( DataName like '%{0}%' ) or ( DataMark like '%{0}%' ) ", keyword);
                QueryStr += ") and ";
            }

            if (QueryStr.EndsWith("or ") || QueryStr.EndsWith("and "))
            {
                if (QueryStr.EndsWith("or "))
                {
                    QueryStr = QueryStr.TrimEnd(" or ".ToCharArray());
                }
                else
                {
                    QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());

                }
            }




            return QueryStr;


        }

        public string QueryCollection_Algorithm(List<DateTime> datetime, string keyword)
        {
            string QueryStr = "";
            List<string> str = new List<string>();//位置sql

            if (datetime != null && datetime.Count != 0)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("UPDATE between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            if (keyword != null)
            {
                QueryStr += " (";

                QueryStr += string.Format("( CHALGNAME like '%{0}%' ) or (", keyword);

                QueryStr += string.Format(" ENGALGNAME like '%{0}%' ) or ( ", keyword);

                QueryStr += string.Format("ALGDESCRIPTION like '%{0}%' ) ", keyword);
                QueryStr += ") and ";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }

        /// <summary>
        /// db02_userdata_vector
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="datetime">可以为null</param>
        /// <param name="usermark"></param>
        /// <returns></returns>
        public string QueryCollection_UserAlgorithm(List<DateTime> datetime, string keyword)
        {
            string QueryStr = "";
            List<string> str = new List<string>();//位置sql

            if (datetime.Count != 0)
            {
                QueryStr += "(";
                DateTime dt1 = datetime[0];
                DateTime dt2 = datetime[1];
                QueryStr += string.Format("UPTIME between '{0}' and '{1}'", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            if (keyword != null)
            {
                QueryStr += " (";

                QueryStr += string.Format("(CHINESENAME like '%{0}%' ) or (", keyword);

                //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";

                QueryStr += string.Format(" ENGLISHNAME like '%{0}%' ) or (", keyword);

                //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                QueryStr += string.Format(" USERREMARK like '%{0}%' ) ", keyword);
                QueryStr += ") and ";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }
        /// <summary>
        /// db02_userdata_vector
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="datetime">可以为null</param>
        /// <param name="usermark"></param>
        /// <returns></returns>
        public string QueryCollection_UserService(string keyword)
        {
            string QueryStr = "";
            List<string> str = new List<string>();//位置sql

            
            //if (datetime.Count != 0)
            //{
            //    QueryStr += "(";
            //    DateTime dt1 = datetime[0];
            //    DateTime dt2 = datetime[1];
            //    QueryStr += string.Format("UPDATE between '{0}' and '{1}'", dt1, dt2);
            //    //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
            //    QueryStr += ") and";
            //}
            
            if (keyword != null)
            {
                QueryStr += " (";

                QueryStr += string.Format(" ServerName like '%{0}%'  ", keyword);

                //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";

                QueryStr += ") or";

                QueryStr += " (";

                QueryStr += string.Format(" Remark like '%{0}%'  ", keyword);

                //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";

                //QueryStr += ") ";

                //QueryStr += " (";
                //QueryStr += string.Format(" USERREMARK like '%{0}%'  ", keyword);
                QueryStr += ") and ";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;

        }


        public DataSet AddDataHostInfo(DataSet ds,string hostIP)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataSet dsHost = MySqlOperator.GetDataSet(string.Format("SELECT NAME FROM db02.db02_hostsinfo where IPADDRESS like '{0}'", hostIP));
                string hostNmae= dsHost.Tables[0].Rows[0][0].ToString();
                ds.Tables[0].Columns.Add("Host");
                ds.Tables[0].Columns.Add("HostIP");
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    ds.Tables[0].Rows[ii]["HostIP"] = hostIP;

                    ds.Tables[0].Rows[ii]["Host"] = hostNmae;
                }

            }

            return ds;
        }

        public string SQLComb_UserRasterTile(List<string> position, List<string> orderCode)
        {
            //WebServiceTools.DtCol.Clear();//清理List
            #region//经纬度转行列号
            int[] rowAndColum = new int[4];
            if (position.Count != 0)
            {
                rowAndColum = DirectlyAddressing.GetRowAndColum(position.ToArray());
                position.Clear();
                foreach (int item in rowAndColum)
                {
                    position.Add(item.ToString());
                }
            }
            #endregion
            string sql = string.Format("select * from productTiles where  DataSourceID like 'P%' and");
            string sql2 = searchcondition.QueryCollectionForSqlite_Raster(position, orderCode);
            if (sql2 == "")
            {
                sql = sql.Substring(0, sql.LastIndexOf(" and"));

            }
            else
            {
                sql = sql + sql2;

            }

            return sql;
        }

        internal string QueryCollection(string keyword)
        {
            throw new NotImplementedException();
        }
*/
    }
}