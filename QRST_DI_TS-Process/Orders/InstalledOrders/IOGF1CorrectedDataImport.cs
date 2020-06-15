using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    /// <summary>
    /// 导入高分一号数据预处理后的数据(校正数据与切片数据)
    /// 处理流程：1. 在数据库中查找未处理的的原始数据
    /// 2. 导出原始数据的QRST_CODE
    /// 3.本订单负责根据QRST_CODE为预处理准备生产环境，待生产完成以后将校正数据与切片数据入库
    /// </summary>
    public class IOGF1CorrectedDataImport
    {
        /// <summary>
        /// Installed Order for GF1 Data Preparing
        /// OrderParams描述
        /// OrderParams[0]为待预处理数据存放路径
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass CreateGF1CorrectedDataImportOrder(string qrst_code)
        {
            OrderClass GF1CorrectedDataImportOrder = new OrderClass();
            //参数：SourceFilePath
            GF1CorrectedDataImportOrder.Type = EnumOrderType.Installed;
            GF1CorrectedDataImportOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            GF1CorrectedDataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
            //  创建文件夹工作空间
            GF1CorrectedDataImportOrder.Tasks.Add(new ITCreateDataInputWSP());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { "" });
            //根据QRST_CODE找到数据路径，将数据放置到工作空间
            GF1CorrectedDataImportOrder.Tasks.Add(new ITPerareGF1Data());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { qrst_code, GF1CorrectedDataImportOrder.OrderWorkspace });
            //  文件解压
            GF1CorrectedDataImportOrder.Tasks.Add(new ITUnzipHJtargz());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { GF1CorrectedDataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(GF1CorrectedDataImportOrder.OrderWorkspace) });

            //源数据文件入库
            //GF1CorrectedDataImportOrder.Tasks.Add(new ITGFDataImport());
            //GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { GF1CorrectedDataImportOrder.OrderWorkspace });

            //  告知预处理
            GF1CorrectedDataImportOrder.Tasks.Add(new ITSendMessage());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { Constant.CorrectRecieveIP,
               string.Format("{0}?{1}?@NoticePreprocess", GF1CorrectedDataImportOrder.OrderCode, StorageBasePath.SharePath_OrignalData(GF1CorrectedDataImportOrder.OrderWorkspace))});

            //  切片入库(建索引)
            GF1CorrectedDataImportOrder.Tasks.Add(new ITStoreTilesNew());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { StorageBasePath.SharePath_TiledData(GF1CorrectedDataImportOrder.OrderWorkspace) });
            //  校正文件入库
            GF1CorrectedDataImportOrder.Tasks.Add(new ITStoreGFCorData());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { GF1CorrectedDataImportOrder.OrderWorkspace });

            //  日志消息
            GF1CorrectedDataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF1CorrectedDataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return GF1CorrectedDataImportOrder;

        }
    }
}
