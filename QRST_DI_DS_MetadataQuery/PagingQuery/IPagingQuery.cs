/*
 * 作者：zxw
 * 创建时间：2013-09-01
 * 描述：支持分页查询接口 
*/
using System.Data;
 
namespace QRST_DI_DS_MetadataQuery.PagingQuery
{
    public interface IPagingQuery
    {
        /// <summary>
        /// 获取查询总记录数
        /// </summary>
        /// <returns></returns>
        int GetTotalRecordNum();

        /// <summary>
        /// 获取指定数据偏移量的数据
        /// </summary>
        /// <param name="startIndex">查询数据的起始索引</param>
        /// <param name="length">查询记录数，若不足</param>
        /// <returns></returns>
        DataTable GetCurrentPageData(int startIndex,int length);
    }
}
