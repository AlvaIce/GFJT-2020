using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Geometries;
using QRST.WorldGlobeTool.PluginEngine;
using QRST.WorldGlobeTool.Utility;

namespace QRST.WorldGlobeTool.DrawUtility
{
    /// <summary>
    /// 绘制搜索结果范围图层
    /// </summary>
    public class DrawExtentsLayer : DrawBaseLayer
    {

        #region 字段

        /// <summary>
        /// 最小海拔高度
        /// </summary>
        protected double MinAltitude = 0;
        /// <summary>
        /// 最大海拔高度
        /// </summary>
        protected double MaxAltitude = 5700000;
        /// <summary>
        /// 线条颜色
        /// </summary>
        private Color lineColor;
        /// <summary>
        /// 范围框图层线条颜色列表
        /// </summary>
        private List<CustomVertex.PositionColored[]> m_listpcs = new List<CustomVertex.PositionColored[]>();
        /// <summary>
        /// 当前的范围框列表
        /// </summary>
        private Dictionary<RectangleF, int> m_currentExtents;
        /// <summary>
        /// 选中的范围框的颜色条列表
        /// </summary>
        private List<CustomVertex.PositionColored[]> m_selectExtentsPCs;
        /// <summary>
        /// 数量文字字体
        /// </summary>
        private Microsoft.DirectX.Direct3D.Font m_CountTextFont;

        #endregion

        #region 构造函数

        public DrawExtentsLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pColor, pWorld, drawArgs)
        {
            lineColor = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }

        public DrawExtentsLayer(string pName, Color pColor, Plugin drawTool, DrawArgs drawArgs)
            : base(pName, pColor, drawTool, drawArgs)
        {
            lineColor = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 添加范围框
        /// </summary>
        /// <param name="rects">矩形范围框及其数量字典</param>

        /// <summary>
        /// 添加矩形框
        /// </summary>
        /// <param name="rects"></param> //返回最大频次数
        public int AddExtents(Dictionary<RectangleF, int> rects)
        {
            m_listpcs.Clear();
            int maxCount = 0;
            foreach (KeyValuePair<RectangleF, int> kvp in rects)
            {
                if (kvp.Value > maxCount)
                {
                    maxCount = kvp.Value;
                }
            }

            foreach (KeyValuePair<RectangleF, int> kvp in rects)
            {
                Point3d pt0 = createPoint3dByLatLon(kvp.Key.Y, kvp.Key.X);
                Point3d pt1 = createPoint3dByLatLon(kvp.Key.Y, kvp.Key.Right);
                Point3d pt2 = createPoint3dByLatLon(kvp.Key.Bottom, kvp.Key.Right);
                Point3d pt3 = createPoint3dByLatLon(kvp.Key.Bottom, kvp.Key.X);

                lineColor = ColorBlend.QRSTRainbow6.getColorbyCount(kvp.Value, maxCount);
                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, lineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, lineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, lineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, lineColor);
                List<CustomVertex.PositionColored> curve5 = getCurveFromPoints(pt0, pt2, lineColor);
                List<CustomVertex.PositionColored> curve6 = getCurveFromPoints(pt2, pt1, lineColor);
                List<CustomVertex.PositionColored> curve7 = getCurveFromPoints(pt1, pt3, lineColor);

                CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count + curve7.Count - 6];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);
                curve5.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3, curve5.Count - 1);
                curve6.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count - 4, curve6.Count - 1);
                curve7.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count - 5, curve7.Count - 1);

                m_listpcs.Add(ppcs);
            }
            return maxCount;

        }
        /// <summary>
        /// 更新范围框，清除原有的绘制内容
        /// </summary>
        /// <param name="rects">矩形框范围</param>
        public void AddExtents1(List<List<float>> rects)
        {
            m_listpcs.Clear();
            foreach (List<float>rect in rects)
            {
                Point3d pt0 = createPoint3dByLatLon(rect[1], rect[0]);
                Point3d pt1 = createPoint3dByLatLon(rect[3], rect[2]);
                Point3d pt2 = createPoint3dByLatLon(rect[5], rect[4]);
                Point3d pt3 = createPoint3dByLatLon(rect[7], rect[6]);


                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, lineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, lineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, lineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, lineColor);

                CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);

                m_listpcs.Add(ppcs);
            }
        }
        /// <summary>
        /// 更新范围框，清除原有的绘制内容
        /// </summary>
        /// <param name="rects">矩形框范围</param>
        public void AddExtents(List<RectangleF> rects)
        {
            m_listpcs.Clear();
            foreach (RectangleF rect in rects)
            {
                Point3d pt0 = createPoint3dByLatLon(rect.Y, rect.X);
                Point3d pt1 = createPoint3dByLatLon(rect.Y, rect.Right);
                Point3d pt2 = createPoint3dByLatLon(rect.Bottom, rect.Right);
                Point3d pt3 = createPoint3dByLatLon(rect.Bottom, rect.X);


                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, lineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, lineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, lineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, lineColor);

                CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);

                m_listpcs.Add(ppcs);
            }
        }

        /// <summary>
        /// 添加范围框
        /// </summary>
        /// <param name="extents">多边形顶点列表，每个多边形的顶点数组按照经度、维度的顺序依次排列</param>
        /// <param name="isClosedLoop">每个多边形顶点数组是否构成闭环（即起始点和结束点的经纬度坐标是否相等）</param>
        public void AddExtents(List<double[]> extents, bool isClosedLoop = true)
        {
            m_listpcs.Clear();
            List<Point3d> p3ds;
            List<CustomVertex.PositionColored> ppcs;
            foreach (double[] extent in extents)
            {
                p3ds = new List<Point3d>();
                ppcs = new List<CustomVertex.PositionColored>();
                for (int i = 0; i < extent.Length; i += 2)
                {
                    p3ds.Add(createPoint3dByLatLon(extent[i + 1], extent[i]));
                }
                if (!isClosedLoop)
                {
                    p3ds.Add(createPoint3dByLatLon(extent[1], extent[0]));
                }
                for (int i = 0; i < p3ds.Count - 1; i++)
                {
                    List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(p3ds[i], p3ds[i + 1], lineColor);
                    ppcs.AddRange(curve1.ToArray());
                }
                m_listpcs.Add(ppcs.ToArray());
            }
        }
        /// <summary>
        /// 选中输入的范围框
        /// </summary>
        /// <param name="rects">要选中的范围框列表</param>
        /// <param name="selectLineColor">选中范围框的颜色</param>
        public void SelectExtents1(List<List<float>> rects, Color selectLineColor)
        {
            m_selectExtentsPCs = new List<CustomVertex.PositionColored[]>();
            foreach (List<float> rf in rects)
            {
                Point3d pt0 = createPoint3dByLatLon(rf[1], rf[0]);
                Point3d pt1 = createPoint3dByLatLon(rf[3], rf[2]);
                Point3d pt2 = createPoint3dByLatLon(rf[5], rf[4]);
                Point3d pt3 = createPoint3dByLatLon(rf[7], rf[6]);

                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, selectLineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, selectLineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, selectLineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, selectLineColor);
                List<CustomVertex.PositionColored> curve5 = getCurveFromPoints(pt0, pt2, selectLineColor);
                List<CustomVertex.PositionColored> curve6 = getCurveFromPoints(pt2, pt1, selectLineColor);
                List<CustomVertex.PositionColored> curve7 = getCurveFromPoints(pt1, pt3, selectLineColor);

                CustomVertex.PositionColored[] ppcs =
                    new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count + curve7.Count - 6];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);
                curve5.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3, curve5.Count - 1);
                curve6.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count - 4, curve6.Count - 1);
                curve7.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count - 5, curve7.Count - 1);

                m_selectExtentsPCs.Add(ppcs);
            }
        }

        /// <summary>
        /// 选中输入的范围框
        /// </summary>
        /// <param name="rects">要选中的范围框列表</param>
        /// <param name="selectLineColor">选中范围框的颜色</param>
        public void SelectExtents(List<RectangleF> rects, Color selectLineColor)
        {
            m_selectExtentsPCs = new List<CustomVertex.PositionColored[]>();
            foreach (RectangleF rf in rects)
            {
                Point3d pt0 = createPoint3dByLatLon(rf.Y, rf.X);
                Point3d pt1 = createPoint3dByLatLon(rf.Y, rf.Right);
                Point3d pt2 = createPoint3dByLatLon(rf.Bottom, rf.Right);
                Point3d pt3 = createPoint3dByLatLon(rf.Bottom, rf.X);

                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, selectLineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, selectLineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, selectLineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, selectLineColor);
                List<CustomVertex.PositionColored> curve5 = getCurveFromPoints(pt0, pt2, selectLineColor);
                List<CustomVertex.PositionColored> curve6 = getCurveFromPoints(pt2, pt1, selectLineColor);
                List<CustomVertex.PositionColored> curve7 = getCurveFromPoints(pt1, pt3, selectLineColor);

                CustomVertex.PositionColored[] ppcs =
                    new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count + curve7.Count - 6];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);
                curve5.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3, curve5.Count - 1);
                curve6.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count - 4, curve6.Count - 1);
                curve7.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count - 5, curve7.Count - 1);

                m_selectExtentsPCs.Add(ppcs);
            }
        }


        /// <summary>
        /// 添加范围框
        /// </summary>
        /// <param name="extents">多边形顶点列表，每个多边形的顶点数组按照经度、维度的顺序依次排列</param>
        /// <param name="isClosedLoop">每个多边形顶点数组是否构成闭环（即起始点和结束点的经纬度坐标是否相等）</param>
        public void SelectExtents(List<double[]> extents, Color selectLineColor, bool isClosedLoop = true)
        {
            m_selectExtentsPCs = new List<CustomVertex.PositionColored[]>();
            List<Point3d> p3ds;
            List<CustomVertex.PositionColored> ppcs;
            foreach (double[] extent in extents)
            {
                p3ds = new List<Point3d>();
                ppcs = new List<CustomVertex.PositionColored>();
                for (int i = 0; i < extent.Length; i += 2)
                {
                    p3ds.Add(createPoint3dByLatLon(extent[i + 1], extent[i]));
                }
                if (!isClosedLoop)
                {
                    p3ds.Add(createPoint3dByLatLon(extent[1], extent[0]));
                }
                for (int i = 0; i < p3ds.Count - 1; i++)
                {
                    List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(p3ds[i], p3ds[i + 1], selectLineColor);
                    ppcs.AddRange(curve1.ToArray());
                }
                m_selectExtentsPCs.Add(ppcs.ToArray());
            }
        }


        /// <summary>
        /// 清除选中的范围框
        /// </summary>
        public void ClearSelectExtents()
        {
            if (m_selectExtentsPCs != null)
            {
                m_selectExtentsPCs.Clear();
                m_selectExtentsPCs = null;
            }
        }

        /// <summary>
        /// 清除范围框
        /// </summary>
        public void ClearExtents()
        {
            if (m_listpcs != null)
            {
                m_listpcs.Clear();
            }
        }
        //画多边形
        public void UpdateExtents(List<List<float>> rects)
        {
            m_listpcs.Clear();
            foreach (List<float> rect in rects)
            {
                Point3d pt0 = createPoint3dByLatLon(rect[1], rect[0]);
                Point3d pt1 = createPoint3dByLatLon(rect[3], rect[2]);
                Point3d pt2 = createPoint3dByLatLon(rect[5], rect[4]);
                Point3d pt3 = createPoint3dByLatLon(rect[7], rect[6]);


                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, lineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, lineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, lineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, lineColor);

                CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);

                m_listpcs.Add(ppcs);
            }
            this.Render(this.drawArgs);//20130926 DLF 解决切片查询不重绘问题
        }
        public void UpdateExtents(List<RectangleF> rects)
        {
            m_listpcs.Clear();
            foreach (RectangleF rect in rects)
            {
                Point3d pt0 = createPoint3dByLatLon(rect.Y, rect.X);
                Point3d pt1 = createPoint3dByLatLon(rect.Y, rect.Right);
                Point3d pt2 = createPoint3dByLatLon(rect.Bottom, rect.Right);
                Point3d pt3 = createPoint3dByLatLon(rect.Bottom, rect.X);


                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, lineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, lineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, lineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, lineColor);

                CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3];

                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);

                m_listpcs.Add(ppcs);
            }
            this.Render(this.drawArgs);//20130926 DLF 解决切片查询不重绘问题
        }
        /// <summary>
        /// 根据经纬度构建三维点
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <returns>返回与给定经纬度对应的三维点</returns>
        private Point3d createPoint3dByLatLon(double lat, double lon)
        {
            Point3d pt3d = new Point3d();
            pt3d.X = lon;
            pt3d.Y = lat;
            pt3d.Z = (useTerrain) ? getElevation(lat, lon) : defaultElev;
            return pt3d;
        }

        /// <summary>
        /// 根据经纬度和高程构建三维点
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <param name="altoff">高程</param>
        /// <returns>返回与给定经纬度和高程对应的三维点</returns>
        private Point3d CreatePoint3dByLatLon(double lat, double lon, double altoff)
        {
            Point3d pt3d = new Point3d();
            pt3d.X = lon;
            pt3d.Y = lat;
            pt3d.Z = ((useTerrain) ? getElevation(lat, lon) : defaultElev) + altoff;
            return pt3d;
        }

        #endregion

        #region base Override

        public override void Initialize(DrawArgs drawArgs)
        {
            base.Initialize(drawArgs);
            IsInitialized = true;
        }

        public override void Update(DrawArgs drawArgs)
        {
            if (!this.IsInitialized)
                this.Initialize(drawArgs);
        }

        public override void Dispose()
        {
            IsInitialized = false;
            base.Dispose();
        }

        public override bool IsOn
        {
            get
            {
                return base.IsOn;
            }
            set
            {
                base.IsOn = value;
                //控制栅格化Polygon的显示 ProjectedVectorRenderer中
                if (State == DrawState.Complete)
                {
                    if (isOn)
                    {
                        this.Render(this.drawArgs);
                    }
                }
            }
        }

        /// <summary>
        /// 处理选中事件
        /// </summary>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            foreach (KeyValuePair<RectangleF, int> kvp in m_currentExtents)
            {
                if (DrawArgs.CurrentMouseLongtitude.Degrees > kvp.Key.Left
                    && DrawArgs.CurrentMouseLongtitude.Degrees < kvp.Key.Right
                    && DrawArgs.CurrentMouseLatitude.Degrees > kvp.Key.Bottom
                    && DrawArgs.CurrentMouseLatitude.Degrees < kvp.Key.Top)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 图层渲染
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            // Turn off light
            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = false;

            // Check that textures are initialised
            if (!IsInitialized)
                Initialize(drawArgs);

            Device device = drawArgs.device;
            device.RenderState.ZBufferEnable = false;
            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.VertexFormat = CustomVertex.PositionColored.Format;


            Vector3 rc = new Vector3(
                (float)drawArgs.WorldCamera.ReferenceCenter.X,
                (float)drawArgs.WorldCamera.ReferenceCenter.Y,
                (float)drawArgs.WorldCamera.ReferenceCenter.Z
                );

            drawArgs.device.Transform.World = Matrix.Translation(-rc);

            if (m_listpcs != null)
            {
                try
                {
                    foreach (CustomVertex.PositionColored[] ppcs in m_listpcs)
                    {
                        drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, ppcs.Length - 1, ppcs);
                    }
                }
                catch (Exception)
                {
                    
                }
              
            }

            //如果选中范围框数不为空，则绘制选中的范围框
            if (m_selectExtentsPCs != null)
            {
                foreach (CustomVertex.PositionColored[] ppcs in m_selectExtentsPCs)
                {
                    drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, ppcs.Length - 1, ppcs);
                }
            }

            drawArgs.device.Transform.World = drawArgs.WorldCamera.WorldMatrix;

            #region 绘制数量

            if (m_currentExtents != null)
            {

                foreach (KeyValuePair<RectangleF, int> kvp in m_currentExtents)
                {
                    if (kvp.Key.Top > DrawArgs.CurrentMouseLatitude.Degrees
                        && kvp.Key.Bottom < DrawArgs.CurrentMouseLatitude.Degrees
                        && kvp.Key.Left < DrawArgs.CurrentMouseLongtitude.Degrees
                        && kvp.Key.Right > DrawArgs.CurrentMouseLongtitude.Degrees)
                    {
                        DrawTextFormat format = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Bottom;
                        Rectangle rect = Rectangle.FromLTRB(DrawArgs.CurrentMousePosition.X,
                            DrawArgs.CurrentMousePosition.Y,
                            DrawArgs.CurrentMousePosition.X + 100, DrawArgs.CurrentMousePosition.Y + 40);
                        drawArgs.DefaultDrawingFont.DrawText(null, string.Format("数量：{0}", kvp.Value),
                            rect, format, is3DMapMode ? Color.Black : Color.White);
                    }
                }
            }

            #endregion

            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = true;
        }

        #region 鼠标键盘事件

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

        }

        public override void MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;
        }

        #endregion

        #endregion

    }
}
