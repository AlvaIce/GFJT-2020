using System;
using System.Web;
using System.Web.Services;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process;
using System.Xml.Serialization;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_TS_Process.Site;
using System.IO;
using QRST_DI_TS_Process.Orders.InstalledOrders;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.Collections.Generic;

public struct Expression
{
	static Expression()
	{

	}
}
public class TileInfo
{
	public string IPaddress
	{
		get;
		set;
	}
	public List<string> tilesNameList = new List<string>();
	private int tilesNum = 0;
	string tempName = "";
	public void CreateTileList(string tileName)
	{
		int index = tilesNum / 100;
		if (tilesNameList.Count < (index + 1))
			tilesNameList.Add(tileName);
		else
		{
			tilesNameList[index]+="#"+tileName;
		}
		tilesNum++;
	}
}
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class SubmitOrder : System.Web.Services.WebService
{
	public SubmitOrder()
	{
		if (!Constant.Created)
			Constant.Create();
		TSPCommonReference.Create();
		//如果使用设计的组件，请取消注释以下行 
		//InitializeComponent(); 
	}

	//[WebMethod]
	//public string HelloWorld()
	//{
	//    return "Hello World";
	//}

	//[WebMethod(Description = "根据选择的订单类型获取所有属于该类型的订单描述信息，若为空，则返回所有的订单描述信息")]
	//public List<OrderDef> GetOrderDefList(EnumOrderType orderType)
	//{
	//    List<OrderDef> orderDefLst = new List<OrderDef>();
	//    if (orderType == null)
	//    {
	//        return OrderDefManager.GetOrderDefFromDB("");
	//    }
	//    else
	//    {
	//        return OrderDefManager.GetOrderDefFromDB(string.Format("Type = '{0}'", orderType.ToString()));
	//    } 
	//}

	//[WebMethod(Description = "下单,根据选择的订单种类，以及设置的订单执行参数下单")]
	//public int SubmitOrderList(string orderdefName,string[] orderParas,List<string[]> tasksParas)
	//{
	//    List<OrderDef> orderDefLst = OrderDefManager.GetOrderDefFromDB(string.Format("Name = '{0}'",orderdefName));

	//    if(orderDefLst.Count > 0)
	//    {
	//        OrderClass orderclass = OrderDefManager.OrderDef2OrderClass(orderDefLst[0]);
	//        if (orderclass.Type == EnumOrderType.Installed)    //内置订单
	//        {
	//            orderclass = OrderManager.CreateInstalledOrder(orderclass.OrderName,orderParas);
	//        }
	//        else
	//        {
	//            orderclass = OrderManager.CreateNewOrder(orderclass.OrderName,orderclass.Priority,orderclass.Owner,orderclass.Tasks,tasksParas,orderclass.TSSiteIP);
	//        }
	//        try
	//        {
	//            OrderManager.AddNewOrder2DB(orderclass);
	//            return 1;
	//        }
	//        catch (Exception ex)
	//        {
	//            return 0;
	//        }
	//    }
	//    else
	//    {
	//        //没有找到订单类型
	//        return 0;
	//    }
	//}



	/// <summary>
	/// 给集成共享暴露一个共享地址
	/// </summary>
	/// <returns></returns>
	//[WebMethod(Description = "创建一个共享的工作空间，用户将数据临时放入该空间下，触发入库，若为空字符，则申请失败！")]
	//public  string GetNewWorkspace()
	//{
	//    string sharePath ="-1";
	//    TServerSiteManager.UpdateOptimalStorageSiteList();
	//    if (TServerSiteManager.optimalStorageSites.Count > 0)
	//    {
	//        string storeIP = TServerSiteManager.optimalStorageSites[0].IPAdress;
	//        string code = OrderManager.GetNewCode();
	//        sharePath = OrderManager.BuildWorkSpaceByOrderCode(storeIP,code);
	//    }
	//    return sharePath;
	//}
	/// <summary>
	/// 给集成共享暴露一个共享地址
	/// </summary>
	/// <returns></returns>
	[WebMethod(Description = "创建一个共享的工作空间，用户将数据临时放入该空间下，触发入库，若为'-1'，则申请失败！dataType为数据类型")]
	public string GetNewWorkSpace(string dataType)
	{
		string sharePath = "-1";
		TServerSiteManager.UpdateOptimalStorageSiteList();
		if (TServerSiteManager.optimalStorageSites.Count > 0)
		{
			string storeIP = TServerSiteManager.optimalStorageSites[0].IPAdress;
			string code = OrderManager.GetNewCode();
			sharePath = OrderManager.BuildWorkSpaceByOrderCode(storeIP, code, dataType);
		}
		return sharePath;
	}

	[WebMethod(Description = "触发数据入库！")]
	public int SubmitHJDataImport(string sharePath)
	{
		//检核文件是否存在
		if (!Directory.Exists(sharePath))
		{
			return -2;                    //共享的工作空间不存在
		}
		string[] files = Directory.GetFiles(sharePath, "*.tar.gz");
		if (files.Length == 0)
		{
			return -1;                 //文件未找到
		}
		try
		{
			TSPCommonReference.Create();
			List<OrderClass> orderclassLst = OrderManager.CreateInstalledBatchOrder("IONormalHJBatchImport", new string[] { sharePath });
			foreach (OrderClass orderclass in orderclassLst)
			{
                orderclass.Priority = EnumOrderPriority.High;
				OrderManager.AddNewOrder2DB(orderclass);
			}
			return 1;   //成功触发入库
		}
		catch (Exception)
		{
			return 0;                      //未知原因
		}
	}

	[WebMethod(Description = "触发数据批量入库！datatype为数据类型，dirPath为数据目录")]
	public int SubmitDataImport(string datatype, string sharePath)
	{
		//检核文件是否存在
		if (!Directory.Exists(sharePath))
		{
			return -2;                    //共享的工作空间不存在
		}

		try
		{
			TSPCommonReference.Create();
			if (datatype.ToLower() == "modis")
			{
				string[] files = Directory.GetFiles(sharePath, "*.hdf");
				if (files.Length == 0)
				{
					return -1;                 //文件未找到
				}
				else
				{
					List<OrderClass> orderLst = new IORasterDataImportNew().CreateRasterDataBatchImport(datatype, files);
					foreach (OrderClass orderclass in orderLst)
					{
                        orderclass.Priority = EnumOrderPriority.High;
						OrderManager.AddNewOrder2DB(orderclass);
					}
					return 1;
				}
			}
			else if (datatype.ToLower() == "noaa")
			{
				List<OrderClass> orderLst = new IONoaaBatchImport().CreateNoaaBatchImport(sharePath);
				foreach (OrderClass orderclass in orderLst)
				{
                    orderclass.Priority = EnumOrderPriority.High;
					OrderManager.AddNewOrder2DB(orderclass);
				}
				return 1;
			}
			else
			{
				return -3;
			}

		}
		catch (Exception)
		{
			return 0;                      //未知原因
		}
	}



	[WebMethod(Description = "触发数据入库！dataType为需要上传的数据类型，sharePath为数据所在的网络路径,生产线")]
	public int SubmitImportOrder(string dataType, string sharePath)
	{
		//检核文件是否存在
		if (!Directory.Exists(sharePath))
		{
			return -2;                    //共享的工作空间不存在
		}

		try
		{
			TSPCommonReference.Create();
			switch (dataType)
			{
				case "HJCorrectedData":
					{
						string[] files = Directory.GetFiles(sharePath, "*.tar.gz");
						if (files.Length == 0)
						{
							return -1;                 //文件未找到
						}
						List<OrderClass> orderclassLst = OrderManager.CreateInstalledBatchOrder("IONormalHJBatchImport", new string[] { sharePath });
						foreach (OrderClass orderclass in orderclassLst)
						{
                            orderclass.Priority = EnumOrderPriority.High;
							OrderManager.AddNewOrder2DB(orderclass);
						}
						return 1;   //成功触发入库
					}
				case "AlgorithmCmp":
					{
						string[] files = Directory.GetFiles(sharePath, "*.tar");
						if (files.Length == 0)
						{
							return -1;
						}
						OrderClass orderclass = new IOImportStandCmp().CreateStandCmpImport(sharePath);
                        orderclass.Priority = EnumOrderPriority.High;
                        OrderManager.AddNewOrder2DB(orderclass);
						return 1;
					}
				case "ProductWFL":
					{
						string[] files = Directory.GetFiles(sharePath, "*.tar");
						if (files.Length == 0)
						{
							return -1;
						}
						OrderClass orderclass = new IOImportStandWfl().CreateStandWflImport(sharePath);
                        orderclass.Priority = EnumOrderPriority.High;
						OrderManager.AddNewOrder2DB(orderclass);
						return 1;
					}
				case "UserProduct":
					{
						OrderClass orderclass = new IOImportUserProduct().CreateUserProductImport(sharePath);
                        orderclass.Priority = EnumOrderPriority.High;
                        OrderManager.AddNewOrder2DB(orderclass);
						return 1;
					}
				default:
					return -3;     //没有对应的数据入库类型
			}
		}
		catch (Exception)
		{
			return 0;                      //未知原因
		}
	}

	[WebMethod(Description = "用户数据上传统一接口，dataType为上传类型，pathID源数据url,opID为操作号！")]
	public int UploadData(string dataType, string pathID, string opID)
	{
		try
		{
			OrderClass orderclass = new IOUserDataImport().CreateUserDataImport(dataType, pathID, opID);
            orderclass.Priority = EnumOrderPriority.High;
            OrderManager.AddNewOrder2DB(orderclass);
			return 1;
		}
		catch (Exception)
		{
			return 0;
		}
	}

	[WebMethod(Description = "集成共享下载数据统一接口，dataID为数据唯一编码，pathID为下载目的地,opID为操作号！")]
	public int DownLoad(string dataID, string pathID, string opID)
	{
		try
		{
			OrderClass orderclass = new IODownLoadUserData().CreateDownLoadUserData(dataID, pathID, opID);
            orderclass.Priority = EnumOrderPriority.High;
            OrderManager.AddNewOrder2DB(orderclass);
			return 1;
		}
		catch (Exception)
		{
			return 0;
		}
	}

    [WebMethod(Description = "集成共享下载数据统一接口，dataName为数据名称，dataID为数据唯一编码，pathID为下载目的地,opID为操作号！")]
    public int DownLoadwithName(string dataName, string dataID, string pathID, string opID)
    {
        try
        {
            string dataIDName = String.Format("{0}#{1}", dataID, dataName);
            OrderClass orderclass = new IODownLoadUserData().CreateDownLoadUserData(dataIDName, pathID, opID);
            orderclass.Priority = EnumOrderPriority.High;
            OrderManager.AddNewOrder2DB(orderclass);
            return 1;
        }
        catch (Exception)
        {
            return 0;
        }
    }


	/// <summary>
	/// 创建内置指定订单
	/// "IOHJdataImport"
	/// "IOGF1dataPrepare"
	/// "IOCbersDataImport"
	/// "IOModisDataImport"
	/// "IONoaaDataImport"
	/// "IODEMDataImport"
	/// "IOVectorImport"
	/// "IONormalHJDataImport"
	/// "IOSortedVectorImport"
	/// "IOTMDataImport"
	/// "IOCopyAssistFiles"
	/// "IOTJQuickBirdDataImport"
	/// "IOHHALOSDataImport"
	/// "IOSZWorldView2DataImport"
	/// "IOZJCoastlandALOSDataImport"
	/// "IOTJ5MRSImageDataImport"
	/// "IOUserDataImport"
	/// "IODownLoadUserData"
	/// "IOImportStandCmp"
	/// "IOImportStandWfl"
	/// "IOImportUserProduct"
	/// "IORasterDataImportNew"
	/// "IOImportZipCmp"
	/// "IOGF1DataImport"
	/// "IOZY3DataImport"
	/// "IOHJDataImportStandard"
	/// "IOGF1CorrectedDataImport"
	/// "IOZY02cDataImport"
	/// "IONormalDataImport"
	/// </summary>
	/// <returns></returns>
	[WebMethod(Description = "提交内置指定订单")]
	public string SubmitInstalledOrder(string orderName, string[] orderParas)
	{
		//王栋工作流 任务驱动入库 joki 131127
		OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParas);
        neworder.Priority = EnumOrderPriority.High;
        OrderManager.AddNewOrder2DB(neworder);
		return neworder.OrderCode;
	}
	[WebMethod(Description = "提交内置指定订单,瓦片下载使用")]
	public List<string> SubmitTilesInstalledOrder(string orderName, string[] orderParas)
	{
		//谢毅工作流 任务驱动入库 ksk 131127
		List<string> orderCode=new List<string> ();
		string[] tiles = orderParas[0].Split('#');
		DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
		Dictionary<string, string> tileInfo = new Dictionary<string, string>();
		List<TileInfo> tilesInfo = new List<TileInfo>();
		string failTiles = "";
		foreach (var item in tiles)
		{
			bool Contain = false;
			string ip = "-1";
			string desPath = da.GetPathByFileName(item+".jpg", out ip);
			if (ip != "-1")
			{
				foreach (var info in tilesInfo)
				{
					if (info.IPaddress == ip)
					{
						info.CreateTileList(item);
						Contain = true;
						break;
					}
				}
				if (Contain)
					continue;
				TileInfo tile = new TileInfo();
				tile.IPaddress = ip;
				tile.CreateTileList(item);
				tilesInfo.Add(tile);
				//if (!tileInfo.ContainsKey(ip))
				//{
				//    tileInfo.Add(ip, item);
				//}
				//else
				//{
				//    string strTemp = tileInfo[ip];
				//    strTemp += "#" + item;
				//    tileInfo[ip] = strTemp;
				//}
				//tileDesPath tile = new tileDesPath();
				//tile.Ip = ip;
				//tile.Despath = desPath;
			}
			else
			{
				failTiles += item + "#";
				//tilefileNames.Add(failedpath);
			}
		}
		foreach (var item in tilesInfo)
		{
			foreach (var tileNameStr in item.tilesNameList)
			{
				string[] orderParasTemp = { tileNameStr, orderParas[1], orderParas[2], item.IPaddress};
				OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParasTemp);
                neworder.Priority = EnumOrderPriority.High;
                OrderManager.AddNewOrder2DB(neworder);
				orderCode.Add(neworder.OrderCode);
			}
		}
		//foreach (var item in tileInfo)
		//{
		//    string[] orderParasTemp = { item.Value, orderParas[1], orderParas[2], item.Key };
		//    OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParasTemp);
		//    OrderManager.AddNewOrder2DB(neworder);
		//    orderCode.Add(neworder.OrderCode);
		//}
		failTiles=failTiles.TrimEnd('#');
		if(failTiles!="")
		{
			string[] orderParasTemp = { failTiles, orderParas[1], orderParas[2],TServerSiteManager.GetCenterSiteIP() };
			OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParasTemp);
            neworder.Priority = EnumOrderPriority.High;
            OrderManager.AddNewOrder2DB(neworder);
			orderCode.Add(neworder.OrderCode);
		}
		return orderCode;
		//OrderClass neworder = OrderManager.CreateInstalledOrder(orderName, orderParas);
		//OrderManager.AddNewOrder2DB(neworder);
		//return neworder.OrderCode;
	}
	[WebMethod(Description = "为预处理准备数据生产工作空间！-1表示创建失败,否则传送[订单号：工作空间路径]，例如：'P1475369832501:\\127.0.0.1\\QRST_DB_Share\\P1475369832501'！ zxw 20130818")]
	public string CreateWorkSpaceForPreProcess()
	{
		string orderPath = "-1";
		OrderClass orderClass = new IOGF1dataPrepare().CreateGF1dataPrepareOrder();
        orderClass.Priority = EnumOrderPriority.High;
        OrderManager.AddNewOrder2DB(orderClass);
		//循环监控订单是否被执行
		string condition = string.Format("s.OrderCode = '{0}' and s.Status = 'Suspended'", orderClass.OrderCode);
		DateTime dtNow = DateTime.Now;
		DateTime dtSpan = DateTime.Now;
		while ((dtSpan - dtNow).TotalMilliseconds < 10000) //三秒内没有创建好，则创建工作空间失败
		{
			DataTable orderLst = OrderManager.GetOrderList(condition);
			if (orderLst.Rows.Count > 0)
			{
				//  orderClass = OrderManager.DBRow2OrderCls(orderLst.Rows[0]);
				string addressip = orderLst.Rows[0]["addressip"].ToString();
				//找到工作空间位置，例如 \\127.0.0.1\QRST_DB_Share\P1475369832501
				orderPath = string.Format(@"{1}:\\{0}\QRST_DB_Share\{1}", addressip, orderClass.OrderCode);
				return orderPath;
			}
			dtSpan = DateTime.Now;
		}
		//超时 
		return orderPath;
	}
	[WebMethod(Description = "为原始数据及元数据入库订单创建工作空间。-1表示创建失败,否则传送[订单号]，例如：'P1475369832501'")]
	public string ApplyNewGF1DataImportOrder()
	{
		string orderPath = "-1";
		OrderClass orderClass = new IOGF1DataImport().Create();
        orderClass.Priority = EnumOrderPriority.High;
		OrderManager.SubmitOrder(orderClass);
		return orderClass.OrderCode;
	}

	//  [WebMethod(Description = "为原始数据及元数据入库订单创建工作空间。-1表示创建失败,否则传送[订单号]，例如：'P1475369832501',参数是导入的数据类型如：GF1，ZY02C，HJ，ZY3")]
	//public string ApplyNewDataImportOrder(string dataType)
	//{
	//    string orderPath = "-1";
	//    OrderClass orderClass;
	//    switch(dataType)
	//    {
	//        case "GF1": orderClass =new  IOGF1DataImport().Create();
	//            break;
	//        case "ZY02C": orderClass = null;
	//            break;
	//        case "HJ": orderClass = null;
	//            break;
	//        case "ZY3": orderClass = new IOZY3DataImport().Create();
	//            break;
	//        default orderClass = null;
	//    }
	//    OrderManager.SubmitOrder(orderClass);
	//    return orderClass.OrderCode;
	//}


	[WebMethod(Description = "为原始数据及元数据入库订单创建工作空间。-1表示创建失败,否则传送[工作空间路径]，例如：'\\127.0.0.1\\QRST_DB_Share\\P1475369832501'！")]
	public string GetOrderWorkspace(string ordercode)
	{
		////循环监控订单是否被执行
		OrderClass order = OrderManager.GetOrderByCode(ordercode);
		return (order == null) ? "-1" : (order.OrderWorkspace);
	}
	[WebMethod(Description = "继续执行下一步订单")]
	public bool ResumeGF1DataImportOrder(string ordercode)
	{
		////循环监控订单是否被执行

		OrderClass order = OrderManager.GetOrderByCode(ordercode);
		if (order != null && order.Status == EnumOrderStatusType.Suspended)
		{
			OrderManager.UpdateOrderStatus(ordercode, EnumOrderStatusType.Waiting);
			return true;
		}
		return false;
	}


	[WebMethod(Description = "更改订单状态,使订单进入等待状态 zxw 20130818")]
	public void SetOrderStatus2Waiting(string orderCode)
	{
		OrderManager.UpdateOrderStatus(orderCode, EnumOrderStatusType.Waiting);
	}


	[WebMethod(Description = "更改订单状态,使订单进入等待状态 joki 131128")]
	public string GetOrderStatus(string ordercode)
	{
		OrderClass order = OrderManager.GetOrderByCode(ordercode);
		if (order != null)
		{
			return order.Status.ToString();
		}
		return "Missing";
	}
	private string GetFailedTilePath()
	{
		string CenterIP = TServerSiteManager.GetCenterSiteIP();
		string pattern = @"^(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

		if (System.Text.RegularExpressions.Regex.IsMatch(CenterIP, pattern))
		{
			return string.Format(@"\\{0}\{1}\{2}\", CenterIP, StorageBasePath.QRST_DB_Tile, StorageBasePath.FailedTile);
		}
		else
		{
			return "";
		}
	}

    //add by JiangBin
    [WebMethod(Description = "提交切片信息,返回值ture表示成功。tile中格式为层级,行号,列号，多个tile以英文分号分隔，如8,1255,2366;8,1255,2367")]
    public bool SubmitTileOrder(string UserName, string ProductType, string tile)
    {
        string sql = String.Format("insert into tileorder(userName,productType,needTile,submitTime) value('{0}','{1}','{2}','{3}')", UserName, ProductType, tile, DateTime.Now);
        try
        {
             TSPCommonReference.dbOperating.ISDB.ExecuteSql(sql);
             return true;
        }
        catch
        {
            return false;
        }
    }
}