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

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataSJ9A : MetaData
	{
		public string[] sj9AttributeNames = {
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
		public string[] sj9AttributeValues;

		public MetaDataSJ9A(string _name, string _qrst_code)
		{
			sj9AttributeValues = new string[sj9AttributeNames.Length];
			Name = _name;
			QRST_CODE = _qrst_code;
		}

		public MetaDataSJ9A()
		{
			sj9AttributeValues = new string[sj9AttributeNames.Length];
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
			set
			{
				sj9AttributeValues[0] = value;
			}
			get
			{
				return sj9AttributeValues[0];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SensorID
		{
			set
			{
				sj9AttributeValues[1] = value;
			}
			get
			{
				return sj9AttributeValues[1];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ReceiveTime
		{
			set
			{
				sj9AttributeValues[2] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(sj9AttributeValues[2], out dt))
				{
                    return dt;
				}
                return dt;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrbitID
		{
			set
			{
				sj9AttributeValues[3] = value;
			}
			get
			{
				return sj9AttributeValues[3];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public double DATAUPPERLEFTLAT
		{
			set
			{
				sj9AttributeValues[4] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[4], out dt))
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
			set
			{
				sj9AttributeValues[5] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[5], out dt))
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
			set
			{
				sj9AttributeValues[6] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[6], out dt))
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
			set
			{
				sj9AttributeValues[7] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[7], out dt))
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
			set
			{
				sj9AttributeValues[8] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[8], out dt))
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
			set
			{
				sj9AttributeValues[9] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[9], out dt))
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
			set
			{
				sj9AttributeValues[10] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[10], out dt))
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
			set
			{
				sj9AttributeValues[11] = value.ToString();
			}
			get
			{
				double dt;
				if (double.TryParse(sj9AttributeValues[11], out dt))
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
			set
			{
				sj9AttributeValues[12] = value;
			}
			get
			{
				return sj9AttributeValues[12];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneID
		{
			set
			{
				sj9AttributeValues[13] = value;
			}
			get
			{
				return sj9AttributeValues[13];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductID
		{
			set
			{
				sj9AttributeValues[14] = value;
			}
			get
			{
				return sj9AttributeValues[14];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductLevel
		{
			set
			{
				sj9AttributeValues[15] = value;
			}
			get
			{
				return sj9AttributeValues[15];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductQuality
		{
			set
			{
				sj9AttributeValues[16] = value;
			}
			get
			{
				return sj9AttributeValues[16];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductQualityReport
		{
			set
			{
				sj9AttributeValues[17] = value;
			}
			get
			{
				return sj9AttributeValues[17];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProductFormat
		{
			set
			{
				sj9AttributeValues[18] = value;
			}
			get
			{
				return sj9AttributeValues[18];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ProduceTime
		{
			set
			{
				sj9AttributeValues[19] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(sj9AttributeValues[19], out dt))
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
			set
			{
				sj9AttributeValues[20] = value;
			}
			get
			{
				return sj9AttributeValues[20];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ScenePath
		{
			set
			{
				sj9AttributeValues[21] = value;
			}
			get
			{
				return sj9AttributeValues[21];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneRow
		{
			set
			{
				sj9AttributeValues[22] = value;
			}
			get
			{
				return sj9AttributeValues[22];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatPath
		{
			set
			{
				sj9AttributeValues[23] = value;
			}
			get
			{
				return sj9AttributeValues[23];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatRow
		{
			set
			{
				sj9AttributeValues[24] = value;
			}
			get
			{
				return sj9AttributeValues[24];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneCount
		{
			set
			{
				sj9AttributeValues[25] = value;
			}
			get
			{
				return sj9AttributeValues[25];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SceneShift
		{
			set
			{
				sj9AttributeValues[26] = value;
			}
			get
			{
				return sj9AttributeValues[26];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime StartTime
		{
			set
			{
				sj9AttributeValues[27] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(sj9AttributeValues[27], out dt))
				{
                    return dt;
				}
                return dt;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime EndTime
		{
			set
			{
				sj9AttributeValues[28] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(sj9AttributeValues[28], out dt))
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
			set
			{
				sj9AttributeValues[29] = value.ToString();
			}
			get
			{
				DateTime dt;
				if (DateTime.TryParse(sj9AttributeValues[29], out dt))
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
			set
			{
				sj9AttributeValues[30] = value;
			}
			get
			{
				return sj9AttributeValues[30];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WidthInPixels
		{
			set
			{
				sj9AttributeValues[31] = value;
			}
			get
			{
				return sj9AttributeValues[31];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HeightInPixels
		{
			set
			{
				sj9AttributeValues[32] = value;
			}
			get
			{
				return sj9AttributeValues[32];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WidthInMeters
		{
			set
			{
				sj9AttributeValues[33] = value;
			}
			get
			{
				return sj9AttributeValues[33];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HeightInMeters
		{
			set
			{
				sj9AttributeValues[34] = value;
			}
			get
			{
				return sj9AttributeValues[34];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CloudPercent
		{
			set
			{
				sj9AttributeValues[35] = value;
			}
			get
			{
				return sj9AttributeValues[35];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string QualityInfo
		{
			set
			{
				sj9AttributeValues[36] = value;
			}
			get
			{
				return sj9AttributeValues[36];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PixelBits
		{
			set
			{
				sj9AttributeValues[37] = value;
			}
			get
			{
				return sj9AttributeValues[37];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ValidPixelBits
		{
			set
			{
				sj9AttributeValues[38] = value;
			}
			get
			{
				return sj9AttributeValues[38];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RollViewingAngle
		{
			set
			{
				sj9AttributeValues[39] = value;
			}
			get
			{
				return sj9AttributeValues[39];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PitchViewingAngle
		{
			set
			{
				sj9AttributeValues[40] = value;
			}
			get
			{
				return sj9AttributeValues[40];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RollSatelliteAngle
		{
			set
			{
				sj9AttributeValues[41] = value;
			}
			get
			{
				return sj9AttributeValues[41];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PitchSatelliteAngle
		{
			set
			{
				sj9AttributeValues[42] = value;
			}
			get
			{
				return sj9AttributeValues[42];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string YawSatelliteAngle
		{
			set
			{
				sj9AttributeValues[43] = value;
			}
			get
			{
				return sj9AttributeValues[43];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SolarAzimuth
		{
			set
			{
				sj9AttributeValues[44] = value;
			}
			get
			{
				return sj9AttributeValues[44];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SolarZenith
		{
			set
			{
				sj9AttributeValues[45] = value;
			}
			get
			{
				return sj9AttributeValues[45];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatelliteAzimuth
		{
			set
			{
				sj9AttributeValues[46] = value;
			}
			get
			{
				return sj9AttributeValues[46];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatelliteZenith
		{
			set
			{
				sj9AttributeValues[47] = value;
			}
			get
			{
				return sj9AttributeValues[47];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GainMode
		{
			set
			{
				sj9AttributeValues[48] = value;
			}
			get
			{
				return sj9AttributeValues[48];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IntegrationTime
		{
			set
			{
				sj9AttributeValues[49] = value;
			}
			get
			{
				return sj9AttributeValues[49];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IntegrationLevel
		{
			set
			{
				sj9AttributeValues[50] = value;
			}
			get
			{
				return sj9AttributeValues[50];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MapProjection
		{
			set
			{
				sj9AttributeValues[51] = value;
			}
			get
			{
				return sj9AttributeValues[51];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EarthEllipsoid
		{
			set
			{
				sj9AttributeValues[52] = value;
			}
			get
			{
				return sj9AttributeValues[52];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ZoneNo
		{
			set
			{
				sj9AttributeValues[53] = value;
			}
			get
			{
				return sj9AttributeValues[53];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ResamplingKernel
		{
			set
			{
				sj9AttributeValues[54] = value;
			}
			get
			{
				return sj9AttributeValues[54];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HeightMode
		{
			set
			{
				sj9AttributeValues[55] = value;
			}
			get
			{
				return sj9AttributeValues[55];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MtfCorrection
		{
			set
			{
				sj9AttributeValues[56] = value;
			}
			get
			{
				return sj9AttributeValues[56];
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RelativeCorrectionData
		{
			set
			{
				sj9AttributeValues[57] = value;
			}
			get
			{
				return sj9AttributeValues[57];
			}
		}
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
                Constant.IdbOperating.LockTable("prod_sj9a",EnumDBType.MIDB);
				string presql = string.Format("select ID,QRST_CODE from prod_sj9a where Name ='{0}'", Name);
				DataSet ds = sqlBase.GetDataSet(presql);
				if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
				{
					ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
					QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
					presql = string.Format("delete from prod_sj9a where QRST_CODE ='{0}'", QRST_CODE);
					//DataSet ds = sqlBase.GetDataSet(presql);
					int i = sqlBase.ExecuteSql(presql);
				}
				else
				{
					tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
					ID = sqlBase.GetMaxID("ID", "prod_sj9a");
					QRST_CODE = tablecode.GetDataQRSTCode("prod_sj9a", ID);
				}

				StringBuilder strSql = new StringBuilder();
				strSql.Append("insert into prod_sj9a(");
                strSql.Append(
                    "ID,Name,SatelliteID,SensorID,ReceiveTime,OrbitID,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT,DATALOWERLEFTLONG,ProduceType,SceneID,ProductID,ProductLevel,ProductQuality,ProductQualityReport,ProductFormat,ProduceTime,Bands,ScenePath,SceneRow,SatPath,SatRow,SceneCount,SceneShift,StartTime,EndTime,CenterTime,ImageGSD,WidthInPixels,HeightInPixels,WidthInMeters,HeightInMeters,CloudPercent,QualityInfo,PixelBits,ValidPixelBits,RollViewingAngle,PitchViewingAngle,RollSatelliteAngle,PitchSatelliteAngle,YawSatelliteAngle,SolarAzimuth,SolarZenith,SatelliteAzimuth,SatelliteZenith,GainMode,IntegrationTime,IntegrationLevel,MapProjection,EarthEllipsoid,ZoneNo,ResamplingKernel,HeightMode,MtfCorrection,RelativeCorrectionData,QRST_CODE)");
				strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},'{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}', '{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}')",
                        ID, Name, SatelliteID, SensorID, ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss"), OrbitID, DATAUPPERLEFTLAT, DATAUPPERLEFTLONG,
                        DATAUPPERRIGHTLAT, DATAUPPERRIGHTLONG, DATALOWERRIGHTLAT, DATALOWERRIGHTLONG, DATALOWERLEFTLAT,
                        DATALOWERLEFTLONG, ProduceType, SceneID, ProductID, ProductLevel, ProductQuality,
                        ProductQualityReport, ProductFormat, ProduceTime.ToString("yyyy-MM-dd HH:mm:ss"), Bands, ScenePath, SceneRow, SatPath, SatRow,
                        SceneCount, SceneShift, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), CenterTime.ToString("yyyy-MM-dd HH:mm:ss"), ImageGSD, WidthInPixels, HeightInPixels,
                        WidthInMeters, HeightInMeters, CloudPercent, QualityInfo, PixelBits, ValidPixelBits,
                        RollViewingAngle, PitchViewingAngle, RollSatelliteAngle, PitchSatelliteAngle, YawSatelliteAngle,
                        SolarAzimuth, SolarZenith, SatelliteAzimuth, SatelliteZenith, GainMode, IntegrationTime,
                        IntegrationLevel, MapProjection, EarthEllipsoid, ZoneNo, ResamplingKernel, HeightMode,
                        MtfCorrection, RelativeCorrectionData, QRST_CODE));

                //strSql.Append("@ID,@Name,@SatelliteID,@SensorID,@ReceiveTime,@OrbitID,@DATAUPPERLEFTLAT,@DATAUPPERLEFTLONG,@DATAUPPERRIGHTLAT,@DATAUPPERRIGHTLONG,@DATALOWERRIGHTLAT,@DATALOWERRIGHTLONG,@DATALOWERLEFTLAT,@DATALOWERLEFTLONG,@ProduceType,@SceneID,@ProductID,@ProductLevel,@ProductQuality,@ProductQualityReport,@ProductFormat,@ProduceTime,@Bands,@ScenePath,@SceneRow,@SatPath,@SatRow,@SceneCount,@SceneShift,@StartTime,@EndTime,@CenterTime,@ImageGSD,@WidthInPixels,@HeightInPixels,@WidthInMeters,@HeightInMeters,@CloudPercent,@QualityInfo,@PixelBits,@ValidPixelBits,@RollViewingAngle,@PitchViewingAngle,@RollSatelliteAngle,@PitchSatelliteAngle,@YawSatelliteAngle,@SolarAzimuth,@SolarZenith,@SatelliteAzimuth,@SatelliteZenith,@GainMode,@IntegrationTime,@IntegrationLevel,@MapProjection,@EarthEllipsoid,@ZoneNo,@ResamplingKernel,@HeightMode,@MtfCorrection,@RelativeCorrectionData,@QRST_CODE)");
                //MySqlParameter[] parameters = {
                //	new MySqlParameter("@ID", MySqlDbType.Decimal,20),
                //	new MySqlParameter("@Name", MySqlDbType.Text),
                //	new MySqlParameter("@SatelliteID", MySqlDbType.VarChar,20),
                //	new MySqlParameter("@SensorID", MySqlDbType.VarChar,20),
                //	new MySqlParameter("@ReceiveTime", MySqlDbType.DateTime),
                //	new MySqlParameter("@OrbitID", MySqlDbType.VarChar,20),
                //	new MySqlParameter("@DATAUPPERLEFTLAT", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@DATAUPPERLEFTLONG", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@DATAUPPERRIGHTLAT", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@DATAUPPERRIGHTLONG", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@DATALOWERRIGHTLAT", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@DATALOWERRIGHTLONG", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@DATALOWERLEFTLAT", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@DATALOWERLEFTLONG", MySqlDbType.Decimal,10),
                //	new MySqlParameter("@ProduceType", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SceneID", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ProductID", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ProductLevel", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ProductQuality", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ProductQualityReport", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ProductFormat", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ProduceTime", MySqlDbType.DateTime),
                //	new MySqlParameter("@Bands", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ScenePath", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SceneRow", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SatPath", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SatRow", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SceneCount", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SceneShift", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@StartTime", MySqlDbType.DateTime),
                //	new MySqlParameter("@EndTime", MySqlDbType.DateTime),
                //	new MySqlParameter("@CenterTime", MySqlDbType.DateTime),
                //	new MySqlParameter("@ImageGSD", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@WidthInPixels", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@HeightInPixels", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@WidthInMeters", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@HeightInMeters", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@CloudPercent", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@QualityInfo", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@PixelBits", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ValidPixelBits", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@RollViewingAngle", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@PitchViewingAngle", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@RollSatelliteAngle", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@PitchSatelliteAngle", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@YawSatelliteAngle", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SolarAzimuth", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SolarZenith", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SatelliteAzimuth", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@SatelliteZenith", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@GainMode", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@IntegrationTime", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@IntegrationLevel", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@MapProjection", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@EarthEllipsoid", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ZoneNo", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@ResamplingKernel", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@HeightMode", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@MtfCorrection", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@RelativeCorrectionData", MySqlDbType.VarChar,45),
                //	new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45)};
                //parameters[0].Value = ID;
                //parameters[1].Value = Name;
                //parameters[2].Value = SatelliteID;
                //parameters[3].Value = SensorID;
                //parameters[4].Value = ReceiveTime;
                //parameters[5].Value = OrbitID;
                //parameters[6].Value = DATAUPPERLEFTLAT;
                //parameters[7].Value = DATAUPPERLEFTLONG;
                //parameters[8].Value = DATAUPPERRIGHTLAT;
                //parameters[9].Value = DATAUPPERRIGHTLONG;
                //parameters[10].Value = DATALOWERRIGHTLAT;
                //parameters[11].Value = DATALOWERRIGHTLONG;
                //parameters[12].Value = DATALOWERLEFTLAT;
                //parameters[13].Value = DATALOWERLEFTLONG;
                //parameters[14].Value = ProduceType;
                //parameters[15].Value = SceneID;
                //parameters[16].Value = ProductID;
                //parameters[17].Value = ProductLevel;
                //parameters[18].Value = ProductQuality;
                //parameters[19].Value = ProductQualityReport;
                //parameters[20].Value = ProductFormat;
                //parameters[21].Value = ProduceTime;
                //parameters[22].Value = Bands;
                //parameters[23].Value = ScenePath;
                //parameters[24].Value = SceneRow;
                //parameters[25].Value = SatPath;
                //parameters[26].Value = SatRow;
                //parameters[27].Value = SceneCount;
                //parameters[28].Value = SceneShift;
                //parameters[29].Value = StartTime;
                //parameters[30].Value = EndTime;
                //parameters[31].Value = CenterTime;
                //parameters[32].Value = ImageGSD;
                //parameters[33].Value = WidthInPixels;
                //parameters[34].Value = HeightInPixels;
                //parameters[35].Value = WidthInMeters;
                //parameters[36].Value = HeightInMeters;
                //parameters[37].Value = CloudPercent;
                //parameters[38].Value = QualityInfo;
                //parameters[39].Value = PixelBits;
                //parameters[40].Value = ValidPixelBits;
                //parameters[41].Value = RollViewingAngle;
                //parameters[42].Value = PitchViewingAngle;
                //parameters[43].Value = RollSatelliteAngle;
                //parameters[44].Value = PitchSatelliteAngle;
                //parameters[45].Value = YawSatelliteAngle;
                //parameters[46].Value = SolarAzimuth;
                //parameters[47].Value = SolarZenith;
                //parameters[48].Value = SatelliteAzimuth;
                //parameters[49].Value = SatelliteZenith;
                //parameters[50].Value = GainMode;
                //parameters[51].Value = IntegrationTime;
                //parameters[52].Value = IntegrationLevel;
                //parameters[53].Value = MapProjection;
                //parameters[54].Value = EarthEllipsoid;
                //parameters[55].Value = ZoneNo;
                //parameters[56].Value = ResamplingKernel;
                //parameters[57].Value = HeightMode;
                //parameters[58].Value = MtfCorrection;
                //parameters[59].Value = RelativeCorrectionData;
                //parameters[60].Value = QRST_CODE;

                sqlBase.ExecuteSql(strSql.ToString());


				string destCorrectedData = this.GetCorrectedDataPath();
				//如果纠正归档数据目录存在且里面有文件则1，否则为-1
				string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
				string updatesql = string.Format("update prod_sj9a set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

				sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_sj9a",EnumDBType.MIDB);
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
				for (int i = 0; i < sj9AttributeNames.Length; i++)
				{
					node = root.GetElementsByTagName(sj9AttributeNames[i]).Item(0);
					if (node == null)
					{
						sj9AttributeValues[i] = "";
					}
					else
					{
						sj9AttributeValues[i] = node.InnerText;
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
				string year = strArr[4].Substring(0, 4);
				string month = strArr[4].Substring(4, 2);
				string day = strArr[4].Substring(6, 2);
				return string.Format("实验验证数据库\\SJ9A卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)));
			}
			else
			{
				return base.GetRelateDataPath();
			}

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
			strSql.Append("select * from prod_sj9a ");
			strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

			using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
			{
				if (ds.Tables[0].Rows.Count > 0)
				{
					Name = ds.Tables[0].Rows[0]["NAME"].ToString();
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
