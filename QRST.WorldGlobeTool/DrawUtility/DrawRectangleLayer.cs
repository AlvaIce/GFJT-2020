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
    /// 绘制矩形区域图层
    /// </summary>
    public class DrawRectangleLayer : DrawBaseLayer
    {
        #region  字段

        /// <summary>
        /// 绘制完毕事件
        /// </summary>
        public event EventHandler OnCompleted;
        /// <summary>
        /// 线的颜色
        /// </summary>
        public Color lineColor;
        /// <summary>
        /// 线的宽度
        /// </summary>
        public float lineWidth = 5.0f;
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
        DrawPolygons m_polygonDrawer;

        #endregion

        #region  构造函数

        /// <summary>
        /// 初始化一个DrawRectangleLayer实例对象
        /// </summary>
        /// <param name="pName">图层名称</param>
        /// <param name="pColor">图层绘制线的颜色</param>
        /// <param name="pWorld">图层所属的世界</param>
        /// <param name="drawArgs">绘制参数</param>
        public DrawRectangleLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pColor, pWorld, drawArgs)
        {
            lineColor = Color.FromArgb(pColor.A, pColor.R, pColor.G, pColor.B);
        }

        /// <summary>
        /// 初始化一个DrawRectangleLayer实例对象
        /// </summary>
        /// <param name="pName">图层名称</param>
        /// <param name="pColor">图层绘制线的颜色</param>
        /// <param name="drawPlugin">插件</param>
        /// <param name="drawArgs">绘制参数</param>
        public DrawRectangleLayer(string pName, Color pColor, Plugin drawPlugin, DrawArgs drawArgs)
            : base(pName, pColor, drawPlugin, drawArgs)
        {
            lineColor = Color.FromArgb(pColor.A, pColor.R, pColor.G, pColor.B);
        }

        #endregion

        #region 重载方法

        /// <summary>
        /// 初始化图层
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        public override void Initialize(DrawArgs drawArgs)
        {
            base.Initialize(drawArgs);
            IsInitialized = true;
        }

        /// <summary>
        /// 更新图层
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
            base.Dispose();
        }

        /// <summary>
        /// 获取或设置图层是否可显示
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
        /// 渲染当前图层
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            // 关闭灯光效果
            if (World.Settings.EnableSunShading)
                drawArgs.device.RenderState.Lighting = false;

            // Check that textures are initialised，检查是否初始化
            if (!IsInitialized)
                Initialize(drawArgs);

            if (DrawArgs.MouseCursor == CursorType.Arrow)
                // Use our cursor when the mouse isn't over other elements requiring different cursor
                DrawArgs.MouseCursor = CursorType.Cross;

            if (State == DrawState.Idle)
                return;

            Device device = drawArgs.device;
            device.RenderState.ZBufferEnable = false;
            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.VertexFormat = CustomVertex.PositionColored.Format;

            if (State == DrawState.Complete || movingFill)
            {
                //if (!m_pf.Initialized)
                //{
                //    m_pf.Initialize(drawArgs);
                //    World.ProjectedVectorRenderer.Update(drawArgs);
                //}
                //m_pf.Render(drawArgs);

                //if (imagelyr != null)
                //{
                //    if (!imagelyr.Initialized)
                //    {
                //        imagelyr.Initialize(drawArgs);
                //    }
                //    imagelyr.Render(drawArgs);
                //}

                //if (State== DrawState.Complete)
                //{
                //    DrawVertical();
                //}

                //  m_polygonDrawer.Render(drawArgs);
            }

            if (PointList.Count > 0)//State == DrawState.Drawing && !movingFill)// || drawArgs.WorldCamera.Altitude > MaxAltitude)
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

            //打开灯光效果
            if (World.Settings.EnableSunShading)
                drawArgs.device.RenderState.Lighting = true;
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

            // Test if mouse was clicked and dragged  测试鼠标是单击还是拖动
            if (mouseDragged())
                return;

            //  鼠标右键单击，清除绘制图形
            if (e.Button == MouseButtons.Right)
            {
                State = DrawState.Idle;
                this.Dispose();
                return;
            }

            //  其他鼠标单击弹起，直接退出
            if (e.Button != MouseButtons.Left)
                return;

            // 鼠标左键弹起事件
            if (State == DrawState.Idle)
            {
                if (!this.Initialized)
                {
                    this.Initialize(this.drawArgs);
                }
                State = DrawState.Drawing;
            }
            //处于绘制状态、点列表的数目＜1、初始化完毕
            if ((State == DrawState.Drawing && PointList.Count < 1) && this.Initialized)
            {
                Angle lat;
                Angle lon;

                //获取点的经纬度信息
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

                PointList.Add(pickPt);
            }
        }

        /// <summary>
        /// 鼠标移动
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

                PointList.Add(pickPt);
            }

            State = DrawState.Complete;
            Point3d[] Pts = new Point3d[4];
            Pts[0] = PointList[0];
            Pts[1] = new Point3d(Pts[0].X, movingPt.Y, Pts[0].Z);
            Pts[2] = movingPt;
            Pts[3] = new Point3d(movingPt.X, Pts[0].Y, Pts[0].Z);

            PointList.Clear();
            PointList.Add(Pts[0]);
            PointList.Add(Pts[1]);
            PointList.Add(Pts[2]);
            PointList.Add(Pts[3]);

            if (OnCompleted != null)
            {
                OnCompleted(this, new EventArgs());
            }
        }

        /// <summary>
        /// 键盘按键弹起事件
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

        #region 受保护方法

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

            if (m_polygonDrawer != null)
            {
                m_polygonDrawer.Dispose();
                m_polygonDrawer = null;
                //System.GC.Collect();
            }

            m_polygonDrawer = new DrawPolygons(m_polygon, World);
        }

        protected void CreatePolygon()
        {
            Point3d[] Pts = new Point3d[4];
            Pts[0] = PointList[0];
            Pts[1] = new Point3d(Pts[0].X, PointList[1].Y, Pts[0].Z);
            Pts[2] = PointList[1];
            Pts[3] = new Point3d(PointList[1].X, Pts[0].Y, Pts[0].Z);

            CreatePolygon(Pts);
        }

        public void DrawRectangle(Point3d fromPoint, Point3d toPoint)
        {
            this.PointList.Clear();
            this.PointList.Add(fromPoint);
            movingPt = toPoint;
            State = DrawState.Drawing;
            UpdateTrackVertexList();
            State = DrawState.Complete;
        }

        protected void UpdateTrackVertexList()
        {

            if (State == DrawState.Drawing && !movingPt.IsNaN && PointList.Count>0)
            {
                Point3d[] Pts = new Point3d[4];
                Pts[0] = PointList[0];
                Pts[1] = new Point3d(Pts[0].X, movingPt.Y, Pts[0].Z);
                Pts[2] = movingPt;
                Pts[3] = new Point3d(movingPt.X, Pts[0].Y, Pts[0].Z);

                if (movingFill)
                {
                    CreatePolygon(Pts);
                }
                else
                {

                    List<CustomVertex.PositionColored> movingPcs1 = getCurveFromPoints(Pts[0], Pts[1], lineColor);
                    List<CustomVertex.PositionColored> movingPcs2 = getCurveFromPoints(Pts[1], Pts[2], lineColor);
                    List<CustomVertex.PositionColored> movingPcs3 = getCurveFromPoints(Pts[2], Pts[3], lineColor);
                    List<CustomVertex.PositionColored> movingPcs4 = getCurveFromPoints(Pts[3], Pts[0], lineColor);

                    pcs = null;
                    pcs = new CustomVertex.PositionColored[movingPcs1.Count + movingPcs2.Count + movingPcs3.Count + movingPcs4.Count - 3];
                    movingPcs1.CopyTo(pcs);
                    movingPcs2.CopyTo(1, pcs, movingPcs1.Count, movingPcs2.Count - 1);
                    movingPcs3.CopyTo(1, pcs, movingPcs1.Count + movingPcs2.Count - 1, movingPcs3.Count - 1);
                    movingPcs4.CopyTo(1, pcs, movingPcs1.Count + movingPcs2.Count + movingPcs3.Count - 2, movingPcs4.Count - 1);
                }
            }
        }

        #endregion

        //test
        public void DrawVertical()
        {
            CustomVertex.PositionColored[] pcList = new CustomVertex.PositionColored[(PointList.Count + 1) * 2];
            int j = 0;
            for (int i = 0; i < PointList.Count + 1; i++)
            {
                Point3d curPoint = (i == PointList.Count) ? PointList[0] : PointList[i];

                Point3d verticalPt = new Point3d(curPoint.X, curPoint.Y, curPoint.Z + DistanceAboveSurface);
                CustomVertex.PositionColored newPc = Point3d2PositionColored(verticalPt, Color.FromArgb(128, fillColor));
                pcList[j] = newPc;
                j++;
                Point3d groundPt = new Point3d(curPoint.X, curPoint.Y, 0);
                CustomVertex.PositionColored newPc2 = Point3d2PositionColored(groundPt, Color.FromArgb(128, fillColor));
                pcList[j] = newPc2;
                j++;
            }

            Vector3 rc = new Vector3(
                (float)drawArgs.WorldCamera.ReferenceCenter.X,
                (float)drawArgs.WorldCamera.ReferenceCenter.Y,
                (float)drawArgs.WorldCamera.ReferenceCenter.Z
                );

            drawArgs.device.Transform.World = Matrix.Translation(-rc);

            //float oldWidth = drawArgs.device.RenderState.PointSize;
            //drawArgs.device.RenderState.PointSize = 3.5f;
            drawArgs.device.DrawUserPrimitives(PrimitiveType.TriangleStrip, pcList.Length - 2, pcList);
            //drawArgs.device.RenderState.PointSize = oldWidth;

            drawArgs.device.Transform.World = drawArgs.WorldCamera.WorldMatrix;
        }


    }
}
