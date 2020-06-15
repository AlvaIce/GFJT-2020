using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WorldWind;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using WorldWind.PluginEngine;
using System.Collections;
using System.IO;
using Qrst;

namespace DrawTools.Plugins
{
    public class DrawRectangleLayer : DrawBaseLayer
    {

        //public bool IsTmpDrawGraphicLyr;
        //protected PolygonFeature m_pf;

        public event EventHandler OnCompeleted;

        public Color lineColor;
        public float lineWidth = 2.0f;
        protected double MinAltitude = 0;
        protected double MaxAltitude = 5700000;
        public double DistanceAboveSurface = 0;
        public bool movingFill = false;

        Polygon m_polygon;
        //ImageLayer imagelyr;
        DrawPolygons m_polygonDrawer;

        public DrawRectangleLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pColor, pWorld, drawArgs) 
        {
            lineColor = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }

        public DrawRectangleLayer(string pName, Color pColor, DrawBaseTool drawTool, DrawArgs drawArgs)
            : base(pName,pColor,drawTool,drawArgs)  
        {
            lineColor = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }

        public void DrawRectangle(Point3d fromPoint,Point3d toPoint)
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
            if (State == DrawState.Drawing &&PointList.Count>0&& !movingPt.IsNaN)
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

        #region base Override
        public override void Initialize(DrawArgs drawArgs)
        {
            base.Initialize(drawArgs);
            isInitialized = true;
        }

        public override void Update(DrawArgs drawArgs)
        {
            if (!this.isInitialized)
                this.Initialize(drawArgs);
        }

        public override void Dispose()
        {
            isInitialized = false;
            //if (m_pf != null)
            //{
            //    m_pf.Dispose();     //Dispose 用法，参考PlolygonFeature中的Polygon  JOKI
            //    m_pf = null;
            //}
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
                        //CreatePolygon();

                        this.Render(this.drawArgs);
                    }
                    else
                    {
                        //if (m_pf != null)
                        //{
                        //    m_pf.Dispose();     //Dispose 用法，参考PlolygonFeature中的Polygon  JOKI
                        //    m_pf = null;
                        //}
                    }
                }
            }
        }

        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            return false;
        }
        
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            // Turn off light
            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = false;

            // Check that textures are initialised
            if (!isInitialized)
                Initialize(drawArgs);

            if (DrawArgs.MouseCursor != CursorType.Cross)
                // Use our cursor when the mouse isn't over other elements requiring different cursor
                DrawArgs.MouseCursor = CursorType.Cross;


            // Draw the measure line + ends
            /*
            device.DrawUserPrimitives(PrimitiveType.LineStrip, measureLine.Length-1, measureLine);
            device.DrawUserPrimitives(PrimitiveType.LineStrip, startPoint.Length-1, startPoint);
            device.DrawUserPrimitives(PrimitiveType.LineList, endPoint.Length>>1, endPoint);
            */
            Device device = drawArgs.device;
            device.RenderState.ZBufferEnable = false;
            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.VertexFormat = CustomVertex.PositionColored.Format;


            if (State == DrawState.Idle)
                return;

            if (State== DrawState.Complete|| movingFill)
            {
                //if (!m_pf.Initialized)
                //{
                //    m_pf.Initialize(drawArgs);
                //    World.ProjectedVectorRenderer.Update(drawArgs);
                //}
                //m_pf.Render(drawArgs);

                //if (imagelyr!=null)
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

            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = true;
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;
            
            if (!isOn)
                return;
            mouseDownPoint = DrawArgs.LastMousePosition;
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
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

                return;
            }

            //  MouseButtons.Other mouse buttons clicked
            if (e.Button != MouseButtons.Left)
                return;

            // MouseButtons.Left
            #region state

            if (State == DrawState.Idle)
            {
                if (!this.Initialized)
                {
                    this.Initialize(this.drawArgs);
                }
                State = DrawState.Drawing;
            }

            #endregion
            if ((State == DrawState.Drawing && PointList.Count < 1) && this.Initialized)
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

                //UpdatePolygenFeature();
            }
        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;
            
            if (!isOn)
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

            if (!this.Initialized)
            {
                this.Initialize(this.drawArgs);
            }

            movingPt.IsNaN = false;
            movingPt.X = lon.Degrees;
            movingPt.Y = lat.Degrees;
            movingPt.Z = (useTerrain) ? getElevation(lat.Degrees, lon.Degrees) : defaultElev;

            if (State !=  DrawState.Drawing)
                return;
            UpdateTrackVertexList();

            //movingPc = Point3d2PositionColored(movingPt, Color);

            //UpdateLineFeature();
        }

        public override void MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
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
            //UpdatePolygenFeature();
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

            //CreatePolygon();
            //this.drawTool.Polygon = this.m_polygon;

            if (OnCompeleted!=null)
            {
                OnCompeleted(this, new EventArgs());
            }
        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;
            
        }
        #endregion

        #region 填充多边形 含凹多边行
        protected void CreatePolygon(Point3d[] pts)
        {
            LinearRing lr = new LinearRing();
            lr.Points = pts;
            //m_pf = new PolygonFeature("tmp", World, lr, null, color);
            //m_pf.Outline = true;
            //m_pf.OutlineColor = lineColor;
            //m_pf.OutlineWidth = lineWidth;
            //m_pf.DistanceAboveSurface = 100000;

            m_polygon = new Polygon();
            m_polygon.outerBoundary = lr;
            //m_polygon.innerBoundaries = m_innerRings;
            m_polygon.PolgonColor = color;
            m_polygon.Fill = true;
            m_polygon.ParentRenderable = this;
            m_polygon.Outline = true;
            m_polygon.OutlineColor = lineColor;
            m_polygon.LineWidth = lineWidth;

            if (m_polygonDrawer!=null)
            {
                m_polygonDrawer.Dispose();
                m_polygonDrawer = null;
                //System.GC.Collect();
            }

            m_polygonDrawer = new DrawPolygons(m_polygon, World);
            //imagelyr = FillPolygon(m_polygon);

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

        /* 写在DrawPolygons中
        public ImageLayer FillPolygon(Polygon polygon)
        {
            //create polygon imagestream
            ImageLayer imageLayer = null;

            GeographicBoundingBox gbox = polygon.GetGeographicBoundingBox();
            Bitmap b = null;
            Graphics g = null;
            if (b == null)
            {                
                b = new Bitmap(
                    FillImageSize.Width,      //JOKI 图像Width像素个数，越多分辨率越高也越清晰
                    FillImageSize.Height,     //JOKI 图像Height像素个数，越多分辨率越高也越清晰
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }

            if (g == null)
            {
                g = Graphics.FromImage(b);
            }

            #region Fill innerBounderies
            if (polygon.innerBoundaries != null && polygon.innerBoundaries.Length > 0)
            {
                if (polygon.Fill)
                {
                    using (SolidBrush brush = new SolidBrush(polygon.PolgonColor))
                    {
                        g.FillPolygon(brush, getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, gbox, b.Size));
                    }
                }

                if (polygon.Outline)
                {
                    using (Pen p = new Pen(polygon.OutlineColor, polygon.LineWidth))
                    {
                        g.DrawPolygon(p,
                                getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, gbox, b.Size));
                    }
                }

                if (polygon.Fill)
                {
                    using (SolidBrush brush = new SolidBrush(System.Drawing.Color.Black))
                    {
                        for (int i = 0; i < polygon.innerBoundaries.Length; i++)
                        {
                            g.FillPolygon(brush,
                                getScreenPoints(polygon.innerBoundaries[i].Points, 0, polygon.innerBoundaries[i].Points.Length, gbox, b.Size));
                        }
                    }
                }
            }
            #endregion
            #region Fill OutBounderries Only
            else
            {
                if (polygon.Fill)
                {
                    using (SolidBrush brush = new SolidBrush(polygon.PolgonColor))
                    {
                        g.FillPolygon(brush, getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, gbox, b.Size));
                    }
                }

                if (polygon.Outline)
                {
                    using (Pen p = new Pen(polygon.OutlineColor, polygon.LineWidth))
                    {
                        g.DrawPolygon(p, getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, gbox, b.Size));
                    }
                }
            }
            #endregion

            #region Create ImageLayer
            string id = System.DateTime.Now.Ticks.ToString();

            if (b != null)
            {
                MemoryStream ms = new MemoryStream();
                b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                //must copy original stream into new stream, if not, error occurs, not sure why
                MemoryStream m_ImageStream = new MemoryStream(ms.GetBuffer());

                imageLayer = new WorldWind.Renderable.ImageLayer(
                    id,
                    World,
                    DistanceAboveSurface,
                    m_ImageStream,
                    System.Drawing.Color.Black.ToArgb(),
                    (float)gbox.South,
                    (float)gbox.North,
                    (float)gbox.West,
                    (float)gbox.East,
                    1.0f,//(float)m_parentProjectedLayer.Opacity / 255.0f                    
                    World.TerrainAccessor);

                ms.Close();
            }
            #endregion

            if (b != null)
            {
                b.Dispose();
            }
            if (g != null)
            {
                g.Dispose();
            }

            b = null;
            g = null;

            System.GC.Collect();

            return imageLayer;
        }

        private System.Drawing.Point[] getScreenPoints(Point3d[] sourcePoints, int offset, int length, GeographicBoundingBox dstBB, Size dstImageSize)
        {
            double degreesPerPixelX = (dstBB.East - dstBB.West) / (double)dstImageSize.Width;
            double degreesPerPixelY = (dstBB.North - dstBB.South) / (double)dstImageSize.Height;

            ArrayList screenPointList = new ArrayList();
            for (int i = offset; i < offset + length; i++)
            {
                double screenX = (sourcePoints[i].X - dstBB.West) / degreesPerPixelX;
                double screenY = (dstBB.North - sourcePoints[i].Y) / degreesPerPixelY;

                if (screenPointList.Count > 0)
                {
                    Point v = (Point)screenPointList[screenPointList.Count - 1];
                    if (v.X == (int)screenX && v.Y == (int)screenY)
                    {
                        continue;
                    }
                }

                screenPointList.Add(new Point((int)screenX, (int)screenY));
            }

            if (screenPointList.Count <= 1)
                return new Point[] { new Point(0, 0), new Point(0, 0) };

            return (Point[])screenPointList.ToArray(typeof(Point));
        }
        */
        #endregion

        //test
        public void DrawVertical()
        {
            CustomVertex.PositionColored[] pcList = new CustomVertex.PositionColored[(PointList.Count + 1) * 2];
            int j=0;
            for (int i = 0; i < PointList.Count + 1; i++)
            {
                Point3d curPoint = (i == PointList.Count) ? PointList[0] : PointList[i];

                Point3d verticalPt = new Point3d(curPoint.X, curPoint.Y, curPoint.Z + DistanceAboveSurface);
                CustomVertex.PositionColored newPc = Point3d2PositionColored(verticalPt, Color.FromArgb(128,color));
                pcList[j] = newPc;
                j++;
                Point3d groundPt = new Point3d(curPoint.X, curPoint.Y, 0);
                CustomVertex.PositionColored newPc2 = Point3d2PositionColored(groundPt, Color.FromArgb(128, color));
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
