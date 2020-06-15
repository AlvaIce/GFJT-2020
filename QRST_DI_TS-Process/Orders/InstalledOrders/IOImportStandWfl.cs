using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOImportStandWfl
    {
        /// <summary>
        /// 导入标准产品流程
        /// </summary>
        /// <returns></returns>
        public OrderClass CreateStandWflImport(string dataPath)
        {
            OrderClass orderClass = new OrderClass() { Type = EnumOrderType.Installed, OrderCode = OrderManager.GetNewCode(), /*订单赋参*/OrderParams = new string[] { dataPath } };

            //解压算法组件包
            orderClass.Tasks.Add(new ITUnzipTarFile());
            orderClass.TaskParams.Add(new string[] { dataPath });

            //提取元数据，执行数据导入
            orderClass.Tasks.Add(new ITImportStandWfl());
            orderClass.TaskParams.Add(new string[] { dataPath });

            return orderClass;
        }
    }
}
