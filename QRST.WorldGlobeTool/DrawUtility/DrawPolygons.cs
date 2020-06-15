using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Collections;
using QRST.WorldGlobeTool.Geometries;
using QRST.WorldGlobeTool.Renderable;

namespace QRST.WorldGlobeTool.DrawUtility
{
    /// <summary>
    /// 绘制多边形
    /// </summary>
    public class DrawPolygons
    {

        #region  字段

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
        /// 距离表面海拔高度
        /// </summary>
        public double DistanceAboveSurface = 0;
        /// <summary>
        /// 填充影像的尺寸
        /// </summary>
        public Size FillImageSize;
        /// <summary>
        /// 图层所属的世界
        /// </summary>
        public World m_world;
        /// <summary>
        /// 影像图层
        /// </summary>
        ImageLayer imagelyr;
        /// <summary>
        /// 图层是否可见
        /// </summary>
        public bool IsOn;
        /// <summary>
        /// 多边形列表
        /// </summary>
        Polygon[] m_Polygons;
        /// <summary>
        /// 几何绘制外边框
        /// </summary>
        public GeographicBoundingBox m_GeoBBox;

        #endregion

        #region  绘制多边形

        /// <summary>
        /// 初始化一个DrawPolygons实例
        /// </summary>
        /// <param name="polygons">多边形数组</param>
        /// <param name="world">世界</param>
        public DrawPolygons(Polygon[] polygons, World world)
        {
            m_Polygons = new Polygon[polygons.Length];
            polygons.CopyTo(m_Polygons, 0);
            m_world = world;
            FillImageSize = new Size(300, 300);

            GetGeographicBoundingBox();
            imagelyr = FillPolygon();
            IsOn = true;
        }

        /// <summary>
        /// 初始化一个DrawPolygons实例
        /// </summary>
        /// <param name="polygons">多边形列表</param>
        /// <param name="world">世界</param>
        public DrawPolygons(List<Polygon> polygons, World world)
        {
            m_Polygons = new Polygon[polygons.Count];
            polygons.CopyTo(m_Polygons, 0);
            m_world = world;
            FillImageSize = new Size(300, 300);

            GetGeographicBoundingBox();
            imagelyr = FillPolygon();
            IsOn = true;
        }

        /// <summary>
        /// 初始化一个DrawPolygons实例
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <param name="world">世界</param>
        public DrawPolygons(Polygon polygon, World world)
        {
            m_Polygons = new Polygon[1];
            m_Polygons[0] = polygon;
            m_world = world;
            FillImageSize = new Size(300, 300);

            GetGeographicBoundingBox();
            imagelyr = FillPolygon();
            IsOn = true;
        }

        #endregion

        /// <summary>
        /// 渲染当前图层
        /// </summary>
        /// <param name="drawArgs"></param>
        public void Render(DrawArgs drawArgs)
        {
            if (imagelyr == null)
            {
                return;
            }

            if (!IsOn)
            {
                return;
            }

            if (!imagelyr.Initialized)
            {
                imagelyr.Initialize(drawArgs);
            }
            imagelyr.Render(drawArgs);
        }

        /// <summary>
        /// 获取几何绘制外边框
        /// </summary>
        protected void GetGeographicBoundingBox()
        {
            if (m_Polygons == null ||
                m_Polygons.Length < 1)
                return;

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;

            m_GeoBBox = new GeographicBoundingBox(maxY, minY, minX, maxX, minZ, maxZ);


            for (int i = 0; i < m_Polygons.Length; i++)
            {
                Polygon plg = m_Polygons[i];
                GeographicBoundingBox plgGBB = plg.GetGeographicBoundingBox();
                m_GeoBBox.ExtentGeoBBox(plgGBB);
            }
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <returns></returns>
        protected ImageLayer FillPolygon()
        {
            //create polygon imagestream
            ImageLayer imageLayer = null;

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

            for (int idxPlg = 0; idxPlg < m_Polygons.Length; idxPlg++)
            {
                Polygon polygon = m_Polygons[idxPlg];
                //GeographicBoundingBox gbox = polygon.GetGeographicBoundingBox();

                #region Fill innerBounderies
                if (polygon.innerBoundaries != null && polygon.innerBoundaries.Length > 0)
                {
                    if (polygon.Fill)
                    {
                        for (int i = 0; i < polygon.innerBoundaries.Length; i++)
                        {
                            //挖内环,绘制不规则多边形或图片
                            byte[] bytes = new byte[polygon.innerBoundaries[i].Points.Length];
                            for (int ii = 0; ii < polygon.innerBoundaries[i].Points.Length; ii++)
                            {
                                if (ii == 0)
                                {
                                    bytes[ii] = (byte)System.Drawing.Drawing2D.PathPointType.Start;
                                }
                                else if (ii == polygon.innerBoundaries[i].Points.Length - 1)
                                {

                                    bytes[ii] = (byte)(System.Drawing.Drawing2D.PathPointType.Line | System.Drawing.Drawing2D.PathPointType.CloseSubpath);
                                }
                                else
                                {
                                    bytes[ii] = (byte)System.Drawing.Drawing2D.PathPointType.Line;
                                }
                            }
                            g.ExcludeClip(new Region(new System.Drawing.Drawing2D.GraphicsPath(getScreenPoints(polygon.innerBoundaries[i].Points, 0, polygon.innerBoundaries[i].Points.Length, m_GeoBBox, b.Size), bytes)));
                        }

                    }
                    if (polygon.Fill)
                    {
                        using (SolidBrush brush = new SolidBrush(polygon.PolgonColor))
                        {
                            g.FillPolygon(brush, getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, m_GeoBBox, b.Size));
                        }
                    }

                    if (polygon.Outline)
                    {
                        using (Pen p = new Pen(polygon.OutlineColor, polygon.LineWidth))
                        {
                            g.DrawPolygon(p,
                                    getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, m_GeoBBox, b.Size));

                            for (int i = 0; i < polygon.innerBoundaries.Length; i++)
                            {
                                g.DrawPolygon(p,
                                        getScreenPoints(polygon.innerBoundaries[i].Points, 0, polygon.innerBoundaries[i].Points.Length, m_GeoBBox, b.Size));
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
                            g.FillPolygon(brush, getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, m_GeoBBox, b.Size));
                        }
                    }

                    if (polygon.Outline)
                    {
                        using (Pen p = new Pen(polygon.OutlineColor, polygon.LineWidth))
                        {
                            g.DrawPolygon(p, getScreenPoints(polygon.outerBoundary.Points, 0, polygon.outerBoundary.Points.Length, m_GeoBBox, b.Size));
                        }
                    }
                }
                #endregion

            }

            #region Create ImageLayer
            string id = System.DateTime.Now.Ticks.ToString();

            if (b != null)
            {
                MemoryStream ms = new MemoryStream();
                b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                //must copy original stream into new stream, if not, error occurs, not sure why
                MemoryStream m_ImageStream = new MemoryStream(ms.GetBuffer());

                imageLayer = new ImageLayer(
                    id,
                    m_world,
                    DistanceAboveSurface,
                    m_ImageStream,
                    System.Drawing.Color.Black.ToArgb(),
                    (float)m_GeoBBox.South,
                    (float)m_GeoBBox.North,
                    (float)m_GeoBBox.West,
                    (float)m_GeoBBox.East,
                    1.0f,//(float)m_parentProjectedLayer.Opacity / 255.0f                    
                    m_world.TerrainAccessor);

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

        /// <summary>
        /// 获取窗口的点
        /// </summary>
        /// <param name="sourcePoints"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="dstBB"></param>
        /// <param name="dstImageSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (imagelyr != null)
            {
                imagelyr.Dispose();
                imagelyr = null;
            }

            if (m_Polygons != null)
            {
                m_Polygons = null;
            }
            System.GC.Collect();
        }

    }
}
