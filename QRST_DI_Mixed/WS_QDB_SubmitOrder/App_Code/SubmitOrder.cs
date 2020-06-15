using System;
using System.Web.Services;
using QRST_DI_Resources;
using System.Collections.Generic;
using QRST_DI_SS_DBInterfaces.IDBService;

[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[WebServiceBinding(ConformsTo = WsiProfiles.None)]
public class SubmitOrder : System.Web.Services.WebService
{
    private IQDB_SubmitOrder _submitOrderServ;
    public SubmitOrder()
	{
        if (!Constant.ServiceIsConnected)
        {
            Constant.InitializeTcpConnection();
        }
        _submitOrderServ = Constant.ISubmitOrderSer;
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
        return _submitOrderServ.GetNewWorkSpace(dataType);
	}

	[WebMethod(Description = "触发数据入库！")]
	public int SubmitHJDataImport(string sharePath)
	{
        return _submitOrderServ.SubmitHJDataImport(sharePath);   
	}

    [WebMethod(Description = "触发DOC数据入库！(日期转为字符串)")]//网络组要求将Datetime修改为String
    public int SubmitDOCDataImportToString(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, string DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, string UPLOADTIME, string FILESIZE, string sharePath)  //, string sharePath
    {
         return _submitOrderServ.SubmitDOCDataImportToString( TITLE,  DOCTYPE,  KEYWORD,  ABSTRACT,  DOCDATE,
             DESCRIPTION,  AUTHOR,  UPLOADER,  UPLOADTIME,  FILESIZE,  sharePath);
    }



    [WebMethod(Description = "触发DOC数据入库！")]//原始入库
    public int SubmitDOCDataImport(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, DateTime DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, DateTime UPLOADTIME, int FILESIZE, string sharePath)  //, string sharePath
    {
        return _submitOrderServ.SubmitDOCDataImport(TITLE, DOCTYPE, KEYWORD, ABSTRACT, DOCDATE, DESCRIPTION, AUTHOR, UPLOADER, UPLOADTIME, FILESIZE, sharePath);    
    }

	[WebMethod(Description = "触发数据批量入库！datatype为数据类型，dirPath为数据目录")]
	public int SubmitDataImport(string datatype, string sharePath)
	{
        return _submitOrderServ.SubmitDataImport( datatype,  sharePath);
	}



	[WebMethod(Description = "触发数据入库！dataType为需要上传的数据类型，sharePath为数据所在的网络路径,生产线")]
	public int SubmitImportOrder(string dataType, string sharePath)
	{
        return _submitOrderServ.SubmitImportOrder( dataType,  sharePath);        
	}

	[WebMethod(Description = "用户数据上传统一接口，dataType为上传类型，pathID源数据url,opID为操作号！")]
	public int UploadData(string dataType, string pathID, string opID)
   {
      return _submitOrderServ.UploadData( dataType,  pathID,  opID);        
	}

    [WebMethod(Description = "集成共享P2P下载数据统一接口，dataName为数据名称，支持GF原始数据和gff瓦片数据（瓦片数据不支持WCPO打包数据），dataPath为拷贝的目标共享文件夹,opID为操作号！")]
    public int DownLoad_P2P(string dataName, string dataSharePath, string opID, string webserviceIp)
    {
        return _submitOrderServ.DownLoad_P2P( dataName,  dataSharePath,  opID, webserviceIp);       
    }

    [WebMethod(Description = "集成共享下载数据统一接口，dataID为数据唯一编码，pathID为下载目的地,opID为操作号！")]
    public int DownLoad(string dataID, string pathID, string opID)
    {
        return DownLoad(dataID, pathID, opID, "", "");
    }
    [WebMethod(Description = "集成共享下载数据统一接口，dataID为数据唯一编码，pathID为下载目的地,opID为操作号,GFdataName为数据名称！", EnableSession = true, MessageName = "DownLoad2")]
    public int DownLoad(string dataID, string pathID, string opID, string GFdataName, string webservice)
    {
        return _submitOrderServ.DownLoad( dataID,  pathID,  opID,  GFdataName, webservice);       
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
       return _submitOrderServ.SubmitInstalledOrder( orderName,orderParas);       
	}
	[WebMethod(Description = "提交内置指定订单,瓦片下载使用")]
	public List<string> SubmitTilesInstalledOrder(string orderName, string[] orderParas)
	{
       return _submitOrderServ.SubmitTilesInstalledOrder( orderName, orderParas);       
	}
	[WebMethod(Description = "为预处理准备数据生产工作空间！-1表示创建失败,否则传送[订单号：工作空间路径]，例如：'P1475369832501:\\127.0.0.1\\QRST_DB_Share\\P1475369832501'！ zxw 20130818")]
	public string CreateWorkSpaceForPreProcess()
	{
		return _submitOrderServ.CreateWorkSpaceForPreProcess();
	}
	[WebMethod(Description = "为原始数据及元数据入库订单创建工作空间。-1表示创建失败,否则传送[订单号]，例如：'P1475369832501'")]
	public string ApplyNewGF1DataImportOrder()
	{
		 return _submitOrderServ.ApplyNewGF1DataImportOrder();        
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
          return _submitOrderServ.GetOrderWorkspace( ordercode);        
	}
	[WebMethod(Description = "继续执行下一步订单")]
	public bool ResumeGF1DataImportOrder(string ordercode)
	{
		 return _submitOrderServ.ResumeGF1DataImportOrder( ordercode);        
	}


	[WebMethod(Description = "更改订单状态,使订单进入等待状态 zxw 20130818")]
	public void SetOrderStatus2Waiting(string orderCode)
	{
         _submitOrderServ.SetOrderStatus2Waiting( orderCode);       
	}


	[WebMethod(Description = "更改订单状态,使订单进入等待状态 joki 131128")]
	public string GetOrderStatus(string ordercode)
	{
         return _submitOrderServ.GetOrderStatus( ordercode);        
	}
}
