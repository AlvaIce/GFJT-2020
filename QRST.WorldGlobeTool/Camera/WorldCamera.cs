using System;
using QRST.WorldGlobeTool.Geometries;
using QRST.WorldGlobeTool.Utility;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace QRST.WorldGlobeTool.Camera
{
    /// <summary>
    /// The "normal" camera
    /// </summary>
    public class WorldCamera : CameraBase
    {
        /// <summary>
        /// 目标纬度
        /// </summary>
        protected Angle _targetLatitude;
        /// <summary>
        /// 目标经度
        /// </summary>
        protected Angle _targetLongitude;
        /// <summary>
        /// 目标海拔高度
        /// </summary>
        protected double _targetAltitude;
        /// <summary>
        /// 目标距离
        /// </summary>
        protected double _targetDistance;
        /// <summary>
        /// 
        /// </summary>
        protected Angle _targetHeading;
        /// <summary>
        /// 
        /// </summary>
        protected Angle _targetBank;
        /// <summary>
        /// 
        /// </summary>
        protected Angle _targetTilt;
        /// <summary>
        /// 
        /// </summary>
        protected Angle _targetFov;
        /// <summary>
        /// 
        /// </summary>
        protected Quaternion4d _targetOrientation;

        /// <summary>
        /// 初始化一个世界照相机类
        /// </summary>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="radius">半径</param>
        public WorldCamera(Vector3 targetPosition, double radius)
            : base(targetPosition, radius)
        {
            this._targetOrientation = m_Orientation;
            this._targetDistance = this._distance;
            this._targetAltitude = this._altitude;
            this._targetTilt = this._tilt;
            this._targetFov = this._fov;
        }

        public override void SetPosition(double lat, double lon, double heading, double _altitude, double tilt, double bank)
        {
            if (double.IsNaN(lat)) lat = this._latitude.Degrees;
            if (double.IsNaN(lon)) lon = this._longitude.Degrees;
            if (double.IsNaN(heading)) heading = this._heading.Degrees;
            if (double.IsNaN(bank)) bank = _targetBank.Degrees;

            this._targetOrientation = Quaternion4d.EulerToQuaternion(
                MathEngine.DegreesToRadians(lon),
                MathEngine.DegreesToRadians(lat),
                MathEngine.DegreesToRadians(heading));

            Point3d v = Quaternion4d.QuaternionToEuler(this._targetOrientation);


            this._targetLatitude.Radians = v.Y;
            this._targetLongitude.Radians = v.X;
            this._targetHeading.Radians = v.Z;

            if (!double.IsNaN(tilt))
                this.Tilt = Angle.FromDegrees(tilt);
            if (!double.IsNaN(_altitude))
                Altitude = _altitude;
            this.Bank = Angle.FromDegrees(bank);
        }

        public override Angle Heading
        {
            get { return _heading; }
            set
            {
                _heading = value;
                _targetHeading = value;
            }
        }

        Angle angle = new Angle();
        protected void SlerpToTargetOrientation(double percent)
        {
            double c = Quaternion4d.Dot(m_Orientation, _targetOrientation);

            if (c > 1.0)
                c = 1.0;
            else if (c < -1.0)
                c = -1.0;

            angle = Angle.FromRadians(Math.Acos(c));

            m_Orientation = Quaternion4d.Slerp(m_Orientation, _targetOrientation, percent);

            _tilt += (_targetTilt - _tilt) * percent;
            _bank += (_targetBank - _bank) * percent;
            _distance += (_targetDistance - _distance) * percent;
            ComputeAltitude(_distance, _tilt);
            _fov += (_targetFov - _fov) * percent;
        }

        #region Public properties

        public override double TargetAltitude
        {
            get
            {
                return this._targetAltitude;
            }
            set
            {
                if (value < _terrainElevation * World.Settings.VerticalExaggeration + 100)
                    value = _terrainElevation * World.Settings.VerticalExaggeration + 100;
                if (value > 7 * this._worldRadius)
                    value = 7 * this._worldRadius;
                this._targetAltitude = value;
                this._altitude = value;
                ComputeTargetDistance(this._targetAltitude, this._targetTilt);
            }
        }

        public Angle TargetHeading
        {
            get { return this._targetHeading; }
            set
            {
                this._targetHeading = value;
            }
        }

        public Angle TargetLatitude
        {
            get { return this._targetLatitude; }
            set { this._targetLatitude = value; }
        }

        public Angle TargetLongitude
        {
            get { return this._targetLongitude; }
            set { this._targetLongitude = value; }
        }

        public Quaternion4d TargetOrientation
        {
            get { return this._targetOrientation; }
            set { this._targetOrientation = value; }
        }

        public override Angle Fov
        {
            get { return this._targetFov; }
            set
            {
                if (value > World.Settings.cameraFovMax)
                    value = World.Settings.cameraFovMax;
                if (value < World.Settings.cameraFovMin)
                    value = World.Settings.cameraFovMin;
                this._targetFov = value;
            }
        }

        #endregion


        #region ICamera interface

        public override void Update(Device device)
        {
            if (_altitude < _terrainElevation * World.Settings.VerticalExaggeration + minimumAltitude)
            {
                _targetAltitude = _terrainElevation * World.Settings.VerticalExaggeration + minimumAltitude;
                ComputeTargetDistance(_targetAltitude, _targetTilt);
            }

            SlerpToTargetOrientation(World.Settings.cameraSlerpPercentage);
            base.Update(device);
        }

        #endregion

        public override string ToString()
        {
            string res = base.ToString() +
                string.Format(
                "\nTarget: ({0}, {1} @ {2:f0}m)\nTarget Altitude: {3:f0}m",
                _targetLatitude, _targetLongitude, _targetDistance, _targetAltitude) + "\nAngle: " + angle;
            return res;
        }

        public override void RotationYawPitchRoll(Angle yaw, Angle pitch, Angle roll)
        {
            _targetOrientation = Quaternion4d.EulerToQuaternion(yaw.Radians, pitch.Radians, roll.Radians) * _targetOrientation;

            Point3d v = Quaternion4d.QuaternionToEuler(_targetOrientation);
            if (!double.IsNaN(v.Y))
                this._targetLatitude.Radians = v.Y;
            if (!double.IsNaN(v.X))
                this._targetLongitude.Radians = v.X;
            if (Math.Abs(roll.Radians) > double.Epsilon)
                this._targetHeading.Radians = v.Z;
        }

        public override Angle Bank
        {
            get { return _targetBank; }
            set
            {
                if (Angle.IsNaN(value))
                    return;

                _targetBank = value;
                if (!World.Settings.cameraSmooth)
                    _bank = value;
            }
        }

        public override Angle Tilt
        {
            get { return _targetTilt; }
            set
            {
                if (value > maxTilt)
                    value = maxTilt;
                else if (value < minTilt)
                    value = minTilt;

                _targetTilt = value;
                ComputeTargetAltitude(_targetDistance, _targetTilt);
                if (!World.Settings.cameraSmooth)
                    _tilt = value;
            }
        }

        public override double TargetDistance
        {
            get
            {
                return _targetDistance;
            }
            set
            {
                if (value < minimumAltitude + _terrainElevation * World.Settings.VerticalExaggeration)
                    value = minimumAltitude + TerrainElevation * World.Settings.VerticalExaggeration;
                if (value > maximumAltitude)
                    value = maximumAltitude;
                _targetDistance = value;
                ComputeTargetAltitude(_targetDistance, _targetTilt);
                if (!World.Settings.cameraSmooth)
                {
                    base._distance = _targetDistance;
                    base._altitude = _targetAltitude;
                }
            }
        }

        /// <summary>
        /// Zoom camera in/out (distance) 
        /// </summary>
        /// <param name="percent">Positive value = zoom in, negative=out</param>
        public override void Zoom(float percent)
        {
            if (percent > 0)
            {
                // In
                double factor = 1.0f + percent;
                TargetAltitude /= factor;
            }
            else
            {
                // Out
                double factor = 1.0f - percent;
                TargetAltitude *= factor;
            }
        }

        protected void ComputeTargetDistance(double altitude, Angle tilt)
        {
            double cos = Math.Cos(Math.PI - tilt.Radians);
            double x = _worldRadius * cos;
            double hyp = _worldRadius + altitude;
            double y = Math.Sqrt(_worldRadius * _worldRadius * cos * cos + hyp * hyp - _worldRadius * _worldRadius);
            double res = x - y;
            if (res < 0)
                res = x + y;
            _targetDistance = res;
        }

        protected void ComputeTargetAltitude(double distance, Angle tilt)
        {
            double dfromeq = Math.Sqrt(_worldRadius * _worldRadius + distance * distance -
                2 * _worldRadius * distance * Math.Cos(Math.PI - tilt.Radians));
            double alt = dfromeq - _worldRadius;
            if (alt < minimumAltitude + _terrainElevation * World.Settings.VerticalExaggeration)
                alt = minimumAltitude + _terrainElevation * World.Settings.VerticalExaggeration;
            else if (alt > maximumAltitude)
                alt = maximumAltitude;
            _targetAltitude = alt;
        }
    }
}
