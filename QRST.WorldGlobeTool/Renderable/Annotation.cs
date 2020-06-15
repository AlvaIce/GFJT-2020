using System;
using QRST.WorldGlobeTool.Utility;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace QRST.WorldGlobeTool.Renderable
{
    public class Annotation : RenderableObject
    {
        private double m_latitude;
        private double m_longitude;
        private double m_Altitude;
        private Geometries.Point3d m_positionD;
        /// <summary>
        /// 记录了上次的视图矩阵
        /// </summary>
        Matrix lastView = Matrix.Identity;
        private Color m_textColorAtImageMode;
        private Color m_textColorAtMapMode;
        /// <summary>
        /// 最高能见度
        /// </summary>
        private double MaximumDisplayDistance = double.MaxValue;
        /// <summary>
        /// 最低能见度
        /// </summary>
        private double MinimumDisplayDistance;
        const int labelWidth = 1000;


        public Annotation(string name,
            double latitude,
            double longitude,
            Color textColorAtImageMode,
            Color textColorAtMapMode,
            double maximumDisplayDistance,
            double minimumDisplayDistance)
            : base(name)
        {
            //设置经纬度
            m_latitude = latitude;
            m_longitude = longitude;
            m_textColorAtImageMode = textColorAtImageMode;
            m_textColorAtMapMode = textColorAtMapMode;
            MaximumDisplayDistance = maximumDisplayDistance;
            MinimumDisplayDistance = minimumDisplayDistance;
            this.RenderPriority = RenderPriority.Placenames;
        }

        public override void Initialize(DrawArgs drawArgs)
        {
            //计算当前每一度有多少个格网
            double samplesPerDegree = 50.0 / (drawArgs.WorldCamera.ViewRange.Degrees);
            //计算当前经纬度的海拔系信息
            double elevation = 0;
            //计算实际显示高度=夸大因子*Altitude+海拔高度
            double altitude = (World.Settings.VerticalExaggeration * m_Altitude + World.Settings.VerticalExaggeration * elevation);

            //转换为屏幕向量信息
            Position = MathEngine.SphericalToCartesian(m_latitude, m_longitude,
                altitude + drawArgs.WorldCamera.WorldRadius);

            //转换到屏幕坐标
            m_positionD = MathEngine.SphericalToCartesianD(
                Angle.FromDegrees(m_latitude),
                Angle.FromDegrees(m_longitude),
                altitude + drawArgs.WorldCamera.WorldRadius);

            IsInitialized = true;
        }

        public override void Update(DrawArgs drawArgs)
        {
            if (drawArgs.WorldCamera.ViewMatrix != lastView && drawArgs.WorldCamera.Altitude < 300000)
            {
                double samplesPerDegree = 50.0 / drawArgs.WorldCamera.ViewRange.Degrees;
                double elevation = 0;
                double altitude = World.Settings.VerticalExaggeration * m_Altitude + World.Settings.VerticalExaggeration * elevation;
                Position = MathEngine.SphericalToCartesian(m_latitude, m_longitude,
                    altitude + drawArgs.WorldCamera.WorldRadius);

                lastView = drawArgs.WorldCamera.ViewMatrix;
            }
        }

        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            if (!IsInitialized)
                Initialize(drawArgs);

            if (!drawArgs.WorldCamera.ViewFrustum.ContainsPoint(MathEngine.SphericalToCartesian(m_latitude, m_longitude, drawArgs.CurrentWorld.EquatorialRadius)))
                return;

            try
            {
                //计算文字的范围
                Rectangle textRect = drawArgs.DefaultDrawingFont.MeasureString(null, name, DrawTextFormat.None, 0);
                //计算标注的的位置
                Vector3 translationVector = new Vector3(
                (float)(m_positionD.X - drawArgs.WorldCamera.ReferenceCenter.X),
                (float)(m_positionD.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                (float)(m_positionD.Z - drawArgs.WorldCamera.ReferenceCenter.Z));
                Vector3 projectedPoint = drawArgs.WorldCamera.Project(translationVector);
                double distanceToAnnotation = Vector3.Length(this.Position - drawArgs.WorldCamera.Position);
                if (distanceToAnnotation > MaximumDisplayDistance * 3 / 4)
                    return;
                if (distanceToAnnotation < MinimumDisplayDistance)
                    return;
                Rectangle rectAnnotation = new Rectangle(
                    (int)projectedPoint.X - (labelWidth >> 1),
                    (int)(projectedPoint.Y - (drawArgs.DefaultDrawingFont.Description.Height >> 1)),
                    labelWidth, drawArgs.ScreenHeight);
                drawArgs.DefaultDrawingFont.DrawText(null, name, rectAnnotation, DrawTextFormat.Center, is3DMapMode ? m_textColorAtMapMode : m_textColorAtImageMode);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
            }
        }

        public override void Dispose()
        {
        }

    }
}
