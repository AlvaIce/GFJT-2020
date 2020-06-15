using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using DotSpatial.Data;
using DotSpatial.Projections;
using QRST.WorldGlobeTool.Geometries;
using QRST.WorldGlobeTool.PluginEngine;
using QRST.WorldGlobeTool.Renderable;
using System.ComponentModel;
using QRST.WorldGlobeTool.VisualForms;

namespace QRST.WorldGlobeTool.DrawUtility
{
    public class DrawShapefileLayer : DrawBaseLayer
    {
        /// <summary>
        /// 绘制的线宽度
        /// </summary>
        protected float lineWidth = 1.0f;

        double m_maxdd = 5700000;
        /// <summary>
        /// 最高能见度
        /// </summary>
        public override double MaximumDisplayDistance
        {
            get { return m_maxdd; }
            set { m_maxdd = value; }
        }
        double m_mindd = 0;
        /// <summary>
        /// 最低能见度
        /// </summary>
        public override double MinimumDisplayDistance
        {
            get { return m_mindd; }
            set { m_mindd = value; }
        }
        ///// <summary>
        ///// 最小可视高度
        ///// </summary>
        //public double MinimumDisplayDistance = 0;
        ///// <summary>
        ///// 最大可视高度
        ///// </summary>
        //public double MaximumDisplayDistance = 5700000;
        /// <summary>
        /// 矢量线的绘制颜色，三维影像模式下
        /// </summary>
        private Color m_LineColorAtImageMode;
        /// <summary>
        /// 矢量线的绘制颜色，三维地图模式下
        /// </summary>
        private Color m_LineColorAtMapMode;
        /// <summary>
        /// 要绘制的文本的颜色，三维影像模式下
        /// </summary>
        private Color m_textColorAtImageMode;
        /// <summary>
        /// 要绘制的文本的颜色，三维地图模式下
        /// </summary>
        private Color m_textColorAtMapMode;
        /// <summary>
        /// 与每个子矢量相对应的标签文本标注
        /// </summary>
        public List<Annotation> placeNameAnnotation;
        /// <summary>
        /// 与每个子矢量相对应的顶点列表
        /// </summary>
        List<CustomVertex.PositionColored[]> listpcs;
        /// <summary>
        /// 与每个子矢量相对应的顶点列表
        /// </summary>
        List<CustomVertex.PositionColored[]> listPCS;
        /// <summary>
        /// 与每个子矢量对应的多边形列表
        /// </summary>
        private List<DrawPolygons> m_ShapePolygons;
        /// <summary>
        /// 与每个子矢量相对应的边框列表
        /// </summary>
        private List<GeographicBoundingBox> listGeoBoundingBox;
        /// <summary>
        /// 与每个子矢量相对应的边框列表
        /// </summary>
        private List<BoundingBox> listBoundingBox;
        /// <summary>
        /// 是否显示与矢量对应的标签文本
        /// </summary>
        private bool m_isShowLabel;
        /// <summary>
        /// 与矢量相对应的标签文本在矢量表中的列名称
        /// </summary>
        private string m_labelColumnName;
        /// <summary>
        /// 是否已经打开矢量文件
        /// </summary>
        private bool m_isOpenShapeFile = false;
        /// <summary>
        /// 矢量文件路径
        /// </summary>
        private string m_ShapeFilePath;

        /// <summary>
        /// 获取或设置与图层关联的矢量文件的路径
        /// </summary>
        public string ShapeFilePath
        {
            get { return m_ShapeFilePath; }
            set { m_ShapeFilePath = value; }
        }

        /// <summary>
        /// 重载获取或设置是否处在三维地图模式下的状态
        /// </summary>
        public override bool Is3DMapMode
        {
            get
            {
                return base.Is3DMapMode;
            }
            set
            {
                base.Is3DMapMode = value;
                if (placeNameAnnotation != null && placeNameAnnotation.Count > 0)
                {
                    foreach (Annotation annotation in placeNameAnnotation)
                    {
                        annotation.Is3DMapMode = value;
                    }
                }
                if (listpcs != null && listpcs.Count > 0)
                {
                    for (int i = 0; i < listpcs.Count; i++)
                    {
                        for (int j = 0; j < listpcs[i].Length; j++)
                        {
                            listpcs[i][j].Color = is3DMapMode ? m_LineColorAtMapMode.ToArgb() : m_LineColorAtImageMode.ToArgb();
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 初始化一个矢量绘制图层
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="lineColorAtImageMode"></param>
        /// <param name="linwidth"></param>
        /// <param name="pWorld"></param>
        /// <param name="drawArgs"></param>
        /// <param name="maxAltitude"></param>
        /// <param name="minAltitude"></param>
        /// <param name="isShowLabel"></param>
        /// <param name="labelColumnName"></param>
        /// <param name="textColorAtImageMode"></param>
        public DrawShapefileLayer(string pName, Color lineColorAtImageMode, Color lineColorAtMapMode, float linwidth,
            World pWorld, DrawArgs drawArgs, double maxAltitude, double minAltitude,
            bool isShowLabel, string labelColumnName, Color textColorAtImageMode, Color textColorAtMapMode)
            : base(pName, lineColorAtImageMode, pWorld, drawArgs)
        {
            lineWidth = linwidth;
            MinimumDisplayDistance = minAltitude;
            MaximumDisplayDistance = maxAltitude;
            m_LineColorAtImageMode = lineColorAtImageMode;
            m_LineColorAtMapMode = lineColorAtMapMode;
            m_isShowLabel = isShowLabel;
            m_labelColumnName = labelColumnName;
            m_textColorAtImageMode = textColorAtImageMode;
            m_textColorAtMapMode = textColorAtMapMode;
        }

        /// <summary>
        /// 初始化一个矢量绘制图层
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pColor"></param>
        /// <param name="drawTool"></param>
        /// <param name="drawArgs"></param>
        public DrawShapefileLayer(string pName, Color pColor, Plugin drawTool, DrawArgs drawArgs)
            : base(pName, pColor, drawTool, drawArgs)
        {
            m_LineColorAtImageMode = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }

        public DrawShapefileLayer(string pName, Color pColor, float linwidth, World pWorld, DrawArgs drawArgs, double maxAltitude, double minAltitude)
            : base(pName, pColor, pWorld, drawArgs)
        {
            lineWidth = linwidth;
            MinimumDisplayDistance = minAltitude;
            MaximumDisplayDistance = maxAltitude;
            m_LineColorAtImageMode = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }
        /// <summary>
        /// 打开矢量文件
        /// </summary>
        /// <param name="shpfile"></param>
        public void OpenShapefile(string shpfile)
        {
            if (System.IO.Path.GetExtension(shpfile).ToLower() != ".shp")
                return;
            m_ShapeFilePath = shpfile;
            //启动后台进程进行矢量文件的打开初始化
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }

        #region 暂时无用的方法

        public void UpdateExtents(List<RectangleF> rects)
        {
            listpcs.Clear();
            foreach (RectangleF rect in rects)
            {
                Point3d pt0 = CreatePoint3dByLatLon(rect.Y, rect.X);
                Point3d pt1 = CreatePoint3dByLatLon(rect.Y, rect.Right);
                Point3d pt2 = CreatePoint3dByLatLon(rect.Bottom, rect.Right);
                Point3d pt3 = CreatePoint3dByLatLon(rect.Bottom, rect.X);


                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, m_LineColorAtImageMode);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, m_LineColorAtImageMode);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, m_LineColorAtImageMode);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, m_LineColorAtImageMode);

                CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);

                listpcs.Add(ppcs);
            }

        }

        private Point3d CreatePoint3dByLatLon(double lat, double lon)
        {
            Point3d pt3d = new Point3d();
            pt3d.X = lon;
            pt3d.Y = lat;
            pt3d.Z = (useTerrain) ? getElevation(lat, lon) : defaultElev;

            return pt3d;
        }


        private Color getColorbyCount(int p, int max)
        {
            //000000255 000128128 1281280000 255000000

            int blue = 255;
            int green = 0;
            int red = 0;
            int all = (int)(383 * ((float)p / (float)max));
            for (int i = 0; i < all; i++)
            {
                if (blue > green)
                {
                    green = green + 1;
                    blue = blue - 1;
                }
                else if (blue > 0)
                {
                    red = red + 1;
                    blue = blue - 1;
                }
                else
                {
                    green = green - 1;
                    red = red + 1;
                }

            }
            return Color.FromArgb(255, red, green, blue);
        }

        private Point3d CreatePoint3dByLatLon(double lat, double lon, double altoff)
        {
            Point3d pt3d = new Point3d();
            pt3d.X = lon;
            pt3d.Y = lat;
            pt3d.Z = ((useTerrain) ? getElevation(lat, lon) : defaultElev) + altoff;

            return pt3d;
        }

        #endregion

        #region base Override
        public override void Initialize(DrawArgs drawArgs)
        {
            base.Initialize(drawArgs);
            OpenShapefile(m_ShapeFilePath);
            IsInitialized = true;
        }

        public override void BuildContextMenu(System.Windows.Forms.ContextMenu menu)
        {
            base.BuildContextMenu(menu);
            menu.MenuItems.Add(m_isShowLabel ? "隐藏标签" : "显示标签", new EventHandler(OnLabelVisiableChanged));
            menu.MenuItems.Add("属性表", new EventHandler(ShowShapeInfo));
        }

        private void ShowShapeInfo(object sender, EventArgs e)
        {
            ShapeFileInfoDlg sfid = new ShapeFileInfoDlg(m_ShapeFilePath.Replace(".shp", ".dbf"), false);
            sfid.Show();
        }

        private void OnLabelVisiableChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem menuItem = sender as System.Windows.Forms.MenuItem;
            m_isShowLabel = !m_isShowLabel;
            menuItem.Text = m_isShowLabel ? "隐藏标签" : "显示标签";
            if (m_labelColumnName != "" && m_isShowLabel == true && placeNameAnnotation.Count == 0)
            {
                readPlaceNameAnnotation();
            }
        }

        public override void Update(DrawArgs drawArgs)
        {
            if (!this.IsInitialized)
                this.Initialize(drawArgs);
        }

        public override void Dispose()
        {
            IsInitialized = false;
            base.Dispose();
        }

        public override bool IsOn
        {
            get
            {
                return base.IsOn;
            }
            set
            {
                base.IsOn = value;
                //控制栅格化Polygon的显示 ProjectedVectorRenderer中
                if (State == DrawState.Complete)
                {
                    if (isOn)
                    {
                        this.Render(this.drawArgs);
                    }
                }
            }
        }

        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            return false;
        }

        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            if (!(drawArgs.WorldCamera.AltitudeAboveTerrain > MinimumDisplayDistance && drawArgs.WorldCamera.AltitudeAboveTerrain < MaximumDisplayDistance))
                return;

            if (!m_isOpenShapeFile)
                return;


            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = false;

            // Check that textures are initialised
            if (!IsInitialized)
                Initialize(drawArgs);

            if (listBoundingBox == null)
            {
                listBoundingBox = new List<BoundingBox>();
                foreach (GeographicBoundingBox gbb in listGeoBoundingBox)
                {
                    listBoundingBox.Add(new BoundingBox(
                        (float)gbb.South, (float)gbb.North, (float)gbb.West, (float)gbb.East,
                        (float)drawArgs.CurrentWorld.EquatorialRadius,
                        (float)drawArgs.CurrentWorld.EquatorialRadius));
                }
                //listpcs为另外一个线程定义，IsOn为false之后，listpcs就被释放掉了因此要拷贝到主线程中
                listPCS = new List<CustomVertex.PositionColored[]>();
                foreach (CustomVertex.PositionColored[] pcs in listpcs)
                {
                    CustomVertex.PositionColored[] mpcs=new CustomVertex.PositionColored[pcs.Length];
                    pcs.CopyTo(mpcs,0);
                    listPCS.Add(mpcs);
                }
                
                //listpcs.CopyTo(listPCS);
            }

            Device device = drawArgs.device;
            device.RenderState.ZBufferEnable = false;
            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.VertexFormat = CustomVertex.PositionColored.Format;


            Vector3 rc = new Vector3(
                (float)drawArgs.WorldCamera.ReferenceCenter.X,
                (float)drawArgs.WorldCamera.ReferenceCenter.Y,
                (float)drawArgs.WorldCamera.ReferenceCenter.Z
                );

            drawArgs.device.Transform.World = Matrix.Translation(-rc);

            try
            {
                //绘制边界曲线
                for (int i = 0; i < listPCS.Count; i++)
                {
                    //判断多边形的外边框是否与当前照相机的视域相交
                    if (drawArgs.WorldCamera.ViewFrustum.Intersects(listBoundingBox[i]))
                    {
                        drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, listPCS[i].Length - 1, listPCS[i]);
                    }
                }
            }
            catch
            { }

            //绘制矢量标注
            if (m_isShowLabel)
            {
                foreach (Annotation annotation in placeNameAnnotation)
                {
                    annotation.Render(drawArgs);
                }
            }


            drawArgs.device.Transform.World = drawArgs.WorldCamera.WorldMatrix;

            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = true;
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

        }

        public override void MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;
        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 后台进程结束，设置打开矢量文件成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_isOpenShapeFile = true;
        }

        /// <summary>
        /// 后台进程执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            initShapeFile();
        }

        /// <summary>
        /// 初始化矢量文件
        /// </summary>
        private void initShapeFile()
        {
            //初始化列表
            listpcs = new List<CustomVertex.PositionColored[]>();
            m_ShapePolygons = new List<DrawPolygons>();
            placeNameAnnotation = new List<Annotation>();
            listGeoBoundingBox = new List<GeographicBoundingBox>();

            //打开矢量文件
            Shapefile _sf = Shapefile.OpenFile(m_ShapeFilePath);
            //投影转换
            ProjectionInfo piWGS1984 = KnownCoordinateSystems.Geographic.World.WGS1984;
            if (_sf.Projection != piWGS1984 && _sf.CanReproject)
                _sf.Reproject(piWGS1984);//如果投影不是WGS1984投影，则进行投影变换
            //元数据信息填充
            this._metaData["north"] = _sf.Extent.MaxY;
            this._metaData["south"] = _sf.Extent.MinY;
            this._metaData["west"] = _sf.Extent.MinX;
            this._metaData["east"] = _sf.Extent.MaxX;

            #region 读取矢量文件中的子矢量内容

            foreach (IFeature ft in _sf.Features)
            {
                Shape shape = ft.ToShape();
                List<double[]> verticess = new List<double[]>();
                if (shape.Range.Parts == null)
                {
                    verticess.Add(shape.Vertices);
                }
                else
                {
                    foreach (PartRange pr in shape.Range.Parts)
                    {
                        verticess.Add(pr.Vertices);
                    }
                }

                #region 读取与矢量相关的标签

                if (m_isShowLabel)
                {
                    string shapeName = ft.DataRow[m_labelColumnName].ToString();
                    double centerLon = 0, centerLat = 0;
                    long count = 0;
                    foreach (double[] vts in verticess)
                    {
                        count += vts.Length / 2;
                        for (int i = 0; i < vts.Length; i += 2)
                        {
                            centerLon += vts[i];
                            centerLat += vts[i + 1];
                        }
                    }
                    centerLon /= count;
                    centerLat /= count;
                    Annotation annotation = new Annotation(shapeName, centerLat, centerLon,
                        m_textColorAtImageMode, m_textColorAtMapMode, MaximumDisplayDistance, MinimumDisplayDistance);
                    annotation.IsOn = true;
                    placeNameAnnotation.Add(annotation);
                }

                #endregion

                int lastPolygonFirstPointIndex = 0;
                foreach (double[] vts in verticess)
                {
                    List<Point3d> p3ds = new List<Point3d>();
                    List<CustomVertex.PositionColored> ppcs = new List<CustomVertex.PositionColored>();
                    p3ds.Add(CreatePoint3dByLatLon(vts[lastPolygonFirstPointIndex + 1], vts[lastPolygonFirstPointIndex]));

                    for (int i = lastPolygonFirstPointIndex + 2; i < vts.Length; i += 2)
                    {
                        p3ds.Add(CreatePoint3dByLatLon(vts[i + 1], vts[i]));
                        if (vts[i + 1] == vts[lastPolygonFirstPointIndex + 1] && vts[i] == vts[lastPolygonFirstPointIndex])
                        {
                            lastPolygonFirstPointIndex = i + 2;
                            break;
                        }
                    }

                    if (p3ds.Count > 1)
                    {
                        for (int i = 0; i < p3ds.Count - 1; i++)
                        {
                            List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(p3ds[i], p3ds[i + 1], is3DMapMode ? m_LineColorAtMapMode : m_LineColorAtImageMode);
                            ppcs.AddRange(curve1.ToArray());
                        }
                        //m_ShapePolygons.Add(CreatePolygon(p3ds));  按多边形方式绘制速度比较慢，和多边形绘制的方式有关系
                    }
                    listpcs.Add(ppcs.ToArray());
                    listGeoBoundingBox.Add(new GeographicBoundingBox(ft.Envelope.Maximum.Y, ft.Envelope.Minimum.Y, ft.Envelope.Minimum.X, ft.Envelope.Maximum.X));
                }
            }

            #endregion
        }

        /// <summary>
        /// 初始化矢量文件
        /// </summary>
        private void readPlaceNameAnnotation()
        {
            //初始化列表
            placeNameAnnotation = new List<Annotation>();

            //打开矢量文件
            Shapefile _sf = Shapefile.OpenFile(m_ShapeFilePath);
            //投影转换
            ProjectionInfo piWGS1984 = KnownCoordinateSystems.Geographic.World.WGS1984;
            if (_sf.Projection != piWGS1984 && _sf.CanReproject)
                _sf.Reproject(piWGS1984);//如果投影不是WGS1984投影，则进行投影变换

            #region 读取矢量文件中的子矢量内容

            foreach (IFeature ft in _sf.Features)
            {
                Shape shape = ft.ToShape();
                List<double[]> verticess = new List<double[]>();
                if (shape.Range.Parts == null)
                {
                    verticess.Add(shape.Vertices);
                }
                else
                {
                    foreach (PartRange pr in shape.Range.Parts)
                    {
                        verticess.Add(pr.Vertices);
                    }
                }

                if (m_isShowLabel)
                {
                    string shapeName = ft.DataRow[m_labelColumnName].ToString();
                    double centerLon = 0, centerLat = 0;
                    long count = 0;
                    foreach (double[] vts in verticess)
                    {
                        count += vts.Length / 2;
                        for (int i = 0; i < vts.Length; i += 2)
                        {
                            centerLon += vts[i];
                            centerLat += vts[i + 1];
                        }
                    }
                    centerLon /= count;
                    centerLat /= count;
                    Annotation annotation = new Annotation(shapeName, centerLat, centerLon,
                        m_textColorAtImageMode, m_textColorAtMapMode, MaximumDisplayDistance, MinimumDisplayDistance);
                    annotation.IsOn = true;
                    placeNameAnnotation.Add(annotation);
                }
            }
            #endregion
        }


        /// <summary>
        /// 创建多边形
        /// </summary>
        /// <param name="pts">顶点列表</param>
        protected DrawPolygons CreatePolygon(List<Point3d> pts)
        {
            LinearRing lr = new LinearRing();
            lr.Points = pts.ToArray();
            Polygon polygon = new Polygon();
            polygon.outerBoundary = lr;
            polygon.PolgonColor = is3DMapMode ? Color.FromArgb(20, m_LineColorAtMapMode) : Color.FromArgb(20, m_LineColorAtImageMode);
            polygon.Fill = true;
            polygon.ParentRenderable = this;
            polygon.Outline = true;
            polygon.OutlineColor = is3DMapMode ? m_LineColorAtMapMode : m_LineColorAtImageMode;
            polygon.LineWidth = lineWidth;
            return new DrawPolygons(polygon, World);
        }

        #endregion

    }
}
