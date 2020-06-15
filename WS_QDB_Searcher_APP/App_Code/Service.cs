using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data;
using System.Xml;
using System.Text;
using QRST_DI_Resources;
using QRST_DI_WebServiceUtil;
using QRST_DI_SS_Basis.MetadataQuery;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]

public class Service : System.Web.Services.WebService
{
    //HJSearchCondition searchcondition;
    WebServiceTools BaseUtil;
    DataSet ds;
    XmlElement xmlTree;
    //MySqlBaseUtilities MySqlBaseUti;

    public Service()
    {

        if (!Constant.Created)
        {
            Constant.Create();
            Constant.InitializeTcpConnection();
        }
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent();
        //searchcondition = new HJSearchCondition();
        BaseUtil = new WebServiceTools();
        ds = new DataSet();
        //MySqlBaseUti = new MySqlBaseUtilities();

    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    #region
    //[WebMethod(Description = "纠正数据（切片)。所有参数须实例化不能为null")]
    private DataSet SearTile(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<int> tileLevel, int pageIndex)
    {
        //List<string> IPcol = WebServiceTools.ipcServ.GetHostIPList();
        List<string> IPcol = WebServiceTools.getLocalAndCenterIP();
        string _url = @"WS_QDB_Searcher_Sqlite/Service.asmx";
        string nmspc = "WebReference";
        string classname = "Service";
        string fun = "SearTile2";
        List<object> objs = new List<object>();
        objs.Add(position.ToArray());
        objs.Add(datetime.ToArray());
        objs.Add(satellite.ToArray());
        objs.Add(sensor.ToArray());
        objs.Add(datatype.ToArray());
        objs.Add(tileLevel.ToArray());
        objs.Add(pageIndex);
        objs.Add(true);

        WebServiceTools.DtCol.Clear();
        ds = BaseUtil.invokSearchMySQLWebService(IPcol, _url, nmspc, classname, fun, objs, true);

        return ds;
    }

    [WebMethod(Description = "纠正数据（切片)。所有参数须实例化不能为null")]
    public DataSet SearTilePaged2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int startIndex, int offset)
    {
        //List<string> IPcol = WebServiceTools.ipcServ.GetHostIPList();
        List<string> IPcol = WebServiceTools.getLocalAndCenterIP();
        string _url = @"WS_QDB_Searcher_Sqlite/Service.asmx";
        string nmspc = "WebReference";
        string classname = "Service";
        string fun = "SearTilePaged2";
        List<object> objs = new List<object>();
        objs.Add(position.ToArray());
        objs.Add(datetime.ToArray());
        objs.Add(satellite.ToArray());
        objs.Add(sensor.ToArray());
        objs.Add(datatype.ToArray());
        objs.Add(tileLevel.ToArray());
        objs.Add(startIndex);
        objs.Add(offset);
        objs.Add(true);

        WebServiceTools.DtCol.Clear();
        ds = BaseUtil.invokSearchMySQLWebService(IPcol, _url, nmspc, classname, fun, objs, true);

        return ds;
    }

    //[WebMethod(Description = "HJ产品（切片）。所有参数（除tileLevel 可为null）须实例化不能为null")]
    //public DataSet SearPRODTile(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, int pageIndex)
    //{
    //    List<string> IPcol = WebServiceTools.ipcServ.GetHostIPList();

    //    string _url = @"WS_QDB_Searcher_Sqlite/Service.asmx";
    //    string nmspc = "WebReference";
    //    string classname = "Service";
    //    string fun = "SearPRODTile2";
    //    List<object> objs = new List<object>();
    //    objs.Add(position.ToArray());
    //    objs.Add(datetime.ToArray());
    //    objs.Add(datatype.ToArray());
    //    objs.Add(tileLevel.ToArray());
    //    objs.Add(pageIndex);
    //    objs.Add(true);

    //    WebServiceTools.DtCol.Clear();
    //    ds = BaseUtil.invokSearchMySQLWebService(IPcol, _url, nmspc, classname, fun, objs, true);
    //    return ds;
    //}

    //[WebMethod(Description = "HJ产品（切片）,可根据产品来源查询。所有参数（除tileLevel 可为null）须实例化不能为null")]
    //public DataSet SearPRODTile2(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, List<string> IPcol, int pageIndex)
    //{
    //    if (IPcol.Count == 0)
    //    {
    //        IPcol = WebServiceTools.ipcServ.GetHostIPList();
    //    }
    //    string _url = @"WS_QDB_Searcher_Sqlite/Service.asmx";
    //    string nmspc = "WebReference";
    //    string classname = "Service";
    //    string fun = "SearPRODTile2";
    //    List<object> objs = new List<object>();
    //    objs.Add(position.ToArray());
    //    objs.Add(datetime.ToArray());
    //    objs.Add(datatype.ToArray());
    //    objs.Add(tileLevel.ToArray());
    //    objs.Add(pageIndex);
    //    objs.Add(true);

    //    WebServiceTools.DtCol.Clear();
    //    ds = BaseUtil.invokSearchMySQLWebService(IPcol, _url, nmspc, classname, fun, objs, true);
    //    return ds;
    //}


    //[WebMethod(Description = "用户发布切片数据。所有参数须实例化不能为null")]
    //public DataSet SearUserRasterTile(List<string> position, List<DateTime> datetime, string keyword, int pageIndex)
    //{
    //    List<string> IPcol = WebServiceTools.ipcServ.GetHostIPList();
    //    string _url = @"WS_QDB_Searcher_Sqlite/Service.asmx";
    //    string nmspc = "WebReference";
    //    string classname = "Service";
    //    string fun = "SearUserRasterTile2";
    //    List<object> objs = new List<object>();
    //    objs.Add(position.ToArray());
    //    objs.Add(datetime.ToArray());
    //    objs.Add(keyword);
    //    objs.Add(pageIndex);
    //    objs.Add(true);

    //    WebServiceTools.DtCol.Clear();
    //    ds = BaseUtil.invokSearchMySQLWebService(IPcol, _url, nmspc, classname, fun, objs, true);

    //    return ds;

    //}

    #endregion

    #region MySQL

    [WebMethod(Description = "以一个XElement对象返回的数据库中数据类型目录")]
    public XmlElement GetTreeCatalogByXML()
    {
        List<string> IPcol = WebServiceTools.getCenterIP();
        string _url = @"WS_QDB_Searcher_MySQL/Service.asmx";
        string nmspc = "WebReference";
        string classname = "Service";
        string fun = "GetTreeCatalogByXML";
        List<object> objs = new List<object>();
        //objs.Add(true);

        WebServiceTools.DtCol.Clear();
        xmlTree = BaseUtil.invokSearchMySQLWSXml(IPcol, _url, nmspc, classname, fun, objs);

        return xmlTree;

    }

    [WebMethod(Description = "根据表编码获取要查询表或视图的查询字段结构")]
    public DataSet GetDataTableStruct(string tableCode)
    {
        List<string> IPcol = WebServiceTools.getCenterIP();
        string _url = @"WS_QDB_Searcher_MySQL/Service.asmx";
        string nmspc = "WebReference";
        string classname = "Service";
        string fun = "GetDataTableStruct";
        List<object> objs = new List<object>();
        objs.Add(tableCode);

        WebServiceTools.DtCol.Clear();
        ds = BaseUtil.invokSearchMySQLWebService(IPcol, _url, nmspc, classname, fun, objs);

        return ds;
    }

    [WebMethod(Description = "查询综合数据库元数据表！")]
    public QueryResponse GetMetadata(QueryRequest _request)
    {
        List<string> IPcol = WebServiceTools.getLocalAndCenterIP();
        string _url = @"WS_QDB_Searcher_MySQL/Service.asmx";
        string nmspc = "WebReference";
        string classname = "Service";
        string fun = "GetMetadataStr";
        List<object> objs = new List<object>();

        string xmlString = string.Empty;
        System.Xml.Serialization.XmlSerializer xmlser = new System.Xml.Serialization.XmlSerializer(typeof(QueryRequest));
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        {
            xmlser.Serialize(ms, _request);
            xmlString = Encoding.UTF8.GetString(ms.ToArray());
        }
        objs.Add(xmlString);
        WebServiceTools.DtCol.Clear();
        ds = BaseUtil.invokSearchMySQLWSCustom(IPcol, _url, nmspc, classname, fun, objs);

        QueryResponse response = new QueryResponse();
        response.recordSet = ds;
        response.totalRecordCount =BaseUtil.allrecordNum;
        response.exception = BaseUtil.exceptions;

        return response;
    }
    #endregion


    [WebMethod(Description = "测试")]
    public DataSet Test()
    {
        List<string> IPMod = new List<string>() { };
        List<string> sensor = new List<string>() {};
        List<int> dt = new List<int>() { };
        List<string> satellite = new List<string>() { };
        List<string> TileType = new List<string>() {};
        List<string> position = new List<string>() { };
        List<string> datatype = new List<string>() { };
        List<int> tileLevel = new List<int>() { };
        List<int> datetime = new List<int>() { };
        DateTime dt1 = DateTime.Now;
        ds = SearTile(position, dt, satellite, sensor, TileType, tileLevel,1);
        return ds;
 
    }
}