/*注：
 * 1）SQLite库默认密码“987654”；
 * 2）不能开着SQLiteDeveloper 否则表被锁定无法执行创建表以及插入数据索引等操作。
 */
using System;
using System.Collections.Generic;

using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_TS_Basis.DBEngine
{
    //20170421  迁移到DI_SS_Basis   
    //public enum TileIndexUpdateType
    //{
    //    InsertUpdate = 0,
    //    Delete = 1
    //}

    public class TileIndexUpdateUtilities
    {
        public DirectlyAddressing da;
        private IDbBaseUtilities _iLiteBaseUtilities;
        public TileIndexUpdateUtilities()
        {
            da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            _iLiteBaseUtilities = Constant.ITileDbUtilities;
        }

        public void TileIndexUpdate(TileIndexUpdateType type, List<string> tilenames, string dbfile)
        {
            switch (type)
            {
                case TileIndexUpdateType.InsertUpdate:
                    AddIndex(tilenames, dbfile);
                    break;
                case TileIndexUpdateType.Delete:
                    DeleteIndex(tilenames, dbfile);
                    break;
                default:
                    break;
            }

        }

        public void TileIndexUpdate(TileIndexUpdateType type, List<string> tilenames)
        {
            switch (type)
            {
                case TileIndexUpdateType.InsertUpdate:
                    AddIndex(tilenames);
                    break;
                case TileIndexUpdateType.Delete:
                    DeleteIndex(tilenames);
                    break;
                default:
                    break;
            }

        }

        private void DeleteIndex(List<string> tilenames)
        {
            Dictionary<string, List<string>> dicDBsourcesSqls = new Dictionary<string, List<string>>();
            foreach (string tn in tilenames)
            {
                string deletesql, dbsource;
                getDeleteSql_Dbfile(tn, out deletesql, out dbsource);
                if (deletesql != "" && dbsource != "")
                {
                    if (!dicDBsourcesSqls.ContainsKey(dbsource))
                    {
                        dicDBsourcesSqls.Add(dbsource, new List<string>());
                    }
                    if (!dicDBsourcesSqls[dbsource].Contains(deletesql))
                    {
                        dicDBsourcesSqls[dbsource].Add(deletesql);
                    }
                }
            }

            foreach (KeyValuePair<string, List<string>> kvp in dicDBsourcesSqls)
            {
                //迁移到服务进程  jianghua 20170415
                //SqliteBaseUtilities dbDelete = new SqliteBaseUtilities();

                //dbDelete.DeleteData(kvp.Value, kvp.Key);
                _iLiteBaseUtilities.DeleteData(kvp.Value, kvp.Key);
            }
        }

        private void AddIndex(List<string> tilenames)
        {
            Dictionary<string, List<string>> dicDBsourcesSqls = new Dictionary<string, List<string>>();
            foreach (string tn in tilenames)
            {
                string insertsql, dbsource;
                getInsertSql_Dbfile(tn, out insertsql,out dbsource);
                if (insertsql!=""&&dbsource!="")
                {
                    if (!dicDBsourcesSqls.ContainsKey(dbsource))
                    {
                        dicDBsourcesSqls.Add(dbsource,new List<string>());
                    }
                    if (!dicDBsourcesSqls[dbsource].Contains(insertsql))
                    {
                        dicDBsourcesSqls[dbsource].Add(insertsql);
                    }
                }
            }

            foreach (KeyValuePair<string, List<string>> kvp in dicDBsourcesSqls)
            {
                //迁移到服务进程  jianghua 20170415
                //SqliteBaseUtilities dbInsert = new SqliteBaseUtilities();
                try
                {
                    string s=_iLiteBaseUtilities.GetDbConnection();
                    //dbInsert.InSertData(kvp.Value, kvp.Key);
                    _iLiteBaseUtilities.InSertData(kvp.Value, kvp.Key);
                 
                }
                catch(Exception ex)
                {

                }
            }

        }

        private void DeleteIndex(List<string> tilenames, string dbfile)
        {
            //迁移到服务进程  jianghua 20170415
            //SqliteBaseUtilities dbDelete = new SqliteBaseUtilities();
            List<string> sqlstrs = new List<string>();
            foreach (string tn in tilenames)
            {
                string[] sqlandIPMod = getDeleteSqlStr(tn);
                if (sqlandIPMod!=null)
                {
                sqlstrs.Add(sqlandIPMod[0]);
            }
            }
            //dbDelete.DeleteData(sqlstrs, dbfile);
            _iLiteBaseUtilities.DeleteData(sqlstrs, dbfile);

        }

        private void AddIndex(List<string> tilenames, string dbfile)
    {
        //迁移到服务进程  jianghua 20170415
        //SqliteBaseUtilities dbInsert = new SqliteBaseUtilities();
        List<string> sqlstrs = new List<string>();
        foreach (string tn in tilenames)
        {
            string[] sqlandIPMod = getInsertSqlStr(tn);
            if (sqlandIPMod != null && sqlandIPMod.Length == 2)
            {
                sqlstrs.Add(sqlandIPMod[0]);
            }
        }
        //dbInsert.InSertData(sqlstrs, dbfile);
        _iLiteBaseUtilities.InSertData(sqlstrs, dbfile);

    }

        private void DeleteIndex(IDbBaseUtilities dbDelete, string fileName)
        {
            //获取配号
            //int sNo = directlyAddressing.GetStorageNo(fileName);
            //int sNo = GetIPMod(fileName);
            string[] sqlandIPMod = getDeleteSqlStr(fileName);   //将sqlanddblevel改为了sqlandIPMod
            if (sqlandIPMod != null && sqlandIPMod.Length==2)
            {
                string sql = sqlandIPMod[0];
                string sNo = sqlandIPMod[1];

                string ip = da.GetIPbyMod(sNo);
                string rootDir = @"\\" + ip + @"\QRST_DB_Tile\";
                string dbsource = String.Format(@"{0}{1}\QDB_IDX_{1}.db", rootDir, sNo);

                dbDelete.DeleteData(sql, dbsource);
            }
        }


        private void getDeleteSql_Dbfile(string fileName, out string deletesql, out string dbsource)
        {
            //获取配号
            //int sNo = directlyAddressing.GetStorageNo(fileName);
            //int sNo = GetIPMod(fileName);
            string[] sqlandIPMod = getDeleteSqlStr(fileName);   //将sqlanddblevel改为了sqlandIPMod
            if (sqlandIPMod != null && sqlandIPMod.Length == 2)
            {
                string sql = sqlandIPMod[0];
                string sNo = sqlandIPMod[1];
                string rootDir;
                dbsource = "";
                switch (Constant.DbStorage)
                {
                    case EnumDbStorage.MULTIPLE:
                        string ip = da.GetIPbyMod(sNo);
                        rootDir = @"\\" + ip + @"\QRST_DB_Tile\";
                        dbsource = String.Format(@"{0}{1}\QDB_IDX_{1}.db", rootDir, sNo);
                        break;
                    case EnumDbStorage.SINGLE:
                        rootDir = string.Format(@"{0}\{1}\", Constant.PcDBRootPath, "QRST_DB_Tile");
                        //string rootDir = @"\\" + ip + @"\QRST_DB_Tile\";
                        dbsource = String.Format(@"{0}{1}\QDB_IDX_{1}.db", rootDir, sNo);
                        break;
                    case EnumDbStorage.CLUSTER:
                        break;
                }
                deletesql = sql;

            }
            else
            {
                deletesql = "";
                dbsource = "";
            }
        }

        private void getInsertSql_Dbfile(string fileName,out string insertsql,out string dbsource)
        {
            string[] sqlandIPMod = getInsertSqlStr(fileName);
            if (sqlandIPMod != null && sqlandIPMod.Length == 2)
            {
                string sql = sqlandIPMod[0];
                //database的数据源  /4/1/QDB_IDX_4_2.db   4为level、2为配号；现在的是QDB_IDX_2.db：2为配号；  
                //获取IP余号
                string sNo = sqlandIPMod[1];
                string rootDir = null;
                dbsource = "";
                //根据IP获取数据库文件路径 @jianghua SQLite迁移
                switch (Constant.DbStorage)
                {
                    case EnumDbStorage.MULTIPLE:
                        string ip = da.GetIPbyMod(sNo);

                        rootDir = @"\\" + ip + @"\QRST_DB_Tile\";
                        //string rootDir = @"\\127.0.0.1\QRST_DB_Tile\";
                        dbsource = rootDir + sNo + @"\QDB_IDX_" + sNo + ".db";
                        break;
                    case EnumDbStorage.SINGLE:
                         rootDir = Constant.PcDBRootPath + @"\QRST_DB_Tile\";
                        //string rootDir = @"\\127.0.0.1\QRST_DB_Tile\";
                        dbsource = rootDir + sNo + @"\QDB_IDX_" + sNo + ".db";
                        break;
                    case EnumDbStorage.CLUSTER:
                        break;
                }
                insertsql = sql;
            }
            else
            {
                insertsql = "";
                dbsource = "";
            }

        }

        private void AddIndex(IDbBaseUtilities dbInsert, string fileName)
        {
            string[] sqlandIPMod = new string[2];
            sqlandIPMod = getInsertSqlStr(fileName);
            if (sqlandIPMod != null && sqlandIPMod.Length == 2)
            {
                string sql = sqlandIPMod[0];
                //database的数据源  /4/1/QDB_IDX_4_2.db   4为level、2为配号；现在的是QDB_IDX_2.db：2为配号；  
                //获取IP余号
                //int sNo = GetIPMod(fileName);
                string sNo;
                sNo = sqlandIPMod[1];
                //string rootDir = @"\\127.0.0.1\QRST_DB_Tile\";
                string ip = da.GetIPbyMod(sNo);
                string rootDir = @"\\" + ip + @"\QRST_DB_Tile\";
                //if (!Directory.Exists(rootDir))
                //{
                //    rootDir = @"E:\HJDatabaseStorageSite\";
                //}
                string dbsource = rootDir + sNo + @"\QDB_IDX_" + sNo + ".db";

                dbInsert.InSertData(sql, dbsource);
            }

            //throw new NotImplementedException();     //
        }

        //返回插入的sql语句与level
        //将“correctedTiles”和“productTiles”中添加了Time字段，修改时间：2013/2/27 zxw
        private string[] getInsertSqlStr(string fileName)
        {
            try
            {
                TileNameArgs tileName = da.GetTileNameArgs(fileName);
                if (!tileName.Created)
                {
                    return null;
                }
                string[] sqlanddbIPMod = new string[2];  //返回的sql语句和数据等级

                DateTime dt = DateTime.Now;
                switch (tileName.Type)
                {
                    case TileNameArgs.TileType.ProdTile:
                        ProdTileNameArgs prodtilename = tileName as ProdTileNameArgs;
                        sqlanddbIPMod[0] = string.Format("INSERT INTO productTiles(ProdType,DataSourceID,Date,Level,Row,Col,Time,Provider,Availability,Cloud) values('{0}','{1}',{2},'{3}',{4},{5},'{6}','{7}',{8},{9})", prodtilename.ProdType, prodtilename.SrcProdIDsString, (prodtilename.DateTime.Length == 8) ? prodtilename.DateTime + "24" : prodtilename.DateTime, prodtilename.TileLevel, prodtilename.Row, prodtilename.Col, dt, Constant.INDUSTRYCODE, prodtilename.Availability, prodtilename.Cloud);
                        int ipmod = (Convert.ToInt32(prodtilename.Row) + Convert.ToInt32(prodtilename.Col)) % da._MaxMod;
                        sqlanddbIPMod[1] = ipmod.ToString();
                        break;
                    case TileNameArgs.TileType.CorrectedTile:
                        CorrectedTileNameArgs correctedtilename = tileName as CorrectedTileNameArgs;
                        sqlanddbIPMod[0] = string.Format("INSERT INTO correctedTiles(DataSourceID,Satellite,Sensor,Date,Level,Row,Col,type,Time,Provider,Availability,Cloud) values('{0}','{1}','{2}',{3},'{4}',{5},{6},'{7}','{8}','{9}',{10},{11})", correctedtilename.SrcProdID, correctedtilename.Satellite, correctedtilename.Sensor, (correctedtilename.DateTime.Length == 8) ? correctedtilename.DateTime + "24" : correctedtilename.DateTime, correctedtilename.TileLevel, correctedtilename.Row, correctedtilename.Col, correctedtilename.DataType, dt, Constant.INDUSTRYCODE, correctedtilename.Availability, correctedtilename.Cloud);
                        ipmod = (Convert.ToInt32(correctedtilename.Row) + Convert.ToInt32(correctedtilename.Col)) % da._MaxMod;
                        sqlanddbIPMod[1] = ipmod.ToString();
                        break;
                    case TileNameArgs.TileType.ClassifySampleTile:
                        ClassifySampleTileNameArgs clstilename = tileName as ClassifySampleTileNameArgs;
                        sqlanddbIPMod[0] = string.Format("INSERT INTO classifySampleTiles(CategoryCode,SampleTypeID,ShootTime,DataSource,Level,Row,Col) values({0},'{1}',{2},'{3}','{4}',{5},{6})", clstilename.CategoryCode, clstilename.SampleTypeID, (clstilename.DateTime.Length == 8) ? clstilename.DateTime + "24" : clstilename.DateTime, clstilename.SrcTileID, clstilename.TileLevel, clstilename.Row, clstilename.Col);
                        ipmod = (Convert.ToInt32(clstilename.Row) + Convert.ToInt32(clstilename.Col)) % da._MaxMod;
                        sqlanddbIPMod[1] = ipmod.ToString();
                        break;
                    default:
                        sqlanddbIPMod = null;
                        break;
                }
                return sqlanddbIPMod;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //返回删除的sql语句及level
        private string[] getDeleteSqlStr(string fileName)
        {
            try
            {
                TileNameArgs tileName = da.GetTileNameArgs(fileName);
                if (!tileName.Created)
                {
                    return null;
                }
                string[] sqlanddbIPMod = new string[2];  //返回的sql语句和数据等级

                switch (tileName.Type)
                {
                    case TileNameArgs.TileType.ProdTile:
                        ProdTileNameArgs prodtilename = tileName as ProdTileNameArgs;
                        sqlanddbIPMod[0] = string.Format("DELETE FROM productTiles where ProdType='{0}' and DataSourceID='{1}' and Date={2} and Level='{3}' and Row={4} and Col={5}", prodtilename.ProdType, prodtilename.SrcProdIDsString, prodtilename.DateTime, prodtilename.TileLevel, prodtilename.Row, prodtilename.Col);
                        int ipmod = (Convert.ToInt32(prodtilename.Row) + Convert.ToInt32(prodtilename.Col)) % da._MaxMod;
                        sqlanddbIPMod[1] = ipmod.ToString();
                        break;
                    case TileNameArgs.TileType.CorrectedTile:
                        CorrectedTileNameArgs correctedtilename = tileName as CorrectedTileNameArgs;
                        if (correctedtilename.DataType == "Preview")
                        {
                            sqlanddbIPMod[0] = string.Format("DELETE FROM correctedTiles where DataSourceID='{0}' and Satellite='{1}' and Sensor='{2}' and Date={3} and Level='{4}' and Row={5} and Col={6} and type='Preview'", correctedtilename.SrcProdID, correctedtilename.Satellite, correctedtilename.Sensor, correctedtilename.DateTime, correctedtilename.TileLevel, correctedtilename.Row, correctedtilename.Col);
                        }
                        else
                        {
                            sqlanddbIPMod[0] = string.Format("DELETE FROM correctedTiles where DataSourceID='{0}' and Satellite='{1}' and Sensor='{2}' and Date={3} and Level='{4}' and Row={5} and Col={6} and type='{7}'", correctedtilename.SrcProdID, correctedtilename.Satellite, correctedtilename.Sensor, correctedtilename.DateTime, correctedtilename.TileLevel, correctedtilename.Row, correctedtilename.Col, correctedtilename.DataType);
                        }
                        ipmod = (Convert.ToInt32(correctedtilename.Row) + Convert.ToInt32(correctedtilename.Col)) % da._MaxMod;
                        sqlanddbIPMod[1] = ipmod.ToString();
                        break;
                    case TileNameArgs.TileType.ClassifySampleTile:
                        ClassifySampleTileNameArgs clstilename = tileName as ClassifySampleTileNameArgs;
                        sqlanddbIPMod[0] = string.Format("DELETE FROM classifySampleTiles where SampleTypeID={0} and CategoryCode='{1}' and ShootTime={2} and DataSource='{3}'and Level='{4}' and Row={5} and Col={6}",
                            clstilename.SampleTypeID, clstilename.CategoryCode, clstilename.DateTime, clstilename.SrcTileID, clstilename.TileLevel, clstilename.Row, clstilename.Col);
                        ipmod = (Convert.ToInt32(clstilename.Row) + Convert.ToInt32(clstilename.Col)) % da._MaxMod;
                        sqlanddbIPMod[1] = ipmod.ToString();
                        break;
                    default:
                        sqlanddbIPMod = null;
                        break;
                }
                return sqlanddbIPMod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
