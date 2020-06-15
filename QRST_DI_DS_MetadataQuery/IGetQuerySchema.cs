/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：查询方案接口，暂时有基于视图的查询和基于表的查询
*/ 
using System.Collections.Generic;
using System.Data;

namespace QRST_DI_DS_MetadataQuery
{
    public interface IGetQuerySchema
    {
        /// <summary>
        /// 获取字段编码与字段名的键/值对应关系
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetFieldName();  
        
        /// <summary>
        /// 获取查询的表或视图的名字
        /// </summary>
        /// <returns></returns>
         string GetTableName();
         /// <summary>
         /// 获取查询的表或视图的字段信息
         /// </summary>
         /// <returns></returns>
         DataSet GetTableStruct();
    }
}
