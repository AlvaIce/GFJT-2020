using System;
using System.Collections.Generic;
using System.Data;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucDataAnalyst : DevExpress.XtraEditors.XtraUserControl
    {
        struct DataCount
        {
            public string 数据类型 { get; set; }
            public int 数据量 { get; set; }
            public List<DataCount> 子分类 { get; set; }
        }

        private SiteDb _db;

        public SiteDb Db
        {
            get
            {
                return _db;
            }
            set
            {
                _db = value;
                RefreshDisplayedData();
            }
        }

        public mucDataAnalyst()
        {
            InitializeComponent();
        }

       /// <summary>
        /// 当切换数据库时，刷新统计信息
       /// </summary>
        public void RefreshDisplayedData()
        {
            if(_db != null)
            {
                chartControl1.Visible = true;
                panelControl1.Visible = true;

                panelControl2.Visible = false;
                panelControlData.Visible = false; 

                lbldbName.Text = _db.DESCRIPTION;
                lblDocCount.Text = _db.GetSpecificDataRecord(EnumDataKind.System_Document).ToString();
                lblRasterCount.Text = _db.GetSpecificDataRecord(EnumDataKind.System_Raster).ToString();
                lblVectorCount.Text = _db.GetSpecificDataRecord(EnumDataKind.System_Vector).ToString();
                lblTableCount.Text = _db.GetSpecificDataRecord(EnumDataKind.System_Table).ToString();

                lblTotalCount.Text = _db.GetTotalRecord().ToString();

                //造数据
                //Random ran = new Random();
                //double vectorSize=ran.NextDouble() * 97;
                //if (!String.IsNullOrEmpty(lblVectorCount.Text))
                //{
                //    if (int.Parse(lblVectorCount.Text) > 0)
                //    {
                //        lblVectorSize.Text = vectorSize.ToString().Substring(0,5) + "MB";    
                //    }
                //    else
                //    {
                //        vectorSize = 0;
                //    }
                //}
             
                //double rasterSize = ran.NextDouble() * 100067;
                //   if (!String.IsNullOrEmpty(lblRasterCount.Text ))
                //{
                //    if (int.Parse(lblRasterCount.Text ) > 0)
                //    {
                //        lblRasterSize.Text = rasterSize.ToString().Substring(0,9)+ "MB";   
                //    }
                //    else
                //    {
                //        rasterSize = 0;
                //    }
                //}
                  
                //double tableSize = ran.NextDouble() * 98;
                //       if (!String.IsNullOrEmpty(  lblTableCount.Text  ))
                //{
                //    if (int.Parse(  lblTableCount.Text  ) > 0)
                //    {
                //        lblTableSize.Text = tableSize.ToString().Substring(0,5) + "MB";   
                //    }
                //    else
                //    {
                //        tableSize = 0;
                //    }
                //}
                     
                //lblTotalSize.Text = (tableSize + vectorSize + rasterSize).ToString() + "MB";
                       //
                //修改Chart信息
                //chartControl1.SeriesSerializable 
            //    this.chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            //chartTitle1});
                 DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
                chartTitle1.Text = _db.DESCRIPTION+"数据类型统计";
                  chartTitle1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.chartControl1.Titles.Clear();
                this.chartControl1.Titles.Add(chartTitle1);
              //  object obj = chartControl1.SeriesSerializable[0].Points[0].Values[0];
                chartControl1.SeriesSerializable[0].Points[0].Values[0] = double.Parse(lblVectorCount.Text);
                chartControl1.SeriesSerializable[0].Points[1].Values[0] = double.Parse(lblRasterCount.Text);
                chartControl1.SeriesSerializable[0].Points[2].Values[0] = double.Parse(lblDocCount.Text);
                chartControl1.SeriesSerializable[0].Points[3].Values[0] = double.Parse(lblTableCount.Text);
                chartControl1.RefreshData();
            }
        }
        /// <summary>
        /// 具体数据类型的统计分析
        /// </summary>
        public void DisplaySpecificData(EnumDataKind datakind)
        {
            List<EnumDataKind> datakinds = new List<EnumDataKind>();
            datakinds.Add(datakind);
            DisplaySpecificDatas(datakinds);

        }
        /// <summary>
        /// 2016/12/15   数据库增加一类文档数据（System_NormalFile）
        /// </summary>
        /// <param name="datakinds"></param>
        public void DisplaySpecificDatas(List<EnumDataKind> datakinds)
        {
            chartControl1.Visible = false;
            panelControl1.Visible = false;

            panelControl2.Visible = true;
            panelControlData.Visible = true;

            //将信息显示在gridcontrol
            DataTable dt = new DataTable();
            DataColumn column1 = new DataColumn { ColumnName = "DataType", DataType = Type.GetType("System.String") };
            DataColumn column2 = new DataColumn { ColumnName = "DataNum", DataType = Type.GetType("System.Int32") };

            dt.Columns.Add(column1);
            dt.Columns.Add(column2);

            //将统计信息显示在柱状图中
            //标题
            string title = "";
            Dictionary<string, int> dics = new Dictionary<string, int>();
            foreach (var datakind in datakinds)
            {
                switch (datakind)
                {
                    case EnumDataKind.System_Document: title = "文档数据"; break;
                    case EnumDataKind.System_NormalFile: title = "文档数据"; break;
                    case EnumDataKind.System_Raster: title = "栅格数据 "; break;
                    case EnumDataKind.System_Table: title = "表格数据 "; break;
                    case EnumDataKind.System_Vector: title = "矢量数据"; break;
                    default: break;
                }

                Dictionary<string, int> dic = Db.GetSpecificDataCount(datakind);
                foreach (KeyValuePair<string, int> kvp in dic)
                {
                    try
                    {
                        dics.Add(kvp.Key, kvp.Value);
                    }
                    catch
                    { }
                }
            }
            List<DataCount> datacounts = CreateDataCount(dics);
            chartControlBar.SeriesSerializable[0].Points.Clear();
            chartControlBar.SeriesSerializable[0].LegendText = "数据记录数";
            foreach (DataCount temp in datacounts)
            {
                //DataRow dr = dt.NewRow();
                //dr["DataType"] = temp.Key;
                //dr["DataNum"] = temp.Value;
                //dt.Rows.Add(dr);

                DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(temp.数据类型, new object[] {
            (temp.数据量)});
                chartControlBar.SeriesSerializable[0].Points.Add(seriesPoint1);
            }
            //gridControl1.DataSource = dt;
            gridControl1.DataSource = datacounts;

            this.chartControlBar.Titles[0].Text = Db.DESCRIPTION + title + "分类统计";
            this.chartControlBar.RefreshData();
        }

        private List<DataCount> CreateDataCount(Dictionary<string, int> dic)
        {
            List<DataCount> output=new List<DataCount>();
            foreach (KeyValuePair<string,int> kvp in dic)
            {
                DataCount dc = new DataCount();
                dc.数据类型 = kvp.Key;
                dc.数据量 = kvp.Value;
                dc.子分类 = getDataCountChildSet(kvp.Key);
                output.Add(dc);
            }
            return output;
        }

        private List<DataCount> getDataCountChildSet(string p)
        {
            List<DataCount> output = new List<DataCount>();
            switch (p)
            {
                case "高分系列卫星数据":
                    try
                    {
                        string sql = "select SatelliteID, count(*) from prod_gf1  GROUP BY SatelliteID;";
                        DataSet ds = Db.sqlUtilities.GetDataSet(sql);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DataCount dc = new DataCount();
                                dc.数据类型 = dr[0].ToString();
                                dc.数据量 = int.Parse(dr[1].ToString());
                                output.Add(dc);
                            }
                        }
                    }
                    catch
                    { }
                    break;
                case "非规影像级产品":
                    try
                    {
                        string sql = "select ProdType, count(*) from imageprod  GROUP BY ProdType;";
                        DataSet ds = Db.sqlUtilities.GetDataSet(sql);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DataCount dc = new DataCount();
                                dc.数据类型 = dr[0].ToString();
                                dc.数据量 = int.Parse(dr[1].ToString());
                                output.Add(dc);
                            }
                        }
                    }
                    catch
                    { }
                    break;
                default:
                    break;
            }
            return output;
        }
        
        /// <summary>
        /// 获取对应数据类型的详细信息
        /// </summary>
        /// <param name="datakind"></param>
        /// <returns></returns>
        //DataTable GetDetailInfo(EnumDataKind datakind)
        //{
        //    DataTable dt = new DataTable();
        //    DataColumn column1 = new DataColumn { ColumnName = "DataType", DataType = Type.GetType("System.String") };
        //    DataColumn column2 = new DataColumn { ColumnName = "DataNum", DataType = Type.GetType("System.Int32") };

        //    dt.Columns.Add(column1);
        //    dt.Columns.Add(column2);

        //    Dictionary<string, int> dic = Db.GetSpecificDataCount(datakind);
        //    foreach (var temp in dic)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["DataType"] = temp.Key;
        //        dr["DataNum"] = temp.Value;
        //        dt.Rows.Add(dr);
        //    }
        //}
    }
}
