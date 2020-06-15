using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Process.Site;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertNOAAMetaData:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertNOAAMetaData"; }
            set { }
        }

        public override void Process()
        {
            string sourceFilePath = ProcessArgu[0];
            //  读xml 元数据
            this.ParentOrder.Logs.Add("开始元数据入库。");
            MetaDataReader mdReader = new MetaDataReader();
            MetaDataNOAA metNoaa = mdReader.ReadMetaDataNOAA(sourceFilePath);
            //元数据入库
            string insertSql = "";
            MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
            mdDBImporter.ImportData(EnumMetadataTypes.NOAA, metNoaa, sourceFilePath, TServerSiteManager.ConvertTSSiteIP2TSSiteCode(this.ParentOrder.TSSiteIP), out insertSql);
            this.ParentOrder.Logs.Add("完成元数据入库。");
        }
    }
}
