/* 
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：基于表的查询方案,即外部查询直接基于数据库表
 * 修改时间：2013-08-06
 * 修改内容：支持将elementSet[0]标记为“*”时查询全部字段
*/
using System;
using System.Collections.Generic;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.Data;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_MetadataQuery
{
    public class TableBasedQuerySchema:IGetQuerySchema
    {
        readonly IDbBaseUtilities baseUtilities;
        string[] elementSet;                                               //查询字段码
        readonly string tableCode;                                                  //数据库表
        private string tableName;


        public TableBasedQuerySchema(string[] _elementSet,string _tableCode, IDbBaseUtilities _baseUtilities)
        {
            baseUtilities = _baseUtilities;
            elementSet = _elementSet;
            tableCode = _tableCode;
            tablecode_Dal tablecode = new tablecode_Dal(baseUtilities);
            tableName = tablecode.GetTableName(tableCode);
        }

        public Dictionary<string, string> GetFieldName()
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                DataSet ds = GetTableStruct();

                //如果第一个字母为“*”，则返回全部字段
                if (elementSet[0].Equals("*"))
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        dic.Add(ds.Tables[0].Rows[j]["FieldCode"].ToString(), ds.Tables[0].Rows[j][0].ToString());
                    }
                    return dic;
                }

                for (int i = 0; i < elementSet.Length; i++)
                {
                    if (!dic.ContainsKey(elementSet[i]))
                    {
                        FieldElement fieldElement = new FieldElement(elementSet[i]);
                        if (fieldElement.index - 1 > ds.Tables[0].Rows.Count)
                        {
                            throw new Exception(string.Format("查询的字段'{0}'越界！",elementSet[i]));
                        }
                        dic.Add(elementSet[i], ds.Tables[0].Rows[fieldElement.index - 1][0].ToString());
                    }
                }
                return dic;
            }catch(Exception ex)       //将异常抛到上层
            {
                throw ex;
            }
        }

        public string GetTableName()
        {
            return tableName;
        }
        /// <summary>
        /// 获取要查询的表结构，包括Field,Type,FieldCode
        /// </summary>
        /// <returns></returns>
        public DataSet GetTableStruct()
        {
            try
            {
                DataSet ds = baseUtilities.GetTableStruct(tableName);
                if (ds != null&&ds.Tables[0]!=null)
                {
                    DataColumn dc = new DataColumn() { ColumnName = "FieldCode", DataType = Type.GetType("System.String") };
                    //移除“null”,"key","default","Extra"列
                    //ds.Tables[0].Columns.RemoveAt(5);
                    //ds.Tables[0].Columns.RemoveAt(4);
                    //ds.Tables[0].Columns.RemoveAt(3);
                    //ds.Tables[0].Columns.RemoveAt(2);

                    ds.Tables[0].Columns.Add(dc);
                    for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
                    {
                        ds.Tables[0].Rows[i]["FieldCode"] = string.Format("{0}.{1}", tableCode, i + 1);
                    }
                }
                else
                {
                    throw new Exception(string.Format("未能找到'{0}'的表结构信息",tableName));
                }
                return ds;
            }
            catch(Exception ex)
            {
                throw new Exception("表结构信息查询失败！"+ex);
            }
        } 
    }
}
