using System.Windows.Forms;

namespace QRST.WorldGlobeTool.VisualForms
{
    public partial class UCGeoBoundingBox : UserControl
    {

        public bool SetGeoValuesSuc = false;
        public UCGeoBoundingBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取地理信息数组
        /// </summary>
        /// <returns>返回地理信息数组：西经(°)、北纬(°)、东经(°)、南纬(°)、海拔高度(m)</returns>
        public double[] GetGeoValues()
        {
            double[] geoResult = new double[5];
            geoResult[0] = (double)this.numericUpDownWest.Value;
            geoResult[1] = (double)this.numericUpDownNorth.Value;
            geoResult[2] = (double)this.numericUpDownEast.Value;
            geoResult[3] = (double)this.numericUpDownSouth.Value;
            geoResult[4] = (double)this.numericUpDownAltitude.Value;
            return geoResult;
        }

        /// <summary>
        /// 设置地理信息
        /// </summary>
        /// <param name="west">西经(°)</param>
        /// <param name="north">北纬(°)</param>
        /// <param name="east">东经(°)</param>
        /// <param name="south">南纬(°)</param>
        /// <param name="altitude">海拔高度(m)</param>
        public void SetGeoValues(double west, double north, double east, double south, double altitude)
        {
            try
            {
                this.numericUpDownWest.Value = (decimal)west;
                this.numericUpDownNorth.Value = (decimal)north;
                this.numericUpDownEast.Value = (decimal)east;
                this.numericUpDownSouth.Value = (decimal)south;
                this.numericUpDownAltitude.Value = (decimal)altitude;
                SetGeoValuesSuc = true;
            }
            catch
            {
                SetGeoValuesSuc = false;

                MessageBox.Show("范围识别失败，请确认经纬度范围");
            }            
        }
    }
}
