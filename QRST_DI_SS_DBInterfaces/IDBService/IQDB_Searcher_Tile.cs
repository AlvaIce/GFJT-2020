

using DotSpatial.Topology;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_SS_Basis.TileSearch;
using System;
/**描述：瓦片数据库操作接口。
* 作者：jianghua
* 日期：20170411
*/
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace QRST_DI_SS_DBInterfaces.IDBService
{
    public interface IQDB_Searcher_Tile
    {

        /// <summary>
        ///  SQLite查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="DataSourcePath"></param>
        /// <returns></returns>
        DataSet GetDataSetFromSQLite(string sql, string DataSourcePath);

        /// <summary>
        /// 通用方法，查询数据库去重字段取值，一般用于查询之前查找字段取值可能性。或者类似功能。
        /// </summary>
        /// <param name="distinctSql"></param>
        /// <returns></returns>
        List<string> GetDataDistinct(string distinctSql);

        /// <summary>
        /// 获取数据切片中的所有卫星类型取值
        /// </summary>
        /// <returns></returns>
        List<string> SearTileSatellites();

        /// <summary>
        /// 获取数据切片中的所有传感器类型取值
        /// </summary>
        /// <returns></returns>
        List<string> SearTileSensors();

        ///// <summary>
        ///// 获取所有产品的类型
        ///// </summary>
        ///// <returns></returns>
        List<string> SearProdType();

        /// <summary>
        /// 查询数据库中已有数据切片等级的列表，可提供给客户端作为查询条件
        /// </summary>
        /// <returns></returns>
        List<string> SearTileLevels();

        /// <summary>
        /// 提供给客户端切片高级查询条件
        /// </summary>
        /// <returns></returns>
        DataSet SearTileAllAttr();

        ///// <summary>
        ///// 提供给客户端多节点执行Sql语句
        ///// </summary>
        ///// <param name="sql"></param>
        //void ExecuteNonQuery(string sql);

        /// <summary>
        /// 提供给客户端高级产品切片查询条件
        /// </summary>
        /// <returns></returns>
        DataSet SearProTileAllAttr();

        /// <summary>
        /// 查询数据库中已有产品切片等级的列表，可提供给客户端作为查询条件
        /// </summary>
        /// <returns></returns>
        List<string> SearProTileLevels();

        ///// <summary>
        ///// 纠正数据（切片)。所有参数须实例化不能为null,返回记录数目（输出参数）。
        ///// </summary>
        ///// <param name="position"></param>
        ///// <param name="datetime"></param>
        ///// <param name="satellite"></param>
        ///// <param name="sensor"></param>
        ///// <param name="datatype"></param>
        ///// <param name="tileLevel"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="AllRecordCount"></param>
        ///// <returns></returns>
        //DataSet SearTile(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
        //    List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize, out int AllRecordCount);

        ///// <summary>
        ///// 纠正数据（切片)。所有参数须实例化不能为null,返回记录数目（写入Dataset）。
        ///// </summary>
        ///// <param name="position"></param>
        ///// <param name="datetime"></param>
        ///// <param name="satellite"></param>
        ///// <param name="sensor"></param>
        ///// <param name="datatype"></param>
        ///// <param name="tileLevel"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <returns></returns>
        //DataSet SearTile2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
        //    List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize);

        /// <summary>
        /// 数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null
        /// </summary>
        /// <param name="ColAndRow"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="OtherQuery"></param>
        /// <param name="allRecordCount"></param>
        /// <param name="startIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataSet SearTilePagedBaseColAndRow(List<string> ColAndRow, List<int> datetime, List<string> satellite,
            List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery,
            out int allRecordCount, int startIndex, int offset);

        ///// <summary>
        ///// 数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null
        ///// </summary>
        ///// <param name="position"></param>
        ///// <param name="datetime"></param>
        ///// <param name="satellite"></param>
        ///// <param name="sensor"></param>
        ///// <param name="datatype"></param>
        ///// <param name="tileLevel"></param>
        ///// <param name="allRecordCount"></param>
        ///// <param name="startIndex"></param>
        ///// <param name="offset"></param>
        ///// <returns></returns>
        //DataSet SearTilePaged(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
        //    List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset);

        /// <summary>
        /// 数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="OtherQuery"></param>
        /// <param name="allRecordCount"></param>
        /// <param name="startIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataSet SearTilePaged1(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
            List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex,
            int offset);

        /// <summary>
        /// 为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。
        /// 参数依次为：行号，列号，层级号
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <returns></returns>
        DataSet SearTileYaan(string row, string col, string tileLevel);

        /// <summary>
        /// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <returns></returns>
        DataSet SearTileYaanBatch(List<string> row, List<string> col, string tileLevel);
            ///// <summary>
            ///// 数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null
            ///// </summary>
            ///// <param name="coordsStr"></param>
            ///// <param name="type"></param>
            ///// <param name="position"></param>
            ///// <param name="datetime"></param>
            ///// <param name="satellite"></param>
            ///// <param name="sensor"></param>
            ///// <param name="datatype"></param>
            ///// <param name="tileLevel"></param>
            ///// <param name="OtherQuery"></param>
            ///// <param name="allRecordCount"></param>
            ///// <param name="startIndex"></param>
            ///// <param name="offset"></param>
            ///// <returns></returns>
            //DataSet SearFliterTilePaged2(string coordsStr, string type, List<string> position, List<int> datetime,
            //    List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel,
            //    string OtherQuery, out int allRecordCount, int startIndex, int offset);

        /// <summary>
        /// 数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="OtherQuery"></param>
        /// <param name="allRecordCount"></param>
        /// <param name="startIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataSet SearFliterTilePaged1(List<double[]> coords, string type, List<string> position, List<int> datetime,
            List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel,
            string OtherQuery, out int allRecordCount, int startIndex, int offset);

        ///// <summary>
        ///// 根据切片文件名，返回切片路径
        ///// </summary>
        ///// <param name="tileNames"></param>
        ///// <returns></returns>
        //List<string> TilePathsList(List<string> tileNames);

        ///// <summary>
        ///// 通过批量的瓦片的层级行列号信息查询到这些行列号信息的瓦片元数据信息
        ///// </summary>
        ///// <param name="tileInfos"></param>
        ///// <returns></returns>
        //DataSet searTileByColAndRowBatch(List<List<string>> tileInfos);

        ///// <summary>
        ///// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        ///// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        ///// </summary>
        ///// <param name="row"></param>
        ///// <param name="col"></param>
        ///// <param name="tileLevel"></param>
        ///// <returns></returns>
        //DataSet SearTileBatch(List<string> row, List<string> col, string tileLevel);

        ///// <summary>
        ///// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        ///// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        ///// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        ///// </summary>
        ///// <param name="sat"></param>
        ///// <param name="sensor"></param>
        ///// <param name="tileLevel"></param>
        ///// <param name="coordsStr"></param>
        ///// <param name="startdate"></param>
        ///// <param name="enddate"></param>
        ///// <param name="needTilePath"></param>
        ///// <returns></returns>
        //DataSet SearImgTilePathBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr,
        //    int startdate, int enddate, bool needTilePath = true);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。参数说明：行号，
        /// 列号分别为一维数组，并且行列号一一对应。
        /// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="tileLevel"></param>
        /// <param name="coordsStr"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        DataSet SearImgTileCountBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr,
            int startdate, int enddate);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="tileLevel"></param>
        /// <param name="coordsStr"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        DataSet SearImgTileBatch_coordsStr(string sat, string sensor, string tileLevel,
            string coordsStr, int startdate, int enddate, string priority);

        ///// <summary>
        ///// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        ///// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        ///// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        ///// </summary>
        ///// <param name="sat"></param>
        ///// <param name="sensor"></param>
        ///// <param name="row"></param>
        ///// <param name="col"></param>
        ///// <param name="tileLevel"></param>
        ///// <param name="startdate"></param>
        ///// <param name="enddate"></param>
        ///// <returns></returns>
        //DataSet SearImgTileBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel,
        //    int startdate, int enddate);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// </summary>
        /// <param name="prodType"></param>
        /// <param name="coordsStr"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        DataSet SearProdTileCountBatch_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate,
            int enddate);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// </summary>
        /// <param name="prodType"></param>
        /// <param name="coordsStr"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        DataSet SearProdTileBatch_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate, string priority);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。
        /// 所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// </summary>
        /// <param name="prodType"></param>
        /// <param name="coordsStr"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        DataSet SearProdTilePathBatch_coordsStr(string prodType, string coordsStr, string tileLevel, 
            int startdate, int enddate, string priority, bool needTilePath = true);

        /// <summary>
        /// HJ产品（切片）,实现了分页检索（QRST内部）。所有参数须实例化不能为null
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="allRecordCount"></param>
        /// <param name="startIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataSet SearPRODTilePaged(List<string> position, List<int> datetime, List<string> datatype,
            List<string> tileLevel, out int allRecordCount, int startIndex, int offset);

        /// <summary>
        /// 为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。
        /// 参数依次为：行号，列号，层级号；sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        DataSet SearImgTileSingle(string sat, string sensor, string row, string col, string tileLevel, int startdate,
            int enddate);

        /// <summary>
        /// 为雅安提供，数据切片,返回结果。所有参数须实例化不能为null。参数依次为：行号，列号，层级号
        /// </summary>
        /// <param name="prodType"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="lv"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        DataSet SearProdTileSingle(string prodType, string row, string col, string lv, int startdate, int enddate);
        ///// <summary>
        ///// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。
        ///// 所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        ///// </summary>
        ///// <param name="prodType"></param>
        ///// <param name="rows"></param>
        ///// <param name="cols"></param>
        ///// <param name="lv"></param>
        ///// <param name="startdate"></param>
        ///// <param name="enddate"></param>
        ///// <returns></returns>
        //DataSet SearProdTileBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate,
        //    int enddate);

        ///// <summary>
        ///// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。
        ///// 所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        ///// </summary>
        ///// <param name="prodType"></param>
        ///// <param name="rows"></param>
        ///// <param name="cols"></param>
        ///// <param name="lv"></param>
        ///// <param name="startdate"></param>
        ///// <param name="enddate"></param>
        ///// <param name="needTilePath"></param>
        ///// <returns></returns>
        //DataSet SearProdTilePathBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate,
        //    int enddate, bool needTilePath = true);

        /// <summary>
        /// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。
        /// 所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="othercondition"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        DataSet SearTileBatchAllType(string tablename, List<string> row, List<string> col, string tileLevel, string sat,
            string sensor, string othercondition, bool needTilePath = false);

        /// <summary>
        /// 单次全覆盖检索，批量数据切片,相同行列号、层级，只返回时间最近的结果。
        /// 所有参数须实例化不能为null。参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="coordsStr"></param>
        /// <param name="tileLevel"></param>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="othercondition"></param>
        /// <param name="needTilePath"></param>
        /// <returns></returns>
        DataSet SearTileBatchAllType_coordsStr(string tablename, string coordsStr, string tileLevel, 
            string sat, string sensor, string othercondition, string priority, bool needTilePath = false);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="sensor"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        DataSet SearImgTileCountBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel,
            int startdate, int enddate);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        DataSet SearProdTileCountBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate,
            int enddate);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        DataSet SearTileCountBatchAllType(string tablename, List<string> row, List<string> col, string tileLevel,
            string sat, string sensor, string othercondition);

        /// <summary>
        /// 批量数据切片DEM,相同行列号、层级，只返回时间最近的结果。所有参数须实例化不能为null。
        /// 参数说明：行号，列号分别为一维数组，并且行列号一一对应。
        /// sat, sensor支持同时查多个卫星传感器，全部为空，多个用,隔开
        /// </summary>
        DataSet SearTileCountBatchAllType_coordsStr(string tablename, string coordsStr, string tileLevel, string sat,
            string sensor, string othercondition);

        /// <summary>
        /// 数据切片,返回结果。所有参数须实例化不能为null
        /// </summary>
        /// <param name="position"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <returns></returns>
        List<string> SearTileGlobe(List<string> position, List<string> satellite, List<string> sensor,
            List<string> datatype, List<string> tileLevel);

        /// <summary>
        /// 数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataSet SearTilePaged2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
            List<string> datatype, List<string> tileLevel, int startIndex, int offset);

        /// <summary>
        /// 数据切片,返回结果分页，可指定页大小。所有参数须实例化不能为null
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="startIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        string SearTilePaged3(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
            List<string> datatype, List<string> tileLevel, int startIndex, int offset);

        /// <summary>
        /// 产品（切片）。所有参数（除tileLevel 可为null）须实例化不能为null,返回记录数目（输出参数）。
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="AllRecordCount"></param>
        /// <returns></returns>
        DataSet SearPRODTile(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel,
            int pageIndex, int pageSize, out int AllRecordCount);

        /// <summary>
        /// 产品（切片）。所有参数（除tileLevel 可为null）须实例化不能为null,返回记录数目（输出参数）。
        /// </summary>
        DataSet SearPRODTile2(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel,
            int pageIndex, int pageSize);

        /// <summary>
        /// 查询所有不同区域的切片种类，并统计个数，用于在球上绘制数据图形
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <returns></returns>
        DataSet SearSpaceDistinctTiles(List<string> position, List<int> datetime, List<string> satellite,
            List<string> sensor, List<string> datatype, List<string> tileLevel);

        /// <summary>
        /// 数据切片,返回结果。所有参数须实例化不能为null
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="category"></param>
        /// <param name="type"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <param name="tileLevel"></param>
        /// <param name="OtherQuery"></param>
        /// <param name="allRecordCount"></param>
        /// <param name="startIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        DataSet SearTileByRegion(string regionName, string category, string type, List<int> datetime,
            List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel,
            string OtherQuery, out int allRecordCount, int startIndex, int offset);

        List<string> SearTileGlobeTest();
        string SearTileForSimulation(List<string> RowCols, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel);
        String SearTileForSimulation1(string[][] RowCol, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel);
        DataSet SearPRODTileTest();
        //DataSet SearPRODTile1(List<string> position, List<int> datetime, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize, string OtherQuery, out int AllRecordCount);

        String SearTileForKSH(string satellite, string sensor, string datetime, string row, string col, string tileLevel);
        DataSet SearTileForKSHByRowAndCol(string tileLevel, string minRow, string maxRow, string minCol, string maxCol);


        string SearchTileJPG(string dateTime, string tileLevel, string row, string col);
        List<string> SearchTileJPGForGloble(string dateTime, string tileLevel, string row, string col);
        DataSet SearPRODTileForJCGX(List<string> position, List<int> datetime, List<string> ProdType, List<string> tileLevel, out int allRecordCount, int startIndex, int offset);
        DataSet SearTileForJCGX(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset);
        int SearTileForJCGXtest();
        DataSet SearProdTilePathBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, bool needTilePath = true);
        DataSet SearProdTileBatch(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate);
        DataSet SearImgTilePathBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, bool needTilePath = true);
        DataSet SearImgTileBatch(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate);
        DataSet SearImgTilePathBatch_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string priority, bool needTilePath = true);
        DataSet SearTileBatch(List<string> row, List<string> col, string tileLevel);
        DataSet searTileByColAndRowBatch(List<List<string>> tileInfos);
        DataSet SearTileSingleAllType(string tablename, string row, string col, string tileLevel, string otherCondition, bool needTilePath = false);
        DataSet TestSearTileSingleAllType(string tablename);
        DataSet SearProdTilePathSingle(string prodType, string row, string col, string lv, int startdate, int enddate);
        DataSet SearImgTilePathSingle(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate);
        List<string> TilePathsList(List<string> tileNames);
        DataSet SearFliterTilePaged2(string coordsStr, string type, List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, string OtherQuery, out int allRecordCount, int startIndex, int offset);
        DataSet SearTilePaged(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, out int allRecordCount, int startIndex, int offset);
        int SearTilePagedBaseColAndRowTest();
        DataSet SearTile(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize, out int AllRecordCount);

        void ExecuteNonQuery(string sql);
    
        void getLvRowColFromCoordsStr(string coordsStr, string tileLevel, out List<int> rows, out List<int> cols);

        DataSet SearTile2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype, List<string> tileLevel, int pageIndex, int pageSize);

        List<string> GetCTileDistinctAttrs();

        List<string> GetPTileDistinctAttrs();

        List<string> UpdateDistinctTables();

        System.Data.DataSet SearImgTileSingleAddCloudAvail(string sat, string sensor, string row, string col, string tileLevel, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability);

        DataSet SearTileSingleAllTypeAddCloudAvai(string tablename, string row, string col, string tileLevel, string otherCondition, string sqlcloudAndAvailability, bool needTilePath = false);

        DataSet SearProdTileSingleAddCloudAvail(string prodType, string row, string col, string lv, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability);

        DataSet SearImgTileBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority);

        DataSet SearImgTilePathBatchAddCloudAvail_coordsStr(string sat, string sensor, 
            string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority, bool needTilePath = true);

        DataSet SearImgTileBatchAddPriority(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, string priority);

        DataSet SearImgTilePathBatchAddPriority(string sat, string sensor, List<string> row, List<string> col, string tileLevel, int startdate, int enddate, string priority, bool needTilePath = true);

        DataSet SearProdTileBatchAddPriority(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, string priority);

        DataSet SearProdTilePathBatchAddPriority(string prodType, List<string> rows, List<string> cols, string lv, int startdate, int enddate, string priority, bool needTilePath = true);

        DataSet SearTileBatchAllTypeAddPriority(string tablename, List<string> row, List<string> col, string tileLevel, string sat, string sensor, string othercondition, string priority, bool needTilePath = false);

        DataSet SearTileBatchAllTypeAddCloudAvail_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability, string priority, bool needTilePath = false);

        DataSet SearTileCountBatchAllTypeAddCloudAvai_coordsStr(string tablename, string coordsStr, string tileLevel, string sat, string sensor, string othercondition, string sqlcloudAndAvailability);

        DataSet SearImgTileCountBatchAddCloudAvail_coordsStr(string sat, string sensor, string tileLevel, string coordsStr, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability);

        DataSet SearProdTileCountBatchAddCloudAvail_coordsStr(string prodType, string coordsStr, string tileLevel, int startdate, int enddate, string Mincloud, string Maxcloud, string Minavailability, string Maxavailability);

        }

}
