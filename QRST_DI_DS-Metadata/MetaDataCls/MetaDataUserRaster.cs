using System;
using System.Text;
using System.Xml;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataUserRaster:MetaData
    {
        public MetaDataUserRaster()
        {
            userRasterValues = new string[userRasterAttributes.Length];
        }

        public string[] userRasterAttributes = new string[]
        {
            "proName","proEngName","proLevel",
            "prodataName","productDate","dataName",
            "satelliteId","sensorId","sceneDate","pixelSpacing",
            "sceneCenterLat","sceneCenterLong","dataUpperLeftLat",
            "dataUpperLeftLong","dataUpperRightLat","dataUpperRightLong",
            "dataLowerLeftLat","dataLowerLeftLong","dataLowerRightLat",
            "dataLowerRightLong","dataFormatDes","SceneRow",
             "ScenePath","mapProjection","DataType"
        };
        public string[] userRasterValues;
        public MetaDataPublicInfo publicInfo;  //公共属性值

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
        public string DataType
        {
            set { userRasterValues[24] = value; }
            get { return userRasterValues[24]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string proName
        {
            set { userRasterValues[0] = value; }
            get { return userRasterValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string proEngName
        {
            set { userRasterValues[1] = value; }
            get { return userRasterValues[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? proLevel
        {
            set { userRasterValues[2] = value.ToString(); }
            get
            {
                try
                {
                    return int.Parse(userRasterValues[2]);
                }
                catch(Exception)
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string prodataName
        {
            set { userRasterValues[3] = value; }
            get { return userRasterValues[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime productDate
        {
            set { userRasterValues[4] = value.ToString(); }
            get
            {
                try
                {
                    DateTime dt= DateTime.Parse(userRasterValues[4]);
                    return dt;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dataName
        {
            set { userRasterValues[5] = value; }
            get { return userRasterValues[5]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string satelliteId
        {
            set { userRasterValues[6] = value; }
            get { return userRasterValues[6]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sensorId
        {
            set { userRasterValues[7] = value; }
            get { return userRasterValues[7]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime sceneDate
        {
            set { userRasterValues[8] = value.ToString(); }
            get
            {
                try
                {
                    DateTime dt= DateTime.Parse(userRasterValues[8]);
                    return dt;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? pixelSpacing
        {
            set { userRasterValues[9] = value.ToString(); }
            get
            {
                try
                {
                    return int.Parse(userRasterValues[9]);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? sceneCenterLat
        {
            set { userRasterValues[10] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[10]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? sceneCenterLong
        {
            set { userRasterValues[11] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[11]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataUpperLeftLat
        {
            set { userRasterValues[12] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[12]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataUpperLeftLong
        {
            set { userRasterValues[13] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[13]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataUpperRightLat
        {
            set { userRasterValues[14] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[14]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataUpperRightLong
        {
            set { userRasterValues[15] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[15]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataLowerLeftLat
        {
            set { userRasterValues[16] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[16]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataLowerLeftLong
        {
            set { userRasterValues[17] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[17]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataLowerRightLat
        {
            set { userRasterValues[18] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[18]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? dataLowerRightLong
        {
            set { userRasterValues[19] = value.ToString(); }
            get
            {
                try
                {
                    return decimal.Parse(userRasterValues[19]);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dataFormatDes
        {
            set { userRasterValues[20] = value; }
            get { return userRasterValues[20]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SceneRow
        {
            set { userRasterValues[21] = value; }
            get { return userRasterValues[21]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ScenePath
        {
            set { userRasterValues[22] = value; }
            get { return userRasterValues[22]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mapProjection
        {
            set { userRasterValues[23] = value; }
            get { return userRasterValues[23]; }
        }

        #endregion Model

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
                //读取一般信息
                for (int i = 0; i < userRasterAttributes.Length; i++)
                {
                    node = root.GetElementsByTagName(userRasterAttributes[i]).Item(0);
                    if (node == null)
                    {
                        userRasterValues[i] = "";
                    }
                    else
                    {
                        userRasterValues[i] = node.InnerText;
                    }
                }
                //读取公共信息
                XmlNodeList publicInfoArr = root.GetElementsByTagName("PublicInfo");
                if (publicInfoArr.Count > 0)
                {
                    publicInfo = new MetaDataPublicInfo();
                    publicInfo.ReadAttribute(publicInfoArr[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取元数据信息失败！" + ex.ToString());
            }
        }
        
        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //导入基本信息
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("isdb_userraster",EnumDBType.MIDB);
                tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                ID = sqlBase.GetMaxID("ID", "isdb_userraster");
                QRST_CODE = tablecode.GetDataQRSTCode("isdb_userraster", ID);

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into isdb_userraster(");
                strSql.Append("ID,DataType,proName,proEngName,proLevel,prodataName,productDate,dataName,satelliteId," +
                              "sensorId,sceneDate,pixelSpacing,sceneCenterLat,sceneCenterLong,dataUpperLeftLat,dataUpperLeftLong,dataUpperRightLat,dataUpperRightLong," +
                              "dataLowerLeftLat,dataLowerLeftLong,dataLowerRightLat,dataLowerRightLong,dataFormatDes,SceneRow,ScenePath,mapProjection,QRST_CODE)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}',{11},{12},{13},{14},{15},{16},{17},{18}," +
                        "{19},{20},{21},'{22}','{23}','{24}','{25}','{26}')", ID, DataType, proName, proEngName,
                        proLevel, prodataName, productDate.ToString("yyyy-MM-dd HH:mm:ss"), dataName, satelliteId, sensorId, sceneDate.ToString("yyyy-MM-dd HH:mm:ss"), pixelSpacing,
                        sceneCenterLat, sceneCenterLong, dataUpperLeftLat, dataUpperLeftLong, dataUpperRightLat,
                        dataUpperRightLong, dataLowerLeftLat, dataLowerLeftLong, dataLowerRightLat, dataLowerRightLong,
                        dataFormatDes, SceneRow, ScenePath, mapProjection, QRST_CODE));
                //strSql.Append("@ID,@DataType,@proName,@proEngName,@proLevel,@prodataName,@productDate,@dataName,@satelliteId,@sensorId,@sceneDate,@pixelSpacing,@sceneCenterLat,@sceneCenterLong,
                //@dataUpperLeftLat,@dataUpperLeftLong,@dataUpperRightLat,@dataUpperRightLong,@dataLowerLeftLat,@dataLowerLeftLong,@dataLowerRightLat,@dataLowerRightLong,@dataFormatDes,@SceneRow,
                //@ScenePath,@mapProjection,@QRST_CODE)");
                //MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,9),
					//new MySqlParameter("@DataType", MySqlDbType.VarChar,50),
					//new MySqlParameter("@proName", MySqlDbType.Text),
					//new MySqlParameter("@proEngName", MySqlDbType.Text),
					//new MySqlParameter("@proLevel", MySqlDbType.Int32,3),
					//new MySqlParameter("@prodataName", MySqlDbType.Text),
					//new MySqlParameter("@productDate", MySqlDbType.DateTime),
					//new MySqlParameter("@dataName", MySqlDbType.Text),
					//new MySqlParameter("@satelliteId", MySqlDbType.VarChar,50),
					//new MySqlParameter("@sensorId", MySqlDbType.VarChar,50),
					//new MySqlParameter("@sceneDate", MySqlDbType.DateTime),
					//new MySqlParameter("@pixelSpacing", MySqlDbType.Int32,5),
					//new MySqlParameter("@sceneCenterLat", MySqlDbType.Decimal,10),
					//new MySqlParameter("@sceneCenterLong", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataUpperLeftLat", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataUpperLeftLong", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataUpperRightLat", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataUpperRightLong", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataLowerLeftLat", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataLowerLeftLong", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataLowerRightLat", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataLowerRightLong", MySqlDbType.Decimal,10),
					//new MySqlParameter("@dataFormatDes", MySqlDbType.Text),
					//new MySqlParameter("@SceneRow", MySqlDbType.Text),
					//new MySqlParameter("@ScenePath", MySqlDbType.Text),
					//new MySqlParameter("@mapProjection", MySqlDbType.Text),
					//new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,50)};
     //           parameters[0].Value = ID;
     //           parameters[1].Value = DataType;
     //           parameters[2].Value = proName;
     //           parameters[3].Value = proEngName;
     //           parameters[4].Value = proLevel;
     //           parameters[5].Value = prodataName;
     //           parameters[6].Value = productDate;
     //           parameters[7].Value = dataName;
     //           parameters[8].Value = satelliteId;
     //           parameters[9].Value = sensorId;
     //           parameters[10].Value = sceneDate;
     //           parameters[11].Value = pixelSpacing;
     //           parameters[12].Value = sceneCenterLat;
     //           parameters[13].Value = sceneCenterLong;
     //           parameters[14].Value = dataUpperLeftLat;
     //           parameters[15].Value = dataUpperLeftLong;
     //           parameters[16].Value = dataUpperRightLat;
     //           parameters[17].Value = dataUpperRightLong;
     //           parameters[18].Value = dataLowerLeftLat;
     //           parameters[19].Value = dataLowerLeftLong;
     //           parameters[20].Value = dataLowerRightLat;
     //           parameters[21].Value = dataLowerRightLong;
     //           parameters[22].Value = dataFormatDes;
     //           parameters[23].Value = SceneRow;
     //           parameters[24].Value = ScenePath;
     //           parameters[25].Value = mapProjection;
     //           parameters[26].Value = QRST_CODE;
                sqlBase.ExecuteSql(strSql.ToString());
                Constant.IdbOperating.UnlockTable("isdb_userraster",EnumDBType.MIDB);

                //存储公共信息
                if (publicInfo != null)
                {
                    publicInfo.ImportData(sqlBase, QRST_CODE, tablecode);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("元数据导入失败" + ex.ToString());
            }
        }
    }
}
