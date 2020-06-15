using QRST_DI_TS_Process.Tasks.InstalledTasks;


namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IODownLoadDOC
    {
        public OrderClass CreateDownLoadDOC(string qrst_code, string destPath, string mouid)
        {
            OrderClass orderClass = new OrderClass();
            orderClass.Type = EnumOrderType.Installed;
            orderClass.OrderCode = OrderManager.GetNewCode();
            //订单赋参
            orderClass.OrderParams = new string[] { qrst_code, destPath, mouid };

            //获取元数据文件
            orderClass.Tasks.Add(new ITDownLoadDOC());
            orderClass.TaskParams.Add(new string[] { qrst_code, destPath, mouid });



            return orderClass;
        
        }
    }
}
