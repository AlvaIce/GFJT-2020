using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Process.Site;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertModisMetaData:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertModisMetaData"; }
            set { }
        }

        public override void Process()
        {
            string sourceFilePath = ProcessArgu[0];
            //  读xml 元数据
            this.ParentOrder.Logs.Add(string.Format("开始元数据入库。"));
            MetaDataReader mdReader = new MetaDataReader();
            MetaDataModis metModis = mdReader.ReadMetaDataModis(sourceFilePath);
            //元数据入库
            string insertSql = "";
            MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
            mdDBImporter.ImportData(EnumMetadataTypes.MODIS, metModis, sourceFilePath, TServerSiteManager.ConvertTSSiteIP2TSSiteCode(this.ParentOrder.TSSiteIP), out insertSql);
            this.ParentOrder.Logs.Add(string.Format("完成元数据入库。"));
        }
    }
}
