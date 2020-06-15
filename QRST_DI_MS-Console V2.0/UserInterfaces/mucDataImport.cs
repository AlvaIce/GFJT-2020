using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using System.IO;
using QRST_DI_Resources;
using System.Threading.Tasks;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucDataImport : DevExpress.XtraEditors.XtraUserControl
    {
        public mucDataImport()
        {
            InitializeComponent();
            _isCreated = false;
        }

        public bool _isCreated;
        public void Create()
        {
            ctrlVectorDataImport1.Create(TheUniversal.BSDB.sqlUtilities, TheUniversal.currentUser);
            ctrlRasterProdImportor1.Create(TheUniversal.INDB.sqlUtilities, TheUniversal.currentUser);
            ctrlNormalFileImportor1.Create(TheUniversal.ISDB.sqlUtilities, TheUniversal.currentUser);
            ctrlMouldDocImportor1.Create(TheUniversal.ISDB.sqlUtilities, TheUniversal.currentUser);
            ucImportTarGZ1.Create(TheUniversal.EVDB.sqlUtilities, TheUniversal.currentUser);
            _isCreated = true;
        }

        public void SetVirtualDirPath(string typestr, string path, string target)
        {
            switch (typestr)
            {
                case "原始数据":
                    ucImportTarGZ1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "非规影像级产品":
                    ctrlRasterProdImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "规格化数据":
                    ucTileImport1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "矢量数据":
                    ctrlVectorDataImport1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "文档资料":
                    ctrlMouldDocImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "一般文件":
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "专题产品发布":

                    break;
                case "实测数据":
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "示范区DEM":
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "专题产品":
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "算法模型":
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                case "系统部署包":
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path, target);
                    break;
                default:
                    break;
            }
        }

        public void Create_NormalFileImportor()
        {
            ctrlNormalFileImportor1.Create(TheUniversal.ISDB.sqlUtilities, TheUniversal.currentUser);
        }

        public void Create_XJDemImportor()
        {
            IDbBaseUtilities dbutil = TheUniversal.EVDB.sqlUtilities;
            dbutil = TheUniversal.BSDB.sqlUtilities;
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile' and DATA_CODE = 'BSDB-XJ02';";
            System.Data.DataSet ds = dbutil.GetDataSet(sql);

            tablecode_Dal tablecode_dal = new tablecode_Dal(dbutil);
            DataRow dr = ds.Tables[0].Rows[0];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjdemDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjdemDt.tablecapital = dr["NAME"].ToString();
            xjdemDt.tablecode = dr["DATA_CODE"].ToString();
            xjdemDt.groupcode = dr["GROUP_CODE"].ToString();
            xjdemDt.tablename = tablecode_dal.GetTableName(xjdemDt.tablecode);

            ctrlNormalFileImportor1.Create(dbutil, TheUniversal.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjdemDt });
        }

        public void Create_XJBopuImportor()
        {
            IDbBaseUtilities dbutil = TheUniversal.EVDB.sqlUtilities;
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile' and (DATA_CODE = 'EVDB-XJ02' or DATA_CODE = 'EVDB-XJ03');";
            System.Data.DataSet ds = dbutil.GetDataSet(sql);

            tablecode_Dal tablecode_dal = new tablecode_Dal(dbutil);
            DataRow dr = ds.Tables[0].Rows[0];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjsdclDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjsdclDt.tablecapital = dr["NAME"].ToString();
            xjsdclDt.tablecode = dr["DATA_CODE"].ToString();
            xjsdclDt.groupcode = dr["GROUP_CODE"].ToString();
            xjsdclDt.tablename = tablecode_dal.GetTableName(xjsdclDt.tablecode);

             dr = ds.Tables[0].Rows[1];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjxdyzDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjxdyzDt.tablecapital = dr["NAME"].ToString();
            xjxdyzDt.tablecode = dr["DATA_CODE"].ToString();
            xjxdyzDt.groupcode = dr["GROUP_CODE"].ToString();
            xjxdyzDt.tablename = tablecode_dal.GetTableName(xjxdyzDt.tablecode);

            ctrlNormalFileImportor1.Create(dbutil, TheUniversal.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjsdclDt, xjxdyzDt });
        }

        public void Create_XJProdsImportor()
        {
            IDbBaseUtilities dbutil = TheUniversal.EVDB.sqlUtilities;
            dbutil = TheUniversal.INDB.sqlUtilities;
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile' and (DATA_CODE = 'INDB-XJ02' or DATA_CODE = 'INDB-XJ03' or DATA_CODE = 'INDB-XJ04' or DATA_CODE = 'INDB-XJ05' or DATA_CODE = 'INDB-XJ06' or DATA_CODE = 'INDB-XJ07');";
            System.Data.DataSet ds = dbutil.GetDataSet(sql);

            tablecode_Dal tablecode_dal = new tablecode_Dal(dbutil);
            DataRow dr = ds.Tables[0].Rows[0];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjslzylDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjslzylDt.tablecapital = dr["NAME"].ToString();
            xjslzylDt.tablecode = dr["DATA_CODE"].ToString();
            xjslzylDt.groupcode = dr["GROUP_CODE"].ToString();
            xjslzylDt.tablename = tablecode_dal.GetTableName(xjslzylDt.tablecode);

            dr = ds.Tables[0].Rows[1];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjjxDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjjxDt.tablecapital = dr["NAME"].ToString();
            xjjxDt.tablecode = dr["DATA_CODE"].ToString();
            xjjxDt.groupcode = dr["GROUP_CODE"].ToString();
            xjjxDt.tablename = tablecode_dal.GetTableName(xjjxDt.tablecode);


            dr = ds.Tables[0].Rows[2];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj4Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj4Dt.tablecapital = dr["NAME"].ToString();
            xj4Dt.tablecode = dr["DATA_CODE"].ToString();
            xj4Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj4Dt.tablename = tablecode_dal.GetTableName(xj4Dt.tablecode);
            
            dr = ds.Tables[0].Rows[3];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj5Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj5Dt.tablecapital = dr["NAME"].ToString();
            xj5Dt.tablecode = dr["DATA_CODE"].ToString();
            xj5Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj5Dt.tablename = tablecode_dal.GetTableName(xj5Dt.tablecode);
            
            dr = ds.Tables[0].Rows[4];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj6Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj6Dt.tablecapital = dr["NAME"].ToString();
            xj6Dt.tablecode = dr["DATA_CODE"].ToString();
            xj6Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj6Dt.tablename = tablecode_dal.GetTableName(xj6Dt.tablecode);
            
            dr = ds.Tables[0].Rows[5];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj7Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj7Dt.tablecapital = dr["NAME"].ToString();
            xj7Dt.tablecode = dr["DATA_CODE"].ToString();
            xj7Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj7Dt.tablename = tablecode_dal.GetTableName(xj7Dt.tablecode);


            ctrlNormalFileImportor1.Create(dbutil, TheUniversal.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjslzylDt, xjjxDt, xj4Dt, xj5Dt, xj6Dt, xj7Dt });
        }
        public void Create_XJModuleImportor()
        {
            IDbBaseUtilities dbutil = TheUniversal.EVDB.sqlUtilities;
            dbutil = TheUniversal.MADB.sqlUtilities;
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile' and (DATA_CODE = 'MADB-XJ08' or DATA_CODE = 'MADB-XJ03' or DATA_CODE = 'MADB-XJ04' or DATA_CODE = 'MADB-XJ05' or DATA_CODE = 'MADB-XJ06' or DATA_CODE = 'MADB-XJ07');";
            System.Data.DataSet ds = dbutil.GetDataSet(sql);

            tablecode_Dal tablecode_dal = new tablecode_Dal(dbutil);
            DataRow dr = ds.Tables[0].Rows[0];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjslzylDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjslzylDt.tablecapital = dr["NAME"].ToString();
            xjslzylDt.tablecode = dr["DATA_CODE"].ToString();
            xjslzylDt.groupcode = dr["GROUP_CODE"].ToString();
            xjslzylDt.tablename = tablecode_dal.GetTableName(xjslzylDt.tablecode);

            dr = ds.Tables[0].Rows[1];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjjxDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjjxDt.tablecapital = dr["NAME"].ToString();
            xjjxDt.tablecode = dr["DATA_CODE"].ToString();
            xjjxDt.groupcode = dr["GROUP_CODE"].ToString();
            xjjxDt.tablename = tablecode_dal.GetTableName(xjjxDt.tablecode);

            dr = ds.Tables[0].Rows[2];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj4Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj4Dt.tablecapital = dr["NAME"].ToString();
            xj4Dt.tablecode = dr["DATA_CODE"].ToString();
            xj4Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj4Dt.tablename = tablecode_dal.GetTableName(xj4Dt.tablecode);

            dr = ds.Tables[0].Rows[3];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj5Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj5Dt.tablecapital = dr["NAME"].ToString();
            xj5Dt.tablecode = dr["DATA_CODE"].ToString();
            xj5Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj5Dt.tablename = tablecode_dal.GetTableName(xj5Dt.tablecode);

            dr = ds.Tables[0].Rows[4];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj6Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj6Dt.tablecapital = dr["NAME"].ToString();
            xj6Dt.tablecode = dr["DATA_CODE"].ToString();
            xj6Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj6Dt.tablename = tablecode_dal.GetTableName(xj6Dt.tablecode);

            dr = ds.Tables[0].Rows[5];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xj7Dt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xj7Dt.tablecapital = dr["NAME"].ToString();
            xj7Dt.tablecode = dr["DATA_CODE"].ToString();
            xj7Dt.groupcode = dr["GROUP_CODE"].ToString();
            xj7Dt.tablename = tablecode_dal.GetTableName(xj7Dt.tablecode);

            ctrlNormalFileImportor1.Create(dbutil, TheUniversal.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjslzylDt, xjjxDt, xj4Dt, xj5Dt, xj6Dt, xj7Dt });
        }

        public void Create_XJSystemsImportor()
        {
            IDbBaseUtilities dbutil = TheUniversal.EVDB.sqlUtilities;
            dbutil = TheUniversal.MADB.sqlUtilities;
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile' and DATA_CODE = 'MADB-XJ02';";
            System.Data.DataSet ds = dbutil.GetDataSet(sql);

            tablecode_Dal tablecode_dal = new tablecode_Dal(dbutil);
            DataRow dr = ds.Tables[0].Rows[0];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjsystemDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjsystemDt.tablecapital = dr["NAME"].ToString();
            xjsystemDt.tablecode = dr["DATA_CODE"].ToString();
            xjsystemDt.groupcode = dr["GROUP_CODE"].ToString();
            xjsystemDt.tablename = tablecode_dal.GetTableName(xjsystemDt.tablecode);

            ctrlNormalFileImportor1.Create(dbutil, TheUniversal.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjsystemDt });
        }
    }
}
