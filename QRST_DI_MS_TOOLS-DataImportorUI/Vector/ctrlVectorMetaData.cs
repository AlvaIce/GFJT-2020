using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_MS_TOOLS_DataImportorUI.Common;
using QRST_DI_DS_Metadata.MetaDataCls;

namespace QRST_DI_MS_TOOLS_DataImportorUI.Vector
{
    public partial class ctrlVectorMetaData : UserControl
    {
        public SingleDataVector _singleDataVector = null;

        public ctrlVectorMetaData()
        {
            InitializeComponent();
        }

        public void Create(SingleDataVector sdv)
        {
            _singleDataVector = sdv;
            MetaDataVector vectorMetadata = sdv._metaData;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("数据名称：{0}", vectorMetadata.ProductName));
            sb.AppendLine(string.Format("数据大小：{0}", vectorMetadata.DataSize));
            sb.AppendLine(string.Format("生产时间：{0}", vectorMetadata.ProduceDate));
            sb.AppendLine(string.Format("最大经度：{0}", vectorMetadata.ExtentRight));
            sb.AppendLine(string.Format("最小经度：{0}", vectorMetadata.ExtentLeft));
            sb.AppendLine(string.Format("最大纬度：{0}", vectorMetadata.ExtentUp));
            sb.AppendLine(string.Format("最小纬度：{0}", vectorMetadata.ExtentDown));
            sb.AppendLine(string.Format("数据格式：{0}", vectorMetadata.DataFormat));
            sb.AppendLine(string.Format("数据类型：{0}", vectorMetadata.DataType));
            sb.AppendLine(string.Format("投影系统：{0}", vectorMetadata.MapProjectPara));
            sb.AppendLine(string.Format("坐标系统：{0}", vectorMetadata.Coordinate));

            textMetaData.Text = sb.ToString();

            userMDSettingCtrl.Create(sdv);
            
            this.Refresh();
        }

        //收集用户输入的元数据信息
        public void SetCustomizedMetaData()
        {
            IVectorUserMetaDataSetting userMDSetting=userMDSettingCtrl as IVectorUserMetaDataSetting;
            userMDSetting.SetCustomizedMetaData();
        }

        public bool Check()
        {
            if (_singleDataVector._metaData == null)
            {
                return false;
            }
            return true;
        }


    }
}
