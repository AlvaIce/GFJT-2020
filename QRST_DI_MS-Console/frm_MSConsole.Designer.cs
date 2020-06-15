namespace QRST_DI_MS_Console
{
    partial class frm_MSConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_MSConsole));
            this.panelControlLeftPanel = new DevExpress.XtraEditors.PanelControl();
            this.nbc_sysNavi = new DevExpress.XtraNavBar.NavBarControl();
            this.nbg_Searcher = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbi_3DSearcher = new DevExpress.XtraNavBar.NavBarItem();
            this.nbi_DetailViewer = new DevExpress.XtraNavBar.NavBarItem();
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
            this.nbi_SpectrumFeature = new DevExpress.XtraNavBar.NavBarItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.rbpg_Home = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpg_about = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpggrp_sysTools = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pnl_Main = new DevExpress.XtraEditors.PanelControl();
            this.applicationMenu1 = new DevExpress.XtraBars.Ribbon.ApplicationMenu();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            this.gdd_skins = new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlLeftPanel)).BeginInit();
            this.panelControlLeftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbc_sysNavi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdd_skins)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlLeftPanel
            // 
            this.panelControlLeftPanel.Controls.Add(this.nbc_sysNavi);
            this.panelControlLeftPanel.Controls.Add(this.pictureBox1);
            this.panelControlLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControlLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.panelControlLeftPanel.Margin = new System.Windows.Forms.Padding(5);
            this.panelControlLeftPanel.Name = "panelControlLeftPanel";
            this.panelControlLeftPanel.Size = new System.Drawing.Size(204, 562);
            this.panelControlLeftPanel.TabIndex = 14;
            // 
            // nbc_sysNavi
            // 
            this.nbc_sysNavi.ActiveGroup = this.nbg_Searcher;
            this.nbc_sysNavi.AllowSelectedLink = true;
            this.nbc_sysNavi.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.nbc_sysNavi.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.nbc_sysNavi.Location = new System.Drawing.Point(2, 80);
            this.nbc_sysNavi.Name = "nbc_sysNavi";
            this.nbc_sysNavi.OptionsNavPane.ExpandedWidth = 0;
            this.nbc_sysNavi.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.nbc_sysNavi.Size = new System.Drawing.Size(200, 480);
            this.nbc_sysNavi.TabIndex = 8;
            this.nbc_sysNavi.SelectedLinkChanged += new DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventHandler(this.nbc_sysNavi_SelectedLinkChanged);
            // 
            // nbg_Searcher
            // 
            this.nbg_Searcher.Caption = "数据查询浏览子系统";
            this.nbg_Searcher.Expanded = true;
            this.nbg_Searcher.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.nbg_Searcher.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_3DSearcher),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbi_DetailViewer)});
            this.nbg_Searcher.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.数据库查询浏览子系统;
            this.nbg_Searcher.Name = "nbg_Searcher";
            this.nbg_Searcher.SelectedLinkIndex = 1;
            // 
            // nbi_3DSearcher
            // 
            this.nbi_3DSearcher.Caption = "三维可视化综合检索";
            this.nbi_3DSearcher.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.三维可视化综合检索;
            this.nbi_3DSearcher.Name = "nbi_3DSearcher";
            // 
            // nbi_DetailViewer
            // 
            this.nbi_DetailViewer.Caption = "查看详细结果";
            this.nbi_DetailViewer.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.查看详细结果版面;
            this.nbi_DetailViewer.Name = "nbi_DetailViewer";
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
            this.nbg_Maintain.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.数据更新维护子系统;
            this.nbg_Maintain.Name = "nbg_Maintain";
            this.nbg_Maintain.TopVisibleLinkIndex = 2;
            // 
            // nbi_DataMaintainer
            // 
            this.nbi_DataMaintainer.Caption = "数据更新维护";
            this.nbi_DataMaintainer.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.数据更新维护1;
            this.nbi_DataMaintainer.Name = "nbi_DataMaintainer";
            // 
            // nbi_DataBacker
            // 
            this.nbi_DataBacker.Caption = "数据备份恢复";
            this.nbi_DataBacker.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.数据备份恢复;
            this.nbi_DataBacker.Name = "nbi_DataBacker";
            // 
            // nbi_DataAnalyst
            // 
            this.nbi_DataAnalyst.Caption = "分类统计分析";
            this.nbi_DataAnalyst.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.分类统计分析;
            this.nbi_DataAnalyst.Name = "nbi_DataAnalyst";
            // 
            // nbi_DataQualityInspection
            // 
            this.nbi_DataQualityInspection.Caption = "数据质量检查";
            this.nbi_DataQualityInspection.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.数据质量检查;
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
            this.nbg_Changer.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.数据库交换子系统;
            this.nbg_Changer.Name = "nbg_Changer";
            // 
            // nbi_OrderDefiner
            // 
            this.nbi_OrderDefiner.Caption = "订单定制";
            this.nbi_OrderDefiner.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.订单订制;
            this.nbi_OrderDefiner.Name = "nbi_OrderDefiner";
            // 
            // nbi_TaskMonitor
            // 
            this.nbi_TaskMonitor.Caption = "订单管控";
            this.nbi_TaskMonitor.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.订单管控;
            this.nbi_TaskMonitor.Name = "nbi_TaskMonitor";
            // 
            // nbi_TaskDefiner
            // 
            this.nbi_TaskDefiner.Caption = "任务定制";
            this.nbi_TaskDefiner.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.任务定制;
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
            this.nbg_Metadata.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.元数据管理子系统;
            this.nbg_Metadata.Name = "nbg_Metadata";
            // 
            // nbi_MetadataMaker
            // 
            this.nbi_MetadataMaker.Caption = "元数据制作";
            this.nbi_MetadataMaker.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.元数据修改;
            this.nbi_MetadataMaker.Name = "nbi_MetadataMaker";
            // 
            // nbi_MetadataModifier
            // 
            this.nbi_MetadataModifier.Caption = "元数据修改";
            this.nbi_MetadataModifier.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.元数据制作;
            this.nbi_MetadataModifier.Name = "nbi_MetadataModifier";
            // 
            // nbi_SatalliteSensor
            // 
            this.nbi_SatalliteSensor.Caption = "卫星传感器数据管理";
            this.nbi_SatalliteSensor.LargeImage = global::QRST_DI_MS_Console.Properties.Resources._20121231010242968_easyicon_cn_32;
            this.nbi_SatalliteSensor.Name = "nbi_SatalliteSensor";
            // 
            // nbi_ExternalDB
            // 
            this.nbi_ExternalDB.Caption = "外部数据库集成";
            this.nbi_ExternalDB.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.Xp_MadB_007;
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
            this.nbg_SysManager.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.系统管理子系统;
            this.nbg_SysManager.Name = "nbg_SysManager";
            // 
            // nbi_TSSiterManager
            // 
            this.nbi_TSSiterManager.Caption = "服务阵列站点管理";
            this.nbi_TSSiterManager.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.服务器阵列站点管理;
            this.nbi_TSSiterManager.Name = "nbi_TSSiterManager";
            // 
            // nbi_DSSiteManager
            // 
            this.nbi_DSSiteManager.Caption = "数据阵列站点管理";
            this.nbi_DSSiteManager.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.数据阵列站点管理;
            this.nbi_DSSiteManager.Name = "nbi_DSSiteManager";
            // 
            // nbi_SysMonitor
            // 
            this.nbi_SysMonitor.Caption = "系统状态监控";
            this.nbi_SysMonitor.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.系统状态监控;
            this.nbi_SysMonitor.Name = "nbi_SysMonitor";
            // 
            // nbi_UserManager
            // 
            this.nbi_UserManager.Caption = "用户管理";
            this.nbi_UserManager.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.用户管理;
            this.nbi_UserManager.Name = "nbi_UserManager";
            // 
            // nbi_SysToolset
            // 
            this.nbi_SysToolset.Caption = "系统工具集";
            this.nbi_SysToolset.LargeImage = global::QRST_DI_MS_Console.Properties.Resources.系统工具集;
            this.nbi_SysToolset.Name = "nbi_SysToolset";
            // 
            // nbi_SpectrumFeature
            // 
            this.nbi_SpectrumFeature.Caption = "波普特征数据库查询";
            this.nbi_SpectrumFeature.Name = "nbi_SpectrumFeature";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::QRST_DI_MS_Console.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ribbonStatusBar1);
            this.panelControl1.Controls.Add(this.ribbon);
            this.panelControl1.Controls.Add(this.pnl_Main);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(204, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(680, 562);
            this.panelControl1.TabIndex = 23;
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem1);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(2, 535);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbon;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(676, 25);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 11;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // ribbon
            // 
            this.ribbon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ribbon.ApplicationButtonText = null;
            this.ribbon.Dock = System.Windows.Forms.DockStyle.None;
            // 
            // 
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.ExpandCollapseItem.Name = "";
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.barStaticItem1,
            this.barSubItem1});
            this.ribbon.Location = new System.Drawing.Point(3, -22);
            this.ribbon.Margin = new System.Windows.Forms.Padding(5);
            this.ribbon.MaxItemId = 13;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rbpg_Home,
            this.rbpg_about});
            this.ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowCategoryInCaption = false;
            this.ribbon.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.ShowOnMultiplePages;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(676, 147);
            this.ribbon.StatusBar = this.ribbonStatusBar1;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "格式转换";
            this.barSubItem1.Id = 12;
            this.barSubItem1.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.服务器阵列站点管理;
            this.barSubItem1.Name = "barSubItem1";
            this.barSubItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barSubItem1_ItemClick);
            // 
            // rbpg_Home
            // 
            this.rbpg_Home.Name = "rbpg_Home";
            this.rbpg_Home.Text = "主页";
            // 
            // rbpg_about
            // 
            this.rbpg_about.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbpggrp_sysTools});
            this.rbpg_about.Name = "rbpg_about";
            this.rbpg_about.Text = "关于";
            // 
            // rbpggrp_sysTools
            // 
            this.rbpggrp_sysTools.ItemLinks.Add(this.barSubItem1);
            this.rbpggrp_sysTools.Name = "rbpggrp_sysTools";
            this.rbpggrp_sysTools.Text = "系统工具集";
            // 
            // pnl_Main
            // 
            this.pnl_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_Main.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.pnl_Main.Location = new System.Drawing.Point(0, 125);
            this.pnl_Main.Margin = new System.Windows.Forms.Padding(5);
            this.pnl_Main.Name = "pnl_Main";
            this.pnl_Main.Size = new System.Drawing.Size(680, 416);
            this.pnl_Main.TabIndex = 3;
            // 
            // applicationMenu1
            // 
            this.applicationMenu1.Name = "applicationMenu1";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2010 Blue";
            // 
            // gdd_skins
            // 
            this.gdd_skins.Name = "gdd_skins";
            // 
            // frm_MSConsole
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.ForeColor = System.Drawing.Color.Red;
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControlLeftPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frm_MSConsole";
            this.Text = "中国遥感卫星应用服务系统----综合数据库运行管理子系统";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            ((System.ComponentModel.ISupportInitialize)(this.panelControlLeftPanel)).EndInit();
            this.panelControlLeftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbc_sysNavi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdd_skins)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlLeftPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl pnl_Main;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpg_Home;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpg_about;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpggrp_sysTools;
        private DevExpress.XtraNavBar.NavBarControl nbc_sysNavi;
        private DevExpress.XtraNavBar.NavBarGroup nbg_Changer;
        private DevExpress.XtraNavBar.NavBarItem nbi_OrderDefiner;
        private DevExpress.XtraNavBar.NavBarItem nbi_TaskMonitor;
        private DevExpress.XtraNavBar.NavBarItem nbi_TaskDefiner;
        private DevExpress.XtraNavBar.NavBarGroup nbg_Searcher;
        private DevExpress.XtraNavBar.NavBarItem nbi_3DSearcher;
        private DevExpress.XtraNavBar.NavBarItem nbi_DetailViewer;
        private DevExpress.XtraNavBar.NavBarGroup nbg_Maintain;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataMaintainer;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataBacker;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataAnalyst;
        private DevExpress.XtraNavBar.NavBarItem nbi_DataQualityInspection;
        private DevExpress.XtraNavBar.NavBarGroup nbg_Metadata;
        private DevExpress.XtraNavBar.NavBarItem nbi_MetadataMaker;
        private DevExpress.XtraNavBar.NavBarItem nbi_MetadataModifier;
        private DevExpress.XtraNavBar.NavBarItem nbi_SatalliteSensor;
        private DevExpress.XtraNavBar.NavBarItem nbi_ExternalDB;
        private DevExpress.XtraNavBar.NavBarItem nbi_SecureDBIntegrater;
        private DevExpress.XtraNavBar.NavBarGroup nbg_SysManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_TSSiterManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_DSSiteManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_SysMonitor;
        private DevExpress.XtraNavBar.NavBarItem nbi_UserManager;
        private DevExpress.XtraNavBar.NavBarItem nbi_SysToolset;
        private DevExpress.XtraNavBar.NavBarItem nbi_SpectrumFeature;
        private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown gdd_skins;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
    }
}