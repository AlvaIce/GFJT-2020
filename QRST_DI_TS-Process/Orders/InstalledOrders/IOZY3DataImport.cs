using System.Collections.Generic;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.IO;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOZY3DataImport
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
            OrderClass ZY3dataImportOrder = new OrderClass();
            //参数：SourceFilePath
            ZY3dataImportOrder.Type = EnumOrderType.Installed;
            ZY3dataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            ZY3dataImportOrder.Tasks.Add(new ITOutputLoginfo());
            ZY3dataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            ZY3dataImportOrder.Tasks.Add(new ITCreateHJDataInputWSP());
            ZY3dataImportOrder.TaskParams.Add(new string[] { "" });
            //由第三方拷贝数据到工作空间

            //  文件解压
            ZY3dataImportOrder.Tasks.Add(new ITUnzipZY3targzXmlJpg());
            ZY3dataImportOrder.TaskParams.Add(new string[] { ZY3dataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(ZY3dataImportOrder.OrderWorkspace) });

            //源数据文件入库
            ZY3dataImportOrder.Tasks.Add(new ITZY3DataImport());
            ZY3dataImportOrder.TaskParams.Add(new string[] { ZY3dataImportOrder.OrderWorkspace });

            //  日志消息
            ZY3dataImportOrder.Tasks.Add(new ITOutputLoginfo());
            ZY3dataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return ZY3dataImportOrder;

        }

        /// <summary>
        /// 创建批量ZY3星数据的准备订单
        /// </summary>
        /// <param name="sourceFileDirectory">ZY3星数据的存放文件夹</param>
        /// <returns></returns>
        public List<OrderClass> CreateBatchOrder(string sourceFileDirectory)
        {
            List<OrderClass> orderClassLst = new List<OrderClass>();
            string[] sourceFiles = Directory.GetFiles(sourceFileDirectory);
            for (int i = 0; i < sourceFiles.Length; i++)
            {
                if (sourceFiles[i].EndsWith(".tar") || sourceFiles[i].EndsWith(".TAR"))
                    orderClassLst.Add(Create());
            }
            return orderClassLst;
        }
    }
}
