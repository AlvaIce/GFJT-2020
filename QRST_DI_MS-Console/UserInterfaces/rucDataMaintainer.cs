using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_DS_MetadataQuery;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public class rucDataMaintainer:RibbonPageBaseUC
    {
    //    private DevExpress.XtraBars.BarButtonItem barButtonShowImg;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSave;
        private DevExpress.XtraBars.BarButtonItem barButtonDelete;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAllSelected;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAllNotSelected;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
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
            this.barButtonItemAllNotSelected});
            this.ribbonControl1.MaxItemId = 13;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1});
            this.ribbonControl1.Size = new System.Drawing.Size(687, 147);
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemSave, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonDelete, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllSelected, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAllNotSelected, true);
            this.ribbonPageGroup1.Text = "操作";
            // 
            // barButtonItemSave
            // 
            this.barButtonItemSave.Caption = "保存更改";
            this.barButtonItemSave.Id = 2;
            this.barButtonItemSave.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.保存4;
            this.barButtonItemSave.Name = "barButtonItemSave";
            this.barButtonItemSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSave_ItemClick);
            // 
            // barButtonDelete
            // 
            this.barButtonDelete.Caption = "删除记录";
            this.barButtonDelete.Id = 3;
            this.barButtonDelete.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除记录2;
            this.barButtonDelete.Name = "barButtonDelete";
            this.barButtonDelete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonDelete_ItemClick);
            // 
            // barButtonItemAllSelected
            // 
            this.barButtonItemAllSelected.Caption = "选中全部数据";
            this.barButtonItemAllSelected.Id = 4;
            this.barButtonItemAllSelected.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.全选;
            this.barButtonItemAllSelected.Name = "barButtonItemAllSelected";
            this.barButtonItemAllSelected.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAllSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAllSelected_ItemClick);
            // 
            // barButtonItemAllNotSelected
            // 
            this.barButtonItemAllNotSelected.Caption = "不选中数据";
            this.barButtonItemAllNotSelected.Id = 5;
            this.barButtonItemAllNotSelected.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.全不选;
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
            // rucDataMaintainer
            // 
            this.Name = "rucDataMaintainer";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        public static DataSet SearchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
        {
            DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            //配置基本查询信息

            QueryRequest qr = new QueryRequest();
            qr.dataBase = "EVDB";
            qr.elementSet = new string[] { "*" };
            qr.tableCode = "EVDB-32";
            qr.recordSetStartPointSpecified = StartIndex;
            qr.offset = ResultCount;


            IGetQuerySchema querySchema = new FieldViewBasedQuerySchema(qr.elementSet, qr.tableCode, mySQLOperator.EVDB);
            //排序字段
            if (!(strOrderBy == null || OrderByType == null || strOrderBy.Count != OrderByType.Count))
            {
                OrderBy[] orderByArr = new OrderBy[strOrderBy.Count];
                for (int i = 0; i < orderByArr.Length; i++)
                {
                    orderByArr[i] = new OrderBy();
                    orderByArr[i].accessPointField = strOrderBy[i];
                    orderByArr[i].orderType = OrderByType[i] == 0 ? OrderType.ASC : OrderType.DESC;
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
            QRST_DI_DS_MetadataQuery.QueryResponse queryResponse = query.Query();
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
    }
}
