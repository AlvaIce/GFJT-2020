using System.IO;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    class IOCopyAssistFiles
    {
        public OrderClass createCopyAssistFiles(string SourceFileDir, string orderworkspace, string groupcode)
        {
            OrderClass copyAssistFilesOrder = new OrderClass();
            copyAssistFilesOrder.Type = EnumOrderType.Installed;
            copyAssistFilesOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            copyAssistFilesOrder.Tasks.Add(new ITOutputLoginfo());
            copyAssistFilesOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //参数：SourceFilePath
            copyAssistFilesOrder.OrderParams = new string[] { SourceFileDir, orderworkspace, groupcode};
            copyAssistFilesOrder.SetOrderWorkspace = orderworkspace;   //重置工作空间 到数据准备订单位置         

            //文件入库
            //设置文件在数据阵列里面的路径,\\172.16.0.185\综合数据库\基础空间数据库\地形图\全国1：5万道路、水系、行政区划基础地理数据\
            string dir = (new StoragePath(groupcode)).getGroupAddress();
            foreach (string file in Directory.GetFiles(SourceFileDir))
            {
                copyAssistFilesOrder.Tasks.Add(new ITCopyFiles());
                string destFile = dir + Path.GetFileName(file);
                copyAssistFilesOrder.TaskParams.Add(new string[] { file, destFile });
            }
            
            return copyAssistFilesOrder;
        }
    }
}
