using System;
using System.Windows.Forms;
using System.IO;

namespace QRST.WorldGlobeTool.VisualForms
{
    /// <summary>
    /// 添加图像图层
    /// </summary>
    public partial class AddImagesLayer : Form
    {
        public AddImagesLayer()
        {
            InitializeComponent();
        }

        public AddImagesLayer(QRSTWorldGlobeControl globeControl)
        {
            InitializeComponent();
            m_GlobeControl = globeControl;
            numericUpDownGeoTiffLevelZeroDegree.Enabled = false;
            numericUpDownGeoTiffPyramidLevelCount.Enabled = false;
        }

        QRSTWorldGlobeControl m_GlobeControl;


        #region 添加JPG/PNG图层

        private void buttonSelectJPGOrPNG_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "加载JPG/PNG文件";
            ofd.Filter = "jpg(*.jpg;*.jpeg)|*.jpg;*.jpeg|png(*.png)|*.png";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.textBoxJPGOrPNGFilePath.Text = ofd.FileName;
                this.textBoxJPGOrPNGLayerName.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                
                Envelop imageEnvelop = m_GlobeControl.QrstGlobe.GetEnvelop(ofd.FileName, false);
                
                this.ucGeoBoundingBoxJPGOrPNG.SetGeoValues(imageEnvelop.West, imageEnvelop.North,
                    imageEnvelop.East, imageEnvelop.South, 0);
            }
        }

        private void buttonJPGOrPNGAdd_Click(object sender, EventArgs e)
        {
            double[] geoValues = this.ucGeoBoundingBoxJPGOrPNG.GetGeoValues();
            string filePath = this.textBoxJPGOrPNGFilePath.Text;
            string layerName = this.textBoxJPGOrPNGLayerName.Text;
            byte backColor = Convert.ToByte(this.numericUpDownJPGOrPNGBackvalue.Value);
            byte opacity = Convert.ToByte(this.numericUpDownJPGOrPNGOpacity.Value);
            m_GlobeControl.QrstGlobe.AddImage(layerName, 
                geoValues[4], geoValues[3], geoValues[1], geoValues[0], geoValues[2], 
                backColor, opacity, filePath);
            m_GlobeControl.GotoLatLonViewRange(geoValues[3], geoValues[1], geoValues[0], geoValues[2]);
            this.Close();
        }

        private void buttonJPGOrPNGCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        } 

        #endregion

        #region 添加GeoTiff图层

        private void buttonSelectGeoTiff_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "加载Geotiff文件";
            ofd.Filter = "tiff(*.tif)|*.tif|tiff(*.tiff)|*.tiff";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.textBoxGeoTiffFilePath.Text = ofd.FileName;
                this.textBoxGeoTiffLayerName.Text = Path.GetFileNameWithoutExtension(ofd.FileName);

                Envelop imageEnvelop = m_GlobeControl.QrstGlobe.GetEnvelop(ofd.FileName, false);
                this.ucGeoBoundingBoxGeoTiff.SetGeoValues(imageEnvelop.West, imageEnvelop.North,
                    imageEnvelop.East, imageEnvelop.South, 0);
            }
        }

        private void buttonGeoTiffAdd_Click(object sender, EventArgs e)
        {
            double[] geoValues = this.ucGeoBoundingBoxGeoTiff.GetGeoValues();
            double level0 = (double)this.numericUpDownGeoTiffLevelZeroDegree.Value;
            int levelCount = (int)this.numericUpDownGeoTiffPyramidLevelCount.Value;

            string filePath = this.textBoxGeoTiffFilePath.Text.ToString();
            string layerName = this.textBoxGeoTiffLayerName.Text;
            byte backColor = Convert.ToByte(this.numericUpDownGeoTiffBackvalue.Value);
            byte opacity = Convert.ToByte(this.numericUpDownGeoTiffOpacity.Value);

            string cachePath = Path.Combine(Application.StartupPath, "Cache\\TempCache\\" + layerName);
            if (!System.IO.Directory.Exists(cachePath))
            {
                System.IO.Directory.CreateDirectory(cachePath);
            }

            if (radioButtonGeoTiffTile.Checked)
            {  //按照切片方式加载
                m_GlobeControl.QrstGlobe.AddGeoTiffLayer(layerName, 
                    geoValues[4], geoValues[3] - 1, geoValues[1] + 1, geoValues[0] - 1, geoValues[2] + 1,
                    backColor, opacity, true, level0, levelCount, new string[] { filePath }, cachePath);
            } 
            else if(radioButtonGeoTiffWhole.Checked)
            {  //按照整体贴图方式加载
                m_GlobeControl.QrstGlobe.AddImage(layerName,
                geoValues[4], geoValues[3], geoValues[1], geoValues[0], geoValues[2],
                backColor, opacity, filePath);
            }
            m_GlobeControl.GotoLatLonViewRange(geoValues[3], geoValues[1], geoValues[0], geoValues[2]);
            this.Close();
        }

        private void buttonGeoTiffCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButtonGeoTiffWhole_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGeoTiffWhole.Checked)
            {
                numericUpDownGeoTiffLevelZeroDegree.Enabled = false;
                numericUpDownGeoTiffPyramidLevelCount.Enabled = false;
            }
        }

        private void radioButtonGeoTiffTile_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGeoTiffTile.Checked)
            {
                numericUpDownGeoTiffLevelZeroDegree.Enabled = true;
                numericUpDownGeoTiffPyramidLevelCount.Enabled = true;
            }
        } 

        #endregion
    }
}
