using System;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Geometries;
using QRST.WorldGlobeTool.Terrain;
using QRST.WorldGlobeTool.Renderable;
using QRST.WorldGlobeTool.Utility;

namespace QRST.WorldGlobeTool
{
    /// <summary>
    /// 地球对象  绘制类
    /// </summary>
    public class World : RenderableObject
    {

        #region 私有变量

        /// <summary>
        /// 赤道半径
        /// </summary>
        private double m_EquatorialRadius;
        /// <summary>
        /// 赤道半径长度 千米
        /// </summary>
        private const double FLATTENING = 6378.135;
        /// <summary>
        /// 半长轴 长度 米
        /// </summary>
        private const double SEMIMAJORAXIS = 6378137.0;
        /// <summary>
        /// 半短轴 长度 米
        /// </summary>
        private const double SEMIMINORAXIS = 6356752.31425;
        /// <summary>
        /// 数字高程对象 TerrainAccessor
        /// </summary>
        private TerrainAccessor m_TerrainAccessor;
        /// <summary>
        /// 要进行绘制的所有对象集合 
        /// </summary>
        private RenderableObjectList m_RenderableObjects;
        /// <summary>
        /// 在屏幕上显示的文字集合 IList
        /// </summary>
        private System.Collections.IList m_OnScreenMessages;
        /// <summary>
        /// 上次高程的更新时间
        /// </summary>
        private DateTime m_LastElevationUpdate = System.DateTime.Now;
        /// <summary>
        /// 太阳宽度
        /// </summary>
        private int m_sunWidth = 72;
        /// <summary>
        /// 太阳高度
        /// </summary>
        private int m_sunHeight = 72;
        /// <summary>
        /// 
        /// </summary>
        private Sprite m_sprite = null;
        /// <summary>
        /// 太阳文理
        /// </summary>
        private Texture m_sunTexture = null;
        /// <summary>
        /// 太阳表面描述
        /// </summary>
        private SurfaceDescription m_sunSurfaceDescription;

        #endregion

        #region  公共变量

        /// <summary>
        /// 地球属性配置对象
        /// </summary>
        public static WorldSettings Settings = new WorldSettings();
        /// <summary>
        /// 地球大气散射对象
        /// </summary>
        public AtmosphericScatteringSphere OuterSphere = null;

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取是否绘制的是地球对象
        /// </summary>
        public bool IsEarth
        {
            get
            {
                return this.Name == "Earth";
            }
        }
        /// <summary>
        /// 获取或设置要在球上显示的消息信息 。
        /// </summary>
        public System.Collections.IList OnScreenMessages
        {
            get
            {
                return this.m_OnScreenMessages;
            }
            set
            {
                this.m_OnScreenMessages = value;
            }
        }
        /// <summary>
        /// 获取或设置高程对象
        /// </summary>
        public TerrainAccessor TerrainAccessor
        {
            get
            {
                return this.m_TerrainAccessor;
            }
            set
            {
                this.m_TerrainAccessor = value;
            }
        }
        /// <summary>
        /// 获取地球半径
        /// </summary>
        public double EquatorialRadius
        {
            get
            {
                return this.m_EquatorialRadius;
            }
        }
        /// <summary>
        /// 获取或设置地球图层
        /// </summary>
        public RenderableObjectList RenderableObjects
        {
            get
            {
                return this.m_RenderableObjects;
            }
            set
            {
                this.m_RenderableObjects = value;
            }
        }

        #endregion

        #region  构造函数

        /// <summary>
        /// 初始化一个地球类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="orientation"></param>
        /// <param name="equatorialRadius"></param>
        /// <param name="cacheDirectory"></param>
        /// <param name="terrainAccessor"></param>
        public World(string name, Vector3 position, Quaternion orientation, double equatorialRadius,
            string cacheDirectory,
            TerrainAccessor terrainAccessor)
            : base(name, position, orientation)
        {
            //6378137.0
            this.m_EquatorialRadius = equatorialRadius;
            //设置 数字高程 对象
            this.m_TerrainAccessor = terrainAccessor;
            //初始化world的 Renderable对象，名字是"Earth".
            this.m_RenderableObjects = new RenderableObjectList(this.Name);
            //在元数据集合里，添加 Cache的路径
            this.MetaData.Add("CacheDirectory", cacheDirectory);
            //大气光环实例
            OuterSphere = new AtmosphericScatteringSphere();
            AtmosphericScatteringSphere.m_fInnerRadius = (float)equatorialRadius;
            AtmosphericScatteringSphere.m_fOuterRadius = (float)equatorialRadius * 1.15f;
            OuterSphere.Init((float)equatorialRadius * 1.15f, 75, 75);
        }

        #endregion

        #region 绘制 地球的 相关 参数：太阳，星星，地球，坐标轴

        /// <summary>
        /// 初始化地球所有图层的相关参数
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            try
            {
                if (this.IsInitialized)
                    return;
                this.RenderableObjects.Initialize(drawArgs);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                this.IsInitialized = true;
            }
        }

        /// <summary>
        /// 3.绘制地球
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            //绘制星星
            if (World.Settings.showStars)
            {
                RenderStars(drawArgs, RenderableObjects);
            }
            //设置坐标转换
            if (drawArgs.CurrentWorld.IsEarth && World.Settings.EnableAtmosphericScattering)
            {
                float aspectRatio = (float)drawArgs.WorldCamera.Viewport.Width / drawArgs.WorldCamera.Viewport.Height;  //绘制区域的宽高比
                float zNear = (float)drawArgs.WorldCamera.Altitude * 0.1f;  //
                double distToCenterOfPlanet = (drawArgs.WorldCamera.Altitude + m_EquatorialRadius);  //照相机海拔到地心的距离
                double tangentalDistance = Math.Sqrt(distToCenterOfPlanet * distToCenterOfPlanet - m_EquatorialRadius * m_EquatorialRadius);
                double amosphereThickness = Math.Sqrt(OuterSphere.m_radius * OuterSphere.m_radius + m_EquatorialRadius * m_EquatorialRadius);
                Matrix proj = drawArgs.device.Transform.Projection;
                drawArgs.device.Transform.Projection = Matrix.PerspectiveFovRH((float)drawArgs.WorldCamera.Fov.Radians, aspectRatio, zNear, (float)(tangentalDistance + amosphereThickness));
                drawArgs.device.RenderState.ZBufferEnable = false;
                drawArgs.device.RenderState.CullMode = Cull.CounterClockwise;
                OuterSphere.Render(drawArgs);       //绘制大气圈
                drawArgs.device.RenderState.CullMode = Cull.Clockwise;
                drawArgs.device.RenderState.ZBufferEnable = true;
                drawArgs.device.Transform.Projection = proj;
            }

            //绘制太阳
            if (World.Settings.EnableSunShading)
                RenderSun(drawArgs);

            //绘制地球表面
            Render(RenderableObjects, RenderPriority.TerrainMappedImages, drawArgs);

            //绘制天空
            Render(RenderableObjects, RenderPriority.AtmosphericImages, drawArgs);

            //render Icons
            //绘制图标
            Render(RenderableObjects, RenderPriority.Icons, drawArgs);

            //ZYM:20130714-绘制控制点
            Render(RenderableObjects, RenderPriority.GCPs, drawArgs);

            //绘制 线条
            Render(RenderableObjects, RenderPriority.LinePaths, drawArgs);

            //render Custom
            //绘制自定义
            Render(RenderableObjects, RenderPriority.Custom, drawArgs);
        }
        
        /// <summary>
        /// 绘制地球对象上的图层
        /// </summary>
        /// <param name="renderable"></param>
        /// <param name="priority"></param>
        /// <param name="drawArgs"></param>
        private void Render(RenderableObject renderable, RenderPriority priority, DrawArgs drawArgs)
        {
            //若是绘制星星，则返回，因为星星图层已经绘制过了
            //if (!renderable.IsOn || (renderable.Name != null && renderable.Name.Equals("Starfield")))
            if (!renderable.IsOn || (renderable.Name != null && renderable.Name.Equals("星空")))
                return;

            //绘制Icon图层
            if (priority == RenderPriority.Icons && renderable is Icons)
            {
                renderable.Render(drawArgs);
            }
            else if (priority == RenderPriority.GCPs && renderable is GCPs)
            {
                renderable.Render(drawArgs);
            }
            //绘制RenderableObjectList类型图层下的所有子图层
            else if (renderable is RenderableObjectList)
            {
                RenderableObjectList rol = (RenderableObjectList)renderable;
                for (int i = 0; i < rol.ChildObjects.Count; i++)
                {
                    Render((RenderableObject)rol.ChildObjects[i], priority, drawArgs);
                }
            }
            //绘制RenderPriority.SurfaceImages类型的图层
            else if (priority == RenderPriority.TerrainMappedImages)
            {
                if (renderable.RenderPriority == RenderPriority.SurfaceImages || renderable.RenderPriority == RenderPriority.TerrainMappedImages)
                {
                    renderable.Render(drawArgs);
                }
            }
            //绘制RenderPriority.LinePaths和RenderPriority.AtmosphericImages类型的图层
            else if (renderable.RenderPriority == priority)
            {
                renderable.Render(drawArgs);
            }
        }

        /// <summary>
        /// 1.绘制太阳
        /// </summary>
        /// <param name="drawArgs"></param>
        private void RenderSun(DrawArgs drawArgs)
        {
            //根据时间计算太阳的位置
            Point3d sunPosition = -SunCalculator.GetGeocentricPosition(TimeKeeper.CurrentTimeUtc);
            Point3d sunSpherical = MathEngine.CartesianToSphericalD(sunPosition.X, sunPosition.Y, sunPosition.Z);
            sunPosition = MathEngine.SphericalToCartesianD(
                Angle.FromRadians(sunSpherical.Y),
                Angle.FromRadians(sunSpherical.Z),
                150000000000);

            Vector3 sunVector = new Vector3((float)sunPosition.X, (float)sunPosition.Y, (float)sunPosition.Z);

            Frustum viewFrustum = new Frustum();

            float aspectRatio = (float)drawArgs.WorldCamera.Viewport.Width / drawArgs.WorldCamera.Viewport.Height;
            Matrix projectionMatrix = Matrix.PerspectiveFovRH((float)drawArgs.WorldCamera.Fov.Radians, aspectRatio, 1.0f, 300000000000);

            viewFrustum.Update(
                Matrix.Multiply(drawArgs.WorldCamera.AbsoluteWorldMatrix,
                Matrix.Multiply(drawArgs.WorldCamera.AbsoluteViewMatrix,
                    projectionMatrix)));

            if (!viewFrustum.ContainsPoint(sunVector))
                return;

            Vector3 translationVector = new Vector3(
                (float)(sunPosition.X - drawArgs.WorldCamera.ReferenceCenter.X),
                (float)(sunPosition.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                (float)(sunPosition.Z - drawArgs.WorldCamera.ReferenceCenter.Z));

            Vector3 projectedPoint = drawArgs.WorldCamera.Project(translationVector);

            if (m_sunTexture == null)
            {
                m_sunTexture = ImageHelper.LoadTexture(Path.Combine(((QRSTWorldGlobeControl)drawArgs.parentControl).DataDirectory, @"Space\sun.dds"));
                m_sunSurfaceDescription = m_sunTexture.GetLevelDescription(0);
            }

            if (m_sprite == null)
            {
                m_sprite = new Sprite(drawArgs.device);
            }

            m_sprite.Begin(SpriteFlags.AlphaBlend);

            // Render icon
            float xscale = (float)m_sunWidth / m_sunSurfaceDescription.Width;
            float yscale = (float)m_sunHeight / m_sunSurfaceDescription.Height;
            m_sprite.Transform = Matrix.Scaling(xscale, yscale, 0);

            m_sprite.Transform *= Matrix.Translation(projectedPoint.X, projectedPoint.Y, 0);
            m_sprite.Draw(m_sunTexture,
                new Vector3(m_sunSurfaceDescription.Width >> 1, m_sunSurfaceDescription.Height >> 1, 0),
                Vector3.Empty,
                System.Drawing.Color.FromArgb(253, 253, 200).ToArgb());

            // Reset transform to prepare for text rendering later
            m_sprite.Transform = Matrix.Identity;
            m_sprite.End();
        }

        /// <summary>
        /// 2.绘制星星
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <param name="renderable"></param>
        private void RenderStars(DrawArgs drawArgs, RenderableObject renderable)
        {
            if (renderable is RenderableObjectList)
            {
                RenderableObjectList rol = (RenderableObjectList)renderable;
                for (int i = 0; i < rol.ChildObjects.Count; i++)
                {
                    RenderStars(drawArgs, (RenderableObject)rol.ChildObjects[i]);
                }
            }
            //else if (renderable.Name != null && renderable.Name.Equals("Starfield"))
            else if (renderable.Name != null && renderable.Name.Equals("星空"))
            {
                try
                {
                    renderable.Render(drawArgs);
                }
                catch(Exception ex)
                {
                    throw new Exception("绘制星星失败！\n" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 更新地球相关参数
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Update(DrawArgs drawArgs)
        {
            if (!this.IsInitialized)
            {
                this.Initialize(drawArgs);
            }

            if (this.RenderableObjects != null)
            {
                this.RenderableObjects.Update(drawArgs);
            }

            if (this.TerrainAccessor != null)
            {
                if (drawArgs.WorldCamera.Altitude < 300000)
                {
                    if (System.DateTime.Now - this.m_LastElevationUpdate > TimeSpan.FromMilliseconds(500))
                    {
                        drawArgs.WorldCamera.TerrainElevation = (short)this.TerrainAccessor.GetElevationAt(drawArgs.WorldCamera.Latitude.Degrees, drawArgs.WorldCamera.Longitude.Degrees, 100.0 / drawArgs.WorldCamera.ViewRange.Degrees);
                        this.m_LastElevationUpdate = System.DateTime.Now;
                    }
                }
                else
                    drawArgs.WorldCamera.TerrainElevation = 0;
            }
            else
            {
                drawArgs.WorldCamera.TerrainElevation = 0;
            }

            if (World.Settings.EnableAtmosphericScattering && OuterSphere != null)
                OuterSphere.Update(drawArgs);
        }

        #endregion

        #region 计算球上两点之间的距离

        /// <summary>
        /// Computes the great circle distance between two pairs of lat/longs.
        /// 计算两个经纬度之间最大圆的距离
        /// </summary>
        public static Angle ApproxAngularDistance(Angle latA, Angle lonA, Angle latB, Angle lonB)
        {
            Angle dlon = lonB - lonA;
            Angle dlat = latB - latA;
            double k = Math.Sin(dlat.Radians * 0.5);
            double l = Math.Sin(dlon.Radians * 0.5);
            double a = k * k + Math.Cos(latA.Radians) * Math.Cos(latB.Radians) * l * l;
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            return Angle.FromRadians(c);
        }

        /// <summary>
        /// 计算球上两点之间的距离
        /// </summary>
        public double ApproxDistance(Angle latA, Angle lonA, Angle latB, Angle lonB)
        {
            double distance = m_EquatorialRadius * ApproxAngularDistance(latA, lonA, latB, lonB).Radians;
            return distance;
        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {

            if (this.RenderableObjects != null)
            {
                this.RenderableObjects.Dispose();
                this.RenderableObjects = null;
            }

            if (OuterSphere != null)
            {
                OuterSphere.Dispose();
            }
        }

        /// <summary>
        /// 设置图层的透明度
        /// </summary>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="opacity"></param>
        public void SetLayerOpacity(string category, string name, float opacity)
        {
            this.setLayerOpacity(this.m_RenderableObjects, category, name, opacity);
        }

        /// <summary>
        /// 鼠标点选处理事件
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <returns></returns>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            return this.m_RenderableObjects.PerformSelectionAction(drawArgs);
        }

        /// <summary>
        /// Intermediate points on a great circle
        /// In previous sections we have found intermediate points on a great circle given either
        /// the crossing latitude or longitude. Here we find points (lat,lon) a given fraction of the
        /// distance (d) between them. Suppose the starting point is (lat1,lon1) and the final point
        /// (lat2,lon2) and we want the point a fraction f along the great circle route. f=0 is
        /// point 1. f=1 is point 2. The two points cannot be antipodal ( i.e. lat1+lat2=0 and
        /// abs(lon1-lon2)=pi) because then the route is undefined.
        /// </summary>
        /// <param name="f">Fraction of the distance for intermediate point (0..1)</param>
        public static void IntermediateGCPoint(float f, Angle lat1, Angle lon1, Angle lat2, Angle lon2, Angle d,
            out Angle lat, out Angle lon)
        {
            double sind = Math.Sin(d.Radians);
            double cosLat1 = Math.Cos(lat1.Radians);
            double cosLat2 = Math.Cos(lat2.Radians);
            double A = Math.Sin((1 - f) * d.Radians) / sind;
            double B = Math.Sin(f * d.Radians) / sind;
            double x = A * cosLat1 * Math.Cos(lon1.Radians) + B * cosLat2 * Math.Cos(lon2.Radians);
            double y = A * cosLat1 * Math.Sin(lon1.Radians) + B * cosLat2 * Math.Sin(lon2.Radians);
            double z = A * Math.Sin(lat1.Radians) + B * Math.Sin(lat2.Radians);
            lat = Angle.FromRadians(Math.Atan2(z, Math.Sqrt(x * x + y * y)));
            lon = Angle.FromRadians(Math.Atan2(y, x));
        }

        /// <summary>
        /// Intermediate points on a great circle
        /// In previous sections we have found intermediate points on a great circle given either
        /// the crossing latitude or longitude. Here we find points (lat,lon) a given fraction of the
        /// distance (d) between them. Suppose the starting point is (lat1,lon1) and the final point
        /// (lat2,lon2) and we want the point a fraction f along the great circle route. f=0 is
        /// point 1. f=1 is point 2. The two points cannot be antipodal ( i.e. lat1+lat2=0 and
        /// abs(lon1-lon2)=pi) because then the route is undefined.
        /// </summary>
        /// <param name="f">Fraction of the distance for intermediate point (0..1)</param>
        public Vector3 IntermediateGCPoint(float f, Angle lat1, Angle lon1, Angle lat2, Angle lon2, Angle d)
        {
            double sind = Math.Sin(d.Radians);
            double cosLat1 = Math.Cos(lat1.Radians);
            double cosLat2 = Math.Cos(lat2.Radians);
            double A = Math.Sin((1 - f) * d.Radians) / sind;
            double B = Math.Sin(f * d.Radians) / sind;
            double x = A * cosLat1 * Math.Cos(lon1.Radians) + B * cosLat2 * Math.Cos(lon2.Radians);
            double y = A * cosLat1 * Math.Sin(lon1.Radians) + B * cosLat2 * Math.Sin(lon2.Radians);
            double z = A * Math.Sin(lat1.Radians) + B * Math.Sin(lat2.Radians);
            Angle lat = Angle.FromRadians(Math.Atan2(z, Math.Sqrt(x * x + y * y)));
            Angle lon = Angle.FromRadians(Math.Atan2(y, x));

            Vector3 v = MathEngine.SphericalToCartesian(lat, lon, m_EquatorialRadius);
            return v;
        }

        #endregion

        
        #region  私有方法

        /// <summary>
        /// 获取图层所在的路径
        /// </summary>
        /// <param name="renderable"></param>
        /// <returns></returns>
        private static string getRenderablePathString(RenderableObject renderable)
        {
            if (renderable.ParentList == null)
            {
                return renderable.Name;
            }
            else
            {
                return getRenderablePathString(renderable.ParentList) + Path.DirectorySeparatorChar + renderable.Name;
            }
        }

        /// <summary>
        /// 设置图层的透明度
        /// </summary>
        /// <param name="ro"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="opacity"></param>
        private void setLayerOpacity(RenderableObject ro, string category, string name, float opacity)
        {
            foreach (string key in ro.MetaData.Keys)
            {
                if (String.Compare(key, category, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                {
                    if (ro.MetaData[key].GetType() == typeof(String))
                    {
                        string curValue = ro.MetaData[key] as string;
                        if (String.Compare(curValue, name, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                        {
                            ro.Opacity = (byte)(255 * opacity);
                        }
                    }
                    break;
                }
            }

            RenderableObjectList rol = ro as RenderableObjectList;
            if (rol != null)
            {
                foreach (RenderableObject childRo in rol.ChildObjects)
                    setLayerOpacity(childRo, category, name, opacity);
            }
        }

        #endregion

    }
}
