using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using System;
using Qrst.Camera;
using Qrst.Menu;
using Qrst;
using Qrst.Net;
using Qrst.Net.Wms;
using Qrst.Interop;
using Qrst.Terrain;
using Qrst.Renderable;
using Qrst.Plugins;
using System.Configuration;
using WorldWind.PluginEngine;
using System.Reflection;
using System.Collections.Generic;
using DrawTools.Plugins;

namespace Qrst
{
    [ToolboxBitmap(typeof(QrstAxGlobeControl), "Resources.QrstAxGlobeControl.png")]
    public partial class QrstAxGlobeControl : UserControl
    {
        #region 私有对象

        private Device m_Device3d;   //Direct 设备对象
        private PresentParameters m_presentParams; //这个对象确定设备向屏幕显示数据的方式
        private DrawArgs drawArgs; //绘制的参数

        private World m_World; //世界 对象
        private Cache m_Cache; //缓存 对象

        private Thread m_WorkerThread;       //工作线程
        private bool m_WorkerThreadRunning;  //工作线程 是否 工作中 ....

        private string _caption = "";
        private string saveScreenShotFilePath; //屏幕截图 保存路径
        private ImageFileFormat saveScreenShotImageFileFormat = ImageFileFormat.Png; //屏幕截图 保存类型

        private bool m_isRenderDisabled; // True when WW isn't active - CPU saver
        private bool isMouseDragging; //是否用鼠标 正在拖动

        private const int positionAlphaStep = 20;
        private int positionAlpha = 255;

        private Point mouseDownStartPosition = Point.Empty; //鼠标开始的位置
        private bool renderWireFrame;

        private TocObjector layerManager = null;//图层管理器
        private string _DataDirectory = "";//配置路径
        private string _cacheDirectory = "";
        private Microsoft.DirectX.Direct3D.Font spatialFont = null;
        private Microsoft.DirectX.Direct3D.Font dcsrsFont = null;
        System.Configuration.Configuration earthConfig = null;


        public PluginCompiler compiler;

        #endregion

        #region 公共属性

        public string CacheDirectory
        {
            get
            {
                return _cacheDirectory;
            }
            set
            {
                _cacheDirectory = value;
            }
        }
        /// <summary>
        /// 配置数据路径
        /// </summary>
        public string DataDirectory
        {
            get
            {
                return _DataDirectory;
            }
            set
            {
                _DataDirectory = value;
            }
        }
        /// <summary>
        /// 获取或设置当前World（星球）
        /// </summary>
        public World CurrentWorld
        {
            get
            {
                return m_World;
            }
            set
            {
                m_World = value;
                if (m_World != null)
                {
                    //创建摄像机对象
                    MomentumCamera camera = new MomentumCamera(m_World.Position, m_World.EquatorialRadius);
                    //设置绘制对象参数（drawArgs）的照相机对象（WorldCamera）为当前摄像机
                    this.drawArgs.WorldCamera = camera;
                    //设置绘制对象参数（drawArgs）的地球对象
                    this.drawArgs.CurrentWorld = value;
                    //创建图层管理器对象
                    layerManager = new TocObjector(m_World, this.DrawArgs);
                    //添加网络格网对象
                    m_World.RenderableObjects.Add(new Renderable.LatLongGrid(m_World));


                    //显示图层管理器//dlf修改
                    World.Settings.showLayerManager = false;
                    //显示格网
                    World.Settings.showLatLonLines = false;
                    //显示位置信息
                    World.Settings.showPosition = true;
                    //显示中间的十字
                    World.Settings.showCrosshairs = true;
                    //隐藏太阳效果
                    World.Settings.EnableSunShading = false;

                    World.Settings.CameraHasInertia = true;//转动地球的时候有惯性
                    World.Settings.CameraSmooth = true;
                    World.Settings.EnableAtmosphericScattering = true;//大气圈散射效果


                }
            }
        }
        /// <summary>
        /// 获取Tocbar
        /// </summary>
        public TocObjector LayerManager
        {
            get
            {
                return layerManager;
            }
        }

        private Globe _qrstGlobe = null;
        public Globe QrstGlobe
        {
            get
            {
                return _qrstGlobe;
            }
            set
            {
                _qrstGlobe = value;
            }
        }
        /// <summary>
        /// 获取或设置Window的Text标题属性
        /// </summary>
        public string Caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                this._caption = value;
            }
        }
        /// <summary>
        /// 获取绘制对象参数对象(DrawArgs)
        /// </summary>
        public DrawArgs DrawArgs
        {
            get { return this.drawArgs; }
        }
        /// <summary>
        /// 获取或设置缓存对象
        /// </summary>
        public Cache Cache
        {
            get
            {
                return m_Cache;
            }
            set
            {
                m_Cache = value;
            }
        }

        /// <summary>
        /// Disables rendering (CPU tick saver)
        /// </summary>
        public bool IsRenderDisabled
        {
            get
            {
                return m_isRenderDisabled;
            }
            set
            {
                m_isRenderDisabled = value;
            }
        }

        #endregion

        public QrstAxGlobeControl()
        {
            InitializeComponent();
        }
        public void GlobeLoad()
        {
            renderWireFrame = false;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            // m_Device3d 不会被创建，除非要绘制的大小是大于1*1的。
            this.Size = new Size(this.Width, this.Height);
            try
            {
                //若不是设计窗体阶段（若window）不在IDE窗体中，则不进行初始化。
                if (!IsInDesignMode())
                    this.InitializeGraphics();

                //初始化m_Device3d设备对象。
                this.drawArgs = new DrawArgs(m_Device3d, this);
                this.drawArgs.screenHeight = this.Height;
                this.drawArgs.screenWidth = this.Width;
                TimeKeeper.Start();
            }
            catch (InvalidCallException caught)
            {
                throw new InvalidCallException(
                    "无法找到可用的图形适配器 . 请确定您使用的是最新版本的DirectX.", caught);
            }
            catch (NotAvailableException caught)
            {
                throw new NotAvailableException(
                    "无法找到可用的图形适配器 . 请确定您使用的是最新版本的DirectX.", caught);
            }
            if (this._DataDirectory == "")
                throw new Exception("请设置地球对象所需的数据路径。");
            if (this.CacheDirectory == "")
                throw new Exception("请设置地球对象的缓存路径。");

            spatialFont = this.drawArgs.CreateFont("微软雅黑", 10, System.Drawing.FontStyle.Bold);
            dcsrsFont = this.drawArgs.CreateFont("Bodoni MT Black", 16, System.Drawing.FontStyle.Bold);

            ExeConfigurationFileMap qrstConfigFile = new ExeConfigurationFileMap();
            qrstConfigFile.ExeConfigFilename = Path.Combine(this.DataDirectory, "QrstGlobe.config");//配置文件路径
            earthConfig = ConfigurationManager.OpenMappedExeConfiguration(qrstConfigFile, ConfigurationUserLevel.None);

            ConfigrationQrstGlobe();//初始化window对象
            InitializeQrstPlanet();//初始化地球对象
            this._qrstGlobe = new Globe(this);

            World newWorld = this.CurrentWorld;
            //添加底图
            if (newWorld.RenderableObjects.ChildObjects.Count > 0)
                newWorld.RenderableObjects.ChildObjects.Insert(newWorld.RenderableObjects.ChildObjects.Count - 1, getURLBlueMarble(newWorld, this.Cache));
            else
                newWorld.RenderableObjects = getURLBlueMarble(newWorld, this.Cache);

            Stars3DLayer starlayer = new Stars3DLayer("Starfield", Path.Combine(this.DataDirectory, "Space\\"), this);
            this.CurrentWorld.RenderableObjects.ChildObjects.Insert(0, starlayer);

            SkyLayer skyLayer = new SkyLayer("天空背景", this);
            this.CurrentWorld.RenderableObjects.ChildObjects.Insert(0, skyLayer);
            //World.Settings.ConvertDownloadedImagesToDds = false;
            Application.Idle += new EventHandler(this.OnApplicationIdle);
            Application.DoEvents();
        }

        /// <summary>
        /// 配置地球控件的属性
        /// </summary>
        private void ConfigrationQrstGlobe()
        {
            DateTime dtNowTime = DateTime.Now;
            DateTime CacheCleanupTime = dtNowTime.AddHours(1);

            //设置缓存的最大，最小容量
            long CacheUpperLimit = (long)Convert.ToInt32(earthConfig.ConnectionStrings.ConnectionStrings["CacheUpperLimit"].ToString()) * 1024L * 1024L * 1024L;//最大
            long CacheLowerLimit = (long)Convert.ToInt32(earthConfig.ConnectionStrings.ConnectionStrings["CacheUpperLimit"].ToString()) * 1024L * 1024L;	//最小容量

            DateTime dtStartTime = DateTime.Now;

            //把缓存信息放到QrstWindow中去。
            this.Cache = new Cache(
                CacheDirectory,//缓存保存的跟目录
                CacheLowerLimit,//缓存文件夹的最高上线
                CacheUpperLimit,//缓存文件夹的最低上线
                CacheCleanupTime - dtNowTime,//缓存保留的时间
                CacheCleanupTime - dtNowTime);//当前开始的事件
            //是否有log404错误。
            Qrst.Net.WebDownload.Log404Errors = World.Settings.Log404Errors;
            this.BackColor = Color.AliceBlue;
        }
        /// <summary>
        /// 配置地球对象
        /// </summary>
        private void InitializeQrstPlanet()
        {

            #region 创建数字高程图层

            World.Settings.VerticalExaggeration = 3.0f;//夸大因子
            //地形区域的范围：这里只显示中国区域的地形图（DEM数据太大，所有没放全国的）
            double north = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["DEM_North"].ToString());
            double south = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["DEM_South"].ToString());
            double west = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["DEM_West"].ToString());
            double east = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["DEM_East"].ToString());
            //DEM图层的名称
            string terrainAccessorName = "World DEM";
            //DEM图层的ServerUri
            string serverUrl = null;
            //放在服务器端的DEM数据集合的名称
            string dataSetName = null;
            //第一层的每个格网的度数
            double levelZeroTileSizeDegrees = double.NaN;
            //总共有多少层
            uint numberLevels = 0;
            //每一个切片有多少个采样点
            uint samplesPerTile = 0;
            //数据的值类型
            string dataFormat = null;
            //高程文件的后缀名
            string fileExtension = null;
            serverUrl = earthConfig.ConnectionStrings.ConnectionStrings["DEM_Url"].ToString();
            dataSetName = earthConfig.ConnectionStrings.ConnectionStrings["DEM_Dataset"].ToString();
            levelZeroTileSizeDegrees = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["DEM_levelZeroTileSizeDegrees"].ToString());
            numberLevels = (uint)Convert.ToInt32(earthConfig.ConnectionStrings.ConnectionStrings["DEM_numberLevels"].ToString());
            samplesPerTile = 150;
            dataFormat = "Int16";
            fileExtension = "bil";
            string terrainLayerName = earthConfig.ConnectionStrings.ConnectionStrings["DEM_LayerName"].ToString();
            //创建一个高程服务对象
            TerrainTileService tts = new TerrainTileService(
                        serverUrl,
                        dataSetName,
                        levelZeroTileSizeDegrees,
                        (int)samplesPerTile,
                        fileExtension,
                        (int)numberLevels,
                        Path.Combine(this.CacheDirectory, terrainLayerName),
                        World.Settings.TerrainTileRetryInterval,
                        dataFormat);
            //创建一个高程图层
            TerrainAccessor newTerrainAccessor = new NltTerrainAccessor(
                        terrainAccessorName,
                        west,
                        south,
                        east,
                        north,
                        tts,
                        null);

            #endregion

            #region 创建地球对象

            //新建地球对象
            string worldName = "Earth";//地球对象的名称
            double equatorialRadius = 6378137.0;//地球的半径
            World newWorld = new World(
                        worldName,
                        new Microsoft.DirectX.Vector3(0, 0, 0),
                        new Microsoft.DirectX.Quaternion(0, 0, 0, 0),
                        equatorialRadius,
                        this.Cache.CacheDirectory,
                        newTerrainAccessor
                        );
            //把新建的这个地球对象赋予当前的QrstWindow对象
            newWorld.Position = new Vector3(30, 110, 0);
            this.CurrentWorld = newWorld;

            #endregion
        }

        /// <summary>
        /// 获取全球12月BlueMarble底图
        /// </summary>
        /// <param name="parentWorld"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        private RenderableObjectList getBlueMarble(World parentWorld, Cache cache)
        {
            try
            {
                //Blue Marble 切片图层
                RenderableObjectList parentRenderable = new RenderableObjectList("Blue Marble Cache");
                parentRenderable.ParentList = parentWorld.RenderableObjects;
                //是否显示此图层
                parentRenderable.IsOn = false;
                parentRenderable.DisableExpansion = false;
                parentRenderable.ShowOnlyOneLayer = false;
                //存储QrstWindow的主要对象
                parentRenderable.MetaData.Add("XmlSource", this.DataDirectory);//着重看	
                parentRenderable.MetaData.Add("World", parentWorld);//着重看
                parentRenderable.MetaData.Add("Cache", cache);//着重看
                //此图层做为表面图层
                parentRenderable.RenderPriority = Qrst.Renderable.RenderPriority.SurfaceImages;
                //图层的空间信息，范围信息，高程信息
                string name = "BlueMarble";
                double distanceAboveSurface = 0.0;
                double north = 90;
                double south = -90;
                double west = -180;
                double east = 180;
                //是否显示数字高程
                bool terrainMapped = true;
                TimeSpan dataExpiration = TimeSpan.MaxValue;
                ImageStore[] imageStores = new ImageStore[1];
                double levelZeroTileSizeDegrees = 36;//切片的第0层的度数，也就是到多少度的时候，开始显示
                int numberLevels = 5;//总共有几层。超过了这个数，就不再找更高层的切片数据了。
                string imageFileExtension = "jpg";//切片的后缀名
                string permanentDirectory = Path.Combine(this.DataDirectory, @"Earth\bmng.topo.bathy.200412");//数据源路径
                TimeSpan dataExpirationTiles = TimeSpan.MaxValue;
                string cacheDir = Path.Combine(this.DataDirectory, @"Earth\bmng.topo.bathy.200412");//缓存路径
                byte opacity = 255;//透明色

                //切片图层对象
                ImageStore ia = new ImageStore();
                ia.DataDirectory = permanentDirectory;
                ia.LevelZeroTileSizeDegrees = levelZeroTileSizeDegrees;
                ia.LevelCount = numberLevels;
                ia.ImageExtension = imageFileExtension;
                ia.CacheDirectory = cacheDir;

                //下载时的图标
                imageStores[0] = ia;
                imageStores[0].ServerLogo = Path.Combine(this.DataDirectory, @"Icons\dcsrsdcsf.png");
                //切片对象
                QuadTileSet qts = null;
                qts = new QuadTileSet
                (
                    name,
                    parentWorld,
                    distanceAboveSurface,

                    north,
                    south,
                    west,
                    east,
                    terrainMapped,
                    imageStores
                );
                qts.CacheExpirationTime = dataExpiration;

                //透明色
                System.Drawing.Color c = Color.FromArgb(opacity, 0, 0, 0);//Color [A=255, R=0, G=0, B=0]
                qts.ColorKey = c.ToArgb();

                qts.ParentList = parentRenderable;
                qts.IsOn = true;
                qts.MetaData.Add("XmlSource", (string)parentRenderable.MetaData["XmlSource"]);
                parentRenderable.Add(qts);

                //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
                Qrst.Renderable.ImageLayer m_BlueMarbleBase = new Qrst.Renderable.ImageLayer
                    (
                    "Blue Marble",
                    parentWorld,
                    0,
                    null,
                    -90, 90, -180, 180, 1.0f, parentWorld.TerrainAccessor
                    );
                //底图的路径
                m_BlueMarbleBase.ImagePath = Path.Combine(this.DataDirectory, @"Earth\bmng.topo.bathy.200412\bmng.topo.bathy.200412.jpg");

                RenderableObjectList renderableCollection = new RenderableObjectList("世界地图");
                renderableCollection.Add(m_BlueMarbleBase);
                renderableCollection.Add(parentRenderable);
                return renderableCollection;

            }
            catch
            {
                return null;
            }
        }

        private RenderableObjectList getURLBlueMarble(World parentWorld, Cache cache)
        {
            //Blue Marble 切片图层
            RenderableObjectList parentRenderable = new RenderableObjectList("Blue Marble Cache");
            parentRenderable.ParentList = parentWorld.RenderableObjects;
            //是否显示此图层
            parentRenderable.IsOn = true;
            parentRenderable.DisableExpansion = false;
            parentRenderable.ShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象	
            parentRenderable.MetaData.Add("World", parentWorld);//着重看
            parentRenderable.MetaData.Add("Cache", cache);//着重看
            //此图层做为表面图层
            parentRenderable.RenderPriority = Qrst.Renderable.RenderPriority.SurfaceImages;
            //图层的空间信息，范围信息，高程信息
            string name = "BlueMarble";
            double distanceAboveSurface = 0.0;
            double north = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_North"].ToString());
            double south = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_South"].ToString());
            double west = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_West"].ToString());
            double east = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_East"].ToString());
            //是否显示数字高程
            bool terrainMapped = true;

            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            double levelZeroTileSizeDegrees = Convert.ToDouble(earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_levelZeroTileSizeDegrees"].ToString()); ;//切片的第0层的度数，也就是到多少度的时候，开始显示
            int numberLevels = Convert.ToInt32(earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_numberLevels"].ToString());//总共有几层。超过了这个数，就不再找更高层的切片数据了。
            string imageFileExtension = "jpg";//切片的后缀名
            string layerName = earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_LayerName"].ToString();
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;
            string cacheDir = Path.Combine(this.CacheDirectory, layerName);//缓存路径
            byte opacity = 255;//透明色
            string dataSetName = earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_Dataset"].ToString();
            string serverUrl = earthConfig.ConnectionStrings.ConnectionStrings["BlueMarble_Url"].ToString();

            ImageStore ia = new NltImageStore(dataSetName, serverUrl);
            ia.DataDirectory = null;
            ia.LevelZeroTileSizeDegrees = levelZeroTileSizeDegrees;
            ia.LevelCount = numberLevels;
            ia.ImageExtension = imageFileExtension;
            ia.CacheDirectory = cacheDir;
            ia.ServerLogo = Path.Combine(this.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                parentWorld,
                distanceAboveSurface,

                north,
                south,
                west,
                east,
                terrainMapped,
                imageStores
            );
            qts.CacheExpirationTime = dataExpiration;
            qts.ServerLogoFilePath = ia.ServerLogo;
            //透明色
            System.Drawing.Color c = Color.FromArgb(opacity, 0, 0, 0);//Color [A=255, R=0, G=0, B=0]
            qts.ColorKey = c.ToArgb();

            qts.ParentList = parentRenderable;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)parentRenderable.MetaData["XmlSource"]);
            parentRenderable.Add(qts);

            //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
            Qrst.Renderable.ImageLayer m_BlueMarbleBase = new Qrst.Renderable.ImageLayer
                (
                "Blue Marble",
                parentWorld,
                0,
                null,
                -90, 90, -180, 180, 1.0f, parentWorld.TerrainAccessor
                );
            //底图的路径
            m_BlueMarbleBase.ImagePath = Path.Combine(this.DataDirectory, @"Earth\bmng.topo.bathy.200412.jpg");

            RenderableObjectList renderableCollection = new RenderableObjectList("世界地图");
            renderableCollection.Add(m_BlueMarbleBase);
            renderableCollection.Add(parentRenderable);
            return renderableCollection;
        }


        private void InitializePluginCompiler()
        {
            compiler = new PluginCompiler(this, DirectoryPath);

            //#if DEBUG
            // Search for plugins in worldwind.exe (plugin development/debugging aid)
            compiler.FindPlugins(Assembly.GetExecutingAssembly());
            //#endif

            compiler.FindPlugins();
            compiler.LoadStartupPlugins();
        }

        #region Public methods
        public void ViewInitial()
        {
            this.QrstGlobe.Goto(30.0, 110.0, 12756300.0);
        }

        public void ClearSearchResultExtents()
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "tmpDrawExtentsLayer")
                {
                    delyr = lyr as DrawExtentsLayer;
                }
            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer("tmpDrawExtentsLayer", Color.FromArgb(50, 0, 255, 0), m_World, DrawArgs);
                m_World.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }

            delyr.IsOn = true;
            List<RectangleF> extents = new List<RectangleF>();
            delyr.UpdateExtents(extents);
        }
        public void ClearPoly(bool isDraw)
        {
            DrawPolygonLayer polylyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "tmpDrawPolygonLyr1")
                {
                    polylyr = lyr as DrawPolygonLayer;
                }
            }
            if (!isDraw && polylyr != null)
            {
                PluginInfo p=null;
                foreach (PluginInfo varPi in this.compiler.Plugins)
                {
                    if (varPi.Plugin != null && varPi.Plugin is DrawTools.Plugins.DrawPolygonTool)
                    {
                        varPi.Plugin.PluginUnload();
                        p = varPi;
                        break;
                    }
                }
                this.compiler.Plugins.Remove(p);
            }
                //m_World.RenderableObjects.Remove(polylyr);
                //polylyr.IsOn = false;
        }
        public void DrawSearchResultExtents(List<RectangleF> extents)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "tmpDrawExtentsLayer")
                {
                    delyr = lyr as DrawExtentsLayer;
                }
            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer("tmpDrawExtentsLayer", Color.FromArgb(50, 0, 255, 0), m_World, DrawArgs);
                m_World.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }

            // delyr.IsOn = true;
            delyr.UpdateExtents(extents);
        }

        public void DrawShapeFile(string shapefile, Color color, float linwidth, double maxAltitude, double minAltitude)
        {
            DrawShapefileLayer delyr = null;


            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == Path.GetFileNameWithoutExtension(shapefile))
                {
                    delyr = lyr as DrawShapefileLayer;

                }
            }


            if (delyr == null)
            {
                delyr = new DrawShapefileLayer(Path.GetFileNameWithoutExtension(shapefile), color, linwidth, m_World, DrawArgs, maxAltitude, minAltitude);

                m_World.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }

            delyr.IsOn = true;
            delyr.OpenShapefile(shapefile);

        }

        public void DrawSearchResultExtents(Dictionary<System.Drawing.RectangleF, int> extents, out int maxNum)
        {

            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "tmpDrawExtentsLayer1")
                {
                    delyr = lyr as DrawExtentsLayer;
                }
            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer("tmpDrawExtentsLayer1", Color.FromArgb(50, 0, 255, 0), m_World, DrawArgs);
                m_World.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }

            delyr.IsOn = true;
            maxNum = delyr.AddExtents(extents);
        }

        public void DrawPolygonLayer(Point3d fromPoint, Point3d toPoint)
        {
            DrawRectangleLayer polygenlyr = null;
            DrawPolygonLayer polylyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "tmpDrawPolylineLyr")
                {
                    polygenlyr = lyr as DrawRectangleLayer;
                }
                else if (lyr.Name == "tmpDrawPolygonLyr1")
                {
                    polylyr = lyr as DrawPolygonLayer;
                }
            }
            if (polygenlyr == null)
            {
                polygenlyr = new DrawRectangleLayer("tmpDrawPolylineLyr", Color.FromArgb(50, 255, 0, 0), m_World, DrawArgs);
                m_World.RenderableObjects.Add(polygenlyr);
            }
            else if (polylyr == null)
            {
                polylyr = new DrawPolygonLayer("tmpDrawPolygonLyr1", Color.FromArgb(50, 255, 0, 0), m_World, DrawArgs);
                m_World.RenderableObjects.Add(polylyr);
            }
            polygenlyr.IsOn = true;
            polygenlyr.DrawRectangle(fromPoint, toPoint);

        }


        //添加一个临时图层,并将矩形框绘制到该图层上   zxw 20130901
        public void DrawSelectedExtents(List<RectangleF> extents)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "tmpSelectedDrawExtentsLayer")
                {
                    delyr = lyr as DrawExtentsLayer;
                }

            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer("tmpSelectedDrawExtentsLayer", Color.FromArgb(50, 255, 0, 0), m_World, DrawArgs);
                m_World.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }

            delyr.IsOn = true;
            delyr.UpdateExtents(extents);
        }
        public void DrawCheckedExtents(List<RectangleF> extents)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "tmpCheckedDrawExtentsLayer")
                {
                    delyr = lyr as DrawExtentsLayer;
                }

            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer("tmpCheckedDrawExtentsLayer", Color.FromArgb(0x06, 0x00, 0xFF), m_World, DrawArgs);
                m_World.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }

            delyr.IsOn = true;
            delyr.UpdateExtents(extents);
        }
        /// <summary>
        /// 控制图层显示
        /// </summary>
        /// <param name="layerName">图层名</param>
        /// <param name="_isOn">是否显示</param>
        public void LayerControl(string layerName, bool _isOn)
        {
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    lyr.IsOn = _isOn;
                    break;
                }

            }
        }

        /// <summary>
        /// 判断某个图层是否显示
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public bool IsOn(string layerName)
        {
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    return lyr.IsOn;
                }

            }
            return false;
        }



        /// <summary>
        /// 将屏幕坐标转换为球体上的经纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double[] ConvertScreenPos2LatAndLon(int x, int y)
        {
            Angle ax, ay;
            double[] pos = new double[2];
            for (int i = 0; i < m_World.RenderableObjects.ChildObjects.Count; i++)
            {
                if (m_World.RenderableObjects.ChildObjects[i] is DrawBaseLayer)
                {
                    ((DrawBaseLayer)m_World.RenderableObjects.ChildObjects[i]).drawArgs.WorldCamera.PickingRayIntersection(x, y, out ax, out ay);
                    pos[0] = ax.Degrees;
                    pos[1] = ay.Degrees;
                    return pos;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取矩阵选框的最大最小经纬度范围
        /// </summary>
        /// <returns>最大最小经纬度范围</returns>
        public double[] GetSelectRectangle()
        {
            double[] rectangle = new double[4] { 999, 999, 999, 999 };
            foreach (PluginInfo varPi in this.compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawTools.Plugins.DrawRectangleTool)
                {
                    rectangle[0] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[0].X;
                    rectangle[1] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[0].Y;
                    rectangle[2] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[2].X;
                    rectangle[3] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[2].Y;
                }
            }
            return rectangle;
        }
        DrawPolygonLayer layer;
        public bool needRefreshDrawPolyLayer = true;
        public void DrawPoly(Angle lat, Angle lon)
        {
            if (needRefreshDrawPolyLayer)
            {
                foreach (PluginInfo varPi in this.compiler.Plugins)
                {
                    layer = ((DrawPolygonTool)varPi.Plugin).drawLayer;
                }
                if (layer.PointList != null && layer.PointList.Count != 0)
                {
                    layer.PointList.Clear();
                }
                if (layer.VertexList != null && layer.VertexList.Count != 0)
                {
                    layer.VertexList.Clear();
                }
                needRefreshDrawPolyLayer = false;
            }
            layer.DrawPoint(lat, lon);
        }
        public void CompletePoly(Angle lat, Angle lon)
        {
            layer.completeDraw(lat, lon);
        }
        public Point3d GetPolyPoint()
        {
            Point3d pt3 = new Point3d();
            foreach (PluginInfo varPi in this.compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawTools.Plugins.DrawPolygonTool)
                {  
                    pt3 = (varPi.Plugin as DrawPolygonTool).drawLayer.pt3d;
                }
            }
            return pt3;
        }
        ///// <summary>
        ///// 获取多边形点
        ///// </summary>
        ///// <returns></returns>
        //public List<double> GetPolygonPoint()
        //{
        //    List<double> list = new List<double>();
        //    foreach (PluginInfo varPi in this.compiler.Plugins)
        //    {
        //        if (varPi.Plugin != null && varPi.Plugin is DrawTools.Plugins.DrawPolygonTool)
        //        {
        //            List<Point3d> listPolygonPoint = new List<Point3d>();
        //            listPolygonPoint = (varPi.Plugin as DrawPolygonTool).drawLayer.PointList;
        //            for (int i = 0; i < listPolygonPoint.Count - 1; i++)
        //            {
        //                list.Add(listPolygonPoint[i].X);
        //                list.Add(listPolygonPoint[i].Y);
        //            }
        //        }
        //    }
        //    return list;
        //}

        public void DrawRefPolygon()
        {

        }
        public event EventHandler OnDrawRectangleCompeleted;
        public event EventHandler OnDrawPolygOnCompleted;
        public event EventHandler OnPolyUp;
        /// <summary>
        /// 使用矩阵选框工具
        /// </summary>
        bool isDrawRect = false;
        public void UsingDrawRectangleTool()
        {
            isDrawRect = true;
            //获取插件编译器
            if (this.compiler == null)
            {
                InitializePluginCompiler();
            }

            //读取矩形绘制工具 
            bool noDptool = true;

            foreach (PluginInfo varPi in this.compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawTools.Plugins.DrawRectangleTool)
                {
                    if (varPi.Plugin.IsLoaded)
                    {
                        //Tool清空当前使用工具
                        if (World.Settings.CurrentWwTool == varPi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }

                        ((DrawRectangleTool)varPi.Plugin).OnCompleted -= new EventHandler(QrstAxGlobeControl_OnDrawRectangleCompeleted);
                        varPi.Plugin.PluginUnload();
                        this.compiler.Plugins.Remove(varPi);
                    }
                    else
                    {
                        varPi.Plugin.PluginLoad(this, DirectoryPath);
                        World.Settings.CurrentWwTool = varPi.Plugin;
                        ((DrawRectangleTool)varPi.Plugin).OnCompleted += new EventHandler(QrstAxGlobeControl_OnDrawRectangleCompeleted);
                    }
                    noDptool = false;
                    break;
                }
                else if (isDrawPloy)
                {
                    //Tool清空当前使用工具
                    World.Settings.CurrentWwTool = null;
                    varPi.Plugin.PluginUnload();
                    this.compiler.Plugins.Remove(varPi);
                    break;
                }
            }

            //如果编译器里没有该工具 则创建之 并加载
            if (noDptool)
            {
                PluginInfo pi = new PluginInfo();
                pi.Plugin = new DrawTools.Plugins.DrawRectangleTool();
                pi.Name = typeof(DrawTools.Plugins.DrawRectangleTool).Name;
                pi.Description = "DrawRectangleTool.";
                this.compiler.Plugins.Add(pi);
                pi.Plugin.PluginLoad(this, DirectoryPath);
                World.Settings.CurrentWwTool = pi.Plugin;
                ((DrawRectangleTool)pi.Plugin).OnCompleted += new EventHandler(QrstAxGlobeControl_OnDrawRectangleCompeleted);
            }
        }
        public bool flag = false;
        public void UsingDrawPloygonTool()
        {
            //获取插件编译器
            if (this.compiler == null)
            {
                InitializePluginCompiler();
            }
            isDrawPloy = true;
            //读取绘制工具 
            bool noDptool = true;

            foreach (PluginInfo varPi in this.compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawTools.Plugins.DrawPolygonTool)
                {
                    if (!varPi.Plugin.IsLoaded)
                    {
                        varPi.Plugin.PluginLoad(this, DirectoryPath);
                        World.Settings.CurrentWwTool = varPi.Plugin;
                        ((DrawPolygonTool)varPi.Plugin).OnUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)varPi.Plugin).OnCompleted += new EventHandler(QrstAxGlobeControl_OnDrawPolygOnCompleted);
                    }
                    noDptool = false;
                    break;
                }
                else if (isDrawRect)
                {
                    World.Settings.CurrentWwTool = null;
                    varPi.Plugin.PluginUnload();
                    this.compiler.Plugins.Remove(varPi);
                    break;
                }
            }

            //如果编译器里没有该工具 则创建之 并加载
            if (noDptool)
            {
                PluginInfo pi = new PluginInfo();
                pi.Plugin = new DrawTools.Plugins.DrawPolygonTool();
                pi.Name = typeof(DrawTools.Plugins.DrawPolygonTool).Name;
                pi.Description = "DrawPolygonTool.";
                this.compiler.Plugins.Add(pi);
                pi.Plugin.PluginLoad(this, DirectoryPath);
                World.Settings.CurrentWwTool = pi.Plugin;
                ((DrawPolygonTool)pi.Plugin).OnUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                ((DrawPolygonTool)pi.Plugin).OnCompleted += new EventHandler(QrstAxGlobeControl_OnDrawPolygOnCompleted);
            }
        }

        bool isDrawPloy = false;
        public void TurnOnOffDrawPloygonTool()
        {
            //获取插件编译器
            if (this.compiler == null)
            {
                InitializePluginCompiler();
            }
            isDrawPloy = true;
            //读取绘制工具 
            bool noDptool = true;
            if (flag)
            {
                foreach (PluginInfo varPi in this.compiler.Plugins)
                {
                    if (varPi.Plugin.IsLoaded)
                    {
                        //Tool清空当前使用工具
                        if (World.Settings.CurrentWwTool == varPi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        ((DrawPolygonTool)varPi.Plugin).OnUp -= new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)varPi.Plugin).OnCompleted -= new EventHandler(QrstAxGlobeControl_OnDrawPolygOnCompleted);
                        varPi.Plugin.PluginUnload();
                        this.compiler.Plugins.Remove(varPi);
                        this.compiler.Plugins.Clear();
                        flag = false;
                        break;
                    }

                }
            }
            foreach (PluginInfo varPi in this.compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawTools.Plugins.DrawPolygonTool)
                {
                    if (varPi.Plugin.IsLoaded)
                    {
                        //Tool清空当前使用工具
                        if (World.Settings.CurrentWwTool == varPi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        ((DrawPolygonTool)varPi.Plugin).OnUp -= new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)varPi.Plugin).OnCompleted -= new EventHandler(QrstAxGlobeControl_OnDrawPolygOnCompleted);
                        varPi.Plugin.PluginUnload();
                        this.compiler.Plugins.Remove(varPi);
                    }
                    else
                    {
                        varPi.Plugin.PluginLoad(this, DirectoryPath);
                        World.Settings.CurrentWwTool = varPi.Plugin;
                        ((DrawPolygonTool)varPi.Plugin).OnUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)varPi.Plugin).OnCompleted += new EventHandler(QrstAxGlobeControl_OnDrawPolygOnCompleted);
                    }
                    noDptool = false;
                    break;
                }
                else if (isDrawRect)
                {
                    //Tool清空当前使用工具
                    World.Settings.CurrentWwTool = null;
                    varPi.Plugin.PluginUnload();
                    this.compiler.Plugins.Remove(varPi);
                    break;
                }
            }
            //如果编译器里没有该工具 则创建之 并加载
            if (noDptool)
            {
                PluginInfo pi = new PluginInfo();
                pi.Plugin = new DrawTools.Plugins.DrawPolygonTool();
                pi.Name = typeof(DrawTools.Plugins.DrawPolygonTool).Name;
                pi.Description = "DrawPolygonTool.";
                this.compiler.Plugins.Add(pi);
                pi.Plugin.PluginLoad(this, DirectoryPath);
                World.Settings.CurrentWwTool = pi.Plugin;
                ((DrawPolygonTool)pi.Plugin).OnUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                ((DrawPolygonTool)pi.Plugin).OnCompleted += new EventHandler(QrstAxGlobeControl_OnDrawPolygOnCompleted);
            }
        }


        void QrstAxGlobeControl_OnDrawRectangleCompeleted(object sender, EventArgs e)
        {
            if (OnDrawRectangleCompeleted != null)
            {
                OnDrawRectangleCompeleted(sender, e);
            }
        }
        void QrstAxGlobeControl_OnPolyUp(object sender, EventArgs e)
        {
            if (OnPolyUp != null)
            {
                OnPolyUp(sender,e);
            }
        }
        void QrstAxGlobeControl_OnDrawPolygOnCompleted(object sender, EventArgs e)
        {
            if (OnDrawPolygOnCompleted != null)
            {
                OnDrawPolygOnCompleted(sender, e);
            }
        }

        /// <summary>
        /// 移动地球的方法
        /// </summary>
        /// <param name="latitude">Latitude in degrees of target position. (-90 - 90).</param>
        /// <param name="longitude">Longitude in degrees of target position. (-180 - 180).</param>
        /// <param name="heading">Camera heading in degrees (0-360) or double.NaN for no change.</param>
        /// <param name="altitude">Camera altitude in meters or double.NaN for no change.</param>
        /// <param name="perpendicularViewRange"></param>
        /// <param name="tilt">Camera tilt in degrees (-90 - 90) or double.NaN for no change.</param>
        public void GotoLatLon(double latitude, double longitude, double heading, double altitude, double perpendicularViewRange, double tilt)
        {
            if (!double.IsNaN(perpendicularViewRange))
                altitude = m_World.EquatorialRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
            if (altitude < 1)
                altitude = 1;
            this.drawArgs.WorldCamera.SetPosition(latitude, longitude, heading, altitude, tilt);
        }

        public void GotoLatLon(double latitude, double longitude)
        {
            this.drawArgs.WorldCamera.SetPosition(latitude, longitude,
                this.drawArgs.WorldCamera.Heading.Degrees,
                this.drawArgs.WorldCamera.Altitude,
                this.drawArgs.WorldCamera.Tilt.Degrees);
        }

        public void GotoLatLonAltitude(double latitude, double longitude, double altitude)
        {
            this.drawArgs.WorldCamera.SetPosition(latitude, longitude,
                this.drawArgs.WorldCamera.Heading.Degrees,
                altitude,
                this.drawArgs.WorldCamera.Tilt.Degrees);
        }

        public void GotoLatLonHeadingViewRange(double latitude, double longitude, double heading, double perpendicularViewRange)
        {
            double altitude = m_World.EquatorialRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
            this.GotoLatLonHeadingAltitude(latitude, longitude, heading, altitude);
        }

        public void GotoLatLonViewRange(double latitude, double longitude, double perpendicularViewRange)
        {
            double altitude = m_World.EquatorialRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
            this.GotoLatLonHeadingAltitude(latitude, longitude, this.drawArgs.WorldCamera.Heading.Degrees, altitude);
        }

        public void GotoLatLonHeadingAltitude(double latitude, double longitude, double heading, double altitude)
        {
            this.drawArgs.WorldCamera.SetPosition(latitude, longitude,
                heading,
                altitude,
                this.drawArgs.WorldCamera.Tilt.Degrees);
        }

        /// <summary>
        /// Saves the current view to file.
        /// </summary>
        /// <param name="filePath">Path and filename of output file.  
        /// Extension is used to determine the image format.</param>
        public void SaveScreenshot(string filePath)
        {
            if (m_Device3d == null)
                return;

            FileInfo saveFileInfo = new FileInfo(filePath);
            string ext = saveFileInfo.Extension.Replace(".", "");
            try
            {
                this.saveScreenShotImageFileFormat = (ImageFileFormat)Enum.Parse(typeof(ImageFileFormat), ext, true);
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Unknown file type/file extension for file '" + filePath + "'.  Unable to save.");
            }

            if (!saveFileInfo.Directory.Exists)
                saveFileInfo.Directory.Create();

            this.saveScreenShotFilePath = filePath;
        }

        /// <summary>
        /// Borrowed from FlightGear and Tom Miller's blog
        /// </summary>
        public void OnApplicationIdle(object sender, EventArgs e)
        {
            try
            {
                //if (Parent.Focused && !Focused)
                //    Focus();

                while (IsAppStillIdle)
                {
                    if (!World.Settings.AlwaysRenderWindow && m_isRenderDisabled && !World.Settings.CameraHasMomentum)
                        return;
                    Render();
                    // Flip
                    drawArgs.Present();
                }

            }
            catch (DeviceLostException)
            {
                AttemptRecovery();
            }
            catch
            {
            }
        }

        #endregion

        /// <summary>
        /// 确定是否有任何窗口排队的信息 .
        /// </summary>
        private bool IsAppStillIdle
        {
            get
            {
                Qrst.Interop.NativeMethods.Message msg;
                return !Qrst.Interop.NativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }


        /// <summary>
        /// 当窗体进行重新绘制时所并且m_isRenderDisabled为True时，所触发的事件 .
        /// 其他的绘制是在 WndProc 里面进行处理的 .
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {

            //绘制最后一个活动的场景，使用户界面保持响应
            try
            {
                if (m_Device3d == null)
                {
                    e.Graphics.Clear(this.BackColor);
                    return;
                }

                //渲染
                Render();
            }
            //若绘制发生错误的话，则尝试修复，并强制重新绘制
            catch (DeviceLostException)
            {
                try
                {
                    AttemptRecovery();
                    // 强制重新绘制
                    Render();
                    m_Device3d.Present();
                }
                catch (DirectXException)
                {
                }
            }
        }

        System.Collections.ArrayList m_FrameTimes = new ArrayList();

        /// <summary>
        /// 开始绘制画板。
        /// </summary>
        public void Render()
        {
            //获得当前的开始时间
            long startTicks = 0;
            PerformanceTimer.QueryPerformanceCounter(ref startTicks);
            try
            {
                //开始进行准备绘制
                this.drawArgs.BeginRender();
                //绘制地球的背景色，这里为黑色
                System.Drawing.Color backgroundColor = System.Drawing.Color.Black;
                m_Device3d.Clear(ClearFlags.Target | ClearFlags.ZBuffer, backgroundColor, 1.0f, 0);
                //判断工作线程是否为空
                if (m_WorkerThread == null)
                {
                    //设置工作线程锁
                    m_WorkerThreadRunning = true;
                    //新建一个工作线程
                    m_WorkerThread = new Thread(new ThreadStart(WorkerThread));
                    //工作线程名称
                    m_WorkerThread.Name = "Qrst.Window.WorkerThread";
                    //设置为后台线程
                    //前台线程与后台线程的区别：.NET Framework 中的所有线程都被指定为前台线程或后台线程。这两种线程唯一的区别是 — 后台线程不会阻止进程终止。在属于一个进程的所有前台线程终止之后，公共语言运行库 (CLR) 就会结束进程，从而终止仍在运行的任何后台线程。
                    m_WorkerThread.IsBackground = true;
                    //设置线程的优先级别：若是在BelowNormal的状态下，则会使绘制的更加平滑，但是在性能好的机器上会特别的慢。
                    if (World.Settings.UseBelowNormalPriorityUpdateThread)
                    {
                        m_WorkerThread.Priority = ThreadPriority.BelowNormal;
                    }
                    else
                    {
                        m_WorkerThread.Priority = ThreadPriority.Normal;
                    }
                    //线程开始进行运行
                    m_WorkerThread.Start();
                }

                //更新摄象机参数
                this.drawArgs.WorldCamera.Update(m_Device3d);
                //开始绘制场景
                m_Device3d.BeginScene();
                if (renderWireFrame)
                    m_Device3d.RenderState.FillMode = FillMode.WireFrame;//绘制场景的方式是边框图
                else
                    m_Device3d.RenderState.FillMode = FillMode.Solid;//实体图
                drawArgs.RenderWireFrame = renderWireFrame;

                //开始绘制当前 地球 。
                m_World.Render(this.drawArgs);
                //显示Cross的
                if (World.Settings.ShowCrosshairs)
                    this.DrawCrossHairs();
                //显示Dcsrs图标
                RenderLabel();
                //DrawIcon(this.DataDirectory + @"\Icons\stateName.png", 110, this.Height - 80);
                //if (World.Settings.ShowTarget)
                //    DrawIcon(DataDirectory + @"\Data\Icons\target.png", this.Width/2-21, this.Height/2 - 23);
                //保存当前视区的图像
                if (saveScreenShotFilePath != null)
                    SaveScreenShot();
                //显示空间信息
                if (World.Settings.ShowPosition)
                    RenderPositionInfo();
                //显示TocBar
                if (World.Settings.ShowLayerManager)
                    layerManager.Render(drawArgs);

                drawArgs.device.RenderState.ZBufferEnable = false;
                if (renderWireFrame)
                    m_Device3d.RenderState.FillMode = FillMode.Solid;
                m_Device3d.RenderState.FogEnable = false;

                //显示框Box文字信息
                if (m_World.OnScreenMessages != null)
                {
                    try
                    {
                        foreach (OnScreenMessage dm in m_World.OnScreenMessages)
                        {
                            int xPos = (int)Math.Round(dm.X * this.Width);
                            int yPos = (int)Math.Round(dm.Y * this.Height);
                            Rectangle posRect =
                                new Rectangle(xPos, yPos, this.Width, this.Height);
                            this.drawArgs.defaultDrawingFont.DrawText(null,
                                dm.Message, posRect,
                                DrawTextFormat.NoClip | DrawTextFormat.WordBreak,
                                Color.White);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                m_Device3d.EndScene();
            }
            catch
            {
            }
            finally
            {
                this.drawArgs.EndRender();
            }
            //更新鼠标状态
            drawArgs.UpdateMouseCursor(this);
        }

        protected void RenderLabel()
        {
            DrawTextFormat dtf = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Left;
            //文本框背景颜色
            int positionBackColor = positionAlpha << 24;
            //文本框字体颜色
            int positionForeColor = (int)((uint)(positionAlpha << 24) + 0xffffffu);
            int x = 60;
            int y = this.Height - 90;
            Rectangle textRect = Rectangle.FromLTRB(x, y, x + 150, y + 30);
            this.dcsrsFont.DrawText(null, "Dcsrs●2011", textRect, dtf, positionBackColor);
            this.dcsrsFont.DrawText(null, "Dcsrs●2011", textRect, dtf, positionForeColor);
        }
        /// <summary>
        /// 绘制文本信息,包括：当前经纬度、高程、倾斜角等空间信息
        /// </summary>
        protected void RenderPositionInfo()
        {
            // 显示经纬度信息
            string alt = null;
            double agl = this.drawArgs.WorldCamera.AltitudeAboveTerrain;//高度
            alt = ConvertUnits.GetDisplayString(agl).Replace("km", "");

            string dist = null;
            double dgl = this.drawArgs.WorldCamera.Distance;
            dist = ConvertUnits.GetDisplayString(dgl);//高度

            // 倾斜角
            double heading = this.drawArgs.WorldCamera.Heading.Degrees;
            if (heading < 0)
                heading += 360;

            string latPosition = "纬度：" + this.drawArgs.WorldCamera.Latitude.Degrees.ToString("f4") + "°";
            string lonPosition = "经度：" + this.drawArgs.WorldCamera.Longitude.Degrees.ToString("f4") + "°";
            string tilt = "视角海拔：" + alt + "千米";
            string headingString = "视角倾斜：" + heading.ToString("f4") + "°]";
            string version = "版权所有：论证中心.Qrst.Globe.V.1.0 ";
            string title = "空间信息描述：[ ";
            DrawTextFormat dtf = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Left;

            //文本框背景颜色
            int positionBackColor = positionAlpha << 24;
            //文本框字体颜色
            int positionForeColor = (int)((uint)(positionAlpha << 24) + 0xffffffu);

            int x = 60;
            int y = drawArgs.screenHeight - 30;
            Rectangle textRect = Rectangle.FromLTRB(x, y, x + 150, y + 30);
            this.spatialFont.DrawText(null, title, textRect, dtf, positionBackColor);
            this.spatialFont.DrawText(null, title, textRect, dtf, positionForeColor);
            x = 160;
            textRect = Rectangle.FromLTRB(x, y, x + 120, y + 30);
            this.spatialFont.DrawText(null, latPosition, textRect, dtf, positionBackColor);
            this.spatialFont.DrawText(null, latPosition, textRect, dtf, positionForeColor);
            x = 160 + 100 + 10;
            textRect = Rectangle.FromLTRB(x, y, x + 120, y + 30);
            this.spatialFont.DrawText(null, lonPosition, textRect, dtf, positionBackColor);
            this.spatialFont.DrawText(null, lonPosition, textRect, dtf, positionForeColor);
            x = 160 + 215 + 10;
            textRect = Rectangle.FromLTRB(x, y, x + 160, y + 30);
            this.spatialFont.DrawText(null, tilt, textRect, dtf, positionBackColor);
            this.spatialFont.DrawText(null, tilt, textRect, dtf, positionForeColor);
            x = 160 + 375 + 10;
            textRect = Rectangle.FromLTRB(x, y, x + 160, y + 30);
            this.spatialFont.DrawText(null, headingString, textRect, dtf, positionBackColor);
            this.spatialFont.DrawText(null, headingString, textRect, dtf, positionForeColor);
            x = 60;
            y = drawArgs.screenHeight - 60;
            textRect = Rectangle.FromLTRB(x, y, x + 250, y + 30);
            this.spatialFont.DrawText(null, version, textRect, dtf, positionBackColor);
            this.spatialFont.DrawText(null, version, textRect, dtf, positionForeColor);
        }

        Line crossHairs;//十字架对象
        int crossHairColor = Color.WhiteSmoke.ToArgb();//十字架的颜色
        /// <summary>
        /// 绘制中心十字架的问题
        /// </summary>
        protected void DrawCrossHairs()
        {
            //十字架的Size
            int crossHairSize = 2;
            //创建十字架对象
            if (this.crossHairs == null)
            {
                crossHairs = new Line(m_Device3d);
            }

            Vector2[] left = new Vector2[2];
            Vector2[] right = new Vector2[2];
            Vector2[] top = new Vector2[2];
            Vector2[] bottom = new Vector2[2];
            Vector2[] center = new Vector2[5];
            left[0].X = this.Width / 2 - crossHairSize - 10;
            left[0].Y = this.Height / 2;
            left[1].X = this.Width / 2 - crossHairSize - 3;
            left[1].Y = this.Height / 2;
            center[0].X = left[1].X;
            center[0].Y = left[1].Y;
            right[0].X = this.Width / 2 + crossHairSize + 10;
            right[0].Y = this.Height / 2;
            right[1].X = this.Width / 2 + crossHairSize + 3;
            right[1].Y = this.Height / 2;
            center[2].X = right[1].X;
            center[2].Y = right[1].Y;
            top[0].X = this.Width / 2;
            top[0].Y = this.Height / 2 - crossHairSize - 10;
            top[1].X = this.Width / 2;
            top[1].Y = this.Height / 2 - crossHairSize - 3;
            center[3].X = top[1].X;
            center[3].Y = top[1].Y;
            bottom[0].X = this.Width / 2;
            bottom[0].Y = this.Height / 2 + crossHairSize + 10;
            bottom[1].X = this.Width / 2;
            bottom[1].Y = this.Height / 2 + crossHairSize + 3;
            center[1].X = bottom[1].X;
            center[1].Y = bottom[1].Y;

            center[4].X = left[1].X;
            center[4].Y = left[1].Y;

            crossHairs.Width = 2;
            crossHairs.Begin();
            crossHairs.Draw(left, crossHairColor);
            crossHairs.Draw(right, crossHairColor);
            crossHairs.Draw(top, crossHairColor);
            crossHairs.Draw(bottom, crossHairColor);
            //crossHairs.Draw(center, crossHairColor);
            crossHairs.End();

            //Vector2[] vertical = new Vector2[2];
            //Vector2[] horizontal = new Vector2[2];
            //horizontal[0].X = this.Width / 2 - crossHairSize;
            //horizontal[0].Y = this.Height / 2;
            //horizontal[1].X = this.Width / 2 + crossHairSize;
            //horizontal[1].Y = this.Height / 2;

            //vertical[0].X = this.Width / 2;
            //vertical[0].Y = this.Height / 2 - crossHairSize;
            //vertical[1].X = this.Width / 2;
            //vertical[1].Y = this.Height / 2 + crossHairSize;
            //crossHairs.Width = 5;
            //crossHairs.Begin();
            //crossHairs.Draw(horizontal, crossHairColor);
            //crossHairs.Draw(vertical, crossHairColor);
            //crossHairs.End();
        }

        protected void DrawIcon(string textureFileName, int positionX, int positionY)
        {
            Texture iconTexture = null;
            int iconHeight = 0;
            int iconWidth = 0;
            bool iconIsRotated = false;
            double rotatedRadians = 0.0;
            int normalColor = Color.FromArgb(250, 255, 255, 255).ToArgb();
            if (ImageHelper.IsGdiSupportedImageFormat(textureFileName))
            {
                // Load without rescaling source bitmap
                using (Image image = ImageHelper.LoadImage(textureFileName))
                    iconTexture = LoadImage(m_Device3d, image, ref iconWidth, ref iconHeight);
            }
            else
            {
                // Only DirectX can read this file, might get upscaled depending on input dimensions.
                iconTexture = ImageHelper.LoadIconTexture(textureFileName);
                // Read texture level 0 size
                using (Surface s = iconTexture.GetSurfaceLevel(0))
                {
                    SurfaceDescription desc = s.Description;
                    iconWidth = desc.Width;
                    iconHeight = desc.Height;
                }
            }
            //iconWidth = 69;
            //iconHeight = 20;
            Sprite m_sprite = new Sprite(m_Device3d);
            m_sprite.Begin(SpriteFlags.AlphaBlend);
            if (iconTexture != null)
            {
                // Render icon
                float xscale = 0.8f;
                float yscale = 0.8f;
                m_sprite.Transform = Matrix.Scaling(xscale, yscale, 0);

                if (iconIsRotated)
                    m_sprite.Transform *= Matrix.RotationZ((float)rotatedRadians - (float)drawArgs.WorldCamera.Heading.Radians);

                m_sprite.Transform *= Matrix.Translation((float)positionX, (float)positionY, 0);
                m_sprite.Draw(iconTexture,
                    new Vector3(iconWidth >> 1, iconHeight >> 1, 0),
                    Vector3.Empty,
                    normalColor);

                // Reset transform to prepare for text rendering later
                m_sprite.Transform = Matrix.Identity;
            }

            m_sprite.End();

        }

        protected Texture LoadImage(Device device, Image image, ref int iconWidth, ref int iconHeight)
        {
            iconWidth = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Width) / Math.Log(2)))));
            if (iconWidth > device.DeviceCaps.MaxTextureWidth)
                iconWidth = device.DeviceCaps.MaxTextureWidth;

            iconHeight = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Height) / Math.Log(2)))));
            if (iconHeight > device.DeviceCaps.MaxTextureHeight)
                iconHeight = device.DeviceCaps.MaxTextureHeight;

            using (Bitmap textureSource = new Bitmap(iconWidth, iconHeight))
            using (Graphics g = Graphics.FromImage(textureSource))
            {
                g.DrawImage(image, 0, 0, iconWidth, iconHeight);

                Texture iconTexture = new Texture(device, textureSource, Usage.None, Pool.Managed);
                return iconTexture;
            }
        }

        /// <summary>
        /// 保存屏幕截图
        /// </summary>
        protected void SaveScreenShot()
        {
            try
            {
                using (Surface backbuffer = m_Device3d.GetBackBuffer(0, 0, BackBufferType.Mono))
                    SurfaceLoader.Save(saveScreenShotFilePath, saveScreenShotImageFileFormat, backbuffer);
                saveScreenShotFilePath = null;
            }
            catch (InvalidCallException caught)
            {
                MessageBox.Show(caught.Message, "保存截图失败!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置要在球上显示的文字信息
        /// </summary>
        /// <param name="messages"></param>
        public void SetDisplayMessages(IList messages)
        {
            m_World.OnScreenMessages = messages;
        }
        /// <summary>
        /// 设置夸大因子
        /// </summary>
        /// <param name="exageration"></param>
        public void SetVerticalExaggeration(double exageration)
        {
            World.Settings.VerticalExaggeration = (float)exageration;
        }

        /// <summary>
        /// 设置观测方向和角度
        /// </summary>
        /// <param name="horiz"></param>
        /// <param name="vert"></param>
        /// <param name="elev"></param>
        public void SetViewDirection(double horiz, double vert, double elev)
        {
            this.drawArgs.WorldCamera.SetPosition(this.drawArgs.WorldCamera.Latitude.Degrees, this.drawArgs.WorldCamera.Longitude.Degrees, horiz,
                this.drawArgs.WorldCamera.Altitude, vert);
        }

        /// <summary>
        /// 设置所有Layers的透明度
        /// </summary>
        /// <param name="layers"></param>
        public void SetLayers(IList layers)
        {
            if (layers != null)
            {
                foreach (LayerDescriptor ld in layers)
                {
                    this.CurrentWorld.SetLayerOpacity(ld.Category, ld.Name, (float)ld.Opacity * 0.01f);
                }
            }
        }
        public void SetViewOriginal()
        {
            this.SetViewPosition(30.0, 110.0, 0.0, 12756300.0, 0.0);
        }

        /// <summary>
        /// 设置视图
        /// </summary>
        /// <param name="degreesLatitude">纬度</param>
        /// <param name="degreesLongitude">经度</param>
        /// <param name="heading">南北轴倾角</param> 
        /// <param name="altitude">海拔高程</param>
        /// <param name="tilt">视角倾斜</param>
        public void SetViewPosition(double degreesLatitude, double degreesLongitude, double heading, double altitude, double tilt)
        {
            this.drawArgs.WorldCamera.SetPosition(degreesLatitude, degreesLongitude, heading, altitude, tilt);
        }

        /// <summary>
        /// 设置照相机的位置和高度
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <param name="degreesLongitude"></param>
        /// <param name="metersElevation"></param>
        public void SetViewPosition(double degreesLatitude, double degreesLongitude,
            double metersElevation)
        {
            this.drawArgs.WorldCamera.SetPosition(degreesLatitude, degreesLongitude, this.drawArgs.WorldCamera.Heading.Degrees,
                metersElevation, this.drawArgs.WorldCamera.Tilt.Degrees);
        }


        /// <summary>
        /// 试图回复3D Device.
        /// </summary>
        protected void AttemptRecovery()
        {
            try
            {
                m_Device3d.TestCooperativeLevel();
            }
            catch (DeviceLostException)
            {
            }
            catch (DeviceNotResetException)
            {
                try
                {
                    m_Device3d.Reset(m_presentParams);
                }
                catch (DeviceLostException)
                {
                }
            }
        }

        public static string DirectoryPath = string.Format(@"{0}\{1}\", Application.StartupPath, "Plugins");
        #region 鼠标事件的处理

        public static bool mouseToolUsing = false;
        /// <summary>
        /// 球窗体的鼠标滚轮事件
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                //判断鼠标的区域范围是否在LayerManager的区域范围内，若是，则返回，不做任何操作
                if (this.layerManager.OnMouseWheel(e))
                    return;
                //Zoom照相机
                this.drawArgs.WorldCamera.ZoomStepped(e.Delta / 120.0f);
            }
            finally
            {
                base.OnMouseWheel(e);
            }
        }
        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //if (mouseToolUsing)
            //{
            //    base.OnMouseDown(e);
            //}

            //设置当前窗体Focus
            this.Focus();
            DrawArgs.LastMousePosition.X = e.X;
            DrawArgs.LastMousePosition.Y = e.Y;

            mouseDownStartPosition.X = e.X;
            mouseDownStartPosition.Y = e.Y;


            try
            {
                bool handled = false;

                if (!handled)
                {
                    if (!this.layerManager.OnMouseDown(e))
                    {

                    }
                }
            }
            finally
            {
                if (e.Button == MouseButtons.Left)
                    DrawArgs.IsLeftMouseButtonDown = true;

                if (e.Button == MouseButtons.Right)
                    DrawArgs.IsRightMouseButtonDown = true;
                base.OnMouseDown(e);
            }
        }


        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        bool isDoubleClick = false;
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            //if (mouseToolUsing)
            //{
            //    base.OnMouseDoubleClick(e);
            //}

            isDoubleClick = true;
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != null)
            {
                base.OnMouseUp(e);
                return;
            }

            DrawArgs.LastMousePosition.X = e.X;
            DrawArgs.LastMousePosition.Y = e.Y;

            try
            {
                bool handled = false;
                if (!handled)
                {
                    // 若鼠标点击不在球的范围内，则不处理
                    if (mouseDownStartPosition == Point.Empty)
                        return;

                    mouseDownStartPosition = Point.Empty;
                    if (!this.isMouseDragging)
                    {
                        //处理拖拽图层列表工具栏事件
                        bool isLayerManager = this.layerManager.OnMouseUp(e);
                        if (isLayerManager)
                            return;
                    }
                    //若当前世界对象为空，则不处理
                    if (m_World == null)
                        return;
                    //判断是不是鼠标双击事件，此处处理鼠标双击事件
                    if (isDoubleClick)
                    {
                        isDoubleClick = false;
                        if (e.Button == MouseButtons.Left)//若是鼠标左键双击，则缩放ZOOMIN
                        {
                            drawArgs.WorldCamera.Zoom(World.Settings.CameraDoubleClickZoomFactor);
                        }
                        else if (e.Button == MouseButtons.Right)//若是鼠标右键双击，则缩放ZOOMOUT
                        {
                            drawArgs.WorldCamera.Zoom(-World.Settings.CameraDoubleClickZoomFactor);
                        }
                    }//处理鼠标单击事件
                    else
                    {
                        if (e.Button == MouseButtons.Left)//处理鼠标左键
                        {
                            if (this.isMouseDragging)
                            {
                                this.isMouseDragging = false;
                            }
                            else
                            {
                                if (!m_World.PerformSelectionAction(this.drawArgs))//处理世界对象下的所有图层的鼠标点击事件PerformSelectionAction
                                {

                                    Angle targetLatitude;
                                    Angle targetLongitude;
                                    this.drawArgs.WorldCamera.PickingRayIntersection(
                                        DrawArgs.LastMousePosition.X,
                                        DrawArgs.LastMousePosition.Y,
                                        out targetLatitude,
                                        out targetLongitude);
                                    if (!Angle.IsNaN(targetLatitude))
                                        this.drawArgs.WorldCamera.PointGoto(targetLatitude, targetLongitude);
                                }
                            }
                        }
                        else if (e.Button == MouseButtons.Right)//处理鼠标右键
                        {
                            if (this.isMouseDragging)
                                this.isMouseDragging = false;
                            else
                            {
                                if (!m_World.PerformSelectionAction(this.drawArgs))
                                {

                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                //最后设置鼠标为原始空的状态
                if (e.Button == MouseButtons.Left)
                    DrawArgs.IsLeftMouseButtonDown = false;

                if (e.Button == MouseButtons.Right)
                    DrawArgs.IsRightMouseButtonDown = false;
                base.OnMouseUp(e);
            }
        }
        /// <summary>
        /// 处理地球鼠标移动的操作
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //if (mouseToolUsing)
            //{
            //    base.OnMouseMove(e);
            //}

            //设置鼠标移动的图标
            DrawArgs.MouseCursor = CursorType.Arrow;

            try
            {
                bool handled = false;
                if (!handled)
                {
                    int deltaX = e.X - DrawArgs.LastMousePosition.X;
                    int deltaY = e.Y - DrawArgs.LastMousePosition.Y;
                    float deltaXNormalized = (float)deltaX / drawArgs.screenWidth;
                    float deltaYNormalized = (float)deltaY / drawArgs.screenHeight;

                    if (!this.isMouseDragging)
                    {
                        //处理LayerManager的拖拽拉大或拉小对象
                        if (this.layerManager.OnMouseMove(e))
                        {
                            base.OnMouseMove(e);
                            return;
                        }
                    }
                    //若之前没有记录鼠标按下的操作，则不进行任何处理，返回
                    if (mouseDownStartPosition == Point.Empty)
                        return;
                    //判断之前按下的是鼠标左键还是右键
                    bool isMouseLeftButtonDown = ((int)e.Button & (int)MouseButtons.Left) != 0;
                    bool isMouseRightButtonDown = ((int)e.Button & (int)MouseButtons.Right) != 0;

                    //设置鼠标拖拽地球标志为True，在MouseUp事件里捕获，进行处理
                    if (isMouseLeftButtonDown || isMouseRightButtonDown)
                    {
                        int dx = this.mouseDownStartPosition.X - e.X;
                        int dy = this.mouseDownStartPosition.Y - e.Y;
                        int distanceSquared = dx * dx + dy * dy;
                        if (distanceSquared > 3 * 3)
                            this.isMouseDragging = true;
                    }
                    //鼠标左键操作，则进行地球拖拽
                    if (isMouseLeftButtonDown && !isMouseRightButtonDown)
                    {
                        Angle prevLat, prevLon;
                        this.drawArgs.WorldCamera.PickingRayIntersection(
                            DrawArgs.LastMousePosition.X,
                            DrawArgs.LastMousePosition.Y,
                            out prevLat,
                            out prevLon);

                        Angle curLat, curLon;
                        this.drawArgs.WorldCamera.PickingRayIntersection(
                            e.X,
                            e.Y,
                            out curLat,
                            out curLon);

                        double factor = (this.drawArgs.WorldCamera.Altitude) / (1500 * this.CurrentWorld.EquatorialRadius);
                        drawArgs.WorldCamera.RotationYawPitchRoll(
                            Angle.FromRadians(DrawArgs.LastMousePosition.X - e.X) * factor,
                            Angle.FromRadians(e.Y - DrawArgs.LastMousePosition.Y) * factor,
                            Angle.Zero);

                    }
                    else if (!isMouseLeftButtonDown && isMouseRightButtonDown)//若是右键，则是进行地球的方向改变操作
                    {

                        Angle deltaEyeDirection = Angle.FromRadians(-deltaXNormalized * World.Settings.CameraRotationSpeed);
                        this.drawArgs.WorldCamera.RotationYawPitchRoll(Angle.Zero, Angle.Zero, deltaEyeDirection);
                        this.drawArgs.WorldCamera.Tilt += Angle.FromRadians(deltaYNormalized * World.Settings.CameraRotationSpeed);
                    }
                    else if (isMouseLeftButtonDown && isMouseRightButtonDown)//若是左键或右键同时操作，则进行地球的缩放
                    {
                        // Both buttons (zoom)
                        if (Math.Abs(deltaYNormalized) > float.Epsilon)
                            this.drawArgs.WorldCamera.Zoom(-deltaYNormalized);

                    }
                }
            }
            catch
            {
            }
            finally
            {

                this.drawArgs.WorldCamera.PickingRayIntersection(
                    e.X,
                    e.Y,
                    out cLat,
                    out cLon);

                DrawArgs.LastMousePosition.X = e.X;
                DrawArgs.LastMousePosition.Y = e.Y;
                base.OnMouseMove(e);
            }
        }
        Angle cLat, cLon;//经纬度信息
        protected override void OnMouseLeave(EventArgs e)
        {
            //if (mouseToolUsing)
            //{
            //    base.OnMouseLeave(e);
            //}

            if (layerManager != null)
                // reset menu bar mouse hover state.
                layerManager.OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, -1, -1, 0));
            base.OnMouseLeave(e);
        }

        #endregion

        /// <summary>
        /// 判断是否是窗体设计模式
        /// </summary>
        /// <returns></returns>
        private static bool IsInDesignMode()
        {
            string applicationExe = Application.ExecutablePath.ToUpper(CultureInfo.InvariantCulture);
            bool result = applicationExe.EndsWith("DEVENV.EXE");
            return result;
        }

        #region Device对象的初始化

        /// <summary>
        /// 初始化绘制图形,也就是Device对象的初始化
        /// </summary>
        private void InitializeGraphics()
        {
            // 建立我们的呈现参数
            m_presentParams = new PresentParameters();

            m_presentParams.Windowed = true;
            m_presentParams.SwapEffect = SwapEffect.Discard;
            m_presentParams.AutoDepthStencilFormat = DepthFormat.D16;
            m_presentParams.EnableAutoDepthStencil = true;

            int adapterOrdinal = 0;
            try
            {
                // Store the default adapter
                adapterOrdinal = Manager.Adapters.Default.Adapter;
            }
            catch
            {
                // User probably needs to upgrade DirectX or install a 3D capable graphics adapter
                throw new NotAvailableException();
            }

            DeviceType dType = DeviceType.Hardware;

            foreach (AdapterInformation ai in Manager.Adapters)
            {
                if (ai.Information.Description.IndexOf("NVPerfHUD") >= 0)
                {
                    adapterOrdinal = ai.Adapter;
                    dType = DeviceType.Reference;
                }
            }
            CreateFlags flags = CreateFlags.SoftwareVertexProcessing;

            // Check to see if we can use a pure hardware m_Device3d
            Caps caps = Manager.GetDeviceCaps(adapterOrdinal, DeviceType.Hardware);

            // Do we support hardware vertex processing?
            if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                //	// Replace the software vertex processing
                flags = CreateFlags.HardwareVertexProcessing;

            // Use multi-threading for now - TODO: See if the code can be changed such that this isn't necessary (Texture Loading for example)
            flags |= CreateFlags.MultiThreaded | CreateFlags.FpuPreserve;

            try
            {
                // Create our m_Device3d
                m_Device3d = new Device(adapterOrdinal, dType, this, flags, m_presentParams);
            }
            catch (Microsoft.DirectX.DirectXException)
            {
                throw new NotSupportedException("Unable to create the Direct3D m_Device3d.");
            }

            // Hook the m_Device3d reset event
            m_Device3d.DeviceReset += new EventHandler(OnDeviceReset);
            m_Device3d.DeviceResizing += new CancelEventHandler(m_Device3d_DeviceResizing);
            OnDeviceReset(m_Device3d, null);
        }

        private void OnDeviceReset(object sender, EventArgs e)
        {
            // Can we use anisotropic texture minify filter?
            if (m_Device3d.DeviceCaps.TextureFilterCaps.SupportsMinifyAnisotropic)
            {
                m_Device3d.SamplerState[0].MinFilter = TextureFilter.Anisotropic;
            }
            else if (m_Device3d.DeviceCaps.TextureFilterCaps.SupportsMinifyLinear)
            {
                m_Device3d.SamplerState[0].MinFilter = TextureFilter.Linear;
            }

            // What about magnify filter?
            if (m_Device3d.DeviceCaps.TextureFilterCaps.SupportsMagnifyAnisotropic)
            {
                m_Device3d.SamplerState[0].MagFilter = TextureFilter.Anisotropic;
            }
            else if (m_Device3d.DeviceCaps.TextureFilterCaps.SupportsMagnifyLinear)
            {
                m_Device3d.SamplerState[0].MagFilter = TextureFilter.Linear;
            }


            m_Device3d.SamplerState[0].AddressU = TextureAddress.Clamp;
            m_Device3d.SamplerState[0].AddressV = TextureAddress.Clamp;

            m_Device3d.RenderState.Clipping = true;
            m_Device3d.RenderState.CullMode = Cull.Clockwise;
            m_Device3d.RenderState.Lighting = false;
            m_Device3d.RenderState.Ambient = World.Settings.StandardAmbientColor;

            m_Device3d.RenderState.ZBufferEnable = true;
            m_Device3d.RenderState.AlphaBlendEnable = true;
            m_Device3d.RenderState.SourceBlend = Blend.SourceAlpha;
            m_Device3d.RenderState.DestinationBlend = Blend.InvSourceAlpha;
        }

        private void m_Device3d_DeviceResizing(object sender, CancelEventArgs e)
        {
            if (this.Size.Width == 0 || this.Size.Height == 0)
            {
                e.Cancel = true;
                return;
            }

            this.drawArgs.screenHeight = this.Height;
            this.drawArgs.screenWidth = this.Width;
        }

        #endregion

        /// <summary>
        /// 后台更新的线程
        /// </summary>
        private void WorkerThread()
        {
            //设置更新事件，这里设置每秒钟更新6次 
            //const int refreshIntervalMs = 150;
            const int refreshIntervalMs = 500;//修改为每秒2次 joki 131218

            while (m_WorkerThreadRunning)
            {
                try
                {
                    //设置线程的优先级
                    if (World.Settings.UseBelowNormalPriorityUpdateThread && m_WorkerThread.Priority == System.Threading.ThreadPriority.Normal)
                    {
                        m_WorkerThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                    }
                    else if (!World.Settings.UseBelowNormalPriorityUpdateThread && m_WorkerThread.Priority == System.Threading.ThreadPriority.BelowNormal)
                    {
                        m_WorkerThread.Priority = System.Threading.ThreadPriority.Normal;
                    }

                    //记录当前更新时的时间
                    long startTicks = 0;
                    PerformanceTimer.QueryPerformanceCounter(ref startTicks);

                    //更新 世界的 DrawArgs的参数.
                    m_World.Update(this.drawArgs);

                    //记录更新后的当前事件
                    long endTicks = 0;
                    PerformanceTimer.QueryPerformanceCounter(ref endTicks);

                    //计算两次事件的间隔。
                    float elapsedMilliSeconds = 1000 * (float)(endTicks - startTicks) / PerformanceTimer.TicksPerSecond;
                    float remaining = refreshIntervalMs - elapsedMilliSeconds;
                    //若两次事件的间隔>0的话，则证明，还没有到刷新的事件，就让线程进入睡眠状态
                    if (remaining > 0)
                        Thread.Sleep((int)remaining);
                }
                catch
                {
                }
            }
        }

    }
}
