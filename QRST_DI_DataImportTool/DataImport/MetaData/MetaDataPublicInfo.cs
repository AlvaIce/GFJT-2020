using System;
using System.Text;
using System.Xml;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using MySql.Data.MySqlClient;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DataImportTool.DataImport.MetaData
{
    public class MetaDataPublicInfo:MetaData
    {
        public string[] publicInfoAttributes = new string[]
        {
            "UserName","UpLoadTime","Department","Price","PublicCloud","FileSize","DownloadCount","AverageScore"
        };
        public MetaDataPublicInfo()
        {
            publicInfoValues = new string[publicInfoAttributes.Length];
        }
        #region Model
        public string[] publicInfoValues;
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
        public string UserName
        {
            set { publicInfoValues[0] = value; }
            get { return publicInfoValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpLoadTime
        {
            set { publicInfoValues[1] = value.ToString(); }
            get
            {
                try
                {
                    return DateTime.Parse(publicInfoValues[1]);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Department
        {
            set { publicInfoValues[2] = value; }
            get { return publicInfoValues[2]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Price
        {
            set { publicInfoValues[3] = value; }
            get { return publicInfoValues[3]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PublicCloud
        {
            set { publicInfoValues[4] = value; }
            get { return publicInfoValues[4]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double FileSize
        {
            set { publicInfoValues[5] = value.ToString(); }
            get
            {
                try
                {
                    return double.Parse(publicInfoValues[5]);
                }
                catch (Exception ex)
                {
                    return 0;
                }
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
        /// <summary>
        /// 
        /// </summary>
        public int? DownloadCount
        {
            set { publicInfoValues[6] = value.ToString(); }
            get
            {
                try
                {
                    return int.Parse(publicInfoValues[6]);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? AverageScore
        {
            set { publicInfoValues[7] = value.ToString(); }
            get
            {
                try
                {
                    return double.Parse(publicInfoValues[7]);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataID
        {
            set;
            get;
        }
        #endregion Model


        public void ReadAttribute(XmlNode node)
        {
            if (node != null)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (i < publicInfoValues.Length)
                        publicInfoValues[i] = node.ChildNodes[i].InnerText;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlBase"></param>
        /// <param name="dataID">与该公共信息关联的元数据QRST_CODE</param>
        public void ImportData()
        {
            try
            {
                IDbBaseUtilities isdbUtility = Universial.dbOperating.GetsqlBaseObj("ISDB");
                tablecode_Dal tablecode = new tablecode_Dal(isdbUtility);
                ID = isdbUtility.GetMaxID("ID", " isdb_publicinfo");
                QRST_CODE = tablecode.GetDataQRSTCode("isdb_publicinfo", ID);

                StringBuilder strSql3 = new StringBuilder();
                strSql3.Append("insert into isdb_publicinfo(");
                strSql3.Append("ID,UserName,UpLoadTime,Department,Price,PublicCloud,FileSize,QRST_CODE,DownloadCount,AverageScore,DataID)");
                strSql3.Append(" values (");
                strSql3.Append("@ID,@UserName,@UpLoadTime,@Department,@Price,@PublicCloud,@FileSize,@QRST_CODE,@DownloadCount,@AverageScore,@DataID)");
                MySqlParameter[] parameters3 = {
                    new MySqlParameter("@ID", MySqlDbType.Int32,7),
                    new MySqlParameter("@UserName", MySqlDbType.VarChar,200),
                    new MySqlParameter("@UpLoadTime", MySqlDbType.DateTime),
                    new MySqlParameter("@Department", MySqlDbType.Text),
                    new MySqlParameter("@Price", MySqlDbType.Text),
                    new MySqlParameter("@PublicCloud", MySqlDbType.Text),
                    new MySqlParameter("@FileSize", MySqlDbType.Double,9),
                    new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45),
                    new MySqlParameter("@DownloadCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@AverageScore", MySqlDbType.Double,10),
                    new MySqlParameter("@DataID", MySqlDbType.Text)};
                parameters3[0].Value = ID;
                parameters3[1].Value = UserName;
                parameters3[2].Value = UpLoadTime;
                parameters3[3].Value = Department;
                parameters3[4].Value = Price;
                parameters3[5].Value = PublicCloud;
                parameters3[6].Value = FileSize;
                parameters3[7].Value = QRST_CODE;
                parameters3[8].Value = DownloadCount;
                parameters3[9].Value = AverageScore;
                parameters3[10].Value = DataID;
                isdbUtility.ExecuteSql(strSql3.ToString(), parameters3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
