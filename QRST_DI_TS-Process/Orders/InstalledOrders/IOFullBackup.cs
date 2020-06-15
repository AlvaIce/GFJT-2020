using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOFullBackup
    {
        public OrderClass Create()
        {
            OrderClass FullBackupOrder  = new OrderClass();

            //参数：SourceFilePath
            FullBackupOrder.Type = EnumOrderType.Installed;
            FullBackupOrder.OrderCode = OrderManager.GetNewCode();

            FullBackupOrder.Tasks.Add(new ITFullBackup());
            FullBackupOrder.TaskParams.Add(new string[] {"正在进行数据库备份"});

            return FullBackupOrder;

        }
    }
}
