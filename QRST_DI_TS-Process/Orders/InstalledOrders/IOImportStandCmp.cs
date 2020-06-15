using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOImportStandCmp
    {
        /// <summary>
        /// 用于导入用户上传数据
        /// </summary>
        /// <returns></returns>
         public OrderClass CreateStandCmpImport(string dataPath)
        {
            OrderClass orderClass = new OrderClass() { Type = EnumOrderType.Installed, OrderCode = OrderManager.GetNewCode(), /*订单赋参*/OrderParams = new string[] { dataPath } };
                        //解压算法组件包
            orderClass.Tasks.Add(new ITUnzipTarFile());
            orderClass.TaskParams.Add(new string[] { dataPath});

            //提取元数据，执行数据导入
            orderClass.Tasks.Add(new ITImportStandardCmp());
            orderClass.TaskParams.Add(new string[] { dataPath });

            return orderClass;
        }
    }
}
