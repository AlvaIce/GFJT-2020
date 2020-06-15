using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    class IODownLoadTiles
	{
		public OrderClass CreateDownLoadTiles(string dataID, string pathID, string opID,string tilesIp)
		{
			OrderClass orderClass = new OrderClass();
			orderClass.Type = EnumOrderType.Installed;
			orderClass.TSSiteIP = tilesIp;
			orderClass.OrderCode = OrderManager.GetNewCode();

			orderClass.SetOrderWorkspace = "%OrderWorkspace%";
			//订单赋参
			//orderClass.OrderParams = new string[] { dataID, pathID, opID };

			//获取元数据文件
			orderClass.Tasks.Add(new ITDownLoadTiles());
			orderClass.TaskParams.Add(new string[] { dataID, pathID, opID });
			return orderClass;
		}
	}
}
