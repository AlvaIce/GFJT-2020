using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using QRST_DI_DS_Basis.DBEngine;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using System.IO;
using QRST_DI_MS_Basis.QueryBase;
using DevExpress.XtraEditors.Repository;
using QRST_DI_MS_Console.QueryInner;
using System.Threading.Tasks;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.Paths;
using SharpMap.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using System.Text.RegularExpressions;
using QRST_DI_Resources;
using QRST_DI_DS_Metadata.MetaDataCls;
using System.Net;
using DevExpress.XtraCharts;
using System.Diagnostics;
using QRST_DI_TS_Basis.DirectlyAddress;
using Qrst;
using QRST_DI_MS_Console.WS_QDB_Searcher_MySQL;

namespace QRST_DI_MS_Console.UserInterfaces
{
	public partial class mucDetailViewer: DevExpress.XtraEditors.XtraUserControl
    {
        #region 定义特有字段
        private metadatacatalognode_Mdl _selectedQueryObj;
		public static DataTable tempTable = null;
        public EnumDisplayEnum displaySchema;                              //显示模式
        public metadatacatalognode_Mdl selectedQueryObj                   //存储需要查询的数据类型对象 zxw 2013/08/08
        {
            get
            {
                return _selectedQueryObj;
            }
            set
            {
                _selectedQueryObj = value;
                this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                switch (_selectedQueryObj.GROUP_TYPE.ToLower())
                {
                    case "system_vector":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(MainMapImage);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomIn);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomOut);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonPan);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonFullExtent);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(labelControlPosition);
                        SwitchDisplaySchema("normal");       //zxw 20131221 自动切换到正常模式
                        break;
                    case "system_raster":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                         SwitchDisplaySchema("normal");       //zxw 20131221 自动切换到正常模式
                        break;
                    case "system_document":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                         SwitchDisplaySchema("alltable");       //zxw 20131221 自动切换到全表模式
                        break;
                    case "system_table":
                        if (_selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                        {
                            this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.chartControl1);
                            SwitchDisplaySchema("normal");       //zxw 20131221 自动切换到正常模式
                        }
                        else
                        {
                            this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                            SwitchDisplaySchema("alltable");       //zxw 20131221 自动切换到全表模式
                        }
                        break;
                    case "system_tile":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                        this.pictureEditImgData.Dock = DockStyle.Fill;
                        SwitchDisplaySchema("normal");       //zxw 20131221 自动切换到正常模式
                        break;
                    default:
                        break;
                }
            }
        }
        const  string ChooseDataColumnCaption = "选择数据";
        public QueryPara queryPara;
        
        WS_QDB_GetDataService.Service getDataclient ;                     //切片数据获取服务对象
        #endregion

        SharpMap.Forms.MapImage MainMapImage = new SharpMap.Forms.MapImage();

        public mucDetailViewer()
        {
            InitializeComponent();

            //zxw 添加对ctrlImageEditControl的初始化  20131221
            this.pictureEditImgData = new QRST_DI_MS_Console.UserInterfaces.CtrlImageDisplay(this);
            this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
            this.pictureEditImgData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEditImgData.Image = null;
            this.pictureEditImgData.Location = new System.Drawing.Point(0, 0);
            this.pictureEditImgData.Name = "pictureEditImgData";
            this.pictureEditImgData.Size = new System.Drawing.Size(381, 396);
            this.pictureEditImgData.TabIndex = 11;

            MainMapImage.Name = "VectorViewer";
            MainMapImage.Dock = DockStyle.Fill;
            MainMapImage.BackColor = Color.White;
            MainMapImage.MouseMove += MainMapImage_MouseMove;
            MainMapImage.MouseLeave += MainMapImage_MouseLeave;
        }

        private void MainMapImage_MouseMove(SharpMap.Geometries.Point WorldPos, MouseEventArgs ImagePos)
        {
            labelControlPosition.Text = String.Format("鼠标位置：{0:N5}, {1:N5}", WorldPos.X, WorldPos.Y);
        }

        private void MainMapImage_MouseLeave(object sender, EventArgs e)
        {
            labelControlPosition.Text = "";
        }
        /// <summary>
        /// 窗体加载的时候获取此种数据类型对应的  相对地址和 构成绝对地址的字段列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mucDetailViewer_Load(object sender, EventArgs e)
        {
            //DLF20130830 添加。解决问题：切片查询时，第一次查询得到数据，第二次查询得不到数据时，详细信息面板未清空。
            textDetailnfo.Text = string.Empty;

            SwitchDisplaySchema("alltable");
        }
        /// <summary>
        /// 自定义非绑定字段的数据，此事件在 load事件之后触发执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            
        }



        /// <summary>
        /// 上下分隔条控件大小改变事件，设置上下分隔条的位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainerControlTableViewer_SizeChanged(object sender, EventArgs e)
        {
            //if (this.splitContainerControlTableViewer.Height > 600)
            //{
            //    this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height - 300;
            //}
            //else
            //{
            //    this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height / 2;
            //}
        }
        /// <summary>
        /// 左右分隔条控件大小改变事件，设置左右分隔条的位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainerControlImgAndDetail_SizeChanged(object sender, EventArgs e)
        {
            //zxw 20131221 删除了自动调整界面
            //if (this.splitContainerControlImgAndDetail.Width>800)
            //{
            //    this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width - 400;
            //}
            //else
            //{
            //    this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width / 2;
            //}
            if (displaySchema == EnumDisplayEnum.AllPicture)
            {
            }
            else
            {
            }

            if (MainMapImage.Map.Layers.Count > 0)
            {
                MainMapImage.Map.ZoomToExtents();
                MainMapImage.Refresh();
                simpleButtonZoomIn.BringToFront();
                simpleButtonZoomOut.BringToFront();
                simpleButtonPan.BringToFront();
                simpleButtonFullExtent.BringToFront();
            }

        }

        void SizeToNomal()
        {

            if (this.splitContainerControlTableViewer.Height > 600)
            {
                this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height - 300;
            }
            else
            {
                this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height / 2;
            }
            if (this.splitContainerControlImgAndDetail.Width > 800)
            {
                this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width - 400;
            }
            else
            {
                this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width / 2;
            }

            if (MainMapImage.Map.Layers.Count > 0)
            {
                MainMapImage.Map.ZoomToExtents();
                MainMapImage.Refresh();
                simpleButtonZoomIn.BringToFront();
                simpleButtonZoomOut.BringToFront();
                simpleButtonPan.BringToFront();
                simpleButtonFullExtent.BringToFront();
            }
        }
        /// <summary>
        /// 行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_RowClick(object sender, RowClickEventArgs e)
        {

        }

        private string GetVector(string p)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 填充详细信息的Gridview表,添加缩略图列和选中列
        /// </summary>
        /// <param name="ds"></param>
        public void setGridControl(DataSet ds)
        {
          //  gridControlTable.DataSource = ds;

            if (ds==null)
            {
                return;
            }

			tempTable = ds.Tables[0].Copy();
            gridViewMain.Columns.Clear();
            //数据选择列
            DataColumn checkDownColumn = new DataColumn() { ColumnName = ChooseDataColumnCaption, DataType = typeof(bool) };

            //DLF 0822因异常添加
            if (ds==null || ds.Tables.Count == 0)
            {
                gridControlDataList.DataSource = null;
                return;
            }
           
            DataTable dt = ds.Tables[0];
            //如果checkDownColumn不存在
            if (!dt.Columns.Contains(checkDownColumn.ColumnName))
            {
                dt.Columns.Add(checkDownColumn);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
            }
            gridControlDataList.DataSource = dt;
            //只显示前面十五列
            for (int i = 0; i < gridViewMain.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gridViewMain.Columns[i].OptionsColumn.AllowEdit = false;
                    gridViewMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (i != gridViewMain.Columns.Count - 1)
                {
                    gridViewMain.Columns[i].Visible = false;
                }
            }
            if (gridViewMain.Columns.Count>1)
            {
                gridViewMain.Columns[gridViewMain.Columns.Count-1].OptionsColumn.AllowEdit = true;
                gridViewMain.Columns[gridViewMain.Columns.Count-1].Visible = true;
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].VisibleIndex = 0;
            }
        }

        public void setGridControl(DataTable dt)
        {
            //  gridControlTable.DataSource = ds;

			tempTable = dt.Copy();
            gridViewMain.Columns.Clear();
            //数据选择列
            DataColumn checkDownColumn = new DataColumn() { ColumnName = ChooseDataColumnCaption, DataType = typeof(bool) };
            //DLF 0822因异常添加
            if (dt==null)
            {
                gridControlDataList.DataSource = null;
                return;
            }

            dt.Columns.Add(checkDownColumn);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
            }
            gridControlDataList.DataSource = dt;
            //只显示前面十五列
            for (int i = 0; i < gridViewMain.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gridViewMain.Columns[i].OptionsColumn.AllowEdit = false;
                    gridViewMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (i != gridViewMain.Columns.Count - 1)
                {
                    gridViewMain.Columns[i].Visible = false;
                }
            }
            if (gridViewMain.Columns.Count > 1)
            {
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].OptionsColumn.AllowEdit = true;
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].Visible = true;
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].VisibleIndex = 0;
            }
        }

        public void SelectAllData(bool isselected)
        {
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if(dt==null)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                dt.Rows[i][ChooseDataColumnCaption] = isselected;
            }
        }

        public void DownLoadSelectedData()
        {
            if (selectedQueryObj==null)
            {
                MessageBox.Show("没有要下载的数据！");
                return;
            }
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;

            List<string> lst = new List<string>();
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))  //下载切片
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        lst.Add(dt.Rows[i]["TileFileName"].ToString());
                    }
                }
            }
            else  //下载非切片数据
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        lst.Add(dt.Rows[i][queryPara.QRST_CODE].ToString());

                    }
                }
            }
            FrmDownLoad frmDownLoad = new FrmDownLoad(selectedQueryObj, lst);
            frmDownLoad.Show();
        }

        /// <summary>
        /// 详细信息窗体上部左侧控件
        /// </summary>
        /// <param name="fileTypeCategory"></param>
        internal void setQuickViewControl(FileTypeCategory fileTypeCategory)
        {
            switch (fileTypeCategory)
            {
                case FileTypeCategory.Vectors:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(MainMapImage);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomIn);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomOut);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonPan);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonFullExtent);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(labelControlPosition);
            	    break;
                case FileTypeCategory.Rasters:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
            	    break;
                case FileTypeCategory.Documents:
            	    break;
                case FileTypeCategory.Sheets:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                    break;
                case FileTypeCategory.Tiles:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                    break;
                case FileTypeCategory.NotDefine:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                    break;
                default:
                    break;
            }
        }


        private void simpleButtonZoomIn_Click(object sender, EventArgs e)
        {
            MainMapImage.ActiveTool = MapImage.Tools.ZoomIn;
        }

        private void simpleButtonZoomOut_Click(object sender, EventArgs e)
        {
            MainMapImage.ActiveTool = MapImage.Tools.ZoomOut;
        }

        private void simpleButtonPan_Click(object sender, EventArgs e)
        {
            MainMapImage.ActiveTool = MapImage.Tools.Pan;
        }

        private void simpleButtonFullExtent_Click(object sender, EventArgs e)
        {
            if (MainMapImage.Map.Layers.Count>0)
            {
                MainMapImage.Map.ZoomToExtents();
                MainMapImage.Refresh();

                simpleButtonZoomIn.BringToFront();
                simpleButtonZoomOut.BringToFront();
                simpleButtonPan.BringToFront();
                simpleButtonFullExtent.BringToFront();
            }

        }

        #region   切换视图模式方法
        public void SwitchDisplaySchema(string schema)
        {
            if(schema.ToLower().Equals("allpicture")) //全图展示
            {
                displaySchema = EnumDisplayEnum.AllPicture;
                splitContainerControlImgAndDetail.SplitterPosition = splitContainerControlImgAndDetail.Width;
                splitContainerControlTableViewer.SplitterPosition = splitContainerControlTableViewer.Height;
                
            }
            else if (schema.ToLower().Equals("alltable")) //全表显示
            {
                //zxw 20131221  更改全表的显示模式
                displaySchema = EnumDisplayEnum.AllTable;
                splitContainerControlImgAndDetail.SplitterPosition = 0;
                splitContainerControlTableViewer.SplitterPosition = 0;
                splitContainerControlgrid.SplitterPosition = splitContainerControlgrid.Width*3/4;
                
            }
            else          //正常显示
            {
                displaySchema = EnumDisplayEnum.Normal;
                SizeToNomal();
            }
        }
        #endregion
        /// <summary>
        /// 显示记录的详细信息
        /// </summary>
        /// <param name="dr"></param>
        void DisplayDetailInfo(DataRow dr)
        {
            try
            {
                if (dr != null)
                {
                    DataTable dt = (DataTable)gridControlDataList.DataSource;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("详细信息:");
                    sb.AppendLine("");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!dt.Columns[i].Caption.Equals("选择数据"))
                        {
                            string info = string.Format("{0}:{1}", dt.Columns[i].Caption, dr[i]);
                            sb.AppendLine(info);
                        }
                    }
                    textDetailnfo.Text = sb.ToString();
                    //zxw 20131221 为简便，暂时搞两个显示详细信息的吧
                    memoEditDetail.Text = sb.ToString();
                }
                else
                {
                    textDetailnfo.Text = string.Empty;
                    //zxw 20131221 为简便，暂时搞两个显示详细信息的吧
                    memoEditDetail.Text = string.Empty;
                }
            }
            catch(Exception ex)
            {
                Exception e = ex;
            }
         
        }

        

        /// <summary>
        /// 换行时，将选中行的信息加载到详细信息列表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            RefreshInfo();
        }

        /// <summary>
        /// 刷新界面信息
        /// </summary>
        public void RefreshInfo()
        {
            if (gridViewMain.FocusedRowHandle > gridViewMain.DataRowCount)
                gridViewMain.FocusedRowHandle = 0;
            DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            DisplayDetailInfo(dr);
            if (dr == null)
            {
                //清空显示面板
                pictureEditImgData.Image = null;
                return;
            }
            //显示缩略图信息
            if (queryPara.QRST_CODE != null)
            {
                string id;
                if ((!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector")) && selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() != "rcdb" && (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")))
                {
                    id = dr[queryPara.QRST_CODE].ToString();
                    GetImageDel del = GetImage;
                    IAsyncResult ar = del.BeginInvoke(id, GetImageCallBack, del);
                }
                else if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                {
                    GetBopuDataDel del = GetBopuData;
                    IAsyncResult ar = del.BeginInvoke(GetBopuDataCallBack, del);
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))          //展示矢量数据
                {
                    //添加矢量数据  zxw 20131020
                    string storePath = MetaData.GetDataAddress(dr[queryPara.QRST_CODE].ToString());
                    if (Directory.Exists(storePath)) //
                    {
                        //异步执行适量数据加载
                        GetShapeFileDel del = GetShapeFile;
                        IAsyncResult ar = del.BeginInvoke(storePath, GetShapeFileCallBack, del);

                    }
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))             //展示切片数据 
                {
                    if (getDataclient == null)
                    {
                        getDataclient = new WS_QDB_GetDataService.Service();
                    }
                    string tileFilename = getDataclient.GetTilesList(new string[] { dr[queryPara.QRST_CODE].ToString() })[0];
                    GetTileImageDel del = GetTileImage;
                    IAsyncResult ar = del.BeginInvoke(tileFilename, GetTileImageCallBack, del);
                }

            }
            else
            {
                pictureEditImgData.Image = null;
            }

            //在三维球上显示选中记录的空间范围
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
            {
                //绘制空间范围到三维球
                if (((RasterQueryPara)queryPara).spacialAvailable)
                {
					DrawCheckedExtent((RasterQueryPara)queryPara);
                    DrawRasterExtent((RasterQueryPara)queryPara);
                }
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))
            {
                //绘制空间范围到三维球
                if (((VectorQueryPara)queryPara).spacialAvailable)
                {
                    DrawVectorExtent((VectorQueryPara)queryPara);
                }
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                //绘制空间范围到三维球
                if (((RasterQueryPara)queryPara).spacialAvailable)
                {
                    DrawRasterExtent((RasterQueryPara)queryPara);
                }
            }
			

        }

		void DrawCheckedExtent(RasterQueryPara queryPara)
        {
            muc3DSearcher _muc3DSearcher = (muc3DSearcher)MSUserInterface.listMSUI[0].uiMainUC;
           // DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);

            if (queryPara.spacialAvailable)
            {
				 DataTable dt = (DataTable)gridControlDataList.DataSource;
				 List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
					if ((bool)dt.Rows[i][ChooseDataColumnCaption])
					{

						DataRow dr = dt.Rows[i];
						float minLat;
						float maxLat;
						float minLon;
						float maxLon;
						if (dr != null)
						{
							if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
							{
								string level = dr["Level"].ToString();
								string[] rowandColumn = new string[4];
								rowandColumn[0] = dr["Row"].ToString();
								rowandColumn[1] = dr["Col"].ToString();

								double[] extent = GetLatAndLong(rowandColumn, level);

								minLat = float.Parse(extent[0].ToString());
								maxLat = float.Parse(extent[2].ToString());
								minLon = float.Parse(extent[1].ToString());
								maxLon = float.Parse(extent[3].ToString());

							}
							else
							{
								minLat = Math.Min(float.Parse(dr[queryPara.DATALOWERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLAT].ToString()));
								maxLat = Math.Max(float.Parse(dr[queryPara.DATAUPPERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATAUPPERRIGHTLAT].ToString()));
								minLon = Math.Min(float.Parse(dr[queryPara.DATAUPPERLEFTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERLEFTLONG].ToString()));
								maxLon = Math.Max(float.Parse(dr[queryPara.DATAUPPERRIGHTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLONG].ToString()));
							}
							extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));

						}
					}
					//else
					//{
					//    if (_muc3DSearcher.Is3DViewer)
					//    {
					//        QrstAxGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
					//        if (_QrstAxGlobeControl != null)
					//        {
					//            _QrstAxGlobeControl.DrawSearchResultExtents(extents);
					//        }
					//    }
					//    else
					//    {
					//        uc2DSearcher _uc2DSearcher = this.pictureEditImgData.GetUc2DSearcher();
					//        if (_uc2DSearcher != null)
					//        {
					//            if (extents.Count > 0)
					//            {
					//                string extent = extents[0].X.ToString() + "," + extents[0].Y.ToString() + "," + (extents[0].X + extents[0].Width).ToString() + "," + (extents[0].Y + extents[0].Height).ToString();
					//                _uc2DSearcher.DrawSelectedExtents(extent);
					//            }
					//        }
					//    }
					//}
				}
				if (_muc3DSearcher.Is3DViewer)
				{
					QrstAxGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
					if (_QrstAxGlobeControl != null)
					{
						_QrstAxGlobeControl.DrawCheckedExtents(extents);
					}
				}
				else
				{
					uc2DSearcher _uc2DSearcher = this.pictureEditImgData.GetUc2DSearcher();
					if (_uc2DSearcher != null)
					{
						if (extents.Count > 0)
						{
							string extent = extents[0].X.ToString() + "," + extents[0].Y.ToString() + "," + (extents[0].X + extents[0].Width).ToString() + "," + (extents[0].Y + extents[0].Height).ToString();
							_uc2DSearcher.DrawSelectedExtents(extent);
						}
					}
				}
            }
        }

		#region 定义绘制空间范围的方法
		void DrawRasterExtent(RasterQueryPara queryPara)
        {
            muc3DSearcher _muc3DSearcher = (muc3DSearcher)MSUserInterface.listMSUI[0].uiMainUC;
            DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            if (queryPara.spacialAvailable)
            {
                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();

                float minLat;
                float maxLat;
                float minLon;
                float maxLon;
                if (dr != null)
                {
                    if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                    {
                        string level = dr["Level"].ToString();
                        string[] rowandColumn = new string[4];
                        rowandColumn[0] = dr["Row"].ToString();
                        rowandColumn[1] = dr["Col"].ToString();

                        double[] extent = GetLatAndLong(rowandColumn, level);

                        minLat = float.Parse(extent[0].ToString());
                        maxLat = float.Parse(extent[2].ToString());
                        minLon = float.Parse(extent[1].ToString());
                        maxLon = float.Parse(extent[3].ToString());

                    }
                    else
                    {
                        minLat = Math.Min(float.Parse(dr[queryPara.DATALOWERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLAT].ToString()));
                        maxLat = Math.Max(float.Parse(dr[queryPara.DATAUPPERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATAUPPERRIGHTLAT].ToString()));
                        minLon = Math.Min(float.Parse(dr[queryPara.DATAUPPERLEFTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERLEFTLONG].ToString()));
                        maxLon = Math.Max(float.Parse(dr[queryPara.DATAUPPERRIGHTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLONG].ToString()));        
                    }
                    extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
                }
                if (_muc3DSearcher.Is3DViewer)
                {
                QrstAxGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
                if(_QrstAxGlobeControl!= null)
                {
                    _QrstAxGlobeControl.DrawSelectedExtents(extents);
                    if(extents.Count>0)
                    {
                        double distance = 1026000.48 * Math.Sqrt(extents[0].Height*extents[0].Height+extents[0].Width*extents[0].Width);
                        _QrstAxGlobeControl.SetViewPosition(extents[0].Bottom, extents[0].Left, 0.0, distance, 0.0);
                        }
                    }
                }
                else
                {
                    uc2DSearcher _uc2DSearcher = this.pictureEditImgData.GetUc2DSearcher();
                    if (_uc2DSearcher != null)
                    {
                        if (extents.Count > 0)
                        {
                            string extent = extents[0].X.ToString() + "," + extents[0].Y.ToString() + "," + (extents[0].X + extents[0].Width).ToString() + "," + (extents[0].Y+extents[0].Height).ToString();
                            _uc2DSearcher.DrawSelectedExtents(extent);
                        }
                    }
                }
            }
        }

        void DrawVectorExtent(VectorQueryPara queryPara)
        {
            DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            if(dr==null)
            {
                return;
            }
            if (queryPara.spacialAvailable)
            {
                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
                    extents.Add(new System.Drawing.RectangleF(
                    float.Parse(dr[queryPara.extentLeftField].ToString()),
                    float.Parse(dr[queryPara.extentDownField].ToString()),
                    float.Parse(dr[queryPara.extentRightField].ToString()) - float.Parse(dr[queryPara.extentLeftField].ToString()),
                    float.Parse(dr[queryPara.extentUpField].ToString()) - float.Parse(dr[queryPara.extentDownField].ToString())));
                QrstAxGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
                if (_QrstAxGlobeControl != null)
                {
                    _QrstAxGlobeControl.DrawSelectedExtents(extents);
                    if (extents.Count > 0)
                    {
                        _QrstAxGlobeControl.SetViewPosition(extents[0].Bottom, extents[0].Left, 0.0, 1000000.0, 0.0);
                    }
                }
            }
        }

        public static double[] GetLatAndLong(string[] rowAndColum, string lv)
        {
            double a = DirectlyAddressing.getLevelRate(lv);
            double[] latAndlong = new double[4];
            latAndlong[0] = Convert.ToDouble(rowAndColum[0]) / a - 90;
            latAndlong[1] = Convert.ToDouble(rowAndColum[1]) / a - 180;
            latAndlong[2] = Convert.ToDouble(Convert.ToInt32(rowAndColum[0]) + 1) / a - 90;
            latAndlong[3] = Convert.ToDouble(Convert.ToInt32(rowAndColum[1]) + 1) / a - 180;
            return latAndlong;
        }


        #endregion

        private void gridViewMain_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if(e.Info.IsRowIndicator&&e.RowHandle>=0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }


        #region 采用异步调用的方式获取缩略图对象
        /// <summary>
        /// 显示缩略图
        /// 通过ID查找缓存文件夹中是否存在该文件，若不存在，则通过id查找该记录的文件存储路径，在对应路径下是否存在缩略图
        /// 若不存在，则检查thumbnail字段
        /// 判断thumbnail是路径、url还是二进制数据，
        /// 若为路径，则通过该路径查看图是否存在，存在则显示，将文件拷贝到缓存
        /// 若为url，则请求该地址获取图片，并将图片下载到缓存
        /// 若为二进制，则直接转换为图片进行显示
        /// </summary>
        Image GetImage(string id)
        {
            //缓存位置
            string catchPath = string.Format(@"{0}\{1}", Application.StartupPath, @"Cache\Thumbnail");
            string qrst_code = ConvertQRSTCODE(id);
            Image image = null;
            if (!string.IsNullOrEmpty(qrst_code))
            {
                string[] files = Directory.GetFiles(catchPath, string.Format("{0}.*", qrst_code));
                if (files.Length > 0)  //从缓存获取缩略图
                {
                    image = Image.FromFile(files[0]);
                }
                else       //没有在缓存中找到缩略图，
                {
                    //通过id查找该记录的文件存储路径，在对应路径下是否存在缩略图
                    string pPath = MetaData.GetDataAddress(qrst_code);
                    string picName;
                    if (ExistPicture(pPath, out picName))
                    {
                        string cachFile = string.Format(@"{0}\{1}{2}", catchPath, qrst_code, Path.GetExtension(picName));
                        File.Copy(picName, cachFile,true);

                        //展示缩略图...
                        image = Image.FromFile(cachFile);
                    }
                    else
                    {
                        if (queryPara.THUMBNAIL != null)
                        {
                            string fieldValue = ((DataTable)gridControlDataList.DataSource).Rows[gridViewMain.FocusedRowHandle][queryPara.THUMBNAIL].ToString();
                            if (IsUrl(fieldValue))   //从网络获取图片，下载到本地缓存并展示
                            {
                                image = GetImageFromWeb(fieldValue, qrst_code);
                            }
                            else if (fieldValue.Equals("System.Byte[]"))  //判断是否为二进制
                            {
                                byte[] bytes = ((byte[])((DataTable)gridControlDataList.DataSource).Rows[gridViewMain.FocusedRowHandle][queryPara.THUMBNAIL]);
                                image = GetImageFromBlob(bytes, qrst_code);
                            }
                        }
                        else
                        {
                            image = null;
                        }
                    }
                }
            }
            return image;
        }

        public delegate Image GetImageDel(string id);

        void GetImageCallBack(IAsyncResult ar)
        {
            GetImageDel del = (GetImageDel)ar.AsyncState;
            Image image = del.EndInvoke(ar);
            if (pictureEditImgData.IsHandleCreated)
            {
                pictureEditImgData.Invoke(new DiaplayDel(DisplayImage), image);
            }
        }

        void DisplayImage(Image image)
        {
            pictureEditImgData.Image = image;
        }

        public delegate void DiaplayDel(Image image);
        #endregion

        #region  异步调用获取波普特征数据曲线数据
        /// <summary>
        /// 获取波普数据   由Tomcat发布服务改成IIS发布服务 @jianghua 2015.8.15
        /// </summary>
        /// <returns></returns>
        string GetBopuData()
        {
            //SoilService.SoilServicePortTypeClient soilclient;
            //WaterService.WaterServicePortTypeClient waterclient;
            //AtmosphereService.AtmosphereServicePortTypeClient atmosphereclient;
            //CityObjService.CityObjServicePortTypeClient cityobjclient;
            //RockService.RockServicePortTypeClient rockclient;
            //VNorthService.VegetationNorthServicePortTypeClient vnorthclient;
            //VSouthService.VegetationSouthServicePortTypeClient vsouthclient;

            try
            {
                string bpData = "";
                if (gridViewMain.FocusedRowHandle < 0)
                {
                    return null;
                }
                string fseq;
                DataTable dt = (DataTable)gridControlDataList.DataSource;
                if (dt.Rows[gridViewMain.FocusedRowHandle]["序号"] != null)
                {
                    fseq = dt.Rows[gridViewMain.FocusedRowHandle]["序号"].ToString();
                    Service mySQLSerVice = new Service();
                    switch (selectedQueryObj.NAME)
                    {
                        case "土壤":
//                             SoilService.SoilServicePortTypeClient soilclient = new SoilService.SoilServicePortTypeClient("SoilServiceHttpSoap12Endpoint");
//                             bpData = soilclient.getGPSJ(fseq);
                            bpData = mySQLSerVice.getSoilGPSJ(fseq);
                            break;
                        case "南方植被":
//                             VSouthService.VegetationSouthServicePortTypeClient vsouthclient = new VSouthService.VegetationSouthServicePortTypeClient("VegetationSouthServiceHttpSoap12Endpoint");
//                             bpData = vsouthclient.getGPSJ(fseq);
                            bpData = mySQLSerVice.getSouVegGPSJ(fseq);
                            break;
                        case "北方植被":
//                             VNorthService.VegetationNorthServicePortTypeClient vnorthclient = new VNorthService.VegetationNorthServicePortTypeClient("VegetationNorthServiceHttpSoap11Endpoint");
//                             bpData = vnorthclient.getGPSJ(fseq);
                            bpData = mySQLSerVice.getNorVegGPSJ(fseq);
                            break;
                        case "城市目标":
//                             CityObjService.CityObjServicePortTypeClient cityObj = new CityObjService.CityObjServicePortTypeClient("CityObjServiceHttpSoap12Endpoint");
//                             bpData = cityObj.getGPSJ(fseq);
                            bpData = mySQLSerVice.getCityGPSJ(fseq);
                            break;
                        case "地表大气":
                            break;
                        case "水体":
//                             WaterService.WaterServicePortTypeClient waterclient = new WaterService.WaterServicePortTypeClient("WaterServiceHttpSoap12Endpoint");
//                             bpData = waterclient.getGPSJ(fseq);
                            bpData = mySQLSerVice.getWaterGPSJ(fseq);
                            break;
                        case "岩矿":
//                             RockService.RockServicePortTypeClient rockclient = new RockService.RockServicePortTypeClient("RockServiceHttpSoap12Endpoint");
//                             bpData = rockclient.getGPSJ(fseq);
                            bpData = mySQLSerVice.getRockGPSJ(fseq);
                            break;
                        default:
                            break;
                    }
                }
                return bpData;
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        /// <summary>
        /// 解析波普曲线数据，转换为可显示的数据序列
        /// </summary>
        /// <param name="bpData"></param>
        /// <returns></returns>
        double[,] ResolveBopuData(string bpData)
        {
            try
            {
                BopuData bopuData = JSON.JSON.parse<BopuData>(bpData);
                if (bopuData.types != null && bopuData.types.Length > 0)
                {
                    string str = bopuData.types[0].type;
                    string[] strArr = str.Split(";".ToCharArray());

                    double[,] bopudata = new double[(strArr.Length + 100) / 100, 2];
                    int j = 0;
                    for (int i = 0; i < strArr.Length - 1; i++)
                    {
                        string[] doubleata = strArr[i].Split(",".ToCharArray());
                        bopudata[j, 0] = Double.Parse(doubleata[0]);
                        bopudata[j, 1] = Double.Parse(doubleata[1]);
                        j++;
                        i += 100;
                    }
                    return bopudata;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 展示波普曲线
        /// </summary>
        /// <param name="bopuSeq"></param>
        void DisplayBopuSeq(double[,] bopuSeq)
        {

            chartControl1.Series[0].Points.Clear();
            if(bopuSeq == null)
            {
                return;
            }
            for (int i = 0; i < bopuSeq.GetLength(0); i++)
            {
                chartControl1.Series[0].Points.Add(new SeriesPoint(bopuSeq[i, 0], bopuSeq[i, 1]));
            }
        }

        void GetBopuDataCallBack(IAsyncResult ar)
        {
            try
            {
                GetBopuDataDel del = (GetBopuDataDel)ar.AsyncState;
                string seq = del.EndInvoke(ar);
                if (chartControl1.IsHandleCreated)
                {
                    double[,] seqArr = ResolveBopuData(seq);
                    chartControl1.Invoke(new DisplayBopuSeqDel(DisplayBopuSeq), seqArr);
                }
            }
            catch (Exception)
            {
            }
        }

        public delegate string GetBopuDataDel();
        public delegate void DisplayBopuSeqDel(double[,] bopuSeq);
        #endregion

        #region 异步调用获取切片数据缩略图 
        public delegate Image GetTileImageDel(string tileFileName);

        Image GetTileImage(string _TileFileName)
        {
            if (_TileFileName.EndsWith(".png")||_TileFileName.EndsWith(".jpg"))
            {
                if(File.Exists(_TileFileName))
                {
                    return Image.FromFile(_TileFileName);
                }
            }

            try
            {
                string thumbnailName = _TileFileName.Remove(_TileFileName.IndexOf('-')) + ".jpg";
                string thumbnailPNGName = _TileFileName.Remove(_TileFileName.IndexOf('-')) + ".png";
                if (File.Exists(thumbnailPNGName))
                {
                    return Image.FromFile(thumbnailPNGName);
                }
                else if (File.Exists(thumbnailName))
                {
                    return Image.FromFile(thumbnailName);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        void GetTileImageCallBack(IAsyncResult ar)
        {
            GetTileImageDel del = (GetTileImageDel)ar.AsyncState;
            Image image = del.EndInvoke(ar);
            if (pictureEditImgData.IsHandleCreated)
            {
                pictureEditImgData.Invoke(new DiaplayDel(DisplayImage), image);
            }
        }

       

        #endregion

        #region 异步调用获取矢量数据
        public delegate string GetShapeFileDel(string fileDir);

        string GetShapeFile(string fileDir)
        {
            string[] shapeFiles = Directory.GetFiles(fileDir, "*.shp");//目前仅支持显示shapefile  
            if (shapeFiles.Length > 0)
            {
                return shapeFiles[0];
            }
            else
                return null;
        }

        void GetShapeFileCallBack(IAsyncResult ar)
        {
            GetShapeFileDel del = (GetShapeFileDel)ar.AsyncState;
            string shapeFile = del.EndInvoke(ar);
            if (MainMapImage.IsHandleCreated)
            {
                MainMapImage.Invoke(new DisplayShpFileDel(DisplayShpFile),shapeFile);
            }
        }

        public delegate void DisplayShpFileDel(string filePath);

        void DisplayShpFile(string filePath)
        {
            //zxw 20131221 
            try
            {
                MainMapImage.Map.Layers.Clear();
                if (!string.IsNullOrEmpty(filePath))
                {
                    SharpMap.Layers.VectorLayer layer = new SharpMap.Layers.VectorLayer(Path.GetFileNameWithoutExtension(filePath));
                    layer.DataSource = new SharpMap.Data.Providers.ShapeFile(filePath);
                    MainMapImage.Map.Layers.Add(layer);
                }
                MainMapImage.Refresh();
            }
            catch(ArgumentException)
            {
            }
          
        }
        #endregion


        /// <summary>
        /// 获取指定路径的缩略图
        /// </summary>
        /// <returns></returns>
        Image GetImageFromPath(string filePath)
        {
            if(File.Exists(filePath))
            {
                return Image.FromFile(filePath);
            }
            return null;
        }

        Image GetImageFromBlob(byte[] bytes,string qrst_code)
        {
            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                byte[] arrayByte = new byte[1024];
                int imgLong = (int)ms.Length;
                int l = 0;
                string cachPath = string.Format(@"{0}\{1}", Application.StartupPath, @"Cache\Thumbnail");
                string cachFile = string.Format(@"{0}\{1}.jpg", cachPath, qrst_code);
                FileStream fso = new FileStream(cachFile, FileMode.Create);
                while (l < imgLong)
                {
                    int i = ms.Read(arrayByte, 0, 1024);
                    fso.Write(arrayByte, 0, i);
                    l += i;
                }

                fso.Close();
                ms.Close();
                return Image.FromFile(cachFile);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        Image GetImageFromWeb(string imgUrl, string qrst_code)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
            request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
            request.Timeout = 3000;
            string cachPath = string.Format(@"{0}\{1}", Application.StartupPath, @"Cache\Thumbnail");
            string cachFile = string.Format(@"{0}\{1}.jpg", cachPath, qrst_code);
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    if (response.ContentType.ToLower().StartsWith("image/"))
                    {
                        byte[] arrayByte = new byte[1024];
                        int imgLong = (int)response.ContentLength;
                        int l = 0;
                        FileStream fso = new FileStream(cachFile, FileMode.Create);
                        while (l < imgLong)
                        {
                            int i = stream.Read(arrayByte, 0, 1024);
                            fso.Write(arrayByte, 0, i);
                            l += i;
                        }
                        fso.Close();
                        stream.Close();
                        response.Close();
                        return Image.FromFile(cachFile);
                    }
                    else
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将id转换为标准的QRSt_CODE形式”0001-MADB-10-9“,若不是识别的形式，则
        /// </summary>
        /// <returns></returns>
        string ConvertQRSTCODE(string id)
        {
            Regex rgx1 = new Regex(@"^\d{4}-\w{4}-\d{1,}-\d{1,}$");  //正则表达式匹配qrst_code的标准形式如0001-MADB-10-9
            Regex rgx2 = new Regex(@"^\w{4}-\d{1,}-\d{1,}$");          //正则表达式匹配qrst_code的老版本形式如MADB-10-9
            Regex rgx3 = new Regex(@"^\d{1,}$");                             //匹配ID号
            if (rgx1.IsMatch(id))
            {
                return id;
            }
            else if (rgx2.IsMatch(id))
            {
                return string.Format("{0}-{1}",Constant.INDUSTRYCODE,id);
            }
            else if (rgx3.IsMatch(id))
            {
                return string.Format("{0}-{1}-{2}", Constant.INDUSTRYCODE,queryPara.dataCode,id);
            }
            else
                return id;   //原来为"" zxw 20131223 应急
        }
        /// <summary>
        /// 判断字符是否为url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool IsUrl(string url)
        {
            if (url.StartsWith("http://"))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 判断路径下是否存在图片
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        bool ExistPicture(string _path,out string _picName)
        {
            _picName = "";
            if (Directory.Exists(_path))
            {
                string[] files = Directory.GetFiles(_path);
                for (int i = 0; i < files.Length;i++ )
                {
                    if (IsPic(files[i]))
                    {
                        _picName = files[i];
                        return true;
                    }
                }
                return false;
            }
            else if(File.Exists(_path))
            {
                if(IsPic(_path))
                {
                    _picName = _path;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断文件是否为图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool IsPic(string _filename)
        {
            string extent = Path.GetExtension(_filename).ToLower();
            if (extent.Contains("bmp") || extent.Contains("jpg") || extent.Contains("gif") || extent.Contains("png"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
		//判断是否是检索切片后第一次进入该页面。
		public bool isFirst = true;
        private void mucDetailViewer_VisibleChanged(object sender, EventArgs e)
        {
            muc3DSearcher _muc3DSearcher = (muc3DSearcher)MSUserInterface.listMSUI[0].uiMainUC;
          
            if (this.Visible)
            {
                if (_muc3DSearcher.Is3DViewer)
                {
                _muc3DSearcher.globeClickEvent += GlobeClickEvent;
                }
                else
                {
                    _muc3DSearcher.uc2DSearcher1.addMapClickEvent();
                    _muc3DSearcher.uc2DSearcher1.onClickCompletedEvent += new Action<double,double>(GlobeClickEvent);
                }
                //(DataTable)gridControlDataList.DataSource
                if (gridControlDataList.DataSource!=null)
                {
                    DataTable dt = (DataTable)gridControlDataList.DataSource;
                    if(dt.Rows.Count>0) //加载第一行信息
                    {
                        DataRow dr = dt.Rows[0];
                        DisplayDetailInfo(dr);
                        if (dr == null)
                        {
                            //清空显示面板
                            pictureEditImgData.Image = null;
                            return;
                        }
                        //显示缩略图信息
                        if (queryPara.QRST_CODE != null)
                        {
                            string id;
                            if ((!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector")) && selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() != "rcdb" && (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")))
                            {
                                id = dr[queryPara.QRST_CODE].ToString();
                                GetImageDel del = GetImage;
                                IAsyncResult ar = del.BeginInvoke(id, GetImageCallBack, del);
                            }
                            else if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                            {
                                GetBopuDataDel del = GetBopuData;
                                IAsyncResult ar = del.BeginInvoke(GetBopuDataCallBack, del);
                            }
                            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))          //展示矢量数据
                            {
                            }
                            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))             //展示切片数据 
                            {
								//if (isFirst)
								//{
								//    ctrlPage.SetPageSize(1000);
								//    isFirst = false;
								//}
                            }

                        }
                        else
                        {
                            pictureEditImgData.Image = null;
                        }
                    }
                }

                //加载三维球
                if (selectedQueryObj!=null&&(selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster") || selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")))
                {
                    if(_muc3DSearcher.Is3DViewer)
                    {
                this.pictureEditImgData.SetQrstAxGlobeControl(_muc3DSearcher.qrstAxGlobeControl1);
                    }
                    else
                    {
                        this.pictureEditImgData.SetUc2DSeacher(_muc3DSearcher.uc2DSearcher1);
                    }
                }

            }
            else
            {
                _muc3DSearcher.SetQrstAxGlobeControl();
                if (_muc3DSearcher.Is3DViewer)
                {
                _muc3DSearcher.globeClickEvent -= GlobeClickEvent;
                }
                else
                {
                    _muc3DSearcher.uc2DSearcher1.removeMapClickEvent();
                }
            }
        }

        public CtrlPage GetCtrlPage()
        {
            return this.ctrlPage;
        }

        public void ExportMetadata()
        {
			gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if(dt == null)
            {
                XtraMessageBox.Show("没有要导出的数据！");
                return;
            }
            DataTable selectedData = dt.Clone();
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                {
                   
                    DataRow dr = selectedData.NewRow();

                    for (int j = 0; j < selectedData.Columns.Count;j++ )
                    {
                        dr[j] = dt.Rows[i][j];
                    }
                    selectedData.Rows.Add(dr);
                }
            }

            FrmExportMetadata frmexportDara = new FrmExportMetadata(selectedData);
            frmexportDara.Show();
            //if (dt == null || dt.Rows.Count == 0)
            //{
            //    MessageBox.Show("没有需要导出的元数据信息！");
            //    return;
            //}
            //else
            //{
            //    FileStream fs = new FileStream(filename,FileMode.Create);
            //    StreamWriter sw = new StreamWriter(fs);
            //    StringBuilder header = new StringBuilder();
            //    for (int i = 0; i < dt.Columns.Count;i++ )
            //    {
            //        header.AppendFormat("{0}#;#",dt.Columns[i].ColumnName);
            //    }
            //    sw.WriteLine(header.ToString());
            //    for (int i = 0; i < dt.Rows.Count;i++ )
            //    {
            //        StringBuilder row = new StringBuilder();
            //        for (int j = 0; j < dt.Columns.Count;j++ )
            //        {
            //            row.AppendFormat("{0}#;#",dt.Rows[i][j].ToString());
            //        }
            //        sw.WriteLine(row.ToString());
            //    }
            //    sw.Close();
            //    fs.Close();

            //}
        }


        /// <summary>
        /// 网格覆盖统计分析
        /// </summary>
        public void GridStatistic(int Kc,int Kr)
        {
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if(dt == null)
            {
                return;
            }
            
            Dictionary<System.Drawing.RectangleF, int> extents = new Dictionary<System.Drawing.RectangleF, int>();

            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
            {
                RasterQueryPara para = (RasterQueryPara)queryPara;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Point2d upperleft = new Point2d(double.Parse(dt.Rows[i][para.DATAUPPERLEFTLAT].ToString()), double.Parse(dt.Rows[i][para.DATAUPPERLEFTLONG].ToString()));
                    Point2d upperright = new Point2d(double.Parse(dt.Rows[i][para.DATAUPPERRIGHTLAT].ToString()), double.Parse(dt.Rows[i][para.DATAUPPERRIGHTLONG].ToString()));
                    Point2d lowerleft = new Point2d(double.Parse(dt.Rows[i][para.DATALOWERLEFTLAT].ToString()), double.Parse(dt.Rows[i][para.DATALOWERLEFTLONG].ToString()));
                    Point2d lowerright = new Point2d(double.Parse(dt.Rows[i][para.DATALOWERRIGHTLAT].ToString()), double.Parse(dt.Rows[i][para.DATALOWERRIGHTLONG].ToString()));

                    //计算矩形的外接举行
                    Point2d[] outerExtent = SpacialUtil.SpacialUtils.GetOutExtent(upperleft, upperright, lowerleft, lowerright);
                    //计算矩形的行列范围
                    int[] rowcolRange = SpacialUtil.SpacialUtils.GetRowColRange(Kr, Kc, outerExtent[0], outerExtent[1]);
                    //将行列号转换为经纬度坐标后放入extents字典
                    int rowNum = rowcolRange[1] - rowcolRange[0]+1;
                    int colNum = rowcolRange[3] - rowcolRange[2]+1;
                    for (int j = 0; j < rowNum; j++)
                    {
                        for (int k = 0; k < colNum; k++)
                        {
                            System.Drawing.RectangleF key;
                            Point2d[] extent = SpacialUtil.SpacialUtils.GetGridExtent(Kr, Kc, rowcolRange[0] + j+1+1, rowcolRange[2] + k+1);
                            if (ExitRectage(extents, new RectangleF((float)extent[0].Y, (float)extent[0].X, (float)(extent[1].Y - extent[0].Y), (float)(extent[0].X - extent[1].X)), out key))
                            {
                                extents[key] = extents[key] + 1;
                            }
                            else
                            {
                                extents.Add(key, 1);
                            }
                        }
                    }
                }
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))   //切片统计
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string level = dt.Rows[i]["Level"].ToString();
                    string[] rowandColumn = new string[4];
                    rowandColumn[0] = dt.Rows[i]["Row"].ToString();
                    rowandColumn[1] = dt.Rows[i]["Col"].ToString();

                    double[] extent = GetLatAndLong(rowandColumn, level);
                    double minLat = double.Parse(extent[0].ToString());
                    double maxLat = double.Parse(extent[2].ToString());
                    double minLon = double.Parse(extent[1].ToString());
                    double maxLon = double.Parse(extent[3].ToString());

                    Point2d upperleft = new Point2d(maxLat, minLon);
                    Point2d upperright = new Point2d(maxLat, maxLon);
                    Point2d lowerleft = new Point2d(minLat, minLon);
                    Point2d lowerright = new Point2d(minLat, maxLon);

                            System.Drawing.RectangleF key;
                            Point2d[] extent1 = new Point2d[] { lowerleft, upperright }; 
        
                            if (ExitRectage(extents, new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[0].X - extent1[1].X)), out key))
                            {
                                extents[key] = extents[key] + 1;
                            }
                            else
                            {
                                extents.Add(key, 1);
                            }
                        //}
                   // }
                }
            }
            QrstAxGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
            if (_QrstAxGlobeControl != null)
            {
                int maxNum = 0;
                _QrstAxGlobeControl.DrawSearchResultExtents(extents,out maxNum);

                ColorRange colorRange = new ColorRange(maxNum, _QrstAxGlobeControl);
                colorRange.Visible = _QrstAxGlobeControl.IsOn("tmpDrawExtentsLayer1");
                //添加统计色带
                if(_QrstAxGlobeControl.Controls.ContainsKey("colorRange"))
                {
                    _QrstAxGlobeControl.Controls.RemoveByKey("colorRange");
                }
                _QrstAxGlobeControl.Controls.Add(colorRange);
               
            }
        }

        public void LayerControl(string layerName,bool _isOn)
        {
            QrstAxGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
            _QrstAxGlobeControl.LayerControl(layerName,_isOn);

            if (layerName == "tmpDrawExtentsLayer1")
            {
                //20130921  dlf因异常添加判断  异常出现在查询转入详细信息界面后，直接点击隐藏网格。
                if (_QrstAxGlobeControl.Controls["colorRange"]!=null)
                _QrstAxGlobeControl.Controls["colorRange"].Visible = _isOn;
            }
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

        /// <summary>
        /// 响应球体点击事件,根据点击的经纬度，找出详细信息列表中的记录并显示
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
       public void GlobeClickEvent(double lat,double lon)
        {
            if (selectedQueryObj != null && (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") || selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster")))
           {
                if(((RasterQueryPara)queryPara).spacialAvailable)
                {
                    RasterQueryPara para = (RasterQueryPara)queryPara;
                 //   List<int> rowHandler = new List<int>(); 
                    Dictionary<int, double> rowInfo = new Dictionary<int, double>();  //记录包含了该点的数据列编号和面积

                    //修改排序后，球面点击位置与实际定位记录不符的错误   zxw 20131018
            
                    
                    if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
                    {
                        for (int i = 0; i < gridViewMain.RowCount; i++)
                        {
                            Point2d upperleft = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERLEFTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERLEFTLONG].ToString()));
                            Point2d upperright = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERRIGHTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERRIGHTLONG].ToString()));
                            Point2d lowerleft = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERLEFTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERLEFTLONG].ToString()));
                            Point2d lowerright = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERRIGHTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERRIGHTLONG].ToString()));
                            if (recContains(upperleft, upperright, lowerleft, lowerright, new Point2d(lat, lon)))
                            {
                                rowInfo.Add(i, GetRectangleArea(upperleft, upperright, lowerleft, lowerright));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < gridViewMain.RowCount; i++)
                        {
                            string level = gridViewMain.GetDataRow(i)["Level"].ToString();
                            string[] rowandColumn = new string[4];
                            rowandColumn[0] = gridViewMain.GetDataRow(i)["Row"].ToString();
                            rowandColumn[1] = gridViewMain.GetDataRow(i)["Col"].ToString();

                            double[] extent = GetLatAndLong(rowandColumn, level);
                            double minLat = double.Parse(extent[0].ToString());
                            double maxLat = double.Parse(extent[2].ToString());
                            double minLon = double.Parse(extent[1].ToString());
                            double maxLon = double.Parse(extent[3].ToString());

                            Point2d upperleft = new Point2d(maxLat,minLon);
                            Point2d upperright = new Point2d(maxLat,maxLon);
                            Point2d lowerleft = new Point2d(minLat,minLon);
                            Point2d lowerright = new Point2d(minLat,maxLon);
                            if (recContains(upperleft, upperright, lowerleft, lowerright, new Point2d(lat, lon)))
                            {
                                rowInfo.Add(i, GetRectangleArea(upperleft, upperright, lowerleft, lowerright));
                            }
                        }
                    }

                    //将面积最小的矩形框显示
                    if(rowInfo.Count>0)
                    {
                        int minindex = -1;
                      foreach(KeyValuePair<int,double> keyvalue in rowInfo)
                      {
                          if (minindex == -1)
                          {
                              minindex = keyvalue.Key;
                          }
                          else
                          {
                              if(rowInfo[minindex]>keyvalue.Value)
                              {
                                  minindex = keyvalue.Key;
                              }
                          }
                      }
                        if(minindex!=-1)
                        {
                            gridViewMain.FocusedRowHandle = minindex;
                        }
                    }
                }
           }
        }

        /// <summary>
        /// 获取四边形面积
        /// </summary>
        /// <returns></returns>
       public double GetRectangleArea(Point2d upperleft,Point2d upperright,Point2d lowerleft,Point2d lowerright )
       {
           double a, b, c;
           a = GetDistance(upperleft,lowerright);
           b = GetDistance(upperleft,upperright);
           c = GetDistance(upperright,lowerright);
           double s1 = GetTriangleArea(a,b,c);

           b = GetDistance(upperleft,lowerleft);
           c = GetDistance(lowerleft,lowerright);
           double s2 = GetTriangleArea(a,b,c);
           return s1+s2;
       }

       double GetDistance(Point2d p1,Point2d p2)
       {
           return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X)+(p1.Y-p2.Y)*(p1.Y-p2.Y));
       }
        /// <summary>
        /// 根据三边长计算三角形面积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
       double GetTriangleArea(double a,double b,double c)
       {
           double p = (a + b + c) / 2;
           return Math.Sqrt(p*(p-a)*(p-b)*(p-c));
       }

        /// <summary>
       /// 判断objectPoint是否在前面四个点形成的矩形的外包矩形中
        /// </summary>
        /// <param name="upperleft"></param>
        /// <param name="upperright"></param>
        /// <param name="lowerleft"></param>
        /// <param name="lowerright"></param>
        /// <param name="objectPoint"></param>
        /// <returns></returns>
       public bool recContains(Point2d upperleft, Point2d upperright, Point2d lowerleft, Point2d lowerright,Point2d objectPoint)
       {
           if ((objectPoint.X <= Math.Max(upperright.X, upperleft.X)) && (objectPoint.X >= Math.Min(lowerleft.X, lowerright.X)) && (objectPoint.Y >= Math.Min(lowerleft.Y, upperleft.Y)) && (objectPoint.Y <= Math.Max(lowerright.Y, upperright.Y)))
           {
               return true;
           }
           return false;
       }

        /// <summary>
        /// 变化   zxw 20131221
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void splitContainerControlgrid_SizeChanged(object sender, EventArgs e)
       {
           if (displaySchema == EnumDisplayEnum.AllTable)
           {

               splitContainerControlgrid.SplitterPosition = splitContainerControlgrid.Width * 3 / 4;
               splitContainerControlTableViewer.SplitterPosition = 0;
               splitContainerControlImgAndDetail.SplitterPosition = 0;
               int height = splitContainerControlgrid.Height;
           }
           else
           {
               splitContainerControlgrid.SplitterPosition = splitContainerControlgrid.Width;
           }
           
       }

    
    }

    public enum EnumDisplayEnum
    {
        AllTable = 0,   //全表模式
        AllPicture = 1,  //全图
        Normal =2,      //正常模式

    }

}
