using System;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using QRST_DI_MS_Component.Common;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using System.Data;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public class rucDetailViewer:RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarButtonItem barButtonItemAllSelected;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDownLoad;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAllNotSelected;
        public mucDetailViewer mucView;
        public static bool canDown = true;
        private DevExpress.XtraBars.BarButtonItem barButtonNormal;
        private DevExpress.XtraBars.BarButtonItem barButtonAllPic;
        private DevExpress.XtraBars.BarButtonItem barButtonAllTable;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem btnExportMetadata;
        private DevExpress.XtraBars.BarButtonItem btnDownLoadInfo;
        private DevExpress.XtraBars.BarButtonItem barButtonItemGridAnalysis;
        private DevExpress.XtraBars.BarEditItem barEditItemCol;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem barEditItemRow;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarEditItem barEditItemHideRecord;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditHideRecord;
        private DevExpress.XtraBars.BarEditItem barEditItemHideGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditHideGrid;
        private DevExpress.XtraBars.BarButtonItem barButtonItemPartSelected;
        private DevExpress.XtraBars.BarButtonItem barItm_Add2VirtualDir;
        private DevExpress.XtraBars.BarButtonItem  barButtonItemDownLoadGFF;

        static metadatacatalognode_Mdl selectedQueryObj = null;      
        private string LocalDownLoadPath=string.Empty;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;

        private static rucDetailViewer runDViewer;

        public rucDetailViewer()
            :base()
        {
            InitializeComponent();
            ribbonPageGroup3.Visible = false;
            runDViewer = this;
        }
        public rucDetailViewer(object objDetailUC)
            : base(objDetailUC)
        {
            InitializeComponent();
            ribbonPageGroup3.Visible = false;
            mucView = objDetailUC as mucDetailViewer;
            //MSUserInterface msui=new MSUserInterface(()
            //mucView = MSUserInterface.GetMSUIbyRibbonpage(this);
            runDViewer = this;
        }
        public static rucDetailViewer getInstance()
        {
            return runDViewer;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rucDetailViewer));
            this.barButtonItemAllSelected = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDownLoad = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemAllNotSelected = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonNormal = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonAllTable = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonAllPic = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportMetadata = new DevExpress.XtraBars.BarButtonItem();
            this.btnDownLoadInfo = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemRow = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemCol = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemHideGrid = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEditHideGrid = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barEditItemHideRecord = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEditHideRecord = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barButtonItemGridAnalysis = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemPartSelected = new DevExpress.XtraBars.BarButtonItem();
            this.barItm_Add2VirtualDir = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDownLoadGFF = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditHideGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditHideRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemAllSelected,
            this.barButtonItemDownLoad,
            this.barButtonItemAllNotSelected,
            this.barButtonNormal,
            this.barButtonAllPic,
            this.barButtonAllTable,
            this.btnExportMetadata,
            this.btnDownLoadInfo,
            this.barButtonItemGridAnalysis,
            this.barEditItemCol,
            this.barEditItemRow,
            this.barEditItemHideRecord,
            this.barEditItemHideGrid,
            this.barButtonItemPartSelected,
            this.barItm_Add2VirtualDir,
            this.barButtonItemDownLoadGFF,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3});
            this.ribbonControl1.MaxItemId = 21;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemCheckEditHideRecord,
            this.repositoryItemCheckEditHideGrid});
            this.ribbonControl1.Size = new System.Drawing.Size(1500, 147);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup4,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemDownLoad);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemDownLoadGFF);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_Add2VirtualDir);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllSelected, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllNotSelected, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportMetadata, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnDownLoadInfo, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemPartSelected);
            this.ribbonPageGroup1.Text = "操作";
            // 
            // barButtonItemAllSelected
            // 
            this.barButtonItemAllSelected.Caption = "选中全部数据";
            this.barButtonItemAllSelected.Id = 2;
            this.barButtonItemAllSelected.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemAllSelected.LargeGlyph")));
            this.barButtonItemAllSelected.Name = "barButtonItemAllSelected";
            this.barButtonItemAllSelected.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAllSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAllSelected_ItemClick);
            // 
            // barButtonItemDownLoad
            // 
            this.barButtonItemDownLoad.Caption = "下载选中数据";
            this.barButtonItemDownLoad.Id = 3;
            this.barButtonItemDownLoad.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemDownLoad.LargeGlyph")));
            this.barButtonItemDownLoad.Name = "barButtonItemDownLoad";
            this.barButtonItemDownLoad.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDownLoad.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDownLoad_ItemClick);
            // 
            // barButtonItemAllNotSelected
            // 
            this.barButtonItemAllNotSelected.Caption = "不选中数据";
            this.barButtonItemAllNotSelected.Id = 4;
            this.barButtonItemAllNotSelected.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemAllNotSelected.LargeGlyph")));
            this.barButtonItemAllNotSelected.Name = "barButtonItemAllNotSelected";
            this.barButtonItemAllNotSelected.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAllNotSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAllNotSelected_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonNormal);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonAllTable, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonAllPic, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "视图切换";
            // 
            // barButtonNormal
            // 
            this.barButtonNormal.Caption = "正常模式";
            this.barButtonNormal.Id = 5;
            this.barButtonNormal.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonNormal.LargeGlyph")));
            this.barButtonNormal.Name = "barButtonNormal";
            this.barButtonNormal.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonNormal_ItemClick);
            // 
            // barButtonAllTable
            // 
            this.barButtonAllTable.Caption = "全表模式";
            this.barButtonAllTable.Id = 7;
            this.barButtonAllTable.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonAllTable.LargeGlyph")));
            this.barButtonAllTable.Name = "barButtonAllTable";
            this.barButtonAllTable.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonAllTable_ItemClick);
            // 
            // barButtonAllPic
            // 
            this.barButtonAllPic.Caption = "全图模式";
            this.barButtonAllPic.Id = 6;
            this.barButtonAllPic.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonAllPic.LargeGlyph")));
            this.barButtonAllPic.Name = "barButtonAllPic";
            this.barButtonAllPic.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonAllPic_ItemClick);
            // 
            // btnExportMetadata
            // 
            this.btnExportMetadata.Caption = "导出选中数据";
            this.btnExportMetadata.Id = 8;
            this.btnExportMetadata.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnExportMetadata.LargeGlyph")));
            this.btnExportMetadata.Name = "btnExportMetadata";
            this.btnExportMetadata.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportMetadata_ItemClick);
            // 
            // btnDownLoadInfo
            // 
            this.btnDownLoadInfo.Caption = "下载列表";
            this.btnDownLoadInfo.Id = 9;
            this.btnDownLoadInfo.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnDownLoadInfo.LargeGlyph")));
            this.btnDownLoadInfo.Name = "btnDownLoadInfo";
            this.btnDownLoadInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDownLoadInfo_ItemClick);
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemRow);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemCol);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemHideGrid, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemHideRecord);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemGridAnalysis, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "数据统计";
            this.ribbonPageGroup3.Visible = false;
            // 
            // barEditItemRow
            // 
            this.barEditItemRow.Caption = "行划分系数";
            this.barEditItemRow.Edit = this.repositoryItemComboBox2;
            this.barEditItemRow.EditValue = 180;
            this.barEditItemRow.Id = 12;
            this.barEditItemRow.Name = "barEditItemRow";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Items.AddRange(new object[] {
            "10",
            "20",
            "90",
            "180",
            "360"});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // barEditItemCol
            // 
            this.barEditItemCol.Caption = "列划分系数";
            this.barEditItemCol.Edit = this.repositoryItemComboBox1;
            this.barEditItemCol.EditValue = 360;
            this.barEditItemCol.Id = 11;
            this.barEditItemCol.Name = "barEditItemCol";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "10",
            "20",
            "90",
            "180",
            "360"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // barEditItemHideGrid
            // 
            this.barEditItemHideGrid.Caption = "隐藏网格";
            this.barEditItemHideGrid.Edit = this.repositoryItemCheckEditHideGrid;
            this.barEditItemHideGrid.EditValue = false;
            this.barEditItemHideGrid.Id = 14;
            this.barEditItemHideGrid.Name = "barEditItemHideGrid";
            // 
            // repositoryItemCheckEditHideGrid
            // 
            this.repositoryItemCheckEditHideGrid.AutoHeight = false;
            this.repositoryItemCheckEditHideGrid.Name = "repositoryItemCheckEditHideGrid";
            this.repositoryItemCheckEditHideGrid.CheckStateChanged += new System.EventHandler(this.repositoryItemCheckEditHideGrid_CheckStateChanged);
            // 
            // barEditItemHideRecord
            // 
            this.barEditItemHideRecord.Caption = "隐藏记录";
            this.barEditItemHideRecord.Edit = this.repositoryItemCheckEditHideRecord;
            this.barEditItemHideRecord.EditValue = false;
            this.barEditItemHideRecord.Id = 13;
            this.barEditItemHideRecord.Name = "barEditItemHideRecord";
            // 
            // repositoryItemCheckEditHideRecord
            // 
            this.repositoryItemCheckEditHideRecord.AutoHeight = false;
            this.repositoryItemCheckEditHideRecord.Name = "repositoryItemCheckEditHideRecord";
            this.repositoryItemCheckEditHideRecord.CheckStateChanged += new System.EventHandler(this.repositoryItemCheckEditHideRecord_CheckStateChanged);
            // 
            // barButtonItemGridAnalysis
            // 
            this.barButtonItemGridAnalysis.Caption = "网格分析";
            this.barButtonItemGridAnalysis.Id = 10;
            this.barButtonItemGridAnalysis.Name = "barButtonItemGridAnalysis";
            this.barButtonItemGridAnalysis.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemGridAnalysis_ItemClick);
            // 
            // barButtonItemPartSelected
            // 
            this.barButtonItemPartSelected.Caption = "选中挑选数据";
            this.barButtonItemPartSelected.Id = 15;
            this.barButtonItemPartSelected.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemPartSelected.LargeGlyph")));
            this.barButtonItemPartSelected.Name = "barButtonItemPartSelected";
            this.barButtonItemPartSelected.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemPartSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemPartSelected_ItemClick);
            // 
            // barItm_Add2VirtualDir
            // 
            this.barItm_Add2VirtualDir.Caption = "添加到我的空间";
            this.barItm_Add2VirtualDir.Id = 16;
            this.barItm_Add2VirtualDir.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barItm_Add2VirtualDir.LargeGlyph")));
            this.barItm_Add2VirtualDir.Name = "barItm_Add2VirtualDir";
            this.barItm_Add2VirtualDir.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barItm_Add2VirtualDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItm_Add2VirtualDir_ItemClick);
            // 
            // barButtonItemDownLoadGFF
            // 
            this.barButtonItemDownLoadGFF.Caption = "下载gff数据";
            this.barButtonItemDownLoadGFF.Id = 17;
            this.barButtonItemDownLoadGFF.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemDownLoadGFF.LargeGlyph")));
            this.barButtonItemDownLoadGFF.Name = "barButtonItemDownLoadGFF";
            this.barButtonItemDownLoadGFF.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDownLoadGFF.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItemDownLoadGFF.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDownLoadGFF_ItemClick);
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "任务分配";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "任务分发";
            this.barButtonItem3.Id = 20;
            this.barButtonItem3.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem3.LargeGlyph")));
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 18;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 19;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // rucDetailViewer
            // 
            this.Name = "rucDetailViewer";
            this.Size = new System.Drawing.Size(1500, 150);
            this.Load += new System.EventHandler(this.rucDetailViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditHideGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditHideRecord)).EndInit();
            this.ResumeLayout(false);

        }
        public  void UpdateView()
        {
            setNotShowBarDownLoadGFF();
            selectedQueryObj = muc3DSearcher._metadatacatalognode_Mdl;
            if (selectedQueryObj == null)
            {
                return;
            }
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")&&ruc3DSearcher.clickFlag == true)
            {
                setShowBarDownLoadGFF();
                ruc3DSearcher.clickFlag = false;
            }
            else
            {
                setNotShowBarDownLoadGFF();
            }

        }
        /// <summary>
        /// 设置当点击瓦片的单时相全覆盖时，显示下载GFF数据按钮
        /// </summary>
        public  void setShowBarDownLoadGFF() 
        {
            barButtonItemDownLoadGFF.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

        }
        /// <summary>
        /// 只有当点击瓦片的单时相全覆盖时，显示下载GFF数据按钮，否则都不显示
        /// </summary>
        public  void setNotShowBarDownLoadGFF()
        {
           barButtonItemDownLoadGFF.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

        }

        //下载选中数据
        private void barButtonItemDownLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.DownLoadSelectedData();

        }

        private void barButtonItemAllNotSelected_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.SelectAllData(false);
        }
        //选中所有数据
        private void barButtonItemAllSelected_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           mucView.SelectAllData(true);
        }
        //选中挑选的数据
        private void barButtonItemPartSelected_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.SelectPartData(true);
        }


        public void  setDownLoadAvailable(bool inBool)
        {
            this.barButtonItemDownLoad.Enabled = inBool;
        }

        private void rucDetailViewer_Load(object sender, EventArgs e)
        {

        }

        private void barButtonNormal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.SwitchDisplaySchema("normal");
        }

        private void barButtonAllPic_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.SwitchDisplaySchema("allpicture");
        }

        private void barButtonAllTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.SwitchDisplaySchema("alltable");
        }


        /// <summary>
        /// 导出元数据信息 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportMetadata_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.ExportMetadata();
        }

        /// <summary>
        /// 显示下载列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownLoadInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmDownLoadLst.GetInstance().Show();
        }

        /// <summary>
        /// 进行数据网格统计分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemGridAnalysis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.GridStatistic(int.Parse(barEditItemCol.EditValue.ToString()), int.Parse(barEditItemRow.EditValue.ToString()));
        }

        private void repositoryItemCheckEditHideRecord_CheckStateChanged(object sender, EventArgs e)
        {
            if (((CheckEdit)sender).CheckState == CheckState.Checked)
            {
                mucView.LayerControl("tmpDrawExtentsLayer", false);
            }
            else
            {
                mucView.LayerControl("tmpDrawExtentsLayer", true);
            }
         
        }

        private void repositoryItemCheckEditHideGrid_CheckStateChanged(object sender, EventArgs e)
        {
            if (((CheckEdit)sender).CheckState == CheckState.Checked)
            {
                mucView.LayerControl("tmpDrawExtentsLayer1", false);
            }
            else
            {
                mucView.LayerControl("tmpDrawExtentsLayer1", true);
            }
        }

        private void barItm_Add2VirtualDir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.AddVirtualDirSelectedData();
        }
        /// <summary>
        /// 瓦片数据下载gff压缩包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDownLoadGFF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.DownLoadSelectedGFFData();
        }

        //任务分配
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (mucView == null || mucView.AddSelectedData() == null)
            {
                MessageBox.Show("请先进行数据检索！");
                return;
            }
            DataTable dt = mucView.AddSelectedData();
            metadatacatalognode_Mdl selectedQueryObj = mucView._selectedQueryObj;
            QueryPara queryPara = mucView.queryPara;
            if (dt.Rows.Count<1)
            {
                MessageBox.Show("没有选择要分发的数据！");
                return;
            }
            FrmTaskDistribution taskDistribution = new FrmTaskDistribution(dt, selectedQueryObj, queryPara);
            taskDistribution.Show();
        }
    
    }
}
