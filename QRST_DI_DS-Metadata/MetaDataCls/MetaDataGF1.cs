/*
 * 作者：zxw
 * 创建时间：2013-08-17
 * 描述：用于描述高分一号卫星元数据信息
*/
using System;
using System.Text;
using System.Xml;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.IO;
using System.Data;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using System.Globalization;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataGF1:MetaData 
    {
        public string[] gf1AttributeNames = {
                                    "SatelliteID",          
                                    "SensorID",      
                                    "ReceiveTime",        
                                    "OrbitID",   
                                    "TopLeftLatitude",
                                    "TopLeftLongitude",
                                     "TopRightLatitude","TopRightLongitude",
                                     "BottomRightLatitude","BottomRightLongitude","BottomLeftLatitude","BottomLeftLongitude",
                                    "ProduceType",  
                                    "SceneID",       
                                    "ProductID",     
                                    "ProductLevel",        
                                    "ProductQuality",          
                                    "ProductQualityReport",         
                                    "ProductFormat",       
                                    "ProduceTime",     
                                    "Bands",          
                                    "ScenePath", 
                                    "SceneRow",
                                    "SatPath",
                                    "SatRow",
                                    "SceneCount",
                                    "SceneShift",
                                    "StartTime",
                                    "EndTime",
                                     "CenterTime",
                                     "ImageGSD",
                                     "WidthInPixels",
                                     "HeightInPixels",
                                     "WidthInMeters",
                                     "HeightInMeters",
                                     "CloudPercent",
                                     "QualityInfo",
                                     "PixelBits",
                                     "ValidPixelBits",
                                     "RollViewingAngle",
                                     "PitchViewingAngle",
                                     "RollSatelliteAngle",
                                     "PitchSatelliteAngle",
                                     "YawSatelliteAngle",
                                     "SolarAzimuth",
                                     "SolarZenith",
                                     "SatelliteAzimuth",
                                     "SatelliteZenith",
                                     "GainMode",
                                     "IntegrationTime",
                                     "IntegrationLevel",
                                     "MapProjection",
                                     "EarthEllipsoid",
                                     "ZoneNo",
                                     "ResamplingKernel",
                                     "HeightMode",
                                     "MtfCorrection",
                                     "RelativeCorrectionData",
                                  };
        public string[] gf1AttributeValues;

        public MetaDataGF1(string _name,string _qrst_code)
        {
            gf1AttributeValues = new string[gf1AttributeNames.Length];
            Name = _name;
            QRST_CODE = _qrst_code;
        }

        public MetaDataGF1()
        {
            gf1AttributeValues = new string[gf1AttributeNames.Length];
        }

        #region Model
       
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteID
        {
            set { gf1AttributeValues[0] = value; }
            get { return gf1AttributeValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SensorID
        {
            set { gf1AttributeValues[1] = value; }
            get { return gf1AttributeValues[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String ReceiveTime
        {
            set { gf1AttributeValues[2] = value.ToString(); }
            get {
                return gf1AttributeValues[2];
                //DateTime dt;
                //if (DateTime.TryParse(gf1AttributeValues[2],out dt))
                //{
                //    return dt;
                //}
                //else
                //{
                //    return null;
                //}
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrbitID
        {
            set { gf1AttributeValues[3] = value; }
            get { return gf1AttributeValues[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATAUPPERLEFTLAT
        {
            set { gf1AttributeValues[4] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[4], out dt))
                {
                    return dt;
                }
                else
                {
                    return 90;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATAUPPERLEFTLONG
        {
            set { gf1AttributeValues[5] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[5], out dt))
                {
                    return dt;
                }
                else
                {
                    return -180;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATAUPPERRIGHTLAT
        {
            set { gf1AttributeValues[6] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[6], out dt))
                {
                    return dt;
                }
                else
                {
                    return 90;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATAUPPERRIGHTLONG
        {
            set { gf1AttributeValues[7] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[7], out dt))
                {
                    return dt;
                }
                else
                {
                    return 180;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATALOWERRIGHTLAT
        {
            set { gf1AttributeValues[8] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[8], out dt))
                {
                    return dt;
                }
                else
                {
                    return -90;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATALOWERRIGHTLONG
        {
            set { gf1AttributeValues[9] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[9], out dt))
                {
                    return dt;
                }
                else
                {
                    return 180;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATALOWERLEFTLAT
        {
            set { gf1AttributeValues[10] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[10], out dt))
                {
                    return dt;
                }
                else
                {
                    return -90;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATALOWERLEFTLONG
        {
            set { gf1AttributeValues[11] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf1AttributeValues[11], out dt))
                {
                    return dt;
                }
                else
                {
                    return -180;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProduceType
        {
            set { gf1AttributeValues[12] = value; }
            get { return gf1AttributeValues[12]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneID
        {
            set { gf1AttributeValues[13] = value; }
            get { return gf1AttributeValues[13]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductID
        {
            set { gf1AttributeValues[14] = value; }
            get { return gf1AttributeValues[14]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductLevel
        {
            set { gf1AttributeValues[15] = value; }
            get { return gf1AttributeValues[15]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductQuality
        {
            set { gf1AttributeValues[16] = value; }
            get { return gf1AttributeValues[16]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductQualityReport
        {
            set { gf1AttributeValues[17] = value; }
            get { return gf1AttributeValues[17]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductFormat
        {
            set { gf1AttributeValues[18] = value; }
            get { return gf1AttributeValues[18]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ProduceTime
        {
            set { gf1AttributeValues[19] = value.ToString(); }
            get {
                DateTime dt;
                if (DateTime.TryParse(gf1AttributeValues[19], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bands
        {
            set { gf1AttributeValues[20] = value; }
            get { return gf1AttributeValues[20]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ScenePath
        {
            set { gf1AttributeValues[21] = value; }
            get { return gf1AttributeValues[21]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneRow
        {
            set { gf1AttributeValues[22] = value; }
            get { return gf1AttributeValues[22]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatPath
        {
            set { gf1AttributeValues[23] = value; }
            get { return gf1AttributeValues[23]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatRow
        {
            set { gf1AttributeValues[24] = value; }
            get { return gf1AttributeValues[24]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneCount
        {
            set { gf1AttributeValues[25] = value; }
            get { return gf1AttributeValues[25]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneShift
        {
            set { gf1AttributeValues[26] = value; }
            get { return gf1AttributeValues[26]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            set { gf1AttributeValues[27] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf1AttributeValues[27], out dt))
                { }

                    return dt;
                

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime
        {
            set { gf1AttributeValues[28] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf1AttributeValues[28], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CenterTime
        {
            set { gf1AttributeValues[29] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf1AttributeValues[29], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageGSD
        {
            set { gf1AttributeValues[30] = value; }
            get { return gf1AttributeValues[30]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WidthInPixels
        {
            set { gf1AttributeValues[31] = value; }
            get { return gf1AttributeValues[31]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightInPixels
        {
            set { gf1AttributeValues[32] = value; }
            get { return gf1AttributeValues[32]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WidthInMeters
        {
            set { gf1AttributeValues[33] = value; }
            get { return gf1AttributeValues[33]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightInMeters
        {
            set { gf1AttributeValues[34] = value; }
            get { return gf1AttributeValues[34]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CloudPercent
        {
            set { gf1AttributeValues[35] = value; }
            get { return gf1AttributeValues[35]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QualityInfo
        {
            set { gf1AttributeValues[36] = value; }
            get { return gf1AttributeValues[36]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PixelBits
        {
            set { gf1AttributeValues[37] = value; }
            get { return gf1AttributeValues[37]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ValidPixelBits
        {
            set { gf1AttributeValues[38] = value; }
            get { return gf1AttributeValues[38]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RollViewingAngle
        {
            set { gf1AttributeValues[39] = value; }
            get { return gf1AttributeValues[39]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PitchViewingAngle
        {
            set { gf1AttributeValues[40] = value; }
            get { return gf1AttributeValues[40]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RollSatelliteAngle
        {
            set { gf1AttributeValues[41] = value; }
            get { return gf1AttributeValues[41]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PitchSatelliteAngle
        {
            set { gf1AttributeValues[42] = value; }
            get { return gf1AttributeValues[42]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YawSatelliteAngle
        {
            set { gf1AttributeValues[43] = value; }
            get { return gf1AttributeValues[43]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SolarAzimuth
        {
            set { gf1AttributeValues[44] = value; }
            get { return gf1AttributeValues[44]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SolarZenith
        {
            set { gf1AttributeValues[45] = value; }
            get { return gf1AttributeValues[45]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteAzimuth
        {
            set { gf1AttributeValues[46] = value; }
            get { return gf1AttributeValues[46]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteZenith
        {
            set { gf1AttributeValues[47] = value; }
            get { return gf1AttributeValues[47]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GainMode
        {
            set { gf1AttributeValues[48] = value; }
            get { return gf1AttributeValues[48]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntegrationTime
        {
            set { gf1AttributeValues[49] = value; }
            get { return gf1AttributeValues[49]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntegrationLevel
        {
            set { gf1AttributeValues[50] = value; }
            get { return gf1AttributeValues[50]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MapProjection
        {
            set { gf1AttributeValues[51] = value; }
            get { return gf1AttributeValues[51]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EarthEllipsoid
        {
            set { gf1AttributeValues[52] = value; }
            get { return gf1AttributeValues[52]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ZoneNo
        {
            set { gf1AttributeValues[53] = value; }
            get { return gf1AttributeValues[53]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResamplingKernel
        {
            set { gf1AttributeValues[54] = value; }
            get { return gf1AttributeValues[54]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightMode
        {
            set { gf1AttributeValues[55] = value; }
            get { return gf1AttributeValues[55]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MtfCorrection
        {
            set { gf1AttributeValues[56] = value; }
            get { return gf1AttributeValues[56]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RelativeCorrectionData
        {
            set { gf1AttributeValues[57] = value; }
            get { return gf1AttributeValues[57]; }
        }
        public string CorDataFlag = "";
        
        /// <summary>
        /// 
        /// </summary>
        public string QRST_CODE
        {
            set;
            get;
        }
        #endregion Model

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {

                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("prod_gf1",EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_gf1 where Name ='{0}'", Name);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_gf1 where QRST_CODE ='{0}'", QRST_CODE);
                    //DataSet ds = sqlBase.GetDataSet(presql);
                   int i= sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_gf1");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_gf1", ID);
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_gf1(");
                strSql.Append("ID,Name,SatelliteID,SensorID,ReceiveTime,OrbitID,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG," +
                              "DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT," +
                              "DATALOWERLEFTLONG,ProduceType,SceneID,ProductID,ProductLevel,ProductQuality,ProductQualityReport,ProductFormat," +
                              "ProduceTime,Bands,ScenePath,SceneRow,SatPath,SatRow,SceneCount,SceneShift,StartTime,EndTime," +
                              "CenterTime,ImageGSD,WidthInPixels,HeightInPixels,WidthInMeters,HeightInMeters,CloudPercent," +
                              "QualityInfo,PixelBits,ValidPixelBits,RollViewingAngle,PitchViewingAngle,RollSatelliteAngle," +
                              "PitchSatelliteAngle,YawSatelliteAngle,SolarAzimuth,SolarZenith,SatelliteAzimuth,SatelliteZenith," +
                              "GainMode,IntegrationTime,IntegrationLevel,MapProjection,EarthEllipsoid,ZoneNo,ResamplingKernel," +
                              "HeightMode,MtfCorrection,RelativeCorrectionData,QRST_CODE)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},'{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}'," +"'{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}')", ID, Name, SatelliteID, SensorID, 
                        ReceiveTime, OrbitID, DATAUPPERLEFTLAT, DATAUPPERLEFTLONG, DATAUPPERRIGHTLAT, DATAUPPERRIGHTLONG, DATALOWERRIGHTLAT, DATALOWERRIGHTLONG,
                        DATALOWERLEFTLAT, DATALOWERLEFTLONG, ProduceType, SceneID, ProductID, ProductLevel, ProductQuality, ProductQualityReport, ProductFormat,
                        ProduceTime.ToString("yyyy-MM-dd HH:mm:ss"), Bands, ScenePath, SceneRow, SatPath, SatRow, SceneCount, SceneShift, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), CenterTime.ToString("yyyy-MM-dd HH:mm:ss"), ImageGSD, WidthInPixels,
                        HeightInPixels, WidthInMeters, HeightInMeters, CloudPercent, QualityInfo, PixelBits, ValidPixelBits, RollViewingAngle, PitchViewingAngle,
                        RollSatelliteAngle, PitchSatelliteAngle, YawSatelliteAngle, SolarAzimuth, SolarZenith, SatelliteAzimuth, SatelliteZenith, GainMode, IntegrationTime,
                        IntegrationLevel, MapProjection, EarthEllipsoid, ZoneNo, ResamplingKernel, HeightMode, MtfCorrection, RelativeCorrectionData, QRST_CODE));
              

                int result = sqlBase.ExecuteSql(strSql.ToString());


                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_gf1 set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_gf1",EnumDBType.MIDB);

            }
            catch (Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }
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
                for (int i = 0; i < gf1AttributeNames.Length; i++)
                {
                    node = root.GetElementsByTagName(gf1AttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        gf1AttributeValues[i] = "";
                    }
                    else
                    {
                        gf1AttributeValues[i] = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }

        //GF1_WFV1_E114.1_N22.9_20130712_L1A0000052397.tar.gz
        public override string GetRelateDataPath()
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 6)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0,4);
                string month = strArr[4].Substring(4,2);
                string day = strArr[4].Substring(6,2);
                return string.Format("实验验证数据库\\GF1卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)));
            }
            else
            {
                return base.GetRelateDataPath();
            }
           
        }

        public static string GetRelateDataPath(string Name)
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 6)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);
                return string.Format("实验验证数据库\\GF1卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)), Name);
            }
            return "";
        }

        public static bool HasCorrectedData(string qrst_code,IDbBaseUtilities evdb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CorDataFlag from prod_gf1 ");
            strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);
            DataSet ds = evdb.GetDataSet(strSql.ToString());
            try
            {
                if (ds.Tables[0].Rows[0][0].ToString() != "-1")
                {
                    return true;
                }
            }
            catch 
            {
            }
            return false;
        }

        public string GetCorrectedDataPath()
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 6)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);
                return string.Format("{6}数据产品库\\数据预处理产品\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)), StoragePath.StoreBasePath);
            }
            else
            {
                return "";
            }
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from prod_gf1 ");
            strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

            using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                    SatelliteID = ds.Tables[0].Rows[0]["SatelliteID"].ToString();
                    SensorID = ds.Tables[0].Rows[0]["SensorID"].ToString();
                    CorDataFlag = ds.Tables[0].Rows[0]["CorDataFlag"].ToString();
                    IsCreated = true;
                }
                else
                {
                    IsCreated = false;
                }
            }
        }

    }
}
