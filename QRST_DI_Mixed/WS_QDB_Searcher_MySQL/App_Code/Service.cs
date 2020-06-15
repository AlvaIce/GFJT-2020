using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data;
using System.Xml;
using DotSpatial.Topology;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_SS_DBInterfaces.IDBService;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class Service : System.Web.Services.WebService, IWSSearcher4JCGX
{
    //QueryObj queryConditions;
    IQDB_Searcher_Db _searcherDb;

    public Service()
    {

        if (!Constant.ServiceIsConnected)
        {
            Constant.InitializeTcpConnection();
        }
        _searcherDb = Constant.ISearcherDbServ;
    }

    //下面方法是测试用的 因为form后台不能调用ws_submitorder
    [WebMethod(Description = "触发DOC数据入库！下面方法是测试用的 因为form后台不能调用ws_submitorder")]
    public int SubmitDOCDataImport(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, DateTime DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, DateTime UPLOADTIME, int FILESIZE, string sharePath)  //, string sharePath
    {
        return _searcherDb.SubmitDOCDataImport(TITLE, DOCTYPE, KEYWORD, ABSTRACT, DOCDATE, DESCRIPTION, AUTHOR, UPLOADER, UPLOADTIME, FILESIZE, sharePath);

    }
    //下面方法是测试用的 因为form后台不能调用ws_submitorder
    [WebMethod(Description = "触发DOC数据入库！(日期转为字符串)下面方法是测试用的 因为form后台不能调用ws_submitorder")]//网络组要求将Datetime修改为String
    public int SubmitDOCDataImportToString(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, string DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, string UPLOADTIME, string FILESIZE, string sharePath)  //, string sharePath
    {
        return _searcherDb.SubmitDOCDataImportToString(TITLE, DOCTYPE, KEYWORD, ABSTRACT, DOCDATE, DESCRIPTION, AUTHOR, UPLOADER, UPLOADTIME, FILESIZE, sharePath);
    }

    //下面方法是测试用的 因为form后台不能调用ws_GetData  
    //[WebMethod(Description = "根据QRST_CODE获取路径拷贝文件(本地直接复制方法)下面方法是测试用的 因为form后台不能调用ws_GetData")]
    //public string CopyByQrstCode(string code, string sharePath)
    //{
    //    return _searcherDb.CopyByQrstCode( code,  sharePath);        
    //}


    //下面方法是测试用的 因为form后台不能调用ws_GetData
    //[WebMethod(Description = "根据QRST_CODE获取路径拷贝文件（创建order并提交订单task）下面方法是测试用的 因为form后台不能调用ws_GetData")]
    //public string CopyByQrstCodeOrderTask(string code, string sharePath, string mouid)
    //{
    //   return _searcherDb.CopyByQrstCodeOrderTask( code,  sharePath,  mouid);       

    //}

    [WebMethod(Description = "以一个XElement对象返回的数据库中数据类型目录")]
    public XmlDocument GetTreeCatalogByXML()
    {
        return _searcherDb.GetTreeCatalogByXML();

    }

    [WebMethod(Description = "根据数据类型编码获得可选字段列表")]
    public string[] GetFieldsByDataCode(string dataCode)
    {
        return _searcherDb.GetFieldsByDataCode(dataCode);

    }
    [WebMethod(Description = "查询水溶胶数据")]
    public DataSet SearchAOD(List<string> datetime, string dataType)
    {
        return _searcherDb.SearchAOD(datetime, dataType);

    }
    [WebMethod(Description = "根据字段高级查询文档DOC")]
    public DataSet SearchDOCByField(string TITLE, string DOCTYPE, string KEYWORD, string AUTHOR, string UPLOADER, string UPLOADTIMEStart, string UPLOADTIMEOver)//List<string> DOCDATE,, List<string> UPLOADTIME
    {
        return _searcherDb.SearchDOCByField(TITLE, DOCTYPE, KEYWORD, AUTHOR, UPLOADER, UPLOADTIMEStart, UPLOADTIMEOver);

    }

    [WebMethod(Description = "根据字段模糊查询文档DOC")]
    public DataSet SearchDOCByKeyWord(string KeyWord)
    {
        return _searcherDb.SearchDOCByKeyWord(KeyWord);
    }

    [WebMethod(Description = "根据字段高级查询当前页比如100条数据文档DOC")]
    public DataSet SearchDOCByFieldPage(string TITLE, string DOCTYPE, string KEYWORD, string AUTHOR, string UPLOADER, string UPLOADTIMEStart, string UPLOADTIMEOver, int pageNow, int pageSize)//List<string> DOCDATE,, List<string> UPLOADTIME
    {
        return _searcherDb.SearchDOCByFieldPage(TITLE, DOCTYPE, KEYWORD, AUTHOR, UPLOADER, UPLOADTIMEStart, UPLOADTIMEOver, pageNow, pageSize);

    }

    //[WebMethod(Description = "根据查询条件对象编码获得查询结果数据集,LogicalOperator和SimpleCondition的数目要一样，LogicalOperator表示前后两个SimpleCondition的逻辑运算关系")]
    //public DataSet SerchMySQLDB(List<LogicalOperator> listLogicalOperators, List<QRST_DI_MS_Basis.QueryBase.SimpleCondition> listSimpleConditions,string additionalCondition, string dataCode)
    //{


    //    returnDS = new DataSet();

    //    if (listLogicalOperators.Count!=listSimpleConditions.Count||dataCode.Length < 4||listLogicalOperators.Count==0)
    //    {
    //        return returnDS;
    //    }
    //    string tableName;
    //    string dbName;
    //    string dbviewName;
    //    //根据表编码获取数据库名称
    //    dbName = dataCode.Substring(0, 4);

    //    //根据数据库名称获取库访问实例
    //    mySQLOperator = new DBMySqlOperating();
    //    switch (dbName)
    //    {
    //        case "BSDB":
    //            MySqlBaseUti = mySQLOperator.BSDB;
    //            break;
    //        case "EVDB":
    //            MySqlBaseUti = mySQLOperator.EVDB;
    //            break;
    //        case "IPDB":
    //            MySqlBaseUti = mySQLOperator.IPDB;
    //            break;
    //        case "ISDB":
    //            MySqlBaseUti = mySQLOperator.ISDB;
    //            break;
    //        case "MADB":
    //            MySqlBaseUti = mySQLOperator.MADB;
    //            break;
    //        case "MIDB":
    //            MySqlBaseUti = mySQLOperator.MIDB;
    //            break;
    //        case "RCDB":
    //            MySqlBaseUti = mySQLOperator.RCDB;
    //            break;
    //        default:
    //            break;
    //    }

    //    //根据库访问实例和表编码获取表名
    //    tablecode_Dal tablecodeDal = new tablecode_Dal(MySqlBaseUti);
    //    tableName = tablecodeDal.GetTableName(dataCode);

    //    //根据表名获得表视图
    //    table_view_Dal tableviewDal = new table_view_Dal(MySqlBaseUti);
    //    string[] arrFields = tableviewDal.GetFields(tableName);

    //    //表对应的视图名称
    //    dbviewName = string.Concat(tableName, "_view");

    //    string sqltoQuery = string.Format("select * from {0} where (", dbviewName);
    //    string sqlCondition = sqlBaseTool.GetSqlCondition(listLogicalOperators, listSimpleConditions,arrFields);
    //    sqltoQuery += sqlCondition;

    //    sqltoQuery += additionalCondition;
    //    sqltoQuery += ")";
    //    returnDS = MySqlBaseUti.GetDataSet(sqltoQuery);

    //    return returnDS;
    //}


    #region 生产线接口
    [WebMethod(Description = "算法组件查询接口测试接口（公开给PL生产线）")]
    public DataSet SearchAlgorithmPLtest()
    {
        string AlgEnName = "CompassFilter";
        string AlgCnName = "方向滤波产品";
        string ComponentVersion = "1.0";
        int AllRecordsCount = 1;
        return _searcherDb.SearchAlgorithmPL(AlgEnName, AlgCnName, ComponentVersion, out AllRecordsCount, 0, 10);

    }
    [WebMethod(Description = "算法组件查询接口（公开给PL生产线）")]
    public DataSet SearchAlgorithmPL(string AlgEnName, string AlgCnName, string ComponentVersion, out int AllRecordsCount, int StartIndex, int ResultCount)
    {
        return _searcherDb.SearchAlgorithmPL(AlgEnName, AlgCnName, ComponentVersion, out AllRecordsCount, StartIndex, ResultCount);

    }

    [WebMethod(Description = "GFF格式的数据查询，通过自定义组装查询条件(查询条件语法标准符合sql的where子句)获得查询结果,查询条件如 name = 'HJ1A_CCD2_20110419_4_246_596-1.gff'")]
    public DataSet SearchGFFData(string queryCondition)
    {
        return _searcherDb.SearchGFFData(queryCondition);

    }

    [WebMethod(Description = "算法组件查询测试接口（公开给JCGX）")]
    public DataSet SearchCorrectedDataPLTest()
    {
        List<string> UploadDate = new List<string>();
        int a = 0;
        return SearchCorrectedDataPL(UploadDate, UploadDate, UploadDate, UploadDate, UploadDate, out a, 1, 10);
    }

    //public DataSet SearchCorrectedData(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, string DnMark, List<string> PixelSpacing, List<int> DataSizeRange, List<int> CloudNumRange, out int AllRecordsCount, int StartIndex, int ResultCount)
    [WebMethod(Description = "纠正后未切片数据（公开给PL生产线）")]
    public DataSet SearchCorrectedDataPL(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, out int AllRecordsCount, int StartIndex, int ResultCount)
    {
        return _searcherDb.SearchCorrectedDataPL(position, datetime, satellite, sensor, PixelSpacing, out AllRecordsCount, StartIndex, ResultCount);

    }

    [WebMethod(Description = "算法组件查询接口（公开给PL生产线）")]
    public DataSet SearchProductWFLPL(string ProductEnName, string ProductCnName, string ProductLevel, string version, out int AllRecordsCount, int StartIndex, int ResultCount)
    {
        return _searcherDb.SearchProductWFLPL(ProductEnName, ProductCnName, ProductLevel, version, out AllRecordsCount, StartIndex, ResultCount);

    }
    #endregion

    #region 集成共享接口

    #region 查询表记录
    [WebMethod(Description = "算法组件查询测试接口（公开给JCGX）")]
    public DataSet SearchAlgorithmJCGXTest()
    {
        List<string> UploadDate = new List<string>();
        int a = 0;
        return SearchAlgorithmJCGX(UploadDate, UploadDate, UploadDate, UploadDate, UploadDate, UploadDate, UploadDate, null, out a, 1, 10, 1, UploadDate, new List<int> { });
    }
    [WebMethod(Description = "算法组件查询接口（公开给JCGX）")]
    public DataSet SearchAlgorithmJCGX(List<string> UploadDate, List<string> ArtificialType, List<string> AlgEnName, List<string> DllName, List<string> AlgCnName, List<string> SuitableSatellites, List<string> SuitableSensors, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.SearchAlgorithmJCGX(UploadDate, ArtificialType, AlgEnName, DllName, AlgCnName, SuitableSatellites, SuitableSensors, IsOnCloud, out AllRecordsCount, StartIndex, ResultCount, QueryRange, strOrderBy, OrderByType);
    }

    [WebMethod(Description = "文档查询接口（公开给JCGX）")]
    public DataSet SearchDocumentJCGX(List<string> DocumentName, List<string> DocumentType, List<string> ProgramName, List<string> DocumentReleaseTime, List<string> Author, List<string> KeyWords, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.SearchDocumentJCGX(DocumentName, DocumentType, ProgramName, DocumentReleaseTime, Author, KeyWords, IsOnCloud, out AllRecordsCount, StartIndex, ResultCount, QueryRange, strOrderBy, OrderByType);

    }

    [WebMethod(Description = "工具包查询接口（公开给JCGX）")]
    public DataSet SearchToolkitJCGX(List<string> ToolkitReleaseTime, List<string> ToolkitName, List<string> ToolkitType, List<string> SuitableSatellites, List<string> SuitableSensors, List<string> OSBits, List<string> OStype, List<string> Author, List<string> KeyWords, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.SearchToolkitJCGX(ToolkitReleaseTime, ToolkitName, ToolkitType, SuitableSatellites, SuitableSensors, OSBits, OStype, Author, KeyWords, IsOnCloud, out AllRecordsCount, StartIndex, ResultCount, QueryRange, strOrderBy, OrderByType);

    }
    [WebMethod(Description = "纠正后数据查询接口测试接口（公开给JCGX）")]
    public DataSet SearchCorrectedDataJCGXtest()
    {
        string regionName = "北京";
        List<string> position = new List<string>();
        List<int> order = new List<int>();
        int AllRecordsCount = 1;
        return _searcherDb.SearchCorrectedDataJCGX(position, position, position, position, position, order, null, out AllRecordsCount, 0, 10, 0, position, order);
    }
    [WebMethod(Description = "纠正后数据查询接口（公开给JCGX）")]
    public DataSet SearchCorrectedDataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.SearchCorrectedDataJCGX(position, datetime, satellite, sensor, PixelSpacing, DataSizeRange, IsOnCloud, out AllRecordsCount, StartIndex, ResultCount, QueryRange, strOrderBy, OrderByType);
    }
    [WebMethod(Description = "查询原始的GF1数据测试接口（公开给JCGX）")]
    public DataSet SearchGF1DataJCGXtest()
    {
        string regionName = "北京";
        List<string> position = new List<string>();
        List<int> order = new List<int>();
        int AllRecordsCount = 1;
        return _searcherDb.SearchGF1DataJCGX(position, new List<string>() { "2012-05-05 10:10:22", "2015-05-05 10:10:22" }, position, position, out AllRecordsCount, 0, 10, position, order);
    }
    [WebMethod(Description = "查询原始的GF1数据（公开给JCGX）")]
    public DataSet SearchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.SearchGF1DataJCGX(position, datetime, satellite, sensor, out AllRecordsCount, StartIndex, ResultCount, strOrderBy, OrderByType);
    }

    [WebMethod(Description = "行政区检索测试接口")]
    public DataSet SearchGFByRegionTest()
    {
        int m = 5;
        List<int> n = new List<int>() { 0 };
        return SearchGFByRegion("北京市", "市", null, null, null, null, out m, 0, 5, null, n);
    }

    [WebMethod(Description = "行政区检索")]
    public DataSet SearchGFByRegion(string regionName, string category, string type, List<string> datetime,
        List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.SearchGFByRegion(regionName, category, type, datetime, satellite, sensor, out AllRecordsCount, StartIndex, ResultCount, strOrderBy, OrderByType);
    }

    [WebMethod(Description = "查询原始的GF1数据测试接口（公开给JCGX）")]
    public DataSet SearchBatchGF1DataJCGXTest()
    {
        List<string> position = new List<string>();
        List<int> order = new List<int>();
        int AllRecordsCount = 1;
        return _searcherDb.SearchBatchGF1DataJCGX(position, null, position, position, out AllRecordsCount, 1, 10, position, order, null, null, null);
    }


    [WebMethod(Description = "查询原始的GF1数据（公开给JCGX）")]
    public DataSet SearchBatchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType, string PointField, string KeyWords, string KeyValues)
    {
        return _searcherDb.SearchBatchGF1DataJCGX(position, datetime, satellite, sensor, out AllRecordsCount, StartIndex, ResultCount, strOrderBy, OrderByType, PointField, KeyWords, KeyValues);
    }
    [WebMethod(Description = "测试")]
    public DataSet SearchData()
    {
        return _searcherDb.SearchData();
    }

    [WebMethod(Description = "根据数据提交时间对GF数据进行检索，公开给JCGX")]
    public DataSet SearchGF1DataForJCGXByImportTime(String importStartTime, String importEndTime)
    {
        return _searcherDb.SearchGF1DataForJCGXByImportTime(importStartTime, importEndTime);
    }


    [WebMethod(Description = "GFF数据查询")]
    public DataSet SearchGFFDataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.SearchGFFDataJCGX(position, datetime, satellite, sensor, PixelSpacing, DataSizeRange, IsOnCloud, out AllRecordsCount, StartIndex, ResultCount, strOrderBy, OrderByType);
    }

    #endregion

    #region 根据编码查询单条记录详细信息
    //[WebMethod(Description = "根据数据ID获取该数据的详细元数据信息")]
    //public DataSet GetDataInfoByID(string dataID)
    //{
    //    return MetaData.GetDataInfo(dataID);
    //}
    #endregion

    #region 查询、添加、修改用户评价、评分等公共信息
    [WebMethod(Description = "根据用户ID和数据ID查询用户评价信息（公开给JCGX）")]
    public DataSet SearchUserEvaluationInfoJCGX(string DataId, string UserId, out int AllRecordsCount, int StartIndex, int ResultCount)
    {
        return _searcherDb.SearchUserEvaluationInfoJCGX(DataId, UserId, out AllRecordsCount, StartIndex, ResultCount);
    }

    [WebMethod(Description = "添加用户评价信息")]
    public string AddUserEvaluationInfoJCGX(string dataID, string UserID, string Description, int Score, string BuyId)
    {
        return _searcherDb.AddUserEvaluationInfoJCGX(dataID, UserID, Description, Score, BuyId);
    }

    [WebMethod(Description = "根据数据ID,用户ID以及评价信息的时间范围，删除评价信息 ")]
    public string DeleteUserEvaluationInfoJCGX(int id)
    {
        return _searcherDb.DeleteUserEvaluationInfoJCGX(id);
    }

    [WebMethod(Description = "根据数据ID,修改数据的价格、下载次数以及平均得分")]
    public string UpDatePublicInfoJCGX(string DataID, string Price, int DownloadCount, double AverageScore)
    {
        return _searcherDb.UpDatePublicInfoJCGX(DataID, Price, DownloadCount, AverageScore);
    }

    [WebMethod(Description = "获取某个数据的评价记录数")]
    public int GetEvaluationCount(string dataID)
    {
        return _searcherDb.GetEvaluationCount(dataID);
    }
    #endregion

    #region 查询行政区划信息
    [WebMethod(Description = "查询中国的省信息")]
    public DataSet SearchProvince()
    {
        return _searcherDb.SearchProvince();
    }

    [WebMethod(Description = "根据省名称查询该省的地级市")]
    public DataSet SearchCityByProvince(string provinceName)
    {
        return _searcherDb.SearchCityByProvince(provinceName);
    }

    [WebMethod(Description = "根据省市查询该市的街道，县市")]
    public DataSet SearchCountyByCity(string provinceName, string cityName)
    {
        return _searcherDb.SearchCountyByCity(provinceName, cityName);
    }


    #endregion

    #region  行业信息查询
    [WebMethod(Description = "返回所有的行业信息")]
    public DataSet GetIndustryInfo()
    {
        return _searcherDb.GetIndustryInfo();
    }
    #endregion

    #region 产品级别和产品名称查询
    [WebMethod(Description = "返回所有生产产品的信息")]
    public DataSet GetProductInfo()
    {
        return _searcherDb.GetProductInfo();
    }
    #endregion

    #region 算法详情信息查询
    [WebMethod(Description = "根据算法ID获取该算法的参数信息")]
    public DataSet GetParasInfoByAlgID(string AlgID, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType)
    {
        return _searcherDb.GetParasInfoByAlgID(AlgID, out AllRecordsCount, StartIndex, ResultCount, strOrderBy, OrderByType);
    }
    #endregion

    #endregion

    #region 统一的对外查询接口 20130909 zxw
    [WebMethod(Description = "根据表编码获取要查询表或视图的查询字段结构")]
    public DataSet GetDataTableStruct(string tableCode)
    {
        return _searcherDb.GetDataTableStruct(tableCode);
    }
    [WebMethod(Description = "查询综合数据库元数据表！")]
    public QueryResponse GetMetadata(QueryRequest _request)
    {
        return _searcherDb.GetMetadata(_request);

    }
    [WebMethod(Description = "查询综合数据库元数据表！")]
    public DataSet GetMetadataStr(string _xmlstrQueryRequest)
    {
        return _searcherDb.GetMetadataStr(_xmlstrQueryRequest);
    }

    [WebMethod(Description = "获取查询记录总数！")]
    public int GetTotalRecord(QueryRequest _request)
    {
        return _searcherDb.GetTotalRecord(_request);
    }

    [WebMethod(Description = "根据数据ID查询数据记录详情！")]
    public DataSet GetDetailInfo(string qrst_code)
    {
        return _searcherDb.GetDetailInfo(qrst_code);
    }

    #endregion

    #region 行政区划分级及 对应经纬度获取
    [WebMethod(Description = "获取省级行政区划列表")]
    public string[] GetProvinceLst()
    {
        return _searcherDb.GetProvinceLst();
    }

    [WebMethod(Description = "获取市级行政区划列表")]
    public string[] GetCityLstByProvince(string province)
    {
        return _searcherDb.GetCityLstByProvince(province);
    }

    [WebMethod(Description = "获取县级行政区划列表")]
    public string[] GetCountyLstByProvinceCity(string province, string city)
    {
        return _searcherDb.GetCountyLstByProvinceCity(province, city);
    }

    [WebMethod(Description = "获取经纬度信息，根据省，市，县。 最大经度，最大纬度，最小经度，最小纬度")]
    public double[] GetSpacialInfoByCounty(string province, string city, string county)
    {
        return _searcherDb.GetSpacialInfoByCounty(province, city, county);
    }

    [WebMethod(Description = "获取经纬度信息，根据省，市。 最大经度，最大纬度，最小经度，最小纬度")]
    public double[] GetSpacialInfoByCity(string province, string city)
    {
        return _searcherDb.GetSpacialInfoByCity(province, city);
    }

    [WebMethod(Description = "获取经纬度信息，根据省。最大经度，最大纬度，最小经度，最小纬度")]
    public double[] GetSpacialInfoByProvince(string province)
    {
        return _searcherDb.GetSpacialInfoByProvince(province);
    }

    [WebMethod(Description = "获取经纬度信息，根据省，市，县。点信息详情")]
    public string GetSpacialDetailInfoByCounty(string province, string city, string county)
    {
        return _searcherDb.GetSpacialDetailInfoByCounty(province, city, county);
    }

    [WebMethod(Description = "获取经纬度信息，根据省，市。点信息详情")]
    public string GetSpaciaDetaillInfoByCity(string province, string city)
    {
        return _searcherDb.GetSpaciaDetaillInfoByCity(province, city);
    }

    [WebMethod(Description = "获取经纬度信息，根据省。点信息详情")]
    public string GetSpacialDetailInfoByProvince(string province)
    {
        return _searcherDb.GetSpacialDetailInfoByProvince(province);
    }
    #endregion

    #region 仿真组接口
    [WebMethod(Description = "仿真组：查找卫星、传感器、波段完整信息列表")]
    public string GetSateAndSensorAndBandInfo()
    {
        return _searcherDb.GetSateAndSensorAndBandInfo();
    }

    [WebMethod(Description = "仿真组：查找所有卫星信息")]
    public string GetSateInfo()
    {
        return _searcherDb.GetSateInfo();
    }

    [WebMethod(Description = "仿真组：根据卫星编号获取对应传感器信息")]
    public string GetSensorLstBySateID(string SateID)
    {
        return _searcherDb.GetSensorLstBySateID(SateID);
    }

    [WebMethod(Description = "仿真组：根据传感器编号获取对应波段信息")]
    public string GetBandLstBySensorID(string SensorID)
    {
        return _searcherDb.GetBandLstBySensorID(SensorID);
    }
    #endregion

    #region 数据自我检验接口
    [WebMethod(Description = "验证数据是否已经存入数据库,若已存在返回值为true")]
    public bool GF1DataCertificate(string dataName)
    {
        return _searcherDb.GF1DataCertificate(dataName);
    }

    [WebMethod(Description = "验证多条数据是否已经存入数据库，返回未入库的数据列表测试接口。")]
    public List<string> GF1DataCertificate_Multitest()
    {
        List<string> dataNames = new List<string>() { "GF1_PMS1_E82.2_N44.7_20130816_L1A0000070784.tar.gz" };
        return _searcherDb.GF1DataCertificate_Multi(dataNames);
    }
    [WebMethod(Description = "验证多条数据是否已经存入数据库，返回未入库的数据列表。")]
    public List<string> GF1DataCertificate_Multi(List<string> dataNames)
    {
        return _searcherDb.GF1DataCertificate_Multi(dataNames);
    }

    #endregion


    #region "973Std接口"
    [WebMethod(Description = "查询973标准数据")]
    public DataSet Search973StdData(string stdName, string prodType,
        string startTime, string endTime, double dStartLong, double dEndLong, double dStartLat, double dEndLat, string filenameFilter)
    {
        return _searcherDb.Search973StdData(stdName, prodType, startTime, endTime, dStartLong, dEndLong, dStartLat, dEndLat, filenameFilter);
    }
    #endregion



    #region "973接口"
    [WebMethod(Description = "分页查询973标准和原始数据")]
    public DataSet Search973Data(string dataName, string prodType, string startTime, string endTime, double dStartLong, double dEndLong, double dStartLat, double dEndLat, int onPage, int countOfPerPage, out int totalCount)
    {
        return _searcherDb.Search973Data(dataName, prodType, startTime, endTime, dStartLong, dEndLong, dStartLat, dEndLat, onPage, countOfPerPage, out totalCount);
    }

    #endregion


    #region "973下载接口"
    [WebMethod(Description = "973下载")]
    public string Download973Data(List<string> listPath, List<string> listFilename, string targetFolder)
    {
        return _searcherDb.Download973Data(listPath, listFilename, targetFolder);
    }

    #endregion


    #region "973详情接口"
    [WebMethod(Description = "返回文件名的详细信息")]
    public DataSet Info973(string dataName, string searchFileName)
    {
        return _searcherDb.Info973(dataName, searchFileName);
    }


    #endregion

    #region "集成共享网站查询，基础空间数据查找"
    [WebMethod(Description = "返回查询到的基础空间数据库中的所有父节点名称")]
    public String[] getAllBSDBGroupNodeNames()
    {
        return _searcherDb.getAllBSDBGroupNodeNames();
    }

    [WebMethod(Description = "根据父节点名称返回子节点名称列表")]
    public String[] getChildNamesByGroupNameForBSDB(String groupName)
    {
        return _searcherDb.getChildNamesByGroupNameForBSDB(groupName);
    }

    [WebMethod(Description = "根据要查询的数据名称查询数据库中所包含此类数据的元数据信息")]
    public System.Data.DataSet searchBSBDInfosByQueryName(String keyWord, double maxLon, double minLon, double maxLat, double minLat,
        String startTime, String endTime, int pageNum, int pageSize)
    {
        return _searcherDb.searchBSBDInfosByQueryName(keyWord, maxLon, minLon, maxLat, minLat, startTime, endTime, pageNum, pageSize);
    }

    [WebMethod(Description = "根据要查询的数据名称查询数据库中所包含此类数据的元数据信息")]
    public System.Data.DataSet getBSBDInfosByQueryName(String childNodeName, double maxLon, double minLon, double maxLat, double minLat, String startTime, String endTime,
        String keyWord, Boolean searchType, int pageNum, int pageSize)
    {
        return _searcherDb.getBSBDInfosByQueryName(childNodeName, maxLon, minLon, maxLat, minLat, startTime, endTime, keyWord, searchType, pageNum, pageSize);
    }
    #endregion

    #region "集成共享网站查询，模型算法查找"
    [WebMethod(Description = "返回查询到的模型算法数据库中的所有父节点名称")]
    public String[] getAllMADBGroupNodeNames()
    {
        return _searcherDb.getAllMADBGroupNodeNames();
    }

    [WebMethod(Description = "根据父节点名称返回子节点名称列表")]
    public String[] getChildNamesByGroupNameForMADB(String groupName)
    {
        return _searcherDb.getChildNamesByGroupNameForMADB(groupName);
    }

    [WebMethod(Description = "根据要查询的数据名称查询数据库中所包含此类数据的元数据信息")]
    public System.Data.DataSet getMADBInfosByQueryName(String childNodeName, string startTime, string endTime, String keyWord, string fileName, Boolean searchType, int pageNum, int pageSize)
    {
        return _searcherDb.getMADBInfosByQueryName(childNodeName, startTime, endTime, keyWord, fileName, searchType, pageNum, pageSize);
    }
    #endregion

    #region 集成共享网站使用_波普特征数据查询
    [WebMethod(Description = "集成共享使用_获取波普对象名")]
    public System.Data.DataSet GetBPNames()
    {
        return _searcherDb.GetBPNames();
    }

    [WebMethod(Description = "集成共享使用_获取土壤波普特征记录")]
    public DataTable GetSoilBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string soilname, string soilzilei, string pagenum, string pagesize)
    {
        return _searcherDb.GetSoilBPRecord(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, soilname, soilzilei, pagenum, pagesize);
    }
    [WebMethod(Description = "集成共享使用_获取土壤波普'子类'参数值")]
    public string[] GetSoilTypes()
    {
        return _searcherDb.GetSoilTypes();
    }
    [WebMethod(Description = "集成共享使用_获取岩矿波普特征记录")]
    public DataTable GetRockBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string r_ykmc, string r_yklb, string r_ykzl, string r_sslb, string pagenum, string pagesize)
    {
        return _searcherDb.GetRockBPRecord(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, r_ykmc, r_yklb, r_ykzl, r_sslb, pagenum, pagesize);
    }

    [WebMethod(Description = "集成共享使用_获取岩矿波普‘岩矿类别’参数")]
    public string[] GetRockType()
    {
        return _searcherDb.GetRockType();
    }
    [WebMethod(Description = "集成共享使用_获取岩矿波普‘岩矿子类’参数")]
    public string[] GetRockSubType()
    {
        return _searcherDb.GetRockSubType();
    }
    [WebMethod(Description = "集成共享使用_获取岩矿波普‘所属类别’参数")]
    public string[] GetRockSSLR()
    {
        return _searcherDb.GetRockSSLR();
    }
    [WebMethod(Description = "集成共享使用_获取北方植被波普特征记录")]
    public DataTable GetNorthVegBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string v_zbmc, string v_zblb, string v_clbw, string v_whq, string pagenum, string pagesize)
    {
        return _searcherDb.GetNorthVegBPRecord(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, pagenum, pagesize);
    }
    [WebMethod(Description = "集成共享使用_获取北方植被波普‘植被类别’参数")]
    public string[] GetNorthVegType()
    {
        return _searcherDb.GetNorthVegType();
    }
    [WebMethod(Description = "集成共享使用_获取北方植被波普‘测量部位’参数")]
    public string[] GetNorthVegBW()
    {
        return _searcherDb.GetNorthVegBW();
    }
    [WebMethod(Description = "集成共享使用_获取北方植被波普‘物候期’参数")]
    public string[] GetNorthVegWHQ()
    {
        return _searcherDb.GetNorthVegWHQ();
    }
    [WebMethod(Description = "集成共享使用_获取南方植被波普特征记录")]
    public DataTable GetSouthVegBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string v_zbmc, string v_zblb, string v_clbw, string v_whq, string pagenum, string pagesize)
    {
        return _searcherDb.GetSouthVegBPRecord(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, v_zbmc, v_zblb, v_clbw, v_whq, pagenum, pagesize);
    }
    [WebMethod(Description = "集成共享数据更新‘起止时间、卫星’参数")]
    public System.Data.DataSet UpdateForJCGX(string startTime, string endTime, string satellite)
    {
        return _searcherDb.UpdateForJCGX(startTime, endTime, satellite);
    }
    [WebMethod(Description = "集成共享使用_获取城市目标‘目标类别’参数")]
    public string[] GetCityObjType()
    {
        return _searcherDb.GetCityObjType();
    }
    [WebMethod(Description = "集成共享使用_获取城市目标波普‘目标名称’参数")]
    public string[] GetCityObjName()
    {
        return _searcherDb.GetCityObjName();
    }
    [WebMethod(Description = "集成共享使用_获取城市目标波普特征记录")]
    public DataTable GetCityObjBPRecord(string begindate, string enddate, string c_csmc, string c_cslb, string pagenum, string pagesize)
    {
        return _searcherDb.GetCityObjBPRecord(begindate, enddate, c_csmc, c_cslb, pagenum, pagesize);
    }
    [WebMethod(Description = "集成共享使用_获取水体波普‘光谱仪器’参数")]
    public string[] GetWaterGPYQ()
    {
        return _searcherDb.GetWaterGPYQ();
    }
    [WebMethod(Description = "集成共享使用_获取水体波普‘所属类别’参数")]
    public string[] GetWaterType()
    {
        return _searcherDb.GetWaterType();
    }
    [WebMethod(Description = "集成共享使用_获取水体波普‘水域名称’参数")]
    public string[] GetWaterSYMC()
    {
        return _searcherDb.GetWaterSYMC();
    }
    [WebMethod(Description = "集成共享使用_获取水体波普特征记录")]
    public DataTable GetWaterBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string w_symc, string w_gpyq, string w_sslb, string pagenum, string pagesize)
    {
        return _searcherDb.GetWaterBPRecord(begindate, enddate, Maxlat, MaxLon, MinLat, MinLon, w_symc, w_gpyq, w_sslb, pagenum, pagesize);
    }
    [WebMethod(Description = "集成共享使用_获取地表大气波普‘观测站编号’参数")]
    public string[] GetAtomsDZBH()
    {
        return _searcherDb.GetAtomsDZBH();
    }
    [WebMethod(Description = "集成共享使用_获取地表大气波普‘观测站名称’参数")]
    public string[] GetAtomsDZMC()
    {
        return _searcherDb.GetAtomsDZMC();
    }
    [WebMethod(Description = "集成共享使用_获取地表大气波普特征记录")]
    public DataTable GetAtomspBPRecord(string Maxlat, string MaxLon, string MinLat, string MinLon, string a_zdmc, string a_zdbh, string pagenum, string pagesize)
    {
        return _searcherDb.GetAtomspBPRecord(Maxlat, MaxLon, MinLat, MinLon, a_zdmc, a_zdbh, pagenum, pagesize);
    }
    [WebMethod(Description = "集成共享使用_获取南方植被波普‘物候期’参数")]
    public string[] GetSouthVegWHQ()
    {
        return _searcherDb.GetSouthVegWHQ();
    }
    [WebMethod(Description = "集成共享使用_获取南方植被波普‘测量部位’参数")]
    public string[] GetSouthVegBW()
    {
        return _searcherDb.GetSouthVegBW();
    }
    [WebMethod(Description = "集成共享使用_获取南方植被波普‘植被类别’参数")]
    public string[] GetSouthVegType()
    {
        return _searcherDb.GetSouthVegType();
    }
    #endregion

    [WebMethod(Description = "根据一个面切分成格网，获取每个格网有多少条数据相交并返回数据集,暂时不用")]
    public List<System.Data.DataSet> SerachDataByCoordsStrceshi1(string coordsStr, string tilelevel, int startdate, int enddate, out List<int> allcount)
    {
        return _searcherDb.SerachDataByCoordsStrceshi1(coordsStr, tilelevel, startdate, enddate, out allcount);
    }

    [WebMethod(Description = "根据一个面切分成格网，获取每个格网有多少条数据相交并返回数据集(行号，列号，个数),暂时不用")]
    public System.Data.DataSet SerachDataCountByCoordsStrceshi2(string coordsStr, string tilelevel)
    {
        return _searcherDb.SerachDataCountByCoordsStrceshi2(coordsStr, tilelevel);
    }

    [WebMethod(Description = "根据一个面切分成格网，获取每个格网有多少条数据相交并返回数据集(行号，列号，个数)")]
    public System.Data.DataTable SerachDataCountByCoordsStr(string coordsStr, string tilelevel, string datalist)
    {
        return _searcherDb.SerachDataCountByCoordsStr(coordsStr, tilelevel, datalist);
    }
}
