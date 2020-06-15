using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Process.Site;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    class ITInsertTJ5MRSImageMetaData : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertTJ5MRSImageMetaData"; }
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
                MetaDataRaster metRaster = mdReader.ReadMetaDataDOM(excelFilePath);
                //元数据入库
                string insertSql = "";
                MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
                mdDBImporter.ImportData(EnumMetadataTypes.TJ5MRSImage, metRaster, excelFilePath, TServerSiteManager.ConvertTSSiteIP2TSSiteCode(this.ParentOrder.TSSiteIP), out insertSql);
                this.ParentOrder.Logs.Add(string.Format("完成元数据入库。"));
            }
            catch (System.Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("天津5米分辨率遥感影像元数据插入异常{0}", ex.Message));
            }
        }
    }
}