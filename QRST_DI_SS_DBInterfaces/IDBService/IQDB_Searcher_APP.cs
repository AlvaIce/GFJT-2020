using System.Collections.Generic;
using System.Data;
using System.Xml;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;

namespace QRST_DI_SS_DBInterfaces.IDBService
{
   public interface IQDB_Searcher_APP
    {

        ///// <summary>
        ///// 纠正数据（切片)。所有参数须实例化不能为null
        ///// </summary>
        ///// <param name="position"></param>
        ///// <param name="datetime"></param>
        ///// <param name="satellite"></param>
        ///// <param name="sensor"></param>
        ///// <param name="datatype"></param>
        ///// <param name="tileLevel"></param>
        ///// <param name="pageIndex"></param>
        ///// <returns></returns>
        //DataSet SearTile(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
        //    List<string> datatype, List<int> tileLevel, int pageIndex);

        ///// <summary>
        ///// 纠正数据（切片)。所有参数须实例化不能为null
        ///// </summary>
        ///// <param name="position"></param>
        ///// <param name="datetime"></param>
        ///// <param name="satellite"></param>
        ///// <param name="sensor"></param>
        ///// <param name="datatype"></param>
        ///// <param name="tileLevel"></param>
        ///// <param name="startIndex"></param>
        ///// <param name="offset"></param>
        ///// <returns></returns>
        //DataSet SearTilePaged2(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor,
        //    List<string> datatype, List<string> tileLevel, int startIndex, int offset);

        //#region SQLite
        ///// <summary>
        ///// 以一个XElement对象返回的数据库中数据类型目录
        ///// </summary>
        ///// <returns></returns>
        //XmlElement GetTreeCatalogByXML();

        ///// <summary>
        ///// 根据表编码获取要查询表或视图的查询字段结构
        ///// </summary>
        ///// <param name="tableCode"></param>
        ///// <returns></returns>
        //DataSet GetDataTableStruct(string tableCode);

        ///// <summary>
        ///// 查询综合数据库元数据表！
        ///// </summary>
        ///// <param name="_request"></param>
        ///// <returns></returns>
        //QueryResponse GetMetadata(QueryRequest _request);


        //#endregion
    }
}
