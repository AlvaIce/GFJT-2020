using System;
using QRST_DI_MS_Component.VirtualDirUI;
//
namespace QRST_DI_MS_Desktop.UserInterfaces
{ 
    class rucVirtualDirManager:RibbonPageBaseUC
    {
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarButtonItem barItm_NewDir;
        private DevExpress.XtraBars.BarButtonItem barItm_DeleteDir;
        private DevExpress.XtraBars.BarButtonItem barItm_Copy;
        private DevExpress.XtraBars.BarButtonItem barItm_Clip;
        private DevExpress.XtraBars.BarButtonItem barItm_Paste;
        private DevExpress.XtraBars.BarButtonItem barItm_Rename;
        mucVirtualDirManager mucVirtualDirMgr;
        private DevExpress.XtraBars.BarButtonItem barItem_UpdateVDir;
        private DevExpress.XtraBars.BarButtonItem barItem_Download;
        private DevExpress.XtraBars.BarSubItem barSubItem_ImportData;
        private DevExpress.XtraBars.BarButtonItem barBtn_ImportSrcData;
        private DevExpress.XtraBars.BarButtonItem barBtn_ImportImageProd;
        private DevExpress.XtraBars.BarButtonItem barBtn_ImportTile;
        private DevExpress.XtraBars.BarButtonItem barBtn_ImportNormalFile;
        private DevExpress.XtraBars.BarButtonItem barBtn_ImportDoc;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barBtn_ImportVector;
        private DevExpress.XtraBars.BarButtonItem barItem_UndoChecked;
        ctrlVirtualDir _vdctrlmain;
        public rucVirtualDirManager()
            : base()
        {
            InitializeComponent();
            barItm_NewDir.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.newfile;
            barItm_Rename.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.Rename_48.ToBitmap();
            barItm_Paste.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.paste_48.ToBitmap();
            barItm_DeleteDir.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.delete_file.ToBitmap();
            barItm_Copy.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.copy_file.ToBitmap();
            barItm_Clip.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.move_file.ToBitmap();

        }
        public rucVirtualDirManager(object objMUC)
            : base(objMUC)
        {
            mucVirtualDirMgr = objMUC as mucVirtualDirManager;

            InitializeComponent();
            barItm_NewDir.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.newfile;
            barItm_Rename.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.Rename_48.ToBitmap();
            barItm_Paste.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.paste_48.ToBitmap();
            barItm_DeleteDir.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.delete_file.ToBitmap();
            barItm_Copy.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.copy_file.ToBitmap();
            barItm_Clip.LargeGlyph = global::QRST_DI_MS_Component.Properties.Resources.move_file.ToBitmap();

            mucVirtualDirMgr.Create();
            mucVirtualDirManager.MainInstance = mucVirtualDirMgr;

            _vdctrlmain = mucVirtualDirMgr.virtualDirUC.MainVirtualDirCtrl;
            _vdctrlmain.CopyBtnVisitableChanged += new EventHandler(MainVirtualDirCtrl_CopyBtnVisitableChanged);
            _vdctrlmain.PastBtnVisitableChanged += new EventHandler(MainVirtualDirCtrl_PastBtnVisitableChanged);
            //_vdctrlmain.ClipBtnVisitableChanged += new EventHandler(_vdctrlmain_ClipBtnVisitableChanged);
        }

        void _vdctrlmain_ClipBtnVisitableChanged(object sender, EventArgs e)
        {
            barItm_Clip.Enabled = (bool)sender;
        }

        private void InitializeComponent()
        {
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barItm_NewDir = new DevExpress.XtraBars.BarButtonItem();
            this.barItm_DeleteDir = new DevExpress.XtraBars.BarButtonItem();
            this.barItm_Copy = new DevExpress.XtraBars.BarButtonItem();
            this.barItm_Clip = new DevExpress.XtraBars.BarButtonItem();
            this.barItm_Paste = new DevExpress.XtraBars.BarButtonItem();
            this.barItm_Rename = new DevExpress.XtraBars.BarButtonItem();
            this.barItem_UpdateVDir = new DevExpress.XtraBars.BarButtonItem();
            this.barItem_Download = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barSubItem_ImportData = new DevExpress.XtraBars.BarSubItem();
            this.barBtn_ImportSrcData = new DevExpress.XtraBars.BarButtonItem();
            this.barBtn_ImportImageProd = new DevExpress.XtraBars.BarButtonItem();
            this.barBtn_ImportTile = new DevExpress.XtraBars.BarButtonItem();
            this.barBtn_ImportVector = new DevExpress.XtraBars.BarButtonItem();
            this.barBtn_ImportDoc = new DevExpress.XtraBars.BarButtonItem();
            this.barBtn_ImportNormalFile = new DevExpress.XtraBars.BarButtonItem();
            this.barItem_UndoChecked = new DevExpress.XtraBars.BarButtonItem();
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
            this.barItm_NewDir,
            this.barItm_DeleteDir,
            this.barItm_Copy,
            this.barItm_Clip,
            this.barItm_Paste,
            this.barItm_Rename,
            this.barItem_UpdateVDir,
            this.barItem_Download,
            this.barSubItem_ImportData,
            this.barBtn_ImportSrcData,
            this.barBtn_ImportImageProd,
            this.barBtn_ImportTile,
            this.barBtn_ImportNormalFile,
            this.barBtn_ImportDoc,
            this.barBtn_ImportVector,
            this.barItem_UndoChecked});
            this.ribbonControl1.MaxItemId = 33;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.ribbonControl1.Size = new System.Drawing.Size(687, 147);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_NewDir);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_Rename);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_DeleteDir);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_Copy);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_Clip);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_Paste);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItem_UpdateVDir);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItem_UndoChecked);
            this.ribbonPageGroup1.Text = "虚拟目录管理";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // barItm_NewDir
            // 
            this.barItm_NewDir.Caption = "新建文件夹";
            this.barItm_NewDir.Id = 14;
            this.barItm_NewDir.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources._32;
            this.barItm_NewDir.Name = "barItm_NewDir";
            this.barItm_NewDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItm_NewDir_ItemClick);
            // 
            // barItm_DeleteDir
            // 
            this.barItm_DeleteDir.Caption = "删除";
            this.barItm_DeleteDir.Id = 15;
            this.barItm_DeleteDir.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources._32;
            this.barItm_DeleteDir.Name = "barItm_DeleteDir";
            this.barItm_DeleteDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItm_DeleteDir_ItemClick);
            // 
            // barItm_Copy
            // 
            this.barItm_Copy.Caption = "复制";
            this.barItm_Copy.Id = 16;
            this.barItm_Copy.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources._32;
            this.barItm_Copy.Name = "barItm_Copy";
            this.barItm_Copy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItm_Copy_ItemClick);
            // 
            // barItm_Clip
            // 
            this.barItm_Clip.Caption = "剪切";
            this.barItm_Clip.Id = 17;
            this.barItm_Clip.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources._32;
            this.barItm_Clip.Name = "barItm_Clip";
            this.barItm_Clip.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItm_Clip_ItemClick);
            // 
            // barItm_Paste
            // 
            this.barItm_Paste.Caption = "粘贴";
            this.barItm_Paste.Id = 18;
            this.barItm_Paste.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources._32;
            this.barItm_Paste.Name = "barItm_Paste";
            this.barItm_Paste.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItm_Paste_ItemClick);
            // 
            // barItm_Rename
            // 
            this.barItm_Rename.Caption = "重命名文件夹";
            this.barItm_Rename.Id = 19;
            this.barItm_Rename.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources._32;
            this.barItm_Rename.Name = "barItm_Rename";
            this.barItm_Rename.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItm_Rename_ItemClick);
            // 
            // barItem_UpdateVDir
            // 
            this.barItem_UpdateVDir.Caption = "刷新";
            this.barItem_UpdateVDir.Id = 20;
            this.barItem_UpdateVDir.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.备份恢复;
            this.barItem_UpdateVDir.Name = "barItem_UpdateVDir";
            this.barItem_UpdateVDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItem_UpdateVDir_ItemClick);
            // 
            // barItem_Download
            // 
            this.barItem_Download.Caption = "下载";
            this.barItem_Download.Id = 22;
            this.barItem_Download.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.下载此数据;
            this.barItem_Download.Name = "barItem_Download";
            this.barItem_Download.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItem_Download_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barItem_Download);
            this.ribbonPageGroup2.ItemLinks.Add(this.barSubItem_ImportData);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "数据操作";
            // 
            // barSubItem_ImportData
            // 
            this.barSubItem_ImportData.Caption = "导入";
            this.barSubItem_ImportData.Enabled = false;
            this.barSubItem_ImportData.Id = 24;
            this.barSubItem_ImportData.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.订单管理;
            this.barSubItem_ImportData.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtn_ImportSrcData),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtn_ImportImageProd),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtn_ImportTile),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtn_ImportVector),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtn_ImportDoc),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtn_ImportNormalFile)});
            this.barSubItem_ImportData.Name = "barSubItem_ImportData";
            // 
            // barBtn_ImportSrcData
            // 
            this.barBtn_ImportSrcData.Caption = "原始数据导入";
            this.barBtn_ImportSrcData.Id = 26;
            this.barBtn_ImportSrcData.Name = "barBtn_ImportSrcData";
            this.barBtn_ImportSrcData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtn_ImportSrcData_ItemClick);
            // 
            // barBtn_ImportImageProd
            // 
            this.barBtn_ImportImageProd.Caption = "非规影像级产品导入";
            this.barBtn_ImportImageProd.Id = 27;
            this.barBtn_ImportImageProd.Name = "barBtn_ImportImageProd";
            this.barBtn_ImportImageProd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtn_ImportImageProd_ItemClick);
            // 
            // barBtn_ImportTile
            // 
            this.barBtn_ImportTile.Caption = "规格化数据导入";
            this.barBtn_ImportTile.Id = 28;
            this.barBtn_ImportTile.Name = "barBtn_ImportTile";
            this.barBtn_ImportTile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtn_ImportTile_ItemClick);
            // 
            // barBtn_ImportVector
            // 
            this.barBtn_ImportVector.Caption = "矢量数据导入";
            this.barBtn_ImportVector.Id = 31;
            this.barBtn_ImportVector.Name = "barBtn_ImportVector";
            this.barBtn_ImportVector.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtn_ImportVector_ItemClick);
            // 
            // barBtn_ImportDoc
            // 
            this.barBtn_ImportDoc.Caption = "文档数据导入";
            this.barBtn_ImportDoc.Id = 30;
            this.barBtn_ImportDoc.Name = "barBtn_ImportDoc";
            this.barBtn_ImportDoc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtn_ImportDoc_ItemClick);
            // 
            // barBtn_ImportNormalFile
            // 
            this.barBtn_ImportNormalFile.Caption = "一般文件导入";
            this.barBtn_ImportNormalFile.Id = 29;
            this.barBtn_ImportNormalFile.Name = "barBtn_ImportNormalFile";
            this.barBtn_ImportNormalFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtn_ImportNormalFile_ItemClick);
            // 
            // barItem_UndoChecked
            // 
            this.barItem_UndoChecked.Caption = "清空选择";
            this.barItem_UndoChecked.Id = 32;
            this.barItem_UndoChecked.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.清空选择;
            this.barItem_UndoChecked.Name = "barItem_UndoChecked";
            this.barItem_UndoChecked.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItem_UndoChecked_ItemClick);
            // 
            // rucVirtualDirManager
            // 
            this.Name = "rucVirtualDirManager";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }


        void MainVirtualDirCtrl_PastBtnVisitableChanged(object sender, EventArgs e)
        {
            barItm_Paste.Enabled = (bool)sender;
        }

        void MainVirtualDirCtrl_CopyBtnVisitableChanged(object sender, EventArgs e)
        {
            barItm_Copy.Enabled = (bool)sender;
        }


        private void barItm_NewDir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _vdctrlmain.NewDir();
        }

        private void barItm_Rename_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _vdctrlmain.ReNameDir();
        }

        private void barItm_DeleteDir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _vdctrlmain.DeleteDir();
        }

        private void barItm_Copy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _vdctrlmain.Copy();
        }

        private void barItm_Clip_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _vdctrlmain.MoveDir();
        }

        private void barItm_Paste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _vdctrlmain.Paste();
        }

        private void barItem_UpdateVDir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucVirtualDirMgr.UpdateVirtualDir();
        }

        private void barItem_Download_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucVirtualDirMgr.virtualDirUC.DownloadSelectFile();
        }

        private void barBtn_ImportSrcData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtn_ImportImageProd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtn_ImportTile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtn_ImportVector_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtn_ImportDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barBtn_ImportNormalFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barItem_UndoChecked_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _vdctrlmain.Empty();
            _vdctrlmain.Refresh();
        }
    }
}
