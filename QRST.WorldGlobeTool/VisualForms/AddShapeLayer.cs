using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace QRST.WorldGlobeTool.VisualForms
{
    public partial class AddShapeLayer : Form
    {
        public AddShapeLayer()
        {
            InitializeComponent();
        }

        public AddShapeLayer(QRSTWorldGlobeControl GlobeControl)
        {
            InitializeComponent();
            m_GlobeControl = GlobeControl;
            numericUpDownShapeLevelZeroDegree.Enabled = false;
            numericUpDownShapePyramidLevelCount.Enabled = false;
        }

        QRSTWorldGlobeControl m_GlobeControl;

        private void buttonSelectShape_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "加载Shape文件";
            ofd.Filter = "Shape文件|*.shp;*.SHP";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.textBoxShapeFilePath.Text = ofd.FileName;
                this.textBoxShapeLayerName.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                Envelop imageEnvelop = m_GlobeControl.QrstGlobe.GetEnvelop(ofd.FileName, true);
                this.ucGeoBoundingBoxShape.SetGeoValues(imageEnvelop.West, imageEnvelop.North,
                    imageEnvelop.East, imageEnvelop.South, 0);
                switch (m_GlobeControl.QrstGlobe.GetShapeFeatureType(ofd.FileName))
                {
                    case ShapeFeatureType.Point:
                    case ShapeFeatureType.MultiPoint:
                        this.textBoxShapeType.Text = "点状矢量";
                        break;
                    case ShapeFeatureType.Line:
                        this.textBoxShapeType.Text = "线状矢量";
                        break;
                    case ShapeFeatureType.Polygon:
                        this.textBoxShapeType.Text = "多边形矢量";
                        break;
                    case ShapeFeatureType.Unspecified:
                        this.textBoxShapeType.Text = "未知类型矢量";
                        break;
                }
            }
        }

        private void labelShapeLineColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.FullOpen = true;
            cd.SolidColorOnly = true;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.labelShapeLineColor.BackColor = cd.Color;
            }
        }

        private void buttonShapeAdd_Click(object sender, EventArgs e)
        {
            double[] geoValues = this.ucGeoBoundingBoxShape.GetGeoValues();
            string filePath = this.textBoxShapeFilePath.Text;
            string layerName = this.textBoxShapeLayerName.Text;


            if (radioButtonShapeMode.Checked)
            {
                if (this.textBoxShapeType.Text == "点状矢量")
                {
                    m_GlobeControl.QrstGlobe.AddShapePointLayer(layerName, filePath,
                        "", 25, 25, Path.Combine(m_GlobeControl.DataDirectory, @"Icons\placemark_circle.png"),
                        m_GlobeControl.CurrentWorld.EquatorialRadius * 1.5, 0, Color.White, Color.Black);
                }
                else if (this.textBoxShapeType.Text == "线状矢量" || this.textBoxShapeType.Text == "多边形矢量")
                {
                    m_GlobeControl.QrstGlobe.AddShapeLineOrPolygonLayer(layerName, filePath,
                        labelShapeLineColor.BackColor, labelShapeLineColor.BackColor,
                        1, m_GlobeControl.CurrentWorld.EquatorialRadius * 1.5, 0, false, "",
                        Color.White, Color.Black);
                }
            }
            else
            {
                //TODO:添加切片方式加载矢量
            }

            m_GlobeControl.GotoLatLonViewRange(geoValues[3], geoValues[1], geoValues[0], geoValues[2]);
            this.Close();
        }

        private void buttonShapeCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButtonShapeMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonShapeMode.Checked)
            {
                numericUpDownShapeLevelZeroDegree.Enabled = false;
                numericUpDownShapePyramidLevelCount.Enabled = false;
            }
        }

        private void radioButtonShapeTileMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonShapeTileMode.Checked)
            {
                numericUpDownShapeLevelZeroDegree.Enabled = true;
                numericUpDownShapePyramidLevelCount.Enabled = true;
            }
        }
    }
}
