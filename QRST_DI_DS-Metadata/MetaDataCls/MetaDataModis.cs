using System;
using System.Text;
using System.Globalization;
using DotSpatial.Data.Rasters.GdalExtension;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;


namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataModis : MetaData
    {
        //string[] modisAttributeNames = {
        //                                   "ASSOCIATEDPLATFORMSHORTNAME",//Terra
        //                                   "ASSOCIATEDSENSORSHORTNAME",//MODIS
        //                                   "LOCALGRANULEID",//MOD021KM.A2009153.0400.005.2009153140357.hdf
        //                                   "ANCILLARYINPUTPOINTER",//MOD03.A2009153.0400.005.2009153115540.hdf
        //                                   "ORBITNUMBER",//50296
        //                                   "DAYNIGHTFLAG",//Day
        //                               };
        public string[] modisAttributeNames = {
                                           "satellite",
                                           "sensor",
                                           "rawFile",
                                           "ancillaryFile",
                                           "productStyle",
                                           "orbitNumber",
                                           "dayNightFlag",
                                           "beginDate",
                                           "endDate",
                                           "dataUpperLeftLat",
                                           "dataUpperLeftLong",
                                           "dataUpperRightLat",
                                           "dataUpperRightLong",
                                           "dataLowerRightLat",
                                           "dataLowerRightLong",
                                           "dataLowerLeftLat",
                                           "dataLowerLeftLong",
                                           "rawFilePath",
                                           "ancillaryFilePath",
                                           "overviewFilePath",
                                           "NAME",
                                           "MODTYPE"
                                       };
        public string[] modisAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties
   

        private string satellite;
        //卫星名，例如Tera
        public string Satellite
        {
            get { return modisAttributeValues[0]; }
            set { modisAttributeValues[0] = value; }
        }

        private string sensor;
        //传感器名，例如MODIS
        public string Sensor
        {
            get { return modisAttributeValues[1]; }
            set { modisAttributeValues[1] = value; }
        }

        private string rawFile;
        //原文件名，例如MOD021KM.A2009153.0400.005.2009153140357.hdf
        public string RawFile
        {
            get { return modisAttributeValues[2]; }
            set { modisAttributeValues[2] = value; }
        }

        private string ancillaryFile;
        //定位文件名，例如MOD03.A2009153.0400.005.2009153115540.hdf
        public string AncillaryFile
        {
            get { return modisAttributeValues[3]; }
            set { modisAttributeValues[3] = value; }
        }

        private string productStyle;
        //产品类别，例如MOD021KM
        public string ProductStyle
        {
            get { return modisAttributeValues[4]; }
            set { modisAttributeValues[4] = value; }
        }

        private int orbitNumber;
        //轨道号，例如50296
        public int OrbitNumber
        {
            get { return int.Parse(modisAttributeValues[5]); }
            set { modisAttributeValues[5] = value.ToString(); }
        }

        private string dayNightFlag;
        //日夜标识，例如Day
        public string DayNightFlag
        {
            get { return modisAttributeValues[6]; }
            set { modisAttributeValues[6] = value; }
        }

        private DateTime beginDate;
        //数据开始日期，例如2009-06-02 04:00:00.000000
        public DateTime BeginDate
        {
            get {
                DateTime dt= Convert.ToDateTime(modisAttributeValues[7], CultureInfo.CurrentCulture);
                return dt; }
            set { modisAttributeValues[7] = value.ToString(); }
        }

        private DateTime endDate;
        //数据结束日期，例如2009-06-02 04:05:00.000000
        public DateTime EndDate
        {
            get {
                DateTime dt= Convert.ToDateTime(modisAttributeValues[8], CultureInfo.CurrentCulture);
                return dt;
            }
            set { modisAttributeValues[8] = value.ToString(); }
        }

        private double dataUpperLeftLat;
        //左上角纬度，例如45.6491404992752
        public double DataUpperLeftLat
        {
            get { return double.Parse(modisAttributeValues[9]); }
            set { modisAttributeValues[9] = value.ToString(); }
        }

        private double dataUpperLeftLong;
        //左上角经度，例如90.7374083148379
        public double DataUpperLeftLong
        {
            get { return double.Parse(modisAttributeValues[10]); }
            set { modisAttributeValues[10] = value.ToString(); }
        }

        private double dataUpperRightLat;
        //右上角纬度，例如41.5088415616411
        public double DataUpperRightLat
        {
            get { return double.Parse(modisAttributeValues[11]); }
            set { modisAttributeValues[11] = value.ToString(); }
        }

        private double dataUpperRightLong;
        //右上角经度，例如119.413645680131
        public double DataUpperRightLong
        {
            get { return double.Parse(modisAttributeValues[12]); }
            set { modisAttributeValues[12] = value.ToString(); }
        }

        private double dataLowerRightLat;
        //右下角纬度，例如24.2162765376915
        public double DataLowerRightLat
        {
            get { return double.Parse(modisAttributeValues[13]); }
            set { modisAttributeValues[13] = value.ToString(); }
        }

        private double dataLowerRightLong;
        //右下角经度，例如122.06409144538
        public double DataLowerRightLong
        {
            get { return double.Parse(modisAttributeValues[14]); }
            set { modisAttributeValues[14] = value.ToString(); }
        }

        private double dataLowerLeftLat;
        //左下角纬度，例如27.3766788230445
        public double DataLowerLeftLat
        {
            get { return double.Parse(modisAttributeValues[15]); }
            set { modisAttributeValues[15] = value.ToString(); }
        }

        private double dataLowerLeftLong;
        //左下角经度，例如88.8646993862138
        public double DataLowerLeftLong
        {
            get { return double.Parse(modisAttributeValues[16]); }
            set { modisAttributeValues[16] = value.ToString(); }
        }

        private string rawFilePath;
        //原数据路径，例如"D:\...."
        public string RawFilePath
        {
            get { return modisAttributeValues[17]; }
            set { modisAttributeValues[17] = value; }
        }

        private string ancillaryFilePath;
        //辅助数据路径，例如"D:\..."
        public string AncillaryFilePath
        {
            get { return modisAttributeValues[18]; }
            set { modisAttributeValues[18] = value; }
        }

        private string overviewFilePath;
        //缩略图路径，例如"D:\abc\cde.jpg"
        public string OverviewFilePath
        {
            get { return modisAttributeValues[19]; }
            set { modisAttributeValues[19] = value; }
        }

        public string NAME
        {
            get { return modisAttributeValues[20]; }
            set { modisAttributeValues[20] = value; }
        }

        public string MODTYPE
        {
            get { return modisAttributeValues[21]; }
            set { modisAttributeValues[20] = value; }
        }

        #endregion

        #region Method
        public MetaDataModis()
        {
            _dataType = EnumMetadataTypes.MODIS;
            modisAttributeValues = new string[modisAttributeNames.Length];
        }
        /// <summary>
        /// 读取MODIS数据中属性
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="otherParameters">其它属性值,如hdf中没有的“文件路径”等</param>
        /// <param name="output_attribute_string">输出属性值</param>
        public void readModisAttribute(string fileName, string[] otherParameters, out string[] outputAttributeValues)
        {
            GdalConfiguration.ConfigureGdal();
            //Gdal.AllRegister();
            string[] metadata = null;

            try
            {
                metadata = GdalUtility.GetMetadate(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("打开hdf文件错误，请检验Modis数据文件是否损坏！");
            }

            string str1 = null, str2 = null;
            string lon = null;
            int a, b;
            //name
            modisAttributeValues[20] = Path.GetFileNameWithoutExtension(fileName); 
            //modtype
            if (modisAttributeValues[20].StartsWith("MOD03"))
            {
                modisAttributeValues[21] = "MOD03";
            }
            else if (modisAttributeValues[20].StartsWith("MOBRGB"))
            {
                modisAttributeValues[21] = "MODRGB";
            }
            else
            {
                modisAttributeValues[21] = modisAttributeValues[20].Substring(0, modisAttributeValues[20].IndexOf('.'));
                //设置辅助文件和缩略图文件在数据阵列里面的基础路径,\\172.16.0.1\综合数据库\实验验证库\MODIS\MOBRGB\2003\12\03\  视图中加上文件名称为文件名的文件和最后的文件名
            }
            //satellite
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("ASSOCIATEDPLATFORMSHORTNAME"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    modisAttributeValues[0] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //sensor
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("ASSOCIATEDSENSORSHORTNAME"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    modisAttributeValues[1] = metadata[i].Substring(b).Split(' ')[0];
                    break;
                }
            }
            //rawFile
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("LOCALGRANULEID"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    modisAttributeValues[2] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //ancillaryFile
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("ANCILLARYINPUTPOINTER"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    modisAttributeValues[3] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //productStyle
            modisAttributeValues[4] = modisAttributeValues[2].Split('.')[0];
            //orbitNumber
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("ORBITNUMBER"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    modisAttributeValues[5] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //dayNightFlag
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("DAYNIGHTFLAG"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    modisAttributeValues[6] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //beginDate
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("RANGEBEGINNINGDATE"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str1 = metadata[i].Substring(b).ToString();
                }
                if (sm.Contains("RANGEBEGINNINGTIME"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str2 = metadata[i].Substring(b).ToString();
                }

            }
            modisAttributeValues[7] = str1 + " " + str2;
            //endDate
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("RANGEENDINGDATE"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str1 = metadata[i].Substring(b).ToString();
                }
                if (sm.Contains("RANGEENDINGTIME"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str2 = metadata[i].Substring(b).ToString();
                }

            }
            modisAttributeValues[8] = str1 + " " + str2;
            //dataUpperLeftLat,dataUpperLeftLong, dataUpperRightLat,dataUpperRightLong
            //dataLowerRightLat,dataLowerRightLong,dataLowerLeftLat,dataLowerLeftLong
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("GRINGPOINTLATITUDE"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str1 = metadata[i].Substring(b).ToString();
                }
            }
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("GRINGPOINTLONGITUDE"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str2 = metadata[i].Substring(b).ToString();
                }
            }
            string[] commaLat = str1.Split(',');
            string[] commaLong = str2.Split(',');
            modisAttributeValues[9] = commaLat[0];
            modisAttributeValues[10] = commaLong[0];
            modisAttributeValues[11] = commaLat[1];
            modisAttributeValues[12] = commaLong[1];
            modisAttributeValues[13] = commaLat[2];
            modisAttributeValues[14] = commaLong[2];
            modisAttributeValues[15] = commaLat[3];
            modisAttributeValues[16] = commaLong[3];

            //rawFilePath
            modisAttributeValues[17] = otherParameters[0];
            //ancillaryFilePath
            modisAttributeValues[18] = otherParameters[1];
            //overviewFilePath
            modisAttributeValues[19] = otherParameters[2];

            outputAttributeValues = modisAttributeValues;
        }

        /// <summary>
        /// 读取Modis数据的元数据信息。zxw 2013/06/15
        /// </summary>
        /// <param name="fileName"></param>
        public override void ReadAttributes(string dataFile)
        {
            base.ReadAttributes(dataFile);

            string[] paths = dataFile.Split("\\".ToCharArray());
            string datapath = paths[paths.Length - 1].ToString();
            if (datapath.StartsWith("MOBRGB"))//缩略图文件 MOBRGB.A2009255.0320.005.2010341202801.jpg
            {
                BeginDate = Convert.ToDateTime(TimeConvert.datetimeConvert(datapath.Substring(8, 4), datapath.Substring(12, 3)), CultureInfo.CurrentCulture);
            }


            //文件类型检查
            if (System.IO.Path.GetExtension(dataFile) != ".hdf")
            {
                throw new Exception("MetaDataUtilities:Modis数据类型输入错误。");
            }
            string ancillaryFile = "";
            string overViewFile = "";
            //读取元数据，并赋值
            string[] otherParameters = {
                                           //原文件路径
                                           dataFile.Substring(0,dataFile.Length-4),
                                           //辅助数据路径
                                           ancillaryFile,
                                           //缩略图路径
                                           overViewFile,
                                       };
            try
            {
                readModisAttribute(dataFile, otherParameters, out modisAttributeValues);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            if (Satellite == null || Satellite.Equals(""))
            {
                throw new Exception("缺少卫星信息！！");
            }
            else if (Sensor == null || Sensor.Equals(""))
            {
                throw new Exception("缺少传感器信息！！");
            }
            else if (BeginDate == null || BeginDate.Equals("") || EndDate == null || EndDate.Equals(""))
            {
                throw new Exception("缺少成像时间信息！！");
            }
            for (int i = 9; i < 17; i++)
            {
                string str = modisAttributeValues[i];
                if (str == null || str.Equals(""))
                {
                    throw new Exception(String.Format("元数据信息不全，缺少{0}信息", modisAttributeNames[i]));
                }
            }
        }

        /// <summary>
        /// 导入Modis元数据信息      zxw 2013/06/15
        /// </summary>
        /// <param name="sqlBase"></param>
        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            //TableLocker dblock = new TableLocker(sqlBase);
            Constant.IdbOperating.LockTable("prod_modis",EnumDBType.MIDB);
            tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
            int ID = sqlBase.GetMaxID("ID", "prod_modis");
            QRST_CODE = tablecode.GetDataQRSTCode("prod_modis", ID);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into prod_modis(");
            strSql.Append("ID,STORAGESITE,NAME,BEGINDATE,ENDDATE,satellite,sensor,productStyle,orbitNumber,dayNightFlag,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,overviewFilePath,ancillaryFilePath,DESCRIPTION,QRST_CODE,MODTYPE)");
            strSql.Append(" values (");
            strSql.Append(
                string.Format(
                    "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},{11},{12},{13},{14},{15},{16},{17},'{18}'," +
                    "'{19}','{20}','{21}','{22}')",
                    ID, "", NAME, BeginDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"), Satellite, Sensor, ProductStyle, OrbitNumber, DayNightFlag,
                    DataUpperLeftLat, DataUpperLeftLong, DataUpperRightLat, DataUpperRightLong, DataLowerRightLat,
                    DataLowerRightLong,
                    DataLowerLeftLat, DataLowerLeftLong, overviewFilePath, ancillaryFilePath, "", QRST_CODE, MODTYPE));
            //       strSql.Append("@ID,@STORAGESITE,@NAME,@BEGINDATE,@ENDDATE,@satellite,@sensor,@productStyle,@orbitNumber,@dayNightFlag,@DATAUPPERLEFTLAT,@DATAUPPERLEFTLONG,@DATAUPPERRIGHTLAT,@DATAUPPERRIGHTLONG,@DATALOWERRIGHTLAT,@DATALOWERRIGHTLONG,@DATALOWERLEFTLAT,@DATALOWERLEFTLONG,@overviewFilePath,@ancillaryFilePath,@DESCRIPTION,@QRST_CODE,@MODTYPE)");
            //       MySqlParameter[] parameters = {
            //new MySqlParameter("@ID", MySqlDbType.Decimal,20),
            //new MySqlParameter("@STORAGESITE", MySqlDbType.Text),
            //new MySqlParameter("@NAME", MySqlDbType.Text),
            //new MySqlParameter("@BEGINDATE", MySqlDbType.DateTime),
            //new MySqlParameter("@ENDDATE", MySqlDbType.DateTime),
            //new MySqlParameter("@satellite", MySqlDbType.Text),
            //new MySqlParameter("@sensor", MySqlDbType.Text),
            //new MySqlParameter("@productStyle", MySqlDbType.Text),
            //new MySqlParameter("@orbitNumber", MySqlDbType.Text),
            //new MySqlParameter("@dayNightFlag", MySqlDbType.Text),
            //new MySqlParameter("@DATAUPPERLEFTLAT", MySqlDbType.Decimal,10),
            //new MySqlParameter("@DATAUPPERLEFTLONG", MySqlDbType.Decimal,10),
            //new MySqlParameter("@DATAUPPERRIGHTLAT", MySqlDbType.Decimal,10),
            //new MySqlParameter("@DATAUPPERRIGHTLONG", MySqlDbType.Decimal,10),
            //new MySqlParameter("@DATALOWERRIGHTLAT", MySqlDbType.Decimal,10),
            //new MySqlParameter("@DATALOWERRIGHTLONG", MySqlDbType.Decimal,10),
            //new MySqlParameter("@DATALOWERLEFTLAT", MySqlDbType.Decimal,10),
            //new MySqlParameter("@DATALOWERLEFTLONG", MySqlDbType.Decimal,10),
            //new MySqlParameter("@overviewFilePath", MySqlDbType.Text),
            //new MySqlParameter("@ancillaryFilePath", MySqlDbType.Text),
            //new MySqlParameter("@DESCRIPTION", MySqlDbType.Text),
            //new MySqlParameter("@QRST_CODE", MySqlDbType.Text),
            //new MySqlParameter("@MODTYPE", MySqlDbType.Text)};
            //       parameters[0].Value = ID;
            //       parameters[1].Value = "";
            //       parameters[2].Value = NAME;
            //       parameters[3].Value = BeginDate;
            //       parameters[4].Value = EndDate;
            //       parameters[5].Value = Satellite;
            //       parameters[6].Value = Sensor;
            //       parameters[7].Value = ProductStyle;
            //       parameters[8].Value = OrbitNumber;
            //       parameters[9].Value = DayNightFlag;
            //       parameters[10].Value = DataUpperLeftLat;
            //       parameters[11].Value = DataUpperLeftLong;
            //       parameters[12].Value = DataUpperRightLat;
            //       parameters[13].Value = DataUpperRightLong;
            //       parameters[14].Value = DataLowerRightLat;
            //       parameters[15].Value = DataLowerRightLong;
            //       parameters[16].Value = DataLowerLeftLat;
            //       parameters[17].Value = DataLowerLeftLong;
            //       parameters[18].Value = overviewFilePath;
            //       parameters[19].Value = ancillaryFilePath;
            //       parameters[20].Value = "";
            //       parameters[21].Value = QRST_CODE;
            //       parameters[22].Value = MODTYPE;

            sqlBase.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("prod_modis",EnumDBType.MIDB);
        }

        /// <summary>
        /// modis数据组织规则 zxw 20130615
        /// 数据样例：“MOD02HKM.A2000057.0325.005.2006355073325.hdf”
        /// 数据组织：产品缩写\数据获取时间   MOD02HKM\2006
        /// 修改信息：zxw 20130625  内容：将路径组织规则改为原有的路径组织规则
        /// </summary>
        /// <returns></returns>
        public override string GetRelateDataPath()
        {
            //string relatePath = "";
            //string[] pathArr = NAME.Split(".".ToCharArray());
            //string productYear ;  //产品生产年
            //if (pathArr.Length == 5)
            //{
            //    productYear = pathArr[4].Substring(0, 4);
            //}
            //else
            //{
            //    throw new Exception("Modis数据命名不规范！");
            //}
            //relatePath = string.Format(@"{0}\{1}\{2}",pathArr[0],productYear,NAME);
            //return relatePath;
           
            return   String.Format("实验验证数据库\\MODIS\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", Satellite, Sensor, string.Format("{0:0000}", BeginDate.Year), string.Format("{0:00}", BeginDate.Month), string.Format("{0:00}", BeginDate.Day),NAME);
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from prod_modis ");
            strSql.AppendFormat(" where QRST_CODE = '{0}'",qrst_code);

            using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    NAME = ds.Tables[0].Rows[0]["NAME"].ToString();
                    Satellite = ds.Tables[0].Rows[0]["SATELLITE"].ToString();
                    Sensor = ds.Tables[0].Rows[0]["SENSOR"].ToString();
                    if (ds.Tables[0].Rows[0]["BEGINDATE"].ToString() != "")
                    {
                        BeginDate = DateTime.Parse(ds.Tables[0].Rows[0]["BEGINDATE"].ToString());
                    }
                    IsCreated = true;
                }
                else
                {
                    IsCreated = false;
                }
            }
        }

        
        #endregion
    }
}
