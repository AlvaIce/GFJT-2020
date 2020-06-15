using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IODOCDataImport
    {
        /// <summary>
        /// Installed Order for GF1 Data Import
        /// OrderParams描述
        /// OrderParams[0]为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass Create(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, string DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, string UPLOADTIME, string FILESIZE, string sharePath)//string[] orderParas
        {
            OrderClass DOCdataImportOrder = new OrderClass();
            DOCdataImportOrder.OrderParams = new string[] {  TITLE, DOCTYPE, KEYWORD, ABSTRACT, DOCDATE.ToString(), DESCRIPTION, AUTHOR, UPLOADER, UPLOADTIME.ToString(), FILESIZE.ToString(), sharePath ,""};

            //参数：SourceFilePath
            DOCdataImportOrder.Type = EnumOrderType.Installed;
            DOCdataImportOrder.OrderCode = OrderManager.GetNewCode();

            //DOCdataImportOrder.Tasks.Add(new ITCreateHJDataInputWSP());
            //DOCdataImportOrder.TaskParams.Add(new string[] { "" });

            //  日志消息
            DOCdataImportOrder.Tasks.Add(new ITOutputLoginfo());
            DOCdataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
        
            //读取元数据task
            DOCdataImportOrder.Tasks.Add(new ITInsertDocMetaData());
            DOCdataImportOrder.TaskParams.Add(new string[] { TITLE, DOCTYPE, KEYWORD, ABSTRACT, DOCDATE.ToString(), DESCRIPTION, AUTHOR, UPLOADER, UPLOADTIME.ToString(), FILESIZE.ToString() });
           
            //拷贝文件task
            DOCdataImportOrder.Tasks.Add(new ITImportDocFile());
            DOCdataImportOrder.TaskParams.Add(new string[] { sharePath });

          
            //  日志消息
            DOCdataImportOrder.Tasks.Add(new ITOutputLoginfo());
            DOCdataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return DOCdataImportOrder;

        }
    }
}
