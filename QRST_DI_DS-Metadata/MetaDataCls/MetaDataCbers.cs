using System;
using System.Text;
using System.Globalization;
using System.Xml;
using System.Data;
using System.IO;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataCbers : MetaData
    {
        public string[] cbersAttributeNames = {
                                    "satName",           //CBERS2B
                                    "recStationId",      //GUA
                                    "productId",         //145190
                                    "sceneID",           //452375
                                    "correctionLevel",   //2
                                    "sensorID",          //CCD
                                    "pixelSpacing",      //19.5
                                    "scenePath",         //8
                                    "sceneRow",          //65
                                    "sceneDate",         //2008-07-06 03:46:46.000
                                    "earthModel",        //WGS_84
                                    "mapProjection",     //UTM
                                    "utmZone",           //48
                                    "resampleTechnique", //CC
                                    "upperLeftLat","upperLeftLong","upperRightLat","upperRightLong",
                                    "lowerRightLat","lowerRightLong","lowerLeftLat","lowerLeftLong",
                                    //以后扩展的属性放在这里
                                    "rawFilePath","overviewFilePath"
                                  };
        public string[] cbersAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>
        
        #region Properties
        public  int ID;
        public string NAME
        {
            get;
            set;
        }
        private string satellite;
        //卫星名，例如CBERS2B
        public string Satellite
        {
            get { return cbersAttributeValues[0]; }
            set { cbersAttributeValues[0] = value; }
        }
        
        private string recStation;
        //接收站，例如GUA
        public string RecStation

        {
            get { return cbersAttributeValues[1]; }
            set { cbersAttributeValues[1] = value; }
        }
        
        private long productID;
        //标准产品号，例如145190
        public long ProductID
        {
            get { return long.Parse(cbersAttributeValues[2]); }
            set { cbersAttributeValues[2] = value.ToString(); }
        }

        private long senceID;
        //标准景号，例如452375
        public long SenceID
        {
            get { return long.Parse(cbersAttributeValues[3]); }
            set { cbersAttributeValues[3] = value.ToString(); }
        }

        private string productLevel;
        //产品级别，例如2
        public string ProductLevel
        {
            get { return cbersAttributeValues[4]; }
            set { cbersAttributeValues[4] = value; }
        }

        private string sensor;
        //传感器，例如CCD
        public string Sensor
        {
            get { return cbersAttributeValues[5]; }
            set { cbersAttributeValues[5] = value; }
        }

        private float pixelSpacing;
        //空间分辨率，例如19.5
        public float PixelSpacing
        {
            get { return float.Parse(cbersAttributeValues[6]); }
            set { cbersAttributeValues[6] = value.ToString(); }
        }

        private int sceneOrbitPath;
        //景path，例如8
        public int SceneOrbitPath
        {
            get { return int.Parse(cbersAttributeValues[7]); }
            set { cbersAttributeValues[7] = value.ToString(); }
        }

        private int sceneOrbitRow;
        //景row，例如65
        public int SceneOrbitRow
        {
            get { return int.Parse(cbersAttributeValues[8]); }
            set { cbersAttributeValues[8] = value.ToString(); }
        }

        private DateTime sceneDate;
        //数据接收时间，例如2008-07-06 03:46:46.000
        public DateTime SceneDate
        {
            get {
                return Convert.ToDateTime(cbersAttributeValues[9], CultureInfo.CurrentCulture);
            }
            set { cbersAttributeValues[9] = value.ToString(); }
        }

        private string earthModel;
        //椭球，例如WGS_84
        public string EarthModel
        {
            get { return cbersAttributeValues[10]; }
            set { cbersAttributeValues[10] = value; }
        }

        private string mapProjection;
        //投影，例如UTM
        public string MapProjection
        {
            get { return cbersAttributeValues[11]; }
            set { cbersAttributeValues[11] = value; }
        }

        private string zone;
        //带号，例如48
        public string Zone
        {
            get { return cbersAttributeValues[12]; }
            set { cbersAttributeValues[12] = value; }
        }

        private string resampleTechnique;
        //重采样方法，例如CC
        public string ResampleTechnique
        {
            get { return cbersAttributeValues[13]; }
            set { cbersAttributeValues[13] = value; }
        }

        private double dataUpperLeftLat;
        //左上角纬度，例如31.684104
        public double DataUpperLeftLat
        {
            get { return double.Parse(cbersAttributeValues[14]); }
            set { cbersAttributeValues[14] = value.ToString(); }
        }

        private double dataUpperLeftLong;
        //左上角经度，例如106.414302
        public double DataUpperLeftLong
        {
            get { return double.Parse(cbersAttributeValues[15]); }
            set { cbersAttributeValues[15] = value.ToString(); }
        }

        private double dataUpperRightLat;
        //右上角纬度，例如31.684104
        public double DataUpperRightLat
        {
            get { return double.Parse(cbersAttributeValues[16]); }
            set { cbersAttributeValues[16] = value.ToString(); }
        }

        private double dataUpperRightLong;
        //右上角经度，例如107.84578
        public double DataUpperRightLong
        {
            get { return double.Parse(cbersAttributeValues[17]); }
            set { cbersAttributeValues[17] = value.ToString(); }
        }

        private double dataLowerRightLat;
        //右下角纬度，例如30.446737
        public double DataLowerRightLat
        {
            get { return double.Parse(cbersAttributeValues[18]); }
            set { cbersAttributeValues[18] = value.ToString(); }
        }

        private double dataLowerRightLong;
        //右下角经度，例如107.84578
        public double DataLowerRightLong
        {
            get { return double.Parse(cbersAttributeValues[19]); }
            set { cbersAttributeValues[19] = value.ToString(); }
        }

        private double dataLowerLeftLat;
        //左下角纬度，例如30.446737
        public double DataLowerLeftLat
        {
            get { return double.Parse(cbersAttributeValues[20]); }
            set { cbersAttributeValues[20] = value.ToString(); }
        }

        private double dataLowerLeftLong;
        //左下角经度，例如106.414302
        public double DataLowerLeftLong
        {
            get { return double.Parse(cbersAttributeValues[21]); }
            set { cbersAttributeValues[21] = value.ToString(); }
        }

        private string rawFilePath;
        //原数据路径，例如"D:\...."
        public string RawFilePath
        {
            get { return cbersAttributeValues[22]; }
            set { cbersAttributeValues[22] = value; }
        }

        private string overviewFilePath;
        //缩略图路径，例如"D:\abc\cde.jpg"
        public string OverviewFilePath
        {
            get { return cbersAttributeValues[23]; }
            set { cbersAttributeValues[23] = value; }
        }

        #endregion

        #region Method
        public MetaDataCbers()
        {
            _dataType = EnumMetadataTypes.CBERS;
            cbersAttributeValues = new string[cbersAttributeNames.Length];
        }
        /// <summary>
        /// 读取CBERS和HJ数据XML中属性
        /// </summary>
        /// <param name="fileName">XML文件名</param>
        /// <param name="otherParameters">其它属性值,如xml中没有的“文件路径”等</param>
        /// <param name="output_attribute_string">输出属性值</param>
        public void readCbersHjAttribute(string fileName, string[] otherParameters, out string[] outputAttributeValues)
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

            string[] attributeValues = new string[cbersAttributeNames.Length];
            for (int i = 0; i < cbersAttributeNames.Length; i++)
                if (i < cbersAttributeNames.Length - otherParameters.Length)
                {
                    node = root.GetElementsByTagName(cbersAttributeNames[i]).Item(0);
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
                    attributeValues[i] = otherParameters[i - cbersAttributeNames.Length + otherParameters.Length];
            outputAttributeValues = attributeValues;
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NAME,STORAGESITE,SATELLITE,RECSTATION,PRODUCTID,SCENEID,PRODUCTLEVEL,SENSOR,PIXELSPACING,SCENEORBITPATH,SCENEORBITROW,SCENEDATE,EARTHMODEL,MAPPROJECTION,ZONE,RESAMPLETECHNIQUE,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,OVERVIEWFILEPATH,DESCRIPTION,QRST_CODE from prod_cbers ");
            strSql.AppendFormat(" where QRST_CODE= '{0}' ", qrst_code);

            using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                    {
                        ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    }
                    NAME = ds.Tables[0].Rows[0]["NAME"].ToString();
                    Satellite = ds.Tables[0].Rows[0]["SATELLITE"].ToString();
                    RecStation = ds.Tables[0].Rows[0]["RECSTATION"].ToString();
                    ProductID = long.Parse(ds.Tables[0].Rows[0]["PRODUCTID"].ToString());
                    ProductLevel = ds.Tables[0].Rows[0]["PRODUCTLEVEL"].ToString();
                    Sensor = ds.Tables[0].Rows[0]["SENSOR"].ToString();
                    if (ds.Tables[0].Rows[0]["PIXELSPACING"].ToString() != "")
                    {
                        PixelSpacing = float.Parse(ds.Tables[0].Rows[0]["PIXELSPACING"].ToString());
                    }
                    SceneOrbitPath = int.Parse(ds.Tables[0].Rows[0]["SCENEORBITPATH"].ToString());
                    if (ds.Tables[0].Rows[0]["SCENEORBITROW"].ToString() != "")
                    {
                        sceneOrbitRow = int.Parse(ds.Tables[0].Rows[0]["SCENEORBITROW"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["SCENEDATE"].ToString() != "")
                    {
                        SceneDate = DateTime.Parse(ds.Tables[0].Rows[0]["SCENEDATE"].ToString());
                    }
                    EarthModel = ds.Tables[0].Rows[0]["EARTHMODEL"].ToString();
                    MapProjection = ds.Tables[0].Rows[0]["MAPPROJECTION"].ToString();
                    Zone = ds.Tables[0].Rows[0]["ZONE"].ToString();
                    ResampleTechnique = ds.Tables[0].Rows[0]["RESAMPLETECHNIQUE"].ToString();
                    if (ds.Tables[0].Rows[0]["DATAUPPERLEFTLAT"].ToString() != "")
                    {
                        DataUpperLeftLat = double.Parse(ds.Tables[0].Rows[0]["DATAUPPERLEFTLAT"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["DATAUPPERLEFTLONG"].ToString() != "")
                    {
                        DataUpperLeftLong = double.Parse(ds.Tables[0].Rows[0]["DATAUPPERLEFTLONG"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["DATAUPPERRIGHTLAT"].ToString() != "")
                    {
                        DataUpperRightLat = double.Parse(ds.Tables[0].Rows[0]["DATAUPPERRIGHTLAT"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["DATAUPPERRIGHTLONG"].ToString() != "")
                    {
                        DataUpperRightLong = double.Parse(ds.Tables[0].Rows[0]["DATAUPPERRIGHTLONG"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["DATALOWERRIGHTLAT"].ToString() != "")
                    {
                        DataLowerRightLat = double.Parse(ds.Tables[0].Rows[0]["DATALOWERRIGHTLAT"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["DATALOWERRIGHTLONG"].ToString() != "")
                    {
                        DataLowerRightLong = double.Parse(ds.Tables[0].Rows[0]["DATALOWERRIGHTLONG"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["DATALOWERLEFTLAT"].ToString() != "")
                    {
                        DataLowerLeftLat = double.Parse(ds.Tables[0].Rows[0]["DATALOWERLEFTLAT"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["DATALOWERLEFTLONG"].ToString() != "")
                    {
                        DataLowerLeftLong = double.Parse(ds.Tables[0].Rows[0]["DATALOWERLEFTLONG"].ToString());
                    }
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    IsCreated = true;
                }
                else
                {
                    IsCreated = false;
                }
            }
        }

        /// <summary>
        /// 获取cbers数据在数据阵列中的相对路径
        /// </summary>\\172.16.0.1\综合数据库\实验验证库\CBERS\cbers2b\Asa\2003\12\03\数据名称\
        /// <returns></returns>
        public override string GetRelateDataPath()
        {
            return string.Format("实验验证数据库\\CBERS\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", Satellite, Sensor, string.Format("{0:0000}", SceneDate.Year), string.Format("{0:00}", SceneDate.Month), string.Format("{0:00}", SceneDate.Day),Path.GetFileNameWithoutExtension(NAME));
        }


        #endregion
    }
}
