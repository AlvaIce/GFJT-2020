using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucDataAnalyst:RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemVector;
        private DevExpress.XtraBars.BarButtonItem barButtonItemRaster;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDoc;
        private DevExpress.XtraBars.BarButtonItem barButtonItemTable;
        private DevExpress.XtraBars.BarButtonItem barButtonItemIncreaseTrend;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        mucDataAnalyst mucAnalyst;
        public rucDataAnalyst()
            : base()
        {
            InitializeComponent();
        }
        public rucDataAnalyst(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            mucAnalyst = objMUC as mucDataAnalyst;
            
            for (int i = 0 ; i < TheUniversal.subDbLst.Count ;i++ )
            {
                this.repositoryItemComboBox1.Items.Add(TheUniversal.subDbLst[i].DESCRIPTION);
            }
            barEditItem1.EditValue = TheUniversal.subDbLst[0].DESCRIPTION;
            mucAnalyst.Db = TheUniversal.subDbLst[0];
        }

        private void InitializeComponent()
        {
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemRaster = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemVector = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemTable = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDoc = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemIncreaseTrend = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
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
            this.barEditItem1,
            this.barButtonItemVector,
            this.barButtonItemRaster,
            this.barButtonItemDoc,
            this.barButtonItemTable,
            this.barButtonItemIncreaseTrend});
            this.ribbonControl1.MaxItemId = 8;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItem1, true);
            this.ribbonPageGroup1.Text = "数据库";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemRaster, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemVector, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemTable, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemDoc, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemIncreaseTrend, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "统计分析";
            // 
            // barButtonItemRaster
            // 
            this.barButtonItemRaster.Caption = "栅格数据";
            this.barButtonItemRaster.Id = 4;
            this.barButtonItemRaster.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.栅格数据;
            this.barButtonItemRaster.Name = "barButtonItemRaster";
            this.barButtonItemRaster.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemRaster.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemRaster_ItemClick);
            // 
            // barButtonItemVector
            // 
            this.barButtonItemVector.Caption = "矢量数据";
            this.barButtonItemVector.Id = 3;
            this.barButtonItemVector.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.矢量数据_;
            this.barButtonItemVector.Name = "barButtonItemVector";
            this.barButtonItemVector.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemVector.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemVector_ItemClick);
            // 
            // barButtonItemTable
            // 
            this.barButtonItemTable.Caption = "表格数据";
            this.barButtonItemTable.Id = 6;
            this.barButtonItemTable.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.表格数据;
            this.barButtonItemTable.Name = "barButtonItemTable";
            this.barButtonItemTable.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemTable.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemTable_ItemClick);
            // 
            // barButtonItemDoc
            // 
            this.barButtonItemDoc.Caption = "文档数据";
            this.barButtonItemDoc.Id = 5;
            this.barButtonItemDoc.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.文档数据;
            this.barButtonItemDoc.Name = "barButtonItemDoc";
            this.barButtonItemDoc.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDoc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDoc_ItemClick);
            // 
            // barButtonItemIncreaseTrend
            // 
            this.barButtonItemIncreaseTrend.Caption = "增长趋势";
            this.barButtonItemIncreaseTrend.Id = 7;
            this.barButtonItemIncreaseTrend.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.变化趋势;
            this.barButtonItemIncreaseTrend.Name = "barButtonItemIncreaseTrend";
            this.barButtonItemIncreaseTrend.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemIncreaseTrend.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemIncreaseTrend_ItemClick);
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "数据库名称";
            this.barEditItem1.Edit = this.repositoryItemComboBox1;
            this.barEditItem1.Id = 2;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.Width = 150;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.SelectedIndexChanged += new System.EventHandler(this.repositoryItemComboBox1_SelectedIndexChanged);
            // 
            // rucDataAnalyst
            // 
            this.Name = "rucDataAnalyst";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 当数据库切换时，刷新界面统计信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dbName = ((ComboBoxEdit)sender).SelectedItem.ToString();
                mucAnalyst.Db = TheUniversal.GetsubDbByName(dbName);
                ribbonPageGroup2.Visible = true;
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("无法获得统计信息！");
                ribbonPageGroup2.Visible = false;
            }
        
        }

        /// <summary>
        /// 矢量数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemVector_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucAnalyst.DisplaySpecificData(EnumDataKind.System_Vector);
        }

        /// <summary>
        /// 栅格数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemRaster_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucAnalyst.DisplaySpecificData(EnumDataKind.System_Raster);
        }

        /// <summary>
        /// 文档数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucAnalyst.DisplaySpecificData(EnumDataKind.System_Document);
        }

        /// <summary>
        /// 表格数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucAnalyst.DisplaySpecificData(EnumDataKind.System_Table);
        }

        /// <summary>
        /// 增长趋势
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemIncreaseTrend_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           // mucAnalyst.DisplaySpecificData();
        }
    }
}
