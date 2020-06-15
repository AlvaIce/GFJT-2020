using System.Collections.Generic;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOGF1dataPrepare 
    {
        /// <summary>
        /// Installed Order for GF1 Data Preparing
        /// OrderParams描述
        /// OrderParams[0]为待预处理数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateGF1dataPrepareOrder()
        {
            OrderClass GF1dataPrepareOrder = new OrderClass();
            //参数：SourceFilePath
            GF1dataPrepareOrder.Type = EnumOrderType.Installed;
            GF1dataPrepareOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            GF1dataPrepareOrder.Tasks.Add(new ITOutputLoginfo());
            GF1dataPrepareOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            GF1dataPrepareOrder.Tasks.Add(new ITCreateHJDataInputWSP());
            GF1dataPrepareOrder.TaskParams.Add(new string[] {""});
            //由第三方拷贝数据到工作空间
            //  文件解压
            GF1dataPrepareOrder.Tasks.Add(new ITUnzipHJtargz());
            GF1dataPrepareOrder.TaskParams.Add(new string[] { GF1dataPrepareOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(GF1dataPrepareOrder.OrderWorkspace) });

            //源数据文件入库
            GF1dataPrepareOrder.Tasks.Add(new ITGFDataImport());
            GF1dataPrepareOrder.TaskParams.Add(new string[] { GF1dataPrepareOrder.OrderWorkspace });

            //  告知预处理
            GF1dataPrepareOrder.Tasks.Add(new ITSendMessage());
            GF1dataPrepareOrder.TaskParams.Add(new string[] { Constant.CorrectRecieveIP,
               string.Format("{0}?{1}?@NoticePreprocess", GF1dataPrepareOrder.OrderCode, StorageBasePath.SharePath_OrignalData(GF1dataPrepareOrder.OrderWorkspace))});
          
            //  切片入库(建索引)
            GF1dataPrepareOrder.Tasks.Add(new ITStoreTilesNew());
            GF1dataPrepareOrder.TaskParams.Add(new string[] { StorageBasePath.SharePath_TiledData(GF1dataPrepareOrder.OrderWorkspace) });
            //  校正文件入库
            GF1dataPrepareOrder.Tasks.Add(new ITStoreGFCorData());
            GF1dataPrepareOrder.TaskParams.Add(new string[] { GF1dataPrepareOrder.OrderWorkspace });

            //  日志消息
            GF1dataPrepareOrder.Tasks.Add(new ITOutputLoginfo());
            GF1dataPrepareOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return GF1dataPrepareOrder;

        }

        /// <summary>
        /// 创建批量环境星数据的准备订单
        /// </summary>
        /// <param name="sourceFileDirectory">环境星数据的存放文件夹</param>
        /// <returns></returns>
        public List<OrderClass> CreateBatchHJdataPrepareOrder(string sourceFileDirectory)
        {
            List<OrderClass> orderClassLst = new List<OrderClass>();
            string [] sourceFiles = Directory.GetFiles(sourceFileDirectory);
            for (int i = 0 ; i < sourceFiles.Length ;i++ )
            {
                if (sourceFiles[i].EndsWith(".tar.gz") || sourceFiles[i].EndsWith(".TAR.GZ") || sourceFiles[i].EndsWith(".TAR.gz"))
                  orderClassLst.Add(CreateGF1dataPrepareOrder());
            }
            return orderClassLst;
        }

    }
}
