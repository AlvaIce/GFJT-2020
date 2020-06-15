using System.Collections.Generic;
using System.Web.Services;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBService;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class Service : System.Web.Services.WebService
{
    private IQDB_GetData _getDataServer;
    public Service()
    {

        if (!Constant.ServiceIsConnected)
        {
            Constant.InitializeTcpConnection();
        }

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
        _getDataServer = Constant.IGetDataService;
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
    [WebMethod(Description = "根据数据类型编码获得数据存放相对路径1")]
    public string GetRalativeAddByDataCode(string dataCode)
    {
        return _getDataServer.GetRalativeAddByDataCode(dataCode);
    }
    [WebMethod(Description = "根据数据类型编码获得构成数据存放相对路径2的字段")]
    public string[] GetAddressFields(string dataCode)
    {
        return _getDataServer.GetAddressFields(dataCode);
    }
    [WebMethod(Description = "获得纠正后未切片数据的构成存放相对路径的字段")]
    private string[] GetCorrectedDataAddressFields()
    {
        return _getDataServer.GetCorrectedDataAddressFields();
    }
    [WebMethod(Description = "根据查询得到的数据名称，获得纠正后未切片数据的存储路径")]
    public string GetCorrectedDataAddress(string DataName)
    {
        return _getDataServer.GetCorrectedDataAddress(DataName);
    }

    /// <summary>
    /// 1)	HJ纠正数据（切片）、2)	HJ产品（切片）、3)	HJ分类（切片）
    /// </summary>
    /// <param name="qrst_code">数据标识</param>
    /// <returns>数据所在路径</returns>
    [WebMethod(Description = "获取切片数据路径")]
    public List<string> GetTilesList(List<string> tileNames)
    {
        
        return  _getDataServer.GetTilesList(tileNames);
    }


    [WebMethod(Description = "根据查询得到的原数据编码，获得数据所在文件夹的存储路径")]
    public string GetSourceDataPath(string DataCode)
    {
        return _getDataServer.GetSourceDataPath(DataCode);
    }

    [WebMethod(Description = "根据切片文件名，找到瓦片数据后打包推送到指定目录下")]
    public string PushTileZipToDirByName(string tileName, string originalOrderCode)
    {
         
        return _getDataServer.PushTileZipToDirByName(tileName,originalOrderCode);
    }

    [WebMethod(Description = "根据QRST_CODE获取路径拷贝文件(本地直接复制方法)")]
    public string CopyByQrstCode(string code, string sharePath)
    {
        return _getDataServer.CopyByQrstCode(code,sharePath);

    }

    [WebMethod(Description = "根据QRST_CODE获取路径拷贝文件（创建order并提交订单task）")]
    public string CopyByQrstCodeOrderTask(string code, string sharePath, string mouid)
    {

        return _getDataServer.CopyByQrstCodeOrderTask(code, sharePath, mouid);

    }

    #region 生产线接口
    [WebMethod(Description = "生产线接口：根据数据的datacode删除对应的元数据记录与文件")]
    public int DeleteData(string datacode)
    {

        return _getDataServer.DeleteData(datacode);
    }

    [WebMethod(Description = "生产线接口：根据查询数据名称以及数据编码获取数据存放路径,目前支持AlgorithmCmp和ProductWFL查询")]
    public string GetData(string DataType, string QRST_CODE)
    {
        return _getDataServer.GetData( DataType,  QRST_CODE);
    }

    //  [WebMethod(Description = "生产线接口：根据数据类型与数据编码下载数据，destPath为目的地址，必须为网络共享文件夹,目前支持AlgorithmCmp和ProductWFL下载")]
    //public string DownLoadData(string DataType, string QRST_CODE,string destPath)
    //{
    //    //如果是HJCorrectedData，则
    //    if (Directory.Exists(destPath))
    //    {
    //        if (DataType == "AlgorithmCmp" || DataType == "ProductWFL")
    //        {
    //            string srcPath = MetaData.GetDataAddress(QRST_CODE);
    //            if (srcPath != "-1" && srcPath != "0")
    //            {

    //            }
    //        }
    //        else
    //        {
    //            return "-1";
    //        }
    //    }
    //    else
    //    {
    //        return "网络路径未找到！";
    //    }
    //}
    #endregion
}