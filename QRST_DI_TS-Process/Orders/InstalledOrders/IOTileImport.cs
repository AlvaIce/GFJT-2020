using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    class IOTileImport
	{
		public OrderClass CreateIOTileImportOrder(string param)
		{
			OrderClass IOTileImport = new OrderClass();
			//参数：SourceFilePath
			IOTileImport.Type = EnumOrderType.Installed;
			IOTileImport.OrderCode = OrderManager.GetNewCode();
			//IOTileImport.SetOrderWorkspace = param;
			//设置任务,任务赋参
			//  日志消息
			IOTileImport.Tasks.Add(new ITOutputLoginfo());
			IOTileImport.TaskParams.Add(new string[] { "开始订单任务！" });
			//  切片入库(建索引)
			IOTileImport.Tasks.Add(new ITStoreTilesNew());
			IOTileImport.TaskParams.Add(new string[] { param });
			//  日志消息
			IOTileImport.Tasks.Add(new ITOutputLoginfo());
			IOTileImport.TaskParams.Add(new string[] { "完成数据入库" });

			return IOTileImport;
		}
	}
}
