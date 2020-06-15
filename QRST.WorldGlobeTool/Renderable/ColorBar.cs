using System.Drawing;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Utility;

namespace QRST.WorldGlobeTool.Renderable
{
    /// <summary>
    /// 颜色带
    /// </summary>
    public class ColorBar : RenderableObject
    {

        /// <summary>
        /// 颜色带代表的最小值
        /// </summary>
        private double m_MinValue;
        /// <summary>
        /// 颜色带代表的最大值
        /// </summary>
        private double m_MaxValue;
        /// <summary>
        /// 色带值的类型
        /// </summary>
        private ColorBarValueType m_ValueType;
        /// <summary>
        /// 获取或设置色带值的类型
        /// </summary>
        public ColorBarValueType ValueType
        {
            get { return m_ValueType; }
            set { m_ValueType = value; }
        }
        /// <summary>
        /// 颜色带标题字体
        /// </summary>
        private Microsoft.DirectX.Direct3D.Font m_TitleFont;
        /// <summary>
        /// 颜色带文字字体
        /// </summary>
        private Microsoft.DirectX.Direct3D.Font m_ColorTextFont;
        /// <summary>
        /// 颜色带位置
        /// </summary>
        private Point m_Location;
        /// <summary>
        /// 获取或设置颜色带位置
        /// </summary>
        public Point Location
        {
            get { return m_Location; }
            set { m_Location = value; }
        }
        /// <summary>
        /// 颜色带宽度
        /// </summary>
        private int m_Width;
        /// <summary>
        /// 获取或设置颜色带宽度
        /// </summary>
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }
        /// <summary>
        /// 颜色带高度
        /// </summary>
        private int m_Height;
        /// <summary>
        /// 获取或设置颜色带高度
        /// </summary>
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }
        /// <summary>
        /// 色带停靠位置
        /// </summary>
        private ColorBarAnchor m_Anchor = ColorBarAnchor.LeftBottom;
        /// <summary>
        /// 获取或设置色带停靠位置
        /// </summary>
        public ColorBarAnchor Anchor
        {
            get { return m_Anchor; }
            set { m_Anchor = value; }
        }
        /// <summary>
        /// 颜色带的标题
        /// </summary>
        private string m_ColorBarTitle;
        /// <summary>
        /// 获取或设置色带标题
        /// </summary>
        public string ColorBarTitle
        {
            get { return m_ColorBarTitle; }
            set { m_ColorBarTitle = value; }
        }
        /// <summary>
        /// 当前色带是否显示中间值
        /// </summary>
        private bool m_IsShowMiddleValue = true;
        /// <summary>
        /// 获取或设置当前色带是否显示中间值
        /// </summary>
        public bool IsShowMiddleValue
        {
            get { return m_IsShowMiddleValue; }
            set { m_IsShowMiddleValue = value; }
        }
        /// <summary>
        /// 与色带图层相关联的色带数组
        /// </summary>
        private ColorBlend m_ColorBarColorBlend = ColorBlend.QRSTRainbow6;
        /// <summary>
        /// 获取或设置与色带图层相关联的色带数组
        /// </summary>
        public ColorBlend ColorBarColorBlend
        {
            get { return m_ColorBarColorBlend; }
            set { m_ColorBarColorBlend = value; }
        }

        /// <summary>
        /// 初始化一个颜色带绘制类
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="minValue">颜色带代表的最小值</param>
        /// <param name="maxValue">颜色带代表的最大值</param>
        public ColorBar(string layerName, double minValue, double maxValue)
            : base(layerName)
        {
            m_MinValue = minValue;
            m_MaxValue = maxValue;
        }

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            setTextFont(drawArgs);
            setColorBarLocation(drawArgs);
            IsInitialized = true;
        }

        public override void Update(DrawArgs drawArgs)
        {
            setTextFont(drawArgs);
            setColorBarLocation(drawArgs);
        }

        /// <summary>
        /// 渲染三维球体
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            if (!IsInitialized)
                Initialize(drawArgs);


            #region 绘制色带

            CustomVertex.TransformedColored[] lineVerts;  //线段顶点列表
            switch (m_Anchor)
            {
                case ColorBarAnchor.Left:
                case ColorBarAnchor.LeftTop:
                case ColorBarAnchor.LeftBottom:
                case ColorBarAnchor.Right:
                case ColorBarAnchor.RightTop:
                case ColorBarAnchor.RightBottom:
                    for (int i = 1; i < m_Height; i++)
                    {
                        lineVerts = new CustomVertex.TransformedColored[2];
                        lineVerts[0].X = m_Location.X;
                        lineVerts[0].Y = m_Location.Y + m_Height - i;
                        lineVerts[0].Z = 0.0f;
                        lineVerts[0].Color = m_ColorBarColorBlend.GetColor(i / (float)m_Height).ToArgb();

                        lineVerts[1].X = m_Location.X + m_Width;
                        lineVerts[1].Y = lineVerts[0].Y;
                        lineVerts[1].Z = 0.0f;
                        lineVerts[1].Color = lineVerts[0].Color;
                        drawArgs.device.TextureState[0].ColorOperation = TextureOperation.Disable;
                        drawArgs.device.VertexFormat = CustomVertex.TransformedColored.Format;
                        drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, lineVerts.Length - 1, lineVerts);
                    }
                    break;
                case ColorBarAnchor.Top:
                case ColorBarAnchor.TopLeft:
                case ColorBarAnchor.TopRight:
                case ColorBarAnchor.Bottom:
                case ColorBarAnchor.BottomLeft:
                case ColorBarAnchor.BottomRight:
                    for (int i = 1; i < m_Width; i++)
                    {
                        lineVerts = new CustomVertex.TransformedColored[2];
                        lineVerts[0].X = m_Location.X + i;
                        lineVerts[0].Y = m_Location.Y;
                        lineVerts[0].Z = 0.0f;
                        lineVerts[0].Color = m_ColorBarColorBlend.GetColor(i / (float)m_Width).ToArgb();

                        lineVerts[1].X = lineVerts[0].X;
                        lineVerts[1].Y = m_Location.Y + m_Height;
                        lineVerts[1].Z = 0.0f;
                        lineVerts[1].Color = lineVerts[0].Color;
                        drawArgs.device.TextureState[0].ColorOperation = TextureOperation.Disable;
                        drawArgs.device.VertexFormat = CustomVertex.TransformedColored.Format;
                        drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, lineVerts.Length - 1, lineVerts);
                    }
                    break;
            }

            #endregion

            #region 绘制文字

            Rectangle colorTextRectangle;
            DrawTextFormat minValueFormat, midValueFormat, maxValueFormat;
            switch (m_Anchor)
            {
                case ColorBarAnchor.Left:
                case ColorBarAnchor.LeftTop:
                case ColorBarAnchor.LeftBottom:
                default:
                    colorTextRectangle = new Rectangle(m_Location.X + m_Width + 2, m_Location.Y,
                        drawArgs.ScreenWidth, m_Height);
                    minValueFormat = DrawTextFormat.Left | DrawTextFormat.Bottom;
                    midValueFormat = DrawTextFormat.Left | DrawTextFormat.VerticalCenter;
                    maxValueFormat = DrawTextFormat.Left | DrawTextFormat.Top;
                    break;
                case ColorBarAnchor.Right:
                case ColorBarAnchor.RightTop:
                case ColorBarAnchor.RightBottom:
                    colorTextRectangle = new Rectangle(0, m_Location.Y,
                        drawArgs.ScreenWidth - m_Width - 3, m_Height);
                    minValueFormat = DrawTextFormat.Right | DrawTextFormat.Bottom;
                    midValueFormat = DrawTextFormat.Right | DrawTextFormat.VerticalCenter;
                    maxValueFormat = DrawTextFormat.Right | DrawTextFormat.Top;
                    break;
                case ColorBarAnchor.Top:
                case ColorBarAnchor.TopLeft:
                case ColorBarAnchor.TopRight:
                    colorTextRectangle = new Rectangle(m_Location.X, m_Location.Y + m_Height + 2,
                        m_Width, drawArgs.ScreenHeight);
                    minValueFormat = DrawTextFormat.Left | DrawTextFormat.Top;
                    midValueFormat = DrawTextFormat.Center | DrawTextFormat.Top;
                    maxValueFormat = DrawTextFormat.Right | DrawTextFormat.Top;
                    break;
                case ColorBarAnchor.Bottom:
                case ColorBarAnchor.BottomLeft:
                case ColorBarAnchor.BottomRight:
                    colorTextRectangle = new Rectangle(m_Location.X, 0,
                        m_Width, drawArgs.ScreenHeight - m_Height - 3);
                    minValueFormat = DrawTextFormat.Left | DrawTextFormat.Bottom;
                    midValueFormat = DrawTextFormat.Center | DrawTextFormat.Bottom;
                    maxValueFormat = DrawTextFormat.Right | DrawTextFormat.Bottom;
                    break;
            }
            m_ColorTextFont.DrawText(null, m_MaxValue.ToString(), colorTextRectangle, maxValueFormat, m_ColorBarColorBlend.GetColor(1.0f).ToArgb());
            m_ColorTextFont.DrawText(null, m_MinValue.ToString(), colorTextRectangle, minValueFormat, m_ColorBarColorBlend.GetColor(0.0f).ToArgb());
            if (m_IsShowMiddleValue)
            {
                string midText = m_ValueType == ColorBarValueType.整型 ? (((long)m_MaxValue + (long)m_MinValue) / 2).ToString() : (((double)m_MaxValue + (double)m_MinValue) / 2.0).ToString("0.00");
                m_ColorTextFont.DrawText(null, midText, colorTextRectangle, midValueFormat, m_ColorBarColorBlend.GetColor(0.5f).ToArgb());
            }

            //绘制标题
            if (m_ColorBarTitle != "")
            {
                Rectangle titleRectangle = m_TitleFont.MeasureString(null, m_ColorBarTitle, DrawTextFormat.Right, Color.White);
                titleRectangle.X = 0;
                titleRectangle.Y = m_Location.Y;
                titleRectangle.Width = drawArgs.ScreenWidth;
                m_TitleFont.DrawText(null, m_ColorBarTitle, titleRectangle, DrawTextFormat.Right, is3DMapMode ? Color.DeepSkyBlue : Color.FloralWhite);
            }

            #endregion
        }

        public override void Dispose()
        {
            
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="drawArgs"></param>
        private void setTextFont(DrawArgs drawArgs)
        {
            FontDescription description = new FontDescription();
            description.FaceName = "微软雅黑";
            int height = (int)(drawArgs.ScreenHeight / 20);
            height = height < 20 ? 20 : height;
            description.Height = height;
            description.Weight = FontWeight.Bold;
            m_TitleFont = drawArgs.CreateFont(description);
            m_ColorTextFont = drawArgs.CreateFont(description);
        }

        /// <summary>
        /// 设置色带左上角位置
        /// </summary>
        /// <param name="drawArgs"></param>
        private void setColorBarLocation(DrawArgs drawArgs)
        {
            switch (m_Anchor)
            {
                case ColorBarAnchor.Left:
                    m_Location = new Point(1, 1);
                    m_Height = drawArgs.ScreenHeight - 2;
                    break;
                case ColorBarAnchor.LeftTop:
                    m_Location = new Point(1, 1);
                    break;
                case ColorBarAnchor.LeftBottom:
                    m_Location = new Point(1, drawArgs.ScreenHeight - m_Height - 1);
                    break;
                case ColorBarAnchor.Top:
                    m_Location = new Point(1, 1);
                    m_Width = drawArgs.ScreenWidth - 2;
                    break;
                case ColorBarAnchor.TopLeft:
                    m_Location = new Point(1, 1);
                    break;
                case ColorBarAnchor.TopRight:
                    m_Location = new Point(drawArgs.ScreenWidth - m_Width - 1, 1);
                    break;
                case ColorBarAnchor.Right:
                    m_Location = new Point(drawArgs.ScreenWidth - m_Width - 1, 1);
                    m_Height = drawArgs.ScreenHeight - 2;
                    break;
                case ColorBarAnchor.RightTop:
                    m_Location = new Point(drawArgs.ScreenWidth - m_Width - 1, 1);
                    break;
                case ColorBarAnchor.RightBottom:
                    m_Location = new Point(drawArgs.ScreenWidth - m_Width - 1, drawArgs.ScreenHeight - m_Height - 1);
                    break;
                case ColorBarAnchor.Bottom:
                    m_Location = new Point(1, drawArgs.ScreenHeight - m_Height - 1);
                    m_Width = drawArgs.ScreenWidth - 2;
                    break;
                case ColorBarAnchor.BottomLeft:
                    m_Location = new Point(1, drawArgs.ScreenHeight - m_Height - 1);
                    break;
                case ColorBarAnchor.BottomRight:
                    m_Location = new Point(drawArgs.ScreenWidth - m_Width - 1, drawArgs.ScreenHeight - m_Height - 1);
                    break;
            }
        }

        #endregion

    }

    /// <summary>
    /// 色带停靠方式
    /// </summary>
    public enum ColorBarAnchor
    {
        /// <summary>
        /// 停靠窗口左侧，从上到下绘制
        /// </summary>
        Left,
        /// <summary>
        /// 停靠窗口左上角，从上到下绘制
        /// </summary>
        LeftTop,
        /// <summary>
        /// 停靠窗口左下角，从上到下绘制
        /// </summary>
        LeftBottom,
        /// <summary>
        /// 停靠窗口顶部，从左到右绘制
        /// </summary>
        Top,
        /// <summary>
        /// 停靠窗口顶部靠左，从左到右绘制
        /// </summary>
        TopLeft,
        /// <summary>
        /// 停靠窗口顶部靠右，从左到右绘制
        /// </summary>
        TopRight,
        /// <summary>
        /// 停靠窗口右侧，从上到下绘制
        /// </summary>
        Right,
        /// <summary>
        /// 停靠窗口右上角，从上到下绘制
        /// </summary>
        RightTop,
        /// <summary>
        /// 停靠窗口右下角，从上到下绘制
        /// </summary>
        RightBottom,
        /// <summary>
        /// 停靠窗口底部，从左到右绘制
        /// </summary>
        Bottom,
        /// <summary>
        /// 停靠窗口底部靠左，从左到右绘制
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 停靠窗口底部靠右，从左到右绘制
        /// </summary>
        BottomRight
    }

    /// <summary>
    /// 色带值的类型
    /// </summary>
    public enum ColorBarValueType
    {
        /// <summary>
        /// 包含byte、short、int和long等类型数据
        /// </summary>
        整型,
        /// <summary>
        /// 包含decimal、float和double类型数据
        /// </summary>
        浮点型
    }

}
