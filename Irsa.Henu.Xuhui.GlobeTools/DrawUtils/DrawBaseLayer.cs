using System;
using System.Collections.Generic;
using System.Text;
using WorldWind;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using WorldWind.PluginEngine;
using Qrst.Renderable;
using Qrst;

namespace DrawTools.Plugins
{
    public abstract class DrawBaseLayer : RenderableObject
    {
        public enum DrawState
        {
            Idle,       //clear Graphic
            Drawing,
            Complete
        }

        public DrawState State;
        public Color color;
        public  DrawArgs drawArgs;
        protected DrawBaseTool drawTool;
        protected Point mouseDownPoint;
        public double defaultElev = 0;
        public bool useTerrain;
        public double elevateSamplifyRate = 1024;
        public bool isPointGotoEnabled;  //不响应WorldWind的鼠标点击缩放浏览事件

        public List<Point3d> PointList;
        public List<CustomVertex.PositionColored> VertexList;
        protected CustomVertex.PositionColored[] pcs;
        protected Point3d movingPt;

        public DrawBaseLayer(string pName, Color pColor, World pWorld, DrawArgs drawArgs)
            : base(pName, pWorld)
        {
            this.color = pColor;
            this.drawArgs = drawArgs;
            this.State = DrawState.Idle;
            drawTool = null;
            useTerrain = false;
        }

        public DrawBaseLayer(string pName, Color pColor, DrawBaseTool drawTool, DrawArgs drawArgs)
            : this(pName,pColor,drawTool.ParentApplication.CurrentWorld,drawArgs)
        {
            this.drawTool = drawTool;
        }

        //public DrawBaseLayer(string name, WorldWind.World parentWorld, Microsoft.DirectX.Quaternion orientation)
        //    : base(name, parentWorld, orientation)
        //{ }

        public double getElevation(double lat, double lon)
        {
            double elevation = 0;
            elevation = World.TerrainAccessor.GetElevationAt(lat, lon, elevateSamplifyRate);
            return elevation;
        }

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

        public List<CustomVertex.PositionColored> getCurveFromPoints(Point3d startPt, Point3d endPt, Color color)
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

        #region Events
        public abstract void MouseDown(object sender, MouseEventArgs e);

        public abstract void MouseUp(object sender, MouseEventArgs e);

        // Double click event
        public abstract void MouseDoubleClick(object sender, MouseEventArgs e);

        public abstract void MouseMove(object sender, MouseEventArgs e);

        public abstract void KeyUp(object sender, KeyEventArgs e);

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

        #region Override

        public override bool IsOn
        {
            get
            {
                return base.IsOn;
            }
            set
            {
                //if (value == isOn)
                //    return;
                //base.IsOn = value;
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

        public override void Initialize(DrawArgs drawArgs)
        {
            VertexList = new List<CustomVertex.PositionColored>();
            PointList = new List<Point3d>();
            movingPt = new Point3d();
            movingPt.IsNaN = true;
        }

        public override void Dispose()
        {
            PointList = null;
            VertexList = null;
            movingPt = null;        
        }

        #endregion

    }
}
