using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using log4net;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataImageProd : MetaData
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataImageProd));
        static string _tablename = "imageprod";
        static string _groupcode = "";
        public static string GetDefaultGroupCode(IDbBaseUtilities sqlBase)
        {
            if (_groupcode!="")
            {
                return _groupcode;
            }
            else
            {
                string sql = string.Format("select metadatacatalognode.Group_Code from metadatacatalognode left join tablecode on metadatacatalognode.Data_Code = tablecode.QRST_CODE where tablecode.Table_Name = '{0}';", _tablename);
                System.Data.DataSet ds=sqlBase.GetDataSet(sql);
                try
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                catch { }
                return "";
            }
        }

        public string[] vectorAttributeNames ={
                                "Name",           //
                                "ProdType",          //
                                "SourceDataName",             //
                                "ProducedDate",               //
                                "Produsor",          //
                                "UploadDate",              //             
                                "Size",         //  
                                "Remark",            //
                                "GroupCode",
                                "UploadUser"
                            };
        public string[] vectorAttributeValues;
        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties
        public List<metadatacatalognode_Mdl> nodeLst;

        private string name;
        //
        public string Name
        {
            get { return vectorAttributeValues[0]; }
            set { vectorAttributeValues[0] = value; }
        }

        private string prodType;
        //
        public string ProdType
        {
            get { return vectorAttributeValues[1]; }
            set { vectorAttributeValues[1] = value; }
        }

        private string sourceDataName;
        //
        public string SourceDataName
        {
            get { return vectorAttributeValues[2]; }
            set { vectorAttributeValues[2] = value; }
        }
        private DateTime producedDate;
        //
        public DateTime ProducedDate
        {
            get {
                DateTime dt= Convert.ToDateTime(vectorAttributeValues[3], CultureInfo.CurrentCulture);
                return dt;
            }
            set { vectorAttributeValues[3] = value.ToString(); }
        }

        private string produsor;
        //
        public string Produsor
        {
            get { return vectorAttributeValues[4]; }
            set { vectorAttributeValues[4] = value; }
        }

        private DateTime uploadDate;
        //
        public DateTime UploadDate
        {
            get {
                DateTime dt= Convert.ToDateTime(vectorAttributeValues[5], CultureInfo.CurrentCulture);
                return dt;
            }
            set { vectorAttributeValues[5] = value.ToString(); }
        }

        private double size;
        //
        public double Size
        {
            get { return double.Parse(vectorAttributeValues[6]); }
            set { vectorAttributeValues[6] = value.ToString(); }
        }

        private string remark;
        //
        public string Remark
        {
            get { return vectorAttributeValues[7]; }
            set { vectorAttributeValues[7] = value; }
        }

        public string GroupCode
        {
            get { return vectorAttributeValues[8]; }
            set { vectorAttributeValues[8] = value; }
        }

        public string UploadUser
        {
            get { return vectorAttributeValues[9]; }
            set { vectorAttributeValues[9] = value; }
        }
        #endregion

        #region Method

        public MetaDataImageProd()
        {
            _dataType = EnumMetadataTypes.ImageProd;
            vectorAttributeValues = new string[vectorAttributeNames.Length];
        }

        public override string GetRelateDataPath()
        {
            //\\192.168.2.109\zhsjk\信息产品库\[分支]\NAME_QRSTCODE\
            if (!string.IsNullOrEmpty(this.Name) && nodeLst != null)
            {
                string relatePath = "";
                for (int i = 0; i < nodeLst.Count; i++)
                {
                    relatePath = string.Format(@"{0}\{1}", nodeLst[i].NAME, relatePath);
                }
                relatePath = string.Format(@"{0}{1}#{2}", relatePath, this.Name,this.QRST_CODE);
                relatePath = relatePath.TrimEnd(@"\".ToCharArray());
                relatePath = relatePath.Replace(":", "");
                return relatePath;
            }
            else
                return null;
        }

        /// <summary>
        /// 获取元数据实体，必须元数据入库后才有元数据实体
        /// </summary>
        /// <param name="qrst_code">库表中的数据QRST_CODE</param>
        /// <param name="sqlBase">数据库链接</param>
        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            string querySql = string.Format("select * from imageprod where QRST_CODE = '{0}'", qrst_code);
            System.Data.DataSet ds = sqlBase.GetDataSet(querySql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                this.QRST_CODE = qrst_code;

                metadatacatalognode_Mdl nodeMdl = new metadatacatalognode_Mdl();
                nodeMdl.GROUP_CODE = ds.Tables[0].Rows[0]["GROUPCODE"].ToString();
                metadatacatalognode_r_Dal nodeDal = new metadatacatalognode_r_Dal(sqlBase);
                nodeLst = nodeDal.GetParent(nodeMdl);

                this.IsCreated = true;
            }
        }

        public override void ReadAttributes(string fileName)
        {
            base.ReadAttributes(fileName);
            DirectoryInfo di = new DirectoryInfo(fileName);

            //this.Name=Path.GetFileNameWithoutExtension(fileName);
            this.Name = di.Name;
            if (this.Name.Contains("#"))
            {
                string[] cont = this.Name.Split('#');
                try
                {
                    this.SourceDataName = cont[0]+".tar.gz";
                }
                catch { }
                try
                {
                    this.ProdType = cont[1];
                }
                catch { }
            }
            this.ProducedDate = di.LastAccessTime;
            this.Size = GetDirSize(di);

        }

        private double GetDirSize( DirectoryInfo di)
        {
            double dirsize = 0;
            FileInfo[] fis = di.GetFiles();
            foreach (FileInfo fi in fis)
            {
                dirsize += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            foreach (DirectoryInfo cdi in dis)
            {
                dirsize += GetDirSize(cdi);
            }
            return dirsize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlBase">Universial.dbOperating.INDB</param>
        public override void ImportData(IDbBaseUtilities sqlBase)
        {
            //TableLocker dblock = new TableLocker(sqlBase);
            Constant.IdbOperating.LockTable("imageprod",EnumDBType.MIDB);
            tablecode_Dal tablecode = new tablecode_Dal(sqlBase);
            int id = sqlBase.GetMaxID("ID", "imageprod");
            QRST_CODE = tablecode.GetDataQRSTCode("imageprod", id);
            string uploadDate = UploadDate.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into imageprod(");
            strSql.Append("Name,ProdType,SourceDataName,ProducedDate,Produsor,UploadDate,Size,Remark,GROUPCODE,QRST_CODE,UploadUser)");
            strSql.Append(" values (");
            strSql.Append(string.Format("'{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}')",
                Name, ProdType, SourceDataName, ProducedDate.ToString("yyyy-MM-dd HH:mm:ss"), Produsor, uploadDate, Size, Remark, GroupCode, QRST_CODE,
                UploadUser));
            //       strSql.Append("@Name,@ProdType,@SourceDataName,@ProducedDate,@Produsor,@UploadDate,@Size,@Remark,@GROUPCODE,@QRST_CODE,@UploadUser)");
            //       MySqlParameter[] parameters = {
            //new MySqlParameter("@Name", MySqlDbType.VarChar,50),
            //new MySqlParameter("@ProdType", MySqlDbType.VarChar,50),
            //new MySqlParameter("@SourceDataName", MySqlDbType.VarChar,50),
            //new MySqlParameter("@ProducedDate", MySqlDbType.DateTime),
            //new MySqlParameter("@Produsor", MySqlDbType.VarChar,35),
            //new MySqlParameter("@UploadDate", MySqlDbType.DateTime),
            //new MySqlParameter("@Size", MySqlDbType.Double),
            //new MySqlParameter("@Remark", MySqlDbType.Text),
            //new MySqlParameter("@GROUPCODE", MySqlDbType.VarChar,100),
            //new MySqlParameter("@QRST_CODE", MySqlDbType.VarChar,100),
            //new MySqlParameter("@UploadUser", MySqlDbType.VarChar,35)};
            //       parameters[0].Value = this.Name;
            //       parameters[1].Value = this.ProdType;
            //       parameters[2].Value = this.SourceDataName;
            //       parameters[3].Value = this.ProducedDate;
            //       parameters[4].Value = this.Produsor;
            //       parameters[5].Value = this.UploadDate;
            //       parameters[6].Value = this.Size;
            //       parameters[7].Value = this.Remark;
            //       parameters[8].Value = this.GroupCode;
            //       parameters[9].Value = this.QRST_CODE;
            //       parameters[10].Value = this.UploadUser;

            sqlBase.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("imageprod",EnumDBType.MIDB);
            
            log.Info("元数据信息导入完成!");

        }

        #endregion
    }
}
