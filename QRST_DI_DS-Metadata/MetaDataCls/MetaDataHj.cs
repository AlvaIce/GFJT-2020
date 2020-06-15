using System;
using System.Text;
using System.Globalization;
using System.Xml;
using System.Data;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataHj : MetaData
    {

        public string[] hjAttributeNames={
                                "satelliteId",           //HJ1A
                                "recStationId",          //MYN
                                "productId",             //324149
                                "sceneId",               //327909
                                "productLevel",          //LEVEL2
                                "sensorId",              //CCD1              
                                "pixelSpacing",         //30.000  
                                "scenePath",            //15
                                "sceneRow",             //76
                                "sceneDate",            //2010-06-13 04:04:18.13
                                "earthModel",           //WGS_84
                                "mapProjection",        //UTM
                                "zone",                 //48N
                                "resampleTechnique",    //BL
                                "dataFormatDes",        //GEOTIFF
                                "sunElevation",         //73.745
                                "sunAzimuthElevation",  //300.965
                                "dataUpperLeftLat","dataUpperLeftLong","dataUpperRightLat","dataUpperRightLong",
                                "dataLowerRightLat","dataLowerRightLong","dataLowerLeftLat","dataLowerLeftLong",
                                //以后扩展的属性放在这里
                                "name","overviewFilePath","CorDataPath"
                            };
        
        public string[] hjAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>
        #region Properties
        public string NAME
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }
        
        private string satellite;
        //卫星名，例如HJ1A
        public string Satellite
        {
            get { return hjAttributeValues[0]; }
            set { hjAttributeValues[0] = value; }
        }

        private string recStation;
        //接收站，例如MYN
        public string RecStation
        {
            get { return hjAttributeValues[1]; }
            set { hjAttributeValues[1] = value; }
        }

        private long productID;
        //标准产品号，例如324149
        public long ProductID
        {
            get { return long.Parse(hjAttributeValues[2]); }
            set { hjAttributeValues[2] = value.ToString(); }
        }

        private long senceID;
        //标准景号，例如327909
        public long SenceID
        {
            get { return long.Parse(hjAttributeValues[3]); }
            set { hjAttributeValues[3] = value.ToString(); }
        }

        private string productLevel;
        //产品级别，例如LEVEL2
        public string ProductLevel
        {
            get { return hjAttributeValues[4]; }
            set { hjAttributeValues[4] = value; }
        }

        private string sensor;
        //传感器，例如CCD1
        public string Sensor
        {
            get { return hjAttributeValues[5]; }
            set { hjAttributeValues[5] = value; }
        }

        private float pixelSpacing;
        //空间分辨率，例如30.000
        public float PixelSpacing
        {
            get { return float.Parse(hjAttributeValues[6]); }
            set { hjAttributeValues[6] = value.ToString(); }
        }

        private int sceneOrbitPath;
        //景path，例如15
        public int SceneOrbitPath
        {
            get { return int.Parse(hjAttributeValues[7]); }
            set { hjAttributeValues[7] = value.ToString(); }
        }

        private int sceneOrbitRow;
        //景row，例如76
        public int SceneOrbitRow
        {
            get { return int.Parse(hjAttributeValues[8]); }
            set { hjAttributeValues[8] = value.ToString(); }
        }

        private DateTime sceneDate;
        //数据接收时间，例如2010-06-13 04:04:18.13
        public DateTime SceneDate
        {
            
            get {
                DateTime dt = Convert.ToDateTime(hjAttributeValues[9], CultureInfo.CurrentCulture);
                return dt;
               }
            set { hjAttributeValues[9] = value.ToString(); }
        }

        private string earthModel;
        //椭球，例如WGS_84
        public string EarthModel
        {
            get { return hjAttributeValues[10]; }
            set { hjAttributeValues[10] = value; }
        }

        private string mapProjection;
        //投影，例如UTM
        public string MapProjection
        {
            get { return hjAttributeValues[11]; }
            set { hjAttributeValues[11] = value; }
        }

        private string zone;
        //带号，例如48N
        public string Zone
        {
            get { return hjAttributeValues[12]; }
            set { hjAttributeValues[12] = value; }
        }

        private string resampleTechnique;
        //重采样方法，例如BL
        public string ResampleTechnique
        {
            get { return hjAttributeValues[13]; }
            set { hjAttributeValues[13] = value; }
        }

        private string dataFormatDes;
        //数据格式，例如GEOTIFF
        public string DataFormatDes
        {
            get { return hjAttributeValues[14]; }
            set { hjAttributeValues[14] = value; }
        }

        private double sunElevation;
        //太阳高度角，例如73.745
        public double SunElevation
        {
            get { return double.Parse(hjAttributeValues[15]); }
            set { hjAttributeValues[15] = value.ToString(); }
        }

        private double sunAzimuthElevation;
        //太阳方位角，例如300.965
        public double SunAzimuthElevation
        {
            get { return double.Parse(hjAttributeValues[16]); }
            set { hjAttributeValues[16] = value.ToString(); }
        }

        private double dataUpperLeftLat;
        //左上角纬度，例如34.401465
        public double DataUpperLeftLat
        {
            get { return double.Parse(hjAttributeValues[17]); }
            set { hjAttributeValues[17] = value.ToString(); }
        }

        private double dataUpperLeftLong;
        //左上角经度，例如102.227978
        public double DataUpperLeftLong
        {
            get { return double.Parse(hjAttributeValues[18]); }
            set { hjAttributeValues[18] = value.ToString(); }
        }

        private double dataUpperRightLat;
        //右上角纬度，例如33.678849
        public double DataUpperRightLat
        {
            get { return double.Parse(hjAttributeValues[19]); }
            set { hjAttributeValues[19] = value.ToString(); }
        }

        private double dataUpperRightLong;
        //右上角经度，例如106.693466
        public double DataUpperRightLong
        {
            get { return double.Parse(hjAttributeValues[20]); }
            set { hjAttributeValues[20] = value.ToString(); }
        }

        private double dataLowerRightLat;
        //右下角纬度，例如30.537453
        public double DataLowerRightLat
        {
            get { return double.Parse(hjAttributeValues[21]); }
            set { hjAttributeValues[21] = value.ToString(); }
        }

        private double dataLowerRightLong;
        //右下角经度，例如105.712068
        public double DataLowerRightLong
        {
            get { return double.Parse(hjAttributeValues[22]); }
            set { hjAttributeValues[22] = value.ToString(); }
        }

        private double dataLowerLeftLat;
        //左下角纬度，例如31.230519
        public double DataLowerLeftLat
        {
            get { return double.Parse(hjAttributeValues[23]); }
            set { hjAttributeValues[23] = value.ToString(); }
        }

        private double dataLowerLeftLong;
        //左下角经度，例如101.387694
        public double DataLowerLeftLong
        {
            get { return double.Parse(hjAttributeValues[24]); }
            set { hjAttributeValues[24] = value.ToString(); }
        }

        private string sourcePath;
        //原数据路径，例如"D:\...."
        public string SourcePath
        {
            get { return hjAttributeValues[25]; }
            set { hjAttributeValues[25] = value; }
        }

        private string overviewFilePath;
        //缩略图路径，例如"D:\abc\cde.jpg"
        public string OverviewFilePath
        {
            get { return hjAttributeValues[26]; }
            set { hjAttributeValues[26] = value; }
        }

        private string corDataPath="-1";
        //纠正后数据路径，例如"\\192.168.1.122\23\",无则为-1
        public string CorDataPath
        {
            get { return hjAttributeValues[27]; }
            set { hjAttributeValues[27] = value; }
        }
        #endregion

        #region Method
        public MetaDataHj()
        {
            _dataType = EnumMetadataTypes.HJ;
            hjAttributeValues = new string[hjAttributeNames.Length];
        }
        /// <summary>
        /// 读取CBERS和HJ数据XML中属性
        /// </summary>
        /// <param name="fileName">XML文件名</param>
        /// <param name="otherParameters">其它属性值,如xml中没有的“文件路径”等</param>
        /// <param name="output_attribute_string">输出属性值</param>
        public void readCbersHjAttribute(string fileName, string[] otherParameters,out string[] outputAttributeValues)
        {
            XmlDocument root = new XmlDocument();
            try
            {
                root.Load(fileName);
            }
            catch (System.Exception ex)
            {
                throw new Exception("xml文件损坏，请检查！");
            }
            XmlNode node = null;
            
            string[] attributeValues = new string[hjAttributeNames.Length];
            for (int i = 0; i < hjAttributeNames.Length; i++)
                if (i < hjAttributeNames.Length - otherParameters.Length)
                {
                    node = root.GetElementsByTagName(hjAttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        attributeValues[i] = "无";
                    }
                    else
                    {
                        attributeValues[i] = node.InnerText;
                    }
                }
                else
                    attributeValues[i] = otherParameters[i - hjAttributeNames.Length + otherParameters.Length];     
            outputAttributeValues = attributeValues;
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NAME,STORAGESITE,SATELLITE,RECSTATION,PRODUCTID,SCENEID,PRODUCTLEVEL,SENSOR,PIXELSPACING,SCENEORBITPATH,SCENEORBITROW,SCENEDATE,EARTHMODEL,MAPPROJECTION,ZONE,RESAMPLETECHNIQUE,DATAFORMATDES,SUNELEVATION,SUNAZIMUTHELEVATION,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,OVERVIEWFILEPATH,QRST_CODE,CorDataPath from prod_hj ");
            strSql.AppendFormat(string.Format(" where QRST_CODE='{0}' ",qrst_code));
            DataSet ds = sqlBase.GetDataSet(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                NAME = ds.Tables[0].Rows[0]["NAME"].ToString();
                Satellite = ds.Tables[0].Rows[0]["SATELLITE"].ToString();
                RecStation = ds.Tables[0].Rows[0]["RECSTATION"].ToString();
                ProductID = long.Parse(ds.Tables[0].Rows[0]["PRODUCTID"].ToString());
                ProductLevel= ds.Tables[0].Rows[0]["PRODUCTLEVEL"].ToString();
                Sensor = ds.Tables[0].Rows[0]["SENSOR"].ToString();
                if (ds.Tables[0].Rows[0]["PIXELSPACING"].ToString() != "")
                {
                    PixelSpacing = float.Parse(ds.Tables[0].Rows[0]["PIXELSPACING"].ToString());
                }
                SceneOrbitPath= int.Parse(ds.Tables[0].Rows[0]["SCENEORBITPATH"].ToString());
                if (ds.Tables[0].Rows[0]["SCENEORBITROW"].ToString() != "")
                {
                    SceneOrbitRow = int.Parse(ds.Tables[0].Rows[0]["SCENEORBITROW"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SCENEDATE"].ToString() != "")
                {
                   SceneDate= Convert.ToDateTime(ds.Tables[0].Rows[0]["SCENEDATE"].ToString());
                }
                EarthModel = ds.Tables[0].Rows[0]["EARTHMODEL"].ToString();
                MapProjection = ds.Tables[0].Rows[0]["MAPPROJECTION"].ToString();
                Zone= ds.Tables[0].Rows[0]["ZONE"].ToString();
                ResampleTechnique= ds.Tables[0].Rows[0]["RESAMPLETECHNIQUE"].ToString();
                DataFormatDes = ds.Tables[0].Rows[0]["DATAFORMATDES"].ToString();
                if (ds.Tables[0].Rows[0]["SUNELEVATION"].ToString() != "")
                {
                    SunElevation= float.Parse(ds.Tables[0].Rows[0]["SUNELEVATION"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SUNAZIMUTHELEVATION"].ToString() != "")
                {
                   SunAzimuthElevation= float.Parse(ds.Tables[0].Rows[0]["SUNAZIMUTHELEVATION"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATAUPPERLEFTLAT"].ToString() != "")
                {
                   DataUpperLeftLat = float.Parse(ds.Tables[0].Rows[0]["DATAUPPERLEFTLAT"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATAUPPERLEFTLONG"].ToString() != "")
                {
                    DataUpperLeftLong = float.Parse(ds.Tables[0].Rows[0]["DATAUPPERLEFTLONG"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATAUPPERRIGHTLAT"].ToString() != "")
                {
                    DataUpperRightLat = float.Parse(ds.Tables[0].Rows[0]["DATAUPPERRIGHTLAT"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATAUPPERRIGHTLONG"].ToString() != "")
                {
                    DataUpperRightLong = float.Parse(ds.Tables[0].Rows[0]["DATAUPPERRIGHTLONG"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATALOWERRIGHTLAT"].ToString() != "")
                {
                    DataLowerRightLat = float.Parse(ds.Tables[0].Rows[0]["DATALOWERRIGHTLAT"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATALOWERRIGHTLONG"].ToString() != "")
                {
                    DataLowerRightLong = float.Parse(ds.Tables[0].Rows[0]["DATALOWERRIGHTLONG"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATALOWERLEFTLAT"].ToString() != "")
                {
                    DataLowerLeftLat = float.Parse(ds.Tables[0].Rows[0]["DATALOWERLEFTLAT"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DATALOWERLEFTLONG"].ToString() != "")
                {
                    DataLowerLeftLong = float.Parse(ds.Tables[0].Rows[0]["DATALOWERLEFTLONG"].ToString());
                }
                QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                CorDataPath = ds.Tables[0].Rows[0]["CorDataPath"].ToString();
                IsCreated = true;
            }
            else
            {
                IsCreated = false;
            }
        }

        /// <summary>
        /// 构建环境星数据的存储相对路径
        /// 实验验证数据库\环境卫星数据\HJ1A\CCD1\2008\09\28\HJ1A-CCD1-1-60-20080928-L10000004570
        /// </summary>
        /// <returns></returns>
        public override string GetRelateDataPath()
        {
            string sourceFileNameWithoutExt = (NAME.Length > 0) ?
            Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(NAME)) : "";

            //构建存储路径
            string[] namefeature = sourceFileNameWithoutExt.Split('-');
            string satellite = "";
            string sensor = "";
            string year = "";
            string month = "";
            string day = "";

            if (namefeature.Length == 6)
            {
                //HJ1B-CCD1-454-68-20100204-L20000246816.tar.gz
                satellite = namefeature[0];
                sensor = namefeature[1];
                year = namefeature[4].Substring(0, 4);
                month = namefeature[4].Substring(4, 2);
                day = namefeature[4].Substring(6, 2);
            }
            else if (namefeature.Length == 7)
            {
                //HJ1A-HSI-1-65-B2-20110816-L20000595562.tar.gz"
                satellite = namefeature[0];
                sensor = namefeature[1];
                year = namefeature[5].Substring(0, 4);
                month = namefeature[5].Substring(4, 2);
                day = namefeature[5].Substring(6, 2);
            }
            string relatePath = @"实验验证数据库\环境卫星数据\";
            if(!string.IsNullOrEmpty(satellite))
            {
                 relatePath = string.Format(@"{0}{1}\",relatePath, satellite);
            }
            if(!string.IsNullOrEmpty(sensor))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, sensor);
            }
            if (!string.IsNullOrEmpty(year))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, year);
            }
            if (!string.IsNullOrEmpty(month))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, month);
            }
            if (!string.IsNullOrEmpty(day))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, day);
            }

            string StorePath_SourceProject = string.Format(@"{0}{1}\", relatePath, sourceFileNameWithoutExt);
            return StorePath_SourceProject;
        }

        public string GetCorrectedDataPath()
        {
            string sourceFileNameWithoutExt = (NAME.Length > 0) ?
           Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(NAME)) : "";

            //构建存储路径
            string[] namefeature = sourceFileNameWithoutExt.Split('-');
            string satellite = "";
            string sensor = "";
            string year = "";
            string month = "";
            string day = "";

            if (namefeature.Length == 6)
            {
                //HJ1B-CCD1-454-68-20100204-L20000246816.tar.gz
                satellite = namefeature[0];
                sensor = namefeature[1];
                year = namefeature[4].Substring(0, 4);
                month = namefeature[4].Substring(4, 2);
                day = namefeature[4].Substring(6, 2);
            }
            else if (namefeature.Length == 7)
            {
                //HJ1A-HSI-1-65-B2-20110816-L20000595562.tar.gz"
                satellite = namefeature[0];
                sensor = namefeature[1];
                year = namefeature[5].Substring(0, 4);
                month = namefeature[5].Substring(4, 2);
                day = namefeature[5].Substring(6, 2);
            }
            string relatePath = @"数据产品库\数据预处理产品\环境卫星数据\";
            if (!string.IsNullOrEmpty(satellite))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, satellite);
            }
            if (!string.IsNullOrEmpty(sensor))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, sensor);
            }
            if (!string.IsNullOrEmpty(year))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, year);
            }
            if (!string.IsNullOrEmpty(month))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, month);
            }
            if (!string.IsNullOrEmpty(day))
            {
                relatePath = string.Format(@"{0}{1}\", relatePath, day);
            }

            string StorePath_SourceProject = string.Format(@"{0}{1}\", relatePath, sourceFileNameWithoutExt);
            return StorePath_SourceProject;
        }

        public override void ReadAttributes(string fileName)
        {
            XmlDocument root = new XmlDocument();
            try
            {
                root.Load(fileName);
            }
            catch (System.Exception ex)
            {
                throw new Exception("xml文件损坏，请检查！");
            }
            XmlNode node = null;

            try
            {
                for (int i = 0; i < hjAttributeNames.Length; i++)
                {
                    node = root.GetElementsByTagName(hjAttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        hjAttributeValues[i] = "";
                    }
                    else
                    {
                        hjAttributeValues[i] = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("prod_hj",EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_hj where Name ='{0}'", NAME);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString()).ToString();
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_hj where QRST_CODE ='{0}'", QRST_CODE);
                    //DataSet ds = sqlBase.GetDataSet(presql);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_hj").ToString();
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_hj", int.Parse(ID));
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_hj(");
                strSql.Append("ID,NAME,STORAGESITE,SATELLITE,RECSTATION,PRODUCTID,SCENEID,PRODUCTLEVEL,SENSOR,PIXELSPACING,SCENEORBITPATH,SCENEORBITROW,SCENEDATE,EARTHMODEL,MAPPROJECTION,ZONE,RESAMPLETECHNIQUE,DATAFORMATDES,SUNELEVATION,SUNAZIMUTHELEVATION,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,OVERVIEWFILEPATH,QRST_CODE,CorDataPath)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}',{11},'{12}','{13}','{14}','{15}','{16}','{17}',{18}," +
                        "{19},{20},{21},{22},{23},{24},{25},{26},{27},'{28}','{29}','{30}')",
                        ID, NAME, "", Satellite, recStation, ProductID, SenceID, ProductLevel, Sensor, PixelSpacing,
                        SceneOrbitPath, SceneOrbitRow, SceneDate.ToString("yyyy-MM-dd HH:mm:ss"), EarthModel, MapProjection, Zone, ResampleTechnique,
                        DataFormatDes, SunElevation, SunAzimuthElevation, DataUpperLeftLat, DataUpperLeftLong,
                        dataUpperRightLat,
                        DataUpperRightLong, DataLowerRightLat, DataLowerRightLong, DataLowerLeftLat, DataLowerLeftLong,
                        "", QRST_CODE, "-1"));
                //           strSql.Append("@ID,@NAME,@STORAGESITE,@SATELLITE,@RECSTATION,@PRODUCTID,@SCENEID,@PRODUCTLEVEL,@SENSOR,@PIXELSPACING,@SCENEORBITPATH,@SCENEORBITROW,@SCENEDATE,@EARTHMODEL,@MAPPROJECTION,@ZONE,@RESAMPLETECHNIQUE,@DATAFORMATDES,@SUNELEVATION,@SUNAZIMUTHELEVATION,@DATAUPPERLEFTLAT,@DATAUPPERLEFTLONG,@DATAUPPERRIGHTLAT,@DATAUPPERRIGHTLONG,@DATALOWERRIGHTLAT,@DATALOWERRIGHTLONG,@DATALOWERLEFTLAT,@DATALOWERLEFTLONG,@OVERVIEWFILEPATH,@QRST_CODE,@CorDataPath)");
                //           MySqlParameter[] parameters = {
                //new MySqlParameter("@ID", MySqlDbType.Decimal,20),
                //new MySqlParameter("@NAME", MySqlDbType.Text),
                //new MySqlParameter("@STORAGESITE", MySqlDbType.Text),
                //new MySqlParameter("@SATELLITE", MySqlDbType.Text),
                //new MySqlParameter("@RECSTATION", MySqlDbType.Text),
                //new MySqlParameter("@PRODUCTID", MySqlDbType.Text),
                //new MySqlParameter("@SCENEID", MySqlDbType.Text),
                //new MySqlParameter("@PRODUCTLEVEL", MySqlDbType.Text),
                //new MySqlParameter("@SENSOR", MySqlDbType.Text),
                //new MySqlParameter("@PIXELSPACING", MySqlDbType.Decimal,10),
                //new MySqlParameter("@SCENEORBITPATH", MySqlDbType.Text),
                //new MySqlParameter("@SCENEORBITROW", MySqlDbType.Decimal,10),
                //new MySqlParameter("@SCENEDATE", MySqlDbType.DateTime),
                //new MySqlParameter("@EARTHMODEL", MySqlDbType.Text),
                //new MySqlParameter("@MAPPROJECTION", MySqlDbType.Text),
                //new MySqlParameter("@ZONE", MySqlDbType.Text),
                //new MySqlParameter("@RESAMPLETECHNIQUE", MySqlDbType.Text),
                //new MySqlParameter("@DATAFORMATDES", MySqlDbType.Text),
                //new MySqlParameter("@SUNELEVATION", MySqlDbType.Decimal,10),
                //new MySqlParameter("@SUNAZIMUTHELEVATION", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATAUPPERLEFTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATAUPPERLEFTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATAUPPERRIGHTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATAUPPERRIGHTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERRIGHTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERRIGHTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERLEFTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERLEFTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@OVERVIEWFILEPATH", MySqlDbType.Text),
                //new MySqlParameter("@QRST_CODE", MySqlDbType.Text),
                //new MySqlParameter("@CorDataPath", MySqlDbType.VarChar,500)};
                //           parameters[0].Value = ID;
                //           parameters[1].Value = NAME;
                //           parameters[2].Value = "";
                //           parameters[3].Value = Satellite;
                //           parameters[4].Value = recStation;
                //           parameters[5].Value = ProductID;
                //           parameters[6].Value = SenceID;
                //           parameters[7].Value = ProductLevel;
                //           parameters[8].Value = Sensor;
                //           parameters[9].Value = PixelSpacing;
                //           parameters[10].Value = SceneOrbitPath;
                //           parameters[11].Value = SceneOrbitRow;
                //           parameters[12].Value = SceneDate;
                //           parameters[13].Value = EarthModel;
                //           parameters[14].Value = MapProjection;
                //           parameters[15].Value = Zone;
                //           parameters[16].Value = ResampleTechnique;
                //           parameters[17].Value = DataFormatDes;
                //           parameters[18].Value = SunElevation;
                //           parameters[19].Value = SunAzimuthElevation;
                //           parameters[20].Value = DataUpperLeftLat;
                //           parameters[21].Value = DataUpperLeftLong;
                //           parameters[22].Value = dataUpperRightLat;
                //           parameters[23].Value = DataUpperRightLong;
                //           parameters[24].Value = DataLowerRightLat;
                //           parameters[25].Value = DataLowerRightLong;
                //           parameters[26].Value = DataLowerLeftLat;
                //           parameters[27].Value = DataLowerLeftLong;
                //           parameters[28].Value = "";
                //           parameters[29].Value = QRST_CODE;
                //           parameters[30].Value = "-1";

                sqlBase.ExecuteSql(strSql.ToString());

                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string destCorrectedData = this.GetCorrectedDataPath();
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_hj set CorDataFlag = {0} where Name = '{1}'", corDataPath, NAME);
                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.LockTable("prod_hj",EnumDBType.MIDB);
            }
            catch(Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }
        }
        #endregion
        //HJ1A-CCD1-1-60-L20000029776.tar.gz
    }
}
