using QRST_DI_DS_Metadata;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Site;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertNormalHJMetaData
        :TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertNormalHJMetaData"; }
            set { }
        }

        public override void Process()
        {
            string sourceFilePath= ProcessArgu[0];

            //  读xml 元数据入库
           this.ParentOrder.Logs.Add(string.Format("开始元数据入库。"));
            MetaDataReader mdReader = new MetaDataReader();
            MetaDataHj hjMeta = mdReader.ReadMetaDataHj(sourceFilePath, "-1");
            string insertSql = "";
            hjMeta.OverviewFilePath = StorageBasePath.StorePath_SourceProject(sourceFilePath) + Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sourceFilePath))+".JPG";
            MetaDataDBImporter mdDBImporter = new MetaDataDBImporter();
            mdDBImporter.ImportData(EnumMetadataTypes.HJ, hjMeta, sourceFilePath, TServerSiteManager.ConvertTSSiteIP2TSSiteCode(this.ParentOrder.TSSiteIP), out insertSql);
            this.ParentOrder.OrderParams[0] = hjMeta.QRST_CODE;

            //添加日期：2013/05/21 zxw
            this.ParentOrder.Logs.Add(string.Format("录入元数据公共信息。"));
            MetaDataStandardPublicInfo publicinfo = new MetaDataStandardPublicInfo();
            publicinfo.DataSize = new FileInfo(this.ProcessArgu[0]).Length / (1024 * 1024);
            publicinfo.DataCode = hjMeta.QRST_CODE;
            IDbOperating sqLiteOperating = Constant.IdbOperating;
            IDbBaseUtilities baseUtilities = sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
            MetaDataStandardPublicInfo.Add(publicinfo, baseUtilities);
            this.ParentOrder.Logs.Add(string.Format("完成元数据公共信息录入。"));
    
            //QDB_Base.Sys.LogUtils.ConsoleWriteOutLine(insertSql, order.Code);
            this.ParentOrder.Logs.Add(string.Format("完成元数据入库。"));
        }


    }
}
