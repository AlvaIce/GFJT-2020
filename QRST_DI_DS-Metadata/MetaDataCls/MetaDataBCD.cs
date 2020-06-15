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
    public class MetaDataBCD : MetaData
    {
        public string[] bcdAttributeNames = {
                                    "SatelliteID"
    , "ReceiveStationID"
    , "SensorID"
    , "ReceiveTime"
    , "OrbitID"
    , "OrbitType"
    , "AttType"
    , "StripID"
    , "ProduceType"
    , "SceneID"
    , "DDSFlag"
    , "ProductID"
    , "ProductLevel"
    , "ProductFormat"
    , "ProduceTime"
    , "Bands"
    , "ScenePath"
    , "SceneRow"
    , "SatPath"
    , "SatRow"
    , "SceneCount"
    , "SceneShift"
    , "StartTime"
    , "EndTime"
    , "CenterTime"
    , "StartLine"
    , "EndLine"
    , "ImageGSD"
    , "WidthInPixels"
    , "HeightInPixels"
    , "WidthInMeters"
    , "HeightInMeters"
    , "RegionName"
    , "CloudPercent"
    , "DataSize"
    , "RollViewingAngle"
    , "PitchViewingAngle"
    , "PitchSatelliteAngle"
    , "RollSatelliteAngle"
    , "YawSatelliteAngle"
    , "SolarAzimuth"
    , "SolarZenith"
    , "SatelliteAzimuth"
    , "SatelliteZenith"
    , "GainMode"
    , "IntegrationTime"
    , "IntegrationLevel"
    , "MapProjection"
    , "EarthEllipsoid"
    , "ZoneNo"
    , "ResamplingKernel"
    , "HeightMode"
    , "EphemerisData"
    , "AttitudeData"
    , "RadiometricMethod"
    , "MtfCorrection"
    , "Denoise"
    , "RayleighCorrection"
    , "UsedGCPNo"
    , "CenterLatitude"
    , "CenterLongitude"
    , "TopLeftLatitude"
    , "TopLeftLongitude"
    , "TopRightLatitude"
    , "TopRightLongitude"
    , "BottomRightLatitude"
    , "BottomRightLongitude"
    , "BottomLeftLatitude"
    , "BottomLeftLongitude"
    , "TopLeftMapX"
    , "TopLeftMapY"
    , "TopRightMapX"
    , "TopRightMapY"
    , "BottomRightMapX"
    , "BottomRightMapY"
    , "BottomLeftMapX"
    , "BottomLeftMapY"
    , "DataArchiveFile"
    , "BrowseFileLocation"
    , "ThumbFileLocation"
    };
        public string[] bcdAttributeValues;

        public MetaDataBCD(string _name, string _qrst_code)
        {
            bcdAttributeValues = new string[bcdAttributeNames.Length];
            Name = _name;
            QRST_CODE = _qrst_code;
        }

        public MetaDataBCD()
        {
            bcdAttributeValues = new string[bcdAttributeNames.Length];
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
            set { bcdAttributeValues[0] = value; }
            get { return bcdAttributeValues[0]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ReceiveStationID
        {
            set { bcdAttributeValues[1] = value.ToString(); }
            get { return bcdAttributeValues[1]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SensorID
        {
            set { bcdAttributeValues[2] = value; }
            get { return bcdAttributeValues[2]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ReceiveTime
        {
            set { bcdAttributeValues[3] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(bcdAttributeValues[3], out dt))
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
            set { bcdAttributeValues[4] = value; }
            get { return bcdAttributeValues[4]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OrbitType
        {
            set { bcdAttributeValues[5] = value; }
            get { return bcdAttributeValues[5]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AttType
        {
            set { bcdAttributeValues[6] = value; }
            get { return bcdAttributeValues[6]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StripID
        {
            set { bcdAttributeValues[7] = value; }
            get { return bcdAttributeValues[7]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProduceType
        {
            set { bcdAttributeValues[8] = value; }
            get { return bcdAttributeValues[8]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SceneID
        {
            set { bcdAttributeValues[9] = value; }
            get { return bcdAttributeValues[9]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DDSFlag
        {
            set { bcdAttributeValues[10] = value; }
            get { return bcdAttributeValues[10]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProductID
        {
            set { bcdAttributeValues[11] = value; }
            get { return bcdAttributeValues[11]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProductLevel
        {
            set { bcdAttributeValues[12] = value; }
            get { return bcdAttributeValues[12]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProductFormat
        {
            set { bcdAttributeValues[13] = value; }
            get { return bcdAttributeValues[13]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ProduceTime
        {
            set { bcdAttributeValues[14] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(bcdAttributeValues[14], out dt))
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
            set { bcdAttributeValues[15] = value; }
            get { return bcdAttributeValues[15]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ScenePath
        {
            set { bcdAttributeValues[16] = value; }
            get { return bcdAttributeValues[16]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SceneRow
        {
            set { bcdAttributeValues[17] = value; }
            get { return bcdAttributeValues[17]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SatPath
        {
            set { bcdAttributeValues[18] = value; }
            get { return bcdAttributeValues[18]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SatRow
        {
            set { bcdAttributeValues[19] = value; }
            get { return bcdAttributeValues[19]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SceneCount
        {
            set { bcdAttributeValues[20] = value; }
            get { return bcdAttributeValues[20]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SceneShift
        {
            set { bcdAttributeValues[21] = value; }
            get { return bcdAttributeValues[21]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            set { bcdAttributeValues[22] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(bcdAttributeValues[22], out dt))
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
            set { bcdAttributeValues[23] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(bcdAttributeValues[23], out dt))
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
            set { bcdAttributeValues[24] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(bcdAttributeValues[24], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StartLine
        {
            set { bcdAttributeValues[25] = value; }
            get { return bcdAttributeValues[25]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EndLine
        {
            set { bcdAttributeValues[26] = value; }
            get { return bcdAttributeValues[26]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ImageGSD
        {
            set { bcdAttributeValues[27] = value; }
            get { return bcdAttributeValues[27]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string WidthInPixels
        {
            set { bcdAttributeValues[28] = value; }
            get { return bcdAttributeValues[28]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HeightInPixels
        {
            set { bcdAttributeValues[29] = value; }
            get { return bcdAttributeValues[29]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string WidthInMeters
        {
            set { bcdAttributeValues[30] = value; }
            get { return bcdAttributeValues[30]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HeightInMeters
        {
            set { bcdAttributeValues[31] = value; }
            get { return bcdAttributeValues[31]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RegionName
        {
            set { bcdAttributeValues[32] = value; }
            get { return bcdAttributeValues[32]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CloudPercent
        {
            set { bcdAttributeValues[33] = value; }
            get { return bcdAttributeValues[33]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double DataSize
        {
            set { bcdAttributeValues[34] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[34], out dt))
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
        public double RollViewingAngle
        {
            set { bcdAttributeValues[35] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[35], out dt))
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
        public double PitchViewingAngle
        {
            set { bcdAttributeValues[36] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[36], out dt))
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
        public double PitchSatelliteAngle
        {
            set { bcdAttributeValues[37] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[37], out dt))
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
        public double RollSatelliteAngle
        {
            set { bcdAttributeValues[38] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[38], out dt))
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
        public double YawSatelliteAngle
        {
            set { bcdAttributeValues[39] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[39], out dt))
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
        public double SolarAzimuth
        {
            set { bcdAttributeValues[40] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[40], out dt))
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
        public double SolarZenith
        {
            set { bcdAttributeValues[41] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[41], out dt))
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
        public double SatelliteAzimuth
        {
            set { bcdAttributeValues[42] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[42], out dt))
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
        public double SatelliteZenith
        {
            set { bcdAttributeValues[43] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[43], out dt))
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
        public string GainMode
        {
            set { bcdAttributeValues[44] = value; }
            get { return bcdAttributeValues[44]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime IntegrationTime
        {
            set { bcdAttributeValues[45] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(bcdAttributeValues[45], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IntegrationLevel
        {
            set { bcdAttributeValues[46] = value; }
            get { return bcdAttributeValues[46]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MapProjection
        {
            set { bcdAttributeValues[47] = value; }
            get { return bcdAttributeValues[47]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EarthEllipsoid
        {
            set { bcdAttributeValues[48] = value; }
            get { return bcdAttributeValues[48]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ZoneNo
        {
            set { bcdAttributeValues[49] = value; }
            get { return bcdAttributeValues[49]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ResamplingKernel
        {
            set { bcdAttributeValues[50] = value; }
            get { return bcdAttributeValues[50]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HeightMode
        {
            set { bcdAttributeValues[51] = value; }
            get { return bcdAttributeValues[51]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EphemerisData
        {
            set { bcdAttributeValues[52] = value; }
            get { return bcdAttributeValues[52]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AttitudeData
        {
            set { bcdAttributeValues[53] = value; }
            get { return bcdAttributeValues[53]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RadiometricMethod
        {
            set { bcdAttributeValues[54] = value; }
            get { return bcdAttributeValues[54]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MtfCorrection
        {
            set { bcdAttributeValues[55] = value; }
            get { return bcdAttributeValues[55]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Denoise
        {
            set { bcdAttributeValues[56] = value; }
            get { return bcdAttributeValues[56]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RayleighCorrection
        {
            set { bcdAttributeValues[57] = value; }
            get { return bcdAttributeValues[57]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UsedGCPNo
        {
            set { bcdAttributeValues[58] = value; }
            get { return bcdAttributeValues[58]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double CenterLatitude
        {
            set { bcdAttributeValues[59] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[59], out dt))
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
        public double CenterLongitude
        {
            set { bcdAttributeValues[60] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[60], out dt))
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
        public double TopLeftLatitude
        {
            set { bcdAttributeValues[61] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[61], out dt))
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
        public double TopLeftLongitude
        {
            set { bcdAttributeValues[62] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[62], out dt))
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
        public double TopRightLatitude
        {
            set { bcdAttributeValues[63] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[63], out dt))
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
        public double TopRightLongitude
        {
            set { bcdAttributeValues[64] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[64], out dt))
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
        public double BottomRightLatitude
        {
            set { bcdAttributeValues[65] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[65], out dt))
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
        public double BottomRightLongitude
        {
            set { bcdAttributeValues[66] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[66], out dt))
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
        public double BottomLeftLatitude
        {
            set { bcdAttributeValues[67] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[67], out dt))
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
        public double BottomLeftLongitude
        {
            set { bcdAttributeValues[68] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[68], out dt))
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
        public double TopLeftMapX
        {
            set { bcdAttributeValues[69] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[69], out dt))
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
        public double TopLeftMapY
        {
            set { bcdAttributeValues[70] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[70], out dt))
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
        public double TopRightMapX
        {
            set { bcdAttributeValues[71] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[71], out dt))
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
        public double TopRightMapY
        {
            set { bcdAttributeValues[72] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[72], out dt))
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
        public double BottomRightMapX
        {
            set { bcdAttributeValues[73] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[73], out dt))
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
        public double BottomRightMapY
        {
            set { bcdAttributeValues[74] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[74], out dt))
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
        public double BottomLeftMapX
        {
            set { bcdAttributeValues[75] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[75], out dt))
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
        public double BottomLeftMapY
        {
            set { bcdAttributeValues[76] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(bcdAttributeValues[76], out dt))
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
        public string DataArchiveFile
        {
            set { bcdAttributeValues[77] = value; }
            get { return bcdAttributeValues[77]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BrowseFileLocation
        {
            set { bcdAttributeValues[78] = value; }
            get { return bcdAttributeValues[78]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ThumbFileLocation
        {
            set { bcdAttributeValues[79] = value; }
            get { return bcdAttributeValues[79]; }
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
                Constant.IdbOperating.LockTable("prod_gf1bcd", EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_gf1bcd where Name ='{0}'", Name);
                // Console.WriteLine(presql);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_gf1bcd where QRST_CODE ='{0}'", QRST_CODE);

                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_gf1bcd");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_gf1bcd", ID);
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_gf1bcd(");
                strSql.Append("ID,Name,SatelliteID"
    + ",ReceiveStationID"
    + ",SensorID"
    + ",ReceiveTime"
    + ",OrbitID"
    + ",OrbitType"
    + ",AttType"
    + ",StripID"
    + ",ProduceType"
    + ",SceneID"
    + ",DDSFlag"
    + ",ProductID"
    + ",ProductLevel"
    + ",ProductFormat"
    + ",ProduceTime"
    + ",Bands"
    + ",ScenePath"
    + ",SceneRow"
    + ",SatPath"
    + ",SatRow"
    + ",SceneCount"
    + ",SceneShift"
    + ",StartTime"
    + ",EndTime"
    + ",CenterTime"
    + ",StartLine"
    + ",EndLine"
    + ",ImageGSD"
    + ",WidthInPixels"
    + ",HeightInPixels"
    + ",WidthInMeters"
    + ",HeightInMeters"
    + ",RegionName"
    + ",CloudPercent"
    + ",DataSize"
    + ",RollViewingAngle"
    + ",PitchViewingAngle"
    + ",PitchSatelliteAngle"
    + ",RollSatelliteAngle"
    + ",YawSatelliteAngle"
    + ",SolarAzimuth"
    + ",SolarZenith"
    + ",SatelliteAzimuth"
    + ",SatelliteZenith"
    + ",GainMode"
    + ",IntegrationTime"
    + ",IntegrationLevel"
    + ",MapProjection"
    + ",EarthEllipsoid"
    + ",ZoneNo"
    + ",ResamplingKernel"
    + ",HeightMode"
    + ",EphemerisData"
    + ",AttitudeData"
    + ",RadiometricMethod"
    + ",MtfCorrection"
    + ",Denoise"
    + ",RayleighCorrection"
    + ",UsedGCPNo"
    + ",CenterLatitude"
    + ",CenterLongitude"
    + ",TopLeftLatitude"
    + ",TopLeftLongitude"
    + ",TopRightLatitude"
    + ",TopRightLongitude"
    + ",BottomRightLatitude"
    + ",BottomRightLongitude"
    + ",BottomLeftLatitude"
    + ",BottomLeftLongitude"
    + ",TopLeftMapX"
    + ",TopLeftMapY"
    + ",TopRightMapX"
    + ",TopRightMapY"
    + ",BottomRightMapX"
    + ",BottomRightMapY"
    + ",BottomLeftMapX"
    + ",BottomLeftMapY"
    + ",DataArchiveFile"
    + ",BrowseFileLocation"
    + ",ThumbFileLocation,QRST_CODE,size)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}'," +
                        "'{61}','{62}','{63}','{64}','{65}','{66}','{67}','{68}','{69}','{70}','{71}','{72}','{73}'," +
                        "'{74}','{75}','{76}','{77}','{78}','{79}','{80}','{81}','{82}',{83});"
                        , ID
                        , Name
                        , SatelliteID
                        , ReceiveStationID
                        , SensorID
                        , ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss")
                        , OrbitID
                        , OrbitType
                        , AttType
                        , StripID
                        , ProduceType
                        , SceneID
                        , DDSFlag
                        , ProductID
                        , ProductLevel
                        , ProductFormat
                        , ProduceTime.ToString("yyyy-MM-dd HH:mm:ss")
                        , Bands
                        , ScenePath
                        , SceneRow
                        , SatPath
                        , SatRow
                        , SceneCount
                        , SceneShift
                        , StartTime.ToString("yyyy-MM-dd HH:mm:ss")
                        , EndTime.ToString("yyyy-MM-dd HH:mm:ss")
                        , CenterTime.ToString("yyyy-MM-dd HH:mm:ss")
                        , StartLine
                        , EndLine
                        , ImageGSD
                        , WidthInPixels
                        , HeightInPixels
                        , WidthInMeters
                        , HeightInMeters
                        , RegionName
                        , CloudPercent
                        , DataSize
                        , RollViewingAngle
                        , PitchViewingAngle
                        , PitchSatelliteAngle
                        , RollSatelliteAngle
                        , YawSatelliteAngle
                        , SolarAzimuth
                        , SolarZenith
                        , SatelliteAzimuth
                        , SatelliteZenith
                        , GainMode
                        , IntegrationTime.ToString("yyyy-MM-dd HH:mm:ss")
                        , IntegrationLevel
                        , MapProjection
                        , EarthEllipsoid
                        , ZoneNo
                        , ResamplingKernel
                        , HeightMode
                        , EphemerisData
                        , AttitudeData
                        , RadiometricMethod
                        , MtfCorrection
                        , Denoise
                        , RayleighCorrection
                        , UsedGCPNo
                        , CenterLatitude
                        , CenterLongitude
                        , TopLeftLatitude
                        , TopLeftLongitude
                        , TopRightLatitude
                        , TopRightLongitude
                        , BottomRightLatitude
                        , BottomRightLongitude
                        , BottomLeftLatitude
                        , BottomLeftLongitude
                        , TopLeftMapX
                        , TopLeftMapY
                        , TopRightMapX
                        , TopRightMapY
                        , BottomRightMapX
                        , BottomRightMapY
                        , BottomLeftMapX
                        , BottomLeftMapY
                        , DataArchiveFile
                        , BrowseFileLocation
                        , ThumbFileLocation
                        , QRST_CODE
                        , size
                        //, string.Format("ST_SetSRID(ST_MakePolygon(ST_MakeLine(ARRAY[ST_Point({0}, {1}), ST_Point({2}, {3}), ST_Point({4}, {5}), ST_Point({6}, {7}), ST_Point({8}, {9})])), 4326)", TopLeftLongitude, TopLeftLatitude, TopRightLongitude, TopRightLatitude, BottomRightLongitude, BottomRightLatitude, BottomLeftLongitude, BottomLeftLatitude, TopLeftLongitude, TopLeftLatitude)
                        ));
                //Console.WriteLine(strSql.ToString());
                sqlBase.ExecuteSql(strSql.ToString());
                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_gf1bcd set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_gf1bcd", EnumDBType.MIDB);

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
                for (int i = 0; i < bcdAttributeNames.Length; i++)
                {
                    node = root.GetElementsByTagName(bcdAttributeNames[i]).Item(0);
                    if (node == null)
                    {
                        bcdAttributeValues[i] = "";
                    }
                    else
                    {
                        bcdAttributeValues[i] = node.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息出错" + ex.ToString());
            }
        }

        //GF1B_PMS_E125.3_N43.5_20180622_L1A0005110192.tar.gz
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
                return string.Format("实验验证数据库\\光学卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)), Name);
            }
            return "";
        }

        public static bool HasCorrectedData(string qrst_code, IDbBaseUtilities evdb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CorDataFlag from prod_gf1bcd ");
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
            strSql.Append("select NAME,SatelliteID,SensorID,CorDataFlag from prod_gf1bcd ");
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
