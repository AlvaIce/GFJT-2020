using System;
using System.Collections.Generic;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Geometries;
using System.Windows.Forms;
using Microsoft.DirectX;
using QRST.WorldGlobeTool.Utility;
using QRST.WorldGlobeTool.Renderable;
using System.Drawing;
using QRST.WorldGlobeTool.PluginEngine;

namespace QRST.WorldGlobeTool.DrawUtility
{
    /// <summary>
    /// 绘制基类，继承自可渲染对象RenderableObject
    /// </summary>
    public abstract class DrawBaseLayer : RenderableObject
    {
        /// <summary>
        /// 绘制状态
        /// </summary>
        public enum DrawState
        {
            /// <summary>
            /// 空闲状态
            /// </summary>
            Idle,
            /// <summary>
            /// 绘制状态
            /// </summary>
            Drawing,
            /// <summary>
            /// 完成状态
            /// </summary>
            Complete
        }

        #region  字段

        /// <summary>
        /// 当前绘制状态
        /// </summary>
        public DrawState State;
        /// <summary>
        /// 多边形的内部填充颜色
        /// </summary>
        public Color fillColor;
        /// <summary>
        /// 绘制参数
        /// </summary>
        public DrawArgs drawArgs;
        /// <summary>
        /// 与当前绘制对象绑定的插件
        /// </summary>
        protected Plugin drawPlugin;
        /// <summary>
        /// 鼠标按下的点
        /// </summary>
        protected Point mouseDownPoint;
        /// <summary>
        /// 默认海拔高度
        /// </summary>
        public double defaultElev = 0;
        /// <summary>
        /// 是否考虑地形因素
        /// </summary>
        public bool useTerrain;
        /// <summary>
        /// 
        /// </summary>
        public double elevateSamplifyRate = 1024;
        /// <summary>
        /// 是否响应WorldWind的鼠标点击缩放浏览事件
        /// </summary>
        public bool isPointGotoEnabled;
        /// <summary>
        /// 点列表
        /// </summary>
        public List<Point3d> PointList;
        /// <summary>
        /// 顶点列表
        /// </summary>
        public List<CustomVertex.PositionColored> VertexList;
        /// <summary>
        /// 顶点颜色数组
        /// </summary>
        public CustomVertex.PositionColored[] pcs;
        /// <summary>
        /// 移动的点
        /// </summary>
        protected Point3d movingPt;

        #endregion

        #region  构造函数

        /// <summary>
        /// 初始化一个DrawBaseLayer实例
        /// </summary>
        /// <param name="pName">图层名称</param>
        /// <param name="pColor">线条颜色</param>
        /// <param name="pWorld">所属的世界</param>
        /// <param name="drawArgs">绘制参数</param>
        public DrawBaseLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pWorld)
        {
            this.fillColor = Color.FromArgb(20, pColor.R, pColor.G, pColor.B);
            this.drawArgs = drawArgs;
            this.State = DrawState.Idle;
            drawPlugin = null;
            useTerrain = false;
        }

        /// <summary>
        /// 初始化一个DrawBaseLayer实例
        /// </summary>
        /// <param name="pName">图层名称</param>
        /// <param name="pColor">线条颜色</param>
        /// <param name="drawTool">绘制工具</param>
        /// <param name="drawArgs">绘制参数</param>
        public DrawBaseLayer(string pName, Color pColor, Plugin drawTool, DrawArgs drawArgs)
            : this(pName, pColor, drawTool.ParentApplication.CurrentWorld, drawArgs)
        {
            this.drawPlugin = drawTool;
        }

        #endregion

        #region 抽象事件

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public abstract void MouseDown(object sender, MouseEventArgs e);

        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        public abstract void MouseUp(object sender, MouseEventArgs e);

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public abstract void MouseDoubleClick(object sender, MouseEventArgs e);

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        public abstract void MouseMove(object sender, MouseEventArgs e);

        /// <summary>
        /// 键盘弹起事件
        /// </summary>
        public abstract void KeyUp(object sender, KeyEventArgs e);

        #endregion

        #region  受保护方法

        /// <summary>
        /// 获取海拔高度
        /// </summary>
        /// <param name="lat">纬度坐标</param>
        /// <param name="lon">经度坐标</param>
        /// <returns>返回给定经纬度坐标处的海拔高度</returns>
        protected double getElevation(double lat, double lon)
        {
            double elevation = 0;
            elevation = World.TerrainAccessor.GetElevationAt(lat, lon, elevateSamplifyRate);
            return elevation;
        }

        /// <summary>
        /// 三维点转换为位置颜色
        /// </summary>
        /// <param name="point">三维点</param>
        /// <param name="color">颜色信息</param>
        /// <returns>返回位置颜色</returns>
        public CustomVertex.PositionColored Point3d2PositionColored(Point3d point, Color color)
        {
            CustomVertex.PositionColored pc = new CustomVertex.PositionColored();
            pc.Color = color.ToArgb();
            Vector3 vec = MathEngine.SphericalToCartesian(point.Y, point.X, World.EquatorialRadius + point.Z * World.Settings.VerticalExaggeration);
            pc.X = vec.X;
            pc.Y = vec.Y;
            pc.Z = vec.Z;

            return pc;
        }

        /// <summary>
        /// 从起始和结束点中获取曲线位置颜色
        /// </summary>
        /// <param name="startPt">起始点</param>
        /// <param name="endPt">结束点</param>
        /// <param name="color">颜色信息</param>
        /// <returns></returns>
        protected List<CustomVertex.PositionColored> getCurveFromPoints(Point3d startPt, Point3d endPt, Color color)
        {
            List<CustomVertex.PositionColored> rstCurve = new List<CustomVertex.PositionColored>();
            Angle angularDistance = World.ApproxAngularDistance(Angle.FromDegrees(startPt.Y), Angle.FromDegrees(startPt.X), Angle.FromDegrees(endPt.Y), Angle.FromDegrees(endPt.X));

            int samples = (int)(angularDistance.Radians * 30);  // 1 point for every 2 degrees.
            if (samples < 2)
                samples = 2;

            List<Point3d> ptss = new List<Point3d>();
            CustomVertex.PositionColored newPc;

            newPc = Point3d2PositionColored(startPt, color);
            rstCurve.Add(newPc);


            for (int i = 1; i < samples - 1; i++)
            {
                float t = (float)i / (samples - 1);

                Angle lat, lon = Angle.Zero;
                World.IntermediateGCPoint(t,
                    Angle.FromDegrees(startPt.Y),
                    Angle.FromDegrees(startPt.X),
                    Angle.FromDegrees(endPt.Y),
                    Angle.FromDegrees(endPt.X),
                    angularDistance,
                    out lat, out lon);

                Point3d newPt = new Point3d();
                newPt.X = lon.Degrees;
                newPt.Y = lat.Degrees;
                newPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;

                ptss.Add(newPt);

                newPc = Point3d2PositionColored(newPt, color);
                rstCurve.Add(newPc);
            }


            newPc = Point3d2PositionColored(endPt, color);
            rstCurve.Add(newPc);

            return rstCurve;
        }

        /// <summary>
        /// 直接得到纹理对象
        /// </summary>
        /// <param name="device">设备</param>
        /// <param name="textureFileName">纹理文件名称</param>
        protected Texture LoadImage(Device device, string textureFileName)
        {
            Image image = ImageHelper.LoadImage(textureFileName);
            int Width = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Width) / Math.Log(2)))));
            if (Width > device.DeviceCaps.MaxTextureWidth)
                Width = device.DeviceCaps.MaxTextureWidth;

            int Height = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Height) / Math.Log(2)))));
            if (Height > device.DeviceCaps.MaxTextureHeight)
                Height = device.DeviceCaps.MaxTextureHeight;

            using (Bitmap textureSource = new Bitmap(Width, Height))
            using (Graphics g = Graphics.FromImage(textureSource))
            {
                g.DrawImage(image, 0, 0, Width, Height);
                return new Texture(device, textureSource, Usage.None, Pool.Managed);
            }
        }

        /// <summary>
        /// 判断鼠标是否在给定的点上
        /// </summary>
        /// <param name="mouseY">鼠标所在位置Y方向纬度坐标</param>
        /// <param name="mouseX">鼠标所在位置X方向经度坐标</param>
        /// <param name="pointLat">点纬度坐标</param>
        /// <param name="pointLon">点经度坐标</param>
        /// <returns>鼠标在点上返回true，都则返回false</returns>
        protected bool IsMouseOnPoint(double mouseY, double mouseX, double pointLat, double pointLon)
        {
            return Math.Abs(mouseY - pointLat) < 0.05 && Math.Abs(mouseX - pointLon) < 0.05;
        }

        /// <summary>
        /// 鼠标是否拖动
        /// </summary>
        /// <returns>如果鼠标按下的位置和上次的位置大于3*3像素，说明是鼠标拖动情况，否则不是</returns>
        protected bool mouseDragged()
        {
            int dx = DrawArgs.LastMousePosition.X - mouseDownPoint.X;
            int dy = DrawArgs.LastMousePosition.Y - mouseDownPoint.Y;
            if (dx * dx + dy * dy > 3 * 3)
                return true;
            else
                return false;
        }

        #endregion

        #region 重载方法或属性

        /// <summary>
        /// 重载父类删除图层的方法
        /// 增加卸载插件一步
        /// </summary>
        public override void Delete()
        {
            World.Settings.CurrentWwTool = null;
            base.Delete();
            drawPlugin.Unload();
        }

        /// <summary>
        /// 获取或设置是否显示当前图层
        /// </summary>
        public override bool IsOn
        {
            get
            {
                return base.IsOn;
            }
            set
            {
                this.isOn = value;
                if (isOn)
                {
                    // Can't use point goto while measuring
                    isPointGotoEnabled = World.Settings.CameraIsPointGoto;
                    World.Settings.CameraIsPointGoto = false;
                }
                else
                {
                    World.Settings.CameraIsPointGoto = isPointGotoEnabled;
                }
            }
        }

        /// <summary>
        /// 初始化当前图层
        /// </summary>
        /// <param name="drawArgs">绘制信息</param>
        public override void Initialize(DrawArgs drawArgs)
        {
            VertexList = new List<CustomVertex.PositionColored>();
            PointList = new List<Point3d>();
            movingPt = new Point3d();
            movingPt.IsNaN = true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            PointList = null;
            VertexList = null;
            movingPt = null;
        }

        #endregion

    }
}
