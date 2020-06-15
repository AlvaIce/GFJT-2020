using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Topology;
using DotSpatial.Projections;
using QRST_DI_TS_Basis.DirectlyAddress;
 
namespace QRST_DI_TS_Basis.Search
{
    public class GridGeneration
    {
        /// <summary>
        /// 将指定区域生成格网，目前区域只能唯一。如不唯一需要合并
        /// </summary>
        /// <param name="ife">指定区域</param>
        /// <param name="baseGridDir">底图目录</param>
        public GridGeneration(string level)
        {
            TileLevel = level;
        }
        List<Envelope> EnvList = new List<Envelope>();//目标格网
        List<string[]> AOITile = new List<string[]>();//目标瓦片
        private string TileLevel = "";

        private void GetAOITilesCR(IFeatureSet fs)//进度条问题，再考虑
        {
            foreach (IFeature feature in fs.Features)
            {
                //DateTime beforDT = System.DateTime.Now;
                //得到外接矩形包含的行列号
                string[] feaEnvelope = new string[] { 
                    feature.Envelope.Minimum.Y.ToString(),
                    feature.Envelope.Minimum.X.ToString() , 
                    feature.Envelope.Maximum.Y.ToString(), 
                    feature.Envelope.Maximum.X.ToString() };
                int[] colRow = DirectlyAddressing.GetRowAndColum(feaEnvelope, TileLevel);//最小行，最小列，最大行，最大列
               
                TilesFilter(feature, colRow);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="colRow">最小行，最小列，最大行，最大列</param>
        private void TilesFilter(IFeature feature, int[] colRow)
        {

            #region 原方法，可行的，但逻辑不直观
            /*
                int rownum = colRow[2] - colRow[0] + 1;
                int colnum = colRow[3] - colRow[1] + 1;
                //获得六参数：起始纬度、纬度分辨率、纬度偏移量、起始经度、经度偏移量、经度分辨率
                double[] GT = new double[6];
                double resolution = double.Parse(DirectlyAddressing.GetDegreeByStrLv(TileLevel));
                GT[1] = GT[5] = resolution;
                string[] minTileRC = { colRow[0].ToString(), colRow[1].ToString() };
                double[] minTileLB = DirectlyAddressing.GetLatAndLong(minTileRC, TileLevel);//最小纬度，最小经度，最大纬度，最大经度
                GT[0] = minTileLB[0];//最小纬度 大范围（总格网）起始纬度
                GT[3] = minTileLB[1];//最小经度 大范围（总格网）起始纬度
                GT[2] = GT[4] = 0;
                //Thread thdSub = new Thread(showProgressbar);
                //frmb.ShowDialog();
                //int m = 0;
                //thdSub.Start();
                //frmb.prcBar.Maximum = colnum * rownum;
                //判断多边形和瓦片矩形是否相交，相交即为区域瓦片 注意point（x，y）x为精度，y为纬度
                for (int i = 0; i < rownum; i++)
                    for (int j = 0; j < colnum; j++)
                    {
                        try
                        {
                            //frmb.prcBar.Value = m;
                            //m++;
                            //TaskRun(m);
                            List<double[]> Tile4Geolist = TileGeoTrans(i, j, GT);       //
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
                            throw e;
                            break;
                        }
                    }
                 */
            #endregion

            #region 替换方法
            int rownum = colRow[2] - colRow[0] + 1;
            int colnum = colRow[3] - colRow[1] + 1;
           
            double lvrate=DirectlyAddressing.getLevelRate(TileLevel);
            //判断多边形和瓦片矩形是否相交，相交即为区域瓦片 注意point（x，y）x为精度，y为纬度
            for (int i = 0; i < rownum; i++)
                for (int j = 0; j < colnum; j++)
                {
                    try
                    {
                        //frmb.prcBar.Value = m;
                        //m++;
                        //TaskRun(m);
                        double[] tileMMLL = DirectlyAddressing.GetLatAndLong(colRow[0] + i, colRow[1] + j, lvrate);
                        Coordinate coord1 = new Coordinate(tileMMLL[1], tileMMLL[0]);
                        Coordinate coord2 = new Coordinate(tileMMLL[3], tileMMLL[2]);
                        Envelope enve = new Envelope(coord1, coord2);
                        bool overlap = feature.Intersects(enve);
                        if (overlap)
                        {
                            string[] tile = new string[2];
                            tile[0] = (i + colRow[0]).ToString();
                            tile[1] = (j + colRow[1]).ToString();
                            AOITile.Add(tile);
                            EnvList.Add(enve);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        break;
                    }
                }
            #endregion
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="colRow">最小行，最小列，最大行，最大列</param>
        public static void TilesFilter(string coordsStr, string lvstr, List<double> rows, List<double> cols, out List<double> rowsFilted, out List<double> colsFilted)
        {
            IList<Coordinate> coords = GetCoordsFormStr(coordsStr);
            IFeature feature = GetFeatureFromCoords(coords);
            TilesFilter(feature, lvstr, rows, cols, out rowsFilted, out colsFilted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="colRow">最小行，最小列，最大行，最大列</param>
        public static void TilesFilter(IFeature feature, string lvstr,List<double> rows, List<double> cols, out List<double> rowsFilted, out List<double> colsFilted)
        {
            rowsFilted = new List<double>();
            colsFilted = new List<double>();
            if (rows==null||cols==null)
                return;
            if (rows.Count != cols.Count)
                return;

            #region 替换方法 原方法参考 TilesFilter(IFeature feature, int[] colRow)

            double lvrate = DirectlyAddressing.getLevelRate(lvstr);
            //判断多边形和瓦片矩形是否相交，相交即为区域瓦片 注意point（x，y）x为精度，y为纬度
            for (int i = 0; i < rows.Count; i++)
                for (int j = 0; j < cols.Count; j++)
                {
                    try
                    {
                        //frmb.prcBar.Value = m;
                        //m++;
                        //TaskRun(m);
                        double[] tileMMLL = DirectlyAddressing.GetLatAndLong(rows[i], cols[j], lvrate);
                        Coordinate coord1 = new Coordinate(tileMMLL[1], tileMMLL[0]);
                        Coordinate coord2 = new Coordinate(tileMMLL[3], tileMMLL[2]);
                        Envelope enve = new Envelope(coord1, coord2);
                        bool overlap = feature.Intersects(enve);
                        if (overlap)
                        {
                            rowsFilted.Add(rows[i]);
                            colsFilted.Add(cols[j]);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        break;
                    }
                }
            #endregion
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="colRow">最小行，最小列，最大行，最大列</param>
        public static void TilesFilter(IFeature feature, System.Data.DataSet tileds)
        {
            //ds 是否包含rcl
            if (tileds != null && tileds.Tables.Count > 0 && tileds.Tables[0].Rows.Count > 0)
            {
                if (!(tileds.Tables[0].Columns.Contains("Row") && tileds.Tables[0].Columns.Contains("Col") && tileds.Tables[0].Columns.Contains("Level")))
                {
                    return;
                }
            }
            else
            {
                return;
            }


            #region 替换方法 原方法参考 TilesFilter(IFeature feature, int[] colRow)

            //判断多边形和瓦片矩形是否相交，相交即为区域瓦片 注意point（x，y）x为精度，y为纬度
            for (int i = tileds.Tables[0].Rows.Count - 1; i > -1; i--)
            {
                try
                {
                    double lvrate = DirectlyAddressing.getLevelRate(tileds.Tables[0].Rows[i]["Level"].ToString());
                    double row = Convert.ToDouble(tileds.Tables[0].Rows[i]["Row"]);
                    double col = Convert.ToDouble(tileds.Tables[0].Rows[i]["Col"]);
                    double[] tileMMLL = DirectlyAddressing.GetLatAndLong(row, col, lvrate);
                    Coordinate coord1 = new Coordinate(tileMMLL[1], tileMMLL[0]);
                    Coordinate coord2 = new Coordinate(tileMMLL[3], tileMMLL[2]);
                    Envelope enve = new Envelope(coord1, coord2);
                    bool overlap = feature.Intersects(enve);
                    if (!overlap)
                    {
                        tileds.Tables[0].Rows.RemoveAt(i);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    break;
                }
            }
            #endregion
        }

        /// <summary>
        /// 计算指定格网的经纬度坐标
        /// 按大范围（总格网）起始经纬度、分辨率、当前格网相对行列序号（起始为0）计算
        /// </summary>
        /// <param name="a">指定格网的相对行号</param>
        /// <param name="b">指定格网的相对列号</param>
        /// <param name="gt">起始纬度、纬度分辨率、纬度偏移量、起始经度、经度偏移量、经度分辨率</param>
        /// <returns></returns>
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

        /// <summary>
        /// 将输入的string类型点串参数查找最大最小经纬度
        /// </summary>
        /// <param name="coordsStr">120.18,30.11;121.22,31.87;122.88,30.99;</param>
        /// <returns>最小纬度、最小经度、最大纬度、最大经度</returns>
        public static double[] GetMinMaxLatLonFormCoordsStr(string coordsStr)
        {
            double minlat, minlon, maxlat, maxlon;
            minlon = minlat = double.MaxValue;
            maxlon = maxlat = double.MinValue;

            string[] ptlist = coordsStr.Split(';');
            foreach (string ptstr in ptlist)
            {
                if (ptstr.Trim().Length > 0)
                {
                    string[] coordstr = ptstr.Split(',');

                    try
                    {
                        double lon = double.Parse(coordstr[0]);
                        double lat = double.Parse(coordstr[1]);

                        maxlon = (lon > maxlon) ? lon : maxlon;
                        maxlat = (lat > maxlat) ? lat : maxlat;
                        minlon = (lon < minlon) ? lon : minlon;
                        minlat = (lat < minlat) ? lat : minlat;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return new double[] { minlat, minlon, maxlat, maxlon };
        }

        /// <summary>
        /// 将输入的string类型点串参数转换成Coordinate列表
        /// 输入为多边形点串，最后点可以不为首点，程序内部会自动加点闭合，例如
        /// 120.18,30,11;121.22,31.87;122.88,30.99;
        /// </summary>
        /// <param name="pts">120.18,30.11;121.22,31.87;122.88,30.99;</param>
        /// <returns></returns>
        public static IList<Coordinate> GetCoordsFormStr(string pts)
        {
            //zsm 161018
            IList<Coordinate> coords = new List<Coordinate>();
            string[] ptlist = pts.Split(';');
            foreach (string ptstr in ptlist)
            {
                if (ptstr.Trim().Length > 0)
                {
                    string[] coordstr = ptstr.Split(',');
                    Coordinate coord = null;
                    try
                    {
                        coord = new Coordinate(double.Parse(coordstr[0]), double.Parse(coordstr[1]));
                    }
                    catch (Exception)
                    {
                        coord = null;
                    }
                    if (coord != null)
                    {
                        coords.Add(coord);
                    }
                }
            }

            if (coords.Count > 0 && (coords[0].X != coords[coords.Count - 1].X || coords[0].Y != coords[coords.Count - 1].Y))
            {
                //判断多边形是否闭合，即首尾相等
                coords.Add(coords[0]);  //执行闭合
            }

            return coords;
        }

        public static IFeature GetFeatureFromCoords(IList<Coordinate> coords, bool usingSimplify = true)
        {
            if (coords == null)
            {
                return null;
            }

            //坐标串简化处理
            IList<Coordinate> coords_simp = coords;
            if (usingSimplify && coords.Count > 20)
            {
                //坐标串简化处理
                coords_simp = ShapeSimplifier.Simplifier(coords);
            }
            //建立多边形Feature
            IFeature ft = new Feature(FeatureType.Polygon, coords_simp);

            return ft;
        }

        public static IFeatureSet GetFeatureSetFromCoords(IList<Coordinate> coords, bool usingSimplify = true)
        {
            if (coords == null)
            {
                return null;
            }

            IFeatureSet fs = new FeatureSet(FeatureType.Polygon);
            fs.Features.Add(GetFeatureFromCoords(coords, usingSimplify));

            return fs;
        }

        /// <summary>
        /// 根据多边形点串生成指定层级的格网行列号
        /// </summary>
        /// <param name="coordsStr">输入为多边形点串，最后点可以不为首点，程序内部会自动加点闭合，例如120.18,30,11;121.22,31.87;122.88,30.99;</param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void GetAOITilesGrid(string coordsStr, out List<string> rows, out List<string> cols)
        {
            IList<Coordinate> coords = GetCoordsFormStr(coordsStr);
            IFeatureSet fs = GetFeatureSetFromCoords(coords);
            GetAOITilesGrid(fs, out rows, out cols);
        }

        /// <summary>
        /// 根据多边形对象生成指定层级的格网行列号，支持多个ft、多个part
        /// </summary>
        /// <param name="fs">可以从shp里获取，支持多个ft、多个part</param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void GetAOITilesGrid(IFeatureSet fs, out List<string> rows, out List<string> cols)
        {
            this.GetAOITilesCR(fs);
            rows = new List<string>();
            cols = new List<string>();


            for (int i = 0; i < AOITile.Count; i++)
            {
                rows.Add(AOITile[i][0]);
                cols.Add(AOITile[i][1]);
            }
        }

        //生成格网Shp文件
        /// <summary>
        ///  根据多边形对象生成指定层级的格网行列号，以Shapefile形式输出，支持多个ft、多个part
        /// </summary>
        /// <param name="fs">可以从shp里获取，支持多个ft、多个part</param>
        /// <param name="gridShpPath">输出的Shp路径</param>
        /// <returns>输出的格网FS</returns>
        public IFeatureSet GetAOITilesGrid(IFeatureSet fs, string gridShpPath)
        {
            this.GetAOITilesCR(fs);
            IFeatureSet _ifeatureSet = CreatShpFile(gridShpPath);
            if (_ifeatureSet != null && !_ifeatureSet.IsDisposed)
            {
                for (int i = 0; i < AOITile.Count; i++)
                {
                    IPolygon ipoly = EnvList[i].ToPolygon();
                    Feature f = new Feature(ipoly);
                    _ifeatureSet.Features.Add(f);
                    if (f != null)
                    {
                        if (f.DataRow.Table.Columns.Contains("Level"))//表示tatatable中一行数据
                        {
                            f.DataRow["Level"] = TileLevel;
                        }
                        if (f.DataRow.Table.Columns.Contains("Row"))
                        {
                            f.DataRow["Row"] = AOITile[i][0];
                        }
                        if (f.DataRow.Table.Columns.Contains("Column"))
                        {
                            f.DataRow["Column"] = AOITile[i][1];
                        }
                        if (f.DataRow.Table.Columns.Contains("West"))
                        {
                            f.DataRow["West"] = EnvList[i].X;//f为Feature类//外接矩形xy为左下坐标时
                        }
                        if (f.DataRow.Table.Columns.Contains("North"))
                        {
                            f.DataRow["North"] = EnvList[i].Y;
                        }
                        if (f.DataRow.Table.Columns.Contains("East"))
                        {
                            f.DataRow["East"] = EnvList[i].Width + EnvList[i].X;
                        }
                        if (f.DataRow.Table.Columns.Contains("South"))
                        {
                            f.DataRow["South"] = EnvList[i].Y - EnvList[i].Height;
                        }
                    }
                }
                _ifeatureSet.UpdateExtent();
                _ifeatureSet.Save();
            }
            return _ifeatureSet;
        }

        private IFeatureSet CreatShpFile(string fsPath)
        {
            //新建featureset，标明名各项属性，加入feature保存
            FeatureSet fs = new FeatureSet(FeatureType.Polygon);
            fs.Projection = ProjectionInfo.FromEpsgCode(4326);
            fs.CoordinateType = CoordinateType.Regular;
            fs.DataTable.Columns.Add("Level", typeof(string));
            fs.DataTable.Columns.Add("Row", typeof(double));
            fs.DataTable.Columns.Add("Column", typeof(double));
            fs.DataTable.Columns.Add("West", typeof(double));
            fs.DataTable.Columns.Add("North", typeof(double));
            fs.DataTable.Columns.Add("East", typeof(double));
            fs.DataTable.Columns.Add("South", typeof(double));
            fs.IndexMode = false;
            fs.FilePath = fsPath;
            fs.Save();
            IFeatureSet obj = DataManager.DefaultDataManager.OpenVector(fs.FilePath, true, null);
            return obj;
        }
    }
}
