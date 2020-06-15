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
using DotSpatial.Data;
using SharpMap.Geometries;
using DotSpatial.Topology;

namespace DrawTools.Plugins
{
    public class DrawShapefileLayer : DrawBaseLayer
    { 
        protected float lineWidth = 1.0f;
        protected double MinAltitude = 0;
        protected double MaxAltitude = 5700000;
        public Color lineColor;


        public DrawShapefileLayer(string pName, Color pColor, float linwidth, World pWorld, DrawArgs drawArgs, double maxAltitude, double minAltitude)
            : base(pName, pColor, pWorld, drawArgs)
        {
            lineWidth = linwidth;
            MinAltitude = minAltitude;
            MaxAltitude = maxAltitude;
            lineColor = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }

        public DrawShapefileLayer(string pName, Color pColor, DrawBaseTool drawTool, DrawArgs drawArgs)
            : base(pName,pColor,drawTool,drawArgs)
        {
            lineColor = Color.FromArgb(255, pColor.R, pColor.G, pColor.B);
        }
        List<CustomVertex.PositionColored[]> listpcs = new List<CustomVertex.PositionColored[]>();
        public void UpdateExtents(List<RectangleF> rects)
        {
            listpcs.Clear();
            foreach (RectangleF rect in rects)
            {
                Point3d pt0 =  CreatePoint3dByLatLon(rect.Y,  rect.X);
                Point3d pt1 =  CreatePoint3dByLatLon(rect.Y,  rect.Right);
                Point3d pt2 =  CreatePoint3dByLatLon(rect.Bottom,  rect.Right);
                Point3d pt3 =  CreatePoint3dByLatLon(rect.Bottom,  rect.X);


                List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, lineColor);
                List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, lineColor);
                List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, lineColor);
                List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, lineColor);

                CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3];
                
                curve1.CopyTo(ppcs);
                curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
                curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
                curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);

                listpcs.Add(ppcs);
            }

        }

        private Point3d CreatePoint3dByLatLon(double lat, double lon)
        {
            Point3d pt3d = new Point3d();
            pt3d.X = lon;
            pt3d.Y = lat;
            pt3d.Z = (useTerrain) ? getElevation(lat, lon) : defaultElev;

            return pt3d;
        }


        public void OpenShapefile(string shpfile)
        {
            listpcs.Clear();

            Shapefile _sf = Shapefile.OpenFile(shpfile);

            foreach (IFeature ft in _sf.Features)
            {
                Shape shape = ft.ToShape();
                List<double[]> verticess = new List<double[]>();
                if (shape.Range.Parts == null)
                {
                    verticess.Add(shape.Vertices);
                }
                else
                {
                    foreach (PartRange pr in shape.Range.Parts)
                    {
                        verticess.Add(pr.Vertices);
                    }
                }

                foreach (double[] vts in verticess)
                {
                    double lon = 0, lat = 0;
                    Boolean islon = true;
                    List<Point3d> p3ds = new List<Point3d>();
                    List<CustomVertex.PositionColored> ppcs = new List<CustomVertex.PositionColored>();

                    foreach (double d in vts)
                    {
                        if (islon)
                        {
                            lon = d;
                            islon = false;
                            continue;
                        }
                        else
                        {
                            lat = d;
                            Point3d pt = CreatePoint3dByLatLon(lat, lon);
                            p3ds.Add(pt);
                            islon = true;
                        }
                    }
                    if (p3ds.Count > 1)
                    {
                        for (int i = 0; i < p3ds.Count - 1; i++)
                        {
                            List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(p3ds[i], p3ds[i + 1], lineColor);
                            ppcs.AddRange(curve1.ToArray());
                        }
                    }
                    listpcs.Add(ppcs.ToArray());
                }

            }

            //int maxCount = 0;
            //foreach (KeyValuePair<RectangleF, int> kvp in rects)
            //{
            //    if (kvp.Value > maxCount)
            //    {
            //        maxCount = kvp.Value;
            //    }
            //}

            //foreach (KeyValuePair<RectangleF, int> kvp in rects)
            //{
            //    Point3d pt0 = CreatePoint3dByLatLon(kvp.Key.Y, kvp.Key.X);
            //    Point3d pt1 = CreatePoint3dByLatLon(kvp.Key.Y, kvp.Key.Right);
            //    Point3d pt2 = CreatePoint3dByLatLon(kvp.Key.Bottom, kvp.Key.Right);
            //    Point3d pt3 = CreatePoint3dByLatLon(kvp.Key.Bottom, kvp.Key.X);

            //    lineColor = getColorbyCount(kvp.Value, maxCount);
            //    List<CustomVertex.PositionColored> curve1 = getCurveFromPoints(pt0, pt1, lineColor);
            //    List<CustomVertex.PositionColored> curve2 = getCurveFromPoints(pt1, pt2, lineColor);
            //    List<CustomVertex.PositionColored> curve3 = getCurveFromPoints(pt2, pt3, lineColor);
               //List<CustomVertex.PositionColored> curve4 = getCurveFromPoints(pt3, pt0, lineColor);
            //    List<CustomVertex.PositionColored> curve5 = getCurveFromPoints(pt0, pt2, lineColor);
            //    List<CustomVertex.PositionColored> curve6 = getCurveFromPoints(pt2, pt1, lineColor);
            //    List<CustomVertex.PositionColored> curve7 = getCurveFromPoints(pt1, pt3, lineColor);

            //    CustomVertex.PositionColored[] ppcs = new CustomVertex.PositionColored[curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count + curve7.Count - 6];

            //    curve1.CopyTo(ppcs);
            //    curve2.CopyTo(1, ppcs, curve1.Count, curve2.Count - 1);
            //    curve3.CopyTo(1, ppcs, curve1.Count + curve2.Count - 1, curve3.Count - 1);
            //    curve4.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count - 2, curve4.Count - 1);
            //    curve5.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count - 3, curve5.Count - 1);
            //    curve6.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count - 4, curve6.Count - 1);
            //    curve7.CopyTo(1, ppcs, curve1.Count + curve2.Count + curve3.Count + curve4.Count + curve5.Count + curve6.Count - 5, curve7.Count - 1);

            //    listpcs.Add(ppcs);
            //}

        }

        private Color getColorbyCount(int p,int max)
        {
            //000000255 000128128 1281280000 255000000
                
            int blue = 255;
            int green =0;
            int red = 0;
            int all = (int)(383 * ((float)p / (float)max));
            for (int i = 0; i < all; i++)
			{
                if (blue>green)
                {
                    green = green + 1;
                    blue = blue - 1;
                }
                else if(blue>0)
                {
                    red = red + 1;
                    blue = blue - 1;
                }
                else
                {
                    green = green - 1;
                    red = red + 1;
                }

			}
             return Color.FromArgb(255, red, green, blue);
        }

        private Point3d CreatePoint3dByLatLon(double lat, double lon, double altoff)
        {
            Point3d pt3d = new Point3d();
            pt3d.X = lon;
            pt3d.Y = lat;
            pt3d.Z = ((useTerrain) ? getElevation(lat, lon) : defaultElev) + altoff;

            return pt3d;
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

            if(!(drawArgs.WorldCamera.AltitudeAboveTerrain>MinAltitude&&drawArgs.WorldCamera.AltitudeAboveTerrain<MaxAltitude))
            {
                return;
            }


            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = false;

            // Check that textures are initialised
            if (!isInitialized)
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

            foreach (CustomVertex.PositionColored[] ppcs in listpcs)
            {
                drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, ppcs.Length - 1, ppcs);
            }

            drawArgs.device.Transform.World = drawArgs.WorldCamera.WorldMatrix;



            if (World.Settings.EnableSunShading) drawArgs.device.RenderState.Lighting = true;
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;

            if (!isOn)
                return;
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;

            if (!isOn)
                return;

        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;

            if (!isOn)
                return;

        }

        public override void MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;

        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (World.Settings.CurrentWwTool != this.drawTool)
                return;
        }
        #endregion

    }
    
}
