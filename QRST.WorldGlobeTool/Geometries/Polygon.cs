using System.Drawing;
using QRST.WorldGlobeTool.Renderable;

namespace QRST.WorldGlobeTool.Geometries
{
    /// <summary>
    /// Summary description for Polygon.
    /// 多边形的基本描述
    /// </summary>
    public class Polygon
    {
        #region  字段
        /// <summary>
        /// 多边形外边框
        /// </summary>
        public LinearRing outerBoundary = null;
        /// <summary>
        /// 外边框列表
        /// </summary>
        public LinearRing[] innerBoundaries = null;
        /// <summary>
        /// 多边形颜色
        /// </summary>
        public Color PolgonColor = Color.Red;
        /// <summary>
        /// 多边形外围线的颜色
        /// </summary>
        public Color OutlineColor = Color.Black;
        /// <summary>
        /// 线的颜色
        /// </summary>
        public float LineWidth = 1.0f;
        /// <summary>
        /// 是否有外围线
        /// </summary>
        public bool Outline = true;
        /// <summary>
        /// 是否填充
        /// </summary>
        public bool Fill = true;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible = true;
        /// <summary>
        /// 是否移动
        /// </summary>
        public bool Remove = false;
        /// <summary>
        /// 可渲染对象
        /// </summary>
        public RenderableObject ParentRenderable = null;

        #endregion

        /// <summary>
        /// 获取几何绘图的外边框
        /// </summary>
        /// <returns></returns>
        public GeographicBoundingBox GetGeographicBoundingBox()
        {
            if (outerBoundary == null ||
                outerBoundary.Points == null ||
                outerBoundary.Points.Length == 0)
                return null;

            double minX = outerBoundary.Points[0].X;
            double maxX = outerBoundary.Points[0].X;
            double minY = outerBoundary.Points[0].Y;
            double maxY = outerBoundary.Points[0].Y;
            double minZ = outerBoundary.Points[0].Z;
            double maxZ = outerBoundary.Points[0].Z;

            for (int i = 1; i < outerBoundary.Points.Length; i++)
            {
                if (outerBoundary.Points[i].X < minX)
                    minX = outerBoundary.Points[i].X;
                if (outerBoundary.Points[i].X > maxX)
                    maxX = outerBoundary.Points[i].X;
                if (outerBoundary.Points[i].Y < minY)
                    minY = outerBoundary.Points[i].Y;
                if (outerBoundary.Points[i].Y > maxY)
                    maxY = outerBoundary.Points[i].Y;
                if (outerBoundary.Points[i].Z < minZ)
                    minZ = outerBoundary.Points[i].Y;
                if (outerBoundary.Points[i].Z > maxZ)
                    maxZ = outerBoundary.Points[i].Y;
            }

            return new GeographicBoundingBox(
                maxY, minY, minX, maxX, minZ, maxZ);
        }
    }
}
