using System;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    class IOHHALOSDataImport
    {
        public OrderClass CreateHHALOSImportOrder(string excelFilePath, string orderworkspace, string sourceFileDir)
        {
            OrderClass HHALOSImportOrder = new OrderClass();
            HHALOSImportOrder.Type = EnumOrderType.Installed;
            HHALOSImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            HHALOSImportOrder.Tasks.Add(new ITOutputLoginfo());
            HHALOSImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            HHALOSImportOrder.OrderParams = new string[] { excelFilePath, orderworkspace, sourceFileDir };
            HHALOSImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  元数据提取入库
            HHALOSImportOrder.Tasks.Add(new ITInsertHHAlosMetaData());
            HHALOSImportOrder.TaskParams.Add(new string[] { excelFilePath });

            //  源数据文件入库
            HHALOSImportOrder.Tasks.Add(new ITImportRasterFiles());
            //设置文件在数据阵列里面的路径,\\172.16.0.185\综合数据库\基础空间数据库\DEM\全国范围30米分辨率DEM数据\
            string dir = String.Format("{0}", (new StoragePath("BSDB-2-20")).getGroupAddress());
            HHALOSImportOrder.TaskParams.Add(new string[] { sourceFileDir, dir });

            return HHALOSImportOrder;
        }
    }
}
