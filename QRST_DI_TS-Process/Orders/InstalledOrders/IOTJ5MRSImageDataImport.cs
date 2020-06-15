using System;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    class IOTJ5MRSImageDataImport
    {
        public OrderClass CreateTJ5MRSImageImportOrder(string excelFilePath, string orderworkspace, string sourceFileDir)
        {
            OrderClass TJ5MRSImageImportOrder = new OrderClass();
            TJ5MRSImageImportOrder.Type = EnumOrderType.Installed;
            TJ5MRSImageImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            TJ5MRSImageImportOrder.Tasks.Add(new ITOutputLoginfo());
            TJ5MRSImageImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            TJ5MRSImageImportOrder.OrderParams = new string[] { excelFilePath, orderworkspace, sourceFileDir };
            TJ5MRSImageImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  元数据提取入库
            TJ5MRSImageImportOrder.Tasks.Add(new ITInsertTJ5MRSImageMetaData());
            TJ5MRSImageImportOrder.TaskParams.Add(new string[] { excelFilePath });

            //  源数据文件入库
            TJ5MRSImageImportOrder.Tasks.Add(new ITImportRasterFiles());
            //设置文件在数据阵列里面的路径,\\172.16.0.185\综合数据库\基础空间数据库\DEM\全国范围30米分辨率DEM数据\
            string dir = String.Format("{0}", (new StoragePath("BSDB-2-24")).getGroupAddress());
            TJ5MRSImageImportOrder.TaskParams.Add(new string[] { sourceFileDir, dir });

            return TJ5MRSImageImportOrder;
        }
    }
}
