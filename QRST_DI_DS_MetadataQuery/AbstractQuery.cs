/*
 * 作者：zxw
 * 创建时间：2013-07-31
 * 描述：查询抽象类，统一实现将QueryRequest的sql条件对象转换为sql语句
*/ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_MetadataQuery
{
    public class AbstractQuery
    {
        public IGetQuerySchema querySchema;
        public QueryRequest queryRequest;
        public IDbBaseUtilities baseUtilities;
        const string protocolVersion = "v1.0";

        public string GetQuerySql()
        {
            if (queryRequest != null)
            {
                try
                {
                    Check();
                    StringBuilder sb = new StringBuilder("select ");
                    string tableName = querySchema.GetTableName();
                    Dictionary<string, string> fields = querySchema.GetFieldName();
                    //sql语句中添加查询字段
                    for (int i = 0; i < fields.Count; i++)
                    {
                        sb.Append(fields.ElementAt(i).Value);
                        if (i != fields.Count - 1)
                        {
                            sb.Append(",");
                        }
                        else
                        {
                            sb.Append(" ");
                        }
                    }
                    sb.AppendFormat("from {0}  ", tableName);
                    //添加where子句
                    if (queryRequest.complexCondition != null)
                    {
                        string condition = queryRequest.complexCondition.GetSqlStr(fields);
                        if (!string.IsNullOrEmpty(condition))
                        {
                            sb.AppendFormat(" where {0} ", condition);
                        }
                    }
                    //添加排序字句
                    if (queryRequest.orderBy!=null&&queryRequest.orderBy.Length > 0)
                    {
                        sb.Append("order by ");
                        for (int j = 0; j < queryRequest.orderBy.Length; j++)
                        {
                            if (j < queryRequest.orderBy.Length - 1)
                                sb.AppendFormat("{0} {1}, ", fields[queryRequest.orderBy[j].accessPointField], queryRequest.orderBy[j].orderType.ToString());
                            else
                                sb.AppendFormat("{0} {1} ", fields[queryRequest.orderBy[j].accessPointField], queryRequest.orderBy[j].orderType.ToString());
                        }
                    }
                    //添加limit子句,offset为每页个数或最后一页个数，如果为-1表示不分页
                    if (queryRequest.offset!=-1)
                    {
                        sb.AppendFormat("limit {0},{1}", queryRequest.recordSetStartPointSpecified, queryRequest.offset);
                    }
                  
                    //写查询日志
                    //string filename = AppDomain.CurrentDomain.BaseDirectory + "\\" + "querylog.txt";
                    //FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write);
                    //StreamWriter sw = new StreamWriter(fs);
                    //sw.WriteLine("[{0}]:{1}", DateTime.Now, sb.ToString());
                    //sw.Close();
                    //fs.Close();

                    return sb.ToString();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("未定义查询方案！");
            }
        }

        /// <summary>
        /// 获取查询的总记录数
        /// </summary>
        /// <returns></returns>
        public int GetRecordCount()
        {
            if (queryRequest != null)
            {
                try
                {
                    Check();
                    StringBuilder sb = new StringBuilder("select count(*) ");
                    string tableName = querySchema.GetTableName();
                    sb.AppendFormat("from {0} ",  tableName);
                    Dictionary<string, string> fields = querySchema.GetFieldName();
                    //添加where子句
                    if (queryRequest.complexCondition != null)
                    {
                        string condition = queryRequest.complexCondition.GetSqlStr(fields);
                        if (!string.IsNullOrEmpty(condition))
                        {
                            sb.AppendFormat(" where {0} ", condition);
                        }
                    }
                    DataSet ds = baseUtilities.GetDataSet(sb.ToString());
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取过滤查询的总记录数
        /// </summary>
        /// <returns></returns>
        public int GetRecordCount(DotSpatial.Data.IFeature iFeature)
        {
            if (queryRequest != null)
            {
                try
                {
                    Check();
                    StringBuilder sb = new StringBuilder("select count(*) ");
                    string tableName = querySchema.GetTableName();
                    sb.AppendFormat("from {0}.{1}  ", queryRequest.dataBase, tableName);
                    Dictionary<string, string> fields = querySchema.GetFieldName();
                    //添加where子句
                    if (queryRequest.complexCondition != null)
                    {
                        string condition = queryRequest.complexCondition.GetSqlStr(fields);
                        if (!string.IsNullOrEmpty(condition))
                        {
                            sb.AppendFormat(" where {0} ", condition);
                        }
                    }
                    DataSet ds = baseUtilities.GetDataSet(sb.ToString());
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            return 0;
        }
        
        /// <summary>
        ///检查QueryRequest对象输入是否合法
        /// </summary>
        private void Check()
        {
            if (queryRequest.protocolVersion != protocolVersion)
            {
                throw new Exception("查询版本'protocolVersion'不一致!");
            }
            if(queryRequest.recordSetStartPointSpecified<0)
            {
                throw new Exception("数据查询集的起始索引'recordSetStartPointSpecified'越界，该索引应大于0!");
            }
            if (queryRequest.offset < 0)
            {
                throw new Exception("数据查询集的偏移量'offset'应该大于0!");
            }
        }
    }
}
