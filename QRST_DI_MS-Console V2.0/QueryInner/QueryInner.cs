using System.Collections.Generic;
using QRST_DI_MS_Basis.QueryBase;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Metadata.MetaDataDefiner;
 
namespace QRST_DI_MS_Desktop.QueryInner
{
    public class QueryInner
    {
        public List<DataTypeClass> listDataType = new List<DataTypeClass>();

        /// <summary>
        /// 数据库名称列表
        /// </summary>
        public static List<string> listsubDBName = new List<string>();

        //public static List<metadatacatalognode_Mdl> listDataType;

        public QueryInner()
        {

            //查询数据库  填充listDataType  调用MS-Basis中的QueryConnDB中的方法
            //listDataType = new List<metadatacatalognode_Mdl>();
            GetAllList();
        }
        /// <summary>
        /// 填充数据类型列表
        /// </summary>
        private void GetAllList()
        {
            foreach (SiteDb subDB in TheUniversal.subDbLst)
            {
                //SiteDb subDB = TheUniversal.subDbLst[i];

                if (subDB.metadatacatalognode!=null)
                {
                    List<metadatacatalognode_Mdl> listMetadataMdl = subDB.metadatacatalognode.GetCatalogGroup(string.Format(" NAME = '{0}'", subDB.DESCRIPTION));

                    if (subDB.metadatacatalognode_r!=null)
                    {
                        List<string> Childs=subDB.metadatacatalognode_r.GetGroupChild(listMetadataMdl[0].GROUP_CODE);
                        if (Childs.Count>0)
                        {
                            for (int i=0;i<Childs.Count;i++)
                            {
                                List<metadatacatalognode_Mdl> listChildsMetadataMdl = subDB.metadatacatalognode.GetCatalogGroup(string.Format("GROUP_CODE = '{0}'", Childs[i]));

                                if (listChildsMetadataMdl.Count<=0)
                                {
                                    break;
                                }
                                DataTypeClass dtCls = new DataTypeClass();
                                dtCls.HostDBCode = subDB.NAME;
                                dtCls.CatagoryName = listChildsMetadataMdl[0].NAME;
                                dtCls.CatagoryCode = listChildsMetadataMdl[0].GROUP_CODE;

                                switch (listChildsMetadataMdl[0].GROUP_TYPE)
                                {
                                    case "system_Vector":
                                        dtCls.DataFileType = FileTypeCategory.Vectors;
                                        break;
                                    case "system_Raster":
                                        dtCls.DataFileType = FileTypeCategory.Rasters;
                                        break;
                                    case "system_Doc":
                                        dtCls.DataFileType = FileTypeCategory.Documents;
                                        break;
                                    case "system_DataSet":
                                        dtCls.DataFileType = FileTypeCategory.Sheets;
                                        break;
                                    default:
                                        dtCls.DataFileType = FileTypeCategory.NotDefine;
                                        break;
                                }
                                listDataType.Add(dtCls);
                            }
                        }
                        
                    }

                }
                
            }
        }

        /// <summary>
        /// 根据输入的DataTypeItem的编码获得中文名称
        /// </summary>
        /// <param name="inCode"> 输入的编码</param>
        /// <returns></returns>
        public  string GetCatagoryNameByCode(string inCode)
        {
            string iName = string.Empty;
            if (listDataType.Count > 0)
            {
                foreach (DataTypeClass dtCls in listDataType)
                {
                    if (dtCls.CatagoryCode == inCode)
                    {
                        iName = dtCls.CatagoryName;
                        break;
                    }
                }
            }
            return iName;
        }
        /// <summary>
        /// 根据输入的DataTypeItem的中文名称获得编码
        /// </summary>
        /// <param name="inName">输入的中文名称</param>
        /// <returns></returns>
        public  string GetCatagoryCodeByName(string inName)
        {
            string iCode = string.Empty;
            if (listDataType.Count>0)
            {
                foreach (DataTypeClass dtCls in listDataType)
                {
                    if (dtCls.CatagoryName==inName)
                    {
                        iCode = dtCls.CatagoryCode;
                        break;
                    }
                }
            }
            return iCode;
        }
        /// <summary>
        /// 根据输入的数据库类型名称获得类型的文件分类
        /// </summary>
        /// <param name="inName"></param>
        /// <returns></returns>
        public FileTypeCategory GetFileTypeByName(string inName)
        {
            FileTypeCategory fileType = FileTypeCategory.NotDefine;
            if (listDataType.Count > 0)
            {
                foreach (DataTypeClass dtCls in listDataType)
                {
                    if (dtCls.CatagoryName == inName)
                    {
                        fileType = dtCls.DataFileType;
                        break;
                    }
                }
            }
            return fileType;
        }

        /// <summary>
        /// 根据输入的数据库类型编码获得类型的文件分类
        /// </summary>
        /// <param name="inName"></param>
        /// <returns></returns>
        public FileTypeCategory GetFileTypeByCode(string inCode)
        {
            FileTypeCategory fileType = FileTypeCategory.NotDefine;
            if (listDataType.Count > 0)
            {
                foreach (DataTypeClass dtCls in listDataType)
                {
                    if (dtCls.CatagoryCode == inCode)
                    {
                        fileType = dtCls.DataFileType;
                        break;
                    }
                }
            }
            return fileType;
        }

        /// <summary>
        /// 根据输入的数据库类型名称获得类型所属数据库
        /// </summary>
        /// <param name="inName"></param>
        /// <returns></returns>
        public string GetDBCodeByName(string inName)
        {
            string DBCode = string.Empty;
            if (listDataType.Count > 0)
            {
                foreach (DataTypeClass dtCls in listDataType)
                {
                    if (dtCls.CatagoryName == inName)
                    {
                        DBCode = dtCls.HostDBCode;
                        break;
                    }
                }
            }
            return DBCode;
        }

        /// <summary>
        /// 根据输入的数据库类型编码获得类型所属数据库
        /// </summary>
        /// <param name="inName"></param>
        /// <returns></returns>
        public string GetDBCodeByCode(string inCode)
        {
            string DBCode = string.Empty;
            if (listDataType.Count > 0)
            {
                foreach (DataTypeClass dtCls in listDataType)
                {
                    if (dtCls.CatagoryCode == inCode)
                    {
                        DBCode = dtCls.HostDBCode;
                        break;
                    }
                }
            }
            return DBCode;
        }

        

    }
}
