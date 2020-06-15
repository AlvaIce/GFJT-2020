using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 菜单有用工具类
    /// </summary>
    public sealed class MenuUtils
    {

        /// <summary>
        /// 私有构造函数，防止被初始化
        /// </summary>
        private MenuUtils() { }

        /// <summary>
        /// 绘制线条
        /// </summary>
        /// <param name="linePoints">线条顶点数组</param>
        /// <param name="color">线条颜色</param>
        /// <param name="device">设备</param>
        public static void DrawLine(Vector2[] linePoints, int color, Device device)
        {
            CustomVertex.TransformedColored[] lineVerts = new CustomVertex.TransformedColored[linePoints.Length];

            for (int i = 0; i < linePoints.Length; i++)
            {
                lineVerts[i].X = linePoints[i].X;
                lineVerts[i].Y = linePoints[i].Y;
                lineVerts[i].Z = 0.0f;

                lineVerts[i].Color = color;
            }

            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.VertexFormat = CustomVertex.TransformedColored.Format;

            device.DrawUserPrimitives(PrimitiveType.LineStrip, lineVerts.Length - 1, lineVerts);
        }

        /// <summary>
        /// 绘制盒子
        /// </summary>
        /// <param name="ulx">左上角X坐标</param>
        /// <param name="uly">左上角Y坐标</param>
        /// <param name="width">盒子宽度</param>
        /// <param name="height">盒子高度</param>
        /// <param name="z">Z方向高度值</param>
        /// <param name="color">线条颜色</param>
        /// <param name="device">设备</param>
        public static void DrawBox(int ulx, int uly, int width, int height, float z, int color, Device device)
        {
            CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[4];
            verts[0].X = (float)ulx;
            verts[0].Y = (float)uly;
            verts[0].Z = z;
            verts[0].Color = color;

            verts[1].X = (float)ulx;
            verts[1].Y = (float)uly + height;
            verts[1].Z = z;
            verts[1].Color = color;

            verts[2].X = (float)ulx + width;
            verts[2].Y = (float)uly;
            verts[2].Z = z;
            verts[2].Color = color;

            verts[3].X = (float)ulx + width;
            verts[3].Y = (float)uly + height;
            verts[3].Z = z;
            verts[3].Color = color;

            device.VertexFormat = CustomVertex.TransformedColored.Format;
            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, verts.Length - 2, verts);
        }

        /// <summary>
        /// 绘制扇形区域
        /// </summary>
        /// <param name="startAngle">起始角度</param>
        /// <param name="endAngle">结束角度</param>
        /// <param name="centerX">中心点X坐标</param>
        /// <param name="centerY">中心点Y坐标</param>
        /// <param name="radius">扇形半径</param>
        /// <param name="z">Z方向高度值</param>
        /// <param name="color">线条颜色</param>
        /// <param name="device">设备</param>
        public static void DrawSector(double startAngle, double endAngle, int centerX, int centerY, int radius, float z, int color, Device device)
        {
            int prec = 7;

            CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[prec + 2];
            verts[0].X = centerX;
            verts[0].Y = centerY;
            verts[0].Z = z;
            verts[0].Color = color;
            double angleInc = (double)(endAngle - startAngle) / prec;

            for (int i = 0; i <= prec; i++)
            {
                verts[i + 1].X = (float)Math.Cos((double)(startAngle + angleInc * i)) * radius + centerX;
                verts[i + 1].Y = (float)Math.Sin((double)(startAngle + angleInc * i)) * radius * (-1.0f) + centerY;
                verts[i + 1].Z = z;
                verts[i + 1].Color = color;
            }

            device.VertexFormat = CustomVertex.TransformedColored.Format;
            device.TextureState[0].ColorOperation = TextureOperation.Disable;
            device.DrawUserPrimitives(PrimitiveType.TriangleFan, verts.Length - 2, verts);
        }

    }
}
