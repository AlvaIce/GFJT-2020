using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace QRST.WorldGlobeTool.Utility
{
    /// <summary>
    /// Defines arrays of colors and positions used for interpolating color blending in a multicolor gradient.
    /// 定义了多个颜色和位置的数组，以实现在渐变色中实现调色插值
    /// </summary>
    public class ColorBlend
    {

        #region 变量

        /// <summary>
        /// 颜色数组
        /// </summary>
        private Color[] _Colors;
        /// <summary>
        /// 位置数组
        /// </summary>
        private float[] _Positions;
        /// <summary>
        /// 最大值
        /// </summary>
        private float _maximum = float.NaN;
        /// <summary>
        /// 最小值
        /// </summary>
        private float _minimum = float.NaN;

        #endregion

        #region 构造函数

        /// <summary>
        /// 内部构造函数
        /// </summary>
        internal ColorBlend()
        {
        }

        /// <summary>
        /// 初始化一个ColorBlend实例
        /// </summary>
        /// <param name="colors">An array of Color structures that represents the colors to use at corresponding positions along a gradient.</param>
        /// <param name="positions">An array of values that specify percentages of distance along the gradient line.</param>
        public ColorBlend(Color[] colors, float[] positions)
        {
            _Colors = colors;
            Positions = positions;
        }

        #endregion

        #region 属性

        /// <summary>
        /// Gets or sets an array of colors that represents the colors to use at corresponding positions along a gradient.
        /// </summary>
        /// <value>An array of <see cref="System.Drawing.Color"/> structures that represents the colors to use at corresponding positions along a gradient.</value>
        /// <remarks>
        /// This property is an array of <see cref="System.Drawing.Color"/> structures that represents the colors to use at corresponding positions
        /// along a gradient. Along with the Positions property, this property defines a multicolor gradient.
        /// </remarks>
        public Color[] Colors
        {
            get { return _Colors; }
            set { _Colors = value; }
        }

        /// <summary>
        /// Gets or sets the positions along a gradient line.
        /// </summary>
        /// <value>An array of values that specify percentages of distance along the gradient line.</value>
        /// <remarks>
        /// <para>The elements of this array specify percentages of distance along the gradient line.
        /// For example, an element value of 0.2f specifies that this point is 20 percent of the total
        /// distance from the starting point. The elements in this array are represented by float
        /// values between 0.0f and 1.0f, and the first element of the array must be 0.0f and the
        /// last element must be 1.0f.</para>
        /// <pre>Along with the Colors property, this property defines a multicolor gradient.</pre>
        /// </remarks>
        public float[] Positions
        {
            get { return _Positions; }
            set
            {
                _Positions = value;
                if (value == null)
                    _minimum = _maximum = float.NaN;
                else
                {
                    _minimum = value[0];
                    _maximum = value[value.GetUpperBound(0)];
                }
            }
        }

        #endregion

        #region 公共方法


        /// <summary>
        /// 根据覆盖次数，显示颜色
        /// </summary>
        /// <param name="p"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Color getColorbyCount(int p, int max)
        {
            //000000255 000128128 1281280000 255000000

            int blue = 255;
            int green = 0;
            int red = 0;
            int all = (int)(383 * ((float)p / (float)max));
            for (int i = 0; i < all; i++)
            {
                if (blue > green)
                {
                    green = green + 1;
                    blue = blue - 1;
                }
                else if (blue > 0)
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

        /// <summary>
        /// 根据输入的位置比例获取对应的颜色值
        /// </summary>
        /// <remarks>如果输入位置值超过了[0..1]刻度范围，那么只截取使用小数部分</remarks>
        /// <param name="pos">刻度尺上的位置（0.0f-1.0f之间）</param>
        /// <returns>刻度尺上的颜色</returns>
        public Color GetColor(float pos)
        {
            if (float.IsNaN(_minimum))
                throw (new ArgumentException("Positions not set"));
            if (_Colors.Length != _Positions.Length)
                throw (new ArgumentException("Colors and Positions arrays must be of equal length"));
            if (_Colors.Length < 2)
                throw (new ArgumentException("At least two colors must be defined in the ColorBlend"));
            /*
            if (_Positions[0] != 0f)
                throw (new ArgumentException("First position value must be 0.0f"));
            if (_Positions[_Positions.Length - 1] != 1f)
                throw (new ArgumentException("Last position value must be 1.0f"));
            
             */
            if (pos > 1 || pos < 0) pos -= (float)Math.Floor(pos);
            int i = 1;
            while (i < _Positions.Length && _Positions[i] < pos)
                i++;
            float frac = (pos - _Positions[i - 1]) / (_Positions[i] - _Positions[i - 1]);
            frac = Math.Max(frac, 0.0f);
            frac = Math.Min(frac, 1.0f);
            int R = (int)Math.Round((_Colors[i - 1].R * (1 - frac) + _Colors[i].R * frac));
            int G = (int)Math.Round((_Colors[i - 1].G * (1 - frac) + _Colors[i].G * frac));
            int B = (int)Math.Round((_Colors[i - 1].B * (1 - frac) + _Colors[i].B * frac));
            int A = (int)Math.Round((_Colors[i - 1].A * (1 - frac) + _Colors[i].A * frac));
            return Color.FromArgb(A, R, G, B);
        }

        /// <summary>
        /// 将当前色带转换为一个线性渐变的画刷
        /// </summary>
        /// <param name="rectangle">矩形框</param>
        /// <param name="angle">角度</param>
        /// <returns>返回一个线性渐变的画刷</returns>
        public LinearGradientBrush ToBrush(Rectangle rectangle, float angle)
        {
            LinearGradientBrush br = new LinearGradientBrush(rectangle, Color.Black, Color.Black, angle, true);
            System.Drawing.Drawing2D.ColorBlend cb = new System.Drawing.Drawing2D.ColorBlend();
            cb.Colors = _Colors;
            //scale and translate positions to range[0.0, 1.0]
            float[] positions = new float[_Positions.Length];
            float range = _maximum - _minimum;
            for (int i = 0; i < _Positions.Length; i++)
                positions[i] = (_Positions[i] - _minimum) / range;
            cb.Positions = positions;
            br.InterpolationColors = cb;
            return br;
        }

        #endregion

        #region 预定义颜色刻度

        /// <summary>
        /// Gets a linear gradient scale with six colours making a rainbow from Blue to Red.
        /// 获取一个从蓝色到红色中间由流种颜色构成的彩虹样式渐变的色带
        /// </summary>
        /// <remarks>
        /// 颜色跨度按照下面的1/5间隔组成
        /// Colors span the following with an interval of 1/5:
        /// { Color.Blue, Color.Aqua, Color.Lime, Color.White, Color.Yellow, Color.Red }
        /// </remarks>
        public static ColorBlend QRSTRainbow6
        {
            get
            {
                return new ColorBlend(
                    new[] { Color.Blue, Color.Aqua, Color.Lime, Color.White, Color.Yellow, Color.Red },
                    new[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f });
            }
        }

        /// <summary>
        /// Gets a linear gradient scale with six colours making a rainbow from Blue to Red.
        /// 获取一个从蓝色到红色中间由流种颜色构成的彩虹样式渐变的色带，不使用0值
        /// </summary>
        /// <remarks>
        /// 颜色跨度按照下面的1/5间隔组成
        /// Colors span the following with an interval of 1/5:
        /// { Color.FromArgb(255, 1, 1, 255),  Color.FromArgb(255, 1, 255, 255),   Color.FromArgb(255, 1, 255, 1),
        ///  Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 255, 1), Color.FromArgb(255, 255, 1, 1) }
        /// </remarks>
        public static ColorBlend QRSTRainbow6WithoutZero
        {
            get
            {
                return new ColorBlend(
                    new[] { Color.FromArgb(255, 1, 1, 255), 
                        Color.FromArgb(255, 1, 255, 255), 
                        Color.FromArgb(255, 1, 255, 1),
                        Color.FromArgb(255, 255, 255, 255),
                        Color.FromArgb(255, 255, 255, 1), 
                        Color.FromArgb(255, 255, 1, 1) },
                    new[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f });
            }
        }

        /// <summary>
        /// Gets a linear gradient scale with seven colours making a rainbow from red to violet.
        /// 获取一个从红色到紫色中间由七种颜色构成的彩虹样式渐变的色带
        /// </summary>
        /// <remarks>
        /// 颜色跨度按照下面的1/6间隔组成
        /// Colors span the following with an interval of 1/6:
        /// { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet }
        /// </remarks>
        public static ColorBlend Rainbow7
        {
            get
            {
                return new ColorBlend(
                    new[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet },
                    new[] { 0 / 6f, 1 / 6f, 2 / 6f, 3 / 6f, 4 / 6f, 5 / 6f, 6 / 6f });
            }
        }

        /// <summary>
        /// Gets a linear gradient scale with five colours making a rainbow from red to blue.
        /// 获取一个从红色到蓝色中间由五种颜色构成的彩虹样式渐变的色带
        /// </summary>
        /// <remarks>
        /// 颜色跨度按照下面的0.25间隔组成
        /// Colors span the following with an interval of 0.25:
        /// { Color.Red, Color.Yellow, Color.Green, Color.Cyan, Color.Blue }
        /// </remarks>
        public static ColorBlend Rainbow5
        {
            get
            {
                return new ColorBlend(
                    new[] { Color.Red, Color.Yellow, Color.Green, Color.Cyan, Color.Blue },
                    new[] { 0f, 0.25f, 0.5f, 0.75f, 1f });
            }
        }

        /// <summary>
        /// 获取一个从黑色到白色渐变的色带
        /// </summary>
        public static ColorBlend BlackToWhite
        {
            get { return new ColorBlend(new[] { Color.Black, Color.White }, new[] { 0f, 1f }); }
        }

        /// <summary>
        /// 获取一个从白色到黑色渐变的色带
        /// </summary>
        public static ColorBlend WhiteToBlack
        {
            get { return new ColorBlend(new[] { Color.White, Color.Black }, new[] { 0f, 1f }); }
        }

        /// <summary>
        /// 获取一个从红色到绿色渐变的色带
        /// </summary>
        public static ColorBlend RedToGreen
        {
            get { return new ColorBlend(new[] { Color.Red, Color.Green }, new[] { 0f, 1f }); }
        }

        /// <summary>
        /// 获取一个从绿色到红色渐变的色带
        /// </summary>
        public static ColorBlend GreenToRed
        {
            get { return new ColorBlend(new[] { Color.Green, Color.Red }, new[] { 0f, 1f }); }
        }

        /// <summary>
        /// 获取一个从蓝色到绿色渐变的色带
        /// </summary>
        public static ColorBlend BlueToGreen
        {
            get { return new ColorBlend(new[] { Color.Blue, Color.Green }, new[] { 0f, 1f }); }
        }

        /// <summary>
        /// 获取一个从绿色到蓝色渐变的色带
        /// </summary>
        public static ColorBlend GreenToBlue
        {
            get { return new ColorBlend(new[] { Color.Green, Color.Blue }, new[] { 0f, 1f }); }
        }

        /// <summary>
        /// 获取一个从红色到蓝色渐变的色带
        /// </summary>
        public static ColorBlend RedToBlue
        {
            get { return new ColorBlend(new[] { Color.Red, Color.Blue }, new[] { 0f, 1f }); }
        }

        /// <summary>
        /// 获取一个从蓝色到红色渐变的色带
        /// </summary>
        public static ColorBlend BlueToRed
        {
            get { return new ColorBlend(new[] { Color.Blue, Color.Red }, new[] { 0f, 1f }); }
        }

        #endregion

        #region 有用的方法

        /// <summary>
        /// 根据输入的两个颜色创建一个线性渐变的色带
        /// </summary>
        /// <param name="fromColor">起始颜色</param>
        /// <param name="toColor">结束颜色</param>
        /// <returns>返回一个色带</returns>
        public static ColorBlend TwoColors(Color fromColor, Color toColor)
        {
            return new ColorBlend(new[] { fromColor, toColor }, new[] { 0f, 1f });
        }

        /// <summary>
        /// 根据输入的三个颜色创建一个线性渐变的色带
        /// </summary>
        /// <param name="fromColor">起始颜色</param>
        /// <param name="middleColor">中间颜色</param>
        /// <param name="toColor">结束颜色</param>
        /// <returns>返回一个色带</returns>
        public static ColorBlend ThreeColors(Color fromColor, Color middleColor, Color toColor)
        {
            return new ColorBlend(new[] { fromColor, middleColor, toColor }, new[] { 0f, 0.5f, 1f });
        }

        #endregion
    }
}
