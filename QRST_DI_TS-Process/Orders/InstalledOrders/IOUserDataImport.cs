using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOUserDataImport
    {
        /// <summary>
        /// 用于导入用户上传数据
        /// </summary>
        /// <returns></returns>
        public OrderClass CreateUserDataImport(string dataType,string pathID,string opID)
        {
            OrderClass orderClass = new OrderClass();
            orderClass.Type = EnumOrderType.Installed;
            orderClass.OrderCode = OrderManager.GetNewCode();
            //订单赋参
            orderClass.OrderParams = new string[] { dataType,pathID,opID };

            //获取元数据文件
            orderClass.Tasks.Add(new ITGetMetaDataFile());
            orderClass.TaskParams.Add(new string[] { pathID,opID });

            //提取元数据，执行数据导入
            orderClass.Tasks.Add(new ITImportUserData());
            orderClass.TaskParams.Add(new string[] { dataType, pathID ,opID});

            return orderClass;
        }
    }
}
