using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_Resources;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 

namespace QRST_DI_DS_Metadata
{
    public class MetaDataDBImporter
    {

        #region Variables&Properties
        private string _dbServer;
        private string _userID;
        private string _password;
        public IDbBaseUtilities _oraUtil;
        public static string _dbName;
        public static Dictionary<string, string> _dicTableNames;
        public Dictionary<string, string> _dicFailedImportedData;
        private IDbOperating CreateNewCode = Constant.IdbOperating;

        private string _importingStatus;
        public string ImportingStatus
        {
            get
            {
                return _importingStatus;
            }
        }

        private MetaData _importingMetaData;
        public MetaData ImportingMetaData
        {
            get
            {
                return _importingMetaData;
            }
        }

        private EnumMetadataTypes _importingDataType;
        public EnumMetadataTypes ImportingDataType
        {
            get
            {
                return _importingDataType;
            }
        }
        #endregion

        #region events
        public event EventHandler ImportingStatusUpdated;
        #endregion

        public MetaDataDBImporter()
        {
            //初始化表名
            InitTableNames();

            //创建Oracle数据库引擎
            //_oraUtil = new MySqlBaseUtilities(Constant.ConnectionStringEVDB);
            _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringEVDB);
        }
        /// <summary>
        /// 设置表名默认值
        /// </summary>
        public void InitTableNames()
        {
            _dbName = "evdb";
            _dicTableNames = new Dictionary<string, string>();
            //_dicTableNames.Add("RASTER_DATA_LIST", "RASTER_DATA_LIST");
            //_dicTableNames.Add("satellites", "satellites");
            //_dicTableNames.Add("sensors", "sensors");
            //_dicTableNames.Add("RECEIVER_TYPE", "RECEIVER_TYPE");
            _dicTableNames.Add("PROD_CBERS", "PROD_CBERS");
            _dicTableNames.Add("PROD_HJ", "PROD_HJ");
            _dicTableNames.Add("PROD_MODIS", "PROD_MODIS");
            _dicTableNames.Add("PROD_NOAA", "PROD_NOAA");
            _dicTableNames.Add("PRODS_Vector", "PRODS_VECTOR");
            _dicTableNames.Add("PRODS_national30dem", "PRODS_NATIONAL30DEM");
            _dicTableNames.Add("PRODS_nationaltm", "PRODS_NATIONALTM");
            // _dicTableNames.Add("PROD_TYPE_MODIS", "PROD_TYPE_MODIS");
            //_dicTableNames.Add("DB02_PROD_GROUP_MODIS", "DB02_PROD_GROUP_MODIS");
        }

        #region Database Import MetaData

        /// <summary>
        /// 数据目录导入
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="folderPath">数据目录</param>
        //public void ImportDir(EnumDataTypes dataType, string folderPath)
        //{
        //    _dicFailedImportedData = new Dictionary<string, string>();      //初始化导入失败数据列表

        //    string[] allFiles = null;
        //    int count = 0;
        //    _importingDataType = dataType;

        //    switch (dataType)
        //    {
        //        case EnumDataTypes.Unknown:
        //            break;
        //        case EnumDataTypes.MODIS:
        //            //遍历目录 
        //            allFiles = Directory.GetFiles(folderPath, "*.hdf", SearchOption.AllDirectories);
        //            count = 0;
        //            foreach (string dataFile in allFiles)
        //            {
        //                count++;
        //                _importingStatus = string.Format("正在对{0}进行入库...[{1}/{2}].", dataFile, count, allFiles.Length);
        //                if (count == allFiles.Length )
        //                {

        //                    _importingStatus = "所有数据入库完毕";
        //                }



        //                try
        //                {
        //                    ImportData(EnumDataTypes.MODIS, dataFile);
        //                }
        //                catch (Exception ex)
        //                {
        //                    _dicFailedImportedData.Add(dataFile, ex.Message);
        //                }




        //            }
        //            break;
        //        case EnumDataTypes.CBERS:
        //            //遍历目录 
        //            //allFiles = Directory.GetFiles(folderPath, "*.zip", SearchOption.AllDirectories);
        //            //count = 0;
        //            //foreach (string dataFile in allFiles)
        //            //{
        //            //    _importingStatus = string.Format("正在对{0}进行入库...[{1}/{2}].", dataFile, count, allFiles.Length);
        //            //    try
        //            //    {
        //            //        ImportData(EnumDataTypes.CBERS, dataFile);
        //            //    }
        //            //    catch (Exception ex)
        //            //    {
        //            //        _dicFailedImportedData.Add(dataFile, ex.Message);
        //            //    }
        //            //    count++;
        //            //}
        //            allFiles = Directory.GetFiles(folderPath, "*.tar.gz", SearchOption.AllDirectories);
        //            count = 0;
        //            foreach (string dataFile in allFiles)
        //            {
        //                count++;
        //                _importingStatus = string.Format("正在对{0}进行入库...[{1}/{2}].", dataFile, count, allFiles.Length);
        //                if (count == allFiles.Length)
        //                {
        //                    _importingStatus = "所有数据入库完毕";
        //                }


        //                try
        //                {
        //                    ImportData(EnumDataTypes.CBERS, dataFile);
        //                }
        //                catch (Exception ex)
        //                {
        //                    _dicFailedImportedData.Add(dataFile, ex.Message);
        //                }



        //            }
        //            break;
        //        case EnumDataTypes.HJ:
        //            //遍历目录 
        //            allFiles = Directory.GetFiles(folderPath, "*.tar.gz", SearchOption.AllDirectories);
        //            count = 0;
        //            foreach (string dataFile in allFiles)
        //            {
        //                 count++;

        //                  _importingStatus = string.Format("正在对{0}进行入库...[{1}/{2}].", dataFile, count, allFiles.Length);
        //                  if (count == allFiles.Length)
        //                  {
        //                      string end = "所有数据入库完毕";
        //                      _importingStatus = end;
        //                  }

        //                try
        //                {
        //                    ImportData(EnumDataTypes.HJ, dataFile);
        //                }
        //                catch (Exception ex)
        //                {
        //                    _dicFailedImportedData.Add(dataFile, ex.Message);
        //                }



        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// 时间+随机数命名
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetTempFormTime(DateTime dt)
        {
            return string.Format("{0:yyyyMMddHHmmss}", dt);
        }

        public void ImportData(EnumMetadataTypes dataType, DataTable dt)
        {
            switch (dataType)
            {
                case EnumMetadataTypes.Unknown:
                    break;
                case EnumMetadataTypes.VECTOR:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    _oraUtil.AddDataTableToDB("prods_vector", dt);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 数据入库
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="filePath">数据路径</param>
        public void ImportData(EnumMetadataTypes dataType, MetaData md, string filePath, string sitecode, out string sqlstring)
        {
            sqlstring = "";
            //读取数据的元数据信息到MetaData类
            //MetaDataReader mdUtil = new MetaDataReader();
            // MetaData md = mdUtil.ReadMetaData(dataType, filePath);

            //更新进度信息
            _importingMetaData = md;
            if (ImportingStatusUpdated != null)
            {
                ImportingStatusUpdated(null, new EventArgs());
            }

            //进行元数据入库
            switch (dataType)
            {
                case EnumMetadataTypes.Unknown:
                    break;
                case EnumMetadataTypes.MODIS:
                    MetaDataModis mmodis = md as MetaDataModis;
                    mmodis.RawFilePath = filePath;
                    WriteMetaDataModis(mmodis,sitecode);
                    break;
                case EnumMetadataTypes.CBERS:
                    MetaDataCbers mcbers = md as MetaDataCbers;
                    mcbers.RawFilePath = filePath;
                    WriteMetaDataCbers(mcbers,sitecode);
                    break;
                case EnumMetadataTypes.NOAA:
                    MetaDataNOAA mnoaa = md as MetaDataNOAA;
                    mnoaa.RawFilePath = filePath;
                    WriteMetaDataNOAA(mnoaa, sitecode);
                    break;
                case EnumMetadataTypes.HJ:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了。
                    //_oraUtil = new MySqlBaseUtilities(Constant.ConnectionStringEVDB);
                    //_dbName = EnumDBType.EVDB.ToString();
                    MetaDataHj mhj = md as MetaDataHj;
                    WriteMetaDataHj(mhj, sitecode, out sqlstring);
                    break;
                case EnumMetadataTypes.VECTOR:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataVector mdv = md as MetaDataVector;
                    WriteMetaDataVector(mdv);
                    break;
                  case EnumMetadataTypes.TM:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataRaster mdTM = md as MetaDataRaster;
                    WriteMetaDataTM(mdTM);
                    break;
                case EnumMetadataTypes.DEM:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataRaster mdDEM = md as MetaDataRaster;
                    WriteMetaDataDEM(mdDEM);
                    break;
                case EnumMetadataTypes.HHALOS:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataRaster mdHHALOS = md as MetaDataRaster;
                    WriteMetaDataDOM(mdHHALOS,"prod_hh_alos");
                    break;
                case EnumMetadataTypes.SZWORLDVIEW2:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataRaster mdSZWORLDVIEW2 = md as MetaDataRaster;
                    WriteMetaDataDOM(mdSZWORLDVIEW2,"prod_sz_worldview2");
                    break;
                case EnumMetadataTypes.TJQBird:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataRaster mdTJQBird = md as MetaDataRaster;
                    WriteMetaDataDOM(mdTJQBird,"prod_tj_qbird");
                    break;
                case EnumMetadataTypes.ZJCOASTALOS:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataRaster mdZJCOASTALOS = md as MetaDataRaster;
                    WriteMetaDataDOM(mdZJCOASTALOS,"prod_zjcoastland_alos");
                    break;
                case EnumMetadataTypes.TJ5MRSImage:
                    //在前面已经把默认的元数据入库类的初始化连接和数据库名称指向实验验证库了,所以改到基础空间库。
                    _oraUtil = Constant.IdbServerUtilities.GetSubDbUtilByCon(Constant.ConnectionStringBSDB);
                    MetaDataRaster mdTJ5MRSImage = md as MetaDataRaster;
                    WriteMetaDataDOM(mdTJ5MRSImage, "prod_tj5mrsimage");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 从数据的内容找到数据对应的相对应种类的ID
        /// </summary>
        /// <param name="IDcolumnName">数据所属的ID的列名</param>
        /// <param name="tableName">数据所属的表</param>
        /// <param name="contentColumnName">数据属性的列名</param>
        /// <param name="content">数据属性对应的值</param>
        /// <returns></returns>
        public string GetDataInfoID(string IDcolumnName, string tableName, string contentColumnName, string content)
        {

            DataSet Result = new DataSet();
            string DataInfoID = "";

            Result = _oraUtil.GetDataSet(string.Format("select {0} from {1} where {2}= '{3}'", IDcolumnName, tableName, contentColumnName, content));
            DataTable resulttable = Result.Tables[0];
            if (resulttable.Rows.Count != 0)
            {
                DataInfoID = resulttable.Rows[0][0].ToString();
            }
            else
            {
                DataInfoID = "temp";
            }
            return DataInfoID;
        }
        /// <summary>
        /// 返回已存的最大的ID 
        /// </summary>
        /// <returns></returns>
        public int GetMaxID(string IDname, string tablename)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select MAX({0}) from {1}", IDname, tablename));
            DataSet ds = _oraUtil.GetDataSet(strSql.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string maxid = ds.Tables[0].Rows[0][0].ToString();
                if (maxid == "")
                {
                    return 0;
                }
                else
                {
                    int result = Convert.ToInt32(maxid);
                    return result;
                }
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 获取表的CODE 
        /// </summary>
        /// <param name="IDcolumnName">表CODE的列名</param>
        /// <param name="tableColumnName">表的名字的列名</param>
        /// <param name="tablename">表的名字的内容</param>
        /// <param name="standardtablename">所有表的CODE所在的标准表</param>
        /// <returns></returns>
        public string GetTableCode(string IDcolumnName, string tableColumnName, string tablename, string standardtablename)
        {
            string sqlstr = string.Format("select {0} from {1} where {2} ='{3}'", IDcolumnName, standardtablename, tableColumnName, tablename);
            DataSet Result = new DataSet();
            string tableID = "";
            DataSet ds = _oraUtil.GetDataSet(sqlstr.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                tableID = ds.Tables[0].Rows[0][0].ToString();
                return tableID;
            }
            else
            {
                return "";
            }


        }


        /// <summary>
        /// 获取数据的名字
        /// </summary>
        /// <param name="rawfilepath">数据存放的完整路径</param>
        /// <returns></returns>
        public string GetDataName(string rawfilepath)
        {
            string dataname = "";
            string[] paths = rawfilepath.Split("\\".ToCharArray());
            string datapath = paths[paths.Length - 1].ToString();
            switch (_importingDataType)
            {
                case EnumMetadataTypes.MODIS:
                    dataname = Path.GetFileNameWithoutExtension(datapath);
                    break;
                case EnumMetadataTypes.HJ:
                    dataname = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(datapath));
                    break;
                case EnumMetadataTypes.CBERS:
                    dataname = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(datapath));
                    break;
                case EnumMetadataTypes.NOAA:
                    dataname = Path.GetFileNameWithoutExtension(datapath);
                    break;

            }

            return dataname;
        }

        /// <summary>
        /// 返回HJ星的插入语句
        /// </summary>
        /// <param name="metaDataHj"></param>
        /// <returns></returns>
        public string GetInsetHJ(MetaDataHj metaDataHj, string site)
        {
            string dataname = metaDataHj.NAME;
            string tbname = _dicTableNames["PROD_HJ"];
            int count = _oraUtil.GetRecordCount("*", _dicTableNames["PROD_HJ"], "where NAME = '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where NAME = '{1}'", _dicTableNames["PROD_HJ"], dataname));
            }
            string returnstr = "";
            string satelliteID =metaDataHj.Satellite;
            string sensorID =  metaDataHj.Sensor;

            //string datatypeID = GetDataInfoID("ID", "DB02_PRODS_RASTER", "DATATYPE", metaDataHj.DataType.ToString());
            int id = 0;
            string QrstCode = String.Format("{0}-{1}{2}", Constant.INDUSTRYCODE, CreateNewCode.GetPreNewColumnCodeFromTableName(tbname, EnumDBType.EVDB, out id), id);
            metaDataHj.QRST_CODE = QrstCode;
            // qrstcode=CreateNewCode.GetNewColumnCodeFromTableName("DB02_PROD_HJ");
            //returnstr = string.Format("insert into  {0}{30}{31} values({32},'{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}',{10},to_date('{11}','yyyy-mm-dd hh24:mi:ss'),'{12}','{13}','{14}','{15}','{16}',{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},'{27}','{28}','{29}','{33}')",         //Oracle
            returnstr = string.Format("insert into  {34}.{0}{30}{31} values({32},'{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}',{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},'{27}','{28}','{29}','{33}')",           //MySQL 不需要to_date()
               _dicTableNames["PROD_HJ"],
               dataname,
               satelliteID,
               metaDataHj.RecStation,
               metaDataHj.ProductID,
               metaDataHj.SenceID,
               metaDataHj.ProductLevel,
               sensorID,
               metaDataHj.PixelSpacing,
               metaDataHj.SceneOrbitPath,
               metaDataHj.SceneOrbitRow,
               metaDataHj.SceneDate.ToString("yyyy-MM-dd HH:mm:ss"),
               metaDataHj.EarthModel,
               metaDataHj.MapProjection,
               metaDataHj.Zone,
               metaDataHj.ResampleTechnique,
               metaDataHj.DataFormatDes,
               metaDataHj.SunElevation,
               metaDataHj.SunAzimuthElevation,
               metaDataHj.DataUpperLeftLat,
               metaDataHj.DataUpperLeftLong,
               metaDataHj.DataUpperRightLat,
               metaDataHj.DataUpperRightLong,
               metaDataHj.DataLowerLeftLat,
               metaDataHj.DataLowerRightLong,
               metaDataHj.DataLowerRightLat,
               metaDataHj.DataLowerLeftLong,
               site,
               metaDataHj.OverviewFilePath.Replace(@"\", @"\\"),
               QrstCode,
               "(ID,NAME,SATELLITE,RECSTATION,PRODUCTID,SCENEID,PRODUCTLEVEL,SENSOR,PIXELSPACING,SCENEORBITPATH,SCENEORBITROW,SCENEDATE,EARTHMODEL,MAPPROJECTION,ZONE,RESAMPLETECHNIQUE,DATAFORMATDES,SUNELEVATION,SUNAZIMUTHELEVATION,DATAUPPERLEFTLAT,",
               "DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,STORAGESITE,OVERVIEWFILEPATH,QRST_CODE,CorDataPath)",
               id,
               metaDataHj.CorDataPath.Replace(@"\", @"\\"),
               _dbName);
            return returnstr;
        }

        static bool dblock = false;
        /// <summary>
        /// HJ元数据信息入库
        /// </summary>
        /// <param name="metaDataHj">HJ元数据信息</param>
        public void WriteMetaDataHj(MetaDataHj metaDataHj, string siteQCode)
        {
            string sqlstr;
            WriteMetaDataHj(metaDataHj, siteQCode, out sqlstr);
        }
        public void WriteMetaDataHj(MetaDataHj metaDataHj, string siteQCode, out string sqlstr)
        {
            //锁定表
            while (dblock)
            {
                System.Threading.Thread.Sleep(10);
            }
            dblock = true;

            //获取插入SQL语句,查看数据是否存在
            string existsql = string.Format("select count(*) from prod_hj where name = '{0}'",metaDataHj.NAME);
            DataSet recordCountSet = _oraUtil.GetDataSet(existsql);
            if (recordCountSet.Tables[0] != null && recordCountSet.Tables[0].Rows.Count > 0)
            {
                int recordCount = int.Parse(recordCountSet.Tables[0].Rows[0][0].ToString());
                if (recordCount > 0) //删除旧的记录
                {
                    string deletesql = string.Format("delete from prod_hj where name = '{0}'",metaDataHj.NAME);
                    _oraUtil.ExecuteSql(deletesql);
                }
            }
            string mdhjInsertSql = GetInsetHJ(metaDataHj, siteQCode);
            sqlstr = mdhjInsertSql;
            int resultRow = _oraUtil.ExecuteSql(mdhjInsertSql);
            if (resultRow > 0)
            {
                test();

            }
            dblock = false;


            ////——待修改

            ////判断是否已存在数据记录
            //string name = _dicTableNames["DB02_PROD_HJ"];
            //int count = _oraUtil.GetRecordCount("*", _dicTableNames["DB02_PROD_HJ"], "where NAME = '" + Path.GetFileName(metaDataHj.RawFilePath) + "' and " );
            //if (count > 0)
            //{
            //    //删除已存在的记录
            //    _oraUtil.ExecuteSql(string.Format("Delete from {0} where RAWFILEPATH='{1}'", _dicTableNames["DB02_PROD_HJ"], metaDataHj.RawFilePath));
            //}
            //string satelliteID = GetDataInfoID("QRST_CODE", "satellites", "NAME", metaDataHj.Satellite);
            //string sensorID = GetDataInfoID("QRST_CODE", "sensors", "NAME", metaDataHj.Sensor);
            //string dataname = GetDataName(metaDataHj.RawFilePath);
            ////string datatypeID = GetDataInfoID("ID", "DB02_PRODS_RASTER", "DATATYPE", metaDataHj.DataType.ToString());
            //int id = CreateNewCode.GetNewID("DB02_PROD_HJ");

            ////插入新纪录
            //int resultRow= _oraUtil.ExecuteSql(string.Format("insert into  {0}{30}{31} values({32},'{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}',{10},to_date('{11}','yyyy-mm-dd hh24:mi:ss'),'{12}','{13}','{14}','{15}','{16}',{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},'{27}','{28}','{29}')",
            //    _dicTableNames["DB02_PROD_HJ"],

            //    dataname,
            //    satelliteID,
            //    metaDataHj.RecStation,
            //    metaDataHj.ProductID,
            //    metaDataHj.SenceID,
            //    metaDataHj.ProductLevel,
            //    sensorID,
            //    metaDataHj.PixelSpacing,
            //    metaDataHj.SceneOrbitPath,
            //    metaDataHj.SceneOrbitRow,
            //    metaDataHj.SceneDate,
            //    metaDataHj.EarthModel,
            //    metaDataHj.MapProjection,
            //    metaDataHj.Zone,
            //    metaDataHj.ResampleTechnique,
            //    metaDataHj.DataFormatDes,
            //    metaDataHj.SunElevation,
            //    metaDataHj.SunAzimuthElevation,
            //    metaDataHj.DataUpperLeftLat,
            //    metaDataHj.DataUpperLeftLong,
            //    metaDataHj.DataUpperRightLat,
            //    metaDataHj.DataUpperRightLong,
            //    metaDataHj.DataLowerLeftLat,
            //    metaDataHj.DataLowerRightLong,
            //    metaDataHj.DataLowerRightLat,
            //    metaDataHj.DataLowerLeftLong,
            //    metaDataHj.RawFilePath ,
            //    metaDataHj.OverviewFilePath,
            //    CreateNewCode.GetNewQrstCodeFromTableName("DB02_PROD_HJ"),
            //    "(ID,NAME,SATELLITE,RECSTATION,PRODUCTID,SCENEID,PRODUCTLEVEL,SENSOR,PIXELSPACING,SCENEORBITPATH,SCENEORBITROW,SCENEDATE,EARTHMODEL,MAPPROJECTION,ZONE,RESAMPLETECHNIQUE,DATAFORMATDES,SUNELEVATION,SUNAZIMUTHELEVATION,DATAUPPERLEFTLAT,",
            //    "DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,RAWFILEPATH,OVERVIEWFILEPATH,QRST_CODE)",
            //    id ));
            //if (resultRow > 0)
            //{
            //    test();

            //}

        }

        public void test()
        {
        }

        /// <summary>
        /// Vector元数据信息入库
        /// </summary>
        /// <param name="metaDataNOAA">Vector元数据信息</param>
        private void WriteMetaDataVector(MetaDataVector metaDataVector)
        {
            //——待修改
            string dataname = metaDataVector.DataName;

            //判断是否已存在数据记录
            int count = _oraUtil.GetRecordCount("*", _dicTableNames["PRODS_Vector"], "where DATANAME= '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where DATANAME='{1}'", _dicTableNames["PRODS_Vector"], dataname));
            }

            // string datatypeID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataCbers.DataType.ToString());
            int id = CreateNewCode.GetNewID("PRODS_Vector", EnumDBType.BSDB);

            //插入新纪录
            //_oraUtil.ExecuteSql(string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},to_date('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //Oracle
            string sqlstr = "insert into " + _dicTableNames["PRODS_Vector"] + " VALUES ('" + id + "', '" + metaDataVector.Name + "', '" + metaDataVector.ProductName + "', '" + metaDataVector.ProduceDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + metaDataVector.Produceorg + "', '" + metaDataVector.DataName + "', '" + metaDataVector.DataSource + "', '" + metaDataVector.DataType + "', '" + metaDataVector.ExtentUp + "', '" + metaDataVector.ExtentDown + "', '" + metaDataVector.ExtentLeft + "', '" + metaDataVector.ExtentRight + "', '" + metaDataVector.DataSize + "', '" + metaDataVector.DataFormat + "', '" + metaDataVector.MapProjectPara + "', '" + metaDataVector.Coordinate + "','" + metaDataVector.ZoneNo + "', '" + metaDataVector.Security + "', '" + metaDataVector.DataQulity + "', '" + metaDataVector.Scale + "', '" + metaDataVector.MetaProduceDate + "', '" + metaDataVector.MetaProduceorg + "', '" + metaDataVector.MetaProductor + "','" + metaDataVector.Remark + "', '" + metaDataVector.GroupCode + "','" + metaDataVector.SDE +  "')";
            _oraUtil.ExecuteSql(sqlstr);
        }

/// <summary>
        /// TM元数据信息入库
        /// </summary>
        /// <param name="metaDataNOAA">TM元数据信息</param>
        private void WriteMetaDataTM(MetaDataRaster metaDataRaster)
        {
            //——待修改
            string dataname = metaDataRaster.DataName;

            //判断是否已存在数据记录
            int count = _oraUtil.GetRecordCount("*", _dicTableNames["PRODS_nationaltm"], "where DATANAME= '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where DATANAME='{1}'", _dicTableNames["PRODS_nationaltm"], dataname));
            }

            // string datatypeID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataCbers.DataType.ToString());
            int id = CreateNewCode.GetNewID("PRODS_nationaltm", EnumDBType.BSDB);

            //插入新纪录
            //_oraUtil.ExecuteSql(string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},to_date('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //Oracle
            string sqlstr = "insert into " + _dicTableNames["PRODS_nationaltm"] + " VALUES ('" + id + "', '" + metaDataRaster.MetaName + "', '" + metaDataRaster.ProductName + "', '" + metaDataRaster.ProduceDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + metaDataRaster.Produceorg + "', '" + metaDataRaster.DataName + "', '" + metaDataRaster.DataSource + "', '" + metaDataRaster.DataType + "', '" + metaDataRaster.ExtentUp + "', '" + metaDataRaster.ExtentDown + "', '" + metaDataRaster.ExtentLeft + "', '" + metaDataRaster.ExtentRight + "', '" + metaDataRaster.DataSize + "', '" + metaDataRaster.DataFormat + "', '" + metaDataRaster.MapProjectPara + "', '" + metaDataRaster.Coordinate + "','" + metaDataRaster.ZoneNo + "', '" + metaDataRaster.Security + "', '" + metaDataRaster.DataQulity + "', '" + metaDataRaster.Resolution +"','"+ metaDataRaster.BandNum +"','"+ metaDataRaster.DataFile +"','"+ metaDataRaster.SceneYear +"','"+ metaDataRaster.CorrectRef + "', '" + metaDataRaster.MetaProduceData + "', '" + metaDataRaster.MetaProduceorg + "', '" + metaDataRaster.MetaProductor + "','" + metaDataRaster.Remark + "')";
            _oraUtil.ExecuteSql(sqlstr);
        }


        /// <summary>
        /// DOM元数据信息入库
        /// </summary>
        /// <param name="metaDataNOAA">DOM元数据信息</param>
        private void WriteMetaDataDOM(MetaDataRaster metaDataRaster,string tablename)
        {
            //——待修改
            string dataname = metaDataRaster.DataName;

            //判断是否已存在数据记录
            int count = _oraUtil.GetRecordCount("*", tablename, "where DATANAME= '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where DATANAME='{1}'", tablename, dataname));
            }

            // string datatypeID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataCbers.DataType.ToString());
            int id = CreateNewCode.GetNewID(tablename, EnumDBType.BSDB);

            //插入新纪录
            //_oraUtil.ExecuteSql(string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},to_date('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //Oracle
            string sqlstr = "insert into " +tablename + " VALUES ('" + id + "', '" + metaDataRaster.MetaName + "', '" + metaDataRaster.ProductName + "', '" + metaDataRaster.ProduceDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + metaDataRaster.Produceorg + "', '" + metaDataRaster.DataName + "', '" + metaDataRaster.DataSource + "', '" + metaDataRaster.DataType + "', '" + metaDataRaster.ExtentUp + "', '" + metaDataRaster.ExtentDown + "', '" + metaDataRaster.ExtentLeft + "', '" + metaDataRaster.ExtentRight + "', '" + metaDataRaster.DataSize + "', '" + metaDataRaster.DataFormat + "', '" + metaDataRaster.MapProjectPara + "', '" + metaDataRaster.Coordinate + "','" + metaDataRaster.ZoneNo + "', '" + metaDataRaster.Security + "', '" + metaDataRaster.DataQulity + "', '" + metaDataRaster.Resolution + "','" + metaDataRaster.BandNum + "','" + metaDataRaster.DataFile + "','" + metaDataRaster.SceneYear + "','" + metaDataRaster.CorrectRef + "', '" + metaDataRaster.MetaProduceData + "', '" + metaDataRaster.MetaProduceorg + "', '" + metaDataRaster.MetaProductor + "','" + metaDataRaster.Remark + "')";
            _oraUtil.ExecuteSql(sqlstr);
        }



        /// <summary>
        /// DEM元数据信息入库
        /// </summary>
        /// <param name="metaDataNOAA">DEM元数据信息</param>
        private void WriteMetaDataDEM(MetaDataRaster metaDataRaster)
        {
            //——待修改
            string dataname = metaDataRaster.DataName;

            //判断是否已存在数据记录
            int count = _oraUtil.GetRecordCount("*", _dicTableNames["PRODS_national30dem"], "where DATANAME= '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where DATANAME='{1}'", _dicTableNames["PRODS_national30dem"], dataname));
            }

            // string datatypeID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataCbers.DataType.ToString());
            int id = CreateNewCode.GetNewID("PRODS_national30dem", EnumDBType.BSDB);

            //插入新纪录
            //_oraUtil.ExecuteSql(string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},to_date('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //Oracle
            string sqlstr = "insert into " + _dicTableNames["PRODS_national30dem"] + " VALUES ('" + id + "', '" + metaDataRaster.MetaName + "', '" + metaDataRaster.ProductName + "', '" + metaDataRaster.ProduceDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + metaDataRaster.Produceorg + "', '" + metaDataRaster.DataName + "', '" + metaDataRaster.DataSource + "', '" + metaDataRaster.DataType + "', '" + metaDataRaster.ExtentUp + "', '" + metaDataRaster.ExtentDown + "', '" + metaDataRaster.ExtentLeft + "', '" + metaDataRaster.ExtentRight + "', '" + metaDataRaster.DataSize + "', '" + metaDataRaster.DataFormat + "', '" + metaDataRaster.MapProjectPara + "', '" + metaDataRaster.Coordinate + "','" + metaDataRaster.ZoneNo + "', '" + metaDataRaster.Security + "', '" + metaDataRaster.DataQulity + "', '" + metaDataRaster.Resolution + "','" + metaDataRaster.BandNum + "','" + metaDataRaster.DataFile + "','" + metaDataRaster.SceneYear + "','" + metaDataRaster.CorrectRef + "', '" + metaDataRaster.MetaProduceData + "', '" + metaDataRaster.MetaProduceorg + "', '" + metaDataRaster.MetaProductor + "','" + metaDataRaster.Remark + "')";
            _oraUtil.ExecuteSql(sqlstr);
        }
        /// <summary>
        /// NOAA元数据信息入库
        /// </summary>
        /// <param name="metaDataNOAA">NOAA元数据信息</param>
        private void WriteMetaDataNOAA(MetaDataNOAA metaDataNOAA, string site)
        {
            //——待修改
            string dataname = GetDataName(metaDataNOAA.RawFilePath);
            
            //判断是否已存在数据记录
            int count = _oraUtil.GetRecordCount("*", _dicTableNames["PROD_NOAA"], "where NAME= '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where NAME='{1}'", _dicTableNames["PROD_NOAA"], dataname));
            }

            string satelliteID =metaDataNOAA.Satellite;
            string sensorID = metaDataNOAA.Sensor;

            // string datatypeID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataCbers.DataType.ToString());
            int id = CreateNewCode.GetNewID("PROD_NOAA", EnumDBType.EVDB);

            //插入新纪录
            //_oraUtil.ExecuteSql(string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},to_date('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //Oracle
             string sqltemp = string.Format("insert into {0}{16} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",       //MySQL 不用to_date
                 _dicTableNames["PROD_NOAA"],
                 id,
                 dataname,
                 site,
                 satelliteID,
                 sensorID,
                 metaDataNOAA.DataType,
                 metaDataNOAA.Revolution,
                 metaDataNOAA.Source,
                 metaDataNOAA.ProcessingCenter,
                 metaDataNOAA.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                 metaDataNOAA.StopDate.ToString("yyyy-MM-dd HH:mm:ss"),
                 metaDataNOAA.Location,
                 metaDataNOAA.OverviewFilePath,
                 "",
                 string.Format("{0}-{1}",Constant.INDUSTRYCODE,CreateNewCode.GetNewQrstCodeFromTableName("PROD_NOAA", EnumDBType.EVDB)),
                 "(ID,NAME,STORAGESITE,SATELLITE,SENSOR,DATA_TYPE,REVOLUTION,SOURCE,PROCESSING_CENTER,STARTDATE,STOPDATE,LOCATION,OVERVIEWFILEPATH,DESCRIPTION,QRST_CODE)");
            sqltemp = sqltemp.Replace("\\", "\\\\");
            _oraUtil.ExecuteSql(sqltemp);
        }


        /// <summary>
        /// 返回执行插入的Cbers语句
        /// </summary>
        /// <param name="metaDataCbers"></param>
        /// <returns></returns>
        //public string GetInsertCbers(MetaDataCbers metaDataCbers, string storagesite, string qrstcode)
        //{
        //    string dataname = GetDataName(metaDataCbers.RawFilePath);
        //    int count = _oraUtil.GetRecordCount("*", _dicTableNames["PROD_CBERS"], "where NAME= '" + dataname + "'");
        //    if (count > 0)
        //    {
        //        //删除已存在的记录
        //        _oraUtil.ExecuteSql(string.Format("Delete from {0} where NAME = '{1}'", _dicTableNames["PROD_CBERS"], dataname));
        //    }
        //    string returnCbersSQl = "";
        //    string satelliteID = GetDataInfoID("QRST_CODE", "satellites", "NAME", metaDataCbers.Satellite);
        //    string sensorID = GetDataInfoID("QRST_CODE", "sensors", "NAME", metaDataCbers.Sensor);
        //    //string dataname = GetDataName(metaDataCbers.RawFilePath);
        //    // string datatypeID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataCbers.DataType.ToString());
        //    // int id = GetMaxID("ID", "DB02_PROD_CBERS") + 1;
        //    int id = Convert.ToInt32(qrstcode);
        //    string name = _dicTableNames["PROD_CBERS"];
        //    string QrstCode = CreateNewCode.GetPreNewColumnCodeFromTableName(name, EnumDBType.EVDB) + qrstcode;
        //    // qrstcode=CreateNewCode.GetNewColumnCodeFromTableName("DB02_PROD_CBERS");
        //    //returnCbersSQl = string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},to_date('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //Oracle
        //    returnCbersSQl = string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},'{13}','{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //MySql 不用to_date
        //        _dicTableNames["PROD_CBERS"],
        //        id,
        //        dataname,
        //        storagesite,
        //        satelliteID,
        //        metaDataCbers.RecStation,
        //        metaDataCbers.ProductID,
        //        metaDataCbers.SenceID,
        //        metaDataCbers.ProductLevel,
        //        sensorID,
        //        metaDataCbers.PixelSpacing,
        //        metaDataCbers.SceneOrbitPath,
        //        metaDataCbers.SceneOrbitRow,
        //        metaDataCbers.SceneDate,
        //        metaDataCbers.EarthModel,
        //        metaDataCbers.MapProjection,
        //        metaDataCbers.Zone,
        //        metaDataCbers.ResampleTechnique,
        //        metaDataCbers.DataUpperLeftLat,
        //        metaDataCbers.DataUpperLeftLong,
        //        metaDataCbers.DataUpperRightLat,
        //        metaDataCbers.DataUpperRightLong,
        //        metaDataCbers.DataLowerRightLat,
        //        metaDataCbers.DataLowerRightLong,
        //        metaDataCbers.DataLowerLeftLat,
        //        metaDataCbers.DataLowerLeftLong,

        //        metaDataCbers.OverviewFilePath,
        //        "",
        //        QrstCode,
        //        "(ID,NAME,STORAGESITE,SATELLITE,RECSTATION,PRODUCTID,SCENEID,PRODUCTLEVEL,SENSOR,PIXELSPACING,SCENEORBITPATH,SCENEORBITROW,SCENEDATE,EARTHMODEL,MAPPROJECTION,ZONE,RESAMPLETECHNIQUE,DATAUPPERLEFTLAT,",
        //        "DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,OVERVIEWFILEPATH,DESCRIPTION,QRST_CODE)");
        //    return returnCbersSQl;
        //}
        /// <summary>
        /// CBERS元数据信息入库
        /// </summary>
        /// <param name="metaDataHj">CBERS元数据信息</param>
        private void WriteMetaDataCbers(MetaDataCbers metaDataCbers,string site)
        {
            //——待修改
            string dataname = GetDataName(metaDataCbers.RawFilePath);
            //判断是否已存在数据记录
            int count = _oraUtil.GetRecordCount("*", _dicTableNames["PROD_CBERS"], "where NAME= '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where NAME='{1}'", _dicTableNames["PROD_CBERS"], dataname));
            }

            string satelliteID = metaDataCbers.Satellite;
            string sensorID = metaDataCbers.Sensor;
            
            // string datatypeID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataCbers.DataType.ToString());
            int id = CreateNewCode.GetNewID("PROD_CBERS", EnumDBType.EVDB);

            //插入新纪录
            //_oraUtil.ExecuteSql(string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},to_date('{13}','yyyy-mm-dd hh24:mi:ss'),'{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",     //Oracle
            _oraUtil.ExecuteSql(string.Format("insert into {0}{29}{30} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12},'{13}','{14}','{15}','{16}','{17}',{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}')",       //MySQL 不用to_date
                 _dicTableNames["PROD_CBERS"],
                 id,
                 dataname,
                 site,
                 satelliteID,
                 metaDataCbers.RecStation,
                 metaDataCbers.ProductID,
                 metaDataCbers.SenceID,
                 metaDataCbers.ProductLevel,
                 sensorID,
                 metaDataCbers.PixelSpacing,
                 metaDataCbers.SceneOrbitPath,
                 metaDataCbers.SceneOrbitRow,
                 metaDataCbers.SceneDate.ToString("yyyy-MM-dd HH:mm:ss"),
                 metaDataCbers.EarthModel,
                 metaDataCbers.MapProjection,
                 metaDataCbers.Zone,
                 metaDataCbers.ResampleTechnique,
                 metaDataCbers.DataUpperLeftLat,
                 metaDataCbers.DataUpperLeftLong,
                 metaDataCbers.DataUpperRightLat,
                 metaDataCbers.DataUpperRightLong,
                 metaDataCbers.DataLowerRightLat,
                 metaDataCbers.DataLowerRightLong,
                 metaDataCbers.DataLowerLeftLat,
                 metaDataCbers.DataLowerLeftLong,

                 metaDataCbers.OverviewFilePath,
                 "",
                  string.Format("{0}-{1}", Constant.INDUSTRYCODE, CreateNewCode.GetNewQrstCodeFromTableName("PROD_CBERS", EnumDBType.EVDB)),
                 "(ID,NAME,STORAGESITE,SATELLITE,RECSTATION,PRODUCTID,SCENEID,PRODUCTLEVEL,SENSOR,PIXELSPACING,SCENEORBITPATH,SCENEORBITROW,SCENEDATE,EARTHMODEL,MAPPROJECTION,ZONE,RESAMPLETECHNIQUE,DATAUPPERLEFTLAT,",
                 "DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,OVERVIEWFILEPATH,DESCRIPTION,QRST_CODE)"));
        }
        /// <summary>
        /// 返回执行Modis的sql语句
        /// </summary>
        /// <param name="metaDataModis"></param>
        /// <returns></returns>
        //public string GetInsertModis(MetaDataModis metaDataModis, string storagesite, string qrstcode)
        //{

        //    string dataname = GetDataName(metaDataModis.RawFilePath);
        //    int count = _oraUtil.GetRecordCount("*", _dicTableNames["PROD_MODIS"], "where NAME= '" + dataname + "'");
        //    if (count > 0)
        //    {
        //        //删除已存在的记录
        //        _oraUtil.ExecuteSql(string.Format("Delete from {0} where NAME='{1}'", _dicTableNames["PROD_MODIS"], dataname));
        //    }
        //    string returnsql;
        //    string satelliteID = GetDataInfoID("QRST_CODE", "satellites", "NAME", metaDataModis.Satellite);
        //    string sensorID = GetDataInfoID("QRST_CODE", "sensors", "NAME", metaDataModis.Sensor);

        //    // string ID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataModis.DataType.ToString());
        //    //int id = GetMaxID("ID", "DB02_PROD_MODIS") + 1;
        //    int id = Convert.ToInt32(qrstcode);
        //    string name = _dicTableNames["PROD_MODIS"];
        //    string QrstCode = CreateNewCode.GetPreNewColumnCodeFromTableName(name, EnumDBType.EVDB) + qrstcode;
        //    //returnsql = string.Format("insert into {0}{25}{26} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}',to_date('{11}','yyyy-mm-dd hh24:mi:ss'),to_date('{12}','yyyy-mm-dd hh24:mi:ss'),{13},{14},{15},{16},{17},{18},{19},{20},'{21}','{22}','{23}','{24}')",     //Oracle
        //    returnsql = string.Format("insert into {0}{25}{26} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}','{11}','{12}',{13},{14},{15},{16},{17},{18},{19},{20},'{21}','{22}','{23}','{24}')",       //MySQL 不用to_date
        //        _dicTableNames["PROD_MODIS"],
        //        id,
        //        dataname,
        //        storagesite,
        //        satelliteID,
        //        sensorID,
        //        metaDataModis.RawFile,
        //        metaDataModis.AncillaryFile,
        //        metaDataModis.ProductStyle,
        //        metaDataModis.OrbitNumber,
        //        metaDataModis.DayNightFlag,
        //        metaDataModis.BeginDate,
        //        metaDataModis.EndDate,
        //        metaDataModis.DataUpperLeftLat,
        //        metaDataModis.DataUpperLeftLong,
        //        metaDataModis.DataUpperRightLat,
        //        metaDataModis.DataUpperRightLong,
        //        metaDataModis.DataLowerRightLat,
        //        metaDataModis.DataLowerRightLong,
        //        metaDataModis.DataLowerLeftLat,
        //        metaDataModis.DataLowerLeftLong,

        //        metaDataModis.AncillaryFile,
        //        metaDataModis.OverviewFilePath,
        //        "",
        //         QrstCode,
        //        "(ID,NAME,STORAGESITE,SATELLITE,SENSOR,RAWFILE,ANCILLARYFILE,PRODUCTSTYLE,ORBITNUMBER,DAYNIGHTFLAG,BEGINDATE,ENDDATE,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,",
        //        "DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,ANCILLARYFILEPATH,OVERVIEWFILEPATH,DESCRIPTION,QRST_CODE)");
        //    return returnsql;
        //}

        /// <summary>
        /// MODIS元数据信息入库
        /// </summary>
        /// <param name="metaDataHj">MODIS元数据信息</param>
        public void WriteMetaDataModis(MetaDataModis metaDataModis,string site)
        {
            //——待修改
            string dataname = GetDataName(metaDataModis.RawFilePath);
            string modType = "";

            //判断是否已存在数据记录
            int count = _oraUtil.GetRecordCount("*", _dicTableNames["PROD_MODIS"], "where NAME= '" + dataname + "'");
            if (count > 0)
            {
                //删除已存在的记录
                _oraUtil.ExecuteSql(string.Format("Delete from {0} where NAME='{1}'", _dicTableNames["PROD_MODIS"], dataname));
            }
            int id = CreateNewCode.GetNewID("PROD_MODIS", EnumDBType.EVDB);
            if (dataname.StartsWith("MOD03"))
            {
                modType = "MOD03";
            }
            else if (dataname.StartsWith("MOBRGB"))
            {
                modType = "MODRGB";
                string sql = String.Format("insert into {0}(ID,NAME,BEGINDATE,MODTYPE) values({1},'{2}','{3}','{4}')", _dicTableNames["PROD_MODIS"], id, dataname, metaDataModis.BeginDate, modType);
                _oraUtil.ExecuteSql(sql);
                return;
            } 
            else
            {
                modType = dataname.Substring(0, dataname.IndexOf('.'));
                //设置辅助文件和缩略图文件在数据阵列里面的基础路径,\\172.16.0.1\综合数据库\实验验证库\MODIS\MOBRGB\2003\12\03\  视图中加上文件名称为文件名的文件和最后的文件名
                metaDataModis.OverviewFilePath = String.Format("{0}实验验证数据库\\MODIS\\MOBRGB\\{1}\\{2}\\{3}\\", StoragePath.StoreBasePath, string.Format("{0:0000}", metaDataModis.BeginDate.Year), string.Format("{0:00}", metaDataModis.BeginDate.Month), string.Format("{0:00}", metaDataModis.BeginDate.Day));
                metaDataModis.AncillaryFile = String.Format("{0}实验验证数据库\\MODIS\\{1}\\{2}\\{3}\\{4}\\{5}\\", StoragePath.StoreBasePath, metaDataModis.Satellite, metaDataModis.Sensor, string.Format("{0:0000}", metaDataModis.BeginDate.Year), string.Format("{0:00}", metaDataModis.BeginDate.Month), string.Format("{0:00}", metaDataModis.BeginDate.Day));
            }
            
            

            string satelliteID = GetDataInfoID("QRST_CODE", "satellites", "NAME", metaDataModis.Satellite);
            string sensorID = GetDataInfoID("QRST_CODE", "sensors", "NAME", metaDataModis.Sensor);
            // string ID = GetDataInfoID("ID", "RASTER_DATA_LIST", "DATATYPE", metaDataModis.DataType.ToString());
            

            //插入新纪录
            //_oraUtil.ExecuteSql(string.Format("insert into {0}{25}{26} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}',to_date('{11}','yyyy-mm-dd hh24:mi:ss'),to_date('{12}','yyyy-mm-dd hh24:mi:ss'),{13},{14},{15},{16},{17},{18},{19},{20},'{21}','{22}','{23}','{24}')",     //oracle
            string sqltemp = string.Format("insert into {0}{23}{24} values({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},{11},{12},{13},{14},{15},{16},{17},'{18}','{19}','{20}','{21}','{22}')",       //mysql 不用to_date
                 _dicTableNames["PROD_MODIS"],
                 id,
                 dataname,
                 satelliteID,
                 sensorID,
                 metaDataModis.ProductStyle,
                 metaDataModis.OrbitNumber,
                 metaDataModis.DayNightFlag,
                 metaDataModis.BeginDate,
                 metaDataModis.EndDate,
                 metaDataModis.DataUpperLeftLat,
                 metaDataModis.DataUpperLeftLong,
                 metaDataModis.DataUpperRightLat,
                 metaDataModis.DataUpperRightLong,
                 metaDataModis.DataLowerRightLat,
                 metaDataModis.DataLowerRightLong,
                 metaDataModis.DataLowerLeftLat,
                 metaDataModis.DataLowerLeftLong,
                 metaDataModis.AncillaryFile,
                 metaDataModis.OverviewFilePath,
                 "",
                  CreateNewCode.GetNewQrstCodeFromTableName("PROD_MODIS", EnumDBType.EVDB),
                  modType,
                 "(ID,NAME,SATELLITE,SENSOR,PRODUCTSTYLE,ORBITNUMBER,DAYNIGHTFLAG,BEGINDATE,ENDDATE,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,",
                 "DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,ANCILLARYFILEPATH,OVERVIEWFILEPATH,DESCRIPTION,QRST_CODE,MODTYPE)");
            sqltemp = sqltemp.Replace("\\","\\\\");
            _oraUtil.ExecuteSql(sqltemp);

        }
        #endregion

        #region Database Check MetaData

        /// <summary>
        /// 检查某一类型元数据表，对表中数据丢失记录进行清除，对同份数据重复记录进行更新
        /// </summary>
        /// <param name="dataType"></param>
        public void CheckMetaDataTable(EnumMetadataTypes dataType)
        {
            //获取表名
            string prodTable = "";
            switch (dataType)
            {
                case EnumMetadataTypes.MODIS:
                    prodTable = _dicTableNames["PROD_MODIS"];
                    break;
                case EnumMetadataTypes.CBERS:
                    prodTable = _dicTableNames["PROD_CBERS"];
                    break;
                case EnumMetadataTypes.HJ:
                    prodTable = _dicTableNames["PROD_HJ"];
                    break;
                case EnumMetadataTypes.NOAA:
                    prodTable = _dicTableNames["PROD_NOAA"];
                    break;
                case EnumMetadataTypes.Unknown:
                default:
                    throw new Exception("CheckMetaDataTable:Data type error.");
            }
            //获取数据路径列表
            DataSet ds = _oraUtil.GetDataSet(string.Format("select PATH,ID from {0}", prodTable));
            //判断各路径数据是否存在
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string path = ds.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();
                string id = ds.Tables[0].Rows[i].ItemArray.GetValue(1).ToString();
                if (!System.IO.File.Exists(path))
                {
                    //删除不存在的数据记录信息
                    _oraUtil.ExecuteSql(string.Format("Delete from {0} where ID={1}", prodTable, id));
                }
            }


            //——待修改


            //对于同份数据多个记录，进行更新
            //获取各个Path及其对应的记录个数
            //对记录个数>1的进行删除
            //插入新记录

            string sql = string.Format("delete from {0} where ID not in (select max(ID) from {0} gruop by NAME having count(*)>1)", prodTable);
            _oraUtil.ExecuteSql(sql);

        }
        #endregion
    }
}
