using System;

namespace QRST.WorldGlobeTool.Geometries
{
    /// <summary>
    /// 三维点
    /// </summary>
    public class Point3d
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNaN = false;
        /// <summary>
        /// 3维点对应的X坐标
        /// </summary>
        public double X;
        /// <summary>
        /// 3维点对应的Y坐标
        /// </summary>
        public double Y;
        /// <summary>
        /// 3维点对应的Z坐标
        /// </summary>
        public double Z;

        #region  构造函数

        /// <summary>
        /// 初始化一个空的Point3d实例
        /// </summary>
        public Point3d()
        { }

        /// <summary>
        /// 初始化一个Point3d实例
        /// </summary>
        /// <param name="x">三维点的X坐标</param>
        /// <param name="y">三维点的Y坐标</param>
        /// <param name="z">三维点的Z坐标</param>
        public Point3d(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        /// <summary>
        /// 拷贝构造函数，初始化一个Point3d实例
        /// </summary>
        /// <param name="P">要拷贝的三维点实例</param>
        public Point3d(Point3d P)
        {
            X = P.X;
            Y = P.Y;
            X = P.Z;
        }

        #endregion

        #region  运算



        #endregion

        /// <summary>
        /// 两个三维点的叉积
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point3d cross(Point3d p)
        {
            return new Point3d(
                Y * p.Z - Z * p.Y,
                Z * p.X - X * p.Z,
                X * p.Y - Y * p.X
                );
        }

        /// <summary>
        /// 两个三维点的点积
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double dotProduct(Point3d p)
        {
            return X * p.X + Y * p.Y + Z * p.Z;
        }

        // other operators
        public double norm()	// L2 norm
        {
            return Math.Sqrt(norm2());
        }

        public double norm2() // squared L2 norm
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// 归一化
        /// </summary>
        /// <returns>返回归一化的三维点坐标</returns>
        public Point3d normalize() // normalization
        {
            double n = norm();
            return new Point3d(X / n, Y / n, Z / n);
        }

        /// <summary>
        /// 三维向量的长度
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public static Angle GetAngle(Point3d p1, Point3d p2)
        {
            Angle returnAngle = new Angle();
            returnAngle.Radians = Math.Acos(Point3d.dot(p1, p2) / (p1.Length * p2.Length));
            return returnAngle;
        }

        #region  运算符重载

        /// <summary>
        /// 两个三维点的加法操作
        /// </summary>
        /// <param name="P1">被加三维点</param>
        /// <param name="P2">加数三维点</param>
        /// <returns>返回两个三维点的加法和</returns>
        public static Point3d operator +(Point3d P1, Point3d P2)
        {
            return new Point3d(P1.X + P2.X, P1.Y + P2.Y, P1.Z + P2.Z);
        }

        /// <summary>
        /// 两个三维点的减法操作
        /// </summary>
        /// <param name="P1">被减三维点</param>
        /// <param name="P2">减数三维点</param>
        /// <returns>返回两个三维点的差值</returns>
        public static Point3d operator -(Point3d P1, Point3d P2)
        {
            return new Point3d(P1.X - P2.X, P1.Y - P2.Y, P1.Z - P2.Z);
        }

        /// <summary>
        /// 求反操作
        /// </summary>
        /// <param name="P">三维点</param>
        /// <returns>返回与三维点坐标相反的三维点</returns>
        public static Point3d operator -(Point3d P)
        {
            return new Point3d(-P.X, -P.Y, -P.Z);
        }

        /// <summary>
        /// 三维点和常数的乘法操作
        /// </summary>
        /// <param name="P">被乘三维点</param>
        /// <param name="k">常数乘数</param>
        /// <returns>返回三维点和常数的乘积</returns>
        public static Point3d operator *(Point3d P, double k)
        {
            return new Point3d(P.X * k, P.Y * k, P.Z * k);
        }

        /// <summary>
        /// 常数和三维点的乘法操作
        /// </summary>
        /// <param name="k">被乘数</param>
        /// <param name="P">三维点乘数</param>
        /// <returns>返回常数和三维点的乘积</returns>
        public static Point3d operator *(double k, Point3d P)
        {
            return new Point3d(P.X * k, P.Y * k, P.Z * k);
        }

        /// <summary>
        /// 两个三维点的内积
        /// </summary>
        /// <param name="P1">三维点1</param>
        /// <param name="P2">三维点2</param>
        /// <returns>返回两个三维点的内积</returns>
        public static double dot(Point3d P1, Point3d P2)
        {
            return (P1.X * P2.X + P1.Y * P2.Y + P1.Z * P2.Z);
        }

        /// <summary>
        /// 两个三维点的乘积
        /// </summary>
        /// <param name="P1">三维点1</param>
        /// <param name="P2">三维点2</param>
        /// <returns>返回两个三维点的乘积</returns>
        public static Point3d operator *(Point3d P1, Point3d P2)
        {
            return new Point3d(P1.Y * P2.Z - P1.Z * P2.Y,
                P1.Z * P2.X - P1.X * P2.Z, P1.X * P2.Y - P1.Y * P2.X);
        }


        /// <summary>
        /// 两个三维点的叉积
        /// </summary>
        /// <param name="P1">三维点1</param>
        /// <param name="P2">三维点2</param>
        /// <returns>返回两个三维点的叉积</returns>
        public static Point3d cross(Point3d P1, Point3d P2)
        {
            return P1 * P2;
        }

        /// <summary>
        /// 三维点和常数的除法操作
        /// </summary>
        /// <param name="P">被除三维点</param>
        /// <param name="k">除数常数</param>
        /// <returns>返回三维点和常数的除法商</returns>
        public static Point3d operator /(Point3d P, double k)
        {
            return new Point3d(P.X / k, P.Y / k, P.Z / k);
        }

        /// <summary>
        /// 重载相等操作
        /// </summary>
        /// <param name="o">被判断的对象</param>
        /// <returns>返回当前对象和被判断的对象是否相等</returns>
        public override bool Equals(object o)
        {
            try
            {
                return (bool)(this == (Point3d)o);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 重载获取哈希码操作
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            //not the best algorithm for hashing, but whatever...
            return (int)(X * Y * Z);
        }

        /// <summary>
        /// 相等运算符，判断两个三维点是否相等
        /// </summary>
        /// <param name="P1">三维点1</param>
        /// <param name="P2">三维点2</param>
        /// <returns>返回两个三维点是否相等，相等为true；否则为false</returns>
        public static bool operator ==(Point3d P1, Point3d P2) // equal?
        {
            return (P1.X == P2.X && P1.Y == P2.Y && P1.Z == P2.Z);
        }

        /// <summary>
        /// 不相等运算符，判断两个三维点是否不相等
        /// </summary>
        /// <param name="P1">三维点1</param>
        /// <param name="P2">三维点2</param>
        /// <returns>返回两个三维点是否不相等，不相等为true；否则为false</returns>
        public static bool operator !=(Point3d P1, Point3d P2) // equal?
        {
            return (P1.X != P2.X || P1.Y != P2.Y || P1.Z != P2.Z);
        }

        #endregion

        // Normal direction corresponds to a right handed traverse of ordered points.
        public Point3d unit_normal(Point3d P0, Point3d P1, Point3d P2)
        {
            Point3d p = (P1 - P0) * (P2 - P0);
            double l = p.norm();
            return new Point3d(p.X / l, p.Y / l, p.Z / l);
        }
    }
}
