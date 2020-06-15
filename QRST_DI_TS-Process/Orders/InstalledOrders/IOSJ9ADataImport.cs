using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Basis.DirectlyAddress;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOSJ9ADataImport
	{
		/// <summary>
		/// Installed Order for GF1 Data Import
		/// OrderParams描述
		/// OrderParams[0]为待导入数据存放路径
		/// </summary>
		/// <param name="SourceFilePath"></param>
		/// <returns></returns>
		public OrderClass Create()
		{
			OrderClass SJ9ADataImportOrder = new OrderClass();

			//参数：SourceFilePath
			SJ9ADataImportOrder.Type = EnumOrderType.Installed;
			SJ9ADataImportOrder.OrderCode = OrderManager.GetNewCode();

			//设置任务,任务赋参
			//  日志消息
			SJ9ADataImportOrder.Tasks.Add(new ITOutputLoginfo());
			SJ9ADataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
			//  创建文件夹工作空间
			SJ9ADataImportOrder.Tasks.Add(new ITCreateHJDataInputWSP());
			SJ9ADataImportOrder.TaskParams.Add(new string[] { "" });
			//由第三方拷贝数据到工作空间

			//  文件解压
			SJ9ADataImportOrder.Tasks.Add(new ITUnzipGFtargz4XmlJpg());
			SJ9ADataImportOrder.TaskParams.Add(new string[] { SJ9ADataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(SJ9ADataImportOrder.OrderWorkspace) });

			//源数据文件入库
			SJ9ADataImportOrder.Tasks.Add(new ITSJ9ADataImport());
			SJ9ADataImportOrder.TaskParams.Add(new string[] { SJ9ADataImportOrder.OrderWorkspace });

			//  日志消息
			SJ9ADataImportOrder.Tasks.Add(new ITOutputLoginfo());
			SJ9ADataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

			return SJ9ADataImportOrder;

		}
	}
}
