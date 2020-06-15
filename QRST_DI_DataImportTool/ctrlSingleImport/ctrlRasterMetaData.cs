using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport.FileData;
using QRST_DI_DataImportTool.DataImport.MetaData;

namespace QRST_DI_DataImportTool.ctrlSingleImport
{
    public partial class ctrlRasterMetaData : UserControl, IGetMetaData
    {
        MetaDataUserRaster metaDataUserRaster;

        FileData filedata;

        public ctrlRasterMetaData()
        {
            InitializeComponent();
        }

        //收集用户输入的元数据信息
        public MetaData GetMetaData(string groupCode)
        {
            metaDataUserRaster.dataLowerLeftLat = Decimal.Parse(textdataLowerLeftLat.Text);
            metaDataUserRaster.dataLowerLeftLong = Decimal.Parse(textdataLowerLeftLong.Text);
            metaDataUserRaster.dataLowerRightLong = Decimal.Parse(textdataLowerRightLong.Text);
            metaDataUserRaster.dataUpperLeftLat = Decimal.Parse(textdataUpperLeftLat.Text);
            metaDataUserRaster.satelliteId = textsatelliteId.Text;
            metaDataUserRaster.sensorId = textsensorId.Text;
            metaDataUserRaster.proName=textproName.Text  ;
            metaDataUserRaster.productDate = (DateTime)dateTimePicker1.Value;
            metaDataUserRaster.publicInfo.UserName = textuploader.Text;
            metaDataUserRaster.publicInfo.Department=textDep.Text  ;

            return metaDataUserRaster;
        }

        public void DisplayMetaData(MetaData metadata)
        {
            metaDataUserRaster = (MetaDataUserRaster)metadata;
            textdataLowerLeftLat.Text = metaDataUserRaster.dataLowerLeftLat.ToString();
            textdataLowerLeftLong.Text = metaDataUserRaster.dataLowerLeftLong.ToString();
            textdataLowerRightLong.Text = metaDataUserRaster.dataLowerRightLong.ToString();
            textdataUpperLeftLat.Text = metaDataUserRaster.dataUpperLeftLat.ToString();
            textsatelliteId.Text = metaDataUserRaster.satelliteId;
            textsensorId.Text = metaDataUserRaster.sensorId;
            textproName.Text = metaDataUserRaster.proName;
            dateTimePicker1.Value = (DateTime)metaDataUserRaster.productDate;
            textuploader.Text = metaDataUserRaster.publicInfo.UserName;
            textDep.Text = metaDataUserRaster.publicInfo.Department;

            List<string> lstFiles = filedata.GetAttacheMents();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(lstFiles.ToArray());


        }

        public bool Check()
        {
            StringBuilder info = new StringBuilder();
            double temp;
            if (string.IsNullOrEmpty(textproName.Text))
            {
                MessageBox.Show("请填写数据名称");
                return false;
            }
            if (!Double.TryParse(textdataLowerLeftLat.Text,out temp))
            {
                MessageBox.Show("最小纬度填写有误！");
                return false;
            }
            if (!Double.TryParse(textdataLowerLeftLong.Text, out temp))
            {
                MessageBox.Show("最小经度填写有误！");
                return false;
            }
            if (!Double.TryParse(textdataLowerRightLong.Text, out temp))
            {
                MessageBox.Show("最大经度填写有误！");
                return false;
            }
            if (!Double.TryParse(textdataUpperLeftLat.Text, out temp))
            {
                MessageBox.Show("最大纬度填写有误！");
                return false;
            }
            
            return true;
        }


        public void GetFileObj(FileData filedata)
        {
            this.filedata = filedata;
        }
    }
}
