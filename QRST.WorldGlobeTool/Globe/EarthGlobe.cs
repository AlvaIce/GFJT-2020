using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Stores;
using QRST.WorldGlobeTool.Renderable;
using QRST.WorldGlobeTool.DrawUtility;
using QRST.WorldGlobeTool.Utility;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Data.Rasters.GdalExtension;

namespace QRST.WorldGlobeTool.Globe
{
    /// <summary>
    /// 球体类，提供相关显示属性设置、图层添加等功能
    /// </summary>
    public class EarthGlobe
    {

        #region 私有字段

        /// <summary>
        /// 球控件对象
        /// </summary>
        private QRSTWorldGlobeControl m_GlobeControl;

        /// <summary>
        /// 配置目录
        /// </summary>
        private string m_DataDirectory = "";

        /// <summary>
        /// 缓存路径
        /// </summary>
        private string m_CacheDirectory = "";

        /// <summary>
        /// 影像缓存
        /// </summary>
        private Hashtable m_ImageCache = new Hashtable();

        #endregion 私有字段

        #region 公共属性

        /// <summary>
        /// 获取或设置球控件对象
        /// </summary>
        public QRSTWorldGlobeControl GlobeControl
        {
            get
            {
                return m_GlobeControl;
            }
            set
            {
                m_GlobeControl = value;
            }
        }

        /// <summary>
        /// 获取或设置夸大因子
        /// </summary>
        public float VerticalExaggeration
        {
            get
            {
                return World.Settings.VerticalExaggeration;
            }
            set
            {
                World.Settings.VerticalExaggeration = value;
                this.GlobeControl.Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置配置根目录
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
                this.GlobeControl.DataDirectory = this.m_DataDirectory;//配置根目录路径
            }
        }

        /// <summary>
        /// 获取或设置缓存的存放根目录
        /// </summary>
        public string CacheDirectory
        {
            get
            {
                return this.m_CacheDirectory;
            }
            set
            {
                this.m_CacheDirectory = value;
                this.GlobeControl.Cache.CacheDirectory = m_CacheDirectory;
                this.GlobeControl.CacheDirectory = m_CacheDirectory;
            }
        }

        /// <summary>
        /// 获取或设置是否显示格网
        /// </summary>
        public bool ShowLatLonLines
        {
            get
            {
                return World.Settings.ShowLatLonLines;
            }
            set
            {
                World.Settings.ShowLatLonLines = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示版权信息
        /// </summary>
        public bool ShowCopyright
        {
            get { return World.Settings.ShowCopyright; }
            set { World.Settings.ShowCopyright = value; }
        }

        /// <summary>
        /// 获取或设置是否显示空间信息
        /// </summary>
        public bool ShowPosition
        {
            get
            {
                return World.Settings.ShowPosition;
            }
            set
            {
                World.Settings.ShowPosition = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示图层管理器
        /// </summary>
        public bool ShowLayerManager
        {
            get
            {
                return World.Settings.ShowLayerManager;
            }
            set
            {
                World.Settings.ShowLayerManager = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示太阳阴影效果
        /// </summary>
        public bool ShowSunShading
        {
            get
            {
                return World.Settings.EnableSunShading;
            }
            set
            {
                World.Settings.EnableSunShading = value;
            }
        }

        #endregion 公共属性

        #region 构造函数

        /// <summary>
        /// 创建一个地球对象实例
        /// </summary>
        /// <param name="QrstGlobe"></param>
        public EarthGlobe(QRSTWorldGlobeControl QrstGlobe)
        {
            this.GlobeControl = QrstGlobe;
            this.m_DataDirectory = this.GlobeControl.DataDirectory;
            this.m_CacheDirectory = this.GlobeControl.CacheDirectory;
        }

        #endregion 构造函数

        #region 添加图层

        #region 添加基础图层

        /// <summary>
        /// 添加世界洲边界图层
        /// </summary>
        public void AddWorldContinentBoundaryLayer()
        {
            //添加世界国界矢量图层
            this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(getShapeLineOrPolygonLayer(
                "全球洲界", Path.Combine(this.DataDirectory, @"Earth\WorldSHP\WorldContinent.shp"), false,
                Color.Yellow, Color.DarkOrange, 8.0f, 6378137.0 * 10, 6378137.0 * 5, true, "CONTINENT",
                Color.White, Color.Black));
        }

        /// <summary>
        /// 添加国界及河流图层
        /// </summary>
        public void AddWorldCountryBoundaryLayer()
        {
            //添加世界国界矢量图层
            this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(getShapeLineOrPolygonLayer(
                "全球国界", Path.Combine(this.DataDirectory, @"Earth\WorldSHP\WorldCountry.shp"), false,
                Color.Yellow, Color.DarkSlateGray, 4.0f, 6378137.0 * 5, 100000, true, "CNTRY_NAME",
                Color.White, Color.Black));

            //添加全球河流矢量图层
            this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(getShapeLineOrPolygonLayer(
                "全球河流", Path.Combine(this.DataDirectory, @"Earth\WorldSHP\WorldRiver.shp"), false,
                Color.DarkBlue, Color.DarkBlue, 8.0f, 6378137.0 * 5, 0, true, "NAME",
                Color.BlueViolet, Color.BlueViolet));
        }

        /// <summary>
        /// 添加中国省界图层
        /// </summary>
        public void AddChinaBoundaryLayer()
        {
            //添加中国省界矢量图层
            AddShapeLineOrPolygonLayer(
                "中国省界", Path.Combine(this.DataDirectory, @"China\ChinaSHP\中国省界.shp"),
                Color.Yellow, Color.Black, 4.0f, 6378137.0 * 2 + 100, 500000, false, "",
                Color.White, Color.Black);
            //添加中国省界图标
            AddPlacesLayer
                (
                    "中国省界图标", Path.Combine(this.DataDirectory, @"China\Place\ChinaSheng.qrstp"),
                    25, 25, Path.Combine(this.DataDirectory, @"Icons\placemark_circle.png"),
                    5000000, 0, Color.White, Color.Black
                );
        }

        /// <summary>
        /// 添加中国县界图层
        /// </summary>
        /// <param name="isShowCountyName">是否显示县名称</param>
        public void AddChinaCountyBoundaryLayer(bool isShowCountyName)
        {
            //添加中国县界矢量图层
            AddShapeLineOrPolygonLayer(
                "中国县界", Path.Combine(this.DataDirectory, @"China\ChinaSHP\中国县界.shp"),
                Color.WhiteSmoke, Color.Red, 1.0f, 1200000, 1000, isShowCountyName, "NAME",
                Color.White, Color.Black);
        }

        /// <summary>
        /// 添加全球矢量图层集合
        /// </summary>
        /// <param name="shapeFilesDirectory"></param>
        public void AddWorldShapeLayerSet()
        {
            RenderableObjectList rol = new RenderableObjectList("全球矢量");
            rol.Add(getShapeLineOrPolygonLayer(
                "全球洲界", Path.Combine(this.DataDirectory, @"Earth\WorldSHP\WorldContinent.shp"), true,
                Color.BlueViolet, Color.BlueViolet, 8.0f, 6378137.0 * 10, 0, false, "CONTINENT",
                Color.BlueViolet, Color.BlueViolet));
            rol.Add(getShapeLineOrPolygonLayer(
                "全球国界", Path.Combine(this.DataDirectory, @"Earth\WorldSHP\WorldCountry.shp"), true,
                Color.LimeGreen, Color.LimeGreen, 4.0f, 6378137.0 * 5, 0, false, "CNTRY_NAME",
                Color.LimeGreen, Color.Green));
            rol.Add(getShapeLineOrPolygonLayer(
                "全球河流", Path.Combine(this.DataDirectory, @"Earth\WorldSHP\WorldRiver.shp"), false,
                Color.DarkBlue, Color.DarkBlue, 8.0f, 6378137.0 * 5, 0, false, "NAME",
                Color.BlueViolet, Color.BlueViolet));
            this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(rol);
        }

        /// <summary>
        /// 添加中国矢量图层集合
        /// </summary>
        public void AddChinaShapeLayerSet()
        {
            RenderableObjectList rol = this.m_GlobeControl.CurrentWorld.RenderableObjects;
            string contriesPath = Path.Combine(this.DataDirectory, @"China\ChinaSHP\国家.shp");
            if (File.Exists(contriesPath))
            {
                rol.Add(getShapeLineOrPolygonLayer(
                "国家", contriesPath, true,
                Color.LightPink, Color.LightPink, 4.0f, 10000000.0, 1000000, true, "CNTRY_NAME",
                Color.LightPink, Color.Pink));
            }
            
            //rol.Add(getShapeLineOrPolygonLayer(
            //    "国界线", Path.Combine(this.DataDirectory, @"China\ChinaSHP\国界线.shp"), true,
            //    Color.LimeGreen, Color.LimeGreen, 4.0f, 6378137.0 * 2 + 100, 500000, false, "",
            //    Color.LimeGreen, Color.Green));
            rol.Add(getShapeLineOrPolygonLayer(
                "中国省界", Path.Combine(this.DataDirectory, @"China\ChinaSHP\中国省界2.shp"), true,
                Color.Yellow, Color.Black, 4.0f, 6378137.0 * 2 + 1000, 0, false, "CHINESE_CH",
                Color.LightGray, Color.LightGray));
            rol.Add(getShapePointLayer(
                "一级城市", Path.Combine(this.DataDirectory, @"China\ChinaSHP\一级城市.shp"), "NAME",
                25, 25, Path.Combine(this.DataDirectory, @"Icons\placemark_circle.png"),
                6378137.0 * 1.2, 1500000, Color.LightGray, Color.LightGray));
            //rol.Add(AddShapeLayer(
            //    "中国市界", Path.Combine(this.DataDirectory, @"China\ChinaSHP\中国市界.shp"), false,
            //    Color.WhiteSmoke, Color.Red, 1.0f, 6378137.0, 1000, false, "NAME",
            //    Color.White, Color.Black));
            rol.Add(getShapePointLayer(
                "二级城市", Path.Combine(this.DataDirectory, @"China\ChinaSHP\二级城市.shp"), "NAME",
                8, 8, Path.Combine(this.DataDirectory, @"Icons\placemark.png"),
                1500000, 100000, Color.LightGray, Color.LightGray));
            rol.Add(getShapeLineOrPolygonLayer(
                "中国县界", Path.Combine(this.DataDirectory, @"China\ChinaSHP\中国县界.shp"), true,
                Color.LightGray, Color.Red, 1.0f, 200000, 0, true, "NAME",
                Color.LightGray, Color.LightGray));
            //rol.Add(getShapeLineOrPolygonLayer(
            //    "国道", Path.Combine(this.DataDirectory, @"China\ChinaSHP\国道.shp"), false,
            //    Color.WhiteSmoke, Color.Red, 1.0f, 6378137.0, 1000, false, "NAME",
            //    Color.White, Color.Black));
            //rol.Add(getShapeLineOrPolygonLayer(
            //    "主要铁路", Path.Combine(this.DataDirectory, @"China\ChinaSHP\主要铁路.shp"), false,
            //    Color.WhiteSmoke, Color.Red, 1.0f, 6378137.0, 1000, false, "",
            //    Color.White, Color.Black));
        }

        #endregion

        #region 添加影像图层
        public List<ImageLayer> imageList = new List<ImageLayer>();
        /// <summary>
        /// 1.添加标准的影像图层(纹理图层)
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="opacity">透明度</param>
        /// <param name="imagePath">影像路径</param>
        /// <returns>返回布尔型，只是是否添加图层成功</returns>
        public bool AddImage(string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, int backColor, byte opacity, string imagePath)
        {
            try
            {
                QRSTWorldGlobeControl window = this.GlobeControl;
                //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
                ImageLayer image = GetImage(name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, backColor, opacity, imagePath);
                image.ParentList = window.CurrentWorld.RenderableObjects;
                image.IsOn = true;
                //添加当前图层
                //window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, image);
                window.CurrentWorld.RenderableObjects.Add(image);
                imageList.Add(image);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 1.清除的影像图层(纹理图层)
        /// </summary>  
        /// <param name="name">影像名字</param> 
        /// <returns>返回布尔型，只是是否添加图层成功</returns>
        public void RemoveImages()
        {
            if (imageList != null)
            {
                QRSTWorldGlobeControl window = this.GlobeControl;

                foreach (var image in imageList)
                {
                    window.CurrentWorld.RenderableObjects.Remove(image);

                }
                imageList.Clear();
            }

        }
        /// <summary>
        /// 1.添加标准的影像图层(纹理图层)
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="opacity">透明度</param>
        /// <param name="imagePath">影像路径</param>
        /// <returns>返回布尔型，只是是否添加图层成功</returns>
        public bool AddImage(string name, double distanceAboveSurface, double uplat, double uplon, double leftlat, double leftlon, double downlat, double downlon, double rightlat, double rightlon, int backColor, byte opacity, string imagePath)
        {
            try
            {
                QRSTWorldGlobeControl window = this.GlobeControl;
                //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
                ImageLayer image = GetImage(name, distanceAboveSurface, uplat, uplon,  leftlat, leftlon,downlat, downlon, rightlat, rightlon, backColor, opacity, imagePath);
                image.ParentList = window.CurrentWorld.RenderableObjects;
                image.IsOn = true;
                //添加当前图层
                //window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, image);
                window.CurrentWorld.RenderableObjects.Add(image);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加新图片到已有图片集中，在图层管理面板中显示在同一个顶层集合下
        /// </summary>
        /// <param name="setName">图层集合名称</param>
        /// <param name="name">新的图片显示名称</param>
        /// <param name="distanceAboveSurface">距离表层的距离</param>
        /// <param name="minLat">最小纬度</param>
        /// <param name="maxLat">最大纬度</param>
        /// <param name="minLon">最小经度</param>
        /// <param name="maxLon">最大经度</param>
        /// <param name="backColor">背景色</param>
        /// <param name="opacity">透明度</param>
        /// <param name="imagePath">图片路径</param>
        /// <returns></returns>
        public bool AddToImageSet(string setName, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, int backColor, byte opacity, string imagePath)
        {
            try
            {
                QRSTWorldGlobeControl window = this.GlobeControl;
                RenderableObjectList windowRenderable = window.CurrentWorld.RenderableObjects;
                RenderableObjectList imageSet = null;
                for (int i = 0; i < windowRenderable.ChildObjects.Count; i++)
                {
                    if (windowRenderable.ChildObjects[i] is RenderableObjectList)
                    {
                        RenderableObjectList thisObjectList = windowRenderable.ChildObjects[i] as RenderableObjectList;
                        if (thisObjectList.Name == setName)
                        {
                            imageSet = thisObjectList;
                            break;
                        }
                    }
                }
                if (imageSet == null)
                {
                    imageSet = new RenderableObjectList(setName);
                    imageSet.ParentList = window.CurrentWorld.RenderableObjects;
                    //是否显示此图层
                    imageSet.IsOn = true;
                    imageSet.DisableExpansion = false;
                    imageSet.IsShowOnlyOneLayer = false;
                    //存储QrstWindow的主要对象
                    imageSet.MetaData.Add("World", window.CurrentWorld);//着重看
                    imageSet.MetaData.Add("Cache", window.Cache);//着重看

                    imageSet.MetaData["north"] = maxLat;
                    imageSet.MetaData["south"] = minLat;
                    imageSet.MetaData["west"] = minLon;
                    imageSet.MetaData["east"] = maxLon;

                    //此图层做为表面图层
                    imageSet.RenderPriority = RenderPriority.SurfaceImages;
                    //window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, imageSet);
                    window.CurrentWorld.RenderableObjects.Add(imageSet);
                }
                //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
                ImageLayer image = GetImage(name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, backColor, opacity, imagePath);
                image.ParentList = imageSet;
                //添加当前图层
                imageSet.Add(image);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 2.获得标准的影像图层对象（纹理图层）
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="opacity">透明度</param>
        /// <param name="imagePath">影像路径</param>
        /// <returns>返回ImageLayer对象</returns>
        public ImageLayer GetImage(string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, int backColor, byte opacity, string imagePath)
        {
            QRSTWorldGlobeControl window = this.GlobeControl;

            //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
            ImageLayer image = new ImageLayer
                (
                name,
                window.CurrentWorld,
                distanceAboveSurface,
                imagePath,
                minLat, maxLat, minLon, maxLon, opacity, window.CurrentWorld.TerrainAccessor
                );
            image.MetaData["north"] = maxLat;
            image.MetaData["south"] = minLat;
            image.MetaData["west"] = minLon;
            image.MetaData["east"] = maxLon;
            System.Drawing.Color c = System.Drawing.Color.FromArgb(opacity, backColor, backColor, backColor);//Color [A=255, R=0, G=0, B=0]
            image.TransparentColor = c.ToArgb();
            image.ParentList = window.CurrentWorld.RenderableObjects;
            return image;
        }

        /// <summary>
        /// 3.获得标准的影像图层对象（纹理图层），by四角坐标
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="Up">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="opacity">透明度</param>
        /// <param name="imagePath">影像路径</param>
        /// <returns>返回ImageLayer对象</returns>
        public ImageLayer GetImage(string name, double distanceAboveSurface, double uplat, double uplon, double leftlat, double leftlon, double downlat, double downlon, double rightlat, double rightlon, int backColor, byte opacity, string imagePath)
        {
            QRSTWorldGlobeControl window = this.GlobeControl;

            //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
            ImageLayer image = new ImageLayer
                (
                name,
                window.CurrentWorld,
                distanceAboveSurface,
                imagePath,
                uplat, uplon, downlat, downlon, leftlat, leftlon, rightlat, rightlon,
                opacity, window.CurrentWorld.TerrainAccessor
                );
            image.MetaData["north"] = uplat;
            image.MetaData["south"] = downlat;
            image.MetaData["west"] = leftlon;
            image.MetaData["east"] = rightlon;
            System.Drawing.Color c = System.Drawing.Color.FromArgb(opacity, backColor, backColor, backColor);//Color [A=255, R=0, G=0, B=0]
            image.TransparentColor = c.ToArgb();
            image.ParentList = window.CurrentWorld.RenderableObjects;
            return image;
        }

        #endregion 添加影像图层

        #region 添加瓦片影像

        /// <summary>
        /// 3.添加切片好的影像图层
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="opacity">透明度</param>
        /// <param name="visible">是否显示</param>
        /// <param name="levelZeroTileSizeDegrees">第一层格网的度数</param>
        /// <param name="numberLevels">总共的层数</param>
        /// <param name="imageFileExtension">切片的后缀名</param>
        /// <param name="imagePath">切片图像的路径</param>
        /// <returns>返回布尔型，只是是否添加图层成功</returns>
        public bool AddTiledImage(
             string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, byte opacity, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string imageFileExtension, string imagePath)
        {
            try
            {
                QRSTWorldGlobeControl window = this.GlobeControl;
                //切片图层
                RenderableObjectList tiledImage = GetTiledImage(window, name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, opacity, visible, levelZeroTileSizeDegrees, numberLevels, imageFileExtension, imagePath);
                tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
                //window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, tiledImage);
                window.CurrentWorld.RenderableObjects.Add(tiledImage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 4.获得切片图层对象
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="opacity">透明度</param>
        /// <param name="visible">是否显示</param>
        /// <param name="levelZeroTileSizeDegrees">第一层格网的度数</param>
        /// <param name="numberLevels">总共的层数</param>
        /// <param name="imageFileExtension">切片的后缀名</param>
        /// <param name="imagePath">切片图像的路径</param>
        /// <returns>返回RenderableObjectList</returns>
        public RenderableObjectList GetTiledImage(
            QRSTWorldGlobeControl window, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, byte opacity, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string imageFileExtension, string imagePath)
        {
            //切片图层
            RenderableObjectList tiledImage = new RenderableObjectList(name);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            //是否显示此图层
            tiledImage.IsOn = visible;
            tiledImage.DisableExpansion = false;
            tiledImage.IsShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象
            tiledImage.MetaData.Add("World", window.CurrentWorld);//着重看
            tiledImage.MetaData.Add("Cache", window.Cache);//着重看

            tiledImage.MetaData["north"] = maxLat;
            tiledImage.MetaData["south"] = minLat;
            tiledImage.MetaData["west"] = minLon;
            tiledImage.MetaData["east"] = maxLon;

            //此图层做为表面图层
            tiledImage.RenderPriority = RenderPriority.SurfaceImages;

            //是否显示数字高程
            bool terrainMapped = true;
            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;

            //切片图层对象
            ImageStore ia = new ImageStore();
            ia.DataDirectory = imagePath;
            ia.LevelZeroTileSizeDegrees = levelZeroTileSizeDegrees;
            ia.LevelCount = numberLevels;
            ia.ImageExtension = imageFileExtension;
            ia.CacheDirectory = imagePath;
            ia.ServerLogo = Path.Combine(this.m_GlobeControl.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            imageStores[0].ServerLogo = Path.Combine(this.m_GlobeControl.DataDirectory, @"Icons\dcsrsdcsf.png");
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                this.GlobeControl.CurrentWorld,
                distanceAboveSurface,
                maxLat,
                minLat,
                minLon,
                maxLon,
                terrainMapped,
                imageStores
            );

            qts.CacheExpirationTime = dataExpiration;
            qts.ServerLogoFilePath = ia.ServerLogo;
            //透明色
            System.Drawing.Color c = Color.FromArgb(opacity, 0, 0, 0);//Color [A=255, R=0, G=0, B=0]
            qts.ColorKey = c.ToArgb();
            qts.ParentList = tiledImage;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)tiledImage.MetaData["XmlSource"]);
            tiledImage.Add(qts);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            return tiledImage;
        }

        #endregion 添加瓦片影像

        #region 添加GeoTiff图层

        /// <summary>
        /// 添加Geotiff图层
        /// </summary>
        /// <param name="name">图层名称</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="backgroundData">背景色</param>
        /// <param name="visible">是否显示</param>
        /// <param name="levelZeroTileSizeDegrees">第0层的度数</param>
        /// <param name="numberLevels">总共的层数</param>
        /// <param name="tiffFiles">要显示的Tiff文件</param>
        /// <param name="cachePath">缓存存放路径</param>
        /// <returns></returns>
        public bool AddGeoTiffLayer(string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, byte backgroundData, byte opacity, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string[] tiffFiles, string cachePath)
        {
            try
            {
                QRSTWorldGlobeControl window = this.GlobeControl;
                //切片图层
                RenderableObjectList tiledImage = GetGeotiffLayer(window, name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, backgroundData, opacity, visible, levelZeroTileSizeDegrees, numberLevels, tiffFiles, cachePath);
                tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
                //window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, tiledImage);
                window.CurrentWorld.RenderableObjects.Add(tiledImage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public RenderableObjectList GetGeotiffLayer(QRSTWorldGlobeControl window, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, byte backgroundData, byte opacity, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string[] tiffFiles, string cachePath)
        {
            //切片图层
            RenderableObjectList tiledImage = new RenderableObjectList(name);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            //是否显示此图层
            tiledImage.IsOn = visible;
            tiledImage.DisableExpansion = false;
            tiledImage.IsShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象
            tiledImage.MetaData.Add("World", window.CurrentWorld);//着重看
            tiledImage.MetaData.Add("Cache", window.Cache);//着重看
            tiledImage.MetaData["north"] = maxLat;
            tiledImage.MetaData["south"] = minLat;
            tiledImage.MetaData["west"] = minLon;
            tiledImage.MetaData["east"] = maxLon;

            //此图层做为表面图层
            tiledImage.RenderPriority = RenderPriority.Custom;

            //是否显示数字高程
            bool terrainMapped = true;
            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;

            //切片图层对象
            GeotiffLayerStore ia = new GeotiffLayerStore();
            ia.LevelZeroTileSizeDegrees = levelZeroTileSizeDegrees;
            ia.LevelCount = numberLevels;
            ia.ImageExtension = "jpg";
            ia.CacheDirectory = cachePath;
            ia.Layers = tiffFiles;
            ia.formate = "image/png";
            ia.ServerLogo = Path.Combine(window.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                window.CurrentWorld,
                distanceAboveSurface,
                maxLat,
                minLat,
                minLon,
                maxLon,
                terrainMapped,
                imageStores
            );
            qts.ServerLogoFilePath = ia.ServerLogo;
            qts.CacheExpirationTime = dataExpiration;

            ////透明色
            Color c = Color.FromArgb(opacity, backgroundData, backgroundData, backgroundData);//Color [A=255, R=0, G=0, B=0]
            qts.ColorKey = c.ToArgb();

            qts.ParentList = tiledImage;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)tiledImage.MetaData["XmlSource"]);
            tiledImage.Add(qts);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            return tiledImage;
        }

        #endregion 添加GeoTiff图层

        #region 添加静态WMS服务图层
        public bool AddURLLayer(string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
         double levelZeroTileSizeDegrees, int numberLevels, string cachePath, string serverUrl, string dataSetName)
        {
            try
            {
                QRSTWorldGlobeControl window = this.GlobeControl;
                //切片图层
                RenderableObject tiledImage = GetURLLayer(window, name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, visible, levelZeroTileSizeDegrees, numberLevels, cachePath, serverUrl, dataSetName);
                tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
                window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, tiledImage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public RenderableObjectList GetURLLayer(QRSTWorldGlobeControl window, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string cachePath, string serverUrl, string dataSetName)
        {

            //Blue Marble 切片图层
            RenderableObjectList parentRenderable = new RenderableObjectList(name);
            parentRenderable.ParentList = window.CurrentWorld.RenderableObjects;
            //是否显示此图层
            parentRenderable.IsOn = true;
            parentRenderable.DisableExpansion = false;
            //parentRenderable.ShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象	
            parentRenderable.MetaData.Add("World", window.CurrentWorld);//着重看
            //此图层做为表面图层
            parentRenderable.RenderPriority = QRST.WorldGlobeTool.RenderPriority.SurfaceImages;
            //图层的空间信息，范围信息，高程信息
            double north = Convert.ToDouble(maxLat);
            double south = Convert.ToDouble(minLat);
            double west = Convert.ToDouble(minLon);
            double east = Convert.ToDouble(maxLon);
            //是否显示数字高程
            bool terrainMapped = true;

            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            string imageFileExtension = "jpg";//切片的后缀名
            string layerName = name;
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;
            string cacheDir = Path.Combine(window.CacheDirectory, layerName);//缓存路径
            byte opacity = 255;//透明色

            TiledImageStore ia = new TiledImageStore();
            ia.m_dataSetName = dataSetName;
            ia.m_serverUri = serverUrl;
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
                window.CurrentWorld,
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
            return parentRenderable;

        }
        /// <summary>
        /// 5.添加静态WMS图层
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="visible">是否显示</param>
        /// <param name="levelZeroTileSizeDegrees">第一层格网的度数</param>
        /// <param name="numberLevels">总共的层数</param>
        /// <param name="imageFileExtension">切片的后缀名</param>
        /// <param name="cacheDir">缓存存放路径</param>
        /// <param name="serverGetMapUrl">Wms图层的Http地址</param>
        /// <param name="wmsVersion">1.3.0</param>
        /// <returns>返回布尔类型，是否添加Wms成功</returns>
        public bool AddWmsLayer(string name, string layerName, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string imageFileExtension, string cacheDir, string serverGetMapUrl, string wmsVersion)
        {
            try
            {
                QRSTWorldGlobeControl window = this.GlobeControl;
                RenderableObject tiledImage = GetWmsLayer(name, layerName, distanceAboveSurface, minLat, maxLat, minLon, maxLon, visible, levelZeroTileSizeDegrees, numberLevels, imageFileExtension, cacheDir, serverGetMapUrl, wmsVersion);
                //window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, tiledImage);
                window.CurrentWorld.RenderableObjects.ChildObjects.Add(tiledImage);

                tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 6.获得静态WMS图层
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="distanceAboveSurface">高度</param>
        /// <param name="minLat">最小经度</param>
        /// <param name="maxLat">最大经度</param>
        /// <param name="minLon">最小纬度</param>
        /// <param name="maxLon">最大纬度</param>
        /// <param name="visible">是否显示</param>
        /// <param name="levelZeroTileSizeDegrees">第一层格网的度数</param>
        /// <param name="numberLevels">总共的层数</param>
        /// <param name="imageFileExtension">切片的后缀名</param>
        /// <param name="cacheDir">缓存存放路径</param>
        /// <param name="serverGetMapUrl">Wms图层的Http地址</param>
        /// <param name="wmsVersion">1.3.0</param>
        /// <returns>返回静态Wms图层</returns>
        public RenderableObject GetWmsLayer(string name, string layerName, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string imageFileExtension, string cacheDir, string serverGetMapUrl, string wmsVersion)
        {
            QRSTWorldGlobeControl window = this.GlobeControl;
            //是否显示数字高程
            bool terrainMapped = false;
            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;

            WmsImageStore wmsLayerStore = new WmsImageStore();
            wmsLayerStore.ImageFormat = "image/png";
            wmsLayerStore.ImageExtension = imageFileExtension; //png
            wmsLayerStore.CacheDirectory = cacheDir;//;
            wmsLayerStore.ServerGetMapUrl = serverGetMapUrl;
            wmsLayerStore.Version = wmsVersion;
            wmsLayerStore.WMSLayerName = layerName + "&transparent=true";

            string serverLogoPath = Path.Combine(this.m_GlobeControl.DataDirectory, @"Icons\dcsrsdcsf.png");

            wmsLayerStore.LevelCount = numberLevels;
            wmsLayerStore.LevelZeroTileSizeDegrees = levelZeroTileSizeDegrees;
            wmsLayerStore.ServerLogo = serverLogoPath;
            imageStores[0] = wmsLayerStore;
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                this.GlobeControl.CurrentWorld,
                distanceAboveSurface,
                maxLat,
                minLat,
                minLon,
                maxLon,
                terrainMapped,
                imageStores
            );

            qts.CacheExpirationTime = dataExpiration;

            ////透明色
            System.Drawing.Color c = Color.FromArgb(255, 0, 0, 0);//Color [A=255, R=0, G=0, B=0]
            qts.ColorKey = c.ToArgb();

            qts.ParentList = window.CurrentWorld.RenderableObjects;
            qts.IsOn = true;
            qts.MetaData["north"] = maxLat;
            qts.MetaData["south"] = minLat;
            qts.MetaData["west"] = minLon;
            qts.MetaData["east"] = maxLon;

            qts.ParentList = window.CurrentWorld.RenderableObjects;
            qts.IsOn = visible;
            return (RenderableObject)qts;
        }

        #endregion 添加静态WMS服务图层
             
        #region 添加矢量图层

        /// <summary>
        /// 获取矢量文件中矢量的类型
        /// </summary>
        /// <param name="shapeFilePath">矢量文件路径</param>
        /// <returns>返回矢量类型</returns>
        public ShapeFeatureType GetShapeFeatureType(string shapeFilePath)
        {
            using (Shapefile sf = Shapefile.OpenFile(shapeFilePath))
            {
                switch (sf.FeatureType)
                {
                    case DotSpatial.Topology.FeatureType.Point:
                        return ShapeFeatureType.Point;
                    case DotSpatial.Topology.FeatureType.Line:
                        return ShapeFeatureType.Line;
                    case DotSpatial.Topology.FeatureType.Polygon:
                        return ShapeFeatureType.Polygon;
                    case DotSpatial.Topology.FeatureType.MultiPoint:
                        return ShapeFeatureType.MultiPoint;
                    case DotSpatial.Topology.FeatureType.Unspecified:
                    default:
                        return ShapeFeatureType.Unspecified;
                }
            }
        }

        /// <summary>
        /// 添加矢量图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="shapefile"></param>
        /// <param name="lineColorAtImageMode"></param>
        /// <param name="linwidth"></param>
        /// <param name="maxAltitude"></param>
        /// <param name="minAltitude"></param>
        public void AddShapeLineOrPolygonLayer(string layerName, string shapefile,
            Color lineColorAtImageMode, Color lineColorAtMapMode,
            float linwidth, double maxAltitude, double minAltitude,
            bool isShowLabel, string labelColumnName, Color textColorAtImageMode, Color textColorAtMapMode)
        {
            this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(getShapeLineOrPolygonLayer(layerName,
                shapefile, true, lineColorAtImageMode, lineColorAtMapMode,
                linwidth, maxAltitude, minAltitude, isShowLabel,
                labelColumnName, textColorAtImageMode, textColorAtMapMode));
        }

        /// <summary>
        /// 添加矢量图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="shapefile"></param>
        /// <param name="isOn"></param>
        /// <param name="lineColorAtImageMode"></param>
        /// <param name="lineColorAtMapMode"></param>
        /// <param name="linwidth"></param>
        /// <param name="maxAltitude"></param>
        /// <param name="minAltitude"></param>
        /// <param name="isShowLabel"></param>
        /// <param name="labelColumnName"></param>
        /// <param name="textColorAtImageMode"></param>
        /// <param name="textColorAtMapMode"></param>
        /// <returns></returns>
        private DrawShapefileLayer getShapeLineOrPolygonLayer(string layerName, string shapefile,
            bool isOn,
            Color lineColorAtImageMode, Color lineColorAtMapMode,
            float linwidth, double maxAltitude, double minAltitude,
            bool isShowLabel, string labelColumnName, Color textColorAtImageMode, Color textColorAtMapMode)
        {
            DrawShapefileLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawShapefileLayer;
                }
            }

            if (delyr == null)
                delyr = new DrawShapefileLayer(layerName, lineColorAtImageMode, lineColorAtMapMode,
                    linwidth, this.m_GlobeControl.CurrentWorld, this.m_GlobeControl.DrawArgs,
                    maxAltitude, minAltitude, isShowLabel, labelColumnName, textColorAtImageMode, textColorAtMapMode);
            delyr.RenderPriority = RenderPriority.LinePaths;
            delyr.IsOn = isOn;
            delyr.ShapeFilePath = shapefile;
            return delyr;
        }

        /// <summary>
        /// 从矢量文件中添加点标注图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="shapeFilePath"></param>
        /// <param name="labelColumnName"></param>
        /// <param name="iconWidth"></param>
        /// <param name="iconHeight"></param>
        /// <param name="iconPath"></param>
        /// <param name="maxVisible"></param>
        /// <param name="minVisible"></param>
        /// <param name="textColorAtImageMode"></param>
        /// <param name="textColorAtMapMode"></param>
        /// <returns></returns>
        public QRST.WorldGlobeTool.Renderable.Icons getShapePointLayer(string layerName,
            string shapeFilePath, string labelColumnName,
            int iconWidth, int iconHeight, string iconPath,
            double maxVisible, double minVisible,
            Color textColorAtImageMode, Color textColorAtMapMode)
        {
             QRST.WorldGlobeTool.Renderable.Icons iconsLayer = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    iconsLayer = lyr as QRST.WorldGlobeTool.Renderable.Icons;
                }
            }

            if (iconsLayer == null)

                iconsLayer = new QRST.WorldGlobeTool.Renderable.Icons(layerName);

            foreach (QRST.WorldGlobeTool.Renderable.Icon icon in getPointIcons(shapeFilePath, labelColumnName, iconWidth, iconHeight, iconPath))
            {
                icon.MaximumDisplayDistance = maxVisible;
                icon.MinimumDisplayDistance = minVisible;
                iconsLayer.Add(icon);
            }
            iconsLayer.RenderPriority = RenderPriority.Icons;
            iconsLayer.IsSelectable = false;
            iconsLayer.IsOn = true;
            iconsLayer.MaximumDisplayDistance = maxVisible;
            iconsLayer.MinimumDisplayDistance = minVisible;
            iconsLayer.DescriptionColorAtImageMode = textColorAtImageMode.ToArgb();
            iconsLayer.DescriptionColorAtMapMode = textColorAtMapMode.ToArgb();
            return iconsLayer;
        }
        #region 从矢量文件中添加点标注

        /// <summary>
        /// 从矢量文件中添加点标注图层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="shapeFilePath"></param>
        /// <param name="labelColumnName"></param>
        /// <param name="iconWidth"></param>
        /// <param name="iconHeight"></param>
        /// <param name="iconPath"></param>
        /// <param name="maxVisible"></param>
        /// <param name="minVisible"></param>
        /// <param name="textColorAtImageMode"></param>
        /// <param name="textColorAtMapMode"></param>
        /// <returns></returns>
        public QRST.WorldGlobeTool.Renderable.Icons AddShapePointLayer(string layerName,
            string shapeFilePath, string labelColumnName,
            int iconWidth, int iconHeight, string iconPath,
            double maxVisible, double minVisible,
            Color textColorAtImageMode, Color textColorAtMapMode)
        {
            QRST.WorldGlobeTool.Renderable.Icons iconsLayer = new QRST.WorldGlobeTool.Renderable.Icons(layerName);

            foreach (QRST.WorldGlobeTool.Renderable.Icon icon in getPointIcons(shapeFilePath, labelColumnName, iconWidth, iconHeight, iconPath))
            {
                icon.MaximumDisplayDistance = maxVisible;
                icon.MinimumDisplayDistance = minVisible;
                iconsLayer.Add(icon);
            }
            iconsLayer.RenderPriority = RenderPriority.Icons;
            iconsLayer.IsSelectable = false;
            iconsLayer.IsOn = true;
            iconsLayer.MaximumDisplayDistance = maxVisible;
            iconsLayer.MinimumDisplayDistance = minVisible;
            iconsLayer.DescriptionColorAtImageMode = textColorAtImageMode.ToArgb();
            iconsLayer.DescriptionColorAtMapMode = textColorAtMapMode.ToArgb();
            this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(iconsLayer);
            return iconsLayer;
        }

        /// <summary>
        /// 从文件中读取位置基本信息
        /// </summary>
        /// <param name="shapeFileName"></param>
        /// <param name="iconWidth"></param>
        /// <param name="iconHeight"></param>
        /// <param name="iconPath"></param>
        /// <returns></returns>
        private QRST.WorldGlobeTool.Renderable.Icon[] getPointIcons(string shapeFileName, string labelColumnName, int iconWidth, int iconHeight, string iconPath)
        {
            QRST.WorldGlobeTool.Renderable.Icon[] icons = null;

            //打开矢量文件
            Shapefile _sf = Shapefile.OpenFile(shapeFileName);
            //投影转换
            ProjectionInfo piWGS1984 = KnownCoordinateSystems.Geographic.World.WGS1984;
            if (_sf.Projection != piWGS1984 && _sf.CanReproject)
                _sf.Reproject(piWGS1984);//如果投影不是WGS1984投影，则进行投影变换

            if (_sf.FeatureType == DotSpatial.Topology.FeatureType.Point || _sf.FeatureType == DotSpatial.Topology.FeatureType.MultiPoint)
            {
                icons = new QRST.WorldGlobeTool.Renderable.Icon[_sf.Features.Count];
                for (int i = 0; i < _sf.Features.Count; i++)
                {
                    icons[i] = new QRST.WorldGlobeTool.Renderable.Icon(
                        (labelColumnName!="")?_sf.Features[i].DataRow[labelColumnName].ToString():"",
                        _sf.Features[i].Envelope.Minimum.Y,
                        _sf.Features[i].Envelope.Minimum.X, 0);
                    icons[i].IsSelectable = false;
                    icons[i].IsOn = true;
                    icons[i].Width = iconWidth;
                    icons[i].Height = iconHeight;
                    icons[i].TextureFileName = iconPath;
                    icons[i].SaveFilePath = iconPath;
                    icons[i].NameAlwaysVisible = true;
                }
            }

            return icons;
        }


        #endregion


        #endregion

        #region 添加数据库检索结果图层
        /// <summary>
        /// 添加数据库检索结果边框图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="extents">数据边框列表</param>
        /// <param name="lineColor">数据边框线条颜色</param>
        public void AddDBSearchResultExtentsLayer1(string layerName, List<List<float>> extents, Color lineColor)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer(layerName, lineColor,
                    this.m_GlobeControl.CurrentWorld, this.m_GlobeControl.DrawArgs);
                this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }
            delyr.RenderPriority = RenderPriority.LinePaths;
            delyr.IsOn = true;
            delyr.AddExtents1(extents);
        }
        /// <summary>
        /// 添加数据库检索结果边框图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="extents">数据边框列表</param>
        /// <param name="lineColor">数据边框线条颜色</param>
        public void AddDBSearchResultExtentsLayer(string layerName, List<RectangleF> extents, Color lineColor)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer(layerName, lineColor,
                    this.m_GlobeControl.CurrentWorld, this.m_GlobeControl.DrawArgs);
                this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }
            delyr.RenderPriority = RenderPriority.LinePaths;
            delyr.IsOn = true;
            delyr.AddExtents(extents);
        }

        /// <summary>
        /// 添加数据库检索结果边框图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="extents">数据边框字典</param>
        /// <param name="isShowColorBar">可选项，是否显示颜色条</param>
        /// <param name="colorBarLayerName">可选项，显示颜色条时的颜色条图层名称</param>
        /// <param name="colorBarTitle">可选项，在显示颜色条时需要指定颜色条的标题</param>
        public void AddDBSearchResultExtentsLayer(string layerName, Dictionary<RectangleF, int> extents, bool isShowColorBar = false, string colorBarLayerName = "Color Bar", string colorBarTitle = "")
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer(layerName, Color.FromArgb(50, 0, 255, 0),
                    this.m_GlobeControl.CurrentWorld, this.m_GlobeControl.DrawArgs);
                this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }
            delyr.RenderPriority = RenderPriority.LinePaths;
            delyr.IsOn = true;
            delyr.IsSelectable = true;
            delyr.AddExtents(extents);
            if (isShowColorBar)
            {
                RemoveDBSearchColorBar(colorBarLayerName);
                int maxCount = 0;
                int minCount = int.MaxValue;
                foreach (KeyValuePair<RectangleF, int> kvp in extents)
                {
                    maxCount = kvp.Value > maxCount ? kvp.Value : maxCount;
                    minCount = kvp.Value < minCount ? kvp.Value : minCount;
                }
                if (maxCount == minCount)
                    minCount = 0;

                if (maxCount == minCount && maxCount == 0)
                {
                    maxCount = 1;
                    minCount = 0;
                }

                ColorBar cb = new ColorBar(colorBarLayerName, minCount, maxCount);
                cb.ValueType = ColorBarValueType.整型;
                cb.IsShowMiddleValue = false;
                cb.Anchor = ColorBarAnchor.Right;
                cb.Width = 20;
                cb.Height = 20;
                cb.ColorBarColorBlend = ColorBlend.QRSTRainbow6;
                this.GlobeControl.CurrentWorld.RenderableObjects.Add(cb);
            }
        }

        /// <summary>
        /// 添加数据库检索结果边框图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="extents">多边形顶点列表</param>
        /// <param name="lineColor">线条颜色</param>
        /// <param name="isClosedLoop">多边形的顶点列表是否构成闭环</param>
        public void AddDBSearchResultExtentsLayer(string layerName, List<double[]> extents, Color lineColor, bool isClosedLoop)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }

            if (delyr == null)
            {
                delyr = new DrawExtentsLayer(layerName, lineColor,
                    this.m_GlobeControl.CurrentWorld, this.m_GlobeControl.DrawArgs);
                this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(delyr);        //加载图层 drawLayer
            }
            delyr.RenderPriority = RenderPriority.LinePaths;
            delyr.IsOn = true;
            delyr.AddExtents(extents, isClosedLoop);
        }

        /// <summary>
        /// 选中单个或多个数据检索结果范围框
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="extents">选中的范围框列表</param>
        /// <param name="selectLineColor">选中的范围框线条的颜色</param>
        public void SelectDBSearchResultExtents1(string layerName, List<List<float>> extents, Color selectLineColor)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }
            if (delyr != null)
            {
                delyr.SelectExtents1(extents, selectLineColor);
            }
        }
        /// <summary>
        /// 选中单个或多个数据检索结果范围框
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="extents">选中的范围框列表</param>
        /// <param name="selectLineColor">选中的范围框线条的颜色</param>
        public void SelectDBSearchResultExtents(string layerName, List<RectangleF> extents, Color selectLineColor)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }
            if (delyr != null)
            {
                delyr.SelectExtents(extents, selectLineColor);
            }
        }

        /// <summary>
        /// 选中单个或多个数据检索结果范围框
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="extents">选中的范围框顶点数组列表</param>
        /// <param name="selectLineColor">选中的范围框线条的颜色</param>
        /// <param name="isClosedLoop">多边形的顶点列表是否构成闭环</param>
        public void SelectDBSearchResultExtents(string layerName, List<double[]> extents, Color selectLineColor, bool isClosedLoop)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }
            if (delyr != null)
            {
                delyr.SelectExtents(extents, selectLineColor, isClosedLoop);

            }
        }

        /// <summary>
        /// 清除选中的数据检索结果范围框
        /// </summary>
        /// <param name="layerName">图层名称</param>
        public void ClearSelectDBSearchResultExtents(string layerName)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }
            if (delyr != null)
            {
                delyr.ClearSelectExtents();
            }
        }

        /// <summary>
        /// 清除数据库检索结果边框图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        public void ClearDBSearchResultExtents(string layerName)
        {
            DrawExtentsLayer delyr = null;
            foreach (RenderableObject lyr in this.m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (lyr.Name == layerName)
                {
                    delyr = lyr as DrawExtentsLayer;
                    break;
                }
            }

            if (delyr != null)
            {
                delyr.ClearExtents();
            }
        }

        /// <summary>
        /// 移除与数据库检索结果相关联的色带
        /// </summary>
        /// <param name="colorBarLayerName"></param>
        public void RemoveDBSearchColorBar(string colorBarLayerName = "Color Bar")
        {
            for (int i = 0; i < m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects.Count; i++)
            {
                if (m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects[i] is RenderableObject)
                {
                    if (((RenderableObject)m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects[i]).Name == colorBarLayerName)
                    {
                        ((RenderableObject)m_GlobeControl.CurrentWorld.RenderableObjects.ChildObjects[i]).Delete();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 是否存在矩形框
        /// </summary>
        /// <param name="extents">矩形框列表</param>
        /// <param name="rectangleF">要寻找的矩形框</param>
        /// <returns>存在指定的矩形框返回true，否则返回false</returns>
        public bool IsExistRectangle(Dictionary<RectangleF, int> extents, RectangleF rectangleF)
        {
            foreach (KeyValuePair<System.Drawing.RectangleF, int> kvp in extents)
            {
                if (kvp.Key.Equals(rectangleF))
                    return true;
            }
            return false;
        }

        #endregion

        #region 添加省份标注

        /// <summary>
        /// 添加省份标注地标图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="fileName">以“.qrstp”为后缀的省份标注文件路径</param>
        /// <param name="iconWidth">省份图标宽度</param>
        /// <param name="iconHeight">省份图标高度</param>
        /// <param name="iconPath">省份图标位置</param>
        /// <param name="maxVisible">省份标注最大可见高度</param>
        /// <param name="minVisible">省份标注最小可见高度</param>
        /// <returns></returns>
        public bool AddPlacesLayer(string layerName,
            string fileName, int iconWidth, int iconHeight, string iconPath,
            double maxVisible, double minVisible, Color textColorAtImageMode, Color textColorAtMapMode)
        {
            return AddPlaces(GetPlaces(fileName, iconWidth, iconHeight, iconPath), layerName, maxVisible, minVisible, textColorAtImageMode, textColorAtMapMode);
        }

        /// <summary>
        /// 添加省的图标
        /// </summary>
        /// <param name="icons"></param>
        /// <returns></returns>
        private bool AddPlaces(object[] icons, string Name, double maxVisible, double minVisible, Color textColorAtImageMode, Color textColorAtMapMode)
        {
            try
            {
                Icons places = new Icons(Name);
                QRST.WorldGlobeTool.Renderable.Icon ic = null;
                foreach (object icon in icons)
                {
                    if (icon is QRST.WorldGlobeTool.Renderable.Icon)
                    {
                        ic = icon as QRST.WorldGlobeTool.Renderable.Icon;
                        ic.ParentList = places;
                        ic.MaximumDisplayDistance = maxVisible;
                        ic.MinimumDisplayDistance = minVisible;
                        places.Add(ic);
                        ic.OnClick += new RenderableObject.Click(places_OnClick);
                    }
                }
                places.IsSelectable = true;
                places.IsOn = true;
                places.MaximumDisplayDistance = maxVisible;
                places.MinimumDisplayDistance = minVisible;
                places.DescriptionColorAtImageMode = textColorAtImageMode.ToArgb();
                places.DescriptionColorAtMapMode = textColorAtMapMode.ToArgb();
                this.AddLayer(places);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 省图标单击事件，显示关于省份的简介信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="drawArgs"></param>
        private void places_OnClick(RenderableObject sender, DrawArgs drawArgs)
        {
            QRST.WorldGlobeTool.Renderable.Icon ic = sender as QRST.WorldGlobeTool.Renderable.Icon;
            if (drawArgs.WorldCamera.Altitude < ic.MaximumDisplayDistance)
            {

            }
        }

        /// <summary>
        /// 从文件中读取位置基本信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="iconWidth"></param>
        /// <param name="iconHeight"></param>
        /// <param name="iconPath"></param>
        /// <returns></returns>
        private QRST.WorldGlobeTool.Renderable.Icon[] GetPlaces(string fileName, int iconWidth, int iconHeight, string iconPath)
        {
            QRST.WorldGlobeTool.Renderable.Icon[] icons = null;
            FileInfo fileInfo = new FileInfo(fileName);
            //打开当前qrstp文件
            using (BinaryReader reader = new BinaryReader(new BufferedStream(fileInfo.OpenRead()), Encoding.GetEncoding("GB2312")))
            {
                //读出来总共有多少挑数据
                int placenameCount = reader.ReadInt32();
                icons = new QRST.WorldGlobeTool.Renderable.Icon[placenameCount];
                //循环查询地域名
                for (int i = 0; i < placenameCount; i++)
                {
                    //获得当前数据信息
                    QrstPlacename pn = new QrstPlacename();
                    ReadPlaceName(reader, ref pn);
                    icons[i] = new QRST.WorldGlobeTool.Renderable.Icon(pn.Name, pn.Lat, pn.Lon, 0);
                    icons[i].IsSelectable = true;
                    icons[i].IsOn = true;
                    icons[i].Width = iconWidth;
                    icons[i].Height = iconHeight;
                    icons[i].TextureFileName = iconPath;
                    icons[i].SaveFilePath = iconPath;
                    icons[i].NameAlwaysVisible = true;
                    icons[i].Description = pn.metaData["Description"] == null ? string.Empty : pn.metaData["Description"].ToString();
                }
            }

            return icons;
        }

        /// <summary>
        /// 读取一条数据的信息
        /// </summary>
        /// <param name="br"></param>
        /// <param name="pn"></param>
        private void ReadPlaceName(BinaryReader br, ref QrstPlacename pn)
        {
            pn.Name = br.ReadString(); // 读取名称
            pn.Lat = br.ReadSingle(); // 读取经纬度
            pn.Lon = br.ReadSingle(); // 读取经纬度
            pn.metaData = new Hashtable();
            int metaCount = br.ReadInt32(); // 读取有多少个元数据

            //读取元数据信息
            for (int j = 0; j < metaCount; j++)
            {
                string strKey = br.ReadString();
                string strValue = br.ReadString();
                pn.metaData.Add(strKey, strValue);
            }
        }

        #endregion 添加省份标注

        #region 添加控制点

        /// <summary>
        /// 从打开的对话框选择的文件中添加几何或辐射控制点图层
        /// </summary>
        /// <param name="gcpType">控制点类型</param>
        public void AddGCPLayerBySelectFile(GCPType gcpType)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT文件|*.txt;*.TXT";
            ofd.Title = "打开控制点文件";
            ofd.FileName = string.Format("{0}控制点文件", gcpType == GCPType.GeoGCP ? "几何" : "辐射");
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string selectFileName = ofd.FileName;
                switch (gcpType)
                {
                    case GCPType.GeoGCP:
                        {
                            List<GCP> gcps = getGeoGCPs(selectFileName);
                            if (gcps.Count > 0)
                                AddGCPs(gcps, "几何控制点：" + Path.GetFileNameWithoutExtension(selectFileName),
                                    2500000);
                            break;
                        }
                    case GCPType.ATMGCP:
                        {
                            string sourceImageWidthAndHeight;
                            List<GCP> gcps = getATMGCPs(selectFileName, out sourceImageWidthAndHeight);
                            if (gcps.Count > 0)
                                AddGCPs(gcps, "辐射控制点：" + Path.GetFileNameWithoutExtension(selectFileName),
                                    2500000, sourceImageWidthAndHeight);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 从指定控制点文件路径添加几何或辐射控制点图层
        /// </summary>
        /// <param name="gcpFilePath">控制点文件路径</param>
        /// <param name="gcpType">控制点类型</param>
        public void AddGCPLayerFromFile(string gcpFilePath, GCPType gcpType)
        {
            switch (gcpType)
            {
                case GCPType.GeoGCP:
                    {
                        List<GCP> gcps = getGeoGCPs(gcpFilePath);
                        if (gcps.Count > 0)
                            AddGCPs(gcps, "几何控制点：" + Path.GetFileNameWithoutExtension(gcpFilePath),
                                2500000);
                        break;
                    }
                case GCPType.ATMGCP:
                    {
                        string sourceImageWidthAndHeight;
                        List<GCP> gcps = getATMGCPs(gcpFilePath, out sourceImageWidthAndHeight);
                        if (gcps.Count > 0)
                            AddGCPs(gcps, "辐射控制点：" + Path.GetFileNameWithoutExtension(gcpFilePath),
                                2500000, sourceImageWidthAndHeight);
                        break;
                    }
            }
        }

        /// <summary>
        /// 添加一个空的控制点图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="gcpType">控制点类型</param>
        public void AddGCPLayer(string layerName, GCPType gcpType)
        {
            GCPs gcpsLayer = new GCPs(layerName);
            gcpsLayer.IsSelectable = true;
            gcpsLayer.IsOn = true;
            RenderableObjectList rol = findListLayer(this.GlobeControl.CurrentWorld.RenderableObjects, layerName);
            if (rol != null)
                rol.Delete();
            gcpsLayer.ParentList = this.GlobeControl.CurrentWorld.RenderableObjects;
            this.AddLayer(gcpsLayer);
        }

        /// <summary>
        /// 添加控制点到指定的图层中
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="gcpType">控制点类型</param>
        /// <param name="gcpName">控制点名称</param>
        /// <param name="lat">控制点纬度</param>
        /// <param name="lon">控制点经度</param>
        /// <param name="x">控制点横坐标</param>
        /// <param name="y">控制点纵坐标</param>
        /// <param name="isErrorNormal">控制点误差是否在正常范围内</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddGCPToLayer(string layerName, GCPType gcpType,
            string gcpName, double lat, double lon, double x, double y, bool isErrorNormal)
        {
            try
            {
                GCPs resultGCPs = findListLayer(this.m_GlobeControl.CurrentWorld.RenderableObjects, layerName) as GCPs;
                GCP gcp = new GCP(gcpName, y, x, lat, lon, 0);
                gcp.IsSelectable = true;
                gcp.IsOn = true;
                gcp.NameAlwaysVisible = true;
                gcp.Description = string.Format("纬度：{0}°\n经度：{1}°", lat, lon);
                gcp.QrstGlobe = this.m_GlobeControl;
                gcp.ParentList = resultGCPs;
                gcp.GcpType = gcpType;
                gcp.IsGCPErrorNormal = isErrorNormal;
                resultGCPs.Add(gcp);
                gcp.MaximumDisplayDistance = 2500000;
                Goto(lat, lon, 25000);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加影像边界框
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="boundingBoxVertex">包含影像顶点坐标的数组: 每个顶点经度坐标在前，纬度坐标在后</param>
        public void AddImageBoundingBox(string layerName, double[] boundingBoxVertex)
        {
            DrawExtentsLayer delyr = null;
            delyr = new DrawExtentsLayer(layerName, Color.FromArgb(255, 255, 255, 0),
                    this.m_GlobeControl.CurrentWorld, this.m_GlobeControl.DrawArgs);
            delyr.RenderPriority = RenderPriority.LinePaths;
            delyr.IsOn = true;
            List<double[]> extents = new List<double[]>();
            extents.Add(boundingBoxVertex);
            delyr.AddExtents(extents, false);
            this.m_GlobeControl.CurrentWorld.RenderableObjects.Add(delyr);
        }

        /// <summary>
        /// 更新控制点误差状态，改变控制点图标颜色
        /// </summary>
        /// <param name="layerName">控制点图层名称</param>
        /// <param name="gcpIDAndErrorState">控制点编号和误差状态字典</param>
        /// <returns>返回是否更新成功</returns>
        public bool UpdateGCPErrorState(string layerName, Dictionary<int, bool> gcpIDAndErrorState)
        {
            try
            {
                GCPs resultGCPs = findListLayer(this.m_GlobeControl.CurrentWorld.RenderableObjects, layerName) as GCPs;
                foreach (var v in gcpIDAndErrorState)
                {
                    IEnumerable<GCP> query = resultGCPs.ChildObjects.Cast<GCP>().Where(g => g.Name == v.Key.ToString());
                    if (query.Count() > 0)
                        query.ToList<GCP>()[0].IsGCPErrorNormal = v.Value;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从指定的控制点图层中删除控制点编号列表中的控制点
        /// </summary>
        /// <param name="layerName">控制点图层名称</param>
        /// <param name="gcpIDList">要删除的控制点编号列表</param>
        /// <returns>返回是否删除成功</returns>
        public bool DeleteGCP(string layerName, List<string> gcpIDList)
        {
            try
            {
                GCPs resultGCPs = findListLayer(this.m_GlobeControl.CurrentWorld.RenderableObjects, layerName) as GCPs;
                foreach (var gcpID in gcpIDList)
                {
                    IEnumerable<GCP> query = resultGCPs.ChildObjects.Cast<GCP>().Where(g => g.Name == gcpID);
                    if (query.Count() > 0)
                    {
                        query.ToList<GCP>()[0].Delete();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加控制点
        /// </summary>
        /// <param name="gcps"></param>
        /// <param name="layerName"></param>
        /// <param name="maxVisible"></param>
        /// <param name="sourceImageWidthAndHeight"></param>
        /// <returns></returns>
        private bool AddGCPs(List<GCP> gcps, string layerName, double maxVisible, string sourceImageWidthAndHeight = null)
        {
            try
            {
                GCPs resultGCPs = new GCPs(layerName);
                foreach (GCP gcp in gcps)
                {
                    gcp.ParentList = resultGCPs;
                    gcp.MaximumDisplayDistance = maxVisible;
                    resultGCPs.Add(gcp);
                }
                resultGCPs.IsSelectable = true;
                resultGCPs.IsOn = true;
                if (sourceImageWidthAndHeight != null)
                    resultGCPs.SourceImageWidthAndHeight = sourceImageWidthAndHeight;
                RenderableObjectList rol = findListLayer(this.GlobeControl.CurrentWorld.RenderableObjects, layerName);
                if (rol != null)
                    rol.Delete();
                resultGCPs.ParentList = this.GlobeControl.CurrentWorld.RenderableObjects;
                this.AddLayer(resultGCPs);
                double centerLat, centerLon;
                //定位地球旋转到控制点的中心位置
                resultGCPs.GetGCPsCenter(out centerLat, out centerLon);
                Goto(centerLat, centerLon, 700000);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 几何控制点TXT文件解析
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<GCP> getGeoGCPs(string fileName)
        {
            List<GCP> gcps = new List<GCP>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                string[] subLine;
                GCP gcp;
                while ((line = sr.ReadLine()) != null)
                {
                    subLine = line.Split(' ');
                    gcp = new GCP(subLine[0], double.Parse(subLine[2]), double.Parse(subLine[1]), double.Parse(subLine[4]), double.Parse(subLine[3]), 0);
                    gcp.IsSelectable = true;
                    gcp.IsOn = true;
                    gcp.NameAlwaysVisible = true;
                    gcp.Description = string.Format("纬度：{0}°\n经度：{1}°", subLine[4], subLine[3]);
                    //ZYM-20140108:增加误差大小的设置
                    gcp.GcpType = GCPType.GeoGCP;
                    gcp.QrstGlobe = this.m_GlobeControl;
                    gcp.IsGCPErrorNormal = bool.Parse(subLine[5]);
                    gcps.Add(gcp);
                }
            }
            return gcps;
        }

        /// <summary>
        /// 辐射控制点TXT文件解析
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<GCP> getATMGCPs(string fileName, out string sourceImageWidthAndHeight)
        {
            List<GCP> gcps = new List<GCP>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                string[] subLine;
                GCP gcp;
                sourceImageWidthAndHeight = sr.ReadLine();  //辐射控制点文件的第一行为图像的尺寸信息
                while ((line = sr.ReadLine()) != null)
                {
                    subLine = line.Split(',');
                    gcp = new GCP(subLine[0], double.Parse(subLine[2]), double.Parse(subLine[1]), double.Parse(subLine[4]), double.Parse(subLine[3]), 0);
                    gcp.IsSelectable = true;
                    gcp.IsOn = true;
                    gcp.NameAlwaysVisible = true;
                    gcp.Description = string.Format("纬度：{0}°\n经度：{1}°", subLine[3], subLine[2]);
                    //ZYM-20140108:增加误差大小的设置
                    gcp.GcpType = GCPType.ATMGCP;
                    gcp.QrstGlobe = this.m_GlobeControl;
                    gcp.IsGCPErrorNormal = bool.Parse(subLine[6]);
                    gcps.Add(gcp);
                }
            }
            return gcps;
        }

        #endregion 添加控制点


        #endregion 添加图层

        #region 对外提供接口和方法

        /// <summary>
        /// 更新渲染图层的三维影像图或三维地图模式
        /// </summary>
        /// <param name="renderableObject">渲染对象</param>
        public void UpdateRenderableMode(RenderableObject renderableObject)
        {
            renderableObject.Is3DMapMode = m_GlobeControl.Is3DMapMode;
            if (renderableObject is RenderableObjectList)
            {
                RenderableObjectList rol = renderableObject as RenderableObjectList;
                foreach (RenderableObject subRo in rol.ChildObjects)
                {
                    UpdateRenderableMode(subRo);
                }
            }
        }

        /// <summary>
        /// 使摄像机到某个特定位置
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="metersElevation"></param>
        public void Goto(double latitude, double longitude, double metersElevation)
        {
            this.GlobeControl.SetViewPosition(latitude, longitude, metersElevation);
        }

        /// <summary>
        /// 获取外接边框
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="isShapeFile">文件是否是矢量文件</param>
        /// <returns>返回输入栅格或矢量文件的外边框（经纬度坐标单位）</returns>
        public Envelop GetEnvelop(string fileName, bool isShapeFile)
        {
            Envelop envelop = new Envelop();
            try
            {
                Extent fileExtent = new Extent();
                ProjectionInfo piWGS1984 = KnownCoordinateSystems.Geographic.World.WGS1984;
                if (isShapeFile)
                {
                    Shapefile sf = Shapefile.OpenFile(fileName);
                    if (sf.Projection != piWGS1984 && sf.CanReproject)
                        sf.Reproject(piWGS1984);//如果投影不是WGS1984投影，则进行投影变换
                    fileExtent = sf.Extent;
                }
                else
                {
                    GdalRasterProvider grp = new GdalRasterProvider();
                    IRaster ir = grp.Open(fileName);
                    GdalImageProvider dip = new GdalImageProvider();
                    IImageData ddd = dip.Open(fileName);
                    if (ir.Projection != piWGS1984 && ir.CanReproject)
                        ir.Reproject(piWGS1984);//如果投影不是WGS1984投影，则进行投影变换
                    fileExtent = ir.Extent;
                }
                envelop.North = fileExtent.MaxY;
                envelop.South = fileExtent.MinY;
                envelop.West = fileExtent.MinX;
                envelop.East = fileExtent.MaxX;
            }
            catch (Exception ex)
            {
                MessageBox.Show("坐标提取失败：" + ex.Message);
            }
            return envelop;
        }

        /// <summary>
        /// 根据名称找到图层
        /// </summary>
        /// <param name="ro">父图层</param>
        /// <param name="name">要查找的图层的名称</param>
        /// <returns>RenderableObject类型，返回找到的图层</returns>
        public RenderableObject FindSubLayer(RenderableObject ro, string name)
        {
            string s = "";
            if (!ro.IsOn) return null;
            if (ro is RenderableObjectList)
            {
                RenderableObjectList rol = (RenderableObjectList)ro;
                for (int i = 0; i < rol.ChildObjects.Count; i++)
                {
                    RenderableObject l = (RenderableObject)rol.ChildObjects[i];
                    if (l is RenderableObjectList)
                    {
                        RenderableObject found = FindSubLayer((RenderableObject)l, name);
                        if (found != null) return found;
                    }
                    else
                    {
                        s += l.Name + ", ";
                        if (l.Name.IndexOf(name) != -1 && l.IsOn) return l;
                    }
                }
            }
            else
            {
                if (ro.Name.IndexOf(name) != -1 && ro.IsOn) return ro;
            }
            return null;
        }

        /// <summary>
        /// 根据经纬度，得到当前点的颜色信息
        /// </summary>
        /// <param name="layer">图层名</param>
        /// <param name="lat">经度</param>
        /// <param name="lon">纬度</param>
        /// <param name="level">第几层</param>
        /// <returns>当前经纬度下的点所对应的颜色值（Color）</returns>
        public Color GetColorAt(QuadTileSet layer, double lat, double lon, int level)
        {
            Color color = Color.White;
            // 找到当前层每格代表多少度
            double tileSizeDegree = layer.ImageStores[0].LevelZeroTileSizeDegrees;
            if (level > 0) for (int i = 0; i < level; i++) tileSizeDegree /= 2;
            // 计算当前经纬度所在格网行列号
            int row = (int)Math.Floor((lat + 90) / tileSizeDegree);
            int col = (int)Math.Floor((lon + 180) / tileSizeDegree);
            // 计算格网边界经纬度
            double south = -90.0f + row * tileSizeDegree;
            double north = south + tileSizeDegree;
            double west = -180.0f + col * tileSizeDegree;
            double east = west + tileSizeDegree;

            // 在缓存中查找
            string key = level.ToString() + row.ToString() + col.ToString();
            Bitmap img = (Bitmap)m_ImageCache[key];
            if (img == null)
            {
                //得到一个切片，根据最大最小经纬度得到当前切片
                QuadTile qt = new QuadTile(south, north, west, east, level, layer);
                //获得当前切片所在路径
                string tilePath = layer.ImageStores[0].GetLocalPath(qt);
                //判断当前纹理文件是否存在
                if (File.Exists(tilePath))
                {
                    //加载当前切片
                    Texture texture = layer.ImageStores[0].LoadFile(qt);
                    //转换成bmp
                    GraphicsStream gs = TextureLoader.SaveToStream(ImageFileFormat.Bmp, texture);
                    img = new Bitmap(gs);
                    //放入缓存中
                    m_ImageCache.Add(key, img);
                    texture.Dispose();
                }
                qt.Dispose();
            }
            // 在缓存中找到图片
            if (img != null)
            {
                // 计算当前经纬度所在图片中的行列号
                double x = (lon - west) / tileSizeDegree * img.Width;
                double y = (north - lat) / tileSizeDegree * img.Height;
                // 确保经纬度所在范围在图片内部
                if (x >= img.Width) x = img.Width - 1;
                if (y >= img.Height) y = img.Height - 1;
                if (x < 0) x = 0;
                if (y < 0) y = 0;

                // 计算与当前要取点最近的4个像素的平均值，作为当前要取像素的颜色值
                int xNW = (int)Math.Floor(x);
                int yNW = (int)Math.Floor(y);
                float xFactor = (float)(x - xNW);
                float yFactor = (float)(y - yNW);
                Color NW = img.GetPixel(xNW, yNW);
                Color NE = (xNW + 1) < img.Width ? img.GetPixel(xNW + 1, yNW) : NW;
                Color SW = (yNW + 1) < img.Height ? img.GetPixel(xNW, yNW + 1) : NW;
                Color SE = (xNW + 1) < img.Width && (yNW + 1) < img.Height ?
                    img.GetPixel(xNW + 1, yNW + 1) : NW;

                Color N = Color.FromArgb(
                    (byte)((float)NW.R * (1f - xFactor) + (float)NE.R * xFactor),
                    (byte)((float)NW.G * (1f - xFactor) + (float)NE.G * xFactor),
                    (byte)((float)NW.B * (1f - xFactor) + (float)NE.B * xFactor));
                Color S = Color.FromArgb(
                    (byte)((float)SW.R * (1f - xFactor) + (float)SE.R * xFactor),
                    (byte)((float)SW.G * (1f - xFactor) + (float)SE.G * xFactor),
                    (byte)((float)SW.B * (1f - xFactor) + (float)SE.B * xFactor));
                color = Color.FromArgb(
                    (byte)((float)N.R * (1f - yFactor) + (float)S.R * yFactor),
                    (byte)((float)N.G * (1f - yFactor) + (float)S.G * yFactor),
                    (byte)((float)N.B * (1f - yFactor) + (float)S.B * yFactor));
            }
            return color;
        }

        /// <summary>
        /// 释放影像资源
        /// </summary>
        public void DisposeImageCache()
        {
            if (m_ImageCache.Count > 0)
            {
                foreach (Bitmap img in m_ImageCache.Values)
                    img.Dispose();
                m_ImageCache.Clear();
            }
        }

        /// <summary>
        /// 添加图层对象
        /// </summary>
        /// <param name="layer">图层对象</param>
        /// <param name="index">图层索引位置</param>
        /// <returns></returns>
        public bool AddLayer(object layer, int index)
        {
            try
            {
                this.GlobeControl.CurrentWorld.RenderableObjects.ChildObjects.Insert(index, layer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加图层对象
        /// </summary>
        /// <param name="layer">图层对象</param>
        /// <returns></returns>
        public bool AddLayer(object layer)
        {
            try
            {
                this.GlobeControl.CurrentWorld.RenderableObjects.ChildObjects.Add(layer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取指定控制点图层名所包含的控制点列表
        /// </summary>
        /// <param name="gcpLayerName">图层名称</param>
        /// <returns>返回指定图层包含的控制点列表</returns>
        public List<GCP> GetGCPsByLayerName(string gcpLayerName)
        {
            List<GCP> resultGCP = new List<GCP>();
            foreach (Object obj in this.GlobeControl.CurrentWorld.RenderableObjects.ChildObjects)
            {
                if (obj is GCPs)
                {
                    GCPs tempGCPs = obj as GCPs;
                    if (tempGCPs.Name == gcpLayerName)
                    {
                        foreach (Object gcp in tempGCPs.ChildObjects)
                            resultGCP.Add((GCP)gcp);
                        break;
                    }
                }
            }
            return resultGCP;
        }

        #endregion 对外提供接口和方法

        #region 私有方法

        /// <summary>
        /// 重置瓦片集缓存
        /// </summary>
        /// <param name="ro"></param>
        private void resetQuadTileSetCache(RenderableObject ro)
        {
            if (ro.IsOn && ro is QuadTileSet)
            {
                QuadTileSet qts = (QuadTileSet)ro;
                qts.ResetCacheForCurrentView(this.GlobeControl.DrawArgs.WorldCamera);
            }
            else if (ro is RenderableObjectList)
            {
                RenderableObjectList rol = (RenderableObjectList)ro;
                foreach (RenderableObject curRo in rol.ChildObjects)
                {
                    resetQuadTileSetCache(curRo);
                }
            }
        }

        /// <summary>
        /// 寻找列表图层
        /// </summary>
        /// <param name="rootRenderableObject">根列表图层</param>
        /// <param name="layerName">要寻找的图层名称</param>
        /// <returns>返回找到的列表图层</returns>
        private RenderableObjectList findListLayer(RenderableObjectList rootRenderableObject, string layerName)
        {
            for (int i = 0; i < rootRenderableObject.ChildObjects.Count; i++)
            {
                if (rootRenderableObject.ChildObjects[i] is RenderableObjectList)
                {
                    if (((RenderableObjectList)rootRenderableObject.ChildObjects[i]).Name == layerName)
                        return (RenderableObjectList)rootRenderableObject.ChildObjects[i];
                }
            }

            return null;
        }
        
        #endregion 私有方法
    }
}
