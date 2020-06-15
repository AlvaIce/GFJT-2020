using System;
using System.Globalization;
using System.Text;
using System.Data;
using DotSpatial.Data.Rasters.GdalExtension;

using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataNOAA : MetaData
    {
        public string[] noaaAttributeNames = {
                                    "satellite",           //NOAA-15(K)
                                    "sensor",          //AVHRR
                                    "dataType",      //AVHRR GAC
                                    "revolution",         //29340
                                    "source",           //Wallops Island,Virginia,USA
                                    "processingCenter",   //NOAA/NESDIS-SuitLand,Maryland,USA
                                    "startDate",          //2008-07-06 03:46:46.000
                                    "stopDate",      //2008-07-06 03:54:26.000
                                    "location",         //Descending
                                    //以后扩展的属性放在这里
                                    "rawFilePath","overviewFilePath"
                                  };
        public string[] noaaAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties
        public string NAME
        {
            get;
            set;
        }

        private string satellite;
        //卫星名，例如CBERS2B
        public string Satellite
        {
            get { return noaaAttributeValues[0]; }
            set { noaaAttributeValues[0] = value; }
        }

        private string sensor;
        //传感器，AVHRR
        public string Sensor
        {
            get { return noaaAttributeValues[1]; }
            set { noaaAttributeValues[1] = value; }
        }

        private string dataType;
        //数据类型，例如GAC
        public string DataType
        {
            get { return noaaAttributeValues[2]; }
            set { noaaAttributeValues[2] = value; }
        }

        private long revolution;
        //分辨率
        public long Revolution
        {
            get { return long.Parse(noaaAttributeValues[3]); }
            set { noaaAttributeValues[3] = value.ToString(); }
        }

        private string source;
        //元数据地址
        public string Source
        {
            get { return noaaAttributeValues[4]; }
            set { noaaAttributeValues[4] = value; }
        }

        private string processingCenter;
        //处理中心
        public string ProcessingCenter
        {
            get { return noaaAttributeValues[5]; }
            set { noaaAttributeValues[5] = value; }
        }

        private DateTime startDate;
        //观测开始时间，例如2008-07-06 03:46:46.000
        public DateTime StartDate
        {
            get {
                DateTime dt= Convert.ToDateTime(noaaAttributeValues[6], CultureInfo.CurrentCulture);
                return dt;
            }
            set { noaaAttributeValues[6] = value.ToString(); }
        }

        private DateTime stopDate;
        //观测结束时间，例如2008-07-06 03:46:46.000
        public DateTime StopDate
        {
            get {
                DateTime dt= Convert.ToDateTime(noaaAttributeValues[7], CultureInfo.CurrentCulture);
                return dt;
            }
            set { noaaAttributeValues[7] = value.ToString(); }
        }

        private string location;
        //投影，例如UTM
        public string Location
        {
            get { return noaaAttributeValues[8]; }
            set { noaaAttributeValues[8] = value; }
        }

        private string rawFilePath;
        //原数据路径，例如"D:\...."
        public string RawFilePath
        {
            get { return noaaAttributeValues[9]; }
            set { noaaAttributeValues[9] = value; }
        }

        private string overviewFilePath;
        //缩略图路径，例如"D:\abc\cde.jpg"
        public string OverviewFilePath
        {
            get { return noaaAttributeValues[10]; }
            set { noaaAttributeValues[10] = value; }
        }

        #endregion

        #region Method
        public MetaDataNOAA()
        {
            _dataType = EnumMetadataTypes.NOAA;
            noaaAttributeValues = new string[noaaAttributeNames.Length];
        }
        /// <summary>
        /// 读取NOAA数据中元数据
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="otherParameters">其它属性值</param>
        /// <param name="output_attribute_string">输出属性值</param>
        public void readNoaaAttribute(string fileName, string[] otherParameters, out string[] outputAttributeValues)
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
                throw new Exception("打开NOAA文件错误,请检查文件格式是否符合要求或者文件是否损坏！");
            }

            string str1 = null;
            string lon = null;
            int a, b;
            //satellite
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("SATELLITE"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    noaaAttributeValues[0] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //sensor
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("DATA_TYPE"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    noaaAttributeValues[1] = metadata[i].Substring(b).Split(' ')[0];
                    noaaAttributeValues[2] = metadata[i].Substring(b).Split(' ')[1];
                    break;
                }
            }
            //revolution
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("REVOLUTION"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    noaaAttributeValues[3] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //Source
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("SOURCE"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    noaaAttributeValues[4] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //Process_Center
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("PROCESSING_CENTER"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    noaaAttributeValues[5] = metadata[i].Substring(b).ToString();
                    break;
                }
            }
            //beginDate
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("START"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str1 = metadata[i].Substring(b); //year:2004,day:4,millisecond:76762975
                    noaaAttributeValues[6] = TimeConvert.datetimeConvert(str1.Split(',')[0].Split(':')[1], str1.Split(',')[1].Split(':')[1], str1.Split(',')[2].Split(':')[1]);
                }

            }
            //endDate
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("STOP"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    str1 = metadata[i].Substring(b); //year:2004,day:4,millisecond:76762975
                    noaaAttributeValues[7] = TimeConvert.datetimeConvert(str1.Split(',')[0].Split(':')[1], str1.Split(',')[1].Split(':')[1], str1.Split(',')[2].Split(':')[1]);
                }

            }
            //LOCATION
            for (int i = 0; i < metadata.Length; i++)
            {
                string sm = metadata[i].ToString();
                if (sm.Contains("LOCATION"))
                {
                    a = metadata[i].IndexOf("=");
                    b = a + 1;
                    lon = metadata[i].Substring(0, a);
                    noaaAttributeValues[8] = metadata[i].Substring(b).ToString();
                    break;
                }
            }


            //rawFilePath
            noaaAttributeValues[9] = otherParameters[0];
            //overviewFilePath
            noaaAttributeValues[10] = otherParameters[1];

            outputAttributeValues = noaaAttributeValues;
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,NAME,STARTDATE,STOPDATE,STORAGESITE,SATELLITE,SENSOR,DATA_TYPE,REVOLUTION,SOURCE,PROCESSING_CENTER,LOCATION,OVERVIEWFILEPATH,DESCRIPTION,QRST_CODE,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG from prod_noaa ");
            strSql.AppendFormat(" where QRST_CODE = '{0}'",qrst_code);

            DataSet ds = sqlBase.GetDataSet(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                NAME = ds.Tables[0].Rows[0]["NAME"].ToString();
                if (ds.Tables[0].Rows[0]["STARTDATE"].ToString() != "")
                {
                    StartDate = DateTime.Parse(ds.Tables[0].Rows[0]["STARTDATE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["STOPDATE"].ToString() != "")
                {
                    StopDate = DateTime.Parse(ds.Tables[0].Rows[0]["STOPDATE"].ToString());
                }
                Satellite = ds.Tables[0].Rows[0]["SATELLITE"].ToString();
                Sensor = ds.Tables[0].Rows[0]["SENSOR"].ToString();
                DataType = ds.Tables[0].Rows[0]["DATA_TYPE"].ToString();
                Revolution = long.Parse(ds.Tables[0].Rows[0]["REVOLUTION"].ToString());
                Source = ds.Tables[0].Rows[0]["SOURCE"].ToString();
                ProcessingCenter = ds.Tables[0].Rows[0]["PROCESSING_CENTER"].ToString();
                Location = ds.Tables[0].Rows[0]["LOCATION"].ToString();
                QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                //if (ds.Tables[0].Rows[0]["DATAUPPERLEFTLAT"].ToString() != "")
                //{
                //    datau = decimal.Parse(ds.Tables[0].Rows[0]["DATAUPPERLEFTLAT"].ToString());
                //}
                //if (ds.Tables[0].Rows[0]["DATAUPPERLEFTLONG"].ToString() != "")
                //{
                //    DATAUPPERLEFTLONG = decimal.Parse(ds.Tables[0].Rows[0]["DATAUPPERLEFTLONG"].ToString());
                //}
                //if (ds.Tables[0].Rows[0]["DATAUPPERRIGHTLAT"].ToString() != "")
                //{
                //    DATAUPPERRIGHTLAT = decimal.Parse(ds.Tables[0].Rows[0]["DATAUPPERRIGHTLAT"].ToString());
                //}
                //if (ds.Tables[0].Rows[0]["DATAUPPERRIGHTLONG"].ToString() != "")
                //{
                //    DATAUPPERRIGHTLONG = decimal.Parse(ds.Tables[0].Rows[0]["DATAUPPERRIGHTLONG"].ToString());
                //}
                //if (ds.Tables[0].Rows[0]["DATALOWERRIGHTLAT"].ToString() != "")
                //{
                //    DATALOWERRIGHTLAT = decimal.Parse(ds.Tables[0].Rows[0]["DATALOWERRIGHTLAT"].ToString());
                //}
                //if (ds.Tables[0].Rows[0]["DATALOWERRIGHTLONG"].ToString() != "")
                //{
                //    DATALOWERRIGHTLONG = decimal.Parse(ds.Tables[0].Rows[0]["DATALOWERRIGHTLONG"].ToString());
                //}
                //if (ds.Tables[0].Rows[0]["DATALOWERLEFTLAT"].ToString() != "")
                //{
                //    DATALOWERLEFTLAT = decimal.Parse(ds.Tables[0].Rows[0]["DATALOWERLEFTLAT"].ToString());
                //}
                //if (ds.Tables[0].Rows[0]["DATALOWERLEFTLONG"].ToString() != "")
                //{
                //    DATALOWERLEFTLONG = decimal.Parse(ds.Tables[0].Rows[0]["DATALOWERLEFTLONG"].ToString());
                //}
                //return model;
                IsCreated = true;
            }
            else
            {
                IsCreated = false;
            }
        }

        public override string GetRelateDataPath()
        {
            string dir = String.Format("实验验证数据库\\NOAA\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", Satellite, Sensor, string.Format("{0:0000}", StartDate.Year), string.Format("{0:00}", StartDate.Month), string.Format("{0:00}",StartDate.Day), NAME);
            return dir;
        }
        #endregion
    }
}
