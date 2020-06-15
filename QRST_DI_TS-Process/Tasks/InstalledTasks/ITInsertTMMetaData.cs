using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Process.Site;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertTMMetaData : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertTMMetaData"; }
            set { }
        }

        public override void Process()
        {
            string excelFilePath = ProcessArgu[0];
            try
            {
                //  读xml 元数据
                this.ParentOrder.Logs.Add(string.Format("开始元数据入库。"));
                MetaDataReader mdReader = new MetaDataReader();
                MetaDataRaster metRaster = mdReader.ReadMetaDataTM(excelFilePath);
                //元数据入库
                string insertSql = "";
                MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
                mdDBImporter.ImportData(EnumMetadataTypes.TM, metRaster, excelFilePath, TServerSiteManager.ConvertTSSiteIP2TSSiteCode(this.ParentOrder.TSSiteIP), out insertSql);
            }
            catch (System.Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("全国范围TM数据元数据插入异常{0}", ex.Message));
            }
            this.ParentOrder.Logs.Add(string.Format("完成元数据入库。"));
        }
    }
}