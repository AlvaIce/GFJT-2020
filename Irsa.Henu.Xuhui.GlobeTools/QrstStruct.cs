using System;
using System.Globalization;
using Microsoft.DirectX;
using System.Collections;

namespace Qrst
{
	/// <summary>
	/// �ǶȽṹ��
	/// </summary>
	public struct Angle
	{
        /// <summary>
        /// ���ȵ�λ
        /// </summary>
		[NonSerialized]
		public double Radians;

		/// <summary>
		/// �ӻ���ת��Ϊ�Ƕ�
		/// </summary>
		public static Angle FromRadians(double radians)
		{
			Angle res = new Angle();
			res.Radians = radians;	
			return res;
		}

		/// <summary>
		/// ����һ���Ƕȶ���
		/// </summary>
		public static Angle FromDegrees(double degrees)
		{
			Angle res = new Angle();
			res.Radians = Math.PI * degrees / 180.0;
			return res;
		}

		/// <summary>
		/// ��λ��Ƕ�
		/// </summary>
		public static readonly Angle Zero;

		/// <summary>
		/// ��С�Ƕ�
		/// </summary>
		public static readonly Angle MinValue = Angle.FromRadians(double.MinValue);

		/// <summary>
		/// ���Ƕ�
		/// </summary>
		public static readonly Angle MaxValue = Angle.FromRadians(double.MaxValue);

		/// <summary>
		/// ����ĽǶ�
		/// </summary>
		public static readonly Angle NaN = Angle.FromRadians(double.NaN);

		public double Degrees
		{
			get { return MathEngine.RadiansToDegrees(this.Radians);}
			set { this.Radians = MathEngine.DegreesToRadians(value); }
		}


		/// <summary>
		/// ���ؽǶȵľ���ֵ�Ƕȣ���-�Ƕȱ�+�Ƕȡ�
		/// </summary>
		public static Angle Abs( Angle a )
		{
			return Angle.FromRadians(Math.Abs(a.Radians));
		}

		/// <summary>
		/// �жϽǶ��Ƿ�ΪNaN
		/// </summary>
		public static bool IsNaN(Angle a)
		{
			return double.IsNaN(a.Radians);
		}

        /// <summary>
        /// �ж������Ƕ��Ƿ���ͬ
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
	    /// ��==�ж������Ƕ��Ƿ���ͬ
	    /// </summary>
	    /// <param name="a"></param>
	    /// <param name="b"></param>
	    /// <returns></returns>
		public static bool operator ==(Angle a, Angle b) {
			return Math.Abs(a.Radians - b.Radians) < Single.Epsilon;
		}
        /// <summary>
        /// ��!=�ж������ǶȲ���ͬ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
		public static bool operator !=(Angle a, Angle b) {
			return Math.Abs(a.Radians - b.Radians) > Single.Epsilon;
		}
        /// <summary>
        /// �жϽǶ�a�Ƿ�С�ڽǶ�b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
		public static bool operator <(Angle a, Angle b) 
		{
			return a.Radians < b.Radians;
		}
        /// <summary>
        /// �жϽǶ�a�Ƿ���ڽǶ�b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
		public static bool operator >(Angle a, Angle b) 
		{
			return a.Radians > b.Radians;
		}
        /// <summary>
        /// �Ƕ�a��Ƕ�b���
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
        /// �Ƕ�a��Ƕ�b���
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
        /// �Ƕ�a��ϵ�����
        /// </summary>
        /// <param name="a"></param>
        /// <param name="times"></param>
        /// <returns></returns>
		public static Angle operator *(Angle a, double times) 
		{
			return Angle.FromRadians(a.Radians * times);
		}
        /// <summary>
        /// ϵ����Ƕ�a���
        /// </summary>
        /// <param name="times"></param>
        /// <param name="a"></param>
        /// <returns></returns>
		public static Angle operator *(double times, Angle a) 
		{
			return Angle.FromRadians(a.Radians * times);
		}
        /// <summary>
        /// ϵ����Ƕ�a���
        /// </summary>
        /// <param name="divisor"></param>
        /// <param name="a"></param>
        /// <returns></returns>
		public static Angle operator /(double divisor, Angle a) 
		{
			return Angle.FromRadians(a.Radians / divisor);
		}
        /// <summary>
        /// �Ƕ�a��ϵ�����
        /// </summary>
        /// <param name="a"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
		public static Angle operator /(Angle a, double divisor) 
		{
			return Angle.FromRadians(a.Radians / divisor);
		}
        /// <summary>
        /// ��õ�ǰ�Ƕȵ�HashCode
        /// </summary>
        /// <returns></returns>
		public override int GetHashCode() 
		{
			return (int)(Radians*100000);
		}

		/// <summary>
		/// ���л��Ƕȶ���ʹ�Ƕȱ仯��-360��+360֮��
		/// </summary>
		public void Normalize()
		{
			if(Radians>Math.PI*2)
				Radians -= Math.PI*2;
			if(Radians<-Math.PI*2)
				Radians += Math.PI*2;
		}

		/// <summary>
		/// �ѽǶȻ���ת��Ϊ��'��'��'�ַ���
		/// </summary>
		/// <returns>String on format dd�mm'ss.sss"</returns>
		public string ToStringDms()
		{
			double decimalDegrees = this.Degrees;
			double d = Math.Abs(decimalDegrees);
			double m = (60*(d-Math.Floor(d)));
			double s = (60*(m-Math.Floor(m)));

			return String.Format("{0}1}'{2:f3}\"", 
				(int)d*Math.Sign(decimalDegrees), 
				(int)m, 
				s);
		}

		public override string ToString()
		{
			return Degrees.ToString(CultureInfo.InvariantCulture)+"��";
		}
	}
    /// <summary>
    /// �����ṹ��
    /// </summary>
    public struct QrstPlacename
    {
        public string ID;
        public string Name;
        public float Lat;
        public float Lon;
        public Vector3 cartesianPoint;
        public Hashtable metaData;
    }
    /// <summary>
    /// ��λ
    /// </summary>
    public enum Units
    {
        English,
        Metric
    }

    /// <summary>
    /// ����ģʽ
    /// </summary>
    public enum MeasureMode
    {
        Single,
        Multi
    }


    public static class ConvertUnits
    {
        /// <summary>
        /// ��ȡĬ����������Ӧ�ĵ�λ
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static string GetDisplayString(double distance)
        {
            if (World.Settings.DisplayUnits == Units.Metric)
            {
                if (distance >= 1000)
                {
                    return string.Format("{0:,.0} km", distance / 1000);
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
    }

    /// <summary>
    /// �߶�ģʽ
    /// </summary>
    public enum AltitudeMode
    {
        ClampedToGround,
        RelativeToGround,
        Absolute
    }


    public struct Envelop
    {
        public double South ;
        public double North ;
        public double West ;
        public double East ;
    }
}
