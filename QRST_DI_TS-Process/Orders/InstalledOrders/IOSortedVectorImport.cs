using System;
using System.IO;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOSortedVectorImport
    {
        public OrderClass createSortedVectorDataImport(string excelFilePath, string shpFilePath, string orderworkspace, string groupcode, string index)
        {
            string shpFileDir = Path.GetDirectoryName(shpFilePath);
            OrderClass vectordataImportOrder = new OrderClass();
            vectordataImportOrder.Type = EnumOrderType.Installed;
            vectordataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            vectordataImportOrder.Tasks.Add(new ITOutputLoginfo());
            vectordataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            vectordataImportOrder.OrderParams = new string[] {  excelFilePath,  shpFilePath,  orderworkspace,  groupcode, index };
            vectordataImportOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置         

            //  元数据提取入库
            vectordataImportOrder.Tasks.Add(new ITInsertSortedVectorMeta());
            vectordataImportOrder.TaskParams.Add(new string[] { excelFilePath, groupcode, index.ToString() });

            //文件入库
            vectordataImportOrder.Tasks.Add(new ITImportVectorFiles());
            string dataname = Path.GetFileNameWithoutExtension(shpFilePath);
            //设置文件在数据阵列里面的路径,\\172.16.0.185\综合数据库\基础空间数据库\地形图\全国1：5万道路、水系、行政区划基础地理数据\数据名称\
            string dir = String.Format("{0}{1}\\", (new StoragePath(groupcode)).getGroupAddress(), dataname);
            vectordataImportOrder.TaskParams.Add(new string[] { shpFilePath, dir });
            return vectordataImportOrder;
        }
    }
}
