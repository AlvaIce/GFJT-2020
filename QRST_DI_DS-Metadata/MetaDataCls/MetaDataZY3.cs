using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataZY3 : MetaData 
    {
        public string[] zy3AttributeNames = {
                                    "SatelliteID",          
                                    "SensorID",      
                                    "ReceiveTime",        
                                    "OrbitID",   
                                    "ProduceType",  
                                    "SceneID",       
                                    "ProductID",     
                                    "ProductLevel",        
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
                                     "TopLeftLatitude",
                                    "TopLeftLongitude",
                                     "TopRightLatitude","TopRightLongitude",
                                     "BottomRightLatitude","BottomRightLongitude","BottomLeftLatitude","BottomLeftLongitude","YawSatelliteAngle",
                                  };
        public string[] zy3AttributeValues;

          public MetaDataZY3(string _name,string _qrst_code)
        {
            zy3AttributeValues = new string[zy3AttributeValues.Length];
            NAME = _name;
            QRST_CODE = _qrst_code;
        }

          public MetaDataZY3()
        {
            zy3AttributeValues = new string[zy3AttributeNames.Length];
        }


        #region 元数据字段
        /// <summary>
        /// 
        /// </summary>
        public string NAME
        {
            set;
            get;
        }

        public int ID
        {
            set;
            get;
        }

        public string QRST_CODE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int CorDataFlag
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteID
        {
            set { zy3AttributeValues[0] = value; }
            get { return zy3AttributeValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SensorID
        {
            set { zy3AttributeValues[1] = value; }
            get { return zy3AttributeValues[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ReceiveTime
        {
            set { zy3AttributeValues[2] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy3AttributeValues[2], out dt))
                {
                    return dt;
                }
                else
                {
                    
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrbitID
        {
            set { zy3AttributeValues[3] = value; }
            get { return zy3AttributeValues[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProduceType
        {
            set { zy3AttributeValues[4] = value; }
            get { return zy3AttributeValues[4]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneID
        {
            set { zy3AttributeValues[5] = value; }
            get { return zy3AttributeValues[5]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductID
        {
            set { zy3AttributeValues[6] = value; }
            get { return zy3AttributeValues[6]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductLevel
        {
            set { zy3AttributeValues[7] = value; }
            get { return zy3AttributeValues[7]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductFormat
        {
            set { zy3AttributeValues[8] = value; }
            get { return zy3AttributeValues[8]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ProduceTime
        {
            set { zy3AttributeValues[9] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy3AttributeValues[9], out dt))
                {
                    return dt;
                }
                else
                {
                   
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bands
        {
            set { zy3AttributeValues[10] = value; }
            get { return zy3AttributeValues[10]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ScenePath
        {
            set { zy3AttributeValues[11] = value; }
            get { return zy3AttributeValues[11]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneRow
        {
            set { zy3AttributeValues[12] = value; }
            get { return zy3AttributeValues[12]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatPath
        {
            set { zy3AttributeValues[13] = value; }
            get { return zy3AttributeValues[13]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatRow
        {
            set { zy3AttributeValues[14] = value; }
            get { return zy3AttributeValues[14]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneCount
        {
            set { zy3AttributeValues[15] = value; }
            get { return zy3AttributeValues[15]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneShift
        {
            set { zy3AttributeValues[16] = value; }
            get { return zy3AttributeValues[16]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            set { zy3AttributeValues[17] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy3AttributeValues[17], out dt))
                {
                    return dt;
                }
                else
                {
                  
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime
        {
            set { zy3AttributeValues[18] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy3AttributeValues[18], out dt))
                {
                    return dt;
                }
                else
                {
                    
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CenterTime
        {
            set { zy3AttributeValues[19] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy3AttributeValues[19], out dt))
                {
                    return dt;
                }
                else
                {
                    
                }
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageGSD
        {
            set { zy3AttributeValues[20] = value; }
            get { return zy3AttributeValues[20]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WidthInPixels
        {
            set { zy3AttributeValues[21] = value; }
            get { return zy3AttributeValues[21]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightInPixels
        {
            set { zy3AttributeValues[22] = value; }
            get { return zy3AttributeValues[22]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WidthInMeters
        {
            set { zy3AttributeValues[23] = value; }
            get { return zy3AttributeValues[23]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightInMeters
        {
            set { zy3AttributeValues[24] = value; }
            get { return zy3AttributeValues[24]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double CloudPercent
        {
            set { zy3AttributeValues[25] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[25], out dt))
                {
                    return dt;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QualityInfo
        {
            set { zy3AttributeValues[26] = value; }
            get { return zy3AttributeValues[26]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PixelBits
        {
            set { zy3AttributeValues[27] = value; }
            get { return zy3AttributeValues[27]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ValidPixelBits
        {
            set { zy3AttributeValues[28] = value; }
            get { return zy3AttributeValues[28]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RollViewingAngle
        {
            set { zy3AttributeValues[29] = value; }
            get { return zy3AttributeValues[29]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PitchViewingAngle
        {
            set { zy3AttributeValues[30] = value; }
            get { return zy3AttributeValues[30]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RollSatelliteAngle
        {
            set { zy3AttributeValues[31] = value; }
            get { return zy3AttributeValues[31]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PitchSatelliteAngle
        {
            set { zy3AttributeValues[32] = value; }
            get { return zy3AttributeValues[32]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YawSatelliteAngle
        {
            set { zy3AttributeValues[55] = value; }
            get { return zy3AttributeValues[55]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SolarAzimuth
        {
            set { zy3AttributeValues[33] = value; }
            get { return zy3AttributeValues[33]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SolarZenith
        {
            set { zy3AttributeValues[34] = value; }
            get { return zy3AttributeValues[34]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteAzimuth
        {
            set { zy3AttributeValues[35] = value; }
            get { return zy3AttributeValues[35]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteZenith
        {
            set { zy3AttributeValues[36] = value; }
            get { return zy3AttributeValues[36]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GainMode
        {
            set { zy3AttributeValues[37] = value; }
            get { return zy3AttributeValues[37]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntegrationTime
        {
            set { zy3AttributeValues[38] = value; }
            get { return zy3AttributeValues[38]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntegrationLevel
        {
            set { zy3AttributeValues[39] = value; }
            get { return zy3AttributeValues[39]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MapProjection
        {
            set { zy3AttributeValues[40] = value; }
            get { return zy3AttributeValues[40]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EarthEllipsoid
        {
            set { zy3AttributeValues[41] = value; }
            get { return zy3AttributeValues[41]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ZoneNo
        {
            set { zy3AttributeValues[42] = value; }
            get { return zy3AttributeValues[42]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResamplingKernel
        {
            set { zy3AttributeValues[43] = value; }
            get { return zy3AttributeValues[43]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightMode
        {
            set { zy3AttributeValues[44] = value; }
            get { return zy3AttributeValues[44]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MtfCorrection
        {
            set { zy3AttributeValues[45] = value; }
            get { return zy3AttributeValues[45]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RelativeCorrectionData
        {
            set { zy3AttributeValues[46] = value; }
            get { return zy3AttributeValues[46]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double DATAUPPERLEFTLAT
        {
            set { zy3AttributeValues[47] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[47], out dt))
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
            set { zy3AttributeValues[48] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[48], out dt))
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
            set { zy3AttributeValues[49] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[49], out dt))
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
            set { zy3AttributeValues[50] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[50], out dt))
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
            set { zy3AttributeValues[51] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[51], out dt))
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
            set { zy3AttributeValues[52] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[52], out dt))
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
            set { zy3AttributeValues[53] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[53], out dt))
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
            set { zy3AttributeValues[54] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy3AttributeValues[54], out dt))
                {
                    return dt;
                }
                else
                {
                    return -180;
                }
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
                for (int i = 0; i < zy3AttributeNames.Length; i++)
                {
                    node = root.GetElementsByTagName(zy3AttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        zy3AttributeValues[i] = "";
                    }
                    else
                    {
                        zy3AttributeValues[i] = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }

        //ZY3_MUX_E115.2_N40.2_20120926_L1A0000695359.tar.gz
        public override string GetRelateDataPath()
        {
            string[] strArr = NAME.Split("_".ToCharArray());
            if (strArr.Length == 6)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);
                return string.Format("实验验证数据库\\ZY3卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(NAME)));
            }
            else
            {
                return base.GetRelateDataPath();
            }
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from prod_zy3 ");
            strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

            using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    NAME = ds.Tables[0].Rows[0]["NAME"].ToString();
                    SatelliteID = ds.Tables[0].Rows[0]["SatelliteID"].ToString();
                    SensorID = ds.Tables[0].Rows[0]["SensorID"].ToString();
                    IsCreated = true;
                }
                else
                {
                    IsCreated = false;
                }
            }
        }

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("prod_zy3",EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_zy3 where Name ='{0}'", NAME);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_zy3 where QRST_CODE ='{0}'", QRST_CODE);
                    //DataSet ds = sqlBase.GetDataSet(presql);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_zy3");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_zy3", ID);
                }


                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_zy3(");
                strSql.Append("ID,NAME,SatelliteID,SensorID,ReceiveTime,OrbitID,ProduceType,SceneID,ProductID,ProductLevel,ProductFormat,ProduceTime,Bands,ScenePath,SceneRow,SatPath,SatRow,SceneCount,SceneShift,StartTime,EndTime,CenterTime,ImageGSD,WidthInPixels,HeightInPixels,WidthInMeters,HeightInMeters,CloudPercent,QualityInfo,PixelBits,ValidPixelBits,RollViewingAngle,PitchViewingAngle,RollSatelliteAngle,PitchSatelliteAngle,YawSatelliteAngle,SolarAzimuth,SolarZenith,SatelliteAzimuth,SatelliteZenith,GainMode,IntegrationTime,IntegrationLevel,MapProjection,EarthEllipsoid,ZoneNo,ResamplingKernel,HeightMode,MtfCorrection,RelativeCorrectionData,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,QRST_CODE,CorDataFlag)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},'{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}'," +
                        "'{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}',{50},{51},{52},{53},{54},{55},{56},{57},'{58}',{59})", ID, NAME,
                        SatelliteID, SensorID, ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss"), OrbitID, ProduceType, SceneID, ProductID, ProductLevel,
                        ProductFormat, ProduceTime.ToString("yyyy-MM-dd HH:mm:ss"), Bands, ScenePath, SceneRow, SatPath, SatRow, SceneCount,
                        SceneShift, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), CenterTime.ToString("yyyy-MM-dd HH:mm:ss"), ImageGSD, WidthInPixels, HeightInPixels, WidthInMeters, HeightInMeters,
                        CloudPercent, QualityInfo, PixelBits, ValidPixelBits, RollViewingAngle, PitchViewingAngle,
                        RollSatelliteAngle, PitchSatelliteAngle, YawSatelliteAngle, SolarAzimuth, SolarZenith,
                        SatelliteAzimuth, SatelliteZenith, GainMode, IntegrationTime, IntegrationLevel, MapProjection,
                        EarthEllipsoid, ZoneNo, ResamplingKernel, HeightMode, MtfCorrection, RelativeCorrectionData,
                       DATAUPPERLEFTLAT, DATAUPPERLEFTLONG, DATAUPPERRIGHTLAT,
                        DATAUPPERRIGHTLONG, DATALOWERRIGHTLAT, DATALOWERRIGHTLONG, DATALOWERLEFTLAT, DATALOWERLEFTLONG,
                        QRST_CODE, CorDataFlag));
     //           strSql.Append("@ID,@NAME,@SatelliteID,@SensorID,@ReceiveTime,@OrbitID,@ProduceType,@SceneID,@ProductID,@ProductLevel,@ProductFormat,@ProduceTime,@Bands,@ScenePath,@SceneRow,@SatPath,@SatRow,@SceneCount,@SceneShift,@StartTime,@EndTime,@CenterTime,@ImageGSD,@WidthInPixels,@HeightInPixels,@WidthInMeters,@HeightInMeters,@CloudPercent,@QualityInfo,@PixelBits,@ValidPixelBits,@RollViewingAngle,@PitchViewingAngle,@RollSatelliteAngle,@PitchSatelliteAngle,@YawSatelliteAngle,@SolarAzimuth,@SolarZenith,@SatelliteAzimuth,@SatelliteZenith,@GainMode,@IntegrationTime,@IntegrationLevel,@MapProjection,@EarthEllipsoid,@ZoneNo,@ResamplingKernel,@HeightMode,@MtfCorrection,@RelativeCorrectionData,@DATAUPPERLEFTLAT,@DATAUPPERLEFTLONG,@DATAUPPERRIGHTLAT,@DATAUPPERRIGHTLONG,@DATALOWERRIGHTLAT,@DATALOWERRIGHTLONG,@DATALOWERLEFTLAT,@DATALOWERLEFTLONG,@QRST_CODE,@CorDataFlag)");
     //           MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Decimal,20),
					//new MySqlParameter("@NAME", MySqlDbType.Text),
					//new MySqlParameter("@SatelliteID", MySqlDbType.VarChar,20),
					//new MySqlParameter("@SensorID", MySqlDbType.VarChar,20),
					//new MySqlParameter("@ReceiveTime", MySqlDbType.DateTime),
					//new MySqlParameter("@OrbitID", MySqlDbType.VarChar,20),
					//new MySqlParameter("@ProduceType", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SceneID", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ProductID", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ProductLevel", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ProductFormat", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ProduceTime", MySqlDbType.DateTime),
					//new MySqlParameter("@Bands", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ScenePath", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SceneRow", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SatPath", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SatRow", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SceneCount", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SceneShift", MySqlDbType.VarChar,45),
					//new MySqlParameter("@StartTime", MySqlDbType.DateTime),
					//new MySqlParameter("@EndTime", MySqlDbType.DateTime),
					//new MySqlParameter("@CenterTime", MySqlDbType.DateTime),
					//new MySqlParameter("@ImageGSD", MySqlDbType.VarChar,45),
					//new MySqlParameter("@WidthInPixels", MySqlDbType.VarChar,45),
					//new MySqlParameter("@HeightInPixels", MySqlDbType.VarChar,45),
					//new MySqlParameter("@WidthInMeters", MySqlDbType.VarChar,45),
					//new MySqlParameter("@HeightInMeters", MySqlDbType.VarChar,45),
					//new MySqlParameter("@CloudPercent", MySqlDbType.Decimal,10),
					//new MySqlParameter("@QualityInfo", MySqlDbType.VarChar,45),
					//new MySqlParameter("@PixelBits", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ValidPixelBits", MySqlDbType.VarChar,45),
					//new MySqlParameter("@RollViewingAngle", MySqlDbType.VarChar,45),
					//new MySqlParameter("@PitchViewingAngle", MySqlDbType.VarChar,45),
					//new MySqlParameter("@RollSatelliteAngle", MySqlDbType.VarChar,45),
					//new MySqlParameter("@PitchSatelliteAngle", MySqlDbType.VarChar,45),
					//new MySqlParameter("@YawSatelliteAngle", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SolarAzimuth", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SolarZenith", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SatelliteAzimuth", MySqlDbType.VarChar,45),
					//new MySqlParameter("@SatelliteZenith", MySqlDbType.VarChar,45),
					//new MySqlParameter("@GainMode", MySqlDbType.VarChar,45),
					//new MySqlParameter("@IntegrationTime", MySqlDbType.VarChar,45),
					//new MySqlParameter("@IntegrationLevel", MySqlDbType.VarChar,45),
					//new MySqlParameter("@MapProjection", MySqlDbType.VarChar,45),
					//new MySqlParameter("@EarthEllipsoid", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ZoneNo", MySqlDbType.VarChar,45),
					//new MySqlParameter("@ResamplingKernel", MySqlDbType.VarChar,45),
					//new MySqlParameter("@HeightMode", MySqlDbType.VarChar,45),
					//new MySqlParameter("@MtfCorrection", MySqlDbType.VarChar,45),
					//new MySqlParameter("@RelativeCorrectionData", MySqlDbType.VarChar,45),
					//new MySqlParameter("@DATAUPPERLEFTLAT", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATAUPPERLEFTLONG", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATAUPPERRIGHTLAT", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATAUPPERRIGHTLONG", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATALOWERRIGHTLAT", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATALOWERRIGHTLONG", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATALOWERLEFTLAT", MySqlDbType.Decimal,10),
					//new MySqlParameter("@DATALOWERLEFTLONG", MySqlDbType.Decimal,10),
					//new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45),
					//new MySqlParameter("@CorDataFlag", MySqlDbType.Int32,4)};
     //           parameters[0].Value =ID;
     //           parameters[1].Value =NAME;
     //           parameters[2].Value =SatelliteID;
     //           parameters[3].Value =SensorID;
     //           parameters[4].Value =ReceiveTime;
     //           parameters[5].Value =OrbitID;
     //           parameters[6].Value =ProduceType;
     //           parameters[7].Value =SceneID;
     //           parameters[8].Value =ProductID;
     //           parameters[9].Value =ProductLevel;
     //           parameters[10].Value =ProductFormat;
     //           parameters[11].Value =ProduceTime;
     //           parameters[12].Value =Bands;
     //           parameters[13].Value =ScenePath;
     //           parameters[14].Value =SceneRow;
     //           parameters[15].Value =SatPath;
     //           parameters[16].Value =SatRow;
     //           parameters[17].Value =SceneCount;
     //           parameters[18].Value =SceneShift;
     //           parameters[19].Value =StartTime;
     //           parameters[20].Value =EndTime;
     //           parameters[21].Value =CenterTime;
     //           parameters[22].Value =ImageGSD;
     //           parameters[23].Value =WidthInPixels;
     //           parameters[24].Value =HeightInPixels;
     //           parameters[25].Value =WidthInMeters;
     //           parameters[26].Value =HeightInMeters;
     //           parameters[27].Value =CloudPercent;
     //           parameters[28].Value =QualityInfo;
     //           parameters[29].Value =PixelBits;
     //           parameters[30].Value =ValidPixelBits;
     //           parameters[31].Value =RollViewingAngle;
     //           parameters[32].Value =PitchViewingAngle;
     //           parameters[33].Value =RollSatelliteAngle;
     //           parameters[34].Value =PitchSatelliteAngle;
     //           parameters[35].Value =YawSatelliteAngle;
     //           parameters[36].Value =SolarAzimuth;
     //           parameters[37].Value =SolarZenith;
     //           parameters[38].Value =SatelliteAzimuth;
     //           parameters[39].Value =SatelliteZenith;
     //           parameters[40].Value =GainMode;
     //           parameters[41].Value =IntegrationTime;
     //           parameters[42].Value =IntegrationLevel;
     //           parameters[43].Value =MapProjection;
     //           parameters[44].Value =EarthEllipsoid;
     //           parameters[45].Value =ZoneNo;
     //           parameters[46].Value =ResamplingKernel;
     //           parameters[47].Value =HeightMode;
     //           parameters[48].Value =MtfCorrection;
     //           parameters[49].Value =RelativeCorrectionData;
     //           parameters[50].Value =DATAUPPERLEFTLAT;
     //           parameters[51].Value =DATAUPPERLEFTLONG;
     //           parameters[52].Value =DATAUPPERRIGHTLAT;
     //           parameters[53].Value =DATAUPPERRIGHTLONG;
     //           parameters[54].Value =DATALOWERRIGHTLAT;
     //           parameters[55].Value =DATALOWERRIGHTLONG;
     //           parameters[56].Value =DATALOWERLEFTLAT;
     //           parameters[57].Value =DATALOWERLEFTLONG;
     //           parameters[58].Value =QRST_CODE;
     //           parameters[59].Value =CorDataFlag;

                sqlBase.ExecuteSql(strSql.ToString());

                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_zy3 set CorDataFlag = {0} where Name = '{1}'", corDataPath, NAME);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_zy3",EnumDBType.MIDB);
            }
            catch (Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }
        }

        public string GetCorrectedDataPath()
        {
            string[] strArr = NAME.Split("_".ToCharArray());
            if (strArr.Length == 6)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);
                return string.Format("{6}数据产品库\\数据预处理产品\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(NAME)), StoragePath.StoreBasePath);
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}
