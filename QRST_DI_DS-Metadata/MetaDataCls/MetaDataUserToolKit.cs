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
    public class MetaDataUserToolKit:MetaData
    {
        public string[]  toolkitAttributes = new string[]
        {
            "toolname","type","satelliteId",
            "sensorId","releaseTime","toolVersion",
            "bit","OS","author","unitName","keywords",
            "toolDescribe","filename"
        };

        public string[] toolkitValues;   //一般属性值
        public MetaDataPublicInfo publicInfo;  //公共属性值

        public MetaDataUserToolKit()
        {
            toolkitValues = new string[toolkitAttributes.Length];
        }

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
        public string toolname
        {
            set { toolkitValues[0] = value; }
            get { return toolkitValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string type
        {
            set { toolkitValues[1] = value; }
            get { return toolkitValues[1]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string satelliteId
        {
            set { toolkitValues[2] = value; }
            get { return toolkitValues[2]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sensorId
        {
            set { toolkitValues[3] = value; }
            get { return toolkitValues[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime releaseTime
        {
            set { toolkitValues[4] = value.ToString(); }
            get {
                try
                {
                    DateTime dt= DateTime.Parse(toolkitValues[4]);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string toolVersion
        {
            set { toolkitValues[5] = value; }
            get { return toolkitValues[5]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bit
        {
            set { toolkitValues[6] = value; }
            get { return toolkitValues[6]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OS
        {
            set { toolkitValues[7] = value; }
            get { return toolkitValues[7]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string author
        {
            set { toolkitValues[8] = value; }
            get { return toolkitValues[8]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string unitName
        {
            set { toolkitValues[9] = value; }
            get { return toolkitValues[9]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string keywords
        {
            set { toolkitValues[10] = value; }
            get { return toolkitValues[10]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string toolDescribe
        {
            set { toolkitValues[11] = value; }
            get { return toolkitValues[11]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string filename
        {
            set { toolkitValues[12] = value; }
            get { return toolkitValues[12]; }
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
                //读取一般信息
                for (int i = 0; i < toolkitAttributes.Length; i++)
                {
                    node = root.GetElementsByTagName(toolkitAttributes[i]).Item(0);
                    if (node == null)
                    {
                        toolkitValues[i] = "";
                    }
                    else
                    {
                        toolkitValues[i] = node.InnerText;
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
                throw new Exception("读取元数据信息失败！"+ex.ToString());
            }
        }

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //导入基本信息
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("isdb_toolkit",EnumDBType.MIDB);
                tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                ID = sqlBase.GetMaxID("ID", "isdb_toolkit");
                QRST_CODE = tablecode.GetDataQRSTCode("isdb_toolkit", ID);
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into isdb_toolkit(");
                strSql.Append("ID,toolname,type,satelliteId,sensorId,releaseTime,toolVersion,bit,OS,author,unitName,keywords,toolDescribe,filename,QRST_CODE)");
                strSql.Append(" values (");
                strSql.Append(
                    string.Format(
                        "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
                        ID, toolname, type, satelliteId, sensorId, releaseTime.ToString("yyyy-MM-dd HH:mm:ss"), toolVersion, bit, OS, author, unitName,
                        keywords, toolDescribe, filename, QRST_CODE));
     //           strSql.Append("@ID,@toolname,@type,@satelliteId,@sensorId,@releaseTime,@toolVersion,@bit,@OS,@author,@unitName,@keywords,@toolDescribe,@filename,@QRST_CODE)");
     //           MySqlParameter[] parameters = {
					//new MySqlParameter("@ID", MySqlDbType.Int32,22),
					//new MySqlParameter("@toolname", MySqlDbType.Text),
					//new MySqlParameter("@type", MySqlDbType.VarChar,200),
					//new MySqlParameter("@satelliteId", MySqlDbType.Text),
					//new MySqlParameter("@sensorId", MySqlDbType.Text),
					//new MySqlParameter("@releaseTime", MySqlDbType.DateTime),
					//new MySqlParameter("@toolVersion", MySqlDbType.Text),
					//new MySqlParameter("@bit", MySqlDbType.VarChar,20),
					//new MySqlParameter("@OS", MySqlDbType.Text),
					//new MySqlParameter("@author", MySqlDbType.Text),
					//new MySqlParameter("@unitName", MySqlDbType.Text),
					//new MySqlParameter("@keywords", MySqlDbType.Text),
					//new MySqlParameter("@toolDescribe", MySqlDbType.Text),
					//new MySqlParameter("@filename", MySqlDbType.Text),
					//new MySqlParameter("@QRST_CODE", MySqlDbType.Text)};
     //           parameters[0].Value = ID;
     //           parameters[1].Value = toolname;
     //           parameters[2].Value = type;
     //           parameters[3].Value = satelliteId;
     //           parameters[4].Value = sensorId;
     //           parameters[5].Value = releaseTime;
     //           parameters[6].Value = toolVersion;
     //           parameters[7].Value = bit;
     //           parameters[8].Value = OS;
     //           parameters[9].Value = author;
     //           parameters[10].Value =unitName;
     //           parameters[11].Value = keywords;
     //           parameters[12].Value = toolDescribe;
     //           parameters[13].Value = filename;
     //           parameters[14].Value = QRST_CODE;
                sqlBase.ExecuteSql(strSql.ToString());
                Constant.IdbOperating.UnlockTable("isdb_toolkit",EnumDBType.MIDB);

                //存储公共信息
                if (publicInfo != null)
                {
                    publicInfo.ImportData(sqlBase, QRST_CODE, tablecode);
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception("元数据导入失败"+ex.ToString());
            }
        }
    }
}
