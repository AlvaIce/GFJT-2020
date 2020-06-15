namespace QRST_DI_MS_Console.UserInterfaces
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
            this.TarImportbarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.TileImport = new DevExpress.XtraBars.BarButtonItem();
            this.DataImportTools = new DevExpress.XtraBars.BarButtonItem();
            this.GF1ImportButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.HJImportButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ZY3ImportButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.ZY02CImportButtonItem = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.TarImportbarSubItem,
            this.TileImport,
            this.DataImportTools,
            this.GF1ImportButtonItem,
            this.HJImportButtonItem,
            this.ZY3ImportButtonItem,
            this.ZY02CImportButtonItem});
            this.ribbonControl1.MaxItemId = 26;
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.DataImportTools);
            this.ribbonPageGroup1.ItemLinks.Add(this.TarImportbarSubItem);
            this.ribbonPageGroup1.ItemLinks.Add(this.TileImport);
            this.ribbonPageGroup1.Text = "数据入库";
            // 
            // TarImportbarSubItem
            // 
            this.TarImportbarSubItem.Caption = "原始数据入库";
            this.TarImportbarSubItem.Id = 5;
            this.TarImportbarSubItem.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.订单订制;
            this.TarImportbarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.GF1ImportButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.HJImportButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.ZY3ImportButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.ZY02CImportButtonItem)});
            this.TarImportbarSubItem.Name = "TarImportbarSubItem";
            // 
            // TileImport
            // 
            this.TileImport.Caption = "规格化数据入库";
            this.TileImport.Id = 6;
            this.TileImport.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.订单定制;
            this.TileImport.Name = "TileImport";
            this.TileImport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TileImport_ItemClick);
            // 
            // DataImportTools
            // 
            this.DataImportTools.Caption = "数据入库工具";
            this.DataImportTools.Id = 7;
            this.DataImportTools.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.系统工具集;
            this.DataImportTools.Name = "DataImportTools";
            this.DataImportTools.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DataImportTools_ItemClick);
            // 
            // GF1ImportButtonItem
            // 
            this.GF1ImportButtonItem.Caption = "GF1数据入库";
            this.GF1ImportButtonItem.Description = "GF1";
            this.GF1ImportButtonItem.Id = 22;
            this.GF1ImportButtonItem.Name = "GF1ImportButtonItem";
            this.GF1ImportButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ImportButtonItem_ItemClick);
            // 
            // HJImportButtonItem
            // 
            this.HJImportButtonItem.Caption = "HJ数据入库";
            this.HJImportButtonItem.Id = 23;
            this.HJImportButtonItem.Name = "HJImportButtonItem";
            this.HJImportButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ImportButtonItem_ItemClick);
            // 
            // ZY3ImportButtonItem
            // 
            this.ZY3ImportButtonItem.Caption = "ZY3数据入库";
            this.ZY3ImportButtonItem.Id = 24;
            this.ZY3ImportButtonItem.Name = "ZY3ImportButtonItem";
            this.ZY3ImportButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ImportButtonItem_ItemClick);
            // 
            // ZY02CImportButtonItem
            // 
            this.ZY02CImportButtonItem.Caption = "ZY02C数据入库";
            this.ZY02CImportButtonItem.Id = 25;
            this.ZY02CImportButtonItem.Name = "ZY02CImportButtonItem";
            this.ZY02CImportButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ImportButtonItem_ItemClick);
            // 
            // rucDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "rucDataImport";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem TileImport;
        private DevExpress.XtraBars.BarButtonItem DataImportTools;
        private DevExpress.XtraBars.BarSubItem TarImportbarSubItem;
        private DevExpress.XtraBars.BarButtonItem GF1ImportButtonItem;
        private DevExpress.XtraBars.BarButtonItem HJImportButtonItem;
        private DevExpress.XtraBars.BarButtonItem ZY3ImportButtonItem;
        private DevExpress.XtraBars.BarButtonItem ZY02CImportButtonItem;
    }
}