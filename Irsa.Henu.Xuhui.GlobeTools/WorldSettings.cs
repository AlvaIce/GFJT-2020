using Microsoft.DirectX.Direct3D;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Qrst
{

    /// <summary>
    /// 地球属性设置
    /// </summary>
    public class WorldSettings
    {
        #region 杂项设置
        /// <summary>
        /// 获取或设置时间加速比
        /// </summary>
        public float TimeMultiplier
        {
            get { return TimeKeeper.TimeMultiplier; }
            set { TimeKeeper.TimeMultiplier = value; }
        }

        internal int maxSimultaneousDownloads = 1;
        /// <summary>
        /// 获取或设置最大同时下载个数
        /// </summary>
        public int MaxSimultaneousDownloads
        {
            get { return maxSimultaneousDownloads; }
            set
            {
                if (value > 20)
                    maxSimultaneousDownloads = 20;
                else if (value < 1)
                    maxSimultaneousDownloads = 1;
                else
                    maxSimultaneousDownloads = value;
            }
        }

        private TimeSpan terrainTileRetryInterval = TimeSpan.FromMinutes(30);
        /// <summary>
        /// 获取或设置数字高程图像的重新下载时间.最小是1分钟.
        /// </summary>
        public TimeSpan TerrainTileRetryInterval
        {
            get
            {
                return terrainTileRetryInterval;
            }
            set
            {
                TimeSpan minimum = TimeSpan.FromMinutes(1);
                if (value < minimum)
                    value = minimum;
                terrainTileRetryInterval = value;
            }
        }

        internal int downloadQueuedColor = Color.FromArgb(50, 128, 168, 128).ToArgb();
        /// <summary>
        /// 获取或设置下载数据的进度槽的颜色.
        /// </summary>
        public Color DownloadQueuedColor
        {
            get { return Color.FromArgb(downloadQueuedColor); }
            set { downloadQueuedColor = value.ToArgb(); }
        }

        private Units m_displayUnits = Units.Metric;
        /// <summary>
        /// 获取或设置计量单位，默认是米
        /// </summary>
        public Units DisplayUnits
        {
            get
            {
                return m_displayUnits;
            }
            set
            {
                m_displayUnits = value;
            }
        }

        /// <summary>
        /// 返回Log404错误
        /// </summary>
        public bool Log404Errors
        {
            get { return Qrst.Net.WebDownload.Log404Errors; }
            set { Qrst.Net.WebDownload.Log404Errors = value; }
        }

        #endregion

        #region 大气散射效果设置
        internal bool enableAtmosphericScattering = false;
        /// <summary>
        /// 属性：是否允许大气散射效果.
        /// </summary>
        public bool EnableAtmosphericScattering
        {
            get { return enableAtmosphericScattering; }

            set { enableAtmosphericScattering = value; }
        }

        internal bool forceCpuAtmosphere = true;
        /// <summary>
        /// 属性：是否强迫CPU替代GPU计算大气散射.
        /// </summary>
        public bool ForceCpuAtmosphere
        {
            get { return forceCpuAtmosphere; }
            set { forceCpuAtmosphere = value; }
        }

        #endregion

        #region 默认字体设置||Tocbar设置||下载图标颜色设置|

        #region 默认字体设置
        /// <summary>
        /// 默认字体名称
        /// </summary>
        internal string defaultFontName = "Tahoma";

        /// <summary>
        /// 默认字体大小
        /// </summary>
        internal float defaultFontSize = 9.0f;

        /// <summary>
        /// 默认字体格式：加粗、斜线、正常
        /// </summary>
        internal FontStyle defaultFontStyle = FontStyle.Regular;

        #endregion

        #region Tocbar设置

        /// <summary>
        /// 显示TocBar
        /// </summary>
        public bool showLayerManager = false;
        /// <summary>
        /// Toc窗体中显示Layer的字体
        /// </summary>
        internal string layerManagerFontName = "微软雅黑";

        /// <summary>
        /// Toc窗体中显示Layer的字体的大小
        /// </summary>
        internal float layerManagerFontSize = 10;

        /// <summary>
        /// Toc窗体中显示Layer的字体的样式：加粗、斜线、正常
        /// </summary>
        internal FontStyle layerManagerFontStyle = FontStyle.Regular;
        /// <summary>
        /// Toc窗体的大小
        /// </summary>
        internal int layerManagerWidth = 300;

        /// <summary>
        /// TocBar的背景色
        /// </summary>
        internal int menuBackColor = Color.FromArgb(150, 40, 40, 40).ToArgb();
        /// <summary>
        /// Tocbar的边框
        /// </summary>
        internal int menuOutlineColor = Color.FromArgb(200, 132, 157, 189).ToArgb();
        /// <summary>
        /// Tocbar的Scrollbar的颜色
        /// </summary>
        internal int scrollbarColor = System.Drawing.Color.FromArgb(200, 132, 157, 189).ToArgb();
        /// <summary>
        /// Tocbar的Scrollbar被点中后的颜色
        /// </summary>
        internal int scrollbarHotColor = System.Drawing.Color.FromArgb(170, 255, 255, 255).ToArgb();

        #endregion

        /// <summary>
        /// 显示中心十字架
        /// </summary>
        public bool showCrosshairs = false;
        /// <summary>
        /// 平滑字体设置，是否显示平滑字体
        /// </summary>
        internal bool antiAliasedText = false;
        /// <summary>
        /// 球的每秒刷新次数，默认是50次
        /// </summary>
        internal int throttleFpsHz = 50;
        /// <summary>
        /// 是否显示下载图标
        /// </summary>
        internal bool showDownloadIndicator = true;


        internal int downloadTerrainRectangleColor = Color.FromArgb(50, 0, 0, 255).ToArgb();
        internal int downloadProgressColor = Color.FromArgb(50, 255, 0, 0).ToArgb();
        internal int downloadLogoColor = Color.FromArgb(180, 255, 255, 255).ToArgb();

        /// <summary>
        /// 是否显示下载图标
        /// </summary>
        public bool ShowDownloadIndicator
        {
            get { return showDownloadIndicator; }
            set { showDownloadIndicator = value; }
        }
        /// <summary>
        /// Tocbar的Scrollbar被点中后的颜色
        /// </summary>
        public Color ScrollbarHotColor
        {
            get { return Color.FromArgb(scrollbarHotColor); }
            set { scrollbarHotColor = value.ToArgb(); }
        }
        /// <summary>
        /// Tocbar的Scrollbar的颜色
        /// </summary>
        public Color ScrollbarColor
        {
            get { return Color.FromArgb(scrollbarColor); }
            set { scrollbarColor = value.ToArgb(); }
        }
        /// <summary>
        /// Tocbar的边框
        /// </summary>
        public Color MenuOutlineColor
        {
            get { return Color.FromArgb(menuOutlineColor); }
            set { menuOutlineColor = value.ToArgb(); }
        }
        /// <summary>
        /// TocBar的背景色
        /// </summary>
        public Color MenuBackColor
        {
            get { return Color.FromArgb(menuBackColor); }
            set { menuBackColor = value.ToArgb(); }
        }
        /// <summary>
        /// 下载Logo的颜色
        /// </summary>
        public Color DownloadLogoColor
        {
            get { return Color.FromArgb(downloadLogoColor); }
            set { downloadLogoColor = value.ToArgb(); }
        }
        /// <summary>
        /// 下载Logo进度的颜色
        /// </summary>
        public Color DownloadProgressColor
        {
            get { return Color.FromArgb(downloadProgressColor); }
            set { downloadProgressColor = value.ToArgb(); }
        }
        /// <summary>
        /// 下载高程进度的颜色
        /// </summary>
        public Color DownloadTerrainRectangleColor
        {
            get { return Color.FromArgb(downloadTerrainRectangleColor); }
            set { downloadTerrainRectangleColor = value.ToArgb(); }
        }
        /// <summary>
        /// 获取或设置显示TocBar
        /// </summary>
        public bool ShowLayerManager
        {
            get { return showLayerManager; }
            set { showLayerManager = value; }
        }
        /// <summary>
        /// 获取或设置显示中心区十字
        /// </summary>
        public bool ShowCrosshairs
        {
            get { return showCrosshairs; }
            set { showCrosshairs = value; }
        }
        /// <summary>
        /// 获取或设置默认字体的名称
        /// </summary>
        public string DefaultFontName
        {
            get { return defaultFontName; }
            set { defaultFontName = value; }
        }
        /// <summary>
        /// 获取或设置默认字体的大小
        /// </summary>
        public float DefaultFontSize
        {
            get { return defaultFontSize; }
            set { defaultFontSize = value; }
        }
        /// <summary>
        /// 获取或设置默认字体的样式：加粗|倾斜|正常
        /// </summary>
        public FontStyle DefaultFontStyle
        {
            get { return defaultFontStyle; }
            set { defaultFontStyle = value; }
        }
        /// <summary>
        /// 获取或设置Tocbar的字体名称
        /// </summary>
        public string LayerManagerFontName
        {
            get { return layerManagerFontName; }
            set { layerManagerFontName = value; }
        }
        /// <summary>
        /// 获取或设置Tocbar字体的大小
        /// </summary>
        public float LayerManagerFontSize
        {
            get { return layerManagerFontSize; }
            set { layerManagerFontSize = value; }
        }
        /// <summary>
        /// 获取或设置Tocbar字体的样式：加粗|倾斜|正常
        /// </summary>
        public FontStyle LayerManagerFontStyle
        {
            get { return layerManagerFontStyle; }
            set { layerManagerFontStyle = value; }
        }
        /// <summary>
        /// 获取或设置Tocbar的宽度
        /// </summary>
        public int LayerManagerWidth
        {
            get { return layerManagerWidth; }
            set { layerManagerWidth = value; }
        }
        /// <summary>
        /// 获取或设置是否平滑字体
        /// </summary>
        public bool AntiAliasedText
        {
            get { return antiAliasedText; }
            set { antiAliasedText = value; }
        }
        #endregion

        #region 经纬度格网设置||空间信息显示设置

        /// <summary>
        /// 是否显示经纬网格线，初始为假.
        /// </summary>
        public bool showLatLonLines = false;

        /// <summary>
        /// 经纬线的颜色.
        /// </summary>
        public int latLonLinesColor = System.Drawing.Color.FromArgb(200, 160, 160, 160).ToArgb();

        /// <summary>
        /// 赤道线颜色.
        /// </summary>
        public int equatorLineColor = System.Drawing.Color.FromArgb(230, 255, 199, 183).ToArgb();

        /// <summary>
        /// 是否画回归线
        /// </summary>
        internal bool showTropicLines = true;

        /// <summary>
        /// 回归线的颜色
        /// </summary>
        public int tropicLinesColor = System.Drawing.Color.FromArgb(230, 125, 150, 160).ToArgb();

        /// <summary>
        /// 获取或设置是否显示经纬度格网
        /// </summary>
        public bool ShowLatLonLines
        {
            get { return showLatLonLines; }
            set { showLatLonLines = value; }
        }
        /// <summary>
        /// 获取或设置经纬度格网颜色
        /// </summary>
        public Color LatLonLinesColor
        {
            get { return Color.FromArgb(latLonLinesColor); }
            set { latLonLinesColor = value.ToArgb(); }
        }
        /// <summary>
        /// 获取或设置经纬度格网赤道颜色
        /// </summary>
        public Color EquatorLineColor
        {
            get { return Color.FromArgb(equatorLineColor); }
            set { equatorLineColor = value.ToArgb(); }
        }
        /// <summary>
        /// 获取或设置是否显示纬度格网回归线
        /// </summary>
        public bool ShowTropicLines
        {
            get { return showTropicLines; }
            set { showTropicLines = value; }
        }
        /// <summary>
        /// 获取或设置纬度格网回归线的颜色
        /// </summary>
        public Color TropicLinesColor
        {
            get { return Color.FromArgb(tropicLinesColor); }
            set { tropicLinesColor = value.ToArgb(); }
        }

        /// <summary>
        /// 是否显示位置信息
        /// </summary>
        public bool showPosition = true;
        /// <summary>
        /// 获取或设置空间信息
        /// </summary>
        public bool ShowPosition
        {
            get { return showPosition; }
            set { showPosition = value; }
        }
        #endregion

        #region 摄像机的参数设置
        /// <summary>
        /// 摄像机的纬度
        /// </summary>
        internal Angle cameraLatitude = Angle.FromDegrees(30.0);
        /// <summary>
        /// 摄像机的经度
        /// </summary>
        internal Angle cameraLongitude = Angle.FromDegrees(110.0);
        /// <summary>
        /// 摄像机高度 初始值为10000000米
        /// </summary>
        internal double cameraAltitudeMeters = 10000000;
        /// <summary>
        /// 摄像机横摆
        /// </summary>
        internal Angle cameraHeading = Angle.FromDegrees(0.0);
        /// <summary>
        /// 摄像机倾角
        /// </summary>
        internal Angle cameraTilt = Angle.FromDegrees(0.0);

        /// <summary>
        /// 摄像机鼠标点击点移动
        /// </summary>
        internal bool cameraIsPointGoto = false;
        /// <summary>
        /// 摄像机是否有动量,若为True则，鼠标移动地球后，地球就会朝着那个方向一直转
        /// </summary>
        internal bool cameraHasMomentum = false;

        #region 鼠标移动球后，球会有一个微弱的转动惯性，这两个属性必须都设置为True才行
        /// <summary>
        /// 摄像机是否有惯性.
        /// </summary>
        internal bool cameraHasInertia = true;
        /// <summary>
        /// 摄像机是否平滑
        /// </summary>
        internal bool cameraSmooth = true;

        /// <summary>
        /// 摄像机惯量移动的标准值
        /// </summary>
        internal float cameraSlerpStandard = 0.35f;
        /// <summary>
        /// 摄像机惯量的移动值
        /// </summary>
        internal float cameraSlerpInertia = 0.05f;
        /// <summary>
        /// 摄像机惯量的移动%
        /// </summary>
        internal float cameraSlerpPercentage = 0.05f;
        #endregion

        #region 视场角设置
        /// <summary>
        /// 摄像机瞬时视场角
        /// </summary>
        internal Angle cameraFov = Angle.FromRadians(Math.PI * 0.25f);
        /// <summary>
        /// 摄像机最小视场角  初始为5
        /// </summary>
        internal Angle cameraFovMin = Angle.FromDegrees(5);
        /// <summary>
        /// 摄像机最大视场  初始为150
        /// </summary>
        internal Angle cameraFovMax = Angle.FromDegrees(150);

        #endregion

        #region 摄像机ZoomIn和ZoomOut的速度

        /// <summary>
        /// 摄像机放大缩小系数， 初始化为 0.015
        /// </summary>
        internal float cameraZoomStepFactor = 0.02f;
        /// <summary>
        /// 摄像机放大缩小加速倍数  初始为10
        /// </summary>
        internal float cameraZoomAcceleration = 10f;

        #endregion

        internal float cameraRotationSpeed = 3.5f;
        internal bool elevateCameraLookatPoint = true;

        /// <summary>
        /// 没感觉有什么用
        /// </summary>
        public bool ElevateCameraLookatPoint
        {
            get { return elevateCameraLookatPoint; }
            set { elevateCameraLookatPoint = value; }
        }
        /// <summary>
        /// 获取或设置摄像机的经度
        /// </summary>
        public Angle CameraLatitude
        {
            get { return cameraLatitude; }
            set { cameraLatitude = value; }
        }
        /// <summary>
        /// 获取或设置摄像机的纬度
        /// </summary>
        public Angle CameraLongitude
        {
            get { return cameraLongitude; }
            set { cameraLongitude = value; }
        }
        /// <summary>
        /// 获取或设置摄像机的高度
        /// </summary>
        public double CameraAltitude
        {
            get { return cameraAltitudeMeters; }
            set { cameraAltitudeMeters = value; }
        }
        /// <summary>
        /// 获取或设置摄像机的反转度
        /// </summary>
        public Angle CameraHeading
        {
            get { return cameraHeading; }
            set { cameraHeading = value; }
        }
        /// <summary>
        /// 获取或设置摄像机的倾斜度
        /// </summary>
        public Angle CameraTilt
        {
            get { return cameraTilt; }
            set { cameraTilt = value; }
        }
        /// <summary>
        /// 获取或设置是否可以鼠标点击移动球
        /// </summary>
        public bool CameraIsPointGoto
        {
            get { return cameraIsPointGoto; }
            set { cameraIsPointGoto = value; }
        }
        /// <summary>
        /// 获取或设置是否使相机平滑
        /// </summary>
        public bool CameraSmooth
        {
            get { return cameraSmooth; }
            set { cameraSmooth = value; }
        }
        /// <summary>
        /// 获取或设置摄像机是否有惯量的移动
        /// </summary>
        public bool CameraHasInertia
        {
            get { return cameraHasInertia; }
            set
            {
                cameraHasInertia = value;
                cameraSlerpPercentage = cameraHasInertia ? cameraSlerpInertia : cameraSlerpStandard;
            }
        }
        /// <summary>
        /// 获取或设置球随鼠标的移动而一直移动
        /// </summary>
        public bool CameraHasMomentum
        {
            get { return cameraHasMomentum; }
            set { cameraHasMomentum = value; }
        }
        /// <summary>
        /// 获取或设置球移动的惯量
        /// </summary>
        public float CameraSlerpInertia
        {
            get { return cameraSlerpInertia; }
            set
            {
                cameraSlerpInertia = value;
                if (cameraHasInertia)
                    cameraSlerpPercentage = cameraSlerpInertia;
            }
        }
        /// <summary>
        /// 获取或设置球移动的惯量的标准值
        /// </summary>
        public float CameraSlerpStandard
        {
            get { return cameraSlerpStandard; }
            set
            {
                cameraSlerpStandard = value;
                if (!cameraHasInertia)
                    cameraSlerpPercentage = cameraSlerpStandard;
            }
        }
        /// <summary>
        /// 获取或设置摄像机的Fov值
        /// </summary>
        public Angle CameraFov
        {
            get { return cameraFov; }
            set { cameraFov = value; }
        }
        /// <summary>
        /// 获取或设置摄像机的最小FOV值
        /// </summary>
        public Angle CameraFovMin
        {
            get { return cameraFovMin; }
            set { cameraFovMin = value; }
        }
        /// <summary>
        /// 获取或设置摄像机的最大FOV值
        /// </summary>
        public Angle CameraFovMax
        {
            get { return cameraFovMax; }
            set { cameraFovMax = value; }
        }
        /// <summary>
        /// 获取或设置摄像机Zoom的速率
        /// </summary>
        public float CameraZoomStepFactor
        {
            get { return cameraZoomStepFactor; }
            set
            {
                const float maxValue = 0.3f;
                const float minValue = 1e-4f;

                if (value >= maxValue)
                    value = maxValue;
                if (value <= minValue)
                    value = minValue;
                cameraZoomStepFactor = value;
            }
        }
        /// <summary>
        /// 获取或设置摄像机Zoom的速率的加速比例
        /// </summary>
        public float CameraZoomAcceleration
        {
            get { return cameraZoomAcceleration; }
            set
            {
                const float maxValue = 50f;
                const float minValue = 1f;

                if (value >= maxValue)
                    value = maxValue;
                if (value <= minValue)
                    value = minValue;

                cameraZoomAcceleration = value;
            }
        }

        float m_cameraDoubleClickZoomFactor = 2.0f;
        /// <summary>
        /// 获取或设置摄像机鼠标双击Zoom的值
        /// </summary>
        public float CameraDoubleClickZoomFactor
        {
            get { return m_cameraDoubleClickZoomFactor; }
            set
            {
                m_cameraDoubleClickZoomFactor = value;
            }
        }
        /// <summary>
        /// 获取或设置摄像机旋转的速度
        /// </summary>
        public float CameraRotationSpeed
        {
            get { return cameraRotationSpeed; }
            set { cameraRotationSpeed = value; }
        }

        #endregion

        #region 渲染的属性设置||太阳设置.

        /// <summary>
        ///  纹理格式,默认为DDS格式
        /// </summary>
        private Format textureFormat = Format.Dxt3;
        /// <summary>
        /// 是否使用小于正常优先级的更新线程   初始为假
        /// </summary>
        private bool m_UseBelowNormalPriorityUpdateThread = false;
        /// <summary>
        /// 是否总是渲染窗口   初始为假
        /// </summary>
        private bool m_AlwaysRenderWindow = false;
        /// <summary>
        /// 是否将下载的图像转换为DDS格式
        /// </summary>
        private bool convertDownloadedImagesToDds = true;
        /// <summary>
        /// 获取或设置是否将图像转换为DDS格式文件
        /// </summary>
        public bool ConvertDownloadedImagesToDds
        {
            get
            {
                return convertDownloadedImagesToDds;
            }
            set
            {
                convertDownloadedImagesToDds = value;
            }
        }
        /// <summary>
        /// 获取或设置是否一直渲染Window
        /// </summary>
        public bool AlwaysRenderWindow
        {
            get
            {
                return m_AlwaysRenderWindow;
            }
            set
            {
                m_AlwaysRenderWindow = value;
            }
        }
        /// <summary>
        /// 获取或设置纹理的格式
        /// </summary>
        public Format TextureFormat
        {
            get
            {
                //	return Format.Dxt3;
                return textureFormat;
            }
            set
            {
                textureFormat = value;
            }
        }
        /// <summary>
        /// 获取或设置是否使用在正常优先级以下的更新线程
        /// </summary>
        public bool UseBelowNormalPriorityUpdateThread
        {
            get
            {
                return m_UseBelowNormalPriorityUpdateThread;
            }
            set
            {
                m_UseBelowNormalPriorityUpdateThread = value;
            }
        }

        #region 太阳信息设置
        /// <summary>
        /// 是否能够显示太阳阴影
        /// </summary>
        private bool m_enableSunShading = true;
        /// <summary>
        /// 获取或设置是否显示太阳阴影
        /// </summary>
        public bool EnableSunShading
        {
            get
            {
                return m_enableSunShading;
            }
            set
            {
                m_enableSunShading = value;
            }
        }

        /// <summary>
        /// 时间和太阳同步 标示
        /// </summary>
        private bool m_sunSynchedWithTime = true;
        /// <summary>
        /// 获取或设置时间和太阳是否同步
        /// </summary>
        public bool SunSynchedWithTime
        {
            get
            {
                return m_sunSynchedWithTime;
            }
            set
            {
                m_sunSynchedWithTime = value;
            }
        }
        /// <summary>
        /// 太阳仰角.
        /// </summary>
        private double m_sunElevation = Math.PI / 4;
        /// <summary>
        /// 获取或设置太阳仰角.在太阳位置与时间不同步时.默认为 : Math.PI / 4.
        /// </summary>
        public double SunElevation
        {
            get
            {
                return m_sunElevation;
            }
            set
            {
                m_sunElevation = value;
            }
        }
        /// <summary>
        /// 太阳横摆值
        /// </summary>
        private double m_sunHeading = -Math.PI / 4;
        /// <summary>
        /// 获取或设置太阳横摆值
        /// </summary>
        public double SunHeading
        {
            get
            {
                return m_sunHeading;
            }
            set
            {
                m_sunHeading = value;
            }
        }
        /// <summary>
        /// 太阳距离地球的距离.太阳离地球有1.5亿千米
        /// </summary>
        private double m_sunDistance = 150000000000;
        /// <summary>
        /// 获取或设置太阳距离地球的距离.(米)
        /// </summary>
        public double SunDistance
        {
            get
            {
                return m_sunDistance;
            }
            set
            {
                m_sunDistance = value;
            }
        }

        /// <summary>
        /// 太阳阴影周围环境颜色
        /// </summary>
        private int m_shadingAmbientColor = System.Drawing.Color.FromArgb(50, 50, 50).ToArgb();
        /// <summary>
        /// 获取或设置太阳阴影周围环境颜色
        /// </summary>
        public System.Drawing.Color ShadingAmbientColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(m_shadingAmbientColor);
            }
            set
            {
                m_shadingAmbientColor = value.ToArgb();
            }
        }
        /// <summary>
        /// 标准周围环境颜色
        /// </summary>
        private int m_standardAmbientColor = System.Drawing.Color.FromArgb(64, 64, 64).ToArgb();
        /// <summary>
        /// 获取桌设置太阳标准周围环境颜色
        /// </summary>
        public System.Drawing.Color StandardAmbientColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(m_standardAmbientColor);
            }
            set
            {
                m_standardAmbientColor = value.ToArgb();
            }
        }

        #endregion


        #endregion

        #region 高程属性信息的设置.

        private float minSamplesPerDegree = 3.0f;
        /// <summary>
        /// 获取或设置每度的最小格网数
        /// </summary>
        public float MinSamplesPerDegree
        {
            get
            {
                return minSamplesPerDegree;
            }
            set
            {
                minSamplesPerDegree = value;
            }
        }

        //暂时不明白
        private bool useWorldSurfaceRenderer = true;
        /// <summary>
        /// 获取或设置是否使用地球表面的多种地形可视化渲染映射层，初始为真.（暂时不明白）
        /// </summary>
        public bool UseWorldSurfaceRenderer
        {
            get
            {
                return useWorldSurfaceRenderer;
            }
            set
            {
                useWorldSurfaceRenderer = value;
            }
        }


        private float verticalExaggeration = 3.0f;
        /// <summary>
        /// 获取或设置夸大因子,即,水平与垂直的比. 这里范围是:[1～20].超过这个范围,会抛出异常.
        /// </summary>
        public float VerticalExaggeration
        {
            get
            {
                return verticalExaggeration;
            }
            set
            {
                if (value > 20)
                    throw new ArgumentException("Vertical exaggeration out of range: " + value);
                if (value <= 0)
                    verticalExaggeration = Single.Epsilon;
                else
                    verticalExaggeration = value;
            }
        }

        #endregion

        #region Tools

        //JOKI
        private Object m_CurrentWwTool;

        [Browsable(true), Category("Tools")]
        [Description("Current WorldWind Tool. Object for BaseWwTool")]
        public Object CurrentWwTool
        {
            get
            {
                //	return Format.Dxt3;
                return m_CurrentWwTool;
            }
            set
            {
                if (value==null)
                {
                    QrstAxGlobeControl.mouseToolUsing = false;
                }
                else
                {
                    QrstAxGlobeControl.mouseToolUsing = true;
                }
                m_CurrentWwTool = value;
            }
        }
        #endregion

        #region Measure tool : 测量工具的属性设置

        internal MeasureMode measureMode; //测量类型,是单测量,还是多测量.
        internal bool measureShowGroundTrack = true; //测量时候,是否显示背景色
        internal int measureLineGroundColor = Color.FromArgb(222, 0, 255, 0).ToArgb(); //测量的背景色.
        internal int measureLineLinearColor = Color.FromArgb(255, 255, 0, 0).ToArgb(); //测量的时候,画线的颜色

        /// <summary>
        /// 属性信息：度量线的线色 (类型为Color).
        /// </summary>
        [XmlIgnore]
        [Browsable(true), Category("UI")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [Description("Color of the linear distance measure line.")]
        public Color MeasureLineLinearColor
        {
            get { return Color.FromArgb(measureLineLinearColor); }
            set { measureLineLinearColor = value.ToArgb(); }
        }

        /// <summary>
        /// 属性：度量线的线色XML （类型为int）
        /// </summary>
        [Browsable(false)]
        public int MeasureLineLinearColorXml
        {
            get { return measureLineLinearColor; }
            set { measureLineLinearColor = value; }
        }

        /// <summary>
        /// 属性：度量线底色 (类型为color)
        /// </summary>
        [XmlIgnore]
        [Browsable(true), Category("UI")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [Description("Color of the ground track measure line.")]
        public Color MeasureLineGroundColor
        {
            get { return Color.FromArgb(measureLineGroundColor); }
            set { measureLineGroundColor = value.ToArgb(); }
        }
        /// <summary>
        /// 属性：度量线底色XML (类型为int)
        /// </summary>
        [Browsable(false)]
        public int MeasureLineGroundColorXml
        {
            get { return measureLineGroundColor; }
            set { measureLineGroundColor = value; }
        }
        /// <summary>
        /// 属性：度量时是否显示地面踪迹
        /// </summary>
        [Browsable(true), Category("UI")]
        [Description("Display the ground track column in the measurement statistics table.")]
        public bool MeasureShowGroundTrack
        {
            get { return measureShowGroundTrack; }
            set { measureShowGroundTrack = value; }
        }

        /// <summary>
        /// 属性：度量模式
        /// </summary>
        [Browsable(true), Category("UI")]
        [Description("Measure tool operation mode.")]
        public MeasureMode MeasureMode
        {
            get { return measureMode; }
            set { measureMode = value; }
        }

        #endregion

        public override string ToString()
        {
            return "QrstGlobe";
        }
    }
}