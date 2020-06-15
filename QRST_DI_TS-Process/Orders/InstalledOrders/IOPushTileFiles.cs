using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOPushTileFiles
    {
        /// <summary>
        /// 推送查询到的瓦片数据
        /// OrderParams描述
        /// OrderParams[0]为待导入数据存放路径
        /// @zhangfeilong
        /// 2014-10-10
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public OrderClass Create(string tilenames, string originalOrderCode)
        {
            OrderClass TileDataPushOrder = new OrderClass();
            TileDataPushOrder.OrderParams = new string[] { tilenames, originalOrderCode };

            //参数：SourceFilePath
            TileDataPushOrder.Type = EnumOrderType.Installed;
            TileDataPushOrder.OrderCode = OrderManager.GetNewCode();

            //设置任务,任务赋参
            //  日志消息
            TileDataPushOrder.Tasks.Add(new ITOutputLoginfo());
            TileDataPushOrder.TaskParams.Add(new string[] { "开始执行推送查询瓦片数据任务" });

            //将存放瓦片数据的临时文件夹进行打包
            TileDataPushOrder.Tasks.Add(new ITFetchAndZipTileFiles());
            TileDataPushOrder.TaskParams.Add(new string[] { tilenames, originalOrderCode });

            //  日志消息
            TileDataPushOrder.Tasks.Add(new ITOutputLoginfo());
            TileDataPushOrder.TaskParams.Add(new string[] { "完成推送查询瓦片数据任务" });

            return TileDataPushOrder;

        }
    }
}
