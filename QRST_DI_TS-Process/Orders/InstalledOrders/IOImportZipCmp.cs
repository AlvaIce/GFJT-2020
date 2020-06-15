using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    /// <summary>
    /// 导入zip包文件
    /// </summary>
    /// <returns></returns>
    public class IOImportZipCmp
    {
        public OrderClass CreateZipCmpImportOrder(string dataPath)
        {
            OrderClass orderClass = new OrderClass() { Type = EnumOrderType.Installed, OrderCode = OrderManager.GetNewCode(), /*订单赋参*/OrderParams = new string[] { dataPath } };

            //解压算法组件包
            orderClass.Tasks.Add(new ITUnzipZipFile());
            orderClass.TaskParams.Add(new string[] { dataPath });

            //提取元数据，执行数据导入
            orderClass.Tasks.Add(new ITImportZipCmp());
            orderClass.TaskParams.Add(new string[] { dataPath });

            return orderClass;
        }
    }
}
