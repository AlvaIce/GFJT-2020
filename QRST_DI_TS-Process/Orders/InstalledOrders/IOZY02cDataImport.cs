using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Basis.DirectlyAddress;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOZY02cDataImport
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
            OrderClass ZY02CImportOrder = new OrderClass();
            //参数：SourceFilePath
            ZY02CImportOrder.Type = EnumOrderType.Installed;
            ZY02CImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            ZY02CImportOrder.Tasks.Add(new ITOutputLoginfo());
            ZY02CImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            ZY02CImportOrder.Tasks.Add(new ITCreateHJDataInputWSP());
            ZY02CImportOrder.TaskParams.Add(new string[] { "" });
            //由第三方拷贝数据到工作空间

            //  文件解压
            ZY02CImportOrder.Tasks.Add(new ITUnzipZY3targzXmlJpg());
            ZY02CImportOrder.TaskParams.Add(new string[] { ZY02CImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(ZY02CImportOrder.OrderWorkspace) });

            //源数据文件入库
            ZY02CImportOrder.Tasks.Add(new ITZY02cDataImport());
            ZY02CImportOrder.TaskParams.Add(new string[] { ZY02CImportOrder.OrderWorkspace });

            //  日志消息
            ZY02CImportOrder.Tasks.Add(new ITOutputLoginfo());
            ZY02CImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return ZY02CImportOrder;

        }
    }
}
