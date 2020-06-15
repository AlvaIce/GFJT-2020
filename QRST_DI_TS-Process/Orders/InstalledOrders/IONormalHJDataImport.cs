using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IONormalHJDataImport
    {
        public OrderClass CreateNormalHJdataImportOrder(string SourceFilePath, string orderworkspace)
        {
            OrderClass HJdataImportOrder = new OrderClass();
            HJdataImportOrder.Type = EnumOrderType.Installed;
            HJdataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            HJdataImportOrder.Tasks.Add(new ITOutputLoginfo());
            HJdataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            HJdataImportOrder.OrderParams = new string[] { SourceFilePath, orderworkspace };
            HJdataImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置         

            //  元数据提取入库
            HJdataImportOrder.Tasks.Add(new ITInsertNormalHJMetaData());
            HJdataImportOrder.TaskParams.Add(new string[] { SourceFilePath });

            //图片及缩略图入库
            HJdataImportOrder.Tasks.Add(new ITCreateHJImage());
            HJdataImportOrder.TaskParams.Add(new string[] { SourceFilePath,HJdataImportOrder.OrderWorkspace });

            //文件入库
            HJdataImportOrder.Tasks.Add(new ITCopyFiles());
            HJdataImportOrder.TaskParams.Add(new string[] { SourceFilePath, StorageBasePath.StorePath_SourceProject(SourceFilePath) + Path.GetFileName(SourceFilePath) });
            return HJdataImportOrder;
        }
    }
}
