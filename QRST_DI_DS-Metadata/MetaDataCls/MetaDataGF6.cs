using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataGF6 : MetaData
    {
        public string[] gf6AttributeNames = { 
                                                //表字段
                                                "SatelliteID",    //卫星名0
                                                "ReceiveStationID",   //接收地面站代号1
                                                 "SensorID",  //传感器2
                                                 "ReceiveTime",  //接收时间3
                                                 "OrbitID",  //观测圈号4
                                                 "TopLeftLatitude",//61
                                                 "TopLeftLongitude",//62
                                                 "TopRightLatitude",//63
                                                 "TopRightLongitude",//64
                                                 "BottomRightLatitude",//65
                                                 "BottomRightLongitude",//66
                                                 "BottomLeftLatitude",//67
                                                 "BottomLeftLongitude",   //68
                                                 "OrbitType",  //产品格式5
                                                 "AttType",      //景号6
                                                 "StripID",   //产品号7
                                                 "ProduceType",   //观测圈号8
                                                 "SceneID",   //采集起始时间9
                                                 "DDSFlag",    //采集终止时间10
                                                 "ProductID",   //工作模式11
                                                 "ProductLevel",  //定标系数来源12
                                                 "ProductFormat",   //定标系数版本13
                                                 "ProduceTime",  //星下点分辨率14
                                                 "Bands",  //坐标投影方式15
                                                 "ScenePath",  //椭球模型16
                                                 "SceneRow",  //投影后网格大小17
                                                 "SatPath",  //投影区采样点18
                                                 "SatRow",  //采样点经度19
                                                 "SceneCount",   //采样点纬度20
                                                 "SceneShift",    // 发布标识21
                                                 "StartTime", //开始时间22
                                                 "EndTime",  //结束时间23
                                                 "CenterTime",  //24
                                                 "StartLine",   //25
                                                 "EndLine",     //26
                                                 "ImageGSD",   //27
                                                 "WidthInPixels",  //28
                                                 "HeightInPixels",  //29
                                                 "WidthInMeters",  //30
                                                 "HeightInMeters",  //31
                                                 "RegionName",   //32
                                                 "CloudPercent",  //33
                                                 "DataSize",    //34
                                                 "RollViewingAngle",//35
                                                 "PitchViewingAngle",//36
                                                 "PitchSatelliteAngle",//37
                                                 "RollSatelliteAngle",//38
                                                 "YawSatelliteAngle",//39
                                                 "SolarAzimuth",  //40
                                                 "SolarZenith",  //41
                                                 "SatelliteAzimuth",//42
                                                 "SatelliteZenith", //43
                                                 "GainMode",//44
                                                 "IntegrationTime",//45
                                                 "IntegrationLevel",//46
                                                 "MapProjection",//47
                                                 "EarthEllipsoid",//48
                                                 "ZoneNo",//49
                                                 "ResamplingKernel",//50
                                                 "HeightMode",//51
                                                 "EphemerisData",//52
                                                 "AttitudeData",//53
                                                 "RadiometricMethod",//54
                                                 "MtfCorrection", //55
                                                 "Denoise",//56
                                                 "RayleighCorrection",//57
                                                 "UsedGCPNo",//58
                                                 "CenterLatitude",//59
                                                 "CenterLongitude",//60                                                                                          
                                                 "TopLeftMapX",//69
                                                 "TopLeftMapY",//70
                                                 "TopRightMapX",//71
                                                 "TopRightMapY",//72
                                                 "BottomRightMapX",//73
                                                 "BottomRightMapY",//74
                                                 "BottomLeftMapX",//75
                                                 "BottomLeftMapY",//76
                                                 "DataArchiveFile",//77
                                                 "BrowseFileLocation",//78
                                                 "ThumbFileLocation",  //79                                      
                                            };
        public string[] gf6AttibuteValues;

        public MetaDataGF6(string _name, string _qrst_code)
        {
            gf6AttibuteValues = new string[gf6AttributeNames.Length];
            Name = _name;
            QRST_CODE = _qrst_code;
        }
        public MetaDataGF6()
        {
            gf6AttibuteValues = new string[gf6AttributeNames.Length];
        }

        #region  Model
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

        public string SatelliteID
        {
            set { gf6AttibuteValues[0] = value; }
            get { return gf6AttibuteValues[0]; }
        }
        public string ReceiveStationID
        {
            set { gf6AttibuteValues[1] = value; }
            get { return gf6AttibuteValues[1]; }
        }

        public string SensorID
        {
            set { gf6AttibuteValues[2] = value; }
            get { return gf6AttibuteValues[2]; }
        }

        public DateTime ReceiveTime
        {
            set { gf6AttibuteValues[3] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf6AttibuteValues[3], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
        public string OrbitID
        {
            set { gf6AttibuteValues[4] = value; }
            get { return gf6AttibuteValues[4]; }
        }

        public double DATAUPPERLEFTLAT
        {
            set { gf6AttibuteValues[5] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[5], out dt))
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
            set { gf6AttibuteValues[6] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[6], out dt))
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
            set { gf6AttibuteValues[7] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[7], out dt))
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
            set { gf6AttibuteValues[8] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[8], out dt))
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
            set { gf6AttibuteValues[9] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[9], out dt))
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
            set { gf6AttibuteValues[10] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[10], out dt))
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
            set { gf6AttibuteValues[11] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[11], out dt))
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
            set { gf6AttibuteValues[12] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf6AttibuteValues[12], out dt))
                {
                    return dt;
                }
                else
                {
                    return -180;
                }
            }
        }

        public double size
        {
            get;
            set;
        }

        public string OrbitType
        {
            set { gf6AttibuteValues[13] = value; }
            get { return gf6AttibuteValues[13]; }
        }

        public string AttType
        {
            set { gf6AttibuteValues[14] = value; }
            get { return gf6AttibuteValues[14]; }
        }


        public string StripID
        {
            set { gf6AttibuteValues[15] = value; }
            get { return gf6AttibuteValues[15]; }
        }

        public string ProduceType
        {
            set { gf6AttibuteValues[16] = value; }
            get { return gf6AttibuteValues[16]; }
        }

        public string SceneID
        {
            set { gf6AttibuteValues[17] = value; }
            get { return gf6AttibuteValues[17]; }
        }
        public string DDSFlag
        {
            set { gf6AttibuteValues[18] = value; }
            get { return gf6AttibuteValues[18]; }
        }

        public string ProductID
        {
            set { gf6AttibuteValues[19] = value; }
            get { return gf6AttibuteValues[19]; }
        }

        public string ProductLevel
        {
            set { gf6AttibuteValues[20] = value; }
            get { return gf6AttibuteValues[20]; }
        }
        public string ProductFormat
        {
            set { gf6AttibuteValues[21] = value; }
            get { return gf6AttibuteValues[21]; }
        }

        public DateTime ProduceTime
        {
            set { gf6AttibuteValues[22] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf6AttibuteValues[22], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        public string Bands
        {
            set { gf6AttibuteValues[23] = value; }
            get { return gf6AttibuteValues[23]; }
        }

        public string ScenePath
        {
            set { gf6AttibuteValues[24] = value; }
            get { return gf6AttibuteValues[24]; }
        }
        public string SceneRow
        {
            set { gf6AttibuteValues[25] = value; }
            get { return gf6AttibuteValues[25]; }
        }

        public string SatPath
        {
            set { gf6AttibuteValues[26] = value; }
            get { return gf6AttibuteValues[26]; }
        }

        public string SatRow
        {
            set { gf6AttibuteValues[27] = value; }
            get { return gf6AttibuteValues[27]; }
        }

        public string SceneCount
        {
            set { gf6AttibuteValues[28] = value; }
            get { return gf6AttibuteValues[28]; }
        }

        public string SceneShift
        {
            set { gf6AttibuteValues[29] = value; }
            get { return gf6AttibuteValues[29]; }
        }
        public DateTime StartTime
        {
            set { gf6AttibuteValues[30] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf6AttibuteValues[30], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        public DateTime EndTime
        {
            set { gf6AttibuteValues[31] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf6AttibuteValues[31], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        public DateTime CenterTime
        {
            set { gf6AttibuteValues[32] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf6AttibuteValues[32], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        public string StartLine
        {
            set { gf6AttibuteValues[33] = value; }
            get { return gf6AttibuteValues[33]; }
        }
        public string EndLine
        {
            set { gf6AttibuteValues[34] = value; }
            get { return gf6AttibuteValues[34]; }
        }
        public string ImageGSD
        {
            set { gf6AttibuteValues[35] = value; }
            get { return gf6AttibuteValues[35]; }
        }
        public string WidthInPixels
        {
            set { gf6AttibuteValues[36] = value; }
            get { return gf6AttibuteValues[36]; }
        }

        public string HeightInPixels
        {
            set { gf6AttibuteValues[37] = value; }
            get { return gf6AttibuteValues[37]; }
        }
        public string WidthInMeters
        {
            set { gf6AttibuteValues[38] = value; }
            get { return gf6AttibuteValues[38]; }
        }
        public string HeightInMeters
        {
            set { gf6AttibuteValues[39] = value; }
            get { return gf6AttibuteValues[39]; }
        }
        public string RegionName
        {
            set { gf6AttibuteValues[40] = value; }
            get { return gf6AttibuteValues[40]; }
        }

        public string CloudPercent
        {
            set { gf6AttibuteValues[41] = value; }
            get { return gf6AttibuteValues[41]; }
        }
        public string DataSize
        {
            set { gf6AttibuteValues[42] = value; }
            get { return gf6AttibuteValues[42]; }
        }
        public string RollViewingAngle
        {
            set { gf6AttibuteValues[43] = value; }
            get { return gf6AttibuteValues[43]; }
        }
        public string PitchViewingAngle
        {
            set { gf6AttibuteValues[44] = value; }
            get { return gf6AttibuteValues[44]; }
        }

        public string PitchSatelliteAngle
        {
            set { gf6AttibuteValues[45] = value; }
            get { return gf6AttibuteValues[45]; }
        }
        public string RollSatelliteAngle
        {
            set { gf6AttibuteValues[46] = value; }
            get { return gf6AttibuteValues[46]; }
        }
        public string YawSatelliteAngle
        {
            set { gf6AttibuteValues[47] = value; }
            get { return gf6AttibuteValues[47]; }
        }
        public string SolarAzimuth
        {
            set { gf6AttibuteValues[48] = value; }
            get { return gf6AttibuteValues[48]; }
        }

        public string SolarZenith
        {
            set { gf6AttibuteValues[49] = value; }
            get { return gf6AttibuteValues[49]; }
        }
        public string SatelliteAzimuth
        {
            set { gf6AttibuteValues[50] = value; }
            get { return gf6AttibuteValues[50]; }
        }
        public string SatelliteZenith
        {
            set { gf6AttibuteValues[51] = value; }
            get { return gf6AttibuteValues[51]; }
        }
        public string GainMode
        {
            set { gf6AttibuteValues[52] = value; }
            get { return gf6AttibuteValues[52]; }
        }

        public string IntegrationTime
        {
            set { gf6AttibuteValues[53] = value; }
            get { return gf6AttibuteValues[53]; }
        }
        public string IntegrationLevel
        {
            set { gf6AttibuteValues[54] = value; }
            get { return gf6AttibuteValues[54]; }
        }
        public string MapProjection
        {
            set { gf6AttibuteValues[55] = value; }
            get { return gf6AttibuteValues[55]; }
        }
        public string EarthEllipsoid
        {
            set { gf6AttibuteValues[56] = value; }
            get { return gf6AttibuteValues[56]; }
        }

        public string ZoneNo
        {
            set { gf6AttibuteValues[57] = value; }
            get { return gf6AttibuteValues[57]; }
        }
        public string ResamplingKernel
        {
            set { gf6AttibuteValues[58] = value; }
            get { return gf6AttibuteValues[58]; }
        }
        public string HeightMode
        {
            set { gf6AttibuteValues[59] = value; }
            get { return gf6AttibuteValues[59]; }
        }
        public string EphemerisData
        {
            set { gf6AttibuteValues[60] = value; }
            get { return gf6AttibuteValues[60]; }
        }

        public string AttitudeData
        {
            set { gf6AttibuteValues[61] = value; }
            get { return gf6AttibuteValues[61]; }
        }
        public string RadiometricMethod
        {
            set { gf6AttibuteValues[62] = value; }
            get { return gf6AttibuteValues[62]; }
        }
        public string MtfCorrection
        {
            set { gf6AttibuteValues[63] = value; }
            get { return gf6AttibuteValues[63]; }
        }
        public string Denoise
        {
            set { gf6AttibuteValues[64] = value; }
            get { return gf6AttibuteValues[64]; }
        }

        public string RayleighCorrection
        {
            set { gf6AttibuteValues[65] = value; }
            get { return gf6AttibuteValues[65]; }
        }
        public string UsedGCPNo
        {
            set { gf6AttibuteValues[66] = value; }
            get { return gf6AttibuteValues[66]; }
        }
        public string CenterLatitude
        {
            set { gf6AttibuteValues[67] = value; }
            get { return gf6AttibuteValues[67]; }
        }
        public string CenterLongitude
        {
            set { gf6AttibuteValues[68] = value; }
            get { return gf6AttibuteValues[68]; }
        }

        public string TopLeftMapX
        {
            set { gf6AttibuteValues[69] = value; }
            get { return gf6AttibuteValues[69]; }
        }
        public string TopLeftMapY
        {
            set { gf6AttibuteValues[70] = value; }
            get { return gf6AttibuteValues[70]; }
        }
        public string TopRightMapX
        {
            set { gf6AttibuteValues[71] = value; }
            get { return gf6AttibuteValues[71]; }
        }
        public string TopRightMapY
        {
            set { gf6AttibuteValues[72] = value; }
            get { return gf6AttibuteValues[72]; }
        }

        public string BottomRightMapX
        {
            set { gf6AttibuteValues[73] = value; }
            get { return gf6AttibuteValues[73]; }
        }
        public string BottomRightMapY
        {
            set { gf6AttibuteValues[74] = value; }
            get { return gf6AttibuteValues[74]; }
        }
        public string BottomLeftMapX
        {
            set { gf6AttibuteValues[75] = value; }
            get { return gf6AttibuteValues[75]; }
        }
        public string BottomLeftMapY
        {
            set { gf6AttibuteValues[76] = value; }
            get { return gf6AttibuteValues[76]; }
        }

        public string DataArchiveFile
        {
            set { gf6AttibuteValues[77] = value; }
            get { return gf6AttibuteValues[77]; }
        }
        public string BrowseFileLocation
        {
            set { gf6AttibuteValues[78] = value; }
            get { return gf6AttibuteValues[78]; }
        }
        public string ThumbFileLocation
        {

            set { gf6AttibuteValues[79] = value; }
            get { return gf6AttibuteValues[79]; }
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
        #endregion

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {

                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("prod_gf6", EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_gf6 where Name ='{0}'", Name);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_gf6 where QRST_CODE ='{0}'", QRST_CODE);
                    //DataSet ds = sqlBase.GetDataSet(presql);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_gf6");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_gf6", ID);
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_gf6(");
                strSql.Append("ID,Name,SatelliteID,ReceiveStationID,SensorID,ReceiveTime,OrbitID,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG," +
                              "DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,DATALOWERLEFTLAT," +
                              "DATALOWERLEFTLONG,OrbitType,AttType,StripID,ProduceType,SceneID,DDSFlag,ProductID,ProductLevel,ProductFormat," +
                              "ProduceTime,Bands,ScenePath,SceneRow,SatPath,SatRow,SceneCount,SceneShift,StartTime,EndTime," +
                              "CenterTime,StartLine,EndLine,ImageGSD,WidthInPixels,HeightInPixels,WidthInMeters,HeightInMeters,RegionName,CloudPercent," +
                              "DataSize,RollViewingAngle,PitchViewingAngle,PitchSatelliteAngle,RollSatelliteAngle,YawSatelliteAngle," +
                              "SolarAzimuth,SolarZenith,SatelliteAzimuth,SatelliteZenith," +
                              "GainMode,IntegrationTime,IntegrationLevel,MapProjection,EarthEllipsoid,ZoneNo,ResamplingKernel,HeightMode,EphemerisData," +
                              "AttitudeData,RadiometricMethod,MtfCorrection,Denoise,RayleighCorrection,UsedGCPNo,CenterLatitude,CenterLongitude,TopLeftMapX,TopLeftMapY," +
                              "TopRightMapX,TopRightMapY,BottomRightMapX,BottomRightMapY,BottomLeftMapX,BottomLeftMapY,DataArchiveFile,BrowseFileLocation,ThumbFileLocation,QRST_CODE,ImportTime,size)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}','{61}','{62}'," +
                        "'{63}','{64}','{65}','{66}','{67}','{78}','{69}','{70}','{71}','{72}','{73}','{74}','{75}'," +
                        "'{76}','{77}','{78}','{79}','{80}','{81}','{82}','{83}',{84})", ID, Name, SatelliteID, ReceiveStationID, SensorID, ReceiveTime,
                        OrbitID, DATAUPPERLEFTLAT, DATAUPPERLEFTLONG, DATAUPPERRIGHTLAT, DATAUPPERRIGHTLONG, DATALOWERRIGHTLAT, DATALOWERRIGHTLONG,
                        DATALOWERLEFTLAT, DATALOWERLEFTLONG, OrbitType, AttType, StripID, ProduceType, SceneID, DDSFlag, ProductID, ProductLevel, ProductFormat,
                        ProduceTime.ToString("yyyy-MM-dd HH:mm:ss"), Bands, ScenePath, SceneRow, SatPath, SatRow, SceneCount, SceneShift, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        CenterTime.ToString("yyyy-MM-dd HH:mm:ss"), StartLine, EndLine, ImageGSD, WidthInPixels, HeightInPixels, WidthInMeters, HeightInMeters, RegionName, CloudPercent,
                        DataSize, RollViewingAngle, PitchViewingAngle, PitchSatelliteAngle, RollSatelliteAngle, YawSatelliteAngle, SolarAzimuth, SolarZenith, SatelliteAzimuth, SatelliteZenith, GainMode, IntegrationTime,
                        IntegrationLevel, MapProjection, EarthEllipsoid, ZoneNo, ResamplingKernel, HeightMode, EphemerisData, AttitudeData, RadiometricMethod, MtfCorrection, Denoise,
                        RayleighCorrection, UsedGCPNo, CenterLatitude, CenterLongitude, TopLeftMapX, TopLeftMapY, TopRightMapX, TopRightMapY, BottomRightMapX, BottomRightMapY,
                        BottomLeftMapX, BottomLeftMapY, DataArchiveFile, BrowseFileLocation, ThumbFileLocation, QRST_CODE, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), size));


                sqlBase.ExecuteSql(strSql.ToString());


                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_gf6 set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_gf6", EnumDBType.MIDB);

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
                for (int i = 0; i < gf6AttributeNames.Length; i++)
                {
                    node = root.GetElementsByTagName(gf6AttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        gf6AttibuteValues[i] = "";
                    }
                    else
                    {
                        gf6AttibuteValues[i] = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NAME,SatelliteID,SensorID,CorDataFlag from prod_gf6 ");
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
                return string.Format("实验验证数据库\\光学卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)));
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
                return string.Format("实验验证数据库\\GF6卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)), Name);
            }
            return "";
        }

        public static bool HasCorrectedData(string qrst_code, IDbBaseUtilities evdb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CorDataFlag from prod_gf6 ");
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


    }
}
