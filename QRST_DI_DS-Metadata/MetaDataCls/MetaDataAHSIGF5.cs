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
    public class MetaDataAHSIGF5 : MetaData
    {
        public string[] gf5AttributeNames = {
                                     "SatelliteID",
                            "ReceiveStationID" ,
                            "SensorID" ,
                            "ProduceTime" ,
                            "ProductLevel" ,
                            "ProductFormat" ,
                            "SceneID" ,
                            "ProductID" ,
                            "ProduceType" ,
                            "BandsCount" ,
                            "BandsID" ,
                            "ScenePath" ,
                            "SceneRow" ,
                            "SatPath" ,
                            "SatRow" ,
                            "OrbitID" ,
                            "POrbitID" ,
                            "OrbitType" ,
                            "AttType" ,
                            "RollSatelliteAngle" ,
                            "PitchSatelliteAngle" ,
                            "YawSatelliteAngle" ,
                            "SolarAzimuth" ,
                            "SolarZenith" ,
                            "SatelliteAzimuth" ,
                            "SatelliteZenith" ,
                            "StartTime" ,
                            "EndTime" ,
                            "ImageGSD" ,
                            "LinesInPixels" ,
                            "SamplesInPixels" ,
                            "WidthInMeters" ,
                            "HeightInMeters" ,
                            "CloudPercent" ,
                            "GainMode" ,
                            "IntegrationTime" ,
                            "MapProjection" ,
                            "EarthEllipsoid" ,
                            "ZoneNo" ,
                            "RadiometricMethod" ,
                            "CalibrationParam" ,
                            "CalibrationParamVersion" ,
                            "ScaleFactor" ,
                            "ResamplingKernel" ,
                            "Denoise" ,
                            "HeightMode" ,
                            "EphemerisData" ,
                            "AttitudeData" ,
                            "CenterLatitude" ,
                            "CenterLongitude" ,
                            "TopLeftLatitude" ,
                            "TopLeftLongitude" ,
                            "TopRightLatitude" ,
                            "TopRightLongitude" ,
                            "BottomRightLatitude" ,
                            "BottomRightLongitude" ,
                            "BottomLeftLatitude" ,
                            "BottomLeftLongitude" ,
                            "TopLeftMapX" ,
                            "TopLeftMapY" ,
                            "TopRightMapX" ,
                            "TopRightMapY" ,
                            "BottomRightMapX" ,
                            "BottomRightMapY" ,
                            "BottomLeftMapX" ,
                            "BottomLeftMapY",
                            "DDSFlag",
                            "SoftwareVersion"
                                  };
        public string[] gf5AttributeValues;

        public MetaDataAHSIGF5(string _name, string _qrst_code)
        {
            gf5AttributeValues = new string[gf5AttributeNames.Length];
            Name = _name;
            QRST_CODE = _qrst_code;
        }

        public MetaDataAHSIGF5()
        {
            gf5AttributeValues = new string[gf5AttributeNames.Length];
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
            set { gf5AttributeValues[0] = value; }
            get { return gf5AttributeValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiveStationID
        {
            set { gf5AttributeValues[1] = value; }
            get { return gf5AttributeValues[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SensorID
        {
            set { gf5AttributeValues[2] = value; }
            get { return gf5AttributeValues[2]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProduceTime
        {
            set { gf5AttributeValues[3] = value; }
            get { return gf5AttributeValues[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductLevel
        {
            set { gf5AttributeValues[4] = value; }
            get { return gf5AttributeValues[4]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductFormat
        {
            set { gf5AttributeValues[5] = value; }
            get { return gf5AttributeValues[5]; }

        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneID
        {
            set { gf5AttributeValues[6] = value; }
            get { return gf5AttributeValues[6]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductID
        {
            set { gf5AttributeValues[7] = value; }
            get { return gf5AttributeValues[7]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProduceType
        {
            set { gf5AttributeValues[8] = value; }
            get { return gf5AttributeValues[8]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BandsCount
        {
            set { gf5AttributeValues[9] = value; }
            get { return gf5AttributeValues[9]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BandsID
        {
            set { gf5AttributeValues[10] = value; }
            get { return gf5AttributeValues[11]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ScenePath
        {
            set { gf5AttributeValues[11] = value; }
            get { return gf5AttributeValues[11]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneRow
        {
            set { gf5AttributeValues[12] = value; }
            get { return gf5AttributeValues[12]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatPath
        {
            set { gf5AttributeValues[13] = value; }
            get { return gf5AttributeValues[13]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatRow
        {
            set { gf5AttributeValues[14] = value; }
            get { return gf5AttributeValues[14]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrbitID
        {
            set { gf5AttributeValues[15] = value; }
            get { return gf5AttributeValues[15]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string POrbitID
        {
            set { gf5AttributeValues[16] = value; }
            get { return gf5AttributeValues[16]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrbitType
        {
            set { gf5AttributeValues[17] = value; }
            get { return gf5AttributeValues[17]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AttType
        {
            set { gf5AttributeValues[18] = value; }
            get { return gf5AttributeValues[18]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RollSatelliteAngle
        {
            set { gf5AttributeValues[19] = value; }
            get { return gf5AttributeValues[19]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PitchSatelliteAngle
        {
            set { gf5AttributeValues[20] = value; }
            get { return gf5AttributeValues[20]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YawSatelliteAngle
        {
            set { gf5AttributeValues[21] = value; }
            get { return gf5AttributeValues[21]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SolarAzimuth
        {
            set { gf5AttributeValues[22] = value; }
            get { return gf5AttributeValues[22]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SolarZenith
        {
            set { gf5AttributeValues[23] = value; }
            get { return gf5AttributeValues[23]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteAzimuth
        {
            set { gf5AttributeValues[24] = value; }
            get { return gf5AttributeValues[24]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteZenith
        {
            set { gf5AttributeValues[25] = value; }
            get { return gf5AttributeValues[25]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StartTime
        {
            set { gf5AttributeValues[26] = value; }
            get { return gf5AttributeValues[26]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EndTime
        {
            set { gf5AttributeValues[27] = value.ToString(); }
            get { return gf5AttributeValues[27]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageGSD
        {
            set { gf5AttributeValues[28] = value; }
            get { return gf5AttributeValues[28]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LinesInPixels
        {
            set { gf5AttributeValues[29] = value; }
            get { return gf5AttributeValues[29]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SamplesInPixels
        {
            set { gf5AttributeValues[30] = value; }
            get { return gf5AttributeValues[30]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WidthInMeters
        {
            set { gf5AttributeValues[31] = value; }
            get { return gf5AttributeValues[31]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightInMeters
        {
            set { gf5AttributeValues[32] = value; }
            get { return gf5AttributeValues[32]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CloudPercent
        {
            set { gf5AttributeValues[33] = value; }
            get { return gf5AttributeValues[33]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GainMode
        {
            set { gf5AttributeValues[34] = value; }
            get { return gf5AttributeValues[34]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntegrationTime
        {
            set { gf5AttributeValues[35] = value; }
            get { return gf5AttributeValues[35]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MapProjection
        {
            set { gf5AttributeValues[36] = value; }
            get { return gf5AttributeValues[36]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EarthEllipsoid
        {
            set { gf5AttributeValues[37] = value; }
            get { return gf5AttributeValues[37]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ZoneNo
        {
            set { gf5AttributeValues[38] = value; }
            get { return gf5AttributeValues[38]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RadiometricMethod
        {
            set { gf5AttributeValues[39] = value; }
            get { return gf5AttributeValues[39]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CalibrationParam
        {
            set { gf5AttributeValues[40] = value; }
            get { return gf5AttributeValues[40]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CalibrationParamVersion
        {
            set { gf5AttributeValues[41] = value; }
            get { return gf5AttributeValues[41]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ScaleFactor
        {
            set { gf5AttributeValues[42] = value; }
            get { return gf5AttributeValues[42]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResamplingKernel
        {
            set { gf5AttributeValues[43] = value; }
            get { return gf5AttributeValues[43]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Denoise
        {
            set { gf5AttributeValues[44] = value; }
            get { return gf5AttributeValues[44]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeightMode
        {
            set { gf5AttributeValues[45] = value; }
            get { return gf5AttributeValues[45]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EphemerisData
        {
            set { gf5AttributeValues[46] = value; }
            get { return gf5AttributeValues[46]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AttitudeData
        {
            set { gf5AttributeValues[47] = value; }
            get { return gf5AttributeValues[47]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CenterLatitude
        {
            set { gf5AttributeValues[48] = value; }
            get { return gf5AttributeValues[48]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CenterLongitude
        {
            set { gf5AttributeValues[49] = value; }
            get { return gf5AttributeValues[49]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopLeftLatitude
        {
            set { gf5AttributeValues[50] = value; }
            get { return gf5AttributeValues[50]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopLeftLongitude
        {
            set { gf5AttributeValues[51] = value; }
            get { return gf5AttributeValues[51]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopRightLatitude
        {
            set { gf5AttributeValues[52] = value; }
            get { return gf5AttributeValues[52]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopRightLongitude
        {
            set { gf5AttributeValues[53] = value; }
            get { return gf5AttributeValues[53]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomRightLatitude
        {
            set { gf5AttributeValues[54] = value; }
            get { return gf5AttributeValues[54]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomRightLongitude
        {
            set { gf5AttributeValues[55] = value; }
            get { return gf5AttributeValues[55]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomLeftLatitude
        {
            set { gf5AttributeValues[56] = value; }
            get { return gf5AttributeValues[56]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomLeftLongitude
        {
            set { gf5AttributeValues[57] = value; }
            get { return gf5AttributeValues[57]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TopLeftMapX
        {
            set { gf5AttributeValues[58] = value; }
            get { return gf5AttributeValues[58]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopLeftMapY
        {
            set { gf5AttributeValues[59] = value; }
            get { return gf5AttributeValues[59]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopRightMapX
        {
            set { gf5AttributeValues[60] = value; }
            get { return gf5AttributeValues[60]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TopRightMapY
        {
            set { gf5AttributeValues[61] = value; }
            get { return gf5AttributeValues[61]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomRightMapX
        {
            set { gf5AttributeValues[62] = value; }
            get { return gf5AttributeValues[62]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomRightMapY
        {
            set { gf5AttributeValues[63] = value; }
            get { return gf5AttributeValues[63]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomLeftMapX
        {
            set { gf5AttributeValues[64] = value; }
            get { return gf5AttributeValues[64]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BottomLeftMapY
        {
            set { gf5AttributeValues[65] = value; }
            get { return gf5AttributeValues[65]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DDSFlag
        {
            set { gf5AttributeValues[66] = value; }
            get { return gf5AttributeValues[66]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SoftwareVersion
        {
            set { gf5AttributeValues[67] = value; }
            get { return gf5AttributeValues[67]; }
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

        public double size
        {
            get;
            set;
        }

        #endregion Model

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                Constant.IdbOperating.LockTable("prod_ahsi_gf5", EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_ahsi_gf5 where Name ='{0}'", Name);
                Console.WriteLine(presql);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_ahsi_gf5 where QRST_CODE ='{0}'", QRST_CODE);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_ahsi_gf5");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_ahsi_gf5", ID);
                }


                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_ahsi_gf5(");
                strSql.Append("ID,Name,SatelliteID,ReceiveStationID,SensorID,ProduceTime,ProductLevel,ProductFormat," +
                              "SceneID,ProductID,ProduceType,BandsCount,BandsID," +
                              "ScenePath,SceneRow,SatPath,SatRow,OrbitID,POrbitID,OrbitType,AttType," +
                              "RollSatelliteAngle,PitchSatelliteAngle,YawSatelliteAngle,SolarAzimuth,SolarZenith,SatelliteAzimuth,SatelliteZenith,StartTime,EndTime,ImageGSD," +
                              "LinesInPixels,SamplesInPixels,WidthInMeters,HeightInMeters,CloudPercent,GainMode,IntegrationTime," +
                              "MapProjection,EarthEllipsoid,ZoneNo,RadiometricMethod,CalibrationParam,CalibrationParamVersion," +
                              "ScaleFactor,ResamplingKernel,Denoise,HeightMode,EphemerisData,AttitudeData," +
                              "CenterLatitude,CenterLongitude,TopLeftLatitude,TopLeftLongitude,TopRightLatitude,TopRightLongitude,BottomRightLatitude," +
                              "BottomRightLongitude,BottomLeftLatitude,BottomLeftLongitude,TopLeftMapX,TopLeftMapY," +
                              "TopRightMapX,TopRightMapY,BottomRightMapX,BottomRightMapY,BottomLeftMapX,BottomLeftMapY,DDSFlag,SoftwareVersion,QRST_CODE,size,geom)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},'{10}',{11},'{12}',{13},{14},{15},{16},{17},{18}," +
                        "'{19}','{20}',{21}," + "{22},{23},{24},{25},{26},{27},'{28}','{29}',{30},'{31}','{32}'," +
                        "{33},{34},{35},'{36}','{37}','{38}','{39}',{40},{41},'{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}',{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},'{60}','{61}'," +
                        "'{62}','{63}','{64}','{65}','{66}','{67}','{68}','{69}','{70}',{71})", ID, Name, SatelliteID, ReceiveStationID,
                        SensorID, ProduceTime, ProductLevel, ProductFormat, SceneID, ProductID, ProduceType, BandsCount, BandsID,
                        ScenePath, SceneRow, SatPath, SatRow, OrbitID, POrbitID, OrbitType, AttType,
                        RollSatelliteAngle, PitchSatelliteAngle, YawSatelliteAngle, SolarAzimuth, SolarZenith, SatelliteAzimuth, SatelliteZenith, StartTime, EndTime, ImageGSD,
                        LinesInPixels, SamplesInPixels, WidthInMeters, HeightInMeters, CloudPercent, GainMode, IntegrationTime,
                        MapProjection, EarthEllipsoid, ZoneNo, RadiometricMethod, CalibrationParam, CalibrationParamVersion,
                        ScaleFactor, ResamplingKernel, Denoise, HeightMode, EphemerisData, AttitudeData,
                        CenterLatitude, CenterLongitude, TopLeftLatitude, TopLeftLongitude, TopRightLatitude, TopRightLongitude, BottomRightLatitude,
                        BottomRightLongitude, BottomLeftLatitude, BottomLeftLongitude, TopLeftMapX, TopLeftMapY,
                        TopRightMapX, TopRightMapY, BottomRightMapX, BottomRightMapY, BottomLeftMapX, BottomLeftMapY, DDSFlag, SoftwareVersion, QRST_CODE, size, string.Format("ST_SetSRID(ST_MakePolygon(ST_MakeLine(ARRAY[ST_Point({0}, {1}), ST_Point({2}, {3}), ST_Point({4}, {5}), ST_Point({6}, {7}), ST_Point({8}, {9})])), 4326)", TopLeftLongitude, TopLeftLatitude, TopRightLongitude, TopRightLatitude, BottomRightLongitude, BottomRightLatitude,
                        BottomLeftLongitude, BottomLeftLatitude, TopLeftLongitude, TopLeftLatitude)));
                Console.WriteLine(strSql.ToString());

                sqlBase.ExecuteSql(strSql.ToString());


                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_ahsi_gf5 set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_ahsi_gf5", EnumDBType.MIDB);

            }
            catch (Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
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
                for (int i = 0; i < gf5AttributeNames.Length; i++)
                {
                    node = root.GetElementsByTagName(gf5AttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        gf5AttributeValues[i] = "";
                    }
                    else
                    {
                        gf5AttributeValues[i] = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }

        //gf5_WFV1_E114.1_N22.9_20130712_L1A0000052397.tar.gz
        public override string GetRelateDataPath()
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 7)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);
                return string.Format("实验验证数据库\\光学卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Name));
            }
            else
            {
                return base.GetRelateDataPath();
            }

        }

        public static bool HasCorrectedData(string qrst_code, IDbBaseUtilities evdb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CorDataFlag from prod_ahsi_gf5 ");
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


        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NAME,SatelliteID,SensorID,CorDataFlag,qrst_code from prod_ahsi_gf5 ");
            strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

            using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                    SatelliteID = ds.Tables[0].Rows[0]["SatelliteID"].ToString();
                    SensorID = ds.Tables[0].Rows[0]["SensorID"].ToString();
                    CorDataFlag = ds.Tables[0].Rows[0]["CorDataFlag"].ToString();
                    qrst_code = ds.Tables[0].Rows[0]["qrst_code"].ToString();
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
