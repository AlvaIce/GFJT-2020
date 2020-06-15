namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class rucDataImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_SourceData = new DevExpress.XtraBars.BarCheckItem();
            this.GF1ImportButtonItem = new DevExpress.XtraBars.BarCheckItem();
            this.HJImportButtonItem = new DevExpress.XtraBars.BarCheckItem();
            this.ZY3ImportButtonItem = new DevExpress.XtraBars.BarCheckItem();
            this.ZY02CImportButtonItem = new DevExpress.XtraBars.BarCheckItem();
            this.TileImport = new DevExpress.XtraBars.BarCheckItem();
            this.DataImportTools = new DevExpress.XtraBars.BarCheckItem();
            this.btn_RasterProd = new DevExpress.XtraBars.BarCheckItem();
            this.btn_Vector = new DevExpress.XtraBars.BarCheckItem();
            this.btn_Doc = new DevExpress.XtraBars.BarCheckItem();
            this.bar_NormalFiles = new DevExpress.XtraBars.BarCheckItem();
            this.barHAglPublish = new DevExpress.XtraBars.BarCheckItem();
            this.ribbonPageGroup2_XJ = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btn_XJ_BopuDataImp = new DevExpress.XtraBars.BarCheckItem();
            this.btn_XJ_DEMImp = new DevExpress.XtraBars.BarCheckItem();
            this.btn_XJ_ProdImp = new DevExpress.XtraBars.BarCheckItem();
            this.btn_XJ_ModulesImp = new DevExpress.XtraBars.BarCheckItem();
            this.btn_XJ_SystemsImp = new DevExpress.XtraBars.BarCheckItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btn_SourceData,
            this.TileImport,
            this.DataImportTools,
            this.GF1ImportButtonItem,
            this.HJImportButtonItem,
            this.ZY3ImportButtonItem,
            this.ZY02CImportButtonItem,
            this.btn_RasterProd,
            this.btn_Vector,
            this.btn_Doc,
            this.bar_NormalFiles,
            this.barHAglPublish,
            this.btn_XJ_BopuDataImp,
            this.btn_XJ_DEMImp,
            this.btn_XJ_ProdImp,
            this.btn_XJ_ModulesImp,
            this.btn_XJ_SystemsImp});
            this.ribbonControl1.MaxItemId = 38;
            this.ribbonControl1.Size = new System.Drawing.Size(924, 147);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2_XJ});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btn_SourceData);
            this.ribbonPageGroup1.ItemLinks.Add(this.btn_RasterProd);
            this.ribbonPageGroup1.ItemLinks.Add(this.TileImport);
            this.ribbonPageGroup1.ItemLinks.Add(this.btn_Vector);
            this.ribbonPageGroup1.ItemLinks.Add(this.btn_Doc);
            this.ribbonPageGroup1.ItemLinks.Add(this.DataImportTools);
            this.ribbonPageGroup1.ItemLinks.Add(this.bar_NormalFiles);
            this.ribbonPageGroup1.ItemLinks.Add(this.barHAglPublish);
            this.ribbonPageGroup1.Text = "数据入库";
            // 
            // btn_SourceData
            // 
            this.btn_SourceData.Caption = "原始数据";
            this.btn_SourceData.Checked = true;
            this.btn_SourceData.Id = 5;
            this.btn_SourceData.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.MapPackageMPKFile64;
            this.btn_SourceData.Name = "btn_SourceData";
            this.btn_SourceData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_SourceData_ItemClick);
            // 
            // GF1ImportButtonItem
            // 
            this.GF1ImportButtonItem.Caption = "GF1数据入库";
            this.GF1ImportButtonItem.Description = "GF1";
            this.GF1ImportButtonItem.Id = 22;
            this.GF1ImportButtonItem.Name = "GF1ImportButtonItem";
            // 
            // HJImportButtonItem
            // 
            this.HJImportButtonItem.Caption = "HJ数据入库";
            this.HJImportButtonItem.Id = 23;
            this.HJImportButtonItem.Name = "HJImportButtonItem";
            // 
            // ZY3ImportButtonItem
            // 
            this.ZY3ImportButtonItem.Caption = "ZY3数据入库";
            this.ZY3ImportButtonItem.Id = 24;
            this.ZY3ImportButtonItem.Name = "ZY3ImportButtonItem";
            // 
            // ZY02CImportButtonItem
            // 
            this.ZY02CImportButtonItem.Caption = "ZY02C数据入库";
            this.ZY02CImportButtonItem.Id = 25;
            this.ZY02CImportButtonItem.Name = "ZY02CImportButtonItem";
            // 
            // TileImport
            // 
            this.TileImport.Caption = "规格化数据";
            this.TileImport.Id = 6;
            this.TileImport.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.栅格数据;
            this.TileImport.Name = "TileImport";
            this.TileImport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TileImport_ItemClick);
            // 
            // DataImportTools
            // 
            this.DataImportTools.Caption = "工具模块";
            this.DataImportTools.Enabled = false;
            this.DataImportTools.Id = 7;
            this.DataImportTools.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.系统工具集;
            this.DataImportTools.Name = "DataImportTools";
            this.DataImportTools.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // btn_RasterProd
            // 
            this.btn_RasterProd.Caption = "非规影像级产品";
            this.btn_RasterProd.Id = 26;
            this.btn_RasterProd.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.上传图片;
            this.btn_RasterProd.Name = "btn_RasterProd";
            this.btn_RasterProd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_RasterProd_ItemClick);
            // 
            // btn_Vector
            // 
            this.btn_Vector.Caption = "矢量数据";
            this.btn_Vector.Id = 27;
            this.btn_Vector.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.矢量数据_;
            this.btn_Vector.Name = "btn_Vector";
            this.btn_Vector.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Vector_ItemClick);
            // 
            // btn_Doc
            // 
            this.btn_Doc.Caption = "文档资料";
            this.btn_Doc.Id = 28;
            this.btn_Doc.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.表格数据;
            this.btn_Doc.Name = "btn_Doc";
            this.btn_Doc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Doc_ItemClick);
            // 
            // bar_NormalFiles
            // 
            this.bar_NormalFiles.Caption = "一般文件";
            this.bar_NormalFiles.Id = 29;
            this.bar_NormalFiles.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.订单订制;
            this.bar_NormalFiles.Name = "bar_NormalFiles";
            this.bar_NormalFiles.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bar_NormalFiles_ItemClick);
            // 
            // barHAglPublish
            // 
            this.barHAglPublish.Caption = "专题产品发布";
            this.barHAglPublish.Id = 32;
            this.barHAglPublish.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.订单定制;
            this.barHAglPublish.Name = "barHAglPublish";
            this.barHAglPublish.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barHAglPublish_ItemClick);
            // 
            // ribbonPageGroup2_XJ
            // 
            this.ribbonPageGroup2_XJ.ItemLinks.Add(this.btn_XJ_BopuDataImp);
            this.ribbonPageGroup2_XJ.ItemLinks.Add(this.btn_XJ_DEMImp);
            this.ribbonPageGroup2_XJ.ItemLinks.Add(this.btn_XJ_ProdImp);
            this.ribbonPageGroup2_XJ.ItemLinks.Add(this.btn_XJ_ModulesImp);
            this.ribbonPageGroup2_XJ.ItemLinks.Add(this.btn_XJ_SystemsImp);
            this.ribbonPageGroup2_XJ.Name = "ribbonPageGroup2_XJ";
            this.ribbonPageGroup2_XJ.Text = "项目成果归档";
            // 
            // btn_XJ_BopuDataImp
            // 
            this.btn_XJ_BopuDataImp.Caption = "实测数据";
            this.btn_XJ_BopuDataImp.Id = 33;
            this.btn_XJ_BopuDataImp.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.application_view_gallery;
            this.btn_XJ_BopuDataImp.Name = "btn_XJ_BopuDataImp";
            this.btn_XJ_BopuDataImp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_XJ_BopuDataImp_ItemClick);
            // 
            // btn_XJ_DEMImp
            // 
            this.btn_XJ_DEMImp.Caption = "示范区DEM";
            this.btn_XJ_DEMImp.Id = 34;
            this.btn_XJ_DEMImp.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.查看详细结果面板;
            this.btn_XJ_DEMImp.Name = "btn_XJ_DEMImp";
            this.btn_XJ_DEMImp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_XJ_DEMImp_ItemClick);
            // 
            // btn_XJ_ProdImp
            // 
            this.btn_XJ_ProdImp.Caption = "专题产品";
            this.btn_XJ_ProdImp.Id = 35;
            this.btn_XJ_ProdImp.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.自定义任务;
            this.btn_XJ_ProdImp.Name = "btn_XJ_ProdImp";
            this.btn_XJ_ProdImp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_XJ_ProdImp_ItemClick);
            // 
            // btn_XJ_ModulesImp
            // 
            this.btn_XJ_ModulesImp.Caption = "算法模型";
            this.btn_XJ_ModulesImp.Id = 36;
            this.btn_XJ_ModulesImp.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.数据阵列;
            this.btn_XJ_ModulesImp.Name = "btn_XJ_ModulesImp";
            this.btn_XJ_ModulesImp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_XJ_ModulesImp_ItemClick);
            // 
            // btn_XJ_SystemsImp
            // 
            this.btn_XJ_SystemsImp.Caption = "系统部署包";
            this.btn_XJ_SystemsImp.Id = 37;
            this.btn_XJ_SystemsImp.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.远程任务;
            this.btn_XJ_SystemsImp.Name = "btn_XJ_SystemsImp";
            this.btn_XJ_SystemsImp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_XJ_SystemsImp_ItemClick);
            // 
            // rucDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "rucDataImport";
            this.Size = new System.Drawing.Size(924, 150);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarCheckItem TileImport;
        private DevExpress.XtraBars.BarCheckItem DataImportTools;
        private DevExpress.XtraBars.BarCheckItem btn_SourceData;
        private DevExpress.XtraBars.BarCheckItem GF1ImportButtonItem;
        private DevExpress.XtraBars.BarCheckItem HJImportButtonItem;
        private DevExpress.XtraBars.BarCheckItem ZY3ImportButtonItem;
        private DevExpress.XtraBars.BarCheckItem ZY02CImportButtonItem;
        private DevExpress.XtraBars.BarCheckItem btn_RasterProd;
        private DevExpress.XtraBars.BarCheckItem btn_Vector;
        private DevExpress.XtraBars.BarCheckItem btn_Doc;
        private DevExpress.XtraBars.BarCheckItem bar_NormalFiles;
        private DevExpress.XtraBars.BarCheckItem barHAglPublish;
        private DevExpress.XtraBars.BarCheckItem btn_XJ_BopuDataImp;
        private DevExpress.XtraBars.BarCheckItem btn_XJ_DEMImp;
        private DevExpress.XtraBars.BarCheckItem btn_XJ_ProdImp;
        private DevExpress.XtraBars.BarCheckItem btn_XJ_ModulesImp;
        private DevExpress.XtraBars.BarCheckItem btn_XJ_SystemsImp;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2_XJ;
    }
}