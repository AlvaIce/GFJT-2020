using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IONormalDataImport
    {
        public OrderClass Create(string dataCode,string pluginParas)
        {
            OrderClass normaldataImportOrder = new OrderClass();

            normaldataImportOrder.Type = EnumOrderType.Installed;
            normaldataImportOrder.OrderCode = OrderManager.GetNewCode();

            normaldataImportOrder.Tasks.Add(new ITNormalDataImport());
            normaldataImportOrder.TaskParams.Add(new string[]{dataCode,pluginParas});

            return normaldataImportOrder;

        }
    }
}
