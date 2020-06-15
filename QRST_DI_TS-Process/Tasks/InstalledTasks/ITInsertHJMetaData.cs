using QRST_DI_DS_Metadata;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Site;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertHJMetaData:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertHJMetaData"; }
            set { }
        }

        public override void Process()
        {
            string sourceFilePath= ProcessArgu[0];
            string storePath_CorrectedData= StorageBasePath.StorePath_CorrectedData(sourceFilePath);


            string sourceFileNameWithoutExt = (sourceFilePath.Length > 0) ?
               Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sourceFilePath)) : "";


            //  读xml 元数据入库
           this.ParentOrder.Logs.Add(string.Format("开始元数据入库。"));
            MetaDataReader mdReader = new MetaDataReader();
            //如果纠正归档数据目录存在且里面有文件则保持路径，否则为-1
            string corDataPath = (Directory.Exists(storePath_CorrectedData) && Directory.GetFiles(storePath_CorrectedData).Length > 1) ? storePath_CorrectedData : "-1";
            MetaDataHj hjMeta = mdReader.ReadMetaDataHj(string.Format("{0}{1}.XML", storePath_CorrectedData, sourceFileNameWithoutExt), corDataPath);
            string insertSql = "";
            MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
            mdDBImporter.ImportData(EnumMetadataTypes.HJ, hjMeta, sourceFilePath, TServerSiteManager.ConvertTSSiteIP2TSSiteCode(this.ParentOrder.TSSiteIP), out insertSql);
            //QDB_Base.Sys.LogUtils.ConsoleWriteOutLine(insertSql, order.Code);
            this.ParentOrder.Logs.Add(string.Format("完成元数据入库。"));
        }
    }
}
