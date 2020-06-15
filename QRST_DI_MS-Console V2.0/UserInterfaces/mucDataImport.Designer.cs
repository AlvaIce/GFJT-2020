using System.Windows.Forms;
using System.Collections.Generic;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class mucDataImport
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucImportTarGZ1 = new QRST_DI_MS_Component_DataImportorUI.TAR.GZ.UCImportTarGZ();
            this.tab_SourceData = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ctrlRasterProdImportor1 = new QRST_DI_MS_Component_DataImportorUI.Raster.ctrlRasterProdImportor();
            this.tab_rasterdata = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucTileImport1 = new TilesImport.UCTileImport();
            this.tab_Tiles = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel5 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ctrlMouldDocImportor1 = new QRST_DI_MS_Component_DataImportorUI.MouldDoc.ctrlMouldDocImportor();
            this.tab_Doc = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel6 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ctrlNormalFileImportor1 = new QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor();
            this.tab_NormalFiles = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel4 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ctrlVectorDataImport1 = new QRST_DI_MS_Component_DataImportorUI.Vector.ctrlVectorDataImport();
            this.tab_Vector = new DevComponents.DotNetBar.SuperTabItem();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            this.superTabControlPanel3.SuspendLayout();
            this.superTabControlPanel5.SuspendLayout();
            this.superTabControlPanel6.SuspendLayout();
            this.superTabControlPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.Controls.Add(this.superTabControlPanel6);
            this.superTabControl1.Controls.Add(this.superTabControlPanel5);
            this.superTabControl1.Controls.Add(this.superTabControlPanel4);
            this.superTabControl1.Controls.Add(this.superTabControlPanel3);
            this.superTabControl1.Controls.Add(this.superTabControlPanel2);
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(0, 0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 5;
            this.superTabControl1.Size = new System.Drawing.Size(1319, 580);
            this.superTabControl1.TabFont = new System.Drawing.Font("Tahoma", 9F);
            this.superTabControl1.TabIndex = 0;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tab_SourceData,
            this.tab_rasterdata,
            this.tab_Tiles,
            this.tab_Vector,
            this.tab_Doc,
            this.tab_NormalFiles});
            this.superTabControl1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.WinMediaPlayer12;
            this.superTabControl1.TabsVisible = false;
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.ucImportTarGZ1);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(1319, 554);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.tab_SourceData;
            // 
            // ucImportTarGZ1
            // 
            this.ucImportTarGZ1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucImportTarGZ1.Location = new System.Drawing.Point(0, -22);
            this.ucImportTarGZ1.Name = "ucImportTarGZ1";
            this.ucImportTarGZ1.Size = new System.Drawing.Size(1321, 573);
            this.ucImportTarGZ1.TabIndex = 0;
            // 
            // tab_SourceData
            // 
            this.tab_SourceData.AttachedControl = this.superTabControlPanel1;
            this.tab_SourceData.GlobalItem = false;
            this.tab_SourceData.Name = "tab_SourceData";
            this.tab_SourceData.Text = "L2级数据产品（原始）";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.ctrlRasterProdImportor1);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(1319, 554);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.tab_rasterdata;
            // 
            // ctrlRasterProdImportor1
            // 
            this.ctrlRasterProdImportor1.Location = new System.Drawing.Point(0, 0);
            this.ctrlRasterProdImportor1.Name = "ctrlRasterProdImportor1";
            this.ctrlRasterProdImportor1.Size = new System.Drawing.Size(1288, 470);
            this.ctrlRasterProdImportor1.TabIndex = 0;
            // 
            // tab_rasterdata
            // 
            this.tab_rasterdata.AttachedControl = this.superTabControlPanel2;
            this.tab_rasterdata.GlobalItem = false;
            this.tab_rasterdata.Name = "tab_rasterdata";
            this.tab_rasterdata.Text = "L3-5级影像产品";
            // 
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Controls.Add(this.ucTileImport1);
            this.superTabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel3.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.Size = new System.Drawing.Size(1319, 554);
            this.superTabControlPanel3.TabIndex = 0;
            this.superTabControlPanel3.TabItem = this.tab_Tiles;
            // 
            // ucTileImport1
            // 
            this.ucTileImport1.Location = new System.Drawing.Point(13, 16);
            this.ucTileImport1.Name = "ucTileImport1";
            this.ucTileImport1.Size = new System.Drawing.Size(747, 412);
            this.ucTileImport1.TabIndex = 0;
            // 
            // tab_Tiles
            // 
            this.tab_Tiles.AttachedControl = this.superTabControlPanel3;
            this.tab_Tiles.GlobalItem = false;
            this.tab_Tiles.Name = "tab_Tiles";
            this.tab_Tiles.Text = "瓦片数据产品";
            // 
            // superTabControlPanel5
            // 
            this.superTabControlPanel5.Controls.Add(this.ctrlMouldDocImportor1);
            this.superTabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel5.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel5.Name = "superTabControlPanel5";
            this.superTabControlPanel5.Size = new System.Drawing.Size(1319, 554);
            this.superTabControlPanel5.TabIndex = 0;
            this.superTabControlPanel5.TabItem = this.tab_Doc;
            // 
            // ctrlMouldDocImportor1
            // 
            this.ctrlMouldDocImportor1._MetaDataMDoc = null;
            this.ctrlMouldDocImportor1._SourceFile = null;
            this.ctrlMouldDocImportor1.Location = new System.Drawing.Point(13, 18);
            this.ctrlMouldDocImportor1.Name = "ctrlMouldDocImportor1";
            this.ctrlMouldDocImportor1.Size = new System.Drawing.Size(785, 377);
            this.ctrlMouldDocImportor1.TabIndex = 0;
            // 
            // tab_Doc
            // 
            this.tab_Doc.AttachedControl = this.superTabControlPanel5;
            this.tab_Doc.GlobalItem = false;
            this.tab_Doc.Name = "tab_Doc";
            this.tab_Doc.Text = "文档资料";
            // 
            // superTabControlPanel6
            // 
            this.superTabControlPanel6.Controls.Add(this.ctrlNormalFileImportor1);
            this.superTabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel6.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel6.Name = "superTabControlPanel6";
            this.superTabControlPanel6.Size = new System.Drawing.Size(1319, 554);
            this.superTabControlPanel6.TabIndex = 0;
            this.superTabControlPanel6.TabItem = this.tab_NormalFiles;
            // 
            // ctrlNormalFileImportor1
            // 
            this.ctrlNormalFileImportor1.Location = new System.Drawing.Point(3, 3);
            this.ctrlNormalFileImportor1.Name = "ctrlNormalFileImportor1";
            this.ctrlNormalFileImportor1.Size = new System.Drawing.Size(808, 408);
            this.ctrlNormalFileImportor1.TabIndex = 0;
            // 
            // tab_NormalFiles
            // 
            this.tab_NormalFiles.AttachedControl = this.superTabControlPanel6;
            this.tab_NormalFiles.GlobalItem = false;
            this.tab_NormalFiles.Name = "tab_NormalFiles";
            this.tab_NormalFiles.Text = "一般文件";
            // 
            // superTabControlPanel4
            // 
            this.superTabControlPanel4.Controls.Add(this.ctrlVectorDataImport1);
            this.superTabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel4.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel4.Name = "superTabControlPanel4";
            this.superTabControlPanel4.Size = new System.Drawing.Size(1319, 554);
            this.superTabControlPanel4.TabIndex = 0;
            this.superTabControlPanel4.TabItem = this.tab_Vector;
            // 
            // ctrlVectorDataImport1
            // 
            this.ctrlVectorDataImport1.IsSingleImport = false;
            this.ctrlVectorDataImport1.Location = new System.Drawing.Point(3, 3);
            this.ctrlVectorDataImport1.Name = "ctrlVectorDataImport1";
            this.ctrlVectorDataImport1.Size = new System.Drawing.Size(888, 510);
            this.ctrlVectorDataImport1.TabIndex = 0;
            // 
            // tab_Vector
            // 
            this.tab_Vector.AttachedControl = this.superTabControlPanel4;
            this.tab_Vector.GlobalItem = false;
            this.tab_Vector.Name = "tab_Vector";
            this.tab_Vector.Text = "矢量数据";
            // 
            // mucDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.superTabControl1);
            this.Name = "mucDataImport";
            this.Size = new System.Drawing.Size(1319, 580);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.superTabControlPanel3.ResumeLayout(false);
            this.superTabControlPanel5.ResumeLayout(false);
            this.superTabControlPanel6.ResumeLayout(false);
            this.superTabControlPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        Dictionary<string, TreeView> treeDir = new Dictionary<string, TreeView>();
        public DevComponents.DotNetBar.SuperTabControl superTabControl1;
        public DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel6;
        public DevComponents.DotNetBar.SuperTabItem tab_NormalFiles;
        public DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel5;
        public DevComponents.DotNetBar.SuperTabItem tab_Doc;
        public DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel4;
        public DevComponents.DotNetBar.SuperTabItem tab_Vector;
        public DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel3;
        public DevComponents.DotNetBar.SuperTabItem tab_Tiles;
        public DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        public DevComponents.DotNetBar.SuperTabItem tab_rasterdata;
        public DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        public DevComponents.DotNetBar.SuperTabItem tab_SourceData;
        private QRST_DI_MS_Component_DataImportorUI.TAR.GZ.UCImportTarGZ ucImportTarGZ1;
        private TilesImport.UCTileImport ucTileImport1;
        private QRST_DI_MS_Component_DataImportorUI.Vector.ctrlVectorDataImport ctrlVectorDataImport1;
        private QRST_DI_MS_Component_DataImportorUI.Raster.ctrlRasterProdImportor ctrlRasterProdImportor1;
        private QRST_DI_MS_Component_DataImportorUI.NormalFile.ctrlNormalFileImportor ctrlNormalFileImportor1;
        private QRST_DI_MS_Component_DataImportorUI.MouldDoc.ctrlMouldDocImportor ctrlMouldDocImportor1;
    }
}
