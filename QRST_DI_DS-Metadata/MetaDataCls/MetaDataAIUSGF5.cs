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
    public class MetaDataAIUSGF5 : MetaData
    {
        public string[] gf5AttributeNames =
        {
            "SatelliteID",
            "ReceiveStationID",
            "SensorID",
            "ProduceTime",
            "ProductLevel",
            "ProductFormat",
            "SceneID",
            "POrbitID",
            "StartTime",
            "EndTime",
            "InstrumentMode",
            "FrameNumber",
            "QualityFlag",
            "DDSFlag",
            "SoftwareVersion",
            "ProductID",
            "TangentPointNumber",
            "StartTangentHeight",
            "EndTangentHeight",
            "StartCenterLong",
            "EndCenterLong",
            "StartCenterLat",
            "EndCenterLat",
            "StartSatPosition",
            "EndSatPosition",
            "StartSunPosition",
            "EndSunPosition",
            "CalibrationParam",
            "CalibrationParamVersion"
        };
        public string[] gf5AttributeValues;

        public MetaDataAIUSGF5(string _name, string _qrst_code)
        {
            gf5AttributeValues = new string[gf5AttributeNames.Length];
            Name = _name;
            QRST_CODE = _qrst_code;
        }

        public MetaDataAIUSGF5()
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
        public string POrbitID
        {
            set { gf5AttributeValues[7] = value; }
            get { return gf5AttributeValues[7]; }
        }

        public string StartTime
        {
            set { gf5AttributeValues[8] = value; }
            get { return gf5AttributeValues[8]; }
        }

        public string EndTime
        {
            set { gf5AttributeValues[9] = value; }
            get { return gf5AttributeValues[9]; }
        }

        public string InstrumentMode
        {
            set { gf5AttributeValues[10] = value; }
            get { return gf5AttributeValues[10]; }
        }

        public string FrameNumber
        {
            set { gf5AttributeValues[11] = value; }
            get { return gf5AttributeValues[11]; }
        }

        public string QualityFlag
        {
            set { gf5AttributeValues[12] = value; }
            get { return gf5AttributeValues[12]; }
        }

        public string DDSFlag
        {
            set { gf5AttributeValues[13] = value; }
            get { return gf5AttributeValues[13]; }
        }

        public string SoftwareVersion
        {
            set { gf5AttributeValues[14] = value; }
            get { return gf5AttributeValues[14]; }
        }

        public string ProductID
        {
            set { gf5AttributeValues[15] = value; }
            get { return gf5AttributeValues[15]; }
        }

        public string TangentPointNumber
        {
            set { gf5AttributeValues[16] = value; }
            get { return gf5AttributeValues[16]; }
        }

        public string StartTangentHeight
        {
            set { gf5AttributeValues[17] = value; }
            get { return gf5AttributeValues[17]; }
        }

        public string EndTangentHeight
        {
            set { gf5AttributeValues[18] = value; }
            get { return gf5AttributeValues[18]; }
        }

        public string StartCenterLong
        {
            set { gf5AttributeValues[19] = value; }
            get { return gf5AttributeValues[19]; }
        }

        public string EndCenterLong
        {
            set { gf5AttributeValues[20] = value; }
            get { return gf5AttributeValues[20]; }
        }

        public string StartCenterLat
        {
            set { gf5AttributeValues[21] = value; }
            get { return gf5AttributeValues[21]; }
        }

        public string EndCenterLat
        {
            set { gf5AttributeValues[22] = value; }
            get { return gf5AttributeValues[22]; }
        }

        public string StartSatPosition
        {
            set { gf5AttributeValues[23] = value; }
            get { return gf5AttributeValues[23]; }
        }

        public string EndSatPosition
        {
            set { gf5AttributeValues[24] = value; }
            get { return gf5AttributeValues[24]; }
        }

        public string StartSunPosition
        {
            set { gf5AttributeValues[25] = value; }
            get { return gf5AttributeValues[25]; }
        }

        public string EndSunPosition
        {
            set { gf5AttributeValues[26] = value; }
            get { return gf5AttributeValues[26]; }
        }

        public string CalibrationParam
        {
            set { gf5AttributeValues[27] = value; }
            get { return gf5AttributeValues[27]; }
        }

        public string CalibrationParamVersion
        {
            set { gf5AttributeValues[28] = value; }
            get { return gf5AttributeValues[28]; }
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
                Constant.IdbOperating.LockTable("prod_aius_gf5", EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_aius_gf5 where Name ='{0}'", Name);
                Console.WriteLine(presql);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_aius_gf5 where QRST_CODE ='{0}'", QRST_CODE);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_aius_gf5");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_aius_gf5", ID);
                }


                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_aius_gf5(");
                strSql.Append("ID, Name, SatelliteID, ReceiveStationID, SensorID, ProduceTime, ProductLevel, ProductFormat, SceneID, POrbitID, StartTime, EndTime," +
                    " InstrumentMode, FrameNumber, QualityFlag, DDSFlag, SoftwareVersion, ProductID, TangentPointNumber, StartTangentHeight, EndTangentHeight, " +
                    "StartCenterLong, EndCenterLong, StartCenterLat, EndCenterLat, StartSatPosition, EndSatPosition, StartSunPosition, EndSunPosition, CalibrationParam, " +
                    "CalibrationParamVersion, QRST_CODE, size");
                strSql.Append(") values (");
                strSql.Append(
                    string.Format(
                        "{0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', " +
                        "'{12}', '{13}', '{14}', '{15}', '{16}', '{17}', {18}, {19}, {20}, {21}, {22}, " +
                        "{23}, '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', {33})", ID, Name, SatelliteID, ReceiveStationID,
                        SensorID, ProduceTime, ProductLevel, ProductFormat, SceneID, POrbitID, StartTime, EndTime, InstrumentMode,
                        FrameNumber, QualityFlag, DDSFlag, SoftwareVersion, ProductID, TangentPointNumber, StartTangentHeight,
                        EndTangentHeight, StartCenterLong, EndCenterLong, StartCenterLat, EndCenterLat, StartSatPosition,
                        EndSatPosition, StartSunPosition, EndSunPosition, CalibrationParam, CalibrationParamVersion, QRST_CODE, size));
                Console.WriteLine(strSql.ToString());

                sqlBase.ExecuteSql(strSql.ToString());


                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_aius_gf5 set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_aius_gf5", EnumDBType.MIDB);

            }
            catch (Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }
        }

        public string GetCorrectedDataPath()
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 5)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[2].Substring(0, 4);
                string month = strArr[2].Substring(4, 2);
                string day = strArr[2].Substring(6, 2);
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

        // GF5_AIUS_20190520_005480_L10000006281.tar.gz
        public override string GetRelateDataPath()
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 5)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[2].Substring(0, 4);
                string month = strArr[2].Substring(4, 2);
                string day = strArr[2].Substring(6, 2);
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
            strSql.Append("select CorDataFlag from prod_aius_gf5 ");
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
            strSql.Append("select NAME,SatelliteID,SensorID,CorDataFlag,qrst_code from prod_aius_gf5 ");
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
