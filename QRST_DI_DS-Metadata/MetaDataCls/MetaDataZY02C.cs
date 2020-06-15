using System;
using System.Text;
using System.Data;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.IO;
using System.Xml;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataZY02C : MetaData 
    {
          public string[] zy02cAttributeNames = {
                                    "SatelliteID",                                              
                                    "ReceiveStationID",
                                    "SensorID",
                                    "ReceiveTime",        
                                    "OrbitID",
                                    "AttType",
                                    "StripID" ,
                                    "ProduceType",
                                    "SceneID",  
                                    "DDSFlag",       
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
                                     "CloudPercent",
                                     "DataSize",
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
                                     "EphemerisData",
                                     "AttitudeData",
                                     "RadiometricMethod",
                                     "MtfCorrection",
                                     "Denoise",
                                     "RayleighCorrection",
                                     "UsedGCPNo",
                                     "CenterLatitude",
                                     "CenterLongitude",
                                     "TopLeftLatitude",
                                    "TopLeftLongitude",
                                     "TopRightLatitude","TopRightLongitude",
                                     "BottomRightLatitude","BottomRightLongitude","BottomLeftLatitude","BottomLeftLongitude",
                                  };
        
        public string[] zy02cAttributeValues;

        public MetaDataZY02C(string _name, string _qrst_code)
        {
            zy02cAttributeValues = new string[zy02cAttributeNames.Length];
            NAME = _name;
            QRST_CODE = _qrst_code;
        }

        public MetaDataZY02C()
        {
            zy02cAttributeValues = new string[zy02cAttributeNames.Length];
        }
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
            set { zy02cAttributeValues[0]= value; }
            get { return zy02cAttributeValues[0]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReceiveStationID
		{
			set{ zy02cAttributeValues[1]=value;}
            get { return zy02cAttributeValues[1]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SensorID
		{
            set { zy02cAttributeValues[2]= value; }
            get
            {
                return zy02cAttributeValues[2];
            }
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ReceiveTime
		{
            set { zy02cAttributeValues[3] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy02cAttributeValues[3], out dt))
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
            set { zy02cAttributeValues[4]= value; }
            get { return zy02cAttributeValues[4]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttType
		{
            set { zy02cAttributeValues[5] = value; }
            get { return zy02cAttributeValues[5]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string StripID
		{
            set { zy02cAttributeValues[6] = value; }
            get { return zy02cAttributeValues[6]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProduceType
		{
            set { zy02cAttributeValues[7] = value; }
            get { return zy02cAttributeValues[7]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneID
		{
            set { zy02cAttributeValues[8] = value; }
            get { return zy02cAttributeValues[8]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string DDSFlag
		{
            set { zy02cAttributeValues[9] = value; }
            get { return zy02cAttributeValues[9]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductID
		{
            set { zy02cAttributeValues[10] = value; }
            get { return zy02cAttributeValues[10]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductLevel
		{
            set { zy02cAttributeValues[11] = value; }
            get { return zy02cAttributeValues[11]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductFormat
		{
            set { zy02cAttributeValues[12] = value; }
            get { return zy02cAttributeValues[12]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ProduceTime
		{
            set { zy02cAttributeValues[13] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy02cAttributeValues[13], out dt))
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
            set { zy02cAttributeValues[14] = value; }
            get { return zy02cAttributeValues[14]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ScenePath
		{
            set { zy02cAttributeValues[15] = value; }
            get { return zy02cAttributeValues[15]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneRow
		{
            set { zy02cAttributeValues[16] = value; }
            get { return zy02cAttributeValues[16]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatPath
		{
            set { zy02cAttributeValues[17] = value; }
            get { return zy02cAttributeValues[17]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatRow
		{
            set { zy02cAttributeValues[18] = value; }
            get { return zy02cAttributeValues[18]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneCount
		{
            set { zy02cAttributeValues[19] = value; }
            get { return zy02cAttributeValues[19]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneShift
		{
            set { zy02cAttributeValues[20] = value; }
            get { return zy02cAttributeValues[20]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime StartTime
		{
            set { zy02cAttributeValues[21] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy02cAttributeValues[21], out dt))
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
            set { zy02cAttributeValues[22] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(zy02cAttributeValues[22], out dt))
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
            set { zy02cAttributeValues[23] = value.ToString(); }
            get
            {
                DateTime dt;
                try
                {
                    if (DateTime.TryParse(zy02cAttributeValues[23], out dt))
                    {
                        return dt;
                    }
                    else
                    {

                    }
                }
                catch
                {
                    throw;
                }
                return dt;
            }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ImageGSD
		{
            set { zy02cAttributeValues[24] = value; }
            get { return zy02cAttributeValues[24]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string WidthInPixels
		{
            set { zy02cAttributeValues[25] = value; }
            get { return zy02cAttributeValues[25]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string HeightInPixels
		{
            set { zy02cAttributeValues[26] = value; }
            get { return zy02cAttributeValues[26]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public double CloudPercent
		{
            set { zy02cAttributeValues[27] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[27], out dt))
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
		public string DataSize
		{
            set { zy02cAttributeValues[28] = value; }
            get { return zy02cAttributeValues[28]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string RollViewingAngle
		{
            set { zy02cAttributeValues[29] = value; }
            get { return zy02cAttributeValues[29]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string PitchViewingAngle
		{
            set { zy02cAttributeValues[30] = value; }
            get { return zy02cAttributeValues[30]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string RollSatelliteAngle
		{
            set { zy02cAttributeValues[31] = value; }
            get { return zy02cAttributeValues[31]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string PitchSatelliteAngle
		{
            set { zy02cAttributeValues[32] = value; }
            get { return zy02cAttributeValues[32]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string YawSatelliteAngle
		{
            set { zy02cAttributeValues[33] = value; }
            get { return zy02cAttributeValues[33]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SolarAzimuth
		{
            set { zy02cAttributeValues[34] = value; }
            get { return zy02cAttributeValues[34]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SolarZenith
		{
            set { zy02cAttributeValues[35] = value; }
            get { return zy02cAttributeValues[35]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatelliteAzimuth
		{
            set { zy02cAttributeValues[36] = value; }
            get { return zy02cAttributeValues[36]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatelliteZenith
		{
            set { zy02cAttributeValues[37] = value; }
            get { return zy02cAttributeValues[37]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string GainMode
		{
            set { zy02cAttributeValues[38] = value; }
            get { return zy02cAttributeValues[38]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string IntegrationTime
		{
            set { zy02cAttributeValues[39] = value; }
            get { return zy02cAttributeValues[39]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string IntegrationLevel
		{
            set { zy02cAttributeValues[40] = value; }
            get { return zy02cAttributeValues[40]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string EphemerisData
		{
            set { zy02cAttributeValues[41] = value; }
            get { return zy02cAttributeValues[41]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttitudeData
		{
            set { zy02cAttributeValues[42] = value; }
            get { return zy02cAttributeValues[42]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string RadiometricMethod
		{
            set { zy02cAttributeValues[43] = value; }
            get { return zy02cAttributeValues[43]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string MtfCorrection
		{
            set { zy02cAttributeValues[44] = value; }
            get { return zy02cAttributeValues[44]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Denoise
		{
            set { zy02cAttributeValues[45] = value; }
            get { return zy02cAttributeValues[45]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string RayleighCorrection
		{
            set { zy02cAttributeValues[46] = value; }
            get { return zy02cAttributeValues[46]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string UsedGCPNo
		{
            set { zy02cAttributeValues[47] = value; }
            get { return zy02cAttributeValues[47]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string CenterLatitude
		{
            set { zy02cAttributeValues[48] = value; }
            get { return zy02cAttributeValues[48]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string CenterLongitude
		{
            set { zy02cAttributeValues[49] = value; }
            get { return zy02cAttributeValues[49]; }
		}
		/// <summary>
		/// 
		/// </summary>
		public double DATAUPPERLEFTLAT
		{
            set { zy02cAttributeValues[50] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[50], out dt))
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
            set { zy02cAttributeValues[51] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[51], out dt))
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
            set { zy02cAttributeValues[52] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[52], out dt))
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
            set { zy02cAttributeValues[53] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[53], out dt))
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
            set { zy02cAttributeValues[54] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[54], out dt))
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
            set { zy02cAttributeValues[55] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[55], out dt))
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
            set { zy02cAttributeValues[56] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[56], out dt))
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
            set { zy02cAttributeValues[57] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(zy02cAttributeValues[57], out dt))
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
                for (int i = 0; i < zy02cAttributeNames.Length; i++)
                {
                    node = root.GetElementsByTagName(zy02cAttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        zy02cAttributeValues[i] = "";
                    }
                    else
                    {
                        zy02cAttributeValues[i] = node.InnerText;
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
                Constant.IdbOperating.LockTable("prod_zy02c",EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_zy02c where Name ='{0}'", NAME);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_zy02c where QRST_CODE ='{0}'", QRST_CODE);
                    //DataSet ds = sqlBase.GetDataSet(presql);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_zy02c");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_zy02c", ID);
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_zy02c(");
                strSql.Append("ID,NAME,SatelliteID,ReceiveStationID,SensorID,ReceiveTime,OrbitID,AttType,StripID,ProduceType,SceneID,DDSFlag,ProductID,ProductLevel,ProductFormat,ProduceTime,Bands,ScenePath,SceneRow,SatPath,SatRow,SceneCount,SceneShift,StartTime,EndTime,CenterTime,ImageGSD,WidthInPixels,HeightInPixels,CloudPercent,DataSize,RollViewingAngle,PitchViewingAngle,RollSatelliteAngle,PitchSatelliteAngle,YawSatelliteAngle,SolarAzimuth,SolarZenith,SatelliteAzimuth,SatelliteZenith,GainMode,IntegrationTime,IntegrationLevel,EphemerisData,AttitudeData,RadiometricMethod,MtfCorrection,Denoise,RayleighCorrection,UsedGCPNo,CenterLatitude,CenterLongitude,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,QRST_CODE,CorDataFlag)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},'{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}'," +
                        "'{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}','{50}','{51}',{52},{53},{54},{55},{56},{57},{58},{59},'{60}','{61}')", ID, NAME,
                        SatelliteID, ReceiveStationID, SensorID, ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss"), OrbitID, AttType, StripID, ProduceType,
                        SceneID, DDSFlag, ProductID, ProductLevel, ProductFormat, ProduceTime.ToString("yyyy-MM-dd HH:mm:ss"), Bands, ScenePath,
                        SceneRow, SatPath, SatRow, SceneCount, SceneShift, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), CenterTime.ToString("yyyy-MM-dd HH:mm:ss"), ImageGSD,
                        WidthInPixels, HeightInPixels, CloudPercent, DataSize, RollViewingAngle, PitchViewingAngle,
                        RollSatelliteAngle, PitchSatelliteAngle, YawSatelliteAngle, SolarAzimuth, SolarZenith,
                        SatelliteAzimuth, SatelliteZenith, GainMode, IntegrationTime, IntegrationLevel, EphemerisData,
                        AttitudeData, RadiometricMethod, MtfCorrection, Denoise, RayleighCorrection, UsedGCPNo,
                        CenterLatitude, CenterLongitude, DATAUPPERLEFTLAT, DATAUPPERLEFTLONG, DATAUPPERRIGHTLAT,
                        DATAUPPERRIGHTLONG, DATALOWERRIGHTLAT, DATALOWERRIGHTLONG, DATALOWERLEFTLAT, DATALOWERLEFTLONG,
                        QRST_CODE, CorDataFlag));
                //           strSql.Append("@ID,@NAME,@SatelliteID,@ReceiveStationID,@SensorID,@ReceiveTime,@OrbitID,@AttType,@StripID,@ProduceType,@SceneID,@DDSFlag,@ProductID,@ProductLevel,@ProductFormat,@ProduceTime,@Bands,@ScenePath,@SceneRow,@SatPath,@SatRow,@SceneCount,@SceneShift,@StartTime,@EndTime,@CenterTime,@ImageGSD,@WidthInPixels,@HeightInPixels,@CloudPercent,@DataSize,@RollViewingAngle,@PitchViewingAngle,@RollSatelliteAngle,@PitchSatelliteAngle,@YawSatelliteAngle,@SolarAzimuth,@SolarZenith,@SatelliteAzimuth,@SatelliteZenith,@GainMode,@IntegrationTime,@IntegrationLevel,@EphemerisData,@AttitudeData,@RadiometricMethod,@MtfCorrection,@Denoise,@RayleighCorrection,@UsedGCPNo,@CenterLatitude,@CenterLongitude,@DATAUPPERLEFTLAT,@DATAUPPERLEFTLONG,@DATAUPPERRIGHTLAT,@DATAUPPERRIGHTLONG,@DATALOWERRIGHTLAT,@DATALOWERRIGHTLONG,@DATALOWERLEFTLAT,@DATALOWERLEFTLONG,@QRST_CODE,@CorDataFlag)");
                //           MySqlParameter[] parameters = {
                //new MySqlParameter("@ID", MySqlDbType.Decimal,20),
                //new MySqlParameter("@NAME", MySqlDbType.Text),
                //new MySqlParameter("@SatelliteID", MySqlDbType.VarChar,20),
                //new MySqlParameter("@ReceiveStationID", MySqlDbType.VarChar,45),
                //new MySqlParameter("@SensorID", MySqlDbType.VarChar,20),
                //new MySqlParameter("@ReceiveTime", MySqlDbType.DateTime),
                //new MySqlParameter("@OrbitID", MySqlDbType.VarChar,20),
                //new MySqlParameter("@AttType", MySqlDbType.VarChar,45),
                //new MySqlParameter("@StripID", MySqlDbType.VarChar,45),
                //new MySqlParameter("@ProduceType", MySqlDbType.VarChar,45),
                //new MySqlParameter("@SceneID", MySqlDbType.VarChar,45),
                //new MySqlParameter("@DDSFlag", MySqlDbType.VarChar,45),
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
                //new MySqlParameter("@CloudPercent", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DataSize", MySqlDbType.VarChar,45),
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
                //new MySqlParameter("@EphemerisData", MySqlDbType.VarChar,45),
                //new MySqlParameter("@AttitudeData", MySqlDbType.VarChar,45),
                //new MySqlParameter("@RadiometricMethod", MySqlDbType.VarChar,45),
                //new MySqlParameter("@MtfCorrection", MySqlDbType.VarChar,45),
                //new MySqlParameter("@Denoise", MySqlDbType.VarChar,45),
                //new MySqlParameter("@RayleighCorrection", MySqlDbType.VarChar,45),
                //new MySqlParameter("@UsedGCPNo", MySqlDbType.VarChar,45),
                //new MySqlParameter("@CenterLatitude", MySqlDbType.VarChar,45),
                //new MySqlParameter("@CenterLongitude", MySqlDbType.VarChar,45),
                //new MySqlParameter("@DATAUPPERLEFTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATAUPPERLEFTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATAUPPERRIGHTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATAUPPERRIGHTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERRIGHTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERRIGHTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERLEFTLAT", MySqlDbType.Decimal,10),
                //new MySqlParameter("@DATALOWERLEFTLONG", MySqlDbType.Decimal,10),
                //new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45),
                //new MySqlParameter("@CorDataFlag", MySqlDbType.Int16,4)};
                //           parameters[0].Value = ID;
                //           parameters[1].Value = NAME;
                //           parameters[2].Value = SatelliteID;
                //           parameters[3].Value = ReceiveStationID;
                //           parameters[4].Value = SensorID;
                //           parameters[5].Value = ReceiveTime;
                //           parameters[6].Value = OrbitID;
                //           parameters[7].Value = AttType;
                //           parameters[8].Value = StripID;
                //           parameters[9].Value = ProduceType;
                //           parameters[10].Value = SceneID;
                //           parameters[11].Value = DDSFlag;
                //           parameters[12].Value = ProductID;
                //           parameters[13].Value = ProductLevel;
                //           parameters[14].Value = ProductFormat;
                //           parameters[15].Value = ProduceTime;
                //           parameters[16].Value = Bands;
                //           parameters[17].Value = ScenePath;
                //           parameters[18].Value = SceneRow;
                //           parameters[19].Value = SatPath;
                //           parameters[20].Value = SatRow;
                //           parameters[21].Value = SceneCount;
                //           parameters[22].Value = SceneShift;
                //           parameters[23].Value = StartTime;
                //           parameters[24].Value = EndTime;
                //           parameters[25].Value = CenterTime;
                //           parameters[26].Value = ImageGSD;
                //           parameters[27].Value = WidthInPixels;
                //           parameters[28].Value = HeightInPixels;
                //           parameters[29].Value = CloudPercent;
                //           parameters[30].Value = DataSize;
                //           parameters[31].Value = RollViewingAngle;
                //           parameters[32].Value = PitchViewingAngle;
                //           parameters[33].Value = RollSatelliteAngle;
                //           parameters[34].Value = PitchSatelliteAngle;
                //           parameters[35].Value = YawSatelliteAngle;
                //           parameters[36].Value = SolarAzimuth;
                //           parameters[37].Value = SolarZenith;
                //           parameters[38].Value = SatelliteAzimuth;
                //           parameters[39].Value = SatelliteZenith;
                //           parameters[40].Value = GainMode;
                //           parameters[41].Value = IntegrationTime;
                //           parameters[42].Value = IntegrationLevel;
                //           parameters[43].Value = EphemerisData;
                //           parameters[44].Value = AttitudeData;
                //           parameters[45].Value = RadiometricMethod;
                //           parameters[46].Value = MtfCorrection;
                //           parameters[47].Value = Denoise;
                //           parameters[48].Value = RayleighCorrection;
                //           parameters[49].Value = UsedGCPNo;
                //           parameters[50].Value = CenterLatitude;
                //           parameters[51].Value = CenterLongitude;
                //           parameters[52].Value = DATAUPPERLEFTLAT;
                //           parameters[53].Value = DATAUPPERLEFTLONG;
                //           parameters[54].Value = DATAUPPERRIGHTLAT;
                //           parameters[55].Value = DATAUPPERRIGHTLONG;
                //           parameters[56].Value = DATALOWERRIGHTLAT;
                //           parameters[57].Value = DATALOWERRIGHTLONG;
                //           parameters[58].Value = DATALOWERLEFTLAT;
                //           parameters[59].Value = DATALOWERLEFTLONG;
                //           parameters[60].Value = QRST_CODE;
                //           parameters[61].Value = CorDataFlag;

                sqlBase.ExecuteSql(strSql.ToString());

                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_zy02c set CorDataFlag = {0} where Name = '{1}'", corDataPath, NAME);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_zy02c",EnumDBType.MIDB);
            }
            catch(Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }

        //ZY02C_PMS_E122.1_N52.9_20130706_L1C0001260319.tar.gz
        //ZY02C_HRC_E122.6_N52.0_20130902_L1C0001338607.tar.gz
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
                return string.Format("实验验证数据库\\ZY02C卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(NAME)));
            }
            else
            {
                return base.GetRelateDataPath();
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

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from prod_zy02c ");
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
		
    }
}
