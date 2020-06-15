//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Windows.Forms;
//using QRST_DI_DS_Metadata.MetaDataCls;
//using QRST_DI_DS_MetadataQuery.JSONutilty;
//using QRST_DI_Resources;
//using QRST_DI_SS_DBInterfaces.IDBService;

//namespace QRST_DI_MS_Desktop.UserInterfaces
//{
//    public partial class rucSpectrumSearch : RibbonPageBaseUC
//    {

//        /******************************************designer**************************************/
//        private DevExpress.XtraBars.BarEditItem barEditItemBeginDate;
//        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEditBeginTime;
//        private DevExpress.XtraBars.BarEditItem barEditItemEndDate;
//        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEditEndTime;
//        private DevExpress.XtraBars.BarEditItem barEditItemMaxLon;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMaxLon;
//        private DevExpress.XtraBars.BarEditItem barEditItemMinLon;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMinLon;
//        private DevExpress.XtraBars.BarEditItem barEditItemMinLat;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMinLat;
//        private DevExpress.XtraBars.BarEditItem barEditItemMaxLat;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMaxLat;
//        private DevExpress.XtraBars.BarEditItem barEditItemCity_OBJname;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditCity_OBJname;
//        private DevExpress.XtraBars.BarEditItem barEditItemCity_OBJkind;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditCity_OBJkind;
//        private DevExpress.XtraBars.BarEditItem barEditItemSoilName;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditSoilName;
//        private DevExpress.XtraBars.BarEditItem barEditItemSoilzilei;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditSoilzilei;
//        private DevExpress.XtraBars.BarEditItem barEditItemSoildltz;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditSoildltz;
//        private DevExpress.XtraBars.BarEditItem barEditItemSoilfgwmc;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditSoilfgwmc;
//        private DevExpress.XtraBars.BarEditItem barEditItemV_ZBMC;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditV_ZBMC;
//        private DevExpress.XtraBars.BarEditItem barEditItemV_ZBLB;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditV_ZBLB;
//        private DevExpress.XtraBars.BarEditItem barEditItemV_CLBW;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditV_CLBW;
//        private DevExpress.XtraBars.BarEditItem barEditItemV_WHQ;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditV_WHQ;
//        private DevExpress.XtraBars.BarEditItem barEditItemW_SYMC;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditW_SYMC;
//        private DevExpress.XtraBars.BarEditItem barEditItemW_SSLB;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditW_SSLB;
//        private DevExpress.XtraBars.BarEditItem barEditItemW_GPYQ;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditW_GPYQ;
//        private DevExpress.XtraBars.BarEditItem barEditItemW_DLTZ;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditW_DLTZ;
//        private DevExpress.XtraBars.BarEditItem barEditItemR_YKMC;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditR_YKMC;
//        private DevExpress.XtraBars.BarEditItem barEditItemR_YKLB;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditR_YKLB;
//        private DevExpress.XtraBars.BarEditItem barEditItemR_YKZL;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditR_YKZL;
//        private DevExpress.XtraBars.BarEditItem barEditItemR_SSLB;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditR_SSLB;
//        private DevExpress.XtraBars.BarEditItem barEditItemA_ZDMC;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditA_ZDMC;
//        private DevExpress.XtraBars.BarEditItem barEditItemA_ZDBH;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemTextEditA_ZDBH;







//        /// <summary> 
//        /// 必需的设计器变量。
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        private static IQDB_Searcher_Db _dbSearchClient = Constant.ISearcherDbServ;
//        /// <summary> 
//        /// 清理所有正在使用的资源。
//        /// </summary>
//        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region 组件设计器生成的代码

//        /// <summary> 
//        /// 设计器支持所需的方法 - 不要
//        /// 使用代码编辑器修改此方法的内容。
//        /// </summary>
//        private void InitializeComponent()
//        {


//            this.barEditItemBeginDate = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemDateEditBeginTime = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
//            this.barEditItemEndDate = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemDateEditEndTime = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
//            this.barEditItemMaxLon = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemTextEditMaxLon = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.barEditItemMinLon = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemTextEditMinLon = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.barEditItemMaxLat = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemTextEditMaxLat = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.barEditItemMinLat = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemTextEditMinLat = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            barEditItemCity_OBJname = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditCity_OBJname = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemCity_OBJkind = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditCity_OBJkind = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemSoilName = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditSoilName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            barEditItemSoilzilei = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditSoilzilei = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemSoildltz = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditSoildltz = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            barEditItemSoilfgwmc = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditSoilfgwmc = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemV_ZBMC = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditV_ZBMC = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            barEditItemV_ZBLB = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditV_ZBLB = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemV_CLBW = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditV_CLBW = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemV_WHQ = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditV_WHQ = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemW_SYMC = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditW_SYMC = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemW_SSLB = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditW_SSLB = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemW_GPYQ = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditW_GPYQ = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemW_DLTZ = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditW_DLTZ = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            barEditItemR_YKMC = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditR_YKMC = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            barEditItemR_YKLB = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditR_YKLB = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemR_YKZL = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditR_YKZL = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemR_SSLB = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditR_SSLB = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemA_ZDMC = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditA_ZDMC = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            barEditItemA_ZDBH = new DevExpress.XtraBars.BarEditItem();
//            repositoryItemTextEditA_ZDBH = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();



//            this.barEditItem_feature = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.barButtonItem_search = new DevExpress.XtraBars.BarButtonItem();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // ribbonControl1
//            // 
//            // 
//            // 
//            // 
//            this.ribbonControl1.ExpandCollapseItem.Id = 0;
//            this.ribbonControl1.ExpandCollapseItem.Name = "";
//            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.barEditItem_feature,
//            this.barButtonItem_search});
//            this.ribbonControl1.MaxItemId = 5;
//            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemComboBox1});
//            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
//            // 
//            // ribbonPage1
//            // 
//            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
//            this.ribbonPageGroup2,
//            this.ribbonPageGroup3});
//            // 
//            // ribbonPageGroup1
//            // 
//            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItem_feature);
//            this.ribbonPageGroup1.Text = "地物类型选择";
//            // 
//            // barEditItem_feature
//            // 
//            this.barEditItem_feature.Caption = "地物类型";
//            this.barEditItem_feature.Edit = this.repositoryItemComboBox1;
//            this.barEditItem_feature.Id = 2;
//            this.barEditItem_feature.Name = "barEditItem_feature";
//            this.barEditItem_feature.Width = 150;
//            this.barEditItem_feature.EditValueChanged += new System.EventHandler(this.barEditItem_feature_EditValueChanged);
//            // 
//            // repositoryItemComboBox1
//            // 
//            this.repositoryItemComboBox1.AutoHeight = false;
//            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemComboBox1.Items.AddRange(new object[] {
//            "南方植被",
//            "北方植被",
//            "城市目标",
//            "地表大气",
//            "土壤",
//            "水体",
//            "岩矿",
//            ""});
//            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
//            // 
//            // ribbonPageGroup2
//            // 
//            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
//            this.ribbonPageGroup2.Text = "检索条件";
//            // 
//            // ribbonPageGroup3
//            // 
//            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem_search);
//            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
//            this.ribbonPageGroup3.Text = "应用";
//            // 
//            // barButtonItem_search
//            // 
//            this.barButtonItem_search.Caption = "检索";
//            this.barButtonItem_search.Id = 4;
//            this.barButtonItem_search.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.三维可视化检索;
//            this.barButtonItem_search.Name = "barButtonItem_search";
//            this.barButtonItem_search.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
//            this.barButtonItem_search.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemQuery_ItemClick);




//            // 
//            // barEditItemBeginDate
//            // 
//            this.barEditItemBeginDate.Caption = "开始时间";
//            this.barEditItemBeginDate.Edit = this.repositoryItemDateEditBeginTime;
//            this.barEditItemBeginDate.Id = 3;
//            this.barEditItemBeginDate.Name = "barEditItemBeginDate";
//            this.barEditItemBeginDate.Width = 120;
//            // 
//            // repositoryItemDateEditBeginTime
//            // 
//            this.repositoryItemDateEditBeginTime.AutoHeight = false;
//            this.repositoryItemDateEditBeginTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemDateEditBeginTime.Name = "repositoryItemDateEditBeginTime";
//            this.repositoryItemDateEditBeginTime.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton()});
//            // 
//            // barEditItemEndDate
//            // 
//            this.barEditItemEndDate.Caption = "结束时间";
//            this.barEditItemEndDate.Edit = this.repositoryItemDateEditEndTime;
//            this.barEditItemEndDate.Id = 4;
//            this.barEditItemEndDate.Name = "barEditItemEndDate";
//            this.barEditItemEndDate.Width = 120;
//            // 
//            // repositoryItemDateEditEndTime
//            // 
//            this.repositoryItemDateEditEndTime.AutoHeight = false;
//            this.repositoryItemDateEditEndTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemDateEditEndTime.Name = "repositoryItemDateEditEndTime";
//            this.repositoryItemDateEditEndTime.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton()});
//            // 
//            // barEditItemMaxLon
//            // 
//            this.barEditItemMaxLon.Caption = "最大经度";
//            this.barEditItemMaxLon.Edit = this.repositoryItemTextEditMaxLon;
//            this.barEditItemMaxLon.Id = 5;
//            this.barEditItemMaxLon.Name = "barEditItemMaxLon";
//            this.barEditItemMaxLon.Width = 80;
//            // 
//            // repositoryItemTextEditMaxLon
//            // 
//            this.repositoryItemTextEditMaxLon.AutoHeight = false;
//            this.repositoryItemTextEditMaxLon.Name = "repositoryItemTextEditMaxLon";
//            // 
//            // barEditItemMinLon
//            // 
//            this.barEditItemMinLon.Caption = "最小经度";
//            this.barEditItemMinLon.Edit = this.repositoryItemTextEditMinLon;
//            this.barEditItemMinLon.Id = 6;
//            this.barEditItemMinLon.Name = "barEditItemMinLon";
//            this.barEditItemMinLon.Width = 80;
//            // 
//            // repositoryItemTextEditMinLon
//            // 
//            this.repositoryItemTextEditMinLon.AutoHeight = false;
//            this.repositoryItemTextEditMinLon.Name = "repositoryItemTextEditMinLon";
//            // 
//            // barEditItemMaxLat
//            // 
//            this.barEditItemMaxLat.Caption = "最大纬度";
//            this.barEditItemMaxLat.Edit = this.repositoryItemTextEditMaxLat;
//            this.barEditItemMaxLat.Id = 8;
//            this.barEditItemMaxLat.Name = "barEditItemMaxLat";
//            this.barEditItemMaxLat.Width = 80;
//            // 
//            // repositoryItemTextEditMaxLat
//            // 
//            this.repositoryItemTextEditMaxLat.AutoHeight = false;
//            this.repositoryItemTextEditMaxLat.Name = "repositoryItemTextEditMaxLat";
//            // 
//            // barEditItemMinLat
//            // 
//            this.barEditItemMinLat.Caption = "最小纬度";
//            this.barEditItemMinLat.Edit = this.repositoryItemTextEditMinLat;
//            this.barEditItemMinLat.Id = 7;
//            this.barEditItemMinLat.Name = "barEditItemMinLat";
//            this.barEditItemMinLat.Width = 80;
//            // 
//            // repositoryItemTextEditMinLat
//            // 
//            this.repositoryItemTextEditMinLat.AutoHeight = false;
//            this.repositoryItemTextEditMinLat.Name = "repositoryItemTextEditMinLat";

//            //
//            // barEditItemCity_OBJname
//            //
//            this.barEditItemCity_OBJname.Caption = "城市目标名称";
//            this.barEditItemCity_OBJname.Edit = this.repositoryItemTextEditCity_OBJname;
//            this.barEditItemCity_OBJname.Id = 8;
//            this.barEditItemCity_OBJname.Name = "barEditItemCity_OBJname";
//            this.barEditItemCity_OBJname.Width = 80;

//            //
//            //repositoryItemTextEditCity_OBJname
//            //
//            this.repositoryItemTextEditCity_OBJname.AutoHeight = false;
//            this.repositoryItemTextEditCity_OBJname.Name = "repositoryItemTextEditCity_OBJname";
//            //
//            // barEditItemCity_OBJkind
//            //
//            this.barEditItemCity_OBJkind.Caption = "城市目标类别";
//            this.barEditItemCity_OBJkind.Edit = this.repositoryItemTextEditCity_OBJkind;
//            this.barEditItemCity_OBJkind.Id = 9;
//            this.barEditItemCity_OBJkind.Name = "barEditItemCity_OBJname";
//            this.barEditItemCity_OBJkind.Width = 80;
//            //
//            //repositoryItemTextEditCity_OBJkind
//            //
//            this.repositoryItemTextEditCity_OBJkind.AutoHeight = false;
//            this.repositoryItemTextEditCity_OBJkind.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditCity_OBJkind.Name = "repositoryItemTextEditCity_OBJkind";
//            //
//            // barEditItemSoilName
//            //
//            this.barEditItemSoilName.Caption = "土壤名称";
//            this.barEditItemSoilName.Edit = this.repositoryItemTextEditSoilName;
//            this.barEditItemSoilName.Id = 10;
//            this.barEditItemSoilName.Name = "barEditItemSoilName";
//            this.barEditItemSoilName.Width = 80;
//            //
//            //repositoryItemTextEditSoilName
//            //
//            this.repositoryItemTextEditSoilName.AutoHeight = false;
//            this.repositoryItemTextEditSoilName.Name = "repositoryItemTextEditSoilName";
//            //
//            // barEditItemSoilzilei
//            //
//            this.barEditItemSoilzilei.Caption = "土壤子类";
//            this.barEditItemSoilzilei.Edit = this.repositoryItemTextEditSoilzilei;
//            this.barEditItemSoilzilei.Id = 11;
//            this.barEditItemSoilzilei.Name = "barEditItemSoilzilei";
//            this.barEditItemSoilzilei.Width = 80;
//            //
//            //repositoryItemTextEditSoilzilei
//            //
//            this.repositoryItemTextEditSoilzilei.AutoHeight = false;
//            this.repositoryItemTextEditSoilzilei.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditSoilzilei.Name = "repositoryItemTextEditSoilzilei";
//            //
//            // barEditItemSoildltz
//            //
//            this.barEditItemSoildltz.Caption = " 地理特征  ";
//            this.barEditItemSoildltz.Edit = this.repositoryItemTextEditSoildltz;
//            this.barEditItemSoildltz.Id = 12;
//            this.barEditItemSoildltz.Name = "barEditItemSoildltz";
//            this.barEditItemSoildltz.Width = 80;
//            //
//            //repositoryItemTextEditSoildltz
//            //
//            this.repositoryItemTextEditSoildltz.AutoHeight = false;
//            this.repositoryItemTextEditSoildltz.Name = "repositoryItemTextEditSoildltz";
//            //
//            // barEditItemSoilfgwmc
//            //
//            this.barEditItemSoilfgwmc.Caption = "覆盖物名称";
//            this.barEditItemSoilfgwmc.Edit = this.repositoryItemTextEditSoilfgwmc;
//            this.barEditItemSoilfgwmc.Id = 13;
//            this.barEditItemSoilfgwmc.Name = "barEditItemSoilfgwmc";
//            this.barEditItemSoilfgwmc.Width = 80;
//            //
//            //repositoryItemTextEditSoilfgwmc
//            //
//            this.repositoryItemTextEditSoilfgwmc.AutoHeight = false;
//            this.repositoryItemTextEditSoilfgwmc.Name = "repositoryItemTextEditSoilfgwmc";
//            //
//            // barEditItemV_ZBMC
//            //
//            this.barEditItemV_ZBMC.Caption = "植被名称";
//            this.barEditItemV_ZBMC.Edit = this.repositoryItemTextEditV_ZBMC;
//            this.barEditItemV_ZBMC.Id = 14;
//            this.barEditItemV_ZBMC.Name = "barEditItemV_ZBMC";
//            this.barEditItemV_ZBMC.Width = 80;
//            //
//            //repositoryItemTextEditV_ZBMC
//            //
//            this.repositoryItemTextEditV_ZBMC.AutoHeight = false;
//            this.repositoryItemTextEditV_ZBMC.Name = "repositoryItemTextEditV_ZBMC";
//            //
//            // barEditItemV_ZBLB
//            //
//            this.barEditItemV_ZBLB.Caption = "植被类别";
//            this.barEditItemV_ZBLB.Edit = this.repositoryItemTextEditV_ZBLB;
//            this.barEditItemV_ZBLB.Id = 15;
//            this.barEditItemV_ZBLB.Name = "barEditItemV_ZBLB";
//            this.barEditItemV_ZBLB.Width = 80;
//            //
//            //repositoryItemTextEditV_ZBLB
//            //
//            this.repositoryItemTextEditV_ZBLB.AutoHeight = false;
//            this.repositoryItemTextEditV_ZBLB.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditV_ZBLB.Name = "repositoryItemTextEditV_ZBLB";
//            //
//            // barEditItemV_CLBW
//            //
//            this.barEditItemV_CLBW.Caption = "测量部位";
//            this.barEditItemV_CLBW.Edit = this.repositoryItemTextEditV_CLBW;
//            this.barEditItemV_CLBW.Id = 16;
//            this.barEditItemV_CLBW.Name = "barEditItemV_CLBW";
//            this.barEditItemV_CLBW.Width = 80;
//            //
//            //repositoryItemTextEditV_CLBW
//            //
//            this.repositoryItemTextEditV_CLBW.AutoHeight = false;
//            this.repositoryItemTextEditV_CLBW.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditV_CLBW.Name = "repositoryItemTextEditV_CLBW";
//            //
//            // barEditItemV_WHQ
//            //
//            this.barEditItemV_WHQ.Caption = " 物候期  ";
//            this.barEditItemV_WHQ.Edit = this.repositoryItemTextEditV_WHQ;
//            this.barEditItemV_WHQ.Id = 17;
//            this.barEditItemV_WHQ.Name = "barEditItemV_WHQ";
//            this.barEditItemV_WHQ.Width = 80;
//            //
//            //repositoryItemTextEditV_WHQ
//            //
//            this.repositoryItemTextEditV_WHQ.AutoHeight = false;
//            this.repositoryItemTextEditV_WHQ.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditV_WHQ.Name = "repositoryItemTextEditV_WHQ";
//            //
//            // barEditItemR_YKMC
//            //
//            this.barEditItemR_YKMC.Caption = "岩矿名称";
//            this.barEditItemR_YKMC.Edit = this.repositoryItemTextEditR_YKMC;
//            this.barEditItemR_YKMC.Id = 18;
//            this.barEditItemR_YKMC.Name = "barEditItemR_YKMC";
//            this.barEditItemR_YKMC.Width = 80;
//            //
//            //repositoryItemTextEditR_YKMC
//            //
//            this.repositoryItemTextEditR_YKMC.AutoHeight = false;
//            this.repositoryItemTextEditR_YKMC.Name = "repositoryItemTextEditR_YKMC";
//            //
//            // barEditItemR_YKLB
//            //
//            this.barEditItemR_YKLB.Caption = "岩矿类别";
//            this.barEditItemR_YKLB.Edit = this.repositoryItemTextEditR_YKLB;
//            this.barEditItemR_YKLB.Id = 19;
//            this.barEditItemR_YKLB.Name = "barEditItemR_YKLB";
//            this.barEditItemR_YKLB.Width = 80;
//            //
//            //repositoryItemTextEditV_ZBLB
//            //
//            this.repositoryItemTextEditR_YKLB.AutoHeight = false;
//            this.repositoryItemTextEditR_YKLB.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditR_YKLB.Name = "repositoryItemTextEditR_YKLB";
//            //
//            // barEditItemR_YKZL
//            //
//            this.barEditItemR_YKZL.Caption = "岩矿子类";
//            this.barEditItemR_YKZL.Edit = this.repositoryItemTextEditR_YKZL;
//            this.barEditItemR_YKZL.Id = 20;
//            this.barEditItemR_YKZL.Name = "barEditItemR_YKZL";
//            this.barEditItemR_YKZL.Width = 80;
//            //
//            //repositoryItemTextEditR_YKZL
//            //
//            this.repositoryItemTextEditR_YKZL.AutoHeight = false;
//            this.repositoryItemTextEditR_YKZL.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditR_YKZL.Name = "repositoryItemTextEditR_YKZL";
//            //
//            // barEditItemR_SSLB
//            //
//            this.barEditItemR_SSLB.Caption = "所属类别";
//            this.barEditItemR_SSLB.Edit = this.repositoryItemTextEditR_SSLB;
//            this.barEditItemR_SSLB.Id = 21;
//            this.barEditItemR_SSLB.Name = "barEditItemR_SSLB";
//            this.barEditItemR_SSLB.Width = 80;
//            //
//            //repositoryItemTextEditR_SSLB
//            //
//            this.repositoryItemTextEditR_SSLB.AutoHeight = false;
//            this.repositoryItemTextEditR_SSLB.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditR_SSLB.Name = "repositoryItemTextEditR_SSLB";
//            //
//            // barEditItemW_SYMC
//            //
//            this.barEditItemW_SYMC.Caption = "水域名称";
//            this.barEditItemW_SYMC.Edit = this.repositoryItemTextEditW_SYMC;
//            this.barEditItemW_SYMC.Id = 22;
//            this.barEditItemW_SYMC.Name = "barEditItemW_SYMC";
//            this.barEditItemW_SYMC.Width = 80;
//            //
//            //repositoryItemTextEditW_SYMC
//            //
//            this.repositoryItemTextEditW_SYMC.AutoHeight = false;
//            this.repositoryItemTextEditW_SYMC.Name = "repositoryItemTextEditW_SYMC";
//            //
//            // barEditItemW_SSLB
//            //
//            this.barEditItemW_SSLB.Caption = "所属类别";
//            this.barEditItemW_SSLB.Edit = this.repositoryItemTextEditW_SSLB;
//            this.barEditItemW_SSLB.Id = 23;
//            this.barEditItemW_SSLB.Name = "barEditItemW_SSLB";
//            this.barEditItemW_SSLB.Width = 80;
//            //
//            //repositoryItemTextEditW_SSLB
//            //
//            this.repositoryItemTextEditW_SSLB.AutoHeight = false;
//            this.repositoryItemTextEditW_SSLB.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemTextEditW_SSLB.Name = "repositoryItemTextEditW_SSLB";
//            //
//            // barEditItemW_GPYQ
//            //
//            this.barEditItemW_GPYQ.Caption = "光谱仪器";
//            this.barEditItemW_GPYQ.Edit = this.repositoryItemTextEditW_GPYQ;
//            this.barEditItemW_GPYQ.Id = 24;
//            this.barEditItemW_GPYQ.Name = "barEditItemW_GPYQ";
//            this.barEditItemW_GPYQ.Width = 80;
//            //
//            //repositoryItemTextEditW_GPYQ
//            //
//            this.repositoryItemTextEditW_GPYQ.AutoHeight = false;
//            this.repositoryItemTextEditW_GPYQ.Name = "repositoryItemTextEditW_GPYQ";
//            //
//            // barEditItemW_DLTZ
//            //
//            this.barEditItemW_DLTZ.Caption = "地理特征";
//            this.barEditItemW_DLTZ.Edit = this.repositoryItemTextEditW_DLTZ;
//            this.barEditItemW_DLTZ.Id = 25;
//            this.barEditItemW_DLTZ.Name = "barEditItemW_DLTZ";
//            this.barEditItemW_DLTZ.Width = 80;
//            //
//            //repositoryItemTextEditR_SSLB
//            //
//            this.repositoryItemTextEditW_DLTZ.AutoHeight = false;
//            this.repositoryItemTextEditW_DLTZ.Name = "repositoryItemTextEditW_DLTZ";
//            //
//            // barEditItemA_ZDMC
//            //
//            this.barEditItemA_ZDMC.Caption = "观测站名称";
//            this.barEditItemA_ZDMC.Edit = this.repositoryItemTextEditA_ZDMC;
//            this.barEditItemA_ZDMC.Id = 26;
//            this.barEditItemA_ZDMC.Name = "barEditItemA_ZDMC";
//            this.barEditItemA_ZDMC.Width = 80;
//            //
//            //repositoryItemTextEditA_ZDMC
//            //
//            this.repositoryItemTextEditA_ZDMC.AutoHeight = false;
//            this.repositoryItemTextEditA_ZDMC.Name = "repositoryItemTextEditA_ZDMC";
//            //
//            // barEditItemA_ZDBH
//            //
//            this.barEditItemA_ZDBH.Caption = "观测站编号";
//            this.barEditItemA_ZDBH.Edit = this.repositoryItemTextEditA_ZDBH;
//            this.barEditItemA_ZDBH.Id = 27;
//            this.barEditItemA_ZDBH.Name = "barEditItemA_ZDBH";
//            this.barEditItemA_ZDBH.Width = 80;
//            //
//            //repositoryItemTextEditR_SSLB
//            //
//            this.repositoryItemTextEditA_ZDBH.AutoHeight = false;
//            this.repositoryItemTextEditA_ZDBH.Name = "repositoryItemTextEditA_ZDBH";
//            // 
//            // rucSpectrumSearch
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Name = "rucSpectrumSearch";
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraBars.BarEditItem barEditItem_feature;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem_search;
//        /******************************************designer end**********************************/



//        private mucSpectrumSearch mucspectrumsearch;
//        //SoilService.SoilServicePortTypeClient soilclient;
//        //WaterService.WaterServicePortTypeClient waterclient;
//        //QRST_DI_MS_Desktop.AtmosphereService.AtmosphereServicePortTypeClient atmosphereclient;
//        //CityObjService.CityObjServicePortTypeClient cityobjclient;
//        //RockService.RockServicePortTypeClient rockclient;
//        //VNorthService.VegetationNorthServicePortTypeClient vnorthclient;
//        //VSouthService.VegetationSouthServicePortTypeClient vsouthclient;


//        public rucSpectrumSearch()
//        {
//            InitializeComponent();
//            //this.soilclient = new SoilService.SoilServicePortTypeClient("SoilServiceHttpSoap12Endpoint");
//            //this.waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
//            //this.atmosphereclient = new AtmosphereService.AtmosphereServicePortTypeClient("AtmosphereServiceHttpSoap12Endpoint");
//            //this.cityobjclient = new CityObjService.CityObjServicePortTypeClient("CityObjServiceHttpSoap12Endpoint");
//            //this.rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
//            //this.vnorthclient = new VNorthService.VegetationNorthServicePortTypeClient();
//            //this.vsouthclient = new VSouthService.VegetationSouthServicePortTypeClient();
//        }

//        public rucSpectrumSearch(object objmuc)
//            : base(objmuc)
//        {
//            InitializeComponent();

//            mucspectrumsearch = (mucSpectrumSearch)base.ObjMainUC;
//            //this.soilclient = new SoilService.SoilServicePortTypeClient("SoilServiceHttpSoap12Endpoint");
//            //this.waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
//            //this.atmosphereclient = new AtmosphereService.AtmosphereServicePortTypeClient("AtmosphereServiceHttpSoap12Endpoint");
//            //this.cityobjclient = new CityObjService.CityObjServicePortTypeClient("CityObjServiceHttpSoap12Endpoint");
//            //this.rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
//            //this.vnorthclient = new VNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap12Endpoint");
//            //this.vsouthclient = new VSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
//        }

//        private void barEditItem_feature_EditValueChanged(object sender, EventArgs e)
//        {
//            GetClassify classify;
//            gettype[] classifyfgw;
//            string[] strFields;
//            try
//            {
//                switch (this.barEditItem_feature.EditValue.ToString())
//                {
//                    case "南方植被":
//                        //第一次调用时初始化
//                        //if (vsouthclient == null)
//                        //{
//                        //    this.vsouthclient = new VSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
//                        //}
//                        this.ribbonPageGroup2.ItemLinks.Clear();
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemBeginDate, true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEndDate);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_ZBMC, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_ZBLB, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_CLBW, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_WHQ, false, "", "", true);
//                        classify =JSON.parse<GetClassify>(vsouthclient.getTypes());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditV_ZBLB, strFields);
//                        classify = JSON.parse<GetClassify>(vsouthclient.getCLBW());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditV_CLBW, strFields);
//                        classify = JSON.parse<GetClassify>(vsouthclient.getWHQ());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditV_WHQ, strFields);
//                        break;
//                    case "北方植被":
//                        if (vnorthclient == null)
//                        {
//                            this.vnorthclient = new VNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap12Endpoint");
//                        }
//                        this.ribbonPageGroup2.ItemLinks.Clear();
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemBeginDate);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEndDate);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_ZBMC, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_ZBLB, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_CLBW, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemV_WHQ, false, "", "", true);
//                        classify = JSON.parse<GetClassify>(vnorthclient.getTypes());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditV_ZBLB, strFields);
//                        classify = JSON.parse<GetClassify>(vnorthclient.getCLBW());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditV_CLBW, strFields);
//                        classify = JSON.parse<GetClassify>(vnorthclient.getWHQ());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditV_WHQ, strFields);
//                        break;
//                    case "城市目标":
//                        if (cityobjclient == null)
//                        {
//                            this.cityobjclient = new CityObjService.CityObjServicePortTypeClient("CityObjServiceHttpSoap12Endpoint");
//                        }
//                        this.ribbonPageGroup2.ItemLinks.Clear();
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemBeginDate, true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEndDate);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemCity_OBJname, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemCity_OBJkind, false, "", "", true);
//                        classify = JSON.parse<GetClassify>(cityobjclient.getTypes());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditCity_OBJkind, strFields);
//                        classify = JSON.parse<GetClassify>(cityobjclient.getCSMBMC());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditCity_OBJname, strFields);
//                        break;
//                    case "地表大气":
//                        if (atmosphereclient == null)
//                        {
//                            this.atmosphereclient = new AtmosphereService.AtmosphereServicePortTypeClient("AtmosphereServiceHttpSoap12Endpoint");
//                        }
//                        this.ribbonPageGroup2.ItemLinks.Clear();
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemA_ZDMC, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemA_ZDBH, false, "", "", true);
//                        classify = JSON.parse<GetClassify>(atmosphereclient.getZDBH());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditA_ZDBH, strFields);
//                        classify = JSON.parse<GetClassify>(atmosphereclient.getZDMC());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditA_ZDMC, strFields);
//                        break;
//                    case "水体":
//                        if (waterclient == null)
//                        {
//                            this.waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
//                        }
//                        this.ribbonPageGroup2.ItemLinks.Clear();
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemBeginDate, true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEndDate);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemW_SYMC, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemW_SSLB, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemW_DLTZ, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemW_GPYQ, false, "", "", true);
//                        classify = JSON.parse<GetClassify>(waterclient.getSSLB());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditW_SSLB, strFields);
//                        classify = JSON.parse<GetClassify>(waterclient.getGPYQ());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditW_GPYQ, strFields);
//                        classify = JSON.parse<GetClassify>(waterclient.getSYMC());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditW_SYMC, strFields);
//                        break;
//                    case "土壤":
//                        //第一次调用时初始化
//                        if (soilclient == null)
//                        {
//                            this.soilclient = new SoilService.SoilServicePortTypeClient("SoilServiceHttpSoap12Endpoint");
//                        }
//                        this.ribbonPageGroup2.ItemLinks.Clear();
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemBeginDate, true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEndDate);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemSoilName, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemSoilzilei, false, "", "", true);
//                        //this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemSoilfgwmc, true, "", "", true);
//                        classify = JSON.parse<GetClassify>(soilclient.getTypes());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditSoilzilei, strFields);
//                        break;
//                    case "岩矿":
//                        if (this.rockclient == null)
//                        {
//                            this.rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
//                        }
//                        this.ribbonPageGroup2.ItemLinks.Clear();
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemBeginDate, true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEndDate);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemR_YKMC, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemR_YKLB, false, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemR_YKZL, true, "", "", true);
//                        this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemR_SSLB, false, "", "", true);
//                        classify = JSON.parse<GetClassify>(rockclient.getSSLB());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditR_SSLB, strFields);
//                        classify = JSON.parse<GetClassify>(rockclient.getSubTypes());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditR_YKZL, strFields);
//                        classify = JSON.parse<GetClassify>(rockclient.getTypes());
//                        classifyfgw = classify.types;
//                        strFields = new string[classifyfgw.Length];
//                        for (int i = 0; i < classifyfgw.Length; i++)
//                        {
//                            strFields[i] = classifyfgw[i].type;
//                        }
//                        SetAvailableFields(repositoryItemTextEditR_YKLB, strFields);
//                        break;
//                    default:
//                        break;
//                }
//            }
//            catch(Exception ex)
//            {
//                MessageBox.Show("没有找到对应类型的数据!");
//            }
       
//        }

//        private void barButtonItemQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            DataTable dt = null;
//            string begindate = "";
//            string enddate = "";
//            string MaxLon;
//            string Maxlat;
//            string MinLon;
//            string MinLat;


//            try
//            {
//                List<object> fileinfolist = new List<object>();

//                switch (this.barEditItem_feature.EditValue.ToString())
//                {
//                    case "土壤":
//                        if (barEditItemBeginDate.EditValue != null)
//                        {
//                            begindate = ((DateTime)barEditItemBeginDate.EditValue).ToShortDateString();
//                        }
//                        if (barEditItemEndDate.EditValue != null)
//                        {
//                            enddate = ((DateTime)barEditItemEndDate.EditValue).ToShortDateString();
//                        }
//                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
//                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
//                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        string soilname = barEditItemSoilName.EditValue == null ? "" : barEditItemSoilName.EditValue.ToString();
//                        string soilzilei = barEditItemSoilzilei.EditValue == null ? "" : barEditItemSoilzilei.EditValue.ToString();
//                        string soilfgwmc = barEditItemSoilfgwmc.EditValue == null ? "" : barEditItemSoilfgwmc.EditValue.ToString();
//                        string jsonStr = soilclient.getQuerySoils(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, soilname, soilzilei, soilfgwmc, "0", "150");
//                        Soil soil = JSON.parse<Soil>(jsonStr);
//                        foreach (SoilInfo info in soil.soils)
//                        {
//                            fileinfolist.Add(info);
//                        }
//                        dt = QRST_DI_DS_MetadataQuery.JSONutilty.Utilties.ConvertObjPro2DataTable(fileinfolist);
//                        for (int i = 0; i < dt.Columns.Count; i++)
//                        {
//                            dt.Columns[i].ColumnName = Soil.soilattributenames[i];
//                        }
//                        break;
//                    case "南方植被":
//                        if (barEditItemBeginDate.EditValue != null)
//                        {
//                            begindate = ((DateTime)barEditItemBeginDate.EditValue).ToShortDateString();
//                        }
//                        if (barEditItemEndDate.EditValue != null)
//                        {
//                            enddate = ((DateTime)barEditItemEndDate.EditValue).ToShortDateString();
//                        }
//                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
//                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
//                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        string v_zbmc = barEditItemV_ZBMC.EditValue == null ? "" : barEditItemV_ZBMC.EditValue.ToString();
//                        string v_zblb = barEditItemV_ZBLB.EditValue == null ? "" : barEditItemV_ZBLB.EditValue.ToString();
//                        string v_clbw = barEditItemV_CLBW.EditValue == null ? "" : barEditItemV_CLBW.EditValue.ToString();
//                        string v_whq = barEditItemV_WHQ.EditValue == null ? "" : barEditItemV_WHQ.EditValue.ToString();
//                        //第一次调用时初始化
//                        if (vsouthclient == null)
//                        {
//                            this.vsouthclient = new VSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
//                        }
//                        Vegetation vegetation = QRST_DI_DS_MetadataQuery.JSONutilty.JSON.parse<Vegetation>(vsouthclient.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", ""));
//                        foreach (VegetationInfo info in vegetation.vegetations)
//                        {
//                            fileinfolist.Add(info);
//                        }
//                        dt = QRST_DI_DS_MetadataQuery.JSONutilty.Utilties.ConvertObjPro2DataTable(fileinfolist);
//                        for (int i = 0; i < dt.Columns.Count; i++)
//                        {
//                            dt.Columns[i].ColumnName = Vegetation.vegetationattributenames[i];
//                        }
//                        break;
//                    case "北方植被":
//                        if (barEditItemBeginDate.EditValue != null)
//                        {
//                            begindate = ((DateTime)barEditItemBeginDate.EditValue).ToShortDateString();
//                        }
//                        if (barEditItemEndDate.EditValue != null)
//                        {
//                            enddate = ((DateTime)barEditItemEndDate.EditValue).ToShortDateString();
//                        }
//                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
//                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
//                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        v_zbmc = barEditItemV_ZBMC.EditValue == null ? "" : barEditItemV_ZBMC.EditValue.ToString();
//                        v_zblb = barEditItemV_ZBLB.EditValue == null ? "" : barEditItemV_ZBLB.EditValue.ToString();
//                        v_clbw = barEditItemV_CLBW.EditValue == null ? "" : barEditItemV_CLBW.EditValue.ToString();
//                        v_whq = barEditItemV_WHQ.EditValue == null ? "" : barEditItemV_WHQ.EditValue.ToString();
//                        ////////////////////
                 
//                        vegetation = QRST_DI_DS_MetadataQuery.JSONutilty.JSON.parse<Vegetation>(vnorthclient.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", ""));
//                        foreach (VegetationInfo info in vegetation.vegetations)
//                        {
//                            fileinfolist.Add(info);
//                        }
//                        dt = QRST_DI_DS_MetadataQuery.JSONutilty.Utilties.ConvertObjPro2DataTable(fileinfolist);
//                        for (int i = 0; i < dt.Columns.Count; i++)
//                        {
//                            dt.Columns[i].ColumnName = Vegetation.vegetationattributenames[i];
//                        }
//                        break;
//                    case "城市目标":
//                        if (barEditItemBeginDate.EditValue != null)
//                        {
//                            begindate = ((DateTime)barEditItemBeginDate.EditValue).ToShortDateString();
//                        }
//                        if (barEditItemEndDate.EditValue != null)
//                        {
//                            enddate = ((DateTime)barEditItemEndDate.EditValue).ToShortDateString();
//                        }
//                        string c_csmc = barEditItemCity_OBJname.EditValue == null ? "" : barEditItemCity_OBJname.EditValue.ToString();
//                        string c_cslb = barEditItemCity_OBJkind.EditValue == null ? "" : barEditItemCity_OBJkind.EditValue.ToString();
//                        ////////////////////
                
//                        CityObj cityObj = QRST_DI_DS_MetadataQuery.JSONutilty.JSON.parse<CityObj>(cityobjclient.getQueryCityObjs(begindate, enddate, c_csmc, c_cslb, "", ""));
//                        foreach (CityObjInfo info in cityObj.city_objs)
//                        {
//                            fileinfolist.Add(info);
//                        }
//                        dt = QRST_DI_DS_MetadataQuery.JSONutilty.Utilties.ConvertObjPro2DataTable(fileinfolist);
//                        for (int i = 0; i < dt.Columns.Count; i++)
//                        {
//                            dt.Columns[i].ColumnName = CityObj.cityobjattributenames[i];
//                        }
//                        break;
//                    case "地表大气":
//                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
//                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
//                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        string a_zdmc = barEditItemA_ZDMC.EditValue == null ? "" : barEditItemA_ZDMC.EditValue.ToString();
//                        string a_zdbh = barEditItemA_ZDBH.EditValue == null ? "" : barEditItemA_ZDBH.EditValue.ToString();
//                        ////////////////////
                 
//                        Atmosphere atmosphere = QRST_DI_DS_MetadataQuery.JSONutilty.JSON.parse<Atmosphere>(atmosphereclient.getQueryAtmospheres(Maxlat, MaxLon, MinLat, MinLon, a_zdmc, a_zdbh, "", ""));
//                        foreach (AtmosphereInfo info in atmosphere.atmospheres)
//                        {
//                            fileinfolist.Add(info);
//                        }
//                        dt = QRST_DI_DS_MetadataQuery.JSONutilty.Utilties.ConvertObjPro2DataTable(fileinfolist);
//                        for (int i = 0; i < dt.Columns.Count; i++)
//                        {
//                            dt.Columns[i].ColumnName = Atmosphere.atmosphereattributenames[i];
//                        }
//                        break;
//                    case "水体":
//                        if (barEditItemBeginDate.EditValue != null)
//                        {
//                            begindate = ((DateTime)barEditItemBeginDate.EditValue).ToShortDateString();
//                        }
//                        if (barEditItemEndDate.EditValue != null)
//                        {
//                            enddate = ((DateTime)barEditItemEndDate.EditValue).ToShortDateString();
//                        }
//                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
//                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
//                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        string w_symc = barEditItemW_SYMC.EditValue == null ? "" : barEditItemW_SYMC.EditValue.ToString();
//                        string w_sslb = barEditItemW_SSLB.EditValue == null ? "" : barEditItemW_SSLB.EditValue.ToString();
//                        string w_dltz = barEditItemW_DLTZ.EditValue == null ? "" : barEditItemW_DLTZ.EditValue.ToString();
//                        string w_gpyq = barEditItemW_GPYQ.EditValue == null ? "" : barEditItemW_GPYQ.EditValue.ToString();
//                        ////////////////////
               
//                        Water water = QRST_DI_DS_MetadataQuery.JSONutilty.JSON.parse<Water>(waterclient.getQueryWaters(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, w_symc, w_gpyq, w_sslb, "", ""));
//                        foreach (WaterInfo info in water.waters)
//                        {
//                            fileinfolist.Add(info);
//                        }
//                        dt = QRST_DI_DS_MetadataQuery.JSONutilty.Utilties.ConvertObjPro2DataTable(fileinfolist);
//                        for (int i = 0; i < dt.Columns.Count; i++)
//                        {
//                            dt.Columns[i].ColumnName = Water.waterattributenames[i];
//                        }
//                        break;
//                    case "岩矿":
//                        if (barEditItemBeginDate.EditValue != null)
//                        {
//                            begindate = ((DateTime)barEditItemBeginDate.EditValue).ToShortDateString();
//                        }
//                        if (barEditItemEndDate.EditValue != null)
//                        {
//                            enddate = ((DateTime)barEditItemEndDate.EditValue).ToShortDateString();
//                        }
//                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
//                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
//                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
//                        string r_ykmc = barEditItemR_YKMC.EditValue == null ? "" : barEditItemR_YKMC.EditValue.ToString();
//                        string r_yklb = barEditItemR_YKLB.EditValue == null ? "" : barEditItemR_YKLB.EditValue.ToString();
//                        string r_ykzl = barEditItemR_YKZL.EditValue == null ? "" : barEditItemR_YKZL.EditValue.ToString();
//                        string r_sslb = barEditItemR_SSLB.EditValue == null ? "" : barEditItemR_SSLB.EditValue.ToString();
//                        ////////////////////
//                        RockMineral rockmineral = QRST_DI_DS_MetadataQuery.JSONutilty.JSON.parse<RockMineral>(rockclient.getQueryRocks(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, r_ykmc, r_yklb, r_ykzl, r_sslb, "", ""));
//                        foreach (RockMineralInfo info in rockmineral.rocks)
//                        {
//                            fileinfolist.Add(info);
//                        }
//                        dt = QRST_DI_DS_MetadataQuery.JSONutilty.Utilties.ConvertObjPro2DataTable(fileinfolist);
//                        for (int i = 0; i < dt.Columns.Count; i++)
//                        {
//                            dt.Columns[i].ColumnName = RockMineral.rockMineralattributenames[i];
//                        }
//                        break;
//                    default:
//                        MessageBox.Show("请选择地物类型！");
//                        break;
//                }

//                mucspectrumsearch.gridViewMain.Columns.Clear();
//                //mucspectrumsearch.ctrlPage1.gridControl = mucspectrumsearch.gridControlTable;
//                mucspectrumsearch.ctrlPage1.dt = dt;
//                mucspectrumsearch.ctrlPage1._recordNum = dt.Rows.Count;
//                mucspectrumsearch.ctrlPage1.Query();
//            }
//            catch (System.Exception ex)
//            {
//                MessageBox.Show("无法获取查询数据!");
//                mucspectrumsearch.gridViewMain.Columns.Clear();
//            }

//        }


//        void SetAvailableFields(DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryitem, string[] strFields)
//        {
//            repositoryitem.Items.Clear();
//            repositoryitem.Items.AddRange(strFields);
//        }
//    }
//}
