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
    //zsm 2016-10-8
    public class MetaDataDoc:MetaData
    {
        public string[] DocValues;
        public MetaDataPublicInfo publicInfo;  //公共属性值
        public MetaDataDoc()
        {
           DocValues = new string[DocAttributes.Length];
        }
        public MetaDataDoc(string _name, string _qrst_code)
        {
            DocValues = new string[DocAttributes.Length];
            //TITLE = _name;
            QRST_CODE = _qrst_code;
        }
        public string[] DocAttributes = new string[]
        {
            "TITLE","DOCTYPE","KEYWORD",
            "ABSTRACT","DOCDATE","DESCRIPTION",
            "AUTHOR","UPLOADER","UPLOADTIME","FILESIZE"
        };

        public string TITLE
        {
            set { DocValues[0] = value; }
            get { return DocValues[0]; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOCTYPE
        {
            set { DocValues[1] = value; }
            get { return DocValues[1]; }
        }
        public string KEYWORD
        {
            set { DocValues[2] = value; }
            get { return DocValues[2]; }
        }
        public string ABSTRACT
        {
            set { DocValues[3] = value; }
            get { return DocValues[3]; }
        }
        public string DOCDATE
        {
            set { DocValues[4] = value; }
            get { return DocValues[4]; }
        }
        //public DateTime? DOCDATE
        //{
        //    set { DocValues[4] = value.ToString(); }
        //    get
        //    {
        //        try
        //        {
        //            return DateTime.Parse(DocValues[4]);
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}
        public string DESCRIPTION
        {
            set { DocValues[5] = value; }
            get { return DocValues[5]; }
        }
        public string AUTHOR
        {
            set { DocValues[6] = value; }
            get { return DocValues[6]; }
        }
        public string UPLOADER
        {
            set { DocValues[7] = value; }
            get { return DocValues[7]; }
        }

        public string UPLOADTIME
        {
            set { DocValues[8] = value; }
            get { return DocValues[8]; }
        }

        //public DateTime? UPLOADTIME
        //{
        //    set { DocValues[8] = value.ToString(); }
        //    get
        //    {
        //        try
        //        {
        //            return DateTime.Parse(DocValues[8]);
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}
      

        //public int FILESIZE
        //{
        //    set { DocValues[9] = value.ToString(); }
        //    get
        //    {
        //        try
        //        {
        //            return int.Parse(DocValues[9]);
        //        }
        //        catch (Exception ex)
        //        {
        //            return -1;
        //        }
        //    }
        //}
        public string FILESIZE
        {
            set { DocValues[9] = value; }
            get { return DocValues[9]; }
        }
        public string QRST_CODE
        {
            set;
            get;
        }
        public int ID
        {
            set;
            get;
        }

        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            //string strSql = string.Format("select ID,TITLE,DOCTYPE,KEYWORD,ABSTRACT,DOCDATE,DESCRIPTION,AUTHOR,UPLOADER,UPLOADTIME,FILESIZE from mould_doc where QRST_CODE = {0}; ", qrst_code);
            //DataSet ds=sqlBase.GetDataSet(strSql);
            //DataRow dr=ds.t
            QRST_CODE = qrst_code;
            //ID
            //TITLE
            //DOCTYPE
            //    KEYWORD
            //ABSTRACT
            //    DOCDATE
            //DESCRIPTION
            //    AUTHOR
            //UPLOADER
            //    UPLOADTIME=UPLOADTIME
            //FILESIZE=row[FILESIZE];

            base.GetModel(qrst_code, sqlBase);
        }

        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            try
            {
                //导入基本信息
                //TableLocker dblock = new TableLocker(sqlBase);
                Constant.IdbOperating.LockTable("mould_doc",EnumDBType.MIDB);
                tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
                ID = sqlBase.GetMaxID("ID", "mould_doc");
                QRST_CODE = tablecode.GetDataQRSTCode("mould_doc", ID);

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into mould_doc(");
                strSql.Append("ID,TITLE,DOCTYPE,KEYWORD,ABSTRACT,DOCDATE,DESCRIPTION,AUTHOR,UPLOADER,UPLOADTIME,FILESIZE,QRST_CODE)");
                strSql.Append(" values (");
                strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}')",
                    ID, TITLE, DOCTYPE, KEYWORD, ABSTRACT, DOCDATE, DESCRIPTION, AUTHOR, UPLOADER, UPLOADTIME, FILESIZE,
                    QRST_CODE));
                //           strSql.Append("@ID,@TITLE,@DOCTYPE,@KEYWORD,@ABSTRACT,@DOCDATE,@DESCRIPTION,@AUTHOR,@UPLOADER,@UPLOADTIME,@FILESIZE,@QRST_CODE)");
                //           MySqlParameter[] parameters = {
                //new MySqlParameter("@ID", MySqlDbType.Int32,9),
                //new MySqlParameter("@TITLE", MySqlDbType.VarChar,200),
                //new MySqlParameter("@DOCTYPE", MySqlDbType.VarChar,200),
                //new MySqlParameter("@KEYWORD",  MySqlDbType.Text),
                //new MySqlParameter("@ABSTRACT", MySqlDbType.Text),
                //new MySqlParameter("@DOCDATE", MySqlDbType.DateTime),
                //new MySqlParameter("@DESCRIPTION", MySqlDbType.Text),
                //new MySqlParameter("@AUTHOR", MySqlDbType.VarChar,20),
                //new MySqlParameter("@UPLOADER", MySqlDbType.VarChar,200),
                //new MySqlParameter("@UPLOADTIME", MySqlDbType.DateTime),
                //new MySqlParameter("@FILESIZE", MySqlDbType.Int32,22),
                //               new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,45)};
                //           parameters[0].Value = ID;
                //           parameters[1].Value = TITLE;
                //           parameters[2].Value = DOCTYPE;
                //           parameters[3].Value = KEYWORD;
                //           parameters[4].Value = ABSTRACT;
                //           parameters[5].Value = Convert.ToDateTime(DOCDATE);
                //           parameters[6].Value = DESCRIPTION;
                //           parameters[7].Value = AUTHOR;
                //           parameters[8].Value = UPLOADER;
                //           parameters[9].Value = Convert.ToDateTime(UPLOADTIME);
                //           //parameters[10].Value = FILESIZE;
                //           parameters[10].Value = Convert.ToInt32(FILESIZE);
                //           parameters[11].Value = QRST_CODE;
                sqlBase.ExecuteSql(strSql.ToString());
                Constant.IdbOperating.UnlockTable("mould_doc",EnumDBType.MIDB);

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
                //读取一般信息
                for (int i = 0; i < DocValues.Length; i++)
                {
                    node = root.GetElementsByTagName(DocValues[i]).Item(0);
                    if (node == null)
                    {
                        DocValues[i] = "";
                    }
                    else
                    {
                        DocValues[i] = node.InnerText;
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
        public override string GetRelateDataPath()//这有问题
        {
            return string.Format(@"信息服务库\文档成果\{0}", QRST_CODE);    
        }

        /// <summary>
        /// 读取CBERS和HJ数据XML中属性
        /// </summary>
        /// <param name="fileName">XML文件名</param>
        /// <param name="otherParameters">其它属性值,如xml中没有的“文件路径”等</param>
        /// <param name="output_attribute_string">输出属性值</param>
        public void readCbersHjAttribute(string fileName, string[] otherParameters, out string[] outputAttributeValues)
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

            string[] attributeValues = new string[DocValues.Length];
            for (int i = 0; i < DocValues.Length; i++)
                if (i < DocValues.Length - otherParameters.Length)
                {
                    node = root.GetElementsByTagName(DocValues[i]).Item(0);
                    if (node == null)
                    {
                        attributeValues[i] = "无";
                    }
                    else
                    {
                        attributeValues[i] = node.InnerText;
                    }
                }
                else
                    attributeValues[i] = otherParameters[i - DocValues.Length + otherParameters.Length];
            outputAttributeValues = attributeValues;
        }
    }
}
