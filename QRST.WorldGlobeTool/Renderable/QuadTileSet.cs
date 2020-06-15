using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Stores;
using QRST.WorldGlobeTool.Utility;
using QRST.WorldGlobeTool.Camera;

namespace QRST.WorldGlobeTool.Renderable
{
    public class QuadTileSet : RenderableObject
    {
        #region 私有变量成员

        public override Extension Extension
        {
            get
            {
                Extension ext = new Extension(this.North,this.South,this.West,this.East);
                return ext;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        bool m_RenderStruts = true;
        /// <summary>
        /// 下载数据的图标的路径.
        /// </summary>
        protected string m_ServerLogoFilePath;
        /// <summary>
        /// 下载数据的图标的Image路径.
        /// </summary>
        protected Image m_ServerLogoImage;
        /// <summary>
        /// 存储最上层的Tiles.
        /// </summary>
        protected Hashtable m_topmostTiles = new Hashtable();
        /// <summary>
        /// 最北方坐标
        /// </summary>
        protected double m_north;
        /// <summary>
        /// 最南方坐标
        /// </summary>
        protected double m_south;
        /// <summary>
        /// 最西方坐标
        /// </summary>
        protected double m_west;
        /// <summary>
        /// 最东方坐标
        /// </summary>
        protected double m_east;
        /// <summary>
        /// 渲染文件名
        /// </summary>
        bool renderFileNames = false;       //true in default
        /// <summary>
        /// 图标的纹理对象
        /// </summary>
        protected Texture m_iconTexture;
        /// <summary>
        /// 
        /// </summary>
        protected Sprite sprite;
        /// <summary>
        /// 
        /// </summary>
        protected Rectangle m_spriteSize;
        /// <summary>
        /// 进度条
        /// </summary>
        protected ProgressBar progressBar;
        /// <summary>
        /// 
        /// </summary>
        protected Blend m_sourceBlend = Blend.BlendFactor;
        /// <summary>
        /// 
        /// </summary>
        protected Blend m_destinationBlend = Blend.InvBlendFactor;
        /// <summary>
        /// If this value equals CurrentFrameStartTicks the Z buffer needs to be cleared
        /// 上次进行绘制的时间.
        /// </summary>
        protected static long lastRenderTime;
        //public static int MaxConcurrentDownloads = 3;
        /// <summary>
        /// 图层的最大半径
        /// </summary>
        protected double m_layerRadius;
        /// <summary>
        /// 是否总是绘制底层切片 
        /// </summary>
        protected bool m_alwaysRenderBaseTiles;
        /// <summary>
        /// 
        /// </summary>
        protected float m_tileDrawSpread;
        /// <summary>
        /// 
        /// </summary>
        protected float m_tileDrawDistance;
        /// <summary>
        /// 是否下载高程信息
        /// </summary>
        protected bool m_isDownloadingElevation;
        /// <summary>
        /// 重试次数
        /// </summary>
        protected int m_numberRetries;
        /// <summary>
        /// 存储下载的请求。
        /// </summary>
        protected Hashtable m_downloadRequests = new Hashtable();
        /// <summary>
        /// 队列的最大存储量
        /// </summary>
        protected int m_maxQueueSize = 400;
        /// <summary>
        /// 是否绘制地形信息
        /// </summary>
        protected bool m_terrainMapped;
        /// <summary>
        /// 远程下载的图片的信息
        /// </summary>
        protected ImageStore[] m_imageStores;
        /// <summary>
        /// 照相机对象
        /// </summary>
        protected Camera.CameraBase m_camera;
        /// <summary>
        /// 地理数据信息下载请求数组
        /// </summary>
        protected GeoSpatialDownloadRequest[] m_activeDownloads = new GeoSpatialDownloadRequest[20];
        /// <summary>
        /// 开始下载时间
        /// </summary>
        protected DateTime[] m_downloadStarted = new DateTime[20];
        /// <summary>
        /// 下载时，远程连接所等待的最大时间，默认2分钟
        /// </summary>
        protected TimeSpan m_connectionWaitTime = TimeSpan.FromMinutes(2);
        /// <summary>
        /// 开始等待的时间
        /// </summary>
        protected DateTime m_connectionWaitStart;
        /// <summary>
        /// 是否进行连接等待中.
        /// </summary>
        protected bool m_isConnectionWaiting;
        /// <summary>
        /// 是否使能颜色键
        /// </summary>
        protected bool m_enableColorKeying;
        /// <summary>
        /// 
        /// </summary>
        protected Effect m_effect = null;
        /// <summary>
        /// 
        /// </summary>
        protected string m_effectPath = null;
        /// <summary>
        /// 
        /// </summary>
        protected string m_effectTechnique = null;
        /// <summary>
        /// 
        /// </summary>
        static protected EffectPool m_effectPool = new EffectPool();
        /// <summary>
        /// 缓寸清空的时间
        /// </summary>
        protected TimeSpan m_cacheExpirationTime = TimeSpan.MaxValue;

        #endregion

        #region 公共变量

        /// <summary>
        /// Texture showing download in progress
        /// </summary>
        public static Texture DownloadInProgressTexture;

        /// <summary>
        /// Texture showing queued download
        /// </summary>
        public static Texture DownloadQueuedTexture;

        /// <summary>
        /// Texture showing terrain download in progress
        /// </summary>
        public static Texture DownloadTerrainTexture;
        /// <summary>
        /// default: 100% transparent black = transparent
        /// </summary>
        public int ColorKey;
        /// <summary>
        /// If a color range is to be transparent this specifies the brightest transparent color.
        /// The darkest transparent color is set using ColorKey.
        /// </summary>
        public int ColorKeyMax;
        /// <summary>
        /// 
        /// </summary>
        bool m_renderGrayscale = false;
        /// <summary>
        /// 
        /// </summary>
        float m_grayscaleBrightness = 0.0f;

        #endregion

        #region  属性

        public float GrayscaleBrightness
        {
            get { return m_grayscaleBrightness; }
            set { m_grayscaleBrightness = value; }
        }

        public bool RenderGrayscale
        {
            get { return m_renderGrayscale; }
            set { m_renderGrayscale = value; }
        }

        public bool RenderStruts
        {
            get { return m_RenderStruts; }
            set { m_RenderStruts = value; }
        }

        /// <summary>
        /// If images in cache are older than expration time a refresh
        /// from server will be attempted.
        /// </summary>
        public TimeSpan CacheExpirationTime
        {
            get
            {
                return this.m_cacheExpirationTime;
            }
            set
            {
                this.m_cacheExpirationTime = value;
            }
        }

        /// <summary>
        /// Path to a thumbnail image (e.g. for use as a download indicator).
        /// </summary>
        public virtual string ServerLogoFilePath
        {
            get
            {
                return m_ServerLogoFilePath;
            }
            set
            {
                m_ServerLogoFilePath = value;
            }
        }

        public bool RenderFileNames
        {
            get
            {
                return renderFileNames;
            }
            set
            {
                renderFileNames = value;
            }
        }

        /// <summary>
        /// The image referenced by ServerLogoFilePath.
        /// </summary>
        public virtual Image ServerLogoImage
        {
            get
            {
                if (m_ServerLogoImage == null)
                {
                    if (m_ServerLogoFilePath == null)
                        return null;
                    try
                    {
                        if (File.Exists(m_ServerLogoFilePath))
                            m_ServerLogoImage = ImageHelper.LoadImage(m_ServerLogoFilePath);
                    }
                    catch { }
                }
                return m_ServerLogoImage;
            }
        }

        /// <summary>
        /// Path to a thumbnail image (e.g. for use as a download indicator).
        /// </summary>
        public virtual bool HasTransparentRange
        {
            get
            {
                return (ColorKeyMax != 0);
            }
        }

        /// <summary>
        /// Source blend when rendering non-opaque layer
        /// </summary>
        public Blend SourceBlend
        {
            get
            {
                return m_sourceBlend;
            }
            set
            {
                m_sourceBlend = value;
            }
        }

        /// <summary>
        /// Destination blend when rendering non-opaque layer
        /// </summary>
        public Blend DestinationBlend
        {
            get
            {
                return m_destinationBlend;
            }
            set
            {
                m_destinationBlend = value;
            }
        }

        /// <summary>
        /// North bound for this QuadTileSet
        /// </summary>
        public double North
        {
            get
            {
                return m_north;
            }
        }

        /// <summary>
        /// West bound for this QuadTileSet
        /// </summary>
        public double West
        {
            get
            {
                return m_west;
            }
        }

        /// <summary>
        /// South bound for this QuadTileSet
        /// </summary>
        public double South
        {
            get
            {
                return m_south;
            }
        }

        /// <summary>
        /// East bound for this QuadTileSet
        /// </summary>
        public double East
        {
            get
            {
                return m_east;
            }
        }

        /// <summary>
        /// Controls if images are rendered using ColorKey (transparent areas)
        /// </summary>
        public bool EnableColorKeying
        {
            get
            {
                return m_enableColorKeying;
            }
            set
            {
                m_enableColorKeying = value;
            }
        }

        public DateTime ConnectionWaitStart
        {
            get
            {
                return m_connectionWaitStart;
            }
        }

        public bool IsConnectionWaiting
        {
            get
            {
                return m_isConnectionWaiting;
            }
        }

        public double LayerRadius
        {
            get
            {
                return m_layerRadius;
            }
            set
            {
                m_layerRadius = value;
            }
        }

        public bool AlwaysRenderBaseTiles
        {
            get
            {
                return m_alwaysRenderBaseTiles;
            }
            set
            {
                m_alwaysRenderBaseTiles = value;
            }
        }

        public float TileDrawSpread
        {
            get
            {
                return m_tileDrawSpread;
            }
            set
            {
                m_tileDrawSpread = value;
            }
        }

        public float TileDrawDistance
        {
            get
            {
                return m_tileDrawDistance;
            }
            set
            {
                m_tileDrawDistance = value;
            }
        }

        public bool IsDownloadingElevation
        {
            get
            {
                return m_isDownloadingElevation;
            }
            set
            {
                m_isDownloadingElevation = value;
            }
        }

        /// <summary>
        /// 获取或设置重复请求次数
        /// </summary>
        public int NumberRetries
        {
            get
            {
                return m_numberRetries;
            }
            set
            {
                m_numberRetries = value;
            }
        }

        /// <summary>
        /// Controls rendering (flat or terrain mapped)
        /// </summary>
        public bool TerrainMapped
        {
            get { return m_terrainMapped; }
            set { m_terrainMapped = value; }
        }

        public ImageStore[] ImageStores
        {
            get
            {
                return m_imageStores;
            }
        }

        /// <summary>
        /// Tiles in the request for download queue
        /// </summary>
        public Hashtable DownloadRequests
        {
            get
            {
                return m_downloadRequests;
            }
        }

        /// <summary>
        /// The camera controlling the layers update logic
        /// </summary>
        public CameraBase Camera
        {
            get
            {
                return m_camera;
            }
            set
            {
                m_camera = value;
            }
        }

        /// <summary>
        /// Path to the effect used to render this tileset; if null, use fixed function pipeline
        /// </summary>
        public string EffectPath
        {
            get
            {
                return m_effectPath;
            }
            set
            {
                m_effectPath = value;
                // can't reload here because we need a valid DX device for that and EffectPath
                // may be set before DX is initialized, so set null and reload in Update()
                m_effect = null;
            }
        }

        /// <summary>
        /// The effect used to render this tileset.
        /// </summary>
        public Effect Effect
        {
            get
            {
                return m_effect;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref= "T:Qrst.Renderable.QuadTileSet"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentWorld"></param>
        /// <param name="distanceAboveSurface"></param>
        /// <param name="north"></param>
        /// <param name="south"></param>
        /// <param name="west"></param>
        /// <param name="east"></param>
        /// <param name="terrainAccessor"></param>
        /// <param name="imageAccessor"></param>
        public QuadTileSet(
                string name,
                World parentWorld,
                double distanceAboveSurface,
                double north,
                double south,
                double west,
                double east,
                bool terrainMapped,
                                        ImageStore[] imageStores)
            : base(name, parentWorld)
        {
            float layerRadius = (float)(parentWorld.EquatorialRadius + distanceAboveSurface);//图层的最大半径
            m_north = north;
            m_south = south;
            m_west = west;
            m_east = east;

            // Layer center position
            //图层中心点的 笛卡儿 坐标
            Position = MathEngine.SphericalToCartesian(
                    (north + south) * 0.5f,
                    (west + east) * 0.5f,
                    layerRadius);

            m_layerRadius = layerRadius;

            m_tileDrawDistance = 3.5f;
            m_tileDrawSpread = 2.9f;
            m_imageStores = imageStores;
            m_terrainMapped = terrainMapped;

            // Default terrain mapped imagery to terrain mapped priority
            if (terrainMapped)
                m_renderPriority = RenderPriority.TerrainMappedImages;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="drawArgs"></param>
        override public void Initialize(DrawArgs drawArgs)
        {
            Camera = DrawArgs.Camera;

            // Initialize download rectangles
            if (DownloadInProgressTexture == null)
                DownloadInProgressTexture = CreateDownloadRectangle(
                        DrawArgs.Device, World.Settings.DownloadProgressColor, 0);
            if (DownloadQueuedTexture == null)
                DownloadQueuedTexture = CreateDownloadRectangle(
                        DrawArgs.Device, World.Settings.DownloadQueuedColor, 0);
            if (DownloadTerrainTexture == null)
                DownloadTerrainTexture = CreateDownloadRectangle(
                        DrawArgs.Device, World.Settings.DownloadTerrainRectangleColor, 0);

            try
            {
                lock (m_topmostTiles.SyncRoot)
                {
                    foreach (QuadTile qt in m_topmostTiles.Values)
                        qt.Initialize();
                }
            }
            catch
            {
            }
            IsInitialized = true;


            if (MetaData.ContainsKey("EffectPath"))
            {
                m_effectPath = MetaData["EffectPath"] as string;
            }
            else
            {
                m_effectPath = null;
            }
            m_effect = null;
        }

        /// <summary>
        /// 处理鼠标事件
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <returns></returns>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            return false;
        }

        /// <summary>
        /// 更新瓦片数据集
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Update(DrawArgs drawArgs)
        {

            if (!IsInitialized)
                Initialize(drawArgs);

            #region 暂时没有使用

            if (m_effectPath != null && m_effect == null)
            {
                string errs = string.Empty;
                m_effect = Effect.FromFile(DrawArgs.Device, m_effectPath, null, "", ShaderFlags.None, m_effectPool, out errs);

                if (errs != null && errs != string.Empty)
                {
                    Log.Write(Log.Levels.Warning, "Could not load effect " + m_effectPath + ": " + errs);
                    Log.Write(Log.Levels.Warning, "Effect has been disabled.");
                    m_effectPath = null;
                    m_effect = null;
                }
            } 

            #endregion

            if (ImageStores[0].LevelZeroTileSizeDegrees < 180)
            {
                // Check for layer outside view
                double vrd = DrawArgs.Camera.ViewRange.Degrees;
                double latitudeMax = DrawArgs.Camera.Latitude.Degrees + vrd;
                double latitudeMin = DrawArgs.Camera.Latitude.Degrees - vrd;
                double longitudeMax = DrawArgs.Camera.Longitude.Degrees + vrd;
                double longitudeMin = DrawArgs.Camera.Longitude.Degrees - vrd;
                if (latitudeMax < m_south || latitudeMin > m_north || longitudeMax < m_west || longitudeMin > m_east)
                    return;
            }

            if (DrawArgs.Camera.ViewRange * 0.5f >
                    Angle.FromDegrees(TileDrawDistance * ImageStores[0].LevelZeroTileSizeDegrees))
            {
                lock (m_topmostTiles.SyncRoot)
                {
                    foreach (QuadTile qt in m_topmostTiles.Values)
                        qt.Dispose();
                    m_topmostTiles.Clear();
                    ClearDownloadRequests();
                }

                return;
            }

            RemoveInvisibleTiles(DrawArgs.Camera);

            try
            {
                int middleRow = MathEngine.GetRowFromLatitude(DrawArgs.Camera.Latitude, ImageStores[0].LevelZeroTileSizeDegrees);
                int middleCol = MathEngine.GetColFromLongitude(DrawArgs.Camera.Longitude, ImageStores[0].LevelZeroTileSizeDegrees);

                double middleSouth = -90.0f + middleRow * ImageStores[0].LevelZeroTileSizeDegrees;
                double middleNorth = -90.0f + middleRow * ImageStores[0].LevelZeroTileSizeDegrees + ImageStores[0].LevelZeroTileSizeDegrees;
                double middleWest = -180.0f + middleCol * ImageStores[0].LevelZeroTileSizeDegrees;
                double middleEast = -180.0f + middleCol * ImageStores[0].LevelZeroTileSizeDegrees + ImageStores[0].LevelZeroTileSizeDegrees;

                double middleCenterLat = 0.5f * (middleNorth + middleSouth);
                double middleCenterLon = 0.5f * (middleWest + middleEast);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  WorldWind瓦片更新渲染方式：以当前可视区域中心位置所在瓦片向周围扩散一定半径，然后更新加载
                //  此范围内的所有瓦片，每次更新加载都从0级开始向下进行递归搜索可视区域包含的瓦片，直到找到满足
                //  可视高度层级的瓦片。
                // 这种渲染方式存在如下问题：
                // 1. 瓦片拓展搜索的方式存在重复性，中心区域随着拓展半径的增加重复更新了若干次，耗费系统资源；
                // 2. 每次都从0级向下进行递归搜索加载瓦片的方式，虽然可以解决当某一层级缺少数据时的显示问题，
                //     但是也造成了系统资源的浪费，每一层级瓦片的渲染都需要将该瓦片层级之上的所有层级数据都加载，
                //     数据量大时影像视觉效果。
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                int tileSpread = 4;
                for (int i = 0; i < tileSpread; i++)
                {
                    for (double j = middleCenterLat - i * ImageStores[0].LevelZeroTileSizeDegrees; j < middleCenterLat + i * ImageStores[0].LevelZeroTileSizeDegrees; j += ImageStores[0].LevelZeroTileSizeDegrees)
                    {
                        for (double k = middleCenterLon - i * ImageStores[0].LevelZeroTileSizeDegrees; k < middleCenterLon + i * ImageStores[0].LevelZeroTileSizeDegrees; k += ImageStores[0].LevelZeroTileSizeDegrees)
                        {
                            int curRow = MathEngine.GetRowFromLatitude(Angle.FromDegrees(j), ImageStores[0].LevelZeroTileSizeDegrees);
                            int curCol = MathEngine.GetColFromLongitude(Angle.FromDegrees(k), ImageStores[0].LevelZeroTileSizeDegrees);
                            long key = ((long)curRow << 32) + curCol;

                            QuadTile qt = (QuadTile)m_topmostTiles[key];
                            if (qt != null)
                            {
                                qt.Update(drawArgs);
                                continue;
                            }

                            // Check for tile outside layer boundaries
                            double west = -180.0f + curCol * ImageStores[0].LevelZeroTileSizeDegrees;
                            if (west > m_east)
                                continue;

                            double east = west + ImageStores[0].LevelZeroTileSizeDegrees;
                            if (east < m_west)
                                continue;

                            double south = -90.0f + curRow * ImageStores[0].LevelZeroTileSizeDegrees;
                            if (south > m_north)
                                continue;

                            double north = south + ImageStores[0].LevelZeroTileSizeDegrees;
                            if (north < m_south)
                                continue;

                            qt = new QuadTile(south, north, west, east, 0, this);
                            if (DrawArgs.Camera.ViewFrustum.Intersects(qt.BoundingBox))
                            {
                                lock (m_topmostTiles.SyncRoot)
                                    m_topmostTiles.Add(key, qt);
                                qt.Update(drawArgs);
                            }
                        }
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception caught)
            {
                Log.Write(caught);
            }
        }

        /// <summary>
        /// 移除不可用瓦片
        /// </summary>
        /// <param name="camera"></param>
        protected void RemoveInvisibleTiles(CameraBase camera)
        {
            ArrayList deletionList = new ArrayList();

            lock (m_topmostTiles.SyncRoot)
            {
                foreach (long key in m_topmostTiles.Keys)
                {
                    QuadTile qt = (QuadTile)m_topmostTiles[key];
                    if (!camera.ViewFrustum.Intersects(qt.BoundingBox))
                        deletionList.Add(key);
                }

                foreach (long deleteThis in deletionList)
                {
                    QuadTile qt = (QuadTile)m_topmostTiles[deleteThis];
                    if (qt != null)
                    {
                        m_topmostTiles.Remove(deleteThis);
                        qt.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 渲染瓦片集
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            try
            {
                lock (m_topmostTiles.SyncRoot)
                {
                    if (m_topmostTiles.Count <= 0)
                    {
                        return;
                    }

                    Device device = DrawArgs.Device;

                    // Temporary fix: Clear Z buffer between rendering
                    // terrain mapped layers to avoid Z buffer fighting
                    device.Clear(ClearFlags.ZBuffer, 0, 1.0f, 0);
                    device.RenderState.ZBufferEnable = true;
                    lastRenderTime = DrawArgs.CurrentFrameStartTicks;

                    if (!World.Settings.EnableSunShading)
                    {
                        // Set the render states for rendering of quad tiles.
                        // Any quad tile rendering code that adjusts the state should restore it to below values afterwards.
                        device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
                        device.SetTextureStageState(0, TextureStageStates.ColorOperation, (int)TextureOperation.SelectArg1);
                        device.SetTextureStageState(0, TextureStageStates.ColorArgument1, (int)TextureArgument.TextureColor);
                        device.SetTextureStageState(0, TextureStageStates.AlphaArgument1, (int)TextureArgument.TextureColor);
                        device.SetTextureStageState(0, TextureStageStates.AlphaOperation, (int)TextureOperation.SelectArg1);

                        // Be prepared for multi-texturing
                        device.SetTextureStageState(1, TextureStageStates.ColorArgument2, (int)TextureArgument.Current);
                        device.SetTextureStageState(1, TextureStageStates.ColorArgument1, (int)TextureArgument.TextureColor);
                        device.SetTextureStageState(1, TextureStageStates.TextureCoordinateIndex, 0);
                    }
                    device.VertexFormat = CustomVertex.PositionNormalTextured.Format;

                    foreach (QuadTile qt in m_topmostTiles.Values)
                    {
                        bool tmp;
                        List<QuadTile> randerqts = qt.GetRanderableChild(out tmp);
                        for (int i = randerqts.Count-1; i >-1 ; i--)
                        {
                            randerqts[i].Render(drawArgs);                            
                        }

                    }
                    // Restore device states
                    device.SetTextureStageState(1, TextureStageStates.TextureCoordinateIndex, 1);

                    if (m_renderPriority < RenderPriority.TerrainMappedImages)
                        device.RenderState.ZBufferEnable = true;
                }
            }
            catch
            {
            }
            finally
            {
                //if (IsConnectionWaiting)
                //{
                //    if (DateTime.Now.Subtract(TimeSpan.FromSeconds(15)) < ConnectionWaitStart)
                //    {
                //        string s = "与服务器连接失败... 2分钟后重新连接.\n";
                //        drawArgs.UpperLeftCornerText += s;
                //    }
                //}

                //int i = 0;
                //foreach (GeoSpatialDownloadRequest request in m_activeDownloads)
                //{
                //    if (request != null && !request.IsComplete && i < 10)
                //    {
                //        RenderDownloadProgress(drawArgs, request, i++);
                //        // Only render the first
                //        //break;
                //    }
                //}
            }
        }

        /// <summary>
        /// 渲染下载进度
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="request"></param>
        /// <param name="offset"></param>
        public void RenderDownloadProgress(DrawArgs drawArgs, GeoSpatialDownloadRequest request, int offset)
        {
            int halfIconHeight = 24;
            int halfIconWidth = 24;

            Vector3 projectedPoint = new Vector3(DrawArgs.ParentControl.Width - halfIconWidth - 10, DrawArgs.ParentControl.Height - 34 - 4 * offset, 0.5f);

            // Render progress bar
            if (progressBar == null)
                progressBar = new ProgressBar(40, 4);
            progressBar.Draw(drawArgs, projectedPoint.X, projectedPoint.Y + 24, request.ProgressPercent, World.Settings.DownloadProgressColor.ToArgb());
            DrawArgs.Device.RenderState.ZBufferEnable = true;

            // Render server logo
            if (ServerLogoFilePath == null)
                return;

            if (m_iconTexture == null)
                m_iconTexture = ImageHelper.LoadIconTexture(ServerLogoFilePath);

            if (sprite == null)
            {
                using (Surface s = m_iconTexture.GetSurfaceLevel(0))
                {
                    SurfaceDescription desc = s.Description;
                    m_spriteSize = new Rectangle(0, 0, desc.Width, desc.Height);
                }

                this.sprite = new Sprite(DrawArgs.Device);
            }

            float scaleWidth = (float)2.0f * halfIconWidth / m_spriteSize.Width;
            float scaleHeight = (float)2.0f * halfIconHeight / m_spriteSize.Height;

            this.sprite.Begin(SpriteFlags.AlphaBlend);
            this.sprite.Transform = Matrix.Transformation2D(new Vector2(0.0f, 0.0f), 0.0f, new Vector2(scaleWidth, scaleHeight),
                    new Vector2(0, 0),
                    0.0f, new Vector2(projectedPoint.X, projectedPoint.Y));

            this.sprite.Draw(m_iconTexture, m_spriteSize,
                    new Vector3(1.32f * 48, 1.32f * 48, 0), new Vector3(0, 0, 0),
                    World.Settings.DownloadLogoColor);
            this.sprite.End();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            IsInitialized = false;

            // flush downloads
            for (int i = 0; i < World.Settings.MaxSimultaneousDownloads; i++)
            {
                if (m_activeDownloads[i] != null)
                {
                    m_activeDownloads[i].Dispose();
                    m_activeDownloads[i] = null;
                }
            }

            foreach (QuadTile qt in m_topmostTiles.Values)
                qt.Dispose();

            if (m_iconTexture != null)
            {
                m_iconTexture.Dispose();
                m_iconTexture = null;
            }

            if (this.sprite != null)
            {
                this.sprite.Dispose();
                this.sprite = null;
            }
        }

        /// <summary>
        /// 为当前视图重设缓存
        /// </summary>
        /// <param name="camera"></param>
        public virtual void ResetCacheForCurrentView(CameraBase camera)
        {
            //                      if (!ImageStore.IsDownloadableLayer)
            //                              return;

            ArrayList deletionList = new ArrayList();
            //reset "root" tiles that intersect current view
            lock (m_topmostTiles.SyncRoot)
            {
                foreach (long key in m_topmostTiles.Keys)
                {
                    QuadTile qt = (QuadTile)m_topmostTiles[key];
                    if (camera.ViewFrustum.Intersects(qt.BoundingBox))
                    {
                        qt.ResetCache();
                        deletionList.Add(key);
                    }
                }

                foreach (long deletionKey in deletionList)
                    m_topmostTiles.Remove(deletionKey);
            }
        }

        /// <summary>
        /// 清除下载请求
        /// </summary>
        public void ClearDownloadRequests()
        {
            lock (m_downloadRequests.SyncRoot)
            {
                m_downloadRequests.Clear();
            }
        }

        /// <summary>
        /// 添加到下载队列（ZYM:20130706）
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="newRequest"></param>
        public virtual void AddToDownloadQueue(CameraBase camera, GeoSpatialDownloadRequest newRequest)
        {
            QuadTile key = newRequest.QuadTile;
            key.IsWaitingForDownload = true;
            lock (m_downloadRequests.SyncRoot)
            {
                if (m_downloadRequests.Contains(key))
                    return;

                m_downloadRequests.Add(key, newRequest);

                if (m_downloadRequests.Count >= m_maxQueueSize)
                {
                    //remove spatially farthest request
                    GeoSpatialDownloadRequest farthestRequest = null;
                    Angle curDistance = Angle.Zero;
                    Angle farthestDistance = Angle.Zero;
                    foreach (GeoSpatialDownloadRequest curRequest in m_downloadRequests.Values)
                    {
                        curDistance = MathEngine.SphericalDistance(
                                        curRequest.QuadTile.CenterLatitude,
                                        curRequest.QuadTile.CenterLongitude,
                                        camera.Latitude,
                                        camera.Longitude);

                        if (curDistance > farthestDistance)
                        {
                            farthestRequest = curRequest;
                            farthestDistance = curDistance;
                        }
                    }

                    farthestRequest.Dispose();
                    farthestRequest.QuadTile.DownloadRequest = null;
                    m_downloadRequests.Remove(farthestRequest.QuadTile);
                }
            }

            ServiceDownloadQueue();
        }

        /// <summary>
        /// Removes a request from the download queue.
        /// 从下载队列中移除一个下载请求
        /// </summary>
        public virtual void RemoveFromDownloadQueue(GeoSpatialDownloadRequest removeRequest)
        {
            lock (m_downloadRequests.SyncRoot)
            {
                QuadTile key = removeRequest.QuadTile;
                GeoSpatialDownloadRequest request = (GeoSpatialDownloadRequest)m_downloadRequests[key];
                if (request != null)
                {
                    m_downloadRequests.Remove(key);
                    request.QuadTile.DownloadRequest = null;
                }
            }
        }

        /// <summary>
        /// Starts downloads when there are threads available
        /// 当有可用线程时启动数据下载（ZYM:20130706）
        /// </summary>
        public virtual void ServiceDownloadQueue()
        {
            Log.Write(Log.Levels.Verbose, "QTS", "ServiceDownloadQueue: " + m_downloadRequests.Count + " requests waiting");
            lock (m_downloadRequests.SyncRoot)
            {
                for (int i = 0; i < World.Settings.MaxSimultaneousDownloads; i++)
                {
                    if (m_activeDownloads[i] == null)
                        continue;

                    if (!m_activeDownloads[i].IsComplete)
                        continue;

                    m_activeDownloads[i].Cancel();
                    m_activeDownloads[i].Dispose();
                    m_activeDownloads[i] = null;
                }

                if (NumberRetries >= 5 || m_isConnectionWaiting)
                {
                    // Anti hammer in effect
                    if (!m_isConnectionWaiting)
                    {
                        m_connectionWaitStart = DateTime.Now;
                        m_isConnectionWaiting = true;
                    }

                    if (DateTime.Now.Subtract(m_connectionWaitTime) > m_connectionWaitStart)
                    {
                        NumberRetries = 0;
                        m_isConnectionWaiting = false;
                    }
                    return;
                }

                // Queue new downloads
                for (int i = 0; i < World.Settings.MaxSimultaneousDownloads; i++)
                {
                    if (m_activeDownloads[i] != null)
                        continue;

                    if (m_downloadRequests.Count <= 0)
                        continue;

                    m_activeDownloads[i] = GetClosestDownloadRequest();
                    if (m_activeDownloads[i] != null)
                    {
                        m_downloadStarted[i] = DateTime.Now;
                        m_activeDownloads[i].StartDownload();
                    }
                }
            }
        }

        /// <summary>
        /// Finds the "best" tile from queue
        /// 从队列中寻找最佳的瓦片
        /// </summary>
        public virtual GeoSpatialDownloadRequest GetClosestDownloadRequest()
        {
            GeoSpatialDownloadRequest closestRequest = null;
            float largestArea = float.MinValue;

            lock (m_downloadRequests.SyncRoot)
            {
                foreach (GeoSpatialDownloadRequest curRequest in m_downloadRequests.Values)
                {
                    if (curRequest.IsDownloading)
                        continue;

                    QuadTile qt = curRequest.QuadTile;
                    if (!m_camera.ViewFrustum.Intersects(qt.BoundingBox))
                        continue;

                    float screenArea = qt.BoundingBox.CalcRelativeScreenArea(m_camera);
                    if (screenArea > largestArea)
                    {
                        largestArea = screenArea;
                        closestRequest = curRequest;
                    }
                }
            }

            return closestRequest;
        }

        /// <summary>
        /// Creates a tile download indication texture
        /// 创建一个瓦片下载窗口
        /// </summary>
        protected static Texture CreateDownloadRectangle(Device device, Color color, int padding)
        {
            int mid = 128;
            using (Bitmap i = new Bitmap(2 * mid, 2 * mid))
            using (Graphics g = Graphics.FromImage(i))
            using (Pen pen = new Pen(color))
            {
                int width = mid - 1 - 2 * padding;
                g.DrawRectangle(pen, padding, padding, width, width);
                g.DrawRectangle(pen, mid + padding, padding, width, width);
                g.DrawRectangle(pen, padding, mid + padding, width, width);
                g.DrawRectangle(pen, mid + padding, mid + padding, width, width);

                Texture texture = new Texture(device, i, Usage.None, Pool.Managed);
                return texture;
            }
        }
    }
}
