using System;
using System.Collections.Generic;
using System.Text;
using DrawTools.Plugins;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Windows.Forms;
using System.Drawing;
using WorldWind.PluginEngine;
using Qrst;

namespace DrawTools.Plugins
{
    public class DrawPolylineLayer : DrawBaseLayer
    { 
        protected float lineWidth = 1.0f;
        protected double MinAltitude = 0;
        protected double MaxAltitude = 5700000;


        public DrawPolylineLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pColor, pWorld, drawArgs) 
        {
        }

        public DrawPolylineLayer(string pName, Color pColor, DrawBaseTool drawTool, DrawArgs drawArgs)
            : base(pName,pColor,drawTool,drawArgs)  
        {
        }

        protected void UpdateTrackVertexList()
        {
            if (State == DrawState.Drawing && !movingPt.IsNaN)
            {
                List<CustomVertex.PositionColored> movingPcs1 = getCurveFromPoints(PointList[PointList.Count - 1], movingPt, color);
                pcs = null;
                pcs = new CustomVertex.PositionColored[VertexList.Count + movingPcs1.Count-1];
                VertexList.CopyTo(pcs);
                movingPcs1.CopyTo(1, pcs, VertexList.Count, movingPcs1.Count - 1);
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

            if (DrawArgs.MouseCursor == CursorType.Arrow)
                // Use our cursor when the mouse isn't over other elements requiring different cursor
                DrawArgs.MouseCursor = CursorType.Measure;

            if (State == DrawState.Idle)
                return;


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

            //if (State== DrawState.Complete)
            //{
                //if (!m_pf.Initialized)
                //{
                //    m_pf.Initialize(drawArgs);
                //    World.ProjectedVectorRenderer.Update(drawArgs);
                //}
                //m_pf.Render(drawArgs);

            //}

            //if (State == DrawState.Drawing)// || drawArgs.WorldCamera.Altitude > MaxAltitude)
            //{

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
            //}


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
                    List<CustomVertex.PositionColored> newVertexes = getCurveFromPoints(PointList[PointList.Count - 1], pickPt, color);
                    VertexList.AddRange(newVertexes.GetRange(1, newVertexes.Count - 1));
                }
                else
                {
                    VertexList.Add(Point3d2PositionColored(pickPt, color));
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

            if (State !=  DrawState.Drawing)
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

            if (State== DrawState.Drawing)
            {
                State = DrawState.Complete;
                //LinearRing lr = new LinearRing();
                //lr.Points = new Point3d[PointList.Count];
                //PointList.CopyTo(lr.Points);
                //m_pf = new PolygonFeature("tmp", World, lr, null, color);
                //m_pf.Outline = true;
                //m_pf.OutlineColor = lineColor;
                //m_pf.OutlineWidth = 1.0f;
            }
        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;
            
            //throw new NotImplementedException();
        }
        #endregion
    }
    
}
