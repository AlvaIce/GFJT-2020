using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
using QRST.WorldGlobeTool.Geometries;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_DS_MetadataQuery;
using QRST_DI_DS_MetadataQuery.JSONutilty;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using QRST_DI_MS_Desktop.QueryInner;
using QRST_DI_MS_Desktop.UserInterfaces.SearchConditionsPanel;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_TS_Basis.DirectlyAddress;
using Yaan_AppSysWinForm;
using QueryRequest = QRST_DI_SS_Basis.MetadataQuery.QueryRequest;
using QueryResponse = QRST_DI_SS_Basis.MetadataQuery.QueryResponse;
using SimpleCondition = QRST_DI_SS_Basis.MetadataQuery.SimpleCondition;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    class ruc3DSearcher : RibbonPageBaseUC
    {
        #region definition

        private DevExpress.XtraBars.BarEditItem barEditItemBeginDate;
        private string coord;
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

        private RibbonPageGroup rpgModuleSearch;
        private RPGroupModule rpgModule;
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
        public  DevExpress.XtraBars.BarEditItem barEditItemShowPoint;
        public  DevExpress.XtraBars.BarEditItem barEditItemShowLinePoint;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEditShow;
        private DevExpress.XtraBars.BarStaticItem barStaticItemTishi;

        MSUserInterface msUCDetail;
        private DevExpress.XtraBars.BarButtonItem barButtonItemClearCustom;
        private ImageList imageListRucSearch;
        private DevExpress.XtraBars.BarButtonItem barButtonItemShowCustom;
        private DevExpress.XtraBars.BarButtonItem barButtonEarthViewInitial;
        private DevExpress.XtraBars.BarButtonItem barButtonTest;
        private DevExpress.XtraBars.BarButtonItem barButtonReset;
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

        static metadatacatalognode_Mdl selectedQueryObj = null;                  //存储需要查询的数据类型对象 zxw 2013/08/05
        public static IGetQuerySchema querySchema = null;                                                //全局查询方案

        QueryRequest queryRequest;                                                       //查询请求对象
        QueryResponse queryResponse;
        string SpacialInfo = "";
        DateTime startTime;                        //查询中的起始时间
        DateTime endTime;                          //查询中的结束时间
        private DateTime lastSearchTime; //最后查询时间
        private int queryNum;//查询次数
        static QueryPara queryPara;                                                        //查询参数对象
        mucDetailViewer mucdetail;                                                  //详细信息列表空间
        List<SimpleCondition> listSimpleCondistons = new List<SimpleCondition>(); //记录高级检索条件

         //localhostSqlite.Service sqliteclient;
        private static IQDB_Searcher_Tile _tileClient = Constant.ISearcherTileServ;
        private static IQDB_Searcher_Db _dbSearClient = Constant.ISearcherDbServ;
        private static IQDB_GetData _getDataClient = Constant.IGetDataService;
        //IIS发布查询服务
        private string jsonStr;
        GetClassify classify;
        gettype[] classifyfgw;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxPlantType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxCityName;
        private DevExpress.XtraBars.BarEditItem barEditItemTileLevel;
        //private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditTileLevel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxEditTileLevel;
        private DevExpress.XtraBars.BarEditItem barEditItemProTypeCombo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEditProType;
        private DevExpress.XtraBars.BarEditItem barEditItemNumPerPage;
        public DevExpress.XtraBars.BarEditItem BarEditItemSort;
          
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSort;
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
        private DevExpress.XtraBars.BarButtonItem barButtonItemLoadDrawPolygon;
        private DevExpress.XtraBars.BarButtonItem barButtonItemLoadSavePoint;
        private DevExpress.XtraBars.BarButtonItem barButtonItemLoadReadPoint;
        private DevExpress.XtraBars.BarStaticItem barStaticPositionFileName;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDrawPolyline;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSaveLinePoint;
        private DevExpress.XtraBars.BarButtonItem barButtonItemReadLinePoint;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox6;
        private BarButtonItem barButtonItem2D;
        private BarButtonItem barButtonItem3D;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private BarButtonItem barButtonItemRaster;
        private BarButtonItem barButtonItemVector;
        string[] strFields;
        private BarStaticItem barStaticItem4;
        //private GroupControl groupControl1;
        private DevExpress.XtraBars.BarEditItem SearchRule;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup rioGroup;
        private DevExpress.XtraEditors.Controls.RadioGroupItem radioItem1;
        private DevExpress.XtraEditors.Controls.RadioGroupItem radioItem2;
        private System.Data.DataSet ExtentDs;
        #endregion
        public static QRST_DI_SS_Basis.MetadataQuery.Rule rule = QRST_DI_SS_Basis.MetadataQuery.Rule.Intersect;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit5;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox3;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox4;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox5;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit7;
        public static double[] selectedRect = new double[] { -180, -90, 180, 90 };
        private BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox7;
        private BarEditItem barEditItem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox8;
        private BarStaticItem barStaticItem3;
        private BarEditItem barItm_ImageProdTypes;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit rep_barItm_ImageProdTypesEdit1;
        private BarStaticItem barStaticItem_docDatetime;
        private BarButtonItem barButtonItemDelete;
        private BarButtonGroup barButtonGroup1;
        private BarEditItem barEditItem3;
        private BarButtonItem barButtonItemLyr;
        private BarButtonItem barButtonCancelLyr;
        public string MinCloud;   //最小云量
        public string MaxCloud;
        public string MinAvailability;  //最小满幅率
        public string MaxAvailability;

        public static List<double> polyPoint = new List<double>();

        public DataTable Lyrtable = new DataTable();
        public static bool clickLyr = false;
         #endregion

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

        List<string> provinceLst = new List<string>();
        public ruc3DSearcher(object objMUC, object parentControl)
            : base(objMUC)
        {
            InitializeComponent();
        
            rpgModule=new RPGroupModule();
            rpgModuleSearch = rpgModule.rpgModuleSearch;
            mucsearcher = objMUC as muc3DSearcher;
            mucsearcher.TreeSelete += new muc3DSearcher.TreeSeleteDeletege(barEditItemSelDataType_EditValueChanged);
            mucsearcher.Show3DViewer();
            lastSearchTime = DateTime.Now;
            queryNum = 0;
            parentMSconsole = parentControl as frm_MSConsole;
            this.mucsearcher.qrstAxGlobeControl1.OnDrawRectangleCompletedEvent += new EventHandler(qrstAxGlobeControl1_OnDrawRectangleCompeleted);
            this.mucsearcher.qrstAxGlobeControl1.OnDrawPolygonCompleteEvent += new EventHandler(qrstAxGlobeControl1_OnDrawPolygonCompeleted);
            this.mucsearcher.qrstAxGlobeControl1.OnDrawPolyLineCompletedEvent += new EventHandler(qrstAxGlobeControl1_OnDrawPolylineCompeleted);
            this.mucsearcher.qrstAxGlobeControl1.OnPolyUp += new EventHandler(qrstAxGlobeControl1_OnPolyUp);
            this.mucsearcher.uc2DSearcher1.SizeChanged += new EventHandler(uc2DSearcher1_SizeChanged);
            string shpPath = Path.Combine(startPath, "map", "provincialBoundary.shp");
            ProVftset = Shapefile.Open(shpPath);
            string cityShpPath = Path.Combine(startPath, "map", "cityBoundary.shp");
            Cityftset = Shapefile.Open(cityShpPath);
            string countryShpPath = Path.Combine(startPath, "map", "countyBoundary.shp");
            Countyftset = Shapefile.Open(countryShpPath);
            foreach (Feature ft in ProVftset.Features)
            {
                string ftName = ft.DataRow["Name"].ToString();
                provinceLst.Add(ftName);
            }

            ///
            //Coordinate coord1=new DotSpatial.Topology.Coordinate(140,30);
            //Coordinate coord2=new DotSpatial.Topology.Coordinate(141,30);
            //Coordinate coord3=new DotSpatial.Topology.Coordinate(142,30);
            //Coordinate coord4=new DotSpatial.Topology.Coordinate(140,33);
            //Coordinate coord5=new DotSpatial.Topology.Coordinate(140,30);
            //Feature ft2 = new Feature(DotSpatial.Topology.FeatureType.Polygon, new Coordinate[] { coord1, coord2, coord3, coord4, coord5 });
            //IFeature ft3 = ft2 as IFeature;

            ///
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
            //int maxCount = 0;
        }

        void qrstAxGlobeControl1_OnPolyUp(object sender, EventArgs e)
        {
            bool isRightBtnUp = true;
            try
            {
                isRightBtnUp = (sender as string == "true") ? true : false; 
            }
            catch {}
            if (isRightBtnUp)
            {
                this.barEditItemShowPoint.EditValue = "";
                this.barEditItemShowLinePoint.EditValue = "";
            }
            else
            {
                Point3d pt3 = new Point3d();
                pt3 = this.mucsearcher.qrstAxGlobeControl1.GetPolyPoint();

                this.barEditItemShowPoint.EditValue += pt3.X.ToString("0.000000") + "," + pt3.Y.ToString("0.000000") + ";" + Environment.NewLine;
                this.barEditItemShowLinePoint.EditValue += pt3.X.ToString("0.000000") + "," + pt3.Y.ToString("0.000000") + ";" + Environment.NewLine;
            }
        }

        //  zxw 20131222 当显示详细信息时，是否需要重新绘制切片范围
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
        /// 绘制折线完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void qrstAxGlobeControl1_OnDrawPolylineCompeleted(object sender, EventArgs e) 
        {
            string str = this.barEditItemShowLinePoint.EditValue.ToString().Trim().Replace("\r\n", "");
            Shape shp = new Shape(FeatureType.Line);
            List<Coordinate> _polyline = new List<Coordinate>();
            double minLon, maxLon, minLat, maxLat;
            string[] points = str.Split(';');
            foreach(string point in points)
            {
                if (point.Length == 0)
                    continue;
                string[] xy = point.Split(',');
                Coordinate coord = new Coordinate();
                coord.X = Double.Parse(xy[0]);
                coord.Y = Double.Parse(xy[1]);
                _polyline.Add(coord);
            }
            shp.AddPart(_polyline, CoordinateType.Regular);
            testF = new Feature(shp);
            if (this.Cursor != Cursors.Default)
            {
                this.Cursor = Cursors.Default;
            }
            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = true;
            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition.QueryGeometry = testF;
            minLon = testF.Envelope.X;
            maxLon = minLon + testF.Envelope.Width;
            maxLat = testF.Envelope.Y;
            minLat = maxLat - testF.Envelope.Height;

            barEditItemMaxLat.EditValue = maxLat;
            barEditItemMaxLon.EditValue = maxLon;
            barEditItemMinLon.EditValue = minLon;
            barEditItemMinLat.EditValue = minLat;
        }
        /// <summary>
        /// 绘制多边形完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void qrstAxGlobeControl1_OnDrawPolygonCompeleted(object sender, EventArgs e)
        {
            if (barEditItemSpacialInfo.EditValue.ToString() == "行政区域")
            {
            }
            else
            {
                string str = this.barEditItemShowPoint.EditValue.ToString();
                this.barEditItemShowPoint.EditValue = str.Trim();
                Shape shp = new Shape(FeatureType.Polygon);
                List<Coordinate> _polygon = new List<Coordinate>();
                double minLon, maxLon, maxLat, minLat;
                string polygon = this.barEditItemShowPoint.EditValue.ToString().Replace("\r\n", "");
                //strPath.TrimEnd(',').Split(",".ToCharArray());
                string[] array = polygon.Split(';');
                for (int i = 0; i < array.Length - 1; i++)
                {
                    string[] latlon = array[i].Split(',');
                    Coordinate coord = new Coordinate();
                    coord.Y = Convert.ToDouble(latlon[1]);
                    coord.X = Convert.ToDouble(latlon[0]);
                    _polygon.Add(coord);
                }

                shp.AddPart(_polygon, CoordinateType.Regular);
                testF = new Feature(shp);
                //IFeature f = new Feature(shp);

                if (this.Cursor != Cursors.Default)
                {
                    this.Cursor = Cursors.Default;
                }
                QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = true;
                QRST_DI_SS_Basis.MetadataQuery.ComplexCondition.QueryGeometry = testF;
                minLon = testF.Envelope.X;
                maxLon = minLon + testF.Envelope.Width;
                maxLat = testF.Envelope.Y;
                minLat = maxLat - testF.Envelope.Height;

                barEditItemMaxLat.EditValue = maxLat;
                barEditItemMaxLon.EditValue = maxLon;
                barEditItemMinLon.EditValue = minLon;
                barEditItemMinLat.EditValue = minLat;

            }
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
            this.barEditItemShowPoint = new DevExpress.XtraBars.BarEditItem();
            this.barEditItemShowLinePoint = new DevExpress.XtraBars.BarEditItem();
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
            this.barButtonItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItemNumPerPage = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBoxSort = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.BarEditItemSort = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox6 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonEarthViewInitial = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonTest = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonReset = new DevExpress.XtraBars.BarButtonItem();
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
            this.repositoryItemComboBoxEditTileLevel = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
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
            this.barButtonItemLoadDrawPolygon = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemLoadSavePoint = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemLoadReadPoint = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDrawPolyline = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSaveLinePoint = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemReadLinePoint = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticPositionFileName = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem2D = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3D = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
            this.barButtonItemLyr = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonCancelLyr = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemComboBox8 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonItemRaster = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemVector = new DevExpress.XtraBars.BarButtonItem();
            this.SearchRule = new DevExpress.XtraBars.BarEditItem();
            this.rioGroup = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.repositoryItemRichTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.repositoryItemRichTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.repositoryItemRichTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.repositoryItemTextEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemComboBox5 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox3 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox4 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemTextEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemComboBox7 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barItm_ImageProdTypes = new DevExpress.XtraBars.BarEditItem();
            this.rep_barItm_ImageProdTypesEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barStaticItem_docDatetime = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem3 = new DevExpress.XtraBars.BarEditItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMaxCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditMinCol)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSort)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxEditTileLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditProType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSpatialChoose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxProvince)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCounty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rioGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rep_barItm_ImageProdTypesEdit1)).BeginInit();
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
            this.barButtonTest,
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
            this.barButtonItemLoadDrawPolygon,
            this.barEditItemShowPoint,
            this.barEditItemShowLinePoint,
            this.barButtonItemLoadSavePoint,
            this.barButtonItemLoadReadPoint,
            this.barButtonItemDrawPolyline,
            this.barButtonItemReadLinePoint,
            this.barButtonItemSaveLinePoint,
            this.barButtonItem2D,
            this.barButtonItem3D,
            this.barButtonItemRaster,
            this.barButtonItemVector,
            this.barStaticItem4,
            this.barEditItem1,
            this.barEditItem2,
            this.barButtonReset,
            this.barStaticItem3,
            this.barItm_ImageProdTypes,
            this.barStaticItem_docDatetime,
            this.barButtonItemDelete,
            this.barButtonItemLyr});
            this.ribbonControl1.MaxItemId = 156;
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
            this.repositoryItemComboBoxEditTileLevel,
            this.repositoryItemCheckedComboBoxEditProType,
            this.repositoryItemComboBox1,
            this.repositoryItemComboBoxSort,
            this.repositoryItemRangeTrackBar1,
            this.repositoryItemRangeTrackBar2,
            this.repositoryItemRangeTrackBar3,
            this.repositoryItemComboBoxSpatialChoose,
            this.repositoryItemComboBoxProvince,
            this.repositoryItemComboBoxCity,
            this.repositoryItemComboBoxCounty,
            this.repositoryItemComboBox6,
            this.repositoryItemRichTextEdit1,
            this.repositoryItemRichTextEdit2,
            this.repositoryItemRichTextEdit3,
            this.repositoryItemTextEdit5,
            this.repositoryItemTextEdit7,
            this.repositoryItemComboBox2,
            this.repositoryItemComboBox3,
            this.repositoryItemComboBox4,
            this.repositoryItemComboBox5,
            this.repositoryItemComboBox7,
            this.repositoryItemComboBox8,
            this.rep_barItm_ImageProdTypesEdit1});
            this.ribbonControl1.Size = new System.Drawing.Size(1400, 147);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupCustomQuery,
            this.ribbonPageGroupOperation,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barStaticItemTishi);
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
            // barEditItemMaxRow
            // 
            this.barEditItemMaxRow.Caption = "最大行号";
            this.barEditItemMaxRow.Edit = this.repositoryItemTextEditMaxRow;
            this.barEditItemMaxRow.EditValue = "";
            this.barEditItemMaxRow.Id = 368;
            this.barEditItemMaxRow.Name = "barEditItemMaxRow";
            this.barEditItemMaxRow.Width = 80;
            // 
            // repositoryItemTextEditMaxRow
            // 
            this.repositoryItemTextEditMaxRow.AutoHeight = false;
            this.repositoryItemTextEditMaxRow.Name = "repositoryItemTextEditMaxRow";
            // 
            // barEditItemMinRow
            // 
            this.barEditItemMinRow.Caption = "最小行号";
            this.barEditItemMinRow.Edit = this.repositoryItemTextEditMinRow;
            this.barEditItemMinRow.EditValue = "";
            this.barEditItemMinRow.Id = 7;
            this.barEditItemMinRow.Name = "barEditItemMinRow";
            this.barEditItemMinRow.Width = 80;
            // 
            // repositoryItemTextEditMinRow
            // 
            this.repositoryItemTextEditMinRow.AutoHeight = false;
            this.repositoryItemTextEditMinRow.Name = "repositoryItemTextEditMinRow";
            // 
            // barEditItemMaxCol
            // 
            this.barEditItemMaxCol.Caption = "最大列号";
            this.barEditItemMaxCol.Edit = this.repositoryItemTextEditMaxCol;
            this.barEditItemMaxCol.EditValue = "";
            this.barEditItemMaxCol.Id = 365;
            this.barEditItemMaxCol.Name = "barEditItemMaxCol";
            this.barEditItemMaxCol.Width = 80;
            // 
            // repositoryItemTextEditMaxCol
            // 
            this.repositoryItemTextEditMaxCol.AutoHeight = false;
            this.repositoryItemTextEditMaxCol.Name = "repositoryItemTextEditMaxCol";
            // 
            // barEditItemMinCol
            // 
            this.barEditItemMinCol.Caption = "最小列号";
            this.barEditItemMinCol.Edit = this.repositoryItemTextEditMinCol;
            this.barEditItemMinCol.EditValue = "";
            this.barEditItemMinCol.Id = 366;
            this.barEditItemMinCol.Name = "barEditItemMinCol";
            this.barEditItemMinCol.Width = 80;
            // 
            // repositoryItemTextEditMinCol
            // 
            this.repositoryItemTextEditMinCol.AutoHeight = false;
            this.repositoryItemTextEditMinCol.Name = "repositoryItemTextEditMinCol";
            // 
            // barButtonItemHandleSel
            // 
            this.barButtonItemHandleSel.Caption = "手动选取";
            this.barButtonItemHandleSel.Id = 14;
            this.barButtonItemHandleSel.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemHandleSel.LargeGlyph")));
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
            // barEditItemShowPoint
            // 
            this.barEditItemShowPoint.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.barEditItemShowPoint.Appearance.Options.UseBackColor = true;
            this.barEditItemShowPoint.Edit = this.repositoryItemMemoEditShow;
            this.barEditItemShowPoint.EditHeight = 45;
            this.barEditItemShowPoint.EditValue = "";
            this.barEditItemShowPoint.Id = 61;
            this.barEditItemShowPoint.Name = "barEditItemShowPoint";
            this.barEditItemShowPoint.Width = 160;
            // 
            // barEditItemShowLinePoint
            // 
            this.barEditItemShowLinePoint.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.barEditItemShowLinePoint.Appearance.Options.UseBackColor = true;
            this.barEditItemShowLinePoint.Edit = this.repositoryItemMemoEditShow;
            this.barEditItemShowLinePoint.EditHeight = 45;
            this.barEditItemShowLinePoint.EditValue = "";
            this.barEditItemShowLinePoint.Id = 152;
            this.barEditItemShowLinePoint.Name = "barEditItemShowLinePoint";
            this.barEditItemShowLinePoint.Width = 160;
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
            this.ribbonPageGroupOperation.ItemLinks.Add(this.barButtonItemDelete, true);
            this.ribbonPageGroupOperation.ItemLinks.Add(this.barStaticItem4, true);
            this.ribbonPageGroupOperation.ItemLinks.Add(this.barEditItemNumPerPage);
            this.ribbonPageGroupOperation.Name = "ribbonPageGroupOperation";
            this.ribbonPageGroupOperation.Text = "应用";
            // 
            // barButtonItemQuery
            // 
            this.barButtonItemQuery.Caption = "检索";
            this.barButtonItemQuery.Id = 12;
            this.barButtonItemQuery.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemQuery.LargeGlyph")));
            this.barButtonItemQuery.Name = "barButtonItemQuery";
            this.barButtonItemQuery.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemQuery_ItemClick);
            // 
            // barButtonItemShowCustom
            // 
            this.barButtonItemShowCustom.Caption = "显示高级检索";
            this.barButtonItemShowCustom.Enabled = false;
            this.barButtonItemShowCustom.Id = 66;
            this.barButtonItemShowCustom.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemShowCustom.LargeGlyph")));
            this.barButtonItemShowCustom.Name = "barButtonItemShowCustom";
            this.barButtonItemShowCustom.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemShowCustom.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItemShowCustom.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemShowCustom_ItemClick);
            // 
            // barButtonItemDelete
            // 
            this.barButtonItemDelete.Caption = "取消检索";
            this.barButtonItemDelete.Id = 143;
            this.barButtonItemDelete.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemDelete.LargeGlyph")));
            this.barButtonItemDelete.Name = "barButtonItemDelete";
            this.barButtonItemDelete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDelete_ItemClick);
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Caption = "每页显示";
            this.barStaticItem4.Id = 122;
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near;
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
            "100",
            "1000",
            "5000",
            "10000",
            "25000",
            "50000",
            "100000",
            "250000",
            "500000",
            "1000000",
            "全部记录"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBoxSort
            // 
            this.repositoryItemComboBoxSort.AutoHeight = false;
            this.repositoryItemComboBoxSort.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxSort.Items.AddRange(new object[] {
            "默认",
            "时间-云量-满幅率",
            "时间-满幅率-云量",
            "云量-时间-满幅率",
            "云量-满幅率-时间",
            "满幅率-云量-时间",
            "满幅率-时间-云量"});
            this.repositoryItemComboBoxSort.Name = "repositoryItemComboBoxSort";
            // 
            // BarEditItemSort
            // 
            this.BarEditItemSort.Edit = null;
            this.BarEditItemSort.Id = -1;
            this.BarEditItemSort.Name = "BarEditItemSort";
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
            this.barButtonEarthViewInitial.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonEarthViewInitial.LargeGlyph")));
            this.barButtonEarthViewInitial.Name = "barButtonEarthViewInitial";
            this.barButtonEarthViewInitial.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonEarthViewInitial.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonEarthViewInitial_ItemClick);
            // 
            // barButtonTest
            // 
            this.barButtonTest.Caption = "一次全覆盖";
            this.barButtonTest.Id = 131;
            this.barButtonTest.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonTest.LargeGlyph")));
            this.barButtonTest.Name = "barButtonTest";
            this.barButtonTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonTest_ItemClick);
            // 
            // barButtonReset
            // 
            this.barButtonReset.Caption = "数据集复原";
            this.barButtonReset.Id = 132;
            this.barButtonReset.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonReset.LargeGlyph")));
            this.barButtonReset.Name = "barButtonReset";
            this.barButtonReset.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonReset_ItemClick);
            // 
            // repositoryItemPopupContainerEditDatatype
            // 
            this.repositoryItemPopupContainerEditDatatype.AutoHeight = false;
            this.repositoryItemPopupContainerEditDatatype.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemPopupContainerEditDatatype.Name = "repositoryItemPopupContainerEditDatatype";
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
            "like",
            "in"});
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
            this.barButtonItemAddNewQuery.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemAddNewQuery.LargeGlyph")));
            this.barButtonItemAddNewQuery.Name = "barButtonItemAddNewQuery";
            this.barButtonItemAddNewQuery.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAddNewQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAddNewQuery_ItemClick);
            // 
            // barButtonItemClearCustom
            // 
            this.barButtonItemClearCustom.Caption = "清空";
            this.barButtonItemClearCustom.Id = 64;
            this.barButtonItemClearCustom.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemClearCustom.LargeGlyph")));
            this.barButtonItemClearCustom.Name = "barButtonItemClearCustom";
            this.barButtonItemClearCustom.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemClearCustom.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemClearCustom_ItemClick);
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
            this.barEditKeyWords.Caption = "全局关键字";
            this.barEditKeyWords.Edit = this.repositoryItemTextEdit2;
            this.barEditKeyWords.Id = 78;
            this.barEditKeyWords.Name = "barEditKeyWords";
            this.barEditKeyWords.Width = 180;
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
            this.barEditItemTileLevel.Edit = this.repositoryItemComboBoxEditTileLevel;
            this.barEditItemTileLevel.EditValue = "10米";
            this.barEditItemTileLevel.Id = 97;
            this.barEditItemTileLevel.Name = "barEditItemTileLevel";
            this.barEditItemTileLevel.Width = 60;
            // 
            // repositoryItemComboBoxEditTileLevel
            // 
            this.repositoryItemComboBoxEditTileLevel.AutoHeight = false;
            this.repositoryItemComboBoxEditTileLevel.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxEditTileLevel.Name = "repositoryItemComboBoxEditTileLevel";
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
            this.barEditItemTimerTrackBar.Edit = null;
            this.barEditItemTimerTrackBar.Id = 123;
            this.barEditItemTimerTrackBar.Name = "barEditItemTimerTrackBar";
            // 
            // repositoryItemRangeTrackBar1
            // 
            this.repositoryItemRangeTrackBar1.Name = "repositoryItemRangeTrackBar1";
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
            this.repositoryItemRangeTrackBar2.EditValueChanged += new System.EventHandler(this.repositoryItemRangeTrackBar2_EditValueChanged);
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
            this.repositoryItemRangeTrackBar3.EditValueChanged += new System.EventHandler(this.repositoryItemRangeTrackBar3_EditValueChanged);
            // 
            // barEditItemSpacialInfo
            // 
            this.barEditItemSpacialInfo.Caption = "空间信息 ";
            this.barEditItemSpacialInfo.Edit = this.repositoryItemComboBoxSpatialChoose;
            this.barEditItemSpacialInfo.EditValue = "经纬坐标";
            this.barEditItemSpacialInfo.Id = 108;
            this.barEditItemSpacialInfo.Name = "barEditItemSpacialInfo";
            this.barEditItemSpacialInfo.Width = 90;
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
            "自定义多边形",
            "自定义折线"});
            this.repositoryItemComboBoxSpatialChoose.Name = "repositoryItemComboBoxSpatialChoose";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Id = 109;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItemProvince
            // 
            this.barEditItemProvince.Caption = "省/直辖市";
            this.barEditItemProvince.Edit = this.repositoryItemComboBoxProvince;
            this.barEditItemProvince.Id = 110;
            this.barEditItemProvince.Name = "barEditItemProvince";
            this.barEditItemProvince.Width = 80;
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
            this.barEditItemUrban.Caption = "地级市";
            this.barEditItemUrban.Edit = this.repositoryItemComboBoxCity;
            this.barEditItemUrban.Id = 111;
            this.barEditItemUrban.Name = "barEditItemUrban";
            this.barEditItemUrban.Width = 90;
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
            this.barEditItemCounty.Caption = "县市区";
            this.barEditItemCounty.Edit = this.repositoryItemComboBoxCounty;
            this.barEditItemCounty.Id = 112;
            this.barEditItemCounty.Name = "barEditItemCounty";
            this.barEditItemCounty.Width = 90;
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
            // barButtonItemLoadDrawPolygon
            // 
            this.barButtonItemLoadDrawPolygon.Caption = "绘制";
            this.barButtonItemLoadDrawPolygon.Id = 115;
            this.barButtonItemLoadDrawPolygon.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLoadDrawPolygon.LargeGlyph")));
            this.barButtonItemLoadDrawPolygon.Name = "barButtonItemLoadDrawPolygon";
            this.barButtonItemLoadDrawPolygon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemLoadDrawPolygon.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemLoadDrawPolygon_ItemClick);
            // 
            // barButtonItemLoadSavePoint
            // 
            this.barButtonItemLoadSavePoint.Caption = "保存";
            this.barButtonItemLoadSavePoint.Id = 116;
            this.barButtonItemLoadSavePoint.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLoadSavePoint.LargeGlyph")));
            this.barButtonItemLoadSavePoint.Name = "barButtonItemLoadSavePoint";
            this.barButtonItemLoadSavePoint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemLoadSavePoint_ItemClick);
            // 
            // barButtonItemLoadReadPoint
            // 
            this.barButtonItemLoadReadPoint.Caption = "读取";
            this.barButtonItemLoadReadPoint.Id = 117;
            this.barButtonItemLoadReadPoint.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLoadReadPoint.LargeGlyph")));
            this.barButtonItemLoadReadPoint.Name = "barButtonItemLoadReadPoint";
            this.barButtonItemLoadReadPoint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemLoadReadPoint_ItemClick);
            // 
            // barButtonItemDrawPolyline
            // 
            this.barButtonItemDrawPolyline.Caption = "绘制";
            this.barButtonItemDrawPolyline.Id = 153;
            this.barButtonItemDrawPolyline.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemDrawPolyline.LargeGlyph")));
            this.barButtonItemDrawPolyline.Name = "barButtonItemDrawPolyline";
            this.barButtonItemDrawPolyline.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDrawPolyline.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDrawPolyline_ItemClick);
            // 
            // barButtonItemSaveLinePoint
            // 
            this.barButtonItemSaveLinePoint.Caption = "保存";
            this.barButtonItemSaveLinePoint.Id = 155;
            this.barButtonItemSaveLinePoint.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemSaveLinePoint.LargeGlyph")));
            this.barButtonItemSaveLinePoint.Name = "barButtonItemSaveLinePoint";
            this.barButtonItemSaveLinePoint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSaveLinePoint_ItemClick);
            // 
            // barButtonItemReadLinePoint
            // 
            this.barButtonItemReadLinePoint.Caption = "读取";
            this.barButtonItemReadLinePoint.Id = 154;
            this.barButtonItemReadLinePoint.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemReadLinePoint.LargeGlyph")));
            this.barButtonItemReadLinePoint.Name = "barButtonItemReadLinePoint";
            this.barButtonItemReadLinePoint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemReadLinePoint_ItemClick);
            // 
            // barStaticPositionFileName
            // 
            this.barStaticPositionFileName.Id = -1;
            this.barStaticPositionFileName.Name = "barStaticPositionFileName";
            this.barStaticPositionFileName.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem2D
            // 
            this.barButtonItem2D.Caption = "二维地图";
            this.barButtonItem2D.Id = 118;
            this.barButtonItem2D.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2D.LargeGlyph")));
            this.barButtonItem2D.Name = "barButtonItem2D";
            this.barButtonItem2D.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem2D.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2D_ItemClick);
            // 
            // barButtonItem3D
            // 
            this.barButtonItem3D.Caption = "三维影像图";
            this.barButtonItem3D.Enabled = false;
            this.barButtonItem3D.Id = 119;
            this.barButtonItem3D.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem3D.LargeGlyph")));
            this.barButtonItem3D.Name = "barButtonItem3D";
            this.barButtonItem3D.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem3D.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3D_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonEarthViewInitial, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "视图";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.BarEditItemSort, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItem1, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barStaticItem3, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barEditItem2);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonTest, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemLyr, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonCancelLyr, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonReset, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "全覆盖过滤";
            this.ribbonPageGroup3.Visible = false;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Edit = null;
            this.barEditItem1.Id = 133;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "检索优先级：";
            this.barStaticItem3.Id = 135;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barEditItem2
            // 
            this.barEditItem2.Edit = this.repositoryItemComboBoxSort;
            this.barEditItem2.EditValue = "默认";
            this.barEditItem2.Id = 134;
            this.barEditItem2.Name = "barEditItem2";
            this.barEditItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barEditItem2.Width = 80;
            this.barEditItem2.EditValueChanged += new System.EventHandler(this.barEditItem2_EditValueChanged);
            this.barEditItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barEditItem2_ItemClick);
            // 
            // barButtonItemLyr
            // 
            this.barButtonItemLyr.Caption = "贴图";
            this.barButtonItemLyr.Id = 152;
            this.barButtonItemLyr.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLyr.LargeGlyph")));
            this.barButtonItemLyr.Name = "barButtonItemLyr";
            this.barButtonItemLyr.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemLyr_ItemClick);
            // 
            // barButtonCancelLyr
            // 
            this.barButtonCancelLyr.Caption = "取消贴图";
            this.barButtonCancelLyr.Id = -1;
            this.barButtonCancelLyr.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonCancelLyr.LargeGlyph")));
            this.barButtonCancelLyr.Name = "barButtonCancelLyr";
            this.barButtonCancelLyr.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonCancelLyr_ItemClick);
            // 
            // repositoryItemComboBox8
            // 
            this.repositoryItemComboBox8.Name = "repositoryItemComboBox8";
            // 
            // barButtonItemRaster
            // 
            this.barButtonItemRaster.Caption = "二维影像图";
            this.barButtonItemRaster.Id = 120;
            this.barButtonItemRaster.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemRaster.LargeGlyph")));
            this.barButtonItemRaster.Name = "barButtonItemRaster";
            this.barButtonItemRaster.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemRaster.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemRaster_ItemClick);
            // 
            // barButtonItemVector
            // 
            this.barButtonItemVector.Caption = "三维地图";
            this.barButtonItemVector.Enabled = false;
            this.barButtonItemVector.Id = 121;
            this.barButtonItemVector.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemVector.LargeGlyph")));
            this.barButtonItemVector.Name = "barButtonItemVector";
            this.barButtonItemVector.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // SearchRule
            // 
            this.SearchRule.Caption = "检索规则";
            this.SearchRule.Edit = this.rioGroup;
            this.SearchRule.Id = 5;
            this.SearchRule.Name = "SearchRule";
            this.SearchRule.Width = 150;
            // 
            // rioGroup
            // 
            this.rioGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "相交"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("包含", "包含")});
            this.rioGroup.Name = "rioGroup";
            this.rioGroup.SelectedIndexChanged += new System.EventHandler(this.rioGroup_SelectedIndexChanged);
            // 
            // repositoryItemRichTextEdit1
            // 
            this.repositoryItemRichTextEdit1.Name = "repositoryItemRichTextEdit1";
            this.repositoryItemRichTextEdit1.ShowCaretInReadOnly = false;
            // 
            // repositoryItemRichTextEdit2
            // 
            this.repositoryItemRichTextEdit2.Name = "repositoryItemRichTextEdit2";
            this.repositoryItemRichTextEdit2.ShowCaretInReadOnly = false;
            // 
            // repositoryItemRichTextEdit3
            // 
            this.repositoryItemRichTextEdit3.Name = "repositoryItemRichTextEdit3";
            this.repositoryItemRichTextEdit3.ShowCaretInReadOnly = false;
            // 
            // repositoryItemTextEdit5
            // 
            this.repositoryItemTextEdit5.AutoHeight = false;
            this.repositoryItemTextEdit5.Name = "repositoryItemTextEdit5";
            // 
            // repositoryItemComboBox5
            // 
            this.repositoryItemComboBox5.AutoHeight = false;
            this.repositoryItemComboBox5.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox5.Name = "repositoryItemComboBox5";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // repositoryItemComboBox3
            // 
            this.repositoryItemComboBox3.AutoHeight = false;
            this.repositoryItemComboBox3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox3.Name = "repositoryItemComboBox3";
            // 
            // repositoryItemComboBox4
            // 
            this.repositoryItemComboBox4.AutoHeight = false;
            this.repositoryItemComboBox4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox4.Name = "repositoryItemComboBox4";
            // 
            // repositoryItemTextEdit7
            // 
            this.repositoryItemTextEdit7.AutoHeight = false;
            this.repositoryItemTextEdit7.Name = "repositoryItemTextEdit7";
            // 
            // repositoryItemComboBox7
            // 
            this.repositoryItemComboBox7.AutoHeight = false;
            this.repositoryItemComboBox7.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox7.Name = "repositoryItemComboBox7";
            // 
            // barItm_ImageProdTypes
            // 
            this.barItm_ImageProdTypes.Caption = "影像产品类型";
            this.barItm_ImageProdTypes.Edit = this.rep_barItm_ImageProdTypesEdit1;
            this.barItm_ImageProdTypes.Id = 133;
            this.barItm_ImageProdTypes.Name = "barItm_ImageProdTypes";
            // 
            // rep_barItm_ImageProdTypesEdit1
            // 
            this.rep_barItm_ImageProdTypesEdit1.AutoHeight = false;
            this.rep_barItm_ImageProdTypesEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rep_barItm_ImageProdTypesEdit1.Name = "rep_barItm_ImageProdTypesEdit1";
            // 
            // barStaticItem_docDatetime
            // 
            this.barStaticItem_docDatetime.Caption = "文档时间区间";
            this.barStaticItem_docDatetime.Id = 134;
            this.barStaticItem_docDatetime.Name = "barStaticItem_docDatetime";
            this.barStaticItem_docDatetime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem3
            // 
            this.barEditItem3.Edit = this.repositoryItemComboBoxSort;
            this.barEditItem3.EditValue = "请选择";
            this.barEditItem3.Id = 134;
            this.barEditItem3.Name = "barEditItem3";
            this.barEditItem3.Width = 60;
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSort)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxEditTileLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEditProType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRangeTrackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSpatialChoose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxProvince)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxCounty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rioGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rep_barItm_ImageProdTypesEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        //public RangeTrackBarControl Temp;
        /// <summary>
        /// 获取满幅率的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemRangeTrackBar3_EditValueChanged(object sender, EventArgs e)
        {
            RangeTrackBarControl rangeTrackBarControl = (RangeTrackBarControl)sender;
            int minAvailabilityValue = rangeTrackBarControl.Value.Minimum * 10;
            int maxAvailabilityValue = rangeTrackBarControl.Value.Maximum * 10;
            MinAvailability = minAvailabilityValue.ToString();
            MaxAvailability = maxAvailabilityValue.ToString();
        }
       /// <summary>
       /// 获取云量的值
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void repositoryItemRangeTrackBar2_EditValueChanged(object sender, EventArgs e)
        {
            RangeTrackBarControl rangeTrackBarControl = (RangeTrackBarControl)sender;
            //Temp = rangeTrackBarControl;
            int minCloudValue = rangeTrackBarControl.Value.Minimum * 10;
            int maxCloudValue = rangeTrackBarControl.Value.Maximum * 10;
            MinCloud = minCloudValue.ToString();
            MaxCloud = maxCloudValue.ToString();
        }
      
        void rioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.radioItem1.Value == null)
            //    this.radioItem1.Value = "相交";
            int index = ((RadioGroup)sender).SelectedIndex;
            switch (index)
            {
                case 0:
                    rule = QRST_DI_SS_Basis.MetadataQuery.Rule.Intersect;

                    break;
                case 1:
                    rule = QRST_DI_SS_Basis.MetadataQuery.Rule.Contain;
                    break;
                default:
                    rule = QRST_DI_SS_Basis.MetadataQuery.Rule.Intersect;
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
            UpdateInputPanel();
        }

        private void UpdateInputPanel()
        {
            barEditItemShowPoint.EditValue = "";
            barEditItemShowLinePoint.EditValue = "";
            barEditItemSpacialInfo.EditValue = "经纬坐标";
            
               //this.mucsearcher.qrstAxGlobeControl1.ClearPoly(false);
         
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
                    this.rpgModuleSearch.Visible = false;
                    this.barEditItemKeyWord.EditValue = string.Empty;
                    ShowDetailQuery(false);
                    //重构查询方案参数
                    string[] elementSet = new string[] { "*" };
                    IDbBaseUtilities sqlUtilities = TheUniversal.GetsubDbByEngName(selectedQueryObj.GROUP_CODE.Substring(0, 4)).sqlUtilities;

                    this.barEditItemShowQuery.EditValue = string.Empty;   //清空自定义条件

                    //按照数据类型准备查询环境界面
                    if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))    //准备栅格数据查询环境
                    {
                        querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);

                        DataTable dtstruct = querySchema.GetTableStruct().Tables[0];

                        LoadQueryField(dtstruct);
                        //完成栅格查询的一些基本数据加载
                        //显示栅格数据查询界面
                        if (querySchema.GetTableName()=="imageprod_view")
                        {
                            setImageProdTypeSelector();
                        }
                        setSatalliteAndSensor();
                        setBaseSpaceControl();
                        setBaseDateControl();
                        //   setKeyWordControl();

                        //setTrackBar();
                        //setExtentTool();
                        //设置数据集复原按钮显示
                        setTilebarButtonResetShow();
                        //设置不显示优先级按钮
                        setNotQueryPriority();
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_normalfile"))    //准备一般数据查询环境
                    {
                        querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);
                        LoadQueryField(querySchema.GetTableStruct().Tables[0]);
                        setDocQueryControl();
                        setBaseDateControl();
                        //设置不显示优先级按钮
                        setNotQueryPriority();
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))  //准备矢量数据查询环境
                    {
                        querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);
                        LoadQueryField(querySchema.GetTableStruct().Tables[0]);
                        setBaseDateControl();
                        setBaseSpaceControl();
                        setKeyWordControl();
                        //setExtentTool();
                        //设置不显示优先级按钮
                        setNotQueryPriority();
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_table")) //准备表格数据查询环境
                    {
                        //表格数据情况特殊，因为波普特征数据库调用外部WebService服务进行查询，为统一集中到查询模块，需要另划界面
                        if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")          //波普特征数据库，特殊情况，若归为统一，则可去掉
                        {
                            //设置不显示优先级按钮
                            setNotQueryPriority();
                            SetCustomQueryDisable();
                            ShowRcdbQuery(selectedQueryObj.NAME);
                            return;
                        }
                        else if (selectedQueryObj.NAME.Equals("模块控件") || selectedQueryObj.NAME.Equals("系统构件") || selectedQueryObj.NAME.Equals("系统构建信息"))
                        {
                            //this.ribbonPage1.Groups.Remove(this.rpgModuleSearch);
                            if (!this.ribbonPage1.Groups.Contains(this.rpgModuleSearch))
                            {
                                this.ribbonPage1.Groups.Insert(0, this.rpgModuleSearch);
                            }
                            this.rpgModule.UpdateModularDataSet();
                            this.rpgModuleSearch.Visible = true;
                            this.ribbonPageGroup1.Visible = false;
                            if (mucdetail == null)
                            {
                                mucdetail = ((mucDetailViewer)MSUserInterface.listMSUI[1].uiMainUC);
                            }
                            this.rpgModule.SetSearchViewByType(selectedQueryObj.NAME, this.mucdetail);
                        }
                        else
                        {
                            querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);
                            LoadQueryField(querySchema.GetTableStruct().Tables[0]);
                            setTableQueryControl();
                            //设置不显示优先级按钮
                            setNotQueryPriority();
                        }
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_document")) //准备文档数据查询环境
                    {
                        querySchema = new FieldViewBasedQuerySchema(elementSet, selectedQueryObj.DATA_CODE, sqlUtilities);
                        LoadQueryField(querySchema.GetTableStruct().Tables[0]);
                        setDocQueryControl();
                        //设置不显示优先级按钮
                        setNotQueryPriority();
                        setBaseDateControl();
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")) //准备切片数据查询环境
                    {
                        //if (sqliteclient == null)
                        //{
                        //    sqliteclient = new localhostSqlite.Service();
                        //}
                        if (selectedQueryObj.NAME.Equals("规格化影像数据") || selectedQueryObj.NAME.Equals("规格化影像控制数据"))
                        {
                            string[] alldistinct = _tileClient.GetCTileDistinctAttrs().ToArray();
                            string[] satelliteList=new string[0];
                            string[] sensorList = new string[0];
                            string[] tileTypeList = new string[0];
                            string[] levelLst = new string[0];

                            List<string> tmpstrs = new List<string>();
                            foreach (string distattr in alldistinct)
                            {
                                switch (distattr)
                                {
                                    case "【distinctSatellite】":
                                        tmpstrs.Clear();
                                        break;
                                    case "【distinctSensor】":
                                        if (tmpstrs.Count > 0) {
                                            satelliteList = new string[tmpstrs.Count];
                                            tmpstrs.CopyTo(satelliteList);
                                        }
                                        tmpstrs.Clear();
                                        break;
                                    case "【distinctCTtype】":
                                        if (tmpstrs.Count > 0)
                                        {
                                            sensorList = new string[tmpstrs.Count];
                                            tmpstrs.CopyTo(sensorList);
                                        }
                                        tmpstrs.Clear();
                                        break;
                                    case "【distinctCTLevel】":
                                        if (tmpstrs.Count > 0)
                                        {
                                            tileTypeList = new string[tmpstrs.Count];
                                            tmpstrs.CopyTo(tileTypeList);
                                        }
                                        tmpstrs.Clear();
                                        break;
                                    default:
                                        tmpstrs.Add(distattr);
                                        break;
                                }
                            }
                            if (tmpstrs.Count > 0)
                            {
                                levelLst = new string[tmpstrs.Count];
                                tmpstrs.CopyTo(levelLst);
                            }
                            tmpstrs.Clear();
                            //加载卫星传感器
                            satelliteList = _tileClient.SearTileSatellites().ToArray();
                            Array.Sort(satelliteList);
                            this.repositoryItemCheckedComboBoxEditSates.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditSates.Items.AddRange(satelliteList);
                            sensorList = _tileClient.SearTileSensors().ToArray();
                            Array.Sort(sensorList);
                            this.repositoryItemCheckedComboBoxEditSensors.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditSensors.Items.AddRange(sensorList);
                            setSatalliteAndSensor();
                            //加载类型
                           tileTypeList = _tileClient.GetDataDistinct("select distinct type from correctedTiles").ToArray();
                            this.repositoryItemCheckedComboBoxEditDataType.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditDataType.Items.AddRange(tileTypeList);
                            setTileDataTypeControl();

                            //数据切片层级初始化
                            levelLst = _tileClient.SearTileLevels().ToArray();
                            Array.Sort(levelLst);
                            //string[] levelLst = new string[] { "6", "7" };
                            this.repositoryItemComboBoxEditTileLevel.Items.Clear();
                            foreach (string lv in levelLst)
                            {
                                repositoryItemComboBoxEditTileLevel.Items.Add(DirectlyAddressing.GetStrResolutionByLevelChar(lv));
                            }
                            setTileLevelControl();
                            repositoryItemComboBoxFields.Items.Clear();
                            System.Data.DataSet tileData = _tileClient.SearTileAllAttr();
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
                                    tileData = _tileClient.SearTileAllAttr();
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
                            string[] alldistinct = _tileClient.GetPTileDistinctAttrs().ToArray();
                            string[] levelLst = new string[0];
                            string[] prodTypeList = new string[0];

                            List<string> tmpstrs = new List<string>();
                            foreach (string distattr in alldistinct)
                            {
                                switch (distattr)
                                {
                                    case "【distinctPTLevel】":
                                        tmpstrs.Clear();
                                        break;
                                    case "【distinctPTProdType】":
                                        if (tmpstrs.Count > 0)
                                        {
                                            levelLst = new string[tmpstrs.Count];
                                            tmpstrs.CopyTo(levelLst);
                                        }
                                        tmpstrs.Clear();
                                        break;
                                    default:
                                        tmpstrs.Add(distattr);
                                        break;
                                }
                            }
                            if (tmpstrs.Count > 0)
                            {
                                prodTypeList = new string[tmpstrs.Count];
                                tmpstrs.CopyTo(prodTypeList);
                            }
                            tmpstrs.Clear();
                            repositoryItemComboBoxEditTileLevel.Items.Clear();
                            levelLst = _tileClient.SearProTileLevels().ToArray();
                            Array.Sort(levelLst);
                            foreach (string lv in levelLst)
                            {
                                repositoryItemComboBoxEditTileLevel.Items.Add(DirectlyAddressing.GetStrResolutionByLevelChar(lv));
                            }
                            //产品切片层级初始
                            setTileLevelControl();
                            //加载产品类型
                            prodTypeList = _tileClient.GetDataDistinct("select distinct ProdType from productTiles").ToArray();
                            this.repositoryItemCheckedComboBoxEditProType.Items.Clear();
                            this.repositoryItemCheckedComboBoxEditProType.Items.AddRange(prodTypeList);
                            setProTileTypeControl();
                            repositoryItemComboBoxFields.Items.Clear();
                            System.Data.DataSet tileData = _tileClient.SearProTileAllAttr();
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
                                    tileData = _tileClient.SearProTileAllAttr();
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
                        //设置数据集复原按钮不显示
                        setTilebarButtonResetNotShow();
                        //设置检索的优先级
                        setQueryPriority();
                        
                        //增加产品类型
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
            bool hasSpaceConditions = false;
            bool hasDocConditions = false;
            for (int i = 0; i < this.ribbonPageGroup1.ItemLinks.Count; i++)
            {
                if (this.ribbonPageGroup1.ItemLinks[i].Caption == "空间信息 ")
                {
                    hasSpaceConditions = true;
                }
                if (this.ribbonPageGroup1.ItemLinks[i].Caption == "全局关键字")
                {
                    hasDocConditions = true;
                }
            }

            this.barEditItemBeginDate.EditValue = DateTime.Now.AddYears(-20).ToString();
            this.barEditItemEndDate.EditValue = DateTime.Now.ToString();
            if (hasDocConditions && !hasSpaceConditions)
            {
                this.ribbonPageGroup1.ItemLinks.Add(this.barStaticItem_docDatetime, true);
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemBeginDate, false);
            }
            else
            {
                this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemBeginDate, true);
            }
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemEndDate);
            if (hasSpaceConditions)
            {
                this.ribbonPageGroup1.ItemLinks.Add(this.SearchRule);
            }

        }
        /// <summary>
        /// 设置瓦片检索时的检索优先级
        /// </summary>
        void setQueryPriority() 
        {
            this.barStaticItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            this.barEditItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }
        /// <summary>
        /// 设置不显示检索的优先级
        /// </summary>
        void setNotQueryPriority()
        {
            this.barStaticItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barEditItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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
            //if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            //{
            //  this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemLatandLon, true);
            //空间信息选项卡
            //DLF130920   解决重新选择数据类型进行查询时空间信息选项卡可能不为经纬度（如用户刚选择了行政区域进行查询），但界面为经纬度范围的不合理情况
            //下面一句话 执行有异常，不知何原因
            this.repositoryItemComboBoxSpatialChoose.Items.Clear();
            this.repositoryItemComboBoxSpatialChoose.Items.AddRange(new string[] { "经纬坐标", "行政区域", "坐标文件", "自定义多边形", "自定义折线" });//"行列号范围",
            if (this.repositoryItemComboBoxSpatialChoose.Items.Count > 0)
            {
                try
                {
                    //this.barEditItemSpacialInfo.EditValue = null;
                    this.repositoryItemComboBoxCounty.Items.Clear();
                    this.repositoryItemComboBoxCity.Items.Clear();
                    this.repositoryItemComboBoxProvince.Items.Clear();

                    //this.barEditItemUrban.EditValue = null;
                    //this.barEditItemProvince.EditValue = null;

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

            //}
            //else
            //{
            //    this.barEditItemMaxLon.EditValue = 180;
            //    this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMaxLon, true, "", "", true);
            //    this.barEditItemMinLon.EditValue = -180;
            //    this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMinLon, false, "", "", true);
            //    this.barEditItemMaxLat.EditValue = 90;
            //    this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMaxLat, false, "", "", true);
            //    this.barEditItemMinLat.EditValue = -90;
            //    this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemMinLat, false, "", "", true);
            //    this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemHandleSel, true, "", "", true);

            //} 
        }
     
        /// <summary>
        ///空间信息选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        IFeatureSet ProVftset, Cityftset, Countyftset;
        string startPath = Application.StartupPath;
        private void barEditItemSpacialInfo_EditValueChanged(object sender, EventArgs e)
        {
            testF = null;
            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = false;

            //有bug 位置会错乱，当多次切换空间信息类型时 joki 161128
            try
            {
                //默认是瓦片检索时的布局状态，瓦片比卫星影像数据多两个类型字段，故影像检索时要-2。
                int idxSpacialInfoItem = (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")) ? 4 : 2;

                this.barEditItemMaxLon.EditValue = 180;
                this.barEditItemMinLon.EditValue = -180;
                this.barEditItemMaxLat.EditValue = 90;
                this.barEditItemMinLat.EditValue = -90;

                this.barEditItemShowLinePoint.EditValue = "";
                this.barEditItemShowPoint.EditValue = "";
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
                QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = false;
                QRST_DI_SS_Basis.MetadataQuery.ComplexCondition.QueryGeometry = null;

                if (barEditItemSpacialInfo.EditValue.ToString() == "行政区域")
                {
                    this.mucsearcher.qrstAxGlobeControl1.ClearRect(false);
                    repositoryItemComboBoxProvince.Items.Clear();
                    
                    //AreaLocationQuery areaQuery = new AreaLocationQuery(TheUniversal.BSDB.sqlUtilities);
                    //List<string> provinceLst = areaQuery.GetProvinceLst();
                    double minLon, maxLon, maxLat, minLat;
                    if (repositoryItemComboBoxProvince.Items.Count == 0)
                    {
                        if (barEditItemProvince.EditValue!=null && barEditItemProvince.EditValue.ToString() == "BeiJing")
                        {
                            getProvinceFt();
                        }
                        repositoryItemComboBoxProvince.Items.AddRange(provinceLst);
                        barEditItemProvince.EditValue = provinceLst[0];
                    }

                    if (SpacialInfo == "经纬坐标")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemHandleSel);

                    }
                    else if (SpacialInfo == "行列号范围")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxRow);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinRow);
                    }
                    else if (SpacialInfo == "坐标文件")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barStaticPositionFileName);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadPositionFile);
                    }
                    else if (SpacialInfo == "自定义多边形")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadDrawPolygon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowPoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadSavePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadReadPoint);
                    }
                    else if (SpacialInfo == "自定义折线")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemDrawPolyline);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemSaveLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemReadLinePoint);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 1, this.barEditItemProvince);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 2, this.barEditItemUrban);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 4, this.barEditItemCounty);
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    SpacialInfo = "行政区域";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "经纬坐标")
                {
                    repositoryItemComboBoxProvince.Items.Clear();
                    this.mucsearcher.qrstAxGlobeControl1.ClearPoly(false);
                    if (SpacialInfo == "行列号范围")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxRow);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinRow);

                    }
                    else if (SpacialInfo == "坐标文件")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barStaticPositionFileName);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadPositionFile);
                    }
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemProvince);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemUrban);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemCounty);
                    }
                    else if (SpacialInfo == "自定义多边形")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadDrawPolygon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowPoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadSavePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadReadPoint);
                    }
                    else if (SpacialInfo == "自定义折线")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemDrawPolyline);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemSaveLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemReadLinePoint);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 1, this.barEditItemMaxLon);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 2, this.barEditItemMinLon);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 4, this.barEditItemMaxLat);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 5, this.barEditItemMinLat);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 6, this.barButtonItemHandleSel);

                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    SpacialInfo = "经纬坐标";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "坐标文件")
                {
                    repositoryItemComboBoxProvince.Items.Clear();
                    this.mucsearcher.qrstAxGlobeControl1.ClearPoly(false);
                    if (SpacialInfo == "经纬坐标")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemHandleSel);
                    }
                    else if (SpacialInfo == "行列号范围")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxRow);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinRow);
                    }
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemProvince);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemUrban);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemCounty);
                    }
                    else if (SpacialInfo == "自定义多边形")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadDrawPolygon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowPoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadSavePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadReadPoint);
                    }
                    else if (SpacialInfo == "自定义折线")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemDrawPolyline);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemSaveLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemReadLinePoint);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 1, this.barButtonItemLoadPositionFile);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 2, this.barStaticPositionFileName);
                    this.barButtonItemLoadPositionFile.Tag = null;
                    this.barStaticPositionFileName.Caption = "";
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    SpacialInfo = "坐标文件";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "行列号范围")
                {
                    this.mucsearcher.qrstAxGlobeControl1.ClearRect(false);
                    repositoryItemComboBoxProvince.Items.Clear();
                    this.mucsearcher.qrstAxGlobeControl1.ClearPoly(false);
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    if (SpacialInfo == "经纬坐标")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemHandleSel);
                    }
                    else if (SpacialInfo == "坐标文件")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barStaticPositionFileName);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadPositionFile);
                    }
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemProvince);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemUrban);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemCounty);
                    }
                    else if (SpacialInfo == "自定义多边形")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadDrawPolygon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowPoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadSavePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadReadPoint);
                    }
                    else if (SpacialInfo == "自定义折线")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemDrawPolyline);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemSaveLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemReadLinePoint);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 1, this.barEditItemMaxCol);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 2, this.barEditItemMinCol);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 4, this.barEditItemMaxRow);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 5, this.barEditItemMinRow);
                    SpacialInfo = "行列号范围";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "自定义多边形")
                {
                    this.mucsearcher.qrstAxGlobeControl1.ClearRect(false);
                    repositoryItemComboBoxProvince.Items.Clear();
                    this.mucsearcher.qrstAxGlobeControl1.ClearPoly(false);
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    if (SpacialInfo == "经纬坐标")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemHandleSel);
                    }
                    else if (SpacialInfo == "坐标文件")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barStaticPositionFileName);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadPositionFile);
                    }
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemProvince);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemUrban);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemCounty);
                    }
                    else if (SpacialInfo == "行列号范围")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxRow);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxCol);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinRow);
                    }
                    else if (SpacialInfo == "自定义折线")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemDrawPolyline);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemSaveLinePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemReadLinePoint);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 2, this.barButtonItemLoadDrawPolygon);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 1, this.barEditItemShowPoint);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 3, this.barButtonItemLoadSavePoint);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 4, this.barButtonItemLoadReadPoint);
                    SpacialInfo = "自定义多边形";
                }
                else if (barEditItemSpacialInfo.EditValue.ToString() == "自定义折线")
                {
                    this.mucsearcher.qrstAxGlobeControl1.ClearRect(false);
                    repositoryItemComboBoxProvince.Items.Clear();
                    this.mucsearcher.qrstAxGlobeControl1.ClearPoly(false);
                    this.barEditItemMaxCol.EditValue = "";
                    this.barEditItemMinCol.EditValue = "";
                    this.barEditItemMaxRow.EditValue = "";
                    this.barEditItemMinRow.EditValue = "";
                    if (SpacialInfo == "经纬坐标")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLat);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMaxLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemMinLon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemHandleSel);
                    }
                    else if (SpacialInfo == "坐标文件")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barStaticPositionFileName);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadPositionFile);
                    }
                    else if (SpacialInfo == "行政区域")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemProvince);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemUrban);
                        this.ribbonPageGroup1.ItemLinks.Remove(barEditItemCounty);
                    }
                    else if (SpacialInfo == "自定义多边形")
                    {
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadDrawPolygon);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barEditItemShowPoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadSavePoint);
                        this.ribbonPageGroup1.ItemLinks.Remove(this.barButtonItemLoadReadPoint);
                    }
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 2, this.barButtonItemDrawPolyline);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 1, this.barEditItemShowLinePoint);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 3, this.barButtonItemSaveLinePoint);
                    this.ribbonPageGroup1.ItemLinks.Insert(idxSpacialInfoItem + 4, this.barButtonItemReadLinePoint);
                    SpacialInfo = "自定义折线";
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
        /// 设置数据集复原按钮不显示
        /// </summary>
        void setTilebarButtonResetNotShow() 
        {
            //this.ribbonPageGroup3.ItemLinks.Remove(this.barButtonReset);
            this.barButtonReset.Visibility = BarItemVisibility.Never;
        }
        /// <summary>
        /// 设置数据集复原按钮显示
        /// </summary>
        void setTilebarButtonResetShow()
        {
            //this.ribbonPageGroup3.ItemLinks.Add(this.barButtonReset);
            this.barButtonReset.Visibility = BarItemVisibility.Always;
        }

        /// <summary>
        /// 设置关键字检索控件
        /// </summary>
        void setKeyWordControl()
        {
            this.barEditItemKeyWord.EditValue = "";
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemKeyWord, true, "", "", true);
        }

        /// <summary>
        /// 设置非规影像级产品类型选择控件
        /// </summary>
        void setImageProdTypeSelector()
        {
            if (this.rep_barItm_ImageProdTypesEdit1.Items.Count == 0)//加载非规影像级产品类型列表
            {
                string sql = "SELECT DISTINCT prodtype FROM `imageprod`;";

                System.Data.DataSet dsTypes = TheUniversal.INDB.sqlUtilities.GetDataSet(sql);
                if (dsTypes != null)
                {
                    for (int i = 0; i < dsTypes.Tables[0].Rows.Count; i++)
                    {
                        rep_barItm_ImageProdTypesEdit1.Items.Add(dsTypes.Tables[0].Rows[i]["prodtype"]);
                    }
                }

            }
            this.ribbonPageGroup1.ItemLinks.Add(this.barItm_ImageProdTypes, true, "", "", true);
        }


        /// <summary>
        /// 设置卫星传感器查询控件
        /// </summary>
        void setSatalliteAndSensor()
        {

            if (this.repositoryItemCheckedComboBoxEditSensors.Items.Count == 0)//加载传感器数据列表
            {
                System.Data.DataSet dsSensor = Sensors.GetSensorDataSet(TheUniversal.EVDB.sqlUtilities, "");
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
                System.Data.DataSet dsSate = Satellite.GetSatelliteDataSet(TheUniversal.EVDB.sqlUtilities, "");
                if (dsSate != null)
                {
                    for (int i = 0; i < dsSate.Tables[0].Rows.Count; i++)
                    {
                        repositoryItemCheckedComboBoxEditSates.Items.Add(dsSate.Tables[0].Rows[i]["NAME"]);
                    }
                }
            }
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
            this.barEditKeyWords.EditValue = "";
            //this.ribbonPageGroup1.ItemLinks.Add(this.barEditName, true, "", "", true);
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
                //if (mySQLSerVice == null)
                //{
                //    this.mySQLSerVice = new Service();
                //}
                jsonStr = _dbSearClient.getSoilSubTypes();
                classify = JSON.parse<GetClassify>(jsonStr);
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
                //if (mySQLSerVice == null)
                //{
                //    this.mySQLSerVice = new Service();
                //}
                //获取岩石所属类别列表
                jsonStr = _dbSearClient.getRockSSLB();
                classify = JSON.parse<GetClassify>(jsonStr);
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxRockAttribute, strFields);
                //获取岩矿子类列表
                jsonStr = _dbSearClient.getRockSubTypes();
                classify = JSON.parse<GetClassify>(jsonStr);
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxRockSubType, strFields);
                //获取岩矿类别列表
                jsonStr = _dbSearClient.getRockTypes();
                classify = JSON.parse<GetClassify>(jsonStr);
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
                    //if (mySQLSerVice == null)
                    //{
                    //    this.mySQLSerVice = new Service();
                    //}
                    jsonStr = _dbSearClient.getNorVegTypes();
                    classify = JSON.parse<GetClassify>(jsonStr);
                    //classify = JSON.parse<GetClassify>(vnorthclient.getTypes());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantType, strFields);
                    jsonStr = _dbSearClient.getNorVegCLBW();
                    classify = JSON.parse<GetClassify>(jsonStr);
                    //classify = JSON.parse<GetClassify>(vnorthclient.getCLBW());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantPosition, strFields);
                    jsonStr = _dbSearClient.getNorVegWHQ();
                    classify = JSON.parse<GetClassify>(jsonStr);
                    //classify = JSON.parse<GetClassify>(vnorthclient.getWHQ());
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

                    //if (mySQLSerVice == null)
                    //{
                    //    this.mySQLSerVice = new Service();
                    //}
                    jsonStr = _dbSearClient.getSouVegTypes();
                    classify = JSON.parse<GetClassify>(jsonStr);
                    //                     classify = JSON.parse<GetClassify>(vsouthclient.getTypes());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantType, strFields);
                    jsonStr = _dbSearClient.getSouVegCLBW();
                    classify = JSON.parse<GetClassify>(jsonStr);
                    //                     classify = JSON.parse<GetClassify>(vsouthclient.getCLBW());
                    classifyfgw = classify.types;
                    strFields = new string[classifyfgw.Length];
                    for (int i = 0; i < classifyfgw.Length; i++)
                    {
                        strFields[i] = classifyfgw[i].type;
                    }
                    SetAvailableFields(repositoryItemComboBoxPlantPosition, strFields);
                    jsonStr = _dbSearClient.getSouVegWHQ();
                    classify = JSON.parse<GetClassify>(jsonStr);
                    //                     classify = JSON.parse<GetClassify>(vsouthclient.getWHQ());
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
                jsonStr = _dbSearClient.getCityTypes();
                classify = JSON.parse<GetClassify>(jsonStr);
                //                 classify = JSON.parse<GetClassify>(cityobjclient.getTypes());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxCityType, strFields);
                jsonStr = _dbSearClient.getCityCSMBMC();
                classify = JSON.parse<GetClassify>(jsonStr);
                //                 classify = JSON.parse<GetClassify>(cityobjclient.getCSMBMC());
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
                jsonStr = _dbSearClient.getAtmosZDBH();
                classify = JSON.parse<GetClassify>(jsonStr);
                //                 classify = JSON.parse<GetClassify>(atmosphereclient.getZDBH());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxAtmosphereCode, strFields);
                jsonStr = _dbSearClient.getAtmosZDMC();
                classify = JSON.parse<GetClassify>(jsonStr);
                //                 classify = JSON.parse<GetClassify>(atmosphereclient.getZDMC());
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
                jsonStr = _dbSearClient.getWaterSSLB();
                classify = JSON.parse<GetClassify>(jsonStr);
                //                 classify = JSON.parse<GetClassify>(waterclient.getSSLB());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxWaterType, strFields);
                jsonStr = _dbSearClient.getWaterGPYQ();
                classify = JSON.parse<GetClassify>(jsonStr);
                //                 classify = JSON.parse<GetClassify>(waterclient.getGPYQ());
                classifyfgw = classify.types;
                strFields = new string[classifyfgw.Length];
                for (int i = 0; i < classifyfgw.Length; i++)
                {
                    strFields[i] = classifyfgw[i].type;
                }
                SetAvailableFields(repositoryItemComboBoxWaterYiQI, strFields);
                jsonStr = _dbSearClient.getWaterSYMC();
                classify = JSON.parse<GetClassify>(jsonStr);
                //                 classify = JSON.parse<GetClassify>(waterclient.getSYMC());
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

         //this.barEditItemShowPoint.EditValue = "";
         //   isRead = false;
         //   if (this.mucsearcher.Is3DViewer)
         //   {
         //       this.mucsearcher.qrstAxGlobeControl1.TurnOnOffDrawPloygonTool();
         //   }
         //   else
         //   {
         //       //this.mucsearcher.qrstAxGlobeControl1.flag = true;
         //       this.mucsearcher.uc2DSearcher1.enabledDrawPolygon();
         //       this.mucsearcher.uc2DSearcher1.drawPolygonCompletedEvent += new Action(qrstAxGlobeControl1_OnDrawPolygonCompeleted);
         //   }
         //   isDraw = true;
        /// <summary>
        /// 手动选择区域按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemHandleSel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.mucsearcher.Is3DViewer)
            {
                //this.mucsearcher.qrstAxGlobeControl1.TurnOnOffDrawPloygonTool();
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
            QRST_DI_SS_Basis.MetadataQuery.SimpleCondition sm = new QRST_DI_SS_Basis.MetadataQuery.SimpleCondition();
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
            if (barEditItemNumPerPage.EditValue.ToString() == "全部记录")
            {
                return -1;
            }
            else if (int.TryParse(barEditItemNumPerPage.EditValue.ToString(), out size))
            {
                return size;
            }
            else
            {
                return 1000;
            }
        }
        /// <summary>
        /// 检索按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.rpgModuleSearch.Visible)
            {
                ClearExtentLyr();
                this.rpgModule.Search();
                return;
            }


            if (barEditItemBeginDate.EditValue != null && barEditItemEndDate.EditValue != null)
            {
                startTime = DateTime.Parse(barEditItemBeginDate.EditValue.ToString());
                endTime = DateTime.Parse(barEditItemEndDate.EditValue.ToString());
            }
            DateTime nowTime = DateTime.Now;
            TimeSpan BettownTime = nowTime - lastSearchTime;
            if (BettownTime.Days > 0 || BettownTime.Hours > 0 || BettownTime.Minutes > 0 || BettownTime.Seconds > 2 || queryNum == 0)
            {
                clickFlag = false;
                lastSearchTime = nowTime;
                queryNum++;
                Query();
                isDraw = false;
            }
            else
            {
                MessageBox.Show("您查询过于频繁，请稍后再试！");
            }
        }

        System.Data.DataSet distinctDataSet;
        DataTilePara oldTilePara;
        DataTable dtable = new DataTable();
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
            mucdetail.ClearGridView();

            //收集界面上的查询条件，构造查询参数对象queryPara，并构造ComplexCondition,调用IQuery接口完成数据查询
            Check();
            mucdetail.gridViewMain.RowHeight = -1;
            queryPara = null;
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))    //构造栅格数据queryPara对象
            {
                queryPara = ConstructRasterQueryPara();
                if (querySchema.GetTableName() == "imageprod_view")
                {
                    ((RasterQueryPara)queryPara).isimageprod = true;
                }
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
                        System.Data.DataSet ds = new System.Data.DataSet();
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


                    switch (selectedQueryObj.NAME)
                    {
                        case "地表大气":
                            queryPara.QRST_CODE = "站点编号";
                            break;
                        case "土壤":
                        case "南方植被":
                        case "北方植被":
                        case "城市目标":
                        case "水体":
                        case "岩矿":
                            queryPara.QRST_CODE = "光谱数据";
                            break;
                    }

                    return;
                }
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_document"))//构造文档数据queryPara对象
            {
                queryPara = ConstructDocQueryPara();
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_normalfile"))//构造一般文件数据queryPara对象
            {
                queryPara = ConstructNormalFileQueryPara();
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

                    //if (sqliteclient == null)
                    //{
                    //    sqliteclient = new localhostSqlite.Service();
                    //}
                    //设置查看详细信息界面的显示情况
                    mucdetail.setTileSinglePanel2();

                    //DLF 130921
                    //ExtentDs = _tileClient.SearSpaceDistinctTiles(tilePara.spacialPara,
                    //    new int[] { int.Parse(tilePara.timePara[0]), int.Parse(tilePara.timePara[1]) },
                    //    tilePara.satelliteType, tilePara.sensorType, tilePara.dataTileType, tilePara.level);
                    //DrawTileAllExtents(ExtentDs);
                    DateTime dtTask1 = new DateTime();
                    dtTask1 = DateTime.Now;
                    IPagingQuery pagingQuery = new TileDataPagingQuery(_tileClient, tilePara);
                    mucdetail.isFirst = true;
                    CtrlPage ctrlPage = mucdetail.GetCtrlPage();
                    ctrlPage.SetPageSize(GetPageSize());
                    ctrlPage.queryFinishedEventHandle = QueryFinishedEvent;
                    ctrlPage.Binding(pagingQuery);
                    ctrlPage.FirstQuery();
                    ctrlPage.UpdatePageUC();
                    DateTime dtTask2 = DateTime.Now;
                    TimeSpan dttotal = dtTask2 - dtTask1;
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
                        //if (sqliteclient == null)
                        //{
                        //    sqliteclient = new localhostSqlite.Service();
                        //}
                        //设置查看详细信息界面的显示情况
                        mucdetail.setTileSinglePanel2();

                        IPagingQuery pagingQuery = new ProductTileDataPagingQuery(_tileClient, prodPara);
                        CtrlPage ctrlPage = mucdetail.GetCtrlPage();
                        ctrlPage.SetPageSize(GetPageSize());
                        ctrlPage.queryFinishedEventHandle = QueryFinishedEvent;
                        ctrlPage.Binding(pagingQuery);
                        ctrlPage.FirstQuery();
                        ctrlPage.UpdatePageUC();
                    }
                }

            }


            if (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                //构造查询条件对象
                QRST_DI_SS_Basis.MetadataQuery.ComplexCondition queryCondition = ConstructSeniorQueryCondition();
                if (queryPara != null)
                {
                    queryCondition.complexCondition = new QRST_DI_SS_Basis.MetadataQuery.ComplexCondition[1];
                    queryCondition.complexCondition[0] = queryPara.GetSpecificCondition(querySchema);
                    queryCondition.complexCondition[0].ruleName = rule;
                    try
                    {
                        selectedRect[0] = Convert.ToDouble(barEditItemMinLon.EditValue);
                        selectedRect[1] = Convert.ToDouble(barEditItemMinLat.EditValue);
                        selectedRect[2] = Convert.ToDouble(barEditItemMaxLon.EditValue);
                        selectedRect[3] = Convert.ToDouble(barEditItemMaxLat.EditValue);

                        //可以把以下几行代码放到全覆盖里面
                        List<Coordinate> coords = new List<Coordinate>();


                        coords.Add(new Coordinate(selectedRect[0], selectedRect[1]));
                        coords.Add(new Coordinate(selectedRect[0], selectedRect[3]));
                        coords.Add(new Coordinate(selectedRect[2], selectedRect[3]));
                        coords.Add(new Coordinate(selectedRect[2], selectedRect[1]));
                        testF = new Feature(FeatureType.Polygon, coords);
                       // coords.Add(new Coordinate(lulon, lulat));

                        //Coordinate coord1 = new DotSpatial.Topology.Coordinate(140, 30);
                        //Coordinate coord2 = new DotSpatial.Topology.Coordinate(141, 30);
                        //Coordinate coord3 = new DotSpatial.Topology.Coordinate(142, 30);
                        //Coordinate coord4 = new DotSpatial.Topology.Coordinate(140, 33);
                        //Coordinate coord5 = new DotSpatial.Topology.Coordinate(140, 30);
                        //Feature ft2 = new Feature(DotSpatial.Topology.FeatureType.Polygon, new Coordinate[] { coord1, coord2, coord3, coord4, coord5 });
                        //IFeature ft3 = ft2 as IFeature;
                    }
                    catch
                    {
                        selectedRect = new double[] { -180, -90, 180, 90 };
                    }
                    queryCondition.complexCondition[0].selectRation = selectedRect;
                    queryCondition.ruleName = rule;
                }
                queryPara.GetPublicFieldMappedValue(querySchema);
                //构造查询请求
                queryRequest = new QRST_DI_SS_Basis.MetadataQuery.QueryRequest();
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
                dtable = ctrlPage.dt;
                //int PageSize = ctrlPage.PageSize;
                //int totalNum = ctrlPage._recordNum;
                //if (totalNum <= PageSize)
                //{
                //    dtable = ctrlPage.dt;
                //}
                //else
                //{
                //    DataTable dt = new DataTable();
                //    for (int i = 0; i < totalNum / PageSize + 1; i++)
                //    {
                //        dt = pagingQuery.GetCurrentPageData(i * PageSize, PageSize);
                //        dtable.Merge(dt);
                //    }
                //}
               
            }
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

            queryResponse = new QRST_DI_SS_Basis.MetadataQuery.QueryResponse();

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
            _rasterQueryPara.ENDTIME = endTime.ToString("yyyy-MM-dd HH:mm:ss");
            _rasterQueryPara.EXTENTDOWN = barEditItemMinLat.EditValue.ToString();
            _rasterQueryPara.EXTENTLEFT = barEditItemMinLon.EditValue.ToString();
            _rasterQueryPara.EXTENTRIGHT = barEditItemMaxLon.EditValue.ToString();
            _rasterQueryPara.EXTENTUP = barEditItemMaxLat.EditValue.ToString();
            _rasterQueryPara.KEYWORDS = barEditItemKeyWord.EditValue.ToString();
            _rasterQueryPara.STARTTIME = startTime.ToString("yyyy-MM-dd HH:mm:ss");
            _rasterQueryPara.SATELLITE = barEditItemSateCheck.EditValue.ToString();
            _rasterQueryPara.SENSOR = barEditItemSensorCheck.EditValue.ToString();
            _rasterQueryPara.IMAGEPRODTYPE = (barItm_ImageProdTypes.EditValue == null) ? "" : barItm_ImageProdTypes.EditValue.ToString();
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
            _docQueryPara.KEYWORDS = barEditKeyWords.EditValue.ToString();
            _docQueryPara.NAME = barEditName.EditValue.ToString();
            _docQueryPara.STARTTIME = startTime.ToString();
            return _docQueryPara;
        }


        NormalFileQueryPara ConstructNormalFileQueryPara()
        {
            NormalFileQueryPara _normalFileQueryPara = new NormalFileQueryPara();
            _normalFileQueryPara.dataCode = selectedQueryObj.DATA_CODE;
            _normalFileQueryPara.ENDTIME = endTime.ToString();
            _normalFileQueryPara.KEYWORDS = barEditKeyWords.EditValue.ToString();
            _normalFileQueryPara.STARTTIME = startTime.ToString();
            return _normalFileQueryPara;
        }

        /// <summary>
        /// 将高级检索中的信息组装成ComplexCondition对象
        /// </summary>
        /// <returns></returns>
        QRST_DI_SS_Basis.MetadataQuery.ComplexCondition ConstructSeniorQueryCondition()
        {
            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition cp = new QRST_DI_SS_Basis.MetadataQuery.ComplexCondition();
            if (listSimpleCondistons.Count != 0)
            {
                cp.logicOperator = QRST_DI_SS_Basis.MetadataQuery.EnumLogicalOperator.AND;
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
            DateTime time;

            try
            {
                List<object> fileinfolist = new List<object>();
                switch (selectedQueryObj.NAME)
                {
                    case "土壤":
                        if (barEditItemBeginDate.EditValue != null)
                        {
                            time = Convert.ToDateTime(barEditItemBeginDate.EditValue);
                            //begindate = barEditItemBeginDate.EditValue.ToString();
                            begindate = time.ToString("yyyy/M/d");
                            //begindate = barEditItemBeginDate.EditValue.ToString();
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            time = Convert.ToDateTime(barEditItemEndDate.EditValue);
                            enddate = time.ToString("yyyy/M/d");
                            //enddate = barEditItemEndDate.EditValue.ToString();
                        }
                        MaxLon = barEditItemMaxLon.EditValue == null ? "" : barEditItemMaxLon.EditValue.ToString();
                        Maxlat = barEditItemMaxLat.EditValue == null ? "" : barEditItemMaxLat.EditValue.ToString();
                        MinLon = barEditItemMinLon.EditValue == null ? "" : barEditItemMinLon.EditValue.ToString();
                        MinLat = barEditItemMinLat.EditValue == null ? "" : barEditItemMinLat.EditValue.ToString();
                        string soilname = barEditItemSoilName.EditValue == null ? "" : barEditItemSoilName.EditValue.ToString();
                        string soilzilei = barEditItemSoliSubType.EditValue == null ? "" : barEditItemSoliSubType.EditValue.ToString();
                        //                         string jsonStr = soilclient.getQuerySoils(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, soilname, soilzilei, "", "0", "150");
                        jsonStr = _dbSearClient.getQuerySoils(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, soilname, soilzilei, "", "0", "150");
                        Soil soil = JSON.parse<Soil>(jsonStr);
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
                            //begindate = barEditItemBeginDate.EditValue.ToString();
                            time = Convert.ToDateTime(barEditItemBeginDate.EditValue);
                            begindate = time.ToString("yyyyMMdd");
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            time = Convert.ToDateTime(barEditItemEndDate.EditValue);
                            //enddate = barEditItemEndDate.EditValue.ToString();
                            enddate = time.ToString("yyyyMMdd");
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
                        jsonStr = _dbSearClient.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", "");
                        Vegetation vegetation = JSON.parse<Vegetation>(jsonStr);
                        //                         Vegetation vegetation = JSON.parse<Vegetation>(vsouthclient.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", ""));
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
                            //begindate = barEditItemBeginDate.EditValue.ToString();
                            time = Convert.ToDateTime(barEditItemBeginDate.EditValue);
                            begindate = time.ToString("yyyyMMdd");

                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            //enddate = barEditItemEndDate.EditValue.ToString();
                            time = Convert.ToDateTime(barEditItemEndDate.EditValue);
                            enddate = time.ToString("yyyyMMdd");
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
                        jsonStr = _dbSearClient.getQueryNorVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", "");
                        vegetation = JSON.parse<Vegetation>(jsonStr);
                        //                         vegetation = JSON.parse<Vegetation>(vnorthclient.getQueryVegetations(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, "", ""));
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
                            time = Convert.ToDateTime(barEditItemBeginDate.EditValue);
                            //begindate = barEditItemBeginDate.EditValue.ToString();
                            begindate = time.ToString("yyyyMMdd");
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            time = Convert.ToDateTime(barEditItemEndDate.EditValue);
                            //enddate = barEditItemEndDate.EditValue.ToString();
                            enddate = time.ToString("yyyyMMdd");
                        }
                        string c_csmc = barEditItemCityName.EditValue == null ? "" : barEditItemCityName.EditValue.ToString();
                        string c_cslb = barEditItemCityType.EditValue == null ? "" : barEditItemCityType.EditValue.ToString();
                        ////////////////////
                        jsonStr = _dbSearClient.getQueryCityObjs(begindate, enddate, c_csmc, c_cslb, "", "");
                        CityObj cityObj = JSON.parse<CityObj>(jsonStr);
                        //                         CityObj cityObj = JSON.parse<CityObj>(cityobjclient.getQueryCityObjs(begindate, enddate, c_csmc, c_cslb, "", ""));
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
                        jsonStr = _dbSearClient.getQueryAtmospheres(Maxlat, MaxLon, MinLat, MinLon, a_zdmc, a_zdbh, "", "");
                        Atmosphere atmosphere = JSON.parse<Atmosphere>(jsonStr);
                        //                         Atmosphere atmosphere = JSON.parse<Atmosphere>(atmosphereclient.getQueryAtmospheres(Maxlat, MaxLon, MinLat, MinLon, a_zdmc, a_zdbh, "", ""));
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
                            time = Convert.ToDateTime(barEditItemBeginDate.EditValue);
                            //begindate = barEditItemBeginDate.EditValue.ToString();
                            begindate = time.ToString("yyyy/M/d");
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            //enddate = barEditItemEndDate.EditValue.ToString();
                            time = Convert.ToDateTime(barEditItemEndDate.EditValue);
                            enddate = time.ToString("yyyy/M/d");
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
                        jsonStr = _dbSearClient.getQueryWaters(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, w_symc, w_gpyq, w_sslb, "", "");
                        Water water = JSON.parse<Water>(jsonStr);
                        //                         Water water = JSON.parse<Water>(waterclient.getQueryWaters(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, w_symc, w_gpyq, w_sslb, "", ""));
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
                            time = Convert.ToDateTime(barEditItemBeginDate.EditValue);
                            //begindate = barEditItemBeginDate.EditValue.ToString();
                            begindate = time.ToString("yyyy/M/d");
                        }
                        if (barEditItemEndDate.EditValue != null)
                        {
                            //enddate = barEditItemEndDate.EditValue.ToString();
                            time = Convert.ToDateTime(barEditItemEndDate.EditValue);
                            enddate = time.ToString("yyyy/M/d");
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
                        jsonStr = _dbSearClient.getQueryRocks(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, r_ykmc, r_yklb, r_ykzl, r_sslb, "", "");
                        RockMineral rockmineral = JSON.parse<RockMineral>(jsonStr);
                        //                         RockMineral rockmineral = JSON.parse<RockMineral>(rockclient.getQueryRocks(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, r_ykmc, r_yklb, r_ykzl, r_sslb, "", ""));
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
        /// 获取瓦片的切片等级
        /// </summary>
        /// <returns></returns>
        public string[] getTileLevel() 
        {
            string[] selectedlevel;
            if (this.barEditItemTileLevel.EditValue == null || this.barEditItemTileLevel.EditValue.ToString() == "")
            {
                //只要有空间范围就必须传 等级取值。但界面中可能没有选择，此时自动查询并添加全部等级。
                    //if (sqliteclient == null)
                    //{
                    //    sqliteclient = new localhostSqlite.Service();
                    //}
                    //dataTilePara.level = sqliteclient.SearTileLevels();
                    List<string> levels = new List<string>();
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem strresobj in repositoryItemComboBoxEditTileLevel.Items)
                    {
                        string strres = strresobj.Value.ToString();
                        string lvstr = DirectlyAddressing.GetStrLvByResolution(strres);
                        if (lvstr.Trim() != "")
                        {
                            levels.Add(lvstr);
                        }
                    }
                    selectedlevel = levels.ToArray();
                
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
                selectedlevel = levels.ToArray();
            }

            return selectedlevel;
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
                    //if (sqliteclient == null)
                    //{
                    //    sqliteclient = new localhostSqlite.Service();
                    //}
                    //dataTilePara.level = sqliteclient.SearTileLevels();
                    List<string> levels = new List<string>();
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem strresobj in repositoryItemComboBoxEditTileLevel.Items)
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
                string temp = this.barEditItemProTypeCombo.EditValue.ToString();

                proTilePara.productType = temp.Split(',');
            }
            if (this.barEditItemTileLevel.EditValue == null || this.barEditItemTileLevel.EditValue.ToString() == "")
            {
                //只要有空间范围就必须传 等级取值。但界面中可能没有选择，此时自动查询并添加全部等级。
                if (proTilePara.spacialPara.Length > 0)
                {
                    //if (sqliteclient == null)
                    //{
                    //    sqliteclient = new localhostSqlite.Service();
                    //}
                    proTilePara.level = _tileClient.SearProTileLevels().ToArray();
                    //proTilePara.level = sqliteclient.SearProTileLevels();
                    List<string> levels = new List<string>();
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem strresobj in repositoryItemComboBoxEditTileLevel.Items)
                    {
                        string strres = strresobj.Value.ToString();
                        string lvstr = DirectlyAddressing.GetStrLvByResolution(strres);
                        if (lvstr.Trim() != "")
                        {
                            levels.Add(lvstr);
                        }
                    }
                    proTilePara.level = levels.ToArray();
                }
                else
                {
                    proTilePara.level = new string[] { };
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
                proTilePara.level = levels.ToArray();

                //proTilePara.level = this.barEditItemTileLevel.EditValue.ToString().Trim().Split(',');
            }


            //if (this.barEditItemTileLevel.EditValue == null || this.barEditItemTileLevel.EditValue.ToString() == "")
            //{
            //    //只要有空间范围就必须传 等级取值。但界面中可能没有选择，此时自动查询并添加全部等级。
            //    if (proTilePara.spacialPara.Length > 0)
            //    {
            //        //if (sqliteclient == null)
            //        //{
            //        //    sqliteclient = new localhostSqlite.Service();
            //        //}
            //        proTilePara.level = _tileClient.SearProTileLevels();
            //    }
            //    else
            //    {
            //        proTilePara.level = new string[] { };
            //    }
            //}
            //else
            //{
            //    proTilePara.level = this.barEditItemTileLevel.EditValue.ToString().Trim().Split(',');
            //}

            return proTilePara;
        }
        public string[] productType;
        public static string prodType;
        /// <summary>
        /// 填充产品切片的全覆盖查询条件
        /// </summary>
        /// <returns></returns>
        public void SetProductTileFullPara()
        {
            try
            {
                spacialPara[0] = this.barEditItemMinLon.EditValue.ToString();
                spacialPara[1] = this.barEditItemMinLat.EditValue.ToString();
                spacialPara[2] = this.barEditItemMaxLon.EditValue.ToString();
                spacialPara[3] = this.barEditItemMaxLat.EditValue.ToString();
                //if (SpacialInfo == "Longitude and Latitude coordinates")
                if (barEditItemSpacialInfo.EditValue.ToString() == "经纬坐标")
                {
                    List<Coordinate> coords1 = new List<Coordinate>();
                    coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[1])));
                    coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[3])));
                    coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[3])));
                    coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[1])));
                    testF = new Feature(FeatureType.Polygon, coords1);

                }

                IFeature tileft = testF;
                IList<Coordinate> coords = QRST_DI_TS_Basis.Search.ShapeSimplifier.Simplifier(tileft.BasicGeometry.Coordinates);

                //List<Coordinate> coords = new List<Coordinate>();
                //coords.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[1])));
                //coords.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[3])));
                //coords.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[3])));
                //coords.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[1])));
                //testF = new Feature(FeatureType.Polygon, coords);

                coordstr = getCoordstr(coords);
            }
            catch (Exception)
            {
                coordstr = "-180,-90;-180,90;180,90;180,-90";
            }
            if (barEditItemBeginDate.EditValue != null && barEditItemEndDate.EditValue != null)
            {
                startTime = DateTime.Parse(barEditItemBeginDate.EditValue.ToString());
                endTime = DateTime.Parse(barEditItemEndDate.EditValue.ToString());
                //因为瓦片重命名后，时间就由原来的8位变成10位了
                startdate = parseTimeToInt(startTime) * 100;
                enddate = parseTimeToInt(endTime) * 100 + 24;

            }
            if (this.barEditItemProTypeCombo.EditValue == null || this.barEditItemProTypeCombo.EditValue.ToString() == "")
            {
                productType = new string[] { };
            }
            else
            {
                productType = new string[] { this.barEditItemProTypeCombo.EditValue.ToString() };
            }
            //把字符串数组转化为字符串
            prodType = string.Join(",", productType);

            if (this.barEditItemTileLevel.EditValue == null || this.barEditItemTileLevel.EditValue.ToString() == "")
            {
                //只要有空间范围就必须传 等级取值。但界面中可能没有选择，此时自动查询并添加全部等级。
                if (spacialPara.Length > 0)
                {
                    //if (sqliteclient == null)
                    //{
                    //    sqliteclient = new localhostSqlite.Service();
                    //}
                    //level = sqliteclient.SearProTileLevels();

                    List<string> levels = new List<string>();
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem strresobj in repositoryItemComboBoxEditTileLevel.Items)
                    {
                       string strres = strresobj.Value.ToString();
                       string lvstr = DirectlyAddressing.GetStrLvByResolution(strres);
                       if (lvstr.Trim() != "")
                       {
                          levels.Add(lvstr);
                       }
                    }
                    level = levels.ToArray();
                }
                else
                {
                    level = new string[] { };
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
                level = levels.ToArray();
            }
            //把字符串数组转化为字符串
             tileLevel = string.Join(",", level);
          
        }

        public string[] satelliteType;
        public string[] sensorType;
        public  string[] level;
        public Coordinate everycood;
         public string coordstr;
        public string[] spacialPara = new string[] { "-180", "-90", "180", "90" };
        public static int startdate;
        public static int enddate;
        public static string sat =null;
        public static string sensor;
        public static string tileLevel;
        /// <summary>
        /// 填充数据切片全覆盖查询条件
        /// </summary>
        /// <returns></returns>
        public void  SetTileFullQueryPara()
        {
            try
            {
                 spacialPara[0] = this.barEditItemMinLon.EditValue.ToString();
                 spacialPara[1] = this.barEditItemMinLat.EditValue.ToString();
                 spacialPara[2] = this.barEditItemMaxLon.EditValue.ToString();
                 spacialPara[3] = this.barEditItemMaxLat.EditValue.ToString();
                 //if (SpacialInfo == "Longitude and Latitude coordinates")
                 if (barEditItemSpacialInfo.EditValue.ToString() == "经纬坐标")
                 {
                     List<Coordinate> coords1 = new List<Coordinate>();
                     coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[1])));
                     coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[3])));
                     coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[3])));
                     coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[1])));
                     testF = new Feature(FeatureType.Polygon, coords1);

                 }
                 IFeature tileft = testF;                 
                 IList<Coordinate> coords = QRST_DI_TS_Basis.Search.ShapeSimplifier.Simplifier(tileft.BasicGeometry.Coordinates);

                 //List<Coordinate> coords1 = new List<Coordinate>();
                 //coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[1])));
                 //coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[0]), Convert.ToDouble(spacialPara[3])));
                 //coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[3])));
                 //coords1.Add(new Coordinate(Convert.ToDouble(spacialPara[2]), Convert.ToDouble(spacialPara[1])));
                 //testF = new Feature(FeatureType.Polygon, coords);

                coordstr = getCoordstr(coords);
            }
            catch (Exception)
             {
                    coordstr = "-180,-90;-180,90;180,90;180,-90";

             }               
            if (barEditItemBeginDate.EditValue != null && barEditItemEndDate.EditValue != null)
            {
                startTime = DateTime.Parse(barEditItemBeginDate.EditValue.ToString());
                endTime = DateTime.Parse(barEditItemEndDate.EditValue.ToString());
                //因为瓦片重命名后，时间就由原来的8位变成10位了
                startdate = parseTimeToInt(startTime)*100;
                enddate = parseTimeToInt(endTime)*100 + 24;

            }
            if (this.barEditItemSateCheck.EditValue == null || this.barEditItemSateCheck.EditValue.ToString() == "")
            {
                satelliteType = new string[] { };
            }
            else
            {
                satelliteType = this.barEditItemSateCheck.EditValue.ToString().Trim().Split(',');
                for (int i = 0; i < satelliteType.Length; i++)
                {
                    satelliteType[i] = satelliteType[i].Trim();
                }
            }
            //把字符串数组转化为字符串
           sat = string.Join(",", satelliteType);

            if (this.barEditItemSensorCheck.EditValue == null || this.barEditItemSensorCheck.EditValue.ToString() == "")
            {
                sensorType = new string[] { };
            }
            else
            {
                sensorType = this.barEditItemSensorCheck.EditValue.ToString().Split(',');
                for (int i = 0; i < sensorType.Length; i++)
                {
                    sensorType[i] = sensorType[i].Trim();
                }
            }
            //把字符串数组转化为字符串
           sensor = string.Join(",", sensorType);

            if (this.barEditItemTileLevel.EditValue == null || this.barEditItemTileLevel.EditValue.ToString() == "")
            {
                //只要有空间范围就必须传 等级取值。但界面中可能没有选择，此时自动查询并添加全部等级。
                if (spacialPara.Length > 0)
                {
                    //if (sqliteclient == null)
                    //{
                    //    sqliteclient = new localhostSqlite.Service();
                    //}
                    //dataTilePara.level = sqliteclient.SearTileLevels();
                    List<string> levels = new List<string>();
                    foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem strresobj in repositoryItemComboBoxEditTileLevel.Items)
                    {
                        string strres = strresobj.Value.ToString();
                        string lvstr = DirectlyAddressing.GetStrLvByResolution(strres);
                        if (lvstr.Trim() != "")
                        {
                            levels.Add(lvstr);
                        }
                    }
                    level = levels.ToArray();
                }
                else
                {
                    level = new string[] { };
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
                level = levels.ToArray();
            }
            //把字符串数组转化为字符串
             tileLevel = string.Join(",", level);
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
            this.mucsearcher.qrstAxGlobeControl1.ClearPoly(isDraw);
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
        System.Data.DataSet distinctTileInfo;  //用于存放切片覆盖范围的统计信息与空间信息

        /// <summary>
        /// 获取切片覆盖范围统计信息 by zxw 20131222
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public Dictionary<System.Drawing.RectangleF, int> GetTileStasticExtentInfo(System.Data.DataSet ds)
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

        /// <summary>
        /// 20170315  
        /// 增加一个判断，如果表中含有Count字段
        /// </summary>
        /// <returns></returns>
        public Dictionary<System.Drawing.RectangleF, int> GetQueryTileStasticExtentInfo()
        {
            Dictionary<System.Drawing.RectangleF, int> extents1 = new Dictionary<System.Drawing.RectangleF, int>();

            Dictionary<string, int> extents0 = new Dictionary<string, int>();

            DataTable dt = queryResponse.recordSet.Tables[0];
            for (int ii = 0; ii < dt.Rows.Count; ii++)
            {
                string lrc = string.Format("{0}-{1}-{2}", dt.Rows[ii]["Level"].ToString(), dt.Rows[ii]["Row"].ToString(), dt.Rows[ii]["Col"].ToString());
                int count = 0;
                if (dt.Columns.Contains("Count"))
                {
                    try
                    {
                        if (dt.Rows[ii]["Count"].ToString() == "")
                        {
                            continue;
                        }
                        else
                        {
                            //Console.WriteLine("sdasdadsa" + dt.Rows[ii]["Count"].ToString());
                            count = Convert.ToInt32(dt.Rows[ii]["Count"].ToString());
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else
                {
                    count = 1;
                }
                if (extents0.Keys.Contains(lrc))
                    {
                    extents0[lrc] = extents0[lrc] + count;
                }
                else
                    {
                    extents0.Add(lrc, count);
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

                //List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
                List<List<float>> extents = new List<List<float>>();
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
                        List<float> listP = new List<float>();
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLONG].ToString()));
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLAT].ToString()));
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLONG].ToString()));
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLAT].ToString()));
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLONG].ToString()));
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLAT].ToString()));
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLONG].ToString()));
                        listP.Add(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLAT].ToString()));
                        extents.Add(listP);
                        //float minLat = Math.Min(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLAT].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLAT].ToString()));
                        //float maxLat = Math.Max(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLAT].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLAT].ToString()));
                        //float minLon = Math.Min(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERLEFTLONG].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERLEFTLONG].ToString()));
                        //float maxLon = Math.Max(float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATAUPPERRIGHTLONG].ToString()), float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.DATALOWERRIGHTLONG].ToString()));
                        //extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
                    }
                    
                    this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents1(extents);
                    //this.mucsearcher.qrstAxGlobeControl1.DrawSearchResultExtents(extents);
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
        void DrawTileAllExtents(System.Data.DataSet inds)
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
                if (queryResponse.recordSet != null)
                {
                    for (int i = 0; i < queryResponse.recordSet.Tables[0].Rows.Count; i++)
                    {
                        extents.Add(new System.Drawing.RectangleF(
                        float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentLeftField].ToString()),
                        float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentDownField].ToString()),
                        float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentRightField].ToString()) - float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentLeftField].ToString()),
                        float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentUpField].ToString()) - float.Parse(queryResponse.recordSet.Tables[0].Rows[i][queryPara.extentDownField].ToString())));
                    }
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
            
            //barEditItemEndDate.EditValue = dn.ToString();
            barEditItemEndDate.EditValue = dn.AddHours(23).AddMinutes(59).AddSeconds(59).ToString();
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
        ///保存折线坐标文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSaveLinePoint_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "文本文件(*.txt)|";
            string path = null;
            if (!isRead)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FileName.Contains(".") && sfd.FileName.Substring(sfd.FileName.LastIndexOf("."), sfd.FileName.Length - sfd.FileName.LastIndexOf(".")) == ".txt")
                    {
                        path = sfd.FileName;
                    }
                    else
                    {
                        path = sfd.FileName + ".txt";
                    }
                }
                else
                    return;
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                List<double> latList = new List<double>();
                List<double> lonList = new List<double>();
                if (barEditItemShowLinePoint.EditValue.ToString().Length > 0)
                {
                    //开始写入
                    string pointValue = barEditItemShowLinePoint.EditValue.ToString();
                    string coordinate = pointValue.Replace("\r\n", "");
                    string[] array = coordinate.TrimEnd(';').Split(';');
                    // string[] array = coordinate.Split(';');
                    for (int i = 0; i < array.Length; i++)//i < array.Length - 1;
                    {
                        if (array[i].Length == 0)
                            continue;
                        string[] latlon = array[i].Split(',');
                        latList.Add(Double.Parse(latlon[1]));
                        lonList.Add(Double.Parse(latlon[0]));
                        if (i == array.Length - 1 && latList[i] == latList[i - 1] && lonList[i] == lonList[i - 1])
                            continue;
                        sw.Write(array[i] + ";" + Environment.NewLine);
                    }
                    //关闭流
                    sw.Close();
                    fs.Close();
                    MessageBox.Show("保存成功！");
                    //barEditItemShowLinePoint.EditValue = "";
                    barEditItemMaxLat.EditValue = latList.Max();
                    barEditItemMaxLon.EditValue = lonList.Max();
                    barEditItemMinLon.EditValue = lonList.Min();
                    barEditItemMinLat.EditValue = latList.Min();
                }
                else
                {
                    MessageBox.Show("请先绘制多边形！");
                }
            }
            else
            {
                MessageBox.Show("读取状态下不要保存！");
            }
        }
        /// <summary>
        ///保存坐标文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemLoadSavePoint_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "文本文件(*.txt)|";
            string path = null;
            if (!isRead)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if(sfd.FileName.Contains(".")&&sfd.FileName.Substring(sfd.FileName.LastIndexOf("."), sfd.FileName.Length - sfd.FileName.LastIndexOf(".")) == ".txt")
                    {
                        path = sfd.FileName;
                    }
                    else
                    {
                        path = sfd.FileName + ".txt";
                    }
                }
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                List<double> latList = new List<double>();
                List<double> lonList = new List<double>();
                if (barEditItemShowPoint.EditValue.ToString() != "")
                {
                    //开始写入
                    string pointValue = barEditItemShowPoint.EditValue.ToString();
                    string coordinate = pointValue.Replace("\r\n", "");
                    string[] array = coordinate.TrimEnd(';').Split(';');
                    // string[] array = coordinate.Split(';');
                    for (int i = 0; i < array.Length; i++)//i < array.Length - 1;
                    {
                        string[] latlon = array[i].Split(',');
                        latList.Add(Double.Parse(latlon[1]));
                        lonList.Add(Double.Parse(latlon[0]));
                        sw.Write(array[i] + ";" + Environment.NewLine);
                    }
                    //关闭流
                    sw.Close();
                    fs.Close();
                    MessageBox.Show("保存成功！");
                    //barEditItemShowPoint.EditValue = "";
                    barEditItemMaxLat.EditValue = latList.Max();
                    barEditItemMaxLon.EditValue = lonList.Max();
                    barEditItemMinLon.EditValue = lonList.Min();
                    barEditItemMinLat.EditValue = latList.Min();
                }
                else
                {
                    MessageBox.Show("请先绘制多边形！");
                }
            }
            else
            {
                MessageBox.Show("读取状态下不要保存！");
            }
        }
        public FeatureSet createShp(string fName)
        {

            FeatureSet fs = new FeatureSet(FeatureType.Polygon);
            fs.Projection = ProjectionInfo.FromEpsgCode(4326);
            fs.CoordinateType = CoordinateType.Regular;
            fs.IndexMode = false;
            return fs;
        }
        /// <summary>
        ///读取折线坐标文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemReadLinePoint_ItemClick(object sender, ItemClickEventArgs e)
        {
            isRead = true;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.Filter = "文本文件|*.txt";
            double minLon, maxLon, maxLat, minLat;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fname = ofd.FileName;
                StreamReader sr = new StreamReader(fname, Encoding.UTF8);
                this.mucsearcher.qrstAxGlobeControl1.m_flagDrawPloygonTool = true;
                this.mucsearcher.qrstAxGlobeControl1.UsingDrawPolyLineTool();
                this.mucsearcher.qrstAxGlobeControl1.needRefreshDrawPolyLayer = true;
                List<Coordinate> _polyline = new List<Coordinate>();
                List<string[]> list = new List<string[]>();
                List<double> latList = new List<double>();
                List<double> lonList = new List<double>();
                bool isTrue = true;
                string line;
                string content = string.Empty;
                Shape shp = new Shape(FeatureType.Line);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 0 || !line.Contains(',') || !line.Contains(';'))
                    {
                        isTrue = false;
                        MessageBox.Show("坐标文件选择不正确！");
                        break;
                    }
                    string[] xy = line.TrimEnd(';').Split(',');
                    double x,y;
                    if (Double.TryParse(xy[0], out x) && Double.TryParse(xy[1], out y))
                    {
                        if (x < -180 || x > 180 || y < -90 || y > 90)
                        {
                            isTrue = false;
                            MessageBox.Show("坐标文件选择不正确！");
                            break;
                        }
                        list.Add(xy);
                        isTrue = true;
                        content += line;
                        Coordinate coord = new Coordinate();
                        coord.X = x;
                        coord.Y = y;
                        _polyline.Add(coord);
                    }
                    else
                    {
                        isTrue = false;
                        MessageBox.Show("坐标文件选择不正确！");
                        break;
                    }
                }
                if (isTrue && _polyline.Count > 1)
                {
                    for (int i = 0; i < list.Count - 1; i++)//i < list.Count - 1;
                    {
                        QRST.WorldGlobeTool.Angle res = new QRST.WorldGlobeTool.Angle();
                        res.Degrees = Double.Parse(list[i][0]);
                        lonList.Add(Double.Parse(list[i][0]));
                        QRST.WorldGlobeTool.Angle res1 = new QRST.WorldGlobeTool.Angle();
                        res1.Degrees = Double.Parse(list[i][1]);
                        latList.Add(Double.Parse(list[i][1]));
                        this.mucsearcher.qrstAxGlobeControl1.DrawPoly(res1, res, 1);
                    }
                    QRST.WorldGlobeTool.Angle res2 = new QRST.WorldGlobeTool.Angle();
                    res2.Degrees = Double.Parse(list[list.Count - 1][0]);
                    lonList.Add(Double.Parse(list[list.Count - 1][0]));
                    QRST.WorldGlobeTool.Angle res3 = new QRST.WorldGlobeTool.Angle();
                    res3.Degrees = Double.Parse(list[list.Count - 1][1]);
                    latList.Add(Double.Parse(list[list.Count - 1][1]));
                    this.mucsearcher.qrstAxGlobeControl1.CompletePoly(res3, res2, 1);
                    isDraw = true;
                    shp.AddPart(_polyline, CoordinateType.Regular);
                    
                    this.mucsearcher.qrstAxGlobeControl1.SetViewPosition((latList.Max() + latList.Min()) / 2, (lonList.Max() + lonList.Min()) / 2, 0.0, 7000000, 0.0);
                    testF = new Feature(shp);//把f都被testf替换了 20170228
                    QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = true;
                    QRST_DI_SS_Basis.MetadataQuery.ComplexCondition.QueryGeometry = testF;
                    minLon = testF.Envelope.X;
                    maxLon = minLon + testF.Envelope.Width;
                    maxLat = testF.Envelope.Y;
                    minLat = maxLat - testF.Envelope.Height;

                    barEditItemMaxLat.EditValue = maxLat;
                    barEditItemMaxLon.EditValue = maxLon;
                    barEditItemMinLon.EditValue = minLon;
                    barEditItemMinLat.EditValue = minLat;
                    barEditItemShowLinePoint.EditValue = content;
                }
            }
        }
        /// <summary>
        ///读取坐标文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool isRead = false;
        private void barButtonItemLoadReadPoint_ItemClick(object sender, ItemClickEventArgs e)
        {
            isRead = true;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "文本文件|*.txt";
            double minLon, maxLon, maxLat, minLat;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog.FileName;
                StreamReader sr = new StreamReader(fName, Encoding.UTF8);
                string content = null;
                string line = null;
                //FeatureSet fs = createShp(fName);
                Shape shp = new Shape(FeatureType.Polygon);
                List<Coordinate> _polygon = new List<Coordinate>();
                this.mucsearcher.qrstAxGlobeControl1.m_flagDrawPloygonTool = true;
                this.mucsearcher.qrstAxGlobeControl1.TurnOnOffDrawPloygonTool();
                this.mucsearcher.qrstAxGlobeControl1.needRefreshDrawPolyLayer = true;
                List<string[]> list = new List<string[]>();
                List<double> latList = new List<double>();
                List<double> lonList = new List<double>();
                bool isTrue = true;
                double result;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    count++;
                    if (line.Contains(";") && line.Contains(","))
                    {
                        string[] latlon = line.TrimEnd(';').Split(',');
                        if (Double.TryParse(latlon[0], out result) && Double.TryParse(latlon[1], out result))
                        {
                            if ((Double.Parse(latlon[0]) >= -180) && (Double.Parse(latlon[0]) <= 180) && (Double.Parse(latlon[1]) >= -90) && (Double.Parse(latlon[1]) <= 90))
                            {
                                list.Add(latlon);
                                isTrue = true;
                                content += line;
                            }
                            else
                            {
                                isTrue = false;
                                MessageBox.Show("坐标文件选择不正确！");
                                break;
                            }
                            Coordinate coord = new Coordinate();
                            coord.Y = Convert.ToDouble(latlon[1]);
                            coord.X = Convert.ToDouble(latlon[0]);
                            _polygon.Add(coord);
                        }
                        else
                        {
                            isTrue = false;
                            MessageBox.Show("坐标文件选择不正确！");
                            break;
                        }
                    }
                    else
                    {
                        isTrue = false;
                        MessageBox.Show("坐标文件选择不正确！");
                        break;
                    }
                }
                if (isTrue && count!=0)
                {
                    for (int i = 0; i < list.Count; i++)//i < list.Count - 1;
                    {
                        QRST.WorldGlobeTool.Angle res = new QRST.WorldGlobeTool.Angle();
                        res.Degrees = Double.Parse(list[i][0]);
                        lonList.Add(Double.Parse(list[i][0]));
                        QRST.WorldGlobeTool.Angle res1 = new QRST.WorldGlobeTool.Angle();
                        res1.Degrees = Double.Parse(list[i][1]);
                        latList.Add(Double.Parse(list[i][1]));
                        this.mucsearcher.qrstAxGlobeControl1.DrawPoly(res1, res, 0);
                    }
                    QRST.WorldGlobeTool.Angle res2 = new QRST.WorldGlobeTool.Angle();
                    res2.Degrees = Double.Parse(list[list.Count - 1][0]);
                    lonList.Add(Double.Parse(list[list.Count - 1][0]));
                    QRST.WorldGlobeTool.Angle res3 = new QRST.WorldGlobeTool.Angle();
                    res3.Degrees = Double.Parse(list[list.Count - 1][1]);
                    latList.Add(Double.Parse(list[list.Count - 1][1]));
                    this.mucsearcher.qrstAxGlobeControl1.CompletePoly(res3, res2, 0);
                    isDraw = true;
                    shp.AddPart(_polygon, CoordinateType.Regular);
                    //Feature feature = new Feature(shp);
                    //if (feature != null)
                    //{
                    //    fs.Features.Add(feature);
                    //}
                    //fs.InvalidateVertices();
                    //fs.UpdateExtent();

                    //fs.Save();
                    //this.mucsearcher.qrstAxGlobeControl1.DrawShapeFile(fs.FilePath, Color.Red, 1.0f, 5700000, 0);
                    //this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.Goto(sumlat/list.Count, sumlon/list.Count, 7000000);
                    this.mucsearcher.qrstAxGlobeControl1.SetViewPosition((latList.Max() + latList.Min()) / 2, (lonList.Max() + lonList.Min()) / 2, 0.0, 7000000, 0.0);
                    //MessageBox.Show("加载成功！");
                   // IFeature f = new Feature(shp);
                    testF = new Feature(shp);//把f都被testf替换了 20170228
                    QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = true;
                    QRST_DI_SS_Basis.MetadataQuery.ComplexCondition.QueryGeometry = testF;
                    minLon = testF.Envelope.X;
                    maxLon = minLon + testF.Envelope.Width;
                    maxLat = testF.Envelope.Y;
                    minLat = maxLat - testF.Envelope.Height;

                    barEditItemMaxLat.EditValue = maxLat;
                    barEditItemMaxLon.EditValue = maxLon;
                    barEditItemMinLon.EditValue = minLon;
                    barEditItemMinLat.EditValue = minLat;
                    barEditItemShowPoint.EditValue = content;
                }

            }
        }

        /// <summary>
        ///绘制多边形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool isDraw = false;
        private void barButtonItemLoadDrawPolygon_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.barEditItemShowPoint.EditValue = "";
            isRead = false;
            if (this.mucsearcher.Is3DViewer)
            {
              //  this.mucsearcher.qrstAxGlobeControl1.UsingDrawRectangleTool();
                this.mucsearcher.qrstAxGlobeControl1.TurnOnOffDrawPloygonTool();
            }
            else
            {
                //this.mucsearcher.qrstAxGlobeControl1.flag = true;
                this.mucsearcher.uc2DSearcher1.enabledDrawPolygon();
                this.mucsearcher.uc2DSearcher1.drawPolygonCompletedEvent += new Action(qrstAxGlobeControl1_OnDrawPolygonCompeleted);
            }
            isDraw = true;
        }
        List<string> polygonList = new List<string>();
        void qrstAxGlobeControl1_OnDrawPolygonCompeleted()
        {
            if (this.Cursor != Cursors.Default)
            {
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        ///绘制折线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDrawPolyline_ItemClick(object sender, ItemClickEventArgs e) 
        {
            this.barEditItemShowLinePoint.EditValue = "";
            isRead = false;
            if (this.mucsearcher.Is3DViewer)
            {
                this.mucsearcher.qrstAxGlobeControl1.UsingDrawPolyLineTool();
            }
            else
            {
                //this.mucsearcher.uc2DSearcher1.enabledDrawPolygon();
                //this.mucsearcher.uc2DSearcher1.drawPolygonCompletedEvent += new Action(qrstAxGlobeControl1_OnDrawPolygonCompeleted);
            }
            isDraw = true;
        }

        /// <summary>
        /// 加载坐标文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemLoadPositionFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "(坐标文件.txt,多边形文件.shp)|*.txt;*.shp";
            IFeatureSet ftset;
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    double maxLat = 90, maxLon = 180, minLat = -90, minLon = -180;
                    if (Path.GetExtension(fileDlg.FileName).ToLower() == ".txt")
                    {
                        FileStream fs = new FileStream(fileDlg.FileName, FileMode.Open);
                        StreamReader sr = new StreamReader(fs);

                        int recordCount = int.Parse(sr.ReadLine());//输入的字符串不正确
                        Point3d[] posArr = new Point3d[recordCount];

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
                    }
                    else if (Path.GetExtension(fileDlg.FileName).ToLower() == ".shp")
                    {
                        DialogResult dr = MessageBox.Show("是否简化多边形以提高检索效率？", "提示", MessageBoxButtons.YesNo);
                        ftset = Shapefile.Open(fileDlg.FileName);

                       // IFeature f = ftset.Features[0];   //20170228 一下几行的f都被testF替换了
                   
                        testF = ftset.Features[0];
                        if (dr == DialogResult.Yes)
                        {
                            IList<Coordinate> coords = QRST_DI_TS_Basis.Search.ShapeSimplifier.Simplifier(testF.BasicGeometry.Coordinates);
                            testF = new Feature(DotSpatial.Topology.FeatureType.Polygon, coords);
                        }

                        QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = true;
                        QRST_DI_SS_Basis.MetadataQuery.ComplexCondition.QueryGeometry = testF;
                        minLon = testF.Envelope.X;
                        maxLon = minLon + testF.Envelope.Width;
                        maxLat = testF.Envelope.Y;
                        minLat = maxLat - testF.Envelope.Height;

                    }
                    barEditItemMaxLat.EditValue = maxLat;
                    barEditItemMaxLon.EditValue = maxLon;
                    barEditItemMinLon.EditValue = minLon;
                    barEditItemMinLat.EditValue = minLat;
                    //  this.mucsearcher.qrstAxGlobeControl1.DrawPolygenLayer(new Point3d(maxLon, maxLat, 0), new Point3d(minLon, minLat, 0));
                    //this.mucsearcher.qrstAxGlobeControl1.DrawPolygenLayer(posArr);

                    barButtonItemLoadPositionFile.Tag = fileDlg.FileName;
                    barStaticPositionFileName.Caption = Path.GetFileName(fileDlg.FileName);
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
        IFeature testF ;
        //获取省的feature对象并确定市的列表
        public void getProvinceFt()
        {
            cityLst = new List<string>();
            List<string[]> coodList = new List<string[]>();
            foreach (Feature f in ProVftset.Features)
            {
                if (f.DataRow["Name"].ToString() == barEditItemProvince.EditValue.ToString())
                {
                    ProAdcode = f.DataRow["AdminCode"].ToString();
                    IFeature Ifeature = f;
                    testF = f;
                    getSpatialParamFromFt(f);
                    DrawArea(Ifeature);
                    break;
                }
            }
            foreach (Feature ft in Cityftset.Features)
            {
                
                if (ft.DataRow["ADMINCODE"].ToString().Substring(0, 2) == ProAdcode.Substring(0, 2))
                {
                    if (ft.DataRow["NAME"].ToString() != "")
                    {
                        if (!repositoryItemComboBoxCity.Items.Contains(ft.DataRow["Name"].ToString()))
                        {
                            cityLst.Add(ft.DataRow["Name"].ToString());
                        }
                    }
                }
            }
            repositoryItemComboBoxCity.Items.AddRange(cityLst);
        }
        //获取矢量文件的feature对象
        IFeatureSet ShpfileFt;
        public void getShpfileFeature()
        {
            foreach (Feature f in ShpfileFt.Features)
            {
                IFeature Ifeature = f;
                testF = f;
                getSpatialParamFromFt(f);
                DrawArea(Ifeature);
                break;
            }
        
        }
        //bool isMulPly = false;
        public void DrawArea(IFeature ft)
        {
            Shape shape = ft.ToShape();
            if (shape.Range.Parts == null)
            {
                this.mucsearcher.qrstAxGlobeControl1.m_flagDrawPloygonTool = true;
                this.mucsearcher.qrstAxGlobeControl1.TurnOnOffDrawPloygonTool();
                this.mucsearcher.qrstAxGlobeControl1.needRefreshDrawPolyLayer = true;
                DrawArea(shape.Vertices);
                this.mucsearcher.qrstAxGlobeControl1.CompletePoly(new QRST.WorldGlobeTool.Angle(), new QRST.WorldGlobeTool.Angle(), 0);
                this.mucsearcher.qrstAxGlobeControl1.SetViewPosition(ft.BasicGeometry.Envelope.Center().Y, ft.BasicGeometry.Envelope.Center().X, 0.0, 2000000, 0.0);
            }
            else
            {
                this.mucsearcher.qrstAxGlobeControl1.m_flagDrawPloygonTool = true;
                this.mucsearcher.qrstAxGlobeControl1.TurnOnOffDrawPloygonTool();
                this.mucsearcher.qrstAxGlobeControl1.needRefreshDrawPolyLayer = true;
                foreach (PartRange pr in shape.Range.Parts)
                {
                    double[] vts = new double[pr.NumVertices * 2];
                    int startidx = pr.StartIndex * 2;
                    for (int i = 0; i < pr.NumVertices * 2; i++)
                    {
                        vts[i] = pr.Vertices[startidx + i];
                    }
                    DrawArea(vts);
                }

                this.mucsearcher.qrstAxGlobeControl1.CompletePoly(new QRST.WorldGlobeTool.Angle(), new QRST.WorldGlobeTool.Angle(), 0);
                this.mucsearcher.qrstAxGlobeControl1.SetViewPosition(ft.BasicGeometry.Envelope.Center().Y, ft.BasicGeometry.Envelope.Center().X, 0.0, 2000000, 0.0);

            }

            //if (ft.BasicGeometry is MultiPolygon)
            //{
            //    MultiPolygon mplg = ft.BasicGeometry as MultiPolygon;
            //    int count = 0;
            //    foreach (IGeometry g in mplg.Geometries)
            //    {
            //        count++;
            //        DotSpatial.Topology.Polygon plg = g as DotSpatial.Topology.Polygon;
            //        if (count > 1)
            //        {
            //            isMulPly = true;
            //        }
            //        DrawArea(plg.Coordinates.ToList());
            //    }
            //    isMulPly = false;
            //    count = 0;
            //    this.mucsearcher.qrstAxGlobeControl1.CompletePoly(new QRST.WorldGlobeTool.Angle(), new QRST.WorldGlobeTool.Angle());
            //    this.mucsearcher.qrstAxGlobeControl1.SetViewPosition(mplg.Envelope.Center().Y, mplg.Envelope.Center().X, 0.0, 2000000, 0.0);
            //}
            //else if (ft.BasicGeometry is DotSpatial.Topology.Polygon)
            //{
            //    DotSpatial.Topology.Polygon plg = ft.BasicGeometry as DotSpatial.Topology.Polygon;
            //    DrawArea(plg.Coordinates.ToList());
            //    this.mucsearcher.qrstAxGlobeControl1.CompletePoly(new QRST.WorldGlobeTool.Angle(), new QRST.WorldGlobeTool.Angle());
            //    this.mucsearcher.qrstAxGlobeControl1.SetViewPosition(plg.Envelope.Center().Y, plg.Envelope.Center().X, 0.0, 2000000, 0.0);
            //}
        }

        public void DrawArea(double[] list)
        {
            for (int i = 0; i < list.Length - 1; i++)
            {
                QRST.WorldGlobeTool.Angle res = new QRST.WorldGlobeTool.Angle();
                res.Degrees = list[i];      //x
                QRST.WorldGlobeTool.Angle res1 = new QRST.WorldGlobeTool.Angle();
                i++;    //y
                res1.Degrees = list[i];
                this.mucsearcher.qrstAxGlobeControl1.DrawPoly(res1, res, 0);
            }

            QRST.WorldGlobeTool.Angle res2 = new QRST.WorldGlobeTool.Angle();
            res2.Degrees = list[list.Length - 2];
            QRST.WorldGlobeTool.Angle res3 = new QRST.WorldGlobeTool.Angle();
            res3.Degrees = list[list.Length - 1];
            this.mucsearcher.qrstAxGlobeControl1.completePart(res3, res2);
        }
        /// <summary>
        /// 省选择变化时加载市列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        List<string> cityLst;
        string ProAdcode ,code_2= null;
        double minLon, maxLon, maxLat, minLat;
        bool isProtoCity = false;
        private void barEditItemProvince_EditValueChanged(object sender, EventArgs e)
        {
            if (barEditItemProvince.EditValue == null || barEditItemProvince.EditValue.ToString() == "")
            {
                repositoryItemComboBoxCity.Items.Clear();
                barEditItemUrban.EditValue = "";
                repositoryItemComboBoxCounty.Items.Clear();
                barEditItemCounty.EditValue = "";
                repositoryItemComboBoxProvince.Items.Clear();
                List<string> provinceLst = new List<string>();
                foreach (Feature ft in ProVftset.Features)
                {
                    string ftName = ft.DataRow["Name"].ToString();
                    provinceLst.Add(ftName);
                }
                repositoryItemComboBoxProvince.Items.AddRange(provinceLst);
                barEditItemProvince.EditValue = provinceLst[0];
            }
            if (Regex.Matches(barEditItemProvince.EditValue.ToString(), "[a-zA-Z]").Count == 0 && !isProChanged)
            {
                isProtoCity = true;
                repositoryItemComboBoxCity.Items.Clear();
                barEditItemUrban.EditValue = "";
                repositoryItemComboBoxCounty.Items.Clear();
                barEditItemCounty.EditValue = "";
                if (!isGetProFt)
                {
                    getProvinceFt();
                }
                else
                {
                    isGetProFt = false;
                }
            }
            else if (Regex.Matches(barEditItemProvince.EditValue.ToString(), "[a-zA-Z]").Count != 0 && !isProChanged)
            {
                List<string> pList = new List<string>();
                foreach (Feature f in ProVftset.Features)
                {
                    string str = f.DataRow["PY"].ToString();
                    string s = str[0].ToString();
                    for (int j = 0; j < str.Length - 1; j++)
                    {
                        if (str[j].ToString() == " ")
                        {
                            s += str[j + 1];
                        }
                    }
                    if (s.ToLower().Contains(barEditItemProvince.EditValue.ToString())
                        || s.Contains(barEditItemProvince.EditValue.ToString()))
                    {
                        pList.Add(f.DataRow["Name"].ToString());
                    }
                }
                repositoryItemComboBoxProvince.Items.Clear();
                repositoryItemComboBoxProvince.Items.AddRange(pList);
                repositoryItemComboBoxCity.Items.Clear();
                barEditItemUrban.EditValue = "";
                repositoryItemComboBoxCounty.Items.Clear();
                barEditItemCounty.EditValue = "";
                if (repositoryItemComboBoxProvince.Items.Count > 0)
                {
                    barEditItemProvince.EditValue = repositoryItemComboBoxProvince.Items[0].ToString();
                }
                else
                {
                    MessageBox.Show("未找到相应的省！");
                    barEditItemProvince.EditValue = "";
                }
            }
        }

        private void barEditItemUrban_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 根据Feature对象获取空间条件
        /// </summary>
        /// <param name="ft"></param>
        public void getSpatialParamFromFt(IFeature ft)
        {
            //简化多边形以提高检索效率
            IList<Coordinate> coords = QRST_DI_TS_Basis.Search.ShapeSimplifier.Simplifier(ft.BasicGeometry.Coordinates);
            ft = new Feature(DotSpatial.Topology.FeatureType.Polygon, coords);

            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = true;
            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition.QueryGeometry = ft;
            minLon = ft.Envelope.X;
            maxLon = minLon + ft.Envelope.Width;
            maxLat = ft.Envelope.Y;
            minLat = maxLat - ft.Envelope.Height;
            barEditItemMaxLat.EditValue = maxLat;
            barEditItemMaxLon.EditValue = maxLon;
            barEditItemMinLon.EditValue = minLon;
            barEditItemMinLat.EditValue = minLat;
        }
        public IFeature getIFeature()
        {
            IFeature Ifeature = null;
            foreach (Feature f in Cityftset.Features)
            {
                if (f.DataRow["NAME"] == barEditItemUrban.EditValue)
                {
                    ProAdcode = f.DataRow["ADMINCODE"].ToString();
                    Ifeature = f;
                    break;
                }
            }
            return Ifeature;
        }

        public void CityToCounty()
        {
            List<string> countyLst = new List<string>();
            foreach (Feature f in Cityftset.Features)
            {
                if (f.DataRow["NAME"] == barEditItemUrban.EditValue)
                {
                    if (f.DataRow["CODE_2"].ToString() != "0")
                    {
                        code_2 = f.DataRow["CODE_2"].ToString();
                    }
                    ProAdcode = f.DataRow["ADMINCODE"].ToString();
                    break;
                }
            }
            foreach (Feature ft in Countyftset.Features)
            {
                if (ft.DataRow["AdminCode"].ToString().Substring(0, 4) == ProAdcode.Substring(0, 4))
                {
                    countyLst.Add(ft.DataRow["Name"].ToString());
                }
                if (code_2 != null)
                {
                    if (ft.DataRow["AdminCode"].ToString().Substring(0, 4) == code_2)
                    {
                        countyLst.Add(ft.DataRow["Name"].ToString());
                    }
                }
            }
            repositoryItemComboBoxCounty.Items.AddRange(countyLst);
        }
        bool isProChanged = false;
        public void CityToPro()
        {
            repositoryItemComboBoxProvince.Items.Clear();
            string proCode = null;
            foreach (Feature f in Cityftset.Features)
            {
                if (f.DataRow["NAME"] == barEditItemUrban.EditValue)
                {
                    proCode = f.DataRow["ADMINCODE"].ToString().Substring(0, 2);
                    break;
                }
            }
            List<string> provinceLst = new List<string>();
            foreach (Feature ft in ProVftset.Features)
            {
                string ftName = ft.DataRow["Name"].ToString();
                provinceLst.Add(ftName);
            }
            repositoryItemComboBoxProvince.Items.AddRange(provinceLst);
            foreach (Feature f in ProVftset.Features)
            {
                if (f.DataRow["AdminCode"].ToString().Substring(0, 2) == proCode)
                {
                    isProChanged = true;
                    barEditItemProvince.EditValue = f.DataRow["Name"].ToString();
                    isProChanged = false;
                    break;
                }
            }
        }
        bool isUrbanChange = false;
        bool isUrbanLet = false;
        bool isGetProFt = false;
        bool isUrbantoCounty = false;
        private void barEditItemUrban_EditValueChanged(object sender, EventArgs e)
        {
            if (barEditItemUrban.EditValue == null || barEditItemUrban.EditValue.ToString() == "")
            {
                if (barEditItemProvince.EditValue != "" && !countyToCity)
                {
                    getProvinceFt();
                    isGetProFt = true;
                }
            }
            if (Regex.Matches(barEditItemUrban.EditValue.ToString(), "[a-zA-Z]").Count == 0 && !isUrbanChange && !isCountyChange && barEditItemUrban.EditValue.ToString() != "")
            {
                isUrbantoCounty = true;
                repositoryItemComboBoxCounty.Items.Clear();
                barEditItemCounty.EditValue = "";
                
                if (!isProtoCity)
                {
                    CityToPro();
                }
                else
                {
                    isProtoCity = false;
                }
                if (!countyToCity)
                {
                    CityToCounty();
                    IFeature ft = getIFeature();
                    testF = ft;
                    getSpatialParamFromFt(ft);
                    DrawArea(ft);
                    
                }
                else
                {
                    CityToCounty();
                    countyToCity = false;
                }
            }
            else if (Regex.Matches(barEditItemUrban.EditValue.ToString(), "[a-zA-Z]").Count != 0 && !isUrbanChange)
            {
                isUrbanLet = true;
                List<string> pList = new List<string>();
                foreach (Feature f in Cityftset.Features)
                {
                    string str = f.DataRow["PY"].ToString();
                    if (str.ToLower().Contains(barEditItemUrban.EditValue.ToString()) || str.Contains(barEditItemUrban.EditValue.ToString()))
                    {
                        pList.Add(f.DataRow["Name"].ToString());
                    }
                }
                barEditItemCounty.EditValue = "";
                repositoryItemComboBoxCounty.Items.Clear();
                repositoryItemComboBoxCity.Items.Clear();
                repositoryItemComboBoxCity.Items.AddRange(pList);
                if (repositoryItemComboBoxCity.Items.Count > 0)
                {
                    isUrbanChange = true;
                    barEditItemUrban.EditValue = repositoryItemComboBoxCity.Items[0].ToString();
                    isUrbanChange = false;
                }
                else
                {
                    MessageBox.Show("未找到相应的市！");
                    getProvinceFt();
                    barEditItemUrban.EditValue = "";
                }
                isUrbanLet = false;
            }
            else if (isUrbanChange)
            {
                CityToCounty();
                IFeature ft = getIFeature();
                getSpatialParamFromFt(ft);

                DrawArea(ft);
                CityToPro();
            }
            else if (isCountyChange)
            {
                CityToPro();
            }
        }

        /// <summary>
        /// 县变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void getCountyFt()
        {
            //repositoryItemComboBoxCity.Items.Clear();
            //barEditItemUrban.EditValue = "";
            foreach (Feature f in Countyftset.Features)
            {
                if (f.DataRow["Name"] == barEditItemCounty.EditValue)
                {
                    IFeature Ifeature = f;
                    testF = f;
                    getSpatialParamFromFt(Ifeature);

                    DrawArea(Ifeature);
                    
                    break;
                }
            }
        }
        bool isCountyChange = false;
        bool countyToCity = false;
        //由县到市
        public void CountyToCity()
        {
            countyToCity = true;
            repositoryItemComboBoxCity.Items.Clear();
            barEditItemUrban.EditValue = "";
            string cityCode = null, proCode = null;
            foreach (Feature f in Countyftset.Features)
            {
                if (f.DataRow["Name"] == barEditItemCounty.EditValue)
                {
                    cityCode = f.DataRow["AdminCode"].ToString().Substring(0, 4);
                    break;
                }
            }
            //int count = 0;
             List<string> cityLst = new List<string>();
             foreach (Feature f in Cityftset.Features)
             {
                 //count++;
                 if (f.DataRow["ADMINCODE"].ToString().Substring(0, 2) == cityCode.Substring(0, 2))
                 {
                     cityLst.Add(f.DataRow["NAME"].ToString());
                 }

                 if (f.DataRow["CODE_1"].ToString() == cityCode || f.DataRow["CODE_2"].ToString() == cityCode)
                 {
                     isCountyChange = true;
                     barEditItemUrban.EditValue = f.DataRow["NAME"].ToString();
                     isCountyChange = false;
                 }
             }
            //if (count == Cityftset.Features.Count)
            //{
                repositoryItemComboBoxCity.Items.Clear();
                foreach (Feature f in ProVftset.Features)
                {
                    if (f.DataRow["AdminCode"].ToString().Substring(0, 2) == cityCode.Substring(0, 2))
                    {
                        isProChanged = true;
                        barEditItemProvince.EditValue = f.DataRow["Name"].ToString();
                        isProChanged = false;
                        break;
                    }
                }
            //}
            repositoryItemComboBoxCity.Items.AddRange(cityLst);
        }
        private void barEditItemCounty_EditValueChanged(object sender, EventArgs e)
        {
            if (barEditItemCounty.EditValue == null || barEditItemCounty.EditValue.ToString() == "")
            {
                if (barEditItemUrban.EditValue.ToString() != "")
                {
                    if (!isUrbanLet)
                    {
                        getSpatialParamFromFt(getIFeature());
                    }
                    //}
                    //catch (Exception)
                    //{
                    //}
                }
            }
            if (barEditItemCounty.EditValue != null && barEditItemCounty.EditValue.ToString() != "")
            {
                if (Regex.Matches(barEditItemCounty.EditValue.ToString(), "[a-zA-Z]").Count == 0)
                {
                    getCountyFt();
                    if (!isUrbantoCounty)
                    {
                        CountyToCity();
                    }
                    else
                    {
                        isUrbantoCounty = false;
                    }
                }
                else
                {
                    List<string> pList = new List<string>();
                    foreach (Feature f in Countyftset.Features)
                    {
                        string str = f.DataRow["PY"].ToString();
                        string s = str[0].ToString();
                        for (int j = 0; j < str.Length - 1; j++)
                        {
                            if (str[j].ToString() == " ")
                            {
                                s += str[j + 1];
                            }
                        }
                        if (s.ToLower().Contains(barEditItemCounty.EditValue.ToString()) || s.Contains(barEditItemCounty.EditValue.ToString()))
                        {
                            pList.Add(f.DataRow["Name"].ToString());
                        }
                    }
                    repositoryItemComboBoxCounty.Items.Clear();
                    repositoryItemComboBoxCounty.Items.AddRange(pList);
                    if (repositoryItemComboBoxCounty.Items.Count > 0)
                    {
                        barEditItemCounty.EditValue = repositoryItemComboBoxCounty.Items[0].ToString();
                    }
                    else
                    {
                        MessageBox.Show("未找到相应的县！");
                        IFeature ft = getIFeature();
                        DrawArea(ft);
                        barEditItemCounty.EditValue = "";
                    }
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
            this.barButtonItem2D.Enabled = false;
            this.barButtonItem3D.Enabled = true;
            this.barButtonItemRaster.Enabled = false;
            this.mucsearcher.uc2DSearcher1.showRaster();
        }
        /// <summary>
        /// 将时间转换成INT型20170308
        /// </summary>
        /// 是把年月日时分秒的符号去掉（1997/11/22 23:59:59转化为19971122）
        /// <param name="times"></param>
        /// <returns></returns>
        private int parseTimeToInt(DateTime times) 
        {
            int resultTime = 0;
            try
            {
               resultTime = Convert.ToInt32(times.ToString("yyyyMMdd")); 
            }
            catch (Exception)
            {
                resultTime = 0; 
            }
            return resultTime;
        }
        public IList<Coordinate> coords = new List<Coordinate>();
        /// <summary>
        /// 20170308 
        /// 通过所选区域的坐标点，然后通过坐标点获取string类型点串参数
        /// </summary>
        /// <param name="coord"></param>
        public string getCoordstr(IList<Coordinate> coords)
        {
            if (coords == null)
            {
                coordstr = "-180,-90;-180,90;180,90;180,-90";
            }
            else if (coords.Equals(""))
            {
                coordstr = "-180,-90;-180,90;180,90;180,-90";
            }
            else
            {
                IList<string> tempList = new List<string>();
                foreach (Coordinate model in coords)
                {
                    string temp = "" + model.X + "," + model.Y;
                    tempList.Add(temp);

                }
                coordstr = string.Join(";", tempList.ToArray());

            }
            return coordstr;
        }
        string row = null;
        string col = null;
        /// <summary>
        /// 20170313
        /// 获取把某一区域全覆盖的瓦片数
        /// 调用服务SearImgTileCountBatch_coordsStr
        /// </summary>
        /// <param name="paras"></param>
        private void SerSearImgTileCountBatch(string[] paras)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            ds = _tileClient.SearImgTileCountBatch_coordsStr(paras[0], paras[1], paras[2], paras[3], Convert.ToInt32(paras[4]), Convert.ToInt32(paras[5]));
            ////遍历一个表多行多列
            //foreach (DataRow mDr in ds.Tables[0].Rows)
            //{
            //    foreach (DataColumn mDc in ds.Tables[0].Columns)
            //    {
            //        Console.WriteLine("{0},{1}", mDc.ColumnName, mDr[mDc].ToString());
                  
            //    }
            //}

            queryResponse = new QRST_DI_SS_Basis.MetadataQuery.QueryResponse();
            queryResponse.recordSet = ds;
            DrawSpacialExtent();
            if (this.mucsearcher.Is3DViewer)
            {
                SetGlobeToExtent();
            }
            else
            {
                this.mucsearcher.uc2DSearcher1.zoomToDefault();
            } 
        }

        /// <summary>
        /// 20170316
        /// 调用服务SearImgTileBatch_coordsStr，获取把某一区域全覆盖的最新时间的瓦片组合
        /// </summary>
        /// <param name="paras"></param>
        public void serSearImgTileBatch(string[] paras)
        {
            DataTable resultTable = new DataTable();
            resultTable.Clear();
            System.Data.DataSet ds = new System.Data.DataSet();

            ds = _tileClient.SearImgTileBatch_coordsStr(paras[0], paras[1], paras[2], paras[3], Convert.ToInt32(paras[4]), Convert.ToInt32(paras[5]), selectedValue);
            queryResponse = new QRST_DI_SS_Basis.MetadataQuery.QueryResponse();
            queryResponse.recordSet = ds;
            if (ds != null&&ds.Tables.Count> 0)
            {
                try
                {
                    //把dataset的第一行数据删除
                    ds.Tables[0].Rows[0].Delete();
                    ds.Tables[0].AcceptChanges();
                    resultTable = ds.Tables[0];   
                    Lyrtable = resultTable;
                }
                catch (Exception)
                {
                    
                    throw;
                }
               
            }
            if (resultTable == null || resultTable.Rows.Count == 0)
            {
                Lyrtable = null;
                MessageBox.Show("没有查找到数据!");
                return;
            }
            ////服务获取的tileFileName列中含有“#FFFF”等字样，把该列的所有该标记删除
            //foreach (DataRow mDr in resultTable.Rows)
            //{
            //    string tileSerFileName = mDr["TileFileName"].ToString();
            //    if (tileSerFileName.Contains("#"))
            //    {
            //        string tileFilename = tileSerFileName.Substring(0, tileSerFileName.IndexOf("#")) + tileSerFileName.Substring(tileSerFileName.IndexOf("#") + 5);
            //        mDr["TileFileName"] = tileFilename;  
            //    }
                
            //}

            //获取页面信息
            CtrlPage ctrlPage = mucdetail.GetCtrlPage();
            ctrlPage.SetPageSize(GetPageSize());
            int recordCount = resultTable.Rows.Count;
            ctrlPage.RefreshPage(recordCount);

            mucdetail.queryPara = queryPara;
            mucdetail.selectedQueryObj = selectedQueryObj;
            mucdetail.setGridControl(resultTable);

        }
        /// <summary>
        /// 20170322
        /// 获取把某一区域全覆盖的产品切片数
        /// 调用服务SearProdTileCountBatch_coordsStr
        /// </summary>
        /// <param name="prodctParas"></param>
        private void SerSearProdTileCountBatch(string[] prodctParas) 
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            ds = _tileClient.SearProdTileCountBatch_coordsStr(prodctParas[0], prodctParas[1], prodctParas[2], Convert.ToInt32(prodctParas[3]), Convert.ToInt32(prodctParas[4]));
            ////遍历一个表多行多列
            //foreach (DataRow mDr in ds.Tables[0].Rows)
            //{
            //    foreach (DataColumn mDc in ds.Tables[0].Columns)
            //    {
            //        Console.WriteLine("{0},{1}", mDc.ColumnName, mDr[mDc].ToString());

            //    }
            //}
            queryResponse = new QRST_DI_SS_Basis.MetadataQuery.QueryResponse();
            queryResponse.recordSet = ds;
            DrawSpacialExtent();
            if (this.mucsearcher.Is3DViewer)
            {
                SetGlobeToExtent();
            }
            else
            {
                this.mucsearcher.uc2DSearcher1.zoomToDefault();
            } 
        }
        /// <summary>
        /// 20170322
        /// 调用服务SearProdTileBatch_coordsStr，
        /// 获取把某一区域全覆盖的最新时间的瓦片组合
        /// </summary>
        /// <param name="paras"></param>
        public void serSearProdTileBatch(string[] prodctParas)
        {
            DataTable resultTable = new DataTable();
            resultTable.Clear();
            System.Data.DataSet ds = new System.Data.DataSet();

            ds = _tileClient.SearProdTileBatch_coordsStr(prodctParas[0], prodctParas[1], prodctParas[2], Convert.ToInt32(prodctParas[3]), Convert.ToInt32(prodctParas[4]), selectedValue);
            queryResponse = new QRST_DI_SS_Basis.MetadataQuery.QueryResponse();
            queryResponse.recordSet = ds;
            if (ds != null && ds.Tables.Count > 0)
            {
                try
                {
                    //把dataset的第一行数据删除
                    ds.Tables[0].Rows[0].Delete();
                    ds.Tables[0].AcceptChanges();
                    resultTable = ds.Tables[0];

                }
                catch (Exception)
                {

                    throw;
                }

            }
            if (resultTable == null || resultTable.Rows.Count == 0)
            {
                MessageBox.Show("没有查找到数据!");
                return;
            }
            ////服务获取的tileFileName列中含有“#FFFF”等字样，把该列的所有该标记删除
            //foreach (DataRow mDr in resultTable.Rows)
            //{
            //    string tileSerFileName = mDr["TileFileName"].ToString();
            //    if (tileSerFileName.Contains("#"))
            //    {
            //        string tileFilename = tileSerFileName.Substring(0, tileSerFileName.IndexOf("#")) + tileSerFileName.Substring(tileSerFileName.IndexOf("#") + 5);
            //        mDr["TileFileName"] = tileFilename;
            //    }
            //}      

            //获取页面信息
            CtrlPage ctrlPage = mucdetail.GetCtrlPage();
            ctrlPage.SetPageSize(GetPageSize());
            int recordCount = resultTable.Rows.Count;
            ctrlPage.RefreshPage(recordCount);

            mucdetail.queryPara = queryPara;
            mucdetail.selectedQueryObj = selectedQueryObj;
            mucdetail.setGridControl(resultTable);

        }

        public static bool clickFlag = false;   //判断点击的是否是全覆盖按钮
        List<string> list = new List<string>();
        DataTable sourceDt = new DataTable();
        DataTable resultTable = new DataTable();//20170321
        DataTable resultTable1 = new DataTable();
        /// <summary>
        /// 一次全覆盖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clickFlag = true;
            if (this.rpgModuleSearch.Visible)
            {
                ClearExtentLyr();
                this.rpgModule.Search();
                return;
            }
            if (barEditItemBeginDate.EditValue != null && barEditItemEndDate.EditValue != null)
            {
                startTime = DateTime.Parse(barEditItemBeginDate.EditValue.ToString());
                endTime = DateTime.Parse(barEditItemEndDate.EditValue.ToString());
            }
            DateTime nowTime = DateTime.Now;
            TimeSpan BettownTime = nowTime - lastSearchTime;
            if (BettownTime.Days > 0 || BettownTime.Hours > 0 || BettownTime.Minutes > 0 || BettownTime.Seconds > 3 || queryNum == 0)
            {
                lastSearchTime = nowTime;
                queryNum++;
                fullCoverageQuery();
                isDraw = false;
            }
            else
            {
                MessageBox.Show("您查询过于频繁，请稍后再试！");
            }
        }
        string[] paras;
        string[] prodctParas;

        /// <summary>
        /// 全覆盖检索（包括瓦片数据和原始数据的）
        /// </summary>
        private void fullCoverageQuery() 
        {
            ClearExtentLyr();
            if (mucdetail == null)
            {
                mucdetail = ((mucDetailViewer)MSUserInterface.listMSUI[1].uiMainUC);
                mucdetail.VisibleChanged += new EventHandler(mucdetail_VisibleChanged);
            }
            //清空gridViewMain中的数据
            //mucdetail.ClearGridView();
            ////收集界面上的查询条件，构造查询参数对象queryPara，并构造ComplexCondition,调用IQuery接口完成数据查询
            Check();
            mucdetail.gridViewMain.RowHeight = -1;
            //queryPara = null;
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))   //切片数据查询
            {
                //清空gridView1中的数据
                mucdetail.ClearGridView();
                mucdetail.ClearGridView1();

                queryPara = new RasterQueryPara();
                queryPara.QRST_CODE = "TileFileName";
                if (selectedQueryObj.NAME.Equals("规格化影像数据") || selectedQueryObj.NAME.Equals("规格化影像控制数据"))
                {
                   SetTileFullQueryPara();
                   //if (sqliteclient == null)
                   //{
                   //    sqliteclient = new localhostSqlite.Service();

                   //}
                   paras = new string[6];
                    paras[0] = sat;
                    paras[1] = sensor;
                    paras[2] = tileLevel;
                    paras[3] = coordstr;
                    paras[4] = "" + startdate;
                    paras[5] = "" + enddate;
                    //判断是否是全覆盖检索后，第一次进入该页面。
                    mucdetail.isFirst = true;
                    //设置多出现一个panel
                    mucdetail.setTileSingleBothPanel();
                    //调用服务SearImgTileCountBatch_coordsStr，获取把某一区域全覆盖的瓦片数及其行列号，并将其画在三维球上
                    SerSearImgTileCountBatch(paras);                    
                    DateTime dtTask1 = new DateTime();
                    dtTask1 = DateTime.Now;
                    //调用服务SearImgTileBatch_coordsStr，获取把某一区域全覆盖的最新时间的瓦片组合
                    serSearImgTileBatch(paras);

                    DateTime dtTask2 = DateTime.Now;
                    TimeSpan dttotal = dtTask2 - dtTask1;
                    //List<Task> tasks = new List<Task>();
                    //Task tk1 = new Task(o => SerSearImgTileCountBatch((string[])o), paras);
                    //tk1.Start();
                    //Task tk2 = new Task(o => serSearImgTileBatch((string[])o), paras);
                    //tk2.Start();
                    //tasks.Add(tk1);
                    //tasks.Add(tk2);
                    //WaitingForTasks(tasks, 15000);
                }
                else
                {
                    queryPara = new RasterQueryPara();
                    queryPara.QRST_CODE = "TileFileName";
                    if (selectedQueryObj.NAME.Equals("规格化产品数据"))
                   {
                       SetProductTileFullPara();
                       //if (sqliteclient == null)
                       //{
                       //    sqliteclient = new localhostSqlite.Service();

                       //}
                       prodctParas = new string[5];
                       prodctParas[0] = prodType;
                       prodctParas[1] = coordstr;
                       prodctParas[2] = tileLevel;
                       prodctParas[3] = "" + startdate;
                       prodctParas[4] = "" + enddate;

                       //设置多出现一个panel
                       mucdetail.setTileSingleBothPanel();

                       //调用服务SearProdTileCountBatch_coordsStr，获取把某一区域全覆盖的瓦片数及其行列号，并将其画在三维球上
                        SerSearProdTileCountBatch(prodctParas);
                       //////调用服务SearProdTileBatch_coordsStr，获取把某一区域全覆盖的最新时间的瓦片组合
                        serSearProdTileBatch(prodctParas);

                       ////Task tk1 = new Task(o => SerSearProdTileCountBatch((string[])o), paras);
                       ////tk1.Start();
                       ////Task tk2 = new Task(o => serSearProdTileBatch((string[])o), paras);
                       ////tk2.Start();
                   }
                }

            }
            if (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))  //原始数据的全覆盖
            {
                //queryPara = ConstructRasterQueryPara();
                #region
                //ClearExtentLyr();
                //IFeature userft = testF;
                //// testF = null;
                //double minLon = Double.Parse(barEditItemMinLon.EditValue.ToString());
                //double maxLon = Double.Parse(barEditItemMaxLon.EditValue.ToString());
                //double minLat = Convert.ToDouble(barEditItemMinLat.EditValue);
                //double maxLat = Convert.ToDouble(barEditItemMaxLat.EditValue);


                //DataTable resultTable = new DataTable();
                //resultTable = dtable.Copy();
                //resultTable.Clear();
                //int num = dtable.Rows.Count;
                //IFeature f = userft;

                //for (int i = 0; i < num; i++)
                //{
                //    IGeometry ig = getGeomFromRow(dtable.Rows[i]);
                //    if (f.Intersects(ig))//判断这一张图片和剩下的区域（差集）是否有交集 没有就换一张
                //    {
                //        f = f.Difference(ig);//求出 剩下的区域与这一张图片的差集
                //        if (f != null)
                //        {
                //            resultTable.Rows.Add(dtable.Rows[i].ItemArray);
                //        }
                //        else
                //        {
                //            resultTable.Rows.Add(dtable.Rows[i].ItemArray);
                //            break;
                //        }
                //    }
                //}
                //System.Data.DataSet ds = new System.Data.DataSet();
                //ds.Tables.Add(resultTable);
                //queryResponse.recordSet = ds;

                //DrawSpacialExtent();
                //if (resultTable == null || resultTable.Rows.Count == 0)
                //{
                //    MessageBox.Show("没有查找到数据!");
                //    return;
                //}
                //resultTable.Columns.Remove("选择数据");
                //mucdetail.setGridControl(resultTable);
                #endregion
                list.Clear();
                sourceDt = dtable;
                DataView dv = sourceDt.DefaultView;
               // dv.Sort = "接收时间 DESC";
                dv.Sort = "云量百分比 ASC";
                sourceDt = dv.ToTable();
                ClearExtentLyr();
                IFeature userft = testF;
                double minLon = Double.Parse(barEditItemMinLon.EditValue.ToString());
                double maxLon = Double.Parse(barEditItemMaxLon.EditValue.ToString());
                double minLat = Convert.ToDouble(barEditItemMinLat.EditValue);
                double maxLat = Convert.ToDouble(barEditItemMaxLat.EditValue);
                resultTable = sourceDt.Copy();
                resultTable.Clear();

                resultTable1 = sourceDt.Copy();
                resultTable1.Clear();

                int num = sourceDt.Rows.Count;
                IFeature f = userft;
                bool isAllCover = false;
                #region
                for (int i = 0; i < num; i++)
                {
                    IGeometry ig = getGeomFromRow(sourceDt.Rows[i]);
                    if (f.Intersects(ig))//&& sourceDt.Rows[i]["云量百分比"].ToString() == "0"
                    {
                        f = f.Difference(ig);
                        if (f != null)
                        {
                            resultTable.Rows.Add(sourceDt.Rows[i].ItemArray);
                            list.Add(sourceDt.Rows[i]["景序列号"].ToString());//2454700
                            string code = sourceDt.Rows[i]["数据编码"].ToString();
                            int end = code.LastIndexOf("-");
                            string chushu = code.Substring(end + 1, code.Length - end - 1);
                            string shang = Convert.ToString(Convert.ToInt32(chushu) / 1000);
                            string rootPath = StoragePath.StoreBasePath;//zsm 当前磁盘阵列
                            string p = rootPath + "Thumb" + "\\" + shang + "\\" + sourceDt.Rows[i]["数据编码"] + ".jpg";//@\\192.168.10.190\zhsjk\\Thumb\0\0001-EVDB-32-9.jpg
                            if (File.Exists(p))
                            {
                                byte opacity = 255;
                                bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(sourceDt.Rows[i]["数据编码"].ToString(), 100, Double.Parse(sourceDt.Rows[i]["左上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左上经度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上经度"].ToString()), 0, opacity, p);
                            }
                            else
                            {
                                byte opacity = 255;
                                bool addLayer;
                                foreach (var item in StoragePath.StoreHistoryPath)//zsm 循环得到历史磁盘阵列
                                {
                                    string p1 = item + "Thumb" + "\\" + shang + "\\" + sourceDt.Rows[i]["数据编码"] + ".jpg";
                                    if (File.Exists(p1))
                                    {
                                        addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(sourceDt.Rows[i]["数据编码"].ToString(), 100, Double.Parse(sourceDt.Rows[i]["左上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左上经度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上经度"].ToString()), 0, opacity, p1);
                                        break;
                                    }
                                }
                                //  string p = @"E:\zhsjk\Thumb\" + sourceDt.Rows[i]["数据编码"] + ".jpg";
                                // string p = @"E:\zhsjk\Thumb\"+shang+"\\" + sourceDt.Rows[i]["数据编码"] + ".jpg";//E:\zhsjk\Thumb\0001-EVDB-32-76.jpg
                            }
                        }
                        else
                        {
                            resultTable.Rows.Add(sourceDt.Rows[i].ItemArray);
                            list.Add(sourceDt.Rows[i]["景序列号"].ToString());
                            string code = sourceDt.Rows[i]["数据编码"].ToString();
                            int end = code.LastIndexOf("-");
                            string chushu = code.Substring(end + 1, code.Length - end - 1);
                            string shang = Convert.ToString(Convert.ToInt32(chushu) / 1000);
                            string rootPath = StoragePath.StoreBasePath;//zsm 当前磁盘阵列
                            string p = rootPath + "Thumb" + "\\" + shang + "\\" + sourceDt.Rows[i]["数据编码"] + ".jpg";//@\\192.168.10.190\zhsjk\\Thumb\0\0001-EVDB-32-9.jpg
                            if (File.Exists(p))
                            {
                                byte opacity = 255;
                                bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(sourceDt.Rows[i]["数据编码"].ToString(), 100, Double.Parse(sourceDt.Rows[i]["左上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左上经度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上经度"].ToString()), 0, opacity, p);
                            }
                            else
                            {
                                byte opacity = 255;
                                bool addLayer;
                                foreach (var item in StoragePath.StoreHistoryPath)//zsm 循环得到历史磁盘阵列
                                {
                                    string p1 = item + "Thumb" + "\\" + shang + "\\" + sourceDt.Rows[i]["数据编码"] + ".jpg";
                                    if (File.Exists(p1))
                                    {
                                        addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(sourceDt.Rows[i]["数据编码"].ToString(), 100, Double.Parse(sourceDt.Rows[i]["左上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左上经度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上经度"].ToString()), 0, opacity, p1);
                                        break;
                                    }
                                    else
                                    {
                                        // MessageBox.Show("该快试图不存在!!!");
                                    }
                                }
                            }
                            isAllCover = true;
                            break;
                        }
                    }
                }
                MessageBox.Show(list.Count.ToString());
                #endregion
                if (!isAllCover)
                {
                    //getCut(f);
                    //getCutCopy(f);
                }
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.Tables.Add(resultTable);
                queryResponse.recordSet = ds;

                DrawSpacialExtent();
                if (resultTable == null || resultTable.Rows.Count == 0)
                {
                    MessageBox.Show("没有查找到数据!");
                    return;
                }
                resultTable.Columns.Remove("选择数据");
                mucdetail.setGridControl(resultTable);

                //DrawSpacialExtent();
                //resultTable.Columns.Remove("选择数据");
                //mucdetail.setGridControl(resultTable);
            }
        }
        /// <summary>
        /// 判断该数据在表中是否存在，不存在为true
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="dataname"></param>
        /// <returns></returns>
        private bool issame(DataTable datatable, string dataname)
        {
            if (datatable == null)
            {
                return true;
            }
            else
            {
                 int i = datatable.Rows.Count;
                 for (int j = 0; j < i; j++)
                 {
                     if (resultTable1.Rows[j]["数据名称"].ToString() == dataname)
                     {
                         return false;
                     }
                     else
                     {
                         continue;
                     
                     }
                 }
                 return true;              
            }
        }
        List<Envelope> envList = null;
        List<string[]> AOITile = null;
        private void getCut(IFeature f)
        {
            #region
            // string path = @"D:\thumb\";
            // DirectoryInfo TheFolder = new DirectoryInfo(path);
            // string constr = string.Format("Data Source={0};User ID={1};Password={2};database = evdb", "192.168.10.190", "HJDATABASE_ADMIN", "dbadmin_2011");
            // MySqlBaseUtilities mysql = new MySqlBaseUtilities(constr);
            //foreach (FileInfo NextFile in TheFolder.GetFiles())
            //{
            //    string Name = NextFile.ToString().Replace(".jpg", ".");
            //    string sql = string.Format("select * from prod_gf1 where Name like '{0}%'", Name);
            //    System.Data.DataSet ds = mysql.GetDataSet(sql);
            //    DataTable sourceDt = ds.Tables[0];
            //    byte opacity = 255;n
            //    string p = @"D:\\thumb\" + sourceDt.Rows[0]["Name"].ToString().Replace(".tar.gz", "") + ".jpg";

            //    bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(sourceDt.Rows[0]["QRST_CODE"].ToString(), 100,
            //        Double.Parse(sourceDt.Rows[0]["DATAUPPERLEFTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATAUPPERLEFTLONG"].ToString()),
            //        Double.Parse(sourceDt.Rows[0]["DATALOWERLEFTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATALOWERLEFTLONG"].ToString()),
            //        Double.Parse(sourceDt.Rows[0]["DATALOWERRIGHTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATALOWERRIGHTLONG"].ToString()),
            //        Double.Parse(sourceDt.Rows[0]["DATAUPPERRIGHTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATAUPPERRIGHTLONG"].ToString()), 0, opacity, p);
            //}
            #endregion
            double minLon = Double.Parse(barEditItemMinLon.EditValue.ToString());
            double maxLon = Double.Parse(barEditItemMaxLon.EditValue.ToString());
            double minLat = Convert.ToDouble(barEditItemMinLat.EditValue);
            double maxLat = Convert.ToDouble(barEditItemMaxLat.EditValue);
            DateTime dt = DateTime.Now;
            // DataTable resultTable = new DataTable();
            resultTable = sourceDt.Copy();
            resultTable.Clear();
            DataTable temp = new DataTable();
            temp = resultTable;
            int mm = 0;
            int ImgCount = 0;
            envList = new List<Envelope>();//目标格网
            AOITile = new List<string[]>();//目标瓦片
            // GetAOITilesGrid(@"G:\shp文件\25米.shp", f);//11111
            GetAOITilesGrid(@"F:\QRST_APPs\QRST_LANFANG\Lanfang_AppSysWinForm\bin\Debug\区域线化图\五层十五级格网\25米.shp", f);//G:\shp文件\25米.shp
            foreach (var item in AOITile)
            {
                double[] minTileLB = DirectlyAddressing.GetLatAndLong(item, TileLevel);
                string s = string.Format("`左上纬度` >= '{0}' AND `左上经度` <= '{1}' AND `右上纬度` >= '{0}' AND `右上经度` >= '{2}' AND `右下纬度` <= '{3}' AND `右下经度` >= '{2}' AND `左下纬度` <= '{3}' AND `左下经度` <= '{1}'", minTileLB[2], minTileLB[1], minTileLB[3], minTileLB[0]);
                DataRow[] dr = sourceDt.Select(string.Format("`左上纬度` >= '{0}' AND `左上经度` <= '{1}' AND `右上纬度` >= '{0}' AND `右上经度` >= '{2}' AND `右下纬度` <= '{3}' AND `右下经度` >= '{2}' AND `左下纬度` <= '{3}' AND `左下经度` <= '{1}'", minTileLB[2], minTileLB[1], minTileLB[3], minTileLB[0]));
                DataTable dtable = new DataTable();
                dtable = temp.Copy();
                dtable.Clear();
                for (int l = 0; l < dr.Count(); l++)
                {
                    dtable.Rows.Add(dr[l].ItemArray);

                    // resultTable.Rows.Add(dr[l].ItemArray);//该行代码和下面for循环性质一样 zsm
                }
                DataView dv = dtable.DefaultView;

                dv.Sort = "接收时间 Desc";
                DataTable ds = dv.ToTable();

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    //string dataname = ds.Rows[i]["数据名称"].ToString();
                    //int k = resultTable1.Rows.Count;
                    //for (int j = 0; j < k; j++)
                    //{
                    //    if (resultTable1.Rows[j]["数据名称"].ToString() == dataname)
                    //    {
                    //        break;

                    //    }
                    //    DataRow rr = ds.Rows[i];
                    //    resultTable1.Rows.Add(rr.ItemArray);
                    //}
                    string dataname = ds.Rows[i]["数据名称"].ToString();
                    if (issame(resultTable1, dataname))
                    {
                        DataRow rr=ds.Rows[i];
                        resultTable1.Rows.Add(rr.ItemArray);
                    }
                    else
                    { }

                }


                string p = "";
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    double maxLa = Math.Max(Double.Parse(ds.Rows[i]["右上纬度"].ToString()), Double.Parse(ds.Rows[i]["左上纬度"].ToString()));
                    double minLa = Math.Min(Double.Parse(ds.Rows[i]["左下纬度"].ToString()), Double.Parse(ds.Rows[i]["右下纬度"].ToString()));
                    double maxLo = Math.Max(Double.Parse(ds.Rows[i]["右上经度"].ToString()), Double.Parse(ds.Rows[i]["右下经度"].ToString()));
                    double minLo = Math.Min(Double.Parse(ds.Rows[i]["左上经度"].ToString()), Double.Parse(ds.Rows[i]["左下经度"].ToString()));
                    double differLat = Double.Parse(ds.Rows[i]["右上纬度"].ToString()) - Double.Parse(ds.Rows[i]["左上纬度"].ToString());
                    double differLon = Double.Parse(ds.Rows[i]["右上经度"].ToString()) - Double.Parse(ds.Rows[i]["左上经度"].ToString());
                    double sum = differLat * differLat + differLon * differLon;
                    double latlon = Math.Sqrt(sum);//p1p2的距离
                    double averageLon = latlon / 256;//每个像素宽的跨度为∆w
                    double differLeftLon = Double.Parse(ds.Rows[i]["左上经度"].ToString()) - Double.Parse(ds.Rows[i]["左下经度"].ToString());
                    double differLeftLat = Double.Parse(ds.Rows[i]["左上纬度"].ToString()) - Double.Parse(ds.Rows[i]["左下纬度"].ToString());
                    double Leftsum = differLeftLat * differLeftLat + differLeftLon * differLeftLon;
                    double Leftlatlon = Math.Sqrt(Leftsum);//p1p4的距离
                    double LeftaverageLon = Leftlatlon / 256;//每个像素高的跨度为∆h
                    double k1 = (maxLa - Double.Parse(ds.Rows[i]["左下纬度"].ToString())) / (Double.Parse(ds.Rows[i]["左上经度"].ToString()) - minLo);//p1p4的斜率
                    differLeftLat = maxLa - Double.Parse(ds.Rows[i]["左下纬度"].ToString());//p4的y值
                    string code = ds.Rows[i]["数据编码"].ToString();
                    string cloudP = ds.Rows[i]["云量百分比"].ToString();
                    //zsm 20170320
                    int end = code.LastIndexOf("-");
                    string chushu = code.Substring(end + 1, code.Length - end - 1);
                    string shang = Convert.ToString(Convert.ToInt32(chushu) / 1000);
                    string rootPath = StoragePath.StoreBasePath;
                    p = rootPath + "Thumb" + "\\" + shang + "\\" + code + ".jpg";
                    if (File.Exists(p))
                    { }
                    else
                    {
                        //MessageBox.Show("当前磁盘阵列中不存在该快试图，到历史磁盘阵列中查找!，做测试呢");
                        foreach (var itemd in StoragePath.StoreHistoryPath)
                        {
                            p = itemd + "Thumb" + "\\" + shang + "\\" + code + ".jpg";
                            if (File.Exists(p))
                            {
                                break;
                            }
                            else
                            {
                                //MessageBox.Show("该快试图不存在!!!");

                            }
                        }

                    }
                    if (!File.Exists(p))
                    {
                        continue;
                    }
                    Bitmap image = new Bitmap(p);

                    //Bitmap image = new Bitmap(@"E:\zhsjk\Thumb\" + code + ".jpg");

                    #region
                    //X坐标
                    double d1 = k1 * (minTileLB[1] - minLo) - (maxLa - minTileLB[2]) + differLeftLat;//30.75,117.5,31.0,117.75
                    if (d1 < 0)
                    {
                        d1 = -d1;
                    }
                    double sum1 = 1 + k1 * k1;
                    double d11 = Math.Sqrt(sum1);
                    double lx = d1 / d11;//格网的P1’到原始影像p1p4的距离（度）
                    int upN = (Int32)(lx / averageLon);//P1’在快视图中距离左边缘的像素

                    double d2 = k1 * (minTileLB[3] - minLo) - (maxLa - minTileLB[2]) + differLeftLat;
                    if (d2 < 0)
                    {
                        d2 = -d2;
                    }
                    double x2 = d2 / d11;
                    int upN2 = (Int32)(x2 / averageLon);//P2’在快视图中距离左边缘的宽度

                    double d3 = k1 * (minTileLB[3] - minLo) - (maxLa - minTileLB[0]) + differLeftLat;
                    if (d3 < 0)
                    {
                        d3 = -d3;
                    }
                    double x3 = d3 / d11;
                    int upN3 = (Int32)(x3 / averageLon);//P3’在快视图中距离左边缘的宽度

                    double d4 = k1 * (minTileLB[1] - minLo) - (maxLa - minTileLB[0]) + differLeftLat;
                    if (d4 < 0)
                    {
                        d4 = -d4;
                    }
                    double x4 = d4 / d11;
                    int upN4 = (Int32)(x4 / averageLon);//P4’在快视图中距离左边缘的宽度


                    //Y坐标
                    // double dLat = maxLa - Double.Parse(ds.Rows[i]["右上纬度"].ToString());
                    double dLat = (maxLa - Double.Parse(ds.Rows[i]["右上纬度"].ToString())) / (maxLo - Double.Parse(ds.Rows[i]["左上经度"].ToString()));//正确的//p1p2的斜率修改了20170412

                    //double b = dLat * (Double.Parse(ds.Rows[i]["左上经度"].ToString()) - minLo);
                    double b = dLat * (minLo - Double.Parse(ds.Rows[i]["左上经度"].ToString()));//正确的
                    //double sum2 = dLat * dLat + differLon * differLon;
                    double sum2 = 1 + dLat * dLat;//正确的
                    double k2 = Math.Sqrt(sum2);
                    //double c1 = dLat * (minTileLB[1] - minLo) - differLon * (maxLa - minTileLB[2]) - b;
                    double c1 = dLat * (minTileLB[1] - minLo) - (maxLa - minTileLB[2]) + b;//正确的
                    if (c1 < 0)
                    {
                        c1 = -c1;
                    }
                    double y1 = c1 / k2;
                    int M1 = (Int32)(y1 / LeftaverageLon);
                    //double c2 = dLat * (minTileLB[3] - minLo) - differLon * (maxLa - minTileLB[2]) - b;
                    double c2 = dLat * (minTileLB[3] - minLo) - (maxLa - minTileLB[2]) + b;//正确的
                    if (c2 < 0)
                    {
                        c2 = -c2;
                    }
                    double y2 = c2 / k2;
                    int M2 = (Int32)(y2 / LeftaverageLon);
                    //double c3 = dLat * (minTileLB[3] - minLo) - differLon * (maxLa - minTileLB[0]) - b;
                    double c3 = dLat * (minTileLB[3] - minLo) - (maxLa - minTileLB[0]) + b;//正确的
                    if (c3 < 0)
                    {
                        c3 = -c3;
                    }
                    double y3 = c3 / k2;
                    int M3 = (Int32)(y3 / LeftaverageLon);
                    // double c4 = dLat * (minTileLB[1] - minLo) - differLon * (maxLa - minTileLB[0]) - b;
                    double c4 = dLat * (minTileLB[1] - minLo) - (maxLa - minTileLB[0]) + b;//正确的
                    if (c4 < 0)
                    {
                        c4 = -c4;
                    }
                    double y4 = c4 / k2;
                    int M4 = (Int32)(y4 / LeftaverageLon);
                    #endregion
                    System.Drawing.Point[] tmp = new System.Drawing.Point[] { };
                    tmp = new System.Drawing.Point[] {
                 new System.Drawing.Point(M1-1,upN-1),
                 new System.Drawing.Point(M2-1,upN2-1),
                 new System.Drawing.Point(M3-1,upN3-1),
                 new System.Drawing.Point(M4-1,upN4-1)};
                    //  Bitmap image1 = new Bitmap(bmpPath + code + ".jpg");
                    Bitmap image1 = new Bitmap(p);//20170320 BitMap大小 为256*256 将Bitmap锁定到系统内存中
                    Rectangle rect1 = new Rectangle(0, 0, image1.Width, image1.Height);//20170414 创建所绘制图像的位置和大小 是指源图像中需要锁定那一块矩形区域进行处理 
                    System.Drawing.Imaging.BitmapData bmpData1 = image1.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, image1.PixelFormat);
                    IntPtr ptr1 = bmpData1.Scan0;////位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行  
                    //修改了0414
                    int bytes2 = Math.Abs(bmpData1.Stride) * image1.Height;//该行代码用不到
                    //byte[] rgbValues1 = new byte[bytes1];
                    int bytes1 = rect1.Width * 3 * rect1.Height;//Math.Abs(bmpData1.Stride) * image1.Height;
                    byte[] rgbValues1 = new byte[bytes1];

                    System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes1);
                    List<double> rgbList = new List<double>();//像素点的总个数
                    for (int k = 0; k < rgbValues1.Length; k = k + 3)
                    {
                        rgbList.Add(Math.Round(rgbValues1[k] * 0.114 + rgbValues1[k + 1] * 0.587 + rgbValues1[k + 2] * 0.299));// 0.589 有的是0.587
                    }
                    rgbList.Sort();
                    rgbList.Reverse();
                    var g = rgbList.GroupBy(j => j);
                    List<double> keyL = new List<double>();
                    List<int> valueL = new List<int>();
                    double sumCount = 0;//像素点大于阈值的个数
                    double thresholdValue = 0;//阈值
                    foreach (var g1 in g)
                    {
                        sumCount += g1.Count();
                        double x = Math.Round(sumCount / Double.Parse(rgbList.Count.ToString()) * 100);
                        if (Math.Round(sumCount / Double.Parse(rgbList.Count.ToString()) * 100) == Double.Parse(cloudP))
                        {
                            thresholdValue = g1.Key;//阈值有可能是100多 这样云检测不太好 zsm个人认为哦
                            break;
                        }
                    }
                    //CutImge(code, tmp);

                    Bitmap resultImage = new Bitmap(image.Width, image.Height);
                    //建立缓冲图片
                    Graphics gr = Graphics.FromImage(resultImage);
                    gr.Clear(Color.FromArgb(0, Color.Transparent));
                    Rectangle resultRectangle = new Rectangle(0, 0, image.Width, image.Height);
                    int width = image.Width;
                    int height = image.Height;
                    Region reg = new Region();
                    reg.MakeEmpty();
                    GraphicsPath gp = new GraphicsPath();
                    //System.Drawing.Point[] myArray = GetArrayForViewStyle();
                    gp.AddPolygon(tmp);
                    reg.Union(gp);

                    gr.SetClip(reg, CombineMode.Replace);
                    gr.DrawImage(image, resultRectangle);
                    Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);
                    gp.Dispose();
                    reg.Dispose();
                    //Bitmap image = new Bitmap(bmpPath + code + "Cut.jpg");
                   // Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);//该行代码之前是注释的
                    System.Drawing.Imaging.BitmapData bmpData = resultImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, resultImage.PixelFormat);
                    IntPtr ptr = bmpData.Scan0;
                    int bytes = Math.Abs(bmpData.Stride) * image.Height;
                    byte[] rgbValues = new byte[bytes];
                    //int countNum = bmp.Width * bmp.Height;
                    List<int> tempList = new List<int>();//红绿蓝的值
                    List<int> resList = new List<int>();
                    // Copy the RGB values into the array.复制RGB值到数组  
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
                    double count = 0;
                    for (int j = 0; j < rgbValues.Length; j = j + 3)
                    {
                        if (rgbValues[j] != 0 && rgbValues[j + 1] != 0 && rgbValues[j + 2] != 0)
                        {
                            tempList.Add(rgbValues[j]);
                            tempList.Add(rgbValues[j + 1]);
                            tempList.Add(rgbValues[j + 2]);
                            if (Math.Round(rgbValues[j] * 0.114 + rgbValues[j + 1] * 0.587 + rgbValues[j + 2] * 0.299) >= thresholdValue)//像素点的值大于阈值则改点为云
                            {
                                count++;
                            }
                        }
                    }

                    double perc = count / Double.Parse(tempList.Count.ToString());//count值一般都是几十 代表有几十个像素点为云 所以该值为0.0多少多少
                    if (Math.Round(perc) * 100 == 0)//四舍五入后都是0
                    {
                        ImgCount++;
                        list.Add(ds.Rows[i]["景序列号"].ToString());
                        temp.Rows.Add(ds.Rows[i].ItemArray);
                        int Imgwidth = (Int32)((minTileLB[3] - minTileLB[1]) / averageLon) + 1;
                        int Imgheight = (Int32)((minTileLB[2] - minTileLB[0]) / LeftaverageLon) + 1;
                        Bitmap bmpNew = new Bitmap(Imgwidth, Imgheight, PixelFormat.Format24bppRgb);
                        int pixX = 0;
                        int pixY = 0;
                        for (double lat = minTileLB[2]; lat > minTileLB[0]; lat = lat - LeftaverageLon)
                        {
                            for (double lon = minTileLB[1]; lon < minTileLB[3]; lon = lon + averageLon)
                            {
                                List<int> pt = LatLonToPixl(resultTable, lat, lon);
                                if (pt[0] > 255)
                                {
                                    pt[0] = 255;
                                    mm++;
                                }
                                int red = image.GetPixel(pt[1], pt[0]).R;
                                int green = image.GetPixel(pt[1], pt[0]).G;
                                int blue = image.GetPixel(pt[1], pt[0]).B;
                                bmpNew.SetPixel(pixX, pixY, Color.FromArgb(red, green, blue));
                                pixX++;

                            }
                            pixX = 0;
                            pixY++;
                        }
                        //11111//F:\QRST_DI\QRST_DI\QRST_DI_MS-Console V2.0\bin\Debug\gewangthumb
                        string gewangthumbpath = string.Format(@"{0}\{1}\{2}", Application.StartupPath, @"gewangthumb",code);
                        if (Directory.Exists(gewangthumbpath))
                        {
                            bmpNew.Save(gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg", ImageFormat.Jpeg);
                            //bmpNew.Save(@"G:\\thumb\\" + code + "Cut" + ImgCount + ".jpg", ImageFormat.Jpeg);
                            image.Dispose();

                            temp.Clear();
                            //11111
                            string p2 = gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg";
                            //string p2 = @"G:\\thumb\\" + code + "Cut" + ImgCount + ".jpg";
                            byte opacity = 255;
                            //高度改成1了，原来是100
                            bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 1, minTileLB[0], minTileLB[2], minTileLB[1], minTileLB[3], 0, opacity, p2);

                            break;

                        }
                        else
                        {
                            Directory.CreateDirectory(gewangthumbpath);
                            bmpNew.Save(gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg", ImageFormat.Jpeg);
                            //bmpNew.Save(@"G:\\thumb\\" + code + "Cut" + ImgCount + ".jpg", ImageFormat.Jpeg);
                            image.Dispose();

                            temp.Clear();
                            //11111
                            string p2 = gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg";
                            //string p2 = @"G:\\thumb\\" + code + "Cut" + ImgCount + ".jpg";
                            byte opacity = 255;
                            //高度改成1了，原来是100
                            bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 1, minTileLB[0], minTileLB[2], minTileLB[1], minTileLB[3], 0, opacity, p2);

                            break;
                        }
                    }
                }


            }
            list = list.Distinct().ToList();
            // MessageBox.Show(list.Count.ToString());


            //System.Data.DataSet dset = new System.Data.DataSet();
            //dset.Tables.Add(resultTable);
            //queryResponse.recordSet = dset;

            //DrawSpacialExtent(); 


        }
        public void GetAOITilesGrid(string gridPath, IFeature ifea)
        {
            this.GetAOITilesCR(ifea, AOITile);
        }

        private void GetAOITilesCR(IFeature feature, List<string[]> aoiTile)
        {
            //DateTime beforDT = System.DateTime.Now;
            //得到外接矩形包含的行列号
            string[] feaEnvelope = new string[] { 
                    feature.Envelope.Minimum.Y.ToString(),
                    feature.Envelope.Minimum.X.ToString() , 
                    feature.Envelope.Maximum.Y.ToString(), 
                    feature.Envelope.Maximum.X.ToString() };
            int[] colRow = DirectlyAddressing.GetRowAndColum(feaEnvelope, TileLevel);//最小行，最小列，最大行，最大列
            int rownum = colRow[2] - colRow[0] + 1;
            int colnum = colRow[3] - colRow[1] + 1;
            //获得六参数
            double[] GT = new double[6];
            double resolution = double.Parse(DirectlyAddressing.GetDegreeByStrLv(TileLevel));
            GT[1] = GT[5] = resolution;
            string[] minTileRC = { colRow[0].ToString(), colRow[1].ToString() };
            double[] minTileLB = DirectlyAddressing.GetLatAndLong(minTileRC, TileLevel);//最小纬度，最小经度，最大纬度，最大经度
            GT[0] = minTileLB[0];//最小纬度
            GT[3] = minTileLB[1];//最小经度
            GT[2] = GT[4] = 0;

            //判断多边形和瓦片矩形是否相交，相交即为区域瓦片 注意point（x，y）x为精度，y为纬度
            for (int i = 0; i < rownum; i++)
                for (int j = 0; j < colnum; j++)
                {
                    try
                    {
                        List<double[]> Tile4Geolist = TileGeoTrans(i, j, GT);
                        Coordinate coord1 = new Coordinate(Tile4Geolist[0][0], Tile4Geolist[0][1]);
                        Coordinate coord2 = new Coordinate(Tile4Geolist[1][0], Tile4Geolist[1][1]);
                        Envelope enve = new Envelope(coord1, coord2);
                        bool overlap = feature.Intersects(enve);
                        if (overlap)
                        {
                            string[] tile = new string[2];
                            tile[0] = (i + colRow[0]).ToString();
                            tile[1] = (j + colRow[1]).ToString();
                            aoiTile.Add(tile);
                            envList.Add(enve);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message.ToString());
                        break;
                    }
                }
        }

        ///// <summary>
        ///// 并行检索，任务等待
        ///// </summary>
        ///// <param name="tasks">并行检索任务list</param>
        ///// <param name="outtime">超时时间 毫秒单位</param>
        //    private void WaitingForTasks(List<Task> tasks, int outtime = 10000)
        //    {
        //        DateTime startdt = DateTime.Now;
        //        while (true)
        //        {
        //            if ((DateTime.Now - startdt).TotalMilliseconds > outtime)
        //            {
        //                break;
        //            }
        //            bool cmpl = true;
        //            foreach (Task t in tasks)
        //            {
        //                if (!t.IsCompleted)
        //                {
        //                    cmpl = false;
        //                }
        //            }
        //            if (cmpl)
        //            {
        //                break;
        //            }
        //            System.Threading.Thread.Sleep(200);
        //        }
        //    }

        /// <summary>
        /// xmh 20170524
        /// 单时相全覆盖检索后，把结果集在三维球上显示的贴图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemLyr_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clickLyr = true;
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") && clickFlag == true) //切片的单时相全覆盖的贴图
            {
                DataTable temptable = Lyrtable;
                if (temptable != null)
                {
                    for (int i = 0; i < temptable.Rows.Count; i++)
                    {
                        DataRow dr = temptable.Rows[i];
                        AddSingleLyrOrder(dr);

                    }

                }
                else
                {
                    return;
                }

            }
        }


        string ChooseDataColumnCaption
        {
            get
            {
                string caption = "";
                switch (Constant.SystemLanguage)
                {
                    case EnumLanguage.ch:
                        caption = "select the data";
                        break;
                    case EnumLanguage.en:
                        caption = "选择数据";
                        break;
                }
                return caption;

            }
        }

        /// <summary>
        /// xmh 20170525
        /// 取消贴图(1 先获取贴图的瓦片列表，2 遍历列表，一一删除)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonCancelLyr_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucsearcher.qrstAxGlobeControl1.QrstGlobe.RemoveImages();

        }
        private void barButtonReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dtable != null)
            {
                DataTable dt = dtable.Copy();
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.Tables.Add(dt);
                queryResponse.recordSet = ds;

                DrawSpacialExtent();
                dtable.Columns.Remove("选择数据");
                mucdetail.setGridControl(dtable);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //string path = @"D:\thumb\";
            //DirectoryInfo TheFolder = new DirectoryInfo(path);
            //string constr = string.Format("Data Source={0};User ID={1};Password={2};database = evdb", "192.168.1.110", "HJDATABASE_ADMIN", "dbadmin_2011");
            //MySqlBaseUtilities mysql = new MySqlBaseUtilities(constr);
            //foreach (FileInfo NextFile in TheFolder.GetFiles())
            //{
            //    string Name = NextFile.ToString().Replace(".jpg", ".");
            //    string sql = string.Format("select * from prod_gf1 where Name like '{0}%'", Name);
            //    System.Data.DataSet ds = mysql.GetDataSet(sql);
            //    DataTable sourceDt = ds.Tables[0];
            //    byte opacity = 255;
            //    string p = @"D:\\thumb\" + sourceDt.Rows[0]["Name"].ToString().Replace(".tar.gz", "") + ".jpg";

            //    bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(sourceDt.Rows[0]["QRST_CODE"].ToString(), 100,
            //        Double.Parse(sourceDt.Rows[0]["DATAUPPERLEFTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATAUPPERLEFTLONG"].ToString()),
            //        Double.Parse(sourceDt.Rows[0]["DATALOWERLEFTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATALOWERLEFTLONG"].ToString()),
            //        Double.Parse(sourceDt.Rows[0]["DATALOWERRIGHTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATALOWERRIGHTLONG"].ToString()),
            //        Double.Parse(sourceDt.Rows[0]["DATAUPPERRIGHTLAT"].ToString()), Double.Parse(sourceDt.Rows[0]["DATAUPPERRIGHTLONG"].ToString()), 0, opacity, p);
            //}
            ClearExtentLyr();
            IFeature userft = testF;
            double minLon = Double.Parse(barEditItemMinLon.EditValue.ToString());
            double maxLon = Double.Parse(barEditItemMaxLon.EditValue.ToString());
            double minLat = Convert.ToDouble(barEditItemMinLat.EditValue);
            double maxLat = Convert.ToDouble(barEditItemMaxLat.EditValue);
          
           
            DataTable resultTable = new DataTable();
            resultTable = sourceDt.Copy();
            resultTable.Clear();
            DataTable temp = new DataTable();
            temp = resultTable.Copy();
            for (int i = 0; i < sourceDt.Rows.Count-1; i++)
            {
                string s = sourceDt.Rows[i]["接收时间"].ToString().Substring(0, sourceDt.Rows[i]["接收时间"].ToString().LastIndexOf(" "));
                if (sourceDt.Rows[i + 1]["接收时间"].ToString().Substring(0, sourceDt.Rows[i]["接收时间"].ToString().LastIndexOf(" ")) == s)
                {
                    temp.Rows.Add(sourceDt.Rows[i].ItemArray);
                }
                else
                {
                    temp.Rows.Add(sourceDt.Rows[i].ItemArray);
                    DataView tmpdv = temp.DefaultView;
                    tmpdv.Sort = "传感器 DESC";
                    temp = tmpdv.ToTable();
                    resultTable.Merge(temp);
                    temp.Clear();
                }
            }
            sourceDt.Clear();
            sourceDt = resultTable;
            int num = sourceDt.Rows.Count;
            IFeature f = userft;
            DateTime dt = DateTime.Now;
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(sourceDt);
            queryResponse.recordSet = ds;

            DrawSpacialExtent();
            for (int i = 0; i <num; i++)
            {
                IGeometry ig = getGeomFromRow(sourceDt.Rows[i]);
                if (f.Intersects(ig))
                {
                    f = f.Difference(ig);
                    if (f != null)
                    {
                        resultTable.Rows.Add(sourceDt.Rows[i].ItemArray);
                        byte opacity = 255;
                        //string p = @"D:\\thumb\" + sourceDt.Rows[i]["数据名称"].ToString().Replace(".tar.gz", "") + ".jpg";

                        //bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(sourceDt.Rows[i]["数据编码"].ToString(), 100, Double.Parse(sourceDt.Rows[i]["左上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左上经度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["左下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右下经度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上纬度"].ToString()), Double.Parse(sourceDt.Rows[i]["右上经度"].ToString()), 0, opacity, p);

                    }
                    else
                    {
                        resultTable.Rows.Add(sourceDt.Rows[i].ItemArray);
                        byte opacity = 255;
                        break;
                    }
                }
            }
          
            DateTime dt1 = DateTime.Now;
            TimeSpan ts = dt1 - dt;
            MessageBox.Show(ts.ToString());
            //MessageBox.Show(resultTable.Rows.Count.ToString());
            //IFeature userft;
            //double minLon = Double.Parse(barEditItemMinLon.EditValue.ToString());
            //double maxLon = Double.Parse(barEditItemMaxLon.EditValue.ToString());
            //double minLat = Convert.ToDouble(barEditItemMinLat.EditValue);
            //double maxLat = Convert.ToDouble(barEditItemMaxLat.EditValue);
            //DateTime dt = DateTime.Now;
            //DataTable resultTable = new DataTable();
            //resultTable = sourceDt.Copy();
            //resultTable.Clear();
            //IFeature f = testF;
            //int mm = 0;
            //int ImgCount = 0;
            //GetAOITilesGrid(@"F:\QRST_DI\QRST_APPs\QRST_LANFANG\Lanfang_AppSysWinForm\bin\Debug\区域线化图\五层十五级格网\25米.shp",f);
            //foreach (var item in AOITile)
            //{
            //    double[] minTileLB = DirectlyAddresser.GetLatAndLong(item, TileLevel);
            //    DataRow[] dr= sourceDt.Select(string.Format("`左上纬度` >= '{0}' AND `左上经度` <= '{1}' AND `右上纬度` >= '{0}' AND `右上经度` >= '{2}' AND `右下纬度` <= '{3}' AND `右下经度` >= '{2}' AND `左下纬度` <= '{3}' AND `左下经度` <= '{1}'", minTileLB[2], minTileLB[1], minTileLB[3], minTileLB[0]));
            //    DataTable dtable = new DataTable();
            //    dtable = resultTable.Copy();
            //    dtable.Clear();
            //    for (int l = 0; l < dr.Count(); l++)
            //    {
            //        dtable.Rows.Add(dr[l].ItemArray);
            //    }
            //    DataView dv = dtable.DefaultView;
            //    dv.Sort = "接收时间 Desc";
            //   DataTable ds = dv.ToTable();
                      
            //    for (int i = 0; i < ds.Rows.Count; i++)
            //    {
            //        double maxLa = Math.Max(Double.Parse(ds.Rows[i]["右上纬度"].ToString()), Double.Parse(ds.Rows[i]["左上纬度"].ToString()));
            //        double minLa = Math.Min(Double.Parse(ds.Rows[i]["左下纬度"].ToString()), Double.Parse(ds.Rows[i]["右下纬度"].ToString()));
            //        double maxLo = Math.Max(Double.Parse(ds.Rows[i]["右上经度"].ToString()), Double.Parse(ds.Rows[i]["右下经度"].ToString()));
            //        double minLo = Math.Min(Double.Parse(ds.Rows[i]["左上经度"].ToString()), Double.Parse(ds.Rows[i]["左下经度"].ToString()));
            //        double differLat = Double.Parse(ds.Rows[i]["右上纬度"].ToString()) - Double.Parse(ds.Rows[i]["左上纬度"].ToString());
            //        double differLon = Double.Parse(ds.Rows[i]["右上经度"].ToString()) - Double.Parse(ds.Rows[i]["左上经度"].ToString());
            //        double sum = differLat * differLat + differLon * differLon;
            //        double latlon = Math.Sqrt(sum);
            //        double averageLon = latlon / 256;
            //        double diL Parse(ds.Rows[i]["左上经度"].ToString()) - Double.Parse(ds.Rows[i]["左下经度"].ToString());
            //        double differLeftLat = Double.Parse(ds.Rows[i]["左上纬度"].ToString()) - Double.Parse(ds.Rows[i]["左下纬度"].ToString());
            //        double Leftsum = differLeftLat * differLeftLat + differLeftLon * differLeftLon;
            //        double Leftlatlon = Math.Sqrt(Leftsum);
            //        double LeftaverageLon = Leftlatlon / 256;
            //        double k1 = (maxLa - Double.Parse(ds.Rows[i]["左下纬度"].ToString())) / (Double.Parse(ds.Rows[i]["左上经度"].ToString()) - minLo);
            //        differLeftLat = maxLa - Double.Parse(ds.Rows[i]["左下纬度"].ToString());
            //        string code = ds.Rows[i]["数据编码"].ToString();
            //        string cloudP = ds.Rows[i]["云量百分比"].ToString();
            //        Bitmap image = new Bitmap(bmpPath + code + ".jpg");
            //        #region
            //        //if (Int32.Parse(cloudP) == 0)
            //        //{
            //        //    //if (!resultTable.Select(string.Format("数据编码='{0}'", code)).Length.Equals(1))
            //        //    //{
            //        //    ImgCount++;
            //        //        resultTable.Rows.Add(ds.Rows[i].ItemArray);
            //        //        int Imgwidth = (Int32)((minTileLB[3] - minTileLB[1]) / averageLon) + 1;
            //        //        int Imgheight = (Int32)((minTileLB[2] - minTileLB[0]) / LeftaverageLon) + 1;
            //        //        Bitmap bmpNew = new Bitmap(Imgwidth, Imgheight, PixelFormat.Format24bppRgb);
            //        //        int pixX = 0;
            //        //        int pixY = 0;
            //        //        for (double lat = minTileLB[2]; lat > minTileLB[0]; lat = lat - LeftaverageLon)
            //        //        {
            //        //            for (double lon = minTileLB[1]; lon < minTileLB[3]; lon = lon + averageLon)
            //        //            {
            //        //                List<int> pt = LatLonToPixl(resultTable, lat, lon);
            //        //                int red = image.GetPixel(pt[1], pt[0]).R;
            //        //                int green = image.GetPixel(pt[1], pt[0]).G;
            //        //                int blue = image.GetPixel(pt[1], pt[0]).B;
            //        //                bmpNew.SetPixel(pixX, pixY, Color.FromArgb(red, green, blue));
            //        //                pixX++;
            //        //            }
            //        //            pixX = 0;
            //        //            pixY++;
            //        //        }
            //        //        bmpNew.Save(bmpPath + code + "Cut"+ImgCount+".jpg", ImageFormat.Jpeg);
            //        //        image.Dispose();
            //        //        resultTable.Clear();
            //        //        string p = bmpPath + code + "Cut" + ImgCount + ".jpg";
            //        //        byte opacity = 255;
            //        //        //bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, minLa, maxLa, minLo, maxLo, 0, opacity, p);
            //        //        bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, minTileLB[0], minTileLB[2], minTileLB[1], minTileLB[3], 0, opacity, p);
            //        //    //}
            //        //    break;
            //        //}
            //        #endregion

            //        #region
            //        //X坐标
            //     //   double d1 = k1 * (minTileLB[1] - minLo) - (maxLa - minTileLB[2]) + differLeftLat;
            //     //   if (d1 < 0)
            //     //   {
            //     //       d1 = -d1;
            //     //   }
            //     //   double sum1 = 1 + k1 * k1;
            //     //   double d11 = Math.Sqrt(sum1);
            //     //   double lx = d1 / d11;
            //     //   int upN = (Int32)(lx / averageLon);
            //     //   double d2 = k1 * (minTileLB[3] - minLo) - (maxLa - minTileLB[2]) + differLeftLat;
            //     //   if (d2 < 0)
            //     //   {
            //     //       d2 = -d2;
            //     //   }
            //     //   double x2 = d2 / d11;
            //     //   int upN2 = (Int32)(x2 / averageLon);
            //     //   double d3 = k1 * (minTileLB[3] - minLo) - (maxLa - minTileLB[0]) + differLeftLat;
            //     //   if (d3 < 0)
            //     //   {
            //     //       d3 = -d3;
            //     //   }
            //     //   double x3 = d3 / d11;
            //     //   int upN3 = (Int32)(x3 / averageLon);
            //     //   double d4 = k1 * (minTileLB[1] - minLo) - (maxLa - minTileLB[0]) + differLeftLat;
            //     //   if (d4 < 0)
            //     //   {
            //     //       d4 = -d4;
            //     //   }
            //     //   double x4 = d4 / d11;
            //     //   int upN4 = (Int32)(x4 / averageLon);
            //     //   //Y坐标
            //     //   double dLat = maxLa - Double.Parse(ds.Rows[i]["右上纬度"].ToString());
            //     //   double b = dLat * (Double.Parse(ds.Rows[i]["左上经度"].ToString()) - minLo);
            //     //   double sum2 = dLat * dLat + differLon * differLon;
            //     //   double k2 = Math.Sqrt(sum2);
            //     //   double c1 = dLat * (minTileLB[1] - minLo) - differLon * (maxLa - minTileLB[2]) - b;
            //     //   if (c1 < 0)
            //     //   {
            //     //       c1 = -c1;
            //     //   }
            //     //   double y1 = c1 / k2;
            //     //   int M1 = (Int32)(y1 / LeftaverageLon);
            //     //   double c2 = dLat * (minTileLB[3] - minLo) - differLon * (maxLa - minTileLB[2]) - b;
            //     //   if (c2 < 0)
            //     //   {
            //     //       c2 = -c2;
            //     //   }
            //     //   double y2 = c2 / k2;
            //     //   int M2 = (Int32)(y2 / LeftaverageLon);
            //     //   double c3 = dLat * (minTileLB[3] - minLo) - differLon * (maxLa - minTileLB[0]) - b;
            //     //   if (c3 < 0)
            //     //   {
            //     //       c3 = -c3;
            //     //   }
            //     //   double y3 = c3 / k2;
            //     //   int M3 = (Int32)(y3 / LeftaverageLon);
            //     //   double c4 = dLat * (minTileLB[1] - minLo) - differLon * (maxLa - minTileLB[0]) - b;
            //     //   if (c4 < 0)
            //     //   {
            //     //       c4 = -c4;
            //     //   }
            //     //   double y4 = c4 / k2;
            //     //   int M4 = (Int32)(y4 / LeftaverageLon);
            //     //   System.Drawing.Point[] tmp = new System.Drawing.Point[] { };
            //     //   tmp = new System.Drawing.Point[] {
            //     //new System.Drawing.Point(M1-1,upN-1),
            //     //new System.Drawing.Point(M2-1,upN2-1),
            //     //new System.Drawing.Point(M3-1,upN3-1),
            //     //new System.Drawing.Point(M4-1,upN4-1)};
            //        #endregion
            //        Bitmap image1 = new Bitmap(bmpPath + code + ".jpg");
            //        Rectangle rect1 = new Rectangle(0, 0, image1.Width, image1.Height);
            //        System.Drawing.Imaging.BitmapData bmpData1 = image1.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, image1.PixelFormat);
            //        IntPtr ptr1 = bmpData1.Scan0;
            //        int bytes1 = Math.Abs(bmpData1.Stride) * image1.Height;
            //        byte[] rgbValues1 = new byte[bytes1];
            //        System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes1);
            //        List<double> rgbList = new List<double>();
            //        for (int k = 0; k < rgbValues1.Length; k=k+3)
            //        {
            //            rgbList.Add(Math.Round(rgbValues1[k]* 0.114 + rgbValues1[k + 1] * 0.589 + rgbValues1[k + 2] * 0.299));
            //        }
            //        rgbList.Sort();
            //        rgbList.Reverse();
            //        var g = rgbList.GroupBy(j => j);
            //        List<double> keyL = new List<double>();
            //        List<int> valueL = new List<int>();
            //        double sumCount = 0;
            //        double thresholdValue = 0;
            //        foreach (var g1 in g)
            //        {
            //            sumCount += g1.Count();
            //            double x = Math.Round(sumCount / Double.Parse(rgbList.Count.ToString()) * 100);
            //            if (Math.Round(sumCount / Double.Parse(rgbList.Count.ToString()) * 100) == Double.Parse(cloudP) )
            //            {
            //                thresholdValue = g1.Key;
            //                break;
            //            }
            //        }
            //        //CutImge(code, tmp);
            //        resultTable.Clear();
            //        resultTable.Rows.Add(ds.Rows[i].ItemArray);
            //        int Imgwidth = (Int32)((minTileLB[3] - minTileLB[1]) / averageLon) + 1;
            //        int Imgheight = (Int32)((minTileLB[2] - minTileLB[0]) / LeftaverageLon) + 1;
            //        Bitmap bmpNew = new Bitmap(Imgwidth, Imgheight, PixelFormat.Format24bppRgb);
            //        int pixX = 0;
            //        int pixY = 0;
            //        double count = 0;
            //        bool Jump = false;
            //        for (double lat = minTileLB[2]; lat > minTileLB[0]; lat = lat - LeftaverageLon)
            //        {
            //            for (double lon = minTileLB[1]; lon < minTileLB[3]; lon = lon + averageLon)
            //            {
            //                List<int> pt = LatLonToPixl(resultTable, lat, lon);
            //                if (pt[0] > 255||pt[1]>255||pt[1]<0)
            //                {
            //                    Jump = true;
            //                    break;
            //                }
            //                int red = image.GetPixel(pt[1], pt[0]).R;
            //                int green = image.GetPixel(pt[1], pt[0]).G;
            //                int blue = image.GetPixel(pt[1], pt[0]).B;
            //                if (Math.Round(blue * 0.114 + green * 0.589 + red * 0.299) >= thresholdValue)
            //                {
            //                    count++;
            //                }
            //                bmpNew.SetPixel(pixX, pixY, Color.FromArgb(red, green, blue));
            //                pixX++;
            //            }
            //            pixX = 0;
            //            pixY++;
            //        }
            //        if (!Jump)
            //        {
            //            double perc = count / Double.Parse((Imgwidth * Imgheight).ToString());
            //            if (Math.Round(perc) * 100 <= 10)
            //            {
            //                ImgCount++;
            //                bmpNew.Save(@"E:\\thumb\" + code + "Cut" + ImgCount + ".jpg", ImageFormat.Jpeg);
            //                image.Dispose();
            //                string p = @"E:\\thumb\" + code + "Cut" + ImgCount + ".jpg";
            //                byte opacity = 255;
            //                bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, minTileLB[0], minTileLB[2], minTileLB[1], minTileLB[3], 0, opacity, p);
            //            }
            //            break;
            //        }
            //        #region
            //        //Bitmap resultImage = new Bitmap(image.Width, image.Height);
            //        ////建立缓冲图片
            //        //Graphics gr = Graphics.FromImage(resultImage);
            //        //gr.Clear(Color.FromArgb(0, Color.Transparent));
            //        //Rectangle resultRectangle = new Rectangle(0, 0, image.Width, image.Height);
            //        //int width = image.Width;
            //        //int height = image.Height;
            //        //Region reg = new Region();
            //        //reg.MakeEmpty();
            //        //GraphicsPath gp = new GraphicsPath();
            //        ////System.Drawing.Point[] myArray = GetArrayForViewStyle();
            //        //gp.AddPolygon(tmp);
            //        //reg.Union(gp);

            //        //gr.SetClip(reg, CombineMode.Replace);
            //        //gr.DrawImage(image, resultRectangle);
            //        //Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);
            //        //gp.Dispose();
            //        //reg.Dispose();
            //        //System.Drawing.Imaging.BitmapData bmpData = resultImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, resultImage.PixelFormat);
            //        //IntPtr ptr = bmpData.Scan0;
            //        //int bytes = Math.Abs(bmpData.Stride) * image.Height;
            //        //byte[] rgbValues = new byte[bytes];
            //        //List<int> tempList = new List<int>();
            //        //List<int> resList = new List<int>();
            //        //// Copy the RGB values into the array.复制RGB值到数组  
            //        //System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            //        //double count = 0;
            //        //for (int j = 0; j < rgbValues.Length; j=j+3)
            //        //{
            //        //    if (rgbValues[j] != 0 && rgbValues[j+1] != 0 && rgbValues[j+2] != 0)
            //        //    {
            //        //        tempList.Add(rgbValues[j]);
            //        //        tempList.Add(rgbValues[j+1]);
            //        //        tempList.Add(rgbValues[j+2]);
            //        //        if (Math.Round(rgbValues[j] * 0.114 + rgbValues[j + 1] * 0.589 + rgbValues[j + 2] * 0.299)>=thresholdValue)
            //        //        {
            //        //            count++;
            //        //        }
            //        //    }
            //        //}
            //        ////image.Dispose();
            //        //double perc =count / Double.Parse(tempList.Count.ToString());
            //        //if (Math.Round(perc) * 100 <= 3)
            //        //{
            //        //    ImgCount++;
                        
            //        //        resultTable.Rows.Add(ds.Rows[i].ItemArray);
            //        //        int Imgwidth =(Int32)((minTileLB[3] - minTileLB[1]) / averageLon)+1;
            //        //        int Imgheight = (Int32)((minTileLB[2] - minTileLB[0]) / LeftaverageLon)+1;
            //        //        Bitmap bmpNew = new Bitmap(Imgwidth, Imgheight,PixelFormat.Format24bppRgb);
            //        //        //BitmapData bitMD = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //        //        //IntPtr ptrNew = bitMD.Scan0;
            //        //        //int byteNew = Math.Abs(bitMD.Stride) * bmpNew.Height;
            //        //        //byte[] byteData1 = new byte[byteNew];
            //        //        int pixX = 0;
            //        //        int pixY = 0;
            //        //        for (double lat = minTileLB[2]; lat > minTileLB[0]; lat = lat - LeftaverageLon)
            //        //        {
            //        //            for (double lon = minTileLB[1]; lon < minTileLB[3]; lon = lon + averageLon)
            //        //            {
            //        //                List<int> pt = LatLonToPixl(resultTable, lat, lon);
            //        //                if (pt[0] > 255)
            //        //                {
            //        //                    pt[0] = 255;
            //        //                    mm++;
            //        //                }
            //        //                int red = image.GetPixel(pt[1], pt[0]).R;
            //        //                int green = image.GetPixel(pt[1], pt[0]).G;
            //        //                int blue = image.GetPixel(pt[1], pt[0]).B;
            //        //                bmpNew.SetPixel(pixX, pixY, Color.FromArgb(red, green, blue));
            //        //                pixX++;
            //        //            }
            //        //            pixX = 0;
            //        //            pixY++;
            //        //        }

            //        //        bmpNew.Save(bmpPath + code + "Cut"+ImgCount+".jpg", ImageFormat.Jpeg);
            //        //        image.Dispose();
            //        //        resultTable.Clear();
            //        //        string p = bmpPath + code +"Cut"+ImgCount+".jpg";
            //        //        byte opacity = 255;
            //        //        bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, minTileLB[0], minTileLB[2], minTileLB[1], minTileLB[3], 0, opacity, p);
                       
            //        //    break;
            //        //}
            //    }
            //}
            
            ////System.Data.DataSet dset = new System.Data.DataSet();
            ////dset.Tables.Add(resultTable);
            ////queryResponse.recordSet = dset;
            //ClearExtentLyr();
            ////DrawSpacialExtent();
            ////MessageBox.Show(ImgCount.toString());
            ////for (int i = 0; i < num; i++)
            ////{
            ////    IGeometry ig = getGeomFromRow(sourceDt.Rows[i]);
            ////    if (f.Intersects(ig))
            ////    {
            ////        f = f.Difference(ig);
            ////        if (f != null)
            ////        {
            ////            resultTable.Rows.Add(sourceDt.Rows[i].ItemArray);
            ////        }
            ////        else
            ////        {
            ////            resultTable.Rows.Add(sourceDt.Rows[i].ItemArray);
            ////            break;
            ////        }
            ////    }
            ////}
            ////System.Data.DataSet ds = new System.Data.DataSet();
            ////ds.Tables.Add(resultTable);
            ////queryResponse.recordSet = ds;
            ////DrawSpacialExtent();
            //DateTime dt1 = DateTime.Now;
            //TimeSpan ts = dt1 - dt;
            ////MessageBox.Show(mm.ToString());
            //        #endregion
        }
        private List<int> LatLonToPixl(DataTable ds, double lat, double lon)
        {
            double maxLa = Math.Max(Double.Parse(ds.Rows[0]["右上纬度"].ToString()), Double.Parse(ds.Rows[0]["左上纬度"].ToString()));
            double minLa = Math.Min(Double.Parse(ds.Rows[0]["左下纬度"].ToString()), Double.Parse(ds.Rows[0]["右下纬度"].ToString()));
            double maxLo = Math.Max(Double.Parse(ds.Rows[0]["右上经度"].ToString()), Double.Parse(ds.Rows[0]["右下经度"].ToString()));
            double minLo = Math.Min(Double.Parse(ds.Rows[0]["左上经度"].ToString()), Double.Parse(ds.Rows[0]["左下经度"].ToString()));

            double differLat = Double.Parse(ds.Rows[0]["右上纬度"].ToString()) - Double.Parse(ds.Rows[0]["左上纬度"].ToString());
            double differLon = Double.Parse(ds.Rows[0]["右上经度"].ToString()) - Double.Parse(ds.Rows[0]["左上经度"].ToString());
            double sum = differLat * differLat + differLon * differLon;
            double latlon = Math.Sqrt(sum);
            double averageLon = latlon / 256;
            double differLeftLon = Double.Parse(ds.Rows[0]["左上经度"].ToString()) - Double.Parse(ds.Rows[0]["左下经度"].ToString());
            double differLeftLat = Double.Parse(ds.Rows[0]["左上纬度"].ToString()) - Double.Parse(ds.Rows[0]["左下纬度"].ToString());
            double Leftsum = differLeftLat * differLeftLat + differLeftLon * differLeftLon;
            double Leftlatlon = Math.Sqrt(Leftsum);
            double LeftaverageLon = Leftlatlon / 256;
            double k1 = (maxLa - Double.Parse(ds.Rows[0]["左下纬度"].ToString())) / (Double.Parse(ds.Rows[0]["左上经度"].ToString()) - minLo);
            //double k1 = differLeftLat / differLeftLon;
            differLeftLat = maxLa - Double.Parse(ds.Rows[0]["左下纬度"].ToString());
            #region
            ////Y坐标
            //double d1 = k1 * (lon - minLo) - (maxLa - lat) + differLeftLat;
            //if (d1 < 0)
            //{
            //    d1 = -d1;
            //}
            //double sum1 = 1 + k1 * k1;
            //double d11 = Math.Sqrt(sum1);
            //double lx = d1 / d11;
            //int upN = (Int32)(lx / averageLon) - 1;

            ////X坐标
            //double dLat = maxLa - Double.Parse(ds.Rows[0]["右上纬度"].ToString());
            //double b = dLat * (Double.Parse(ds.Rows[0]["左上经度"].ToString()) - minLo);
            //double sum2 = dLat * dLat + differLon * differLon;
            //double k2 = Math.Sqrt(sum2);
            //double c1 = dLat * (lon - minLo) - differLon * (maxLa - lat) - b;
            //if (c1 < 0)
            //{
            //    c1 = -c1;
            //}
            //double y1 = c1 / k2;
            //int M1 = (Int32)(y1 / LeftaverageLon) - 1;
            //List<int> list = new List<int>();
            //list.Add(M1);//M1
            //list.Add(upN);
            //return list;
            #endregion
            #region
            //Y坐标
            double d1 = k1 * (lon - minLo) - (maxLa - lat) + differLeftLat;
            if (d1 < 0)
            {
                d1 = -d1;
            }
            double sum1 = 1 + k1 * k1;
            double d11 = Math.Sqrt(sum1);
            double lx = d1 / d11;
            int upN = (Int32)(lx / averageLon) - 40;//-32

            //X坐标
            double dLat = (maxLa - Double.Parse(ds.Rows[0]["右上纬度"].ToString())) / (maxLo - Double.Parse(ds.Rows[0]["左上经度"].ToString()));
            double b = dLat * (minLo - Double.Parse(ds.Rows[0]["左上经度"].ToString()));
            double sum2 = 1 + dLat * dLat;//正确的
            double k2 = Math.Sqrt(sum2);
            double c1 = dLat * (lon - minLo) - (maxLa - lat) + b;//正确的
            if (c1 < 0)
            {
                c1 = -c1;
            }
            double y1 = c1 / k2;
            int M1 = (Int32)(y1 / LeftaverageLon)-1;
            List<int> list = new List<int>();
            list.Add(M1);//M1
            list.Add(upN);
            return list;
            #endregion
        }
        // string bmpPath = @"E:\zhsjk\Thumb\";20170320
        string bmpPath = StoragePath.StoreBasePath + "\\" + "Thumb" + "\\";
        private void CutImge(string code, System.Drawing.Point[] myArray)
        {
            int end = code.LastIndexOf("-");
            string chushu = code.Substring(end + 1, code.Length - end - 1);
            string shang = Convert.ToString(Convert.ToInt32(chushu) / 1000);
            int ImgCount = 1;
            Bitmap image = new Bitmap(bmpPath + shang + "\\" + code + ".jpg");
            Bitmap resultImage = new Bitmap(image.Width, image.Height);
            //建立缓冲图片
            Graphics gr = Graphics.FromImage(resultImage);
            gr.Clear(Color.FromArgb(0, Color.Transparent));
            Rectangle resultRectangle = new Rectangle(0, 0, image.Width, image.Height);
            int width = image.Width;
            int height = image.Height;
            Region reg = new Region();
            reg.MakeEmpty();
            GraphicsPath gp = new GraphicsPath();
            //System.Drawing.Point[] myArray = GetArrayForViewStyle();
            gp.AddPolygon(myArray);
            reg.Union(gp);

            gr.SetClip(reg, CombineMode.Replace);
            gr.DrawImage(image, resultRectangle);
            Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);
            string gewangthumbpath = string.Format(@"{0}\{1}\{2}", Application.StartupPath, @"gewangthumb", code);//新加
            if (!Directory.Exists(gewangthumbpath))
            {
                Directory.CreateDirectory(gewangthumbpath);
            }
            int ImgCount1 = ImgCount++;
            resultImage.Save(gewangthumbpath + "\\" + code + "Cut" + ImgCount1 + ".jpg", ImageFormat.Jpeg);//新加
           // resultImage.Save(@"E:\thumb\21.jpg", ImageFormat.Jpeg);
            gp.Dispose();
            reg.Dispose();
        }
         //List<Envelope> envList = new List<Envelope>();//目标格网
         //List<string[]> AOITile = new List<string[]>();//目标瓦片
        private string TileLevel = "8";
        private List<double[]> TileGeoTrans(int a, int b, double[] gt)
        {
            List<double[]> tilegeolist = new List<double[]>();
            double[] geoxy1 = new double[2];//存放转换后的地理坐标
            double[] geoxy2 = new double[2];
            double[] geoxy3 = new double[2];
            double[] geoxy4 = new double[2];
            geoxy1[1] = gt[0] + gt[1] * a + gt[2] * b;//纬度y
            geoxy1[0] = gt[3] + gt[4] * a + gt[5] * b;//经度x
            tilegeolist.Add(geoxy1);//左下
            //geoxy2[1] =geoxy1[1]+gt[1];
            //geoxy2[0] = geoxy1[0];
            //tilegeolist.Add(geoxy2);//左上
            geoxy3[1] = geoxy1[1] + gt[1];
            geoxy3[0] = geoxy1[0] + gt[1];
            tilegeolist.Add(geoxy3);//右上
            //geoxy4[1] = geoxy1[1];
            //geoxy4[0] = geoxy1[0] + gt[1];
            //tilegeolist.Add(geoxy4);//右下
            return tilegeolist;
        }
        //private void GetAOITilesCR(IFeature feature, List<string[]> aoiTile)//进度条问题，再考虑
        //{
        //    //DateTime beforDT = System.DateTime.Now;
        //    //得到外接矩形包含的行列号
        //    string[] feaEnvelope = new string[] { 
        //            feature.Envelope.Minimum.Y.ToString(),
        //            feature.Envelope.Minimum.X.ToString() , 
        //            feature.Envelope.Maximum.Y.ToString(), 
        //            feature.Envelope.Maximum.X.ToString() };
        //    int[] colRow = DirectlyAddresser.GetRowAndColum(feaEnvelope, TileLevel);//最小行，最小列，最大行，最大列
        //    int rownum = colRow[2] - colRow[0] + 1;
        //    int colnum = colRow[3] - colRow[1] + 1;
        //    //获得六参数
        //    double[] GT = new double[6];
        //    double resolution = double.Parse(DirectlyAddresser.GetDegreeByStrLv(TileLevel));
        //    GT[1] = GT[5] = resolution;
        //    string[] minTileRC = { colRow[0].ToString(), colRow[1].ToString() };
        //    double[] minTileLB = DirectlyAddresser.GetLatAndLong(minTileRC, TileLevel);//最小纬度，最小经度，最大纬度，最大经度
        //    GT[0] = minTileLB[0];//最小纬度
        //    GT[3] = minTileLB[1];//最小经度
        //    GT[2] = GT[4] = 0;
            
        //    //判断多边形和瓦片矩形是否相交，相交即为区域瓦片 注意point（x，y）x为精度，y为纬度
        //    for (int i = 0; i < rownum; i++)
        //        for (int j = 0; j < colnum; j++)
        //        {
        //            try
        //            {
        //                List<double[]> Tile4Geolist = TileGeoTrans(i, j, GT);
        //                Coordinate coord1 = new Coordinate(Tile4Geolist[0][0], Tile4Geolist[0][1]);
        //                Coordinate coord2 = new Coordinate(Tile4Geolist[1][0], Tile4Geolist[1][1]);
        //                Envelope enve = new Envelope(coord1, coord2);
        //                bool overlap = feature.Intersects(enve);
        //                if (overlap)
        //                {
        //                    string[] tile = new string[2];
        //                    tile[0] = (i + colRow[0]).ToString();
        //                    tile[1] = (j + colRow[1]).ToString();
        //                    aoiTile.Add(tile);
        //                    envList.Add(enve);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                MessageBox.Show(e.Message.ToString());
        //                break;
        //            }
        //        }
        //}
        
        //public void GetAOITilesGrid(string gridPath, IFeature ifea)
        //{
        //    this.GetAOITilesCR(ifea, AOITile);
        //}
     

        /// <summary>
        /// 重新写了全覆盖
        /// </summary>
        /// <param name="f"></param>
        private void getCutCopy(IFeature f)
        {
            resultTable = sourceDt.Copy();
            resultTable.Clear();
            DataTable temp = new DataTable();
            temp = resultTable;

            string p = "";
            int mm = 0;
            int nn = 0;
            int ImgCount = 0;
            envList = new List<Envelope>();//目标格网所有矩形区域
            AOITile = new List<string[]>();//目标格网所有行列号
            GetAOITilesGrid(@"F:\QRST_APPs\QRST_LANFANG\Lanfang_AppSysWinForm\bin\Debug\区域线化图\五层十五级格网\25米.shp", f);

          
            foreach (var item in AOITile)
            {
                //得到每个格网的经纬度范围
                double[] minTileLB = DirectlyAddressing.GetLatAndLong(item, TileLevel);
                //得到覆盖该格网的所有遥感影像
                DataRow[] dr = sourceDt.Select(string.Format("`左上纬度` >= '{0}' AND `左上经度` <= '{1}' AND `右上纬度` >= '{0}' AND `右上经度` >= '{2}' AND `右下纬度` <= '{3}' AND `右下经度` >= '{2}' AND `左下纬度` <= '{3}' AND `左下经度` <= '{1}'", minTileLB[2], minTileLB[1], minTileLB[3], minTileLB[0]));
                DataTable dtable = new DataTable();
                dtable = sourceDt.Copy();
                dtable.Clear();
                for (int i = 0; i < dr.Count(); i++)
                {
                    dtable.Rows.Add(dr[i].ItemArray);
                }
                DataView dv = dtable.DefaultView;
                dv.Sort = "接收时间 Desc";
                DataTable ds = dv.ToTable();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    string dataname = ds.Rows[i]["数据名称"].ToString();
                    if (issame(resultTable1, dataname))
                    {
                        DataRow rr = ds.Rows[i];
                        resultTable1.Rows.Add(rr.ItemArray);

                        string code = ds.Rows[i]["数据编码"].ToString();
                        if (code == "0001-EVDB-32-44" && i == 2)
                        {
                            int end = code.LastIndexOf("-");
                            string chushu = code.Substring(end + 1, code.Length - end - 1);
                            string shang = Convert.ToString(Convert.ToInt32(chushu) / 1000);
                            string rootPath = StoragePath.StoreBasePath;
                            p = rootPath + "Thumb" + "\\" + shang + "\\" + code + ".jpg";
                            byte opacity1 = 255;
                            bool addLayer1 = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, Double.Parse(ds.Rows[i]["左上纬度"].ToString()), Double.Parse(ds.Rows[i]["左上经度"].ToString()), Double.Parse(ds.Rows[i]["左下纬度"].ToString()), Double.Parse(ds.Rows[i]["左下经度"].ToString()), Double.Parse(ds.Rows[i]["右下纬度"].ToString()), Double.Parse(ds.Rows[i]["右下经度"].ToString()), Double.Parse(ds.Rows[i]["右上纬度"].ToString()), Double.Parse(ds.Rows[i]["右上经度"].ToString()), 0, opacity1, p);

                        }
                         
                    }
                    else
                    { }
                }

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    double maxLa = Math.Max(Double.Parse(ds.Rows[i]["右上纬度"].ToString()), Double.Parse(ds.Rows[i]["左上纬度"].ToString()));
                    double minLa = Math.Min(Double.Parse(ds.Rows[i]["左下纬度"].ToString()), Double.Parse(ds.Rows[i]["右下纬度"].ToString()));
                    double maxLo = Math.Max(Double.Parse(ds.Rows[i]["右上经度"].ToString()), Double.Parse(ds.Rows[i]["右下经度"].ToString()));
                    double minLo = Math.Min(Double.Parse(ds.Rows[i]["左上经度"].ToString()), Double.Parse(ds.Rows[i]["左下经度"].ToString()));
                    double differLat = Double.Parse(ds.Rows[i]["右上纬度"].ToString()) - Double.Parse(ds.Rows[i]["左上纬度"].ToString());
                    double differLon = Double.Parse(ds.Rows[i]["右上经度"].ToString()) - Double.Parse(ds.Rows[i]["左上经度"].ToString());
                    double sum = differLat * differLat + differLon * differLon;
                    double latlon = Math.Sqrt(sum);//p1p2的距离
                    double averageLon = latlon / 256;//每个像素宽的跨度为∆w
                    double differLeftLon = Double.Parse(ds.Rows[i]["左上经度"].ToString()) - Double.Parse(ds.Rows[i]["左下经度"].ToString());
                    double differLeftLat = Double.Parse(ds.Rows[i]["左上纬度"].ToString()) - Double.Parse(ds.Rows[i]["左下纬度"].ToString());
                    double Leftsum = differLeftLat * differLeftLat + differLeftLon * differLeftLon;
                    double Leftlatlon = Math.Sqrt(Leftsum);//p1p4的距离
                    double LeftaverageLon = Leftlatlon / 256;//每个像素高的跨度为∆h

                    string code = ds.Rows[i]["数据编码"].ToString();
                    string cloudP = ds.Rows[i]["云量百分比"].ToString();

                    #region 求格网在快试图中的位置开始
                    //X坐标

                    double k1 = (maxLa - Double.Parse(ds.Rows[i]["左下纬度"].ToString())) / (Double.Parse(ds.Rows[i]["左上经度"].ToString()) - minLo);//p1p4的斜率
                    differLeftLat = maxLa - Double.Parse(ds.Rows[i]["左下纬度"].ToString());//p4的y值
                    double d1 = k1 * (minTileLB[1] - minLo) - (maxLa - minTileLB[2]) + differLeftLat;//30.75,117.5,31.0,117.75
                    if (d1 < 0)
                    {
                        d1 = -d1;
                    }
                    double sum1 = 1 + k1 * k1;
                    double d11 = Math.Sqrt(sum1);
                    double lx = d1 / d11;//格网的P1’到原始影像p1p4的距离（度）
                    int upN = (Int32)(lx / averageLon);//P1’在快视图中距离左边缘的像素

                    double d2 = k1 * (minTileLB[3] - minLo) - (maxLa - minTileLB[2]) + differLeftLat;
                    if (d2 < 0)
                    {
                        d2 = -d2;
                    }
                    double x2 = d2 / d11;
                    int upN2 = (Int32)(x2 / averageLon);//P2’在快视图中距离左边缘的宽度

                    double d3 = k1 * (minTileLB[3] - minLo) - (maxLa - minTileLB[0]) + differLeftLat;
                    if (d3 < 0)
                    {
                        d3 = -d3;
                    }
                    double x3 = d3 / d11;
                    int upN3 = (Int32)(x3 / averageLon);//P3’在快视图中距离左边缘的宽度

                    double d4 = k1 * (minTileLB[1] - minLo) - (maxLa - minTileLB[0]) + differLeftLat;
                    if (d4 < 0)
                    {
                        d4 = -d4;
                    }
                    double x4 = d4 / d11;
                    int upN4 = (Int32)(x4 / averageLon);//P4’在快视图中距离左边缘的宽度


                    //Y坐标
                    double dLat = (maxLa - Double.Parse(ds.Rows[i]["右上纬度"].ToString())) / (maxLo - Double.Parse(ds.Rows[i]["左上经度"].ToString()));//正确的//p1p2的斜率修改了20170412
                    double b = dLat * (minLo - Double.Parse(ds.Rows[i]["左上经度"].ToString()));//正确的               
                    double sum2 = 1 + dLat * dLat;//正确的
                    double k2 = Math.Sqrt(sum2);
                    double c1 = dLat * (minTileLB[1] - minLo) - (maxLa - minTileLB[2]) + b;//正确的
                    if (c1 < 0)
                    {
                        c1 = -c1;
                    }
                    double y1 = c1 / k2;
                    int M1 = (Int32)(y1 / LeftaverageLon);

                    double c2 = dLat * (minTileLB[3] - minLo) - (maxLa - minTileLB[2]) + b;//正确的
                    if (c2 < 0)
                    {
                        c2 = -c2;
                    }
                    double y2 = c2 / k2;
                    int M2 = (Int32)(y2 / LeftaverageLon);

                    double c3 = dLat * (minTileLB[3] - minLo) - (maxLa - minTileLB[0]) + b;//正确的
                    if (c3 < 0)
                    {
                        c3 = -c3;
                    }
                    double y3 = c3 / k2;
                    int M3 = (Int32)(y3 / LeftaverageLon);

                    double c4 = dLat * (minTileLB[1] - minLo) - (maxLa - minTileLB[0]) + b;//正确的
                    if (c4 < 0)
                    {
                        c4 = -c4;
                    }
                    double y4 = c4 / k2;
                    int M4 = (Int32)(y4 / LeftaverageLon);

                    System.Drawing.Point[] tmp = new System.Drawing.Point[] { };
                    tmp = new System.Drawing.Point[] {
                 new System.Drawing.Point(M1-1,upN-1),
                 new System.Drawing.Point(M2-1,upN2-1),
                 new System.Drawing.Point(M3-1,upN3-1),
                 new System.Drawing.Point(M4-1,upN4-1)};

                    //tmp = new System.Drawing.Point[] {
                    //new System.Drawing.Point(upN-1,M1-1),
                    //new System.Drawing.Point(upN2-1,M2-1),
                    //new System.Drawing.Point(upN3-1,M3-1),
                    //new System.Drawing.Point(upN4-1,M4-1)};
                   
                    #endregion

                    #region 查找快试图开始
                    //zsm 20170320
                    int end = code.LastIndexOf("-");
                    string chushu = code.Substring(end + 1, code.Length - end - 1);
                    string shang = Convert.ToString(Convert.ToInt32(chushu) / 1000);
                    string rootPath = StoragePath.StoreBasePath;
                    p = rootPath + "Thumb" + "\\" + shang + "\\" + code + ".jpg";
                   // if (code == "0001-EVDB-32-43")
                   // {
                        //byte opacity1 = 255;
                        //bool addLayer1 = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, Double.Parse(ds.Rows[i]["左上纬度"].ToString()), Double.Parse(ds.Rows[i]["左上经度"].ToString()), Double.Parse(ds.Rows[i]["左下纬度"].ToString()), Double.Parse(ds.Rows[i]["左下经度"].ToString()), Double.Parse(ds.Rows[i]["右下纬度"].ToString()), Double.Parse(ds.Rows[i]["右下经度"].ToString()), Double.Parse(ds.Rows[i]["右上纬度"].ToString()), Double.Parse(ds.Rows[i]["右上经度"].ToString()), 0, opacity1, p);


                    //}
                       
                    if (File.Exists(p))
                    { }
                    else
                    {
                        //MessageBox.Show("当前磁盘阵列中不存在该快试图，到历史磁盘阵列中查找!，做测试呢");
                        foreach (var itemd in StoragePath.StoreHistoryPath)
                        {
                            p = itemd + "Thumb" + "\\" + shang + "\\" + code + ".jpg";
                            if (File.Exists(p))
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                    }
                    if (!File.Exists(p))
                    {
                        continue;
                    }
                    #endregion

                    #region 求阈值开始
                    Bitmap image1 = new Bitmap(p);
                    Bitmap image = new Bitmap(p);
                    Rectangle rect1 = new Rectangle(0, 0, image1.Width, image1.Height);//20170414 创建所绘制图像的位置和大小 是指源图像中需要锁定那一块矩形区域进行处理 
                    System.Drawing.Imaging.BitmapData bmpData1 = image1.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, image1.PixelFormat);
                    IntPtr ptr1 = bmpData1.Scan0;////位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行  
                    //修改了0414
                    int bytes2 = Math.Abs(bmpData1.Stride) * image1.Height;//该行代码用不到
                    //byte[] rgbValues1 = new byte[bytes1];
                    int bytes1 = rect1.Width * 3 * rect1.Height;//Math.Abs(bmpData1.Stride) * image1.Height;
                    byte[] rgbValues1 = new byte[bytes1];

                    System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes1);
                    List<double> rgbList = new List<double>();//像素点的总个数
                    for (int k = 0; k < rgbValues1.Length; k = k + 3)
                    {
                        rgbList.Add(Math.Round(rgbValues1[k] * 0.114 + rgbValues1[k + 1] * 0.587 + rgbValues1[k + 2] * 0.299));// 0.589 有的是0.587
                    }
                    rgbList.Sort();
                    rgbList.Reverse();
                    var g = rgbList.GroupBy(j => j);
                    List<double> keyL = new List<double>();
                    List<int> valueL = new List<int>();
                    double sumCount = 0;//像素点大于阈值的个数
                    double thresholdValue = 0;//阈值
                    foreach (var g1 in g)
                    {
                        sumCount += g1.Count();
                        double x = Math.Round(sumCount / Double.Parse(rgbList.Count.ToString()) * 100);
                        if (Math.Round(sumCount / Double.Parse(rgbList.Count.ToString()) * 100) == Double.Parse(cloudP))
                        {
                            thresholdValue = g1.Key;//阈值有可能是100多 这样云检测不太好 zsm个人认为哦
                            break;
                        }
                    }
                    #endregion

                    #region 求截取局部图片开始
                    Bitmap resultImage = new Bitmap(image1.Width, image1.Height);
                    // 建立缓冲图片 取出快试图中局部图
                    Graphics gr = Graphics.FromImage(resultImage);
                    gr.Clear(Color.FromArgb(0, Color.Transparent));
                    Rectangle resultRectangle = new Rectangle(0, 0, image1.Width, image1.Height);
                    int width = image1.Width;
                    int height = image1.Height;
                    Region reg = new Region();
                    reg.MakeEmpty();
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddPolygon(tmp);
                    reg.Union(gp);
                    gr.SetClip(reg, CombineMode.Replace);
                    gr.DrawImage(image, resultRectangle);
                    Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);
                    gp.Dispose();
                    reg.Dispose();
                    #endregion

                    #region 判断局部图片是否有云开始
                    System.Drawing.Imaging.BitmapData bmpData = resultImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, resultImage.PixelFormat);
                    IntPtr ptr = bmpData.Scan0;
                    int bytes = Math.Abs(bmpData.Stride) * image1.Height;//ceshidaima
                    // int bytes = rect.Width * 3 * rect.Height;//Math.Abs(bmpData.Stride) * image.Height;
                    byte[] rgbValues = new byte[bytes];
                    //int countNum = bmp.Width * bmp.Height;
                    List<int> tempList = new List<int>();//红绿蓝的值
                    List<int> resList = new List<int>();
                    // Copy the RGB values into the array.复制RGB值到数组  
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
                    double count = 0;
                    for (int j = 0; j < rgbValues.Length; j = j + 3)
                    {
                        if (rgbValues[j] != 0 && rgbValues[j + 1] != 0 && rgbValues[j + 2] != 0)
                        {
                            tempList.Add(rgbValues[j]);
                            tempList.Add(rgbValues[j + 1]);
                            tempList.Add(rgbValues[j + 2]);
                            if (Math.Round(rgbValues[j] * 0.114 + rgbValues[j + 1] * 0.587 + rgbValues[j + 2] * 0.299) >= thresholdValue)//像素点的值大于阈值则改点为云
                            {
                                count++;
                            }
                        }
                    }
                    double perc = count / Double.Parse(tempList.Count.ToString());//count值一般都是几十 代表有几十个像素点为云 所以该值为0.0多少多少
                    if (Math.Round(perc) * 100 == 0)//四舍五入后都是0
                    {
                        ImgCount++;
                        list.Add(ds.Rows[i]["景序列号"].ToString());
                        temp.Rows.Add(ds.Rows[i].ItemArray);
                        int Imgwidth = (Int32)((minTileLB[3] - minTileLB[1]) / averageLon) + 1;//每次这个值是不唯一的？？？
                        int Imgheight = (Int32)((minTileLB[2] - minTileLB[0]) / LeftaverageLon) + 1;
                        Bitmap bmpNew = new Bitmap(Imgwidth, Imgheight, PixelFormat.Format24bppRgb);//获取图的位置不对，为什么？我怎么获取格网那一块的图呢，不太明白？
                        int pixX = 0;
                        int pixY = 0;
                        for (double lat = minTileLB[2]; lat > minTileLB[0]; lat = lat - LeftaverageLon)//最大纬度LeftaverageLon(31.25,120.75,31.5,121.0)
                        {
                            for (double lon = minTileLB[1]; lon < minTileLB[3]; lon = lon + averageLon)//最小经度
                            {
                                List<int> pt = LatLonToPixl(resultTable, lat, lon);
                                if (pt[0] > 255)
                                {
                                    pt[0] = 255;
                                    mm++;
                                }
                                if (pt[0] <0)
                                {
                                    pt[0] = 0;
                                    mm++;
                                }
                                if (pt[1]>255)
                                {
                                    pt[1] = 255;
                                    nn++;
                                    
                                }
                                if (pt[1] <0)
                                {
                                    pt[1] = 0;
                                    nn++;

                                }
                                int red = image.GetPixel(pt[1], pt[0]).R;
                                int green = image.GetPixel(pt[1], pt[0]).G;
                                int blue = image.GetPixel(pt[1], pt[0]).B;
                                bmpNew.SetPixel(pixX, pixY, Color.FromArgb(red, green, blue));
                                pixX++;

                            }
                            pixX = 0;
                            pixY++;
                        }



                        string gewangthumbpath = string.Format(@"{0}\{1}\{2}", Application.StartupPath, @"gewangthumb", code);
                        if (Directory.Exists(gewangthumbpath))
                        {
                            bmpNew.Save(gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg", ImageFormat.Jpeg);
                            image1.Dispose();
                            temp.Clear();
                            string p2 = gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg";
                            byte opacity = 255;
                            bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 1, minTileLB[0], minTileLB[2], minTileLB[1], minTileLB[3], 0, opacity, p2);
                          //  bool addLayer1 = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, minTileLB[2], minTileLB[1], minTileLB[0], minTileLB[1], minTileLB[0], minTileLB[3], minTileLB[0], minTileLB[3], 0, opacity, p);

                            break;
                        }
                        else
                        {
                            Directory.CreateDirectory(gewangthumbpath);
                            bmpNew.Save(gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg", ImageFormat.Jpeg);
                            image1.Dispose();
                            temp.Clear();
                            string p2 = gewangthumbpath + "\\" + code + "Cut" + ImgCount + ".jpg";
                            byte opacity = 255;
                           
                            bool addLayer = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 1, minTileLB[0], minTileLB[2], minTileLB[1], minTileLB[3], 0, opacity, p2);
                           // bool addLayer1 = this.mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(ds.Rows[i]["数据编码"].ToString(), 100, minTileLB[2], minTileLB[1], minTileLB[0], minTileLB[1], minTileLB[0], minTileLB[3], minTileLB[0], minTileLB[3], 0, opacity, p);

                            break;
                        }

                    }
                    #endregion


                }

            }
        }
        private DataTable SearchData(double[] selectedRect)
        {
            //构造查询条件对象
            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition queryCondition = ConstructSeniorQueryCondition();
            if (queryPara != null)
            {
                queryCondition.complexCondition = new QRST_DI_SS_Basis.MetadataQuery.ComplexCondition[1];
                queryCondition.complexCondition[0] = queryPara.GetSpecificCondition(querySchema);
                queryCondition.complexCondition[0].ruleName = rule;
                queryCondition.complexCondition[0].selectRation = selectedRect;
            }
            queryPara.GetPublicFieldMappedValue(querySchema);

            //构造查询请求
            queryRequest = new QRST_DI_SS_Basis.MetadataQuery.QueryRequest();
            queryRequest.complexCondition = queryCondition;
            queryRequest.dataBase = selectedQueryObj.GROUP_CODE.Substring(0, 4);
            queryRequest.elementSet = new string[1] { "*" };                            //默认查询全部字段
            queryRequest.tableCode = selectedQueryObj.DATA_CODE;

            ViewBasedQuery queryObj = new ViewBasedQuery(queryRequest, querySchema);
            MetaDataPagingQuery mdpq = new MetaDataPagingQuery(queryObj);
            QRST_DI_SS_Basis.MetadataQuery.QueryResponse qr = queryObj.Query();
            DataTable table = new DataTable();
            queryObj.queryRequest.complexCondition.ruleName = QRST_DI_SS_Basis.MetadataQuery.Rule.Contain;
            if (qr != null && qr.recordSet != null && qr.recordSet.Tables.Count > 0)
            {
                System.Data.DataTable tab = qr.recordSet.Tables[0];

                tab = mdpq.GeometryFilter(tab);
                table = tab;
            }
            return table;
        }
        private IGeometry getGeomFromRow(DataRow dr)
        {
            int flag = -1;
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (dc.Caption.Contains("经度") || dc.Caption.Contains("纬度"))
                {
                    flag = 1;
                    break;
                }
                else if (dc.Caption.Contains("数据范围"))
                {
                    flag = 0;
                    break;
                }
            }



            List<Coordinate> coords = new List<Coordinate>();

            if (flag == 1)
            {
                double lulat = Convert.ToDouble(dr["左上纬度"].ToString());
                double lulon = Convert.ToDouble(dr["左上经度"].ToString());
                double rulat = Convert.ToDouble(dr["右上纬度"].ToString());
                double rulon = Convert.ToDouble(dr["右上经度"].ToString());
                double rdlat = Convert.ToDouble(dr["右下纬度"].ToString());
                double rdlon = Convert.ToDouble(dr["右下经度"].ToString());
                double ldlat = Convert.ToDouble(dr["左下纬度"].ToString());
                double ldlon = Convert.ToDouble(dr["左下经度"].ToString());

                coords.Add(new Coordinate(lulon, lulat));
                coords.Add(new Coordinate(rulon, rulat));
                coords.Add(new Coordinate(rdlon, rdlat));
                coords.Add(new Coordinate(ldlon, ldlat));
                coords.Add(new Coordinate(lulon, lulat));

            }
            else if (flag == 0)
            {
                double maxlat = Convert.ToDouble(dr["数据范围上"].ToString());
                double minlat = Convert.ToDouble(dr["数据范围下"].ToString());
                double minlon = Convert.ToDouble(dr["数据范围左"].ToString());
                double maxlon = Convert.ToDouble(dr["数据范围右"].ToString());

                coords.Add(new Coordinate(minlon, maxlat));
                coords.Add(new Coordinate(maxlon, maxlat));
                coords.Add(new Coordinate(maxlon, minlat));
                coords.Add(new Coordinate(minlon, minlat));
                coords.Add(new Coordinate(minlon, maxlat));

            }
            IGeometry poly = new DotSpatial.Topology.Polygon(coords);
            return poly;

        }
        string selectedValue = "默认";
        private void barEditItem2_EditValueChanged(object sender, EventArgs e)
        {
            //如果是瓦片数据的全覆盖检索时
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                selectedValue = barEditItem2.EditValue.ToString();
                //switch (selectedValue)
                //{
                //    case "默认":
                //        EnSelectedValue = "D";
                //        break;
                //    case "时间-云量-满幅率":
                //        EnSelectedValue = "TCA";
                //        break;
                //    case "时间-满幅率-云量":
                //        EnSelectedValue = "TAC";
                //        break;
                //    case "云量-满幅率-时间":
                //        EnSelectedValue = "CAT";
                //        break;
                //    case "云量-时间-满幅率":
                //        EnSelectedValue = "CTA";
                //        break;
                //    case "满幅率-时间-云量":
                //        EnSelectedValue = "ATC";
                //        break;
                //    case "满幅率-云量-时间":
                //        EnSelectedValue = "ACT";
                //        break;

                //    default:
                //        //EnSelectedValue = "D";
                //        break;
                //}

            }
            else   //原始数据的全覆盖检索
            {
                try
                {
                    if (dtable != null)
                    {
                        //sourceDt = dtable;
                        //DataView dv = sourceDt.DefaultView;
                        //dv.Sort = "接收时间 DESC";
                        ////dv.Sort = "接收时间 ASC";
                        //sourceDt = dv.ToTable();

                        DataView dv = dtable.DefaultView;
                        dv.Sort = "接收时间 DESC";
                        //dv.Sort = "接收时间 ASC";
                        dtable = dv.ToTable();
                    }
                }
                catch (Exception)
                {
                }

            }
           
        }
        // 清除检索20170109
        private void barButtonItemDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            selectedQueryObj = muc3DSearcher._metadatacatalognode_Mdl;
            QRST_DI_SS_Basis.MetadataQuery.ComplexCondition._usingGeometry = false;
            this.barEditItemShowPoint.EditValue = "";
            this.barEditItemShowLinePoint.EditValue = "";
            if (!selectedQueryObj.IS_DATASET)
            {
                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_table")) //表格数据查询环境
                {
                    if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")          //波普特征数据库，特殊情况
                    {
                        ClearExtentLyr();

                        mucdetail.deleteBop();

                    }
                    else
                    {
                        ClearExtentLyr();
                    }
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))
                {
                    ClearExtentLyr();
                    mucdetail.DeleteQueryShpFile();
                    
                }
                else
                {
                    ClearExtentLyr();
                }
            }
            else
            {
                XtraMessageBox.Show(string.Format("暂不支持'{0}'数据类型查询!", selectedQueryObj.NAME));
                return;
            }

            try
            {
                mucdetail.DeleteQuery();
            }
            catch (Exception)
            {
            }
            Thread.Sleep(1000);     
        }

        private void barEditItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //瓦片数据的全覆盖检索条件
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                selectedValue = barEditItem2.EditValue.ToString();
                //switch (selectedValue)
                //{
                //    case "默认":
                //        EnSelectedValue = "D";
                //        break;
                //    case "时间-云量-满幅率":
                //        EnSelectedValue = "TCA";
                //        break;
                //    case "时间-满幅率-云量":
                //        EnSelectedValue = "TAC";
                //        break;
                //    case "云量-满幅率-时间":
                //        EnSelectedValue = "CAT";
                //        break;
                //    case "云量-时间-满幅率":
                //        EnSelectedValue = "CTA";
                //        break;
                //    case "满幅率-时间-云量":
                //        EnSelectedValue = "ATC";
                //        break;
                //    case "满幅率-云量-时间":
                //        EnSelectedValue = "ACT";
                //        break;

                //    default:
                //        //EnSelectedValue = "D";
                //        break;
                //}
            }
            else
            {
                try
                {
                    if (dtable != null)
                    {
                        DataView dv = dtable.DefaultView;
                        dv.Sort = "接收时间 DESC";
                        dtable = dv.ToTable();
                    }
                }
                catch (Exception)
                {
                }

            }
        }
            
        # region   瓦片数据的贴图功能 （第一中是把表格中的数据全贴，同样适用于行列号重复的数据；第二种是按行一个一个的贴）
        /// <summary>
        /// 20170510 xmh
        /// 把的检索到的瓦片数据贴在三维球上
        /// </summary>
        public void AddLyrOrder(DataTable dt)
        {
            //WS_QDB_GetDataService.Service getDataclient = null;
            //if (getDataclient == null)
            //{
            //    getDataclient = new WS_QDB_GetDataService.Service();
            //}
            string tilePath = null;
            Dictionary<string, int> extents0 = new Dictionary<string, int>();
            Dictionary<string, string> groupTile = new Dictionary<string, string>();
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string tilerow = dt.Rows[i]["Row"].ToString();
                    string tilecol = dt.Rows[i]["Col"].ToString();
                    string str = dt.Rows[i][queryPara.QRST_CODE].ToString();
                    string tileFilename = _getDataClient.GetTilesList(new string[] { str })[0];

                    if (tileFilename.EndsWith(".png") || tileFilename.EndsWith(".jpg"))
                    {
                        if (File.Exists(tileFilename))
                        {
                            tilePath = tileFilename;
                        }
                    }
                    else
                    {
                        if (tileFilename.Contains(".c"))   //切片数据
                        {
                            try
                            {
                                string thumbnailName = tileFilename.Remove(tileFilename.IndexOf('-')) + ".c.jpg";
                                string thumbnailPNGName = tileFilename.Remove(tileFilename.IndexOf('-')) + ".c.png";
                                if (File.Exists(thumbnailPNGName))
                                {
                                    tilePath = thumbnailPNGName;
                                }
                                else if (File.Exists(thumbnailName))
                                {
                                    tilePath = thumbnailName;
                                }
                                else
                                {
                                    tilePath = tileFilename;
                                }

                            }
                            catch (Exception)
                            {

                            }

                        }
                    }
                    byte opacity = 255;
                    if (lrc[1].Equals(tilerow) && lrc[2].Equals(tilecol))
                    {
                        bool addLayer = mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(str, 1, extent[0], extent[2], extent[1], extent[3], 0, opacity, tilePath);
                        break;

                    }

                }
            }
        }
        /// <summary>
        /// 单一瓦片数据贴图
        /// </summary>
        public  void AddSingleLyrOrder(DataRow dr)
        {
            //WS_QDB_GetDataService.Service getDataclient = null;
            //if (getDataclient == null)
            //{
            //    getDataclient = new WS_QDB_GetDataService.Service();
            //}
            string tilePath = null;
            string level = dr["Level"].ToString();
            string tilerow = dr["Row"].ToString();
            string tilecol = dr["Col"].ToString();
            string str = dr[queryPara.QRST_CODE].ToString();
            string tileFilename = _getDataClient.GetTilesList(new string[] { str })[0];
            double[] extent = GetLatAndLong(tilerow, tilecol, level);
            if (tileFilename.EndsWith(".png") || tileFilename.EndsWith(".jpg"))
            {
                if (File.Exists(tileFilename))
                {
                    tilePath = tileFilename;
                }
            }
            else
            {
                if (tileFilename.Contains(".c"))   //切片数据
                {
                    try
                    {
                        string thumbnailName = tileFilename.Remove(tileFilename.IndexOf('-')) + ".c.jpg";
                        string thumbnailPNGName = tileFilename.Remove(tileFilename.IndexOf('-')) + ".c.png";
                        if (File.Exists(thumbnailPNGName))
                        {
                            tilePath = thumbnailPNGName;
                        }
                        else if (File.Exists(thumbnailName))
                        {
                            tilePath = thumbnailName;
                        }
                        else
                        {
                            tilePath = tileFilename;
                        }

                    }
                    catch (Exception)
                    {

                    }

                }
            }
            byte opacity = 255;
            bool addLayer = mucsearcher.qrstAxGlobeControl1.QrstGlobe.AddImage(str, 1, extent[0], extent[2], extent[1], extent[3], 0, opacity, tilePath);
        }
        # endregion


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
