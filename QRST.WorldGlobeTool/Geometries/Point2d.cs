using System;

namespace QRST.WorldGlobeTool.Geometries
{
    /// <summary>
    /// 二维点
    /// </summary>
    public class Point2d
    {
        /// <summary>
        /// 二维点的X和Y坐标
        /// </summary>
        public double X, Y;

        #region 构造函数

        public Point2d()
        { }

        public Point2d(double xi, double yi)
        {
            X = xi;
            Y = yi;
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="P">二维点P</param>
        public Point2d(Point2d P)
        {
            X = P.X;
            Y = P.Y;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取原点到当前二维点的距离
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        #endregion

        #region 运算符重载

        public static Point2d operator +(Point2d P1, Point2d P2)
        {
            return new Point2d(P1.X + P2.X, P1.Y + P2.Y);
        }

        public static Point2d operator -(Point2d P1, Point2d P2)
        {
            return new Point2d(P1.X - P2.X, P1.Y - P2.Y);
        }

        public static Point2d operator *(Point2d P, double k)
        {
            return new Point2d(P.X * k, P.Y * k);
        }

        public static Point2d operator *(double k, Point2d P)
        {
            return new Point2d(P.X * k, P.Y * k);
        }

        public static Point2d operator /(Point2d P, double k)
        {
            return new Point2d(P.X / k, P.Y / k);
        }

        public static bool operator ==(Point2d P1, Point2d P2) // equal?
        {
            return (P1.X == P2.X && P1.Y == P2.Y);
        }

        public static bool operator !=(Point2d P1, Point2d P2) // equal?
        {
            return (P1.X != P2.X || P1.Y != P2.Y);
        }

        public static double dot(Point2d P1, Point2d P2) // inner product 2
        {
            return (P1.X * P2.X + P1.Y * P2.Y);
        }

        public static Point2d operator -(Point2d P)	// negation
        {
            return new Point2d(-P.X, -P.Y);
        }
        #endregion

        #region 其他运算

        // other operators
        public double norm()
        {
            return Math.Sqrt(norm2());
        }

        public double norm2() // squared L2 norm
        {
            return X * X + Y * Y;
        }

        /// <summary>
        /// 坐标归一化
        /// </summary>
        /// <returns>归一化坐标的二维点</returns>
        public Point2d normalize()
        {
            double n = norm();
            return new Point2d(X / n, Y / n);
        }

        #endregion

        #region 基本重载

        public override bool Equals(object o)
        {
            try
            {
                return (bool)(this == (Point2d)o);
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (int)(X * Y);
        }

        #endregion
    }
}
