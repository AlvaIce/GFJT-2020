using System;
using System.Collections.Generic;
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
    public class MetaDataNormalFile : MetaData
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataNormalFile));
        public string _tablename = "prod_NormalFiles";
        static string _groupcode = "";
        /// <summary>
        /// 无用
        /// </summary>
        /// <param name="isdbUtil"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static string GetDefaultGroupCode(IDbBaseUtilities isdbUtil,string tablename)
        {
            if (_groupcode != "")
            {
                return _groupcode;
            }
            else
            {
                if (isdbUtil == null)
                {
                    return "";
                }
                string sql = string.Format("select metadatacatalognode.Group_Code from metadatacatalognode left join tablecode on metadatacatalognode.Data_Code = tablecode.QRST_CODE where tablecode.Table_Name = '{0}';", tablename);
                System.Data.DataSet ds = isdbUtil.GetDataSet(sql);
                try
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                catch { }
                return "";
            }
        }


        /// <summary>
        /// 属性字段名
        /// </summary>

        #region Properties
        public List<metadatacatalognode_Mdl> nodeLst;

        /*
            	表NormalFiles
		文件名 Name
		原文件名Filename
		文件大小 Size
		创建日期 CreateDatetime
		修改日期 LastModifyDatetime
		创建者UploadUser
		上传日期 UploadDate
		备注Remark
		GroupCode
		QRST_CODE
		        
             */
        public string name;
        public string filename;
        public long size;
        public DateTime createDatetime;
        public DateTime lastmodifydatetime;
        public string uploaduser;
        public DateTime uploaddate;
        public string remark;
        public string groupcode;
        public string QRST_CODE;
        #endregion

        #region Method

        public MetaDataNormalFile(string tablename = "prod_NormalFiles")
        {
            _tablename = tablename;
            _dataType = EnumMetadataTypes.NormalFile;
        }

        public override string GetRelateDataPath()
        {
            //\\192.168.2.109\zhsjk\信息服务库\[分支]\NAME_QRSTCODE.gnf
            if (!string.IsNullOrEmpty(this.name) && nodeLst != null)
            {
                string relatePath = "";
                for (int i = 0; i < nodeLst.Count; i++)
                {
                    relatePath = string.Format(@"{0}\{1}", nodeLst[i].NAME, relatePath);
                }
                relatePath = string.Format(@"{0}{1}#{2}.gnf", relatePath, this.name, this.QRST_CODE);
                relatePath = relatePath.TrimEnd(@"\".ToCharArray());
                relatePath = relatePath.Replace(":", "");
                return relatePath;
            }
            else
                return null;
        }

        public override string GetRelateDataPath(out string originalFilename)
        {
            originalFilename = this.filename;
            return GetRelateDataPath();
        }

        /// <summary>
        /// 获取元数据实体，必须元数据入库后才有元数据实体
        /// </summary>
        /// <param name="qrst_code">库表中的数据QRST_CODE</param>
        /// <param name="sqlBase">数据库链接</param>
        public override void GetModel(string qrst_code, IDbBaseUtilities sqlBase)
        {
            string querySql = string.Format("select * from {1} where QRST_CODE = '{0}'", qrst_code, _tablename);
            System.Data.DataSet ds = sqlBase.GetDataSet(querySql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.name = ds.Tables[0].Rows[0]["NAME"].ToString();
                this.QRST_CODE = qrst_code;
                this.filename=ds.Tables[0].Rows[0]["filename"].ToString();
                this.size=Convert.ToInt32(ds.Tables[0].Rows[0]["size"]);
               DateTime dt =Convert.ToDateTime(ds.Tables[0].Rows[0]["createDatetime"]);
                this.createDatetime = dt;
                 dt=Convert.ToDateTime(ds.Tables[0].Rows[0]["lastmodifydatetime"]);;
                this.lastmodifydatetime = dt;
                this.uploaduser=ds.Tables[0].Rows[0]["uploaduser"].ToString();
                dt =Convert.ToDateTime(ds.Tables[0].Rows[0]["uploaddate"]);
                this.uploaddate = dt;
                this.remark=ds.Tables[0].Rows[0]["remark"].ToString();
                metadatacatalognode_Mdl nodeMdl = new metadatacatalognode_Mdl();
                nodeMdl.GROUP_CODE = ds.Tables[0].Rows[0]["GROUPCODE"].ToString();
                metadatacatalognode_r_Dal nodeDal = new metadatacatalognode_r_Dal(sqlBase);
                nodeLst = nodeDal.GetParent(nodeMdl);

                this.IsCreated = true;
            }
        }

        public override void ReadAttributes(string fname)
        {
            base.ReadAttributes(fname);

            if (!File.Exists(fname))
            {
                return;
            }
            FileInfo fi = new FileInfo(fname);
             name = Path.GetFileNameWithoutExtension(fname);
             filename = Path.GetFileName(fname);
             size = fi.Length;
             createDatetime = fi.CreationTime;
             lastmodifydatetime = fi.LastWriteTime;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlBase">Universial.dbOperating.INDB</param>
        public override void ImportData(IDbBaseUtilities isdbUtil)
        {
            //TableLocker dblock = new TableLocker(isdbUtil);
            Constant.IdbOperating.LockTable(_tablename,EnumDBType.MIDB);
            tablecode_Dal tablecode = new tablecode_Dal(isdbUtil);
            int id = isdbUtil.GetMaxID("ID", _tablename);
            QRST_CODE = tablecode.GetDataQRSTCode(_tablename, id);
            string sql = string.Format("insert into {0}(name,filename,size,createDatetime,lastmodifydatetime,uploaduser,uploaddate,remark,GROUPCODE,QRST_CODE) values ('{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}')", _tablename, name, filename, size, createDatetime.ToString("yyyy-MM-dd HH:mm:ss"), lastmodifydatetime.ToString("yyyy-MM-dd HH:mm:ss"), uploaduser, uploaddate.ToString("yyyy-MM-dd HH:mm:ss"), remark, groupcode, QRST_CODE);
            isdbUtil.ExecuteSql(sql);
            Constant.IdbOperating.UnlockTable(_tablename, EnumDBType.MIDB);

            log.Info("元数据信息导入完成!");

        }

        #endregion
    }
}
