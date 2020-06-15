using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Basis.DirectlyAddress;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    /// <summary>
    /// 新版本的环境星入库 20130903
    /// </summary>
    public class IOHJDataImportStandard
    {

        /// <summary>
        /// Installed Order for GF1 Data Import
        /// OrderParams描述
        /// OrderParams[0]为待导入数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass Create()
        {
            OrderClass HJdataImportOrder = new OrderClass();
            //参数：SourceFilePath
            HJdataImportOrder.Type = EnumOrderType.Installed;
            HJdataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            HJdataImportOrder.Tasks.Add(new ITOutputLoginfo());
            HJdataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            HJdataImportOrder.Tasks.Add(new ITCreateHJDataInputWSP());
            HJdataImportOrder.TaskParams.Add(new string[] { "" });
            //由第三方拷贝数据到工作空间

            //  文件解压
            HJdataImportOrder.Tasks.Add(new ITUnzipHJtargz4XmlJpg());
            HJdataImportOrder.TaskParams.Add(new string[] { HJdataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(HJdataImportOrder.OrderWorkspace) });

            //源数据文件入库
            HJdataImportOrder.Tasks.Add(new ITHJDataImport());
            HJdataImportOrder.TaskParams.Add(new string[] { HJdataImportOrder.OrderWorkspace });

            //  日志消息
            HJdataImportOrder.Tasks.Add(new ITOutputLoginfo());
            HJdataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return HJdataImportOrder;

        }
    }
}
