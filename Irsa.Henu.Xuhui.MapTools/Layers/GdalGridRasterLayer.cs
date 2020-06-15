using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OSGeo.GDAL;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using SharpMap.Data;
using SharpMap.Geometries;
using SharpMap.Rendering.Thematics;
using Point = System.Drawing.Point;

namespace SharpMap.Layers
{
    public class GdalGridRasterLayer : Layer, ICanQueryLayer, IDisposable
    {
        #region 私有变量
        private int _bitDepth = 8;//图像的bit率
        private bool _colorCorrect = true; // 是否进行颜色纠正
        private List<int[]> _curveLut;
        private bool _displayCIR;
        private bool _displayIR;
        private double[] _gain = { 1, 1, 1, 1 };//增益
        private double _gamma = 1;
        private bool _haveSpot; // spot correction
        private List<int[]> _nonSpotCurveLut;
        private double[] _nonSpotGain = { 1, 1, 1, 1 };
        private double _nonSpotGamma = 1;
        private PointF _spot = new PointF(0, 0);
        private List<int[]> _spotCurveLut;
        private double[] _spotGain = { 1, 1, 1, 1 };
        private double _spotGamma = 1;

        private Rectangle _histoBounds;
        private List<int[]> _histogram; //直方图

        private double _innerSpotRadius;
        // outer radius is feather between inner radius and rest of image
        private double _outerSpotRadius;
        // outer radius is feather between inner radius and rest of image

        protected BoundingBox _envelope;//影像范围
        protected Dataset _gdalDataset;//影像数据集合
        internal GeoTransform _geoTransform;//图像的空间信息对象
        protected Size _imagesize;//请求图像的大小
        internal int _lbands;//影像波段数

        private string _projectionWkt = "";//影像投影字符串
        private bool _showClip;
        private Point _stretchPoint;//图像拉伸时的最大最小值信息
        protected ICoordinateTransformation _transform;
        private Color _transparentColor = Color.Empty; // 透明色
        protected bool _useRotation = true; //是否旋转

        #endregion

        #region 属性

        private string _Filename;

        /// <summary>
        /// 获取或设置图层名
        /// </summary>
        public string Filename
        {
            get { return _Filename; }
            set { _Filename = value; }
        }

        /// <summary>
        /// 获取或设置图层渲染的图片的Bit率
        /// </summary>
        public int BitDepth
        {
            get { return _bitDepth; }
            set { _bitDepth = value; }
        }

        /// <summary>
        /// 获取或设置影像的投影字符串
        /// </summary>
        public string Projection
        {
            get { return _projectionWkt; }
            set { _projectionWkt = value; }
        }

        /// <summary>
        /// 获取或设置是否显示红外波段
        /// </summary>
        public bool DisplayIR
        {
            get { return _displayIR; }
            set { _displayIR = value; }
        }

        /// <summary>
        /// 获取或设置是否显示彩虹外
        /// </summary>
        public bool DisplayCIR
        {
            get { return _displayCIR; }
            set { _displayCIR = value; }
        }

        /// <summary>
        /// Gets or sets to display clip
        /// </summary>
        public bool ShowClip
        {
            get { return _showClip; }
            set { _showClip = value; }
        }

        /// <summary>
        /// Gets or sets to display gamma
        /// </summary>
        public double Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }

        /// <summary>
        /// Gets or sets to display gamma for Spot spot
        /// </summary>
        public double SpotGamma
        {
            get { return _spotGamma; }
            set { _spotGamma = value; }
        }

        /// <summary>
        /// Gets or sets to display gamma for NonSpot
        /// </summary>
        public double NonSpotGamma
        {
            get { return _nonSpotGamma; }
            set { _nonSpotGamma = value; }
        }

        /// <summary>
        /// Gets or sets to display red Gain
        /// </summary>
        public double[] Gain
        {
            get { return _gain; }
            set { _gain = value; }
        }

        /// <summary>
        /// Gets or sets to display red Gain for Spot spot
        /// </summary>
        public double[] SpotGain
        {
            get { return _spotGain; }
            set { _spotGain = value; }
        }

        /// <summary>
        /// Gets or sets to display red Gain for Spot spot
        /// </summary>
        public double[] NonSpotGain
        {
            get { return _nonSpotGain; }
            set { _nonSpotGain = value; }
        }

        /// <summary>
        /// Gets or sets to display curve lut
        /// </summary>
        public List<int[]> CurveLut
        {
            get { return _curveLut; }
            set { _curveLut = value; }
        }

        /// <summary>
        /// Correct Spot spot
        /// </summary>
        public bool HaveSpot
        {
            get { return _haveSpot; }
            set { _haveSpot = value; }
        }

        /// <summary>
        /// Gets or sets to display curve lut for Spot spot
        /// </summary>
        public List<int[]> SpotCurveLut
        {
            get { return _spotCurveLut; }
            set { _spotCurveLut = value; }
        }

        /// <summary>
        /// Gets or sets to display curve lut for NonSpot
        /// </summary>
        public List<int[]> NonSpotCurveLut
        {
            get { return _nonSpotCurveLut; }
            set { _nonSpotCurveLut = value; }
        }

        /// <summary>
        /// Gets or sets the center point of the Spot spot
        /// </summary>
        public PointF SpotPoint
        {
            get { return _spot; }
            set { _spot = value; }
        }

        /// <summary>
        /// Gets or sets the inner radius for the spot
        /// </summary>
        public double InnerSpotRadius
        {
            get { return _innerSpotRadius; }
            set { _innerSpotRadius = value; }
        }

        /// <summary>
        /// Gets or sets the outer radius for the spot (feather zone)
        /// </summary>
        public double OuterSpotRadius
        {
            get { return _outerSpotRadius; }
            set { _outerSpotRadius = value; }
        }

        /// <summary>
        /// 获取影像的直方图信息
        /// </summary>
        public List<int[]> Histogram
        {
            get { return _histogram; }
        }


        /// <summary>
        /// 获取影像的波段数
        /// </summary>
        public int Bands
        {
            get { return _lbands; }
        }

        /// <summary>
        /// 获取水平像素分辨率
        /// </summary>
        public double GSD
        {
            get { return _geoTransform.HorizontalPixelResolution; }
        }

        ///<summary>
        /// 获取或设置是否允许旋转
        /// </summary>
        public bool UseRotation
        {
            get { return _useRotation; }
            set
            {
                _useRotation = value;
                _envelope = GetExtent();
            }
        }
        /// <summary>
        /// 获取影像的大小
        /// </summary>
        public Size Size
        {
            get { return _imagesize; }
        }
        /// <summary>
        /// 获取或设置是否进行颜色纠正
        /// </summary>
        public bool ColorCorrect
        {
            get { return _colorCorrect; }
            set { _colorCorrect = value; }
        }
        /// <summary>
        /// 获取或设置直方图的范围
        /// </summary>
        public Rectangle HistoBounds
        {
            get { return _histoBounds; }
            set { _histoBounds = value; }
        }
        /// <summary>
        /// 获取投影转置矩阵
        /// </summary>
        public ICoordinateTransformation Transform
        {
            get { return _transform; }
        }
        /// <summary>
        /// 获取或设置透明色
        /// </summary>
        public Color TransparentColor
        {
            get { return _transparentColor; }
            set { _transparentColor = value; }
        }
        /// <summary>
        /// 获取或设置拉伸点
        /// </summary>
        public Point StretchPoint
        {
            get
            {
                if (_stretchPoint.Y == 0)
                    ComputeStretch();

                return _stretchPoint;
            }
            set { _stretchPoint = value; }
        }

        #endregion

        object thislock;
        /// <summary>
        /// 初始化RasterLayer
        /// </summary>
        /// <param name="strLayerName">图层名称</param>
        /// <param name="imageFilename">图层文件存储路径</param>
        public GdalGridRasterLayer(string strLayerName, string imageFilename)
        {
            LayerName = strLayerName;
            Filename = imageFilename;
            disposed = false;
            EnvironmentalGdal.MakeEnvironment(System.Windows.Forms.Application.StartupPath);
            //Gdal.AllRegister();

            try
            {
                _gdalDataset = Gdal.OpenShared(_Filename, Access.GA_ReadOnly);

                // have gdal read the projection
                _projectionWkt = _gdalDataset.GetProjectionRef();

                // no projection info found in the image...check for a prj
                if (_projectionWkt == "" &&
                    File.Exists(imageFilename.Substring(0, imageFilename.LastIndexOf(".")) + ".prj"))
                {
                    _projectionWkt =
                        File.ReadAllText(imageFilename.Substring(0, imageFilename.LastIndexOf(".")) + ".prj");
                }

                _imagesize = new Size(_gdalDataset.RasterXSize, _gdalDataset.RasterYSize);
                _envelope = GetExtent();
                _histoBounds = new Rectangle((int)_envelope.Left, (int)_envelope.Bottom, (int)_envelope.Width,
                                             (int)_envelope.Height);
                _lbands = _gdalDataset.RasterCount;
            }
            catch (Exception ex)
            {
                _gdalDataset = null;
                throw new Exception("Couldn't load " + imageFilename + "\n\n" + ex.Message + ex.InnerException);
            }

            thislock = new object();
        }
        public GdalGridRasterLayer(string strLayerName, string imageFilename,object thislock):this(strLayerName,imageFilename)
        {
            this.thislock = thislock;
        }

         /// <summary>
        /// 返回影像的范围
        /// </summary>
        /// <returns></returns>
        public override BoundingBox Envelope
        {
            get { return _envelope; }
        }

        /// <summary>
        /// Renders the layer
        /// </summary>
        /// <param name="g">Graphics object reference</param>
        /// <param name="map">Map which is rendered</param>
        public override void Render(Graphics g, Map map)
        {
            if (disposed)
                throw (new ApplicationException("错误: 试图添加一个已经释放的图层"));

            //先进行投影转换，把影像坐标转换为地图坐标。由于Worldwind是GCS坐标，所以需要先把影像投影坐标转换为GCS坐标
            ReprojectToMap(map);
            //图片渲染
            GetPreview(_gdalDataset, map.Size, g, map.Envelope, null, map);            
            base.Render(g, map);
        }

        /// <summary>
        /// 获取图层的投影
        /// </summary>
        /// <returns></returns>
        public ICoordinateSystem GetProjection()
        {
            CoordinateSystemFactory cFac = new CoordinateSystemFactory();

            try
            {
                if (_projectionWkt != "")
                    return cFac.CreateFromWkt(_projectionWkt);
            }
            catch (Exception)
            {

            }

            return null;
        }

        // zoom to native resolution
        public double GetOneToOne(Map map)
        {
            double DsWidth = _imagesize.Width;
            double DsHeight = _imagesize.Height;
            double left, top, right, bottom;
            double dblImgEnvW, dblImgEnvH, dblWindowGndW, dblWindowGndH, dblImginMapW, dblImginMapH;

            BoundingBox bbox = map.Envelope;
            Size size = map.Size;

            // bounds of section of image to be displayed
            left = Math.Max(bbox.Left, _envelope.Left);
            top = Math.Min(bbox.Top, _envelope.Top);
            right = Math.Min(bbox.Right, _envelope.Right);
            bottom = Math.Max(bbox.Bottom, _envelope.Bottom);

            // height and width of envelope of transformed image in ground space
            dblImgEnvW = _envelope.Right - _envelope.Left;
            dblImgEnvH = _envelope.Top - _envelope.Bottom;

            // height and width of display window in ground space
            dblWindowGndW = bbox.Right - bbox.Left;
            dblWindowGndH = bbox.Top - bbox.Bottom;

            // height and width of transformed image in pixel space
            dblImginMapW = size.Width * (dblImgEnvW / dblWindowGndW);
            dblImginMapH = size.Height * (dblImgEnvH / dblWindowGndH);

            // image was not turned on its side
            if ((dblImginMapW > dblImginMapH && DsWidth > DsHeight) ||
                (dblImginMapW < dblImginMapH && DsWidth < DsHeight))
                return map.Zoom * (dblImginMapW / DsWidth);
            // image was turned on its side
            else
                return map.Zoom * (dblImginMapH / DsWidth);
        }

        // zooms to nearest tiff internal resolution set
        public double GetZoomNearestRSet(Map map, bool bZoomIn)
        {
            double DsWidth = _imagesize.Width;
            double DsHeight = _imagesize.Height;
            double left, top, right, bottom;
            double dblImgEnvW, dblImgEnvH, dblWindowGndW, dblWindowGndH, dblImginMapW, dblImginMapH;
            double dblTempWidth = 0;

            BoundingBox bbox = map.Envelope;
            Size size = map.Size;

            // bounds of section of image to be displayed
            left = Math.Max(bbox.Left, _envelope.Left);
            top = Math.Min(bbox.Top, _envelope.Top);
            right = Math.Min(bbox.Right, _envelope.Right);
            bottom = Math.Max(bbox.Bottom, _envelope.Bottom);

            // height and width of envelope of transformed image in ground space
            dblImgEnvW = _envelope.Right - _envelope.Left;
            dblImgEnvH = _envelope.Top - _envelope.Bottom;

            // height and width of display window in ground space
            dblWindowGndW = bbox.Right - bbox.Left;
            dblWindowGndH = bbox.Top - bbox.Bottom;

            // height and width of transformed image in pixel space
            dblImginMapW = size.Width * (dblImgEnvW / dblWindowGndW);
            dblImginMapH = size.Height * (dblImgEnvH / dblWindowGndH);

            // image was not turned on its side
            if ((dblImginMapW > dblImginMapH && DsWidth > DsHeight) ||
                (dblImginMapW < dblImginMapH && DsWidth < DsHeight))
                dblTempWidth = dblImginMapW;
            else
                dblTempWidth = dblImginMapH;

            // zoom level is within the r sets
            if (DsWidth > dblTempWidth && (DsWidth / Math.Pow(2, 8)) < dblTempWidth)
            {
                if (bZoomIn)
                {
                    for (int i = 0; i <= 8; i++)
                    {
                        if (DsWidth / Math.Pow(2, i) > dblTempWidth)
                        {
                            if (DsWidth / Math.Pow(2, i + 1) < dblTempWidth)
                                return map.Zoom * (dblTempWidth / (DsWidth / Math.Pow(2, i)));
                        }
                    }
                }
                else
                {
                    for (int i = 8; i >= 0; i--)
                    {
                        if (DsWidth / Math.Pow(2, i) < dblTempWidth)
                        {
                            if (DsWidth / Math.Pow(2, i - 1) > dblTempWidth)
                                return map.Zoom * (dblTempWidth / (DsWidth / Math.Pow(2, i)));
                        }
                    }
                }
            }


            return map.Zoom;
        }
        /// <summary>
        /// 重置直方图的范围为当前影像的范围
        /// </summary>
        public void ResetHistoRectangle()
        {
            _histoBounds = new Rectangle((int)_envelope.Left, (int)_envelope.Bottom, (int)_envelope.Width,
                                         (int)_envelope.Height);
        }

        /// <summary>
        /// 进行投影转换
        /// </summary>
        /// <param name="mapProjection"></param>
        private void GetTransform(ICoordinateSystem mapProjection)
        {
            if (mapProjection == null || _projectionWkt == "")
            {
                _transform = null;
                return;
            }

            CoordinateSystemFactory cFac = new CoordinateSystemFactory();

            // get our two projections
            ICoordinateSystem srcCoord = cFac.CreateFromWkt(_projectionWkt);
            ICoordinateSystem tgtCoord = mapProjection;

            // raster and map are in same projection, no need to transform
            if (srcCoord.WKT == tgtCoord.WKT)
            {
                _transform = null;
                return;
            }

            // create transform
            _transform = new CoordinateTransformationFactory().CreateFromCoordinateSystems(srcCoord, tgtCoord);
        }
        //徐辉添加，重载获取投影转换矩阵
        private void GetTransform(string mapPrj)
        {
            if (mapPrj == "" || _projectionWkt == "")
            {
                _transform = null;
                return;
            }
            CoordinateSystemFactory cFac = new CoordinateSystemFactory();

            // get our two projections
            ICoordinateSystem srcCoord = cFac.CreateFromWkt(_projectionWkt);
            ICoordinateSystem tgtCoord = cFac.CreateFromWkt(mapPrj); ;

            // raster and map are in same projection, no need to transform
            if (srcCoord.WKT == tgtCoord.WKT)
            {
                _transform = null;
                return;
            }

            // create transform
            _transform = new CoordinateTransformationFactory().CreateFromCoordinateSystems(srcCoord, tgtCoord);

        }
        //获取影像的范围
        private BoundingBox GetExtent()
        {
            if (_gdalDataset != null)
            {
                double right = 0, left = 0, top = 0, bottom = 0;
                double dblW, dblH;

                double[] geoTrans = new double[6];


                _gdalDataset.GetGeoTransform(geoTrans);

                // no rotation...use default transform
                if (!_useRotation && !_haveSpot || (geoTrans[0] == 0 && geoTrans[3] == 0))
                    geoTrans = new[] { 999.5, 1, 0, 1000.5, 0, -1 };

                _geoTransform = new GeoTransform(geoTrans);

                // image pixels
                dblW = _imagesize.Width;
                dblH = _imagesize.Height;

                left = _geoTransform.EnvelopeLeft(dblW, dblH);
                right = _geoTransform.EnvelopeRight(dblW, dblH);
                top = _geoTransform.EnvelopeTop(dblW, dblH);
                bottom = _geoTransform.EnvelopeBottom(dblW, dblH);

                return new BoundingBox(left, bottom, right, top);
            }

            return null;
        }
        /// <summary>
        /// 获取影像4个角点坐标
        /// </summary>
        /// <returns></returns>
        public Collection<Geometries.Point> GetFourCorners()
        {
            Collection<Geometries.Point> points = new Collection<Geometries.Point>();
            double[] dblPoint;

            if (_gdalDataset != null)
            {
                double[] geoTrans = new double[6];
                _gdalDataset.GetGeoTransform(geoTrans);

                // no rotation...use default transform
                if (!_useRotation && !_haveSpot || (geoTrans[0] == 0 && geoTrans[3] == 0))
                    geoTrans = new[] { 999.5, 1, 0, 1000.5, 0, -1 };

                points.Add(new Geometries.Point(geoTrans[0], geoTrans[3]));
                points.Add(new Geometries.Point(geoTrans[0] + (geoTrans[1] * _imagesize.Width),
                                                geoTrans[3] + (geoTrans[4] * _imagesize.Width)));
                points.Add(
                    new Geometries.Point(geoTrans[0] + (geoTrans[1] * _imagesize.Width) + (geoTrans[2] * _imagesize.Height),
                                         geoTrans[3] + (geoTrans[4] * _imagesize.Width) + (geoTrans[5] * _imagesize.Height)));
                points.Add(new Geometries.Point(geoTrans[0] + (geoTrans[2] * _imagesize.Height),
                                                geoTrans[3] + (geoTrans[5] * _imagesize.Height)));

                // transform to map's projection
                if (_transform != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
#if !DotSpatialProjections
                        dblPoint = _transform.MathTransform.Transform(new[] { points[i].X, points[i].Y });
#else
                        dblPoint = points[i].ToDoubleArray();
                        Reproject.ReprojectPoints(dblPoint, null, _transform.Source, _transform.Target, 0, 1);
#endif
                        points[i] = new Geometries.Point(dblPoint[0], dblPoint[1]);
                    }
                }
            }

            return points;
        }
        /// <summary>
        /// 获取影像4个角点坐标
        /// </summary>
        /// <returns></returns>
        public Polygon GetFootprint()
        {
            LinearRing myRing = new LinearRing(GetFourCorners());
            return new Polygon(myRing);
        }
        /// <summary>
        /// 进行投影的变换
        /// </summary>
        private void ApplyTransformToEnvelope()
        {
            double[] leftBottom, leftTop, rightTop, rightBottom;
            double left, right, bottom, top;

            _envelope = GetExtent();

            if (_transform == null)
                return;

            // set envelope
            _envelope = GeometryTransform.TransformBox(_envelope, _transform.MathTransform);
            // do same to histo rectangle
            leftBottom = new double[] { _histoBounds.Left, _histoBounds.Bottom };
            leftTop = new double[] { _histoBounds.Left, _histoBounds.Top };
            rightBottom = new double[] { _histoBounds.Right, _histoBounds.Bottom };
            rightTop = new double[] { _histoBounds.Right, _histoBounds.Top };

            // transform corners into new projection
            leftBottom = _transform.MathTransform.Transform(leftBottom);
            leftTop = _transform.MathTransform.Transform(leftTop);
            rightBottom = _transform.MathTransform.Transform(rightBottom);
            rightTop = _transform.MathTransform.Transform(rightTop);

            // find extents
            left = Math.Min(leftBottom[0], Math.Min(leftTop[0], Math.Min(rightBottom[0], rightTop[0])));
            right = Math.Max(leftBottom[0], Math.Max(leftTop[0], Math.Max(rightBottom[0], rightTop[0])));
            bottom = Math.Min(leftBottom[1], Math.Min(leftTop[1], Math.Min(rightBottom[1], rightTop[1])));
            top = Math.Max(leftBottom[1], Math.Max(leftTop[1], Math.Max(rightBottom[1], rightTop[1])));

            // set histo rectangle
            _histoBounds = new Rectangle((int)left, (int)bottom, (int)right, (int)top);
        }
        /// <summary>
        /// 把影像投影转换到地图的投影
        /// </summary>
        /// <param name="map"></param>
        public void ReprojectToMap(Map map)
        {
            GetTransform(map.Projection);
            ApplyTransformToEnvelope();
        }


        public override short[] FillDEMTile(short[] demtileds, double tileWest, double tileNorth, double levelPerDegree)
        {
            if (disposed)
                throw (new ApplicationException("错误: 试图添加一个已经释放的图层"));
        
            //每个度占几个像素
            double imgPixXperDeg = _gdalDataset.RasterXSize / this.Envelope.Width;
            double imgPixYperDeg = _gdalDataset.RasterYSize / this.Envelope.Height;
            //计算起点   dataset数据的0,0是左上角开始的，而瓦片的0,0是左下角开始的
            int offsetX = (int)((tileWest + 180) * imgPixXperDeg);
            int offsetY = (int)((180-(tileNorth+90)) * imgPixYperDeg);
            //计算终点
            int outsize = (int)(levelPerDegree * imgPixXperDeg);

            if (offsetX + outsize > _gdalDataset.RasterXSize || offsetY + outsize > _gdalDataset.RasterYSize)
            {
                Console.WriteLine(string.Format("Out of bound Exception:Need from ({0},{1}) size ({2},{3}) in ({4},{5})", offsetX, offsetY, outsize, outsize, _gdalDataset.RasterXSize, _gdalDataset.RasterYSize));
                return null;
            }

            try
            {
                unsafe
                {
                    //从影像中读取值，存放在buffer中
                    Band band = _gdalDataset.GetRasterBand(1);

                    //get nodata value if present

                    lock (thislock)
                    {
                        band.ReadRaster(
                            offsetX,
                            offsetY,
                            outsize,
                            outsize,
                            demtileds, 512, 512, 0, 0);
                    }
                }
            }
            catch
            {
                demtileds = null;
            }
            finally
            {
            }

            return demtileds;
        }

        //进行影像的渲染
        protected virtual void GetPreview(Dataset dataset, Size size, Graphics g,
                                          BoundingBox displayBbox, ICoordinateSystem mapProjection, Map map)
        {
            //得到影像的6个点的值
            double[] geoTrans = new double[6];
            _gdalDataset.GetGeoTransform(geoTrans);
            _geoTransform = new GeoTransform(geoTrans);
            //得到影像的大小，xSize,ySize
            double DsWidth = _imagesize.Width;
            double DsHeight = _imagesize.Height;

            double left, top, right, bottom;//上、下、左、右四个边界，临时变量
            //GndX,GndY=请求像元点的地面真实位置
            //ImgX,ImgY=请求像元点的图像值的位置
            //PixX, PixY,循环赋值的临时变量
            double GndX = 0, GndY = 0, ImgX = 0, ImgY = 0, PixX, PixY;
            //波段的颜色表信息
            double[] intVal = new double[Bands];
            //影像的像元值
            double imageVal = 0, SpotVal = 0;
            double bitScalar = 1.0;

            //BMP对象，要进行渲染和赋值的图片
            Bitmap bitmap = null;
            Point bitmapTL = new Point(), bitmapBR = new Point();//当前渲染的图片在大图片中的范围
            //当前渲染的图片在原始影像中的范围
            Geometries.Point imageTL = new Geometries.Point(), imageBR = new Geometries.Point();

            //trueImageBbox:当前投影坐标下的请求显示区域的范围
            //shownImageBbox:原始影像投影坐标系下的请求显示区域的范围
            BoundingBox shownImageBbox, trueImageBbox;

            //请求显示区域的大小
            int bitmapLength, bitmapHeight;
            int displayImageLength, displayImageHeight;
            //bit
            int iPixelSize = 3; //Format24bppRgb = byte[b,g,r] 

            if (dataset != null)
            {
                //判断请求范围与影像自身的范围是否有交集
                if ((displayBbox.Left > _envelope.Right) || (displayBbox.Right < _envelope.Left)
                    || (displayBbox.Top < _envelope.Bottom) || (displayBbox.Bottom > _envelope.Top))
                    return;

                //创建直方图
                _histogram = new List<int[]>();
                for (int i = 0; i < _lbands + 1; i++)
                    _histogram.Add(new int[256]);

                //计算影像的真实范围
                left = Math.Max(displayBbox.Left, _envelope.Left);
                top = Math.Min(displayBbox.Top, _envelope.Top);
                right = Math.Min(displayBbox.Right, _envelope.Right);
                bottom = Math.Max(displayBbox.Bottom, _envelope.Bottom);
                trueImageBbox = new BoundingBox(left, bottom, right, top);

                //把真实请求范围转换到原始影像坐标下的请求范围
                if (_transform != null)
                {
                    _transform.MathTransform.Invert();
                    shownImageBbox = GeometryTransform.TransformBox(trueImageBbox, _transform.MathTransform);
                    _transform.MathTransform.Invert();
                }
                else
                    shownImageBbox = trueImageBbox;

                //计算请求区域在原始影像中的取值的行列范围
                imageBR.X =
                    (int)
                    (Math.Max(_geoTransform.GroundToImage(shownImageBbox.TopLeft).X,
                              Math.Max(_geoTransform.GroundToImage(shownImageBbox.TopRight).X,
                                       Math.Max(_geoTransform.GroundToImage(shownImageBbox.BottomLeft).X,
                                                _geoTransform.GroundToImage(shownImageBbox.BottomRight).X))) + 1);
                imageBR.Y =
                    (int)
                    (Math.Max(_geoTransform.GroundToImage(shownImageBbox.TopLeft).Y,
                              Math.Max(_geoTransform.GroundToImage(shownImageBbox.TopRight).Y,
                                       Math.Max(_geoTransform.GroundToImage(shownImageBbox.BottomLeft).Y,
                                                _geoTransform.GroundToImage(shownImageBbox.BottomRight).Y))) + 1);
                imageTL.X =
                    (int)
                    Math.Min(_geoTransform.GroundToImage(shownImageBbox.TopLeft).X,
                             Math.Min(_geoTransform.GroundToImage(shownImageBbox.TopRight).X,
                                      Math.Min(_geoTransform.GroundToImage(shownImageBbox.BottomLeft).X,
                                               _geoTransform.GroundToImage(shownImageBbox.BottomRight).X)));
                imageTL.Y =
                    (int)
                    Math.Min(_geoTransform.GroundToImage(shownImageBbox.TopLeft).Y,
                             Math.Min(_geoTransform.GroundToImage(shownImageBbox.TopRight).Y,
                                      Math.Min(_geoTransform.GroundToImage(shownImageBbox.BottomLeft).Y,
                                               _geoTransform.GroundToImage(shownImageBbox.BottomRight).Y)));

                //确保请求的DN值在原始影像的大小范围内
                if (imageBR.X > _imagesize.Width)
                    imageBR.X = _imagesize.Width;
                if (imageBR.Y > _imagesize.Height)
                    imageBR.Y = _imagesize.Height;
                if (imageTL.Y < 0)
                    imageTL.Y = 0;
                if (imageTL.X < 0)
                    imageTL.X = 0;

                displayImageLength = (int)(imageBR.X - imageTL.X);
                displayImageHeight = (int)(imageBR.Y - imageTL.Y);


                //计算要在大图片的哪个位置开始进行渲染图片
                bitmapBR = new Point((int)map.WorldToImage(trueImageBbox.BottomRight).X +1,
                                     (int)map.WorldToImage(trueImageBbox.BottomRight).Y +1);
                bitmapTL = new Point((int)map.WorldToImage(trueImageBbox.TopLeft).X,
                                     (int)map.WorldToImage(trueImageBbox.TopLeft).Y);

                //渲染图片的大小
                bitmapLength = bitmapBR.X - bitmapTL.X;
                bitmapHeight = bitmapBR.Y - bitmapTL.Y;
                displayImageLength = bitmapLength;
                displayImageHeight = bitmapHeight;
                //if (bitmapLength > bitmapHeight && displayImageLength < displayImageHeight)
                //{
                //    displayImageLength = bitmapHeight;
                //    displayImageHeight = bitmapLength;
                //}
                //else
                //{
                //    displayImageLength = bitmapLength;
                //    displayImageHeight = bitmapHeight;
                //}

                //bit率
                if (_bitDepth == 12)
                    bitScalar = 16.0;
                else if (_bitDepth == 16)
                    bitScalar = 256.0;
                else if (_bitDepth == 32)
                    bitScalar = 16777216.0;

                if (bitmapLength < 1 || bitmapHeight < 1)
                    return;

                //初始化BMP对象
                bitmap = new Bitmap(bitmapLength, bitmapHeight, PixelFormat.Format24bppRgb);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmapLength, bitmapHeight),
                                                        ImageLockMode.ReadWrite, bitmap.PixelFormat);

                try
                {
                    unsafe
                    {
                        //把所有的值先初始化成黄色，为了最后进行背景色的去除！
                        byte cr = _noDataInitColor.R;
                        byte cg = _noDataInitColor.G;
                        byte cb = _noDataInitColor.B;

                        for (int y = 0; y < bitmapHeight; y++)
                        {
                            byte* brow = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                            for (int x = 0; x < bitmapLength; x++)
                            {
                                Int32 offsetX = x * 3;
                                brow[offsetX++] = cb;
                                brow[offsetX++] = cg;
                                brow[offsetX] = cr;
                            }
                        }

                        // create 3 dimensional buffer [band][x pixel][y pixel]
                        double[][] tempBuffer = new double[Bands][];
                        double[][][] buffer = new double[Bands][][];
                        for (int i = 0; i < Bands; i++)
                        {
                            buffer[i] = new double[displayImageLength][];
                            for (int j = 0; j < displayImageLength; j++)
                                buffer[i][j] = new double[displayImageHeight];
                        }

                        Band[] band = new Band[Bands];
                        int[] ch = new int[Bands];

                        //
                        Double[] noDataValues = new Double[Bands];
                        Double[] scales = new Double[Bands];
                        ColorTable colorTable = null;


                        //从影像中读取值，存放在buffer中
                        for (int i = 0; i < Bands; i++)
                        {
                            tempBuffer[i] = new double[displayImageLength * displayImageHeight];
                            band[i] = dataset.GetRasterBand(i + 1);

                            //get nodata value if present
                            Int32 hasVal = 0;
                            band[i].GetNoDataValue(out noDataValues[i], out hasVal);
                            if (hasVal == 0) noDataValues[i] = Double.NaN;
                            band[i].GetScale(out scales[i], out hasVal);
                            if (hasVal == 0) scales[i] = 1.0;
                            lock (thislock)
                            {
                                band[i].ReadRaster(
                                    (int)imageTL.X,
                                    (int)imageTL.Y,
                                    (int)(imageBR.X - imageTL.X),
                                    (int)(imageBR.Y - imageTL.Y),
                                    tempBuffer[i], displayImageLength, displayImageHeight, 0, 0);
                            }
                            // parse temp buffer into the image x y value buffer
                            long pos = 0;
                            for (int y = 0; y < displayImageHeight; y++)
                            {
                                for (int x = 0; x < displayImageLength; x++, pos++)
                                    buffer[i][x][y] = tempBuffer[i][pos];
                            }

                            if (band[i].GetRasterColorInterpretation() == ColorInterp.GCI_BlueBand) ch[i] = 0;
                            else if (band[i].GetRasterColorInterpretation() == ColorInterp.GCI_GreenBand) ch[i] = 1;
                            else if (band[i].GetRasterColorInterpretation() == ColorInterp.GCI_RedBand) ch[i] = 2;
                            else if (band[i].GetRasterColorInterpretation() == ColorInterp.GCI_Undefined)
                            {
                                if (Bands > 1)
                                    ch[i] = 3; // infrared
                                else
                                {
                                    ch[i] = 4;
                                    if (_colorBlend == null)
                                    {
                                        Double dblMin, dblMax;
                                        band[i].GetMinimum(out dblMin, out hasVal);
                                        if (hasVal == 0) dblMin = Double.NaN;
                                        band[i].GetMaximum(out dblMax, out hasVal);
                                        if (hasVal == 0) dblMax = double.NaN;
                                        if (Double.IsNaN(dblMin) || Double.IsNaN(dblMax))
                                        {
                                            double dblMean, dblStdDev;
                                            band[i].GetStatistics(0, 1, out dblMin, out dblMax, out dblMean, out dblStdDev);
                                            //double dblRange = dblMax - dblMin;
                                            //dblMin -= 0.1*dblRange;
                                            //dblMax += 0.1*dblRange;
                                        }
                                        Single[] minmax = new float[] { Convert.ToSingle(dblMin), 0.5f * Convert.ToSingle(dblMin + dblMax), Convert.ToSingle(dblMax) };
                                        Color[] colors = new Color[] { Color.Blue, Color.Yellow, Color.Red };
                                        _colorBlend = new ColorBlend(colors, minmax);
                                    }
                                    intVal = new Double[3];
                                }
                            }
                            else if (band[i].GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex) ch[i] = 0;
                            else if (band[i].GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
                            {
                                colorTable = band[i].GetRasterColorTable();
                                ch[i] = 5;
                                intVal = new Double[3];
                            }
                            else ch[i] = -1;
                        }

                        // 临时变量
                        int bitmapTLX = bitmapTL.X;
                        int bitmapTLY = bitmapTL.Y;
                        double imageTop = imageTL.Y;
                        double imageLeft = imageTL.X;
                        double dblMapPixelWidth = map.PixelWidth;
                        double dblMapPixelHeight = map.PixelHeight;
                        double dblMapMinX = map.Envelope.Min.X;
                        double dblMapMaxY = map.Envelope.Max.Y;
                        double geoTop, geoLeft, geoHorzPixRes, geoVertPixRes, geoXRot, geoYRot;

                        // get inverse values
                        geoTop = _geoTransform.Inverse[3];
                        geoLeft = _geoTransform.Inverse[0];
                        geoHorzPixRes = _geoTransform.Inverse[1];
                        geoVertPixRes = _geoTransform.Inverse[5];
                        geoXRot = _geoTransform.Inverse[2];
                        geoYRot = _geoTransform.Inverse[4];

                        double dblXScale = (imageBR.X - imageTL.X) / (displayImageLength - 1);
                        double dblYScale = (imageBR.Y - imageTL.Y) / (displayImageHeight - 1);
                        double[] dblPoint;


                        IMathTransform inverseTransform = null;
                        if (_transform != null)
                            inverseTransform = _transform.MathTransform.Inverse();

                        for (PixY = 0; PixY < bitmapBR.Y - bitmapTL.Y; PixY++)
                        {
                            byte* row = (byte*)bitmapData.Scan0 + ((int)Math.Round(PixY) * bitmapData.Stride);

                            for (PixX = 0; PixX < bitmapBR.X - bitmapTL.X; PixX++)
                            {
                                //计算当前像素点的经纬度
                                GndX = trueImageBbox.Left + PixX * dblMapPixelWidth;
                                GndY = trueImageBbox.Top - PixY * dblMapPixelHeight;
                                //GndX = dblMapMinX + (PixX + bitmapTL1.X) * dblMapPixelWidth;
                                //GndY = dblMapMaxY - (PixY + bitmapTL1.Y) * dblMapPixelHeight;
                                //转换到影像原始坐标下
                                if (_transform != null)
                                {
                                    dblPoint = inverseTransform.Transform(new[] { GndX, GndY });
                                    GndX = dblPoint[0];
                                    GndY = dblPoint[1];
                                }

                                //通过地面真实经纬信息，转换到在影像中当前位置的值信息
                                ImgX = (geoLeft + geoHorzPixRes * GndX + geoXRot * GndY);
                                ImgY = (geoTop + geoYRot * GndX + geoVertPixRes * GndY);
                                int xPos=(int)((ImgX - imageLeft) / dblXScale);
                                int yPos=(int)((ImgY - imageTop) / dblYScale);
                                if (ImgX < imageTL.X || ImgX > imageBR.X || ImgY < imageTL.Y || ImgY > imageBR.Y)
                                {
                                    xPos = (int)PixX;
                                    yPos = (int)(bitmapBR.Y - bitmapTL.Y - PixY - 1);                             
                                }

                                if (this.Bands == 3)
                                {
                                    if (buffer[0][xPos][yPos] == 0.0 && buffer[1][xPos][yPos] == 0.0 && buffer[2][xPos][yPos] == 0.0)
                                    {
                                        continue;
                                    }
                                }
                                else if (this.Bands == 1)
                                {
                                    if (buffer[0][xPos][yPos] == 0.0)
                                    {
                                        continue;
                                    }
                                }
                                //颜色纠正
                                for (int i = 0; i < Bands; i++)
                                {

                                    if (double.IsNaN(dblYScale) || double.IsNaN(dblXScale))
                                        intVal[i] = 0;
                                    else
                                        intVal[i] =buffer[i][xPos][yPos];

                                    imageVal = SpotVal = intVal[i] = intVal[i] / bitScalar;
                                    if (ch[i] == 4)
                                    {
                                        if (imageVal != noDataValues[i])
                                        {
                                            Color color = _colorBlend.GetColor(Convert.ToSingle(imageVal));
                                            intVal[0] = color.B;
                                            intVal[1] = color.G;
                                            intVal[2] = color.R;
                                            //intVal[3] = ce.c4;
                                        }
                                        else
                                        {
                                            intVal[0] = cb;
                                            intVal[1] = cg;
                                            intVal[2] = cr;
                                        }
                                    }

                                    else if (ch[i] == 5 && colorTable != null)
                                    {
                                        if (imageVal != noDataValues[i])
                                        {
                                            ColorEntry ce = colorTable.GetColorEntry(Convert.ToInt32(imageVal));
                                            intVal[0] = ce.c3;
                                            intVal[1] = ce.c2;
                                            intVal[2] = ce.c1;
                                            //intVal[3] = ce.c4;
                                        }
                                        else
                                        {
                                            intVal[0] = cb;
                                            intVal[1] = cg;
                                            intVal[2] = cr;
                                        }
                                    }

                                    else
                                    {

                                        if (_colorCorrect)
                                        {
                                            intVal[i] = ApplyColorCorrection(imageVal, SpotVal, ch[i], GndX, GndY);

                                            // if pixel is within ground boundary, add its value to the histogram
                                            if (ch[i] != -1 && intVal[i] > 0 && (_histoBounds.Bottom >= (int)GndY) &&
                                                _histoBounds.Top <= (int)GndY &&
                                                _histoBounds.Left <= (int)GndX && _histoBounds.Right >= (int)GndX)
                                            {
                                                _histogram[ch[i]][(int)intVal[i]]++;
                                            }
                                        }
                                        if (intVal[i] > 255)
                                            intVal[i] = 255;
                                    }
                                }

                                // luminosity
                                if (_lbands >= 3)
                                    _histogram[_lbands][(int)(intVal[2] * 0.2126 + intVal[1] * 0.7152 + intVal[0] * 0.0722)]
                                        ++;
                                //把当前影像值写入图片中去
                                WritePixel(PixX, intVal, iPixelSize, ch, row);
                            }
                        }
                    }
                }

                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }
            //设置背景色为透明色
            bitmap.MakeTransparent(_noDataInitColor);
            if (_transparentColor != Color.Empty)
                bitmap.MakeTransparent(_transparentColor);
            g.DrawImage(bitmap, new Point(bitmapTL.X, bitmapTL.Y));
        }

        protected unsafe void WritePixel(double x, double[] intVal, int iPixelSize, int[] ch, byte* row)
        {
            // write out pixels
            // black and white
            Int32 offsetX = (int)Math.Round(x) * iPixelSize;
            if (Bands == 1 && _bitDepth != 32)
            {
                if (ch[0] < 4)
                {
                    if (_showClip)
                    {
                        if (intVal[0] == 0)
                        {
                            row[offsetX++] = 255;
                            row[offsetX++] = 0;
                            row[offsetX] = 0;
                        }
                        else if (intVal[0] == 255)
                        {
                            row[offsetX++] = 0;
                            row[offsetX++] = 0;
                            row[offsetX] = 255;
                        }
                        else
                        {
                            row[offsetX++] = (byte)intVal[0];
                            row[offsetX++] = (byte)intVal[0];
                            row[offsetX] = (byte)intVal[0];
                        }
                    }
                    else
                    {
                        row[offsetX++] = (byte)intVal[0];
                        row[offsetX++] = (byte)intVal[0];
                        row[offsetX] = (byte)intVal[0];
                    }
                }
                else
                {
                    row[offsetX++] = (byte)intVal[0];
                    row[offsetX++] = (byte)intVal[1];
                    row[offsetX] = (byte)intVal[2];
                }
            }
            // IR grayscale
            else if (DisplayIR && Bands == 4)
            {
                for (int i = 0; i < Bands; i++)
                {
                    if (ch[i] == 3)
                    {
                        if (_showClip)
                        {
                            if (intVal[3] == 0)
                            {
                                row[(int)Math.Round(x) * iPixelSize] = 255;
                                row[(int)Math.Round(x) * iPixelSize + 1] = 0;
                                row[(int)Math.Round(x) * iPixelSize + 2] = 0;
                            }
                            else if (intVal[3] == 255)
                            {
                                row[(int)Math.Round(x) * iPixelSize] = 0;
                                row[(int)Math.Round(x) * iPixelSize + 1] = 0;
                                row[(int)Math.Round(x) * iPixelSize + 2] = 255;
                            }
                            else
                            {
                                row[(int)Math.Round(x) * iPixelSize] = (byte)intVal[i];
                                row[(int)Math.Round(x) * iPixelSize + 1] = (byte)intVal[i];
                                row[(int)Math.Round(x) * iPixelSize + 2] = (byte)intVal[i];
                            }
                        }
                        else
                        {
                            row[(int)Math.Round(x) * iPixelSize] = (byte)intVal[i];
                            row[(int)Math.Round(x) * iPixelSize + 1] = (byte)intVal[i];
                            row[(int)Math.Round(x) * iPixelSize + 2] = (byte)intVal[i];
                        }
                    }
                    else
                        continue;
                }
            }
            // CIR
            else if (DisplayCIR && Bands == 4)
            {
                if (_showClip)
                {
                    if (intVal[0] == 0 && intVal[1] == 0 && intVal[3] == 0)
                    {
                        intVal[3] = intVal[0] = 0;
                        intVal[1] = 255;
                    }
                    else if (intVal[0] == 255 && intVal[1] == 255 && intVal[3] == 255)
                        intVal[1] = intVal[0] = 0;
                }

                for (int i = 0; i < Bands; i++)
                {
                    if (ch[i] != 0 && ch[i] != -1)
                        row[(int)Math.Round(x) * iPixelSize + ch[i] - 1] = (byte)intVal[i];
                }
            }
            // RGB
            else
            {
                if (_showClip)
                {
                    if (intVal[0] == 0 && intVal[1] == 0 && intVal[2] == 0)
                    {
                        intVal[0] = intVal[1] = 0;
                        intVal[2] = 255;
                    }
                    else if (intVal[0] == 255 && intVal[1] == 255 && intVal[2] == 255)
                        intVal[1] = intVal[2] = 0;
                }

                for (int i = 0; i < 3; i++)
                {
                    if (ch[i] != 3 && ch[i] != -1)
                        row[(int)Math.Round(x) * iPixelSize + ch[i]] = (byte)intVal[i];
                }
            }
        }

        // apply any color correction to pixel
        private double ApplyColorCorrection(double imageVal, double spotVal, int channel, double GndX, double GndY)
        {
            double finalVal;
            double distance;
            double imagePct, spotPct;

            finalVal = imageVal;

            if (_haveSpot)
            {
                // gamma
                if (_nonSpotGamma != 1)
                    imageVal = 256 * Math.Pow((imageVal / 256), _nonSpotGamma);

                // gain
                if (channel == 2)
                    imageVal = imageVal * _nonSpotGain[0];
                else if (channel == 1)
                    imageVal = imageVal * _nonSpotGain[1];
                else if (channel == 0)
                    imageVal = imageVal * _nonSpotGain[2];
                else if (channel == 3)
                    imageVal = imageVal * _nonSpotGain[3];

                if (imageVal > 255)
                    imageVal = 255;

                // curve
                if (_nonSpotCurveLut != null)
                    if (_nonSpotCurveLut.Count != 0)
                    {
                        if (channel == 2 || channel == 4)
                            imageVal = _nonSpotCurveLut[0][(int)imageVal];
                        else if (channel == 1)
                            imageVal = _nonSpotCurveLut[1][(int)imageVal];
                        else if (channel == 0)
                            imageVal = _nonSpotCurveLut[2][(int)imageVal];
                        else if (channel == 3)
                            imageVal = _nonSpotCurveLut[3][(int)imageVal];
                    }

                finalVal = imageVal;

                distance = Math.Sqrt(Math.Pow(GndX - SpotPoint.X, 2) + Math.Pow(GndY - SpotPoint.Y, 2));

                if (distance <= _innerSpotRadius + _outerSpotRadius)
                {
                    // gamma
                    if (_spotGamma != 1)
                        spotVal = 256 * Math.Pow((spotVal / 256), _spotGamma);

                    // gain
                    if (channel == 2)
                        spotVal = spotVal * _spotGain[0];
                    else if (channel == 1)
                        spotVal = spotVal * _spotGain[1];
                    else if (channel == 0)
                        spotVal = spotVal * _spotGain[2];
                    else if (channel == 3)
                        spotVal = spotVal * _spotGain[3];

                    if (spotVal > 255)
                        spotVal = 255;

                    // curve
                    if (_spotCurveLut != null)
                        if (_spotCurveLut.Count != 0)
                        {
                            if (channel == 2 || channel == 4)
                                spotVal = _spotCurveLut[0][(int)spotVal];
                            else if (channel == 1)
                                spotVal = _spotCurveLut[1][(int)spotVal];
                            else if (channel == 0)
                                spotVal = _spotCurveLut[2][(int)spotVal];
                            else if (channel == 3)
                                spotVal = _spotCurveLut[3][(int)spotVal];
                        }

                    if (distance < _innerSpotRadius)
                        finalVal = spotVal;
                    else
                    {
                        imagePct = (distance - _innerSpotRadius) / _outerSpotRadius;
                        spotPct = 1 - imagePct;

                        finalVal = (Math.Round((spotVal * spotPct) + (imageVal * imagePct)));
                    }
                }
            }

            // gamma
            if (_gamma != 1)
                finalVal = (256 * Math.Pow((finalVal / 256), _gamma));


            switch (channel)
            {
                case 2:
                    finalVal = finalVal * _gain[0];
                    break;
                case 1:
                    finalVal = finalVal * _gain[1];
                    break;
                case 0:
                    finalVal = finalVal * _gain[2];
                    break;
                case 3:
                    finalVal = finalVal * _gain[3];
                    break;
            }

            if (finalVal > 255)
                finalVal = 255;

            // curve
            if (_curveLut != null)
                if (_curveLut.Count != 0)
                {
                    if (channel == 2 || channel == 4)
                        finalVal = _curveLut[0][(int)finalVal];
                    else if (channel == 1)
                        finalVal = _curveLut[1][(int)finalVal];
                    else if (channel == 0)
                        finalVal = _curveLut[2][(int)finalVal];
                    else if (channel == 3)
                        finalVal = _curveLut[3][(int)finalVal];
                }

            return finalVal;
        }

        // find min and max pixel values of the image
        private void ComputeStretch()
        {
            double min = 99999999, max = -99999999;
            int width, height;

            if (_gdalDataset.RasterYSize < 4000)
            {
                height = _gdalDataset.RasterYSize;
                width = _gdalDataset.RasterXSize;
            }
            else
            {
                height = 4000;
                width = (int)(4000 * (_gdalDataset.RasterXSize / (double)_gdalDataset.RasterYSize));
            }

            double[] buffer = new double[width * height];

            for (int band = 1; band <= _lbands; band++)
            {
                Band RBand = _gdalDataset.GetRasterBand(band);
                RBand.ReadRaster(0, 0, _gdalDataset.RasterXSize, _gdalDataset.RasterYSize, buffer, width, height, 0, 0);

                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] < min)
                        min = buffer[i];
                    if (buffer[i] > max)
                        max = buffer[i];
                }
            }

            if (_bitDepth == 12)
            {
                min /= 16;
                max /= 16;
            }
            else if (_bitDepth == 16)
            {
                min /= 256;
                max /= 256;
            }

            if (max > 255)
                max = 255;

            _stretchPoint = new Point((int)min, (int)max);
        }

        #region Disposers and finalizers

        private bool disposed;

        /// <summary>
        /// Disposes the GdalRasterLayer and release the raster file
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    if (_gdalDataset != null)
                    {
                        try
                        {
                            _gdalDataset.Dispose();
                        }
                        finally
                        {
                            _gdalDataset = null;
                        }
                    }
                disposed = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~GdalGridRasterLayer()
        {
            Dispose(true);
        }

        #endregion

        #region Implementation of ICanQueryLayer

        /// <summary>
        /// Returns the data associated with the centroid of the bounding box.
        /// </summary>
        /// <param name="box">BoundingBox to intersect with</param>
        /// <param name="ds">FeatureDataSet to fill data into</param>
        public void ExecuteIntersectionQuery(BoundingBox box, FeatureDataSet ds)
        {
            Geometries.Point pt = new Geometries.Point(
                box.Left + 0.5 * box.Width,
                box.Top - 0.5 * box.Height);
            ExecuteIntersectionQuery(pt, ds);
        }

        private void ExecuteIntersectionQuery(Geometries.Point pt, FeatureDataSet ds)
        {

            if (CoordinateTransformation != null)
            {
#if !DotSpatialProjections
                CoordinateTransformation.MathTransform.Invert();
                pt = GeometryTransform.TransformPoint(pt, CoordinateTransformation.MathTransform);
                CoordinateTransformation.MathTransform.Invert();
#else
                pt = GeometryTransform.TransformPoint(pt, CoordinateTransformation.Target, CoordinateTransformation.Source);
#endif
            }

            //Setup resulting Table
            FeatureDataTable dt = new FeatureDataTable();
            dt.Columns.Add("Ordinate X", typeof(Double));
            dt.Columns.Add("Ordinate Y", typeof(Double));
            for (int i = 1; i <= Bands; i++)
                dt.Columns.Add(string.Format("Value Band {0}", i), typeof(Double));

            //Get location on raster
            Double[] buffer = new double[1];
            Int32[] bandMap = new int[Bands];
            for (int i = 1; i <= Bands; i++) bandMap[i - 1] = i;
            Geometries.Point imgPt = _geoTransform.GroundToImage(pt);
            Int32 x = Convert.ToInt32(imgPt.X);
            Int32 y = Convert.ToInt32(imgPt.Y);

            //Test if raster ordinates are within bounds
            if (x < 0) return;
            if (y < 0) return;
            if (x >= _imagesize.Width) return;
            if (y >= _imagesize.Height) return;

            //Create new row, add ordinates and location geometry
            FeatureDataRow dr = dt.NewRow();
            dr.Geometry = pt;
            dr[0] = pt.X;
            dr[1] = pt.Y;

            //Add data from raster
            for (int i = 1; i <= Bands; i++)
            {
                Band band = _gdalDataset.GetRasterBand(i);
                //DataType dtype = band.DataType;
                CPLErr res = band.ReadRaster(x, y, 1, 1, buffer, 1, 1, 0, 0);
                if (res == CPLErr.CE_None)
                {
                    dr[1 + i] = buffer[0];
                }
                else
                {
                    dr[1 + i] = Double.NaN;
                }
            }
            //Add new row to table
            dt.Rows.Add(dr);

            //Add table to dataset
            ds.Tables.Add(dt);
        }

        /// <summary>
        /// Returns the data associated with all the geometries that are intersected by 'geom'
        /// </summary>
        /// <param name="geometry">Geometry to intersect with</param>
        /// <param name="ds">FeatureDataSet to fill data into</param>
        public void ExecuteIntersectionQuery(Geometry geometry, FeatureDataSet ds)
        {
            ExecuteIntersectionQuery(geometry.GetBoundingBox(), ds);
        }

        private bool _isQueryEnabled = true;
        public bool IsQueryEnabled
        {
            get
            {
                return _isQueryEnabled;
            }
            set
            {
                _isQueryEnabled = value;
            }
        }

        #endregion

        private Color _noDataInitColor = Color.Yellow;
        public Color NoDataInitColor
        {
            get { return _noDataInitColor; }
            set { _noDataInitColor = value; }
        }

        private ColorBlend _colorBlend;
        public ColorBlend ColorBlend
        {
            get { return _colorBlend; }
            set { _colorBlend = value; }
        }

    }
}
