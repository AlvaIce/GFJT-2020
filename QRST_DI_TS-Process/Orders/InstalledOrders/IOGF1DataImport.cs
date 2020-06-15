using System.Collections.Generic;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOGF1DataImport 
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
            OrderClass GF1dataImportOrder = new OrderClass();
           
            //参数：SourceFilePath
            GF1dataImportOrder.Type = EnumOrderType.Installed;
            GF1dataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            GF1dataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF1dataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            GF1dataImportOrder.Tasks.Add(new ITCreateHJDataInputWSP());
            GF1dataImportOrder.TaskParams.Add(new string[] {""});
            //由第三方拷贝数据到工作空间

            //  文件解压
            GF1dataImportOrder.Tasks.Add(new ITUnzipGFtargz4XmlJpg());
            string test = StorageBasePath.SharePath_OrignalData(GF1dataImportOrder.OrderWorkspace);
            GF1dataImportOrder.TaskParams.Add(new string[] { GF1dataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(GF1dataImportOrder.OrderWorkspace) });

            //质量检测
            GF1dataImportOrder.Tasks.Add(new ITDataQualityInspection());
            GF1dataImportOrder.TaskParams.Add(new string[] { GF1dataImportOrder.OrderWorkspace });

            //源数据文件入库
            GF1dataImportOrder.Tasks.Add(new ITGFDataImport());
            GF1dataImportOrder.TaskParams.Add(new string[] { GF1dataImportOrder.OrderWorkspace });

            //姜斌插入的task，在入库数据的同时，向判定程序发送入库的影像信息
            //GF1dataImportOrder.Tasks.Add(new ITSendGF1Message());
            //GF1dataImportOrder.TaskParams.Add(new string[] { GF1dataImportOrder.OrderWorkspace });
                 
            //  日志消息
            GF1dataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF1dataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return GF1dataImportOrder;

        }

        /// <summary>
        /// 创建批量环境星数据的准备订单
        /// </summary>
        /// <param name="sourceFileDirectory">环境星数据的存放文件夹</param>
        /// <returns></returns>
        public List<OrderClass> CreateBatchOrder(string sourceFileDirectory)
        {
            List<OrderClass> orderClassLst = new List<OrderClass>();
            string [] sourceFiles = Directory.GetFiles(sourceFileDirectory);
            for (int i = 0 ; i < sourceFiles.Length ;i++ )
            {
                if (sourceFiles[i].EndsWith(".tar.gz") || sourceFiles[i].EndsWith(".TAR.GZ") || sourceFiles[i].EndsWith(".TAR.gz"))
                  orderClassLst.Add(Create());
            }
            return orderClassLst;
        }

    }
}

