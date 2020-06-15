using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace QRST_DI_MS_Console.UserInterfaces
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

        private string LocalDownLoadPath=string.Empty;

        public rucDetailViewer()
            :base()
        {
            InitializeComponent();
        }
        public rucDetailViewer(object objDetailUC)
            : base(objDetailUC)
        {
            InitializeComponent();
            mucView = objDetailUC as mucDetailViewer;
            //MSUserInterface msui=new MSUserInterface(()
            //mucView = MSUserInterface.GetMSUIbyRibbonpage(this);
        }

        private void InitializeComponent()
        {
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
            this.ribbonControl1.ExpandCollapseItem.Name = "";
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
            this.barEditItemHideGrid});
            this.ribbonControl1.MaxItemId = 15;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemCheckEditHideRecord,
            this.repositoryItemCheckEditHideGrid});
            this.ribbonControl1.Size = new System.Drawing.Size(876, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemDownLoad);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllSelected, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllNotSelected, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportMetadata, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnDownLoadInfo, true);
            this.ribbonPageGroup1.Text = "操作";
            // 
            // barButtonItemAllSelected
            // 
            this.barButtonItemAllSelected.Caption = "选中全部数据";
            this.barButtonItemAllSelected.Id = 2;
            this.barButtonItemAllSelected.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.全选;
            this.barButtonItemAllSelected.Name = "barButtonItemAllSelected";
            this.barButtonItemAllSelected.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAllSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAllSelected_ItemClick);
            // 
            // barButtonItemDownLoad
            // 
            this.barButtonItemDownLoad.Caption = "下载选中数据";
            this.barButtonItemDownLoad.Id = 3;
            this.barButtonItemDownLoad.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.下载此数据;
            this.barButtonItemDownLoad.Name = "barButtonItemDownLoad";
            this.barButtonItemDownLoad.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDownLoad.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDownLoad_ItemClick);
            // 
            // barButtonItemAllNotSelected
            // 
            this.barButtonItemAllNotSelected.Caption = "不选中数据";
            this.barButtonItemAllNotSelected.Id = 4;
            this.barButtonItemAllNotSelected.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.全不选;
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
            this.barButtonNormal.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.application_view_tile;
            this.barButtonNormal.Name = "barButtonNormal";
            this.barButtonNormal.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonNormal_ItemClick);
            // 
            // barButtonAllTable
            // 
            this.barButtonAllTable.Caption = "全表模式";
            this.barButtonAllTable.Id = 7;
            this.barButtonAllTable.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.application_view_columns;
            this.barButtonAllTable.Name = "barButtonAllTable";
            this.barButtonAllTable.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonAllTable_ItemClick);
            // 
            // barButtonAllPic
            // 
            this.barButtonAllPic.Caption = "全图模式";
            this.barButtonAllPic.Id = 6;
            this.barButtonAllPic.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.application_view_gallery;
            this.barButtonAllPic.Name = "barButtonAllPic";
            this.barButtonAllPic.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonAllPic_ItemClick);
            // 
            // btnExportMetadata
            // 
            this.btnExportMetadata.Caption = "导出选中数据";
            this.btnExportMetadata.Id = 8;
            this.btnExportMetadata.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.icon_doc_excel_angle;
            this.btnExportMetadata.Name = "btnExportMetadata";
            this.btnExportMetadata.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportMetadata_ItemClick);
            // 
            // btnDownLoadInfo
            // 
            this.btnDownLoadInfo.Caption = "下载列表";
            this.btnDownLoadInfo.Id = 9;
            this.btnDownLoadInfo.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.download;
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
            // rucDetailViewer
            // 
            this.Name = "rucDetailViewer";
            this.Size = new System.Drawing.Size(876, 150);
            this.Load += new System.EventHandler(this.rucDetailViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditHideGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditHideRecord)).EndInit();
            this.ResumeLayout(false);

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

        private void barButtonItemAllSelected_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucView.SelectAllData(true);
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

    }
}
