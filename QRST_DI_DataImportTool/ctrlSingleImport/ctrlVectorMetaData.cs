using System.Text;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.DataImport.FileData;

namespace QRST_DI_DataImportTool.ctrlSingleImport
{
    public partial class ctrlVectorMetaData : UserControl,IGetMetaData
    {
        MetaDataVector vectorMetadata;

        public ctrlVectorMetaData()
        {
            InitializeComponent();
        }

        //收集用户输入的元数据信息
        public MetaData GetMetaData(string groupCode)
        {
            if(vectorMetadata!=null)
            {
                if(!string.IsNullOrEmpty(textOrg.Text))
                {
                    vectorMetadata.MetaProduceorg = textOrg.Text;
                }
                if(!string.IsNullOrEmpty(textSrc.Text))
                {
                    vectorMetadata.DataSource = textSrc.Text;
                }
                if(!string.IsNullOrEmpty(groupCode))
                {
                    vectorMetadata.GroupCode = groupCode;
                }
            }
            return vectorMetadata;
        }

        public bool Check()
        {
            return true;
        }

        public void DisplayMetaData(MetaData metadata)
        {
            vectorMetadata = (MetaDataVector)metadata;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("数据名称：{0}",vectorMetadata.ProductName));
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
            this.Refresh();
        }

        public void GetFileObj(FileData filedata)
        {
        }
    }
}
