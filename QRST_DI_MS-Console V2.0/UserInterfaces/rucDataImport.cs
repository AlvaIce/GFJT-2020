using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class rucDataImport : RibbonPageBaseUC
    {
        mucDataImport _mucDataImport;
        List<BarCheckItem> barButtonItems;
        public rucDataImport()
            : base()
        {
            InitializeComponent();
            barButtonItems = new List<BarCheckItem>();
            barButtonItems.AddRange(new BarCheckItem[] { btn_Doc, btn_RasterProd, btn_SourceData, btn_Vector, bar_NormalFiles, TileImport,barHAglPublish, btn_XJ_BopuDataImp, btn_XJ_DEMImp, btn_XJ_ProdImp, btn_XJ_ModulesImp, btn_XJ_SystemsImp });

            btn_SourceData.PerformClick();
        }
        public rucDataImport(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            _mucDataImport = objMUC as mucDataImport;
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_SourceData;
            barButtonItems = new List<BarCheckItem>();
            barButtonItems.AddRange(new BarCheckItem[] { btn_Doc, btn_RasterProd, btn_SourceData, btn_Vector, bar_NormalFiles, TileImport,barHAglPublish, btn_XJ_BopuDataImp, btn_XJ_DEMImp, btn_XJ_ProdImp, btn_XJ_ModulesImp, btn_XJ_SystemsImp });

            if (!_mucDataImport._isCreated) _mucDataImport.Create();

            btn_SourceData.PerformClick();

            
        }

        public void ClickDownloadItem(string itemcaption, string virtualpath,string virtualcode )
        {
            switch (itemcaption)
            {
                case "原始数据":
                    btn_SourceData_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "非规影像级产品":
                    btn_RasterProd_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "规格化数据":
                    TileImport_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "矢量数据":
                    btn_Vector_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "文档资料":
                    btn_Doc_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "一般文件":
                    bar_NormalFiles_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "专题产品发布":
                    barHAglPublish_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "实测数据":
                    btn_XJ_BopuDataImp_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "示范区DEM":
                    btn_XJ_DEMImp_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "专题产品":
                    btn_XJ_ProdImp_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "算法模型":
                    btn_XJ_ModulesImp_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                case "系统部署包":
                    btn_XJ_SystemsImp_ItemClick(null, null);
                    _mucDataImport.SetVirtualDirPath(itemcaption, virtualpath, virtualcode);
                    break;
                default:
                    break;
            }
        }

        private BarCheckItem preCheckedItem;
        private void updateAllBtnDownStatus(BarCheckItem theClickedOne)
        {
            foreach (BarCheckItem bci in barButtonItems)
            {
                if (bci.Checked && bci != barHAglPublish)
                {
                    preCheckedItem = bci;
                }

                bci.Checked = false;
                if (bci == theClickedOne)
                {
                    bci.Checked = true;
                }
            }
        }

        private void btn_SourceData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!_mucDataImport._isCreated) _mucDataImport.Create();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_SourceData;
            //foreach (BarButtonItem item in barButtonItems)
            //{
            //    item.Down = false;
            //}
            //btn_SourceData.Down = true;

            updateAllBtnDownStatus(btn_SourceData);
        }

        private void btn_RasterProd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!_mucDataImport._isCreated) _mucDataImport.Create();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_rasterdata;
            //foreach (BarButtonItem item in barButtonItems)
            //{
            //    item.Down = false;
            //}
            //btn_RasterProd.Down = true;
            updateAllBtnDownStatus(btn_RasterProd as BarCheckItem);

        }

        private void TileImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_Tiles;

            //foreach (BarButtonItem item in barButtonItems)
            //{
            //    item.Down = false;
            //}
            //TileImport.Down = true;
            updateAllBtnDownStatus(TileImport as BarCheckItem);

        }

        private void btn_Vector_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!_mucDataImport._isCreated) _mucDataImport.Create();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_Vector;

            //foreach (BarButtonItem item in barButtonItems)
            //{
            //    item.Down = false;
            //}
            //btn_Vector.Down = true;
            updateAllBtnDownStatus(btn_Vector as BarCheckItem);

        }

        private void btn_Doc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!_mucDataImport._isCreated) _mucDataImport.Create();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_Doc;

            //foreach (BarButtonItem item in barButtonItems)
            //{
            //    item.Down = false;
            //}
            //btn_Doc.Down = true;
            updateAllBtnDownStatus(btn_Doc as BarCheckItem);

        }

        private void bar_NormalFiles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _mucDataImport.Create_NormalFileImportor();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_NormalFiles;

            //foreach (BarButtonItem item in barButtonItems)
            //{
            //    item.Down = false;
            //}
            //bar_NormalFiles.Down = true;
            updateAllBtnDownStatus(bar_NormalFiles as BarCheckItem);

        }

        private void barHAglPublish_ItemClick(object sender, ItemClickEventArgs e)
        {
            updateAllBtnDownStatus(barHAglPublish as BarCheckItem);

            MoveFile.Form1 frmHAlgPublish = new MoveFile.Form1();
            frmHAlgPublish.ShowDialog();


            preCheckedItem.PerformClick();
        }

        private void btn_XJ_BopuDataImp_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mucDataImport.Create_XJBopuImportor();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_NormalFiles;
            updateAllBtnDownStatus(btn_XJ_BopuDataImp as BarCheckItem);

        }

        private void btn_XJ_ProdImp_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mucDataImport.Create_XJProdsImportor();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_NormalFiles;

            btn_XJ_ProdImp.Down = true;
            updateAllBtnDownStatus(btn_XJ_ProdImp as BarCheckItem);

        }

        private void btn_XJ_ModulesImp_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mucDataImport.Create_XJModuleImportor();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_NormalFiles;
            updateAllBtnDownStatus(btn_XJ_ModulesImp as BarCheckItem);

        }

        private void btn_XJ_SystemsImp_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mucDataImport.Create_XJSystemsImportor();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_NormalFiles;
            updateAllBtnDownStatus(btn_XJ_SystemsImp as BarCheckItem);

        }

        private void btn_XJ_DEMImp_ItemClick(object sender, ItemClickEventArgs e)
        {
            _mucDataImport.Create_XJDemImportor();
            _mucDataImport.superTabControl1.SelectedTab = _mucDataImport.tab_NormalFiles;
            updateAllBtnDownStatus(btn_XJ_DEMImp as BarCheckItem);

        }

    }
}
