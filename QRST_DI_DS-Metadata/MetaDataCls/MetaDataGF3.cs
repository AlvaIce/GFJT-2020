/*
 * 创建时间：2017-03-01
 * 描述：用于描述高分三号卫星元数据信息
 * 
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
    public class MetaDataGF3:MetaData
    {
        public string[] gf3AttributeNames = {
                                  "satellite",        //GF3       
                                    "sceneID",      
                                     "segmentID",  
                                    "orbitID",   
                                    "orbitType",
                                    "attiType",     //STAR
                                     "Direction","sceneTotalNum",
                                     "sceneNum",
                                     "productID",   //2015621
                                     "ReceiveTime","DocumentIdentifier",
                                    "IsZeroDopplerSteering",  
                                    "Station",         //MYN
                                    "sensorID",      //SAR
                                    "imagingMode",  //UFS
                                    "imagingNumTotal"  ,     
                                    "imagingNum",          
                                    "lamda",         
                                    "RadarCenterFrequency",
                                     "lookDirection",
                                     "antennaMode","agcMode",
                                     "satelliteTime_start",
                                     "satelliteTime_end",
                                     "CenterTime",
                                     "Rs",
                                     "satVelocity",
                                     "RollAngle",
                                     "PitchAngle",
                                     "YawAngle",
                                     "Xs",
                                     "Ys",
                                     "Zs",
                                     "Vxs",
                                     "Vys",
                                     "Vzs",
                                     "NominalResolution",
                                     "WidthInMeters",
                                     "productLevel",
                                     "productType",
                                     "productFormat","productGentime","productPolar","nearRange",
                                     "refRange","eqvFs","eqvPRF",
                                     "center_latitude","center_longitude",
                                     "topLeft_latitude","topLeft_longitude","topRight_latitude","topRight_longitude","bottomLeft_latitude","bottomLeft_longitude",
                                     "bottomRight_latitude","bottomRight_longitude",
                                     "width",
                                     "height",
                                     "widthspace",
                                     "heightspace",
                                     "sceneShift","imagebit",
                                     "QualifyValue_HH","QualifyValue_HV","QualifyValue_VH","QualifyValue_VV",
                                     "EphemerisData","AttitudeData","algorithm","CalibrationConst",
                                     "MultilookRange","MultilookAzimuth",
                                     "RangeWeightTypede","RangeWeightPara","AzimuthWeightType","AzimuthWeightPara",
                                     "EarthModel","ProjectModel","DEMModel","QualifyModel",
                                     "RadiometricModel","incidenceAngleNearRange","incidenceAngleFarRange",
                                      "DopplerParametersReferenceTime",
                                      "d0","d1","d2","d3","d4","r0","r1","r2","r3","r4",
                                      "ZoneNo",
                                      "DEM",

                                  };
        public string[] gf3AttributeValues;

          public MetaDataGF3(string _name,string _qrst_code)
        {
            gf3AttributeValues = new string[gf3AttributeNames.Length];
            Name = _name;
            QRST_CODE = _qrst_code;
        }

        public MetaDataGF3()
        {
            gf3AttributeValues = new string[gf3AttributeNames.Length];
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
        public string QRST_CODE
        {
            set;
            get;
        }
        public string CorDataFlag = "";
        /// <summary>
        /// 
        /// </summary>
        public string satellite
        {
            set { gf3AttributeValues[0] = value; }
            get { return gf3AttributeValues[0]; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string sceneID
        {
            set { gf3AttributeValues[1] = value; }
            get { return gf3AttributeValues[1]; }
        }
        public string segmentID
        {
            set { gf3AttributeValues[2] = value; }
            get { return gf3AttributeValues[2]; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string orbitID
        {
            set { gf3AttributeValues[3] = value; }
            get { return gf3AttributeValues[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string orbitType
        {
            set { gf3AttributeValues[4] = value; }
            get { return gf3AttributeValues[4]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string attiType
        {
            set { gf3AttributeValues[5] = value; }
            get { return gf3AttributeValues[5]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Direction
        {
            set { gf3AttributeValues[6] = value; }
            get { return gf3AttributeValues[6]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sceneTotalNum
        {
            set { gf3AttributeValues[7] = value; }
            get { return gf3AttributeValues[7]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string sceneNum
        {
            set { gf3AttributeValues[8] = value; }
            get { return gf3AttributeValues[8]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string productID
        {
            set { gf3AttributeValues[9] = value; }
            get { return gf3AttributeValues[9]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ReceiveTime
        {
            set { gf3AttributeValues[10] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf3AttributeValues[10], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }      
        public string DocumentIdentifier
        {
            set { gf3AttributeValues[11] = value; }
            get { return gf3AttributeValues[11]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IsZeroDopplerSteering
        {
            set { gf3AttributeValues[12] = value; }
            get { return gf3AttributeValues[12]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Station
        {
            set { gf3AttributeValues[13] = value; }
            get { return gf3AttributeValues[13]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sensorID
        {
            set { gf3AttributeValues[14] = value; }
            get { return gf3AttributeValues[14]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string imagingMode
        {
            set { gf3AttributeValues[15] = value; }
            get { return gf3AttributeValues[15]; }
        }
         public string imagingNumTotal
        {
            set { gf3AttributeValues[16] = value; }
            get { return gf3AttributeValues[16]; }
        }
         public string imagingNum
        {
            set { gf3AttributeValues[17] = value; }
            get { return gf3AttributeValues[17]; }
        }
         public string lamda
         {
             set { gf3AttributeValues[18] = value; }
             get { return gf3AttributeValues[18]; }
         }
         public string RadarCenterFrequency
         {
             set { gf3AttributeValues[19] = value; }
             get { return gf3AttributeValues[19]; }
         }
        
        /// <summary>
        /// 
        /// </summary>
         public string lookDirection
        {
            set { gf3AttributeValues[20] = value; }
            get { return gf3AttributeValues[20]; }
        }
         public string antennaMode
        {
            set { gf3AttributeValues[21] = value; }
            get { return gf3AttributeValues[21]; }
        }
         public string agcMode
         {
             set { gf3AttributeValues[22] = value; }
             get { return gf3AttributeValues[22]; }
         }
        /// <summary>
        /// 
        /// </summary>
        public DateTime satelliteTime_start
        {
            set { gf3AttributeValues[23] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf3AttributeValues[23], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
        public DateTime satelliteTime_end
        {
            set { gf3AttributeValues[24] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf3AttributeValues[24], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
        public DateTime CenterTime
        {
            set { gf3AttributeValues[25] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf3AttributeValues[25], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
        public string Rs
        {
            set { gf3AttributeValues[26] = value; }
            get { return gf3AttributeValues[26]; }
        }
        public string satVelocity
        {
            set { gf3AttributeValues[27] = value; }
            get { return gf3AttributeValues[27]; }
        }
        public string RollAngle
        {
            set { gf3AttributeValues[28] = value; }
            get { return gf3AttributeValues[28]; }
        }
        public string PitchAngle
        {
            set { gf3AttributeValues[29] = value; }
            get { return gf3AttributeValues[29]; }
        }
        public string YawAngle
        {
            set { gf3AttributeValues[30] = value; }
            get { return gf3AttributeValues[30]; }
        }
        
        public string Xs
        {
            set { gf3AttributeValues[31] = value; }
            get { return gf3AttributeValues[31]; }
        }
        public string Ys
        {
            set { gf3AttributeValues[32] = value; }
            get { return gf3AttributeValues[32]; }
        }
        public string Zs
        {
            set { gf3AttributeValues[33] = value; }
            get { return gf3AttributeValues[33]; }
        }
        public string Vxs
        {
            set { gf3AttributeValues[34] = value; }
            get { return gf3AttributeValues[34]; }
        }
        public string Vys
        {
            set { gf3AttributeValues[35] = value; }
            get { return gf3AttributeValues[35]; }
        }
        public string Vzs
        {
            set { gf3AttributeValues[36] = value; }
            get { return gf3AttributeValues[36]; }
        }

        public string NominalResolution
        {
            set { gf3AttributeValues[37] = value; }
            get { return gf3AttributeValues[37]; }
        }
        public string WidthInMeters
        {
            set { gf3AttributeValues[38] = value; }
            get { return gf3AttributeValues[38]; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string productLevel
        {
            set { gf3AttributeValues[39] = value; }
            get { return gf3AttributeValues[39]; }
        }
        public string productType
        {
            set { gf3AttributeValues[40] = value; }
            get { return gf3AttributeValues[40]; }
        }
        public string productFormat
        {
            set { gf3AttributeValues[41] = value; }
            get { return gf3AttributeValues[41]; }
        }
         public DateTime productGentime
        {
            set { gf3AttributeValues[42] = value.ToString(); }
            get
            {
                DateTime dt;
                if (DateTime.TryParse(gf3AttributeValues[42], out dt))
                {
                    return dt;
                }
                return dt;
            }
        }
         public string productPolar
        {
            set { gf3AttributeValues[43] = value; }
            get { return gf3AttributeValues[43]; }
        }
         public string nearRange
         {
             set { gf3AttributeValues[44] = value; }
             get { return gf3AttributeValues[44]; }
         }
         public string refRange
         {
             set { gf3AttributeValues[45] = value; }
             get { return gf3AttributeValues[45]; }
         }
         public string eqvFs
         {
             set { gf3AttributeValues[46] = value; }
             get { return gf3AttributeValues[46]; }
         }
         public string eqvPRF
         {
             set { gf3AttributeValues[47] = value; }
             get { return gf3AttributeValues[47]; }
         }

        public double center_latitude
        {
            set { gf3AttributeValues[48] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[48], out dt))
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
        public double center_longitude
        {
            set { gf3AttributeValues[49] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[49], out dt))
                {
                    return dt;
                }
                else
                {
                    return 0;
                }
            }
        }
         public double topLeft_latitude
        {
            set { gf3AttributeValues[50] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[50], out dt))
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
        public double topLeft_longitude
        {
            set { gf3AttributeValues[51] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[51], out dt))
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
        public double topRight_latitude
        {
            set { gf3AttributeValues[52] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[52], out dt))
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
        public double topRight_longitude
        {
            set { gf3AttributeValues[53] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[53], out dt))
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
        public double bottomLeft_latitude
        {
            set { gf3AttributeValues[54] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[54], out dt))
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
        public double bottomLeft_longitude
        {
            set { gf3AttributeValues[55] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[55], out dt))
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
        public double bottomRight_latitude
        {
            set { gf3AttributeValues[56] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[56], out dt))
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
        public double bottomRight_longitude
        {
            set { gf3AttributeValues[57] = value.ToString(); }
            get
            {
                double dt;
                if (double.TryParse(gf3AttributeValues[57], out dt))
                {
                    return dt;
                }
                else
                {
                    return 180;
                }
            }
        }
         public string width
        {
            set { gf3AttributeValues[58] = value; }
            get { return gf3AttributeValues[58]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string height
        {
            set { gf3AttributeValues[59] = value; }
            get { return gf3AttributeValues[59]; }
        }
        public string widthspace
        {
            set { gf3AttributeValues[60] = value; }
            get { return gf3AttributeValues[60]; }
        }
        public string heightspace
        {
            set { gf3AttributeValues[61] = value; }
            get { return gf3AttributeValues[61]; }
        }
        public string sceneShift
        {
            set { gf3AttributeValues[62] = value; }
            get { return gf3AttributeValues[62]; }
        }
        
         public string imagebit
        {
            set { gf3AttributeValues[63] = value; }
            get { return gf3AttributeValues[63]; }
        }

         public string QualifyValue_HH
        {
            set { gf3AttributeValues[64] = value; }
            get { return gf3AttributeValues[64]; }
        }
         public string QualifyValue_HV
        {
            set { gf3AttributeValues[65] = value; }
            get { return gf3AttributeValues[65]; }
        }
         public string QualifyValue_VH
        {
            set { gf3AttributeValues[66] = value; }
            get { return gf3AttributeValues[66]; }
        }
         public string QualifyValue_VV
        {
            set { gf3AttributeValues[67] = value; }
            get { return gf3AttributeValues[67]; }
        }
         public string EphemerisData
        {
            set { gf3AttributeValues[68] = value; }
            get { return gf3AttributeValues[68]; }
        }
         public string AttitudeData
        {
            set { gf3AttributeValues[69] = value; }
            get { return gf3AttributeValues[69]; }
        }
         public string algorithm
        {
            set { gf3AttributeValues[70] = value; }
            get { return gf3AttributeValues[70]; }
        }
         public string CalibrationConst
         {
             set { gf3AttributeValues[71] = value; }
             get { return gf3AttributeValues[71]; }
         }
         
         public string MultilookRange
        {
            set { gf3AttributeValues[72] = value; }
            get { return gf3AttributeValues[72]; }
        }
         public string MultilookAzimuth
        {
            set { gf3AttributeValues[73] = value; }
            get { return gf3AttributeValues[73]; }
        }
         public string RangeWeightTypede
         {
             set { gf3AttributeValues[74] = value; }
             get { return gf3AttributeValues[74]; }
         }
         public string RangeWeightPara
         {
             set { gf3AttributeValues[75] = value; }
             get { return gf3AttributeValues[75]; }
         }
         public string AzimuthWeightType
         {
             set { gf3AttributeValues[76] = value; }
             get { return gf3AttributeValues[76]; }
         }
         public string AzimuthWeightPara
         {
             set { gf3AttributeValues[77] = value; }
             get { return gf3AttributeValues[77]; }
         }
        
         public string EarthModel
        {
            set { gf3AttributeValues[78] = value; }
            get { return gf3AttributeValues[78]; }
        }
         public string ProjectModel
        {
            set { gf3AttributeValues[79] = value; }
            get { return gf3AttributeValues[79]; }
        }
         public string DEMModel
        {
            set { gf3AttributeValues[80] = value; }
            get { return gf3AttributeValues[80]; }
        }
         public string QualifyModel
        {
            set { gf3AttributeValues[81] = value; }
            get { return gf3AttributeValues[81]; }
        }
         public string RadiometricModel
        {
            set { gf3AttributeValues[82] = value; }
            get { return gf3AttributeValues[82]; }
        }
         public string incidenceAngleNearRange
         {
             set { gf3AttributeValues[83] = value; }
             get { return gf3AttributeValues[83]; }
         }
         public string incidenceAngleFarRange
         {
             set { gf3AttributeValues[84] = value; }
             get { return gf3AttributeValues[84]; }
         }
         
         public string DopplerParametersReferenceTime
         {
             set { gf3AttributeValues[85] = value; }
             get { return gf3AttributeValues[85]; }
         }

          public string d0
          {
              set { gf3AttributeValues[86] = value; }
              get { return gf3AttributeValues[86]; }

          }
          public string d1
          {
              set { gf3AttributeValues[87] = value; }
              get { return gf3AttributeValues[87]; }

          }
          public string d2
          {
              set { gf3AttributeValues[88] = value; }
              get { return gf3AttributeValues[88]; }

          }
          public string d3
          {
              set { gf3AttributeValues[89] = value; }
              get { return gf3AttributeValues[89]; }

          }
          public string d4
          {
              set { gf3AttributeValues[90] = value; }
              get { return gf3AttributeValues[90]; }

          }
        public string r0
        {
            set { gf3AttributeValues[91] = value; }
            get { return gf3AttributeValues[91]; }

        }
        public string r1
        {
            set { gf3AttributeValues[92] = value; }
            get { return gf3AttributeValues[92]; }

        }
        public string r2
        {
            set { gf3AttributeValues[93] = value; }
            get { return gf3AttributeValues[93]; }

        }
        public string r3
        {
            set { gf3AttributeValues[94] = value; }
            get { return gf3AttributeValues[94]; }

        }
         public string r4
        {
            set { gf3AttributeValues[95] = value; }
            get { return gf3AttributeValues[95]; }
        
        }
         public string ZoneNo
        {
            set { gf3AttributeValues[96] = value; }
            get { return gf3AttributeValues[96]; }
        }
         public string DEM
        {
            set { gf3AttributeValues[97] = value; }
            get { return gf3AttributeValues[97]; }
        }
        #endregion

       /// <summary>
       /// 解析XML获取数值
       /// </summary>
       /// <param name="fileName"></param>
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
                for (int i = 0; i < gf3AttributeNames.Length; i++)
                {
                    string[] nodeNames = gf3AttributeNames[i].Split('_');
                    if (nodeNames.Length == 0)
                    {
                        node = null;
                    }
                    if (nodeNames.Length == 1)
                    {
                        node = root.GetElementsByTagName(nodeNames[0]).Item(0);
                    }
                    else
                    {
                        XmlNodeList nodes = root.GetElementsByTagName(nodeNames[nodeNames.Length - 1]);
                        foreach (XmlNode no in nodes)
                        {
                            XmlNode pNode = no.ParentNode;
                            if (pNode.Name.Equals(nodeNames[nodeNames.Length - 2]))
                            {
                                node = no;
                                break;
                            }
                        }
                    }
                    if (node == null)
                    {
                        gf3AttributeValues[i] = "";
                    }
                    else
                    {
                        gf3AttributeValues[i] = node.InnerText;
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
                Constant.IdbOperating.LockTable("prod_gf3",EnumDBType.MIDB);
                string presql = string.Format("select ID,QRST_CODE from prod_gf3 where Name ='{0}'", Name);
                DataSet ds = sqlBase.GetDataSet(presql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
                    presql = string.Format("delete from prod_gf3 where QRST_CODE ='{0}'", QRST_CODE);
                    //DataSet ds = sqlBase.GetDataSet(presql);
                    int i = sqlBase.ExecuteSql(presql);
                }
                else
                {
                    tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                    ID = sqlBase.GetMaxID("ID", "prod_gf3");
                    QRST_CODE = tablecode.GetDataQRSTCode("prod_gf3", ID);
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into prod_gf3(");
                strSql.Append(
                    "ID,Name,satellite,sceneID,segmentID,orbitID,orbitType,attiType,Direction,sceneTotalNum,sceneNum,productID,ReceiveTime,DocumentIdentifier,IsZeroDopplerSteering,Station,sensorID,imagingMode," +
                    "imagingNumTotal,imagingNum,lamda,RadarCenterFrequency,lookDirection,antennaMode,agcMode,satelliteTime_start,satelliteTime_end,CenterTime,Rs,satVelocity,RollAngle,PitchAngle,YawAngle,Xs,Ys,Zs,Vxs," +
                    "Vys,Vzs,NominalResolution,WidthInMeters,productLevel,productType,productFormat,productGentime,productPolar,nearRange,refRange,eqvFs,eqvPRF,center_latitude,center_longitude,topLeft_latitude,topLeft_longitude," +
                    "topRight_latitude,topRight_longitude,bottomLeft_latitude,bottomLeft_longitude," +
                    "bottomRight_latitude,bottomRight_longitude,width,height,widthspace,heightspace,sceneShift,imagebit,QualifyValue_HH,QualifyValue_HV,QualifyValue_VH,QualifyValue_VV,EphemerisData,AttitudeData,algorithm,CalibrationConst," +
                    "MultilookRange,MultilookAzimuth,RangeWeightTypede,RangeWeightPara,AzimuthWeightType,AzimuthWeightPara,EarthModel,ProjectModel,DEMModel,QualifyModel,RadiometricModel,incidenceAngleNearRange,incidenceAngleFarRange,DopplerParametersReferenceTime,d0,d1,d2,d3,d4,r0,r1,r2,r3,r4,ZoneNo,DEM,QRST_CODE,ImportTime)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'," +
                        "'{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'," +
                        "'{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}'," +
                        "'{48}','{49}',{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},'{60}','{61}','{62}','{63}'," +
                        "'{64}','{65}','{66}','{67}','{68}','{69}','{70}','{71}','{72}','{73}','{74}','{75}','{76}','{77}','{78}','{79}'," +
                        "'{80}','{81}','{82}','{83}','{84}','{85}','{86}','{87}','{88}','{89}','{90}','{91}','{92}','{93}'," +
                        "'{94}','{95}','{96}','{97}','{98}','{99}','{100}','{101}')", ID, Name, satellite, sceneID, segmentID, orbitID, orbitType,
                        attiType, Direction, sceneTotalNum, sceneNum, productID, ReceiveTime.ToString("yyyy-MM-dd HH:mm:ss"), DocumentIdentifier, IsZeroDopplerSteering,
                        Station, sensorID, imagingMode, imagingNumTotal, imagingNum, lamda, RadarCenterFrequency, lookDirection,
                        antennaMode, agcMode, satelliteTime_start.ToString("yyyy-MM-dd HH:mm:ss"), satelliteTime_end.ToString("yyyy-MM-dd HH:mm:ss"), CenterTime.ToString("yyyy-MM-dd HH:mm:ss"), Rs,
                        satVelocity, RollAngle, PitchAngle, YawAngle, Xs, Ys, Zs, Vxs, Vys, Vzs,
                        NominalResolution, WidthInMeters, productLevel, productType, productFormat, productGentime.ToString("yyyy-MM-dd HH:mm:ss"), productPolar, nearRange,
                        refRange, eqvFs, eqvPRF, center_latitude, center_longitude,
                        topLeft_latitude, topLeft_longitude, topRight_latitude, topRight_longitude, bottomLeft_latitude, bottomLeft_longitude,
                        bottomRight_latitude, bottomRight_longitude, width, height, widthspace, heightspace, sceneShift, imagebit,
                        QualifyValue_HH, QualifyValue_HV, QualifyValue_VH, QualifyValue_VV, EphemerisData, AttitudeData, algorithm, CalibrationConst,
                        MultilookRange, MultilookAzimuth, RangeWeightTypede, RangeWeightPara, AzimuthWeightType, AzimuthWeightPara,
                       EarthModel, ProjectModel, DEMModel, QualifyModel, RadiometricModel, incidenceAngleNearRange, incidenceAngleFarRange,
                       DopplerParametersReferenceTime, d0, d1, d2, d3, d4, r0, r1, r2, r3, r4, ZoneNo, DEM, QRST_CODE,DateTime.Now));
                sqlBase.ExecuteSql(strSql.ToString());

                string destCorrectedData = this.GetCorrectedDataPath();
                //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                string updatesql = string.Format("update prod_gf3 set CorDataFlag = {0} where Name = '{1}'", corDataPath, Name);

                sqlBase.ExecuteSql(updatesql);
                Constant.IdbOperating.UnlockTable("prod_gf3",EnumDBType.MIDB);

            }
            catch (Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }
        }

        //GF3_MYN_UFS_001586_E117.2_N31.6_20161127_L1A_DH_L10002015621.tar.gz
        public override string GetRelateDataPath()
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 10)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string imagingMode = strArr[2];
                string year = strArr[6].Substring(0, 4);
                string month = strArr[6].Substring(4, 2);
                string day = strArr[6].Substring(6, 2);
                return string.Format("实验验证数据库\\GF3卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", satellite, sensor,imagingMode, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)));
            }
            else
            {
                return base.GetRelateDataPath();
            }

        }
        public static string GetRelateDataPath(string Name)
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 10)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string imagingMode = strArr[2];
                string year = strArr[6].Substring(0, 4);
                string month = strArr[6].Substring(4, 2);
                string day = strArr[6].Substring(6, 2);
                return string.Format("实验验证数据库\\GF3卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", satellite, sensor, imagingMode, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)));
                //string satellite = strArr[0];
                //string sensor = strArr[1];
                //string year = strArr[6].Substring(0, 4);
                //string month = strArr[6].Substring(4, 2);
                //string day = strArr[6].Substring(6, 2);
                //return string.Format("实验验证数据库\\高分系列卫星数据\\GF3卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)));
            }
            else
            {
                return "";
            }
        }
        public static bool HasCorrectedData(string qrst_code, IDbBaseUtilities evdb)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CorDataFlag from prod_gf3 ");
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

        private string GetCorrectedDataPath()
        {
            string[] strArr = Name.Split("_".ToCharArray());
            if (strArr.Length == 10)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string imagingMode = strArr[2];
                string year = strArr[6].Substring(0, 4);
                string month = strArr[6].Substring(4, 2);
                string day = strArr[6].Substring(6, 2);
                return string.Format("{6}数据产品库\\数据预处理产品\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\", satellite, sensor,imagingMode, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(Name)), StoragePath.StoreBasePath);
            }
            else
            {
                return "";
            }
        }
        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from prod_gf3 ");
            strSql.AppendFormat(" where QRST_CODE = '{0}'", qrst_code);

            using (DataSet ds = sqlBase.GetDataSet(strSql.ToString()))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                    satellite = ds.Tables[0].Rows[0]["satellite"].ToString();
                    sensorID = ds.Tables[0].Rows[0]["sensorID"].ToString();
                    Station = ds.Tables[0].Rows[0]["Station"].ToString();
                    imagingMode = ds.Tables[0].Rows[0]["imagingMode"].ToString();
                    //QRST_CODE = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
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
