using QRST_DI_DS_Basis;
using System.Data;
using QRST_DI_DS_Metadata;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertRasterMetaData : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertRasterMetaData"; }
            set { }
        }

        public override void Process()
        {
            string metaStr = ProcessArgu[0];
            //  从xml中反序列化元数据
            this.ParentOrder.Logs.Add("开始元数据入库。");
            DataTable dt = SerializerUtil.GetDsFormatXml(metaStr);
            //元数据入库
            MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
            mdDBImporter.ImportData(EnumMetadataTypes.RASTER, dt);
            this.ParentOrder.Logs.Add("完成元数据入库。");
        }
    }
}
