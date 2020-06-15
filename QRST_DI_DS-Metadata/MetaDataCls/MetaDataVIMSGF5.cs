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
    public class MetaDataVIMSGF5 : MetaData
    {

        public string[] gf5vimsAttrName = {
                                              "SatelliteID",    //卫星名0
                                                "ReceiveStationID",   //接收地面站代号1
                                                 "SensorID",  //载荷（传感器）2
                                                 "ProduceTime",  //生产时间3
                                                 "ProductLevel",  //产品级别4
                                                 "ProductFormat",  //产品格式5
                                                 "SceneID",      //景号6
                                                 "ProductID",   //产品号7
                                                 "ProduceType",   //生产方式8
                                                 "ScenePath",   //景Path 9
                                                 "SceneRow",   //景Row  10
                                                 "SatPath",  //星下点Path11
                                                 "SatRow",   //星下点Row 12
                                                 "OrbitID",   //轨道圈号 13
                                                 "POrbitID",   //观测圈号14
                                                 "OrbitType",  //轨道类型15
                                                 "AttType",   //姿态类型16
                                                 "RollSatelliteAngle",   //卫星平台平均滚动角17
                                                 "PitchSatelliteAngle",   //卫星平台仰俯角18
                                                 "YawSatelliteAngle",  //卫星平台航偏角19
                                                 "SolarAzimuth",   //太阳方位角20
                                                 "SolarZenith",   //太阳天顶角21
                                                 "SatelliteAzimuth",   //卫星方位角22
                                                 "SatelliteZenith",  //卫星天顶角23
                                                 "StartTime",   //采集起始时间24
                                                 "EndTime",    //采集终止时间25
                                                 "BandsCount",   //产品谱段数26
                                                 "Bands",   //产品波段27
                                                 "SpectralRangeStart",   //光谱范围开始28
                                                 "SpectralRangeEnd",   //光谱范围结束29
                                                 "CentralWavelength",  //中心波长30
                                                 "ImageGSD",   //产品分辨率31
                                                 "WidthInPixels",    //产品行数32
                                                 "HeightInPixels",   //产品列数33
                                                 "WidthInMeters",   //产品宽34
                                                 "HeightInMeters",   //产品高35
                                                 "CloudPercent",   //云覆盖量36
                                                 "GainMode",  //增益模式37
                                                 "CalibrationParam",  //定标系数来源38
                                                 "CalibrationParamVersion",   //定标系数版本39
                                                 "GAINS",   //增益40
                                                 "OFFSETS",    //偏置41
                                                 "IntegrationTime",   //积分时间42
                                                 "IntegrationLevel",  //积分级别43
                                                 "MapProjection",  //投影方式44
                                                 "EarthEllipsoid",  //椭球模型45
                                                 "ZoneNo",  //投影带号46
                                                 "RadiometricMethod", //辐射校正方法47
                                                 "CalParameterVersion",  //辐射定标系数版本48
                                                 "ResamplingKernel",  //重采样方法49
                                                 "Denoise",   //是否去噪50
                                                 "HeightMode",   //高程模型51
                                                 "EphemerisData",  //星历数据类型52
                                                 "AttitudeData",  //姿态数据类型53
                                                 "CenterLatitude",  //中心纬度54
                                                 "CenterLongitude",  //中心经度55
                                                 "TopLeftLatitude",  //左上纬度56
                                                 "TopLeftLongitude",  //左上经度57
                                                 "TopRightLatitude",  //右上纬度58
                                                 "TopRightLongitude",  //右上经度59
                                                 "BottomRightLatitude",  //右下纬度60
                                                 "BottomRightLongitude",  //右下经度61
                                                 "BottomLeftLatitude",    //左下纬度62
                                                 "BottomLeftLongitude",  //左下经度63
                                                 "TopLeftMapX",  //左上X坐标64
                                                 "TopLeftMapY",  //左上Y坐标65
                                                 "TopRightMapX",  //右上X坐标66
                                                 "TopRightMapY",  //右上Y坐标67
                                                 "BottomRightMapX",  //右下X坐标68
                                                 "BottomRightMapY",  //右下Y坐标69
                                                 "BottomLeftMapX",  //左下X坐标70
                                                 "BottomLeftMapY",  //左下Y坐标71                                            
                                                 "DDSFlag",    // 发布标识72
                                                 "SoftwareVersion", //软件版本73
                                          };
        public string[] gf5vimsAtrValues;

        public MetaDataVIMSGF5(string _name, string _qrst_code)
        {
            gf5vimsAtrValues = new string[gf5vimsAttrName.Length];
            Name = _name;
            QRST_CODE = _qrst_code;
        }

        public MetaDataVIMSGF5()
        {
            gf5vimsAtrValues = new string[gf5vimsAttrName.Length];
        }

        #region model
        public int ID
        {
            set;
            get;
        }
        public string Name
        {
            set;
            get;
        }

        public double size
        {
            set;
            get;
        }

        public string SatelliteID
        {
            set { gf5vimsAtrValues[0] = value; }
            get { return gf5vimsAtrValues[0]; }
        }

        public string ReceiveStationID
        {
            set { gf5vimsAtrValues[1] = value; }
            get { return gf5vimsAtrValues[1]; }
        }

        public string SensorID
        {
            set { gf5vimsAtrValues[2] = value; }
            get { return gf5vimsAtrValues[2]; }
        }

        public DateTime ProduceTime
        {
            set { gf5vimsAtrValues[3] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf5vimsAtrValues[3], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        public string ProductLevel
        {
            set { gf5vimsAtrValues[4] = value; }
            get { return gf5vimsAtrValues[4]; }
        }

        public string ProductFormat
        {
            set { gf5vimsAtrValues[5] = value; }
            get { return gf5vimsAtrValues[5]; }
        }

        public string SceneID
        {
            set { gf5vimsAtrValues[6] = value; }
            get { return gf5vimsAtrValues[6]; }
        }

        public string ProductID
        {
            set { gf5vimsAtrValues[7] = value; }
            get { return gf5vimsAtrValues[7]; }
        }

        public string ProduceType
        {
            set { gf5vimsAtrValues[8] = value; }
            get { return gf5vimsAtrValues[8]; }
        }
        public string ScenePath
        {
            set { gf5vimsAtrValues[9] = value; }
            get { return gf5vimsAtrValues[9]; }
        }
        public string SceneRow
        {
            set { gf5vimsAtrValues[10] = value; }
            get { return gf5vimsAtrValues[10]; }
        }
        public string SatPath
        {
            set { gf5vimsAtrValues[11] = value; }
            get { return gf5vimsAtrValues[11]; }
        }
        public string SatRow
        {
            set { gf5vimsAtrValues[12] = value; }
            get { return gf5vimsAtrValues[12]; }
        }
        public string OrbitID
        {
            set { gf5vimsAtrValues[13] = value; }
            get { return gf5vimsAtrValues[13]; }
        }
        public string POrbitID
        {
            set { gf5vimsAtrValues[14] = value; }
            get { return gf5vimsAtrValues[14]; }
        }
        public string OrbitType
        {
            set { gf5vimsAtrValues[15] = value; }
            get { return gf5vimsAtrValues[15]; }
        }
        public string AttType
        {
            set { gf5vimsAtrValues[16] = value; }
            get { return gf5vimsAtrValues[16]; }
        }
        public string RollSatelliteAngle
        {
            set { gf5vimsAtrValues[17] = value; }
            get { return gf5vimsAtrValues[17]; }
        }
        public string PitchSatelliteAngle
        {
            set { gf5vimsAtrValues[18] = value; }
            get { return gf5vimsAtrValues[18]; }
        }
        public string YawSatelliteAngle
        {
            set { gf5vimsAtrValues[19] = value; }
            get { return gf5vimsAtrValues[19]; }
        }
        public string SolarAzimuth
        {
            set { gf5vimsAtrValues[20] = value; }
            get { return gf5vimsAtrValues[20]; }
        }
        public string SolarZenith
        {
            set { gf5vimsAtrValues[21] = value; }
            get { return gf5vimsAtrValues[21]; }
        }
        public string SatelliteAzimuth
        {
            set { gf5vimsAtrValues[22] = value; }
            get { return gf5vimsAtrValues[22]; }
        }
        public string SatelliteZenith
        {
            set { gf5vimsAtrValues[23] = value; }
            get { return gf5vimsAtrValues[23]; }
        }

        public DateTime StartTime
        {
            set { gf5vimsAtrValues[24] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf5vimsAtrValues[24], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        public DateTime EndTime
        {
            set { gf5vimsAtrValues[25] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf5vimsAtrValues[25], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        public string BandsCount
        {
            set { gf5vimsAtrValues[26] = value; }
            get { return gf5vimsAtrValues[26]; }
        }
        public string Bands
        {
            set { gf5vimsAtrValues[27] = value; }
            get { return gf5vimsAtrValues[27]; }
        }
        public string SpectralRangeStart
        {
            set { gf5vimsAtrValues[28] = value; }
            get { return gf5vimsAtrValues[28]; }
        }
        public string SpectralRangeEnd
        {
            set { gf5vimsAtrValues[29] = value; }
            get { return gf5vimsAtrValues[29]; }
        }
        public string CentralWavelength
        {
            set { gf5vimsAtrValues[30] = value; }
            get { return gf5vimsAtrValues[30]; }

        }
        public string ImageGSD
        {
            set { gf5vimsAtrValues[31] = value; }
            get { return gf5vimsAtrValues[31]; }
        }
        public string WidthInPixels
        {
            set { gf5vimsAtrValues[32] = value; }
            get { return gf5vimsAtrValues[32]; }
        }
        public string HeightInPixels
        {
            set { gf5vimsAtrValues[33] = value; }
            get { return gf5vimsAtrValues[33]; }
        }
        public string WidthInMeters
        {
            set { gf5vimsAtrValues[34] = value; }
            get { return gf5vimsAtrValues[34]; }
        }
        public string HeightInMeters
        {
            set { gf5vimsAtrValues[35] = value; }
            get { return gf5vimsAtrValues[35]; }
        }
        public string CloudPercent
        {
            set { gf5vimsAtrValues[36] = value; }
            get { return gf5vimsAtrValues[36]; }
        }
        public string GainMode
        {
            set { gf5vimsAtrValues[37] = value; }
            get { return gf5vimsAtrValues[37]; }
        }
        public string CalibrationParam
        {
            set { gf5vimsAtrValues[38] = value; }
            get { return gf5vimsAtrValues[38]; }
        }
        public string CalibrationParamVersion
        {
            set { gf5vimsAtrValues[39] = value; }
            get { return gf5vimsAtrValues[39]; }
        }
        public string GAINS
        {
            set { gf5vimsAtrValues[40] = value; }
            get { return gf5vimsAtrValues[40]; }
        }
        public string OFFSETS
        {
            set { gf5vimsAtrValues[41] = value; }
            get { return gf5vimsAtrValues[41]; }
        }
        public string IntegrationTime
        {
            set { gf5vimsAtrValues[42] = value; }
            get { return gf5vimsAtrValues[42]; }
        }
        public string IntegrationLevel
        {
            set { gf5vimsAtrValues[43] = value; }
            get { return gf5vimsAtrValues[43]; }
        }
        public string MapProjection
        {
            set { gf5vimsAtrValues[44] = value; }
            get { return gf5vimsAtrValues[44]; }
        }
        public string EarthEllipsoid
        {
            set { gf5vimsAtrValues[45] = value; }
            get { return gf5vimsAtrValues[45]; }
        }
        public string ZoneNo
        {
            set { gf5vimsAtrValues[46] = value; }
            get { return gf5vimsAtrValues[46]; }
        }
        public string RadiometricMethod
        {
            set { gf5vimsAtrValues[47] = value; }
            get { return gf5vimsAtrValues[47]; }
        }
        public string CalParameterVersion
        {
            set { gf5vimsAtrValues[48] = value; }
            get { return gf5vimsAtrValues[48]; }
        }
        public string ResamplingKernel
        {
            set { gf5vimsAtrValues[49] = value; }
            get { return gf5vimsAtrValues[49]; }
        }
        public string Denoise
        {
            set { gf5vimsAtrValues[50] = value; }
            get { return gf5vimsAtrValues[50]; }
        }
        public string HeightMode
        {
            set { gf5vimsAtrValues[51] = value; }
            get { return gf5vimsAtrValues[51]; }
        }
        public string EphemerisData
        {
            set { gf5vimsAtrValues[52] = value; }
            get { return gf5vimsAtrValues[52]; }
        }
        public string AttitudeData
        {
            set { gf5vimsAtrValues[53] = value; }
            get { return gf5vimsAtrValues[53]; }
        }
        public string CenterLatitude
        {
            set { gf5vimsAtrValues[54] = value; }
            get { return gf5vimsAtrValues[54]; }

        }
        public string CenterLongitude
        {
            set { gf5vimsAtrValues[55] = value; }
            get { return gf5vimsAtrValues[55]; }
        }
        public string TopLeftLatitude
        {
            set { gf5vimsAtrValues[56] = value; }
            get { return gf5vimsAtrValues[56]; }
        }
        public string TopLeftLongitude
        {
            set { gf5vimsAtrValues[57] = value; }
            get { return gf5vimsAtrValues[57]; }
        }
        public string TopRightLatitude
        {
            set { gf5vimsAtrValues[58] = value; }
            get { return gf5vimsAtrValues[58]; }
        }
        public string TopRightLongitude
        {
            set { gf5vimsAtrValues[59] = value; }
            get { return gf5vimsAtrValues[59]; }
        }
        public string BottomRightLatitude
        {
            set { gf5vimsAtrValues[60] = value; }
            get { return gf5vimsAtrValues[60]; }
        }
        public string BottomRightLongitude
        {
            set { gf5vimsAtrValues[61] = value; }
            get { return gf5vimsAtrValues[61]; }
        }
        public string BottomLeftLatitude
        {
            set { gf5vimsAtrValues[62] = value; }
            get { return gf5vimsAtrValues[62]; }
        }
        public string BottomLeftLongitude
        {
            set { gf5vimsAtrValues[63] = value; }
            get { return gf5vimsAtrValues[63]; }
        }
        public string TopLeftMapX
        {
            set { gf5vimsAtrValues[64] = value; }
            get { return gf5vimsAtrValues[64]; }
        }
        public string TopLeftMapY
        {
            set { gf5vimsAtrValues[65] = value; }
            get { return gf5vimsAtrValues[65]; }
        }
        public string TopRightMapX
        {
            set { gf5vimsAtrValues[66] = value; }
            get { return gf5vimsAtrValues[66]; }
        }
        public string TopRightMapY
        {
            set { gf5vimsAtrValues[67] = value; }
            get { return gf5vimsAtrValues[67]; }
        }
        public string BottomRightMapX
        {
            set { gf5vimsAtrValues[68] = value; }
            get { return gf5vimsAtrValues[68]; }
        }
        public string BottomRightMapY
        {
            set { gf5vimsAtrValues[69] = value; }
            get { return gf5vimsAtrValues[69]; }
        }
        public string BottomLeftMapX
        {
            set { gf5vimsAtrValues[70] = value; }
            get { return gf5vimsAtrValues[70]; }
        }
        public string BottomLeftMapY
        {
            set { gf5vimsAtrValues[71] = value; }
            get { return gf5vimsAtrValues[71]; }
        }
        public string DDSFlag
        {
            set { gf5vimsAtrValues[72] = value; }
            get { return gf5vimsAtrValues[72]; }
        }
        public string SoftwareVersion
        {
            set { gf5vimsAtrValues[73] = value; }
            get { return gf5vimsAtrValues[73]; }
        }

        public string QRST_CODE
        {
            set;
            get;
        }
        public string CorDataFlag = "";
        #endregion
        /// <summary>
        /// 读取XML属性
        /// </summary>
        /// <param name="fileName"></param>
        public override void ReadAttributes(string fileName)
        {
            XmlDocument root = new XmlDocument();
            try
            {
                root.Load(fileName);
            }
            catch
            {
                throw new Exception("xml文件已损坏！");
            }
            XmlNode node = null;
            try
            {
                for (int i = 0; i < gf5vimsAttrName.Length; i++)
                {
                    node = root.GetElementsByTagName(gf5vimsAttrName[i]).Item(0);
                    if (node == null)
                    {
                        gf5vimsAtrValues[i] = "";
                    }
                    else
                    {
                        gf5vimsAtrValues[i] = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元信息出错：" + ex.ToString());
            }
        }

        public override string GetRelateDataPath()
        {
            string[] strArr = Name.Split("_".ToArray());
            if (strArr.Length == 7)
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
            string[] strArr = Name.Split("_".ToArray());
            if (strArr.Length == 5)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);
                return string.Format("实验验证数据库\\GF5卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)), Name);
            }
            else
            {
                return "";
            }
        }

        public static bool HasCorrectedData(string qrst_code, IDbBaseUtilities evdb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CorDataFlag from prod_vims_gf5");
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
            string[] strArr = Name.Split("_".ToArray());
            if (strArr.Length == 5)
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
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select NAME, SatelliteID, SensorID, CorDataFlag from prod_vims_gf5 ");
            strsql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

            using (DataSet ds = sqlBase.GetDataSet(strsql.ToString()))
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


        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("prod_vims_gf5", EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_vims_gf5 where Name ='{0}'", Name);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_vims_gf5 where QRST_CODE ='{0}'", QRST_CODE);
                    //DataSet ds = sqlBase.GetDataSet(presql);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_vims_gf5");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_vims_gf5", ID);
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_vims_gf5(");
                strSql.Append(
                    "ID,Name,SatelliteID,ReceiveStationID,SensorID,ProduceTime,ProductLevel,ProductFormat,SceneID,ProductID,ProduceType,ScenePath,SceneRow,SatPath," +
                    "SatRow,OrbitID,POrbitID,OrbitType,AttType,RollSatelliteAngle,PitchSatelliteAngle,YawSatelliteAngle,SolarAzimuth," +
                    "SolarZenith,SatelliteAzimuth,SatelliteZenith,StartTime,EndTime,BandsCount,Bands,SpectralRangeStart,SpectralRangeEnd," +
                    "CentralWavelength,ImageGSD,WidthInPixels,HeightInPixels,WidthInMeters,HeightInMeters,CloudPercent,GainMode,CalibrationParam," +
                    "CalibrationParamVersion,GAINS,OFFSETS,IntegrationTime,IntegrationLevel,MapProjection,EarthEllipsoid,ZoneNo," +
                    "RadiometricMethod,CalParameterVersion,ResamplingKernel,Denoise,HeightMode,EphemerisData,AttitudeData," +
                    "CenterLatitude,CenterLongitude,TopLeftLatitude,TopLeftLongitude,TopRightLatitude,TopRightLongitude," +
                    "BottomRightLatitude,BottomRightLongitude,BottomLeftLatitude,BottomLeftLongitude,TopLeftMapX,TopLeftMapY," +
                    "TopRightMapX,TopRightMapY,BottomRightMapX,BottomRightMapY,BottomLeftMapX,BottomLeftMapY,DDSFlag," +
                    "SoftwareVersion,QRST_CODE,size)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}','{61}','{62}','{63}'," +
                        "'{64}','{65}','{66}','{67}','{68}','{69}','{70}','{71}','{72}','{73}','{74}','{75}','{76}',{77})", ID, Name, SatelliteID, ReceiveStationID, SensorID, ProduceTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ProductLevel, ProductFormat, SceneID, ProductID, ProduceType, ScenePath, SceneRow, SatPath, SatRow, OrbitID, POrbitID, OrbitType, AttType,
                        RollSatelliteAngle, PitchSatelliteAngle, YawSatelliteAngle, SolarAzimuth, SolarZenith, SatelliteAzimuth, SatelliteZenith,
                        StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), BandsCount, Bands, SpectralRangeStart, SpectralRangeEnd,
                        CentralWavelength, ImageGSD, WidthInPixels, HeightInPixels, WidthInMeters, HeightInMeters, CloudPercent, GainMode, CalibrationParam,
                        CalibrationParamVersion, GAINS, OFFSETS, IntegrationTime, IntegrationLevel, MapProjection, EarthEllipsoid, ZoneNo,
                        RadiometricMethod, CalParameterVersion, ResamplingKernel, Denoise, HeightMode, EphemerisData, AttitudeData, CenterLatitude, CenterLongitude,
                        TopLeftLatitude, TopLeftLongitude, TopRightLatitude, TopRightLongitude, BottomRightLatitude, BottomRightLongitude,
                        BottomLeftLatitude, BottomLeftLongitude, TopLeftMapX, TopLeftMapY, TopRightMapX, TopRightMapY, BottomRightMapX, BottomRightMapY,
                        BottomLeftMapX, BottomLeftMapY, DDSFlag, SoftwareVersion, QRST_CODE, size));


                sqlBase.ExecuteSql(strSql.ToString());


                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_vims_gf5 set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_vims_gf5", EnumDBType.MIDB);
            }
            catch (Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }
        }
    }
}
