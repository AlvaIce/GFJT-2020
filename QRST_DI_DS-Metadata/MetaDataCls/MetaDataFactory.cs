using System.Data;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    /// <summary>
    /// 创建元数据工厂类
    /// 用于根据元数据的QRST_CODE(0001-EVDB-16-8),获取元数据对象实例。
    /// 创建者：zxw 
    /// 创建时间：2013/06/23
    /// </summary> 
    public class MetaDataFactory
    {
        static IDbBaseUtilities mySqlBaseUtilities =Constant.IdbServerUtilities;

        public static MetaData CreateMetaDataInstance(string qrst_code)
        {
            //获取子库对象
            string[] strArr = qrst_code.Split('-');
            if (strArr.Length != 4)
            {
                return null;
            }
            string subDBName = strArr[1];    //EVDB
            DataTable subDbArr = mySqlBaseUtilities.GetDataSet(string.Format("select * from subdbinfo where NAME = '{0}' ", subDBName)).Tables[0];
            if (subDbArr.Rows.Count <= 0)
            {
                return null;
            }
            string name = subDbArr.Rows[0]["NAME"].ToString();
            string subDbqrstcode = subDbArr.Rows[0]["QRST_CODE"].ToString();
            string description = subDbArr.Rows[0]["DESCRIPTION"].ToString();
            string relativeStr = subDbArr.Rows[0]["ConnectStr"].ToString();
            string connectStr = Constant.IdbOperating.GetAbsoluteDbCon(relativeStr);
            SiteDb subdb = new SiteDb(name, subDbqrstcode, connectStr, description);
            //获取qrst_code对应数据存在的表名
            int startIndex = qrst_code.IndexOf('-');
            int endIndex = qrst_code.LastIndexOf('-');
            string tableCode = qrst_code.Substring(startIndex + 1, endIndex - startIndex - 1);
            string tableName = subdb.tablecode.GetTableName(tableCode);                   //qrst_code记录所在表
            MetaData metadata;
            switch (tableName.ToLower())
            {
                //实验验证库数据
                case "prod_cbers": metadata = new MetaDataCbers();
                    break;
                case "prod_hj": metadata = new MetaDataHj();
                    break;
                case "prod_modis": metadata = new MetaDataModis();
                    break;
                case "prod_noaa": metadata = new MetaDataNOAA();
                    break;
                case "prod_gf1": metadata = new MetaDataGF1();
                    break;
                case "prod_gf1bcd":metadata = new MetaDataBCD();
                    break;
                case "prod_ahsi_gf5":metadata = new MetaDataAHSIGF5();
                    break;
                case "prod_aius_gf5":metadata = new MetaDataAIUSGF5();
                    break;
                case "prod_dpc_gf5":metadata = new MetaDataDPCGF5();
                    break;
                case "prod_emi_gf5":metadata = new MetaDataEMIGF5();
                    break;
                case "prod_gmi_gf5":metadata = new MetaDataGMIGF5();
                    break;
                case "prod_vims_gf5":metadata = new MetaDataVIMSGF5();
                    break;
                case "prod_gf6":metadata = new MetaDataGF6();
                    break;
                case "prod_gf3": metadata = new MetaDataGF3();
                    break;
                case "prod_zy3": metadata = new MetaDataZY3();
                    break;
                case "prod_zy02c": metadata = new MetaDataZY02C();
                    break;
                case "prod_sj9a":
                    metadata = new MetaDataSJ9A();
                    break;
                case "prod_hj1c":
                    metadata = new MetaDataHJ1C();
                    break;
                //模型算法库数据
                case "madb_algorithmcmp": metadata = new MetaDataStandAlgCmp();
                    break;
                //case "madb_document": metadata = new MetaDataUserProduct();
                //    break;
                case "madb_proworkflow": metadata = new MetaDataStandProWfl();
                    break;
                case "madb_toolkit": metadata = new MetaDataStandardToolkit();
                    break;
                //信息服务库
                case "isdb_document": metadata = new MetaDataUserDocument();
                    break;
                case "isdb_toolkit": metadata = new MetaDataUserToolKit();
                    break;
                case "isdb_useralgorithm": metadata = new MetaAlgorithmCmp();
                    break;
                case "isdb_userraster": metadata = new MetaDataUserRaster();
                    break;
                case "prod_normalfiles": metadata = new MetaDataNormalFile();
                    break;
                case "mould_doc": metadata = new MetaDataMouldDoc();
                    break;
                //信息产品库
                case "ipdb_userproduct": metadata = new MetaDataUserProduct();
                    break;
                case "imageprod": metadata = new MetaDataImageProd();
                    break;
                //基础空间库
                case "prods_vector": metadata = new MetaDataVector();
                    break;
                //专题产品库
                case "prod_airport_vector": metadata = new MetaDataVectorSpecial();
                    break;
                default:
                    string sql = string.Format("SELECT GROUP_TYPE FROM `metadatacatalognode` where DATA_CODE = '{0}';", tableCode);
                    DataSet ds = subdb.sqlUtilities.GetDataSet(sql);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string datatype = ds.Tables[0].Rows[0][0].ToString();
                        switch (datatype)
                        {
                            case "System_Vector":
                                metadata = new MetaDataVectorSpecial();
                                break;
                            case "System_Raster":
                                metadata = new MetaDataRasterSpecial();
                                break;
                            case "System_NormalFile":
                                metadata = new MetaDataNormalFile(tableName);
                                break;
                            case "System_Document":
                                metadata = new MetaDataDocSpecial();
                                break;
                            default:
                                metadata = new MetaDataObject();
                                break;
                        }
                    }
                    else
                    {
                        metadata = new MetaDataObject();
                    }
                    break;
            }
            if (metadata != null)
            {
                metadata.GetModel(qrst_code, subdb.sqlUtilities);
            }
            return metadata;
        }
    }
}
