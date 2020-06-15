using System;
using Microsoft.DirectX;
using System.Collections;
using System.Globalization;
using QRST.WorldGlobeTool.Utility;

namespace QRST.WorldGlobeTool
{
    /// <summary>
    /// 角度结构体
    /// </summary>
    public struct Angle
    {
        /// <summary>
        /// 弧度单位
        /// </summary>
        [NonSerialized]
        public double Radians;

        /// <summary>
        /// 从弧度转换为角度
        /// </summary>
        public static Angle FromRadians(double radians)
        {
            Angle res = new Angle();
            res.Radians = radians;
            return res;
        }

        /// <summary>
        /// 创建一个角度对象
        /// </summary>
        public static Angle FromDegrees(double degrees)
        {
            Angle res = new Angle();
            res.Radians = Math.PI * degrees / 180.0;
            return res;
        }

        /// <summary>
        /// 单位零角度
        /// </summary>
        public static readonly Angle Zero;

        /// <summary>
        /// 最小角度
        /// </summary>
        public static readonly Angle MinValue = Angle.FromRadians(double.MinValue);

        /// <summary>
        /// 最大角度
        /// </summary>
        public static readonly Angle MaxValue = Angle.FromRadians(double.MaxValue);

        /// <summary>
        /// 错误的角度
        /// </summary>
        public static readonly Angle NaN = Angle.FromRadians(double.NaN);

        public double Degrees
        {
            get { return MathEngine.RadiansToDegrees(this.Radians); }
            set { this.Radians = MathEngine.DegreesToRadians(value); }
        }


        /// <summary>
        /// 返回角度的绝对值角度，即-角度变+角度。
        /// </summary>
        public static Angle Abs(Angle a)
        {
            return Angle.FromRadians(Math.Abs(a.Radians));
        }

        /// <summary>
        /// 判断角度是否为NaN
        /// </summary>
        public static bool IsNaN(Angle a)
        {
            return double.IsNaN(a.Radians);
        }

        /// <summary>
        /// 判断两个角度是否相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Angle a = (Angle)obj;
            return Math.Abs(Radians - a.Radians) < Single.Epsilon;
        }
        /// <summary>
        /// 用==判断两个角度是否相同
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Angle a, Angle b)
        {
            return Math.Abs(a.Radians - b.Radians) < Single.Epsilon;
        }
        /// <summary>
        /// 用!=判断两个角度不相同
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Angle a, Angle b)
        {
            return Math.Abs(a.Radians - b.Radians) > Single.Epsilon;
        }
        /// <summary>
        /// 判断角度a是否小于角度b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(Angle a, Angle b)
        {
            return a.Radians < b.Radians;
        }
        /// <summary>
        /// 判断角度a是否大于角度b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(Angle a, Angle b)
        {
            return a.Radians > b.Radians;
        }
        /// <summary>
        /// 角度a与角度b相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Angle operator +(Angle a, Angle b)
        {
            double res = a.Radians + b.Radians;
            return Angle.FromRadians(res);
        }
        /// <summary>
        /// 角度a与角度b相减
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Angle operator -(Angle a, Angle b)
        {
            double res = a.Radians - b.Radians;
            return Angle.FromRadians(res);
        }
        /// <summary>
        /// 角度a与系数相乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static Angle operator *(Angle a, double times)
        {
            return Angle.FromRadians(a.Radians * times);
        }
        /// <summary>
        /// 系数与角度a相乘
        /// </summary>
        /// <param name="times"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Angle operator *(double times, Angle a)
        {
            return Angle.FromRadians(a.Radians * times);
        }
        /// <summary>
        /// 系数与角度a相除
        /// </summary>
        /// <param name="divisor"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Angle operator /(double divisor, Angle a)
        {
            return Angle.FromRadians(a.Radians / divisor);
        }
        /// <summary>
        /// 角度a与系数相除
        /// </summary>
        /// <param name="a"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static Angle operator /(Angle a, double divisor)
        {
            return Angle.FromRadians(a.Radians / divisor);
        }
        /// <summary>
        /// 获得当前角度的HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(Radians * 100000);
        }

        /// <summary>
        /// 序列化角度对象，使角度变化在-360到+360之间
        /// </summary>
        public void Normalize()
        {
            if (Radians > Math.PI * 2)
                Radians -= Math.PI * 2;
            if (Radians < -Math.PI * 2)
                Radians += Math.PI * 2;
        }

        /// <summary>
        /// 把角度弧度转换为度'秒'分'字符串
        /// </summary>
        /// <returns>“dd°mm'ss.sss"”格式的字符串</returns>
        public string ToStringDms()
        {
            double decimalDegrees = this.Degrees;
            double d = Math.Abs(decimalDegrees);
            double m = (60 * (d - Math.Floor(d)));
            double s = (60 * (m - Math.Floor(m)));

            return String.Format("{0:##0}°{1:00}'{2:00.00}\"",
                (int)d * Math.Sign(decimalDegrees),
                (int)m,
                s);
        }


        /// <summary>
        /// 获取当前角度对应的中文经纬度表示
        /// </summary>
        /// <param name="isLatitude">是否是纬度</param>
        /// <returns>返回当前角度对应的中文经纬度表示</returns>
        public string GetLatLonStr(bool isLatitude)
        {
            if (isLatitude)
            {
                if (this.Degrees > 0)
                    return "北";
                else if (this.Degrees == 0.0)
                    return " ";
                else
                    return "南";
            }
            else
            {
                if (this.Degrees > 0)
                    return "东";
                else if (this.Degrees == 0.0)
                    return " ";
                else
                    return "西";
            }
        }


        public override string ToString()
        {
            return Degrees.ToString(CultureInfo.InvariantCulture) + "°";
        }
    }

    /// <summary>
    /// 地名结构体
    /// </summary>
    public struct QrstPlacename
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 纬度
        /// </summary>
        public float Lat;
        /// <summary>
        /// 经度
        /// </summary>
        public float Lon;
        /// <summary>
        /// 笛卡尔点
        /// </summary>
        public Vector3 cartesianPoint;
        /// <summary>
        /// 元数据
        /// </summary>
        public Hashtable metaData;
    }

    /// <summary>
    /// 单位
    /// </summary>
    public enum Units
    {
        /// <summary>
        /// 英制的
        /// </summary>
        English,
        /// <summary>
        /// 公制的、米制的
        /// </summary>
        Metric
    }

    /// <summary>
    /// 测量模式
    /// </summary>
    public enum MeasureMode
    {
        /// <summary>
        /// 单线段测量模式
        /// </summary>
        Single,
        /// <summary>
        /// 多线段测量模式
        /// </summary>
        Multi
    }

    /// <summary>
    /// 单位转换
    /// </summary>
    public static class ConvertUnits
    {
        /// <summary>
        /// 获取默认设置所对应的海拔高度
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static string GetDisplayAltitudeString(double distance)
        {
            if (World.Settings.DisplayUnits == Units.Metric)
            {
                if (distance >= 1000)
                {
                    return string.Format("{0:###,###.##} km", distance / 1000);
                }
                else
                {
                    return string.Format("{0:f0} m", distance);
                }
            }
            else
            {
                double feetPerMeter = 3.2808399;
                double feetPerMile = 5280;

                distance *= feetPerMeter;

                if (distance >= feetPerMile)
                {
                    return string.Format("{0:,.0} miles", distance / feetPerMile);
                }
                else
                {
                    return string.Format("{0:f0} ft", distance);
                }
            }
        }

        /// <summary>
        /// 断点列表，与WorldWind的图层层级对应
        /// </summary>
        private static int[] breakPoint = { 1, 2, 8, 26, 80, 242, 728, 2186, 6560, 19682, 59049, 127562 };

        /// <summary>
        /// 获取默认设置所对应的海拔高度
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static int GetDisplayLevel(double initialAltitude, double distance)
        {
            int level = 0;
            for (int i = 0; i < breakPoint.Length; i++)
            {
                if ((int)(initialAltitude / distance) <= breakPoint[i])
                {
                    level = i;
                    break;
                }
            }
            return level;
        }

    }

    /// <summary>
    /// 高度模式
    /// </summary>
    public enum AltitudeMode
    {
        ClampedToGround,
        RelativeToGround,
        Absolute
    }

    /// <summary>
    /// 矩形包络框结构体
    /// </summary>
    public struct Envelop
    {
        public double South;
        public double North;
        public double West;
        public double East;
    }
}
