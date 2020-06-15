using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using DotSpatial.Data;
using DotSpatial.Projections;
using System.Collections;

namespace QRST.WorldGlobeTool.VisualForms
{
    public partial class GotoPlace : Form
    {
        public GotoPlace()
        {
            InitializeComponent();
        }

        public GotoPlace(QRSTWorldGlobeControl GlobeControl)
        {
            InitializeComponent();
            m_GlobeControl = GlobeControl;
        }

        QRSTWorldGlobeControl m_GlobeControl;

        private static XDocument doc;
        private static XElement root;
        private static Hashtable m_countyPosition;

        #region 窗体加载

        /// <summary>
        /// 加载数据
        /// </summary>
        private void GotoPlace_Load(object sender, EventArgs e)
        {
            buttonRegionGoto.Enabled = false;

            #region 行政区域读取

            string regionXMLPath = Path.Combine(m_GlobeControl.DataDirectory, @"China\行政区划.xml");
            if (File.Exists(regionXMLPath))
            {
                doc = XDocument.Load(regionXMLPath);
                root = doc.Element("中国");
                foreach (XElement province in root.Elements())
                {
                    comboBoxProvince.Items.Add(province.Name.LocalName);
                }
                comboBoxProvince.SelectedIndex = 0;
            }

            #endregion

            if (m_countyPosition == null)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                bw.RunWorkerAsync();
            }
            else
            {
                buttonRegionGoto.Enabled = true;
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonRegionGoto.Enabled = true;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            #region 矢量数据读取

            string countyShapePath = Path.Combine(m_GlobeControl.DataDirectory, @"China\ChinaSHP\中国县界.shp");
            if (File.Exists(countyShapePath))
            {
                m_countyPosition = new Hashtable();
                Shapefile _sf = Shapefile.OpenFile(countyShapePath);
                ProjectionInfo piWGS1984 = KnownCoordinateSystems.Geographic.World.WGS1984;
                if (_sf.Projection != piWGS1984 && _sf.CanReproject)
                    _sf.Reproject(piWGS1984);//如果投影不是WGS1984投影，则进行投影变换
                foreach (IFeature ft in _sf.Features)
                {
                    Shape shape = ft.ToShape();
                    List<double[]> verticess = new List<double[]>();
                    if (shape.Range.Parts == null)
                    {
                        verticess.Add(shape.Vertices);
                    }
                    else
                    {
                        foreach (PartRange pr in shape.Range.Parts)
                        {
                            verticess.Add(pr.Vertices);
                        }
                    }
                    string shapeName = ft.DataRow["NAME"].ToString();
                    double centerLon = 0, centerLat = 0;
                    long count = 0;
                    foreach (double[] vts in verticess)
                    {
                        count += vts.Length / 2;
                        for (int i = 0; i < vts.Length; i += 2)
                        {
                            centerLon += vts[i];
                            centerLat += vts[i + 1];
                        }
                    }
                    centerLon /= count;
                    centerLat /= count;
                    shapeName = shapeName == "" ? Guid.NewGuid().ToString() : shapeName;
                    try
                    {
                        m_countyPosition.Add(shapeName, string.Format("{0}#{1}", centerLat, centerLon));
                    }
                    catch (ArgumentException)
                    {
                        m_countyPosition.Add(shapeName + Guid.NewGuid().ToString(), string.Format("{0}#{1}", centerLat, centerLon));
                    }
                }
            }

            #endregion

        } 

        #endregion

        #region 按经纬度进行定位

        /// <summary>
        /// 按经纬度方式定位
        /// </summary>
        private void buttonLLGoto_Click(object sender, EventArgs e)
        {
            try
            {
                double lat = (double)this.numericUpDownLLLat.Value;//经度
                double lon = (double)this.numericUpDownLLLon.Value;//纬度
                double alt = (double)this.numericUpDownLLAltitude.Value;//高度

                m_GlobeControl.QrstGlobe.Goto(lat, lon, alt);//Goto过去
            }
            catch { }
        }

        #endregion

        #region 按行政区域进行定位

        private void comboBoxProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCity.Items.Clear();
            foreach (XElement city in root.Element(comboBoxProvince.SelectedItem.ToString()).Elements())
            {
                comboBoxCity.Items.Add(city.Name.LocalName);
            }
            comboBoxCity.SelectedIndex = 0;
        }

        private void comboBoxCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCounty.Items.Clear();
            foreach (XElement county in root.Element(comboBoxProvince.SelectedItem.ToString()).Element(comboBoxCity.SelectedItem.ToString()).Elements())
            {
                comboBoxCounty.Items.Add(county.Name.LocalName);
            }
            comboBoxCounty.SelectedIndex = 0;
        }

        private void comboBoxCounty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonRegionGoto_Click(object sender, EventArgs e)
        {
            try
            {
                string[] latLonStr;
                object tempStr = m_countyPosition[comboBoxCounty.SelectedItem.ToString()];
                if (tempStr == null)
                {
                    tempStr = m_countyPosition[comboBoxCity.SelectedItem.ToString()];
                    if (tempStr == null)
                    {
                        tempStr = m_countyPosition[comboBoxProvince.SelectedItem.ToString()];
                    }
                }

                if (tempStr != null)
                {
                    latLonStr = tempStr.ToString().Split('#');
                    double lat = double.Parse(latLonStr[0]);//经度
                    double lon = double.Parse(latLonStr[1]);//纬度
                    double alt = (double)this.numericUpDownRegionAltitude.Value;//高度

                    m_GlobeControl.QrstGlobe.Goto(lat, lon, alt);//Goto过去
                }


            }
            catch { }
        } 

        #endregion

        #region 按模糊搜索结果进行定位

        private void textBoxBlur_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dataGridViewBlur.Rows.Clear();
                foreach (DictionaryEntry de in m_countyPosition)
                {
                    if (de.Key.ToString().Contains(textBoxBlur.Text))
                    {
                        dataGridViewBlur.Rows.Add(new object[] { de.Key, de.Value.ToString().Split('#')[0], de.Value.ToString().Split('#')[1] });
                    }
                }
            }
        }

        private void buttonBlur_Click(object sender, EventArgs e)
        {
            if (dataGridViewBlur.RowCount > 0)
            {
                try
                {

                    double lat = double.Parse(dataGridViewBlur.SelectedRows[0].Cells[1].Value.ToString());//纬度
                    double lon = double.Parse(dataGridViewBlur.SelectedRows[0].Cells[2].Value.ToString());//经度
                    double alt = (double)this.numericUpDownBlur.Value;//高度

                    m_GlobeControl.QrstGlobe.Goto(lat, lon, alt);//Goto过去
                }
                catch { }
            }
        }

        #endregion

    }
}
