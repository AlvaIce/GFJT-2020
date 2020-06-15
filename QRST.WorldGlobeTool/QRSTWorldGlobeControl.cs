using System;
using System.IO;
using System.Net;
using System.Globalization;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Net;
using QRST.WorldGlobeTool.Menu;
using QRST.WorldGlobeTool.Camera;
using QRST.WorldGlobeTool.Globe;
using QRST.WorldGlobeTool.Stores;
using QRST.WorldGlobeTool.Terrain;
using QRST.WorldGlobeTool.Utility;
using QRST.WorldGlobeTool.Renderable;
using QRST.WorldGlobeTool.PluginEngine;
using QRST.WorldGlobeTool.DrawUtility;
using QRST.WorldGlobeTool.Geometries;

namespace QRST.WorldGlobeTool
{
    public partial class QRSTWorldGlobeControl : UserControl
    {

        public QRSTWorldGlobeControl()
        {
            InitializeComponent();
        }

        

        #region 私有对象

        /// <summary>
        /// 数据配置路径
        /// </summary>
        private string m_DataDirectory = "";
        /// <summary>
        /// 缓存路径
        /// </summary>
        private string m_CacheDirectory = "";
        /// <summary>
        /// 插件目录
        /// </summary>
        public static string m_PluginsDir = string.Format(@"{0}\{1}\", Application.StartupPath, "Plugins");
        /// <summary>
        /// Direct 设备对象
        /// </summary>
        private Device m_Device3d;
        /// <summary>
        /// 这个对象确定设备向屏幕显示数据的方式
        /// </summary>
        private PresentParameters m_PresentParams;
        /// <summary>
        /// 绘制的参数
        /// </summary>
        private DrawArgs m_DrawArgs;
        /// <summary>
        /// 与当前控件关联的星球
        /// </summary>
        private EarthGlobe m_QRSTGlobe = null;
        /// <summary>
        /// 世界 对象
        /// </summary>
        private World m_World;
        /// <summary>
        /// 缓存 对象
        /// </summary>
        private Cache m_Cache;
        /// <summary>
        /// 工作线程
        /// </summary>
        private Thread m_WorkerThread;
        /// <summary>
        /// 工作线程是否处于工作中
        /// </summary>
        private bool m_WorkerThreadRunning;
        /// <summary>
        /// 屏幕截图保存路径
        /// </summary>
        private string m_SaveScreenShotFilePath;
        /// <summary>
        /// 屏幕截图 保存类型
        /// </summary>
        private ImageFileFormat m_SaveScreenShotImageFileFormat = ImageFileFormat.Png;
        /// <summary>
        /// 是否不启动渲染
        /// </summary>
        private bool m_IsRenderDisabled;
        /// <summary>
        /// 是否用鼠标正在拖动
        /// </summary>
        private bool m_IsMouseDragging;
        /// <summary>
        /// 是否处于标注状态
        /// </summary>
        private bool m_IsAnnotationState;
        /// <summary>
        /// 是否处于控制点拖动状态
        /// </summary>
        private bool m_IsGCPDragState;
        /// <summary>
        /// 鼠标是否双击
        /// </summary>
        private bool m_IsDoubleClick = false;
        /// <summary>
        /// 是否渲染框架
        /// </summary>
        private bool m_IsRenderWireFrame;
        /// <summary>
        /// 文本透明度
        /// </summary>
        private int m_TextAlpha = 255;
        /// <summary>
        /// 鼠标开始的位置
        /// </summary>
        private Point m_MouseDownStartPosition = Point.Empty;
        /// <summary>
        /// 空间信息字体
        /// </summary>
        private Microsoft.DirectX.Direct3D.Font m_SpatialFont = null;
        /// <summary>
        /// 版权信息字体
        /// </summary>
        private Microsoft.DirectX.Direct3D.Font m_CopyrightFont = null;
        /// <summary>
        /// 地球配置信息
        /// </summary>
        private System.Configuration.Configuration m_EarthConfig = null;
        /// <summary>
        /// 当前可视区域的左上角和右下角的经纬度
        /// </summary>
        private static Angle m_ltLat, m_ltLon, m_ubLat, m_ubLon;
        /// <summary>
        /// 插件编译器
        /// </summary>
        private PluginCompiler m_Compiler;
        /// <summary>
        /// 控件刚加载时的初始海拔高度
        /// </summary>
        private static double m_InitialAltitude;
        /// <summary>
        /// 三维球体当前加载数据的层级
        /// </summary>
        private static int m_CurrentLevel;
        /// <summary>
        /// 十字架对象
        /// </summary>
        private Line m_CrossHairs;
        /// <summary>
        /// 十字架的颜色
        /// </summary>
        private int m_CrossHairColor = Color.LawnGreen.ToArgb();
        /// <summary>
        /// 经纬度信息
        /// </summary>
        private Angle cLat, cLon;
        /// <summary>
        /// 正在拖动的GCP
        /// </summary>
        public GCP m_DraggingGCP;
        /// <summary>
        /// 是否处在鼠标工具状态下
        /// </summary>
        public static bool m_IsMouseToolUsing = false;
        /// <summary>
        /// 当前是否树3D地图模式
        /// </summary>
        private bool m_Is3DMapMode;
        /// <summary>
        /// 菜单按钮图标尺寸
        /// </summary>
        private static int m_menuButtonIconSize = 0;
        /// <summary>
        /// 顶部菜单条
        /// </summary>
        private MenuBar _menuBar = new MenuBar(World.Settings.ToolbarAnchor, m_menuButtonIconSize);
        /// <summary>
        /// 图层管理菜单按钮
        /// </summary>
        private LayerManagerButton m_LayerManagerButton;


        #endregion

        #region 公共属性

        /// <summary>
        /// 获取或设置与当前控件关联的插件编译器
        /// </summary>
        public PluginCompiler PCompiler
        {
            get { return this.m_Compiler; }
            set { this.m_Compiler = value; }
        }
        /// <summary>
        /// 获取或设置缓存路径
        /// </summary>
        public string CacheDirectory
        {
            get
            {
                return m_CacheDirectory;
            }
            set
            {
                m_CacheDirectory = value;
            }
        }
        /// <summary>
        /// 获取或设置配置数据路径
        /// </summary>
        public string DataDirectory
        {
            get
            {
                return m_DataDirectory;
            }
            set
            {
                m_DataDirectory = value;
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
                    this.m_DrawArgs.WorldCamera = camera;
                    //设置绘制对象参数（drawArgs）的地球对象
                    this.m_DrawArgs.CurrentWorld = value;
                    //添加网络格网对象
                    m_World.RenderableObjects.Add(new Renderable.LatLongGrid(m_World));
                    ////添加图层管理工具
                    _menuBar = new MenuBar(World.Settings.ToolbarAnchor, m_menuButtonIconSize);
                    m_LayerManagerButton = new LayerManagerButton(
                        Path.Combine(m_DataDirectory, @"Icons\layer-manager.png"),
                        m_World);
                    this._menuBar.AddToolsMenuButton(this.m_LayerManagerButton, 0);
                    this._menuBar.AddToolsMenuButton(new PositionMenuButton(
                        Path.Combine(m_DataDirectory, @"Icons\coordinates.png")), 1);
                    this._menuBar.AddToolsMenuButton(new LatLonMenuButton(
                        Path.Combine(m_DataDirectory, @"Icons\lat-long.png"), m_World), 2);
                    this.m_LayerManagerButton.SetPushed(World.Settings.ShowLayerManager);
                    //显示图层管理器
                    World.Settings.showLayerManager = true;
                    //显示格网
                    World.Settings.showLatLonLines = false;
                    //显示版权信息
                    World.Settings.showCopyright = false;
                    //显示位置信息
                    World.Settings.showPosition = true;
                    //显示中间的十字
                    World.Settings.showCrosshairs = true;
                    //隐藏太阳效果
                    World.Settings.EnableSunShading = false;
                    //转动地球的时候有惯性
                    World.Settings.CameraHasInertia = true;
                    //弃用相机平滑效果
                    World.Settings.CameraSmooth = true;
                    //大气圈散射效果
                    World.Settings.EnableAtmosphericScattering = true;
                }
            }
        }
        /// <summary>
        /// 获取或设置与当前控件关联的星球
        /// </summary>
        public EarthGlobe QrstGlobe
        {
            get
            {
                return m_QRSTGlobe;
            }
            set
            {
                m_QRSTGlobe = value;
            }
        }
        /// <summary>
        /// 获取绘制对象参数对象(DrawArgs)
        /// </summary>
        public DrawArgs DrawArgs
        {
            get { return this.m_DrawArgs; }
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
        /// 获取或设置是否不启动渲染
        /// </summary>
        public bool IsRenderDisabled
        {
            get
            {
                return m_IsRenderDisabled;
            }
            set
            {
                m_IsRenderDisabled = value;
            }
        }
        /// <summary>
        /// 获取或设置是否处于标注注记状态
        /// </summary>
        public bool IsAnnotationState
        {
            get { return m_IsAnnotationState; }
            set { m_IsAnnotationState = value; }
        }
        /// <summary>
        /// 获取或设置是否处于控制点拖动状态
        /// </summary>
        public bool IsGCPDragState
        {
            get { return m_IsGCPDragState; }
            set { m_IsGCPDragState = value; }
        }
        /// <summary>
        /// 确定是否有任何窗口排队的信息
        /// </summary>
        private bool IsAppStillIdle
        {
            get
            {
                QRST.WorldGlobeTool.Utility.NativeMethods.Message msg;
                return !QRST.WorldGlobeTool.Utility.NativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }
        internal LayerManagerButton LayerManagerButton
        {
            get
            {
                return m_LayerManagerButton;
            }
            set
            {
                m_LayerManagerButton = value;
            }
        }

        /// <summary>
        /// 是否是三维地图模式
        /// </summary>
        public bool Is3DMapMode
        {
            get
            {
                return m_Is3DMapMode;
            }
            set
            {
                m_Is3DMapMode = value;
                if (m_World != null)
                    m_QRSTGlobe.UpdateRenderableMode(m_World.RenderableObjects);
            }
        }

        /// <summary>
        /// 获取菜单条
        /// </summary>
        public MenuBar MenuBar
        {
            get
            {
                return this._menuBar;
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
        private string _caption = "";
        #endregion

        #region 球体对外事件
        /// <summary>
        /// 球体重绘制
        /// </summary>
        public event EventHandler OnGlobeRended;

        /// <summary>
        /// 矩形框绘制完毕事件
        /// </summary>
        public event EventHandler OnDrawRectangleCompletedEvent;

        /// <summary>
        /// 多线段绘制完毕事件
        /// </summary>
        public event EventHandler OnDrawPolyLineCompletedEvent;

        /// <summary>
        /// 多边形绘制完毕事件
        /// </summary>
        public event EventHandler OnDrawPolygonCompleteEvent;

        /// <summary>
        /// 数据库检索结果范围框选中事件
        /// </summary>
        public event EventHandler DBSearchTileExtentsSelectedEvent;

        /// <summary>
        /// 控制点拖动事件
        /// </summary>
        public event EventHandler<GCPModifyTypeEventArgs> GCPModifyEvent;

        public event EventHandler OnPolyUp;
        #endregion

        #region 公共方法

        #region 数据加载

        /// <summary>
        /// 加载球体
        /// </summary>
        public void GlobeLoad()
        {
            m_IsRenderWireFrame = false;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            // m_Device3d 不会被创建，除非要绘制的大小是大于1*1的。
            this.Size = new Size(this.Width, this.Height);
            try
            {
                //若不是设计窗体阶段（若window）不在IDE窗体中，则不进行初始化。
                //if (!isInDesignMode())
                this.InitializeGraphics();

                //初始化m_Device3d设备对象。
                this.m_DrawArgs = new DrawArgs(m_Device3d, this);
                this.m_DrawArgs.ScreenHeight = this.Height;
                this.m_DrawArgs.ScreenWidth = this.Width;

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

            m_SpatialFont = this.m_DrawArgs.CreateFont("微软雅黑", 18, System.Drawing.FontStyle.Bold);
            m_CopyrightFont = this.m_DrawArgs.CreateFont("微软雅黑", 18, System.Drawing.FontStyle.Bold);

            ExeConfigurationFileMap qrstConfigFile = new ExeConfigurationFileMap();
            qrstConfigFile.ExeConfigFilename = Path.GetDirectoryName(Application.ExecutablePath) + @"\QRSTWorldGlobe.config";//配置文件路径
            m_EarthConfig = ConfigurationManager.OpenMappedExeConfiguration(qrstConfigFile, ConfigurationUserLevel.None);
            if (m_EarthConfig.ConnectionStrings.ConnectionStrings["UseLocalData"].ToString() == "true")
            {
                m_CacheDirectory = Path.GetDirectoryName(Application.ExecutablePath) + @"\Cache";
                m_DataDirectory = Path.GetDirectoryName(Application.ExecutablePath) + @"\SpatialData";
            }
            else
            {
                m_CacheDirectory = m_EarthConfig.ConnectionStrings.ConnectionStrings["CacheDirectory"].ToString();
                m_DataDirectory = m_EarthConfig.ConnectionStrings.ConnectionStrings["DataDirectory"].ToString();
            }
            World.Settings.CacheDirectory = m_CacheDirectory;
            World.Settings.DataDirectory = m_DataDirectory;

            configrationQrstGlobe();//初始化window对象
            initializeQrstPlanet();//初始化地球对象
            this.m_QRSTGlobe = new EarthGlobe(this);

            Stars3DLayer starlayer = new Stars3DLayer("星空", Path.Combine(this.DataDirectory, "Space\\"), this);
            this.CurrentWorld.RenderableObjects.ChildObjects.Insert(0, starlayer);

            SkyLayer skyLayer = new SkyLayer("天空背景", this);
            this.CurrentWorld.RenderableObjects.ChildObjects.Insert(0, skyLayer);

            World newWorld = this.CurrentWorld;
            newWorld.RenderableObjects.SetWorld(newWorld);
            //添加底图
            RenderableObjectList baseimageROL = new RenderableObjectList("世界影像底图");
            baseimageROL.Add(getBlueMarble(newWorld, this.Cache));
            baseimageROL.Add(getbaseimage(newWorld, this.Cache));
            //if (newWorld.RenderableObjects.ChildObjects.Count > 0)
            //{
                newWorld.RenderableObjects.Add(baseimageROL);
                //newWorld.RenderableObjects.Add(getBlueMarble(newWorld, this.Cache));
                //newWorld.RenderableObjects.Add(getbaseimage(newWorld, this.Cache));
                //newWorld.RenderableObjects.Add(getWhiteWorld(newWorld));
            //}
            //else
            //{
            //    newWorld.RenderableObjects = baseimageROL;             
            //    //newWorld.RenderableObjects = getBlueMarble(newWorld, this.Cache);
            //    //newWorld.RenderableObjects.Add(getbaseimage(newWorld, this.Cache));
            //    //newWorld.RenderableObjects.Add(getWhiteWorld(newWorld));
            //}

            //ZYM:20130708-初始化时寻找程序集内的插件
            initializePluginCompiler();

            Application.Idle += new EventHandler(this.OnApplicationIdle);
            Application.DoEvents();
        }

        #endregion

        #region 图层菜单获取

        /// <summary>
        /// Resets the toolbar
        /// </summary>
        public void ResetToolbar()
        {
            if (this._menuBar != null)
            {
                lock (this._menuBar.LayersMenuButtons.SyncRoot)
                {
                    foreach (IMenu m in this._menuBar.LayersMenuButtons)
                    {
                        m.Dispose();
                    }
                    this._menuBar.LayersMenuButtons.Clear();
                }

                lock (this._menuBar.ToolsMenuButtons.SyncRoot)
                {

                    for (int i = 0; i < this._menuBar.ToolsMenuButtons.Count; i++)
                    {
                        IMenu m = (IMenu)this._menuBar.ToolsMenuButtons[i];
                        if (m != null)
                        {
                            m.Dispose();
                        }
                    }

                    this._menuBar.ToolsMenuButtons.Clear();
                }
            }
            else
            {
            }
        }


        #endregion

        #region  绘制工具


        DrawPolygonLayer layer;
        DrawPolylineLayer lineLayer;
        public bool needRefreshDrawPolyLayer = true;
        public void DrawPoly(Angle lat, Angle lon,int type)
        {
            if (needRefreshDrawPolyLayer)
            {
                PluginInfo pi=null;
                foreach (PluginInfo varPi in this.m_Compiler.Plugins)
                {

                    if (varPi.Plugin is DrawPolygonTool && type == 0)
                    {
                        layer = ((DrawPolygonTool)varPi.Plugin).drawLayer;
                        //pi = varPi;
                    }
                    else if (varPi.Plugin is DrawPolyLineTool && type == 1)
                    {
                        lineLayer = ((DrawPolyLineTool)varPi.Plugin).drawLayer;
                    }
                    
                }
                //if ( pi==null)
                //{
                //    pi = new PluginInfo();
                //    pi.Plugin = new DrawPolygonTool();
                //    pi.Name = typeof(DrawPolygonTool).Name;
                //    pi.Description = "DrawPolygenTool.";
                //    this.m_Compiler.Plugins.Add(pi);
                //    pi.Plugin.PluginLoad(this, m_PluginsDir);
                //    World.Settings.CurrentWwTool = pi.Plugin;
                //    ((DrawPolygonTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);
                //}
                //layer = ((DrawPolygonTool)pi.Plugin).drawLayer;
                if (type == 0)
                {
                    if (layer != null && layer.PointList != null && layer.PointList.Count != 0)
                    {
                        layer.PointList.Clear();
                    }
                    if (layer != null && layer.VertexList != null && layer.VertexList.Count != 0)
                    {
                        layer.VertexList.Clear();
                    }
                }
                if (type == 1)
                {
                    if (lineLayer != null && lineLayer.PointList != null && lineLayer.PointList.Count != 0)
                    {
                        lineLayer.PointList.Clear();
                    }
                    if (lineLayer != null && lineLayer.VertexList != null && lineLayer.VertexList.Count != 0)
                    {
                        lineLayer.VertexList.Clear();
                    }
                }
                needRefreshDrawPolyLayer = false;
            }
            if (layer != null && type == 0)
                layer.DrawPoint(lat, lon);
            if (lineLayer != null && type == 1)
                lineLayer.DrawPoint(lat, lon);
        }
        public void CompletePoly(Angle lat, Angle lon, int type)
        {
            if (layer != null && type == 0)
                layer.completeDraw(lat, lon);
            if (lineLayer != null && type == 1)
                lineLayer.completeDraw(lat, lon);
        }
        public void completePart(Angle lat, Angle lon)
        {
            layer.completePart(lat, lon);
        }
        public Point3d GetPolyPoint()
        {
            Point3d pt3 = new Point3d();
            foreach (PluginInfo varPi in this.m_Compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawPolygonTool)
                {
                    if ((varPi.Plugin as DrawPolygonTool).drawLayer != null)
                        pt3 = (varPi.Plugin as DrawPolygonTool).drawLayer.pt3d;
                }
                else if (varPi.Plugin != null && varPi.Plugin is DrawPolyLineTool)
                {
                    if ((varPi.Plugin as DrawPolyLineTool).drawLayer != null)
                        pt3 = (varPi.Plugin as DrawPolyLineTool).drawLayer.pt3d;
                }
            }
            return pt3;
        }

        public void ClearPoly(bool isDraw)
        {
            DrawPolygonLayer polylyr = null;
            DrawPolylineLayer linelyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "多边形区域图层")
                {
                    polylyr = lyr as DrawPolygonLayer;
                }
                else if (lyr.Name == "多线段图层")
                {
                    linelyr = lyr as DrawPolylineLayer;
                }
            }
            if (!isDraw && polylyr != null)
            {
                PluginInfo p = null;
                foreach (PluginInfo varPi in this.m_Compiler.Plugins)
                {
                    if (varPi.Plugin != null && varPi.Plugin is DrawPolygonTool)
                    {
                        varPi.Plugin.PluginUnload();
                        p = varPi;
                        break;
                    }
                }
                this.m_Compiler.Plugins.Remove(p);
            }
            else if (!isDraw && linelyr != null)
            {
                PluginInfo p = null;
                foreach (PluginInfo varPi in this.m_Compiler.Plugins)
                {
                    if (varPi.Plugin != null && varPi.Plugin is DrawPolyLineTool)
                    {
                        varPi.Plugin.PluginUnload();
                        p = varPi;
                        break;
                    }
                }
                this.m_Compiler.Plugins.Remove(p);
            }
            //m_World.RenderableObjects.Remove(polylyr);
            //polylyr.IsOn = false;
        }
        public void ClearRect(bool isDraw)
        {
            DrawRectangleLayer polylyr = null;
            foreach (RenderableObject lyr in m_World.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == "矩形区域图层")
                {
                    polylyr = lyr as DrawRectangleLayer;
                }
            }
            if (!isDraw && polylyr != null)
            {
                PluginInfo p = null;
                foreach (PluginInfo varPi in this.m_Compiler.Plugins)
                {
                    if (varPi.Plugin != null && varPi.Plugin is DrawRectangleTool)
                    {
                        varPi.Plugin.PluginUnload();
                        p = varPi;
                        break;
                    }
                }
                this.m_Compiler.Plugins.Remove(p);
            }
            //m_World.RenderableObjects.Remove(polylyr);
            //polylyr.IsOn = false;
        }
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
            List<List<float>> extents = new List<List<float>>();
            //List<RectangleF> extents = new List<RectangleF>();
            delyr.UpdateExtents(extents);
        }
        public void DrawSearchResultExtents1(List<List<float>> extents)
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

        public void DrawPolygenLayer(Point3d fromPoint, Point3d toPoint)
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
        public void DrawSelectedExtents(List<List<float>> extents)
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
        public void DrawCheckedExtents(List<List<float>> extents)
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
            double[] rectangle = new double[4];
            foreach (PluginInfo varPi in this.m_Compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawRectangleTool && (varPi.Plugin as DrawRectangleTool).drawLayer != null)
                {
                    rectangle[0] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[0].X;
                    rectangle[1] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[0].Y;
                    rectangle[2] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[2].X;
                    rectangle[3] = (varPi.Plugin as DrawRectangleTool).drawLayer.PointList[2].Y;
                }
            }
            return rectangle;
        }

        /// <summary>
        /// 使用矩阵选框工具
        /// </summary>
        public void UsingDrawRectangleTool()
        {
            //读取矩形绘制工具 
            bool noDptool = true;
            foreach (PluginInfo pi in this.m_Compiler.Plugins)
            {
                if (pi.Plugin != null && pi.Plugin is DrawRectangleTool)
                {
                    if (pi.IsCurrentlyLoaded)
                    {
                        //卸载当前使用工具
                        if (World.Settings.CurrentWwTool == pi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        ((DrawRectangleTool)pi.Plugin).OnCompleted -= new EventHandler(QRSTWorldGlobeControl_OnRectangleCompleted);
                        pi.Plugin.PluginUnload();
                        this.m_Compiler.Plugins.Remove(pi);
                    }
                    else
                    {
                        pi.Plugin.PluginLoad(this, m_PluginsDir);
                        World.Settings.CurrentWwTool = pi.Plugin;
                        ((DrawRectangleTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnRectangleCompleted);
                    }
                    noDptool = false;
                    break;
                }
            }
            //如果编译器里没有该工具 则创建之 并加载
            if (noDptool)
            {
                PluginInfo pi = new PluginInfo();
                pi.Plugin = new DrawRectangleTool();
                pi.Name = typeof(DrawRectangleTool).Name;
                pi.Description = "DrawRectangleTool.";
                this.m_Compiler.Plugins.Add(pi);
                pi.Plugin.PluginLoad(this, m_PluginsDir);
                World.Settings.CurrentWwTool = pi.Plugin;
                ((DrawRectangleTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnRectangleCompleted);
            }
        }

        /// <summary>
        /// 上报矩形框绘制完毕事件
        /// </summary>
        private void QRSTWorldGlobeControl_OnRectangleCompleted(object sender, EventArgs e)
        {
            if (OnDrawRectangleCompletedEvent != null)
            {
                OnDrawRectangleCompletedEvent(sender, null);
            }
        }

        /// <summary>
        /// 获取绘制的多线段的顶点经纬度数组
        /// </summary>
        /// <returns>返回多线段顶点经纬度坐标数组，偶数下标为经度、奇数下标为纬度</returns>
        public double[] GetSelectPolyLine()
        {
            List<double> polyLinePointList = new List<double>();
            foreach (PluginInfo varPi in this.m_Compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawPolyLineTool && (varPi.Plugin as DrawPolyLineTool).drawLayer != null)
                {
                    foreach (Point3d p in (varPi.Plugin as DrawPolyLineTool).drawLayer.PointList)
                    {
                        polyLinePointList.Add(p.X);
                        polyLinePointList.Add(p.Y);
                    }
                }
            }
            return polyLinePointList.ToArray();
        }

        /// <summary>
        /// 使用画线工具
        /// </summary>
        public void UsingDrawPolyLineTool()
        {
            //获取插件编译器
            if (this.m_Compiler == null)
            {
                initializePluginCompiler();
            }
            //读取绘制工具 
            if (m_flagDrawPloygonTool)
            {
                foreach (PluginInfo varPi in this.m_Compiler.Plugins)
                {
                    if (varPi.Plugin.IsLoaded)
                    {
                        //Tool清空当前使用工具
                        if (World.Settings.CurrentWwTool == varPi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        if (varPi.ID == "DrawPolyLineTool")
                        {
                            ((DrawPolyLineTool)varPi.Plugin).OnPolyUp -= new EventHandler(QrstAxGlobeControl_OnPolyUp);
                            ((DrawPolyLineTool)varPi.Plugin).OnCompleted -= new EventHandler(QRSTWorldGlobeControl_OnPolyLineCompleted);

                            varPi.Plugin.PluginUnload();
                            this.m_Compiler.Plugins.Remove(varPi);
                            this.m_Compiler.Plugins.Clear();
                            m_flagDrawPloygonTool = false;
                            break;
                        }
                    }

                }
            }
            //读取画线工具 
            bool noDptool = true;
            foreach (PluginInfo pi in this.m_Compiler.Plugins)
            {
                if (pi.Plugin != null && pi.Plugin is DrawPolyLineTool)
                {
                    if (pi.IsCurrentlyLoaded)
                    {
                        //卸载当前使用工具
                        if (World.Settings.CurrentWwTool == pi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        ((DrawPolyLineTool)pi.Plugin).OnPolyUp -= new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolyLineTool)pi.Plugin).OnCompleted -= new EventHandler(QRSTWorldGlobeControl_OnPolyLineCompleted);
                        pi.Plugin.PluginUnload();
                        this.m_Compiler.Plugins.Remove(pi);
                    }
                    else
                    {
                        pi.Plugin.PluginLoad(this, m_PluginsDir);
                        World.Settings.CurrentWwTool = pi.Plugin;
                        ((DrawPolyLineTool)pi.Plugin).OnPolyUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolyLineTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnPolyLineCompleted);
                    }
                    noDptool = false;
                    break;
                }
            }

            //如果编译器里没有该工具 则创建之 并加载
            if (noDptool)
            {
                PluginInfo pi = new PluginInfo();
                pi.Plugin = new DrawPolyLineTool();
                pi.Name = typeof(DrawPolyLineTool).Name;
                pi.Description = "DrawPolyLineTool.";
                this.m_Compiler.Plugins.Add(pi);
                pi.Plugin.PluginLoad(this, m_PluginsDir);
                World.Settings.CurrentWwTool = pi.Plugin;
                ((DrawPolyLineTool)pi.Plugin).OnPolyUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                ((DrawPolyLineTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnPolyLineCompleted);
            }
        }

        /// <summary>
        /// 上报多线段绘制完毕事件
        /// </summary>
        private void QRSTWorldGlobeControl_OnPolyLineCompleted(object sender, EventArgs e)
        {
            if (OnDrawPolyLineCompletedEvent != null)
            {
                OnDrawPolyLineCompletedEvent(this, null);
            }
        }

        /// <summary>
        /// 获取绘制的多边形的顶点经纬度数组
        /// </summary>
        /// <returns>返回多边形顶点经纬度坐标数组，偶数下标为经度、奇数下标为纬度</returns>
        public double[] GetSelectPolygen()
        {
            List<double> polygenPointList = new List<double>();
            foreach (PluginInfo varPi in this.m_Compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawPolygonTool && (varPi.Plugin as DrawPolygonTool).drawLayer != null)
                {
                    foreach (Point3d p in (varPi.Plugin as DrawPolygonTool).drawLayer.PointList)
                    {
                        polygenPointList.Add(p.X);
                        polygenPointList.Add(p.Y);
                    }
                }
            }
            return polygenPointList.ToArray();
        }

        /// <summary>
        /// 使用画多边形工具
        /// </summary>
        public void UsingDrawPloygonTool()
        {
            //获取插件编译器
            if (this.m_Compiler == null)
            {
                initializePluginCompiler();
            }

            //读取多边形工具 
            bool noDptool = true;

            foreach (PluginInfo pi in this.m_Compiler.Plugins)
            {
                if (pi.Plugin != null && pi.Plugin is DrawPolygonTool)
                {
                    if (pi.IsCurrentlyLoaded)
                    {
                        //卸载当前使用工具
                        if (World.Settings.CurrentWwTool == pi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        ((DrawPolygonTool)pi.Plugin).OnUp -= new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)pi.Plugin).OnCompleted -= new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);
                        pi.Plugin.PluginUnload();
                        this.m_Compiler.Plugins.Remove(pi);
                    }
                    else
                    {
                        pi.Plugin.PluginLoad(this, m_PluginsDir);
                        World.Settings.CurrentWwTool = pi.Plugin;
                        ((DrawPolygonTool)pi.Plugin).OnUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);
                    }
                    noDptool = false;
                    break;
                }
            }

            //如果编译器里没有该工具 则创建之 并加载
            if (noDptool)
            {
                PluginInfo pi = new PluginInfo();
                pi.Plugin = new DrawPolygonTool();
                pi.Name = typeof(DrawPolygonTool).Name;
                pi.Description = "DrawPolygenTool.";
                this.m_Compiler.Plugins.Add(pi);
                pi.Plugin.PluginLoad(this, m_PluginsDir);
                World.Settings.CurrentWwTool = pi.Plugin;
                ((DrawPolygonTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);
            }
        }

        public bool m_flagDrawPloygonTool = false;
        public void TurnOnOffDrawPloygonTool()
        {
            //获取插件编译器
            if (this.m_Compiler == null)
            {
              initializePluginCompiler();
            }
            //读取绘制工具 
            bool noDptool = true;
            if (m_flagDrawPloygonTool)
            {
                foreach (PluginInfo varPi in this.m_Compiler.Plugins)
                {
                    if (varPi.Plugin.IsLoaded)
                    {
                        //Tool清空当前使用工具
                        if (World.Settings.CurrentWwTool == varPi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        if (varPi.ID == "DrawPolygonTool")
                        {
                            ((DrawPolygonTool)varPi.Plugin).OnUp -= new EventHandler(QrstAxGlobeControl_OnPolyUp);
                            ((DrawPolygonTool)varPi.Plugin).OnCompleted -= new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);

                            varPi.Plugin.PluginUnload();
                            this.m_Compiler.Plugins.Remove(varPi);
                            this.m_Compiler.Plugins.Clear();
                            m_flagDrawPloygonTool = false;
                            break;
                        }
                    }

                }
            }
            foreach (PluginInfo varPi in this.m_Compiler.Plugins)
            {
                if (varPi.Plugin != null && varPi.Plugin is DrawPolygonTool)
                {
                    if (varPi.Plugin.IsLoaded)
                    {
                        //Tool清空当前使用工具
                        if (World.Settings.CurrentWwTool == varPi.Plugin)
                        {
                            World.Settings.CurrentWwTool = null;
                        }
                        ((DrawPolygonTool)varPi.Plugin).OnUp -= new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)varPi.Plugin).OnCompleted -= new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);

                        varPi.Plugin.PluginUnload();
                        this.m_Compiler.Plugins.Remove(varPi);

                    }
                    else
                    {
                        varPi.Plugin.PluginLoad(this, m_PluginsDir);
                        World.Settings.CurrentWwTool = varPi.Plugin;
                        ((DrawPolygonTool)varPi.Plugin).OnUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                        ((DrawPolygonTool)varPi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);
                    }
                    noDptool = false;
                    break;
                }
            }
            //如果编译器里没有该工具 则创建之 并加载
            if (noDptool)
            {
                PluginInfo pi = new PluginInfo();
                pi.Plugin = new DrawPolygonTool();
                pi.Name = typeof(DrawPolygonTool).Name;
                pi.Description = "DrawPolygonTool.";
                this.m_Compiler.Plugins.Add(pi);
                pi.Plugin.PluginLoad(this, m_PluginsDir);
                World.Settings.CurrentWwTool = pi.Plugin;
                ((DrawPolygonTool)pi.Plugin).OnUp += new EventHandler(QrstAxGlobeControl_OnPolyUp);
                ((DrawPolygonTool)pi.Plugin).OnCompleted += new EventHandler(QRSTWorldGlobeControl_OnPolygenCompleted);
            }
        }

        void QrstAxGlobeControl_OnPolyUp(object sender, EventArgs e)
        {
            if (OnPolyUp != null)
            {
                OnPolyUp(sender, e);
            }
        }
        /// <summary>
        /// 上报多边形绘制完毕事件
        /// </summary>
        private void QRSTWorldGlobeControl_OnPolygenCompleted(object sender, EventArgs e)
        {
            if (OnDrawPolygonCompleteEvent != null)
            {
                OnDrawPolygonCompleteEvent(this, null);
            }
        }

        #endregion

        #region  移动地球到指定位置

        /// <summary>
        /// 移动地球到指定位置
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        /// <param name="heading">以度为单位的照相机角度(0°~360°)或者“double.NaN”代表没有任何变化</param>
        /// <param name="altitude">以米为单位的照相机高度或者“double.NaN”代表没有任何变化</param>
        /// <param name="perpendicularViewRange"></param>
        /// <param name="tilt">Camera tilt in degrees (-90 - 90) or double.NaN for no change.</param>
        public void GotoLatLon(double latitude, double longitude, double heading, double altitude, double perpendicularViewRange, double tilt)
        {
            if (!double.IsNaN(perpendicularViewRange))
                altitude = m_World.EquatorialRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
            if (altitude < 1)
                altitude = 1;
            this.m_DrawArgs.WorldCamera.SetPosition(latitude, longitude, heading, altitude, tilt);
        }

        /// <summary>
        /// 移动地球到指定经纬度坐标处
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        public void GotoLatLon(double latitude, double longitude)
        {
            this.m_DrawArgs.WorldCamera.SetPosition(latitude, longitude,
                this.m_DrawArgs.WorldCamera.Heading.Degrees,
                this.m_DrawArgs.WorldCamera.Altitude,
                this.m_DrawArgs.WorldCamera.Tilt.Degrees);
        }

        /// <summary>
        /// 移动地球到指定经纬度坐标和高度处
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        /// <param name="altitude">以米为单位的照相机高度或者“double.NaN”代表没有任何变化</param>
        public void GotoLatLonAltitude(double latitude, double longitude, double altitude)
        {
            this.m_DrawArgs.WorldCamera.SetPosition(latitude, longitude,
                this.m_DrawArgs.WorldCamera.Heading.Degrees,
                altitude,
                this.m_DrawArgs.WorldCamera.Tilt.Degrees);
        }

        /// <summary>
        /// 移动地球到指定位置
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        /// <param name="heading">以度为单位的照相机角度(0°~360°)或者“double.NaN”代表没有任何变化</param>
        /// <param name="perpendicularViewRange"></param>
        public void GotoLatLonHeadingViewRange(double latitude, double longitude, double heading, double perpendicularViewRange)
        {
            double altitude = m_World.EquatorialRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
            this.GotoLatLonHeadingAltitude(latitude, longitude, heading, altitude);
        }
        
        /// <summary>
        /// 移动地球到指定位置
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        /// <param name="perpendicularViewRange"></param>
        public void GotoLatLonViewRange(double minlat, double maxlat, double minlon, double maxlon)
        {
            double lonwidth = maxlon - minlon;
            lonwidth = (lonwidth < 0) ? 360 + lonwidth : lonwidth;
            double perpendicularViewRange = (maxlat - minlat > lonwidth ? maxlat - minlat : lonwidth);
            double altitude = m_World.EquatorialRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
            this.GotoLatLonHeadingAltitude((minlat + maxlat) / 2, (maxlon + minlon) / 2, this.m_DrawArgs.WorldCamera.Heading.Degrees, altitude);
            
        }

        /// <summary>
        /// 移动地球到指定位置
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        /// <param name="perpendicularViewRange"></param>
        public void GotoLatLonViewRange(double latitude, double longitude, double perpendicularViewRange)
        {
            double altitude = m_World.EquatorialRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
            this.GotoLatLonHeadingAltitude(latitude, longitude, this.m_DrawArgs.WorldCamera.Heading.Degrees, altitude);
        }

        /// <summary>
        /// 移动地球到指定位置
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        /// <param name="heading">以度为单位的照相机角度(0°~360°)或者“double.NaN”代表没有任何变化</param>
        /// <param name="altitude">以米为单位的照相机高度或者“double.NaN”代表没有任何变化</param>
        public void GotoLatLonHeadingAltitude(double latitude, double longitude, double heading, double altitude)
        {
            this.m_DrawArgs.WorldCamera.SetPosition(latitude, longitude,
                heading,
                altitude,
                this.m_DrawArgs.WorldCamera.Tilt.Degrees);
        }

        /// <summary>
        /// 移动地球到指定位置
        /// </summary>
        /// <param name="latitude">以度为单位的目标位置的纬度(-90°~ 90°)</param>
        /// <param name="longitude">以度为单位的目标位置的经度(-180°~ 180°)</param>
        /// <param name="heading">以度为单位的照相机角度(0°~360°)或者“double.NaN”代表没有任何变化</param>
        /// <param name="altitude">以米为单位的照相机高度或者“double.NaN”代表没有任何变化</param>
        public void GotoLatLonHeadingAltitude(double latitude, double longitude, double heading, double altitude, double tilt)
        {
            this.m_DrawArgs.WorldCamera.SetPosition(latitude, longitude,
                heading,
                altitude,
                tilt);
        }

        /// <summary>
        /// 重置控件到地球刚初始化的位置
        /// </summary>
        public void ResetGlobe()
        {
            GotoLatLonHeadingAltitude(30, 110, 0, 6378137.0 * 2, 0);
        }

        #endregion

        #region 经纬度获取

        /// <summary>
        /// 获取当前鼠标点的经纬度信息
        /// </summary>
        /// <returns>返回当前鼠标点对应的经纬度数组：Longitude、Latitude</returns>
        public double[] GetCurrentMousePositionAsLonLat()
        {
            return new double[] { DrawArgs.CurrentMouseLongtitude.Degrees, 
                                  DrawArgs.CurrentMouseLatitude.Degrees };
        }

        /// <summary>
        /// 获取当前视域中心点的经纬度信息
        /// </summary>
        /// <returns>返回当前视域中心点对应的经纬度数组：Longitude、Latitude</returns>
        public double[] GetCurrentCenterPositionAsLonLat()
        {
            return new double[]{this.m_DrawArgs.WorldCamera.Longitude.Degrees,
                                this.m_DrawArgs.WorldCamera.Latitude.Degrees};
        }

        /// <summary>
        /// 获取当前视域中心点的经纬度信息
        /// </summary>
        /// <returns>返回当前视域中心点对应的经纬度数组：Longitude、Latitude、Altitude、Heading</returns>
        public double[] GetCurrentCenterPosition()
        {
            double agl = this.m_DrawArgs.WorldCamera.AltitudeAboveTerrain;//高度
            double heading = this.m_DrawArgs.WorldCamera.Heading.Degrees;
            if (heading < 0)
                heading += 360;

            return new double[]{this.m_DrawArgs.WorldCamera.Longitude.Degrees,
                                this.m_DrawArgs.WorldCamera.Latitude.Degrees,
                                agl,
                                heading};
        }

        /// <summary>
        /// 获取当前视域的边界范围
        /// </summary>
        /// <returns>返回当前视域的边界范围：左上角经度、纬度，右下角经度、纬度</returns>
        public double[] GetCurrentViewBoundaryAsLonLat()
        {

            //当前可视区域左上角和右下角经纬度坐标计算
            this.m_DrawArgs.WorldCamera.PickingRayIntersection(0, 0, out m_ltLat, out m_ltLon);
            this.m_DrawArgs.WorldCamera.PickingRayIntersection(this.Width, this.Height, out m_ubLat, out m_ubLon);
            DrawArgs.CurrentViewNorthLatitude = m_ltLat.Degrees.CompareTo(double.NaN) == 0 ? 90 : m_ltLat.Degrees;
            DrawArgs.CurrentViewWestLongtitude = m_ltLon.Degrees.CompareTo(double.NaN) == 0 ? -180 : m_ltLon.Degrees;
            DrawArgs.CurrentViewSouthLatitude = m_ubLat.Degrees.CompareTo(double.NaN) == 0 ? -90 : m_ubLat.Degrees;
            DrawArgs.CurrentViewEastLongtitude = m_ubLon.Degrees.CompareTo(double.NaN) == 0 ? 180 : m_ubLon.Degrees;
           
            return new double[]{DrawArgs.CurrentViewWestLongtitude,
                                DrawArgs.CurrentViewNorthLatitude,
                                DrawArgs.CurrentViewEastLongtitude,
                                DrawArgs.CurrentViewSouthLatitude};
        }

        #endregion

        #region  设置相关参数

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
            this.m_DrawArgs.WorldCamera.SetPosition(this.m_DrawArgs.WorldCamera.Latitude.Degrees, this.m_DrawArgs.WorldCamera.Longitude.Degrees, horiz,
                this.m_DrawArgs.WorldCamera.Altitude, vert);
        }

        /// <summary>
        /// 设置所有Layers的透明度
        /// </summary>
        /// <param name="layers"></param>
        public void SetLayers(IList layers)
        {
            //if (layers != null)
            //{
            //    foreach (LayerDescriptor ld in layers)
            //    {
            //        this.CurrentWorld.SetLayerOpacity(ld.Category, ld.Name, (float)ld.Opacity * 0.01f);
            //    }
            //}
        }

        public void SetViewOriginal()
        {
            this.SetViewPosition(30.0, 110.0, 0.0, 12756300.0, 0.0);
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
            this.m_DrawArgs.WorldCamera.SetPosition(degreesLatitude, degreesLongitude, this.m_DrawArgs.WorldCamera.Heading.Degrees,
                metersElevation, this.m_DrawArgs.WorldCamera.Tilt.Degrees);
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
            this.m_DrawArgs.WorldCamera.SetPosition(degreesLatitude, degreesLongitude, heading, altitude, tilt);
        }
        #endregion

        #region  保存截屏

        /// <summary>
        /// Saves the current view to file.
        /// 保存当前视图到文件中
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
                this.m_SaveScreenShotImageFileFormat = (ImageFileFormat)Enum.Parse(typeof(ImageFileFormat), ext, true);
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Unknown file type/file extension for file '" + filePath + "'.  Unable to save.");
            }

            if (!saveFileInfo.Directory.Exists)
                saveFileInfo.Directory.Create();

            this.m_SaveScreenShotFilePath = filePath;
        }

        #endregion

        #region 事件上报

        /// <summary>
        /// 上报控制点修改事件
        /// </summary>
        internal void RaiseGCPModifyEvent(GCPModifyTypeEventArgs e)
        {
            if (GCPModifyEvent != null)
            {
                GCPModifyEvent(this, e);
            }
        }

        #endregion

        #endregion

        #region 渲染

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
                catch (DirectXException ex)
                {
                    MessageBox.Show(ex.Message, "DirectX出现异常");
                }
            }
        }

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
                this.m_DrawArgs.BeginRender();
                //绘制地球的背景色，这里为黑色
                System.Drawing.Color backgroundColor = System.Drawing.Color.Black;
                m_Device3d.Clear(ClearFlags.Target | ClearFlags.ZBuffer, backgroundColor, 1.0f, 0);
                //判断工作线程是否为空
                if (m_WorkerThread == null)
                {
                    //设置工作线程锁
                    m_WorkerThreadRunning = true;
                    //新建一个工作线程
                    m_WorkerThread = new Thread(new ThreadStart(workerThread));
                    //工作线程名称
                    m_WorkerThread.Name = "Qrst.Window.WorkerThread";
                    //设置为后台线程
                    //前台线程与后台线程的区别：.NET Framework 中的所有线程都被指定为前台线程或后台线程。这两种线程唯一的区别是 — 后台线程不会阻止进程终止。在属于一个进程的所有前台线程终止之后，公共语言运行库 (CLR) 就会结束进程，从而终止仍在运行的任何后台线程。
                    m_WorkerThread.IsBackground = true;
                    //设置线程的优先级别：若是在BelowNormal的状态下，则会使绘制的更加平滑，但是在性能好的机器上会特别的慢。
                    World.Settings.UseBelowNormalPriorityUpdateThread = true;
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
                this.m_DrawArgs.WorldCamera.Update(m_Device3d);
                //开始绘制场景
                m_Device3d.BeginScene();
                if (m_IsRenderWireFrame)
                    m_Device3d.RenderState.FillMode = FillMode.WireFrame;//绘制场景的方式是边框图
                else
                    m_Device3d.RenderState.FillMode = FillMode.Solid;//实体图
                m_DrawArgs.RenderWireFrame = m_IsRenderWireFrame;

                //开始绘制当前地球 。
                m_World.Render(this.m_DrawArgs);
                //显示Cross的
                if (World.Settings.ShowCrosshairs)
                    this.RenderCrossHairs();
                //显示Dcsrs图标
                if (World.Settings.ShowCopyright)
                    RenderCopyright();
                //保存当前视区的图像
                if (m_SaveScreenShotFilePath != null)
                    saveScreenShot();
                //显示空间信息
                if (World.Settings.ShowPosition)
                    RenderPositionInfo();

                _menuBar.Render(m_DrawArgs);
                //绘制菜单
                if (World.Settings.ShowLayerManager)
                    m_LayerManagerButton.Render(m_DrawArgs);

                m_DrawArgs.device.RenderState.ZBufferEnable = false;
                if (m_IsRenderWireFrame)
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
                            this.m_DrawArgs.DefaultDrawingFont.DrawText(null,
                                dm.Message, posRect,
                                DrawTextFormat.NoClip | DrawTextFormat.WordBreak,
                                Color.White);
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                    }
                }


                m_Device3d.EndScene();
            }
            catch (Exception ex)
            {
                if (ex is Microsoft.DirectX.Direct3D.InvalidCallException 
                    && ex.TargetSite.Name == "BeginScene")
                {
                    m_Device3d.EndScene();                    
                }

                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.m_DrawArgs.EndRender();
                if (OnGlobeRended!=null)
                {
                    OnGlobeRended(this,new EventArgs());
                }
            }
            //更新鼠标状态
            m_DrawArgs.UpdateMouseCursor(this);
        }


        /// <summary>
        /// 渲染版权信息
        /// </summary>
        protected void RenderCopyright()
        {
            DrawTextFormat dtf = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Left;
            //文本框背景颜色
            int textBackColor = Is3DMapMode ? (int)((uint)(m_TextAlpha << 24) + 0x00ff00u) : m_TextAlpha << 24;
            //文本框字体颜色
            int textForeColor = Is3DMapMode ? (int)((uint)(m_TextAlpha << 24) + 0x00ff00u) : (int)((uint)(m_TextAlpha << 24) + 0xffffffu);
            int x = 5;
            int y = this.Height - 36;
            Rectangle textRect = Rectangle.FromLTRB(x, y, x + 200, y + 36);
            //drawFontText(this.m_CopyrightFont, string.Format("{0}\n{1}", "QRST3DGlobe V1.0", "Copyright © RADI 2013"),
            //                            textRect, dtf, textBackColor, textForeColor);
        }

        /// <summary>
        /// 绘制文本信息,包括：当前经纬度、高程、倾斜角等空间信息
        /// </summary>
        protected void RenderPositionInfo()
        {
            //当前照相机海拔高度计算
            string alt = null;
            double agl = this.m_DrawArgs.WorldCamera.AltitudeAboveTerrain;//高度
            alt = ConvertUnits.GetDisplayAltitudeString(agl);
            //当前显示图层层级计算
            if (m_InitialAltitude == 0.0)
                m_InitialAltitude = agl;
            m_CurrentLevel = ConvertUnits.GetDisplayLevel(m_InitialAltitude, agl);

            string dist = null;
            double dgl = this.m_DrawArgs.WorldCamera.Distance;
            dist = ConvertUnits.GetDisplayAltitudeString(dgl);//高度

            // 倾斜角
            double heading = this.m_DrawArgs.WorldCamera.Heading.Degrees;
            if (heading < 0)
                heading += 360;

            //当前可视区域左上角和右下角经纬度坐标计算
            this.m_DrawArgs.WorldCamera.PickingRayIntersection(0, 0, out m_ltLat, out m_ltLon);
            this.m_DrawArgs.WorldCamera.PickingRayIntersection(this.Width, this.Height, out m_ubLat, out m_ubLon);
            DrawArgs.CurrentViewNorthLatitude = m_ltLat.Degrees.CompareTo(double.NaN) == 0 ? 90 : m_ltLat.Degrees;
            DrawArgs.CurrentViewWestLongtitude = m_ltLon.Degrees.CompareTo(double.NaN) == 0 ? -180 : m_ltLon.Degrees;
            DrawArgs.CurrentViewSouthLatitude = m_ubLat.Degrees.CompareTo(double.NaN) == 0 ? -90 : m_ubLat.Degrees;
            DrawArgs.CurrentViewEastLongtitude = m_ubLon.Degrees.CompareTo(double.NaN) == 0 ? 180 : m_ubLon.Degrees;

            string tempMouseLatPos = DrawArgs.CurrentMouseLatitude.Degrees.ToString("f4");
            if (tempMouseLatPos.Contains("非数字")) tempMouseLatPos = "";
            string tempmouseLonPos = DrawArgs.CurrentMouseLongtitude.Degrees.ToString("f4");
            if (tempmouseLonPos.Contains("非数字")) tempmouseLonPos = "";

            string centerPosition = string.Format("中心点:{0} {1} {2} {3}",
               this.m_DrawArgs.WorldCamera.Latitude.ToStringDms().TrimStart('-'),
               this.m_DrawArgs.WorldCamera.Latitude.GetLatLonStr(true),
               this.m_DrawArgs.WorldCamera.Longitude.ToStringDms().TrimStart('-'),
               this.m_DrawArgs.WorldCamera.Longitude.GetLatLonStr(false));
            string mousePosition = tempMouseLatPos == "" ? "" : string.Format("鼠标:{0} {1} {2} {3}",
                tempMouseLatPos, DrawArgs.CurrentMouseLatitude.GetLatLonStr(true),
                tempmouseLonPos, DrawArgs.CurrentMouseLongtitude.GetLatLonStr(false));

            string aboveSeaLevel = "视角海拔：" + alt;
            string headingString = "视角倾斜：" + heading.ToString("f4") + "°";
            string levelString = "图层层级：" + m_CurrentLevel;
            string lbLatStr = "左上角纬度：" + m_ltLat.Degrees.ToString("f4") + "°";
            string lbLonStr = "左上角经度：" + m_ltLon.Degrees.ToString("f4") + "°";
            string utLatStr = "右下角纬度：" + m_ubLat.Degrees.ToString("f4") + "°";
            string utLonStr = "右下角经度：" + m_ubLon.Degrees.ToString("f4") + "°";

            string distance = "距离：" + this.m_World.ApproxDistance(this.m_DrawArgs.WorldCamera.Latitude, this.m_DrawArgs.WorldCamera.Longitude,
                DrawArgs.CurrentMouseLatitude, DrawArgs.CurrentMouseLongtitude).ToString("f4") + "m\n";

            DrawTextFormat dtf = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Right;
            //文本框背景颜色
            int textBackColor = Is3DMapMode ? (int)((uint)(m_TextAlpha << 24) + 0x00ff00u) : m_TextAlpha << 24;
            //文本框字体颜色
            int textForeColor = Is3DMapMode ? (int)((uint)(m_TextAlpha << 24) + 0x00ff00u) : (int)((uint)(m_TextAlpha << 24) + 0xffffffu);

            //下方显示
            int x = 0;
            int y = this.Height - 20;
            Rectangle textRect;
            textRect = Rectangle.FromLTRB(x, y, this.Width - 270, y + 20);
            drawFontText(this.m_SpatialFont,
                //string.Format("{0}  {1}", mousePosition, centerPosition),
                centerPosition,
                textRect, dtf, textBackColor, textForeColor);
            textRect = Rectangle.FromLTRB(x, y, this.Width - 125, y + 20);
            drawFontText(this.m_SpatialFont, aboveSeaLevel, textRect, dtf, textBackColor, textForeColor);
            textRect = Rectangle.FromLTRB(x, y, this.Width, y + 20);
            drawFontText(this.m_SpatialFont, headingString, textRect, dtf, textBackColor, textForeColor);

        }

        /// <summary>
        /// 绘制中心十字架
        /// </summary>
        protected void RenderCrossHairs()
        {
            //m_CrossHairColor = Is3DMapModel ? Color.Black.ToArgb() : Color.WhiteSmoke.ToArgb();
            //十字架的Size
            int crossHairSize = 2;
            //创建十字架对象
            if (this.m_CrossHairs == null)
            {
                m_CrossHairs = new Line(m_Device3d);
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

            m_CrossHairs.Width = 2;
            m_CrossHairs.Begin();
            m_CrossHairs.Draw(left, m_CrossHairColor);
            m_CrossHairs.Draw(right, m_CrossHairColor);
            m_CrossHairs.Draw(top, m_CrossHairColor);
            m_CrossHairs.Draw(bottom, m_CrossHairColor);
            //crossHairs.Draw(center, crossHairColor);
            m_CrossHairs.End();
        }

        /// <summary>
        /// 绘制图标
        /// </summary>
        /// <param name="textureFileName"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        protected void RenderIcon(string textureFileName, int positionX, int positionY)
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
                    iconTexture = loadImage(m_Device3d, image, ref iconWidth, ref iconHeight);
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
            Sprite m_sprite = new Sprite(m_Device3d);
            m_sprite.Begin(SpriteFlags.AlphaBlend);
            if (iconTexture != null)
            {
                // Render icon
                float xscale = 0.8f;
                float yscale = 0.8f;
                m_sprite.Transform = Matrix.Scaling(xscale, yscale, 0);

                if (iconIsRotated)
                    m_sprite.Transform *= Matrix.RotationZ((float)rotatedRadians - (float)m_DrawArgs.WorldCamera.Heading.Radians);

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

        #endregion

        #region 设备操作

        /// <summary>
        /// 初始化绘制图形,也就是Device对象的初始化
        /// </summary>
        private void InitializeGraphics()
        {
            // 建立我们的呈现参数
            m_PresentParams = new PresentParameters();

            m_PresentParams.Windowed = true;
            m_PresentParams.SwapEffect = SwapEffect.Discard;
            m_PresentParams.AutoDepthStencilFormat = DepthFormat.D16;
            m_PresentParams.EnableAutoDepthStencil = true;

            int adapterOrdinal = 0;
            try
            {
                // Store the default adapter
                adapterOrdinal = Manager.Adapters.Default.Adapter;
            }
            catch (NotAvailableException naex)
            {
                //MessageBox.Show(naex.ToString());
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
                m_Device3d = new Device(adapterOrdinal, dType, this, flags, m_PresentParams);
            }
            catch (Microsoft.DirectX.DirectXException ex)
            {
                throw new NotSupportedException("Unable to create the Direct3D m_Device3d.");
            }

            // Hook the m_Device3d reset event
            m_Device3d.DeviceReset += new EventHandler(OnDeviceReset);
            m_Device3d.DeviceResizing += new CancelEventHandler(m_Device3d_DeviceResizing);
            OnDeviceReset(m_Device3d, null);
        }

        /// <summary>
        /// 处理设备重置时的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 处理设备重新设置大小时的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_Device3d_DeviceResizing(object sender, CancelEventArgs e)
        {
            if (this.Size.Width == 0 || this.Size.Height == 0)
            {
                e.Cancel = true;
                return;
            }

            this.m_DrawArgs.ScreenHeight = this.Height;
            this.m_DrawArgs.ScreenWidth = this.Width;
        }

        /// <summary>
        /// 试图回复3D Device.
        /// </summary>
        private void AttemptRecovery()
        {
            try
            {
                m_Device3d.TestCooperativeLevel();
            }
            catch (DeviceLostException dlex)
            {
                //MessageBox.Show(dlex.ToString());
            }
            catch (DeviceNotResetException)
            {
                try
                {
                    m_Device3d.Reset(m_PresentParams);
                }
                catch (DeviceLostException dlex)
                {
                    //MessageBox.Show(dlex.ToString());
                }
            }
        }

        #endregion

        #region 窗体自身事件处理

        /// <summary>
        /// 应用程序空闲事件
        /// </summary>
        private void OnApplicationIdle(object sender, EventArgs e)
        {
            try
            {
                while (IsAppStillIdle)
                {
                    if (!World.Settings.AlwaysRenderWindow && m_IsRenderDisabled && !World.Settings.CameraHasMomentum)
                        return;
                    Render();
                    m_DrawArgs.Present();
                }
            }
            catch (DeviceLostException)
            {
                AttemptRecovery();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (WebException we)
            {
                MessageBox.Show(we.Message);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        #region 鼠标事件的处理

        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                //判断鼠标的区域范围是否在LayerManager的区域范围内，若是，则返回，不做任何操作
                if (this._menuBar != null && this._menuBar.OnMouseWheel(e))
                    return;

                //Zoom照相机
                this.m_DrawArgs.WorldCamera.ZoomStepped(e.Delta / 120.0f);
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

            //拖动GCP时采用自身的鼠标按下事件
            if (m_IsGCPDragState)
                return;

            //设置当前窗体Focus
            this.Focus();
            DrawArgs.LastMousePosition.X = e.X;
            DrawArgs.LastMousePosition.Y = e.Y;

            m_MouseDownStartPosition.X = e.X;
            m_MouseDownStartPosition.Y = e.Y;


            try
            {
                bool handled = false;

                if (!handled)
                {
                    if (this._menuBar != null && !this._menuBar.OnMouseDown(e))
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
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            //if (mouseToolUsing)
            //{
            //    base.OnMouseDoubleClick(e);
            //}


            m_IsDoubleClick = true;
            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            //if (mouseToolUsing)
            //{
            //    base.OnMouseUp(e);
            //}


            DrawArgs.LastMousePosition.X = e.X;
            DrawArgs.LastMousePosition.Y = e.Y;

            try
            {
                bool handled = false;

                if (!handled)
                {
                    // 若鼠标点击不在球的范围内，则不处理
                    if (m_MouseDownStartPosition == Point.Empty)
                        return;

                    m_MouseDownStartPosition = Point.Empty;
                    if (!this.m_IsMouseDragging)
                    {
                        //处理拖拽图层列表工具栏事件
                        if (this._menuBar != null && this._menuBar.OnMouseUp(e))
                            return;
                    }
                    //若当前世界对象为空，则不处理
                    if (m_World == null)
                        return;
                    //判断是不是鼠标双击事件，此处处理鼠标双击事件
                    if (m_IsDoubleClick)
                    {
                        m_IsDoubleClick = false;
                        //处于绘制工具绘制图形状态时，不进行图层的缩放
                        if (World.Settings.CurrentWwTool != null)
                            return;
                        if (e.Button == MouseButtons.Left)//若是鼠标左键双击，则缩放ZOOMIN
                        {
                            m_DrawArgs.WorldCamera.Zoom(World.Settings.CameraDoubleClickZoomFactor);
                        }
                        else if (e.Button == MouseButtons.Right)//若是鼠标右键双击，则缩放ZOOMOUT
                        {
                            m_DrawArgs.WorldCamera.Zoom(-World.Settings.CameraDoubleClickZoomFactor);
                        }

                    }//处理鼠标单击事件
                    else
                    {
                        if (e.Button == MouseButtons.Left)//处理鼠标左键
                        {
                            if (this.m_IsMouseDragging)
                            {
                                this.m_IsMouseDragging = false;
                            }
                            else
                            {
                                if (m_World.PerformSelectionAction(this.m_DrawArgs))//处理世界对象下的所有图层的鼠标点击事件PerformSelectionAction
                                {
                                    //ZYM:20130924-上报数据检索瓦片范围框选中事件
                                    if (DBSearchTileExtentsSelectedEvent != null)
                                    {
                                        DBSearchTileExtentsSelectedEvent(this, null);
                                    }
                                }
                            }
                        }
                        else if (e.Button == MouseButtons.Right)//处理鼠标右键
                        {
                            if (this.m_IsMouseDragging)
                                this.m_IsMouseDragging = false;
                            else
                            {
                                if (!m_World.PerformSelectionAction(this.m_DrawArgs))
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

            //拖动GCP时采用自身的鼠标按下事件
            if (m_IsGCPDragState && m_DraggingGCP != null)
            {
                //计算当前鼠标点所对应的经纬度信息
                Angle curLat, curLon;
                DrawArgs.WorldCamera.PickingRayIntersection(
                    e.X,
                    e.Y,
                    out curLat,
                    out curLon);
                m_DraggingGCP.SetPosition(curLat.Degrees, curLon.Degrees);
                return;
            }

            //设置鼠标移动的图标
            DrawArgs.MouseCursor = CursorType.Arrow;

            try
            {
                bool handled = false;


                if (!handled)
                {
                    int deltaX = e.X - DrawArgs.LastMousePosition.X;
                    int deltaY = e.Y - DrawArgs.LastMousePosition.Y;
                    float deltaXNormalized = (float)deltaX / m_DrawArgs.ScreenWidth;
                    float deltaYNormalized = (float)deltaY / m_DrawArgs.ScreenHeight;

                    //计算当前鼠标点所对应的经纬度信息
                    Angle curLat, curLon;
                    this.m_DrawArgs.WorldCamera.PickingRayIntersection(
                        e.X,
                        e.Y,
                        out curLat,
                        out curLon);
                    DrawArgs.CurrentMousePosition = new Point(e.X, e.Y);
                    DrawArgs.CurrentMouseLatitude = curLat;
                    DrawArgs.CurrentMouseLongtitude = curLon;

                    if (!this.m_IsMouseDragging)
                    {
                        //处理LayerManager的拖拽拉大或拉小对象
                        if (this._menuBar != null && this._menuBar.OnMouseMove(e))
                        {
                            base.OnMouseMove(e);
                            return;
                        }
                    }
                    //若之前没有记录鼠标按下的操作，则不进行任何处理，返回
                    if (m_MouseDownStartPosition == Point.Empty)
                        return;
                    //判断之前按下的是鼠标左键还是右键
                    bool isMouseLeftButtonDown = ((int)e.Button & (int)MouseButtons.Left) != 0;
                    bool isMouseRightButtonDown = ((int)e.Button & (int)MouseButtons.Right) != 0;

                    //设置鼠标拖拽地球标志为True，在MouseUp事件里捕获，进行处理
                    if (isMouseLeftButtonDown || isMouseRightButtonDown)
                    {
                        int dx = this.m_MouseDownStartPosition.X - e.X;
                        int dy = this.m_MouseDownStartPosition.Y - e.Y;
                        int distanceSquared = dx * dx + dy * dy;
                        if (distanceSquared > 3 * 3)
                            this.m_IsMouseDragging = true;
                    }
                    //鼠标左键操作，则进行地球拖拽
                    if (isMouseLeftButtonDown && !isMouseRightButtonDown)
                    {
                        Angle prevLat, prevLon;
                        this.m_DrawArgs.WorldCamera.PickingRayIntersection(
                            DrawArgs.LastMousePosition.X,
                            DrawArgs.LastMousePosition.Y,
                            out prevLat,
                            out prevLon);

                        double factor = (this.m_DrawArgs.WorldCamera.Altitude) / (1500 * this.CurrentWorld.EquatorialRadius);
                        m_DrawArgs.WorldCamera.RotationYawPitchRoll(
                            Angle.FromRadians(DrawArgs.LastMousePosition.X - e.X) * factor,
                            Angle.FromRadians(e.Y - DrawArgs.LastMousePosition.Y) * factor,
                            Angle.Zero);

                    }
                    else if (!isMouseLeftButtonDown && isMouseRightButtonDown)//若是右键，则是进行地球的方向改变操作
                    {
                        Angle deltaEyeDirection = Angle.FromRadians(-deltaXNormalized * World.Settings.CameraRotationSpeed);
                        this.m_DrawArgs.WorldCamera.RotationYawPitchRoll(Angle.Zero, Angle.Zero, deltaEyeDirection);
                        this.m_DrawArgs.WorldCamera.Tilt += Angle.FromRadians(deltaYNormalized * World.Settings.CameraRotationSpeed);
                    }
                    else if (isMouseLeftButtonDown && isMouseRightButtonDown)//若是左键或右键同时操作，则进行地球的缩放
                    {
                        // Both buttons (zoom)
                        if (Math.Abs(deltaYNormalized) > float.Epsilon)
                            this.m_DrawArgs.WorldCamera.Zoom(-deltaYNormalized);

                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {

                this.m_DrawArgs.WorldCamera.PickingRayIntersection(
                    e.X,
                    e.Y,
                    out cLat,
                    out cLon);

                DrawArgs.LastMousePosition.X = e.X;
                DrawArgs.LastMousePosition.Y = e.Y;
                base.OnMouseMove(e);
            }
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            //if (mouseToolUsing)
            //{
            //    base.OnMouseLeave(e);
            //}

            if (_menuBar != null)
                // reset menu bar mouse hover state.
                _menuBar.OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, -1, -1, 0));
            base.OnMouseLeave(e);
        }

        #endregion


        #endregion

        #region 私有方法

        /// <summary>
        /// 配置地球控件的属性
        /// </summary>
        private void configrationQrstGlobe()
        {
            DateTime dtNowTime = DateTime.Now;
            DateTime CacheCleanupTime = dtNowTime.AddHours(1);

            //设置缓存的最大，最小容量
            long CacheUpperLimit = (long)Convert.ToInt32(m_EarthConfig.ConnectionStrings.ConnectionStrings["CacheUpperLimit"].ToString()) * 1024L * 1024L * 1024L;    //最大容量
            long CacheLowerLimit = (long)Convert.ToInt32(m_EarthConfig.ConnectionStrings.ConnectionStrings["CacheLowerLimit"].ToString()) * 1024L * 1024L * 1024L;	//最小容量

            DateTime dtStartTime = DateTime.Now;

            //把缓存信息放到QrstWindow中去。
            this.Cache = new Cache(
                CacheDirectory,//缓存保存的跟目录
                CacheLowerLimit,//缓存文件夹的最高上线
                CacheUpperLimit,//缓存文件夹的最低上线
                CacheCleanupTime - dtNowTime,//缓存保留的时间
                CacheCleanupTime - dtNowTime);//当前开始的时间
            //是否有log404错误。
            WebDownload.Log404Errors = World.Settings.Log404Errors;
            this.BackColor = Color.AliceBlue;
        }

        /// <summary>
        /// 配置地球对象
        /// </summary>
        private void initializeQrstPlanet()
        {
            #region 创建数字高程图层

            World.Settings.VerticalExaggeration = 3.0f;//夸大因子
            //地形区域的范围：这里只显示中国区域的地形图（DEM数据太大，所有没放全球的）
            double north = Convert.ToDouble(m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_North"].ToString());
            double south = Convert.ToDouble(m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_South"].ToString());
            double west = Convert.ToDouble(m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_West"].ToString());
            double east = Convert.ToDouble(m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_East"].ToString());
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
            serverUrl = m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_Url"].ToString();
            dataSetName = m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_Dataset"].ToString();
            levelZeroTileSizeDegrees = Convert.ToDouble(m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_levelZeroTileSizeDegrees"].ToString());
            numberLevels = (uint)Convert.ToInt32(m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_numberLevels"].ToString());
            samplesPerTile = 150;
            dataFormat = "Int16";
            fileExtension = "bil";
            string terrainLayerName = m_EarthConfig.ConnectionStrings.ConnectionStrings["DEM_LayerName"].ToString();
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
        /// 初始化插件
        /// </summary>
        private void initializePluginCompiler()
        {
            m_Compiler = new PluginCompiler(this, m_PluginsDir);

            //#if DEBUG
            //从当前程序集中发现并加载插件
            m_Compiler.FindPlugins(Assembly.GetExecutingAssembly());
            //#endif

            //从“Plugins”目录中加载插件
            m_Compiler.FindPlugins();
            //  ZYM:20130708-修改为在单击工具时再加载插件。
            //加载启动插件
            //m_Compiler.LoadStartupPlugins();
        }

        /// <summary>
        /// 底图数据获取
        /// </summary>
        /// <param name="parentWorld"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        private RenderableObjectList getBlueMarble(World parentWorld, Cache cache)
        {

            //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
            ImageLayer BlueMarbleBase = new ImageLayer
                (
                "Blue Marble BaseMap",
                parentWorld,
                0,
                null,
                -90, 90, -180, 180, 1.0f, parentWorld.TerrainAccessor
                );
            //底图的路径
            BlueMarbleBase.ImagePath = Path.Combine(this.DataDirectory, @"Earth\BMNG_world.topo.bathy.200405.jpg");

            RenderableObjectList renderableCollection = new RenderableObjectList("世界地图0");
            renderableCollection.Add(BlueMarbleBase);
            return renderableCollection;
        }

        public RenderableObjectList getbaseimage(World parentWorld, Cache cache)
        {
            //Blue Marble 切片图层
            RenderableObjectList parentRenderable = new RenderableObjectList("世界地图1");
            parentRenderable.ParentList = parentWorld.RenderableObjects;
            //是否显示此图层
            parentRenderable.IsOn = true;
            parentRenderable.DisableExpansion = false;
            parentRenderable.IsShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象
            parentRenderable.MetaData.Add("World", parentWorld);
            parentRenderable.MetaData.Add("Cache", cache);
            //此图层做为表面图层
            parentRenderable.RenderPriority = RenderPriority.SurfaceImages;

            //是否显示数字高程
            bool terrainMapped = true;
            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;

            //切片图层对象
            ImageStore ia = new ImageStore();
            ia.LevelZeroTileSizeDegrees = 36;
            ia.LevelCount = 32;
            ia.CacheDirectory = Application.StartupPath + @"\Cache\BMNGWMS2\";
            ia.ServerLogo = Path.Combine(this.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                "BMNGWMS2",
                this.CurrentWorld,
                0,
                90,
                -90,
                -180,
                180,
                terrainMapped,
                imageStores
            );
            qts.ServerLogoFilePath = ia.ServerLogo;
            qts.CacheExpirationTime = dataExpiration;

            ////透明色
            //Color c = Color.FromArgb(opacity, backgroundData, backgroundData, backgroundData);//Color [A=255, R=0, G=0, B=0]
            //qts.ColorKey = c.ToArgb();

            qts.ParentList = parentRenderable;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)parentRenderable.MetaData["XmlSource"]);
            parentRenderable.Add(qts);

            userTilesAdd(parentRenderable, 0.02197266, 3, Application.StartupPath + @"\Cache\Taiyuan2m\", "Taiyuan2m", 38.42048250, 37.45539450, 111.50814187, 113.14813987);

            userTilesAdd(parentRenderable, 0.02197266, 3, Application.StartupPath + @"\Cache\Beijing2m\", "Beijing2m", 41.05902584, 39.43987184, 115.41341742, 117.50636742);

            return parentRenderable;
        }

        private void userTilesAdd(RenderableObjectList parentRenderable,double pLevelZeroTileSizeDegrees,int pLevelCount,string pCachePath,string pLayerName,double pMaxLat,double pMinLat,double pMinLon,double pMaxLon)
        {
           
            //是否显示数字高程
            bool terrainMapped = true;
            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;

            //切片图层对象
            ImageStore ia = new ImageStore();
            ia.LevelZeroTileSizeDegrees = pLevelZeroTileSizeDegrees;
            ia.LevelCount = pLevelCount;
            ia.CacheDirectory = pCachePath;
            ia.ServerLogo = Path.Combine(this.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                pLayerName,
                this.CurrentWorld,
                0,
                pMaxLat,
                pMinLat,
                pMinLon,
                pMaxLon,
                terrainMapped,
                imageStores
            );
            qts.ServerLogoFilePath = ia.ServerLogo;
            qts.CacheExpirationTime = dataExpiration;

            ////透明色
            //Color c = Color.FromArgb(opacity, backgroundData, backgroundData, backgroundData);//Color [A=255, R=0, G=0, B=0]
            //qts.ColorKey = c.ToArgb();

            qts.ParentList = parentRenderable;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)parentRenderable.MetaData["XmlSource"]);
            parentRenderable.Add(qts);
            
        }

        

        /// <summary>
        /// 获取三维地图模式的白色底图
        /// </summary>
        /// <param name="parentWorld"></param>
        /// <returns></returns>
        private RenderableObjectList getWhiteWorld(World parentWorld)
        {
            //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
            ImageLayer MapBase = new ImageLayer
                (
                "三维地图底图",
                parentWorld,
                0,
                null,
                -90, 90, -180, 180, 255
                );
            //底图的路径
            MapBase.ImagePath = Path.Combine(this.DataDirectory, @"Earth\WhiteWorld.jpg");
            MapBase.IsOn = false;
            RenderableObjectList renderableCollection = new RenderableObjectList("三维地图");
            renderableCollection.Add(MapBase);
            renderableCollection.IsOn = false;  //初始不显示此图层
            return renderableCollection;
        }

        /// <summary>
        /// 判断是否是窗体设计模式
        /// </summary>
        /// <returns></returns>
        private bool isInDesignMode()
        {
            string applicationExe = Application.ExecutablePath.ToUpper(CultureInfo.InvariantCulture);
            bool result = applicationExe.EndsWith("DEVENV.EXE");
            return result;
        }

        /// <summary>
        /// 判断球体绘制是否准备好
        /// </summary>
        /// <returns></returns>
        private bool isWorldReady()
        {
            if (this.m_World == null || this.m_DrawArgs == null || this.m_DrawArgs.WorldCamera == null)
                return false;
            else
                return true;
        }


        /// <summary>
        /// 加载影像
        /// </summary>
        /// <param name="device"></param>
        /// <param name="image"></param>
        /// <param name="iconWidth"></param>
        /// <param name="iconHeight"></param>
        /// <returns></returns>
        private Texture loadImage(Device device, Image image, ref int iconWidth, ref int iconHeight)
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
        /// 绘制文本
        /// </summary>
        /// <param name="font">文本字体对象</param>
        /// <param name="drawText">要绘制的文本内容</param>
        /// <param name="rectangle">文本所在的矩形框</param>
        /// <param name="drawTextFormat">文本格式</param>
        /// <param name="backColor">文本的背景颜色</param>
        /// <param name="foreColor">文本的前景颜色</param>
        private void drawFontText(Microsoft.DirectX.Direct3D.Font font, string drawText, Rectangle rectangle,
            DrawTextFormat drawTextFormat, int backColor, int foreColor)
        {
            font.DrawText(null, drawText, rectangle, drawTextFormat, backColor);
            font.DrawText(null, drawText, rectangle, drawTextFormat, foreColor);
        }

        /// <summary>
        /// 后台更新的线程
        /// </summary>
        private void workerThread()
        {
            //设置更新事件，这里设置每秒钟更新6次
            const int refreshIntervalMs = 150;

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
                    m_World.Update(this.m_DrawArgs);

                    //记录更新后的当前事件
                    long endTicks = 0;
                    PerformanceTimer.QueryPerformanceCounter(ref endTicks);

                    //计算两次事件的间隔。
                    float elapsedMilliSeconds = 1000 * (float)(endTicks - startTicks) / PerformanceTimer.TicksPerSecond;
                    float remaining = refreshIntervalMs - elapsedMilliSeconds;
                    //若两次事件的间隔>0的话，则证明，还没有到刷新的时间，就让线程进入睡眠状态
                    if (remaining > 0)
                        Thread.Sleep((int)remaining);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 保存屏幕截图
        /// </summary>
        private void saveScreenShot()
        {
            try
            {
                using (Surface backbuffer = m_Device3d.GetBackBuffer(0, 0, BackBufferType.Mono))
                    SurfaceLoader.Save(m_SaveScreenShotFilePath, m_SaveScreenShotImageFileFormat, backbuffer);
                MessageBox.Show(string.Format("三维球体屏幕截图文件位置：\n\"{0}\"", m_SaveScreenShotFilePath), "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                m_SaveScreenShotFilePath = null;
            }
            catch (InvalidCallException caught)
            {
                MessageBox.Show(caught.Message, "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
