using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Geometries;
using QRST.WorldGlobeTool.PluginEngine;

namespace QRST.WorldGlobeTool.DrawUtility
{
    /// <summary>
    /// 绘制多边形区域
    /// </summary>
    public class DrawPolygonLayer : DrawBaseLayer
    {

        #region  字段
             public event EventHandler OnUp;
        /// <summary>
        /// 绘制完毕事件
        /// </summary>
        public event EventHandler OnCompleted;

        public List<CustomVertex.PositionColored[]> ListPcs;
        /// <summary>
        /// 线的颜色
        /// </summary>
        public Color lineColor;
        /// <summary>
        /// 线的宽度
        /// </summary>
        public float lineWidth = 1.0f;
        /// <summary>
        /// 最小可视海拔高度
        /// </summary>
        protected double MinAltitude = 0;
        /// <summary>
        /// 最大可视海拔高度
        /// </summary>
        protected double MaxAltitude = 5700000;
        /// <summary>
        /// 距离地面高度
        /// </summary>
        public double DistanceAboveSurface = 0;
        /// <summary>
        /// 移动填充
        /// </summary>
        public bool movingFill = false;
        /// <summary>
        /// 多边形
        /// </summary>
        Polygon m_polygon;
        /// <summary>
        /// 绘制多边形
        /// </summary>
        List<DrawPolygons> m_polygonDrawers;

        #endregion

        #region  构造函数

        /// <summary>
        /// 初始化一个DrawPolygenLayer实例
        /// </summary>
        /// <param name="pName">图层名称</param>
        /// <param name="pColor">线条颜色</param>
        /// <param name="pWorld">所属的世界</param>
        /// <param name="drawArgs">绘制参数</param>
        public DrawPolygonLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pColor, pWorld, drawArgs)
        {
            m_polygonDrawers = new List<DrawPolygons>();
            ListPcs = new List<CustomVertex.PositionColored[]>();
            ListPcs.Add(pcs);
            lineColor = Color.FromArgb(pColor.A, pColor.R, pColor.G, pColor.B);
        }

        /// <summary>
        /// 初始化一个DrawPolygenLayer实例
        /// </summary>
        /// <param name="pName">图层名称</param>
        /// <param name="pColor">线条颜色</param>
        /// <param name="drawTool">插件类型</param>
        /// <param name="drawArgs">绘制参数</param>
        public DrawPolygonLayer(string pName, Color pColor, Plugin drawTool, DrawArgs drawArgs)
            : base(pName, pColor, drawTool, drawArgs)
        {
            ListPcs = new List<CustomVertex.PositionColored[]>();
            ListPcs.Add(pcs);
            lineColor = Color.FromArgb(pColor.A, pColor.R, pColor.G, pColor.B);
        }

        #endregion

        #region 重载方法

        /// <summary>
        /// 初始化当前图层
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        public override void Initialize(DrawArgs drawArgs)
        {
            base.Initialize(drawArgs);
            IsInitialized = true;
        }

        /// <summary>
        /// 更新当前图层
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        public override void Update(DrawArgs drawArgs)
        {
            if (!this.IsInitialized)
                this.Initialize(drawArgs);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            IsInitialized = false;
            m_polygonDrawers.Clear();
            base.Dispose();
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
                base.IsOn = value;
                //控制栅格化Polygon的显示 ProjectedVectorRenderer中
                if (State == DrawState.Complete)
                {
                    if (isOn)
                        this.Render(this.drawArgs);
                }
            }
        }

        /// <summary>
        /// 处理图层选中事件
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <returns></returns>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            return false;
        }

        /// <summary>
        /// 渲染图层
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            // 关闭灯光效果
            if (World.Settings.EnableSunShading)
                drawArgs.device.RenderState.Lighting = false;

            // 检查纹理是否被初始化
            if (!IsInitialized)
                Initialize(drawArgs);

            if (DrawArgs.MouseCursor == CursorType.Arrow)
                DrawArgs.MouseCursor = CursorType.Cross;

            if (State == DrawState.Idle)
                return;

            Device device = drawArgs.device;
            device.RenderState.ZBufferEnable = false;
            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.VertexFormat = CustomVertex.PositionColored.Format;

            if (State == DrawState.Complete || movingFill)
            {
                foreach (DrawPolygons m_polygonDrawer in m_polygonDrawers)
                {
                    //foreach (DrawPolygons m_polygonDrawer in m_polygonDrawers)
                    //{
                    m_polygonDrawer.Render(drawArgs);
                    //}
                }
            }

            if (State == DrawState.Drawing && !movingFill)// || drawArgs.WorldCamera.Altitude > MaxAltitude)
            {
                Vector3 rc = new Vector3(
                    (float)drawArgs.WorldCamera.ReferenceCenter.X,
                    (float)drawArgs.WorldCamera.ReferenceCenter.Y,
                    (float)drawArgs.WorldCamera.ReferenceCenter.Z
                    );

                drawArgs.device.Transform.World = Matrix.Translation(-rc);

                //float oldWidth = drawArgs.device.RenderState.PointSize;
                //drawArgs.device.RenderState.PointSize = 3.5f;
                drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, pcs.Length - 1, pcs);
                //drawArgs.device.RenderState.PointSize = oldWidth;

                drawArgs.device.Transform.World = drawArgs.WorldCamera.WorldMatrix;
            }

            if (World.Settings.EnableSunShading)
                drawArgs.device.RenderState.Lighting = true;
        }


        public void DrawPoint(Angle lat, Angle lon)
        {
            //if (World.Settings.CurrentWwTool != this.drawTool)
            //    return;

            if (!isOn)
                return;
            #region state

            if (State == DrawState.Idle || State == DrawState.Complete)
            {
                if (!this.Initialized)
                {
                    this.Initialize(this.drawArgs);
                }
                State = DrawState.Drawing;
            }

            #endregion
            if (State == DrawState.Drawing && this.Initialized)
            {


                if (Angle.IsNaN(lat))
                {
                    return;
                }

                Point3d pickPt = new Point3d();
                pickPt.X = lon.Degrees;
                pickPt.Y = lat.Degrees;
                pickPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;

                if (PointList.Count > 0)
                {
                    List<CustomVertex.PositionColored> newVertexes = getCurveFromPoints(PointList[PointList.Count - 1], pickPt, lineColor);
                    VertexList.AddRange(newVertexes.GetRange(1, newVertexes.Count - 1));
                }
                else
                {
                    VertexList.Add(Point3d2PositionColored(pickPt, lineColor));
                }
                PointList.Add(pickPt);
            }
        }

        public void completePart(Angle lat, Angle lon)
        {
            CreatePolygon();
            PointList.Clear();
        }

        public void completeDraw(Angle lat, Angle lon)
        {
            //if (World.Settings.CurrentWwTool != this.drawTool)
            //    return;

            if (State == DrawState.Drawing)
            {
                //    if (Angle.IsNaN(lat))
                //    {
                //        return;
                //    }

                //    Point3d pickPt = new Point3d();
                //    pickPt.X = lon.Degrees;
                //    pickPt.Y = lat.Degrees;
                //    pickPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;

                //    if (PointList.Count > 0)
                //    {
                //        List<CustomVertex.PositionColored> newVertexes = getCurveFromPoints(PointList[PointList.Count - 1], pickPt, lineColor);
                //        VertexList.AddRange(newVertexes.GetRange(1, newVertexes.Count - 1));
                //    }
                //    else
                //    {
                //        VertexList.Add(Point3d2PositionColored(pickPt, lineColor));
                //    }
                //    PointList.Add(pickPt);
                //}

                //List<CustomVertex.PositionColored> lastVertexes = getCurveFromPoints(PointList[PointList.Count - 1], PointList[0], lineColor);
                //VertexList.AddRange(lastVertexes.GetRange(1, lastVertexes.Count - 1));
                //pcs = new CustomVertex.PositionColored[VertexList.Count];
                //VertexList.CopyTo(pcs);

                State = DrawState.Complete;
                CreatePolygon();
            }
            if (OnCompleted != null)
            {
                OnCompleted(this, new EventArgs());
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;
            mouseDownPoint = DrawArgs.LastMousePosition;
        }
        public Point3d pt3d = new Point3d();
        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

            // Test if mouse was clicked and dragged
            if (mouseDragged())
                return;

            //multiline = new MeasureMultiLine();

            //  MouseButtons.Right
            if (e.Button == MouseButtons.Right)
            {
                State = DrawState.Idle;
                this.Dispose();
                if (OnUp != null)
                {
                    string isRightButton = "true";
                    OnUp(isRightButton, new EventArgs());
                }
                return;
            }

            //  MouseButtons.Other mouse buttons clicked
            if (e.Button != MouseButtons.Left)
                return;

            // MouseButtons.Left
            if (State == DrawState.Idle)
            {
                if (!this.Initialized)
                {
                    this.Initialize(this.drawArgs);
                }
                State = DrawState.Drawing;
            }
            if (State == DrawState.Drawing && this.Initialized)
            {
                Angle lat;
                Angle lon;

                //get Point
                this.drawArgs.WorldCamera.PickingRayIntersection(
                    e.X,
                    e.Y,
                    out lat,
                    out lon);

                if (Angle.IsNaN(lat))
                {
                    return;
                }

                Point3d pickPt = new Point3d();
                pickPt.X = lon.Degrees;
                pickPt.Y = lat.Degrees;
                pickPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;

                if (PointList.Count > 0)
                {
                    List<CustomVertex.PositionColored> newVertexes = getCurveFromPoints(PointList[PointList.Count - 1], pickPt, lineColor);
                    VertexList.AddRange(newVertexes.GetRange(1, newVertexes.Count - 1));
                }
                else
                {
                    VertexList.Add(Point3d2PositionColored(pickPt, lineColor));
                }
                pt3d = pickPt;
                PointList.Add(pickPt);
                if (OnUp != null)
                {
                    string isRightButton = "false";
                    OnUp(isRightButton, new EventArgs());
                }
                //UpdatePolygenFeature();
            }
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

            if (State != DrawState.Drawing)
                return;

            Angle lat;
            Angle lon;
            drawArgs.WorldCamera.PickingRayIntersection(
                e.X,
                e.Y,
                out lat,
                out lon);

            if (Angle.IsNaN(lat))
                return;

            movingPt.IsNaN = false;
            movingPt.X = lon.Degrees;
            movingPt.Y = lat.Degrees;
            movingPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;

            UpdateTrackVertexList();
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;


            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (State == DrawState.Drawing)
            {
                State = DrawState.Complete;
                CreatePolygon();
            }


            if (OnCompleted != null)
            {
                OnCompleted(this, new EventArgs());
            }
        }

        /// <summary>
        /// 键盘弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            //throw new NotImplementedException();
        }

        #endregion

        #region 填充多边形 含凹多边行

        /// <summary>
        /// 更新顶点列表
        /// </summary>
        protected void UpdateTrackVertexList()
        {
            if (State == DrawState.Drawing && !movingPt.IsNaN)
            {
                if (movingFill)
                {
                    Point3d[] Pts = new Point3d[PointList.Count + 1];
                    PointList.CopyTo(Pts);
                    Pts[PointList.Count] = movingPt;
                    CreatePolygon(Pts);
                }
                else
                {
                    List<CustomVertex.PositionColored> movingPcs1 = getCurveFromPoints(PointList[PointList.Count - 1], movingPt, lineColor);
                    List<CustomVertex.PositionColored> movingPcs2 = getCurveFromPoints(movingPt, PointList[0], lineColor);
                    pcs = null;
                    pcs = new CustomVertex.PositionColored[VertexList.Count + movingPcs1.Count + movingPcs2.Count - 2];
                    VertexList.CopyTo(pcs);
                    movingPcs1.CopyTo(1, pcs, VertexList.Count, movingPcs1.Count - 1);
                    movingPcs2.CopyTo(1, pcs, VertexList.Count + movingPcs1.Count - 1, movingPcs2.Count - 1);
                }
            }
        }

        /// <summary>
        /// 创建多边形
        /// </summary>
        /// <param name="pts">顶点列表</param>
        protected void CreatePolygon(Point3d[] pts)
        {
            LinearRing lr = new LinearRing();
            lr.Points = pts;
            m_polygon = new Polygon();
            m_polygon.outerBoundary = lr;
            m_polygon.PolgonColor = fillColor;
            m_polygon.Fill = true;
            m_polygon.ParentRenderable = this;
            m_polygon.Outline = true;
            m_polygon.OutlineColor = lineColor;
            m_polygon.LineWidth = lineWidth;

            if (m_polygonDrawers == null)
            {
                m_polygonDrawers = new List<DrawPolygons>();
            }

            m_polygonDrawers.Add(new DrawPolygons(m_polygon, World));
        }

        /// <summary>
        /// 创建多边形
        /// </summary>
        protected void CreatePolygon()
        {
            if (PointList.Count==0)
            {
                return;
            }
            Point3d[] Pts = new Point3d[PointList.Count];
            PointList.CopyTo(Pts);
            CreatePolygon(Pts);
        }


        #endregion

    }
}
