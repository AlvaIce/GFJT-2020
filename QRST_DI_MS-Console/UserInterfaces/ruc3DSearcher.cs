using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using QRST_DI_MS_Console.QueryInner;
using QRST_DI_MS_Basis.QueryBase;
using DevExpress.XtraEditors;
using System.Data;
using QRST_DI_MS_Console.WS_QDB_Searcher_MySQL;
using System.Xml;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_MetadataQuery;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using QRST_DI_MS_Console.JSON;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using DevExpress.XtraBars;
using Qrst;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class ruc3DSearcher : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarEditItem barEditItemBeginDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEditBeginTime;
        private DevExpress.XtraBars.BarEditItem barEditItemEndDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEditEndTime;
        private DevExpress.XtraBars.BarEditItem barEditItemMaxLon;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMaxLon;
        private DevExpress.XtraBars.BarEditItem barEditItemMinLon;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMinLon;
        private DevExpress.XtraBars.BarEditItem barEditItemMinLat;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMinLat;
        private DevExpress.XtraBars.BarEditItem barEditItemMaxLat;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMaxLat;
        private DevExpress.XtraBars.BarEditItem barEditItemMaxCol;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMaxCol;
        private DevExpress.XtraBars.BarEditItem barEditItemMinCol;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMinCol;
        private DevExpress.XtraBars.BarEditItem barEditItemMinRow;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMinRow;
        private DevExpress.XtraBars.BarEditItem barEditItemMaxRow;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditMaxRow;
        private DevExpress.XtraBars.BarEditItem barEditItemSensorCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditSensors;
        private DevExpress.XtraBars.BarEditItem barEditItemSateCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditSates;
        private DevExpress.XtraBars.BarButtonItem barButtonItemQuery;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupOperation;
        private DevExpress.XtraBars.BarEditItem barEditItem9;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemHandleSel;
        private DevExpress.XtraBars.BarEditItem barEditItemDataTypeCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditDataType;
        private DevExpress.XtraBars.BarEditItem barEditItemServiceTypeCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditServiceType;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditAreaName;
        private DevExpress.XtraBars.BarEditItem barEditItemKeyWord;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditKeyWords;
        private System.ComponentModel.IContainer components;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupContainerEditDatatype;

        public muc3DSearcher mucsearcher;
        private frm_MSConsole parentMSconsole;
        private DevExpress.XtraBars.BarEditItem barEditItemArea;
        private DevExpress.XtraBars.BarEditItem barEditItemDocFileType;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditDocFileType;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupCustomQuery;
        private DevExpress.XtraBars.BarEditItem barEditItemFieldList;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxFields;
        private DevExpress.XtraBars.BarEditItem barEditItemFieldOperator;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxOperators;
        private DevExpress.XtraBars.BarEditItem barEditItemFieldValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditFieldsValue;
        private DevExpress.XtraBars.BarEditItem barEditItemLogicalOperator;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxLogicalOperator;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAddNewQuery;
        private DevExpress.XtraBars.BarEditItem barEditItemShowQuery;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEditShow;
        private DevExpress.XtraBars.BarStaticItem barStaticItemTishi;

        MSUserInterface msUCDetail;
        private DevExpress.XtraBars.BarButtonItem barButtonItemClearCustom;
        private ImageList imageListRucSearch;
        private DevExpress.XtraBars.BarButtonItem barButtonItemShowCustom;
        private DevExpress.XtraBars.BarButtonItem barButtonEarthViewInitial;
        MSUserInterface msUCMaintain;
        private DevExpress.XtraBars.BarEditItem barEditName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barEditKeyWords;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;

        private DevExpress.XtraBars.BarEditItem barEditItemSoilName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraBars.BarEditItem barEditItemSoliSubType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSoilSubType;
        private DevExpress.XtraBars.BarEditItem barEditItemRockName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraBars.BarEditItem barEditItemRockType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxRockType;
        private DevExpress.XtraBars.BarEditItem barEditItemRockSubType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxRockSubType;
        private DevExpress.XtraBars.BarEditItem barEditItemRockAttribute;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxRockAttribute;
        private DevExpress.XtraBars.BarEditItem barEditItemPlantType;
        private DevExpress.XtraBars.BarEditItem barEditItemPlantName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit6;
        private DevExpress.XtraBars.BarEditItem barEditItemPlantPosition;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxPlantPosition;
        private DevExpress.XtraBars.BarEditItem barEditItemPlantTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxPlantTime;
        private DevExpress.XtraBars.BarEditItem barEditItemCityName;
        private DevExpress.XtraBars.BarEditItem barEditItemCityType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxCityType;
        private DevExpress.XtraBars.BarEditItem barEditItemAtmosphereName;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxAtmosphereName;
        private DevExpress.XtraBars.BarEditItem barEditItemAtmosphereCode;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxAtmosphereCode;
        private DevExpress.XtraBars.BarEditItem barEditItemWaterName;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxWaterName;
        private DevExpress.XtraBars.BarEditItem barEditItemWaterType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxWaterType;
        private DevExpress.XtraBars.BarEditItem barEditItemWaterFactor;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit8;
        private DevExpress.XtraBars.BarEditItem barEditItemWaterYiQI;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxWaterYiQI;                                                  //查询返回的结果

        #region 后台定义字段
        metadatacatalognode_Mdl selectedQueryObj = null;                  //存储需要查询的数据类型对象 zxw 2013/08/05
        public static IGetQuerySchema querySchema = null;                                                //全局查询方案

        QRST_DI_DS_MetadataQuery.QueryRequest queryRequest;                                                       //查询请求对象
        QRST_DI_DS_MetadataQuery.QueryResponse queryResponse;
        string SpacialInfo = "";
        DateTime startTime;                        //查询中的起始时间
        DateTime endTime;                          //查询中的结束时间
        private DateTime lastSearchTime; //最后查询时间
        private int queryNum;//查询次数
        QueryPara queryPara;                                                        //查询参数对象
        mucDetailViewer mucdetail;                                                  //详细信息列表空间
        List<QRST_DI_DS_MetadataQuery.SimpleCondition> listSimpleCondistons = new List<QRST_DI_DS_MetadataQuery.SimpleCondition>(); //记录高级检索条件

        //定义遥感应用特征库查询服务对象
        SoilService.SoilServicePortTypeClient soilclient;
        WaterService.WaterServicePortTypeClient waterclient;
        AtmosphereService.AtmosphereServicePortTypeClient atmosphereclient;
        CityObjService.CityObjServicePortTypeClient cityobjclient;
        RockService.RockServicePortTypeClient rockclient;
        VNorthService.VegetationNorthServicePortTypeClient vnorthclient;
        VSouthService.VegetationSouthServicePortTypeClient vsouthclient;
        localhostSqlite.Service sqliteclient;
        //IIS发布查询服务
        private Service mySQLSerVice;
        private string jsonStr;
        GetClassify classify;
        gettype[] classifyfgw;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxPlantType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxCityName;
        private DevExpress.XtraBars.BarEditItem barEditItemTileLevel;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditTileLevel;
        private DevExpress.XtraBars.BarEditItem barEditItemProTypeCombo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditProType;
        private DevExpress.XtraBars.BarEditItem barEditItemNumPerPage;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem barEditItemTimerTrackBar;
        private DevExpress.XtraEditors.Repository.RepositoryItemRangeTrackBar repositoryItemRangeTrackBar1;
        private DevExpress.XtraBars.BarEditItem barEditItemCloudPercent;
        private DevExpress.XtraEditors.Repository.RepositoryItemRangeTrackBar repositoryItemRangeTrackBar2;
        private DevExpress.XtraBars.BarEditItem barEditItemManfuPercent;
        private DevExpress.XtraEditors.Repository.RepositoryItemRangeTrackBar repositoryItemRangeTrackBar3;
        private DevExpress.XtraBars.BarEditItem barEditItemSpacialInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSpatialChoose;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarEditItem barEditItemProvince;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxProvince;
        private DevExpress.XtraBars.BarEditItem barEditItemUrban;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxCity;
        private DevExpress.XtraBars.BarEditItem barEditItemCounty;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxCounty;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemLoadPositionFile;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox6;
        private BarButtonItem barButtonItem2D;
        private BarButtonItem barButtonItem3D;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private BarButtonItem barButtonItemRaster;
        private BarButtonItem barButtonItemVector;
        string[] strFields;
        private BarStaticItem barStaticItem4;
        //private GroupControl groupControl1;
        private DevExpress.XtraBars.BarEditItem SearchRule;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup rioGroup;
        private DevExpress.XtraEditors.Controls.RadioGroupItem radioItem1;
        private DevExpress.XtraEditors.Controls.RadioGroupItem radioItem2;
        private DataSet ExtentDs;
        #endregion
        public static QRST_DI_DS_MetadataQuery.Rule rule = QRST_DI_DS_MetadataQuery.Rule.Intersect;
        public static double[] selectedRect = new double[] { -180, -90, 180, 90 };
        public ruc3DSearcher()
            : base()
        {
            InitializeComponent();
        }

        public ruc3DSearcher(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
        }

        public ruc3DSearcher(object objMUC, object parentControl)
            : base(objMUC)
        {
            InitializeComponent();
            mucsearcher = objMUC as muc3DSearcher;
            mucsearcher.TreeSelete += new muc3DSearcher.TreeSeleteDeletege(barEditItemSelDataType_EditValueChanged);
            mucsearcher.Show3DViewer();
            lastSearchTime = DateTime.Now;
            queryNum = 0;
            parentMSconsole = parentControl as frm_MSConsole;
            this.mucsearcher.qrstAxGlobeControl1.OnDrawRectangleCompeleted += new EventHandler(qrstAxGlobeControl1_OnDrawRectangleCompeleted);
            this.mucsearcher.uc2DSearcher1.SizeChanged += new EventHandler(uc2DSearcher1_SizeChanged);
            ////加载数据类型树
            //for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
            //{
            //    TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
            //    if (tn != null)
            //    {
            //        treeViewDataType.Nodes.Add(tn);
            //    }
            //}
            //treeViewDataType.ExpandAll();
            setQueryDisabled();
            barEditItemSelDataType_EditValueChanged();
            int maxCount = 0;
        }

        //zxw 20131222 当显示详细信息时，是否需要重新绘制切片范围
        void mucdetail_VisibleChanged(object sender, EventArgs e)
        {
            if (selectedQueryObj != null && selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") && selectedQueryObj.NAME.Equals("规格化影像数据") && queryResponse != null && queryResponse.recordSet != null)
            {
                if (mucdetail.Visible)
                {
                    /* 151031 从检索屏到下载屏不用刷新地球
                    ClearExtentLyr();
                    Dictionary<System.Drawing.RectangleF, int> extents1;
                    extents1 = GetQueryTileStasticExtentInfo();
                    int maxCount = 0;
                    this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents1, out maxCount);
                    ColorRange colorRange = new ColorRange(maxCount, this.mucsearcher.qrstAxGlobeControl1);
                    colorRange.Visible = this.mucsearcher.qrstAxGlobeControl1.IsOn("tmpDrawExtentsLayer1");
                    //添加统计色带
                    if (this.mucsearcher.qrstAxGlobeControl1.Controls.ContainsKey("colorRange"))
                    {
                        this.mucsearcher.qrstAxGlobeControl1.Controls.RemoveByKey("colorRange");
                    }
                    this.mucsearcher.qrstAxGlobeControl1.Controls.Add(colorRange);
                */
                 }
            }
        }

        //zxw 20131222 当显示三维球界面时，是否需要重新绘制切片范围
        void mucsearcher_VisibleChanged(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            //if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))   //切片数据查询
            //    {
            //        queryPara = new RasterQueryPara();
            //        queryPara.QRST_CODE = "TileFileName";
            //        if (selectedQueryObj.NAME.Equals("规格化影像数据"))
            //        {
            if (selectedQueryObj != null && selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") && selectedQueryObj.NAME.Equals("规格化影像数据") && distinctTileInfo != null)
            {
                if (mucsearcher.Visible)
                {
                    /*
                    ClearExtentLyr();
                    Dictionary<System.Drawing.RectangleF, int> extents1;
                    extents1 = GetTileStasticExtentInfo(distinctTileInfo);
                    int maxCount = 0;
                    this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents1, out maxCount);
                    ColorRange colorRange = new ColorRange(maxCount, this.mucsearcher.qrstAxGlobeControl1);
                    colorRange.Visible = this.mucsearcher.qrstAxGlobeControl1.IsOn("tmpDrawExtentsLayer1");
                    //添加统计色带
                    if (this.mucsearcher.qrstAxGlobeControl1.Controls.ContainsKey("colorRange"))
                    {
                        this.mucsearcher.qrstAxGlobeControl1.Controls.RemoveByKey("colorRange");
                    }
                    this.mucsearcher.qrstAxGlobeControl1.Controls.Add(colorRange);
                */
                }
            }

        }

        void uc2DSearcher1_SizeChanged(object sender, EventArgs e)
        {
            int[] size = new int[2];
            size[0] = this.mucsearcher.uc2DSearcher1.Width;
            size[1] = this.mucsearcher.uc2DSearcher1.Height;
            mucsearcher.uc2DSearcher1.setSize(size);
        }
        /// <summary>
        /// 绘制矩形框完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void qrstAxGlobeControl1_OnDrawRectangleCompeleted(object sender, EventArgs e)
        {

            selectedRect = this.mucsearcher.qrstAxGlobeControl1.GetSelectRectangle();
            barEditItemMinLon.EditValue = Convert.ToDouble(Math.Min(selectedRect[0], selectedRect[2])).ToString("0.00");
            barEditItemMaxLat.EditValue = Convert.ToDouble(Math.Max(selectedRect[1], selectedRect[3])).ToString("0.00");
            barEditItemMaxLon.EditValue = Convert.ToDouble(Math.Max(selectedRect[0], selectedRect[2])).ToString("0.00");
            barEditItemMinLat.EditValue = Convert.ToDouble(Math.Min(selectedRect[1], selectedRect[3])).ToString("0.00");

            if (this.Cursor != Cursors.Default)
            {
                this.Cursor = Cursors.Default;
            }
        }
        void qrstAxGlobeControl1_OnDrawRectangleCompeleted()
        {
            //double[] selectedRect = new double[] { 0, 0, 0, 0 };
            selectedRect = this.mucsearcher.uc2DSearcher1.GetSelectRectangle();
            barEditItemMinLon.EditValue = Convert.ToDouble(Math.Min(selectedRect[0], selectedRect[1])).ToString("0.00");
            barEditItemMaxLat.EditValue = Convert.ToDouble(Math.Max(selectedRect[2], selectedRect[3])).ToString("0.00");
            barEditItemMaxLon.EditValue = Convert.ToDouble(Math.Max(selectedRect[0], selectedRect[1])).ToString("0.00");
            barEditItemMinLat.EditValue = Convert.ToDouble(Math.Min(selectedRect[2], selectedRect[3])).ToString("0.00");

            if (this.Cursor != Cursors.Default)
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ruc3DSearcher));
            this.barStaticItemTishi = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItemMaxLon = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMaxLon = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemMinLon = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMinLon = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemMaxLat = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMaxLat = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemMinLat = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMinLat = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();

            this.barEditItemMaxRow = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMaxRow = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemMinRow = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMinRow = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemMaxCol = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMaxCol = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemMinCol = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditMinCol = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();

            this.barButtonItemHandleSel = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemShowQuery = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemMemoEditShow = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.barEditItemBeginDate = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEditBeginTime = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItemEndDate = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEditEndTime = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItemSateCheck = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEditSates = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barEditItemSensorCheck = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEditSensors = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barEditItemDataTypeCheck = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEditDataType = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barEditItemServiceTypeCheck = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEditServiceType = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barEditItemArea = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditAreaName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemKeyWord = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditKeyWords = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemDocFileType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEditDocFileType = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barEditItem9 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.ribbonPageGroupOperation = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemQuery = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemShowCustom = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemNumPerPage = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox6 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonEarthViewInitial = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemPopupContainerEditDatatype = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.ribbonPageGroupCustomQuery = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemLogicalOperator = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxLogicalOperator = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemFieldOperator = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxOperators = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemFieldList = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxFields = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemFieldValue = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditFieldsValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItemAddNewQuery = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemClearCustom = new DevExpress.XtraBars.BarButtonItem();
            this.imageListRucSearch = new System.Windows.Forms.ImageList(this.components);
            this.barEditName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditKeyWords = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemSoilName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemSoliSubType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxSoilSubType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemRockName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemRockType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxRockType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemRockSubType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxRockSubType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemRockAttribute = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxRockAttribute = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemPlantType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxPlantType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemPlantName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemPlantPosition = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxPlantPosition = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemPlantTime = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxPlantTime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemCityName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxCityName = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemCityType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxCityType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemAtmosphereName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxAtmosphereName = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemAtmosphereCode = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxAtmosphereCode = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemWaterName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxWaterName = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemWaterType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxWaterType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemWaterFactor = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemWaterYiQI = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxWaterYiQI = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemTileLevel = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEditTileLevel = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barEditItemProTypeCombo = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckedComboBoxEditProType = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barEditItemTimerTrackBar = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemRangeTrackBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemRangeTrackBar();
            this.barEditItemCloudPercent = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemRangeTrackBar2 = new DevExpress.XtraEditors.Repository.RepositoryItemRangeTrackBar();
            this.barEditItemManfuPercent = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemRangeTrackBar3 = new DevExpress.XtraEditors.Repository.RepositoryItemRangeTrackBar();
            this.barEditItemSpacialInfo = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxSpatialChoose = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItemProvince = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxProvince = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemUrban = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxCity = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemCounty = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxCounty = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItemLoadPositionFile = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2D = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3D = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemRaster = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemVector = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.SearchRule = new BarEditItem();
            this.rioGroup = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            radioItem1 = new DevExpress.XtraEditors.Controls.RadioGroupItem();
            radioItem2 = new DevExpress.XtraEditors.Controls.RadioGroupItem();

            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEditShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditBeginTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditBeginTime.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditEndTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditEndTime.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditSates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditSensors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditServiceType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditAreaName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditKeyWords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditDocFileType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEditDatatype)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxLogicalOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxOperators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditFieldsValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSoilSubType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxRockType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxRockSubType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxRockAttribute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPlantType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPlantPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPlantTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCityName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCityType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxAtmosphereName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxAtmosphereCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxWaterName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxWaterType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxWaterYiQI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditTileLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditProType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSpatialChoose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxProvince)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCounty)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Images = this.imageListRucSearch;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barEditItemBeginDate,
            this.barEditItemEndDate,
            this.barEditItemMaxLon,
            this.barEditItemMinLon,
            this.barEditItemMinLat,
            this.barEditItemMaxLat,
            this.barEditItemSensorCheck,
            this.barEditItemSateCheck,
            this.barButtonItemQuery,
            this.barButtonItemHandleSel,
            this.barEditItemDataTypeCheck,
            this.barEditItemServiceTypeCheck,
            this.barEditItemKeyWord,
            this.barEditItemArea,
            this.barEditItemDocFileType,
            this.barEditItemFieldList,
            this.barEditItemFieldOperator,
            this.barEditItemFieldValue,
            this.barEditItemLogicalOperator,
            this.barButtonItemAddNewQuery,
            this.barEditItemShowQuery,
            this.barStaticItemTishi,
            this.barButtonItemClearCustom,
            this.barButtonItemShowCustom,
            this.barButtonEarthViewInitial,
            this.barEditName,
            this.barEditKeyWords,
            this.barEditItemSoilName,
            this.barEditItemSoliSubType,
            this.barEditItemRockName,
            this.barEditItemRockType,
            this.barEditItemRockSubType,
            this.barEditItemRockAttribute,
            this.barEditItemPlantType,
            this.barEditItemPlantName,
            this.barEditItemPlantPosition,
            this.barEditItemPlantTime,
            this.barEditItemCityName,
            this.barEditItemCityType,
            this.barEditItemAtmosphereName,
            this.barEditItemAtmosphereCode,
            this.barEditItemWaterName,
            this.barEditItemWaterType,
            this.barEditItemWaterFactor,
            this.barEditItemWaterYiQI,
            this.barEditItemTileLevel,
            this.barEditItemProTypeCombo,
            this.barEditItemNumPerPage,
            this.barEditItemTimerTrackBar,
            this.barEditItemCloudPercent,
            this.barEditItemManfuPercent,
            this.barEditItemSpacialInfo,
            this.barStaticItem1,
            this.barEditItemProvince,
            this.barEditItemUrban,
            this.barEditItemCounty,
            this.barStaticItem2,
            this.barButtonItemLoadPositionFile,
            this.barButtonItem2D,
            this.barButtonItem3D,
            this.barButtonItemRaster,
            this.barButtonItemVector,
            this.barStaticItem4});
            this.ribbonControl1.MaxItemId = 123;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEditBeginTime,
            this.repositoryItemDateEditEndTime,
            this.repositoryItemTextEditMaxLon,
            this.repositoryItemTextEditMinLon,
            this.repositoryItemTextEditMinLat,
            this.repositoryItemTextEditMaxLat,
            this.repositoryItemCheckedComboBoxEditSensors,
            this.repositoryItemCheckedComboBoxEditSates,
            this.repositoryItemCheckedComboBoxEditDataType,
            this.repositoryItemCheckedComboBoxEditServiceType,
            this.repositoryItemTextEditAreaName,
            this.repositoryItemTextEditKeyWords,
            this.repositoryItemPopupContainerEditDatatype,
            this.repositoryItemCheckedComboBoxEditDocFileType,
            this.repositoryItemComboBoxFields,
            this.repositoryItemComboBoxOperators,
            this.repositoryItemTextEditFieldsValue,
            this.repositoryItemComboBoxLogicalOperator,
            this.repositoryItemMemoEditShow,
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit3,
            this.repositoryItemComboBoxSoilSubType,
            this.repositoryItemTextEdit4,
            this.repositoryItemComboBoxRockType,
            this.repositoryItemComboBoxRockSubType,
            this.repositoryItemComboBoxRockAttribute,
            this.repositoryItemTextEdit6,
            this.repositoryItemComboBoxPlantPosition,
            this.repositoryItemComboBoxPlantTime,
            this.repositoryItemComboBoxCityType,
            this.repositoryItemComboBoxAtmosphereName,
            this.repositoryItemComboBoxAtmosphereCode,
            this.repositoryItemComboBoxWaterName,
            this.repositoryItemComboBoxWaterType,
            this.repositoryItemTextEdit8,
            this.repositoryItemComboBoxWaterYiQI,
            this.repositoryItemComboBoxPlantType,
            this.repositoryItemComboBoxCityName,
            this.repositoryItemCheckedComboBoxEditTileLevel,
            this.repositoryItemCheckedComboBoxEditProType,
            this.repositoryItemComboBox1,
            this.repositoryItemRangeTrackBar1,
            this.repositoryItemRangeTrackBar2,
            this.repositoryItemRangeTrackBar3,
            this.repositoryItemComboBoxSpatialChoose,
            this.repositoryItemComboBoxProvince,
            this.repositoryItemComboBoxCity,
            this.repositoryItemComboBoxCounty,
            this.repositoryItemComboBox6});
            this.ribbonControl1.Size = new System.Drawing.Size(1400, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupCustomQuery,
            this.ribbonPageGroupOperation,
            this.ribbonPageGroup2});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barStaticItemTishi);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "基础检索条件";
            // 
            // barStaticItemTishi
            // 
            this.barStaticItemTishi.Caption = "请选择数据类型进行检索！";
            this.barStaticItemTishi.Id = 62;
            this.barStaticItemTishi.Name = "barStaticItemTishi";
            this.barStaticItemTishi.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItemMaxLon
            // 
            this.barEditItemMaxLon.Caption = "最大经度";
            this.barEditItemMaxLon.Edit = this.repositoryItemTextEditMaxLon;
            this.barEditItemMaxLon.Id = 5;
            this.barEditItemMaxLon.Name = "barEditItemMaxLon";
            this.barEditItemMaxLon.Width = 80;
            // 
            // repositoryItemTextEditMaxLon
            // 
            this.repositoryItemTextEditMaxLon.AutoHeight = false;
            this.repositoryItemTextEditMaxLon.Name = "repositoryItemTextEditMaxLon";
            // 
            // barEditItemMinLon
            // 
            this.barEditItemMinLon.Caption = "最小经度";
            this.barEditItemMinLon.Edit = this.repositoryItemTextEditMinLon;
            this.barEditItemMinLon.Id = 6;
            this.barEditItemMinLon.Name = "barEditItemMinLon";
            this.barEditItemMinLon.Width = 80;
            // 
            // repositoryItemTextEditMinLon
            // 
            this.repositoryItemTextEditMinLon.AutoHeight = false;
            this.repositoryItemTextEditMinLon.Name = "repositoryItemTextEditMinLon";
            // 
            // barEditItemMaxLat
            // 
            this.barEditItemMaxLat.Caption = "最大纬度";
            this.barEditItemMaxLat.Edit = this.repositoryItemTextEditMaxLat;
            this.barEditItemMaxLat.Id = 8;
            this.barEditItemMaxLat.Name = "barEditItemMaxLat";
            this.barEditItemMaxLat.Width = 80;
            // 
            // repositoryItemTextEditMaxLat
            // 
            this.repositoryItemTextEditMaxLat.AutoHeight = false;
            this.repositoryItemTextEditMaxLat.Name = "repositoryItemTextEditMaxLat";
            // 
            // barEditItemMinLat
            // 
            this.barEditItemMinLat.Caption = "最小纬度";
            this.barEditItemMinLat.Edit = this.repositoryItemTextEditMinLat;
            this.barEditItemMinLat.Id = 7;
            this.barEditItemMinLat.Name = "barEditItemMinLat";
            this.barEditItemMinLat.Width = 80;

            // 
            // repositoryItemTextEditMinLat
            // 
            this.repositoryItemTextEditMinLat.AutoHeight = false;
            this.repositoryItemTextEditMinLat.Name = "repositoryItemTextEditMinLat";
            // 
            // barButtonItemHandleSel
            // 
            this.barButtonItemHandleSel.Caption = "手动选取";
            this.barButtonItemHandleSel.Id = 14;
            this.barButtonItemHandleSel.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.手动选取;
            this.barButtonItemHandleSel.Name = "barButtonItemHandleSel";
            this.barButtonItemHandleSel.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemHandleSel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemHandleSel_ItemClick);
            // 
            // barEditItemShowQuery
            // 
            this.barEditItemShowQuery.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.barEditItemShowQuery.Appearance.Options.UseBackColor = true;
            this.barEditItemShowQuery.Edit = this.repositoryItemMemoEditShow;
            this.barEditItemShowQuery.EditHeight = 70;
            this.barEditItemShowQuery.EditValue = "";
            this.barEditItemShowQuery.Id = 60;
            this.barEditItemShowQuery.Name = "barEditItemShowQuery";
            this.barEditItemShowQuery.Width = 160;
            this.barEditItemShowQuery.EditValueChanged += new System.EventHandler(this.barEditItemShowQuery_EditValueChanged);
            // 
            // repositoryItemMemoEditShow
            // 
            this.repositoryItemMemoEditShow.Name = "repositoryItemMemoEditShow";
            // 
            // barEditItemBeginDate
            // 
            this.barEditItemBeginDate.Caption = "开始时间";
            this.barEditItemBeginDate.Edit = this.repositoryItemDateEditBeginTime;
            this.barEditItemBeginDate.Id = 3;
            this.barEditItemBeginDate.Name = "barEditItemBeginDate";
            this.barEditItemBeginDate.Width = 150;
            this.barEditItemBeginDate.EditValueChanged += new System.EventHandler(this.barEditItemBeginDate_EditValueChanged);
            // 
            // repositoryItemDateEditBeginTime
            // 
            this.repositoryItemDateEditBeginTime.AutoHeight = false;
            this.repositoryItemDateEditBeginTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEditBeginTime.EditFormat.FormatString = "f";
            this.repositoryItemDateEditBeginTime.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEditBeginTime.Name = "repositoryItemDateEditBeginTime";
            this.repositoryItemDateEditBeginTime.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // barEditItemEndDate
            // 
            this.barEditItemEndDate.Caption = "结束时间";
            this.barEditItemEndDate.Edit = this.repositoryItemDateEditEndTime;
            this.barEditItemEndDate.Id = 4;
            this.barEditItemEndDate.Name = "barEditItemEndDate";
            this.barEditItemEndDate.Width = 150;
            this.barEditItemEndDate.EditValueChanged += new System.EventHandler(this.barEditItemEndDate_EditValueChanged);
            // 
            // repositoryItemDateEditEndTime
            // 
            this.repositoryItemDateEditEndTime.AutoHeight = false;
            this.repositoryItemDateEditEndTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEditEndTime.EditFormat.FormatString = "f";
            this.repositoryItemDateEditEndTime.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEditEndTime.Name = "repositoryItemDateEditEndTime";
            this.repositoryItemDateEditEndTime.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});

            this.SearchRule.Caption = "检索规则";
            this.SearchRule.Edit = this.rioGroup;
            this.SearchRule.Id = 5;
            this.SearchRule.Name = "barEditSearchRule";
            this.SearchRule.Width = 150;

            this.rioGroup.AutoHeight = false;
            //this.radioItem1.Value = "相交";
            this.radioItem1.Description = "相交";
            this.radioItem2.Value = "包含";
            this.radioItem2.Description = "包含";
            this.rioGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] { radioItem1, radioItem2 });
            this.rioGroup.SelectedIndexChanged += new EventHandler(rioGroup_SelectedIndexChanged);
            // 
            // barEditItemSateCheck
            // 
            this.barEditItemSateCheck.Caption = " 卫星 ";
            this.barEditItemSateCheck.Edit = this.repositoryItemCheckedComboBoxEditSates;
            this.barEditItemSateCheck.Id = 11;
            this.barEditItemSateCheck.Name = "barEditItemSateCheck";
            this.barEditItemSateCheck.Width = 60;
            // 
            // repositoryItemCheckedComboBoxEditSates
            // 
            this.repositoryItemCheckedComboBoxEditSates.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEditSates.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEditSates.Name = "repositoryItemCheckedComboBoxEditSates";
            // 
            // barEditItemSensorCheck
            // 
            this.barEditItemSensorCheck.Caption = "传感器";
            this.barEditItemSensorCheck.Edit = this.repositoryItemCheckedComboBoxEditSensors;
            this.barEditItemSensorCheck.Id = 9;
            this.barEditItemSensorCheck.Name = "barEditItemSensorCheck";
            this.barEditItemSensorCheck.Width = 60;
            // 
            // repositoryItemCheckedComboBoxEditSensors
            // 
            this.repositoryItemCheckedComboBoxEditSensors.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEditSensors.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEditSensors.Name = "repositoryItemCheckedComboBoxEditSensors";
            // 
            // barEditItemDataTypeCheck
            // 
            this.barEditItemDataTypeCheck.Caption = "切片数据类型";
            this.barEditItemDataTypeCheck.Edit = this.repositoryItemCheckedComboBoxEditDataType;
            this.barEditItemDataTypeCheck.Id = 17;
            this.barEditItemDataTypeCheck.Name = "barEditItemDataTypeCheck";
            this.barEditItemDataTypeCheck.Width = 60;
            // 
            // repositoryItemCheckedComboBoxEditDataType
            // 
            this.repositoryItemCheckedComboBoxEditDataType.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEditDataType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEditDataType.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("波段1"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("波段2"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("波段3"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("波段4"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("缩略图")});
            this.repositoryItemCheckedComboBoxEditDataType.Name = "repositoryItemCheckedComboBoxEditDataType";
            // 
            // barEditItemServiceTypeCheck
            // 
            this.barEditItemServiceTypeCheck.Caption = "服务类型";
            this.barEditItemServiceTypeCheck.Edit = this.repositoryItemCheckedComboBoxEditServiceType;
            this.barEditItemServiceTypeCheck.Id = 20;
            this.barEditItemServiceTypeCheck.Name = "barEditItemServiceTypeCheck";
            this.barEditItemServiceTypeCheck.Width = 80;
            // 
            // repositoryItemCheckedComboBoxEditServiceType
            // 
            this.repositoryItemCheckedComboBoxEditServiceType.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEditServiceType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEditServiceType.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("WMS"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("WFS"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("WCS"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("FTP")});
            this.repositoryItemCheckedComboBoxEditServiceType.Name = "repositoryItemCheckedComboBoxEditServiceType";
            // 
            // barEditItemArea
            // 
            this.barEditItemArea.Caption = "地区名称";
            this.barEditItemArea.Edit = this.repositoryItemTextEditAreaName;
            this.barEditItemArea.Id = 21;
            this.barEditItemArea.Name = "barEditItemArea";
            this.barEditItemArea.Width = 80;
            // 
            // repositoryItemTextEditAreaName
            // 
            this.repositoryItemTextEditAreaName.AutoHeight = false;
            this.repositoryItemTextEditAreaName.Name = "repositoryItemTextEditAreaName";
            // 
            // barEditItemKeyWord
            // 
            this.barEditItemKeyWord.Caption = "关键字";
            this.barEditItemKeyWord.Edit = this.repositoryItemTextEditKeyWords;
            this.barEditItemKeyWord.Id = 22;
            this.barEditItemKeyWord.Name = "barEditItemKeyWord";
            this.barEditItemKeyWord.Width = 80;
            // 
            // repositoryItemTextEditKeyWords
            // 
            this.repositoryItemTextEditKeyWords.AutoHeight = false;
            this.repositoryItemTextEditKeyWords.Name = "repositoryItemTextEditKeyWords";
            // 
            // barEditItemDocFileType
            // 
            this.barEditItemDocFileType.Caption = "文件类型";
            this.barEditItemDocFileType.Edit = this.repositoryItemCheckedComboBoxEditDocFileType;
            this.barEditItemDocFileType.Id = 40;
            this.barEditItemDocFileType.Name = "barEditItemDocFileType";
            this.barEditItemDocFileType.Width = 80;
            // 
            // repositoryItemCheckedComboBoxEditDocFileType
            // 
            this.repositoryItemCheckedComboBoxEditDocFileType.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEditDocFileType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEditDocFileType.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("txt"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("doc"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("docx"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("xls"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("ppt"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("pptx"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("xls"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("xlsx"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("pdf")});
            this.repositoryItemCheckedComboBoxEditDocFileType.Name = "repositoryItemCheckedComboBoxEditDocFileType";
            // 
            // barEditItem9
            // 
            this.barEditItem9.Edit = null;
            this.barEditItem9.Id = -1;
            this.barEditItem9.Name = "barEditItem9";
            // 
            // repositoryItemCheckedComboBoxEdit2
            // 
            this.repositoryItemCheckedComboBoxEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEdit2.Name = "repositoryItemCheckedComboBoxEdit2";
            // 
            // ribbonPageGroupOperation
            // 
            this.ribbonPageGroupOperation.ItemLinks.Add(this.barButtonItemQuery);
            this.ribbonPageGroupOperation.ItemLinks.Add(this.barButtonItemShowCustom, true);
            this.ribbonPageGroupOperation.ItemLinks.Add(this.barStaticItem4, true);
            this.ribbonPageGroupOperation.ItemLinks.Add(this.barEditItemNumPerPage);
            this.ribbonPageGroupOperation.Name = "ribbonPageGroupOperation";
            this.ribbonPageGroupOperation.Text = "应用";
            // 
            // barButtonItemQuery
            // 
            this.barButtonItemQuery.Caption = "检索";
            this.barButtonItemQuery.Id = 12;
            this.barButtonItemQuery.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.三维可视化检索;
            this.barButtonItemQuery.Name = "barButtonItemQuery";
            this.barButtonItemQuery.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemQuery_ItemClick);
            // 
            // barButtonItemShowCustom
            // 
            this.barButtonItemShowCustom.Caption = "显示高级检索";
            this.barButtonItemShowCustom.Id = 66;
            this.barButtonItemShowCustom.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources._33;
            this.barButtonItemShowCustom.Name = "barButtonItemShowCustom";
            this.barButtonItemShowCustom.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemShowCustom.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemShowCustom_ItemClick);
            // 
            // barEditItemNumPerPage
            // 
            this.barEditItemNumPerPage.Edit = this.repositoryItemComboBox1;
            this.barEditItemNumPerPage.EditValue = 5000;
            this.barEditItemNumPerPage.Id = 99;
            this.barEditItemNumPerPage.Name = "barEditItemNumPerPage";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "50",
            "100",
            "1000",
            "5000",
            "10000",
            "30000",
            "50000",
            "100000"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox6
            // 
            this.repositoryItemComboBox6.AutoHeight = false;
            this.repositoryItemComboBox6.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox6.Name = "repositoryItemComboBox6";
            // 
            // barButtonEarthViewInitial
            // 
            this.barButtonEarthViewInitial.Caption = "视角归位";
            this.barButtonEarthViewInitial.Id = 75;
            this.barButtonEarthViewInitial.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources._32;
            this.barButtonEarthViewInitial.Name = "barButtonEarthViewInitial";
            this.barButtonEarthViewInitial.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonEarthViewInitial.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonEarthViewInitial_ItemClick);
            // 
            // repositoryItemPopupContainerEditDatatype
            // 
            this.repositoryItemPopupContainerEditDatatype.AutoHeight = false;
            this.repositoryItemPopupContainerEditDatatype.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemPopupContainerEditDatatype.Name = "repositoryItemPopupContainerEditDatatype";
            //this.repositoryItemPopupContainerEditDatatype.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.repositoryItemPopupContainerEdit2_QueryResultValue);
            // 
            // ribbonPageGroupCustomQuery
            // 
            this.ribbonPageGroupCustomQuery.ItemLinks.Add(this.barEditItemLogicalOperator);
            this.ribbonPageGroupCustomQuery.ItemLinks.Add(this.barEditItemFieldOperator);
            this.ribbonPageGroupCustomQuery.ItemLinks.Add(this.barEditItemFieldList, false, "", "", true);
            this.ribbonPageGroupCustomQuery.ItemLinks.Add(this.barEditItemFieldValue, false, "", "", true);
            this.ribbonPageGroupCustomQuery.ItemLinks.Add(this.barButtonItemAddNewQuery, true);
            this.ribbonPageGroupCustomQuery.ItemLinks.Add(this.barEditItemShowQuery, true);
            this.ribbonPageGroupCustomQuery.ItemLinks.Add(this.barButtonItemClearCustom, true);
            this.ribbonPageGroupCustomQuery.Name = "ribbonPageGroupCustomQuery";
            this.ribbonPageGroupCustomQuery.Text = "高级检索";
            this.ribbonPageGroupCustomQuery.Visible = false;
            // 
            // barEditItemLogicalOperator
            // 
            this.barEditItemLogicalOperator.Caption = "  逻辑操作符";
            this.barEditItemLogicalOperator.Edit = this.repositoryItemComboBoxLogicalOperator;
            this.barEditItemLogicalOperator.Id = 44;
            this.barEditItemLogicalOperator.Name = "barEditItemLogicalOperator";
            this.barEditItemLogicalOperator.Width = 60;
            // 
            // repositoryItemComboBoxLogicalOperator
            // 
            this.repositoryItemComboBoxLogicalOperator.AutoHeight = false;
            this.repositoryItemComboBoxLogicalOperator.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxLogicalOperator.Items.AddRange(new object[] {
            "and",
            "or"});
            this.repositoryItemComboBoxLogicalOperator.Name = "repositoryItemComboBoxLogicalOperator";
            // 
            // barEditItemFieldOperator
            // 
            this.barEditItemFieldOperator.Caption = "  字段操作符";
            this.barEditItemFieldOperator.Edit = this.repositoryItemComboBoxOperators;
            this.barEditItemFieldOperator.Id = 42;
            this.barEditItemFieldOperator.Name = "barEditItemFieldOperator";
            this.barEditItemFieldOperator.Width = 60;
            // 
            // repositoryItemComboBoxOperators
            // 
            this.repositoryItemComboBoxOperators.AutoHeight = false;
            this.repositoryItemComboBoxOperators.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxOperators.Items.AddRange(new object[] {
            ">",
            "<",
            ">=",
            "<=",
            "=",
            "<>",
            "like","in"});
            this.repositoryItemComboBoxOperators.Name = "repositoryItemComboBoxOperators";
            // 
            // barEditItemFieldList
            // 
            this.barEditItemFieldList.Caption = "  可检索字段";
            this.barEditItemFieldList.Edit = this.repositoryItemComboBoxFields;
            this.barEditItemFieldList.Id = 41;
            this.barEditItemFieldList.Name = "barEditItemFieldList";
            this.barEditItemFieldList.Width = 80;
            // 
            // repositoryItemComboBoxFields
            // 
            this.repositoryItemComboBoxFields.AutoHeight = false;
            this.repositoryItemComboBoxFields.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxFields.Name = "repositoryItemComboBoxFields";
            // 
            // barEditItemFieldValue
            // 
            this.barEditItemFieldValue.Caption = "     字段取值";
            this.barEditItemFieldValue.Edit = this.repositoryItemTextEditFieldsValue;
            this.barEditItemFieldValue.Id = 43;
            this.barEditItemFieldValue.Name = "barEditItemFieldValue";
            this.barEditItemFieldValue.Width = 80;
            // 
            // repositoryItemTextEditFieldsValue
            // 
            this.repositoryItemTextEditFieldsValue.AutoHeight = false;
            this.repositoryItemTextEditFieldsValue.Name = "repositoryItemTextEditFieldsValue";
            // 
            // barButtonItemAddNewQuery
            // 
            this.barButtonItemAddNewQuery.Caption = "添加";
            this.barButtonItemAddNewQuery.Id = 48;
            this.barButtonItemAddNewQuery.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.添加;
            this.barButtonItemAddNewQuery.Name = "barButtonItemAddNewQuery";
            this.barButtonItemAddNewQuery.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAddNewQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAddNewQuery_ItemClick);
            // 
            // barButtonItemClearCustom
            // 
            this.barButtonItemClearCustom.Caption = "清空";
            this.barButtonItemClearCustom.Id = 64;
            this.barButtonItemClearCustom.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除记录;
            this.barButtonItemClearCustom.Name = "barButtonItemClearCustom";
            this.barButtonItemClearCustom.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemClearCustom.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemClearCustom_ItemClick);
            // 
            // imageListRucSearch
            // 
            this.imageListRucSearch.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListRucSearch.ImageStream")));
            this.imageListRucSearch.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListRucSearch.Images.SetKeyName(0, "查询浏览子系统.png");
            this.imageListRucSearch.Images.SetKeyName(1, "三维可视化检索.png");
            this.imageListRucSearch.Images.SetKeyName(2, "手动选取.png");
            this.imageListRucSearch.Images.SetKeyName(3, "应用.png");
            this.imageListRucSearch.Images.SetKeyName(4, "查看详细结果面板.png");
            this.imageListRucSearch.Images.SetKeyName(5, "下载此数据.png");
            this.imageListRucSearch.Images.SetKeyName(6, "显示图像信息.png");
            this.imageListRucSearch.Images.SetKeyName(7, "删除任务.ico");
            this.imageListRucSearch.Images.SetKeyName(8, "创建数据类型.png");
            // 
            // barEditName
            // 
            this.barEditName.Caption = "文件名";
            this.barEditName.Edit = this.repositoryItemTextEdit1;
            this.barEditName.Id = 77;
            this.barEditName.Name = "barEditName";
            this.barEditName.Width = 80;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barEditKeyWords
            // 
            this.barEditKeyWords.Caption = "关键字";
            this.barEditKeyWords.Edit = this.repositoryItemTextEdit2;
            this.barEditKeyWords.Id = 78;
            this.barEditKeyWords.Name = "barEditKeyWords";
            this.barEditKeyWords.Width = 80;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // barEditItemSoilName
            // 
            this.barEditItemSoilName.Caption = "土壤名称";
            this.barEditItemSoilName.Edit = this.repositoryItemTextEdit3;
            this.barEditItemSoilName.Id = 79;
            this.barEditItemSoilName.Name = "barEditItemSoilName";
            this.barEditItemSoilName.Width = 80;
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // barEditItemSoliSubType
            // 
            this.barEditItemSoliSubType.Caption = "土壤子类";
            this.barEditItemSoliSubType.Edit = this.repositoryItemComboBoxSoilSubType;
            this.barEditItemSoliSubType.Id = 80;
            this.barEditItemSoliSubType.Name = "barEditItemSoliSubType";
            this.barEditItemSoliSubType.Width = 80;
            // 
            // repositoryItemComboBoxSoilSubType
            // 
            this.repositoryItemComboBoxSoilSubType.AutoHeight = false;
            this.repositoryItemComboBoxSoilSubType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxSoilSubType.Name = "repositoryItemComboBoxSoilSubType";
            // 
            // barEditItemRockName
            // 
            this.barEditItemRockName.Caption = "岩矿名称";
            this.barEditItemRockName.Edit = this.repositoryItemTextEdit4;
            this.barEditItemRockName.Id = 81;
            this.barEditItemRockName.Name = "barEditItemRockName";
            this.barEditItemRockName.Width = 80;
            // 
            // repositoryItemTextEdit4
            // 
            this.repositoryItemTextEdit4.AutoHeight = false;
            this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
            // 
            // barEditItemRockType
            // 
            this.barEditItemRockType.Caption = "岩矿类别";
            this.barEditItemRockType.Edit = this.repositoryItemComboBoxRockType;
            this.barEditItemRockType.Id = 82;
            this.barEditItemRockType.Name = "barEditItemRockType";
            this.barEditItemRockType.Width = 80;
            // 
            // repositoryItemComboBoxRockType
            // 
            this.repositoryItemComboBoxRockType.AutoHeight = false;
            this.repositoryItemComboBoxRockType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxRockType.Name = "repositoryItemComboBoxRockType";
            // 
            // barEditItemRockSubType
            // 
            this.barEditItemRockSubType.Caption = "岩矿子类";
            this.barEditItemRockSubType.Edit = this.repositoryItemComboBoxRockSubType;
            this.barEditItemRockSubType.Id = 83;
            this.barEditItemRockSubType.Name = "barEditItemRockSubType";
            this.barEditItemRockSubType.Width = 80;
            // 
            // repositoryItemComboBoxRockSubType
            // 
            this.repositoryItemComboBoxRockSubType.AutoHeight = false;
            this.repositoryItemComboBoxRockSubType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxRockSubType.Name = "repositoryItemComboBoxRockSubType";
            // 
            // barEditItemRockAttribute
            // 
            this.barEditItemRockAttribute.Caption = "所属类别";
            this.barEditItemRockAttribute.Edit = this.repositoryItemComboBoxRockAttribute;
            this.barEditItemRockAttribute.Id = 84;
            this.barEditItemRockAttribute.Name = "barEditItemRockAttribute";
            this.barEditItemRockAttribute.Width = 80;
            // 
            // repositoryItemComboBoxRockAttribute
            // 
            this.repositoryItemComboBoxRockAttribute.AutoHeight = false;
            this.repositoryItemComboBoxRockAttribute.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxRockAttribute.Name = "repositoryItemComboBoxRockAttribute";
            // 
            // barEditItemPlantType
            // 
            this.barEditItemPlantType.Caption = "植被类别";
            this.barEditItemPlantType.Edit = this.repositoryItemComboBoxPlantType;
            this.barEditItemPlantType.Id = 85;
            this.barEditItemPlantType.Name = "barEditItemPlantType";
            this.barEditItemPlantType.Width = 80;
            // 
            // repositoryItemComboBoxPlantType
            // 
            this.repositoryItemComboBoxPlantType.AutoHeight = false;
            this.repositoryItemComboBoxPlantType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxPlantType.Name = "repositoryItemComboBoxPlantType";
            // 
            // barEditItemPlantName
            // 
            this.barEditItemPlantName.Caption = "植被名称";
            this.barEditItemPlantName.Edit = this.repositoryItemTextEdit6;
            this.barEditItemPlantName.Id = 86;
            this.barEditItemPlantName.Name = "barEditItemPlantName";
            this.barEditItemPlantName.Width = 80;
            // 
            // repositoryItemTextEdit6
            // 
            this.repositoryItemTextEdit6.AutoHeight = false;
            this.repositoryItemTextEdit6.Name = "repositoryItemTextEdit6";
            // 
            // barEditItemPlantPosition
            // 
            this.barEditItemPlantPosition.Caption = "测量部位";
            this.barEditItemPlantPosition.Edit = this.repositoryItemComboBoxPlantPosition;
            this.barEditItemPlantPosition.Id = 87;
            this.barEditItemPlantPosition.Name = "barEditItemPlantPosition";
            this.barEditItemPlantPosition.Width = 80;
            // 
            // repositoryItemComboBoxPlantPosition
            // 
            this.repositoryItemComboBoxPlantPosition.AutoHeight = false;
            this.repositoryItemComboBoxPlantPosition.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxPlantPosition.Name = "repositoryItemComboBoxPlantPosition";
            // 
            // barEditItemPlantTime
            // 
            this.barEditItemPlantTime.Caption = "物候期";
            this.barEditItemPlantTime.Edit = this.repositoryItemComboBoxPlantTime;
            this.barEditItemPlantTime.Id = 88;
            this.barEditItemPlantTime.Name = "barEditItemPlantTime";
            this.barEditItemPlantTime.Width = 80;
            // 
            // repositoryItemComboBoxPlantTime
            // 
            this.repositoryItemComboBoxPlantTime.AutoHeight = false;
            this.repositoryItemComboBoxPlantTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxPlantTime.Name = "repositoryItemComboBoxPlantTime";
            // 
            // barEditItemCityName
            // 
            this.barEditItemCityName.Caption = "目标名称";
            this.barEditItemCityName.Edit = this.repositoryItemComboBoxCityName;
            this.barEditItemCityName.Id = 89;
            this.barEditItemCityName.Name = "barEditItemCityName";
            this.barEditItemCityName.Width = 80;
            // 
            // repositoryItemComboBoxCityName
            // 
            this.repositoryItemComboBoxCityName.AutoHeight = false;
            this.repositoryItemComboBoxCityName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxCityName.Name = "repositoryItemComboBoxCityName";
            // 
            // barEditItemCityType
            // 
            this.barEditItemCityType.Caption = "目标类别";
            this.barEditItemCityType.Edit = this.repositoryItemComboBoxCityType;
            this.barEditItemCityType.Id = 90;
            this.barEditItemCityType.Name = "barEditItemCityType";
            this.barEditItemCityType.Width = 80;
            // 
            // repositoryItemComboBoxCityType
            // 
            this.repositoryItemComboBoxCityType.AutoHeight = false;
            this.repositoryItemComboBoxCityType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxCityType.Name = "repositoryItemComboBoxCityType";
            // 
            // barEditItemAtmosphereName
            // 
            this.barEditItemAtmosphereName.Caption = "观测站名称";
            this.barEditItemAtmosphereName.Edit = this.repositoryItemComboBoxAtmosphereName;
            this.barEditItemAtmosphereName.Id = 91;
            this.barEditItemAtmosphereName.Name = "barEditItemAtmosphereName";
            this.barEditItemAtmosphereName.Width = 80;
            // 
            // repositoryItemComboBoxAtmosphereName
            // 
            this.repositoryItemComboBoxAtmosphereName.AutoHeight = false;
            this.repositoryItemComboBoxAtmosphereName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxAtmosphereName.Name = "repositoryItemComboBoxAtmosphereName";
            // 
            // barEditItemAtmosphereCode
            // 
            this.barEditItemAtmosphereCode.Caption = "观测站编号";
            this.barEditItemAtmosphereCode.Edit = this.repositoryItemComboBoxAtmosphereCode;
            this.barEditItemAtmosphereCode.Id = 92;
            this.barEditItemAtmosphereCode.Name = "barEditItemAtmosphereCode";
            this.barEditItemAtmosphereCode.Width = 80;
            // 
            // repositoryItemComboBoxAtmosphereCode
            // 
            this.repositoryItemComboBoxAtmosphereCode.AutoHeight = false;
            this.repositoryItemComboBoxAtmosphereCode.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxAtmosphereCode.Name = "repositoryItemComboBoxAtmosphereCode";
            // 
            // barEditItemWaterName
            // 
            this.barEditItemWaterName.Caption = "水域名称";
            this.barEditItemWaterName.Edit = this.repositoryItemComboBoxWaterName;
            this.barEditItemWaterName.Id = 93;
            this.barEditItemWaterName.Name = "barEditItemWaterName";
            this.barEditItemWaterName.Width = 80;
            // 
            // repositoryItemComboBoxWaterName
            // 
            this.repositoryItemComboBoxWaterName.AutoHeight = false;
            this.repositoryItemComboBoxWaterName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxWaterName.Name = "repositoryItemComboBoxWaterName";
            // 
            // barEditItemWaterType
            // 
            this.barEditItemWaterType.Caption = "所属类别";
            this.barEditItemWaterType.Edit = this.repositoryItemComboBoxWaterType;
            this.barEditItemWaterType.Id = 94;
            this.barEditItemWaterType.Name = "barEditItemWaterType";
            this.barEditItemWaterType.Width = 80;
            // 
            // repositoryItemComboBoxWaterType
            // 
            this.repositoryItemComboBoxWaterType.AutoHeight = false;
            this.repositoryItemComboBoxWaterType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxWaterType.Name = "repositoryItemComboBoxWaterType";
            // 
            // barEditItemWaterFactor
            // 
            this.barEditItemWaterFactor.Caption = "地理特征";
            this.barEditItemWaterFactor.Edit = this.repositoryItemTextEdit8;
            this.barEditItemWaterFactor.Id = 95;
            this.barEditItemWaterFactor.Name = "barEditItemWaterFactor";
            this.barEditItemWaterFactor.Width = 80;
            // 
            // repositoryItemTextEdit8
            // 
            this.repositoryItemTextEdit8.AutoHeight = false;
            this.repositoryItemTextEdit8.Name = "repositoryItemTextEdit8";
            // 
            // barEditItemWaterYiQI
            // 
            this.barEditItemWaterYiQI.Caption = "光谱仪器";
            this.barEditItemWaterYiQI.Edit = this.repositoryItemComboBoxWaterYiQI;
            this.barEditItemWaterYiQI.Id = 96;
            this.barEditItemWaterYiQI.Name = "barEditItemWaterYiQI";
            this.barEditItemWaterYiQI.Width = 80;
            // 
            // repositoryItemComboBoxWaterYiQI
            // 
            this.repositoryItemComboBoxWaterYiQI.AutoHeight = false;
            this.repositoryItemComboBoxWaterYiQI.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxWaterYiQI.Name = "repositoryItemComboBoxWaterYiQI";
            // 
            // barEditItemTileLevel
            // 
            this.barEditItemTileLevel.Caption = "     切片等级";
            this.barEditItemTileLevel.Edit = this.repositoryItemCheckedComboBoxEditTileLevel;
            this.barEditItemTileLevel.Id = 97;
            this.barEditItemTileLevel.Name = "barEditItemTileLevel";
            this.barEditItemTileLevel.Width = 60;
            // 
            // repositoryItemCheckedComboBoxEditTileLevel
            // 
            this.repositoryItemCheckedComboBoxEditTileLevel.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEditTileLevel.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEditTileLevel.Name = "repositoryItemCheckedComboBoxEditTileLevel";
            // 
            // barEditItemProTypeCombo
            // 
            this.barEditItemProTypeCombo.Caption = "产品类型";
            this.barEditItemProTypeCombo.Edit = this.repositoryItemCheckedComboBoxEditProType;
            this.barEditItemProTypeCombo.Id = 98;
            this.barEditItemProTypeCombo.Name = "barEditItemProTypeCombo";
            this.barEditItemProTypeCombo.Width = 80;
            // 
            // repositoryItemCheckedComboBoxEditProType
            // 
            this.repositoryItemCheckedComboBoxEditProType.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEditProType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEditProType.Name = "repositoryItemCheckedComboBoxEditProType";
            // 
            // barEditItemTimerTrackBar
            // 
            //this.barEditItemTimerTrackBar.Edit = this.repositoryItemRangeTrackBar1;
            //this.barEditItemTimerTrackBar.Id = 100;
            //this.barEditItemTimerTrackBar.Name = "barEditItemTimerTrackBar";
            //this.barEditItemTimerTrackBar.Width = 200;
            //// 
            //// repositoryItemRangeTrackBar1
            //// 
            //this.repositoryItemRangeTrackBar1.Name = "repositoryItemRangeTrackBar1";
            //this.repositoryItemRangeTrackBar1.ShowValueToolTip = true;
            //this.repositoryItemRangeTrackBar1.BeforeShowValueToolTip += new DevExpress.XtraEditors.TrackBarValueToolTipEventHandler(this.repositoryItemRangeTrackBar1_BeforeShowValueToolTip);
            //this.repositoryItemRangeTrackBar1.EditValueChanged += new System.EventHandler(this.repositoryItemRangeTrackBar1_EditValueChanged);
            //this.repositoryItemRangeTrackBar1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemRangeTrackBar1_KeyDown);
            //this.repositoryItemRangeTrackBar1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.repositoryItemRangeTrackBar1_KeyPress);
            //this.repositoryItemRangeTrackBar1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.repositoryItemRangeTrackBar1_MouseWheel);
            // 
            // barEditItemCloudPercent
            // 
            this.barEditItemCloudPercent.Caption = "云量(%)";
            this.barEditItemCloudPercent.Edit = this.repositoryItemRangeTrackBar2;
            this.barEditItemCloudPercent.Id = 101;
            this.barEditItemCloudPercent.Name = "barEditItemCloudPercent";
            this.barEditItemCloudPercent.Width = 100;
            // 
            // repositoryItemRangeTrackBar2
            // 
            this.repositoryItemRangeTrackBar2.Name = "repositoryItemRangeTrackBar2";
            // 
            // barEditItemManfuPercent
            // 
            this.barEditItemManfuPercent.Caption = "  满幅率";
            this.barEditItemManfuPercent.Edit = this.repositoryItemRangeTrackBar3;
            this.barEditItemManfuPercent.Id = 102;
            this.barEditItemManfuPercent.Name = "barEditItemManfuPercent";
            this.barEditItemManfuPercent.Width = 100;
            // 
            // repositoryItemRangeTrackBar3
            // 
            this.repositoryItemRangeTrackBar3.Name = "repositoryItemRangeTrackBar3";
            // 
            // barEditItemSpacialInfo
            // 
            this.barEditItemSpacialInfo.Caption = "空间信息";
            this.barEditItemSpacialInfo.Edit = this.repositoryItemComboBoxSpatialChoose;
            this.barEditItemSpacialInfo.EditValue = "经纬坐标";
            this.barEditItemSpacialInfo.Id = 108;
            this.barEditItemSpacialInfo.Name = "barEditItemSpacialInfo";
            this.barEditItemSpacialInfo.Width = 70;
            this.barEditItemSpacialInfo.EditValueChanged += new System.EventHandler(this.barEditItemSpacialInfo_EditValueChanged);
            // 
            // repositoryItemComboBoxSpatialChoose
            // 
            this.repositoryItemComboBoxSpatialChoose.AutoHeight = false;
            this.repositoryItemComboBoxSpatialChoose.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxSpatialChoose.Items.AddRange(new object[] {
            "经纬坐标",
            "行政区域",
            "坐标文件",
			"行列号范围"});
            this.repositoryItemComboBoxSpatialChoose.Name = "repositoryItemComboBoxSpatialChoose";
            // 
            // barEditItemMaxCol
            // 
            this.barEditItemMaxCol.Caption = "最大列号";
            this.barEditItemMaxCol.Edit = this.repositoryItemTextEditMaxCol;
            this.barEditItemMaxCol.Id = 365;
            this.barEditItemMaxCol.Name = "barEditItemMaxCol";
            this.barEditItemMaxCol.Width = 80;
            this.barEditItemMaxCol.EditValue = "";
            // 
            // repositoryItemTextEditMaxLon
            // 
            this.repositoryItemTextEditMaxCol.AutoHeight = false;
            this.repositoryItemTextEditMaxCol.Name = "repositoryItemTextEditMaxCol";
            this.barEditItemMinCol.Caption = "最小列号";
            this.barEditItemMinCol.Edit = this.repositoryItemTextEditMinCol;
            this.barEditItemMinCol.Id = 366;
            this.barEditItemMinCol.Name = "barEditItemMinCol";
            this.barEditItemMinCol.Width = 80;
            this.barEditItemMinCol.EditValue = "";
            // 
            // repositoryItemTextEditMinLon
            // 
            this.repositoryItemTextEditMinCol.AutoHeight = false;
            this.repositoryItemTextEditMinCol.Name = "repositoryItemTextEditMinCol";
            // 
            // barEditItemMaxLat
            // 
            this.barEditItemMaxRow.Caption = "最大行号";
            this.barEditItemMaxRow.Edit = this.repositoryItemTextEditMaxRow;
            this.barEditItemMaxRow.Id = 368;
            this.barEditItemMaxRow.Name = "barEditItemMaxRow";
            this.barEditItemMaxRow.Width = 80;
            this.barEditItemMaxRow.EditValue = "";
            // 
            // repositoryItemTextEditMaxLat
            // 
            this.repositoryItemTextEditMaxRow.AutoHeight = false;
            this.repositoryItemTextEditMaxRow.Name = "repositoryItemTextEditMaxRow";
            // 
            // barEditItemMinLat
            // 
            this.barEditItemMinRow.Caption = "最小行号";
            this.barEditItemMinRow.Edit = this.repositoryItemTextEditMinRow;
            this.barEditItemMinRow.Id = 7;
            this.barEditItemMinRow.Name = "barEditItemMinLat";
            this.barEditItemMinRow.Width = 80;
            this.barEditItemMinRow.EditValue = "";
            this.repositoryItemTextEditMinRow.AutoHeight = false;
            this.repositoryItemTextEditMinRow.Name = "repositoryItemTextEditMinRow";
            // 
            // barStaticItem1
            // 

            this.barStaticItem1.Id = 109;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItemProvince
            // 
            this.barEditItemProvince.Caption = "省(直辖市)";
            this.barEditItemProvince.Edit = this.repositoryItemComboBoxProvince;
            this.barEditItemProvince.Id = 110;
            this.barEditItemProvince.Name = "barEditItemProvince";
            this.barEditItemProvince.Width = 60;
            this.barEditItemProvince.EditValueChanged += new System.EventHandler(this.barEditItemProvince_EditValueChanged);
            // 
            // repositoryItemComboBoxProvince
            // 
            this.repositoryItemComboBoxProvince.AutoHeight = false;
            this.repositoryItemComboBoxProvince.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxProvince.Name = "repositoryItemComboBoxProvince";
            // 
            // barEditItemUrban
            // 
            this.barEditItemUrban.Caption = "  市";
            this.barEditItemUrban.Edit = this.repositoryItemComboBoxCity;
            this.barEditItemUrban.Id = 111;
            this.barEditItemUrban.Name = "barEditItemUrban";
            this.barEditItemUrban.Width = 60;
            this.barEditItemUrban.EditValueChanged += new System.EventHandler(this.barEditItemUrban_EditValueChanged);
            this.barEditItemUrban.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barEditItemUrban_ItemClick);
            // 
            // repositoryItemComboBoxCity
            // 
            this.repositoryItemComboBoxCity.AutoHeight = false;
            this.repositoryItemComboBoxCity.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxCity.Name = "repositoryItemComboBoxCity";
            // 
            // barEditItemCounty
            // 
            this.barEditItemCounty.Caption = "         区县";
            this.barEditItemCounty.Edit = this.repositoryItemComboBoxCounty;
            this.barEditItemCounty.Id = 112;
            this.barEditItemCounty.Name = "barEditItemCounty";
            this.barEditItemCounty.Width = 60;
            this.barEditItemCounty.EditValueChanged += new System.EventHandler(this.barEditItemCounty_EditValueChanged);
            // 
            // repositoryItemComboBoxCounty
            // 
            this.repositoryItemComboBoxCounty.AutoHeight = false;
            this.repositoryItemComboBoxCounty.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxCounty.Name = "repositoryItemComboBoxCounty";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Id = 113;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItemLoadPositionFile
            // 
            this.barButtonItemLoadPositionFile.Caption = "加载坐标文件...";
            this.barButtonItemLoadPositionFile.Id = 114;
            this.barButtonItemLoadPositionFile.Name = "barButtonItemLoadPositionFile";
            this.barButtonItemLoadPositionFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemLoadPositionFile_ItemClick);
            // 
            // barButtonItem2D
            // 
            this.barButtonItem2D.Caption = "二维地图";
            this.barButtonItem2D.Id = 118;
            this.barButtonItem2D.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.创建新订单;
            this.barButtonItem2D.Name = "barButtonItem2D";
            this.barButtonItem2D.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem2D.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2D_ItemClick);
            // 
            // barButtonItem3D
            // 
            this.barButtonItem3D.Caption = "三维影像图";
            this.barButtonItem3D.Enabled = false;
            this.barButtonItem3D.Id = 119;
            this.barButtonItem3D.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.创建新订单;
            this.barButtonItem3D.Name = "barButtonItem3D";
            this.barButtonItem3D.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem3D.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3D_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem3D);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemRaster);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem2D);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonEarthViewInitial, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "视图";
            // 
            // barButtonItemRaster
            // 
            this.barButtonItemRaster.Caption = "二维影像图";
            this.barButtonItemRaster.Id = 120;
            this.barButtonItemRaster.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.创建新订单;
            this.barButtonItemRaster.Name = "barButtonItemRaster";
            this.barButtonItemRaster.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemRaster.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemRaster_ItemClick);
            // 
            // barButtonItemVector
            // 
            this.barButtonItemVector.Caption = "三维地图";
            this.barButtonItemVector.Enabled = false;
            this.barButtonItemVector.Id = 121;
            this.barButtonItemVector.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.创建新订单;
            this.barButtonItemVector.Name = "barButtonItemVector";
            this.barButtonItemVector.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Caption = "每页显示";
            this.barStaticItem4.Id = 122;
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // ruc3DSearcher
            // 
            this.Name = "ruc3DSearcher";
            this.Size = new System.Drawing.Size(1400, 150);
            this.Load += new System.EventHandler(this.ruc3DSearcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxLon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinLon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEditShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditBeginTime.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditBeginTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditEndTime.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditEndTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditSates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditSensors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditServiceType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditAreaName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditKeyWords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditDocFileType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEditDatatype)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxLogicalOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxOperators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditFieldsValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSoilSubType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxRockType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxRockSubType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxRockAttribute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPlantType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPlantPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxPlantTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCityName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCityType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxAtmosphereName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxAtmosphereCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxWaterName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxWaterType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxWaterYiQI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditTileLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditProType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSpatialChoose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxProvince)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCounty)).EndInit();
            this.ResumeLayout(false);

        }

        void rioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioItem1.Value == null)
                this.radioItem1.Value = "相交";
            int index = ((RadioGroup)sender).SelectedIndex;
            switch (index)
            {
                case 0:
                    rule = QRST_DI_DS_MetadataQuery.Rule.Intersect;

                    break;
                case 1:
                    rule = QRST_DI_DS_MetadataQuery.Rule.Contain;
                    break;
                default:
                    rule = QRST_DI_DS_MetadataQuery.Rule.Intersect;
                    this.radioItem1.Value = "相交";
                    break;

            }
            //rioGroup.EditValue
            //((DevExpress.XtraEditors.Controls.RadioGroupItem)rioGroup.Items[((RadioGroup)sender).SelectedIndex]).Value = false;
        }
        //#endregion

        ///// <summary>
        ///// popup控件双击关闭自身,获取要查询的数据选择类型
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void treeViewDataType_DoubleClick(object sender, EventArgs e)
        //{
        //    selectedQueryObj = (metadatacatalognode_Mdl)this.treeViewDataType.SelectedNode.Tag;
        //    ClosePopup();
        //}

        ///// <summary>
        ///// popup控件关闭时获取值
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void repositoryItemPopupContainerEdit2_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e)
        //{
        //    e.Value = this.treeViewDataType.SelectedNode.Text;
        //}

        /// <summary>
        ///根据不同的数据类型，展示不同的查询界面  zxw
        ///若为数据集，则展示无法查询，需要选择有dataCode的数据类型，
        ///然后根据不同数据类型准备不同的查询界面环境，目前支持的查询有：system_raster，system_vector，system_table，system_document，system_tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItemSelDataType_EditValueChanged()
        {
            // .Cursor = Cursors.WaitCursor;
            this.Cursor = Cursors.WaitCursor;
            selectedQueryObj = muc3DSearcher._metadatacatalognode_Mdl;
            if (selectedQueryObj == null)
            {
                return;
            }

            if (!selectedQueryObj.IS_DATASET)
            {
                try
                {
                    //界面准备工作
                    this.ribbonPageGroup1.ItemLinks.Clear();
                    this.ribbonPageGroup1.Visible = true;
                    this.barEditItemKeyWord.EditValue = string.Empty;
                    ShowDetailQuery(false);
                    //重构查询方案参数
                    string[] elementSet = new string[] { "*" };
                    MySqlBaseUtilities sqlUtilities = TheUniversal.GetsubDbByEngName(selectedQueryObj.GROUP_CODE.Substring(0, 4)).sqlUtilities;

                    this.barEditItemShowQuery.EditValue = string.Empty;   //清空自定义条件

                    //按照数据类型准备查询环境界面
                    if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))    //准备栅格数据查询环境
                    {
                        querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);

                        DataTable dtstruct = querySchema.GetTableStruct().Tables[0];

                        LoadQueryField(dtstruct);
                        //完成栅格查询的一些基本数据加载
                        if (this.repositoryItemCheckedComboBoxEditSensors.Items.Count == 0)//加载传感器数据列表
                        {
                            DataSet dsSensor = Sensors.GetSensorDataSet(TheUniversal.EVDB.sqlUtilities, "");
                            if (dsSensor != null)
                            {
                                for (int i = 0; i < dsSensor.Tables[0].Rows.Count; i++)
                                {
                                    repositoryItemCheckedComboBoxEditSensors.Items.Add(dsSensor.Tables[0].Rows[i]["NAME"]);
                                }
                            }
                        }
                        if (this.repositoryItemCheckedComboBoxEditSates.Items.Count == 0)//加载卫星数据列表
                        {
                            DataSet dsSate = Satellite.GetSatelliteDataSet(TheUniversal.EVDB.sqlUtilities, "");
                            if (dsSate != null)
                            {
                                for (int i = 0; i < dsSate.Tables[0].Rows.Count; i++)
                                {
                                    repositoryItemCheckedComboBoxEditSates.Items.Add(dsSate.Tables[0].Rows[i]["NAME"]);
                                }
                            }
                        }
                        //显示栅格数据查询界面
                        setSatalliteAndSensor();
                        setBaseSpaceControl();
                        setBaseDateControl();
                        //   setKeyWordControl();

                        setTrackBar();
                        //setExtentTool();
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))  //准备矢量数据查询环境
                    {
                        querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);
                        LoadQueryField(querySchema.GetTableStruct().Tables[0]);
                        setBaseDateControl();
                        setBaseSpaceControl();
                        setKeyWordControl();
                        //setExtentTool();
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_table")) //准备表格数据查询环境
                    {
                        //表格数据情况特殊，因为波普特征数据库调用外部WebService服务进行查询，为统一集中到查询模块，需要另划界面
                        if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")          //波普特征数据库，特殊情况，若归为统一，则可去掉
                        {
                            SetCustomQueryDisable();
                            ShowRcdbQuery(selectedQueryObj.NAME);
                            return;
                        }
                        else
                        {
                            querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);
                            LoadQueryField(querySchema.GetTableStruct().Tables[0]);
                            setTableQueryControl();
                        }
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_document")) //准备文档数据查询环境
                    {
                        querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);
                        LoadQueryField(querySchema.GetTableStruct().Tables[0]);
                        setBaseDateControl();
                        setDocQueryControl();
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")) //准备切片数据查询环境
                    {
                        if (sqliteclient == null)
                        {
                            sqliteclient = new localhostSqlite.Service();
                        }
                        if (selectedQueryObj.NAME.Equals("规格化影像数据") || selectedQueryObj.NAME.Equals("规格化影像控制数据"))
                        {

                            //加载卫星传感器
                            string[] satelliteList = sqliteclient.SearTileSatellites();
                            Array.Sort(satelliteList);
                            this.repositoryItemCheckedComboBoxEditSates.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditSates.Items.AddRange(satelliteList);
                            string[] sensorList = sqliteclient.SearTileSensors();
                            Array.Sort(sensorList);
                            this.repositoryItemCheckedComboBoxEditSensors.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditSensors.Items.AddRange(sensorList);
                            setSatalliteAndSensor();
                            //加载类型
                            string[] tileTypeList = sqliteclient.GetDataDistinct("select distinct type from correctedTiles");
                            this.repositoryItemCheckedComboBoxEditDataType.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditDataType.Items.AddRange(tileTypeList);
                            setTileDataTypeControl();

                            //数据切片层级初始化
                            string[] levelLst = sqliteclient.SearTileLevels();
                            Array.Sort(levelLst);
                            //string[] levelLst = new string[] { "6", "7" };
                            this.repositoryItemCheckedComboBoxEditTileLevel.Items.Clear();
                            foreach (string lv in levelLst)
                            {
                                repositoryItemCheckedComboBoxEditTileLevel.Items.Add(DirectlyAddressing.GetStrResolutionByLevelChar(lv));
                            }
                            setTileLevelControl();
                            repositoryItemComboBoxFields.Items.Clear();
                            DataSet tileData = sqliteclient.SearTileAllAttr();
                            //if (tileData == null || tileData.Tables.Count <= 0)
                            //    tileData = sqliteclient.SearTileAllAttr();
                            if (tileData != null && tileData.Tables.Count > 0)
                            {
                                repositoryItemComboBoxFields.Items.Clear();
                                for (int i = 0; i < tileData.Tables[0].Columns.Count; i++)
                                {
                                    repositoryItemComboBoxFields.Items.Add(tileData.Tables[0].Columns[i].ColumnName);
                                }

                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("加载高级检索条件失败！是否继续加载高级检索条件，这可能会耗费一段时间", "提示", MessageBoxButtons.OKCancel);
                                if (result == DialogResult.OK)
                                {
                                    tileData = sqliteclient.SearTileAllAttr();
                                    if (tileData != null && tileData.Tables.Count > 0)
                                    {
                                        repositoryItemComboBoxFields.Items.Clear();
                                        for (int i = 0; i < tileData.Tables[0].Columns.Count; i++)
                                        {
                                            repositoryItemComboBoxFields.Items.Add(tileData.Tables[0].Columns[i].ColumnName);
                                        }
                                    }
                                }
                            }
                            // LoadQueryField(tileData.Tables[0]);

                            if (selectedQueryObj.NAME.Equals("规格化影像控制数据"))
                            {
                                repositoryItemCheckedComboBoxEditSates.Items.Clear();
                                repositoryItemCheckedComboBoxEditSates.Items.Add("CP3");
                            }
                        }
                        else
                        {
                            repositoryItemCheckedComboBoxEditTileLevel.Items.Clear();
                            string[] levelLst = sqliteclient.SearProTileLevels();
                            Array.Sort(levelLst);
                            foreach (string lv in levelLst)
                            {
                                repositoryItemCheckedComboBoxEditTileLevel.Items.Add(DirectlyAddressing.GetStrResolutionByLevelChar(lv));
                            }
                            //产品切片层级初始
                            setTileLevelControl();
                            //加载产品类型
                            string[] prodTypeList = sqliteclient.GetDataDistinct("select distinct ProdType from productTiles");
                            this.repositoryItemCheckedComboBoxEditProType.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditProType.Items.AddRange(prodTypeList);
                            setProTileTypeControl();
                            repositoryItemComboBoxFields.Items.Clear();
                            DataSet tileData = sqliteclient.SearProTileAllAttr();
                            //if (tileData == null || tileData.Tables.Count <= 0)
                            //    tileData = sqliteclient.SearProTileAllAttr();
                            if (tileData != null && tileData.Tables.Count > 0)
                            {
                                repositoryItemComboBoxFields.Items.Clear();
                                for (int i = 0; i < tileData.Tables[0].Columns.Count; i++)
                                {
                                    repositoryItemComboBoxFields.Items.Add(tileData.Tables[0].Columns[i].ColumnName);
                                }

                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("加载高级检索条件失败！是否继续加载高级检索条件，这可能会耗费一段时间", "提示", MessageBoxButtons.OKCancel);
                                if (result == DialogResult.OK)
                                {
                                    tileData = sqliteclient.SearProTileAllAttr();
                                    if (tileData != null && tileData.Tables.Count > 0)
                                    {
                                        repositoryItemComboBoxFields.Items.Clear();
                                        for (int i = 0; i < tileData.Tables[0].Columns.Count; i++)
                                        {
                                            repositoryItemComboBoxFields.Items.Add(tileData.Tables[0].Columns[i].ColumnName);
                                        }
                                    }
                                }
                            }
                        }
                        setBaseSpaceControl();
                        setBaseDateControl();
                        setTrackBar();
                        //setExtentTool();
                        SetCustomQueryDisable();
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("暂不支持'{0}'数据类型查询!", selectedQueryObj.NAME));
                        return;
                    }
                    setQueryAvailable();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("查询初始化失败：" + ex.ToString());
                    setQueryDisabled();
                }
            }
            else //将查询设置为不可用
            {
                setQueryDisabled();
            }
            this.Cursor = Cursors.Default;
        }



        #region  设置查询界面相关方法
        /// <summary>
        /// 设置初始化及选择的数据类型不能进行检索时的处理。
        /// </summary>
        void setQueryDisabled()
        {
            this.ribbonPageGroup1.ItemLinks.Clear();
            this.ribbonPageGroup1.ItemLinks.Add(this.barStaticItemTishi);
            this.barStaticItemTishi.Caption = "当前选择的数据类型不能进行检索！";

            this.ribbonPageGroupCustomQuery.Visible = false;
            this.barButtonItemShowCustom.Caption = "显示高级检索";
            this.barButtonItemQuery.Enabled = false;

            this.barButtonItemShowCustom.Enabled = false;
        }
        /// <summary>
        /// 设置查询可用
        /// </summary>
        void setQueryAvailable()
        {
            this.barButtonItemQuery.Enabled = true;
            this.barButtonItemShowCustom.Enabled = true;
        }
        /// <summary>
        /// 设置时间查询控件
        /// </summary>
        void setBaseDateControl()
        {
            this.barEditItemBeginDate.EditValue = DateTime.Now.AddYears(-20).ToString();
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemBeginDate, true);
            this.barEditItemEndDate.EditValue = DateTime.Now.ToString();
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemEndDate);
            this.ribbonPageGroup1.ItemLinks.Add(this.SearchRule);
            //this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemTimerTrackBar);
        }

        /// <summary>
        /// 
        /// </summary>
        void setTrackBar()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemCloudPercent, true, "", "", true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemManfuPercent, false, "", "", true);
        }
        /// <summary>
        /// 设置空间查询控件
        /// </summary>
        void setBaseSpaceControl()
        {
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                //  this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemLatandLon, true);
                //空间信息选项卡
                //DLF130920   解决重新选择数据类型进行查询时空间信息选项卡可能不为经纬度（如用户刚选择了行政区域进行查询），但界面为经纬度范围的不合理情况
                //下面一句话 执行有异常，不知何原因
                this.repositoryItemComboBoxSpatialChoose.Items.Clear();
                this.repositoryItemComboBoxSpatialChoose.Items.AddRange(new string[] { "经纬坐标", "行政区域", "坐标文件", "行列号范围" });
                if (this.repositoryItemComboBoxSpatialChoose.Items.Count > 0)
                {
                    try
                    {
                        //this.barEditItemSpacialInfo.EditValue = null;
                        this.repositoryItemComboBoxCounty.Items.Clear();
                        this.repositoryItemComboBoxCity.Items.Clear();
                        this.repositoryItemComboBoxProvince.Items.Clear();

                        this.barEditItemUrban.EditValue = null;
                        this.barEditItemProvince.EditValue = null;

                        this.barEditItemSpacialInfo.EditValue = this.repositoryItemComboBoxSpatialChoose.Items[0];
                        SpacialInfo = this.barEditItemSpacialInfo.EditValue.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemSpacialInfo, true);

                //this.repositoryItemComboBoxSpatialChoose
                this.barEditItemMaxLon.EditValue = 180;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMaxLon);
                this.barEditItemMinLon.EditValue = -180;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMinLon);


                this.ribbonPageGroup1.ItemLinks.Add(this.barStaticItem1);
                this.barEditItemMaxLat.EditValue = 90;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMaxLat);
                this.barEditItemMinLat.EditValue = -90;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMinLat);
                // this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemFile);
                this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemHandleSel, true, "", "", true);

            }
            else
            {
                this.barEditItemMaxLon.EditValue = 180;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
                this.barEditItemMinLon.EditValue = -180;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
                this.barEditItemMaxLat.EditValue = 90;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
                this.barEditItemMinLat.EditValue = -90;
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemHandleSel, true, "", "", true);

            }
        }
        /// <summary>
        ///空间信息选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItemSpacialInfo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                /*
                 * 重复，后续代码中已执行   joki 151204
                if (this.ribbonPageGroup1.ItemLinks.Count == 16)
                {
                    this.ribbonPageGroup1.ItemLinks.RemoveAt(9);
                    this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                    this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                    this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemHandleSel);

                }
                else if (this.ribbonPageGroup1.ItemLinks.Count == 13)
                {
                    this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                }
                 */


                if (barEditItemSpacialInfo.EditValue.ToString() == "行政区域")
                {
                    AreaLocationQuery areaQuery = new AreaLocationQuery(TheUniversal.BSDB.sqlUtilities);
                    List<string> provinceLst = areaQuery.GetProvinceLst();
                    if (repositoryItemComboBoxProvince.Items.Count == 0)
                    {
                        repositoryItemComboBoxProvince.Items.AddRange(provinceLst);
                        barEditItemProvince.EditValue = provinceLst[0];
                    }

                    if (SpacialInfo == "经纬坐标" || SpacialInfo == "行列号范围")
                    {
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(9);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemHandleSel);

                    }
                    else if (SpacialInfo == "坐标文件")
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    this.ribbonPageGroup1.ItemLinks.Insert(5, this.barEditItemProvince);
                    this.ribbonPageGroup1.ItemLinks.Insert(6, this.barEditItemUrban);
                    this.ribbonPageGroup1.ItemLinks.Insert(8, this.barEditItemCounty);
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    SpacialInfo = "行政区域";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "经纬坐标")
                {
                    //if (this.barEditItemMaxLon.EditValue == null || this.barEditItemMaxLon.EditValue.ToString() == "")
                    this.barEditItemMaxLon.EditValue = 180;
                    //if (this.barEditItemMinLon.EditValue == null || this.barEditItemMinLon.EditValue.ToString() == "")
                    this.barEditItemMinLon.EditValue = -180;
                    //if (this.barEditItemMaxLat.EditValue == null || this.barEditItemMaxLat.EditValue.ToString() == "")
                    this.barEditItemMaxLat.EditValue = 90;
                    //if (this.barEditItemMinLat.EditValue == null || this.barEditItemMinLat.EditValue.ToString() == "")
                    this.barEditItemMinLat.EditValue = -90;

                    if (SpacialInfo == "行列号范围")
                    {
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(9);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);

                    }
                    else if (SpacialInfo == "坐标文件")
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(5, this.barEditItemMaxLon);
                    this.ribbonPageGroup1.ItemLinks.Insert(6, this.barEditItemMinLon);
                    this.ribbonPageGroup1.ItemLinks.Insert(8, this.barEditItemMaxLat);
                    this.ribbonPageGroup1.ItemLinks.Insert(9, this.barEditItemMinLat);
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    SpacialInfo = "经纬坐标";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "坐标文件")
                {
                    if (SpacialInfo == "经纬坐标" || SpacialInfo == "行列号范围")
                    {
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(9);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    }
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(5, this.barButtonItemLoadPositionFile);
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    SpacialInfo = "坐标文件";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "行列号范围")
                {

                    //if (this.barEditItemMaxCol.EditValue == null || this.barEditItemMaxCol.EditValue.ToString() == "")
                    this.barEditItemMaxCol.EditValue = "";
                    //if (this.barEditItemMinCol.EditValue == null || this.barEditItemMinCol.EditValue.ToString() == "")
                    this.barEditItemMinCol.EditValue = "";
                    //if (this.barEditItemMaxRow.EditValue == null || this.barEditItemMaxRow.EditValue.ToString() == "")
                    this.barEditItemMaxRow.EditValue = "";
                    //if (this.barEditItemMinRow.EditValue == null || this.barEditItemMinRow.EditValue.ToString() == "")
                    this.barEditItemMinRow.EditValue = "";
                    if (SpacialInfo == "经纬坐标")
                    {
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(9);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    }
                    else if (SpacialInfo == "坐标文件")
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(8);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(6);
                        this.ribbonPageGroup1.ItemLinks.RemoveAt(5);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(5, this.barEditItemMaxCol);
                    this.ribbonPageGroup1.ItemLinks.Insert(6, this.barEditItemMinCol);
                    this.ribbonPageGroup1.ItemLinks.Insert(8, this.barEditItemMaxRow);
                    this.ribbonPageGroup1.ItemLinks.Insert(9, this.barEditItemMinRow);
                    this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemHandleSel, true, "", "", true);
                    SpacialInfo = "行列号范围";
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 设置框选范围工具
        /// </summary>
        void setExtentTool()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemHandleSel, true, "", "", true);
        }

        /// <summary>
        /// 设置切片数据类型
        /// </summary>
        void setTileDataTypeControl()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemDataTypeCheck, true, "", "", true);
        }
        /// <summary>
        /// 设置产品类型
        /// </summary>
        void setProTileTypeControl()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemProTypeCombo, true, "", "", true);
        }
        /// <summary>
        /// 设置切片层级
        /// </summary>
        void setTileLevelControl()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemTileLevel, false, "", "", true);
            //repositoryItemComboBoxTileLevel.Items.Clear();
        }

        /// <summary>
        /// 设置关键字检索控件
        /// </summary>
        void setKeyWordControl()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemKeyWord, true, "", "", true);
        }


        /// <summary>
        /// 设置卫星传感器查询控件
        /// </summary>
        void setSatalliteAndSensor()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemSateCheck, true, "", "", true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemSensorCheck, false, "", "", true);
        }
        /// <summary>
        /// 设置一般表格查询控件
        /// </summary>
        void setTableQueryControl()
        {
            this.ribbonPageGroup1.Visible = false;
            ShowDetailQuery(true);
        }
        /// <summary>
        /// 设置文档查询控件
        /// </summary>
        void setDocQueryControl()
        {
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditName, true, "", "", true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditKeyWords, false, "", "", true);
        }
        /// <summary>
        /// 是否显示高级查询
        /// </summary>
        /// <param name="isShow"></param>
        void ShowDetailQuery(bool isShow)
        {
            if (isShow)
            {
                this.ribbonPageGroupCustomQuery.Visible = true;
                barButtonItemShowCustom.Caption = "取消高级检索";
            }
            else
            {
                this.ribbonPageGroupCustomQuery.Visible = false;
                barButtonItemShowCustom.Caption = "显示高级检索";
            }
        }

        /// <summary>
        /// 禁用高级查询
        /// </summary>
        void SetCustomQueryDisable()
        {
            this.ribbonPageGroupCustomQuery.Visible = false;
            this.barButtonItemShowCustom.Caption = "显示高级检索";
            this.barButtonItemShowCustom.Enabled = false;
            this.barButtonItemQuery.Enabled = true;
        }

        /// <summary>
        /// 设置土壤查询界面. @由Tomcat发布改成IIS发布 jianghua 2015.8.15
        /// </summary>
        void ShowSoilQuery()
        {
            try
            {
                //第一次调用时初始化
                //if (soilclient == null)
                //{
                //    this.soilclient = new SoilService.SoilServicePortTypeClient("SoilServiceHttpSoap12Endpoint");
                //}
                if (mySQLSerVice == null)
                {
                    this.mySQLSerVice = new Service();
                }
                jsonStr = mySQLSerVice.getSoilSubTypes();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxSoilSubType, strFields);
                setBaseDateControl();
                setBaseSpaceControl();
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemSoilName, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemSoliSubType, false, "", "", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("无法获取'{0}'的查询服务！", selectedQueryObj.NAME));
                setQueryDisabled();
            }
        }
        /// <summary>
        /// 岩矿查询初始化 @由查询服务由Tomcat发布改成IIS发布 jianghua 2015.8.15
        /// </summary>
        void ShowRockQuery()
        {
            try
            {
                //if (this.rockclient == null)
                //{
                //    this.rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
                //}
                if (mySQLSerVice == null)
                {
                    this.mySQLSerVice = new Service();
                }
                //获取岩石所属类别列表
                jsonStr = mySQLSerVice.getRockSSLB();
                classify = JSON.JSON.parse<GetClassify>(jsonStr); 
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxRockAttribute, strFields);
                //获取岩矿子类列表
                jsonStr = mySQLSerVice.getRockSubTypes();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxRockSubType, strFields);
                //获取岩矿类别列表
                jsonStr = mySQLSerVice.getRockTypes();
                classify = JSON.JSON.parse<GetClassify>(jsonStr); 
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxRockType, strFields);

                setBaseDateControl();
                setBaseSpaceControl();
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemRockName, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemRockType, false, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemRockSubType, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemRockAttribute, false, "", "", true);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("无法获取'{0}'的查询服务！", selectedQueryObj.NAME));
                setQueryDisabled();
            }
        }

        /// <summary>
        /// 植被查询初始化 @由查询服务由Tomcat发布改成IIS发布 jianghua 2015.8.15
        /// </summary>
        void ShowPlantQuery()
        {
            try
            {
                if (selectedQueryObj.NAME.Equals("北方植被"))
                {
                    //if (vnorthclient == null)
                    //{
                    //    this.vnorthclient = new VNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap12Endpoint");
                    //}
                    if (mySQLSerVice == null)
                    {
                        this.mySQLSerVice = new Service();
                    }
                    jsonStr = mySQLSerVice.getNorVegTypes();
                    classify = JSON.JSON.parse<GetClassify>(jsonStr);
                    //classify = JSON.JSON.parse<GetClassify>(vnorthclient.getTypes());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantType, strFields);
                    jsonStr = mySQLSerVice.getNorVegCLBW();
                    classify = JSON.JSON.parse<GetClassify>(jsonStr);
                    //classify = JSON.JSON.parse<GetClassify>(vnorthclient.getCLBW());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantPosition, strFields);
                    jsonStr = mySQLSerVice.getNorVegWHQ();
                    classify = JSON.JSON.parse<GetClassify>(jsonStr);
                    //classify = JSON.JSON.parse<GetClassify>(vnorthclient.getWHQ());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantTime, strFields);
                }
                else
                {
//                     if (vsouthclient == null)
//                     {
//                         this.vsouthclient = new VSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
//                     }

                    if (mySQLSerVice == null)
                    {
                        this.mySQLSerVice = new Service();
                    }
                    jsonStr = mySQLSerVice.getSouVegTypes();
                    classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                     classify = JSON.JSON.parse<GetClassify>(vsouthclient.getTypes());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantType, strFields);
                    jsonStr = mySQLSerVice.getSouVegCLBW();
                    classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                     classify = JSON.JSON.parse<GetClassify>(vsouthclient.getCLBW());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantPosition, strFields);
                    jsonStr = mySQLSerVice.getSouVegWHQ();
                    classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                     classify = JSON.JSON.parse<GetClassify>(vsouthclient.getWHQ());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantTime, strFields);
                }

                setBaseDateControl();
                setBaseSpaceControl();
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemPlantName, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemPlantType, false, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemPlantPosition, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemPlantTime, false, "", "", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("无法获取'{0}'的查询服务！", selectedQueryObj.NAME));
                setQueryDisabled();
            }
        }

        /// <summary>
        /// 城市目标初始化  @由查询服务由Tomcat发布改成IIS发布 jianghua 2015.8.15
        /// </summary>
        void ShowCityQuery()
        {
            try
            {
                //第一次调用时初始化
//                 if (cityobjclient == null)
//                 {
//                     this.cityobjclient = new CityObjService.CityObjServicePortTypeClient("CityObjServiceHttpSoap12Endpoint");
//                 }
                if (mySQLSerVice == null)
                {
                    this.mySQLSerVice = new Service();
                }
                jsonStr = mySQLSerVice.getCityTypes();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                 classify = JSON.JSON.parse<GetClassify>(cityobjclient.getTypes());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxCityType, strFields);
                jsonStr = mySQLSerVice.getCityCSMBMC();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                 classify = JSON.JSON.parse<GetClassify>(cityobjclient.getCSMBMC());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxCityName, strFields);
                setBaseDateControl();
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemCityName, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemCityType, false, "", "", true);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("无法获取'{0}'的查询服务！", selectedQueryObj.NAME));
                setQueryDisabled();
            }
        }

        /// <summary>
        /// 设置地表大气查询条件初始化  @由查询服务由Tomcat发布改成IIS发布 jianghua 2015.8.15
        /// </summary>
        void ShowAtmosphereQuery()
        {
            try
            {
//                 if (atmosphereclient == null)
//                 {
//                     this.atmosphereclient = new AtmosphereService.AtmosphereServicePortTypeClient("AtmosphereServiceHttpSoap12Endpoint");
//                 }
                if (mySQLSerVice == null)
                {
                    this.mySQLSerVice = new Service();
                }
                jsonStr = mySQLSerVice.getAtmosZDBH();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                 classify = JSON.JSON.parse<GetClassify>(atmosphereclient.getZDBH());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxAtmosphereCode, strFields);
                jsonStr = mySQLSerVice.getAtmosZDMC();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                 classify = JSON.JSON.parse<GetClassify>(atmosphereclient.getZDMC());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxAtmosphereName, strFields);
                setBaseSpaceControl();
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemAtmosphereName, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemAtmosphereCode, false, "", "", true);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("无法获取'{0}'的查询服务！", selectedQueryObj.NAME));
                setQueryDisabled();
            }
        }

        /// <summary>
        /// 设置水体查询条件初始化 @由查询服务由Tomcat发布改成IIS发布 jianghua 2015.8.15
        /// </summary>
        void ShowWaterQuery()
        {
            try
            {
//                 if (waterclient == null)
//                 {
//                     this.waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
//                 }
                jsonStr = mySQLSerVice.getWaterSSLB();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                 classify = JSON.JSON.parse<GetClassify>(waterclient.getSSLB());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxWaterType, strFields);
                jsonStr = mySQLSerVice.getWaterGPYQ();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                 classify = JSON.JSON.parse<GetClassify>(waterclient.getGPYQ());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxWaterYiQI, strFields);
                jsonStr = mySQLSerVice.getWaterSYMC();
                classify = JSON.JSON.parse<GetClassify>(jsonStr);
//                 classify = JSON.JSON.parse<GetClassify>(waterclient.getSYMC());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxWaterName, strFields);
                setBaseDateControl();
                setBaseSpaceControl();
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemWaterName, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemWaterType, false, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemWaterFactor, true, "", "", true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemWaterYiQI, false, "", "", true);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("无法获取'{0}'的查询服务！", selectedQueryObj.NAME));
                setQueryDisabled();
            }
        }


        void ShowRcdbQuery(string queryType)
        {
            if (muc3DSearcher._selectDataType != null)
            {
                string dataType = muc3DSearcher._selectDataType;
                switch (dataType)
                {
                    case "土壤":
                        ShowSoilQuery();
                        break;
                    case "岩矿":
                        ShowRockQuery();
                        break;
                    case "北方植被":
                        ShowPlantQuery();
                        break;
                    case "南方植被":
                        ShowPlantQuery();
                        break;
                    case "城市目标":
                        ShowCityQuery();
                        break;
                    case "地表大气":
                        ShowAtmosphereQuery();
                        break;
                    case "水体":
                        ShowWaterQuery();
                        break;

                }
            }
            else
            {
                setQueryDisabled();
            }
        }
        #endregion
        void SetAvailableFields(DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryitem, string[] strFields)
        {
            repositoryitem.Items.Clear();
            repositoryitem.Items.AddRange(strFields);
        }
        /// <summary>
        /// 手动选择区域按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemHandleSel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.mucsearcher.Is3DViewer)
            {
                this.mucsearcher.qrstAxGlobeControl1.UsingDrawRectangleTool();
            }
            else
            {
                this.mucsearcher.uc2DSearcher1.enabledDrawPolygon();
                this.mucsearcher.uc2DSearcher1.drawPolygonCompletedEvent += new Action(qrstAxGlobeControl1_OnDrawRectangleCompeleted);
            }
        }

        /// <summary>
        /// 组合新的查询条件后单击确定的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemAddNewQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            #region 检查界面输入是否完整；其次还要补充，输入的正确性
            if (this.barEditItemLogicalOperator.EditValue == null)
            {
                XtraMessageBox.Show("请选择逻辑操作符！");
                return;
            }
            if (this.barEditItemFieldList.EditValue == null)
            {
                XtraMessageBox.Show("请选择检索字段！");
                return;
            }
            if (this.barEditItemFieldOperator.EditValue == null)
            {
                XtraMessageBox.Show("请选择字段操作符！");
                return;
            }
            if (this.barEditItemFieldValue.EditValue == null)
            {
                XtraMessageBox.Show("请输入字段取值！");
                return;
            }
            #endregion
            QRST_DI_DS_MetadataQuery.SimpleCondition sm = new QRST_DI_DS_MetadataQuery.SimpleCondition();
            sm.accessPointField = barEditItemFieldList.EditValue.ToString();
            sm.comparisonOperatorField = barEditItemFieldOperator.EditValue.ToString();
            sm.valueField = barEditItemFieldValue.EditValue.ToString();
            if (sm.comparisonOperatorField.Equals("like"))
            {
                sm.valueField = string.Format("%{0}%", sm.valueField);
            }
            if (sm.comparisonOperatorField.Equals("in"))
            {
                string[] values = sm.valueField.Split(',');
                string strvalue = "";
                foreach (var item in values)
                {
                    strvalue += "'" + item + "',";
                }
                strvalue = strvalue.TrimEnd(',');
                sm.valueField = string.Format(" ({0})", strvalue);
            }
            listSimpleCondistons.Add(sm);
            AddtoMemoText(this.barEditItemLogicalOperator.EditValue.ToString() + "( " + this.barEditItemFieldList.EditValue.ToString() + this.barEditItemFieldOperator.EditValue.ToString() + "'" + this.barEditItemFieldValue.EditValue.ToString() + "' )");
        }
        /// <summary>
        /// 新添加的高级检索条件展示给用户
        /// </summary>
        /// <param name="strNewAdvancedQuery"></param>
        void AddtoMemoText(string strNewAdvancedQuery)
        {
            string strInit = this.barEditItemShowQuery.EditValue.ToString();
            string strtip = string.Concat(string.Format("{0}", strNewAdvancedQuery), ((char)13), ((char)10), strInit);
            this.barEditItemShowQuery.EditValue = strtip;
        }

        /// <summary>
        /// 加载高级查询中的检索字段
        /// </summary>
        void LoadQueryField(DataTable tableStructDt)
        {
            repositoryItemComboBoxFields.Items.Clear();
            for (int i = 0; i < tableStructDt.Rows.Count; i++)
            {
                repositoryItemComboBoxFields.Items.Add(tableStructDt.Rows[i][0]);
            }
        }

        /// <sum
        /// <summary>
        /// 清空用户已经选择的自定义高级检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemClearCustom_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barEditItemShowQuery.EditValue = string.Empty;
            listSimpleCondistons.Clear();
        }

        int GetPageSize()
        {
            int size;
            if (int.TryParse(barEditItemNumPerPage.EditValue.ToString(), out size))
            {
                return size;
            }
            else
            {
                return 100;
            }
        }
        /// <summary>
        /// 检索按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barEditItemBeginDate.EditValue != null && barEditItemEndDate.EditValue != null)
            {
                startTime = DateTime.Parse(barEditItemBeginDate.EditValue.ToString());
                endTime = DateTime.Parse(barEditItemEndDate.EditValue.ToString());
            }
            DateTime nowTime = DateTime.Now;
            TimeSpan BettownTime = nowTime - lastSearchTime;
            if (BettownTime.Days > 0 || BettownTime.Hours > 0 || BettownTime.Minutes > 0 || BettownTime.Seconds > 10 || queryNum == 0)
            {
                lastSearchTime = nowTime;
                queryNum++;
                Query();
            }
            else
            {
                MessageBox.Show("您查询过于频繁，请稍后再试！");
            }
        }

        DataSet distinctDataSet;
        DataTilePara oldTilePara;
        private void Query()
        {
            //try
            //{
                ClearExtentLyr();

                if (mucdetail == null)
                {
                    mucdetail = ((mucDetailViewer)MSUserInterface.listMSUI[1].uiMainUC);
                    mucdetail.VisibleChanged += new EventHandler(mucdetail_VisibleChanged);
                }

                //收集界面上的查询条件，构造查询参数对象queryPara，并构造ComplexCondition,调用IQuery接口完成数据查询
                Check();
                mucdetail = ((mucDetailViewer)MSUserInterface.listMSUI[1].uiMainUC);
                queryPara = null;
                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))    //构造栅格数据queryPara对象
                {
                    queryPara = ConstructRasterQueryPara();
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector")) //构造矢量数据queryPara对象
                {
                    queryPara = ConstructVectorQueryPara();
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_table"))//表格数据仅构造ComplexCondition
                {
                    queryPara = new QueryPara();
                    queryPara.dataCode = selectedQueryObj.DATA_CODE;
                    queryPara.QRST_CODE = "";
                    //表格数据情况特殊，因为波普特征数据库调用外部WebService服务进行查询，为统一集中到查询模块，需要另划界面
                    if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")          //波普特征数据库，特殊情况，若归为统一，则可去掉
                    {
                        DataTable dt = RcdbQuery();

                        if (dt != null)
                        {
                            //设置CtrlPage分页
                            DataSet ds = new DataSet();
                            ds.Tables.Add(dt);
                            IPagingQuery pagingQuery = new DatatablePagingQuery(dt);
                            CtrlPage ctrlPage = mucdetail.GetCtrlPage();
                            ctrlPage.ds = ds;
                            ctrlPage.SetPageSize(GetPageSize());
                            ctrlPage.queryFinishedEventHandle = DataTableQueryFinishedEvent;
                            ctrlPage.Binding(pagingQuery);
                            ctrlPage.FirstQuery();
                            ctrlPage.UpdatePageUC();

                            //parentMSconsole.setNaviItemSelected(barButtonItemViewDetail.Caption);
                        }
                        return;
                    }
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_document"))//构造文档数据queryPara对象
                {
                    queryPara = ConstructDocQueryPara();
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))   //切片数据查询
                {
                    queryPara = new RasterQueryPara();
                    queryPara.QRST_CODE = "TileFileName";
                    if (selectedQueryObj.NAME.Equals("规格化影像数据") || selectedQueryObj.NAME.Equals("规格化影像控制数据"))
                    {
                        DataTilePara tilePara = SetDataTileQueryPara();
                        DateTime dt = DateTime.Parse(tilePara.timePara[0]);

                        tilePara.timePara[0] = dt.ToString("yyyyMMdd");
                        tilePara.timePara[1] = DateTime.Parse(tilePara.timePara[1]).ToString("yyyyMMdd");
                        tilePara.otherQuery = this.barEditItemShowQuery.EditValue.ToString();

                        if (sqliteclient == null)
                        {
                            sqliteclient = new localhostSqlite.Service();
                        }
                        //DLF 130921
                        //ExtentDs = sqliteclient.SearSpaceDistinctTiles(tilePara.spacialPara,
                        //    new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) },
                        //    tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level);
                        //DrawTileAllExtents(ExtentDs);
                        IPagingQuery pagingQuery = new TileDataPagingQuery(sqliteclient, tilePara);
                        mucdetail.isFirst = true;
                        CtrlPage ctrlPage = mucdetail.GetCtrlPage();
                        ctrlPage.SetPageSize(GetPageSize());
                        ctrlPage.queryFinishedEventHandle = QueryFinishedEvent;
                        ctrlPage.Binding(pagingQuery);
                        ctrlPage.FirstQuery();
                        ctrlPage.UpdatePageUC();
                    }
                    else
                    {
                        queryPara = new RasterQueryPara();
                        queryPara.QRST_CODE = "TileFileName";
                        if (selectedQueryObj.NAME.Equals("规格化产品数据"))
                        {
                            ProductTilePara prodPara = SetProductTilePara();
                            DateTime dt = DateTime.Parse(prodPara.timePara[0]);

                            prodPara.timePara[0] = dt.ToString("yyyyMMdd");
                            prodPara.timePara[1] = DateTime.Parse(prodPara.timePara[1]).ToString("yyyyMMdd");

                            if (sqliteclient == null)
                            {
                                sqliteclient = new localhostSqlite.Service();
                            }
                            IPagingQuery pagingQuery = new ProductTileDataPagingQuery(sqliteclient, prodPara);
                            CtrlPage ctrlPage = mucdetail.GetCtrlPage();
                            ctrlPage.SetPageSize(GetPageSize());
                            ctrlPage.queryFinishedEventHandle = QueryFinishedEvent;
                            ctrlPage.FirstQuery();
                            ctrlPage.Binding(pagingQuery);
                            ctrlPage.UpdatePageUC();
                        }
                    }

                }


                if (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                {
                    //构造查询条件对象
                    QRST_DI_DS_MetadataQuery.ComplexCondition queryCondition = ConstructSeniorQueryCondition();
                    if (queryPara != null)
                    {
                        queryCondition.complexCondition = new QRST_DI_DS_MetadataQuery.ComplexCondition[1];
                        queryCondition.complexCondition[0] = queryPara.GetSpecificCondition(querySchema);
                        queryCondition.complexCondition[0].ruleName = rule;
                        try
                        {
                            selectedRect[0] = Convert.ToDouble(barEditItemMinLon.EditValue);
                            selectedRect[1] = Convert.ToDouble(barEditItemMinLat.EditValue);
                            selectedRect[2] = Convert.ToDouble(barEditItemMaxLon.EditValue);
                            selectedRect[3] = Convert.ToDouble(barEditItemMaxLat.EditValue);
                        }
                        catch
                        {
                            selectedRect = new double[] { -180, -90, 180, 90 };
                        }
                        queryCondition.complexCondition[0].selectRation = selectedRect;
                    }
                    queryPara.GetPublicFieldMappedValue(querySchema);
                    //构造查询请求
                    queryRequest = new QRST_DI_DS_MetadataQuery.QueryRequest();
                    queryRequest.complexCondition = queryCondition;
                    queryRequest.dataBase = selectedQueryObj.GROUP_CODE.Substring(0, 4);
                    queryRequest.elementSet = new string[1] { "*" };                            //默认查询全部字段
                    queryRequest.tableCode = selectedQueryObj.DATA_CODE;

                    //触发查询请求（运管系统采用基于视图的全字段查询）,将查询与分页控件绑定
                    ViewBasedQuery queryObj = new ViewBasedQuery(queryRequest, querySchema);
                    IPagingQuery pagingQuery = new MetaDataPagingQuery(queryObj);
                    CtrlPage ctrlPage = mucdetail.GetCtrlPage();
                    ctrlPage.queryFinishedEventHandle = QueryFinishedEvent;
                    ctrlPage.SetPageSize(GetPageSize());
                    ctrlPage.Binding(pagingQuery);
                    ctrlPage.FirstQuery();
                    ctrlPage.UpdatePageUC();
                }


            //}
            //catch (Exception e)
            //{
            //    XtraMessageBox.Show("查询失败：" + e.Message + "\n      要检索的数据已被删除或不存在!");
            //}
        }

        //获取纠正数据查询参数对象
        DataTilePara GetDataTilePara()
        {
            DataTilePara tilePara = SetDataTileQueryPara();
            DateTime dt = DateTime.Parse(tilePara.timePara[0]);

            tilePara.timePara[0] = dt.ToString("yyyyMMdd");
            tilePara.timePara[1] = DateTime.Parse(tilePara.timePara[1]).ToString("yyyyMMdd");
            return tilePara;
        }
        //获取产品数据查询参数对象
        ProductTilePara GetProductTilePara()
        {
            ProductTilePara prodPara = SetProductTilePara();
            DateTime dt = DateTime.Parse(prodPara.timePara[0]);

            prodPara.timePara[0] = dt.ToString("yyyyMMdd");
            prodPara.timePara[1] = DateTime.Parse(prodPara.timePara[1]).ToString("yyyyMMdd");
            return prodPara;
        }
        //查询结束后需要做的工作，如将查询结果显示到详细信息列表，在三维球上绘制查询记录的空间范围 
        public void QueryFinishedEvent()
        {
            //清空原来选择的矩形
            // this.mucsearcher.qrstAxGlobeControl1.DrawSelectedExtents(new List<System.Drawing.RectangleF>());

            queryResponse = new QRST_DI_DS_MetadataQuery.QueryResponse();



            CtrlPage page = mucdetail.GetCtrlPage();
            if (page.dt == null)
            {
                queryResponse.recordSet = null;
            }
            else
            {
                queryResponse.recordSet = page.dt.DataSet;
            }


            mucdetail.queryPara = queryPara;
            mucdetail.selectedQueryObj = selectedQueryObj;
            mucdetail.setGridControl(queryResponse.recordSet);

            //zxw 20131221  如果不是切片，则展示分页查询数据，若为切片，则展示该区域所有数据
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                if (selectedQueryObj.NAME.Equals("规格化产品数据"))
                {
                    ProductTilePara tilePara = GetProductTilePara();
                    //   queryResponse.recordSet = sqliteclient.SearSpaceDistinctTiles(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) },new string[]{tilePara.},);
                }
                else if (selectedQueryObj.NAME.Equals("规格化影像数据"))
                {
                    //查询所有 zxw 20131221
                    //zxw 20131221
                    //DataTilePara tilePara = GetDataTilePara();
                    //if (!tilePara.Equals(oldTilePara))
                    //{
                    //    try
                    //    {
                    //        distinctTileInfo = null;
                    //        oldTilePara = tilePara;
                    //        distinctTileInfo = sqliteclient.SearSpaceDistinctTiles(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level);
                    //        // queryResponse.recordSet = ds;

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        distinctTileInfo = sqliteclient.SearSpaceDistinctTiles(tilePara.spacialPara, new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) }, tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level);

                    //    }

                    //}
                }
            }

            DrawSpacialExtent();
            // mucdetail.RefreshInfo();
            if (this.mucsearcher.Is3DViewer)
            {
                SetGlobeToExtent();
            }
            else
            {
                this.mucsearcher.uc2DSearcher1.zoomToDefault();
            }
        }

        //DataTable查询结束后需要做的工作，如将查询结果显示到详细信息列表，在三维球上绘制查询记录的空间范围 
        public void DataTableQueryFinishedEvent()
        {
            CtrlPage page = mucdetail.GetCtrlPage();
            if (page.dt != null)
            {
                mucdetail.queryPara = queryPara;
                mucdetail.selectedQueryObj = selectedQueryObj;
                mucdetail.setGridControl(page.dt);
            }



            DrawSpacialExtent();
            // mucdetail.RefreshInfo();
            if (this.mucsearcher.Is3DViewer)
            {
                SetGlobeToExtent();
            }
            else
            {
                this.mucsearcher.uc2DSearcher1.zoomToDefault();
            }
        }

        //将球体视角调整为查询范围内
        void SetGlobeToExtent()
        {

            double minlat, minlon, maxlat, maxlon;
            minlat = double.Parse(barEditItemMinLat.EditValue.ToString());
            minlon = double.Parse(barEditItemMinLon.EditValue.ToString());
            maxlat = double.Parse(barEditItemMaxLat.EditValue.ToString());
            maxlon = double.Parse(barEditItemMaxLon.EditValue.ToString());
            if (minlat == -90 && minlon == -180 && maxlat == 90 && maxlon == 180)
            {
                return;
            }
            else if (maxlat - minlat < 0.1 && maxlon - minlon < 0.1)
            {
                maxlat = maxlat + 0.1;
                minlat = minlat - 0.1;
                maxlon = maxlon + 0.1;
                minlon = minlon - 0.1;
            }

            double distance = 92600.48 * Math.Sqrt(Math.Pow(maxlat - minlat, 2) + Math.Pow(maxlon - minlon, 2));
            this.mucsearcher.qrstAxGlobeControl1.SetViewPosition((minlat + maxlat) / 2, (minlon + maxlon) / 2, 0.0, distance, 0.0);


        }

        //将查询结果绘制到三维球上
        public void DrawSpacialExtent()
        {
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
            {
                //绘制空间范围到三维球
                if (((RasterQueryPara)queryPara).spacialAvailable)
                {
                    if (mucsearcher.Is3DViewer)
                    {
                        DrawRasterExtent((RasterQueryPara)queryPara);
                    }
                    else
                    {
                        DrawRaster2D((RasterQueryPara)queryPara);
                    }
                }
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))
            {
                //绘制空间范围到三维球
                if (((VectorQueryPara)queryPara).spacialAvailable)
                    DrawVectorExtent((VectorQueryPara)queryPara);
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                //绘制空间范围到三维球
                if (((RasterQueryPara)queryPara).spacialAvailable)
                {
                    if (mucsearcher.Is3DViewer)
                    {
                        DrawRasterExtent((RasterQueryPara)queryPara);
                    }
                    else
                    {
                        DrawRaster2D((RasterQueryPara)queryPara);
                    }
                }
            }
            //else  //跳转到详细结果页面
            //{
            //    parentMSconsole.setNaviItemSelected(barButtonItemViewDetail.Caption);
            //}
        }


        #region 定义了一下构造queryPara和查询请求的方法
        RasterQueryPara ConstructRasterQueryPara()
        {
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = selectedQueryObj.DATA_CODE;
            _rasterQueryPara.ENDTIME = endTime.ToString();
            _rasterQueryPara.EXTENTDOWN = barEditItemMinLat.EditValue.ToString();
            _rasterQueryPara.EXTENTLEFT = barEditItemMinLon.EditValue.ToString();
            _rasterQueryPara.EXTENTRIGHT = barEditItemMaxLon.EditValue.ToString();
            _rasterQueryPara.EXTENTUP = barEditItemMaxLat.EditValue.ToString();
            _rasterQueryPara.KEYWORDS = barEditItemKeyWord.EditValue.ToString();
            _rasterQueryPara.STARTTIME = startTime.ToString();
            _rasterQueryPara.SATELLITE = barEditItemSateCheck.EditValue.ToString();
            _rasterQueryPara.SENSOR = barEditItemSensorCheck.EditValue.ToString();
            return _rasterQueryPara;
        }

        VectorQueryPara ConstructVectorQueryPara()
        {
            VectorQueryPara _vectorQueryPara = new VectorQueryPara();
            _vectorQueryPara.dataCode = selectedQueryObj.DATA_CODE;
            _vectorQueryPara.ENDTIME = endTime.ToString();
            _vectorQueryPara.EXTENTDOWN = barEditItemMinLat.EditValue.ToString();
            _vectorQueryPara.EXTENTLEFT = barEditItemMinLon.EditValue.ToString();
            _vectorQueryPara.EXTENTRIGHT = barEditItemMaxLon.EditValue.ToString();
            _vectorQueryPara.EXTENTUP = barEditItemMaxLat.EditValue.ToString();
            _vectorQueryPara.KEYWORDS = barEditItemKeyWord.EditValue.ToString();
            _vectorQueryPara.STARTTIME = startTime.ToString();
            _vectorQueryPara.GROUPCODE = selectedQueryObj.GROUP_CODE;
            return _vectorQueryPara;
        }

        DocQueryPara ConstructDocQueryPara()
        {
            DocQueryPara _docQueryPara = new DocQueryPara();
            _docQueryPara.dataCode = selectedQueryObj.DATA_CODE;
            _docQueryPara.ENDTIME = endTime.ToString();
            _docQueryPara.KEYWORDS = barEditItemKeyWord.EditValue.ToString();
            _docQueryPara.NAME = barEditName.EditValue.ToString();
            _docQueryPara.STARTTIME = startTime.ToString();
            return _docQueryPara;
        }

        /// <summary>
        /// 将高级检索中的信息组装成ComplexCondition对象
        /// </summary>
        /// <returns></returns>
        QRST_DI_DS_MetadataQuery.ComplexCondition ConstructSeniorQueryCondition()
        {
            QRST_DI_DS_MetadataQuery.ComplexCondition cp = new QRST_DI_DS_MetadataQuery.ComplexCondition();
            if (listSimpleCondistons.Count != 0)
            {
                cp.logicOperator = QRST_DI_DS_MetadataQuery.EnumLogicalOperator.AND;
                cp.simpleCondition = listSimpleCondistons.ToArray();
            }


            return cp;
        }

        /// <summary>
        /// 处理波普特征数据库查询失败   波普查询服务由Tomcat改成IIS发布 @jianghua 2015.8.15
        /// </summary>
        /// <returns></returns>
        DataTable RcdbQuery()
        {
            DataTable dt = null;
            string begindate = "";
            string enddate = "";
            string MaxLon;
            string Maxlat;
            string MinLon;
            string MinLat;


            try
            {
                List<object> fileinfolist = new List<object>();
                mySQLSerVice = new Service();
                switch (selectedQueryObj.NAME)
                {
                    case "土壤":
                        if (barEditItemBeginDate.EditValue != null)
                        {
                            begindate = barEditItemBeginDate.EditValue.ToString();
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            enddate = barEditItemEndDate.EditValue.ToString();
                        }
                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLat.EditValue.ToString();
                        string soilname = barEditItemSoilName.EditValue == null ? "" : barEditItemSoilName.EditValue.ToString();
                        string soilzilei = barEditItemSoliSubType.EditValue == null ? "" : barEditItemSoliSubType.EditValue.ToString();
//                         string jsonStr = soilclient.getQuerySoils(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, soilname, soilzilei, "", "0", "150");
                        jsonStr = mySQLSerVice.getQuerySoils(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, soilname, soilzilei, "", "0", "150");
                        Soil soil = JSON.JSON.parse<Soil>(jsonStr);
                        foreach (SoilInfo info in soil.soils)
                        {
                            fileinfolist.Add(info);
                        }
                        dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = Soil.soilattributenames[i];
                        }
                        break;
                    case "南方植被":
                        if (barEditItemBeginDate.EditValue != null)
                        {
                            begindate = barEditItemBeginDate.EditValue.ToString();
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            enddate = barEditItemEndDate.EditValue.ToString();
                        }
                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLat.EditValue.ToString();
                        string v_zbmc = barEditItemPlantName.EditValue == null ? "" : barEditItemPlantName.EditValue.ToString();
                        string v_zblb = barEditItemPlantType.EditValue == null ? "" : barEditItemPlantType.EditValue.ToString();
                        string v_clbw = barEditItemPlantPosition.EditValue == null ? "" : barEditItemPlantPosition.EditValue.ToString();
                        string v_whq = barEditItemPlantTime.EditValue == null ? "" : barEditItemPlantTime.EditValue.ToString();
                        //第一次调用时初始化
//                         if (vsouthclient == null)
//                         {
//                             this.vsouthclient = new VSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
//                         }
                        jsonStr = mySQLSerVice.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", "");
                        Vegetation vegetation = JSON.JSON.parse<Vegetation>(jsonStr);
//                         Vegetation vegetation = JSON.JSON.parse<Vegetation>(vsouthclient.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", ""));
                        foreach (VegetationInfo info in vegetation.vegetations)
                        {
                            fileinfolist.Add(info);
                        }
                        dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = Vegetation.vegetationattributenames[i];
                        }
                        break;
                    case "北方植被":
                        if (barEditItemBeginDate.EditValue != null)
                        {
                            begindate = barEditItemBeginDate.EditValue.ToString();
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            enddate = barEditItemEndDate.EditValue.ToString();
                        }
                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLat.EditValue.ToString();
                        v_zbmc = barEditItemPlantName.EditValue == null ? "" : barEditItemPlantName.EditValue.ToString();
                        v_zblb = barEditItemPlantType.EditValue == null ? "" : barEditItemPlantType.EditValue.ToString();
                        v_clbw = barEditItemPlantPosition.EditValue == null ? "" : barEditItemPlantPosition.EditValue.ToString();
                        v_whq = barEditItemPlantTime.EditValue == null ? "" : barEditItemPlantTime.EditValue.ToString();
                        ////////////////////
                        jsonStr = mySQLSerVice.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", "");
                        vegetation = JSON.JSON.parse<Vegetation>(jsonStr);
//                         vegetation = JSON.JSON.parse<Vegetation>(vnorthclient.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", ""));
                        foreach (VegetationInfo info in vegetation.vegetations)
                        {
                            fileinfolist.Add(info);
                        }
                        dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = Vegetation.vegetationattributenames[i];
                        }
                        break;
                    case "城市目标":
                        if (barEditItemBeginDate.EditValue != null)
                        {
                            begindate = barEditItemBeginDate.EditValue.ToString();
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            enddate = barEditItemEndDate.EditValue.ToString();
                        }
                        string c_csmc = barEditItemCityName.EditValue == null ? "" : barEditItemCityName.EditValue.ToString();
                        string c_cslb = barEditItemCityType.EditValue == null ? "" : barEditItemCityType.EditValue.ToString();
                        ////////////////////
                        jsonStr = mySQLSerVice.getQueryCityObjs(begindate, enddate, c_csmc, c_cslb, "", "");
                        CityObj cityObj = JSON.JSON.parse<CityObj>(jsonStr);
//                         CityObj cityObj = JSON.JSON.parse<CityObj>(cityobjclient.getQueryCityObjs(begindate, enddate, c_csmc, c_cslb, "", ""));
                        foreach (CityObjInfo info in cityObj.city_objs)
                        {
                            fileinfolist.Add(info);
                        }
                        dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = CityObj.cityobjattributenames[i];
                        }
                        break;
                    case "地表大气":
                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLat.EditValue.ToString();
                        string a_zdmc = barEditItemAtmosphereName.EditValue == null ? "" : barEditItemAtmosphereName.EditValue.ToString();
                        string a_zdbh = barEditItemAtmosphereCode.EditValue == null ? "" : barEditItemAtmosphereCode.EditValue.ToString();
                        ////////////////////
                        jsonStr = mySQLSerVice.getQueryAtmospheres(Maxlat, MaxLon, MinLat, MinLon, a_zdmc, a_zdbh, "", "");
                        Atmosphere atmosphere = JSON.JSON.parse<Atmosphere>(jsonStr);
//                         Atmosphere atmosphere = JSON.JSON.parse<Atmosphere>(atmosphereclient.getQueryAtmospheres(Maxlat, MaxLon, MinLat, MinLon, a_zdmc, a_zdbh, "", ""));
                        foreach (AtmosphereInfo info in atmosphere.atmospheres)
                        {
                            fileinfolist.Add(info);
                        }
                        dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = Atmosphere.atmosphereattributenames[i];
                        }
                        break;
                    case "水体":
                        if (barEditItemBeginDate.EditValue != null)
                        {
                            begindate = barEditItemBeginDate.EditValue.ToString();
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            enddate = barEditItemEndDate.EditValue.ToString();
                        }
                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLat.EditValue.ToString();
                        string w_symc = barEditItemWaterName.EditValue == null ? "" : barEditItemWaterName.EditValue.ToString();
                        string w_sslb = barEditItemWaterType.EditValue == null ? "" : barEditItemWaterType.EditValue.ToString();
                        string w_dltz = barEditItemWaterFactor.EditValue == null ? "" : barEditItemWaterFactor.EditValue.ToString();
                        string w_gpyq = barEditItemWaterYiQI.EditValue == null ? "" : barEditItemWaterYiQI.EditValue.ToString();
                        ////////////////////
                        jsonStr = mySQLSerVice.getQueryWaters(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, w_symc, w_gpyq, w_sslb, "", "");
                        Water water = JSON.JSON.parse<Water>(jsonStr);
//                         Water water = JSON.JSON.parse<Water>(waterclient.getQueryWaters(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, w_symc, w_gpyq, w_sslb, "", ""));
                        foreach (WaterInfo info in water.waters)
                        {
                            fileinfolist.Add(info);
                        }
                        dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = Water.waterattributenames[i];
                        }
                        break;
                    case "岩矿":
                        if (barEditItemBeginDate.EditValue != null)
                        {
                            begindate = barEditItemBeginDate.EditValue.ToString();
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            enddate = barEditItemEndDate.EditValue.ToString();
                        }
                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLat.EditValue.ToString();
                        string r_ykmc = barEditItemRockName.EditValue == null ? "" : barEditItemRockName.EditValue.ToString();
                        string r_yklb = barEditItemRockType.EditValue == null ? "" : barEditItemRockType.EditValue.ToString();
                        string r_ykzl = barEditItemRockSubType.EditValue == null ? "" : barEditItemRockSubType.EditValue.ToString();
                        string r_sslb = barEditItemRockAttribute.EditValue == null ? "" : barEditItemRockAttribute.EditValue.ToString();
                        ////////////////////
                        jsonStr = mySQLSerVice.getQueryRocks(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, r_ykmc, r_yklb, r_ykzl, r_sslb, "", "");
                        RockMineral rockmineral = JSON.JSON.parse<RockMineral>(jsonStr);
//                         RockMineral rockmineral = JSON.JSON.parse<RockMineral>(rockclient.getQueryRocks(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, r_ykmc, r_yklb, r_ykzl, r_sslb, "", ""));
                        foreach (RockMineralInfo info in rockmineral.rocks)
                        {
                            fileinfolist.Add(info);
                        }
                        dt = Utilties.ConvertObjPro2DataTable(fileinfolist);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = RockMineral.rockMineralattributenames[i];
                        }
                        break;
                    default:
                        MessageBox.Show("请选择地物类型！");
                        break;
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("无法获取查询数据!");
            }
            return dt;
        }

        /// <summary>
        /// 填充数据切片查询条件
        /// </summary>
        /// <returns></returns>
        DataTilePara SetDataTileQueryPara()
        {
            DataTilePara dataTilePara = new DataTilePara();
            //if()
            {
                dataTilePara.spacialPara[0] = this.barEditItemMinLat.EditValue.ToString();
                dataTilePara.spacialPara[1] = this.barEditItemMinLon.EditValue.ToString();
                dataTilePara.spacialPara[2] = this.barEditItemMaxLat.EditValue.ToString();
                dataTilePara.spacialPara[3] = this.barEditItemMaxLon.EditValue.ToString();
                dataTilePara.ColAndRow[0] = this.barEditItemMinRow.EditValue.ToString();
                dataTilePara.ColAndRow[1] = this.barEditItemMinCol.EditValue.ToString();

                dataTilePara.ColAndRow[2] = this.barEditItemMaxRow.EditValue.ToString();
                dataTilePara.ColAndRow[3] = this.barEditItemMaxCol.EditValue.ToString();
                dataTilePara.timePara[0] = startTime.ToString();
                dataTilePara.timePara[1] = endTime.ToString();
            }
            if (this.barEditItemSateCheck.EditValue == null || this.barEditItemSateCheck.EditValue.ToString() == "")
            {
                //XtraMessageBox.Show("请选择卫星类型！");
                dataTilePara.satelliteType = new string[] { };
            }
            else
            {
                dataTilePara.satelliteType = this.barEditItemSateCheck.EditValue.ToString().Trim().Split(',');
                for (int i = 0; i < dataTilePara.satelliteType.Length; i++)
                {
                    dataTilePara.satelliteType[i] = dataTilePara.satelliteType[i].Trim();
                }
            }

            if (this.barEditItemSensorCheck.EditValue == null || this.barEditItemSensorCheck.EditValue.ToString() == "")
            {
                //XtraMessageBox.Show("请选择传感器类型！");
                dataTilePara.sensorType = new string[] { };
            }
            else
            {
                dataTilePara.sensorType = this.barEditItemSensorCheck.EditValue.ToString().Split(',');
                for (int i = 0; i < dataTilePara.sensorType.Length; i++)
                {
                    dataTilePara.sensorType[i] = dataTilePara.sensorType[i].Trim();
                }
            }

            if (this.barEditItemDataTypeCheck.EditValue == null || this.barEditItemDataTypeCheck.EditValue.ToString() == "")
            {
                //XtraMessageBox.Show("请选择要查询的数据切片类型！");
                dataTilePara.dataTileType = new string[] { };
            }
            else
            {
                dataTilePara.dataTileType = this.barEditItemDataTypeCheck.EditValue.ToString().Trim().Split(',');
                for (int i = 0; i < dataTilePara.dataTileType.Length; i++)
                {
                    dataTilePara.dataTileType[i] = dataTilePara.dataTileType[i].Trim();
                }
            }

            if (this.barEditItemTileLevel.EditValue == null || this.barEditItemTileLevel.EditValue.ToString() == "")
            {
                //只要有空间范围就必须传 等级取值。但界面中可能没有选择，此时自动查询并添加全部等级。
                if (dataTilePara.spacialPara.Length > 0)
                {
                    if (sqliteclient == null)
                    {
                        sqliteclient = new localhostSqlite.Service();
                    }
                    //dataTilePara.level = sqliteclient.SearTileLevels();
                    List<string> levels = new List<string>();
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem strresobj in repositoryItemCheckedComboBoxEditTileLevel.Items)
                    {
                        string strres = strresobj.Value.ToString();
                        string lvstr = DirectlyAddressing.GetStrLvByResolution(strres);
                        if (lvstr.Trim() != "")
                        {
                            levels.Add(lvstr);
                        }
                    }
                    dataTilePara.level = levels.ToArray();
                }
                else
                {
                    dataTilePara.level = new string[] { };
                }
            }
            else
            {
                List<string> levels = new List<string>();
                string[] reslevels = this.barEditItemTileLevel.EditValue.ToString().Trim().Split(',');
                foreach (string strres in reslevels)
                {
                    string lvstr = DirectlyAddressing.GetStrLvByResolution(strres);
                    if (lvstr.Trim() != "")
                    {
                        levels.Add(lvstr);
                    }
                }
                dataTilePara.level = levels.ToArray();
            }

            return dataTilePara;
        }
        /// <summary>
        /// 填充产品切片查询条件
        /// </summary>
        /// <returns></returns>
        ProductTilePara SetProductTilePara()
        {
            ProductTilePara proTilePara = new ProductTilePara();
            //if()
            {
                proTilePara.spacialPara[0] = this.barEditItemMinLat.EditValue.ToString();
                proTilePara.spacialPara[1] = this.barEditItemMinLon.EditValue.ToString();
                proTilePara.spacialPara[2] = this.barEditItemMaxLat.EditValue.ToString();
                proTilePara.spacialPara[3] = this.barEditItemMaxLon.EditValue.ToString();

                proTilePara.timePara[0] = this.barEditItemBeginDate.EditValue.ToString();
                proTilePara.timePara[1] = this.barEditItemEndDate.EditValue.ToString();
            }
            if (this.barEditItemProTypeCombo.EditValue == null || this.barEditItemProTypeCombo.EditValue.ToString() == "")
            {
                proTilePara.productType = new string[] { };
            }
            else
            {
                proTilePara.productType = new string[] { this.barEditItemProTypeCombo.EditValue.ToString() };
            }

            if (this.barEditItemTileLevel.EditValue == null || this.barEditItemTileLevel.EditValue.ToString() == "")
            {
                //只要有空间范围就必须传 等级取值。但界面中可能没有选择，此时自动查询并添加全部等级。
                if (proTilePara.spacialPara.Length > 0)
                {
                    if (sqliteclient == null)
                    {
                        sqliteclient = new localhostSqlite.Service();
                    }
                    proTilePara.level = sqliteclient.SearProTileLevels();
                }
                else
                {
                    proTilePara.level = new string[] { };
                }
            }
            else
            {
                proTilePara.level = this.barEditItemTileLevel.EditValue.ToString().Trim().Split(',');
            }

            return proTilePara;
        }

        #endregion

        #region 定义绘制空间范围的方法

        /// <summary>
        /// 清理图层
        /// </summary>
        void ClearExtentLyr()
        {
            List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
            this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents);
            this.mucsearcher.qrstAxGlobeControl1.DrawCheckedExtents(extents);
            Dictionary<System.Drawing.RectangleF, int> extents1 = new Dictionary<System.Drawing.RectangleF, int>();
            int maxCount;
            this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents1, out maxCount);
            if (this.mucsearcher.qrstAxGlobeControl1.Controls.ContainsKey("colorRange"))
            {
                this.mucsearcher.qrstAxGlobeControl1.Controls.RemoveByKey("colorRange");
            }
        }
        void DrawRaster2D(RasterQueryPara queryPara)
        {
            this.mucsearcher.uc2DSearcher1.layerClear();
            if (queryPara.spacialAvailable)
            {

                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();

                if (queryResponse.recordSet == null || queryResponse.recordSet.Tables.Count == 0)
                {
                    return;
                }

                //不要重复绘制 zxw 20131221 
                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                {
                    Dictionary<System.Drawing.RectangleF, int> extents1 = new Dictionary<System.Drawing.RectangleF, int>();
                    for (int i = 0; i < queryResponse.recordSet.Tables[0].Rows.Count; i++)
                    {
                        string level = queryResponse.recordSet.Tables[0].Rows[i]["Level"].ToString();
                        string[] rowandColumn = new string[4];
                        rowandColumn[0] = queryResponse.recordSet.Tables[0].Rows[i]["Row"].ToString();
                        rowandColumn[1] = queryResponse.recordSet.Tables[0].Rows[i]["Col"].ToString();

                        double[] extent = GetLatAndLong(rowandColumn, level);

                        float minLat = float.Parse(extent[0].ToString());
                        float maxLat = float.Parse(extent[2].ToString());
                        float minLon = float.Parse(extent[1].ToString());
                        float maxLon = float.Parse(extent[3].ToString());

                        Point2d lowerleft = new Point2d(minLat, minLon);
                        Point2d upperright = new Point2d(maxLat, maxLon);
                        System.Drawing.RectangleF key;
                        Point2d[] extent1 = new Point2d[] { lowerleft, upperright };

                        if (ExitRectage(extents1, new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[0].X - extent1[1].X)), out key))
                        {
                            extents1[key] = extents1[key] + 1;
                        }
                        else
                        {
                            extents1.Add(key, 1);
                        }


                        //  extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
                    }
                    int maxCount = 0;
                    this.mucsearcher.uc2DSearcher1.DrawSearchResultExtents(extents1, out maxCount);
                    ColorRange colorRange = new ColorRange(maxCount, this.mucsearcher.uc2DSearcher1);
                    if (this.mucsearcher.uc2DSearcher1.Controls.ContainsKey("colorRange"))
                    {
                        this.mucsearcher.uc2DSearcher1.Controls.RemoveByKey("colorRange");
                    }
                    this.mucsearcher.uc2DSearcher1.Controls.Add(colorRange);
                    colorRange.BringToFront();
                }
                else
                {
                    for (int i = 0; i < queryResponse.recordSet.Tables[0].Rows.Count; i++)
                    {
                        float minLat = Math.Min(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLAT].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLAT].ToString()));
                        float maxLat = Math.Max(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLAT].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLAT].ToString()));
                        float minLon = Math.Min(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLONG].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLONG].ToString()));
                        float maxLon = Math.Max(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLONG].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLONG].ToString()));
                        extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
                    }
                    this.mucsearcher.uc2DSearcher1.DrawSearchResultExtents(extents);
                }
            }
            else
            {
                this.mucsearcher.uc2DSearcher1.layerClear();
            }
        }

        //zxw 20131222 
        DataSet distinctTileInfo;  //用于存放切片覆盖范围的统计信息与空间信息

        /// <summary>
        /// 获取切片覆盖范围统计信息 by zxw 20131222
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public Dictionary<System.Drawing.RectangleF, int> GetTileStasticExtentInfo(DataSet ds)
        {
            Dictionary<System.Drawing.RectangleF, int> extents1 = new Dictionary<System.Drawing.RectangleF, int>();
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string level = ds.Tables[0].Rows[i]["Level"].ToString();
                    string[] rowandColumn = new string[4];
                    rowandColumn[0] = ds.Tables[0].Rows[i]["Row"].ToString();
                    rowandColumn[1] = ds.Tables[0].Rows[i]["Col"].ToString();

                    double[] extent = GetLatAndLong(rowandColumn, level);

                    float minLat = float.Parse(extent[0].ToString());
                    float maxLat = float.Parse(extent[2].ToString());
                    float minLon = float.Parse(extent[1].ToString());
                    float maxLon = float.Parse(extent[3].ToString());

                    //int.Parse(tilePara.timePara[1]) }
                    //直接将统计后的结果绘制到球体上 20130912

                    Point2d lowerleft = new Point2d(minLat, minLon);
                    Point2d upperright = new Point2d(maxLat, maxLon);
                    System.Drawing.RectangleF key;
                    Point2d[] extent1 = new Point2d[] { lowerleft, upperright };

                    //new version by zxw
                    RectangleF reckey = new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[1].X - extent1[0].X));
                    if (!extents1.ContainsKey(reckey))
                    {
                        extents1.Add(reckey, int.Parse(ds.Tables[0].Rows[i]["Spacecount"].ToString()));
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
            }
            return extents1;
        }

        public Dictionary<System.Drawing.RectangleF, int> GetQueryTileStasticExtentInfo()
        {
            Dictionary<System.Drawing.RectangleF, int> extents1 = new Dictionary<System.Drawing.RectangleF, int>();

            Dictionary<string, int> extents0 = new Dictionary<string, int>();

            DataTable dt=queryResponse.recordSet.Tables[0];
            for (int ii = 0; ii < dt.Rows.Count; ii++)
            {
                string lrc = string.Format("{0}-{1}-{2}", dt.Rows[ii]["Level"].ToString(), dt.Rows[ii]["Row"].ToString(), dt.Rows[ii]["Col"].ToString());
                if (extents0.Keys.Contains(lrc))
                {
                    extents0[lrc] = extents0[lrc] + 1;
                }
                else
                {
                    extents0.Add(lrc, 1);
                }
            }

            foreach (KeyValuePair<string, int> kvp in extents0)
            {
                string[] lrc = kvp.Key.Split("-".ToCharArray());
                double[] extent = GetLatAndLong(lrc[1], lrc[2], lrc[0]);

                float minLat = float.Parse(extent[0].ToString());
                float maxLat = float.Parse(extent[2].ToString());
                float minLon = float.Parse(extent[1].ToString());
                float maxLon = float.Parse(extent[3].ToString());

                //int.Parse(tilePara.timePara[1]) }
                //直接将统计后的结果绘制到球体上 20130912

                Point2d lowerleft = new Point2d(minLat, minLon);
                Point2d upperright = new Point2d(maxLat, maxLon);
                Point2d[] extent1 = new Point2d[] { lowerleft, upperright };

                RectangleF ext = new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[1].X - extent1[0].X));
                extents1.Add(ext, kvp.Value);
            }

            return extents1;
        }
        /// <summary>
        /// 绘制三维球范围
        /// </summary>
        /// <param name="queryPara"></param>
        void DrawRasterExtent(RasterQueryPara queryPara)
        {
            ClearExtentLyr();
            if (queryPara.spacialAvailable)
            {

                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();

                //DLF20130822 因异常添加
                if (queryResponse.recordSet == null || queryResponse.recordSet.Tables.Count == 0)
                {
                    return;
                }

                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                {
                    Dictionary<System.Drawing.RectangleF, int> extents1;
                    if (mucsearcher.Visible)
                    {
                        //extents1 = GetTileStasticExtentInfo(distinctTileInfo);
                        extents1 = GetQueryTileStasticExtentInfo();
                    }
                    else
                    {
                        extents1 = GetQueryTileStasticExtentInfo();
                    }

                    int maxCount = 0;
                    this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents1, out maxCount);
                    ColorRange colorRange = new ColorRange(maxCount, this.mucsearcher.qrstAxGlobeControl1);
                    colorRange.Visible = this.mucsearcher.qrstAxGlobeControl1.IsOn("tmpDrawExtentsLayer1");
                    //添加统计色带
                    if (this.mucsearcher.qrstAxGlobeControl1.Controls.ContainsKey("colorRange"))
                    {
                        this.mucsearcher.qrstAxGlobeControl1.Controls.RemoveByKey("colorRange");
                    }
                    this.mucsearcher.qrstAxGlobeControl1.Controls.Add(colorRange);
                }
                else
                {
                    for (int i = 0; i < queryResponse.recordSet.Tables[0].Rows.Count; i++)
                    {
                        float minLat = Math.Min(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLAT].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLAT].ToString()));
                        float maxLat = Math.Max(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLAT].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLAT].ToString()));
                        float minLon = Math.Min(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLONG].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLONG].ToString()));
                        float maxLon = Math.Max(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLONG].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLONG].ToString()));
                        extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
                    }
                    this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents);
                }

            }
            else
            {
                ClearExtentLyr();
            }
        }

        ////zxw 20131222 
        //DataSet distinctTileInfo;  //用于存放切片覆盖范围的统计信息与空间信息

        ///// <summary>
        ///// 获取切片覆盖范围统计信息 by zxw 20131222
        ///// </summary>
        ///// <param name="ds"></param>
        ///// <returns></returns>
        //public Dictionary<System.Drawing.RectangleF, int> GetTileStasticExtentInfo(DataSet ds)
        //{
        //    Dictionary<System.Drawing.RectangleF, int> extents1 = new Dictionary<System.Drawing.RectangleF, int>();
        //    if (ds != null)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            string level = ds.Tables[0].Rows[i]["Level"].ToString();
        //            string[] rowandColumn = new string[4];
        //            rowandColumn[0] = ds.Tables[0].Rows[i]["Row"].ToString();
        //            rowandColumn[1] = ds.Tables[0].Rows[i]["Col"].ToString();

        //            double[] extent = GetLatAndLong(rowandColumn, level);

        //            float minLat = float.Parse(extent[0].ToString());
        //            float maxLat = float.Parse(extent[2].ToString());
        //            float minLon = float.Parse(extent[1].ToString());
        //            float maxLon = float.Parse(extent[3].ToString());

        //            //int.Parse(tilePara.timePara[1]) }
        //            //直接将统计后的结果绘制到球体上 20130912

        //            Point2d lowerleft = new Point2d(minLat, minLon);
        //            Point2d upperright = new Point2d(maxLat, maxLon);
        //            System.Drawing.RectangleF key;
        //            Point2d[] extent1 = new Point2d[] { lowerleft, upperright };

        //            //new version by zxw
        //            RectangleF reckey = new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[1].X - extent1[0].X));
        //            extents1.Add(reckey, int.Parse(ds.Tables[0].Rows[i]["Spacecount"].ToString()));

        //        }
        //    }
        //    return extents1;
        //}


        //public Dictionary<System.Drawing.RectangleF, int> GetQueryTileStasticExtentInfo()
        //{
        //    Dictionary<System.Drawing.RectangleF, int> extents1 = new Dictionary<System.Drawing.RectangleF, int>();
        //    for (int i = 0; i < queryResponse.recordSet.Tables[0].Rows.Count; i++)
        //    {
        //        string level = queryResponse.recordSet.Tables[0].Rows[i]["Level"].ToString();
        //        string[] rowandColumn = new string[4];
        //        rowandColumn[0] = queryResponse.recordSet.Tables[0].Rows[i]["Row"].ToString();
        //        rowandColumn[1] = queryResponse.recordSet.Tables[0].Rows[i]["Col"].ToString();

        //        double[] extent = GetLatAndLong(rowandColumn, level);

        //        float minLat = float.Parse(extent[0].ToString());
        //        float maxLat = float.Parse(extent[2].ToString());
        //        float minLon = float.Parse(extent[1].ToString());
        //        float maxLon = float.Parse(extent[3].ToString());

        //        //int.Parse(tilePara.timePara[1]) }
        //        //直接将统计后的结果绘制到球体上 20130912

        //        Point2d lowerleft = new Point2d(minLat, minLon);
        //        Point2d upperright = new Point2d(maxLat, maxLon);
        //        System.Drawing.RectangleF key;
        //        Point2d[] extent1 = new Point2d[] { lowerleft, upperright };

        //        //zxw 20131221 直接获取区域覆盖的数量

        //        //old version
        //        if (ExitRectage(extents1, new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[1].X - extent1[0].X)), out key))
        //        {
        //            extents1[key] = extents1[key] + 1;
        //        }
        //        else
        //        {
        //            extents1.Add(key, 1);
        //        }

        //    }
        //    return extents1;
        //}
        /// <summary>
        /// 根据切片查询结果（全部区域），划框。 
        /// </summary>
        /// <returns></returns>
        void DrawTileAllExtents(DataSet inds)
        {
            ClearExtentLyr();

            Dictionary<System.Drawing.RectangleF, int> extentReturn = new Dictionary<System.Drawing.RectangleF, int>();
            for (int i = 0; i < inds.Tables[0].Rows.Count; i++)
            {
                string level = inds.Tables[0].Rows[i]["Level"].ToString();
                string[] rowandColumn = new string[4];
                rowandColumn[0] = inds.Tables[0].Rows[i]["Row"].ToString();
                rowandColumn[1] = inds.Tables[0].Rows[i]["Col"].ToString();

                double[] extent = GetLatAndLong(rowandColumn, level);

                float minLat = float.Parse(extent[0].ToString());
                float maxLat = float.Parse(extent[2].ToString());
                float minLon = float.Parse(extent[1].ToString());
                float maxLon = float.Parse(extent[3].ToString());


                //直接将统计后的结果绘制到球体上 20130912

                Point2d lowerleft = new Point2d(minLat, minLon);
                Point2d upperright = new Point2d(maxLat, maxLon);
                System.Drawing.RectangleF key;
                Point2d[] extent1 = new Point2d[] { lowerleft, upperright };

                if (ExitRectage(extentReturn, new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[0].X - extent1[1].X)), out key))
                {
                    extentReturn[key] = extentReturn[key] + 1;
                }
                else
                {
                    extentReturn.Add(key, 1);
                }


                //  extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
            }
            int maxCount = 0;
            this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extentReturn, out maxCount);
            ColorRange colorRange = new ColorRange(maxCount, this.mucsearcher.qrstAxGlobeControl1);
            colorRange.Visible = this.mucsearcher.qrstAxGlobeControl1.IsOn("tmpDrawExtentsLayer1");
            //添加统计色带
            if (this.mucsearcher.qrstAxGlobeControl1.Controls.ContainsKey("colorRange"))
            {
                this.mucsearcher.qrstAxGlobeControl1.Controls.RemoveByKey("colorRange");
            }
            this.mucsearcher.qrstAxGlobeControl1.Controls.Add(colorRange);
        }
        /// <summary>
        /// 判断矩形网格是否存在
        /// </summary>
        /// <param name="extents"></param>
        /// <param name="rectangleF"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool ExitRectage(Dictionary<System.Drawing.RectangleF, int> extents, System.Drawing.RectangleF rectangleF, out System.Drawing.RectangleF key)
        {
            foreach (KeyValuePair<System.Drawing.RectangleF, int> kvp in extents)
            {
                if (kvp.Key.Equals(rectangleF))
                {
                    key = kvp.Key;
                    return true;
                }
            }

            key = rectangleF;
            return false;
        }

        void DrawVectorExtent(VectorQueryPara queryPara)
        {
            if (queryPara.spacialAvailable)
            {
                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
                for (int i = 0; i < queryResponse.recordSet.Tables[0].Rows.Count; i++)
                {
                    extents.Add(new System.Drawing.RectangleF(
                    float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentLeftField].ToString()),
                    float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentDownField].ToString()),
                    float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentRightField].ToString()) - float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentLeftField].ToString()),
                    float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentUpField].ToString()) - float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentDownField].ToString())));
                }
                this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents);
            }
        }

        void DrawTileExtent(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string level = dt.Rows[i]["Level"].ToString();
                string[] rowandColumn = new string[4];
                rowandColumn[0] = dt.Rows[i]["Row"].ToString();
                rowandColumn[1] = dt.Rows[i]["Col"].ToString();

                double[] extent = GetLatAndLong(rowandColumn, level);

                DrawSingalTileExtent(new float[] { (float)extent[0], (float)extent[1], (float)extent[2], (float)extent[3] });
            }
        }

        public static double[] GetLatAndLong(string[] rowAndColum, string lv)
        {
            return GetLatAndLong(rowAndColum[0], rowAndColum[1], lv);
        }

        public static double[] GetLatAndLong(string row, string col, string lv)
        {
            double a = DirectlyAddressing.getLevelRate(lv);
            double[] latAndlong = new double[4];
            latAndlong[0] = Convert.ToDouble(row) / a - 90;
            latAndlong[1] = Convert.ToDouble(col) / a - 180;
            latAndlong[2] = Convert.ToDouble(Convert.ToInt32(row) + 1) / a - 90;
            latAndlong[3] = Convert.ToDouble(Convert.ToInt32(col) + 1) / a - 180;
            return latAndlong;
        }

        void DrawSingalTileExtent(float[] extent)
        {
            List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
            extents.Add(new System.Drawing.RectangleF(
                 extent[0],
                 extent[1],
                 extent[2] - extent[0],
                 extent[3] - extent[1]));
            this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents);
        }
        #endregion
        /// <summary>
        /// 检核并修正一些不合法的用户输入
        /// </summary>
        void Check()
        {
            if (barEditItemEndDate.EditValue == null || string.IsNullOrEmpty(barEditItemEndDate.EditValue.ToString()))
            {
                this.barEditItemEndDate.EditValue = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemEndDate);
            }
            if (barEditItemBeginDate.EditValue == null || string.IsNullOrEmpty(barEditItemBeginDate.EditValue.ToString()))
            {
                this.barEditItemBeginDate.EditValue = string.Format("{0:yyyy/MM/dd}", DateTime.Now.AddYears(-20));
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemBeginDate);
            }
            double temp;
            if (barEditItemMinLat.EditValue == null || !double.TryParse(barEditItemMinLat.EditValue.ToString(), out temp))
            {
                barEditItemMinLat.EditValue = -90;
            }
            if (barEditItemMinLon.EditValue == null || !double.TryParse(barEditItemMinLon.EditValue.ToString(), out temp))
            {
                barEditItemMinLon.EditValue = -180;
            }
            if (barEditItemMaxLon.EditValue == null || !double.TryParse(barEditItemMaxLon.EditValue.ToString(), out temp))
            {
                barEditItemMaxLon.EditValue = 180;
            }
            if (barEditItemMaxLat.EditValue == null || !double.TryParse(barEditItemMaxLat.EditValue.ToString(), out temp))
            {
                barEditItemMaxLat.EditValue = 90;
            }
            if (barEditName.EditValue == null)
            {
                barEditName.EditValue = "";
            }
            if (barEditItemSensorCheck.EditValue == null)
            {
                barEditItemSensorCheck.EditValue = "";
            }
            if (barEditItemSateCheck.EditValue == null)
            {
                barEditItemSateCheck.EditValue = "";
            }
        }


        ///// <summary>
        ///// 查看详细信息 按钮单击事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void barButtonItemViewDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    parentMSconsole.setNaviItemSelected(barButtonItemViewDetail.Caption);
        //    //parentMSconsole.SelectMSUI(msUCDetail);
        //}

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ruc3DSearcher_Load(object sender, EventArgs e)
        {

        }

        private void barEditItemShowQuery_EditValueChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 高级检索选项显示/隐藏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemShowCustom_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonItemShowCustom.Caption == "显示高级检索")
            {
                this.ribbonPageGroupCustomQuery.Visible = true;
                barButtonItemShowCustom.Caption = "取消高级检索";
                return;
            }
            if (barButtonItemShowCustom.Caption == "取消高级检索")
            {
                this.ribbonPageGroupCustomQuery.Visible = false;
                barButtonItemShowCustom.Caption = "显示高级检索";
                return;
            }
        }

        /// <summary>
        /// 更新维护 按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemMaintain_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 树形列表控件项选中后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewDataType_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        /// <summary>
        /// 地球视角归位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonEarthViewInitial_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.mucsearcher.qrstAxGlobeControl1.SetViewOriginal();
            this.mucsearcher.uc2DSearcher1.zoomToDefault(); ;
        }
        #region 方法
        ///// <summary>
        ///// 关闭popup控件方法
        ///// </summary>
        //void ClosePopup()
        //{
        //    if (popupContainerControlDataTree.OwnerEdit != null)
        //        popupContainerControlDataTree.OwnerEdit.ClosePopup();
        //}




        #endregion

        private void repositoryItemRangeTrackBar1_BeforeShowValueToolTip(object sender, TrackBarValueToolTipEventArgs e)
        {
            RangeTrackBarControl rangeTrackBarControl = (RangeTrackBarControl)sender;
            int smallValue = rangeTrackBarControl.Value.Minimum;
            int maxValue = rangeTrackBarControl.Value.Maximum;

            DateTime startTime = DateTime.Parse(barEditItemBeginDate.EditValue.ToString());
            DateTime endTime = DateTime.Parse(barEditItemEndDate.EditValue.ToString());

            TimeSpan ts = endTime - startTime;
            int seconds = (int)ts.TotalSeconds / 10;                 //trackBar的每一隔代表的秒数
            TimeSpan ts1 = new TimeSpan(0, 0, seconds * smallValue);
            DateTime rangeStartTime = startTime + ts1;
            ts1 = new TimeSpan(0, 0, seconds * maxValue);
            DateTime rangeEndTime = startTime + ts1;

            e.ShowArgs.ToolTip = string.Format("起止时间：{0}-{1}", rangeStartTime, rangeEndTime);
        }

        /// <summary>
        /// 起止时间变化时，trackBar置为两端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItemBeginDate_EditValueChanged(object sender, EventArgs e)
        {
            string strTime = ((BarEditItem)sender).EditValue.ToString();
            DateTime dt = DateTime.Parse(strTime);
            DateTime dn = new DateTime(dt.Year, dt.Month, dt.Day);
            barEditItemBeginDate.EditValue = dn.ToString();
            //barEditItemBeginDate.EditValue = ((BarEditItem)sender).EditValue.ToString();
            // object OB = barEditItemTimerTrackBar.EditValue;
        }

        private void barEditItemEndDate_EditValueChanged(object sender, EventArgs e)
        {
            string strTime = ((BarEditItem)sender).EditValue.ToString();
            DateTime dt = DateTime.Parse(strTime);
            DateTime dn = new DateTime(dt.Year, dt.Month, dt.Day);
            barEditItemEndDate.EditValue = dn.ToString();
            // barEditItemEndDate.EditValue = ((BarEditItem)sender).EditValue.ToString();
        }

        public RangeTrackBarControl temp;
        /// <summary>
        /// 时间范围变化时，触发查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemRangeTrackBar1_EditValueChanged(object sender, EventArgs e)
        {

            RangeTrackBarControl rangeTrackBarControl = (RangeTrackBarControl)sender;

            temp = rangeTrackBarControl;
            int smallValue = rangeTrackBarControl.Value.Minimum;
            int maxValue = rangeTrackBarControl.Value.Maximum;
            if (maxValue == 0)
            {
                return;
            }
            DateTime editstartTime = DateTime.Parse(barEditItemBeginDate.EditValue.ToString());
            DateTime editendTime = DateTime.Parse(barEditItemEndDate.EditValue.ToString());
            TimeSpan ts = editendTime - editstartTime;
            int seconds = (int)ts.TotalSeconds / 10;                 //trackBar的每一隔代表的秒数
            TimeSpan ts1 = new TimeSpan(0, 0, seconds * smallValue);
            startTime = editstartTime + ts1;
            ts1 = new TimeSpan(0, 0, seconds * maxValue);
            endTime = editstartTime + ts1;

            Query();
        }

        /// <summary>
        /// 鼠标滚动时改变值范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemRangeTrackBar1_MouseWheel(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// 加载坐标文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemLoadPositionFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "(坐标文件.txt)|*.txt";

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(fileDlg.FileName, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);

                    int recordCount = int.Parse(sr.ReadLine());
                    Point3d[] posArr = new Point3d[recordCount];
                    double maxLat = 90, maxLon = 180, minLat = -90, minLon = -180;

                    for (int i = 0; i < posArr.Length; i++)
                    {
                        string pos = sr.ReadLine();
                        string[] latAndlon = pos.Split(",".ToArray());
                        if (i == 1)
                        {
                            maxLat = double.Parse(latAndlon[0]);
                            maxLon = double.Parse(latAndlon[1]);
                            minLat = double.Parse(latAndlon[0]);
                            minLon = double.Parse(latAndlon[1]);
                        }
                        else
                        {
                            if (maxLat < double.Parse(latAndlon[0]))
                            {
                                maxLat = double.Parse(latAndlon[0]);
                            }
                            if (maxLon < double.Parse(latAndlon[1]))
                            {
                                maxLon = double.Parse(latAndlon[1]);
                            }
                            if (minLon > double.Parse(latAndlon[1]))
                            {
                                minLon = double.Parse(latAndlon[1]);
                            }
                            if (minLat > double.Parse(latAndlon[0]))
                            {
                                minLat = double.Parse(latAndlon[0]);
                            }
                        }
                    }
                    barEditItemMaxLat.EditValue = maxLat;
                    barEditItemMaxLon.EditValue = maxLon;
                    barEditItemMinLon.EditValue = minLon;
                    barEditItemMinLat.EditValue = minLat;
                    //  this.mucsearcher.qrstAxGlobeControl1.DrawPolygenLayer(new Point3d(maxLon, maxLat, 0), new Point3d(minLon, minLat, 0));
                    //  this.mucsearcher.qrstAxGlobeControl1.DrawPolygenLayer(posArr);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("加载的文件格式不正确！");
                }

            }

        }

        /// <summary>
        /// 响应键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemRangeTrackBar1_KeyDown(object sender, KeyEventArgs e)
        {

            string code = e.KeyCode.ToString();
        }

        private void repositoryItemRangeTrackBar1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string code = e.KeyChar.ToString();
        }

        /// <summary>
        /// 省选择变化时加载市列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItemProvince_EditValueChanged(object sender, EventArgs e)
        {
            repositoryItemComboBoxCity.Items.Clear();
            barEditItemUrban.EditValue = "";
            if (barEditItemProvince.EditValue == null || barEditItemProvince.EditValue.ToString() == "")
            {
                return;
            }
            AreaLocationQuery areaQuery = new AreaLocationQuery(TheUniversal.BSDB.sqlUtilities);
            List<string> cityLst = areaQuery.GetCityLstByProvince(barEditItemProvince.EditValue.ToString());
            repositoryItemComboBoxCity.Items.AddRange(cityLst);

            //将地区转为经纬度，并在球体上绘制范围
            double[] latAndLon = areaQuery.GetSpacialInfo(barEditItemProvince.EditValue.ToString());
            if (latAndLon != null)
            {
                barEditItemMaxLat.EditValue = latAndLon[1];
                barEditItemMaxLon.EditValue = latAndLon[0];
                barEditItemMinLat.EditValue = latAndLon[3];
                barEditItemMinLon.EditValue = latAndLon[2];
            }

        }


        private void barEditItemUrban_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 市变化时加载县列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItemUrban_EditValueChanged(object sender, EventArgs e)
        {
            barEditItemCounty.EditValue = "";
            repositoryItemComboBoxCounty.Items.Clear();
            if (barEditItemUrban.EditValue == null || barEditItemUrban.EditValue.ToString() == "")
            {
                return;
            }
            AreaLocationQuery areaQuery = new AreaLocationQuery(TheUniversal.BSDB.sqlUtilities);
            List<string> countyLst = areaQuery.GetCountyLstByProvinceCity(barEditItemProvince.EditValue.ToString(), barEditItemUrban.EditValue.ToString());
            repositoryItemComboBoxCounty.Items.AddRange(countyLst);

            //将地区转为经纬度，并在球体上绘制范围
            double[] latAndLon = areaQuery.GetSpacialInfo(barEditItemProvince.EditValue.ToString(), barEditItemUrban.EditValue.ToString());
            if (latAndLon != null)
            {
                barEditItemMaxLat.EditValue = latAndLon[1];
                barEditItemMaxLon.EditValue = latAndLon[0];
                barEditItemMinLat.EditValue = latAndLon[3];
                barEditItemMinLon.EditValue = latAndLon[2];
            }
        }

        /// <summary>
        /// 县变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItemCounty_EditValueChanged(object sender, EventArgs e)
        {

            if (barEditItemCounty.EditValue != null && barEditItemCounty.EditValue.ToString() != "")
            {
                AreaLocationQuery areaQuery = new AreaLocationQuery(TheUniversal.BSDB.sqlUtilities);
                //将地区转为经纬度，并在球体上绘制范围
                double[] latAndLon = areaQuery.GetSpacialInfo(barEditItemProvince.EditValue.ToString(), barEditItemUrban.EditValue.ToString(), barEditItemCounty.EditValue.ToString());
                if (latAndLon != null)
                {
                    barEditItemMaxLat.EditValue = latAndLon[1];
                    barEditItemMaxLon.EditValue = latAndLon[0];
                    barEditItemMinLat.EditValue = latAndLon[3];
                    barEditItemMinLon.EditValue = latAndLon[2];
                }
            }
        }

        private void barButtonItem3D_ItemClick(object sender, ItemClickEventArgs e)
        {
            mucsearcher.Show3DViewer();
            this.barButtonItem3D.Enabled = false;
            this.barButtonItem2D.Enabled = true;
            this.barButtonItemRaster.Enabled = true;
        }
        private void barButtonItem2D_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] size = new int[2];
            size[0] = mucsearcher.qrstAxGlobeControl1.Width;
            size[1] = mucsearcher.qrstAxGlobeControl1.Height;
            mucsearcher.uc2DSearcher1.setSize(size);
            mucsearcher.Show2DViewer();
            this.barButtonItem2D.Enabled = false;
            this.barButtonItem3D.Enabled = true;
            this.barButtonItemRaster.Enabled = true;
            this.mucsearcher.uc2DSearcher1.showVector();
        }
        private void barButtonItemRaster_ItemClick(object sender, ItemClickEventArgs e)
        {
            mucsearcher.Show2DViewer();
            this.barButtonItem2D.Enabled = true;
            this.barButtonItem3D.Enabled = true;
            this.barButtonItemRaster.Enabled = false;
            this.mucsearcher.uc2DSearcher1.showRaster();
        }
    }

    public enum Rule
    {
        /// <summary>
        /// 相交
        /// </summary>
        Intersect = 0,
        /// <summary>
        /// 包含
        /// </summary>
        Contain = 1,
    }
}
