using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{ 
    public class IODownLoadByP2P
    {
        /// <summary>
        /// 用于下载用户数据
        /// </summary>
        /// <returns></returns>
        public OrderClass CreateIODownLoadByP2P(string dataname, string sharepath, string opID)
        {
            return CreateIODownLoadByP2P(dataname, sharepath, opID,"");
        }
        public OrderClass CreateIODownLoadByP2P(string dataname, string sharepath, string opID, string webserviceIp)
        {
            OrderClass orderClass = new OrderClass();
            orderClass.Type = EnumOrderType.Installed;
            orderClass.OrderCode = OrderManager.GetNewCode();
            //订单赋参
            orderClass.OrderParams = new string[] { dataname, sharepath, opID, webserviceIp };

            //获取元数据文件
            orderClass.Tasks.Add(new ITDownLoadDataP2P());
            orderClass.TaskParams.Add(new string[] { dataname, sharepath, opID, webserviceIp });



            return orderClass;
        }
    }
}
