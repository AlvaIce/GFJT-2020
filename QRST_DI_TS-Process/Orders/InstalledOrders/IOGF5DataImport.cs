using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOGF5DataImport
    {
        /// <summary>
        /// 创建GF5数据入库订单
        /// </summary>
        /// <returns>GF5数据入库订单</returns>
        public OrderClass Create(string filename)
        {
            //创建GF5数据入库订单
            OrderClass GF5dataImportOrder = new OrderClass();
            //设置GF1数据入库订单类型为UpLoad
            GF5dataImportOrder.Type = EnumOrderType.Installed;
            //随机生成GF5数据入库订单编号
            GF5dataImportOrder.OrderCode = OrderManager.GetNewCode();
            //设置GF5数据入库订单任务，并给每个任务赋参
            //添加日志记录任务
            GF5dataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF5dataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });

            //添加创建GF5数据入库订单工作空间任务
            //待入库的GF5数据由用户拷贝到创建的订单工作空间中
            GF5dataImportOrder.Tasks.Add(new ITCreateGF5DataWorkspace());
            GF5dataImportOrder.TaskParams.Add(new string[] { "" });

            //添加解压tar.gz格式的压缩包的任务
            //参数1为待解压文件的路径；参数2为解压后文件的存储路径
            GF5dataImportOrder.Tasks.Add(new ITUnzipTargz4XmlJpg());
            GF5dataImportOrder.TaskParams.Add(new string[] { GF5dataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(GF5dataImportOrder.OrderWorkspace) });

            //添加GF5数据质量检测的任务
            //参数为GF5数据入库订单工作空间
            GF5dataImportOrder.Tasks.Add(new ITGF5DataQualityInspection());
            GF5dataImportOrder.TaskParams.Add(new string[] { GF5dataImportOrder.OrderWorkspace, filename });

            //添加GF5数据入库的任务
            //参数为GF5数据入库订单工作空间
            GF5dataImportOrder.Tasks.Add(new ITGF5DataImport());
            GF5dataImportOrder.TaskParams.Add(new string[] { GF5dataImportOrder.OrderWorkspace, filename });

            //添加日志记录任务
            GF5dataImportOrder.Tasks.Add(new ITOutputLoginfo());
            GF5dataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

            return GF5dataImportOrder;

        }
    }
}
