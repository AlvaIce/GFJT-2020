using System;
using System.Collections.Generic;
using System.Data;
using QRST_DI_DS_MetadataQuery;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public class rucDataMaintainer:RibbonPageBaseUC
    {
    //      private DevExpress.XtraBars.BarButtonItem barButtonShowImg;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSave;
        private DevExpress.XtraBars.BarButtonItem barButtonDelete;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAllSelected;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAllNotSelected;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonSameItems;
        private DevExpress.XtraBars.BarButtonItem barButtonItemReset;
        public mucDataMaintainer mucMaintain;

        public rucDataMaintainer()
            : base()
        {
            InitializeComponent();
        }
        public rucDataMaintainer(object obMaintainUC)
            : base(obMaintainUC)
        {
            InitializeComponent();
			mucMaintain = obMaintainUC as mucDataMaintainer;
        }

        private void InitializeComponent()
        {
            this.barButtonItemSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemAllSelected = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemAllNotSelected = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.barButtonSameItems = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemReset = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
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
            this.barButtonItemSave,
            this.barButtonDelete,
            this.barButtonItemAllSelected,
            this.barButtonItemAllNotSelected,
            this.barButtonSameItems,
            this.barButtonItemReset});
            this.ribbonControl1.MaxItemId = 15;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1});
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemSave, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonDelete, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllSelected, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllNotSelected, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonSameItems,true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemReset,true);
            this.ribbonPageGroup1.Text = "操作";
            // 
            // barButtonItemSave
            // 
            this.barButtonItemSave.Caption = "保存更改";
            this.barButtonItemSave.Id = 2;
            this.barButtonItemSave.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.保存4;
            this.barButtonItemSave.Name = "barButtonItemSave";
            this.barButtonItemSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSave_ItemClick);
            // 
            // barButtonDelete
            // 
            this.barButtonDelete.Caption = "删除记录";
            this.barButtonDelete.Id = 3;
            this.barButtonDelete.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.删除记录2;
            this.barButtonDelete.Name = "barButtonDelete";
            this.barButtonDelete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonDelete_ItemClick);
            // 
            // barButtonItemAllSelected
            // 
            this.barButtonItemAllSelected.Caption = "选中全部数据";
            this.barButtonItemAllSelected.Id = 4;
            this.barButtonItemAllSelected.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.全选;
            this.barButtonItemAllSelected.Name = "barButtonItemAllSelected";
            this.barButtonItemAllSelected.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAllSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAllSelected_ItemClick);
            // 
            // barButtonItemAllNotSelected
            // 
            this.barButtonItemAllNotSelected.Caption = "不选中数据";
            this.barButtonItemAllNotSelected.Id = 5;
            this.barButtonItemAllNotSelected.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.全不选;
            this.barButtonItemAllNotSelected.Name = "barButtonItemAllNotSelected";
            this.barButtonItemAllNotSelected.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAllNotSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAllNotSelected_ItemClick);
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // barButtonSameItems
            // 
            this.barButtonSameItems.Caption = "显示同名冗余记录";
            this.barButtonSameItems.Id = 13;
            this.barButtonSameItems.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.显示同名冗余;
            this.barButtonSameItems.Name = "barButtonSameItems";
            this.barButtonSameItems.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonSameItems.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonSameItems_ItemClick);
            // 
            // barButtonItemReset
            // 
            this.barButtonItemReset.Caption = "还原记录";
            this.barButtonItemReset.Id = 14;
            this.barButtonItemReset.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.还原记录;
            this.barButtonItemReset.Name = "barButtonItemReset";
            this.barButtonItemReset.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemReset.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemReset_ItemClick);
            // 
            // rucDataMaintainer
            // 
            this.Name = "rucDataMaintainer";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        public static DataSet SearchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
        {
            IDbOperating mySQLOperator = Constant.IdbOperating;
            //配置基本查询信息

            QRST_DI_SS_Basis.MetadataQuery.QueryRequest qr = new QRST_DI_SS_Basis.MetadataQuery.QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "EVDB-32";
            qr.recordSetStartPointSpecified = StartIndex;
            qr.offset = ResultCount;


            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB));
            //排序字段
            if (!(strOrderBy == null || OrderByType == null || strOrderBy.Count != OrderByType.Count))
            {
                QRST_DI_SS_Basis.MetadataQuery.OrderBy[] orderByArr = new QRST_DI_SS_Basis.MetadataQuery.OrderBy[strOrderBy.Count];
                for (int i = 0; i < orderByArr.Length; i++)
                {
                    orderByArr[i] = new QRST_DI_SS_Basis.MetadataQuery.OrderBy();
                    orderByArr[i].accessPointField = strOrderBy[i];
                    orderByArr[i].orderType = OrderByType[i] == 0 ? QRST_DI_SS_Basis.MetadataQuery.OrderType.ASC : QRST_DI_SS_Basis.MetadataQuery.OrderType.DESC;
                }
                qr.orderBy = orderByArr;
            }

            //设置查询条件
            RasterQueryPara _rasterQueryPara = new RasterQueryPara();
            _rasterQueryPara.dataCode = qr.tableCode;
            if (datetime!=null&&datetime[0]!=null)
            {
                _rasterQueryPara.STARTTIME = datetime[0];
            }
            if (datetime != null && datetime[1] != null)
            {
                _rasterQueryPara.ENDTIME = datetime[1];
            }
            if (position !=null && position.Count >4)
            {
                _rasterQueryPara.EXTENTDOWN = position[0];
                _rasterQueryPara.EXTENTLEFT = position[1];
                _rasterQueryPara.EXTENTUP = position[2];
                _rasterQueryPara.EXTENTRIGHT = position[3];
            }
            //卫星
            string _satellite = "";
            if (satellite != null)
            {
                for (int i = 0; i < satellite.Count; i++)
                {
                    _satellite = String.Format("{0}{1},", _satellite, satellite[i]);
                }
            }
          
            if(!string.IsNullOrEmpty(_satellite))
            _rasterQueryPara.SATELLITE = _satellite;

            //传感器
            string _sensor = "";
            if(sensor!=null)
            {
                for (int i = 0; i < sensor.Count; i++)
                {
                    _sensor = String.Format("{0}{1},", _sensor, sensor[i]);
                }
            }
    
            if(!string.IsNullOrEmpty(_sensor))
            _rasterQueryPara.SENSOR = _sensor;

            qr.complexCondition = _rasterQueryPara.GetSpecificCondition(querySchema);

            ViewBasedQuery query = new ViewBasedQuery(qr,querySchema);
            AllRecordsCount = query.GetRecordCount();
            QRST_DI_SS_Basis.MetadataQuery.QueryResponse queryResponse = query.Query();
            return queryResponse.recordSet;
        }

         


        private void barButtonShowImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int totalRecord;
             List<string> position = new List<string>(){};
             DataSet ds = SearchGF1DataJCGX(new List<string>() { "-80", "-170", "80", "170" }, new List<string>() { "2013-08-18 03:13:58", "2013-10-18 03:13:58" }, new List<string>() { "GF1" }, new List<string>() { "PMS1", "PMS2", "WFV4" }, out  totalRecord, 0, 1000, new List<string>() { "接收时间" }, new List<int> { 1}); 
        }

		private void barButtonDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			this.mucMaintain.deleteRecord();
		}

		private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
            this.mucMaintain.SaveDeleteResult();
		}
        private void barButtonItemAllSelected_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucMaintain.SelectAllData(true);
        }
        private void barButtonItemAllNotSelected_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucMaintain.SelectAllData(false);
        }
        /// <summary>
        /// xmh   20170411 显示同名冗余记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSameItems_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucMaintain.ShowSameItems();
        }
        /// <summary>
        /// xmh 20170411 还原记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucMaintain.ResetItems();
        }
    }
}
