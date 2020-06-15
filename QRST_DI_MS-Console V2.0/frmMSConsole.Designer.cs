namespace QRST_DI_MS_Desktop
{
    partial class frmMSConsole
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
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.applicationMenu1 = new DevExpress.XtraBars.Ribbon.ApplicationMenu();
            this.bbi_ChooseSkin = new DevExpress.XtraBars.BarButtonItem();
            this.gdd_skins = new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.bbi_sysOption = new DevExpress.XtraBars.BarButtonItem();
            this.bbi_sysHelper = new DevExpress.XtraBars.BarButtonItem();
            this.bbi_sysUpdater = new DevExpress.XtraBars.BarButtonItem();
            this.bsi_sysAbout = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.rbpg_Home = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpg_about = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpggrp_sysTools = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpggrp_sysSupport = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.nbc_sysNavi = new DevExpress.XtraNavBar.NavBarControl();
            this.nbg_Searcher = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbi_3DSearcher = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_DetailViewer = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_SpectrumFeature = new DevExpress.XtraNavBar.NavBarItem();
            this.nbg_Maintain = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbi_DataMaintainer = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_DataBacker = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_DataAnalyst = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_DataQualityInspection = new DevExpress.XtraNavBar.NavBarItem();
            this.nbg_Changer = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbi_OrderDefiner = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_TaskMonitor = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_TaskDefiner = new DevExpress.XtraNavBar.NavBarItem();
            this.nbg_Metadata = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbi_MetadataMaker = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_MetadataModifier = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_SatalliteSensor = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_ExternalDB = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_SecureDBIntegrater = new DevExpress.XtraNavBar.NavBarItem();
            this.nbg_SysManager = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbi_TSSiterManager = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_DSSiteManager = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_SysMonitor = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_UserManager = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_SysToolset = new DevExpress.XtraNavBar.NavBarItem();
            this.pnl_Main = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdd_skins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbc_sysNavi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ApplicationButtonDropDownControl = this.applicationMenu1;
            this.ribbon.ApplicationButtonText = null;
            // 
            // 
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.ExpandCollapseItem.Name = "";
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.bbi_sysOption,
            this.bbi_sysHelper,
            this.bbi_sysUpdater,
            this.bsi_sysAbout,
            this.bbi_ChooseSkin,
            this.barButtonItem1,
            this.barStaticItem1});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 11;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rbpg_Home,
            this.rbpg_about});
            this.ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbon.Size = new System.Drawing.Size(767, 152);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ItemLinks.Add(this.barButtonItem1);
            this.ribbon.Toolbar.ItemLinks.Add(this.barButtonItem1);
            // 
            // applicationMenu1
            // 
            this.applicationMenu1.ItemLinks.Add(this.bbi_ChooseSkin);
            this.applicationMenu1.Name = "applicationMenu1";
            this.applicationMenu1.Ribbon = this.ribbon;
            // 
            // bbi_ChooseSkin
            // 
            this.bbi_ChooseSkin.ActAsDropDown = true;
            this.bbi_ChooseSkin.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.bbi_ChooseSkin.Caption = "更改皮肤";
            this.bbi_ChooseSkin.DropDownControl = this.gdd_skins;
            this.bbi_ChooseSkin.Id = 7;
            this.bbi_ChooseSkin.Name = "bbi_ChooseSkin";
            // 
            // gdd_skins
            // 
            this.gdd_skins.Name = "gdd_skins";
            this.gdd_skins.Ribbon = this.ribbon;
            // 
            // bbi_sysOption
            // 
            this.bbi_sysOption.Caption = "选项";
            this.bbi_sysOption.Id = 1;
            this.bbi_sysOption.Name = "bbi_sysOption";
            this.bbi_sysOption.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // bbi_sysHelper
            // 
            this.bbi_sysHelper.Caption = "帮助";
            this.bbi_sysHelper.Id = 2;
            this.bbi_sysHelper.Name = "bbi_sysHelper";
            this.bbi_sysHelper.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // bbi_sysUpdater
            // 
            this.bbi_sysUpdater.Caption = "更新";
            this.bbi_sysUpdater.Id = 5;
            this.bbi_sysUpdater.Name = "bbi_sysUpdater";
            this.bbi_sysUpdater.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // bsi_sysAbout
            // 
            this.bsi_sysAbout.Caption = "GF应用综合数据库运行管理系统 v1.0";
            this.bsi_sysAbout.Id = 6;
            this.bsi_sysAbout.Name = "bsi_sysAbout";
            this.bsi_sysAbout.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 9;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 10;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // rbpg_Home
            // 
            this.rbpg_Home.Name = "rbpg_Home";
            this.rbpg_Home.Text = "主页";
            // 
            // rbpg_about
            // 
            this.rbpg_about.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbpggrp_sysTools,
            this.rbpggrp_sysSupport});
            this.rbpg_about.Name = "rbpg_about";
            this.rbpg_about.Text = "关于";
            // 
            // rbpggrp_sysTools
            // 
            this.rbpggrp_sysTools.Name = "rbpggrp_sysTools";
            this.rbpggrp_sysTools.Text = "系统工具集";
            // 
            // rbpggrp_sysSupport
            // 
            this.rbpggrp_sysSupport.ItemLinks.Add(this.bbi_sysOption);
            this.rbpggrp_sysSupport.ItemLinks.Add(this.bbi_sysUpdater, true);
            this.rbpggrp_sysSupport.ItemLinks.Add(this.bbi_sysHelper, true);
            this.rbpggrp_sysSupport.ItemLinks.Add(this.bsi_sysAbout, true);
            this.rbpggrp_sysSupport.Name = "rbpggrp_sysSupport";
            this.rbpggrp_sysSupport.Text = "系统支持";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItem1);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 610);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(767, 32);
            // 
            // nbc_sysNavi
            // 
            this.nbc_sysNavi.ActiveGroup = this.nbg_Searcher;
            this.nbc_sysNavi.AllowSelectedLink = true;
            this.nbc_sysNavi.Dock = System.Windows.Forms.DockStyle.Right;
            this.nbc_sysNavi.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.nbg_Searcher,
            this.nbg_Maintain,
            this.nbg_Changer,
            this.nbg_Metadata,
            this.nbg_SysManager});
            this.nbc_sysNavi.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.nbi_3DSearcher,
            this.nbi_DetailViewer,
            this.nbi_TaskMonitor,
            this.nbi_TaskDefiner,
            this.nbi_DataMaintainer,
            this.nbi_DataBacker,
            this.nbi_DataAnalyst,
            this.nbi_MetadataMaker,
            this.nbi_MetadataModifier,
            this.nbi_SatalliteSensor,
            this.nbi_SecureDBIntegrater,
            this.nbi_DSSiteManager,
            this.nbi_TSSiterManager,
            this.nbi_SysMonitor,
            this.nbi_UserManager,
            this.nbi_SysToolset,
            this.nbi_ExternalDB,
            this.nbi_OrderDefiner,
            this.nbi_DataQualityInspection,
            this.nbi_SpectrumFeature});
            this.nbc_sysNavi.Location = new System.Drawing.Point(590, 152);
            this.nbc_sysNavi.Name = "nbc_sysNavi";
            this.nbc_sysNavi.OptionsNavPane.ExpandedWidth = 0;
            this.nbc_sysNavi.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.nbc_sysNavi.Size = new System.Drawing.Size(177, 458);
            this.nbc_sysNavi.TabIndex = 4;
            this.nbc_sysNavi.SelectedLinkChanged += new DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventHandler(this.nbc_sysNavi_SelectedLinkChanged);
            // 
            // nbg_Searcher
            // 
            this.nbg_Searcher.Caption = "数据查询浏览子系统";
            this.nbg_Searcher.Expanded = true;
            this.nbg_Searcher.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.nbg_Searcher.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_3DSearcher),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_DetailViewer),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_SpectrumFeature)});
            this.nbg_Searcher.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.数据库查询浏览子系统;
            this.nbg_Searcher.Name = "nbg_Searcher";
            this.nbg_Searcher.SelectedLinkIndex = 1;
            // 
            // nbi_3DSearcher
            // 
            this.nbi_3DSearcher.Caption = "三维可视化综合检索";
            this.nbi_3DSearcher.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.三维可视化综合检索;
            this.nbi_3DSearcher.Name = "nbi_3DSearcher";
            // 
            // nbi_DetailViewer
            // 
            this.nbi_DetailViewer.Caption = "查看详细结果";
            this.nbi_DetailViewer.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.查看详细结果版面;
            this.nbi_DetailViewer.Name = "nbi_DetailViewer";
            // 
            // nbi_SpectrumFeature
            // 
            this.nbi_SpectrumFeature.Caption = "波普特征数据库查询";
            this.nbi_SpectrumFeature.Name = "nbi_SpectrumFeature";
            // 
            // nbg_Maintain
            // 
            this.nbg_Maintain.Caption = "数据更新维护子系统";
            this.nbg_Maintain.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.nbg_Maintain.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_DataMaintainer),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_DataBacker),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_DataAnalyst),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_DataQualityInspection)});
            this.nbg_Maintain.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.数据更新维护子系统;
            this.nbg_Maintain.Name = "nbg_Maintain";
            this.nbg_Maintain.TopVisibleLinkIndex = 2;
            // 
            // nbi_DataMaintainer
            // 
            this.nbi_DataMaintainer.Caption = "数据更新维护";
            this.nbi_DataMaintainer.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.数据更新维护1;
            this.nbi_DataMaintainer.Name = "nbi_DataMaintainer";
            // 
            // nbi_DataBacker
            // 
            this.nbi_DataBacker.Caption = "数据备份恢复";
            this.nbi_DataBacker.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.数据备份恢复;
            this.nbi_DataBacker.Name = "nbi_DataBacker";
            // 
            // nbi_DataAnalyst
            // 
            this.nbi_DataAnalyst.Caption = "分类统计分析";
            this.nbi_DataAnalyst.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.分类统计分析;
            this.nbi_DataAnalyst.Name = "nbi_DataAnalyst";
            // 
            // nbi_DataQualityInspection
            // 
            this.nbi_DataQualityInspection.Caption = "数据质量检查";
            this.nbi_DataQualityInspection.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.数据质量检查;
            this.nbi_DataQualityInspection.Name = "nbi_DataQualityInspection";
            // 
            // nbg_Changer
            // 
            this.nbg_Changer.Caption = "数据交换子系统";
            this.nbg_Changer.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.nbg_Changer.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_OrderDefiner),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_TaskMonitor),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_TaskDefiner)});
            this.nbg_Changer.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.数据库交换子系统;
            this.nbg_Changer.Name = "nbg_Changer";
            // 
            // nbi_OrderDefiner
            // 
            this.nbi_OrderDefiner.Caption = "订单定制";
            this.nbi_OrderDefiner.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.订单订制;
            this.nbi_OrderDefiner.Name = "nbi_OrderDefiner";
            // 
            // nbi_TaskMonitor
            // 
            this.nbi_TaskMonitor.Caption = "订单管控";
            this.nbi_TaskMonitor.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.订单管控;
            this.nbi_TaskMonitor.Name = "nbi_TaskMonitor";
            // 
            // nbi_TaskDefiner
            // 
            this.nbi_TaskDefiner.Caption = "任务定制";
            this.nbi_TaskDefiner.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.任务定制;
            this.nbi_TaskDefiner.Name = "nbi_TaskDefiner";
            // 
            // nbg_Metadata
            // 
            this.nbg_Metadata.Caption = "元数据管理子系统";
            this.nbg_Metadata.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.nbg_Metadata.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_MetadataMaker),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_MetadataModifier),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_SatalliteSensor),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_ExternalDB),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_SecureDBIntegrater)});
            this.nbg_Metadata.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.元数据管理子系统;
            this.nbg_Metadata.Name = "nbg_Metadata";
            // 
            // nbi_MetadataMaker
            // 
            this.nbi_MetadataMaker.Caption = "元数据制作";
            this.nbi_MetadataMaker.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.元数据修改;
            this.nbi_MetadataMaker.Name = "nbi_MetadataMaker";
            // 
            // nbi_MetadataModifier
            // 
            this.nbi_MetadataModifier.Caption = "元数据修改";
            this.nbi_MetadataModifier.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.元数据制作;
            this.nbi_MetadataModifier.Name = "nbi_MetadataModifier";
            // 
            // nbi_SatalliteSensor
            // 
            this.nbi_SatalliteSensor.Caption = "卫星传感器数据管理";
            this.nbi_SatalliteSensor.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources._20121231010242968_easyicon_cn_32;
            this.nbi_SatalliteSensor.Name = "nbi_SatalliteSensor";
            // 
            // nbi_ExternalDB
            // 
            this.nbi_ExternalDB.Caption = "外部数据库集成";
            this.nbi_ExternalDB.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.Xp_MadB_007;
            this.nbi_ExternalDB.Name = "nbi_ExternalDB";
            // 
            // nbi_SecureDBIntegrater
            // 
            this.nbi_SecureDBIntegrater.Caption = "涉密数据库集成";
            this.nbi_SecureDBIntegrater.Name = "nbi_SecureDBIntegrater";
            // 
            // nbg_SysManager
            // 
            this.nbg_SysManager.Caption = "系统管理子系统";
            this.nbg_SysManager.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.nbg_SysManager.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_TSSiterManager),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_DSSiteManager),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_SysMonitor),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_UserManager),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_SysToolset)});
            this.nbg_SysManager.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.系统管理子系统;
            this.nbg_SysManager.Name = "nbg_SysManager";
            // 
            // nbi_TSSiterManager
            // 
            this.nbi_TSSiterManager.Caption = "服务阵列站点管理";
            this.nbi_TSSiterManager.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.服务器阵列站点管理;
            this.nbi_TSSiterManager.Name = "nbi_TSSiterManager";
            // 
            // nbi_DSSiteManager
            // 
            this.nbi_DSSiteManager.Caption = "数据阵列站点管理";
            this.nbi_DSSiteManager.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.数据阵列站点管理;
            this.nbi_DSSiteManager.Name = "nbi_DSSiteManager";
            // 
            // nbi_SysMonitor
            // 
            this.nbi_SysMonitor.Caption = "系统状态监控";
            this.nbi_SysMonitor.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.系统状态监控;
            this.nbi_SysMonitor.Name = "nbi_SysMonitor";
            // 
            // nbi_UserManager
            // 
            this.nbi_UserManager.Caption = "用户管理";
            this.nbi_UserManager.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.用户管理;
            this.nbi_UserManager.Name = "nbi_UserManager";
            // 
            // nbi_SysToolset
            // 
            this.nbi_SysToolset.Caption = "系统工具集";
            this.nbi_SysToolset.LargeImage = global::QRST_DI_MS_Desktop.Properties.Resources.系统工具集;
            this.nbi_SysToolset.Name = "nbi_SysToolset";
            // 
            // pnl_Main
            // 
            this.pnl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Main.Location = new System.Drawing.Point(0, 152);
            this.pnl_Main.Name = "pnl_Main";
            this.pnl_Main.Size = new System.Drawing.Size(590, 458);
            this.pnl_Main.TabIndex = 3;
            // 
            // frmMSConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 642);
            this.Controls.Add(this.pnl_Main);
            this.Controls.Add(this.nbc_sysNavi);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "frmMSConsole";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "综合数据库运行管理系统";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdd_skins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbc_sysNavi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpggrp_sysTools;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu1;
        private DevExpress.XtraBars.BarButtonItem bbi_sysOption;
        private DevExpress.XtraBars.BarButtonItem bbi_sysHelper;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpg_Home;
        private DevExpress.XtraBars.BarButtonItem bbi_sysUpdater;
        private DevExpress.XtraBars.BarStaticItem bsi_sysAbout;
        private DevExpress.XtraBars.BarButtonItem bbi_ChooseSkin;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown gdd_skins;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpg_about;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpggrp_sysSupport;
        private DevExpress.XtraNavBar.NavBarControl nbc_sysNavi;
        private DevExpress.XtraEditors.PanelControl pnl_Main; 
        private DevExpress.XtraNavBar.NavBarGroup nbg_Searcher;
        private DevExpress.XtraNavBar.NavBarGroup nbg_Maintain;
        private DevExpress.XtraNavBar.NavBarGroup nbg_Changer;
        private DevExpress.XtraNavBar.NavBarGroup nbg_Metadata;
        private DevExpress.XtraNavBar.NavBarGroup nbg_SysManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_3DSearcher;
        private DevExpress.XtraNavBar.NavBarItem nbi_TSSiterManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_DSSiteManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_SysMonitor;
        private DevExpress.XtraNavBar.NavBarItem nbi_UserManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_SysToolset;
        private DevExpress.XtraNavBar.NavBarItem nbi_DetailViewer;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataAnalyst;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataBacker;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataMaintainer;
        private DevExpress.XtraNavBar.NavBarItem nbi_TaskMonitor;
        private DevExpress.XtraNavBar.NavBarItem nbi_TaskDefiner;
        private DevExpress.XtraNavBar.NavBarItem nbi_MetadataMaker;
        private DevExpress.XtraNavBar.NavBarItem nbi_MetadataModifier;
        private DevExpress.XtraNavBar.NavBarItem nbi_SatalliteSensor;
        private DevExpress.XtraNavBar.NavBarItem nbi_SecureDBIntegrater;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraNavBar.NavBarItem nbi_ExternalDB;
        private DevExpress.XtraNavBar.NavBarItem nbi_OrderDefiner;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataQualityInspection;
        private DevExpress.XtraNavBar.NavBarItem nbi_SpectrumFeature;
    }
}
