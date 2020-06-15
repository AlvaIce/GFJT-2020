using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmExportMetadata : DevExpress.XtraEditors.XtraForm
    {
        public List<List<Coordinate>> parts;
        public List<Coordinate> coordinates;
        List<Coordinate> part;
        metadatacatalognode_Mdl selectedQueryObj = null;       
        public FrmExportMetadata(DataTable _dt)
        {
            InitializeComponent();
            dt = _dt;
            if (dt != null)
            {
                for (int i = 0; i < dt.Columns.Count-1;i++ )
                {
                    checkedListBoxColumns.Items.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName, CheckState.Checked, true);
                }
            }
            else
            {
                checkedListBoxColumns.Items.Clear();
            }
        }

        private DataTable dt = null;

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            if (LeftButtonUserControl.button.Contains("基础空间") || LeftButtonUserControl.button.Contains("实验验证") || LeftButtonUserControl.button.Contains("信息产品"))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "文本文件(*.txt)|*.txt|矢量文件(*.shp)|*.shp";       //"文本文件(*.txt,shapefile文件.shp)|*.txt;*.shp";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtStorePath.Text = sfd.FileName;
                    if (txtStorePath.Text.EndsWith("shp"))
                    {
                        this.labelControl2.IsAccessible = false;
                        this.radioGroupDivideChar.IsAccessible = false;
                        this.labelControl2.Visible = false;
                        this.radioGroupDivideChar.Visible = false;

                    }
                }
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "文本文件(*.txt)|*.txt";       //"文本文件(*.txt,shapefile文件.shp)|*.txt;*.shp";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtStorePath.Text = sfd.FileName;                   
                }           
            }
        }
        /// <summary>
        /// 新建矢量文件-实验验证数据库-高分系列卫星数据
        /// zsm 2017-1-4
        /// </summary>
        public void GFDataShapeFile()
        {
            //新建Shapefile
            parts = new List<List<Coordinate>>();
            FeatureSet fs = new FeatureSet(FeatureType.Polygon);
            fs.Projection = ProjectionInfo.FromEpsgCode(4326);
            fs.CoordinateType = CoordinateType.Regular;
            for (int i = 0; i < checkedListBoxColumns.CheckedItemsCount; i++)
            {
                fs.DataTable.Columns.Add(checkedListBoxColumns.CheckedItems[i].ToString());
            }
            //for (int i = 0; i < dt.Columns.Count - 1; i++)
            //{
            //    fs.DataTable.Columns.Add(dt.Columns[i].ColumnName);         
            //}
            fs.IndexMode = false;
            fs.FilePath = txtStorePath.Text;
            //插入行
            Feature f = null;
            if (fs.FeatureType == FeatureType.Polygon)
            {
                int k = 0;
                int g = 0;
                for (int j = 0; j < dt.Columns.Count - 1; j++)
                {
                    if (dt.Columns[j].ColumnName == "左上纬度" || dt.Columns[j].ColumnName == "DATAUPPERLEFTLAT")//左上经度,右上纬度,右上经度,右下纬度,右下经度,左下纬度,左下经度
                    {
                        k = j;
                        break;                   
                    }
                }

                for (int a = 0; a < dt.Columns.Count - 1; a++)
                {
                    if (dt.Columns[a].ColumnName == "数据范围上" || dt.Columns[a].ColumnName == "EXTENTUP")
                    {
                        g = a;
                        break;
                    }
                }
                if (k != 0)//表示坐标名称是以“左上纬度”等来表示的
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        part = new List<Coordinate>();
                        Shape shp = new Shape(fs.FeatureType);//6 5
                        Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][k + 1].ToString()), Convert.ToDouble(dt.Rows[i][k].ToString()));//放入两个double值
                        Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][k + 3].ToString()), Convert.ToDouble(dt.Rows[i][k + 2].ToString()));//放入两个double值
                        Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][k + 5].ToString()), Convert.ToDouble(dt.Rows[i][k + 4].ToString()));//放入两个double值
                        Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][k + 7].ToString()), Convert.ToDouble(dt.Rows[i][k + 6].ToString()));//放入两个double值
                        part.Add(coord1);
                        part.Add(coord2);
                        part.Add(coord3);
                        part.Add(coord4);
                        shp.AddPart(part, fs.CoordinateType);
                        f = new Feature(shp);
                        fs.Features.Add(f);//插入行到表
                        lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
                        progressBarControl1.EditValue = i;

                        for (int j = 0; j < checkedListBoxColumns.ItemCount; j++)
                        {
                            if (checkedListBoxColumns.Items[j].CheckState == CheckState.Checked)
                            {
                                f.DataRow[checkedListBoxColumns.Items[j].ToString()] = dt.Rows[i][j].ToString();
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        part = new List<Coordinate>();
                        Shape shp = new Shape(fs.FeatureType);//
                        Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][g + 2].ToString()), Convert.ToDouble(dt.Rows[i][g].ToString()));//放入两个double值
                        Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][g + 3].ToString()), Convert.ToDouble(dt.Rows[i][g].ToString()));//放入两个double值
                        Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][g + 3].ToString()), Convert.ToDouble(dt.Rows[i][g + 1].ToString()));//放入两个double值
                        Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][g + 2].ToString()), Convert.ToDouble(dt.Rows[i][g + 1].ToString()));//放入两个double值
                        part.Add(coord1);
                        part.Add(coord2);
                        part.Add(coord3);
                        part.Add(coord4);
                        shp.AddPart(part, fs.CoordinateType);
                        f = new Feature(shp);
                        fs.Features.Add(f);//插入行到表
                        lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
                        progressBarControl1.EditValue = i;

                        for (int j = 0; j < checkedListBoxColumns.ItemCount; j++)
                        {
                            if (checkedListBoxColumns.Items[j].CheckState == CheckState.Checked)
                            {
                                f.DataRow[checkedListBoxColumns.Items[j].ToString()] = dt.Rows[i][j].ToString();
                            }
                        }
                    }
                
                }
            }
            //所有行插入后，进行表更新
            try
            {
                fs.InvalidateVertices();
            }
            catch
            { }
            fs.UpdateExtent();
            fs.Save();
            
        }
        #region
        /// <summary>
        /// 新建矢量文件-基础空间数据库-基础矢量数据
        /// zsm 2017-1-4
        /// </summary>
        //public void BasicShpDataShapeFile()
        //{
        //    //新建Shapefile
        //    parts = new List<List<Coordinate>>();
        //    FeatureSet fs = new FeatureSet(FeatureType.Polygon);
        //    fs.Projection = ProjectionInfo.FromEpsgCode(4326);
        //    fs.CoordinateType = CoordinateType.Regular;
        //    fs.DataTable.Columns.Add("元数据名称", typeof(string));        
        //    fs.DataTable.Columns.Add("产品名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品生产日期", typeof(DateTime));      
        //    fs.DataTable.Columns.Add("产品生产单位", typeof(string));
        //    fs.DataTable.Columns.Add("数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("数据来源", typeof(string));
        //    fs.DataTable.Columns.Add("数据类型", typeof(string));
        //    fs.DataTable.Columns.Add("数据范围上", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围下", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围左", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围右", typeof(double));
        //    fs.DataTable.Columns.Add("数据大小", typeof(string));
        //    fs.DataTable.Columns.Add("数据格式", typeof(string));
        //    fs.DataTable.Columns.Add("地图投影参考", typeof(string));
        //    fs.DataTable.Columns.Add("坐标系", typeof(string));
        //    fs.DataTable.Columns.Add("度代号", typeof(string));
        //    fs.DataTable.Columns.Add("密级", typeof(string));
        //    fs.DataTable.Columns.Add("数据质量", typeof(string));
        //    fs.DataTable.Columns.Add("分辨率", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作时间", typeof(DateTime));
        //    fs.DataTable.Columns.Add("元数据制作单位", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作人", typeof(string));

        //    fs.DataTable.Columns.Add("备注", typeof(string));
        //    fs.DataTable.Columns.Add("类型编码", typeof(string));
        //    fs.DataTable.Columns.Add("数据编码", typeof(string));
        //    fs.IndexMode = false;
        //    fs.FilePath = txtStorePath.Text;

        //    //插入行
        //    Feature f = null;
        //    if (fs.FeatureType == FeatureType.Polygon)
        //    {

        //        //怎样把四个坐标赋值给这个list???
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            part = new List<Coordinate>();
        //            Shape shp = new Shape(fs.FeatureType);
        //            Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            part.Add(coord1);
        //            part.Add(coord2);
        //            part.Add(coord3);
        //            part.Add(coord4);
        //            shp.AddPart(part, fs.CoordinateType);
        //            f = new Feature(shp);
        //            fs.Features.Add(f);//插入行到表
        //            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
        //            progressBarControl1.EditValue = i;
        //            f.DataRow["元数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[0].ToString()].ToString();
        //            f.DataRow["产品名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[1].ToString()].ToString();
        //            f.DataRow["产品生产日期"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[2].ToString()].ToString());
        //            f.DataRow["产品生产单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[3].ToString()].ToString();
        //            f.DataRow["数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[4].ToString()].ToString();
        //            f.DataRow["数据来源"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[5].ToString()].ToString();
        //            f.DataRow["数据类型"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[6].ToString()].ToString();
        //            f.DataRow["数据范围上"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString());
        //            f.DataRow["数据范围下"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString());
        //            f.DataRow["数据范围左"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString());
        //            f.DataRow["数据范围右"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString());
        //            f.DataRow["数据大小"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[11].ToString()].ToString();
        //            f.DataRow["数据格式"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["地图投影参考"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[13].ToString()].ToString();
        //            f.DataRow["坐标系"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[14].ToString()].ToString();
        //            f.DataRow["度代号"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[15].ToString()].ToString();
        //            f.DataRow["密级"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[16].ToString()].ToString();
        //            f.DataRow["数据质量"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[17].ToString()].ToString();
        //            f.DataRow["分辨率"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[18].ToString()].ToString();
        //            f.DataRow["元数据制作时间"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[19].ToString()].ToString());
        //            f.DataRow["元数据制作单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[20].ToString()].ToString();
        //            f.DataRow["元数据制作人"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[21].ToString()].ToString();

        //            f.DataRow["备注"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["类型编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[23].ToString()].ToString();
        //            f.DataRow["数据编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[24].ToString()].ToString();


        //        }
        //    }
        
        //    //所有行插入后，进行表更新
        //    try
        //    {
        //        fs.InvalidateVertices();
        //    }
        //    catch
        //    { }
        //    fs.UpdateExtent();
        //    fs.Save();
        
        //}
        //public void EnvironmentDataShapeFile()
        //{
        //    //新建Shapefile
        //    parts = new List<List<Coordinate>>();
        //    FeatureSet fs = new FeatureSet(FeatureType.Polygon);
        //    fs.Projection = ProjectionInfo.FromEpsgCode(4326);
        //    fs.CoordinateType = CoordinateType.Regular;
        //    fs.DataTable.Columns.Add("元数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品生产日期", typeof(DateTime));
        //    fs.DataTable.Columns.Add("产品生产单位", typeof(string));
        //    fs.DataTable.Columns.Add("数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("数据来源", typeof(string));
        //    fs.DataTable.Columns.Add("数据类型", typeof(string));
        //    fs.DataTable.Columns.Add("数据范围上", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围下", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围左", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围右", typeof(double));
        //    fs.DataTable.Columns.Add("数据大小", typeof(string));
        //    fs.DataTable.Columns.Add("数据格式", typeof(string));
        //    fs.DataTable.Columns.Add("地图投影参考", typeof(string));
        //    fs.DataTable.Columns.Add("坐标系", typeof(string));
        //    fs.DataTable.Columns.Add("度代号", typeof(string));
        //    fs.DataTable.Columns.Add("密级", typeof(string));
        //    fs.DataTable.Columns.Add("数据质量", typeof(string));
        //    fs.DataTable.Columns.Add("分辨率", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作时间", typeof(DateTime));
        //    fs.DataTable.Columns.Add("元数据制作单位", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作人", typeof(string));

        //    fs.DataTable.Columns.Add("备注", typeof(string));
        //    fs.DataTable.Columns.Add("类型编码", typeof(string));
        //    fs.DataTable.Columns.Add("数据编码", typeof(string));
        //    fs.IndexMode = false;
        //    fs.FilePath = txtStorePath.Text;

        //    //插入行
        //    Feature f = null;
        //    if (fs.FeatureType == FeatureType.Polygon)
        //    {

        //        //怎样把四个坐标赋值给这个list???
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            part = new List<Coordinate>();
        //            Shape shp = new Shape(fs.FeatureType);
        //            Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            part.Add(coord1);
        //            part.Add(coord2);
        //            part.Add(coord3);
        //            part.Add(coord4);
        //            shp.AddPart(part, fs.CoordinateType);
        //            f = new Feature(shp);
        //            fs.Features.Add(f);//插入行到表
        //            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
        //            progressBarControl1.EditValue = i;
        //            f.DataRow["元数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[0].ToString()].ToString();
        //            f.DataRow["产品名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[1].ToString()].ToString();
        //            f.DataRow["产品生产日期"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[2].ToString()].ToString());
        //            f.DataRow["产品生产单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[3].ToString()].ToString();
        //            f.DataRow["数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[4].ToString()].ToString();
        //            f.DataRow["数据来源"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[5].ToString()].ToString();
        //            f.DataRow["数据类型"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[6].ToString()].ToString();
        //            f.DataRow["数据范围上"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString());
        //            f.DataRow["数据范围下"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString());
        //            f.DataRow["数据范围左"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString());
        //            f.DataRow["数据范围右"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString());
        //            f.DataRow["数据大小"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[11].ToString()].ToString();
        //            f.DataRow["数据格式"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["地图投影参考"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[13].ToString()].ToString();
        //            f.DataRow["坐标系"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[14].ToString()].ToString();
        //            f.DataRow["度代号"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[15].ToString()].ToString();
        //            f.DataRow["密级"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[16].ToString()].ToString();
        //            f.DataRow["数据质量"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[17].ToString()].ToString();
        //            f.DataRow["分辨率"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[18].ToString()].ToString();
        //            f.DataRow["元数据制作时间"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[19].ToString()].ToString());
        //            f.DataRow["元数据制作单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[20].ToString()].ToString();
        //            f.DataRow["元数据制作人"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[21].ToString()].ToString();

        //            f.DataRow["备注"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["类型编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[23].ToString()].ToString();
        //            f.DataRow["数据编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[24].ToString()].ToString();


        //        }
        //    }

        //    //所有行插入后，进行表更新
        //    try
        //    {
        //        fs.InvalidateVertices();
        //    }
        //    catch
        //    { }
        //    fs.UpdateExtent();
        //    fs.Save();

        //}
        //public void CbersDataShapeFile()
        //{
        //    //新建Shapefile
        //    parts = new List<List<Coordinate>>();
        //    FeatureSet fs = new FeatureSet(FeatureType.Polygon);
        //    fs.Projection = ProjectionInfo.FromEpsgCode(4326);
        //    fs.CoordinateType = CoordinateType.Regular;
        //    fs.DataTable.Columns.Add("元数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品生产日期", typeof(DateTime));
        //    fs.DataTable.Columns.Add("产品生产单位", typeof(string));
        //    fs.DataTable.Columns.Add("数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("数据来源", typeof(string));
        //    fs.DataTable.Columns.Add("数据类型", typeof(string));
        //    fs.DataTable.Columns.Add("数据范围上", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围下", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围左", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围右", typeof(double));
        //    fs.DataTable.Columns.Add("数据大小", typeof(string));
        //    fs.DataTable.Columns.Add("数据格式", typeof(string));
        //    fs.DataTable.Columns.Add("地图投影参考", typeof(string));
        //    fs.DataTable.Columns.Add("坐标系", typeof(string));
        //    fs.DataTable.Columns.Add("度代号", typeof(string));
        //    fs.DataTable.Columns.Add("密级", typeof(string));
        //    fs.DataTable.Columns.Add("数据质量", typeof(string));
        //    fs.DataTable.Columns.Add("分辨率", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作时间", typeof(DateTime));
        //    fs.DataTable.Columns.Add("元数据制作单位", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作人", typeof(string));

        //    fs.DataTable.Columns.Add("备注", typeof(string));
        //    fs.DataTable.Columns.Add("类型编码", typeof(string));
        //    fs.DataTable.Columns.Add("数据编码", typeof(string));
        //    fs.IndexMode = false;
        //    fs.FilePath = txtStorePath.Text;

        //    //插入行
        //    Feature f = null;
        //    if (fs.FeatureType == FeatureType.Polygon)
        //    {

        //        //怎样把四个坐标赋值给这个list???
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            part = new List<Coordinate>();
        //            Shape shp = new Shape(fs.FeatureType);
        //            Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            part.Add(coord1);
        //            part.Add(coord2);
        //            part.Add(coord3);
        //            part.Add(coord4);
        //            shp.AddPart(part, fs.CoordinateType);
        //            f = new Feature(shp);
        //            fs.Features.Add(f);//插入行到表
        //            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
        //            progressBarControl1.EditValue = i;
        //            f.DataRow["元数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[0].ToString()].ToString();
        //            f.DataRow["产品名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[1].ToString()].ToString();
        //            f.DataRow["产品生产日期"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[2].ToString()].ToString());
        //            f.DataRow["产品生产单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[3].ToString()].ToString();
        //            f.DataRow["数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[4].ToString()].ToString();
        //            f.DataRow["数据来源"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[5].ToString()].ToString();
        //            f.DataRow["数据类型"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[6].ToString()].ToString();
        //            f.DataRow["数据范围上"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString());
        //            f.DataRow["数据范围下"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString());
        //            f.DataRow["数据范围左"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString());
        //            f.DataRow["数据范围右"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString());
        //            f.DataRow["数据大小"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[11].ToString()].ToString();
        //            f.DataRow["数据格式"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["地图投影参考"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[13].ToString()].ToString();
        //            f.DataRow["坐标系"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[14].ToString()].ToString();
        //            f.DataRow["度代号"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[15].ToString()].ToString();
        //            f.DataRow["密级"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[16].ToString()].ToString();
        //            f.DataRow["数据质量"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[17].ToString()].ToString();
        //            f.DataRow["分辨率"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[18].ToString()].ToString();
        //            f.DataRow["元数据制作时间"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[19].ToString()].ToString());
        //            f.DataRow["元数据制作单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[20].ToString()].ToString();
        //            f.DataRow["元数据制作人"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[21].ToString()].ToString();

        //            f.DataRow["备注"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["类型编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[23].ToString()].ToString();
        //            f.DataRow["数据编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[24].ToString()].ToString();


        //        }
        //    }

        //    //所有行插入后，进行表更新
        //    try
        //    {
        //        fs.InvalidateVertices();
        //    }
        //    catch
        //    { }
        //    fs.UpdateExtent();
        //    fs.Save();

        //}
        //public void modisDataShapeFile()
        //{
        //    //新建Shapefile
        //    parts = new List<List<Coordinate>>();
        //    FeatureSet fs = new FeatureSet(FeatureType.Polygon);
        //    fs.Projection = ProjectionInfo.FromEpsgCode(4326);
        //    fs.CoordinateType = CoordinateType.Regular;
        //    fs.DataTable.Columns.Add("元数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品生产日期", typeof(DateTime));
        //    fs.DataTable.Columns.Add("产品生产单位", typeof(string));
        //    fs.DataTable.Columns.Add("数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("数据来源", typeof(string));
        //    fs.DataTable.Columns.Add("数据类型", typeof(string));
        //    fs.DataTable.Columns.Add("数据范围上", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围下", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围左", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围右", typeof(double));
        //    fs.DataTable.Columns.Add("数据大小", typeof(string));
        //    fs.DataTable.Columns.Add("数据格式", typeof(string));
        //    fs.DataTable.Columns.Add("地图投影参考", typeof(string));
        //    fs.DataTable.Columns.Add("坐标系", typeof(string));
        //    fs.DataTable.Columns.Add("度代号", typeof(string));
        //    fs.DataTable.Columns.Add("密级", typeof(string));
        //    fs.DataTable.Columns.Add("数据质量", typeof(string));
        //    fs.DataTable.Columns.Add("分辨率", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作时间", typeof(DateTime));
        //    fs.DataTable.Columns.Add("元数据制作单位", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作人", typeof(string));

        //    fs.DataTable.Columns.Add("备注", typeof(string));
        //    fs.DataTable.Columns.Add("类型编码", typeof(string));
        //    fs.DataTable.Columns.Add("数据编码", typeof(string));
        //    fs.IndexMode = false;
        //    fs.FilePath = txtStorePath.Text;

        //    //插入行
        //    Feature f = null;
        //    if (fs.FeatureType == FeatureType.Polygon)
        //    {

        //        //怎样把四个坐标赋值给这个list???
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            part = new List<Coordinate>();
        //            Shape shp = new Shape(fs.FeatureType);
        //            Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            part.Add(coord1);
        //            part.Add(coord2);
        //            part.Add(coord3);
        //            part.Add(coord4);
        //            shp.AddPart(part, fs.CoordinateType);
        //            f = new Feature(shp);
        //            fs.Features.Add(f);//插入行到表
        //            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
        //            progressBarControl1.EditValue = i;
        //            f.DataRow["元数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[0].ToString()].ToString();
        //            f.DataRow["产品名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[1].ToString()].ToString();
        //            f.DataRow["产品生产日期"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[2].ToString()].ToString());
        //            f.DataRow["产品生产单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[3].ToString()].ToString();
        //            f.DataRow["数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[4].ToString()].ToString();
        //            f.DataRow["数据来源"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[5].ToString()].ToString();
        //            f.DataRow["数据类型"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[6].ToString()].ToString();
        //            f.DataRow["数据范围上"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString());
        //            f.DataRow["数据范围下"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString());
        //            f.DataRow["数据范围左"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString());
        //            f.DataRow["数据范围右"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString());
        //            f.DataRow["数据大小"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[11].ToString()].ToString();
        //            f.DataRow["数据格式"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["地图投影参考"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[13].ToString()].ToString();
        //            f.DataRow["坐标系"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[14].ToString()].ToString();
        //            f.DataRow["度代号"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[15].ToString()].ToString();
        //            f.DataRow["密级"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[16].ToString()].ToString();
        //            f.DataRow["数据质量"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[17].ToString()].ToString();
        //            f.DataRow["分辨率"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[18].ToString()].ToString();
        //            f.DataRow["元数据制作时间"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[19].ToString()].ToString());
        //            f.DataRow["元数据制作单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[20].ToString()].ToString();
        //            f.DataRow["元数据制作人"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[21].ToString()].ToString();

        //            f.DataRow["备注"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["类型编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[23].ToString()].ToString();
        //            f.DataRow["数据编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[24].ToString()].ToString();


        //        }
        //    }

        //    //所有行插入后，进行表更新
        //    try
        //    {
        //        fs.InvalidateVertices();
        //    }
        //    catch
        //    { }
        //    fs.UpdateExtent();
        //    fs.Save();

        //}
        //public void NOAADataShapeFile()
        //{
        //    //新建Shapefile
        //    parts = new List<List<Coordinate>>();
        //    FeatureSet fs = new FeatureSet(FeatureType.Polygon);
        //    fs.Projection = ProjectionInfo.FromEpsgCode(4326);
        //    fs.CoordinateType = CoordinateType.Regular;
        //    fs.DataTable.Columns.Add("元数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品生产日期", typeof(DateTime));
        //    fs.DataTable.Columns.Add("产品生产单位", typeof(string));
        //    fs.DataTable.Columns.Add("数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("数据来源", typeof(string));
        //    fs.DataTable.Columns.Add("数据类型", typeof(string));
        //    fs.DataTable.Columns.Add("数据范围上", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围下", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围左", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围右", typeof(double));
        //    fs.DataTable.Columns.Add("数据大小", typeof(string));
        //    fs.DataTable.Columns.Add("数据格式", typeof(string));
        //    fs.DataTable.Columns.Add("地图投影参考", typeof(string));
        //    fs.DataTable.Columns.Add("坐标系", typeof(string));
        //    fs.DataTable.Columns.Add("度代号", typeof(string));
        //    fs.DataTable.Columns.Add("密级", typeof(string));
        //    fs.DataTable.Columns.Add("数据质量", typeof(string));
        //    fs.DataTable.Columns.Add("分辨率", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作时间", typeof(DateTime));
        //    fs.DataTable.Columns.Add("元数据制作单位", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作人", typeof(string));

        //    fs.DataTable.Columns.Add("备注", typeof(string));
        //    fs.DataTable.Columns.Add("类型编码", typeof(string));
        //    fs.DataTable.Columns.Add("数据编码", typeof(string));
        //    fs.IndexMode = false;
        //    fs.FilePath = txtStorePath.Text;

        //    //插入行
        //    Feature f = null;
        //    if (fs.FeatureType == FeatureType.Polygon)
        //    {

        //        //怎样把四个坐标赋值给这个list???
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            part = new List<Coordinate>();
        //            Shape shp = new Shape(fs.FeatureType);
        //            Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            part.Add(coord1);
        //            part.Add(coord2);
        //            part.Add(coord3);
        //            part.Add(coord4);
        //            shp.AddPart(part, fs.CoordinateType);
        //            f = new Feature(shp);
        //            fs.Features.Add(f);//插入行到表
        //            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
        //            progressBarControl1.EditValue = i;
        //            f.DataRow["元数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[0].ToString()].ToString();
        //            f.DataRow["产品名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[1].ToString()].ToString();
        //            f.DataRow["产品生产日期"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[2].ToString()].ToString());
        //            f.DataRow["产品生产单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[3].ToString()].ToString();
        //            f.DataRow["数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[4].ToString()].ToString();
        //            f.DataRow["数据来源"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[5].ToString()].ToString();
        //            f.DataRow["数据类型"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[6].ToString()].ToString();
        //            f.DataRow["数据范围上"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString());
        //            f.DataRow["数据范围下"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString());
        //            f.DataRow["数据范围左"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString());
        //            f.DataRow["数据范围右"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString());
        //            f.DataRow["数据大小"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[11].ToString()].ToString();
        //            f.DataRow["数据格式"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["地图投影参考"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[13].ToString()].ToString();
        //            f.DataRow["坐标系"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[14].ToString()].ToString();
        //            f.DataRow["度代号"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[15].ToString()].ToString();
        //            f.DataRow["密级"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[16].ToString()].ToString();
        //            f.DataRow["数据质量"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[17].ToString()].ToString();
        //            f.DataRow["分辨率"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[18].ToString()].ToString();
        //            f.DataRow["元数据制作时间"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[19].ToString()].ToString());
        //            f.DataRow["元数据制作单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[20].ToString()].ToString();
        //            f.DataRow["元数据制作人"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[21].ToString()].ToString();

        //            f.DataRow["备注"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["类型编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[23].ToString()].ToString();
        //            f.DataRow["数据编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[24].ToString()].ToString();


        //        }
        //    }

        //    //所有行插入后，进行表更新
        //    try
        //    {
        //        fs.InvalidateVertices();
        //    }
        //    catch
        //    { }
        //    fs.UpdateExtent();
        //    fs.Save();

        //}
        //public void ZY02CDataShapeFile()
        //{
        //    //新建Shapefile
        //    parts = new List<List<Coordinate>>();
        //    FeatureSet fs = new FeatureSet(FeatureType.Polygon);
        //    fs.Projection = ProjectionInfo.FromEpsgCode(4326);
        //    fs.CoordinateType = CoordinateType.Regular;
        //    fs.DataTable.Columns.Add("元数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品生产日期", typeof(DateTime));
        //    fs.DataTable.Columns.Add("产品生产单位", typeof(string));
        //    fs.DataTable.Columns.Add("数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("数据来源", typeof(string));
        //    fs.DataTable.Columns.Add("数据类型", typeof(string));
        //    fs.DataTable.Columns.Add("数据范围上", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围下", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围左", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围右", typeof(double));
        //    fs.DataTable.Columns.Add("数据大小", typeof(string));
        //    fs.DataTable.Columns.Add("数据格式", typeof(string));
        //    fs.DataTable.Columns.Add("地图投影参考", typeof(string));
        //    fs.DataTable.Columns.Add("坐标系", typeof(string));
        //    fs.DataTable.Columns.Add("度代号", typeof(string));
        //    fs.DataTable.Columns.Add("密级", typeof(string));
        //    fs.DataTable.Columns.Add("数据质量", typeof(string));
        //    fs.DataTable.Columns.Add("分辨率", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作时间", typeof(DateTime));
        //    fs.DataTable.Columns.Add("元数据制作单位", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作人", typeof(string));

        //    fs.DataTable.Columns.Add("备注", typeof(string));
        //    fs.DataTable.Columns.Add("类型编码", typeof(string));
        //    fs.DataTable.Columns.Add("数据编码", typeof(string));
        //    fs.IndexMode = false;
        //    fs.FilePath = txtStorePath.Text;

        //    //插入行
        //    Feature f = null;
        //    if (fs.FeatureType == FeatureType.Polygon)
        //    {

        //        //怎样把四个坐标赋值给这个list???
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            part = new List<Coordinate>();
        //            Shape shp = new Shape(fs.FeatureType);
        //            Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            part.Add(coord1);
        //            part.Add(coord2);
        //            part.Add(coord3);
        //            part.Add(coord4);
        //            shp.AddPart(part, fs.CoordinateType);
        //            f = new Feature(shp);
        //            fs.Features.Add(f);//插入行到表
        //            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
        //            progressBarControl1.EditValue = i;
        //            f.DataRow["元数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[0].ToString()].ToString();
        //            f.DataRow["产品名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[1].ToString()].ToString();
        //            f.DataRow["产品生产日期"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[2].ToString()].ToString());
        //            f.DataRow["产品生产单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[3].ToString()].ToString();
        //            f.DataRow["数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[4].ToString()].ToString();
        //            f.DataRow["数据来源"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[5].ToString()].ToString();
        //            f.DataRow["数据类型"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[6].ToString()].ToString();
        //            f.DataRow["数据范围上"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString());
        //            f.DataRow["数据范围下"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString());
        //            f.DataRow["数据范围左"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString());
        //            f.DataRow["数据范围右"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString());
        //            f.DataRow["数据大小"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[11].ToString()].ToString();
        //            f.DataRow["数据格式"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["地图投影参考"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[13].ToString()].ToString();
        //            f.DataRow["坐标系"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[14].ToString()].ToString();
        //            f.DataRow["度代号"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[15].ToString()].ToString();
        //            f.DataRow["密级"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[16].ToString()].ToString();
        //            f.DataRow["数据质量"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[17].ToString()].ToString();
        //            f.DataRow["分辨率"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[18].ToString()].ToString();
        //            f.DataRow["元数据制作时间"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[19].ToString()].ToString());
        //            f.DataRow["元数据制作单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[20].ToString()].ToString();
        //            f.DataRow["元数据制作人"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[21].ToString()].ToString();

        //            f.DataRow["备注"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["类型编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[23].ToString()].ToString();
        //            f.DataRow["数据编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[24].ToString()].ToString();


        //        }
        //    }

        //    //所有行插入后，进行表更新
        //    try
        //    {
        //        fs.InvalidateVertices();
        //    }
        //    catch
        //    { }
        //    fs.UpdateExtent();
        //    fs.Save();

        //}
        //public void Z3DataShapeFile()
        //{
        //    //新建Shapefile
        //    parts = new List<List<Coordinate>>();
        //    FeatureSet fs = new FeatureSet(FeatureType.Polygon);
        //    fs.Projection = ProjectionInfo.FromEpsgCode(4326);
        //    fs.CoordinateType = CoordinateType.Regular;
        //    fs.DataTable.Columns.Add("元数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品名称", typeof(string));
        //    fs.DataTable.Columns.Add("产品生产日期", typeof(DateTime));
        //    fs.DataTable.Columns.Add("产品生产单位", typeof(string));
        //    fs.DataTable.Columns.Add("数据名称", typeof(string));
        //    fs.DataTable.Columns.Add("数据来源", typeof(string));
        //    fs.DataTable.Columns.Add("数据类型", typeof(string));
        //    fs.DataTable.Columns.Add("数据范围上", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围下", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围左", typeof(double));
        //    fs.DataTable.Columns.Add("数据范围右", typeof(double));
        //    fs.DataTable.Columns.Add("数据大小", typeof(string));
        //    fs.DataTable.Columns.Add("数据格式", typeof(string));
        //    fs.DataTable.Columns.Add("地图投影参考", typeof(string));
        //    fs.DataTable.Columns.Add("坐标系", typeof(string));
        //    fs.DataTable.Columns.Add("度代号", typeof(string));
        //    fs.DataTable.Columns.Add("密级", typeof(string));
        //    fs.DataTable.Columns.Add("数据质量", typeof(string));
        //    fs.DataTable.Columns.Add("分辨率", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作时间", typeof(DateTime));
        //    fs.DataTable.Columns.Add("元数据制作单位", typeof(string));
        //    fs.DataTable.Columns.Add("元数据制作人", typeof(string));

        //    fs.DataTable.Columns.Add("备注", typeof(string));
        //    fs.DataTable.Columns.Add("类型编码", typeof(string));
        //    fs.DataTable.Columns.Add("数据编码", typeof(string));
        //    fs.IndexMode = false;
        //    fs.FilePath = txtStorePath.Text;

        //    //插入行
        //    Feature f = null;
        //    if (fs.FeatureType == FeatureType.Polygon)
        //    {

        //        //怎样把四个坐标赋值给这个list???
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            part = new List<Coordinate>();
        //            Shape shp = new Shape(fs.FeatureType);
        //            Coordinate coord1 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord2 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString()));//放入两个double值
        //            Coordinate coord3 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            Coordinate coord4 = new Coordinate(Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString()), Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString()));//放入两个double值
        //            part.Add(coord1);
        //            part.Add(coord2);
        //            part.Add(coord3);
        //            part.Add(coord4);
        //            shp.AddPart(part, fs.CoordinateType);
        //            f = new Feature(shp);
        //            fs.Features.Add(f);//插入行到表
        //            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
        //            progressBarControl1.EditValue = i;
        //            f.DataRow["元数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[0].ToString()].ToString();
        //            f.DataRow["产品名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[1].ToString()].ToString();
        //            f.DataRow["产品生产日期"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[2].ToString()].ToString());
        //            f.DataRow["产品生产单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[3].ToString()].ToString();
        //            f.DataRow["数据名称"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[4].ToString()].ToString();
        //            f.DataRow["数据来源"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[5].ToString()].ToString();
        //            f.DataRow["数据类型"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[6].ToString()].ToString();
        //            f.DataRow["数据范围上"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[7].ToString()].ToString());
        //            f.DataRow["数据范围下"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[8].ToString()].ToString());
        //            f.DataRow["数据范围左"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[9].ToString()].ToString());
        //            f.DataRow["数据范围右"] = Convert.ToDouble(dt.Rows[i][checkedListBoxColumns.CheckedItems[10].ToString()].ToString());
        //            f.DataRow["数据大小"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[11].ToString()].ToString();
        //            f.DataRow["数据格式"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["地图投影参考"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[13].ToString()].ToString();
        //            f.DataRow["坐标系"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[14].ToString()].ToString();
        //            f.DataRow["度代号"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[15].ToString()].ToString();
        //            f.DataRow["密级"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[16].ToString()].ToString();
        //            f.DataRow["数据质量"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[17].ToString()].ToString();
        //            f.DataRow["分辨率"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[18].ToString()].ToString();
        //            f.DataRow["元数据制作时间"] = Convert.ToDateTime(dt.Rows[i][checkedListBoxColumns.CheckedItems[19].ToString()].ToString());
        //            f.DataRow["元数据制作单位"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[20].ToString()].ToString();
        //            f.DataRow["元数据制作人"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[21].ToString()].ToString();

        //            f.DataRow["备注"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[12].ToString()].ToString();
        //            f.DataRow["类型编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[23].ToString()].ToString();
        //            f.DataRow["数据编码"] = dt.Rows[i][checkedListBoxColumns.CheckedItems[24].ToString()].ToString();


        //        }
        //    }

        //    //所有行插入后，进行表更新
        //    try
        //    {
        //        fs.InvalidateVertices();
        //    }
        //    catch
        //    { }
        //    fs.UpdateExtent();
        //    fs.Save();

        //}
        #endregion

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            checkedListBoxColumns.CheckAll();
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            checkedListBoxColumns.UnCheckAll();
        }
        
        public bool tag = true;
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (tag)//zsm 2017-1-4 加标识判断是否导入成功
            {
                try
                {
                    if (string.IsNullOrEmpty(txtStorePath.Text))
                    {
                        MessageBox.Show("请选择导出文件！");
                        return;
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("没有导出的记录！");
                        return;
                    }
                    else if (checkedListBoxColumns.CheckedItemsCount == 0)
                    {
                        MessageBox.Show("请选择需要导出的列！");
                        return;
                    }




                    progressBarControl1.Properties.Maximum = dt.Rows.Count - 1;
                    progressBarControl1.Properties.Maximum = 0;

                    if (txtStorePath.Text.EndsWith("txt"))
                    {
                        FileStream fs = new FileStream(txtStorePath.Text, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        StringBuilder header = new StringBuilder();
                        for (int i = 0; i < checkedListBoxColumns.CheckedItemsCount; i++)
                        {
                            header.AppendFormat("{0}{1}", checkedListBoxColumns.CheckedItems[i].ToString(), radioGroupDivideChar.Properties.Items[radioGroupDivideChar.SelectedIndex]);
                        }
                        sw.WriteLine(header.ToString());

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i + 1, dt.Rows.Count);
                            progressBarControl1.EditValue = i;
                            StringBuilder row = new StringBuilder();
                            for (int j = 0; j < checkedListBoxColumns.CheckedItemsCount; j++)
                            {
                                row.AppendFormat("{0}{1}", dt.Rows[i][checkedListBoxColumns.CheckedItems[j].ToString()].ToString(), radioGroupDivideChar.Properties.Items[radioGroupDivideChar.SelectedIndex]);
                            }
                            sw.WriteLine(row.ToString());
                        }
                        sw.Close();
                        fs.Close();
                    }
                    else
                    {
                        //selectedQueryObj = muc3DSearcher._metadatacatalognode_Mdl;
                        //string dataname = selectedQueryObj.NAME;
                        //switch (dataname)
                        //{
                        //    case "基础矢量数据":
                        //        BasicShpDataShapeFile();
                        //        break;
                        //    case "高分系列卫星数据":
                        //        GFDataShapeFile();
                        //        break;
                        //    case"环境卫星数据":
                        //        EnvironmentDataShapeFile();
                        //        break;
                        //    case "Cbers卫星数据":
                        //        CbersDataShapeFile();
                        //        break;
                        //    case "modis卫星数据":
                        //        modisDataShapeFile();
                        //        break;
                        //    case "NOAA卫星数据":
                        //        NOAADataShapeFile();
                        //        break;
                        //    case "ZY02C":
                        //        ZY02CDataShapeFile();
                        //        break;
                        //    case "资源3号卫星数据":
                        //        Z3DataShapeFile();
                        //        break;
                        //}
                        GFDataShapeFile();
                    }

                    string name = this.btnExport.Text;
                    this.btnExport.Text = "完成";
                    tag = false;
                }
                catch (Exception ex)
                {
                    lblExportInfo.Text = "数据导出出错:" + ex.ToString();
                }
            }
            else
            {
                this.Close();
                tag = true;
            }

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //public void ExportMetadata(string filename)
        //{
        //    DataTable dt = (DataTable)gridControlDataList.DataSource;
        //    if (dt == null || dt.Rows.Count == 0)
        //    {
        //        MessageBox.Show("没有需要导出的元数据信息！");
        //        return;
        //    }
        //    else
        //    {
        //        FileStream fs = new FileStream(filename, FileMode.Create);
        //        StreamWriter sw = new StreamWriter(fs);
        //        StringBuilder header = new StringBuilder();
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            header.AppendFormat("{0}#;#", dt.Columns[i].ColumnName);
        //        }
        //        sw.WriteLine(header.ToString());
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            StringBuilder row = new StringBuilder();
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                row.AppendFormat("{0}#;#", dt.Rows[i][j].ToString());
        //            }
        //            sw.WriteLine(row.ToString());
        //        }
        //        sw.Close();
        //        fs.Close();

        //    }
        //}
    }
}