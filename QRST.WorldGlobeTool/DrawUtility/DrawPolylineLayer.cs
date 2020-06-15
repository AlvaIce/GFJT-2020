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
    /// 绘制多线段图层
    /// </summary>
    public class DrawPolylineLayer : DrawBaseLayer
    {

        #region  字段
        public bool customArrow = false;
        /// <summary>
        /// 绘制完毕事件
        /// </summary>
        public event EventHandler OnCompleted;
        /// <summary>
        /// 绘制完毕事件
        /// </summary>
        public event EventHandler OnPolyUp;
        /// <summary>
        /// 线的宽度
        /// </summary>
        protected float lineWidth = 1.0f;
        /// <summary>
        /// 最小可视海拔高度
        /// </summary>
        protected double MinAltitude = 0;
        /// <summary>
        /// 最大可视海拔高度
        /// </summary>
        protected double MaxAltitude = 5700000;
        /// <summary>
        /// 当前鼠标点对应的纬度
        /// </summary>
        Angle lat;
        /// <summary>
        /// 当前鼠标点对应的经度
        /// </summary>
        Angle lon;
        /// <summary>
        /// 线的颜色
        /// </summary>
        public Color lineColor;

        

        #endregion

        #region  构造函数

        /// <summary>
        /// 初始化一个DrawPolylineLayer实例
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pColor"></param>
        /// <param name="pWorld"></param>
        /// <param name="drawArgs"></param>
        public DrawPolylineLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pColor, pWorld, drawArgs)
        {
            lineColor = Color.FromArgb(pColor.A, pColor.R, pColor.G, pColor.B);
        }

        /// <summary>
        /// 初始化一个DrawPolylineLayer实例
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pColor"></param>
        /// <param name="drawTool"></param>
        /// <param name="drawArgs"></param>
        public DrawPolylineLayer(string pName, Color pColor, Plugin drawTool, DrawArgs drawArgs)
            : base(pName, pColor, drawTool, drawArgs)
        {
            lineColor = Color.FromArgb(pColor.A, pColor.R, pColor.G, pColor.B);
        }

        #endregion

        #region 重载基类方法

        /// <summary>
        /// 初始化绘制参数
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            base.Initialize(drawArgs);
            IsInitialized = true;
        }

        /// <summary>
        /// 更新绘制图层
        /// </summary>
        /// <param name="drawArgs"></param>
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
        /// 鼠标单击事件，不做额外处理
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
        /// <param name="drawArgs">绘制参数</param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            // Turn off light
            if (World.Settings.EnableSunShading)
                drawArgs.device.RenderState.Lighting = false;

            // Check that textures are initialised
            if (!IsInitialized)
                Initialize(drawArgs);

            if (DrawArgs.MouseCursor == CursorType.Arrow && !customArrow)
                DrawArgs.MouseCursor = CursorType.Cross;

            if (State == DrawState.Idle)
                return;

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
            //float oldWidth = drawArgs.device.RenderState.PointSize;
            //drawArgs.device.RenderState.PointSize = lineWidth;
            drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, pcs.Length - 1, pcs);
            //drawArgs.device.RenderState.PointSize = oldWidth;
            drawArgs.device.Transform.World = drawArgs.WorldCamera.WorldMatrix;

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
                    return;

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

                //虽然我也不知道为什么但是不加这一段整个三维球界面都会变黑
                movingPt.IsNaN = false;
                movingPt.X = pickPt.X;
                movingPt.Y = pickPt.Y;
                movingPt.Z = pickPt.Z;
                UpdateTrackVertexList();
            }
        }
        public void completeDraw(Angle lat, Angle lon)
        {
            //if (World.Settings.CurrentWwTool != this.drawTool)
            //    return;
            //不加这段会缺少最后一段线
            movingPt.IsNaN = false;
            movingPt.X = lon.Degrees;
            movingPt.Y = lat.Degrees;
            movingPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;
            UpdateTrackVertexList();

            if (State == DrawState.Drawing)
            {
                State = DrawState.Complete;
                //if (PointList.Count == 0)
                //{
                //    return;
                //}
                //Point3d[] Pts = new Point3d[PointList.Count];
                //PointList.CopyTo(Pts);
                
            }
            if (OnCompleted != null)
            {
                OnCompleted(this, new EventArgs());
            }
        }
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
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
        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

            //DrawArgs.MouseCursor = CursorType.Cross;

            // Test if mouse was clicked and dragged
            if (mouseDragged())
                return;

            //  MouseButtons.Right
            if (e.Button == MouseButtons.Right)
            {
                State = DrawState.Idle;
                this.Dispose();
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

                //UpdatePolygenFeature();
            }

            if (OnPolyUp != null)
            {
                OnPolyUp(this, new EventArgs());
            }
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            if (!isOn)
                return;

            drawArgs.WorldCamera.PickingRayIntersection(
                e.X,
                e.Y,
                out lat,
                out lon);

            if (State != DrawState.Drawing)
                return;


            if (Angle.IsNaN(lat))
                return;

            movingPt.IsNaN = false;
            movingPt.X = lon.Degrees;
            movingPt.Y = lat.Degrees;
            movingPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;

            UpdateTrackVertexList();

            //movingPc = Point3d2PositionColored(movingPt, Color);

            //UpdateLineFeature();
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
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
            }


            if (OnCompleted != null)
            {
                OnCompleted(this, new EventArgs());
            }
        }

        /// <summary>
        /// 键盘弹起事件
        /// </summary>
        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawPlugin)
                return;

            //throw new NotImplementedException();
        }

        #endregion

        #region  受保护方法

        /// <summary>
        /// 更新顶点列表
        /// </summary>
        protected void UpdateTrackVertexList()
        {
            if (State == DrawState.Drawing && !movingPt.IsNaN)
            {
                List<CustomVertex.PositionColored> movingPcs1 = getCurveFromPoints(PointList[PointList.Count - 1], movingPt, lineColor);
                pcs = null;
                pcs = new CustomVertex.PositionColored[VertexList.Count + movingPcs1.Count - 1];
                VertexList.CopyTo(pcs);
                movingPcs1.CopyTo(1, pcs, VertexList.Count, movingPcs1.Count - 1);
            }
        }

        #endregion

    }
}
