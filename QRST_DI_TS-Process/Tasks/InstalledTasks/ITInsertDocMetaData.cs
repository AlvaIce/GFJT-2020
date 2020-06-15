using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITInsertDocMetaData:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITInsertDocMetaData"; }
            set { }
        }
        public string code = "";
        public override void Process()//这有个问题读取ReadMetaDataDOC是读取什么数据在metadatareader类里面需要添加这个读取方法用哪一个形式
        {    
            //  读元数据
            this.ParentOrder.Logs.Add(string.Format("开始元数据入库。"));     
            MetaDataDoc docMeta = new MetaDataDoc();
            docMeta.TITLE = ProcessArgu[0];
            docMeta.DOCTYPE = ProcessArgu[1];
            docMeta.KEYWORD = ProcessArgu[2];
            docMeta.ABSTRACT = ProcessArgu[3];
            //docMeta.DOCDATE = DateTime.Parse(ProcessArgu[4]);
            docMeta.DOCDATE = ProcessArgu[4];
            docMeta.DESCRIPTION = ProcessArgu[5];
            docMeta.AUTHOR = ProcessArgu[6];
            docMeta.UPLOADER = ProcessArgu[7];
            //docMeta.UPLOADTIME = DateTime.Parse(ProcessArgu[8]);
            docMeta.UPLOADTIME = ProcessArgu[8];
            docMeta.FILESIZE = ProcessArgu[9];

            //元数据入库
            IDbOperating sqLiteOperating = Constant.IdbOperating;
            IDbBaseUtilities baseUtilities = sqLiteOperating.GetSubDbUtilities(EnumDBType.ISDB);
            //DBMySqlOperating mySQLOperator = new DBMySqlOperating();
            docMeta.ImportData(baseUtilities);

            this.ParentOrder.OrderParams[11] = docMeta.QRST_CODE;
       
            this.ParentOrder.Logs.Add(string.Format("完成元数据入库。"));

                    
        }
    }
}
