using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

using Qrst;
using Qrst.Camera;
using Qrst.Menu;
using Qrst.Net;
using Qrst.Net.Wms;
using Qrst.Terrain;
using Qrst.Renderable;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Text;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems.Projections;

namespace Qrst
{
    public class Globe
    {
        #region 私有属性
        //球控件对象
        private Qrst.QrstAxGlobeControl _QrstGlobe;
        //配置目录
        private string _DataDirectory = "";
        //缓存路径
        private string _CacheDirectory = "";
        //影像缓存
        private Hashtable imageCache = new Hashtable();

        #endregion

        #region 公共属性

        public Qrst.QrstAxGlobeControl QrstGlobe
        {
            get
            {
                return _QrstGlobe;
            }
            set
            {
                _QrstGlobe = value;
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
                this.QrstGlobe.Invalidate();
            }
        }
        /// <summary>
        /// 获取或设置配置根目录
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
                this.QrstGlobe.DataDirectory = this._DataDirectory;//配置根目录路径
            }
        }

        /// <summary>
        /// 设置缓存的存放根目录
        /// </summary>
        public string CacheDirectory
        {
            get
            {
                return this._CacheDirectory;
            }
            set
            {
                this._CacheDirectory = value;
                this.QrstGlobe.Cache.CacheDirectory = _CacheDirectory;
                this.QrstGlobe.CacheDirectory = _CacheDirectory;
            }
        }

        /// <summary>
        /// 显示格网
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
        /// 显示空间信息
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
        /// 显示图层管理器
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
        /// 显示太阳阴影效果
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

        #endregion

        /// <summary>
        /// 创建地球对象
        /// </summary>
        /// <param name="QrstGlobe"></param>
        public Globe(Qrst.QrstAxGlobeControl QrstGlobe)
        {
            this.QrstGlobe = QrstGlobe;
            this._DataDirectory = this.QrstGlobe.DataDirectory;
            this._CacheDirectory = this.QrstGlobe.CacheDirectory;
        }

        #region 添加图层
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
                Qrst.QrstAxGlobeControl window = this.QrstGlobe;
                //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
                Qrst.Renderable.ImageLayer image = GetImage(name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, backColor, opacity, imagePath);
                image.ParentList = window.CurrentWorld.RenderableObjects;
                //添加当前图层
                window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, image);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool AddToImageSet(string setName,string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, int backColor, byte opacity, string imagePath)
        {
            try
            {

                Qrst.QrstAxGlobeControl window = this.QrstGlobe;
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
                    imageSet.ShowOnlyOneLayer = false;
                    //存储QrstWindow的主要对象
                    imageSet.MetaData.Add("World", window.CurrentWorld);//着重看
                    imageSet.MetaData.Add("Cache", window.Cache);//着重看

                    imageSet.MetaData["north"] = maxLat;
                    imageSet.MetaData["south"] = minLat;
                    imageSet.MetaData["west"] = minLon;
                    imageSet.MetaData["east"] = maxLon;

                    //此图层做为表面图层
                    imageSet.RenderPriority = Qrst.Renderable.RenderPriority.SurfaceImages;
                    window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, imageSet);
                }
                //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
                Qrst.Renderable.ImageLayer image = GetImage(name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, backColor, opacity, imagePath);
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
        public Qrst.Renderable.ImageLayer GetImage(string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, int backColor, byte opacity, string imagePath)
        {
            Qrst.QrstAxGlobeControl window = this.QrstGlobe;

            //基本的图片显示，支持jpg,png。根据4个脚点坐标，就可以添加
            Qrst.Renderable.ImageLayer image = new Qrst.Renderable.ImageLayer
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
                Qrst.QrstAxGlobeControl window = this.QrstGlobe;
                //切片图层
                RenderableObjectList tiledImage = GetTiledImage(window, name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, opacity, visible, levelZeroTileSizeDegrees, numberLevels, imageFileExtension, imagePath);
                tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
                window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, tiledImage);
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
            Qrst.QrstAxGlobeControl window, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, byte opacity, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string imageFileExtension, string imagePath)
        {

            //切片图层
            RenderableObjectList tiledImage = new RenderableObjectList(name);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            //是否显示此图层
            tiledImage.IsOn = visible;
            tiledImage.DisableExpansion = false;
            tiledImage.ShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象
            tiledImage.MetaData.Add("World", window.CurrentWorld);//着重看
            tiledImage.MetaData.Add("Cache", window.Cache);//着重看

            tiledImage.MetaData["north"] = maxLat;
            tiledImage.MetaData["south"] = minLat;
            tiledImage.MetaData["west"] = minLon;
            tiledImage.MetaData["east"] = maxLon;

            //此图层做为表面图层
            tiledImage.RenderPriority = Qrst.Renderable.RenderPriority.SurfaceImages;

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
            ia.ServerLogo=Path.Combine(this._QrstGlobe.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            imageStores[0].ServerLogo = Path.Combine(this._QrstGlobe.DataDirectory, @"Icons\dcsrsdcsf.png");
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                this.QrstGlobe.CurrentWorld,
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
            //////透明色
            System.Drawing.Color c = Color.FromArgb(opacity, 0, 0, 0);//Color [A=255, R=0, G=0, B=0]
            qts.ColorKey = c.ToArgb();
            //System.Drawing.Color c = Color.Empty;
            //qts.ColorKey = c.ToArgb();
            qts.ParentList = tiledImage;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)tiledImage.MetaData["XmlSource"]);
            tiledImage.Add(qts);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            return tiledImage;
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
                Qrst.QrstAxGlobeControl window = this.QrstGlobe;
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
            Qrst.QrstAxGlobeControl window = this.QrstGlobe;
            //是否显示数字高程
            bool terrainMapped = false;
            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;

            Qrst.Net.Wms.WmsImageStore wmsLayerStore = new Qrst.Net.Wms.WmsImageStore();
            wmsLayerStore.ImageFormat = "image/png";
            wmsLayerStore.ImageExtension = imageFileExtension; //png
            wmsLayerStore.CacheDirectory = cacheDir;//;
            wmsLayerStore.ServerGetMapUrl = serverGetMapUrl;
            wmsLayerStore.Version = wmsVersion;
            wmsLayerStore.WMSLayerName = layerName + "&transparent=true";

            string serverLogoPath = Path.Combine(this._QrstGlobe.DataDirectory, @"Icons\dcsrsdcsf.png");

            wmsLayerStore.LevelCount = numberLevels;
            wmsLayerStore.LevelZeroTileSizeDegrees = levelZeroTileSizeDegrees;
            wmsLayerStore.ServerLogo = serverLogoPath;
            imageStores[0] = wmsLayerStore;
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                this.QrstGlobe.CurrentWorld,
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
            System.Drawing.Color c = Color.FromArgb(255, 255, 255, 255);//Color [A=255, R=0, G=0, B=0]
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
                Qrst.QrstAxGlobeControl window = this.QrstGlobe;
                //切片图层
                RenderableObjectList tiledImage = GetGeotiffLayer(window, name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, backgroundData, opacity, visible, levelZeroTileSizeDegrees, numberLevels, tiffFiles, cachePath);
                tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
                window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, tiledImage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public RenderableObjectList GetGeotiffLayer(Qrst.QrstAxGlobeControl window, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, byte backgroundData, byte opacity, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string[] tiffFiles, string cachePath)
        {
            //切片图层
            RenderableObjectList tiledImage = new RenderableObjectList(name);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            //是否显示此图层
            tiledImage.IsOn = visible;
            tiledImage.DisableExpansion = false;
            tiledImage.ShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象
            tiledImage.MetaData.Add("World", window.CurrentWorld);//着重看
            tiledImage.MetaData.Add("Cache", window.Cache);//着重看
            tiledImage.MetaData["north"] = maxLat;
            tiledImage.MetaData["south"] = minLat;
            tiledImage.MetaData["west"] = minLon;
            tiledImage.MetaData["east"] = maxLon;

            //此图层做为表面图层
            tiledImage.RenderPriority = Qrst.Renderable.RenderPriority.SurfaceImages;

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
            ia.ServerLogo = Path.Combine(this._QrstGlobe.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                this.QrstGlobe.CurrentWorld,
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
            System.Drawing.Color c = Color.FromArgb(opacity, backgroundData, backgroundData, backgroundData);//Color [A=255, R=0, G=0, B=0]
            qts.ColorKey = c.ToArgb();

            qts.ParentList = tiledImage;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)tiledImage.MetaData["XmlSource"]);
            tiledImage.Add(qts);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            return tiledImage;
        }

        /// <summary>
        /// 添加ShapeFile图层
        /// </summary>
        /// <param name="name"></param>
        /// <param name="distanceAboveSurface"></param>
        /// <param name="minLat"></param>
        /// <param name="maxLat"></param>
        /// <param name="minLon"></param>
        /// <param name="maxLon"></param>
        /// <param name="visible"></param>
        /// <param name="levelZeroTileSizeDegrees"></param>
        /// <param name="numberLevels"></param>
        /// <param name="shapeFiles"></param>
        /// <param name="cachePath"></param>
        /// <returns></returns>
        public bool AddShapeLayer(string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, Color fillColor, Pen outlineColor, Pen lineColor, string[] shapeFiles, string cachePath)
        {
            try
            {
                Qrst.QrstAxGlobeControl window = this.QrstGlobe;
                //切片图层
                RenderableObjectList tiledImage = GetShapeLayer(window, name, distanceAboveSurface, minLat, maxLat, minLon, maxLon, visible, levelZeroTileSizeDegrees, numberLevels, fillColor, outlineColor, lineColor, shapeFiles, cachePath);
                tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
                window.CurrentWorld.RenderableObjects.ChildObjects.Insert(window.CurrentWorld.RenderableObjects.ChildObjects.Count - 1, tiledImage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public RenderableObjectList GetShapeLayer(Qrst.QrstAxGlobeControl window, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, Color fillColor, Pen outlineColor, Pen lineColor, string[] shapeFiles, string cachePath)
        {
            //切片图层
            RenderableObjectList tiledImage = new RenderableObjectList(name);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            //是否显示此图层
            tiledImage.IsOn = visible;
            tiledImage.DisableExpansion = false;
            tiledImage.ShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象
            tiledImage.MetaData.Add("World", window.CurrentWorld);//着重看
            tiledImage.MetaData.Add("Cache", window.Cache);//着重看
            tiledImage.MetaData["north"] = maxLat;
            tiledImage.MetaData["south"] = minLat;
            tiledImage.MetaData["west"] = minLon;
            tiledImage.MetaData["east"] = maxLon;
            //此图层做为表面图层
            tiledImage.RenderPriority = Qrst.Renderable.RenderPriority.Icons;

            //是否显示数字高程
            bool terrainMapped = true;
            TimeSpan dataExpiration = TimeSpan.MaxValue;
            ImageStore[] imageStores = new ImageStore[1];
            TimeSpan dataExpirationTiles = TimeSpan.MaxValue;

            //切片图层对象
            ShapeLayerStore ia = new ShapeLayerStore();
            ia.LevelZeroTileSizeDegrees = levelZeroTileSizeDegrees;
            ia.LevelCount = numberLevels;
            ia.ImageExtension = "jpg";
            ia.CacheDirectory = cachePath;
            ia.Layers = shapeFiles;
            ia.formate = "image/png";
            ia.outlineColor = outlineColor;
            ia.fillColor = fillColor;
            ia.lineColor = lineColor;
            ia.ServerLogo = Path.Combine(this._QrstGlobe.DataDirectory, @"Icons\dcsrsdcsf.png");
            //下载时的图标
            imageStores[0] = ia;
            imageStores[0].ServerLogo = Path.Combine(this._QrstGlobe.DataDirectory, @"Icons\dcsrsdcsf.png");
            //切片对象
            QuadTileSet qts = null;
            qts = new QuadTileSet
            (
                name,
                this.QrstGlobe.CurrentWorld,
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
            qts.ParentList = tiledImage;
            qts.IsOn = true;
            qts.MetaData.Add("XmlSource", (string)tiledImage.MetaData["XmlSource"]);
            tiledImage.Add(qts);
            tiledImage.ParentList = window.CurrentWorld.RenderableObjects;
            return tiledImage;
        }

        public bool AddURLLayer(string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
          double levelZeroTileSizeDegrees, int numberLevels, string cachePath, string serverUrl, string dataSetName)
        {
            try
            {
                Qrst.QrstAxGlobeControl window = this.QrstGlobe;
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
        public RenderableObjectList GetURLLayer(Qrst.QrstAxGlobeControl window, string name, double distanceAboveSurface, double minLat, double maxLat, double minLon, double maxLon, bool visible,
            double levelZeroTileSizeDegrees, int numberLevels, string cachePath, string serverUrl, string dataSetName)
        {

            //Blue Marble 切片图层
            RenderableObjectList parentRenderable = new RenderableObjectList(name);
            parentRenderable.ParentList = window.CurrentWorld.RenderableObjects;
            //是否显示此图层
            parentRenderable.IsOn = true;
            parentRenderable.DisableExpansion = false;
            parentRenderable.ShowOnlyOneLayer = false;
            //存储QrstWindow的主要对象	
            parentRenderable.MetaData.Add("World", window.CurrentWorld);//着重看
            //此图层做为表面图层
            parentRenderable.RenderPriority = Qrst.Renderable.RenderPriority.SurfaceImages;
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
            string cacheDir = Path.Combine(this._QrstGlobe.CacheDirectory, layerName);//缓存路径
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
        /// 添加省的图标
        /// </summary>
        /// <param name="icons"></param>
        /// <returns></returns>
        private bool AddPlaces(object[] icons, string Name, double maxVisible)
        {

            try
            {
                Qrst.Renderable.Icons places = new Qrst.Renderable.Icons(Name);
                Qrst.Renderable.Icon ic = null;
                foreach (object icon in icons)
                {
                    if (icon is Qrst.Renderable.Icon)
                    {
                        ic = icon as Qrst.Renderable.Icon;
                        ic.ParentList = places;
                        ic.MaximumDisplayDistance = maxVisible;
                        places.Add(ic);
                        //ic.OnClick += new RenderableObject.Click(places_OnClick);
                    }
                }
                places.isSelectable = true;
                places.IsOn = true;
                this.AddLayer(places);
                return true;

            }
            catch
            {
                return false;
            }
        }

        void places_OnClick(RenderableObject sender, DrawArgs drawArgs)
        {
            //Renderable.Icon ic = sender as Renderable.Icon;
            //DevComponents.DotNetBar.Command command = new DevComponents.DotNetBar.Command();
            //command.Name = "placeof-"+ic.ToString();
            //command.Text = "<font size=\"+1\">更多关于※" + ic.ToString() + "※....</font>";
            //command.Executed += new System.EventHandler(MoreInformation_Executed);
            //DevComponents.DotNetBar.TaskDialogInfo info = new DevComponents.DotNetBar.TaskDialogInfo(
            //    "中国地标", 
            //    DevComponents.DotNetBar.eTaskDialogIcon.BlueFlag, 
            //    ic.ToString(), 
            //    ic.Description+"\t\n\t\n", 
            //    DevComponents.DotNetBar.eTaskDialogButton.Ok, 
            //    DevComponents.DotNetBar.eTaskDialogBackgroundColor.Blue
            //    , null, new DevComponents.DotNetBar.Command[] { command },null,"",null);
            //DevComponents.DotNetBar.eTaskDialogResult result = DevComponents.DotNetBar.TaskDialog.Show(info);
        }
        private void MoreInformation_Executed(object sender, EventArgs e)
        {
            //DevComponents.DotNetBar.PopupItem p = sender as DevComponents.DotNetBar.PopupItem;
            //string placeName = p.Text;
            //placeName = placeName.Split("※".ToCharArray())[1];
            //System.Diagnostics.Process.Start("http://www.baidu.com/s?wd=" + placeName);
            //DevComponents.DotNetBar.TaskDialog.Close(DevComponents.DotNetBar.eTaskDialogResult.Custom1);
        }
        private Qrst.Renderable.Icon[] GetPlaces(string fileName, int iconWidth, int iconHeight, string iconPath)
        {
            Qrst.Renderable.Icon[] icons = null;
            FileInfo fileInfo = new FileInfo(fileName);
            //打开当前qrstp文件
            using (BinaryReader reader = new BinaryReader(new BufferedStream(fileInfo.OpenRead()), Encoding.GetEncoding("GB2312")))
            {
                //读出来总共有多少挑数据
                int placenameCount = reader.ReadInt32();
                icons = new Qrst.Renderable.Icon[placenameCount];
                //循环查询地域名
                for (int i = 0; i < placenameCount; i++)
                {
                    //获得当前数据信息
                    Qrst.QrstPlacename pn = new Qrst.QrstPlacename();
                    ReadPlaceName(reader, ref pn);
                    icons[i] = new Qrst.Renderable.Icon(pn.Name, pn.Lat, pn.Lon, 0);
                    icons[i].isSelectable = true;
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
        /// 添加地标图层
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="iconWidth"></param>
        /// <param name="iconHeight"></param>
        /// <param name="iconPath"></param>
        /// <param name="layerName"></param>
        /// <param name="maxVisible"></param>
        /// <returns></returns>
        public bool AddPlacesLayer(string layerName, string fileName, int iconWidth, int iconHeight, string iconPath, double maxVisible)
        {
            return AddPlaces(GetPlaces(fileName, iconWidth, iconHeight, iconPath), layerName, maxVisible);
        }

        //读取一条数据的信息
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

        public void InValided()
        {
            this.QrstGlobe.Invalidate();
            this.QrstGlobe.Refresh();
            this.QrstGlobe.Update();
        }

        /// <summary>
        /// 使摄像机到某个特定位置
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="metersElevation"></param>
        public void Goto(double latitude, double longitude, double metersElevation)
        {
            this.QrstGlobe.SetViewPosition(latitude, longitude, metersElevation);
        }

        #endregion

        private void resetQuadTileSetCache(RenderableObject ro)
        {
            if (ro.IsOn && ro is QuadTileSet)
            {
                QuadTileSet qts = (QuadTileSet)ro;
                qts.ResetCacheForCurrentView(this.QrstGlobe.DrawArgs.WorldCamera);
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

        public Envelop GetEnvelop(string fileName, bool isShapeFile)
        {
            ICoordinateSystem srcCoor = null;
            SharpMap.Layers.Layer layer = null;
            if (isShapeFile)
            {

                SharpMap.Layers.VectorLayer l = new SharpMap.Layers.VectorLayer(Path.GetFileNameWithoutExtension(fileName));
                l.DataSource = new SharpMap.Data.Providers.ShapeFile(fileName, true);
                l.SRID = 4326;
                srcCoor = ((SharpMap.Data.Providers.ShapeFile)l.DataSource).CoordinateSystem;
                layer = l;
            }
            else
            {
                SharpMap.Layers.GdalRasterLayer l = new SharpMap.Layers.GdalRasterLayer(Path.GetFileNameWithoutExtension(fileName), fileName);
                l.SRID = 4326;
                l.Enabled = true;
                srcCoor = l.GetProjection();
                layer = l;
            }
            Envelop envelop = new Envelop();
            if (srcCoor != null)
            {
                CoordinateSystemFactory cFac = new CoordinateSystemFactory();
                ICoordinateSystem tarCoor = cFac.CreateFromWkt(CoordinatesDescription.GCS);
                ICoordinateTransformation transform = null;
                try
                {
                    transform = new CoordinateTransformationFactory().CreateFromCoordinateSystems(srcCoor, tarCoor);
                }
                catch (Exception e)
                {
                    MessageBox.Show("错误信息", e.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new Envelop();
                }

                SharpMap.Geometries.BoundingBox imageBox = GeometryTransform.TransformBox(layer.Envelope, transform.MathTransform);

                envelop.North = imageBox.Top;
                envelop.South = imageBox.Bottom;
                envelop.West = imageBox.Left;
                envelop.East = imageBox.Right;
            }
            return envelop;

        }

        #region 对外提供借口和方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private RenderableObjectList FindLayer(Qrst.QrstAxGlobeControl window, string layerName)
        {
            RenderableObjectList rootRenderableObject = window.CurrentWorld.RenderableObjects;
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
            if (ro is Qrst.Renderable.RenderableObjectList)
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
            Bitmap img = (Bitmap)imageCache[key];
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
                    imageCache.Add(key, img);
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

        public void DisposeImageCache()
        {
            if (imageCache.Count > 0)
            {
                foreach (Bitmap img in imageCache.Values) img.Dispose();
                imageCache.Clear();
            }
        }


        public double ParseDouble(string s)
        {
            return double.Parse(s, CultureInfo.InvariantCulture);
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
                this.QrstGlobe.CurrentWorld.RenderableObjects.ChildObjects.Insert(index, layer);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddLayer(object layer)
        {
            try
            {
                this.QrstGlobe.CurrentWorld.RenderableObjects.ChildObjects.Add(layer);
                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion
    }
}
