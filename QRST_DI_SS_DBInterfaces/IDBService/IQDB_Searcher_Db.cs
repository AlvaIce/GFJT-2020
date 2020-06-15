/**描述：SQLite八大数据库操作接口
 * 作者：jianghua
 * 日期：20170411
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;
using DotSpatial.Topology;

namespace QRST_DI_SS_DBInterfaces.IDBService
{
   public interface IQDB_Searcher_Db
    {

        /// <summary>
        /// 以一个XElement对象返回的数据库中数据类型目录
        /// </summary>
        /// <returns></returns>
        XmlDocument GetTreeCatalogByXML();

        /// <summary>
        /// 根据数据类型编码获得可选字段列表
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        string[] GetFieldsByDataCode(string dataCode);

        /// <summary>
        /// 查询水溶胶数据
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        DataSet SearchAOD(List<string> datetime, string dataType);

        /// <summary>
        /// 根据字段高级查询文档DOC
        /// </summary>
        /// <param name="TITLE"></param>
        /// <param name="DOCTYPE"></param>
        /// <param name="KEYWORD"></param>
        /// <param name="AUTHOR"></param>
        /// <param name="UPLOADER"></param>
        /// <param name="UPLOADTIMEStart"></param>
        /// <param name="UPLOADTIMEOver"></param>
        /// <returns></returns>
        DataSet SearchDOCByField(string TITLE, string DOCTYPE, string KEYWORD, string AUTHOR, string UPLOADER,
            string UPLOADTIMEStart, string UPLOADTIMEOver);

        /// <summary>
        /// 根据字段模糊查询文档DOC
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        DataSet SearchDOCByKeyWord(string KeyWord);

        /// <summary>
        /// 根据字段高级查询当前页比如100条数据文档DOC
        /// </summary>
        /// <param name="TITLE"></param>
        /// <param name="DOCTYPE"></param>
        /// <param name="KEYWORD"></param>
        /// <param name="AUTHOR"></param>
        /// <param name="UPLOADER"></param>
        /// <param name="UPLOADTIMEStart"></param>
        /// <param name="UPLOADTIMEOver"></param>
        /// <param name="pageNow"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        DataSet SearchDOCByFieldPage(string TITLE, string DOCTYPE, string KEYWORD, string AUTHOR, string UPLOADER,
            string UPLOADTIMEStart, string UPLOADTIMEOver, int pageNow, int pageSize);

        #region 统一的对外查询接口 20130909 zxw
        /// <summary>
        /// 根据表编码获取要查询表或视图的查询字段结构
        /// </summary>
        /// <param name="tableCode"></param>
        /// <returns></returns>
        DataSet GetDataTableStruct(string tableCode);

        /// <summary>
        /// 查询综合数据库元数据表
        /// </summary>
        /// <param name="_request"></param>
        /// <returns></returns>
        QueryResponse GetMetadata(QueryRequest _request);

        /// <summary>
        /// 查询综合数据库元数据表
        /// </summary>
        /// <param name="_xmlstrQueryRequest"></param>
        /// <returns></returns>
        DataSet GetMetadataStr(string _xmlstrQueryRequest);

        /// <summary>
        /// 获取查询记录总数
        /// </summary>
        /// <param name="_request"></param>
        /// <returns></returns>
        int GetTotalRecord(QueryRequest _request);

        /// <summary>
        /// 根据数据ID查询数据记录详情
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <returns></returns>
        DataSet GetDetailInfo(string qrst_code);
        #endregion

        #region 行政区划分级及 对应经纬度获取
        /// <summary>
        /// 获取省级行政区划列表
        /// </summary>
        /// <returns></returns>
        string[] GetProvinceLst();

        /// <summary>
        /// 获取市级行政区划列表
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        string[] GetCityLstByProvince(string province);

        /// <summary>
        /// 获取县级行政区划列表
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        string[] GetCountyLstByProvinceCity(string province, string city);

        /// <summary>
        /// 获取经纬度信息，根据省，市，县。 最大经度，最大纬度，最小经度，最小纬度
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="county"></param>
        /// <returns></returns>
        double[] GetSpacialInfoByCounty(string province, string city, string county);

        /// <summary>
        /// 获取经纬度信息，根据省，市。 最大经度，最大纬度，最小经度，最小纬度
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        double[] GetSpacialInfoByCity(string province, string city);

        /// <summary>
        /// 获取经纬度信息，根据省。最大经度，最大纬度，最小经度，最小纬度
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        double[] GetSpacialInfoByProvince(string province);

        /// <summary>
        /// 获取经纬度信息，根据省，市，县。点信息详情
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="county"></param>
        /// <returns></returns>
        string GetSpacialDetailInfoByCounty(string province, string city, string county);

        /// <summary>
        /// 获取经纬度信息，根据省，市。点信息详情
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        string GetSpaciaDetaillInfoByCity(string province, string city);

        /// <summary>
        /// 获取经纬度信息，根据省。点信息详情
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        string GetSpacialDetailInfoByProvince(string province);
        #endregion

        #region 数据自我检验接口
        /// <summary>
        /// 验证数据是否已经存入数据库,若已存在返回值为true
        /// </summary>
        /// <param name="dataName"></param>
        /// <returns></returns>
        bool GF1DataCertificate(string dataName);

        /// <summary>
        /// 验证多条数据是否已经存入数据库，返回未入库的数据列表。
        /// </summary>
        /// <param name="dataNames"></param>
        /// <returns></returns>
        List<string> GF1DataCertificate_Multi(List<string> dataNames);


        #endregion

        #region MS_波普查询接口
        #region 大气查询
        /// <summary>
        /// 获取大气波普记录
        /// </summary>
        /// <param name="maxLat"></param>
        /// <param name="maxLng"></param>
        /// <param name="minLat"></param>
        /// <param name="minLng"></param>
        /// <param name="zdmc"></param>
        /// <param name="zdbh"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        String getQueryAtmospheres(String maxLat, String maxLng, String minLat, String minLng, String zdmc, String zdbh,
            String pagenum, String pagesize);

        /// <summary>
        /// 获取地表大气波普‘观测站编号’属性值
        /// </summary>
        /// <returns></returns>
        String getAtmosZDBH();

        /// <summary>
        /// 获取地表大气波普‘观测站名称’属性值
        /// </summary>
        /// <returns></returns>
        String getAtmosZDMC();
        #endregion

        #region 城市目标
        /// <summary>
        /// 获取城市目标波普特征记录
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="dte"></param>
        /// <param name="csmbmc"></param>
        /// <param name="csmblb"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        String getQueryCityObjs(String dts, String dte, String csmbmc, String csmblb, String pagenum, String pagesize);

        /// <summary>
        /// 获取城市目标波普‘目标名称’参数
        /// </summary>
        /// <returns></returns>
        String getCityCSMBMC();

        /// <summary>
        /// 获取城市目标‘目标类别’参数
        /// </summary>
        /// <returns></returns>
        String getCityTypes();

        /// <summary>
        /// 获取城市目标波普值
        /// </summary>
        /// <param name="fseq"></param>
        /// <returns></returns>
        String getCityGPSJ(String fseq);
        #endregion

        #region 岩石
        /// <summary>
        /// 获取岩矿波普‘所属类别’参数
        /// </summary>
        /// <returns></returns>
        String getRockSSLB();

        /// <summary>
        /// 获取岩矿波普‘岩矿子类’参数
        /// </summary>
        /// <returns></returns>
        String getRockSubTypes();

        /// <summary>
        /// 获取岩矿波普‘岩矿类别’参数
        /// </summary>
        /// <returns></returns>
        String getRockTypes();

        /// <summary>
        /// 获取岩矿波普特征记录
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="dte"></param>
        /// <param name="maxLat"></param>
        /// <param name="maxLng"></param>
        /// <param name="minLat"></param>
        /// <param name="minLng"></param>
        /// <param name="ykmc"></param>
        /// <param name="yklb"></param>
        /// <param name="ykzl"></param>
        /// <param name="sslb"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        String getQueryRocks(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng,
            String ykmc, String yklb, String ykzl, String sslb, String pagenum, String pagesize);

        /// <summary>
        /// 获取岩矿波普值
        /// </summary>
        /// <param name="fseq"></param>
        /// <returns></returns>
        String getRockGPSJ(String fseq);
        #endregion

        #region 土壤
        /// <summary>
        /// 获取土壤波普'子类'参数值
        /// </summary>
        /// <returns></returns>
        String getSoilSubTypes();

        /// <summary>
        /// 获取土壤波普特征记录
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="dte"></param>
        /// <param name="maxLat"></param>
        /// <param name="maxLng"></param>
        /// <param name="minLat"></param>
        /// <param name="minLng"></param>
        /// <param name="trmc"></param>
        /// <param name="trzl"></param>
        /// <param name="fgwmc"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        String getQuerySoils(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng,
            String trmc, String trzl, String fgwmc, String pagenum, String pagesize);

        /// <summary>
        /// 获取土壤波普值
        /// </summary>
        /// <param name="fseq"></param>
        /// <returns></returns>
        String getSoilGPSJ(String fseq);
        #endregion

        #region 北方植被
        /// <summary>
        /// 获取北方植被波普‘物候期’参数
        /// </summary>
        /// <returns></returns>
        String getNorVegWHQ();

        /// <summary>
        /// 获取北方植被波普‘测量部位’参数"
        /// </summary>
        /// <returns></returns>
        String getNorVegCLBW();

        /// <summary>
        /// 获取北方植被波普‘植被类别’参数
        /// </summary>
        /// <returns></returns>
        String getNorVegTypes();

        /// <summary>
        /// 获取北方植被波普特征记录
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="dte"></param>
        /// <param name="maxLat"></param>
        /// <param name="maxLng"></param>
        /// <param name="minLat"></param>
        /// <param name="minLng"></param>
        /// <param name="zbmc"></param>
        /// <param name="zblb"></param>
        /// <param name="clbw"></param>
        /// <param name="whq"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        String getQueryNorVegetations(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng,
            String zbmc, String zblb, String clbw, String whq, String pagenum, String pagesize);

        /// <summary>
        /// 获取北方植被波普值
        /// </summary>
        /// <param name="fseq"></param>
        /// <returns></returns>
        String getNorVegGPSJ(String fseq);
        #endregion

        #region 南方植被
        /// <summary>
        /// 获取南方植被波普‘物候期’参数
        /// </summary>
        /// <returns></returns>
        String getSouVegWHQ();

        /// <summary>
        /// 获取南方植被波普‘测量部位’参数
        /// </summary>
        /// <returns></returns>
        String getSouVegCLBW();

        /// <summary>
        /// 获取南方植被波普‘植被类别’参数
        /// </summary>
        /// <returns></returns>
        String getSouVegTypes();

        /// <summary>
        /// 获取南方植被波普特征记录
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="dte"></param>
        /// <param name="maxLat"></param>
        /// <param name="maxLng"></param>
        /// <param name="minLat"></param>
        /// <param name="minLng"></param>
        /// <param name="zbmc"></param>
        /// <param name="zblb"></param>
        /// <param name="clbw"></param>
        /// <param name="whq"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        String getQueryVegetations(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng,
            String zbmc, String zblb, String clbw, String whq, String pagenum, String pagesize);

        /// <summary>
        /// 获取南方植被波普值
        /// </summary>
        /// <param name="fseq"></param>
        /// <returns></returns>
        String getSouVegGPSJ(String fseq);
        #endregion

        #region 水
        /// <summary>
        /// 获取水体波普‘光谱仪器’参数
        /// </summary>
        /// <returns></returns>
        String getWaterGPYQ();

        /// <summary>
        /// 获取水体波普‘所属类别’参数
        /// </summary>
        /// <returns></returns>
        String getWaterSSLB();

        /// <summary>
        /// 获取水体波普‘水域名称’参数
        /// </summary>
        /// <returns></returns>
        String getWaterSYMC();

        /// <summary>
        /// 获取水体波普特征记录
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="dte"></param>
        /// <param name="maxLat"></param>
        /// <param name="maxLng"></param>
        /// <param name="minLat"></param>
        /// <param name="minLng"></param>
        /// <param name="symc"></param>
        /// <param name="gpyq"></param>
        /// <param name="sslb"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        String getQueryWaters(String dts, String dte, String maxLat, String maxLng, String minLat, String minLng,
            String symc, String gpyq, String sslb, String pagenum, String pagesize);

        /// <summary>
        /// 获取水体波普值
        /// </summary>
        /// <param name="fseq"></param>
        /// <returns></returns>
        String getWaterGPSJ(String fseq);


        #endregion

        #endregion

        #region 集成共享网站使用_波普特征数据查询
        DataSet GetBPNames();

        DataTable GetSoilBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string soilname, string soilzilei, string pagenum, string pagesize);

        string[] GetSoilTypes();

        DataTable GetRockBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat,
            string MinLon, string r_ykmc, string r_yklb, string r_ykzl, string r_sslb, string pagenum, string pagesize);

        string[] GetRockType();

        string[] GetRockSubType();

        string[] GetRockSSLR();

        DataTable GetNorthVegBPRecord(string begindate, string enddate, string Maxlat,
            string MaxLon, string MinLat, string MinLon, string v_zbmc, string v_zblb, string v_clbw, string v_whq, string pagenum, string pagesize);

        string[] GetNorthVegType();

        string[] GetNorthVegBW();

        string[] GetNorthVegWHQ();

        DataTable GetSouthVegBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string v_zbmc, string v_zblb,
            string v_clbw, string v_whq, string pagenum, string pagesize);

        string[] GetSouthVegType();

        string[] GetSouthVegBW();

        string[] GetSouthVegWHQ();

        DataTable GetAtomspBPRecord(string Maxlat, string MaxLon, string MinLat, string MinLon, string a_zdmc, string a_zdbh, string pagenum, string pagesize);

        string[] GetAtomsDZMC();

        string[] GetAtomsDZBH();

        DataTable GetWaterBPRecord(string begindate, string enddate, string Maxlat, string MaxLon, string MinLat, string MinLon, string w_symc,
            string w_gpyq, string w_sslb, string pagenum, string pagesize);

        string[] GetWaterSYMC();

        string[] GetWaterType();

        string[] GetWaterGPYQ();

        DataTable GetCityObjBPRecord(string begindate, string enddate, string c_csmc, string c_cslb, string pagenum, string pagesize);

        string[] GetCityObjName();

        string[] GetCityObjType();

        DataSet UpdateForJCGX(string startTime, string endTime, string satellite);


        #endregion

        #region "集成共享网站查询，模型算法查找"
        String[] getAllMADBGroupNodeNames();

        String[] getChildNamesByGroupNameForMADB(String groupName);

        DataSet getMADBInfosByQueryName(String childNodeName, string startTime, string endTime, String keyWord, 
            string fileName, Boolean searchType, int pageNum, int pageSize);
        #endregion

        #region "集成共享网站查询，基础空间数据查找"
        String[] getAllBSDBGroupNodeNames();

        String[] getChildNamesByGroupNameForBSDB(String groupName);

        DataSet searchBSBDInfosByQueryName(String keyWord, double maxLon, double minLon, double maxLat, double minLat,
            String startTime, String endTime, int pageNum, int pageSize);

        DataSet getBSBDInfosByQueryName(String childNodeName, double maxLon, double minLon, double maxLat, double minLat,
            String startTime, String endTime, String keyWord, Boolean searchType, int pageNum, int pageSize);

        #endregion

        #region 973接口
        DataSet Info973(string dataName, string searchFileName);
        DataSet Search973Data(string dataName, string prodType, string startTime, string endTime, double dStartLong, double dEndLong,
    double dStartLat, double dEndLat, int onPage, int countOfPerPage, out int totalCount);

        DataSet Search973StdData(string stdName, string prodType, string startTime, string endTime, double dStartLong,
    double dEndLong, double dStartLat, double dEndLat, string filenameFilter);
        #endregion


        #region "973下载接口"
        string Download973Data(List<string> listPath, List<string> listFilename, string targetFolder);

        #region 查询、添加、修改用户评价、评分等公共信息
        DataSet SearchUserEvaluationInfoJCGX(string DataId, string UserId, out int AllRecordsCount, int StartIndex, int ResultCount);

        string AddUserEvaluationInfoJCGX(string dataID, string UserID, string Description, int Score, string BuyId);

        string DeleteUserEvaluationInfoJCGX(int id);

        string UpDatePublicInfoJCGX(string DataID, string Price, int DownloadCount, double AverageScore);

        int GetEvaluationCount(string dataID);
        #endregion

        #region 查询表记录
        DataSet SearchAlgorithmJCGX(List<string> UploadDate, List<string> ArtificialType, List<string> AlgEnName, List<string> DllName, List<string> AlgCnName, List<string> SuitableSatellites, List<string> SuitableSensors, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType);

        DataSet SearchDocumentJCGX(List<string> DocumentName, List<string> DocumentType, List<string> ProgramName, List<string> DocumentReleaseTime, List<string> Author, List<string> KeyWords, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType);

        DataSet SearchToolkitJCGX(List<string> ToolkitReleaseTime, List<string> ToolkitName, List<string> ToolkitType, List<string> SuitableSatellites, List<string> SuitableSensors, List<string> OSBits, List<string> OStype, List<string> Author, List<string> KeyWords, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType);

        DataSet SearchCorrectedDataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, int QueryRange, List<string> strOrderBy, List<int> OrderByType);

        DataSet SearchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType);

        DataSet SearchGFByRegion(string regionName, string category, string type, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType);

        IGeometry getGeomFromRow(DataRow dr);

        DataSet SearchBatchGF1DataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType, string PointField, string KeyWords, string KeyValues);
        DataSet SearchData();
        DataSet SearchGF1DataForJCGXByImportTime(String importStartTime, String importEndTime);
        DataSet SearchGFFDataJCGX(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, List<int> DataSizeRange, string IsOnCloud, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType);
        
       
        
        
        
        

       
        #endregion
        #endregion
        #region 生产线接口
        DataSet SearchAlgorithmPL(string AlgEnName, string AlgCnName, string ComponentVersion, out int AllRecordsCount, int StartIndex, int ResultCount);

        DataSet SearchGFFData(string queryCondition);

        DataSet SearchCorrectedDataPL(List<string> position, List<string> datetime, List<string> satellite, List<string> sensor, List<string> PixelSpacing, out int AllRecordsCount, int StartIndex, int ResultCount);

        DataSet SearchProductWFLPL(string ProductEnName, string ProductCnName, string ProductLevel, string version, out int AllRecordsCount, int StartIndex, int ResultCount);


        #endregion

        #region 仿真组
        string GetBandLstBySensorID(string SensorID);

        string GetSensorLstBySateID(string SateID);

        string GetSateInfo();

        string GetSateAndSensorAndBandInfo();
        #endregion

        #region 算法详情信息查询
        DataSet GetParasInfoByAlgID(string AlgID, out int AllRecordsCount, int StartIndex, int ResultCount, List<string> strOrderBy, List<int> OrderByType);
        #endregion

        string CopyByQrstCodeOrderTask(string code, string sharePath, string mouid);

        string CopyByQrstCode(string code, string sharePath);

        int SubmitDOCDataImportToString(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, string DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, string UPLOADTIME, string FILESIZE, string sharePath);

        int SubmitDOCDataImport(string TITLE, string DOCTYPE, string KEYWORD, string ABSTRACT, DateTime DOCDATE, string DESCRIPTION, string AUTHOR, string UPLOADER, DateTime UPLOADTIME, int FILESIZE, string sharePath);

        DataSet GetProductInfo();

        DataSet GetIndustryInfo();

        DataSet SearchCountyByCity(string provinceName, string cityName);

        DataSet SearchCityByProvince(string provinceName);

        DataSet SearchProvince();

        List<System.Data.DataSet> SerachDataByCoordsStrceshi1(string coordsStr, string tilelevel, int startdate, int enddate, out List<int> allcount);

        System.Data.DataSet SerachDataCountByCoordsStrceshi2(string coordsStr, string tilelevel);

        System.Data.DataTable SerachDataCountByCoordsStr(string coordsStr, string tilelevel, string datalist);

    }
}
