using System;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    class IOZJCoastlandALOSDataImport
    {
        public OrderClass CreateZJCoastlandALOSImportOrder(string excelFilePath, string orderworkspace, string sourceFileDir)
        {
            OrderClass ZJCoastlandALOSImportOrder = new OrderClass();
            ZJCoastlandALOSImportOrder.Type = EnumOrderType.Installed;
            ZJCoastlandALOSImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            ZJCoastlandALOSImportOrder.Tasks.Add(new ITOutputLoginfo());
            ZJCoastlandALOSImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            ZJCoastlandALOSImportOrder.OrderParams = new string[] { excelFilePath, orderworkspace, sourceFileDir };
            ZJCoastlandALOSImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置

            //  元数据提取入库
            ZJCoastlandALOSImportOrder.Tasks.Add(new ITInsertTZJCoastlandAlosMetaData());
            ZJCoastlandALOSImportOrder.TaskParams.Add(new string[] { excelFilePath });

            //  源数据文件入库
            ZJCoastlandALOSImportOrder.Tasks.Add(new ITImportRasterFiles());
            //设置文件在数据阵列里面的路径,\\172.16.0.185\综合数据库\基础空间数据库\DEM\全国范围30米分辨率DEM数据\
            string dir = String.Format("{0}", (new StoragePath("BSDB-2-22")).getGroupAddress());
            ZJCoastlandALOSImportOrder.TaskParams.Add(new string[] { sourceFileDir, dir });

            return ZJCoastlandALOSImportOrder;
        }
    }
}
