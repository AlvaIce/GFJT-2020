using System.Collections.Generic;
using System.Web.Services;
using System.Data;
using QRST_DI_Resources;
using DotSpatial.Topology;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_SS_Basis.TileSearch;
using System;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]

public class Service : System.Web.Services.WebService
{
    IDbBaseUtilities tileUtilities;
    IQDB_Searcher_Tile searcherTile;
    IDbOperating dbOperating;

    public Service()
    {

        if (!Constant.ServiceIsConnected)
        {
            Constant.InitializeTcpConnection();
        }
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
        tileUtilities = Constant.ITileDbUtilities;
        searcherTile = Constant.ISearcherTileServ;

    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod(Description = "通用方法，未测试。 ")]
    public DataSet GetDataSetFromSQLite(string sql, string DataSourcePath)
    {
        return searcherTile.GetDataSetFromSQLite(sql, DataSourcePath);
    }

    [WebMethod(Description = "通用方法，查询数据库去重字段取值，一般用于查询之前查找字段取值可能性。或者类似功能。")]
    public List<string> GetCTileDistinctAttrs()
    {
        return searcherTile.GetCTileDistinctAttrs();
    }

    [WebMethod(Description = "通用方法，查询数据库去重字段取值，一般用于查询之前查找字段取值可能性。或者类似功能。")]
    public List<string> GetPTileDistinctAttrs()
    {
        return searcherTile.GetPTileDistinctAttrs();
    }

    [WebMethod(Description = "通用方法，查询数据库去重字段取值，一般用于查询之前查找字段取值可能性。或者类似功能。")]
    public List<string> UpdateDistinctTables()
    {
        return searcherTile.UpdateDistinctTables();
    }

    [WebMethod(Description = "通用方法，查询数据库去重字段取值，一般用于查询之前查找字段取值可能性。或者类似功能。")]
    public List<string> GetDataDistinct(string distinctSql)
    {
        return searcherTile.GetDataDistinct(distinctSql);
    }

    [WebMethod(Description = "1)可作测试用2)获取数据切片中的所有卫星类型取值")]
    public List<string> SearTileSatellites()
    {
        return searcherTile.SearTileSatellites();
    }
    [WebMethod(Description = "1)可作测试用2)获取数据切片中的所有传感器类型取值")]
    public List<string> SearTileSensors()
    {
        return searcherTile.SearTileSensors();
    }

    [WebMethod(Description = "1)可作测试用2)获取所有产品的类型")]
    public List<string> SearProdType()
    {
        return searcherTile.SearProdType();
    }

    [WebMethod(Description = "1)可作测试用2)查询数据库中已有数据切片等级的列表，可提供给客户端作为查询条件")]
    public List<string> SearTileLevels()
    {
        return searcherTile.SearTileLevels();
    }
    [WebMethod(Description = "提供给客户端切片高级查询条件")]
    public DataSet SearTileAllAttr()
    {
        return searcherTile.SearTileAllAttr();
    }
    [WebMethod(Description = "提供给客户端执行Sql语句")]
    public void ExecuteNonQuery(string sql)
    {
        ExecuteNonQuery(sql);

    }
    [WebMethod(Description = "提供给客户端高级产品切片查询条件")]
    public DataSet SearProTileAllAttr()
    {
        return searcherTile.SearProTileAllAttr();
    }


    [WebMethod(Description = "查询数据库中已有产品切片等级的列表，可提供给客户端作为查询条件")]
    public List<string> SearProTileLevels()
    {
        return searcherTile.SearProTileLevels();
    }

    [WebMethod(Description = "纠正数据（切片)。所有参数须实例化不能为null,返回记录数目（输出参数）。")]
    private DataSet SearTile(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize, out int AllRecordCount)
    {
        return searcherTile.SearTile(position, datetime, satellite, sensor, datatype, tileLevel, pageIndex, pageSize,out AllRecordCount);
    }

    [WebMethod(Description = "纠正数据（切片)。所有参数须实例化不能为null,返回记录数目（写入Dataset）。")]
    private DataSet SearTile2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize)
    {
        return searcherTile.SearTile2(position, datetime, satellite, sensor, datatype, tileLevel, pageIndex, pageSize);
    }

    [WebMethod(Description = "运管查询瓦片测试")]
    public int SearTilePagedBaseColAndRowTest()
    {
        return searcherTile.SearTilePagedBaseColAndRowTest();
    }

    [WebMethod(Description = "数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null")]
    public DataSet SearTilePagedBaseColAndRow(List<string> ColAndRow, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
    {
        return searcherTile.SearTilePagedBaseColAndRow(ColAndRow, datetime, satellite, sensor, datatype, tileLevel, OtherQuery,out allRecordCount, startIndex, offset);
    }


    [WebMethod(Description = "数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null")]
    public DataSet SearTilePaged(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
    {
        return searcherTile.SearTilePaged(position, datetime, satellite, sensor, datatype, tileLevel, out allRecordCount, startIndex, offset);
    }


    [WebMethod(Description = "数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null")]

    public DataSet SearTilePaged1(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
    {
        return searcherTile.SearTilePaged1(position, datetime, satellite, sensor, datatype, tileLevel, OtherQuery,out allRecordCount, startIndex, offset);
    }
    [WebMethod(Description = "数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null")]
    ///position:最小纬度、最小经度、最大纬度、最大经度
    public DataSet SearFliterTilePaged2(string coordsStr, string type, List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
    {
        return searcherTile.SearFliterTilePaged2(coordsStr, type, position, datetime, satellite, sensor, datatype, tileLevel, OtherQuery,out allRecordCount, startIndex, offset);
    }
    [WebMethod(Description = "数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null")]
    ///position:最小纬度、最小经度、最大纬度、最大经度
    public DataSet SearFliterTilePaged1(List<double[]> coords, string type, List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
    {
        return searcherTile.SearFliterTilePaged1(coords, type, position, datetime, satellite, sensor, datatype, tileLevel, OtherQuery,out allRecordCount, startIndex, offset);
    }

    [WebMethod(Description = "根据切片文件名，返回切片路径")]

    public List<string> TilePathsList(List<string> tileNames)
    {
        return searcherTile.TilePathsList(tileNames);
    }

    [WebMethod(Description = "为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号")]
    public DataSet SearTileYaan(string row, string col, string tileLevel)
    {
        return searcherTile.SearTileYaan(row, col, tileLevel);
    }

    [WebMethod(Description = "为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号；sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTilePathSingle(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate)
    {
        return searcherTile.SearImgTilePathSingle(sat, sensor, row, col, tileLevel, startdate, enddate);
    }

    [WebMethod(Description = "为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号；sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开,cloud,availability 分别为云量和满幅率")]
    public System.Data.DataSet SearImgTileSingleAddCloudAvail(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
    {
        return searcherTile.SearImgTileSingleAddCloudAvail(sat, sensor, row, col, tileLevel, startdate, enddate, Mincloud, Maxcloud, Minavailability, Maxavailability);
    }

    [WebMethod(Description = "为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号；sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTileSingle(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate)
    {
        return searcherTile.SearImgTileSingle(sat, sensor, row, col, tileLevel, startdate, enddate);
    }

    [WebMethod(Description = "为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号")]
    public DataSet SearProdTilePathSingle(string prodType, string row, string col, string lv, int startdate, int enddate)
    {
        return searcherTile.SearProdTilePathSingle(prodType, row, col, lv, startdate, enddate);
    }

    [WebMethod(Description = "为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号")]
    public DataSet SearProdTileSingle(string prodType, string row, string col, string lv, int startdate, int enddate)
    {
        return searcherTile.SearProdTileSingle(prodType, row, col, lv, startdate, enddate);
    }

    [WebMethod(Description = "为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号，云量，满幅率")]
    public System.Data.DataSet SearProdTileSingleAddCloudAvail(string prodType, string row, string col, string lv, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
    {
        return searcherTile.SearProdTileSingleAddCloudAvail(prodType, row, col, lv, startdate, enddate, Mincloud, Maxcloud, Minavailability, Maxavailability);
    }

    [WebMethod(Description = "为GIAS提供，查询指定瓦块位置上的数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号")]
    public DataSet TestSearTileSingleAllType(string tablename)
    {
        return searcherTile.TestSearTileSingleAllType(tablename);
    }

    [WebMethod(Description = "为GIAS提供，查询指定瓦块位置上的数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号")]
    public DataSet SearTileSingleAllType(string tablename, string row, string col, string tileLevel, string otherCondition, bool needTilePath = false)
    {
        return searcherTile.SearTileSingleAllType(tablename, row, col, tileLevel, otherCondition, needTilePath = false);
    }

    [WebMethod(Description = "为GIAS提供，查询指定瓦块位置上的数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号")]
    public System.Data.DataSet SearTileSingleAllTypeAddCloudAvai(string tablename, string row, string col, string tileLevel, string otherCondition, string sqlcloudAndAvailability, bool needTilePath = false)
    {
        return searcherTile.SearTileSingleAllTypeAddCloudAvai(tablename, row, col, tileLevel, otherCondition, sqlcloudAndAvailability, needTilePath);
    }

    [WebMethod(Description = "通过批量的瓦片的层级行列号信息查询到这些行列号信息的瓦片元数据信息")]
    public DataSet searTileByColAndRowBatch(List<List<string>> tileInfos)
    {
        return searcherTile.searTileByColAndRowBatch(tileInfos);
    }

    [WebMethod(Description = "单次全覆盖检索，为雅安提供，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearTileYaanBatch(List<string> row, List<string> col, string tileLevel)
    {
        return searcherTile.SearTileYaanBatch(row, col, tileLevel);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearTileBatch(List<string> row, List<string> col, string tileLevel)
    {
        return searcherTile.SearTileBatch(row, col, tileLevel);
    }
    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTileBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string priority)
    {
        return searcherTile.SearImgTileBatch_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate, priority);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTilePathBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string priority, bool needTilePath = true)
    {
        return searcherTile.SearImgTilePathBatch_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate, priority,needTilePath = true);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。cloud,availability 分别为云量和满幅率，priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
    public System.Data.DataSet SearImgTileBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority)
    {
        return searcherTile.SearImgTileBatchAddCloudAvail_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate, Mincloud, Maxcloud, Minavailability, Maxavailability, priority);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
    public System.Data.DataSet SearImgTilePathBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority, bool needTilePath = true)
    {
        return searcherTile.SearImgTilePathBatchAddCloudAvail_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate, Mincloud, Maxcloud, Minavailability, Maxavailability, priority, needTilePath);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTileBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate)
    {
        return searcherTile.SearImgTileBatch(sat, sensor, row, col, tileLevel, startdate, enddate);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTilePathBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, bool needTilePath = true)
    {
        return searcherTile.SearImgTilePathBatch(sat, sensor, row, col, tileLevel, startdate, enddate, needTilePath = true);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
    public System.Data.DataSet SearProdTileBatchAddPriority(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, string priority)
    {
        return searcherTile.SearProdTileBatchAddPriority(prodType, rows, cols, lv, startdate, enddate, priority);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
    public System.Data.DataSet SearImgTileBatchAddPriority(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, string priority)
    {
        return searcherTile.SearImgTileBatchAddPriority(sat, sensor, row, col, tileLevel, startdate, enddate, priority);
    }


    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
    public System.Data.DataSet SearImgTilePathBatchAddPriority(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, string priority, bool needTilePath = true)
    {
        return searcherTile.SearImgTilePathBatchAddPriority(sat, sensor, row, col, tileLevel, startdate, enddate, priority, needTilePath);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearProdTileBatch_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate, string priority)

    {
        return searcherTile.SearProdTileBatch_coordsStr(prodType, coordsStr, tileLevel, startdate, enddate, priority);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearProdTilePathBatch_coordsStr(string prodType, string coordsStr, string tileLevel,
            int startdate, int enddate, string priority, bool needTilePath = true)
    {
        return searcherTile.SearProdTilePathBatch_coordsStr(prodType, coordsStr, tileLevel, startdate, enddate, priority,needTilePath = true);
    }
    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearProdTileBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate)
    {
        return searcherTile.SearProdTileBatch(prodType, rows, cols, lv, startdate, enddate);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearProdTilePathBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, bool needTilePath = true)
    {
        return searcherTile.SearProdTilePathBatch(prodType, rows, cols, lv, startdate, enddate, needTilePath = true);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。priority 为检索优先级，支持为空，例如：满幅率-云量-时间；云量-时间-满幅率；时间-满幅率-云量 等")]
    public System.Data.DataSet SearProdTilePathBatchAddPriority(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, string priority, bool needTilePath = true)
    {
        return searcherTile.SearProdTilePathBatchAddPriority(prodType, rows, cols, lv, startdate, enddate, priority, needTilePath);
    }


    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearTileBatchAllType(string tablename, List<string> row, List<string> col, string tileLevel, string sat, string sensor, string othercondition, bool needTilePath = false)
    {
        return searcherTile.SearTileBatchAllType(tablename, row, col, tileLevel, sat, sensor, othercondition, needTilePath = false);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。priority 为检索优先级,用英文缩词表示，支持为空，例如：ACT:满幅率-云量-时间；CTA:云量-时间-满幅率；TAC:时间-满幅率-云量 等")]
    public System.Data.DataSet SearTileBatchAllTypeAddPriority(string tablename, List<string> row, List<string> col, string tileLevel, string sat, string sensor, string othercondition, string priority, bool needTilePath = false)
    {
        return searcherTile.SearTileBatchAllTypeAddPriority(tablename, row, col, tileLevel, sat, sensor, othercondition, priority, needTilePath);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearTileBatchAllType_coordsStr(string tablename, string coordsStr, string tileLevel,
            string sat, string sensor, string othercondition, string priority, bool needTilePath = false)
    {
         return  SearTileBatchAllType_coordsStr( tablename,  coordsStr,  tileLevel,  sat,  sensor,  othercondition, priority, needTilePath = false);
    }

    [WebMethod(Description = "单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开。priority 为检索优先级,用英文缩词表示，支持为空，例如：ACT:满幅率-云量-时间；CTA:云量-时间-满幅率；TAC:时间-满幅率-云量 等")]
    public System.Data.DataSet SearTileBatchAllTypeAddCloudAvail_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority, bool needTilePath = false)
    {
        return searcherTile.SearTileBatchAllTypeAddCloudAvail_coordsStr(tablename, coordsStr, tileLevel, sat, sensor, othercondition, Mincloud, Maxcloud, Minavailability, Maxavailability, priority, needTilePath);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTileCountBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate)
    {
        return searcherTile.SearImgTileCountBatch_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate);
        
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开,cloud,availability 分别为云量和满幅率，不能为空")]
    public System.Data.DataSet SearImgTileCountBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
    {
        return searcherTile.SearImgTileCountBatchAddCloudAvail_coordsStr(sat, sensor, tileLevel, coordsStr, startdate, enddate,Mincloud, Maxcloud, Minavailability, Maxavailability);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public System.Data.DataSet SearTileCountBatchAllTypeAddCloudAvai_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition, string sqlcloudAndAvailability)
    {
        return searcherTile.SearTileCountBatchAllTypeAddCloudAvai_coordsStr(tablename, coordsStr, tileLevel, sat, sensor, othercondition, sqlcloudAndAvailability);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearImgTileCountBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate)
    {
         return searcherTile.SearImgTileCountBatch(sat, sensor, row, col, tileLevel, startdate, enddate);
    }


    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearProdTileCountBatch_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate)
    {
        return searcherTile.SearProdTileCountBatch_coordsStr(prodType, coordsStr, tileLevel, startdate, enddate);
    }

    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应,cloud,availability 分别为云量和满幅率。")]
    public System.Data.DataSet SearProdTileCountBatchAddCloudAvail_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability)
    {
        return searcherTile.SearProdTileCountBatchAddCloudAvail_coordsStr(prodType, coordsStr, tileLevel, startdate, enddate, Mincloud, Maxcloud, Minavailability, Maxavailability);
    }

    [WebMethod(Description = "批量产品切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。")]
    public DataSet SearProdTileCountBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate)
    {
        return searcherTile.SearProdTileCountBatch( prodType, rows, cols,  lv,  startdate,  enddate);
    }


    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearTileCountBatchAllType(string tablename, List<string> row, List<string> col, string tileLevel, string sat, string sensor, string othercondition)
    {

         return searcherTile.SearTileCountBatchAllType( tablename,  row, col,  tileLevel,  sat,  sensor,  othercondition);
    }


    [WebMethod(Description = "批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开")]
    public DataSet SearTileCountBatchAllType_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition)
    {
        return searcherTile.SearTileCountBatchAllType_coordsStr( tablename,  coordsStr,  tileLevel,  sat,  sensor,  othercondition);
         
    }
     
    [WebMethod(Description = "数据切片,返回结果。测试接口")]
    public List<string> SearTileGlobeTest()
    {

        return searcherTile.SearTileGlobeTest();
    }

    [WebMethod(Description = "数据切片,返回结果。所有参数须实例化不能为null")]
    public List<string> SearTileGlobe(List<string> position, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
    {
         return searcherTile.SearTileGlobe(  position,   satellite,  sensor,  datatype,  tileLevel);
    }


    [WebMethod(Description = "数据切片,返回结果。为仿真组提供,参数RowClos示例{'54,237,322','55,227,312'}，表示第54行号，列号开始位置237，列号结束位置322")]
    public string SearTileForSimulation(List<string> RowCols, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
    {
        return searcherTile.SearTileForSimulation(RowCols, datetime, satellite, sensor, datatype,  tileLevel);
    }

    [WebMethod(Description = "数据切片,返回结果。为仿真组提供。数组为二维三列，第一列为要查询的行号，第二列为要查询的列号开始位置，第三列为要查询的列号结束位置")]
    public string SearTileForSimulation1(string[][] RowCol, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
    {
         return searcherTile.SearTileForSimulation1(  RowCol,   datetime,  satellite,   sensor,   datatype,  tileLevel);
    }
    [WebMethod(Description = "数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null")]
    public DataSet SearTilePaged2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int startIndex, int offset)
    {
        return searcherTile.SearTilePaged2(position, datetime,  satellite,  sensor,  datatype,  tileLevel,  startIndex,  offset);
    }

    [WebMethod(Description = "数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null")]
    public string SearTilePaged3(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int startIndex, int offset)
    {
         return searcherTile.SearTilePaged3(position, datetime,  satellite,  sensor, datatype,  tileLevel,  startIndex,  offset);
    }

    [WebMethod(Description = "产品（切片）。测试")]
    public DataSet SearPRODTileTest()
    {
        return searcherTile.SearPRODTileTest();
    }

    [WebMethod(Description = "产品（切片）。所有参数（除tileLevel 可为null）须实例化不能为null,返回记录数目（输出参数）。")]
    private DataSet SearPRODTile(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize, out int AllRecordCount)
    {
        return searcherTile.SearPRODTile(  position,  datetime,  datatype,   tileLevel,   pageIndex,   pageSize,out  AllRecordCount);
    }


    [WebMethod(Description = "产品（切片）。所有参数（除tileLevel 可为null）须实例化不能为null,返回记录数目（写入Dataset）。")]
    private DataSet SearPRODTile2(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize)
    {
        return searcherTile.SearPRODTile2( position, datetime, datatype, tileLevel,  pageIndex,  pageSize);
    }

    [WebMethod(Description = "HJ产品（切片）,实现了分页检索（QRST内部）。所有参数须实例化不能为null")]
    public DataSet SearPRODTilePaged(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
    {
         return  searcherTile.SearPRODTilePaged(position, datetime,datatype,  tileLevel, out allRecordCount,  startIndex, offset);
    }

    [WebMethod(Description = "查询所有不同区域的切片种类，并统计个数，用于在球上绘制数据图形")]
    public DataSet SearSpaceDistinctTiles(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel)
    {
         return searcherTile.SearSpaceDistinctTiles(position, datetime, satellite,sensor,  datatype,  tileLevel);
    }

    [WebMethod(Description = "为可视化组提供，数据切片,返回查询到的瓦片jpg的路径。所有参数须实例化不能为null。参数依次为：卫星，传感器，时间，行号，列号，层级号")]
    public String SearTileForKSH(string satellite, string sensor, string datetime, string row, string col, string tileLevel)
    {
        return searcherTile.SearTileForKSH(satellite, sensor, datetime, row, col, tileLevel);
    }


     [WebMethod(Description = "为可视化组提供，数据切片,查询到的DataSet。所有参数须实例化不能为null。参数依次为：层级，最小行号，最大行号，最小列号，最大列号")]
     public DataSet SearTileForKSHByRowAndCol(string tileLevel,string minRow,string maxRow,string minCol,string maxCol)
     {
         return searcherTile.SearTileForKSHByRowAndCol(tileLevel, minRow, maxRow, minCol, maxCol);
     }
     [WebMethod(Description = "数据切片,返回结果。所有参数须实例化不能为null")]
     public DataSet SearTileByRegion(string regionName, string category,string type, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset)
     {
         return searcherTile.SearTileByRegion( regionName,  category, type, datetime,  satellite, sensor,  datatype, tileLevel,OtherQuery,out  allRecordCount, startIndex,offset);
     }


    [WebMethod(Description = "查询切片缩略图，为JCGX提供。若不存在，则返回-1")]
    public string SearchTileJPG(string dateTime, string tileLevel, string row, string col)
    {
       return  searcherTile.SearchTileJPG( dateTime,  tileLevel,  row,  col);
    }
    [WebMethod(Description = "查询切片缩略图，为JCGX提供。若不存在，则返回-1")]
    public List<string> SearchTileJPGForGloble(string dateTime, string tileLevel, string row, string col)
    {
        return searcherTile.SearchTileJPGForGloble(dateTime,  tileLevel,  row,  col);
    }

    [WebMethod(Description = "查询产品数据，为JCGX提供。若datatype为空，则只查询缩略图")]
    public DataSet SearPRODTileForJCGX(List<string> position, List<int> datetime, List<string> ProdType, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
    {
        return searcherTile.SearPRODTileForJCGX(position, datetime,ProdType,tileLevel,out allRecordCount,startIndex, offset);
    }

    [WebMethod(Description = "查询切片，为JCGX提供。若datatype为空，则只查询缩略图")]
    public DataSet SearTileForJCGX(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset)
    {
        return searcherTile.SearTileForJCGX(position, datetime, satellite, sensor, datatype, tileLevel,out allRecordCount, startIndex,  offset);
    }

    [WebMethod(Description = "查询切片，为JCGX提供。若datatype为空，则只查询缩略图")]
    public int SearTileForJCGXtest()
    {
        return searcherTile.SearTileForJCGXtest();

    }
}
