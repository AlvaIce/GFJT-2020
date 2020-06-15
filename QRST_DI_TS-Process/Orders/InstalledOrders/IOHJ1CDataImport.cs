using System.Collections.Generic;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Tasks.InstalledTasks;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    public class IOHJ1CDataImport
	{
		/// <summary>
		/// Installed Order for HJ1C Data Import
		/// OrderParams描述
		/// OrderParams[0]为待导入数据存放路径
		/// </summary>
		/// <param name="SourceFilePath"></param>
		/// <returns></returns>
		public OrderClass Create()
		{
			OrderClass HJ1CdataImportOrder = new OrderClass();

			//参数：SourceFilePath
			HJ1CdataImportOrder.Type = EnumOrderType.Installed;
			HJ1CdataImportOrder.OrderCode = OrderManager.GetNewCode();

			//设置任务,任务赋参
			//  日志消息
			HJ1CdataImportOrder.Tasks.Add(new ITOutputLoginfo());
			HJ1CdataImportOrder.TaskParams.Add(new string[] { "开始订单任务！" });
			//  创建文件夹工作空间
			HJ1CdataImportOrder.Tasks.Add(new ITCreateHJDataInputWSP());
			HJ1CdataImportOrder.TaskParams.Add(new string[] { "" });
			//由第三方拷贝数据到工作空间

			//  文件解压
			HJ1CdataImportOrder.Tasks.Add(new ITUnzipHJtargz4XmlJpg());
			HJ1CdataImportOrder.TaskParams.Add(new string[] { HJ1CdataImportOrder.OrderWorkspace, StorageBasePath.SharePath_OrignalData(HJ1CdataImportOrder.OrderWorkspace) });

			//源数据文件入库
			HJ1CdataImportOrder.Tasks.Add(new ITHJ1CDataImport());
			HJ1CdataImportOrder.TaskParams.Add(new string[] { HJ1CdataImportOrder.OrderWorkspace });

			//  日志消息
			HJ1CdataImportOrder.Tasks.Add(new ITOutputLoginfo());
			HJ1CdataImportOrder.TaskParams.Add(new string[] { "完成数据入库" });

			return HJ1CdataImportOrder;

		}

		/// <summary>
		/// 创建批量环境星数据的准备订单
		/// </summary>
		/// <param name="sourceFileDirectory">环境星数据的存放文件夹</param>
		/// <returns></returns>
		public List<OrderClass> CreateBatchOrder(string sourceFileDirectory)
		{
			List<OrderClass> orderClassLst = new List<OrderClass>();
			string[] sourceFiles = Directory.GetFiles(sourceFileDirectory);
			for (int i = 0; i < sourceFiles.Length; i++)
			{
				if (sourceFiles[i].EndsWith(".tar.gz") || sourceFiles[i].EndsWith(".TAR.GZ") || sourceFiles[i].EndsWith(".TAR.gz"))
					orderClassLst.Add(Create());
			}
			return orderClassLst;
		}

	}
}

