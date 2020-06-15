/*
 * 作者：zxw
 * 创建时间：2013-08-02
 * 描述：基于视图以及视图字段的查询方案,即外部查询基于表视图，并且字段编码即为视图字段名称
 * 修改时间：2013-08-06
 * 修改内容：支持将elementSet[0]标记为“*”时查询全部字段
*/ 
using System;
using System.Collections.Generic;
using System.Data;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_MetadataQuery
{
    public class FieldViewBasedQuerySchema:ViewBasedQuerySchema
    {
        public FieldViewBasedQuerySchema(string[] _elementSet, string _tableCode, IDbBaseUtilities _baseUtilities)
            : base(_elementSet, _tableCode, _baseUtilities)
        {
            
        }
        public override Dictionary<string, string> GetFieldName()
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
                        dic.Add(ds.Tables[0].Rows[j][0].ToString(), ds.Tables[0].Rows[j][0].ToString());
                    }
                    return dic;
                }

                for (int i = 0; i < elementSet.Length; i++)
                {
                    DataTable dt = ds.Tables[0];
                    if (!dic.ContainsKey(elementSet[i]))
                    {
                        int j = 0;
                        for (; j < dt.Rows.Count;j++ )
                        {
                            if (elementSet[i] == dt.Rows[j][0].ToString())
                            {
                                dic.Add(elementSet[i], elementSet[i]);
                                 break;
                            }    
                        }
                        if (j == dt.Rows.Count)   //没能在视图字段信息中找到字段elementSet[i]
                        {
                            throw new Exception(string.Format("没能找到字段'{0}'", elementSet[i]));
                        }
                    }
                }
                return dic;
            }
            catch (Exception ex)       //将异常抛到上层
            {
                throw ex;
            }
        }
    }
}
