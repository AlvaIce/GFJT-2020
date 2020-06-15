namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class rucDataQualityInspection
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rucDataQualityInspection));
            this.barEditItemDataType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemPopupContainerDataType = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.popupContainerControlInspectDataType = new DevExpress.XtraEditors.PopupContainerControl();
            this.treeViewInspectDataType = new System.Windows.Forms.TreeView();
            this.repositoryItemDataType = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemSelectFile = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSelectFile = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnFileInspection = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemSelectDir = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControlInspectDataType)).BeginInit();
            this.popupContainerControlInspectDataType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSelectFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSelectDir)).BeginInit();
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
            this.barEditItemDataType,
            this.barButtonItem2,
            this.barEditItemSelectFile,
            this.btnFileInspection,
            this.barButtonItem1,
            this.barButtonItem3});
            this.ribbonControl1.MaxItemId = 25;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDataType,
            this.repositoryItemSelectFile,
            this.repositoryItemSelectDir,
            this.repositoryItemPopupContainerDataType});
            this.ribbonControl1.Size = new System.Drawing.Size(860, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup4});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemDataType);
            this.ribbonPageGroup1.Text = "定制条件";
            // 
            // barEditItemDataType
            // 
            this.barEditItemDataType.Caption = "数据类型";
            this.barEditItemDataType.Edit = this.repositoryItemPopupContainerDataType;
            this.barEditItemDataType.Id = 1;
            this.barEditItemDataType.Name = "barEditItemDataType";
            this.barEditItemDataType.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditItemDataType.Width = 100;
            // 
            // repositoryItemPopupContainerDataType
            // 
            this.repositoryItemPopupContainerDataType.AutoHeight = false;
            this.repositoryItemPopupContainerDataType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemPopupContainerDataType.Name = "repositoryItemPopupContainerDataType";
            this.repositoryItemPopupContainerDataType.PopupControl = this.popupContainerControlInspectDataType;
            this.repositoryItemPopupContainerDataType.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.repositoryItemPopupContainerDataType_QueryResultValue);
            // 
            // popupContainerControlInspectDataType
            // 
            this.popupContainerControlInspectDataType.Controls.Add(this.treeViewInspectDataType);
            this.popupContainerControlInspectDataType.Location = new System.Drawing.Point(3, 106);
            this.popupContainerControlInspectDataType.Name = "popupContainerControlInspectDataType";
            this.popupContainerControlInspectDataType.Size = new System.Drawing.Size(137, 217);
            this.popupContainerControlInspectDataType.TabIndex = 1;
            // 
            // treeViewInspectDataType
            // 
            this.treeViewInspectDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewInspectDataType.Location = new System.Drawing.Point(0, 0);
            this.treeViewInspectDataType.Name = "treeViewInspectDataType";
            this.treeViewInspectDataType.Size = new System.Drawing.Size(137, 217);
            this.treeViewInspectDataType.TabIndex = 0;
            this.treeViewInspectDataType.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewInspectDataType_AfterSelect);
            this.treeViewInspectDataType.DoubleClick += new System.EventHandler(this.treeViewInspectDataType_DoubleClick);
            // 
            // repositoryItemDataType
            // 
            this.repositoryItemDataType.AutoHeight = false;
            this.repositoryItemDataType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDataType.Name = "repositoryItemDataType";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemSelectFile);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnFileInspection, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "检测数据选择";
            // 
            // barEditItemSelectFile
            // 
            this.barEditItemSelectFile.CanOpenEdit = false;
            this.barEditItemSelectFile.Caption = "选择检验数据";
            this.barEditItemSelectFile.Edit = this.repositoryItemSelectFile;
            this.barEditItemSelectFile.Id = 10;
            this.barEditItemSelectFile.Name = "barEditItemSelectFile";
            this.barEditItemSelectFile.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditItemSelectFile.Width = 300;
            this.barEditItemSelectFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barEditItemSelectFile_ItemClick);
            // 
            // repositoryItemSelectFile
            // 
            this.repositoryItemSelectFile.AutoHeight = false;
            this.repositoryItemSelectFile.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSelectFile.Name = "repositoryItemSelectFile";
            this.repositoryItemSelectFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemSelectFile_ButtonClick);
            // 
            // btnFileInspection
            // 
            this.btnFileInspection.Caption = "数据检验";
            this.btnFileInspection.Id = 15;
            this.btnFileInspection.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.数据检验;
            this.btnFileInspection.Name = "btnFileInspection";
            this.btnFileInspection.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnFileInspection.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnFileInspection_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 9;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItem3, true);
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItem1, true);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "数据操作";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "检测日志打印";
            this.barButtonItem3.Id = 24;
            this.barButtonItem3.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.批量检验;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "导出检测报告";
            this.barButtonItem1.Id = 23;
            this.barButtonItem1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.LargeGlyph")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // repositoryItemSelectDir
            // 
            this.repositoryItemSelectDir.AutoHeight = false;
            this.repositoryItemSelectDir.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSelectDir.Name = "repositoryItemSelectDir";
            this.repositoryItemSelectDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemSelectDir_ButtonClick);
            // 
            // rucDataQualityInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.popupContainerControlInspectDataType);
            this.Name = "rucDataQualityInspection";
            this.Size = new System.Drawing.Size(860, 150);
            this.VisibleChanged += new System.EventHandler(this.rucDataQualityInspection_VisibleChanged);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.popupContainerControlInspectDataType, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControlInspectDataType)).EndInit();
            this.popupContainerControlInspectDataType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSelectFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSelectDir)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarEditItem barEditItemDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemDataType;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarEditItem barEditItemSelectFile;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemSelectFile;
        private DevExpress.XtraBars.BarButtonItem btnFileInspection;
        //private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemSelectDir;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraEditors.PopupContainerControl popupContainerControlInspectDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupContainerDataType;
        private System.Windows.Forms.TreeView treeViewInspectDataType;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
    }
}
