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
    public class MetaDataMouldDoc : MetaData
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataNormalFile));
        static string _tablename = "mould_doc";
        static string _groupcode = "";
        public static string GetDefaultGroupCode(IDbBaseUtilities isdbUtil)
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
                string sql = string.Format("select metadatacatalognode.Group_Code from metadatacatalognode left join tablecode on metadatacatalognode.Data_Code = tablecode.QRST_CODE where tablecode.Table_Name = '{0}';", _tablename);
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
            	表mould_doc
		mould_doc.ID,
mould_doc.TITLE AS `文档名称`,
mould_doc.DOCTYPE AS `文档类型`,
mould_doc.KEYWORD AS `关键词`,
mould_doc.ABSTRACT AS `摘要`,
mould_doc.DOCDATE AS `文档日期`,
mould_doc.DESCRIPTION AS `备注说明`,
mould_doc.AUTHOR AS `作者`,
mould_doc.UPLOADER AS `上传者`,
mould_doc.UPLOADTIME AS `上传日期`,
mould_doc.FILESIZE AS `文档大小`,
mould_doc.QRST_CODE AS `数据编码`
		        
             */
        public string Name
        {get{return TITLE;}}
        public string TITLE;
        public string DOCTYPE;
        public string KEYWORD;
        public string ABSTRACT;
        public DateTime DOCDATE;
        public string DESCRIPTION;
        public string AUTHOR;
        public string UPLOADER;
        public DateTime UPLOADTIME;
        public long FILESIZE;
        public string QRST_CODE;
        #endregion

        #region Method

        public MetaDataMouldDoc()
        {
            _dataType = EnumMetadataTypes.MouldDoc;
        }

        public override string GetRelateDataPath()
        {
            //\\192.168.2.109\zhsjk\信息服务库\[分支]\NAME_QRSTCODE.gnf
            if (!string.IsNullOrEmpty(this.Name) && nodeLst != null)
            {
                string relatePath = "";
                for (int i = 0; i < nodeLst.Count; i++)
                {
                    relatePath = string.Format(@"{0}\{1}", nodeLst[i].NAME, relatePath);
                }
                relatePath = string.Format(@"{0}{1}#{2}", relatePath,this.TITLE,this.QRST_CODE);
                //relatePath = relatePath.TrimEnd(@"\".ToCharArray());
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
            string querySql = string.Format("select * from {1} where QRST_CODE = '{0}'", qrst_code, _tablename);
            System.Data.DataSet ds = sqlBase.GetDataSet(querySql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.TITLE = ds.Tables[0].Rows[0]["TITLE"].ToString();
                this.QRST_CODE = qrst_code;

                metadatacatalognode_Mdl nodeMdl = new metadatacatalognode_Mdl();
                nodeMdl.GROUP_CODE = MetaDataMouldDoc.GetDefaultGroupCode(sqlBase);
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
            this.TITLE = Path.GetFileNameWithoutExtension(fname);
            this.DOCDATE= fi.LastWriteTime;
            this.FILESIZE=fi.Length;
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
            string sql = string.Format("insert into {0}(TITLE,DOCTYPE,KEYWORD,ABSTRACT,DOCDATE,DESCRIPTION,AUTHOR,UPLOADER,UPLOADTIME,FILESIZE,QRST_CODE) values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}')",_tablename, TITLE, DOCTYPE, KEYWORD, ABSTRACT, DOCDATE, DESCRIPTION, AUTHOR, UPLOADER, UPLOADTIME, FILESIZE, QRST_CODE);
            isdbUtil.ExecuteSql(sql);
            Constant.IdbOperating.UnlockTable(_tablename,EnumDBType.MIDB);

            log.Info("元数据信息导入完成!");

        }

        #endregion
    }
}
