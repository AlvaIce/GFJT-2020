using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Component.VirtualDirUI
{
    public partial class InputDataForm :DevComponents.DotNetBar.Office2007Form
    {
        public bool _isCreate;
        
        public InputDataForm()
        {
            InitializeComponent();
            _isCreate = false;
        }
        public InputDataForm(int index,string name,string nodePath,string target)
        {
            InitializeComponent();

            this.superTabControl1.Dock = DockStyle.None;
            this.superTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.superTabControl1.Location = new System.Drawing.Point(0, -30);
            this.superTabControl1.Size = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height + 34);

            create();
            tabItemLoadEvent(name, nodePath,target);
            index=index>5?5:index;
            foreach (DevComponents.DotNetBar.SuperTabItem page in this.superTabControl1.Tabs)
            {
                if (page != (superTabControl1.Tabs[index]) as DevComponents.DotNetBar.SuperTabItem)
                    page.Visible = false;
                else this.Text = name+"入库";
            }
            this.superTabControl1.SelectedTabIndex = 0;

        }
        public void tabItemLoadEvent(string name,string path,string target)
        {
            switch (name)
            { 
                case "原始数据":
                    if (!_isCreate) create();
                    ucImportTarGZ2.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
                case "非规影像级产品":
                    if (!_isCreate) create();
                    break;
                case "矢量数据":
                    if (!_isCreate) create();
                    ctrlVectorDataImport1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                     break;
                case "文档资料":
                    if (!_isCreate) create();
                    ctrlMouldDocImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
                case "规格化数据":

                    ucTileImport1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
                case "一般文件":
                 Create_NormalFileImportor();
                 ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                 break;
                case "专题产品发布":
                     MessageBox.Show("专题产品发布正在建设！");
                     ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                     break;
                case "实测数据":
                    Create_XJBopuImportor();
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
                case "示范区DEM":
                    Create_XJDemImportor();
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
                case "专题产品":
                    Create_XJProdsImportor();
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
                case "算法模型":
                    Create_XJModuleImportor();
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
                case "系统部署包":
                    Create_XJSystemsImportor();
                    ctrlNormalFileImportor1.ctrlVirtualDirSetting1.changeCtrlVirtualStatus(path,target);
                    break;
            }
        
        }
        public void create()
        {
            ctrlVectorDataImport1.Create(InputDataService.BSDB_sqlUtilities, InputDataService.currentUser);
            ctrlRasterProdImportor1.Create(InputDataService.INDB_sqlUtilities, InputDataService.currentUser);
            ctrlNormalFileImportor1.Create(InputDataService.ISDB_sqlUtilities, InputDataService.currentUser);
            ctrlMouldDocImportor1.Create(InputDataService.ISDB_sqlUtilities, InputDataService.currentUser);
            ucImportTarGZ2.Create(InputDataService.EVDB_sqlUtilities, InputDataService.currentUser);
            _isCreate = true;
        }
        
        public void Create_NormalFileImportor()
        {
            ctrlNormalFileImportor1.Create(InputDataService.ISDB_sqlUtilities,InputDataService.currentUser);
        }
        public void Create_XJDemImportor()
        {
            IDbBaseUtilities dbutil = InputDataService.BSDB_sqlUtilities;
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile' and DATA_CODE = 'BSDB-XJ02';";
            System.Data.DataSet ds = dbutil.GetDataSet(sql);

            tablecode_Dal tablecode_dal = new tablecode_Dal(dbutil);
            DataRow dr = ds.Tables[0].Rows[0];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjdemDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjdemDt.tablecapital = dr["NAME"].ToString();
            xjdemDt.tablecode = dr["DATA_CODE"].ToString();
            xjdemDt.groupcode = dr["GROUP_CODE"].ToString();
            xjdemDt.tablename = tablecode_dal.GetTableName(xjdemDt.tablecode);

            ctrlNormalFileImportor1.Create(dbutil, InputDataService.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjdemDt });
        }
        //实测数据
        public void Create_XJBopuImportor()
        {
            IDbBaseUtilities dbutil = InputDataService.EVDB_sqlUtilities;
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
            
            ctrlNormalFileImportor1.Create(dbutil, InputDataService.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjsdclDt, xjxdyzDt });
        }

        //专题产品
        public void Create_XJProdsImportor()
        {
            IDbBaseUtilities dbutil = InputDataService.INDB_sqlUtilities;
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


            ctrlNormalFileImportor1.Create(dbutil, InputDataService.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjslzylDt, xjjxDt, xj4Dt, xj5Dt, xj6Dt, xj7Dt });
        }
        //算法模型
        public void Create_XJModuleImportor()
        {
            IDbBaseUtilities dbutil = InputDataService.MADB_sqlUtilities;
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

            ctrlNormalFileImportor1.Create(dbutil, InputDataService.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjslzylDt, xjjxDt, xj4Dt, xj5Dt, xj6Dt, xj7Dt });
        }
        //系统部署包
        public void Create_XJSystemsImportor()
        {
            IDbBaseUtilities dbutil = InputDataService.MADB_sqlUtilities;
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile' and DATA_CODE = 'MADB-XJ02';";
            System.Data.DataSet ds = dbutil.GetDataSet(sql);

            tablecode_Dal tablecode_dal = new tablecode_Dal(dbutil);
            DataRow dr = ds.Tables[0].Rows[0];
            QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType xjsystemDt = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType();
            xjsystemDt.tablecapital = dr["NAME"].ToString();
            xjsystemDt.tablecode = dr["DATA_CODE"].ToString();
            xjsystemDt.groupcode = dr["GROUP_CODE"].ToString();
            xjsystemDt.tablename = tablecode_dal.GetTableName(xjsystemDt.tablecode);

            ctrlNormalFileImportor1.Create(dbutil, InputDataService.currentUser, new List<QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor.DataType>() { xjsystemDt });
        }

        
    }
}
