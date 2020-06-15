using System;
using System.Collections;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Utility;
using QRST.WorldGlobeTool.Geometries;

namespace QRST.WorldGlobeTool.Renderable
{
    /// <summary>
    /// GCP纹理类
    /// </summary>
    public class GCPTexture : IDisposable
    {
        /// <summary>
        /// GCP所对应的纹理
        /// </summary>
        public Texture Texture;
        /// <summary>
        /// GCP的宽度
        /// </summary>
        public int Width;
        /// <summary>
        /// GCP的高度
        /// </summary>
        public int Height;

        /// <summary>
        /// 获得GCP纹理对象
        /// </summary>
        /// <param name="device">d3d对象</param>
        /// <param name="textureFileName">图片的路径</param>
        public GCPTexture(Device device, string textureFileName)
        {
            //判断图片路径是否为空
            if ((textureFileName != null) && textureFileName.Length > 0)
            {
                //判断是否是GDI支持的图片，若是，则用GDI读成Image或Bitmap对象，再转换为纹理，速度会快
                if (ImageHelper.IsGdiSupportedImageFormat(textureFileName))
                {
                    //转换为Texture对象
                    using (Image image = ImageHelper.LoadImage(textureFileName))
                        LoadImage(device, image);
                }
                else
                {
                    //只能用DirectX读取的对象
                    Texture = ImageHelper.LoadGCPTexture(textureFileName);
                    using (Surface s = Texture.GetSurfaceLevel(0))
                    {
                        SurfaceDescription desc = s.Description;
                        Width = desc.Width;
                        Height = desc.Height;
                    }
                }
            }
        }

        /// <summary>
        /// 获得GCP纹理对象 <see cref= "T:Qrst.Renderable.GCPTexture"/> class 
        /// 从一个Bitmap对象.
        /// </summary>
        public GCPTexture(Device device, Bitmap image)
        {
            LoadImage(device, image);
        }

        /// <summary>
        /// 直接得到纹理对象
        /// </summary>
        /// <param name="device"></param>
        /// <param name="image"></param>
        protected void LoadImage(Device device, Image image)
        {
            Width = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Width) / Math.Log(2)))));
            if (Width > device.DeviceCaps.MaxTextureWidth)
                Width = device.DeviceCaps.MaxTextureWidth;

            Height = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Height) / Math.Log(2)))));
            if (Height > device.DeviceCaps.MaxTextureHeight)
                Height = device.DeviceCaps.MaxTextureHeight;

            using (Bitmap textureSource = new Bitmap(Width, Height))
            using (Graphics g = Graphics.FromImage(textureSource))
            {
                g.DrawImage(image, 0, 0, Width, Height);
                if (Texture != null)
                    Texture.Dispose();
                Texture = new Texture(device, textureSource, Usage.None, Pool.Managed);
            }
        }

        #region 释放纹理对象

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Texture != null)
            {
                Texture.Dispose();
                Texture = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

    }

    /// <summary>
    /// 控制点类
    /// </summary>
    public class GCP : RenderableObject
    {

        #region  字段

        public override Extension Extension
        {
            get
            {
                Extension ext = new Extension(this.Latitude, this.Latitude, this.Longitude, this.Longitude);
                return ext;
            }
        }
        /// <summary>
        /// 当前控制点所属的球体控件
        /// </summary>
        public QRSTWorldGlobeControl QrstGlobe;
        /// <summary>
        /// 
        /// </summary>
        public double OnClickZoomAltitude = double.NaN;
        /// <summary>
        /// 
        /// </summary>
        public double OnClickZoomHeading = double.NaN;
        /// <summary>
        /// 
        /// </summary>
        public double OnClickZoomTilt = double.NaN;
        /// <summary>
        /// 上次刷新的时间
        /// </summary>
        public System.DateTime LastRefresh = System.DateTime.MinValue;
        /// <summary>
        /// 刷新间隔
        /// </summary>
        public System.TimeSpan RefreshInterval = System.TimeSpan.MaxValue;
        /// <summary>
        /// 鼠标是否移过当前控制点
        /// </summary>
        //public bool IsMouseHover;
        /// <summary>
        /// 
        /// </summary>
        ArrayList overlays = new ArrayList();
        /// <summary>
        /// 旋转的角度
        /// </summary>
        private Angle m_rotation = Angle.Zero;
        /// <summary>
        /// GCP是否旋转
        /// </summary>
        private bool m_isRotated = false;
        /// <summary>
        /// GCP的位置
        /// </summary>
        private Point3d m_positionD = new Point3d();
        /// <summary>
        /// 
        /// </summary>
        private bool m_nameAlwaysVisible = false;
        /// <summary>
        /// 此GCP的纬度
        /// </summary>
        protected double m_latitude;
        /// <summary>
        /// 此GCP的经度
        /// </summary>
        protected double m_longitude;
        /// <summary>
        /// 此GCP在原始图像中的Y坐标
        /// </summary>
        protected double m_y;
        /// <summary>
        /// 此GCP在原始图像中的X坐标
        /// </summary>
        protected double m_x;
        /// <summary>
        /// GCP的高度
        /// </summary>
        public double Altitude;
        /// <summary>
        /// 保存文件的路径
        /// </summary>
        public string SaveFilePath = null;
        /// <summary>
        /// GCP的文件的路径
        /// </summary>
        public string TextureFileName;
        /// <summary>
        /// GCP所对应的图片对象.
        /// </summary>
        public Bitmap Image;
        /// <summary>
        /// GCP的宽度.
        /// </summary>
        public int Width;
        /// <summary>
        /// GCP的高度
        /// </summary>
        public int Height;
        /// <summary>
        /// 最高能见度
        /// </summary>
        public double MaximumDisplayDistance = double.MaxValue;
        /// <summary>
        /// 最低能见度
        /// </summary>
        public double MinimumDisplayDistance;
        /// <summary>
        /// 判断鼠标是否在此巨型范围内
        /// </summary>
        public Rectangle SelectionRectangle;
        /// <summary>
        /// 记录了上次的视图矩阵
        /// </summary>
        Matrix lastView = Matrix.Identity;
        /// <summary>
        /// 是否正在移动当前控制点
        /// </summary>
        private bool m_isDragging = false;
        /// <summary>
        /// 当前控制点是否刚刚被移动过
        /// </summary>
        public bool IsJustMoved;

        /// <summary>
        /// 控制点类型
        /// </summary>
        private GCPType m_gcpType;

        private bool m_IsGCPErrorNormal;

        #endregion

        #region  属性

        /// <summary>
        /// 获取或设置GCP的位置
        /// </summary>
        public Point3d PositionD
        {
            get { return m_positionD; }
            set { m_positionD = value; }
        }
        /// <summary>
        /// 获取或设置GCP的纬度
        /// </summary>
        public double Latitude
        {
            get { return m_latitude; }
            set { m_latitude = value; }
        }
        /// <summary>
        /// 获取或设置GCP的经度
        /// </summary>
        public double Longitude
        {
            get { return m_longitude; }
            set { m_longitude = value; }
        }
        /// <summary>
        /// 获取此GCP在原始图像中的Y坐标
        /// </summary>
        public double Y
        {
            get { return m_y; }
        }
        /// <summary>
        /// 获取此GCP在原始图像中的X坐标
        /// </summary>
        public double X
        {
            get { return m_x; }
        }
        /// <summary>
        /// 获取或设置旋转控制点的角度
        /// </summary>
        public Angle Rotation
        {
            get
            {
                return m_rotation;
            }
            set
            {
                m_rotation = value;
            }
        }
        /// <summary>
        /// 获取或设置控制点是否旋转
        /// </summary>
        public bool IsRotated
        {
            get
            {
                return m_isRotated;
            }
            set
            {
                m_isRotated = value;
            }
        }
        /// <summary>
        /// 获取或设置是否一直显示GCP的名称
        /// </summary>
        public bool NameAlwaysVisible
        {
            get { return m_nameAlwaysVisible; }
            set { m_nameAlwaysVisible = value; }
        }

        /// <summary>
        /// 获取或设置是否正在移动当前控制点
        /// </summary>
        public bool IsDragging
        {
            get { return m_isDragging; }
            set { m_isDragging = value; }
        }
        /// <summary>
        /// 获取或设置控制点类型
        /// </summary>
        public GCPType GcpType
        {
            get { return m_gcpType; }
            set { m_gcpType = value; }
        }

        /// <summary>
        /// 获取或设置控制点的误差是否在正常范围内
        /// </summary>
        public bool IsGCPErrorNormal
        {
            get
            {
                return m_IsGCPErrorNormal;
            }
            set
            {
                m_IsGCPErrorNormal = value;
                if (value == true)
                {
                    TextureFileName =
                        GcpType == GCPType.GeoGCP ?
                        System.IO.Path.Combine(this.QrstGlobe.DataDirectory, @"Icons\Geo_GCP_24_Normal.png")
                        : System.IO.Path.Combine(this.QrstGlobe.DataDirectory, @"Icons\ATM_GCP_32_Normal.png");
                }
                else
                {
                    TextureFileName =
                        GcpType == GCPType.GeoGCP ?
                        System.IO.Path.Combine(this.QrstGlobe.DataDirectory, @"Icons\Geo_GCP_24_MoreThanThreshold.png")
                        : System.IO.Path.Combine(this.QrstGlobe.DataDirectory, @"Icons\ATM_GCP_32_MoreThanThreshold.png");
                }
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个GCP对象
        /// </summary>
        /// <param name="name">GCP对象名称</param>
        /// <param name="y">控制点在原始影像中的Y坐标</param>
        /// <param name="x">控制点在原始影像中的X坐标</param>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        /// <param name="heightAboveSurface">GCP的高度</param>
        public GCP(string name,
            double y,
            double x,
            double latitude,
            double longitude,
            double heightAboveSurface)
            : base(name)
        {
            m_y = y;
            m_x = x;
            //设置经纬度
            m_latitude = latitude;
            m_longitude = longitude;
            //设置高度
            Altitude = heightAboveSurface;
            this.RenderPriority = RenderPriority.GCPs;
            //ZYM-20140108:修改图标大小由内部设定
            Width = 24;
            Height = 24;
        }

        #endregion

        /// <summary>
        /// 初始化控制点
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            //计算当前每一度有多少个格网
            double samplesPerDegree = 50.0 / (drawArgs.WorldCamera.ViewRange.Degrees);
            //计算当前经纬度的海拔系信息
            double elevation = drawArgs.CurrentWorld.TerrainAccessor.GetElevationAt(m_latitude, m_longitude, samplesPerDegree);
            //计算GCP的实际显示高度=夸大因子*Altitude+海拔高度
            double altitude = (World.Settings.VerticalExaggeration * Altitude + World.Settings.VerticalExaggeration * elevation);
            //转换为屏幕向量信息
            Position = MathEngine.SphericalToCartesian(m_latitude, m_longitude, altitude + drawArgs.WorldCamera.WorldRadius);
            //转换到屏幕坐标
            m_positionD = MathEngine.SphericalToCartesianD(
                Angle.FromDegrees(m_latitude),
                Angle.FromDegrees(m_longitude),
                altitude + drawArgs.WorldCamera.WorldRadius);
            //注册主窗口事件
            QrstGlobe.MouseDown += new System.Windows.Forms.MouseEventHandler(QrstGlobe_MouseDown);
            QrstGlobe.MouseUp += new System.Windows.Forms.MouseEventHandler(QrstGlobe_MouseUp);
            IsInitialized = true;
        }

        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void QrstGlobe_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.IsDragging)
            {
                DrawArgs.MouseCursor = CursorType.Arrow;
                m_isDragging = false;
                this.QrstGlobe.IsGCPDragState = false;
                this.QrstGlobe.m_DraggingGCP = null;
                this.IsJustMoved = true;
                this.QrstGlobe.RaiseGCPModifyEvent(
                    new GCPModifyTypeEventArgs(GCPModifyType.Drag, name, Latitude, Longitude));  //上报拖动完毕事件
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void QrstGlobe_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.QrstGlobe == null)
                return;
            //计算当前鼠标点所对应的经纬度信息
            Angle curLat, curLon;
            this.QrstGlobe.DrawArgs.WorldCamera.PickingRayIntersection(
                e.X,
                e.Y,
                out curLat,
                out curLon);

            //if (this.m_gcpType != GCPType.ATMGCP && isMouseOnGCP(curLat.Degrees, curLon.Degrees, e.X, e.Y) && e.Button == System.Windows.Forms.MouseButtons.Left)
            //TODO:20131228，修改为几何控制点和辐射控制点都能够拖动。
            if (isMouseOnGCP(curLat.Degrees, curLon.Degrees, e.X, e.Y) && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                DrawArgs.MouseCursor = CursorType.SizeAll;
                m_isDragging = true;
                this.QrstGlobe.IsGCPDragState = true;
                this.QrstGlobe.m_DraggingGCP = this;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Update(DrawArgs drawArgs)
        {
            if (drawArgs.WorldCamera.ViewMatrix != lastView && drawArgs.CurrentWorld.TerrainAccessor != null && drawArgs.WorldCamera.Altitude < 300000)
            {
                double samplesPerDegree = 50.0 / drawArgs.WorldCamera.ViewRange.Degrees;
                double elevation = drawArgs.CurrentWorld.TerrainAccessor.GetElevationAt(m_latitude, m_longitude, samplesPerDegree);
                double altitude = World.Settings.VerticalExaggeration * Altitude + World.Settings.VerticalExaggeration * elevation;
                Position = MathEngine.SphericalToCartesian(m_latitude, m_longitude,
                    altitude + drawArgs.WorldCamera.WorldRadius);
                lastView = drawArgs.WorldCamera.ViewMatrix;
            }
        }

        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (this.QrstGlobe != null)
            {
                this.QrstGlobe.MouseDown -= new System.Windows.Forms.MouseEventHandler(QrstGlobe_MouseDown);
                this.QrstGlobe.MouseUp -= new System.Windows.Forms.MouseEventHandler(QrstGlobe_MouseUp);
                this.QrstGlobe = null;
            }
            this.SaveFilePath = null;
            this.overlays = null;
            this.m_positionD = null;
            this.m_latitude = double.NaN;
            this.m_latitude = double.NaN;
            this.m_x = double.NaN;
            this.m_y = double.NaN;
            this.Altitude = double.NaN;
            this.TextureFileName = null;
            this.Image = null;
            this.Width = 0;
            this.Height = 0;
            this.MaximumDisplayDistance = double.NaN;
            this.MinimumDisplayDistance = double.NaN;
            this.SelectionRectangle = Rectangle.Empty;
            this.lastView = Matrix.Zero;
        }

        /// <summary>
        /// 重载删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDeleteClick(object sender, EventArgs e)
        {
            if (sender is GCP)
            {
                GCP gcp = sender as GCP;
                gcp.QrstGlobe.RaiseGCPModifyEvent(
                        new GCPModifyTypeEventArgs(GCPModifyType.Delete, gcp.Name, gcp.Latitude, gcp.Longitude));  //上报拖动完毕事件
            }
            base.OnDeleteClick(sender, e);
        }

        /// <summary>
        /// 删除图层
        /// </summary>
        public override void Delete()
        {
            this.ParentList.Remove(this);
        }

        /// <summary>
        /// 设置GCP的位置
        /// </summary>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        public void SetPosition(double latitude, double longitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;

            // 重新计算GCP的坐标，在初始化方法中
            IsInitialized = false;
        }

        /// <summary>
        /// 设置GCP的位置
        /// </summary>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        /// <param name="altitude"> 高度.</param>
        public void SetPosition(double latitude, double longitude, double altitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;
            Altitude = altitude;

            // 重新计算GCP的坐标，在初始化方法中
            IsInitialized = false;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (m_gcpType)
            {
                case GCPType.GeoGCP:
                    return string.Format("几何控制点：{0}", this.name);
                case GCPType.ATMGCP:
                    return string.Format("辐射控制点：{0}", this.name);
                default:
                    return string.Format("控制点：{0}", this.name);
            }
        }

        /// <summary>
        /// 判断鼠标是否在控制点上
        /// 此方法虽然简单，但是存在一定的局限性：
        /// 当三维球放大的到一定程度时，有可能出现大片区域都可能判断为在控制点上的情况。
        /// </summary>
        /// <param name="mouseLat"></param>
        /// <param name="mouseLon"></param>
        /// <returns></returns>
        private bool isMouseOnGCP(double mouseLat, double mouseLon)
        {
            return Math.Abs(mouseLat - m_latitude) < 0.001 && Math.Abs(mouseLon - m_longitude) < 0.001;
        }

        /// <summary>
        /// 判断鼠标是否在控制点上
        /// 较好的方法
        /// 假定在小范围内的经纬度变化和屏幕坐标变化呈线性变化关系，
        /// 那么可以粗略计算出控制点在屏幕坐标上的像素位置，
        /// 根据控制点的像素位置和鼠标位置的差值进行判断更加准确。
        /// </summary>
        /// <param name="mouseLat"></param>
        /// <param name="mouseLon"></param>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <returns></returns>
        private bool isMouseOnGCP(double mouseLat, double mouseLon, int mouseX, int mouseY)
        {
            double centerLat = this.QrstGlobe.DrawArgs.WorldCamera.Latitude.Degrees;
            double centerLon = this.QrstGlobe.DrawArgs.WorldCamera.Longitude.Degrees;
            double centerX = this.QrstGlobe.DrawArgs.ScreenWidth / 2.0;
            double centerY = this.QrstGlobe.DrawArgs.ScreenHeight / 2.0;
            double gcpX = ((m_latitude - centerLat) / (mouseLat - centerLat)) * (mouseX - centerX) + centerX;
            double gcpY = ((m_longitude - centerLon) / (mouseLon - centerLon)) * (mouseY - centerY) + centerY;
            return Math.Abs(gcpX - mouseX) < Width / 2.0 && Math.Abs(gcpY - mouseY) < Height / 2.0;
        }

    }

    /// <summary>
    /// 控制点几何图层类
    /// </summary>
    public class GCPs : RenderableObjectList
    {

        #region  字段

        public override Extension Extension
        {
            get
            {
                Extension ext = new Extension();
                foreach (GCP icon in this.ChildObjects)
                {
                    ext.Include(icon.Extension);
                }
                return ext;
            }
        }
        /// <summary>
        /// 纹理缓存对象
        /// </summary>
        protected Hashtable m_textures = new Hashtable();
        /// <summary>
        /// DirectX绘制控制点GCP对象
        /// </summary>
        protected Sprite m_sprite;
        /// <summary>
        /// 鼠标Hover上去的颜色
        /// </summary>
        static int hotColor = Color.White.ToArgb();
        /// <summary>
        /// 平常时的颜色
        /// </summary>
        static int normalColor = Color.FromArgb(200, 255, 255, 255).ToArgb();
        /// <summary>
        /// 名字的颜色
        /// </summary>
        static int nameColor = Color.White.ToArgb();
        /// <summary>
        /// 描述信息的文字颜色
        /// </summary>
        static int descriptionColor = Color.White.ToArgb();
        /// <summary>
        /// 刷新时间
        /// </summary>
        System.Timers.Timer refreshTimer;
        /// <summary>
        /// 当前鼠标Hover的对象
        /// </summary>
        protected GCP mouseOverGCP;
        /// <summary>
        /// 是否正在更新
        /// </summary>
        bool isUpdating = false;
        /// <summary>
        /// 是否刚添加过一个GCP
        /// </summary>
        bool isAddedOneGCP = false;
        /// <summary>
        /// 原始影像的宽度和高度信息（辐射控制点存储使用）
        /// </summary>
        public string SourceImageWidthAndHeight;
        /// <summary>
        /// 与当前控制点集合相关联的多边形
        /// </summary>
        //public DrawPolygons AssociatedPolygons;

        #endregion

        /// <summary>
        /// 初始化一个GCPs对象
        /// </summary>
        /// <param name="name"></param>
        public GCPs(string name)
            : base(name)
        {

        }

        public GCPs(string name,
            string dataSource,
            TimeSpan refreshInterval,
            World parentWorld,
            Cache cache)
            : base(name, dataSource, refreshInterval, parentWorld, cache)
        {
        }

        /// <summary>
        /// 添加一个GCP对象.
        /// </summary>
        public void AddGCP(GCP gcp)
        {
            Add(gcp);
        }

        /// <summary>
        /// 添加一个GCP对象.
        /// </summary>
        public override void Add(RenderableObject ro)
        {
            m_children.Add(ro);
            IsInitialized = false;
            isAddedOneGCP = true;
        }

        /// <summary>
        /// 初始化控制点集合：1.把所有的控制点读到缓存Hashtable中去,并设置GCP的高宽.2.计算每个GCP的包围范围
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            //判断是否显示此图层，若不显示，则返回
            if (!isOn)
                return;
            //判断绘制GCP的对象，是否存在，若不存在，则创建，若存在，则先释放
            if (m_sprite != null)
            {
                m_sprite.Dispose();
                m_sprite = null;
            }
            m_sprite = new Sprite(drawArgs.device);


            System.TimeSpan smallestRefreshInterval = System.TimeSpan.MaxValue;

            // 添加所有的GCP对象的纹理到缓存中去，并设置GCP的宽与高
            foreach (RenderableObject ro in m_children)
            {
                GCP gcp = ro as GCP;
                if (gcp == null)
                {
                    // 判断当前GCP是否显示，若不显示，则Continue下一个
                    if (ro.IsOn)
                        ro.Initialize(drawArgs);
                    continue;
                }


                if (gcp.RefreshInterval.TotalMilliseconds != 0 && gcp.RefreshInterval != TimeSpan.MaxValue && gcp.RefreshInterval < smallestRefreshInterval)
                    smallestRefreshInterval = gcp.RefreshInterval;

                // 子GCP初始化
                gcp.Initialize(drawArgs);
                object key = null;
                //创建一个GCPeTexture对象
                GCPTexture gcpTexture = null;

                //从文件中读取当前GCP纹理
                if (gcp.TextureFileName != null && gcp.TextureFileName.Length > 0)
                {
                    // 从缓存中读取GCP纹理对象
                    gcpTexture = (GCPTexture)m_textures[gcp.TextureFileName];
                    //若缓存中不存在，则从文件中读取GCP对象
                    if (gcpTexture == null)
                    {
                        key = gcp.TextureFileName;
                        gcpTexture = new GCPTexture(drawArgs.device, gcp.TextureFileName);
                    }
                }
                //从Bitmap对象中读取GCP对象
                else
                {
                    if (gcp.Image != null)
                    {
                        gcpTexture = (GCPTexture)m_textures[gcp.Image];
                        if (gcpTexture == null)
                        {
                            key = gcp.Image;
                            gcpTexture = new GCPTexture(drawArgs.device, gcp.Image);
                        }
                    }
                }
                //若仍然没有纹理，则循环下一个
                if (gcpTexture == null)
                    continue;
                //若有纹理的话
                if (key != null)
                {
                    //把纹理放到缓存中去
                    m_textures.Add(key, gcpTexture);

                    // 设置GCP的宽度
                    if (gcp.Width == 0)
                        gcp.Width = gcpTexture.Width;
                    // 设置GCP的高度
                    if (gcp.Height == 0)
                        gcp.Height = gcpTexture.Height;
                }
            }

            // 计算GCP的包围盒
            foreach (RenderableObject ro in m_children)
            {
                GCP GCP = ro as GCP;
                if (GCP == null)
                    continue;

                if (GetTexture(GCP) == null)
                {
                    //计算文字的范围
                    GCP.SelectionRectangle = drawArgs.DefaultDrawingFont.MeasureString(null, GCP.Name, DrawTextFormat.None, 0);
                }
                else
                {
                    //计算控制点的范围
                    GCP.SelectionRectangle = new Rectangle(0, 0, GCP.Width, GCP.Height);
                }

                // Center the box at (0,0)
                GCP.SelectionRectangle.Offset(-GCP.SelectionRectangle.Width / 2, -GCP.SelectionRectangle.Height / 2);
            }

            if (refreshTimer == null && smallestRefreshInterval != TimeSpan.MaxValue)
            {
                refreshTimer = new System.Timers.Timer(smallestRefreshInterval.TotalMilliseconds);
                refreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(refreshTimer_Elapsed);
                refreshTimer.Start();
            }

            IsInitialized = true;
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <returns></returns>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            //执行所有的子GCP的PerformSelectionAction方法
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                if (!ro.IsSelectable)
                    continue;

                GCP GCP = ro as GCP;
                if (GCP == null)
                {
                    if (ro.PerformSelectionAction(drawArgs))
                        return true;
                    continue;
                }
                //判断当前GCP的位置，是否在视域范围内,若不在则返回
                if (!drawArgs.WorldCamera.ViewFrustum.ContainsPoint(GCP.Position))
                    continue;

                //判断鼠标的位置是否在控制点的包围盒内，若不在，则返回
                Vector3 referenceCenter = new Vector3(
                    (float)drawArgs.WorldCamera.ReferenceCenter.X,
                    (float)drawArgs.WorldCamera.ReferenceCenter.Y,
                    (float)drawArgs.WorldCamera.ReferenceCenter.Z);

                Vector3 projectedPoint = drawArgs.WorldCamera.Project(GCP.Position - referenceCenter);
                if (!GCP.SelectionRectangle.Contains(
                    DrawArgs.LastMousePosition.X - (int)projectedPoint.X,
                    DrawArgs.LastMousePosition.Y - (int)projectedPoint.Y))
                    continue;

                try
                {
                    //若当前鼠标是左键事件
                    if (DrawArgs.IsLeftMouseButtonDown && !DrawArgs.IsRightMouseButtonDown)
                    {
                        //使摄像机设置到控制点设置的范围  ZYM:20130718-禁用摄像机的移动，确保窗口的不变型
                        //if (GCP.OnClickZoomAltitude != double.NaN || GCP.OnClickZoomHeading != double.NaN || GCP.OnClickZoomTilt != double.NaN)
                        //{
                        //    drawArgs.WorldCamera.SetPosition(
                        //        GCP.Latitude,
                        //        GCP.Longitude,
                        //        GCP.OnClickZoomHeading,
                        //        GCP.OnClickZoomAltitude,
                        //        GCP.OnClickZoomTilt);
                        //}

                        //执行当前GCP的PerformSelection方法
                        if (GCP.IsSelectable)
                            GCP.PerformSelectionAction(drawArgs);

                    }
                    //若当前鼠标是右键事件
                    else if (!DrawArgs.IsLeftMouseButtonDown && DrawArgs.IsRightMouseButtonDown)
                    {
                        //不做任何处理
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// 构建图层右键菜单
        /// </summary>
        /// <param name="menu"></param>
        public override void BuildContextMenu(System.Windows.Forms.ContextMenu menu)
        {
            base.BuildContextMenu(menu);
        }

        /// <summary>
        /// 渲染对象：1.渲染所有的非GCP对象；2.渲染所有的非选中状态的GCP对象；3.渲染选中的GCP对象
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;
            //如果新添加了一个控制点，需要重新初始化一次列表
            if (isAddedOneGCP)
            {
                Initialize(drawArgs);
                isAddedOneGCP = false;
            }

            if (!IsInitialized)
                return;

            // 渲染所有的非GCP对象
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                ro.Render(drawArgs);
            }

            int closestGCPDistanceSquared = int.MaxValue;
            GCP closestGCP = null;

            // 开始渲染所有的的GCP图层
            m_sprite.Begin(SpriteFlags.AlphaBlend);
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                GCP GCP = ro as GCP;
                if (GCP == null)
                    continue;

                //计算控制点的位置
                Vector3 translationVector = new Vector3(
                (float)(GCP.PositionD.X - drawArgs.WorldCamera.ReferenceCenter.X),
                (float)(GCP.PositionD.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                (float)(GCP.PositionD.Z - drawArgs.WorldCamera.ReferenceCenter.Z));

                // 计算最近的一个GCP对象
                Vector3 projectedPoint = drawArgs.WorldCamera.Project(translationVector);

                int dx = DrawArgs.LastMousePosition.X - (int)projectedPoint.X;
                int dy = DrawArgs.LastMousePosition.Y - (int)projectedPoint.Y;
                if (GCP.SelectionRectangle.Contains(dx, dy))
                {
                    // Mouse is over, check whether this GCP is closest
                    int distanceSquared = dx * dx + dy * dy;
                    if (distanceSquared < closestGCPDistanceSquared)
                    {
                        closestGCPDistanceSquared = distanceSquared;
                        closestGCP = GCP;
                    }
                }
                //渲染不是被Hover的图层
                if (GCP != mouseOverGCP)
                    Render(drawArgs, GCP, projectedPoint);
            }

            // 渲染被Hover的图层
            if (mouseOverGCP != null)
            {
                Vector3 translationVector = new Vector3(
                    (float)(mouseOverGCP.PositionD.X - drawArgs.WorldCamera.ReferenceCenter.X),
                    (float)(mouseOverGCP.PositionD.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                    (float)(mouseOverGCP.PositionD.Z - drawArgs.WorldCamera.ReferenceCenter.Z));

                Render(drawArgs, mouseOverGCP, drawArgs.WorldCamera.Project(translationVector));
            }

            mouseOverGCP = closestGCP;

            m_sprite.End();
        }

        /// <summary>
        /// 渲染控制点
        /// </summary>
        protected virtual void Render(DrawArgs drawArgs, GCP gcp, Vector3 projectedPoint)
        {
            //判断当前GCP是否被初始化了，比如，若重新定义了控制点的位置，则它的isInitialized就是false
            if (!gcp.IsInitialized)
                gcp.Initialize(drawArgs);
            //判断当前控制点是否在视地范围内，若不在，则不进行绘制
            if (!drawArgs.WorldCamera.ViewFrustum.ContainsPoint(gcp.Position))
                return;

            // 判断控制点是否在最大，最小可见的范围内，若不在，则不进行绘制
            double distanceToGCP = Vector3.Length(gcp.Position - drawArgs.WorldCamera.Position);
            if (distanceToGCP > gcp.MaximumDisplayDistance)
                return;
            if (distanceToGCP < gcp.MinimumDisplayDistance)
                return;
            //获得当前图层的纹理对象
            GCPTexture gcpTexture = GetTexture(gcp);
            //判断是否是MouseOver对象
            bool isMouseOver = gcp == mouseOverGCP;
            //若是MouseOver对象，则绘制Description里的内容
            if (isMouseOver)
            {
                //若是MouseOver对象
                isMouseOver = true;
                //若当前图层可以操作，则设置当前的鼠标是Hand
                if (gcp.IsSelectable)
                {
                    DrawArgs.MouseCursor = DrawArgs.MouseCursor == CursorType.SizeAll ? CursorType.SizeAll : CursorType.Hand;
                }
                //显示文字描述信息,暂时不需要
                string description = string.Format("纬度：{0:f6}°\n经度：{1:f6}°\n", gcp.Latitude, gcp.Longitude);
                //绘制文字信息
                if (description != null)
                {
                    //设置文字的绘制区域
                    DrawTextFormat format = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Bottom;
                    Rectangle rect = Rectangle.FromLTRB(DrawArgs.CurrentMousePosition.X,
                        DrawArgs.CurrentMousePosition.Y,
                        DrawArgs.CurrentMousePosition.X + 200, DrawArgs.CurrentMousePosition.Y + 60);

                    //绘制边框
                    drawArgs.DefaultDrawingFont.DrawText(
                        m_sprite, description,
                        rect,
                        format, 0xb0 << 24);

                    rect.Offset(2, 0);
                    drawArgs.DefaultDrawingFont.DrawText(
                        m_sprite, description,
                        rect,
                        format, 0xb0 << 24);

                    rect.Offset(0, 2);
                    drawArgs.DefaultDrawingFont.DrawText(
                        m_sprite, description,
                        rect,
                        format, 0xb0 << 24);

                    rect.Offset(-2, 0);
                    drawArgs.DefaultDrawingFont.DrawText(
                        m_sprite, description,
                        rect,
                        format, 0xb0 << 24);

                    // 绘制文字信息
                    rect.Offset(1, -1);
                    drawArgs.DefaultDrawingFont.DrawText(
                        m_sprite, description,
                        rect,
                        format, descriptionColor);
                }
            }

            //获取颜色
            int color = isMouseOver ? hotColor : normalColor;
            if (gcpTexture == null || isMouseOver || gcp.NameAlwaysVisible)
            {
                // 绘制控制点的名称
                if (gcp.Name != null)
                {
                    // Render name field
                    const int labelWidth = 1000; // Dummy value needed for centering the text
                    if (gcpTexture == null)
                    {
                        // Center over target as we have no bitmap
                        Rectangle rect = new Rectangle(
                            (int)projectedPoint.X - (labelWidth >> 1),
                            (int)(projectedPoint.Y - (drawArgs.GCPNameFont.Description.Height >> 1)),
                            labelWidth,
                            drawArgs.ScreenHeight);

                        drawArgs.GCPNameFont.DrawText(m_sprite, gcp.Name, rect, DrawTextFormat.Center, color);
                    }
                    else
                    {
                        // Adjust text to make room for GCP
                        int spacing = (int)(gcp.Width * 0.3f);
                        if (spacing > 10)
                            spacing = 10;
                        int offsetForGCP = (gcp.Width >> 1) + spacing;

                        Rectangle rect = new Rectangle(
                            (int)projectedPoint.X + offsetForGCP,
                            (int)(projectedPoint.Y - (drawArgs.GCPNameFont.Description.Height >> 1)),
                            labelWidth,
                            drawArgs.ScreenHeight);

                        drawArgs.GCPNameFont.DrawText(m_sprite, gcp.Name, rect, DrawTextFormat.WordBreak, color);
                    }
                }
            }

            //绘制控制点
            if (gcpTexture != null)
            {
                // Render GCP
                float xscale = (float)gcp.Width / gcpTexture.Width;
                float yscale = (float)gcp.Height / gcpTexture.Height;
                m_sprite.Transform = Matrix.Scaling(xscale, yscale, 0);

                if (gcp.IsRotated)
                    m_sprite.Transform *= Matrix.RotationZ((float)gcp.Rotation.Radians - (float)drawArgs.WorldCamera.Heading.Radians);

                m_sprite.Transform *= Matrix.Translation(projectedPoint.X, projectedPoint.Y, 0);
                m_sprite.Draw(gcpTexture.Texture,
                    new Vector3(gcpTexture.Width >> 1, gcpTexture.Height >> 1, 0),
                    Vector3.Empty,
                    color);

                // Reset transform to prepare for text rendering later
                m_sprite.Transform = Matrix.Identity;
            }
        }

        /// <summary>
        /// 从缓存中获得当前图层的纹理对象，若不存在，则返回null
        /// </summary>
        protected GCPTexture GetTexture(GCP GCP)
        {
            object key = null;

            if (GCP.Image == null)
            {
                key = GCP.TextureFileName;
            }
            else
            {
                key = GCP.Image;
            }
            if (key == null)
                return null;

            GCPTexture res = (GCPTexture)m_textures[key];
            return res;
        }

        /// <summary>
        /// 获取当前控制点图层的中心
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        public void GetGCPsCenter(out double lat, out double lon)
        {
            lat = 0;
            lon = 0;
            foreach (GCP gcp in m_children)
            {
                lat += gcp.Latitude;
                lon += gcp.Longitude;
            }
            lat /= m_children.Count;
            lon /= m_children.Count;
        }

        /// <summary>
        /// 刷新控制点集合图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isUpdating)
                return;
            isUpdating = true;
            try
            {
                for (int i = 0; i < this.ChildObjects.Count; i++)
                {
                    RenderableObject ro = (RenderableObject)this.ChildObjects[i];
                    if (ro != null && ro.IsOn && ro is GCP)
                    {
                        GCP GCP = (GCP)ro;

                        if (GCP.RefreshInterval == TimeSpan.MaxValue || GCP.LastRefresh > System.DateTime.Now - GCP.RefreshInterval)
                            continue;

                        object key = null;
                        GCPTexture GCPTexture = null;

                        if (GCP.TextureFileName != null && GCP.TextureFileName.Length > 0)
                        {
                            // GCP image from file
                            GCPTexture = (GCPTexture)m_textures[GCP.TextureFileName];
                            if (GCPTexture != null)
                            {
                                GCPTexture tempTexture = GCPTexture;
                                m_textures[GCP.SaveFilePath] = new GCPTexture(DrawArgs.Device, GCP.TextureFileName);
                                tempTexture.Dispose();
                            }
                            else
                            {
                                key = GCP.SaveFilePath;
                                GCPTexture = new GCPTexture(DrawArgs.Device, GCP.TextureFileName);

                                // New texture, cache it
                                m_textures.Add(key, GCPTexture);

                                // Use default dimensions if not set
                                if (GCP.Width == 0)
                                    GCP.Width = GCPTexture.Width;
                                if (GCP.Height == 0)
                                    GCP.Height = GCPTexture.Height;
                            }
                        }
                        else
                        {
                            // GCP image from bitmap
                            if (GCP.Image != null)
                            {
                                GCPTexture = (GCPTexture)m_textures[GCP.Image];
                                if (GCPTexture != null)
                                {
                                    GCPTexture tempTexture = GCPTexture;
                                    m_textures[GCP.SaveFilePath] = new GCPTexture(DrawArgs.Device, GCP.Image);
                                    tempTexture.Dispose();
                                }
                                else
                                {
                                    key = GCP.SaveFilePath;
                                    GCPTexture = new GCPTexture(DrawArgs.Device, GCP.Image);

                                    // New texture, cache it
                                    m_textures.Add(key, GCPTexture);

                                    // Use default dimensions if not set
                                    if (GCP.Width == 0)
                                        GCP.Width = GCPTexture.Width;
                                    if (GCP.Height == 0)
                                        GCP.Height = GCPTexture.Height;
                                }
                            }
                        }

                        GCP.LastRefresh = System.DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                isUpdating = false;
            }
        }

        /// <summary>
        /// 释放所有的对象
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (m_textures != null)
            {
                foreach (GCPTexture GCPTexture in m_textures.Values)
                    GCPTexture.Texture.Dispose();
                m_textures.Clear();
            }

            if (m_sprite != null)
            {
                m_sprite.Dispose();
                m_sprite = null;
            }

            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
                refreshTimer = null;
            }
        }

    }

    /// <summary>
    /// 控制点类型
    /// </summary>
    public enum GCPType
    {
        /// <summary>
        /// 几何控制点
        /// </summary>
        GeoGCP,
        /// <summary>
        /// 辐射控制点
        /// </summary>
        ATMGCP,
        /// <summary>
        /// 自定义控制点
        /// </summary>
        Custom
    }

    /// <summary>
    /// 控制点信息修改类型
    /// </summary>
    public enum GCPModifyType
    {
        /// <summary>
        /// 拖动
        /// </summary>
        Drag,
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }

    /// <summary>
    /// 控制点修改类型事件参数
    /// </summary>
    public class GCPModifyTypeEventArgs : EventArgs
    {
        /// <summary>
        /// 获取控制点信息修改类型
        /// </summary>
        public GCPModifyType ModifyType { get; private set; }

        /// <summary>
        /// 获取控制点编号
        /// </summary>
        public string GCPID { get; private set; }

        /// <summary>
        /// 获取控制点的纬度值
        /// </summary>
        public double Lat { get; private set; }

        /// <summary>
        /// 获取控制点的经度值
        /// </summary>
        public double Lon { get; private set; }

        /// <summary>
        /// 初始化一个GCPModifyTypeEventArgs实例
        /// </summary>
        /// <param name="modifyType">控制点修改类型</param>
        /// <param name="gcpID">控制点编号（控制点名称）</param>
        internal GCPModifyTypeEventArgs(GCPModifyType modifyType, string gcpID, double lat, double lon)
        {
            this.ModifyType = modifyType;
            this.GCPID = gcpID;
            this.Lat = lat;
            this.Lon = lon;
        }
    }

}
