using System;
using Microsoft.DirectX;
using QRST.WorldGlobeTool.Geometries;

namespace QRST.WorldGlobeTool.Utility
{
    public sealed class MathEngine
    {
        /// <summary>
        /// This class has only static methods.
        /// </summary>
        private MathEngine()
        {
        }

        /// <summary>
        /// 把经纬度(lat,lon,radius)坐标转换为笛卡儿坐标(x,y,z)
        /// Converts position in spherical coordinates (lat/lon/altitude) to cartesian (XYZ) coordinates.
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees</param>
        /// <param name="longitude">Longitude in decimal degrees</param>
        /// <param name="radius">Radius (OBS: not altitude)</param>
        /// <returns>Coordinates converted to cartesian (XYZ)</returns>
        public static Vector3 SphericalToCartesian(
            double latitude,
            double longitude,
            double radius
            )
        {
            latitude *= System.Math.PI / 180.0;
            longitude *= System.Math.PI / 180.0;

            double radCosLat = radius * Math.Cos(latitude);

            return new Vector3(
                (float)(radCosLat * Math.Cos(longitude)),
                (float)(radCosLat * Math.Sin(longitude)),
                (float)(radius * Math.Sin(latitude)));
        }

        /// <summary>
        /// Converts position in spherical coordinates (lat/lon/altitude) to cartesian (XYZ) coordinates.
        /// </summary>
        /// <param name="latitude">Latitude (Angle)</param>
        /// <param name="longitude">Longitude (Angle)</param>
        /// <param name="radius">Radius (OBS: not altitude)</param>
        /// <returns>Coordinates converted to cartesian (XYZ)</returns>
        public static Vector3 SphericalToCartesian(
            Angle latitude,
            Angle longitude,
            double radius)
        {
            double latRadians = latitude.Radians;
            double lonRadians = longitude.Radians;

            double radCosLat = radius * Math.Cos(latRadians);

            return new Vector3(
                (float)(radCosLat * Math.Cos(lonRadians)),
                (float)(radCosLat * Math.Sin(lonRadians)),
                (float)(radius * Math.Sin(latRadians)));
        }

        /// <summary>
        /// Converts position in spherical coordinates (lat/lon/altitude) to cartesian (XYZ) coordinates.
        /// </summary>
        /// <param name="latitude">Latitude (Angle)</param>
        /// <param name="longitude">Longitude (Angle)</param>
        /// <param name="radius">Radius (OBS: not altitude)</param>
        /// <returns>Coordinates converted to cartesian (XYZ)</returns>
        public static Point3d SphericalToCartesianD(
            Angle latitude,
            Angle longitude,
            double radius)
        {
            double latRadians = latitude.Radians;
            double lonRadians = longitude.Radians;

            double radCosLat = radius * Math.Cos(latRadians);

            return new Point3d(
                radCosLat * Math.Cos(lonRadians),
                radCosLat * Math.Sin(lonRadians),
                radius * Math.Sin(latRadians));
        }

        /// <summary>
        /// Converts position in cartesian coordinates (XYZ) to spherical (lat/lon/radius) coordinates in radians.
        /// </summary>
        /// <returns>Coordinates converted to spherical coordinates.  X=radius, Y=latitude (radians), Z=longitude (radians).</returns>
        public static Vector3 CartesianToSpherical(float x, float y, float z)
        {
            double rho = Math.Sqrt((double)(x * x + y * y + z * z));
            float longitude = (float)Math.Atan2(y, x);
            float latitude = (float)(Math.Asin(z / rho));

            return new Vector3((float)rho, latitude, longitude);
        }

        public static Point3d CartesianToSphericalD(double x, double y, double z)
        {
            double rho = Math.Sqrt((double)(x * x + y * y + z * z));
            double longitude = Math.Atan2(y, x);
            double latitude = (Math.Asin(z / rho));

            return new Point3d(rho, latitude, longitude);
        }

        /// <summary>
        /// Converts an angle in decimal degrees to angle in radians
        /// </summary>
        /// <param name="degrees">Angle in decimal degrees (0-360)</param>
        /// <returns>Angle in radians (0-2*Pi)</returns>
        public static double DegreesToRadians(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        /// <summary>
        /// Converts an angle in radians to angle in decimal degrees 
        /// </summary>
        /// <param name="radians">Angle in radians (0-2*Pi)</param>
        /// <returns>Angle in decimal degrees (0-360)</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        /// <summary>
        /// Computes the angle (seen from the center of the sphere) between 2 sets of latitude/longitude values.
        /// </summary>
        /// <param name="latA">Latitude of point 1 (decimal degrees)</param>
        /// <param name="lonA">Longitude of point 1 (decimal degrees)</param>
        /// <param name="latB">Latitude of point 2 (decimal degrees)</param>
        /// <param name="lonB">Longitude of point 2 (decimal degrees)</param>
        /// <returns>Angle in decimal degrees</returns>
        public static double SphericalDistanceDegrees(double latA, double lonA, double latB, double lonB)
        {
            double radLatA = MathEngine.DegreesToRadians(latA);
            double radLatB = MathEngine.DegreesToRadians(latB);
            double radLonA = MathEngine.DegreesToRadians(lonA);
            double radLonB = MathEngine.DegreesToRadians(lonB);

            return MathEngine.RadiansToDegrees(
                Math.Acos(Math.Cos(radLatA) * Math.Cos(radLatB) * Math.Cos(radLonA - radLonB) + Math.Sin(radLatA) * Math.Sin(radLatB)));
        }

        /// <summary>
        /// Computes the angular distance between two pairs of lat/longs.
        /// Fails for distances (on earth) smaller than approx. 2km. (returns 0)
        /// </summary>
        public static Angle SphericalDistance(Angle latA, Angle lonA, Angle latB, Angle lonB)
        {
            double radLatA = latA.Radians;
            double radLatB = latB.Radians;
            double radLonA = lonA.Radians;
            double radLonB = lonB.Radians;

            return Angle.FromRadians(Math.Acos(
                Math.Cos(radLatA) * Math.Cos(radLatB) * Math.Cos(radLonA - radLonB) +
                Math.Sin(radLatA) * Math.Sin(radLatB)));
        }

        /// <summary>
        /// Calculates the azimuth from latA/lonA to latB/lonB
        /// Borrowed from http://williams.best.vwh.net/avform.htm
        /// </summary>
        public static Angle Azimuth(Angle latA, Angle lonA, Angle latB, Angle lonB)
        {
            double cosLatB = Math.Cos(latB.Radians);
            Angle tcA = Angle.FromRadians(Math.Atan2(
                Math.Sin(lonA.Radians - lonB.Radians) * cosLatB,
                Math.Cos(latA.Radians) * Math.Sin(latB.Radians) -
                Math.Sin(latA.Radians) * cosLatB *
                Math.Cos(lonA.Radians - lonB.Radians)));
            if (tcA.Radians < 0)
                tcA.Radians = tcA.Radians + Math.PI * 2;
            tcA.Radians = Math.PI * 2 - tcA.Radians;

            return tcA;
        }

        /// <summary>
        /// Transforms a set of Euler angles to a quaternion
        /// </summary>
        /// <param name="yaw">Yaw (radians)</param>
        /// <param name="pitch">Pitch (radians)</param>
        /// <param name="roll">Roll (radians)</param>
        /// <returns>The rotation transformed to a quaternion.</returns>
        public static Quaternion EulerToQuaternion(double yaw, double pitch, double roll)
        {
            double cy = Math.Cos(yaw * 0.5);
            double cp = Math.Cos(pitch * 0.5);
            double cr = Math.Cos(roll * 0.5);
            double sy = Math.Sin(yaw * 0.5);
            double sp = Math.Sin(pitch * 0.5);
            double sr = Math.Sin(roll * 0.5);

            double qw = cy * cp * cr + sy * sp * sr;
            double qx = sy * cp * cr - cy * sp * sr;
            double qy = cy * sp * cr + sy * cp * sr;
            double qz = cy * cp * sr - sy * sp * cr;

            return new Quaternion((float)qx, (float)qy, (float)qz, (float)qw);
        }

        /// <summary>
        /// Transforms a rotation in quaternion form to a set of Euler angles 
        /// </summary>
        /// <returns>The rotation transformed to Euler angles, X=Yaw, Y=Pitch, Z=Roll (radians)</returns>
        public static Vector3 QuaternionToEuler(Quaternion q)
        {
            double q0 = q.W;
            double q1 = q.X;
            double q2 = q.Y;
            double q3 = q.Z;

            double x = Math.Atan2(2 * (q2 * q3 + q0 * q1), (q0 * q0 - q1 * q1 - q2 * q2 + q3 * q3));
            double y = Math.Asin(-2 * (q1 * q3 - q0 * q2));
            double z = Math.Atan2(2 * (q1 * q2 + q0 * q3), (q0 * q0 + q1 * q1 - q2 * q2 - q3 * q3));

            return new Vector3((float)x, (float)y, (float)z);
        }

        #region  瓦片行列号计算

        /// <summary>
        /// Compute the tile number (used in file names) for given latitude and tile size.
        /// 由给定的纬度和瓦片尺寸计算瓦片行编号（在文件名称中使用）
        /// </summary>
        /// <param name="latitude">Latitude (decimal degrees)十进制度数纬度信息</param>
        /// <param name="tileSize">Tile size  (decimal degrees)十进制度数瓦片尺寸</param>
        /// <returns>The tile number返回瓦片的行编号</returns>
        public static int GetRowFromLatitude(double latitude, double tileSize)
        {
            return (int)System.Math.Round((System.Math.Abs(-90.0 - latitude) % 180) / tileSize, 1);
        }

        /// <summary>
        /// Compute the tile number (used in file names) for given latitude and tile size.
        /// 由给定的纬度和瓦片尺寸计算瓦片行编号（在文件名称中使用）
        /// </summary>
        /// <param name="latitude">Latitude (decimal degrees)角度纬度信息</param>
        /// <param name="tileSize">Tile size  (decimal degrees)十进制度数瓦片尺寸</param>
        /// <returns>The tile number返回瓦片的行编号</returns>
        public static int GetRowFromLatitude(Angle latitude, double tileSize)
        {
            return (int)System.Math.Round((System.Math.Abs(-90.0 - latitude.Degrees) % 180) / tileSize, 1);
        }

        /// <summary>
        /// Compute the tile number (used in file names) for given longitude and tile size.
        /// 由给定的经度和瓦片尺寸计算瓦片列编号（在文件名称中使用）
        /// </summary>
        /// <param name="longitude">Longitude (decimal degrees)十进制度数经度信息</param>
        /// <param name="tileSize">Tile size  (decimal degrees)十进制度数瓦片尺寸</param>
        /// <returns>The tile number返回瓦片的列编号</returns>
        public static int GetColFromLongitude(double longitude, double tileSize)
        {
            return (int)System.Math.Round((System.Math.Abs(-180.0 - longitude) % 360) / tileSize, 1);
        }

        /// <summary>
        /// Compute the tile number (used in file names) for given longitude and tile size.
        /// 由给定的经度和瓦片尺寸计算瓦片列编号（在文件名称中使用）
        /// </summary>
        /// <param name="longitude">Longitude (decimal degrees)角度经度信息</param>
        /// <param name="tileSize">Tile size  (decimal degrees)十进制度数瓦片尺寸</param>
        /// <returns>The tile number返回瓦片的列编号</returns>
        public static int GetColFromLongitude(Angle longitude, double tileSize)
        {
            return (int)System.Math.Round((System.Math.Abs(-180.0 - longitude.Degrees) % 360) / tileSize, 1);
        }

        /// <summary>
        /// 根据输入边框信息返回包含指定层级的瓦片行列号的哈希表
        /// </summary>
        /// <param name="minLon">最小经度</param>
        /// <param name="maxLat">最大纬度</param>
        /// <param name="maxLon">最大经度</param>
        /// <param name="minLat">最小纬度</param>
        /// <param name="level">指定层级</param>
        /// <returns>返回在指定层级包含边框对应瓦片的行列号的哈希表</returns>
        public static void GetTilesRowAndColumnByBoundingBox(
            double minLon, double maxLat, double maxLon, double minLat, int level,
            out int[] rows, out int[] columns)
        {
            double levelDegree = 36.0 / (Math.Pow(2.0, level));
            int minX = (int)Math.Floor((minLon + 180) / levelDegree);
            int maxX = (int)Math.Floor((maxLon + 180) / levelDegree);
            int minY = (int)Math.Floor((minLat + 90) / levelDegree);
            int maxY = (int)Math.Floor((maxLat + 90) / levelDegree);
            rows = new int[maxY - minY + 1];
            columns = new int[maxX - minX + 1];
            for (int row = minY; row <= maxY; row++)
                rows[row - minY] = row;
            for (int column = minX; column <= maxX; column++)
                columns[column - minX] = column;
        }

        #endregion

        /// <summary>
        /// Computes the distance between a point and a plane.
        /// </summary>
        /// <param name="p">Plane</param>
        /// <param name="v">Point (XYZ coordinates)</param>
        /// <returns>The shortest distance between the point and the plane.</returns>
        public static float DistancePlaneToPoint(Plane p, Vector3 v)
        {
            return p.A * v.X + p.B * v.Y + p.C + v.Z + p.D;
        }

        /// <summary>
        /// Computes the hypotenuse (sqrt(x?y?).
        /// </summary>
        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

    }
}
